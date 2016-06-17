module Viewer.Pages.Resource

open Suave
open System.IO
open FSharp.Data
open Suave.Cookie
open Viewer.AppConfig
open Viewer.Components
open Viewer.Components.Hotjar
open Viewer.Components.GoogleAnalytics

type ResourceModel = {
  Content : string
  Scripts : string
}

let private buildScripts config =
  [Hotjar.render config.HotjarId
   GoogleAnalytics.render config.GAId]
  |> Seq.fold (fun acc comp -> acc + comp) ""

let page config resourceId =
  let url = sprintf "http://resourceapi.resourceapi:8082/resource/%s" resourceId
  let content =
    try
      Http.RequestString(url)
    with
      | ex -> "Could not find resource."
  DotLiquid.page "templates/resource.html" { Content = content; Scripts = buildScripts config }

