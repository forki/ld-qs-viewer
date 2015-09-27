
module Viewer.Search

open Suave
open Viewer.Types
open Viewer.Utils
open Viewer.Elastic

type SearchModel = {
  Results: SearchResult list
  Filters: string list
}

let search qs getSearchResults =
  match qs with
    | [("", _)] -> {Results = []; Filters = []}
    | _         -> {Results = (qs |> BuildQuery |> getSearchResults); Filters = extractFilters qs}
  |> DotLiquid.page "search.html"
