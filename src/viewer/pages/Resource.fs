module Viewer.Pages.Resource

open Suave
open System.IO
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

let private artifacts testing filename =
  match testing with
    | true -> sprintf "/test_artifacts/published/qualitystandards/%s" filename
    | false -> sprintf "/artifacts/published/qualitystandards/%s" filename

let page (request:HttpRequest) config filename =
  let testing = request.cookies |> Map.containsKey "test"
  let content =
    try
      File.ReadAllText((artifacts testing filename))
    with
      | ex -> "Could not find resource."

  DotLiquid.page "templates/resource.html" { Content = content; Scripts = buildScripts config }

