module Viewer.Model

open Viewer.Types
open Viewer.VocabGeneration

type ViewVocab = {
  Vocab: Vocabulary
  Expanded: bool
}

type SidebarModel = {
  Vocabularies: ViewVocab list
}

type SearchResultsModel = {
  Results: SearchResult list
  Tags: Tag list
  totalCount: int
  ShowHelp : bool
}

type HomeModel = {
  Sidebar: SidebarModel    
  blah: SearchResultsModel
}

type AppConfiguration = {
  Vocabs : Vocabulary list
  GetSearchResults : (bool -> string -> SearchResult list)
  GetKBCount : bool -> int
}
