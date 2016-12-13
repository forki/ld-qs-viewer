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
    pattern = None
    example = None
    detail = Tree (transformTermsToOntologyOption prefix terms)
  }

let private matchVocab (vocabs: Vocabulary list) prefix (predicate:OntologyReference)  = 
  vocabs
  |> List.tryFind (fun v -> v.Property = predicate.Uri)
  |> (fun r -> match r with
               | Some v -> [generateResponseFromVocab prefix v]
               | _ -> [] )

let getPropertyList ontologyConfig =
  let ttlContent = getTtlContent ontologyConfig.CoreTtl
  let graph = Graph.loadTtl (fromString ttlContent)

  let getDataProperty (uri:string) resource =
    resource
    |> (|FunctionalDataProperty|_|) (Uri.from uri) (xsd.string)

  let getObjectProperty (uri:string) resource =
    resource
    |> (|FunctionalObjectProperty|_|) (Uri.from uri)

  let getRange uri =
    uri.ToString()
    |> reinstatePrefix ontologyConfig.Contexts
    |> Some

  let traverse (uri:string) action resource =
    match (|Traverse|_|) (Uri.from uri) resource with
    | None -> None
    | Some x -> x |> List.head |> action

  let getPattern resource =
    let pattern x = x |> getDataProperty "http://www.w3.org/2001/XMLSchema#pattern"
    let first x = x |> traverse "http://www.w3.org/1999/02/22-rdf-syntax-ns#first" pattern
    let restrictions x = x |> traverse "http://www.w3.org/2002/07/owl#withRestrictions" first
    resource |> traverse "http://www.w3.org/2002/07/owl#equivalentClass" restrictions

  let getExamplePattern uri =
    Resource.fromSubject uri graph
    |> fun x -> match x with
                | [] -> None, None
                | x::_ -> getDataProperty "http://www.w3.org/2004/02/skos/core#example" x, getPattern x

  let getDataTypeDetails resource =
    resource
    |> getObjectProperty "http://www.w3.org/2000/01/rdf-schema#range"
    |> fun x -> match x with
                | Some y -> (getRange y), (getExamplePattern y)
                | _ -> None, (None, None)
           
  let getPropertyFromGraph (property:CoreProperty) =
    let resource = Resource.fromSubject(Uri.from property.PropertyId) graph

    let returnResponse resource =
      let range, (example, pattern) = resource |> getDataTypeDetails
      { id = reinstatePrefix ontologyConfig.Contexts property.PropertyId
        label = resource |> getDataProperty "http://www.w3.org/2000/01/rdf-schema#label"
        range = range
        pattern = pattern
        example = example
        detail = Property property.Detail }

    match resource with
    | x::_ -> x |> returnResponse 
    | _ -> { id = reinstatePrefix ontologyConfig.Contexts property.PropertyId
             label = None
             range = None
             pattern = None
             example = None
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