module Viewer.Tests.Components.SearchResults.Tests

open Fuchu
open Swensen.Unquote
open Viewer.Types
open Viewer.Tests.Utils
open Viewer.Components.SearchResults
open Viewer.SuaveExtensions

let private defaultArgs = {
  Qs = []
  GetSearchResults = (fun _ _ -> [])
  GetKBCount = (fun _ -> 0)
  ShowOverview = false
  Testing = false
}

[<Tests>]
let tests =
  setTemplatesDir "src/viewer/bin/Release/"

  testList "Search results component" [

    testCase "Should show message when attempting to search with no filters" <| fun _ ->
      let message =
        render defaultArgs
        |> parseHtml
        |> CQ.select ".message"
        |> CQ.text

      test <@ message = "Please select one or more filters." @>

    testCase "Should present search results" <| fun _ ->
      let getSearchResults _ _ = [{Uri = "";Abstract = ""; Title = ""; Annotations = []};
                                  {Uri = "";Abstract = ""; Title = ""; Annotations = []}]
      let getKBCount _ = 0
      let results =
        render {defaultArgs with GetSearchResults=getSearchResults; GetKBCount=getKBCount}
        |> parseHtml
        |> CQ.select ".results > .result"
        |> CQ.length

      test <@ results = 2 @>

    testCase "Should present a result count" <| fun _ ->
      let getSearchResults _ _ = [{Uri = "";Abstract = ""; Title = ""; Annotations = []};
                                  {Uri = "";Abstract = ""; Title = ""; Annotations = []}]

      let totalCount =
        render {defaultArgs with GetSearchResults=getSearchResults; Qs = [("notused", Some "notused")]}
        |> parseHtml
        |> CQ.select ".card-list-header > .counter"
        |> CQ.text
      test <@ totalCount = "2 filtered items" @>

    testCase "Should present a total KB Quality statement count" <| fun _ ->
      let getKBCount _ = 3

      let totalCount =
        render {defaultArgs with GetKBCount=getKBCount; ShowOverview=true; Qs=[("", Some "")]}
        |> parseHtml
        |> CQ.select ".counter"
        |> CQ.text
      test <@ totalCount = "Total number of NICE Quality statements: 3" @>

    testCase "Should present abstract and link for each result" <| fun _ ->
      let getSearchResults _ _ = [{Uri = "Uri1"; Abstract = "Abstract1"; Title = "Title1"; Annotations = []}]

      let html =
        render {defaultArgs with GetSearchResults=getSearchResults; Qs = [("notused", Some "notused")]}
        |> parseHtml

      let abstracts = html |> CQ.select ".abstract"
      let abstract1 = abstracts |> CQ.first |> CQ.text

      let links = html |> CQ.select ".result > a"
      let link1 = links |> CQ.first |> CQ.attr "href"

      test <@ abstract1 = "Abstract1" @>
      test <@ link1 = "Uri1" @>

    testCase "Should show multiple active filters when they exist on qs" <| fun _ ->
      let qsWithTwoActiveFilters = [("key", Some "http://testing.com/Uri#Term1")
                                    ("key", Some "http://testing.com/Uri#Term2")]
      let tags =
        render {defaultArgs with Qs=qsWithTwoActiveFilters}
        |> parseHtml
        |> CQ.select ".tag-label"

      test <@ tags |> CQ.length = 2 @>
    
    testCase "Should show active filter as tag with label" <| fun _ ->
      let qs = [("key", Some "http://testing.com/Uri#Term1")]

      let tags =
        render {defaultArgs with Qs=qs}
        |> parseHtml
        |> CQ.select ".tag-label"

      test <@ tags |> CQ.first |> CQ.text = "Term1" @>

    testCase "Should show active filter as tag with removal link" <| fun _ ->
      let qsWithTwoActiveFilters = [("key", Some "http://testing.com/Uri#Term1")
                                    ("key", Some "http://testing.com/Uri#Term2")]

      let tags =
        render {defaultArgs with Qs=qsWithTwoActiveFilters}
        |> parseHtml
        |> CQ.select ".tag-remove-link"

      test <@ tags |> CQ.first |> CQ.attr "href" = "/qs/search?key=http%3A%2F%2Ftesting.com%2FUri%23Term2" @>
      test <@ tags |> CQ.last |> CQ.attr "href" = "/qs/search?key=http%3A%2F%2Ftesting.com%2FUri%23Term1" @>

    testCase "Should show annotations for each search result" <| fun _ ->
      let getSearchResults _ _ = [{Uri = "";Abstract = ""; Title = ""; Annotations = ["Annotation1"]};
                                  {Uri = "";Abstract = ""; Title = ""; Annotations = ["Annotation2"]}]
      let getKBCount _ = 0
      let result =
        render {defaultArgs with GetSearchResults=getSearchResults; GetKBCount=getKBCount}
        |> parseHtml
        |> CQ.select ".annotations"
        |> CQ.first
        |> CQ.text
    
      test <@ result = "Annotation1" @>
  ]
