module Viewer.Tests.QueryTests

open NUnit.Framework
open Swensen.Unquote
open Viewer.Elastic
open Viewer.Types

[<Test>]
let ``Elastic query response should be mapped correctly`` () =
  
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

