module Viewer.Tests.HomePageTests

open Suave
open Suave.DotLiquid
open NUnit.Framework
open Swensen.Unquote
open Viewer.Types
open Viewer.Tests.Utils

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
let ``Should add form with search action`` () =
  let action =
    startServer ()
    |> get "/"
    |> CQ.select "form"
    |> CQ.attr "action"
  test <@ action = "/search" @>

[<Test>]
let ``Should present the vocabulary terms in form`` () =
  let vocabularies = [{Name = "Vocab 1";
                       Terms = [{Name = "Term1"; Uri = "Uri1"};
                                {Name = "Term2"; Uri = "Uri2"}]};
                      {Name = "Vocab 2";
                       Terms = [{Name = "Term3"; Uri = "Uri3"}]}]
  let GetSearchResults _ = []

  let html = startServerWithData vocabularies GetSearchResults |> get "/"

  let vocabs = html |> CQ.select ".filter-group > .vocab"

  let vocab1text = vocabs |> CQ.first |> CQ.text
  test <@ vocab1text.Contains("Vocab 1") @>

  let vocab2text = vocabs |> CQ.last |> CQ.text
  test <@ vocab2text.Contains("Vocab 2") @>

  let termCount = html |> CQ.select "input[type='checkbox']" |> CQ.length
  test <@ termCount = 3 @>

[<Test>]
let ``Should have search button`` () =
  let searchButtonLabel =
    startServer ()
    |> get "/"
    |> CQ.select ":submit"
    |> CQ.attr "Value"
  test <@ searchButtonLabel = "Search" @>
