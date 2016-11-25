module Viewer.Tests.Components.AnnotationApi.Tests

open Suave
open NUnit.Framework
open FsUnit
open Viewer.Types
open Viewer.Data.Vocabs.VocabGeneration
open Viewer.Tests.Utils
open Viewer.SuaveExtensions
open FSharp.RDF
open Viewer.YamlParser
open Viewer.Utils
open Stubs
open Viewer.Data.Vocabs.VocabGeneration
open Viewer.AnnotationApi

[<SetUp>]
let ``Run before tests`` () =
  setTemplatesDir "../../../../bin/viewer/web/qs/"

[<Test>]
let ``AnnotationApi: When I call getVocab with an Ontology config I am returned the expected termd structure?`` () = 
  let r = getVocabList Stubs.thingyOntologyConfig

  r |> should equal Stubs.thingyVocabulary

[<Test>]
let ``AnnotationApi: When I call GetAnnotationToolData I am returned a response class structure `` () =
  let response = getAnnotationToolData Stubs.thingyVocabulary Stubs.thingyOntologyConfig
  response |> should equal Stubs.thingyResponse

[<Test>]
let ``AnnotationAPI: Should generate annotation ontology tree json from get`` () =
  let response = startServerWith { baseConfig with Vocabs = Stubs.thingyVocabulary; OntologyConfig = Stubs.thingyOntologyConfig }
                 |> get "/annotationtool/formdata"
                 
  response |> CQ.text |> should equal Stubs.thingyJsonResponse