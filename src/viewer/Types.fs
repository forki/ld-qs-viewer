module Viewer.Types

type Filter = {
  Key: string
  Val: string
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
