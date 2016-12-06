module Viewer.Tests.Components.AnnotationApi.Tests

open Suave
open NUnit.Framework
open FsUnit
open Viewer.Types
open Viewer.ApiTypes
open Viewer.Data.Vocabs.VocabGeneration
open Viewer.Tests.Utils
open Viewer.SuaveExtensions
open FSharp.RDF
open Viewer.YamlParser
open Viewer.Utils
open Stubs
open Viewer.Data.Vocabs.VocabGeneration
open Viewer.AnnotationApi


[<Test>]
let ``AnnotationApi: Should serialize the config json`` () =
  let response = Stubs.thingyConfigFile |> OntologyConfig.build

  response |> should equal Stubs.thingyOntologyConfigUri 

[<Test>]
let ``AnnotationApi: When I call getVocab with an Ontology config I am returned the expected termd structure`` () = 
  let vocabTree = getVocabList Stubs.thingyOntologyConfigFull

  vocabTree |> should equal Stubs.thingyVocabulary

[<Test>]
let ``AnnotationApi: When I call GetAnnotationToolData I am returned a response class structure `` () =
  let response = getAnnotationToolData Stubs.thingyVocabulary Stubs.thingyOntologyConfigFull
  response |> should equal Stubs.thingyResponse

[<Test>]
let ``AnnotationAPI: Should generate annotation ontology tree json from get`` () =
  let response = startServerWith { baseConfig with 
                                    Vocabs = Stubs.thingyVocabulary 
                                    OntologyConfig = Stubs.thingyOntologyConfigFull }
                 |> get "/annotationsformschema"
                 
  response 
  |> CQ.text 
  |> (fun x -> x.ToString().Replace("\r", "")) 
  |> should equal (Stubs.thingyJsonResponse.Replace("\r", ""))
