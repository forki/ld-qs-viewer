module Viewer.Tests.ElasticTests

open NUnit.Framework
open Swensen.Unquote
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
  { "http://ld.nice.org.uk/ns/qualitystandard#qsidentifier" : { "order": "asc" }},
  { "http://ld.nice.org.uk/ns/qualitystandard#stidentifier" : { "order": "asc" }}
]
}"""
  test <@ query = expectedQuery @>

    
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
  { "http://ld.nice.org.uk/ns/qualitystandard#qsidentifier" : { "order": "asc" }},
  { "http://ld.nice.org.uk/ns/qualitystandard#stidentifier" : { "order": "asc" }}
]
}"""
  test <@ query = expectedQuery @>

    
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
  { "http://ld.nice.org.uk/ns/qualitystandard#qsidentifier" : { "order": "asc" }},
  { "http://ld.nice.org.uk/ns/qualitystandard#stidentifier" : { "order": "asc" }}
]
}"""

  test <@ query = expectedQuery @>

    
[<Test>]
let ``GetSearchResults should return an empty list on zero results`` () =
  let StubbedQueryResponse _ _ = "{}"
  let query = "{}"
  let DoSearchWith = GetSearchResults StubbedQueryResponse false
  let results = DoSearchWith query

  test <@ results = [] @>

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
              "http://ld.nice.org.uk/ns/qualitystandard#title": "This is the title"
            }
          }
        ]
      }
    }
    """

  let results = ParseResponse stubbedResponse

  test <@ results = [{Uri = "This is the Uri"; Abstract = "This is the abstract"; Title = "This is the title"}] @>

    
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
              "http://ld.nice.org.uk/ns/qualitystandard#title": "notused"
            }
          },
          {
            "_id": "st1_sd2",
            "_source":{
              "@id":"notused",
              "http://ld.nice.org.uk/ns/qualitystandard#abstract": "notused",
              "http://ld.nice.org.uk/ns/qualitystandard#title": "notused"
            }
          }
        ]
      }
    }
    """

  let results = ParseResponse stubbedResponse
  
  test <@ results.Length = 2 @>

[<Test>]
  let ``Should prefix key with defined url`` () =
      let prefix = "http://something"
      let extractedFilters = [{Vocab = "vocab"; TermUri = "uri"}]

      let putUrlBackIn = prefixUrl prefix

      let results = putUrlBackIn extractedFilters

      test <@ [{Vocab = "vocab"; TermUri = prefix+"uri"}] = results @>
    
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
  { "http://ld.nice.org.uk/ns/qualitystandard#qsidentifier" : { "order": "asc" }},
  { "http://ld.nice.org.uk/ns/qualitystandard#stidentifier" : { "order": "asc" }}
]
}"""
  test <@ query = expectedQuery @>
