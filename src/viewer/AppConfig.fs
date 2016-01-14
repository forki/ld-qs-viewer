module Viewer.AppConfig

open Viewer.Types
open Viewer.Elastic
open Viewer.VocabGeneration
open FSharp.RDF

type Mode =
  | Dev
  | Prod

type AppConfiguration = {
  Vocabs : Vocabulary list
  GetSearchResults : (bool -> string -> SearchResult list)
  GetKBCount : bool -> int
}

let getAppConfig mode =
  match mode with
  | Dev ->
    printf "RUNNING DEV MODE: Using stubbed data\n"
    {Vocabs = Stubs.vocabs
     GetSearchResults = Stubs.getSearchResults
     GetKBCount = Stubs.getKBCount}
  | Prod ->
    {Vocabs = readVocabsFromFiles ()
     GetSearchResults = GetSearchResults RunElasticQuery
     GetKBCount = KnowledgeBaseCount}
