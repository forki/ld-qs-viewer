module Viewer.AnnotationApi

open Serilog
open NICE.Logging
open Viewer.Types
open Viewer.ApiTypes
open Viewer.Data.Vocabs.VocabGeneration
open Chiron

let rec private transformTermsToOntologyOption prefix (terms:Term list) =
  let consolidateTermd terms =
    terms
    |> List.map (fun t -> match t with
                          | Term tt -> [tt] 
                          | _ -> [])
    |> List.concat
    |> List.map (fun td -> { id = prefix (td.Uri.ToString()); label = td.Label; children = (transformTermsToOntologyOption prefix td.Children) } )

  match terms with
  | [] -> []
  | _ -> consolidateTermd terms
   

let generateResponseFromVocab prefix (vocab:Vocabulary) =
  let terms = match vocab.Root with
              | Term t -> t.Children
              | _ -> []
  { id = vocab.Property
    label = Some vocab.Label
    range = None
    detail = Tree (transformTermsToOntologyOption prefix terms)
  }

let private matchVocab (vocabs: Vocabulary list) prefix (predicate:OntologyReference)  = 
  vocabs
  |> List.tryFind (fun v -> v.Property = predicate.Uri)
  |> (fun r -> match r with
               | Some v -> [generateResponseFromVocab prefix v]
               | _ -> [] )

let getAnnotationToolData (vocabs: Vocabulary list) (config:OntologyConfig) =
  let prefix = reinstatePrefix config.Contexts
  
  let trees = config.Ontologies
              |> List.map (fun p ->  matchVocab vocabs prefix p)
              |> List.concat
  { contexts = config.Contexts; properties = trees }

let getAnnotationToolJson (vocabs: Vocabulary list) (config:OntologyConfig) =
  try
    let data = getAnnotationToolData vocabs config
    Success (data |> Json.serialize |> Json.formatWith JsonFormattingOptions.Pretty)
  with
    ex -> Log.Error(sprintf "Exception encountered getting annotation data\n%s" ( ex.ToString() ) )
          Failure (ex.ToString())