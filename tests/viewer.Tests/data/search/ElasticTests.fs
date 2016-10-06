module Viewer.Tests.ElasticTests

open NUnit.Framework
open FsUnit
open Viewer.Data.Search.Elastic
open Viewer.Types
open Viewer.Components.SearchResults

[<Test>]
let ``Should build query correctly for a single vocab and term term`` () =
  let filters = [{Vocab="vocab"; TermUris=["term"]}]
  let query = BuildQuery filters
  let expectedQuery = """{
"from": 0, "size": 1500,
"query": {
  "filtered": {
    "filter" : {
      "bool" : {
        "must" : [
          {"bool" : {
            "should" : [
              {"term" : {"vocab" : "term"}}
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
let ``Should build query correctly for a multiple terms with same vocab`` () =
  let filters = [{Vocab="vocab"; TermUris=["term1";"term2"]}]

  let query = BuildQuery filters
  let expectedQuery = """{
"from": 0, "size": 1500,
"query": {
  "filtered": {
    "filter" : {
      "bool" : {
        "must" : [
          {"bool" : {
            "should" : [
              {"term" : {"vocab" : "term1"}},{"term" : {"vocab" : "term2"}}
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
let ``Should build query correctly for a multiple terms across multiple vocabs`` () =
  let filters = [{Vocab="vocab"; TermUris=["term1";"term2"]}
                 {Vocab="vocab2"; TermUris=["term3";"term4"]}]

  let query = BuildQuery filters
  let expectedQuery = """{
"from": 0, "size": 1500,
"query": {
  "filtered": {
    "filter" : {
      "bool" : {
        "must" : [
          {"bool" : {
            "should" : [
              {"term" : {"vocab" : "term1"}},{"term" : {"vocab" : "term2"}}
            ]
          }},{"bool" : {
            "should" : [
              {"term" : {"vocab2" : "term3"}},{"term" : {"vocab2" : "term4"}}
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
let ``ParseResponse should return an empty list on zero results`` () =
  ParseResponse "" |> should equal []

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
              "http://ld.nice.org.uk/ns/qualitystandard#wasFirstIssuedOn": "01/01/0001"
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
              "http://ld.nice.org.uk/ns/qualitystandard#wasFirstIssuedOn": "01/01/1900"
            }
          },
          {
            "_id": "st1_sd2",
            "_source":{
              "@id":"notused",
              "http://ld.nice.org.uk/ns/qualitystandard#abstract": "notused",
              "http://ld.nice.org.uk/ns/qualitystandard#title": "notused",
              "http://ld.nice.org.uk/ns/qualitystandard#wasFirstIssuedOn": "01/01/1900"
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
    

