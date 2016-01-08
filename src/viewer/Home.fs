module Viewer.Home

open Suave
open Suave.Types
open Suave.Cookie
open Viewer.Model
open Viewer.Utils
open Viewer.VocabGeneration
open Viewer.Elastic
open Viewer.Components

let home (req:HttpRequest) config =
  let testing = req.cookies |> Map.containsKey "test"

  {Sidebar = Sidebar.createModel req config.Vocabs testing
   blah = SearchResults.createModel req config.GetSearchResults config.GetKBCount true testing}
  |> DotLiquid.page "home.html"
