module Viewer.App


open Viewer.Pages
open Suave
open Suave.Filters
open Suave.Operators
open Suave.Files

let setTemplatesDir path =
  DotLiquid.setTemplatesDir(path)

let createApp config =
  choose
        [GET >=> path "/qs" >=> request(fun req -> Home.page req config true)
         GET >=> path "/qs/search" >=> request(fun req -> Home.page req config false)
         GET >=> pathScan "/qualitystandards/%s" (fun (filename) -> request  (fun req -> Resource.page req filename))
         GET >=> path "/annotationtool" >=> request(fun req -> AnnotationTool.page req config false)
         GET >=> path "/annotationtool/toyaml" >=> request(fun req -> AnnotationTool.page req config true)
         GET >=> browseHome
         RequestErrors.NOT_FOUND "Found no handlers"]
