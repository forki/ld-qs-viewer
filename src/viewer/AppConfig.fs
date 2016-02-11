module Viewer.AppConfig

open Viewer.Types
open Viewer.Data.Search.Elastic
open Viewer.Data.Vocabs.VocabGeneration
open FSharp.RDF

type Mode =
  | Dev
  | Prod

type AppConfiguration = {
  Vocabs : Vocabulary list
  GetSearchResults : (bool -> string -> SearchResult list)
  GetKBCount : bool -> int
  HotjarId : string
}

let getAppConfig mode =
  match mode with
  | Dev ->
    printf "RUNNING DEV MODE: Using stubbed data\n"
    {Vocabs = Stubs.vocabs
     GetSearchResults = Stubs.getSearchResults
     GetKBCount = Stubs.getKBCount
     HotjarId = "152152"}
  | Prod ->
    {Vocabs = readVocabsFromFiles ()
     GetSearchResults = GetSearchResults RunElasticQuery
     GetKBCount = KnowledgeBaseCount
     HotjarId = "130640"}
