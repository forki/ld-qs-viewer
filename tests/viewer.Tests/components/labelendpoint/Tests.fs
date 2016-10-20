module Viewer.Tests.Components.LabelEndPoint.Tests

open Suave
open NUnit.Framework
open FsUnit
open Viewer.Types
open Viewer.Data.Vocabs.VocabGeneration
open Viewer.Tests.Utils
open Viewer.SuaveExtensions
open FSharp.RDF

[<SetUp>]
let ``Run before tests`` () =
  setTemplatesDir "../../../../bin/viewer/web/qs/"

[<Test>]
let ``Should retrieve guid by label`` () =

  startServerWith baseConfig
  |> getQuery "/annotationtool/toyaml"
  |> CQ.text
  |> should equal ""
