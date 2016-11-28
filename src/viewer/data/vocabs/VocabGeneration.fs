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

let vocabLookup content lbl = vocabGeneration content lbl

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
  ontologyConfig.Predicates
  |> List.map (fun p -> replacePrefix ontologyConfig.Contexts p )


let getMatchedResource (terms:InverseTerm list) ontologyReference =
  terms
  |> List.filter (fun x -> x.Uri = Uri.from(ontologyReference.Uri))
  |> List.head
  |> (fun x -> { Root = vocabLookup (getTtlContent ontologyReference.SourceTtl) x.Label; Property = x.Uri.ToString(); Label= x.Label} )

let mapResourceToConfig ontologyConfig resources=
  ontologyConfig
  |> replacePrefixes
  |> List.map (fun x -> getMatchedResource resources x)
  
let getVocabList ontologyConfig =
  let ttlContent = getTtlContent ontologyConfig.CoreTtl

  let graph = Graph.loadTtl (fromString ttlContent)
  
  let resources = Resource.fromType (Uri.from "http://www.w3.org/2002/07/owl#ObjectProperty") graph
  resources
  |> List.map (InverseTerm.from "")
  |> mapResourceToConfig ontologyConfig


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
