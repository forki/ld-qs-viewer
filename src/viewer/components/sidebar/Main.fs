module Viewer.Components.Sidebar

open Suave
open Suave.Types
open Suave.Cookie
open Viewer.Model
open Viewer.Utils
open Viewer.VocabGeneration

let createModel (req:HttpRequest) actualVocabs testing =

  let vocabs =
    match testing with
    | true  -> Stubs.vocabsForTests
    | false -> actualVocabs

  let qs = req.query
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
