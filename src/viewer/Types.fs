module Viewer.Types

type VocabularyTerm = {
                 Name: string
                 Uri: string
               }
type Vocabulary = {
                  Name: string
                  Terms: VocabularyTerm list
                }

type SearchResult = {
  Uri: string
}
