module Viewer.Tests.Components.HotJar.Tests

open Suave
open Suave.DotLiquid
open Fuchu
open Swensen.Unquote
open Viewer.Types
open Viewer.Data.Vocabs.VocabGeneration
open Viewer.Tests.Utils
open FSharp.RDF

let hotjarId = "12344"

[<Tests>]
let tests =

    testList "The knowledgebase site" [

      testCase "Should contain the hotjar script for heat map tracking" <| fun _ ->
        let script =
          startServerWith baseConfig
          |> get "/qs"
          |> CQ.select "script[id='hotjar']"
        test <@ script |> CQ.length = 1 @>

      testCase "Should contain HotJar Id" <| fun _ ->
        let script =
          startServerWith { baseConfig with HotjarId = hotjarId} 
          |> get "/qs"
          |> CQ.select "script[id='hotjar']"
          |> CQ.text
        test <@ script.Contains hotjarId @>
    ]
