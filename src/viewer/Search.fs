module Viewer.Search

open Suave
open Suave.Types
open Suave.Cookie
open Viewer.Utils
open Viewer.Types
open Viewer.Model
open Viewer.Elastic
open Viewer.VocabGeneration

let search (req:HttpRequest) getSearchResults vocabs =

  let testing = req.cookies |> Map.containsKey "test"

  let qs = req.query
  match qs with
    | [("", _)] ->
      {Results = []
       Tags = []
       Vocabularies = vocabs |> List.map (fun v -> {Vocab = v; Expanded = false})
       totalCount = 0
       ShowHelp = false}
    | _         ->
      let results = qs |> BuildQuery |> getSearchResults testing
      let filters = extractFilters qs
      let filterTags = createFilterTags filters

      let viewVocabs =
        filters
        |> getVocabsWithState vocabs
        |> List.map (fun v -> {Vocab = v; Expanded = shouldExpandVocab v.Property filters})
      
      {Results = results
       Tags = filterTags
       Vocabularies = viewVocabs
       totalCount = results.Length
       ShowHelp = false}

  |> DotLiquid.page "home.html"

