module Viewer.Data.Search.Queries

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
},
"sort": [
  { "https://nice.org.uk/ontologies/qualitystandard#qsidentifier" : { "order": "desc" }},
  { "https://nice.org.uk/ontologies/qualitystandard#stidentifier" : { "order": "asc" }}
]
}"""
