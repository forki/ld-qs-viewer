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
        Property = "qualitystandard:62496684_7027_4f37_bd0e_264c9ff727fd"
        Label = "Setting"
      }
      {
        Root = vocabLookup "http://schema/ontologies/agegroup.ttl" "Age group"
        Property = "qualitystandard:4e7a368e_eae6_411a_8167_97127b490f99"
        Label = "Age group"
      }
      {
        Root = vocabLookup "http://schema/ontologies/servicearea.ttl" "Service area"
        Property = "qualitystandard:7ae8413a_2811_4a09_a655_eff8d276ec87"
        Label = "Service area"
      }
      {
        Root = vocabLookup "http://schema/ontologies/factorsaffectinghealthorwellbeing.ttl" "Factors affecting health or wellbeing"
        Property = "qualitystandard:18aa6468_de94_4f9f_bd7a_0075fba942a5"
        Label = "Factors affecting health or wellbeing"
      }
      {
        Root = vocabLookup "http://schema/ontologies/conditionordisease.ttl" "Condition or disease"
        Property = "qualitystandard:28745bc0_6538_46ee_8b71_f0cf107563d9"
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
