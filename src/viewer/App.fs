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
        [path "/qs" >>= request(fun req -> home req config true)
         path "/qs/search" >>= request(fun req -> home req config false)
         pathScan "/qualitystandards/%s" (fun (filename) ->
                                          request  (fun req -> resource req filename))
         path "/annotationtool" >>= request(fun req -> annotationTool req config false)
         path "/annotationtool/toyaml" >>= request(fun req -> annotationTool req config true)
         browseHome
         RequestErrors.NOT_FOUND "Found no handlers"]]
