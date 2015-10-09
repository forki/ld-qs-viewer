module Viewer.Search

open Suave
open Suave.Types
open Suave.Cookie
open Viewer.Types
open Viewer.Utils
open Viewer.Elastic
open Viewer.VocabGeneration

type SearchModel = {
  Results: SearchResult list
  Filters: string list
  Vocabularies: Vocabulary list
  totalCount: int
}

let search (req:HttpRequest) getSearchResults getVocabs =
  let testing = req.cookies |> Map.containsKey "test"

  let qs = req.query
  match qs with
    | [("", _)] ->
      {Results = []; Filters = []; Vocabularies = getVocabs(); totalCount = 0}
    | _         ->
      let results = (qs |> BuildQuery |> getSearchResults testing)
      {Results = results; Filters = extractFilters qs; Vocabularies = (GetVocabsWithState getVocabs qs); totalCount = results.Length}

  |> DotLiquid.page "search.html"
