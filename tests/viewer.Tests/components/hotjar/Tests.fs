module Viewer.Tests.Components.HotJar.Tests

open Suave
open Suave.DotLiquid
open Fuchu
open Swensen.Unquote
open Viewer.Types
open Viewer.Data.Vocabs.VocabGeneration
open Viewer.Tests.Utils
open FSharp.RDF

[<Tests>]
let tests =
    setTemplatesDir "src/viewer/bin/Release/"

    testList "The knowledgebase site" [

      testCase "Should contain the hotjar script for heat map tracking" <| fun _ ->
        let script =
          startServerWith baseConfig
          |> get "/qs"
          |> CQ.select "script[id='hotjar']"
        test <@ script |> CQ.length = 1 @>
    ]
