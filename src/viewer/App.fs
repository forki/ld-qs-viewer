module Viewer.App

open Suave
open Suave.Web
open Suave.Http
open Suave.Http.Applicatives
open Suave.Http.Files
open Suave.Http.Successful
open Suave.Types
open Suave.Log
open Suave.Utils
open Viewer.Types
open Viewer.Elastic
open Viewer.Components.Search
open FSharp.Data

let setTemplatesDir path =
  DotLiquid.setTemplatesDir(path)

type HomeModel =  {
   Vocabularies: Vocabulary list
}

type SearchModel = {
  Results: SearchResult list
  Filters: string list
}

let createApp vocabularies getSearchResults =
  choose
    [ GET >>= choose
        [path "/" >>= DotLiquid.page "home.html" {Vocabularies = vocabularies}
         path "/search" >>= request(fun r ->
                                      match r.query with
                                        | [("", _)] -> {Results = []; Filters = []}
                                        | _ -> {Results = (r.query |> BuildQuery |> getSearchResults); Filters = extractFilters r.query}
                                      |> DotLiquid.page "search.html")
         browseHome
         RequestErrors.NOT_FOUND "Found no handlers"]]


