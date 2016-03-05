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
  Components: string
}

let page (req:HttpRequest) config showOverview =
  let testing = req.cookies |> Map.containsKey "test"

  let components =
    [Sidebar.render req.query config.Vocabs testing
     SearchResults.render {Qs=req.query;
                           GetSearchResults = config.GetSearchResults
                           GetKBCount = config.GetKBCount
                           ShowOverview = showOverview
                           Testing = testing}
     Hotjar.render config.HotjarId]
    |> Seq.fold (fun acc comp -> acc + comp) ""

  {Components = components}
  |> DotLiquid.page "templates/home.html"


