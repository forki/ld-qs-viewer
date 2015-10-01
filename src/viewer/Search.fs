
module Viewer.Search

open Suave
open Viewer.Types
open Viewer.Utils
open Viewer.Elastic

type SearchModel = {
  Results: SearchResult list
  Filters: string list
  Vocabularies: Vocabulary list
  totalCount: int
}

let search qs getSearchResults getVocabs =
  let vocab = getVocabs()
  match qs with
    | [("", _)] -> {Results = []; Filters = []; Vocabularies = vocab; totalCount = 0}
    | _         ->
      let results = (qs |> BuildQuery |> getSearchResults)
      {Results = results; Filters = extractFilters qs; Vocabularies = vocab; totalCount = results.Length}
  |> DotLiquid.page "search.html"
