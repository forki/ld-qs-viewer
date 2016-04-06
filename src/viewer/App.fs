module Viewer.App


open Viewer.Pages
open Suave
open Suave.Filters
open Suave.Operators
open Suave.Files

let buildPath pathLocation =
    (path pathLocation <|> path (pathLocation + "/") )

let createApp config =
  choose
        [GET >=> buildPath "/qs" >=> request(fun req -> Home.page req config true)
         GET >=> path "/qs/search" >=> request(fun req -> Home.page req config false)
         GET >=> pathScan "/qualitystandards/%s" (fun (filename) -> request  (fun req -> Resource.page req config filename))
         GET >=> buildPath "/annotationtool" >=> request(fun req -> AnnotationTool.page req config false)
         GET >=> path "/annotationtool/toyaml" >=> request(fun req -> AnnotationTool.page req config true)
         GET >=> browseHome
         RequestErrors.NOT_FOUND "Found no handlers"]
