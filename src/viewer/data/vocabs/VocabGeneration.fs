module Viewer.Data.Vocabs.VocabGeneration

open FSharp.RDF
open Viewer.Types
open Viewer.ApiTypes
open FSharp.Data
open Viewer.SuaveExtensions
open Serilog
open NICE.Logging

///Load all resources from uri and make a map of rdfs:label -> resource uri
let vocabGeneration ttl lbl =
  let gcd = Graph.loadTtl (fromString ttl)
  Resource.fromType (Uri.from "http://www.w3.org/2002/07/owl#Class") gcd
  |> List.map (InverseTerm.from lbl)
  |> List.map (Term.from)
  |> List.fold (++) Empty

let getTtlContent ttl =
  match ttl with
  | Content c -> c
  | Uri u -> Http.RequestString u

let replacePrefix (prefixes:Context list) predicate =
  let uri = predicate.Uri
  prefixes
  |> List.find (fun x -> uri.StartsWith(x.Prefix) = true)
  |> fun x -> { Uri = uri.Replace((sprintf "%s:" x.Prefix), x.Value); SourceTtl = predicate.SourceTtl }

let replacePrefixes ontologyConfig =
  ontologyConfig.Ontologies
  |> List.map (fun p -> replacePrefix ontologyConfig.Contexts p )

let reinstatePrefix (prefixes:Context list) (uri:string) =
  prefixes
  |> List.find (fun x -> uri.StartsWith(x.Value) = true)
  |> fun x -> uri.Replace(x.Value, (sprintf "%s:" x.Prefix))

let getMatchedResource prefix (terms:InverseTerm list) ontologyReference =
  terms
  |> List.filter (fun x -> x.Uri = Uri.from(ontologyReference.Uri))
  |> List.head
  |> (fun x -> { Root = vocabGeneration (getTtlContent ontologyReference.SourceTtl) x.Label; Property = prefix (x.Uri.ToString()) ; Label= x.Label} )

let mapResourceToConfig (ontologyConfig:OntologyConfig) resources=
  let prefix = reinstatePrefix ontologyConfig.Contexts
  ontologyConfig
  |> replacePrefixes
  |> List.map (fun x -> getMatchedResource prefix resources x)
  
let getVocabList ontologyConfig =
  let ttlContent = getTtlContent ontologyConfig.CoreTtl
  let graph = Graph.loadTtl (fromString ttlContent)
  
  let resources = Resource.fromType (Uri.from "http://www.w3.org/2002/07/owl#ObjectProperty") graph
  resources
  |> List.map (InverseTerm.from "")
  |> mapResourceToConfig ontologyConfig

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

//type OntologyResponseType =
//  | Tree of OntologyTreeOption list
//  | Property of CorePropertyDetail
//
//type OntologyResponseProperty =
//  {
//    id: string
//    label: string
//    detail: OntologyResponseType
//  }

//  let string_prop = Resource.fromSubject (Uri.from "https://nice.org.uk/ontologies/core/GUID_stringProperty") graph
//  let string_prop_type = string_prop |> List.head |> (|FunctionalObjectProperty|_|) (Uri.from "http://www.w3.org/2000/01/rdf-schema#range")
//  
//  let date_prop = Resource.fromSubject (Uri.from "https://nice.org.uk/ontologies/core/GUID_conditionalProperty") graph
//  let date_prop_type = date_prop |> List.head |> (|FunctionalObjectProperty|_|) (Uri.from "http://www.w3.org/2000/01/rdf-schema#range")

//  []
  
let readVocabsFromFiles ontologyConfig =
  printf "reading vocabs"
  try
    getVocabList ontologyConfig
  with
    ex -> Log.Error(sprintf "Exception encountered reading ontologies\n%s" ( ex.ToString() ) )
          []

let setSelectedIfFiltered filters vocab =

  let rec filterChildren f acc children =
    match children with
      | [] -> []
      | x::xs -> f x :: filterChildren f acc xs

  let rec filterVocab v =
    match v with
    | Empty -> v
    | Term x -> Term { x with
                        Selected = filters |> Seq.exists (fun (filter:string) -> x.Uri.ToString().Contains(filter) && filter <> "")
                        Children = filterChildren filterVocab [] x.Children }

  {vocab with Root = filterVocab (vocab.Root)}

let getVocabsWithState vocabs (filters: Filter list) =
  let filterUris = filters |> Seq.map (fun x -> x.TermUri)
  vocabs
  |> Seq.map (fun v -> setSelectedIfFiltered filterUris v)
  |> Seq.toList

let renderVocabs vocabs =
  {Vocabularies = vocabs}
  |> template "templates/filters.html"
