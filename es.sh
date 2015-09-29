#example of multi terms from one vocab query
#curl -XPOST "http://localhost:9200/kb/qualitystatement/_search?pretty" -d '{
#  "from": 0, "size": 100,
#  "query": {
#    "filtered": {
#      "filter" : {
#        "bool" : {
#          "should" : [
#            {"term" : {"qualitystandard:setting" : "http://ld.nice.org.uk/ns/qualitystandard/setting#Hospital"}},
#            {"term" : {"qualitystandard:setting" : "http://ld.nice.org.uk/ns/qualitystandard/setting#Outpatient clinic"}}
#          ]
#        }
#      }
#    }
#  } 
#}'


# example of multiple terms from different vocabs query
curl -XPOST "http://localhost:9200/kb/qualitystatement/_search?pretty" -d '{
  "from": 0, "size": 100,
  "query": {
    "filtered": {
      "filter" : {
        "bool" : {
          "must" : [
            {"bool": {
              "should" : [
                  {"term" : {"qualitystandard:setting" : "http://ld.nice.org.uk/ns/qualitystandard/setting#Hospital"}},
                  {"term" : {"qualitystandard:setting" : "http://ld.nice.org.uk/ns/qualitystandard/setting#Outpatient clinic"}}
                ]
              }  
            },
            {"bool": {
              "should" : [
                  {"term" : {"qualitystandard:targetPopulation" : "http://ld.nice.org.uk/ns/qualitystandard/agegroup#All age groups"}}
                ]
              }  
            }
          ]
        }
      }
    }
  } 
}'
