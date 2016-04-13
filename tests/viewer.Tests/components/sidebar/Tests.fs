module Viewer.Tests.Components.Sidebar.Tests

open Suave
open NUnit.Framework
open Swensen.Unquote
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
    Sidebar.render [] [] false
    |> parseHtml
    |> CQ.select "form"
    |> CQ.attr "action"
  test <@ action = "/qs/search" @>

[<Test>]
let ``Should present a vocabulary with a single term as an input checkbox`` () =
  let vocabs = [{Property = "vocab"
                 Root = Term {t with Label = "Vocab 1"
                                     Children = [Term {t with Label = "Term1"; Uri = uri "http://testing.com/Uri1"; ShortenedUri="Uri1"}]}}]

  let html = Sidebar.render [] vocabs false |> parseHtml

  let vocabs = html |> CQ.select ".vocab"

  let vocab1text = vocabs |> CQ.first |> CQ.text
  test <@ vocab1text.Contains("Vocab 1") @>

  let checkboxes = html |> CQ.select "input[type='checkbox']" |> CQ.select ".term"
  test <@ checkboxes |> CQ.first |> CQ.attr "value" = "Uri1" @>
  test <@ checkboxes |> CQ.first |> CQ.attr "name" = "vocab" @>

  let labels = html |> CQ.select ".checkbox > label"
  test <@ labels |> CQ.first |> CQ.text = "Term1" @>

    
[<Test>]
let ``Should present the multiple vocabulary containing multiple terms`` () =
  let vocabs = [{Property = ""
                 Root = Term {t with Children = [Term t
                                                 Term t]}}]

  let html = Sidebar.render [] vocabs false |> parseHtml

  test <@ html |> CQ.select "input[type='checkbox']" |> CQ.select ".term" |> CQ.length = 2 @>

    
[<Test>]
let ``Should present the vocabulary term checkboxes unselected by default`` () =
  let vocabs = [{Property = ""
                 Root = Term {t with Children = [Term t]}}]

  let html = Sidebar.render [] vocabs false 

  test <@ html |> parseHtml |> CQ.select "input[checked]" |> CQ.length = 0 @>

    
[<Test>]
let ``Should present the vocabulary term checkboxes as selected when they exist in the querystring`` () =
  let vocabs = [{Property = "vocab"
                 Root = Term {t with Children = [Term {t with Uri = uri "http://testing.com/Uri1";ShortenedUri="Uri1";}
                                                 Term {t with Uri = uri "http://testing.com/Uri2";ShortenedUri="Uri2";}]}}]

  let qs = [("vocab", Some "Uri2")]
  let html = Sidebar.render qs vocabs false 

  test <@ html
          |> parseHtml
          |> CQ.select "input[checked]"
          |> CQ.length = 1 @>

  test <@ html
          |> parseHtml
          |> CQ.select "input[checked]"
          |> CQ.first
          |> CQ.attr "id" = "http://testing.com/Uri2" @>
    
[<Test>]
let ``Should have an apply filters button`` () =
  let searchButtonLabel =
    Sidebar.render [] [] false
    |> parseHtml
    |> CQ.select ":submit"
    |> CQ.attr "Value"
  test <@ searchButtonLabel = "Apply filters" @>

    
[<Test>]
let ``Should present the vocabulary collapsed by default`` () =
  let vocabs = [{Property = ""
                 Root = Term {t with Children = []}}]

  let html = Sidebar.render [] vocabs false |> parseHtml

  let accordians = html |> CQ.select ".accordion.closed"

  test <@ accordians |> CQ.length = 1 @>

    
[<Test>]
let ``Should present the vocabulary expanded if vocabulary term is in querystring filters`` () =
  let vocabs = [{Property = "vocab:1"
                 Root = Term {t with Children = [Term {t with Uri = uri "http://testing.com/Uri#Term"}]}}
                {Property = "vocab:2"
                 Root = Term t}]

  let qsWithOneFilter = [("vocab%3A1",Some "http://testing.com/Uri#Term")]

  let html = Sidebar.render qsWithOneFilter vocabs false |> parseHtml

  test <@ html |> CQ.select ".accordion.closed.open" |> CQ.length = 1 @>
