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
  console.log("deleted index")
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
  console.log("init index");
    return elasticClient.indices.create({
      index: indexName,
         body: {
          "mappings": {
              "qualitystatement": {
                  "properties": {
                      "@context": {"type": "object", "index": "no", "store": "no"},
                      "@id": {"type": "string", "index": "not_analyzed"},
                      "@type": {"type": "string", "index": "not_analyzed"},
                      "https://nice.org.uk/ontologies/qualitystandard#abstract": {"type": "string", "index": "not_analyzed"},
                      "https://nice.org.uk/ontologies/qualitystandard#hasPositionalId" : {"type" : "string"},
                      "https://nice.org.uk/ontologies/qualitystandard#isNationalPriority" : {"type" : "string"},
                      "https://nice.org.uk/ontologies/qualitystandard#qsidentifier" : {"type" : "integer"},
                      "https://nice.org.uk/ontologies/qualitystandard#stidentifier": {"type": "integer"},
                      "https://nice.org.uk/ontologies/qualitystandard#title": {"type": "string", "index": "not_analyzed"},
                      "https://nice.org.uk/ontologies/qualitystandard#wasFirstIssuedOn" : {"type" : "date","format" : "dateOptionalTime"},
                      "qualitystandard:appliesToAgeGroup" : {"type" : "string", "index" : "not_analyzed"},
                      "qualitystandard:appliesToConditionOrDisease" : {"type" : "string", "index" : "not_analyzed"},
                      "qualitystandard:appliesToFactorsAffectingHealthOrWellbeing" : {"type" : "string", "index" : "not_analyzed"},
                      "qualitystandard:appliesToServiceArea" : {"type" : "string", "index" : "not_analyzed"},
                      "qualitystandard:appliesToSetting" : {"type" : "string", "index" : "not_analyzed"}
                  }
              }
          }
         }
    });
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
                      "https://nice.org.uk/ontologies/qualitystandard#abstract": {"type": "string", "index": "not_analyzed"},
                      "https://nice.org.uk/ontologies/qualitystandard#hasPositionalId" : {"type" : "string"},
                      "https://nice.org.uk/ontologies/qualitystandard#isNationalPriority" : {"type" : "string"},
                      "https://nice.org.uk/ontologies/qualitystandard#qsidentifier" : {"type" : "integer"},
                      "https://nice.org.uk/ontologies/qualitystandard#stidentifier": {"type": "integer"},
                      "https://nice.org.uk/ontologies/qualitystandard#title": {"type": "string", "index": "not_analyzed"},
                      "https://nice.org.uk/ontologies/qualitystandard#wasFirstIssuedOn" : {"type" : "date","format" : "dateOptionalTime"},
                      "qualitystandard:appliesToAgeGroup" : {"type" : "string", "index" : "not_analyzed"},
                      "qualitystandard:appliesToConditionOrDisease" : {"type" : "string", "index" : "not_analyzed"},
                      "qualitystandard:appliesToFactorsAffectingHealthOrWellbeing" : {"type" : "string", "index" : "not_analyzed"},
                      "qualitystandard:appliesToServiceArea" : {"type" : "string", "index" : "not_analyzed"},
                      "qualitystandard:appliesToSetting" : {"type" : "string", "index" : "not_analyzed"}
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
