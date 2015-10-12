module Viewer.App

open Suave
open Suave.Web
open Suave.Http
open Suave.Http.Applicatives
open Suave.Http.Files
open Suave.Http.Successful
open Suave.Types
open Suave.Cookie
open Suave.Log
open Suave.Utils
open Viewer.Types
open Viewer.Search
open Viewer.Resource
open Viewer.Home
open FSharp.Data
open Viewer.VocabGeneration
let setTemplatesDir path =
  DotLiquid.setTemplatesDir(path)

let qualityStandardsDir = "/artifacts/published/"

let createApp vocabs getSearchResults =
  choose
    [ GET >>= choose
        [path "/qs" >>= request(fun req -> home req vocabs)
         path "/qs/search" >>= request(fun req -> search req getSearchResults vocabs)
         pathScan "/qualitystandards/%s" (fun (filename) -> resource filename) 
         browseHome
         RequestErrors.NOT_FOUND "Found no handlers"]]


