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
  blah: SearchResultsModel
  Components: string
}

let page (req:HttpRequest) config showOverview =
  let testing = req.cookies |> Map.containsKey "test"

  let components =
    [Sidebar.render req.query config.Vocabs testing
     Hotjar.render config.HotjarId]
    |> Seq.fold (fun acc comp -> acc + comp) ""

  {blah = SearchResults.createModel req config.GetSearchResults config.GetKBCount showOverview testing
   Components = components
   }
  |> DotLiquid.page "templates/home.html"


