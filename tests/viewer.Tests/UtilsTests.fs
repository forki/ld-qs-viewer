module Viewer.Tests.UtilsTests

open NUnit.Framework
open Viewer.Utils
open Viewer.Types
open FsUnit
open FSharp.RDF
    
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
  let vocabs = [{Root = Term {
                              Uri = Uri.from "http://testing.com/Uri3"
                              ShortenedUri = "unknown/unknown"
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
    
  let filters = [{Vocab = "vocab"; TermUri = "vocabLabel/long-guid-2"}
                 {Vocab = "vocab"; TermUri = "vocabLabel/long-guid-1"}
                ]
  let filterTags = createFilterTags filters vocabs
  filterTags |> should equal [{Label = "Term2"; RemovalQueryString = "vocab=vocabLabel%2Flong-guid-1"}
                              {Label = "Term1"; RemovalQueryString = "vocab=vocabLabel%2Flong-guid-2"}] 


[<Test>]
let ``Should return an empty array when labels are not found`` () =
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
    
  let filters = [ 
                {Vocab = "notused"; TermUri = "guid-does-not-exist-1"};
                {Vocab = "notused"; TermUri = "guid-does-not-exist-2"}
                ]

  let filterTags = createFilterTags filters vocabs
  filterTags |> should equal [] 


