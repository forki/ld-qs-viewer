var webdriver = require("selenium-webdriver"),
    By = webdriver.By;
var elasticsearch = require('./elasticsearch.js');
var config = require('../config.json');

function CustomWorld() {
    this.driver = new webdriver.Builder()
        .withCapabilities(webdriver.Capabilities.phantomjs())
        .build();

    this.statementFinderUrl = "http://" + config.statementFinderUrl;
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
          "https://nice.org.uk/ontologies/qualitystandard/1efaaa6a_c81a_4bd6_b598_c626b21c71fd": "<p>This is an abstract.</p>",
          "qualitystandard:7ae8413a_2811_4a09_a655_eff8d276ec87":  "https://nice.org.uk/ontologies/servicearea/488b1a36_ab26_4752_bc2e_1f988aae2da5",
          "https://nice.org.uk/ontologies/qualitystandard/bc8e0db0_5d8a_4100_98f6_774ac0eb1758": "qs1-st1",
          "https://nice.org.uk/ontologies/qualitystandard/c2cb17d6_238e_437d_af64_1b6f1003bc36": "yes",
          "https://nice.org.uk/ontologies/qualitystandard/3ff270e4_655a_4884_b186_e033f58759de": "0",
          "https://nice.org.uk/ontologies/qualitystandard/9fcb3758_a4d3_49d7_ab10_6591243caa67": "0",
          "https://nice.org.uk/ontologies/qualitystandard/bc8e0db0_5d8a_4100_98f6_774ac0eb1758": "This is a title",
          "https://nice.org.uk/ontologies/qualitystandard/0886da59_2c5f_4124_9f46_6be4537a4099": "2016-04-01"

        }
      };

      return {
        addqsidentifier : function(qsidentifier) {
          esStatement.body["https://nice.org.uk/ontologies/qualitystandard/3ff270e4_655a_4884_b186_e033f58759de"] = qsidentifier;
        },

        addstidentifier : function(stidentifier) {
          esStatement.body["https://nice.org.uk/ontologies/qualitystandard/9fcb3758_a4d3_49d7_ab10_6591243caa67"] = stidentifier;
        },

        addAbstract : function(Abstract) {
          esStatement.body["https://nice.org.uk/ontologies/qualitystandard/1efaaa6a_c81a_4bd6_b598_c626b21c71fd"] = Abstract;
        },

        addserviceArea : function(serviceArea) {
          esStatement.body["qualitystandard:7ae8413a_2811_4a09_a655_eff8d276ec87"] = serviceArea;
        },

        addageGroup : function(ageGroup) {
          esStatement.body["qualitystandard:4e7a368e_eae6_411a_8167_97127b490f99"] = ageGroup;
        },

        addSetting : function(Setting) {
          esStatement.body["qualitystandard:62496684_7027_4f37_bd0e_264c9ff727fd"] = Setting;
        },

        addconditionDiseaese : function(value, explicitAndImplicit) {
          var conditionOrDisease = "qualitystandard:28745bc0_6538_46ee_8b71_f0cf107563d9";
          esStatement.body[conditionOrDisease] = value;
          if (explicitAndImplicit) {
            explicitAndImplicit.explicit.map(function(i) {
              esStatement.body[conditionOrDisease + ':explicit'] = i;
            });

            explicitAndImplicit.implicit.map(function(i) {
              esStatement.body[conditionOrDisease+ ':implicit'] = i;
            });
          }
        },

        addfactorsAffectingHealthOrWellbeing : function(factorsAffectingHealthOrWellbeing) {
          esStatement.body["qualitystandard:18aa6468_de94_4f9f_bd7a_0075fba942a5"] = factorsAffectingHealthOrWellbeing;
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
