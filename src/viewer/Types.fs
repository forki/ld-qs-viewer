module Viewer.Types

type Filter = {
  Vocab: string
  TermUri: string
}

type LabelledFilter = {
  VocabLabel: string
  TermUri: string
}

type SearchResult = {
  Uri: string
  Title: string
  Abstract: string
  FirstIssued: System.DateTime 
}

type Tag = {
  Label: string
  RemovalQueryString: string
}

type Component = {
  Content : string
  Script : string
}
