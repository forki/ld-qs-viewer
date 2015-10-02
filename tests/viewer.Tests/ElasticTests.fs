module Viewer.Tests.QueryTests

open NUnit.Framework
open Swensen.Unquote
open Viewer.Elastic
open Viewer.Types

[<Test>]
let ``Should build query correctly for a single term`` () =
  let qs = [("key", Some("val"))]
  let query = BuildQuery qs
  let expectedQuery = """{
"from": 0, "size": 100,
"query": {
  "filtered": {
    "filter" : {
      "bool" : {
        "must" : [
          {"bool" : {
            "should" : [
              {"term" : {"qualitystandard:key" : "val"}}
            ]
          }}
        ]
      } 
    }
  }
}
}"""
  test <@ query = expectedQuery @>

[<Test>]
let ``Should build query correctly for a multiple terms with same key`` () =
  let qs = [("key", Some("val1"));
            ("key", Some("val2"))]

  let query = BuildQuery qs
  let expectedQuery = """{
"from": 0, "size": 100,
"query": {
  "filtered": {
    "filter" : {
      "bool" : {
        "must" : [
          {"bool" : {
            "should" : [
              {"term" : {"qualitystandard:key" : "val1"}},{"term" : {"qualitystandard:key" : "val2"}}
            ]
          }}
        ]
      } 
    }
  }
}
}"""
  test <@ query = expectedQuery @>

[<Test>]
let ``Should build query correctly for a multiple terms with different keys`` () =
  let qs = [("key", Some("val1"));
            ("key", Some("val2"));
            ("key2", Some("val3"));
            ("key2", Some("val4"))]

  let query = BuildQuery qs
  let expectedQuery = """{
"from": 0, "size": 100,
"query": {
  "filtered": {
    "filter" : {
      "bool" : {
        "must" : [
          {"bool" : {
            "should" : [
              {"term" : {"qualitystandard:key" : "val1"}},{"term" : {"qualitystandard:key" : "val2"}}
            ]
          }},{"bool" : {
            "should" : [
              {"term" : {"qualitystandard:key2" : "val3"}},{"term" : {"qualitystandard:key2" : "val4"}}
            ]
          }}
        ]
      } 
    }
  }
}
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
let ``GetSearchResults should map a single result`` () =

  let StubbedQueryResponse _ _ = 
    """
    {
      "hits":{
        "hits":[
          {
            "_id":"qs1_st1",
            "_source":{
              "@id":"This is the Uri",
              "dcterms:abstract": "This is the abstract"
            }
          }
        ]
      }
    }
    """

  let query = "{}"
  let DoSearchWith = GetSearchResults StubbedQueryResponse false
  let results = DoSearchWith query

  test <@ results = [{Uri = "This is the Uri";Abstract = "This is the abstract"}] @>

[<Test>]
let ``GetSearchResults should map multiple results`` () =
  
  let StubbedQueryResponse _ _ = 
    """
    {
      "hits":{
        "hits":[
          {
            "_id":"qs1_st1",
            "_source":{
              "@id":"",
              "dcterms:abstract": ""
            }
          },
          {
            "_id":"qs1_st2",
            "_source":{
              "@id":"",
              "dcterms:abstract": ""
            }
          }
        ]
      }
    }
    """

  let query = "{}"
  let DoSearchWith = GetSearchResults StubbedQueryResponse false
  let results = DoSearchWith query
  
  test <@ results.Length = 2 @>

