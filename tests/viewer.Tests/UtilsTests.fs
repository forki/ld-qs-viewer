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
let ``Filter values should be extracted from a querystring`` () =
  let qs = [("key1", Some("val1"));
            ("key2", Some("val2"))]
  let filters = extractFilters qs

  test <@ filters = [{Key = "key1"; Val = "val1"}
                     {Key = "key2"; Val = "val2"}] @>

[<Test>]
let ``Filter tags should be created from filters`` () =
  let filters = [{Key = "key"; Val = "http://somelink.com/Uri#val1"}
                 {Key = "key"; Val = "http://somelink.com/Uri#val2"}]
  let filterTags = createFilterTags filters
  test <@ filterTags = [{Label = "val1"; RemovalQueryString = "key=http%3A%2F%2Fsomelink.com%2FUri%23val2"};
                        {Label = "val2"; RemovalQueryString = "key=http%3A%2F%2Fsomelink.com%2FUri%23val1"}] @>
