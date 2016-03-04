module Viewer.Tests.Components.HotJar.Tests

open Fuchu
open Swensen.Unquote
open Viewer.Tests.Utils
open Viewer.Components.Hotjar
open Viewer.SuaveExtensions

[<Tests>]
let tests =
    setTemplatesDir "src/viewer/bin/Release/"

    testList "The knowledgebase site" [

      testCase "Should contain the hotjar script for heat map tracking" <| fun _ ->
        let script =
          render "notused"
          |> parseHtml
          |> CQ.select "script[id='hotjar']"
        test <@ script |> CQ.length = 1 @>

      testCase "Should contain HotJar Id" <| fun _ ->
        let hotjarId = "12345"
        let script =
          render hotjarId
          |> parseHtml
          |> CQ.select "script[id='hotjar']"
          |> CQ.text
        test <@ script.Contains hotjarId @>
      
    ]

