module Viewer.Pages.Resource

open System
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
  CurrentYear: string
}

let private buildScripts config =
  [Hotjar.render config.HotjarId
   GoogleAnalytics.render config.GAId
   """
      <script src="/qs/components/nojs/client/script.js?version=2"></script>
      <script src="/qs/components/jquery/client/script.js?version=2"></script>
      <script src="//cdn.nice.org.uk/V3/Scripts/nice/NICE.TopHat.dev.js" data-environment="live" async=""></script>
      <script src="//cdn.nice.org.uk/V2/Scripts/twitter.bootstrap.min.js" type="text/javascript"></script>
      <script src="//cdn.nice.org.uk/V2/Scripts/NICE.bootstrap.min.js" type="text/javascript"></script>
   """]
  |> Seq.fold (fun acc comp -> acc + comp) ""

let page config resourceId =
  let url = sprintf "http://resourceapi:8082/resource/%s" resourceId
  let content =
    try
      Http.RequestString(url)
    with
      | ex -> "Could not find resource."
  DotLiquid.page "templates/resource.html" { Content = content; Scripts = buildScripts config; CurrentYear = DateTime.Now.Year.ToString()}

