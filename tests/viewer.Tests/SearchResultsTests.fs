module Viewer.Tests.SearchResultsTests

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
let ``Should show message when attempting to search with no filters`` () =
 let message =
   startServer ()
   |> get "/qs/search"
   |> CQ.select ".message"
   |> CQ.text 

 test <@ message = "Please select one or more filters." @>

[<Test>]
let ``Should present search results`` () =
  let GetSearchResults _ _ = [{Uri = "";Abstract = ""; Title = ""};
                              {Uri = "";Abstract = ""; Title = ""}]
  let vocabs = []
  let KBCount = 0
  let results =
    startServerWithData vocabs GetSearchResults KBCount 
    |> getQuery "/qs/search" "notused=notused"
    |> CQ.select ".results > .result"
    |> CQ.length

  test <@ results = 2 @>


[<Test>]
let ``Should present a result count`` () =
  let GetSearchResults _ _ = [{Uri = "";Abstract = ""; Title = ""};
                              {Uri = "";Abstract = ""; Title = ""}]
  let vocabs = []
  let KBCount = 0

  let totalCount =
    startServerWithData vocabs GetSearchResults KBCount
    |> getQuery "/qs/search" "notused=notused"
    |> CQ.select ".card-list-header > .counter"
    |> CQ.text
  test <@ totalCount = "2 filtered items" @>

[<Test>]
let ``Should present abstract and link for each result`` () =
  let GetSearchResults _ _ = [{Uri = "Uri1"; Abstract = "Abstract1"; Title = "Title1"};
                              {Uri = "Uri2"; Abstract = "Abstract2"; Title = "Title2"}]
  let vocabs = []
  let KBCount = 0

  let dom =
    startServerWithData vocabs GetSearchResults KBCount
    |> getQuery "/qs/search" "notused=notused"

  let abstracts = dom |> CQ.select ".abstract"

  let abstract1 = abstracts |> CQ.first |> CQ.text
  let abstract2 = abstracts |> CQ.last |> CQ.text

  test <@ abstract1 = "Abstract1" @>
  test <@ abstract2 = "Abstract2" @>

  let links = dom |> CQ.select ".result > a"
  let link1 = links |> CQ.first |> CQ.attr "href"
  let link2 = links |> CQ.last |> CQ.attr "href"

  test <@ link1 = "Uri1" @>
  test <@ link2 = "Uri2" @>

[<Test>]
let ``Should show active filters as tags with labels`` () =
  let vocabs = []
  let GetSearchResults _ _ = []
  let KBCount = 0
  let qsWithTwoActiveFilters = "key=http%3A%2F%2Ftesting.com%2FUri%23Term1&key=http%3A%2F%2Ftesting.com%2FUri%23Term2"

  let html = startServerWithData vocabs GetSearchResults KBCount |> getQuery "/qs/search" qsWithTwoActiveFilters

  let tags = html |> CQ.select ".tag-label"

  test <@ tags |> CQ.length = 2 @>
  test <@ tags |> CQ.first |> CQ.text = "Term1" @>
  test <@ tags |> CQ.last |> CQ.text = "Term2" @>

[<Test>]
let ``Should show active filters as tags with removal links`` () =
  let vocabs = []
  let GetSearchResults _ _ = []
  let KBCount = 0
  let qsWithTwoActiveFilters = "key=http%3A%2F%2Ftesting.com%2FUri%23Term1&key=http%3A%2F%2Ftesting.com%2FUri%23Term2"

  let html = startServerWithData vocabs GetSearchResults KBCount |> getQuery "/qs/search" qsWithTwoActiveFilters

  let tags = html |> CQ.select ".tag-remove-link"

  test <@ tags |> CQ.length = 2 @>
  test <@ tags |> CQ.first |> CQ.attr "href" = "/qs/search?key=http%3A%2F%2Ftesting.com%2FUri%23Term2" @>
  test <@ tags |> CQ.last |> CQ.attr "href" = "/qs/search?key=http%3A%2F%2Ftesting.com%2FUri%23Term1" @>
