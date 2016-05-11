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
  let action =
    Sidebar.renderNew ""
    |> parseHtml
    |> CQ.select "form"
    |> CQ.attr "action"
  action |> should equal "/qs/search"

[<Test>]
let ``Should have an apply filters button`` () =
  let searchButtonLabel =
    Sidebar.render [] [] false
    |> parseHtml
    |> CQ.select ":submit"
    |> CQ.attr "Value"
  searchButtonLabel |> should equal "Apply filters"

[<Test>]
let ``Should render sidebar with prerendered vocabs`` () =
  let renderedVocabs = """<div class="renderedVocabs"></div>"""

  Sidebar.renderNew renderedVocabs
  |> parseHtml
  |> CQ.select ".renderedVocabs"
  |> should equal 1
