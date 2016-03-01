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
}

let page (req:HttpRequest) config showOverview =
  let testing = req.cookies |> Map.containsKey "test"

  {Sidebar = Sidebar.createModel req config.Vocabs testing
   blah = SearchResults.createModel req config.GetSearchResults config.GetKBCount showOverview testing
   hotjar = Hotjar.createModel config.HotjarId 
   }
  |> DotLiquid.page "templates/home.html"

let template (req:HttpRequest) config showOverview =
  [Hotjar.Build config.HotjarId]
  |> Seq.fold (fun acc comp -> acc + comp) ""
  |> Successful.OK
   

