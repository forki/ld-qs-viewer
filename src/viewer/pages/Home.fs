module Viewer.Pages.Home

open System
open Suave
open Suave.Cookie
open Viewer.Utils
open Viewer.AppConfig
open Viewer.Data.Vocabs.VocabGeneration
open Viewer.Components
open Viewer.Components.Sidebar
open Viewer.Components.SearchResults
open Viewer.Components.Hotjar
open Viewer.Components.GoogleAnalytics

type HomeModel = {
  Content: string
  Scripts: string
}

let private decode qs = 
  match qs with
  | (key,Some v) -> (Uri.UnescapeDataString key, Some v)
  | _ | (_,None) -> qs

let private buildContent (req:HttpRequest) config showOverview =
  let qs = req.query |> List.map decode
  let testing = req.cookies |> Map.containsKey "test"
  let config = 
    match testing with
    | true -> {config with RenderedVocabs = renderVocabs Stubs.vocabs}
    | false -> config

  [Sidebar.render config.RenderedVocabs 
   SearchResults.render {Qs=qs
                         PerformSearch = config.PerformSearch
                         GetKBCount = config.GetKBCount
                         ShowOverview = showOverview
                         Testing = testing}
   Hotjar.render config.HotjarId
   GoogleAnalytics.render config.GAId]
  |> Seq.fold (fun acc comp -> acc + comp) ""

let private buildScripts =
  """<script src="/qs/components/sidebar/client/script.js?version=2"></script>
     <script src="/qs/components/sidebar/client/googleAnalytics.js"></script>
     <script src="/qs/components/nojs/client/script.js?version=2"></script>
     <script src="/qs/components/jquery/client/script.js?version=2"></script>
     <script src="/qs/components/nestedcheckboxes/client/script.js?version=2"></script>
     <script src="/qs/components/main/client/script.js?version=2"></script>
  """

let page (req:HttpRequest) config showOverview =

  {Content = buildContent req config showOverview 
   Scripts = buildScripts}
  |> DotLiquid.page "templates/home.html"


