module Viewer.Pages.Resource

open Suave
open System.IO
open FSharp.Data
open Suave.Cookie
open Viewer.AppConfig
open Viewer.Components
open Viewer.Components.Hotjar


type ResourceModel = {
  Content : string
  Scripts : string
}

let private buildScripts config =
    sprintf """ <script src="/qs/components/googleanalytics/client/script.js"></script>
    %s """ (Hotjar.render config.HotjarId)

let page config resourceId =
  let url = sprintf "http://resourceapi:8082/resource/%s" resourceId
  let content =
    try
      Http.RequestString(url)
    with
      | ex -> "Could not find resource."
  DotLiquid.page "templates/resource.html" { Content = content; Scripts = buildScripts config }

