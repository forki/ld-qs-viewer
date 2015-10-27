module Viewer.Tests.UtilsTests

open Viewer.Utils
open Viewer.Types
open Swensen.Unquote
open NUnit.Framework

[<Test>]
let ``no filters should be extracted from an empty querystring`` () =
  let qs = []
  let filters = extractFilters qs
  test <@ filters = [] @>

[<Test>]
let ``Aggregate querystring values`` () =
  let qs = [("key1", Some("val1"));
            ("key1", Some("val2"));
            ("key2", Some("val3"))]

  let aggs = aggregateQueryStringValues qs
  test <@ aggs = [("key1",["val1";"val2"]);
                  ("key2",["val3"])] @>

[<Test>]
let ``filters should be extracted from a querystring`` () =
  let qs = [("vocab1", Some("uri1"));
            ("vocab2", Some("uri2"))]
  let filters = extractFilters qs

  test <@ filters = [{Vocab = "vocab1"; TermUri = "uri1"}
                     {Vocab = "vocab2"; TermUri = "uri2"}] @>

[<Test>]
let ``Filter tags should be created from filters`` () =
  let filters = [{Vocab = "vocab"; TermUri = "http://somelink.com/Uri#Term1"}
                 {Vocab = "vocab"; TermUri = "http://somelink.com/Uri#Term2"}]
  let filterTags = createFilterTags filters
  test <@ filterTags = [{Label = "Term1"; RemovalQueryString = "vocab=http%3A%2F%2Fsomelink.com%2FUri%23Term2"};
                        {Label = "Term2"; RemovalQueryString = "vocab=http%3A%2F%2Fsomelink.com%2FUri%23Term1"}] @>
