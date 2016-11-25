module Viewer.AppConfig

open System
open Viewer.Data.Search.Search
open Viewer.Data.Search.Elastic
open Viewer.Data.Vocabs.VocabGeneration
open FSharp.RDF
open Viewer.Config
open Viewer.Types

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

let prodOntologyConfig = {
  CoreTtl= Uri "http://schema/ontologies/qualitystandard.ttl"
  Contexts= [ { Prefix="qualitystandard"; Value= "https://nice.org.uk/ontologies/qualitystandard/" } ]
  Predicates = [
      { Uri= "qualitystandard:62496684_7027_4f37_bd0e_264c9ff727fd"; SourceTtl= Uri "http://schema/ontologies/setting.ttl" }
      { Uri= "qualitystandard:4e7a368e_eae6_411a_8167_97127b490f99"; SourceTtl= Uri "http://schema/ontologies/agegroup.ttl" }
      { Uri= "qualitystandard:7ae8413a_2811_4a09_a655_eff8d276ec87"; SourceTtl= Uri "http://schema/ontologies/servicearea.ttl" }
      { Uri= "qualitystandard:18aa6468_de94_4f9f_bd7a_0075fba942a5"; SourceTtl= Uri "http://schema/ontologies/factorsaffectinghealthorwellbeing.ttl" }
      { Uri= "qualitystandard:28745bc0_6538_46ee_8b71_f0cf107563d9"; SourceTtl= Uri "http://schema/ontologies/conditionordisease.ttl" }
  ]
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
    let vocabs = readVocabsFromFiles prodOntologyConfig
    {Vocabs = vocabs
     RenderedVocabs = renderVocabs vocabs
     PerformSearch = performSearchWithProvider Viewer.Data.Search.Elastic.search 
     GetKBCount = KnowledgeBaseCount
     HotjarId = Environment.GetEnvironmentVariable "HOTJARID"
     GAId = Environment.GetEnvironmentVariable "GAID"
     OntologyConfig = prodOntologyConfig}
