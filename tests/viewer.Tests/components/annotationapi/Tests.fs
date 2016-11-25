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

[<SetUp>]
let ``Run before tests`` () =
  setTemplatesDir "../../../../bin/viewer/web/qs/"

let vocabs = [{Property = "vocab:property";
               Root = Term {t with Label = "Vocab Label";
                                   ShortenedUri = "vocabLabel/long-guid-1";
                                   Children = [Term {t with Uri = uri "http://testing.com/Uri#Term1"}
                                               Term {t with Label = "Vocab Label 2";
                                                            ShortenedUri = "vocabLabel2/long-guid-2";
                                                            Uri = uri "http://testing.com/Uri#Term2"}]}
               Label = "Setting"};
              {Property = "vocab:propertyServiceArea";
               Root = Term {t with Label = "Vocab Label";
                                   ShortenedUri = "vocabLabel/long-guid-1";
                                   Children = [Term {t with Uri = uri "http://testing.com/Uri#Term1"}
                                               Term {t with Label = "Vocab Label 3";
                                                            ShortenedUri = "vocabLabel3/long-guid-3";
                                                            Uri = uri "http://testing.com/Uri#Term3"}]}
               Label = "Service Area"}
]
//
//[<Test>]
//let ``AnnotationAPI: Should generate annotation ontology tree json from get`` () =
//  let response = startServerWith baseConfig
//                 |> get "/annotationtool/formdata"
//                 
//  response |> CQ.text |> should equal "Hello world!"

let termdResponse = [
  {
    Root = Term {
      Uri = Uri.from "https://nice.org.uk/ontologies/thingy/thingy_class"
      ShortenedUri = "thingy/thingy_class"
      Label = "Thingy Class"
      Selected = false
      Children = [
        Term {
          Uri = Uri.from "https://nice.org.uk/ontologies/thingy/thingy_level_1"
          ShortenedUri = "thingy/thingy_level_1"
          Label = "Thingy Level 1"
          Selected = false
          Children = [
            Term {
              Uri = Uri.from "https://nice.org.uk/ontologies/thingy/thingy_level_1_1"
              ShortenedUri = "thingy/thingy_level_1_1"
              Label = "Thingy Level 1.1"
              Selected = false
              Children = []
            }
            Term {
              Uri = Uri.from "https://nice.org.uk/ontologies/thingy/thingy_level_1_2"
              ShortenedUri = "thingy/thingy_level_1_2"
              Label = "Thingy Level 1.2"
              Selected = false
              Children = []
            }
          ]
        }
        Term {
          Uri = Uri.from "https://nice.org.uk/ontologies/thingy/thingy_level_2"
          ShortenedUri = "thingy/thingy_level_2"
          Label = "Thingy Level 2"
          Selected = false
          Children = []
        }
      ]
    }
    Property = "https://nice.org.uk/ontologies/core/applies_to_thingy";
    Label = "Thingy"
  }
]

[<Test>]
let ``AnnotationApi: When I call getVocab with an Ontology config I am returned the expected termd structure?`` () = 
  let r = getVocabList Stubs.ontologyConfig

  r |> should equal termdResponse