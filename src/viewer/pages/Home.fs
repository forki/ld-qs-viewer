module Viewer.Pages.Home

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

let private buildContent (req:HttpRequest) config showOverview =
  let testing = req.cookies |> Map.containsKey "test"
  let config = 
    match testing with
    | true -> {config with RenderedVocabs = renderVocabs Stubs.vocabs}
    | false -> config

  [Sidebar.render config.RenderedVocabs 
   SearchResults.render {Qs=req.query
                         GetSearchResults = config.GetSearchResults
                         GetKBCount = config.GetKBCount
                         ShowOverview = showOverview
                         Testing = testing}
   Hotjar.render config.HotjarId
   GoogleAnalytics.render config.GAId]
  |> Seq.fold (fun acc comp -> acc + comp) ""

let private buildScripts =
  """<script src="/qs/components/sidebar/client/script.js?version=1"></script>
     <script src="/qs/components/nojs/client/script.js?version=1"></script>
     <script src="/qs/components/jquery/client/script.js?version=1"></script>
     <script src="/qs/components/nestedcheckboxes/client/script.js?version=1"></script>
     <script src="//cdn.nice.org.uk/V3/Scripts/nice/NICE.EventTracking.js" async></script>
     <script defer>
      if ($.fn.trackevent) $('.counter').trackevent('Search Results', { action: 'Zero Search Results', label: 'penguin gutterfff' });
    </script>
"""

let page (req:HttpRequest) config showOverview =

  {Content = buildContent req config showOverview 
   Scripts = buildScripts}
  |> DotLiquid.page "templates/home.html"


