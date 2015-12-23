module Viewer.Fuchu.Test.YamlAnnotationTests

open Suave
open Suave.DotLiquid
open Fuchu
open FSharp.Data
open Viewer.Tests.Utils

[<Tests>]
let tests =
    setTemplatesDir "src/viewer/bin/Release/templates/"
    testList "Yaml Generation tests" [
        testCase "Generate YAML" <|
          fun _ ->
            let yamlWithTwoActiveFilters = "key=http%3A%2F%2Ftesting.com%2FUri%23Term1&key=http%3A%2F%2Ftesting.com%2FUri%23Term2"
            let html = startServerWith baseConfig |> getQuery "/annotationtool/toyaml" yamlWithTwoActiveFilters
            let yaml = html |> CQ.select ".yaml-content" |> CQ.text
            Assert.Equal("Term1 and Term 2 in YAML block", "key:\n  - Term1\n  - Term2\n", yaml)
        ]
