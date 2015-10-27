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
  Tags: Tag list
  Vocabularies: Vocabulary list
  totalCount: int
}

let search (req:HttpRequest) getSearchResults vocabs =
  let testing = req.cookies |> Map.containsKey "test"

  let qs = req.query
  match qs with
    | [("", _)] ->
      {Results = []; Tags = []; Vocabularies = vocabs; totalCount = 0}
    | _         ->
      let results = (qs |> BuildQuery |> getSearchResults testing)
      let filters = extractFilters qs
      let filterTags = createFilterTags filters
      {Results = results;
       Tags = filterTags;
       Vocabularies = getVocabsWithState vocabs filters
       totalCount = results.Length}

  |> DotLiquid.page "search.html"
