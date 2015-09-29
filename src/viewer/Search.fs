
module Viewer.Search

open Suave
open Viewer.Types
open Viewer.Utils
open Viewer.Elastic

type SearchModel = {
  Results: SearchResult list
  Filters: string list
  Vocabularies: Vocabulary list
}

let search qs getSearchResults getVocabs =
  let vocab = getVocabs()
  match qs with
    | [("", _)] -> {Results = []; Filters = []; Vocabularies = vocab}
    | _         -> {Results = (qs |> BuildQuery |> getSearchResults); Filters = extractFilters qs; Vocabularies = vocab}
  |> DotLiquid.page "search.html"
