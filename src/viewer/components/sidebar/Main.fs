module Viewer.Components.Sidebar

open Viewer.SuaveExtensions
open Viewer.Utils
open Viewer.Data.Vocabs.VocabGeneration

type SidebarModel = {
  Vocabularies: ViewVocab list
}

let private createModel qs actualVocabs testing =

  let vocabs =
    match testing with
    | true  -> Stubs.vocabsForTests
    | false -> actualVocabs

  match qs with
    | [("", _)] ->
      {Vocabularies = vocabs |> List.map (fun v -> {Vocab = v; Expanded = false})}
    | _         ->
      let filters = extractFilters qs

      let viewVocabs =
        filters
        |> getVocabsWithState vocabs
        |> List.map (fun v -> {Vocab = v; Expanded = shouldExpandVocab v.Property filters})

      {Vocabularies = viewVocabs}

let render qs vocabs testing =
  createModel qs vocabs testing
  |> template "components/sidebar/index.html"

