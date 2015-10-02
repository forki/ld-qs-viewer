
module Viewer.Search

open Suave
open Suave.Types
open Suave.Cookie
open Viewer.Types
open Viewer.Utils
open Viewer.Elastic

type SearchModel = {
  Results: SearchResult list
  Filters: string list
  Vocabularies: Vocabulary list
}

let search (req:HttpRequest) getSearchResults getVocabs =
  let testing = req.cookies |> Map.containsKey "test"

  let qs = req.query
  let vocab = getVocabs()
  match qs with
    | [("", _)] -> {Results = []; Filters = []; Vocabularies = vocab}
    | _         -> {Results = (qs |> BuildQuery |> getSearchResults testing); Filters = extractFilters qs; Vocabularies = vocab}
  |> DotLiquid.page "search.html"
