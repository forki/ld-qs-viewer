module Viewer.Components.AnnotationSidebar

open Suave
open Suave.Types
open Viewer.Utils
open Viewer.VocabGeneration

type AnnotationSidebarModel = {
  Vocabularies: ViewVocab list
}

let createModel (req:HttpRequest) vocabs =

  let filters = extractFilters req.query
  let viewVocabs =
    filters
    |> getVocabsWithState vocabs
    |> List.map (fun v -> {Vocab = v; Expanded = shouldExpandVocab v.Property filters})
  {Vocabularies = viewVocabs}
