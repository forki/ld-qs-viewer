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

  test <@ filters = ["val1";"val2"] @>

[<Test>]
let ``Filter tags should be created from filters`` () =
  let filters = ["Something#val1";"Something#val2"]
  let filterTags = createFilterTags filters

  test <@ filterTags = [{Label = "val1"};
                        {Label = "val2"}] @>
