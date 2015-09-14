module Viewer.Tests.Stubs

let stubbedElasticResponseWithTwoResults = 
  """
  {
    "hits":{
      "hits":[
        {
          "_id":"qs1_st1",
          "_source":{
            "@id":"http://ld.nice.org.uk/prov/entity#f178fc5:qualitystandards/qs1/st1/Statement.md",
            "abstract":"This is statement 1"
          }
        }
        {
          "_id":"qs1_st2",
          "_source":{
            "@id":"http://ld.nice.org.uk/prov/entity#f178fc5:qualitystandards/qs1/st2/Statement.md",
            "abstract":"This is statement 2"
          }
        }
      ]
    }
  }
  """
