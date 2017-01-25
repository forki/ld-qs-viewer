module Viewer.Data.Search.Queries

// Elasticsearch query fragments go here

let termQuery = """{"term" : {"%s" : "%s"}}"""

let shouldQuery = """{"bool" : {
            "should" : [
              %s
            ]
          }}"""

let relevancyQuery = """{
"from": 0, "size": 1500,
"query": {
  "bool" : {
    "must" : [
      %s
    ]
  }
},
"sort": [
  { "https://nice.org.uk/ontologies/qualitystandard/3ff270e4_655a_4884_b186_e033f58759de" : { "order": "desc" }},
  { "https://nice.org.uk/ontologies/qualitystandard/9fcb3758_a4d3_49d7_ab10_6591243caa67" : { "order": "asc" }}
]
}"""

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
  { "https://nice.org.uk/ontologies/qualitystandard/3ff270e4_655a_4884_b186_e033f58759de" : { "order": "desc" }},
  { "https://nice.org.uk/ontologies/qualitystandard/9fcb3758_a4d3_49d7_ab10_6591243caa67" : { "order": "asc" }}
]
}"""
