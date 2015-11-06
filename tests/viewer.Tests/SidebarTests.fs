module Viewer.Tests.SidebarTests

open Suave
open Suave.DotLiquid
open NUnit.Framework
open Swensen.Unquote
open Viewer.Types
open Viewer.VocabGeneration
open Viewer.Tests.Utils
open FSharp.RDF

[<SetUp>]
let ``Run before tests`` () =
  setTemplatesDir "templates/"

[<Test>]
let ``Should set the title`` () =
  let title =
    startServerWith baseConfig
    |> get "/qs"
    |> CQ.select "title"
    |> CQ.text

  test <@ title = "BETA Quality Statements Discovery Tool | NICE" @>

[<Test>]
let ``Should add form with search action`` () =
  let action =
    startServerWith baseConfig
    |> get "/qs"
    |> CQ.select "form"
    |> CQ.attr "action"
  test <@ action = "/qs/search" @>

[<Test>]
let ``Should present a vocabulary with a single term as an input checkbox`` () =
  let vocabs = [{Property = "vocab"
                 Root = Term {t with Label = "Vocab 1"
                                     Children = [Term {t with Label = "Term1"; Uri = uri "http://testing.com/Uri1"}]}}]

  let html = startServerWith {baseConfig with Vocabs = vocabs} |> get "/qs"

  let vocabs = html |> CQ.select ".vocab"

  let vocab1text = vocabs |> CQ.first |> CQ.text
  test <@ vocab1text.Contains("Vocab 1") @>

  let checkboxes = html |> CQ.select "input[type='checkbox']"
  test <@ checkboxes |> CQ.first |> CQ.attr "value" = "http://testing.com/Uri1" @>
  test <@ checkboxes |> CQ.first |> CQ.attr "name" = "vocab" @>

  let labels = html |> CQ.select ".checkbox > label"
  test <@ labels |> CQ.first |> CQ.text = "Term1" @>

[<Test>]
let ``Should present the multiple vocabulary containing multiple terms`` () =

  let vocabs = [{Property = ""
                 Root = Term {t with Children = [Term t
                                                 Term t]}}]

  let html = startServerWith {baseConfig with Vocabs = vocabs} |> get "/qs"

  test <@ html |> CQ.select "input[type='checkbox']" |> CQ.length = 2 @>

[<Test>]
let ``Should present the vocabulary term checkboxes unselected by default`` () =

  let vocabs = [{Property = ""
                 Root = Term {t with Children = [Term t]}}]

  let html = startServerWith {baseConfig with Vocabs = vocabs} |> get "/qs"

  test <@ html |> CQ.select "input[checked]" |> CQ.length = 0 @>

[<Test>]
let ``Should present the vocabulary term checkboxes as selected when they exist in the querystring`` () =

  let vocabs = [{Property = "vocab"
                 Root = Term {t with Children = [Term {t with Uri = uri "http://testing.com/Uri1"}
                                                 Term {t with Uri = uri "http://testing.com/Uri2"}]}}]

  let html = startServerWith {baseConfig with Vocabs = vocabs}
             |> getQuery "/qs/search" "vocab=http%3A%2F%2Ftesting.com%2FUri2"

  let selectedCheckboxes = html |> CQ.select "input[checked]"

  test <@ selectedCheckboxes |> CQ.length = 1 @>
  test <@ selectedCheckboxes |> CQ.first |> CQ.attr "value" = "http://testing.com/Uri2" @>

[<Test>]
let ``Should have search button`` () =
  let searchButtonLabel =
    startServerWith baseConfig
    |> get "/qs"
    |> CQ.select ":submit"
    |> CQ.attr "Value"
  test <@ searchButtonLabel = "Search" @>

[<Test>]
let ``Should present the vocabulary collapsed by default`` () =

  let vocabs = [{Property = ""
                 Root = Term {t with Children = []}}]

  let html = startServerWith {baseConfig with Vocabs = vocabs} |> get "/qs"

  let accordians = html |> CQ.select ".accordion.closed"

  test <@ accordians |> CQ.length = 1 @>

[<Test>]
let ``Should present the vocabulary expanded if vocabulary term is in querystring filters`` () =

  let vocabs = [{Property = "vocab:1"
                 Root = Term {t with Children = [Term {t with Uri = uri "http://testing.com/Uri#Term"}]}}
                {Property = "vocab:2"
                 Root = Term t}]

  let qsWithOneFilter = "vocab%3A1=http%3A%2F%2Ftesting.com%2FUri%23Term"

  let html = startServerWith {baseConfig with Vocabs = vocabs}
             |> getQuery "/qs/search" qsWithOneFilter

  test <@ html |> CQ.select ".accordion-trigger.open" |> CQ.length = 1 @>
  test <@ html |> CQ.select ".accordion.closed.open" |> CQ.length = 1 @>
