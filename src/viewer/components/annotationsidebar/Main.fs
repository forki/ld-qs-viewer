module Viewer.Components.AnnotationSidebar

open Suave
open Viewer.Utils
open Viewer.Data.Vocabs.VocabGeneration

type AnnotationSidebarModel = {
  Vocabularies: ViewVocab list
}

let createModel (req:HttpRequest) vocabs =
  printf "%A" req.query

  let filters = extractFilters req.query

  let viewVocabs =
      filters
    |> getVocabsWithState vocabs
    |> List.map (fun v -> {Vocab = v; Expanded = shouldExpandVocab v.Property filters})
  {Vocabularies = viewVocabs}
