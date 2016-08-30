module Viewer.Tests.ElasticTests

open NUnit.Framework
open FsUnit
open Viewer.Data.Search.Elastic
open Viewer.Types
open Viewer.Components.SearchResults

[<Test>]
let ``Should build query correctly for a single term`` () =
  let qs = [{Vocab="key"; TermUri="val"}]
  let query = BuildQuery qs
  let expectedQuery = """{
"from": 0, "size": 1500,
"query": {
  "filtered": {
    "filter" : {
      "bool" : {
        "must" : [
          {"bool" : {
            "should" : [
              {"term" : {"key" : "val"}}
            ]
          }}
        ]
      }
    }
  }
},
"sort": [
  { "http://ld.nice.org.uk/ns/qualitystandard#qsidentifier" : { "order": "desc" }},
  { "http://ld.nice.org.uk/ns/qualitystandard#stidentifier" : { "order": "asc" }}
]
}"""
  query |> should equal expectedQuery

    
[<Test>]
let ``Should build query correctly for a multiple terms with same key`` () =
  let qs = [
      {Vocab="key"; TermUri="val1"}
      {Vocab="key"; TermUri="val2"}
      ]

  let query = BuildQuery qs
  let expectedQuery = """{
"from": 0, "size": 1500,
"query": {
  "filtered": {
    "filter" : {
      "bool" : {
        "must" : [
          {"bool" : {
            "should" : [
              {"term" : {"key" : "val1"}},{"term" : {"key" : "val2"}}
            ]
          }}
        ]
      }
    }
  }
},
"sort": [
  { "http://ld.nice.org.uk/ns/qualitystandard#qsidentifier" : { "order": "desc" }},
  { "http://ld.nice.org.uk/ns/qualitystandard#stidentifier" : { "order": "asc" }}
]
}"""
  query |> should equal expectedQuery

    
[<Test>]
let ``Should build query correctly for a multiple terms with different keys`` () =
  let qs = [
      {Vocab="key"; TermUri="val1"}
      {Vocab="key"; TermUri="val2"}
      {Vocab="key2"; TermUri="val3"}
      {Vocab="key2"; TermUri="val4"}

      ]

  let query = BuildQuery qs
  let expectedQuery = """{
"from": 0, "size": 1500,
"query": {
  "filtered": {
    "filter" : {
      "bool" : {
        "must" : [
          {"bool" : {
            "should" : [
              {"term" : {"key" : "val1"}},{"term" : {"key" : "val2"}}
            ]
          }},{"bool" : {
            "should" : [
              {"term" : {"key2" : "val3"}},{"term" : {"key2" : "val4"}}
            ]
          }}
        ]
      }
    }
  }
},
"sort": [
  { "http://ld.nice.org.uk/ns/qualitystandard#qsidentifier" : { "order": "desc" }},
  { "http://ld.nice.org.uk/ns/qualitystandard#stidentifier" : { "order": "asc" }}
]
}"""

  query |> should equal expectedQuery

    
[<Test>]
let ``GetSearchResults should return an empty list on zero results`` () =
  let StubbedQueryResponse _ _ = "{}"
  let query = "{}"
  let DoSearchWith = GetSearchResults StubbedQueryResponse false
  let results = DoSearchWith query

  results |> should equal []

[<Test>]
let ``ParseResponse should map a single result`` () =
  let stubbedResponse  = 
    """
    {
      "hits":{
        "hits":[
          {
            "_id":"st1_1",
            "_source":{
              "@id":"This is the Uri",
              "http://ld.nice.org.uk/ns/qualitystandard#abstract": "This is the abstract",
              "http://ld.nice.org.uk/ns/qualitystandard#title": "This is the title",
              "http://ld.nice.org.uk/ns/qualitystandard#firstissued": "01/01/0001"
            }
          }
        ]
      }
    }
    """

  let results = ParseResponse stubbedResponse

  results |> should equal [{Uri = "This is the Uri"; Abstract = "This is the abstract"; Title = "This is the title"; FirstIssued = new System.DateTime()}]

    
[<Test>]
let ``ParseResponse should map results`` () =
  let stubbedResponse = 
    """
    {
      "hits":{
        "hits":[
          {
            "_id": "st1_sd1",
            "_source":{
              "@id":"notused",
              "http://ld.nice.org.uk/ns/qualitystandard#abstract": "notused",
              "http://ld.nice.org.uk/ns/qualitystandard#title": "notused",
              "http://ld.nice.org.uk/ns/qualitystandard#firstissued": "01/01/1900"
            }
          },
          {
            "_id": "st1_sd2",
            "_source":{
              "@id":"notused",
              "http://ld.nice.org.uk/ns/qualitystandard#abstract": "notused",
              "http://ld.nice.org.uk/ns/qualitystandard#title": "notused",
              "http://ld.nice.org.uk/ns/qualitystandard#firstissued": "01/01/1900"
            }
          }
        ]
      }
    }
    """

  let results = ParseResponse stubbedResponse
  
  results.Length |> should equal 2

[<Test>]
  let ``Should prefix key with defined url`` () =
      let prefix = "http://something"
      let extractedFilters = [{Vocab = "vocab"; TermUri = "uri"}]

      let putUrlBackIn = prefixFiltersWithBaseUrl prefix

      let results = putUrlBackIn extractedFilters

      [{Vocab = "vocab"; TermUri = prefix+"uri"}] |> should equal results
    
[<Test>]
let ``Should build query correctly for an encoded single term key`` () =
  let qs = [{Vocab="key%3akey"; TermUri="val"}]
  let query = BuildQuery qs
  let expectedQuery = """{
"from": 0, "size": 1500,
"query": {
  "filtered": {
    "filter" : {
      "bool" : {
        "must" : [
          {"bool" : {
            "should" : [
              {"term" : {"key:key" : "val"}}
            ]
          }}
        ]
      }
    }
  }
},
"sort": [
  { "http://ld.nice.org.uk/ns/qualitystandard#qsidentifier" : { "order": "desc" }},
  { "http://ld.nice.org.uk/ns/qualitystandard#stidentifier" : { "order": "asc" }}
]
}"""
  query |> should equal expectedQuery
