module Viewer.Components.SearchResults

open Suave
open Viewer.Utils
open Viewer.Types
open Viewer.Data.Search.Elastic
open Viewer.SuaveExtensions
open Viewer.Config

type SearchResultsParameters = {
  Qs : (string * string option) list
  GetSearchResults : (bool -> string -> SearchResult list)
  GetKBCount : (bool -> int)
  Vocabs : Vocabulary list
  ShowOverview : bool
  Testing : bool
}

type SearchResultsModel = {
  Results: SearchResult list
  Tags: Tag list
  totalCount: int
  ShowHelp : bool
}

let prefixFiltersWithBaseUrl url filters =
    filters |> List.map (fun {Vocab=key; TermUri=value} -> {Vocab=key;TermUri=url+value }) 

let createModel args = 
  match args.Qs with
    | [("", _)] ->
      {Results = []
       Tags = []
       totalCount = if args.ShowOverview then args.GetKBCount args.Testing else 0
       ShowHelp = if args.ShowOverview then true else false}
    | _ ->
      let reAddBaseUrlToFilters = prefixFiltersWithBaseUrl BaseUrl
      let results = args.Qs |> extractFilters |> reAddBaseUrlToFilters |> BuildQuery |> args.GetSearchResults args.Testing

      let filters = extractFilters args.Qs
      let filterTags = createFilterTags filters args.Vocabs

      {Results = results
       Tags = filterTags
       totalCount = results.Length
       ShowHelp = false}

let render args =
  args
  |> createModel
  |> template "components/searchresults/index.html"
