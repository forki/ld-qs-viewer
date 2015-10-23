module Viewer.Types

type Filter = {
  Vocab: string
  TermUri: string
}

type SearchResult = {
  Uri: string
  Title: string
  Abstract: string
}

type Tag = {
  Label: string
  RemovalQueryString: string
}
