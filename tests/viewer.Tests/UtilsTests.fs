module Viewer.Tests.UtilsTests

open Viewer.Utils
open Viewer.Types
open NUnit.Framework
open FsUnit

    
[<Test>]
let ``extractFilters should return empty list given an empty querystring`` () =
  let qs = []
  let filters = extractFilters qs
  filters |> should equal []

    
[<Test>]
let ``extractFilters should convert query string pairs into record type`` () =
  let qs = [("vocab1", Some("uri1"));
            ("vocab2", Some("uri2"))]
  let filters = extractFilters qs

  filters |> should equal [{Vocab = "vocab1"; TermUri = "uri1"}
                           {Vocab = "vocab2"; TermUri = "uri2"}]

    
[<Test>]
let ``aggregateQueryStringValues should group by keys`` () =
  let qs = [
      {Vocab="key1"; TermUri="val1"}
      {Vocab="key1"; TermUri="val2"}
      {Vocab="key2"; TermUri="val3"}
      ] 
  let aggs = aggregateQueryStringValues qs
  aggs |> should equal [("key1",["val1";"val2"])
                        ("key2",["val3"])]

    
[<Test>]
let ``createFilterTags should create filter tags from filters`` () =
  let filters = [{Vocab = "vocab"; TermUri = "vocabLabel/long-guid2"}
                 {Vocab = "vocab"; TermUri = "vocabLabel/long-guid1"}
                ]
  let filterTags = createFilterTags filters
  filterTags |> should equal [{Label = "long-guid2"; RemovalQueryString = "vocab=vocabLabel%2Flong-guid1"}
                              {Label = "long-guid1"; RemovalQueryString = "vocab=vocabLabel%2Flong-guid2"}] 
