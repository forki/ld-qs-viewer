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
  let response = Stubs.dummyConfigFile |> OntologyConfig.build

  response |> should equal Stubs.dummyOntologyConfigUri 

[<Test>]
let ``AnnotationApi: When I call getVocab with an Ontology config I am returned the expected termd structure`` () = 
  let vocabTree = getVocabList Stubs.dummyOntologyConfigFull

  vocabTree |> should equal Stubs.dummyVocabulary

[<Test>]
let ``AnnotationApi: When I call GetAnnotationToolData with a vocab I am returned a vocabulary response class structure `` () =
  let response = getAnnotationToolData Stubs.dummyVocabulary Stubs.dummyOntologyConfigFull
  response |> should equal Stubs.dummyResponse_Vocabs

[<Test>]
let ``AnnotationAPI: Should generate annotation ontology tree json from get`` () =
  let response = startServerWith { baseConfig with 
                                    Vocabs = Stubs.dummyVocabulary 
                                    OntologyConfig = Stubs.dummyOntologyConfigFull }
                 |> get "/annotationtool/formdata"
                 
  response 
  |> CQ.text 
  |> (fun x -> x.ToString().Replace("\r", "")) 
  |> should equal (Stubs.dummyJsonResponse_Vocabs.Replace("\r", ""))

