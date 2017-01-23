/*jshint esversion: 6 */

var webdriver = require("selenium-webdriver"),
    By = webdriver.By;
var elasticsearch = require('./elasticsearch.js');
var config = require('../config.json');

var R = require('ramda')
var M = require('ramda-fantasy').Maybe
var Nothing = M.Nothing
var Just = M.Just

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
          "qualitystandard:7ae8413a_2811_4a09_a655_eff8d276ec87": "https://nice.org.uk/ontologies/servicearea/488b1a36_ab26_4752_bc2e_1f988aae2da5",
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

        addAbstract : function(abstract) {
          esStatement.body["https://nice.org.uk/ontologies/qualitystandard/1efaaa6a_c81a_4bd6_b598_c626b21c71fd"] = abstract;
        },

        addserviceArea : function(serviceArea) {
          esStatement.body["qualitystandard:7ae8413a_2811_4a09_a655_eff8d276ec87"] = serviceArea;
        },

        addageGroup : function(ageGroup) {
          esStatement.body["qualitystandard:4e7a368e_eae6_411a_8167_97127b490f99"] = ageGroup;
        },

        addSetting : function(setting) {
          esStatement.body["qualitystandard:62496684_7027_4f37_bd0e_264c9ff727fd"] = setting;
        },

        addconditionDiseaese : function(value, explicitAndImplicit) {
          const lookup = R.curry((k, obj) => k in obj ? Just(obj[k]) : Nothing());
          const justs = R.chain(M.maybe([], R.of));

          const conditionOrDisease = "qualitystandard:28745bc0_6538_46ee_8b71_f0cf107563d9";
          const root = R.defaultTo(Nothing(), explicitAndImplicit)
          const resultExplicit = lookup('explicit', root)
          const resultImplicit = lookup('implicit', root)

          const allUniqueItems = R.pipe(
            R.prepend(resultExplicit),
            R.prepend(resultImplicit),
            R.flatten,
            justs,
            R.flatten,
            R.uniq
          );

          esStatement.body[conditionOrDisease] = allUniqueItems([ Just( value ) ]);
          esStatement.body[conditionOrDisease + ':explicit'] = resultExplicit.getOrElse([])
          esStatement.body[conditionOrDisease + ':implicit'] = resultImplicit.getOrElse([])

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
