module Viewer.Tests.HomePageTests

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
    startServer ()
    |> get "/"
    |> CQ.select "title"
    |> CQ.text

  test <@ title = "KB - Home" @>

[<Test>]
let ``Should show heading`` () =
  let header =
    startServer ()
    |> get "/"
    |> CQ.select "main > h1"
    |> CQ.text
  test <@ header = "NICE Quality Standards" @>

[<Test>]
let ``Should add form with search action`` () =
  let action =
    startServer ()
    |> get "/"
    |> CQ.select "form"
    |> CQ.attr "action"
  test <@ action = "/search" @>

[<Test>]
let ``Should present the vocabulary terms in form`` () =
  let GetVocabs () = [Term {Uri=(Uri.from "http://goog.com/1" );Label="Vocab 1";Children=[]}
                      Term {Uri=(Uri.from "http://goog.com/2" );Label="Vocab 2";Children=[]}]
  let GetSearchResults _ = []

  let html = startServerWithData GetVocabs GetSearchResults |> get "/"

  let vocabs = html |> CQ.select "form > .vocab"

  let vocab1text = vocabs |> CQ.first |> CQ.text
  test <@ vocab1text.StartsWith("Vocab 1") @>

  let vocab2text = vocabs |> CQ.last |> CQ.text
  test <@ vocab2text.StartsWith("Vocab 2") @>

  let termCount = html |> CQ.select "form > .vocab > input" |> CQ.length
  test <@ termCount = 3 @>

[<Test>]
let ``Should have search button`` () =
  let searchButtonLabel =
    startServer ()
    |> get "/"
    |> CQ.select ":submit"
    |> CQ.attr "Value"
  test <@ searchButtonLabel = "Search" @>
