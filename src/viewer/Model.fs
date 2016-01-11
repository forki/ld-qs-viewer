module Viewer.Model

open Viewer.Types
open Viewer.VocabGeneration
open Viewer.Components.Sidebar
open Viewer.Components.SearchResults

type HomeModel = {
  Sidebar: SidebarModel    
  blah: SearchResultsModel
}

type AppConfiguration = {
  Vocabs : Vocabulary list
  GetSearchResults : (bool -> string -> SearchResult list)
  GetKBCount : bool -> int
}
