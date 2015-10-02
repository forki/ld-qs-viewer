module Viewer.Types

type VocabularyTerm = {
                 Name: string
                 Uri: string
               }
type Vocabulary = {
                  Label: string
                  Name: string
                  Terms: VocabularyTerm list
                }

type SearchResult = {
  Uri: string
  Abstract: string
}
