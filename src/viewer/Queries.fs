module Viewer.Queries

// Elasticsearch query fragments go here

let termQuery = """{"term" : {"%s" : "%s"}}"""

let shouldQuery = """{"bool" : {
            "should" : [
              %s
            ]
          }}"""

let mustQuery = """{
"from": 0, "size": 1500,
"query": {
  "filtered": {
    "filter" : {
      "bool" : {
        "must" : [
          %s
        ]
      } 
    }
  }
}
}"""
