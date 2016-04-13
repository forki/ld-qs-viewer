module Viewer.Tests.Components.SearchResults.Tests

open NUnit.Framework
open Swensen.Unquote
open Viewer.Types
open Viewer.Tests.Utils
open Viewer.Components
open Viewer.Components.SearchResults
open Viewer.SuaveExtensions

[<SetUp>]
let ``Run before tests`` () =
  setTemplatesDir "../../../../src/viewer/bin/Release/"

let private defaultArgs = {
  Qs = []
  GetSearchResults = (fun _ _ -> [])
  GetKBCount = (fun _ -> 0)
  ShowOverview = false
  Testing = false
}

[<Test>]
let ``Should show message when attempting to search with no filters`` () =
  let message =
    SearchResults.render defaultArgs
    |> parseHtml
    |> CQ.select ".message"
    |> CQ.text 

  test <@ message = "Please select one or more filters." @>

    
[<Test>]
let ``Should present search results`` () =
  let getSearchResults _ _ = [{Uri = "";Abstract = ""; Title = ""};
                              {Uri = "";Abstract = ""; Title = ""}]
  let getKBCount _ = 0
  let results =
    SearchResults.render {defaultArgs with GetSearchResults=getSearchResults; GetKBCount=getKBCount}
    |> parseHtml
    |> CQ.select ".results > .result"
    |> CQ.length

  test <@ results = 2 @>

    
[<Test>]
let ``Should present a result count`` () =
  let getSearchResults _ _ = [{Uri = "";Abstract = ""; Title = ""};
                              {Uri = "";Abstract = ""; Title = ""}]

  let totalCount =
    SearchResults.render {defaultArgs with GetSearchResults=getSearchResults; Qs = [("notused", Some "notused")]}
    |> parseHtml
    |> CQ.select ".card-list-header > .counter"
    |> CQ.text
  test <@ totalCount = "2 filtered items" @>

    
[<Test>]
let ``Should present a total KB Quality statement count`` () =
  let getKBCount _ = 3

  let totalCount =
    SearchResults.render {defaultArgs with GetKBCount=getKBCount; ShowOverview=true; Qs=[("", Some "")]}
    |> parseHtml
    |> CQ.select ".counter"
    |> CQ.text
  test <@ totalCount = "Total number of NICE Quality statements: 3" @>

    
[<Test>]
let ``Should present abstract and link for each result`` () =
  let getSearchResults _ _ = [{Uri = "Uri1"; Abstract = "Abstract1"; Title = "Title1"}]

  let html =
    SearchResults.render {defaultArgs with GetSearchResults=getSearchResults; Qs = [("notused", Some "notused")]}
    |> parseHtml

  let abstracts = html |> CQ.select ".abstract"
  let abstract1 = abstracts |> CQ.first |> CQ.text

  let links = html |> CQ.select ".result > a"
  let link1 = links |> CQ.first |> CQ.attr "href"

  test <@ abstract1 = "Abstract1" @>
  test <@ link1 = "Uri1" @>

    
[<Test>]
let ``Should show multiple active filters when they exist on qs`` () =
  let qsWithTwoActiveFilters = [("key", Some "http://testing.com/Uri#Term1")
                                ("key", Some "http://testing.com/Uri#Term2")]
  let tags =
    SearchResults.render {defaultArgs with Qs=qsWithTwoActiveFilters}
    |> parseHtml
    |> CQ.select ".tag-label"

  test <@ tags |> CQ.length = 2 @>
    
    
[<Test>]
let ``Should show active filter as tag with label`` () =
  let qs = [("key", Some "http://testing.com/Uri#Term1")]

  let tags =
    SearchResults.render {defaultArgs with Qs=qs}
    |> parseHtml
    |> CQ.select ".tag-label"

  test <@ tags |> CQ.first |> CQ.text = "Term1" @>

    
[<Test>]
let ``Should show active filter as tag with removal link`` () =
  let qsWithTwoActiveFilters = [("key", Some "http://testing.com/Uri#Term1")
                                ("key", Some "http://testing.com/Uri#Term2")]

  let tags =
    SearchResults.render {defaultArgs with Qs=qsWithTwoActiveFilters}
    |> parseHtml
    |> CQ.select ".tag-remove-link"

  test <@ tags |> CQ.first |> CQ.attr "href" = "/qs/search?key=http%3A%2F%2Ftesting.com%2FUri%23Term2" @>
  test <@ tags |> CQ.last |> CQ.attr "href" = "/qs/search?key=http%3A%2F%2Ftesting.com%2FUri%23Term1" @>
