module Viewer.Fuchu.Test.AnnotationToolTests

open Suave
open Suave.DotLiquid
open Fuchu
open Swensen.Unquote
open FSharp.Data
open Viewer.Tests.Utils
open Viewer.VocabGeneration

let tests =
  setTemplatesDir "src/viewer/bin/Release/"

  testList "Annotation tool tests" [
    testCase "Should generate yaml from annotation tree" <| fun _ ->
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
  ]
