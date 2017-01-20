module Viewer.AppConfig

open System
open Viewer.Data.Search.Search
open Viewer.Data.Search.Elastic
open Viewer.Data.Vocabs.VocabGeneration
open FSharp.RDF
open FSharp.Data
open Viewer.Config
open Viewer.Types
open Viewer.ApiTypes
open Serilog
open NICE.Logging

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
    let config = Stubs.dummyConfigFile |> OntologyConfig.build
    {Vocabs = Stubs.vocabs
     RenderedVocabs = renderVocabs Stubs.vocabs
     PerformSearch = performSearchWithProvider Stubs.search
     GetKBCount = Stubs.getKBCount
     HotjarId = "whoisjaridanyway"
     GAId = "whoisjaridanyway"
     OntologyConfig = config}
  | Prod ->
    let config = try Http.RequestString "http://dev:20002/ontologies/annotationconfig.json" |> OntologyConfig.build
                 with ex -> Log.Error(sprintf "Exception encountered reading configFile\n%s" ( ex.ToString() ) )
                            { CoreTtl = Uri ""; Contexts = []; Ontologies = []; Properties = []}
    let vocabs = config |> readVocabsFromFiles
    {Vocabs = vocabs
     RenderedVocabs = renderVocabs vocabs
     PerformSearch = performSearchWithProvider Viewer.Data.Search.Elastic.search 
     GetKBCount = KnowledgeBaseCount
     HotjarId = Environment.GetEnvironmentVariable "HOTJARID"
     GAId = Environment.GetEnvironmentVariable "GAID"
     OntologyConfig = config}
