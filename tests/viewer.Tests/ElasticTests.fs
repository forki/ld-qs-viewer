module Viewer.Tests.QueryTests

open NUnit.Framework
open Swensen.Unquote
open Viewer.Elastic
open Viewer.Types

[<Test>]
let ``GetSearchResults should return an empty list on zero results`` () =
  let StubbedQueryResponse _ = "{}"
  let query = "{}"
  let DoSearchWith = GetSearchResults StubbedQueryResponse 
  let results = DoSearchWith query

  test <@ results = [] @>


[<Test>]
let ``GetSearchResults should map a single result`` () =

  let StubbedQueryResponse _ = 
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
  let DoSearchWith = GetSearchResults StubbedQueryResponse 
  let results = DoSearchWith query

  test <@ results = [{Uri = "This is the Uri";Abstract = "This is the abstract"}] @>

[<Test>]
let ``GetSearchResults should map multiple results`` () =
  
  let StubbedQueryResponse _ = 
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
  let DoSearchWith = GetSearchResults StubbedQueryResponse 
  let results = DoSearchWith query
  
  test <@ results.Length = 2 @>

