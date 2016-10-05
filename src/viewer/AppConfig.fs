module Viewer.AppConfig

open System
open Viewer.Types
open Viewer.Data.Search.Search
open Viewer.Data.Search.Elastic
open Viewer.Data.Vocabs.VocabGeneration
open FSharp.RDF

type Mode =
  | Dev
  | Prod

type AppConfiguration = {
  Vocabs : Vocabulary list
  RenderedVocabs : string
  PerformSearch : Filter list -> SearchResult list
  GetKBCount : bool -> int
  HotjarId : string
  GAId : string
}

let getAppConfig mode =
  match mode with
  | Dev ->
    printf "RUNNING DEV MODE: Using stubbed data\n"
    {Vocabs = Stubs.vocabs
     RenderedVocabs = renderVocabs Stubs.vocabs
     PerformSearch = performSearchWithProvider Stubs.search
     GetKBCount = Stubs.getKBCount
     HotjarId = "whoisjaridanyway"
     GAId = "whoisjaridanyway"}
  | Prod ->
    let vocabs = readVocabsFromFiles ()
    {Vocabs = vocabs
     RenderedVocabs = renderVocabs vocabs
     PerformSearch = performSearchWithProvider Viewer.Data.Search.Elastic.search 
     GetKBCount = KnowledgeBaseCount
     HotjarId = Environment.GetEnvironmentVariable "HOTJARID"
     GAId = Environment.GetEnvironmentVariable "GAID"}
