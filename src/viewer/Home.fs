module Viewer.Home

open Suave
open Suave.Types
open Suave.Cookie
open Viewer.Utils
open Viewer.AppConfig
open Viewer.Components
open Viewer.Components.Sidebar
open Viewer.Components.SearchResults

type HomeModel = {
  Sidebar: SidebarModel    
  blah: SearchResultsModel
}

let home (req:HttpRequest) config showOverview =
  let testing = req.cookies |> Map.containsKey "test"

  {Sidebar = Sidebar.createModel req config.Vocabs testing
   blah = SearchResults.createModel req config.GetSearchResults config.GetKBCount showOverview testing}
  |> DotLiquid.page "templates/home.html"
