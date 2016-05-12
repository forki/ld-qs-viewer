module Viewer.Tests.Components.Sidebar.Tests

open Suave
open NUnit.Framework
open FsUnit
open Viewer.Types
open Viewer.Data.Vocabs.VocabGeneration
open Viewer.Tests.Utils
open Viewer.Components
open Viewer.SuaveExtensions
open FSharp.RDF

[<SetUp>]
let ``Run before tests`` () =
  setTemplatesDir "../../../../src/viewer/bin/Release/"

[<Test>]
let ``Should add form with search action`` () =
  Sidebar.render ""
  |> parseHtml
  |> CQ.select "form"
  |> CQ.attr "action"
  |> should equal "/qs/search"

[<Test>]
let ``Should have an apply filters button`` () =
  Sidebar.render ""
  |> parseHtml
  |> CQ.select ":submit"
  |> CQ.attr "Value"
  |> should equal "Apply filters"

[<Test>]
let ``Should render sidebar with prerendered vocabs`` () =
  let renderedVocabs = """<div class="renderedVocabs"></div>"""

  Sidebar.render renderedVocabs
  |> parseHtml
  |> CQ.select ".renderedVocabs"
  |> CQ.length 
  |> should equal 1
