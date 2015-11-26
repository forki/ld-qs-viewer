module Viewer.Tests.SearchResultsTests

open Suave
open Suave.DotLiquid
open NUnit.Framework
open Swensen.Unquote
open Viewer.Types
open Viewer.Tests.Utils

[<SetUp>]
let ``Run before tests`` () =
  setTemplatesDir "."

[<Test>]
let ``Should show message when attempting to search with no filters`` () =
 let message =
   startServerWith baseConfig
   |> get "/qs/search"
   |> CQ.select ".message"
   |> CQ.text 

 test <@ message = "Please select one or more filters." @>

[<Test>]
let ``Should present search results`` () =
  let getSearchResults _ _ = [{Uri = "";Abstract = ""; Title = ""};
                              {Uri = "";Abstract = ""; Title = ""}]
  let getKBCount _ = 0
  let results =
    startServerWith {baseConfig with GetSearchResults=getSearchResults}
    |> getQuery "/qs/search" "notused=notused"
    |> CQ.select ".results > .result"
    |> CQ.length

  test <@ results = 2 @>


[<Test>]
let ``Should present a result count`` () =
  let getSearchResults _ _ = [{Uri = "";Abstract = ""; Title = ""};
                              {Uri = "";Abstract = ""; Title = ""}]

  let totalCount =
    startServerWith {baseConfig with GetSearchResults=getSearchResults}
    |> getQuery "/qs/search" "notused=notused"
    |> CQ.select ".card-list-header > .counter"
    |> CQ.text
  test <@ totalCount = "2 filtered items" @>


[<Test>]
let ``Should present a total KB Quality statement count`` () =
  let getKBCount _ = 3

  let totalCount =
    startServerWith {baseConfig with GetKBCount=getKBCount}
    |> get "/qs"
    |> CQ.select ".counter"
    |> CQ.text
  test <@ totalCount = "Total number of NICE Quality statements: 3" @>


[<Test>]
let ``Should present abstract and link for each result`` () =
  let getSearchResults _ _ = [{Uri = "Uri1"; Abstract = "Abstract1"; Title = "Title1"};
                              {Uri = "Uri2"; Abstract = "Abstract2"; Title = "Title2"}]

  let dom =
    startServerWith {baseConfig with GetSearchResults=getSearchResults}
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
  let qsWithTwoActiveFilters = "key=http%3A%2F%2Ftesting.com%2FUri%23Term1&key=http%3A%2F%2Ftesting.com%2FUri%23Term2"

  let html = startServerWith baseConfig |> getQuery "/qs/search" qsWithTwoActiveFilters

  let tags = html |> CQ.select ".tag-label"

  test <@ tags |> CQ.length = 2 @>
  test <@ tags |> CQ.first |> CQ.text = "Term1" @>
  test <@ tags |> CQ.last |> CQ.text = "Term2" @>

[<Test>]
let ``Should show active filters as tags with removal links`` () =
  let qsWithTwoActiveFilters = "key=http%3A%2F%2Ftesting.com%2FUri%23Term1&key=http%3A%2F%2Ftesting.com%2FUri%23Term2"

  let html = startServerWith baseConfig |> getQuery "/qs/search" qsWithTwoActiveFilters

  let tags = html |> CQ.select ".tag-remove-link"

  test <@ tags |> CQ.length = 2 @>
  test <@ tags |> CQ.first |> CQ.attr "href" = "/qs/search?key=http%3A%2F%2Ftesting.com%2FUri%23Term2" @>
  test <@ tags |> CQ.last |> CQ.attr "href" = "/qs/search?key=http%3A%2F%2Ftesting.com%2FUri%23Term1" @>
