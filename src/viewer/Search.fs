module Viewer.Search

open Suave
open Suave.Types
open Suave.Cookie
open Viewer.Utils
open Viewer.Types
open Viewer.Model
open Viewer.Elastic
open Viewer.VocabGeneration
open Viewer.Components

let search (req:HttpRequest) config =
  let testing = req.cookies |> Map.containsKey "test"

  {Sidebar = Sidebar.createModel req config.Vocabs testing
   blah = SearchResults.createModel req config.GetSearchResults config.GetKBCount false testing}
  |> DotLiquid.page "templates/home.html"

