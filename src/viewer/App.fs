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
open Viewer.AnnotationTool

let setTemplatesDir path =
  DotLiquid.setTemplatesDir(path)

let createApp config =
  choose
    [ GET >>= choose
        [path "/qs" >>= request(fun req -> home req config)
         path "/qs/search" >>= request(fun req -> search req config)
         pathScan "/qualitystandards/%s" (fun (filename) ->
                                          request  (fun req -> resource req filename))
         path "/annotationtool" >>= request(fun req -> annotationtool req config.Vocabs)
         path "/annotationtool/toyaml" >>= request(fun req -> toyaml req config.Vocabs)
         path "/annotationtool/fromyaml" >>= request(fun req -> fromyaml req config.Vocabs)
         browseHome
         RequestErrors.NOT_FOUND "Found no handlers"]]
