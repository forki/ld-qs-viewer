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
  Sidebar: SidebarModel    
  blah: SearchResultsModel
  hotjar: HotjarModel
  Components: string
}

let page (req:HttpRequest) config showOverview =
  let testing = req.cookies |> Map.containsKey "test"

  let components =
    [Hotjar.render config.HotjarId]
    |> Seq.fold (fun acc comp -> acc + comp) ""

  {Sidebar = Sidebar.createModel req config.Vocabs testing
   blah = SearchResults.createModel req config.GetSearchResults config.GetKBCount showOverview testing
   hotjar = Hotjar.createModel config.HotjarId 
   Components = components
   }
  |> DotLiquid.page "templates/home.html"


