module Viewer.Fuchu.Test.AnnotationToolTests

open Suave
open Suave.DotLiquid
open Fuchu
open Swensen.Unquote
open FSharp.Data
open Viewer.Tests.Utils

[<Tests>]
let tests =
  setTemplatesDir "src/viewer/bin/Release/templates/"

  testList "Annotation tool tests" [
    testCase "Generate annotations yaml block from tree" <| fun _ ->
      let qsWithTwoActiveFilters = "key=http%3A%2F%2Ftesting.com%2FUri%23Term1&key=http%3A%2F%2Ftesting.com%2FUri%23Term2"
      let html = startServerWith baseConfig |> getQuery "/annotationtool/toyaml" qsWithTwoActiveFilters
      test <@ html |> CQ.select ".yaml-content" |> CQ.text = "key:\n  - Term1\n  - Term2\n" @>
  ]
