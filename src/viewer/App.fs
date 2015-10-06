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
open Viewer.Search
open FSharp.Data
open Viewer.VocabGeneration
let setTemplatesDir path =
  DotLiquid.setTemplatesDir(path)

type HomeModel =  {
   Vocabularies: Vocabulary list
}

let qualityStandardsDir = "/artifacts/published/"

let createApp getVocabs getSearchResults =
  choose
    [ GET >>= choose
        [path "/" >>= DotLiquid.page "home.html" {Vocabularies = getVocabs()}
         path "/search" >>= request(fun r -> search r.query getSearchResults)
         browse qualityStandardsDir
         browseHome
         RequestErrors.NOT_FOUND "Found no handlers"]]


