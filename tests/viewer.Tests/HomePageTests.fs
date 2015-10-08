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

  test <@ title = "BETA Quality Statements Discovery Tool | NICE" @>

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
  let GetVocabs () = [{Root = Term {Uri = (Uri.from "http://testing.com/Vocab1")
                                    Label = "Vocab 1";
                                    Children = [
                                                 Term { Uri = Uri.from "http://testing.com/Uri1";
                                                        Label = "Term1";
                                                        Children = []}]};
                       Property = "v1"};
                      {Root = Term {Uri = (Uri.from "http://testing.com/Vocab2")
                                    Label = "Vocab 2";
                                    Children = [
                                                 Term { Uri = Uri.from "http://testing.com/Uri2";
                                                        Label = "Term2";
                                                        Children = []}]};
                       Property = "v2"}]
  let GetSearchResults _ _ = []

  let html = startServerWithData GetVocabs GetSearchResults |> get "/"

  let vocabs = html |> CQ.select ".vocab"

  let vocab1text = vocabs |> CQ.first |> CQ.text
  test <@ vocab1text.Contains("Vocab 1") @>

  let vocab2text = vocabs |> CQ.last |> CQ.text
  test <@ vocab2text.Contains("Vocab 2") @>

  let termCount = html |> CQ.select "input[type='checkbox']" |> CQ.length
  test <@ termCount = 2 @>

[<Test>]
let ``Should have search button`` () =
  let searchButtonLabel =
    startServer ()
    |> get "/"
    |> CQ.select ":submit"
    |> CQ.attr "Value"
  test <@ searchButtonLabel = "Search" @>
