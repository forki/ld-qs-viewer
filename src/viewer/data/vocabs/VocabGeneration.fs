module Viewer.Data.Vocabs.VocabGeneration

open FSharp.RDF
open Viewer.Types
open FSharp.Data
open Viewer.SuaveExtensions

///Load all resources from uri and make a map of rdfs:label -> resource uri
let vocabGeneration ttl lbl =
  let gcd = Graph.loadTtl (fromString ttl)
  Resource.fromType (Uri.from "http://www.w3.org/2002/07/owl#Class") gcd
  |> List.map (InverseTerm.from lbl)
  |> List.map Term.from
  |> List.fold (++) Empty

let vocabLookup uri lbl = vocabGeneration (Http.RequestString uri) lbl

let readVocabsFromFiles () =
  printf "reading vocabs"
  try
    [
      {
        Root = vocabLookup "http://schema/ontologies/setting.ttl" "Setting"
        Property = "qualitystandard:appliesToSetting"
        Label = "Setting"
      }
      {
        Root = vocabLookup "http://schema/ontologies/agegroup.ttl" "Age group"
        Property = "qualitystandard:appliesToAgeGroup"
        Label = "Age group"
      }
      {
        Root = vocabLookup "http://schema/ontologies/servicearea.ttl" "Service area"
        Property = "qualitystandard:appliesToServiceArea"
        Label = "Service area"
      }
      {
        Root = vocabLookup "http://schema/ontologies/factorsaffectinghealthorwellbeing.ttl" "Factors affecting health or wellbeing"
        Property = "qualitystandard:appliesToFactorsAffectingHealthOrWellbeing"
        Label = "Factors affecting health or wellbeing"
      }
      {
        Root = vocabLookup "http://schema/ontologies/conditionordisease.ttl" "Condition or disease"
        Property = "qualitystandard:appliesToConditionOrDisease"
        Label = "Condition or disease"
      }
    ]
  with
    _ -> []

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
