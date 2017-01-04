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
                             "mapping": {
                                 "type":        "integer"
                             }
                          }
                       },
                       { "notanalyzed": {
                             "match":              "*9fcb3758_a4d3_49d7_ab10_6591243caa67",
                             "mapping": {
                                 "type":        "integer"
                             }
                          }
                       },
                       { "notanalyzed": {
                             "match":              "*0886da59_2c5f_4124_9f46_6be4537a4099",
                             "mapping": {
                                 "type":        "date",
                                 "format":      "dateOptionalTime"
                             }
                          }
                       },
                       { "notanalyzed": {
                             "match":              "*",
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

function refreshIndex() {
  return elasticClient.indices.refresh({
        index: indexName});
}

exports.refreshIndex = refreshIndex;
