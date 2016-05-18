module Viewer.AppConfig

open System
open Viewer.Types
open Viewer.Data.Search.Elastic
open Viewer.Data.Vocabs.VocabGeneration
open FSharp.RDF

type Mode =
  | Dev
  | Prod

type AppConfiguration = {
  Vocabs : Vocabulary list
  RenderedVocabs : string
  GetSearchResults : (bool -> string -> SearchResult list)
  GetKBCount : bool -> int
  HotjarId : string
}

let getAppConfig mode =
  match mode with
  | Dev ->
    printf "RUNNING DEV MODE: Using stubbed data\n"
    {Vocabs = Stubs.vocabs
     RenderedVocabs = renderVocabs Stubs.vocabs
     GetSearchResults = Stubs.getSearchResults
     GetKBCount = Stubs.getKBCount
     HotjarId = "whoisjaridanyway"}
  | Prod ->
    let vocabs = readVocabsFromFiles ()
    {Vocabs = vocabs
     RenderedVocabs = renderVocabs vocabs
     GetSearchResults = GetSearchResults RunElasticQuery
     GetKBCount = KnowledgeBaseCount
     HotjarId = Environment.GetEnvironmentVariable "HOTJARID"}
