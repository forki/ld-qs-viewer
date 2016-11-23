module Viewer.App


open Viewer.Pages
open Viewer.Data.Annotations
open Viewer.SuaveSerilogAdapter
open Suave
open Suave.Filters
open Suave.Operators
open Suave.Files
open Suave.Logging
open Serilog
open Viewer.Utils

let buildPath pathLocation =
    (path pathLocation <|> path (pathLocation + "/") )

let logRequest context =
  sprintf "Received request %A %A" context.request.``method`` context.request.url.PathAndQuery

let createApp config =
  choose
        [log (SuaveSerilogAdapter Log.Logger) logRequest >=> never
         GET >=> buildPath "/" >=> request(fun req -> Home.page req config true)
         GET >=> buildPath "/qs" >=> request(fun req -> Home.page req config true)
         GET >=> path "/search" >=> request(fun req -> Home.page req config false)
         GET >=> path "/qs/search" >=> request(fun req -> Home.page req config false)
         GET >=> pathScan "/resource/%s" (fun resourceId -> Resource.page config resourceId)
         GET >=> pathScan "/things/%s" (fun resourceId -> Resource.page config resourceId)
         GET >=> path "/ontologies" >=> (Successful.OK "Welcome to ontologies")
         GET >=> buildPath "/annotationtool" >=> request(fun req -> AnnotationTool.page req config false)
         GET >=> path "/annotationtool/toyaml" >=> request(fun req -> AnnotationTool.page req config true)
         POST >=> path "/annotationtool/fromyaml" >=> request(fun req -> getQueryStringFromYaml config.Vocabs req |> Redirection.redirect) 
         GET >=> path "/annotationtool/toguidblock" >=>
           request(fun req ->
                    let block = AnnotationEndpoint.toGuidBlock req config.Vocabs
                    Successful.OK block)
         GET >=> browseHome
         RequestErrors.NOT_FOUND "Found no handlers"]
