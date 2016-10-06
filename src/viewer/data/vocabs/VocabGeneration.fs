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
        Root = vocabLookup "http://schema/ns/qualitystandard/setting.ttl" "Setting"
        Property = "qualitystandard:appliesToSetting"
        Label = "Setting"
      }
      {
        Root = vocabLookup "http://schema/ns/qualitystandard/agegroup.ttl" "Age group"
        Property = "qualitystandard:appliesToAgeGroup"
        Label = "Age group"
      }
      {
        Root = vocabLookup "http://schema/ns/qualitystandard/servicearea.ttl" "Service area"
        Property = "qualitystandard:appliesToServiceArea"
        Label = "Service area"
      }
      {
        Root = vocabLookup "http://schema/ns/qualitystandard/factorsaffectinghealthorwellbeing.ttl" "Factors affecting health or wellbeing"
        Property = "qualitystandard:appliesToFactorsAffectingHealthOrWellbeing"
        Label = "Factors affecting health or wellbeing"
      }
      {
        Root = vocabLookup "http://schema/ns/qualitystandard/conditionordisease.ttl" "Condition or disease"
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

let findTheLabel vocabs filterUris =
  let rec getTerm f = function
    | [] -> []
    | x::xs -> match x with
                | Term x -> if f x then 
                              [x]; 
                            else 
                              match xs with
                              | [] -> getTerm f x.Children
                              | _ -> getTerm f xs
                | Empty -> []
  vocabs
  |> List.map (fun v -> getTerm (fun t->t.ShortenedUri.Contains(filterUris)) [v.Root]) 
  |> List.concat
  |> List.map (fun t -> t.Label) 
  |> List.filter (fun l->l <> "")

let renderVocabs vocabs =
  {Vocabularies = vocabs}
  |> template "templates/filters.html"
