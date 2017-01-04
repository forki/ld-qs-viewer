var elasticsearch = require('elasticsearch');
var config = require("../config.json");

var elasticClient = new elasticsearch.Client({
    host: config.elasticUrl,
    log: 'info'
});

var indexName = "kb";

/**
* Delete an existing index
*/
function deleteIndex() {
  return elasticClient.indices.delete({
    index: indexName
  });
}
exports.deleteIndex = deleteIndex;

function createStatement(data) {
  return elasticClient.create(data);
}
exports.createStatement = createStatement;

/**
* create the index
*/
function initIndex() {
    return elasticClient.indices.create({
      index: indexName,
         body: {
             "mappings": {
               "qualitystatement": {
                   "dynamic_templates": [
                       { "notanalyzed": {
                             "match":              "*3ff270e4_655a_4884_b186_e033f58759de",
                             "match_mapping_type": "string",
                             "mapping": {
                                 "type":        "integer"
                             }
                          }
                       },
                       { "notanalyzed": {
                             "match":              "*9fcb3758_a4d3_49d7_ab10_6591243caa67",
                             "match_mapping_type": "string",
                             "mapping": {
                                 "type":        "integer"
                             }
                          }
                       },
                       { "notanalyzed": {
                             "match":              "*0886da59_2c5f_4124_9f46_6be4537a4099",
                             "match_mapping_type": "string",
                             "mapping": {
                                 "type":        "date",
                                 "format":      "dateOptionalTime"
                             }
                          }
                       },
                       { "notanalyzed": {
                             "match":              "*",
                             "match_mapping_type": "string",
                             "mapping": {
                                 "type":        "string",
                                 "index":       "not_analyzed"
                             }
                          }
                       }
                     ]
                  }
              }
          }
      }
    );
}
exports.initIndex = initIndex;

/**
* check if the index exists
*/
function indexExists() {
    return elasticClient.indices.exists({
        index: indexName
    });
}
exports.indexExists = indexExists;

function initMapping() {
    return elasticClient.indices.putMapping({
         index: indexName,
         body: {
          "mappings": {
              "qualitystatement": {
                  "properties": {
                      "@context": {"type": "object", "index": "no", "store": "no"},
                      "@id": {"type": "string", "index": "not_analyzed"},
                      "@type": {"type": "string", "index": "not_analyzed"},
                      "https://nice.org.uk/ontologies/qualitystandard/1efaaa6a_c81a_4bd6_b598_c626b21c71fd": {"type": "string", "index": "not_analyzed"},
                      "https://nice.org.uk/ontologies/qualitystandard/84efb231_0424_461e_9598_1ef5272a597a" : {"type" : "string"},
                      "https://nice.org.uk/ontologies/qualitystandard/c2cb17d6_238e_437d_af64_1b6f1003bc36" : {"type" : "string"},
                      "https://nice.org.uk/ontologies/qualitystandard/3ff270e4_655a_4884_b186_e033f58759de" : {"type" : "integer"},
                      "https://nice.org.uk/ontologies/qualitystandard/9fcb3758_a4d3_49d7_ab10_6591243caa67": {"type": "integer"},
                      "https://nice.org.uk/ontologies/qualitystandard/bc8e0db0_5d8a_4100_98f6_774ac0eb1758": {"type": "string", "index": "not_analyzed"},
                      "https://nice.org.uk/ontologies/qualitystandard/0886da59_2c5f_4124_9f46_6be4537a4099" : {"type" : "date","format" : "dateOptionalTime"},
                      "qualitystandard:4e7a368e_eae6_411a_8167_97127b490f99" : {"type" : "string", "index" : "not_analyzed"},
                      "qualitystandard:28745bc0_6538_46ee_8b71_f0cf107563d9" : {"type" : "string", "index" : "not_analyzed"},
                      "qualitystandard:18aa6468_de94_4f9f_bd7a_0075fba942a5" : {"type" : "string", "index" : "not_analyzed"},
                      "qualitystandard:7ae8413a_2811_4a09_a655_eff8d276ec87" : {"type" : "string", "index" : "not_analyzed"},
                      "qualitystandard:62496684_7027_4f37_bd0e_264c9ff727fd" : {"type" : "string", "index" : "not_analyzed"}
                  }
              }
          }
      }
    });
}
exports.initMapping = initMapping;

function refreshIndex() {
  return elasticClient.indices.refresh({
        index: indexName});
}

exports.refreshIndex = refreshIndex;
