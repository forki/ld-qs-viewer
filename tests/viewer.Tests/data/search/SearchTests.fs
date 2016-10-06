module Viewer.Tests.SearchTests

open NUnit.Framework
open FsUnit
open Viewer.Types
open Viewer.Data.Search.Search

[<Test>]
let ``PerformSearch should return a search result from the search provider`` () =
  let searchFilters = []
  let testing = false

  let expectedSearchResults = 
    [{Uri = "This is the Uri"
      Abstract = "This is the abstract"
      Title = "This is the title"
      FirstIssued = new System.DateTime()}]
  
  let dummyProvider _ = expectedSearchResults

  let performSearch = performSearchWithProvider dummyProvider
  
  let results = performSearch searchFilters
  results |> should equal expectedSearchResults