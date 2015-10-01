module Viewer.Queries

// Elasticsearch query fragments go here

let termQuery = """{"term" : {"qualitystandard:%s" : "%s"}}"""

let shouldQuery = """{"bool" : {
            "should" : [
              %s
            ]
          }}"""

let mustQuery = """{
"from": 0, "size": 100,
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
