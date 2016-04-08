module Viewer.Tests.Components.Sidebar.Tests

open Suave
open NUnit.Framework
open Swensen.Unquote
open Viewer.Types
open Viewer.Data.Vocabs.VocabGeneration
open Viewer.Tests.Utils
open Viewer.Components.Sidebar
open Viewer.SuaveExtensions
open FSharp.RDF

[<SetUp>]
let ``Run before tests`` () =
  setTemplatesDir "../../../../src/viewer/bin/Release/"

[<Test>]
let ``Should add form with search action`` () =
  let action =
    render [] [] false
    |> parseHtml
    |> CQ.select "form"
    |> CQ.attr "action"
  test <@ action = "/qs/search" @>

[<Test>]
let ``Should present a vocabulary with a single term as an input checkbox`` () =
  let vocabs = [{Property = "vocab"
                 Root = Term {t with Label = "Vocab 1"
                                     Children = [Term {t with Label = "Term1"; Uri = uri "http://testing.com/Uri1"}]}}]

  let html = render [] vocabs false |> parseHtml

  let vocabs = html |> CQ.select ".vocab"

  let vocab1text = vocabs |> CQ.first |> CQ.text
  test <@ vocab1text.Contains("Vocab 1") @>

  let checkboxes = html |> CQ.select "input[type='checkbox']" |> CQ.select ".term"
  //test <@ checkboxes |> CQ.first |> CQ.attr "value" = "Uri1" @>
  test <@ checkboxes |> CQ.first |> CQ.attr "name" = "vocab" @>

  let labels = html |> CQ.select ".checkbox > label"
  test <@ labels |> CQ.first |> CQ.text = "Term1" @>

    
[<Test>]
let ``Should present the multiple vocabulary containing multiple terms`` () =
  let vocabs = [{Property = ""
                 Root = Term {t with Children = [Term t
                                                 Term t]}}]

  let html = render [] vocabs false |> parseHtml

  test <@ html |> CQ.select "input[type='checkbox']" |> CQ.select ".term" |> CQ.length = 2 @>

    
[<Test>]
let ``Should present the vocabulary term checkboxes unselected by default`` () =
  let vocabs = [{Property = ""
                 Root = Term {t with Children = [Term t]}}]

  let html = render [] vocabs false |> parseHtml

  test <@ html |> CQ.select "input[checked]" |> CQ.length = 0 @>

    
[<Test>]
let ``Should present the vocabulary term checkboxes as selected when they exist in the querystring`` () =
  let vocabs = [{Property = "vocab"
                 Root = Term {t with Children = [Term {t with Uri = uri "http://testing.com/Uri1"}
                                                 Term {t with Uri = uri "http://testing.com/Uri2"}]}}]

  let qs = [("vocab", Some "http://testing.com/Uri2")]
  let html = render qs vocabs false |> parseHtml

  let selectedCheckboxes = html |> CQ.select "input[checked]"

  test <@ selectedCheckboxes |> CQ.length = 1 @>
  test <@ selectedCheckboxes |> CQ.first |> CQ.attr "value" = "http://testing.com/Uri2" @>

    
[<Test>]
let ``Should have an apply filters button`` () =
  let searchButtonLabel =
    render [] [] false
    |> parseHtml
    |> CQ.select ":submit"
    |> CQ.attr "Value"
  test <@ searchButtonLabel = "Apply filters" @>

    
[<Test>]
let ``Should present the vocabulary collapsed by default`` () =
  let vocabs = [{Property = ""
                 Root = Term {t with Children = []}}]

  let html = render [] vocabs false |> parseHtml

  let accordians = html |> CQ.select ".accordion.closed"

  test <@ accordians |> CQ.length = 1 @>

    
[<Test>]
let ``Should present the vocabulary expanded if vocabulary term is in querystring filters`` () =
  let vocabs = [{Property = "vocab:1"
                 Root = Term {t with Children = [Term {t with Uri = uri "http://testing.com/Uri#Term"}]}}
                {Property = "vocab:2"
                 Root = Term t}]

  let qsWithOneFilter = [("vocab%3A1",Some "http://testing.com/Uri#Term")]

  let html = render qsWithOneFilter vocabs false |> parseHtml

  test <@ html |> CQ.select ".accordion.closed.open" |> CQ.length = 1 @>
