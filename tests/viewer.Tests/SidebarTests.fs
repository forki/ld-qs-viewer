module Viewer.Tests.SidebarTests

open Suave
open Suave.DotLiquid
open NUnit.Framework
open Swensen.Unquote
open Viewer.Types
open Viewer.VocabGeneration
open Viewer.Tests.Utils
open FSharp.RDF

let NoSearchResults _ _ = []
let NoDocuments _ = 0
let vcBuilder = new VocabularyBuilder()

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
  let child = vcBuilder.createChild ("http://testing.com/Uri1", "Term1", false, [])
  let vocabs = vcBuilder.createRoot ("http://testing.com/Vocab1", "Vocab 1", false, [child], "property")

  let html = startServerWith {baseConfig with Vocabs = vocabs} |> get "/qs"

  let vocabs = html |> CQ.select ".vocab"

  let vocab1text = vocabs |> CQ.first |> CQ.text
  test <@ vocab1text.Contains("Vocab 1") @>

  let checkboxes = html |> CQ.select "input[type='checkbox']"
  test <@ checkboxes |> CQ.length = 1 @>
  test <@ checkboxes |> CQ.first |> CQ.attr "value" = "http://testing.com/Uri1" @>
  test <@ checkboxes |> CQ.first |> CQ.attr "name" = "property" @>

  let labels = html |> CQ.select ".checkbox > label"
  test <@ labels |> CQ.length = 1 @>
  test <@ labels |> CQ.first |> CQ.text = "Term1" @>

[<Test>]
let ``Should present the multiple vocabulary containing multiple terms`` () =
  let child1 = vcBuilder.createChild ("http://testing.com/Uri1", "Term1", false, [])
  let child2 = vcBuilder.createChild ("http://testing.com/Uri2", "Term2", false, [])
  let child3 = vcBuilder.createChild ("http://testing.com/Uri3", "Term3", false, [])
  let vocabs = vcBuilder.createRoot ("http://testing.com/Vocab1", "Vocab 1", false, [child1], "v1") @ vcBuilder.createRoot("http://testing.com/Vocab2", "Vocab 2", false, [child2; child3], "v2")

  let html = startServerWith {baseConfig with Vocabs = vocabs} |> get "/qs"

  let checkboxes = html |> CQ.select "input[type='checkbox']"
  test <@ checkboxes |> CQ.length = 3 @>

[<Test>]
let ``Should present the vocabulary term checkboxes unselected by default`` () =
  let child1 = vcBuilder.createChild ("http://testing.com/Uri1", "Term1", false, [])
  let vocabs = vcBuilder.createRoot ("http://testing.com/Vocab1", "Vocab 1", false, [child1], "v1") 
  let html = startServerWith {baseConfig with Vocabs = vocabs} |> get "/qs"

  let selectedCheckboxes = html |> CQ.select "input[checked]"
  test <@ selectedCheckboxes |> CQ.length = 0 @>

[<Test>]
let ``Should present the vocabulary term checkboxes as selected when they exist in the querystring`` () =
  let child1 = vcBuilder.createChild ("http://testing.com/Uri1", "Term1", false, [])
  let child2 = vcBuilder.createChild ("http://testing.com/Uri2", "Term2", true, [])
  let vocabs = vcBuilder.createRoot ("http://testing.com/Vocab1", "Vocab 1", false, [child1;child2], "v1")

  let html = startServerWith {baseConfig with Vocabs = vocabs}
             |> getQuery "/qs/search" "key=http%3A%2F%2Ftesting.com%2FUri2"

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
  let child1 = vcBuilder.createChild ("http://testing.com/Uri2", "Term1", false, [])
  let vocabs = vcBuilder.createRoot ("http://testing.com/Vocab1", "Vocab 1", false, [child1], "v1")

  let html = startServerWith {baseConfig with Vocabs = vocabs} |> get "/qs"

  let accordians = html |> CQ.select ".accordion.closed"

  test <@ accordians |> CQ.length = 1 @>

[<Test>]
let ``Should present the vocabulary expanded if vocabulary term is in querystring filters`` () =
  let child1 = vcBuilder.createChild ("http://testing.com/Uri#Term", "Term1", false, [])
  let vocabs = vcBuilder.createRoot ("http://testing.com/Vocab1", "Vocab 1", false, [child1], "some:vocab") @ vcBuilder.createRoot ("http://testing.com/Vocab1", "Vocab 2", false, [], "anotherVocab")

  let qsWithOneFilter = "some%3Avocab=http%3A%2F%2Ftesting.com%2FUri%23Term"

  let html = startServerWith {baseConfig with Vocabs = vocabs}
             |> getQuery "/qs/search" qsWithOneFilter

  test <@ html |> CQ.select ".accordion-trigger.open" |> CQ.length = 1 @>
  test <@ html |> CQ.select ".accordion.closed.open" |> CQ.length = 1 @>
