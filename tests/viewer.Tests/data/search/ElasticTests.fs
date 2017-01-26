module Viewer.Tests.ElasticTests

open NUnit.Framework
open FsUnit
open Viewer.Data.Search.Elastic
open Viewer.Types
open Viewer.Components.SearchResults
open FSharp.Data

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
  { "https://nice.org.uk/ontologies/qualitystandard/3ff270e4_655a_4884_b186_e033f58759de" : { "order": "desc" }},
  { "https://nice.org.uk/ontologies/qualitystandard/9fcb3758_a4d3_49d7_ab10_6591243caa67" : { "order": "asc" }}
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
  { "https://nice.org.uk/ontologies/qualitystandard/3ff270e4_655a_4884_b186_e033f58759de" : { "order": "desc" }},
  { "https://nice.org.uk/ontologies/qualitystandard/9fcb3758_a4d3_49d7_ab10_6591243caa67" : { "order": "asc" }}
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
  { "https://nice.org.uk/ontologies/qualitystandard/3ff270e4_655a_4884_b186_e033f58759de" : { "order": "desc" }},
  { "https://nice.org.uk/ontologies/qualitystandard/9fcb3758_a4d3_49d7_ab10_6591243caa67" : { "order": "asc" }}
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
              "https://nice.org.uk/ontologies/qualitystandard/1efaaa6a_c81a_4bd6_b598_c626b21c71fd": "This is the abstract",
              "https://nice.org.uk/ontologies/qualitystandard/bc8e0db0_5d8a_4100_98f6_774ac0eb1758": "This is the title",
              "https://nice.org.uk/ontologies/qualitystandard/0886da59_2c5f_4124_9f46_6be4537a4099": "01/01/0001"
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
              "https://nice.org.uk/ontologies/qualitystandard/1efaaa6a_c81a_4bd6_b598_c626b21c71fd": "notused",
              "https://nice.org.uk/ontologies/qualitystandard/bc8e0db0_5d8a_4100_98f6_774ac0eb1758": "notused",
              "https://nice.org.uk/ontologies/qualitystandard/0886da59_2c5f_4124_9f46_6be4537a4099": "01/01/1900"
            }
          },
          {
            "_id": "st1_sd2",
            "_source":{
              "@id":"notused",
              "https://nice.org.uk/ontologies/qualitystandard/1efaaa6a_c81a_4bd6_b598_c626b21c71fd": "notused",
              "https://nice.org.uk/ontologies/qualitystandard/bc8e0db0_5d8a_4100_98f6_774ac0eb1758": "notused",
              "https://nice.org.uk/ontologies/qualitystandard/0886da59_2c5f_4124_9f46_6be4537a4099": "01/01/1900"
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
let ``Should build query with boosted terms`` () =
  let filters = [{Vocab="vocab"; TermUris=["term1";]}]

  let query = BuildQueryWithRelevancy filters
  let expectedQuery = """{
"from": 0,
  "size": 1500,
  "query": {
    "bool": {
      "must": [
        {
          "bool": {
            "should": [
              {
                "match": {
                  "vocab:explicit": {
                    "query": "term1",
                    "boost": 10
                  }
                }
              },
              {
                "match": {
                  "vocab:implicit": {
                    "query": "term1",
                    "boost": 1
                  }
                }
              }
            ]
          }
        }
      ]
    }
  },
  "sort" : ["_score"]
}
"""

  let expectedJson = JsonValue.Parse expectedQuery
  let resultedJson = JsonValue.Parse query

  resultedJson |> should equal expectedJson

