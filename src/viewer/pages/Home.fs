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
  CurrentYear: string
}

let private decode qs = 
  match qs with
  | (key,Some v) -> (Uri.UnescapeDataString key, Some v)
  | _ | (_,None) -> qs

let private matchRelevancyTest qs = 
  match qs with
  | ("relevancyTest",Some ("1")) ->  1
  | ("relevancyTest",Some ("2")) ->  2
  | ("relevancyTest",Some _) ->  0
  | (_,_) -> 0

let relevancyValueFromQueryString qs =
  match qs with
  | [] -> 0 
  | _ ->
    qs 
    |> List.map matchRelevancyTest 
    |> List.head

let relevancyFlagExists qs =
  qs
  |> List.map(fun x -> match x with
                        | ("relevancyTest",Some x) -> x.Equals("1")
                        | _ -> false)
  |> List.head
  

let private buildContent (req:HttpRequest) config showOverview =
  let qs = req.query |> List.map decode
  let testing = req.cookies |> Map.containsKey "test"
  let config = 
    match testing with
    | true -> {config with RenderedVocabs = renderVocabs Stubs.vocabs}
    | false -> config
 
  let relevancyTest args = 
    match args |> relevancyFlagExists with
    | false ->
      config.PerformSearch
    | true ->
      config.PerformSearchWithOrder

  let relevancyFlag = relevancyTest qs

  [Sidebar.render config.RenderedVocabs (relevancyValueFromQueryString qs) 
   SearchResults.render {Qs=qs
                         PerformSearch = relevancyTest qs
                         GetKBCount = config.GetKBCount
                         Vocabs = config.Vocabs
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
     <script src="/qs/components/main/client/script.js?version=3"></script>
     <script src="//cdn.nice.org.uk/V3/Scripts/nice/NICE.TopHat.dev.js" data-environment="live" async=""></script>
     <script src="//cdn.nice.org.uk/V2/Scripts/twitter.bootstrap.min.js" type="text/javascript"></script>
     <script src="//cdn.nice.org.uk/V2/Scripts/NICE.bootstrap.min.js" type="text/javascript"></script>
  """

let page (req:HttpRequest) config showOverview =

  {Content = buildContent req config showOverview 
   Scripts = buildScripts
   CurrentYear = DateTime.Now.Year.ToString()}
  |> DotLiquid.page "templates/home.html"


