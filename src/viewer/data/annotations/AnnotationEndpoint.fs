module Viewer.Data.Annotations.AnnotationEndpoint

open Viewer.Utils
open Viewer.Data.Vocabs.VocabGeneration
open Viewer.Types

let private urlDecode (filter: Filter) =
 {filter with Vocab = System.Uri.UnescapeDataString filter.Vocab}

let private serialiseYaml (filters: Filter list) =
  let createYamlVocabSection acc (vocab, filters) =
    let yamlTerms =
      filters
      |> Seq.fold (fun acc (filter: Filter) -> acc + (sprintf "  - \"%s\"\n" (filter.TermUri))) ""
    sprintf "%s:\n%s" (acc + vocab) yamlTerms

  filters
  |> Seq.map urlDecode
  |> Seq.groupBy(fun filter -> filter.Vocab)
  |> Seq.fold (fun acc section -> createYamlVocabSection acc section) ""


let toGuidBlock qs =
  qs
  |> extractFilters
  |> serialiseYaml
