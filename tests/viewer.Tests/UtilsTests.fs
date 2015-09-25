module Viewer.Tests.UtilsTests

open Viewer.Utils
open Swensen.Unquote
open NUnit.Framework

[<Test>]
let ``no filters should be extracted from an empty querystring`` () =
  let qs = []
  let filters = extractFilters qs
  test <@ filters = [] @>

[<Test>]
let ``Filter values should be extracted from a querystring`` () =
  let qs = [("key1", Some("val1"));
            ("key2", Some("val2"))]
  let filters = extractFilters qs

  test <@ filters = ["val1";"val2"] @>
