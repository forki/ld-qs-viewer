module Viewer.Tests.Components.HotJar.Tests

open NUnit.Framework
open FsUnit
open Viewer.Tests.Utils
open Viewer.Components.Hotjar
open Viewer.SuaveExtensions

[<Test>]
let ``Should contain the hotjar script for heat map tracking`` () =
  let script =
    render "notused"
    |> parseHtml
    |> CQ.select "script[id='hotjar']"
  script |> CQ.length |> should equal 1

      
[<Test>]
let ``Should contain HotJar Id`` () =
  let hotjarId = "12345"
  let script =
    render hotjarId
    |> parseHtml
    |> CQ.select "script[id='hotjar']"
    |> CQ.text
  script |> should contain hotjarId

