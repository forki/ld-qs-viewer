module Viewer.Tests.Components.GoogleAnalytics.Tests

open NUnit.Framework
open FsUnit
open Viewer.Tests.Utils
open Viewer.Components.GoogleAnalytics
open Viewer.SuaveExtensions

[<Test>]
let ``Should contain the google analytics script`` () =
  let script =
    render "notused"
    |> parseHtml
    |> CQ.select "script[id='googleanalytics']"
  script |> CQ.length |> should equal 1

[<Test>]
let ``Should contain correct GA account Id`` () =
  let id = "12345"
  let script =
    render id
    |> parseHtml
    |> CQ.select "script[id='googleanalytics']"
    |> CQ.text
  script |> should contain id
