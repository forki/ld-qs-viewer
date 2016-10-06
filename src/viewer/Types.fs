module Viewer.Types

type Filter = {
  Vocab: string
  TermUri: string
}

type AggregatedFilter = {
  Vocab: string
  TermUris: string list
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
