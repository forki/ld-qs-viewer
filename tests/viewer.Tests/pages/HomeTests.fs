module Viewer.Tests.HomeTests

open NUnit.Framework
open Viewer.Pages.Home
open Suave
open FsUnit
open Viewer.Types
open Viewer.Data.Vocabs.VocabGeneration
open Viewer.Tests.Utils
open Viewer.Components
open Viewer.SuaveExtensions

[<SetUp>]
let ``Run before tests`` () =
  setTemplatesDir "../../../../bin/viewer/web/qs/"

[<Test>]
let ``When relevancy exists in qs Should return true`` () =
  relevancyValueFromQueryString [("relevancyTest", Some("1"))]
  |> should equal 1
  relevancyValueFromQueryString [("relevancyTest", Some("2"))]
  |> should equal 2

[<Test>]
let ``When relevancy does not exist in qs Should return false`` () =
  relevancyValueFromQueryString []
  |> should equal 0

[<Test>]
let ``When relevancy exists in qs with any other value Should return false`` () =
  relevancyValueFromQueryString [("relevancyTest", Some("A"))]
  |> should equal 0

[<Test>]
let ``When variable exists but called something else in qs Should return false`` () =
  relevancyValueFromQueryString [("anythingTest", Some("A"))]
  |> should equal 0


[<Test>]
let ``When rendering sidebar with relevancyTest in qs with value of 1 Should render hidden field`` () =
  Sidebar.render "" 1 
  |> parseHtml
  |> CQ.select "input[type='hidden']"
  |> CQ.attr "value"
  |> should equal "1"

[<Test>]
let ``When rendering sidebar with relevancyTest in qs with value of 2 Should render hidden field`` () =
  Sidebar.render "" 2 
  |> parseHtml
  |> CQ.select "input[type='hidden']"
  |> CQ.attr "value"
  |> should equal "2"

[<Test>]
let ``When rendering sidebar with relevancyTest in qs with value of anything else Should render hidden field`` () =
  Sidebar.render "" 0 
  |> parseHtml
  |> CQ.select "input[type='hidden']"
  |> CQ.attr "value"
  |> should equal null

  Sidebar.render "" 3 
  |> parseHtml
  |> CQ.select "input[type='hidden']"
  |> CQ.attr "value"
  |> should equal null

[<Test>]
let `` When relevancy flag is passed Should return true `` () =
  let args = [("relevancyTest", Some("1"))]

  args 
  |> relevancyFlagExists
  |> should equal true
  

