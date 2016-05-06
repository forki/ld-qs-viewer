module Viewer.Tests.Components.AnnotationSidebar.Tests

open Suave
open NUnit.Framework
open Swensen.Unquote
open Viewer.Types
open Viewer.Data.Vocabs.VocabGeneration
open Viewer.Tests.Utils
open Viewer.SuaveExtensions
open FSharp.RDF

[<Test>]
let ``Should present the vocabulary term checkboxes unselected by default`` () =
  let vocabs = [{Property = ""
                 Root = Term {t with Children = [Term t]}
                 Label = ""}]

  let html = startServerWith {baseConfig with Vocabs = vocabs} |> get "/annotationtool" 
  test <@ html |> CQ.select "input[checked]" |> CQ.length = 0 @>

    
[<Test>]
let ``Should present the vocabulary term checkboxes as selected when they exist in the querystring`` () =
  let vocabs = [{Property = "vocab"
                 Root = Term {t with Children = [Term {t with Uri = uri "http://testing.com/Uri1"; ShortenedUri="Uri1";}
                                                 Term {t with Uri = uri "http://testing.com/Uri2"; ShortenedUri="Uri2";}]}
                 Label = ""}]

  let html = startServerWith {baseConfig with Vocabs = vocabs}
             |> getQuery "/annotationtool/toyaml" "vocab=Uri2"

  let selectedCheckboxes = html |> CQ.select "input[checked]"

  test <@ selectedCheckboxes |> CQ.length = 1 @>
  test <@ selectedCheckboxes |> CQ.first |> CQ.attr "value" = "Uri2" @>

    
[<Test>]
let ``Should present the vocabulary collapsed by default`` () =
  let vocabs = [{Property = ""
                 Root = Term {t with Children = []}
                 Label = ""}]

  let html = startServerWith {baseConfig with Vocabs = vocabs} |> get "/annotationtool"

  let accordians = html |> CQ.select ".accordion.closed"

  test <@ accordians |> CQ.length = 1 @>

    
[<Test>]
let ``Should present the vocabulary expanded if vocabulary term is in querystring filters`` () =
  let vocabs = [{Property = "vocab:1"
                 Root = Term {t with Children = [Term {t with Uri = uri "http://testing.com/Uri#Term"}]}
                 Label = ""}
                {Property = "vocab:2"
                 Root = Term t
                 Label = ""}]

  let qsWithOneFilter = "vocab%3A1=http%3A%2F%2Ftesting.com%2FUri%23Term"

  let html = startServerWith {baseConfig with Vocabs = vocabs}
             |> getQuery "/annotationtool/toyaml" qsWithOneFilter

  test <@ html |> CQ.select ".accordion.closed.open" |> CQ.length = 1 @>
