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
  setTemplatesDir "../../../../bin/viewer/web/qs/"

[<Test>]
let ``Should add form with search action`` () =
  Sidebar.render "" 0
  |> parseHtml
  |> CQ.select "form"
  |> CQ.attr "action"
  |> should equal "/search"

[<Test>]
let ``Should have an apply filters button`` () =
  Sidebar.render "" 0
  |> parseHtml
  |> CQ.select ":submit"
  |> CQ.attr "Value"
  |> should equal "Apply filters"

[<Test>]
let ``Should render sidebar with prerendered vocabs`` () =
  let renderedVocabs = """<div class="renderedVocabs"></div>"""

  Sidebar.render renderedVocabs 0
  |> parseHtml
  |> CQ.select ".renderedVocabs"
  |> CQ.length 
  |> should equal 1
