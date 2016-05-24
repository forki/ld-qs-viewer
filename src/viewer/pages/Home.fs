module Viewer.Pages.Home

open Suave
open Suave.Cookie
open Viewer.Utils
open Viewer.AppConfig
open Viewer.Components
open Viewer.Components.Sidebar
open Viewer.Components.SearchResults
open Viewer.Components.Hotjar

type HomeModel = {
  Content: string
  Scripts: string
}

let private buildContent (req:HttpRequest) config showOverview =
  let testing = req.cookies |> Map.containsKey "test"

  [Sidebar.render config.RenderedVocabs
   SearchResults.render {Qs=req.query
                         GetSearchResults = config.GetSearchResults
                         GetKBCount = config.GetKBCount
                         ShowOverview = showOverview
                         Testing = testing}
   Hotjar.render config.HotjarId]
  |> Seq.fold (fun acc comp -> acc + comp) ""

let private buildScripts =
  """<script src="/qs/components/sidebar/client/script.js"></script>
     <script src="/qs/components/nojs/client/script.js"></script>
     <script src="/qs/components/googleanalytics/client/script.js"></script>
     <script src="/qs/components/jquery/client/script.js"></script>
     <script src="/qs/components/nestedcheckboxes/client/script.js"></script>
     <script src="//cdn.nice.org.uk/V3/Scripts/nice/NICE.EventTracking.js" async></script>
     <script defer>
      if ($.fn.trackevent) $('.counter').trackevent('Search Results', { action: 'Zero Search Results', label: 'penguin gutterfff' });
    </script>
  """

let page (req:HttpRequest) config showOverview =

  {Content = buildContent req config showOverview 
   Scripts = buildScripts}
  |> DotLiquid.page "templates/home.html"


