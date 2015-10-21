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
}
}"""
  test <@ query = expectedQuery @>

[<Test>]
let ``Should build query correctly for a multiple terms with same key`` () =
  let qs = [("key", Some("val1"));
            ("key", Some("val2"))]

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
            "_id":"st1_1",
            "_source":{
              "@id":"http://ld.somedomain.com/prov/entity#XXXXXX:path/qs7/to/Statement.md",
              "http://ld.nice.org.uk/ns/qualitystandard#abstract": "This is the abstract",
              "http://ld.nice.org.uk/ns/qualitystandard#title": "This is the title"
            }
          }
        ]
      }
    }
    """

  let query = "{}"
  let DoSearchWith = GetSearchResults StubbedQueryResponse false
  let results = DoSearchWith query

  test <@ results = [{Uri = "/path/qs7/to/Statement.html"; Abstract = "This is the abstract"; Title = "This is the title (qs7)"}] @>

[<Test>]
let ``GetSearchResults should map multiple results`` () =
  
  let StubbedQueryResponse _ _ = 
    """
    {
      "hits":{
        "hits":[
          {
            "_id": "st1_sd1",
            "_source":{
              "@id":"http://ld.somedomain.com/prov/entity#XXXXXX:path/qs8/to/Statement.md",
              "http://ld.nice.org.uk/ns/qualitystandard#abstract": "Abstract",
              "http://ld.nice.org.uk/ns/qualitystandard#title": "This is the title 1"
            }
          },
          {
            "_id": "st1_sd2",
            "_source":{
              "@id":"http://ld.somedomain.com/prov/entity#XXXXXX:path/qs8/to/Statement.md",
              "http://ld.nice.org.uk/ns/qualitystandard#abstract": "Abstract",
              "http://ld.nice.org.uk/ns/qualitystandard#title": "This is the title 1"
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

[<Test>]
let ``Should build query correctly for an encoded single term key`` () =
  let qs = [("key%3akey", Some("val"))]
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
}
}"""
  test <@ query = expectedQuery @>
