var should = require("should");
var sidebar = require("../../../src/viewer/components/sidebar/client/checkboxesFromUrlFilter.js")();

describe("Given I have a url with filters" ,function() {
  it("should give me an array of keys and values", function() {
    var qs = "?key=value";
    var result = sidebar.extractKeysAndValuesFromUrl(qs);
    var keys = sidebar.extractKeys(result);
    var values = sidebar.extractValues(result);

    keys[0].should.equal("key");
    values[0].should.equal("value");
  })
});

describe("Given I have a url with more filters that have the same key" ,function() {
  it("should give me an array of unique keys and values", function() {
    var qs = "?key1=value&key1=value2";
    var result = sidebar.extractKeysAndValuesFromUrl(qs);
    var keys = sidebar.extractKeys(result);
    var values = sidebar.extractValues(result);
    var uniqueKeys = sidebar.groupBy(keys);

    uniqueKeys.length.should.equal(1);
    uniqueKeys[0].should.equal("key1");
    values[0].should.equal("value");
    values[1].should.equal("value2");
  })
});


