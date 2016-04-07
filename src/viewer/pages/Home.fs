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

  [Sidebar.render req.query config.Vocabs testing
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

"""

let page (req:HttpRequest) config showOverview =

  {Content = buildContent req config showOverview 
   Scripts = buildScripts}
  |> DotLiquid.page "templates/home.html"


