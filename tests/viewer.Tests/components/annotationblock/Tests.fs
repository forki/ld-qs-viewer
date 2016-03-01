module Viewer.Tests.Components.AnnotationBlock.Tests

open Suave
open Fuchu
open Swensen.Unquote
open Viewer.Types
open Viewer.Data.Vocabs.VocabGeneration
open Viewer.Tests.Utils
open Viewer.SuaveExtensions
open FSharp.RDF


let vocabs = [{Property = "vocab:property";
                      Root = Term {t with Label = "Vocab Label";
                                         Children = [Term {t with Uri = uri "http://testing.com/Uri#Term1"}
                                                     Term {t with Uri = uri "http://testing.com/Uri#Term2"}]}}]
[<Tests>]
let tests =



  setTemplatesDir "src/viewer/bin/Release/"

  testList "Annotation block component" [
    testCase "Should generate annotation block from querystring" <| fun _ ->
      let vocabs = [{Property = "vocab:property"
                     Root = Term {t with Label = "Vocab Label"
                                         Children = [Term {t with Uri = uri "http://testing.com/Uri#Term1"}
                                                     Term {t with Uri = uri "http://testing.com/Uri#Term2"}]}}]

      let qsWithTwoVocabTerms = "vocab%3Aproperty=http%3A%2F%2Ftesting.com%2FUri%23Term1&vocab%3Aproperty=http%3A%2F%2Ftesting.com%2FUri%23Term2"

      let yaml = startServerWith {baseConfig with Vocabs = vocabs}
                 |> getQuery "/annotationtool/toyaml" qsWithTwoVocabTerms
                 |> CQ.select ".yaml-content"
                 |> CQ.text

      test <@ yaml = "Vocab Label:\n  - \"Term1\"\n  - \"Term2\"\n" @>

    testCase "Should produce error upon no vocabulary selection" <| fun _ ->
      let errorMessage = startServerWith {baseConfig with Vocabs = vocabs}
                         |> getQuery "/annotationtool/toyaml" ""
                         |> CQ.select ".message"
                         |> CQ.text

      test <@ errorMessage = "Please select an annotation from vocabulary." @>

  ]
