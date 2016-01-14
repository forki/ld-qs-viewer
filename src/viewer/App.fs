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
open Viewer.Pages

let setTemplatesDir path =
  DotLiquid.setTemplatesDir(path)

let createApp config =
  choose
    [ GET >>= choose
        [path "/qs" >>= request(fun req -> Home.page req config true)
         path "/qs/search" >>= request(fun req -> Home.page req config false)
         pathScan "/qualitystandards/%s" (fun (filename) ->
                                          request  (fun req -> Resource.page req filename))
         path "/annotationtool" >>= request(fun req -> AnnotationTool.page req config false)
         path "/annotationtool/toyaml" >>= request(fun req -> AnnotationTool.page req config true)
         browseHome
         RequestErrors.NOT_FOUND "Found no handlers"]]
