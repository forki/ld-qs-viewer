module Viewer.Model

open Viewer.Types
open Viewer.VocabGeneration

type ViewVocab = {
  Vocab: Vocabulary
  Expanded: bool
}

type Model = {
  Results: SearchResult list
  Tags: Tag list
  Vocabularies: ViewVocab list
  totalCount: int
  ShowHelp : bool
}
