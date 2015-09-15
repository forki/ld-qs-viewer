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
open FSharp.Data

let setTemplatesDir path =
  DotLiquid.setTemplatesDir(path)

type HomeModel =  {
   Vocabularies: Vocabulary list
 }

type SearchModel = {
  Results: Result list
  }

let TransformResults queryResponse =
  let json = FSharp.Data.JsonProvider<"elasticResponseSchema.json">.Parse(queryResponse)

  let results = (json.Hits.Hits)
    |> Seq.map (fun x -> {Uri = x.Source.Id})
    |> Seq.toList

  {Results = results}

let createApp vocabularies sendQuery =

  choose
    [ GET >>= choose
        [path "/" >>= DotLiquid.page "home.html" {Vocabularies = vocabularies}
         path "/search" >>= DotLiquid.page "search.html" (TransformResult sendQuery())]]
