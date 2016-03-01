module Viewer.Tests.UtilsTests

open Viewer.Utils
open Viewer.Types
open Fuchu
open Swensen.Unquote

[<Tests>]
let tests =
  testList "Utils" [

    testCase "extractFilters should return empty list given an empty querystring" <| fun _ ->
      let qs = []
      let filters = extractFilters qs
      test <@ filters = [] @>

    testCase "extractFilters should convert query string pairs into record type" <| fun _ ->
      let qs = [("vocab1", Some("uri1"));
                ("vocab2", Some("uri2"))]
      let filters = extractFilters qs

      test <@ filters = [{Vocab = "vocab"; TermUri = "uri1"}
                         {Vocab = "vocab2"; TermUri = "uri2"}] @>

    testCase "aggregateQueryStringValues should group by keys" <| fun _ ->
      let qs = [("key1", Some("val1"));
                ("key1", Some("val2"));
                ("key2", Some("val3"))]

      let aggs = aggregateQueryStringValues qs
      test <@ aggs = [("key1",["val1";"val2"]);
                      ("key2",["val3"])] @>

    testCase "createFilterTags should create filter tags from filters" <| fun _ ->
      let filters = [{Vocab = "vocab"; TermUri = "http://somelink.com/Uri#Term1"}
                     {Vocab = "vocab"; TermUri = "http://somelink.com/Uri#Term2"}]
      let filterTags = createFilterTags filters
      test <@ filterTags = [{Label = "Term1"; RemovalQueryString = "vocab=http%3A%2F%2Fsomelink.com%2FUri%23Term2"};
                            {Label = "Term2"; RemovalQueryString = "vocab=http%3A%2F%2Fsomelink.com%2FUri%23Term1"}] @>
  ]
