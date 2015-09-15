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
let ``GetSearchResults should map results correctly`` () =
  
  let StubbedQueryResponse _ = 
    """
    {
      "hits":{
        "hits":[
          {
            "_id":"qs1_st1",
            "_source":{
              "@id":"Uri1"
            }
          },
          {
            "_id":"qs1_st2",
            "_source":{
              "@id":"Uri2"
            }
          }
        ]
      }
    }
    """

  let query = "{}"
  let DoSearchWith = GetSearchResults StubbedQueryResponse 
  let results = DoSearchWith query
  
  test <@ results = [{Uri = "Uri1"};
                     {Uri = "Uri2"}] @>

