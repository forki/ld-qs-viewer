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
  PerformSearch : AggregatedFilter list -> SearchResult list
  GetKBCount : bool -> int
  HotjarId : string
  GAId : string
  OntologyConfig : OntologyConfig
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
     GAId = "whoisjaridanyway"
     OntologyConfig = Stubs.ontologyConfig}
  | Prod ->
    let vocabs = readVocabsFromFiles Stubs.ontologyConfig
    {Vocabs = vocabs
     RenderedVocabs = renderVocabs vocabs
     PerformSearch = performSearchWithProvider Viewer.Data.Search.Elastic.search 
     GetKBCount = KnowledgeBaseCount
     HotjarId = Environment.GetEnvironmentVariable "HOTJARID"
     GAId = Environment.GetEnvironmentVariable "GAID"
     OntologyConfig = Stubs.ontologyConfig}
