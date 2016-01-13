module Viewer.Tests.Components.AnnotationSidebar.Tests

open Suave
open Suave.DotLiquid
open Fuchu
open Swensen.Unquote
open Viewer.Types
open Viewer.VocabGeneration
open Viewer.Tests.Utils
open FSharp.RDF

[<Tests>]
let tests =
  setTemplatesDir "src/viewer/bin/Release/"

  testList "Annotation sidebar component" [

    testCase "Should present the vocabulary term checkboxes unselected by default" <| fun _ ->
      let vocabs = [{Property = ""
                     Root = Term {t with Children = [Term t]}}]

      let html = startServerWith {baseConfig with Vocabs = vocabs} |> get "/annotationtool"

      test <@ html |> CQ.select "input[checked]" |> CQ.length = 0 @>

    testCase "Should present the vocabulary term checkboxes as selected when they exist in the querystring" <| fun _ ->
      let vocabs = [{Property = "vocab"
                     Root = Term {t with Children = [Term {t with Uri = uri "http://testing.com/Uri1"}
                                                     Term {t with Uri = uri "http://testing.com/Uri2"}]}}]

      let html = startServerWith {baseConfig with Vocabs = vocabs}
                 |> getQuery "/annotationtool/toyaml" "vocab=http%3A%2F%2Ftesting.com%2FUri2"

      let selectedCheckboxes = html |> CQ.select "input[checked]"

      test <@ selectedCheckboxes |> CQ.length = 1 @>
      test <@ selectedCheckboxes |> CQ.first |> CQ.attr "value" = "http://testing.com/Uri2" @>

    testCase "Should present the vocabulary collapsed by default" <| fun _ ->
      let vocabs = [{Property = ""
                     Root = Term {t with Children = []}}]

      let html = startServerWith {baseConfig with Vocabs = vocabs} |> get "/annotationtool"

      let accordians = html |> CQ.select ".accordion.closed"

      test <@ accordians |> CQ.length = 1 @>

    testCase "Should present the vocabulary expanded if vocabulary term is in querystring filters" <| fun _ ->
      let vocabs = [{Property = "vocab:1"
                     Root = Term {t with Children = [Term {t with Uri = uri "http://testing.com/Uri#Term"}]}}
                    {Property = "vocab:2"
                     Root = Term t}]

      let qsWithOneFilter = "vocab%3A1=http%3A%2F%2Ftesting.com%2FUri%23Term"

      let html = startServerWith {baseConfig with Vocabs = vocabs}
                 |> getQuery "/annotationtool/toyaml" qsWithOneFilter

      test <@ html |> CQ.select ".accordion.closed.open" |> CQ.length = 1 @>
  ]
