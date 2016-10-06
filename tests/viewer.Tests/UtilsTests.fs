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
  let aggs = aggregateFiltersByVocab qs
  aggs |> should equal [{Vocab="key1";TermUris=["val1";"val2"]}
                        {Vocab="key2";TermUris=["val3"]}]

    
[<Test>]
let ``createFilterTags should create filter tags from filters`` () =
  let filters = [{Vocab = "vocab"; TermUri = "http://somelink.com/Uri#Term1"}
                 {Vocab = "vocab"; TermUri = "http://somelink.com/Uri#Term2"}]
  let filterTags = createFilterTags filters
  filterTags |> should equal [{Label = "Term1"; RemovalQueryString = "vocab=http%3A%2F%2Fsomelink.com%2FUri%23Term2"}
                              {Label = "Term2"; RemovalQueryString = "vocab=http%3A%2F%2Fsomelink.com%2FUri%23Term1"}] 
