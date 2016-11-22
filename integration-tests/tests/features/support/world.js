var webdriver = require("selenium-webdriver"),
    By = webdriver.By;
var elasticsearch = require('./elasticsearch.js');
var config = require('../config.json');

function CustomWorld() {
    this.driver = new webdriver.Builder()
        .withCapabilities(webdriver.Capabilities.phantomjs())
        .build();

    this.statementFinderUrl = "http://" + config.statementFinderUrl + "/qs";
    this.statementFinderUrlStyleSheet = this.statementFinderUrl + "/style.css";

    this.visit = function(url) {
      return this.driver.get(url);
    };

    this.createStatement = function(createstatements){
      var esStatement = {
        index: "kb",
        type: "qualitystatement",
        refresh: true,
        body: {
          "@id": "http://ld.nice.org.uk/things/0646b769-f791-4fc4-b6df-1fa421452e79",
          "@type": "qualitystandard:QualityStatement",
          "https://nice.org.uk/ontologies/qualitystandard#abstract": "<p>This is an abstract.</p>",
          "qualitystandard:appliesToServiceArea":  "https://nice.org.uk/ontologies/servicearea/488b1a36_ab26_4752_bc2e_1f988aae2da5",
          "https://nice.org.uk/ontologies/qualitystandard#hasPositionalId": "qs1-st1",
          "https://nice.org.uk/ontologies/qualitystandard#isNationalPriority": "yes",
          "https://nice.org.uk/ontologies/qualitystandard#qsidentifier": "0",
          "https://nice.org.uk/ontologies/qualitystandard#stidentifier": "0",
          "https://nice.org.uk/ontologies/qualitystandard#title": "This is a title",
          "https://nice.org.uk/ontologies/qualitystandard#wasFirstIssuedOn": "2016-04-01"

        }
      };

      return {
        addqsidentifier : function(qsidentifier) {
          esStatement.body["https://nice.org.uk/ontologies/qualitystandard#qsidentifier"] = qsidentifier;
        },

        addstidentifier : function(stidentifier) {
          esStatement.body["https://nice.org.uk/ontologies/qualitystandard#stidentifier"] = stidentifier;
        },

        addAbstract : function(Abstract) {
          esStatement.body["https://nice.org.uk/ontologies/qualitystandard#abstract"] = Abstract;
        },

        addserviceArea : function(serviceArea) {
          esStatement.body["qualitystandard:appliesToServiceArea"] = serviceArea;
        },

        addageGroup : function(ageGroup) {
          esStatement.body["qualitystandard:appliesToAgeGroup"] = ageGroup;
        },

        addSetting : function(Setting) {
          esStatement.body["qualitystandard:appliesToSetting"] = Setting;
        },

        addconditionDiseaese : function(conditionDisease) {
          esStatement.body["qualitystandard:appliesToConditionOrDisease"] = conditionDisease;
        },

        addfactorsAffectingHealthOrWellbeing : function(factorsAffectingHealthOrWellbeing) {
          esStatement.body["qualitystandard:appliesToFactorsAffectingHealthOrWellbeing"] = factorsAffectingHealthOrWellbeing;
        },

        build : function() {
          return esStatement;
        }

  };

};

    this.debug = function(ByPromise) {
      this.driver.findElement(ByPromise)
        .then(function(element) {
          element.getText()
            .then(function(t) {
              console.log(t);
            });
      });
    };
}

module.exports = function() {
    this.World = CustomWorld;
};
