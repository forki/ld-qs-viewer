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
let ``AnnotationApi: When I call getVocabList with an Ontology config I am returned the expected termd structure`` () = 
  let vocabTree = getVocabList Stubs.dummyOntologyConfigVocab

  vocabTree |> should equal Stubs.dummyVocabulary

[<Test>]
let ``AnnotationApi: When I call getPropertyList with an Ontology config I am returned the expected list of Properties`` () = 
  let properties = getPropertyList Stubs.dummyOntologyConfigProperties

  properties |> should equal Stubs.dummyResponse_Properties.properties

[<Test>]
let ``AnnotationApi: When I call GetAnnotationToolData with a vocab I am returned a vocabulary response class structure`` () =
  let response = getAnnotationToolData Stubs.dummyVocabulary Stubs.dummyOntologyConfigVocab
  response |> should equal Stubs.dummyResponse_Vocabs

[<Test>]
let ``AnnotationApi: When I call getAnnotationToolJson with a vocab config and vocabs I am returned the expected Json`` () =
  let response = getAnnotationToolJson Stubs.dummyVocabulary Stubs.dummyOntologyConfigVocab
  
  match response with
  | Success s -> s.Replace("\r", "") |> should equal (Stubs.dummyJsonResponse_vocab.Replace("\r", ""))
  | Failure f -> f |> should equal ""

[<Test>]
let ``AnnotationApi: When I call getAnnotationToolJson with a property config and vocabs I am returned the expected Json`` () =
  let response = getAnnotationToolJson Stubs.dummyVocabulary Stubs.dummyOntologyConfigProperties
  
  match response with
  | Success s -> s.Replace("\r", "") |> should equal (Stubs.dummyJsonResponse_properties.Replace("\r", ""))
  | Failure f -> f |> should equal ""

[<Test>]
let ``AnnotationAPI: End to End: When I make a GET to the Annotation Api it should return the annotation json from`` () =
  let response = startServerWith { baseConfig with 
                                    Vocabs = Stubs.dummyVocabulary 
                                    OntologyConfig = Stubs.dummyOntologyConfigFull }
                 |> get "/annotationsformschema"
                 
  response 
  |> CQ.text 
  |> (fun x -> x.ToString().Replace("\r", "")) 
  |> should equal (Stubs.dummyJsonResponse_full.Replace("\r", ""))

