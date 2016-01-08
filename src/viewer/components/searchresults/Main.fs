module Viewer.Components.SearchResults

open Suave
open Suave.Types
open Viewer.Utils
open Viewer.Types
open Viewer.Model
open Viewer.Elastic

let createModel (req:HttpRequest) getSearchResults getKBCount showOverview testing =

  let qs = req.query
  match qs with
    | [("", _)] ->
      {Results = []
       Tags = []
       totalCount = if showOverview then getKBCount testing else 0
       ShowHelp = if showOverview then true else false}
    | _ ->
      let results = qs |> BuildQuery |> getSearchResults testing
      let filters = extractFilters qs
      let filterTags = createFilterTags filters

      {Results = results
       Tags = filterTags
       totalCount = results.Length
       ShowHelp = false}
