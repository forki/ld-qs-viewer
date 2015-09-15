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
open FSharp.Data

let setTemplatesDir path =
  DotLiquid.setTemplatesDir(path)

type HomeModel =  {
   Vocabularies: Vocabulary list
 }

type SearchModel = {
  Results: SearchResult list

  }

let buildQuery qs =
  qs |> Seq.head
  |> (fun a ->
      match a with
      | (k, Some(v)) -> (sprintf "http://192.168.59.103:9200/kb/qs/_search?q=%s:%s" k v))

let createApp vocabularies getSearchResultsFor =
  choose
    [ GET >>= choose
        [path "/" >>= DotLiquid.page "home.html" {Vocabularies = vocabularies}
         path "/search" >>= request(fun r ->
                                    let query = buildQuery(r.query)
                                    DotLiquid.page "search.html" {Results = (getSearchResultsFor query)})

         RequestErrors.NOT_FOUND "Found no handlers"]]


