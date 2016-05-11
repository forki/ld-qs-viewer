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
    Sidebar.render [] [] false
    |> parseHtml
    |> CQ.select "form"
    |> CQ.attr "action"
  action |> should equal "/qs/search"

[<Test>]
let ``Should present a vocabulary with a single term as an input checkbox`` () =
  let vocabs = [{Property = "vocab"
                 Root = Term {t with Label = "Vocab 1"
                                     Children = [Term {t with Label = "Term1"; Uri = uri "http://testing.com/Uri1"; ShortenedUri="Uri1"}]};
                 Label = "Vocab 1"}]

  let html = Sidebar.render [] vocabs false |> parseHtml

  let vocabs = html |> CQ.select ".vocab"

  let vocab1text = vocabs |> CQ.first |> CQ.text
  vocab1text |> should contain "Vocab 1"

  let checkboxes = html |> CQ.select "input[type='checkbox']" |> CQ.select ".term"
  checkboxes |> CQ.first |> CQ.attr "value" |> should equal "Uri1"
  checkboxes |> CQ.first |> CQ.attr "name" |> should equal "vocab"

  let labels = html |> CQ.select ".checkbox > label"
  labels |> CQ.first |> CQ.text |> should equal "Term1"

    
[<Test>]
let ``Should present the multiple vocabulary containing multiple terms`` () =
  let vocabs = [{Property = ""
                 Root = Term {t with Children = [Term t
                                                 Term t]};
                 Label = ""}]

  let html = Sidebar.render [] vocabs false |> parseHtml

  html |> CQ.select "input[type='checkbox']" |> CQ.select ".term" |> CQ.length |> should equal 2

    
[<Test>]
let ``Should present the vocabulary term checkboxes unselected by default`` () =
  let vocabs = [{Property = ""
                 Root = Term {t with Children = [Term t]};
                 Label = ""}]

  let html = Sidebar.render [] vocabs false 

  html |> parseHtml |> CQ.select "input[checked]" |> CQ.length |> should equal 0

    
[<Test>]
let ``Should present the vocabulary term checkboxes as selected when they exist in the querystring`` () =
  let vocabs = [{Property = "vocab"
                 Root = Term {t with Children = [Term {t with Uri = uri "http://testing.com/Uri1";ShortenedUri="Uri1";}
                                                 Term {t with Uri = uri "http://testing.com/Uri2";ShortenedUri="Uri2";}]}
                 Label = ""}]

  let qs = [("vocab", Some "Uri2")]
  let html = Sidebar.render qs vocabs false 

  html
  |> parseHtml
  |> CQ.select "input[checked]"
  |> CQ.length
  |> should equal 1

  html
  |> parseHtml
  |> CQ.select "input[checked]"
  |> CQ.first
  |> CQ.attr "id"
  |> should equal "http://testing.com/Uri2"
    
[<Test>]
let ``Should have an apply filters button`` () =
  let searchButtonLabel =
    Sidebar.render [] [] false
    |> parseHtml
    |> CQ.select ":submit"
    |> CQ.attr "Value"
  searchButtonLabel |> should equal "Apply filters"

    
[<Test>]
let ``Should present the vocabulary collapsed by default`` () =
  let vocabs = [{Property = ""
                 Root = Term {t with Children = []}
                 Label = ""}]

  let html = Sidebar.render [] vocabs false |> parseHtml

  let accordians = html |> CQ.select ".accordion.closed"

  accordians |> CQ.length |> should equal 1

    
[<Test>]
let ``Should present the vocabulary expanded if vocabulary term is in querystring filters`` () =
  let vocabs = [{Property = "vocab:1"
                 Root = Term {t with Children = [Term {t with Uri = uri "http://testing.com/Uri#Term"}]}
                 Label = ""}
                {Property = "vocab:2"
                 Root = Term t
                 Label = ""}]

  let qsWithOneFilter = [("vocab%3A1",Some "http://testing.com/Uri#Term")]

  let html = Sidebar.render qsWithOneFilter vocabs false |> parseHtml

  html |> CQ.select ".accordion.closed.open" |> CQ.length |> should equal 1

[<Test>]
let ``Should render the sidebar with vocabulary property embedded in dom`` () =
  let vocabs = [{Property = "vocab:property"
                 Root = Term t
                 Label = ""}]

  Sidebar.render [] vocabs false
  |> parseHtml
  |> CQ.select ".vocab"
  |> CQ.attr "id"
  |> should equal "vocab:property"
