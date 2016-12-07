module Viewer.AnnotationApi

open FSharp.RDF
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

let private getLabelAndDataType (resource:Resource) =
  let getRange x =
    x
    |> (|FunctionalObjectProperty|_|) (Uri.from "http://www.w3.org/2000/01/rdf-schema#range")

  (getRange resource)

let getPropertyList ontologyConfig =
  let ttlContent = getTtlContent ontologyConfig.CoreTtl
  let graph = Graph.loadTtl (fromString ttlContent)

  let getLabel resource =
    resource
    |> (|FunctionalDataProperty|_|) (Uri.from "http://www.w3.org/2000/01/rdf-schema#label") (xsd.string)

  let getRange resource =
    resource
    |> (|FunctionalObjectProperty|_|) (Uri.from "http://www.w3.org/2000/01/rdf-schema#range")
    |> fun x -> match x with
                | Some y -> y.ToString()
                            |> reinstatePrefix ontologyConfig.Contexts
                            |> Some
                | _ -> None
           

  let getPropertyFromGraph (property:CoreProperty) =
    let resource = Resource.fromSubject(Uri.from property.PropertyId) graph

    let returnResponse resource =
      { id = reinstatePrefix ontologyConfig.Contexts property.PropertyId
        label = resource |> getLabel
        range = resource |> getRange
        detail = Property property.Detail }

    match resource with
    | x::_ -> x |> returnResponse 
    | _ -> { id = reinstatePrefix ontologyConfig.Contexts property.PropertyId
             label = None
             range = None
             detail = Property property.Detail }

  ontologyConfig.Properties
  |> List.map getPropertyFromGraph

let getAnnotationToolData (vocabs: Vocabulary list) (config:OntologyConfig) =
  let prefix = reinstatePrefix config.Contexts
  let properties = getPropertyList config
  let trees = config.Ontologies
              |> List.map (fun p ->  matchVocab vocabs prefix p)
              |> List.concat
  { contexts = config.Contexts; properties = properties @ trees }

let getAnnotationToolJson (vocabs: Vocabulary list) (config:OntologyConfig) =
  try
    let data = getAnnotationToolData vocabs config
    Success (data |> Json.serialize |> Json.formatWith JsonFormattingOptions.Pretty)
  with
    ex -> Log.Error(sprintf "Exception encountered getting annotation data\n%s" ( ex.ToString() ) )
          Failure (ex.ToString())