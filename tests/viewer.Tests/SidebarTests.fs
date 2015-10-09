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
    |> get "/qs"
    |> CQ.select "title"
    |> CQ.text

  test <@ title = "BETA Quality Statements Discovery Tool | NICE" @>

[<Test>]
let ``Should add form with search action`` () =
  let action =
    startServer ()
    |> get "/qs"
    |> CQ.select "form"
    |> CQ.attr "action"
  test <@ action = "/qs/search" @>

[<Test>]
let ``Should present a vocabulary with a single term as an input checkbox`` () =
  let GetVocabs () = [{Root = Term {Uri = (Uri.from "http://testing.com/Vocab1")
                                    Label = "Vocab 1"
                                    Selected = false
                                    Children = [
                                                 Term { Uri = Uri.from "http://testing.com/Uri1"
                                                        Label = "Term1"
                                                        Selected = false
                                                        Children = []}]};
                       Property = "property"}]
  let GetSearchResults _ _ = []

  let html = startServerWithData GetVocabs GetSearchResults |> get "/qs"

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
  let GetVocabs () = [{Root = Term {Uri = (Uri.from "http://testing.com/Vocab1")
                                    Label = "Vocab 1"
                                    Selected = false
                                    Children = [
                                                 Term { Uri = Uri.from "http://testing.com/Uri1"
                                                        Label = "Term1"
                                                        Selected = false
                                                        Children = []}]};
                       Property = "v1"};
                      {Root = Term {Uri = (Uri.from "http://testing.com/Vocab2")
                                    Label = "Vocab 2"
                                    Selected = false
                                    Children = [
                                                 Term { Uri = Uri.from "http://testing.com/Uri2"
                                                        Label = "Term2"
                                                        Selected = false
                                                        Children = []};
                                                 Term { Uri = Uri.from "http://testing.com/Uri3"
                                                        Label = "Term3"
                                                        Selected = false
                                                        Children = []}]};
                       Property = "v2"}]
  let GetSearchResults _ _ = []

  let html = startServerWithData GetVocabs GetSearchResults |> get "/qs"

  let checkboxes = html |> CQ.select "input[type='checkbox']"
  test <@ checkboxes |> CQ.length = 3 @>

[<Test>]
let ``Should present the vocabulary term checkboxes unselected by default`` () =
  let GetVocabs () = [{Root = Term {Uri = (Uri.from "http://testing.com/Vocab1")
                                    Label = "Vocab 1"
                                    Selected = false
                                    Children = [
                                                 Term { Uri = Uri.from "http://testing.com/Uri1"
                                                        Label = "Term1"
                                                        Selected = false
                                                        Children = []}]};
                       Property = "v1"};]
  let GetSearchResults _ _ = []

  let html = startServerWithData GetVocabs GetSearchResults |> get "/qs"

  let selectedCheckboxes = html |> CQ.select "input[checked]"
  test <@ selectedCheckboxes |> CQ.length = 0 @>

[<Test>]
let ``Should present the vocabulary term checkboxes as selected when they exist in the querystring`` () =
  let GetVocabs () = [{Root = Term {Uri = (Uri.from "http://testing.com/Vocab1")
                                    Label = "Vocab 1"
                                    Selected = false
                                    Children = [
                                                 Term { Uri = Uri.from "http://testing.com/Uri1"
                                                        Label = "Term1"
                                                        Selected = false
                                                        Children = []};
                                                 Term { Uri = Uri.from "http://testing.com/Uri2"
                                                        Label = "Term2"
                                                        Selected = true
                                                        Children = []}]};
                       Property = "v1"}]
  let GetSearchResults _ _ = []

  let html = startServerWithData GetVocabs GetSearchResults |> getQuery "/qs/search/" "key=http%3A%2F%2Ftesting.com%2FUri1"

  let selectedCheckboxes = html |> CQ.select "input[checked]"

  test <@ selectedCheckboxes |> CQ.length = 1 @>
  test <@ selectedCheckboxes |> CQ.first |> CQ.attr "value" = "http://testing.com/Uri2" @>

[<Test>]
let ``Should have search button`` () =
  let searchButtonLabel =
    startServer ()
    |> get "/qs"
    |> CQ.select ":submit"
    |> CQ.attr "Value"
  test <@ searchButtonLabel = "Search" @>
