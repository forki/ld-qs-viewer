module Viewer.Tests.Components.SearchResults.Tests

open NUnit.Framework
open FsUnit
open Viewer.Types
open Viewer.Tests.Utils
open Viewer.Components
open Viewer.Components.SearchResults
open Viewer.SuaveExtensions
open FSharp.RDF

[<SetUp>]
let ``Run before tests`` () =
  setTemplatesDir "../../../../bin/viewer/web/qs/"

let vocabs = [{Root = Term {
                            Uri = Uri.from "http://testing.com/Uri3"
                            ShortenedUri = "unknown"
                            Label = "Care home"
                            Selected = false
                            Children = [
                                         Term { 
                                                Uri = Uri.from "http://testing.com/Uri3"
                                                ShortenedUri = "vocabLabel/long-guid-1"
                                                Label = "Term1"
                                                Selected = false
                                                Children = []};
                                         Term { 
                                                Uri = Uri.from "http://testing.com/Uri3"
                                                ShortenedUri = "vocabLabel/long-guid-2"
                                                Label = "Term2"
                                                Selected = false
                                                Children = []};
                                         Term { 
                                                Uri = Uri.from "http://testing.com/Uri3"
                                                ShortenedUri = "vocabLabel/long-guid-3"
                                                Label = "Term3"
                                                Selected = false
                                                Children = []}]};
               Property = "v1";
               Label = ""}]
    


[<Test>]
let ``Should show message when attempting to search with no filters`` () =
  let message =
    SearchResults.render SearchResultsParameters.Empty
    |> parseHtml
    |> CQ.select ".message"
    |> CQ.text 

  message |> should equal "Please select one or more filters."

    
[<Test>]
let ``Should present search results`` () =
  let performSearch _ = [{Uri = "";Abstract = ""; Title = ""; FirstIssued = new System.DateTime()};
                         {Uri = "";Abstract = ""; Title = ""; FirstIssued = new System.DateTime()}]
  let getKBCount _ = 0
  let results =
    SearchResults.render {SearchResultsParameters.Empty with Vocabs=vocabs; PerformSearch=performSearch; GetKBCount=getKBCount}
    |> parseHtml
    |> CQ.select ".results > .result"
    |> CQ.length

  results |> should equal 2

    
[<Test>]
let ``Should present a result count`` () =
  let performSearch _ = [{Uri = "";Abstract = ""; Title = ""; FirstIssued = new System.DateTime()};
                         {Uri = "";Abstract = ""; Title = ""; FirstIssued = new System.DateTime()}]

  let totalCount =
    SearchResults.render {SearchResultsParameters.Empty with Vocabs=vocabs; PerformSearch=performSearch; Qs = [("notused", Some "notused")]}
    |> parseHtml
    |> CQ.select ".card-list-header > .counter"
    |> CQ.text
  totalCount |> should equal "2 filtered items"

    
[<Test>]
let ``Should present a total KB Quality statement count`` () =
  let getKBCount _ = 3

  let totalCount =
    SearchResults.render {SearchResultsParameters.Empty with GetKBCount=getKBCount; ShowOverview=true; Qs=[("", Some "")]}
    |> parseHtml
    |> CQ.select ".counter"
    |> CQ.text
  totalCount |> should equal "Total number of NICE quality statements: 3"

    
[<Test>]
let ``Should render search results with correct components`` () =
  let performSearch _ = [{Uri = "Uri1"; Abstract = "Abstract1"; Title = "Title1"; FirstIssued = new System.DateTime()}]

  let html =
    SearchResults.render {SearchResultsParameters.Empty with Vocabs=vocabs; PerformSearch=performSearch; Qs = [("notused", Some "notused")]}
    |> parseHtml

  let abstracts = html |> CQ.select ".abstract"
  let abstract1 = abstracts |> CQ.first |> CQ.text

  let links = html |> CQ.select ".result > a"
  let link1 = links |> CQ.first |> CQ.attr "href"

  let title = html |> CQ.select ".card-source span" |>  CQ.first |> CQ.text

  let firstIssued = html |> CQ.select ".card-right-align" |> CQ.first |> CQ.text

  abstract1 |> should equal "Abstract1"
  link1 |> should equal "Uri1"
  title |> should equal "Title1"
  firstIssued |> should equal "First issued date: January 0001"

    
[<Test>]
let ``Should show multiple active filters when they exist on qs`` () =
  let qsWithTwoActiveFilters = [("key", Some "vocabLabel/long-guid-1")
                                ("key", Some "vocabLabel/long-guid-2")]
  let tags =
    SearchResults.render {SearchResultsParameters.Empty with Vocabs=vocabs; Qs=qsWithTwoActiveFilters}
    |> parseHtml
    |> CQ.select ".tag-label"

  tags |> CQ.length |> should equal 2
    
    
[<Test>]
let ``Should show active filter as tag with label`` () =
  let qs = [("key", Some "vocabLabel/long-guid-1")]

  let tags =
    SearchResults.render {SearchResultsParameters.Empty with Vocabs=vocabs; Qs=qs}
    |> parseHtml
    |> CQ.select ".tag-label"

  tags |> CQ.first |> CQ.text |> should equal "Term1"

    
[<Test>]
let ``Should show active filter as tag with removal link`` () =
  let qsWithTwoActiveFilters = [("key", Some "vocabLabel/long-guid-1")
                                ("key", Some "vocabLabel/long-guid-2")]

  let tags =
    SearchResults.render {SearchResultsParameters.Empty with  Vocabs=vocabs; Qs=qsWithTwoActiveFilters}
    |> parseHtml
    |> CQ.select ".tag-remove-link"
  
  tags |> CQ.first |> CQ.attr "href" |> should equal "/search?key=vocabLabel%2Flong-guid-2"
  tags |> CQ.last |> CQ.attr "href" |> should equal "/search?key=vocabLabel%2Flong-guid-1"
