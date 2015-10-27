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

  let shouldExpandVocab vocab (filters:Filter list) =
    filters |> List.exists (fun x -> x.Vocab = vocab.Property)

  let testing = req.cookies |> Map.containsKey "test"

  let qs = req.query
  match qs with
    | [("", _)] ->
      {Results = []
       Tags = []
       Vocabularies = vocabs |> List.map (fun v -> {Vocab = v; Expanded = false})
       totalCount = 0}
    | _         ->
      let results = qs |> BuildQuery |> getSearchResults testing
      let filters = extractFilters qs
      let filterTags = createFilterTags filters

      let viewVocabs =
        filters
        |> getVocabsWithState vocabs
        |> List.map (fun v -> {Vocab = v; Expanded = shouldExpandVocab v filters})
      
      {Results = results
       Tags = filterTags
       Vocabularies = viewVocabs
       totalCount = results.Length}

  |> DotLiquid.page "home.html"

