chai.should()

describe("Given filters have been selected and the apply filter button has been clicked", function(){
  it("should send one event with each filter aggregrated into one label", function() {
    var qs = "?key1=value&key1=value";
    var result = sidebar.extractKeysAndValuesFromUrl(qs);
    var keys = sidebar.extractKeys(result);
    var values = sidebar.extractValues(result);
    var uniqueKeys = sidebar.groupBy(keys);


    var ga = sinon.spy();
    
    googleAnalytics.sendFilters(ga, values); 

    var noOfArgumentsPassed = ga.getCalls()[0].args.length;
    noOfArgumentsPassed.should.equal(5); 
    ga.getCalls()[0].args[0].should.equal("send");
    ga.getCalls()[0].args[1].should.equal("event");
    ga.calledOnce.should.be.true; 

  });
});

describe("Given I am on the discovery tool page and I click the Clear All button", function(){
  it("should send an unapplied event to GA", function() {
    var ga = sinon.spy();
    
    googleAnalytics.sendClearFilters(ga); 

    var noOfArgumentsPassed = ga.getCalls()[0].args.length;
    noOfArgumentsPassed.should.equal(4); 
    ga.getCalls()[0].args[0].should.equal("send");
    ga.getCalls()[0].args[1].should.equal("event");
    ga.calledOnce.should.be.true; 

  });
});

describe("Given there has been a filter selected and I click the apply filter button and zero results have returned", function(){
  it("should send an event for each with results provided", function() {
    var ga = sinon.spy();
    
    googleAnalytics.sendResults(ga, []); 

    var noOfArgumentsPassed = ga.getCalls()[0].args.length;
    noOfArgumentsPassed.should.equal(5); 
    ga.getCalls()[0].args[0].should.equal("send");
    ga.getCalls()[0].args[1].should.equal("event");
    ga.getCalls()[0].args[3].should.equal("Without results");
    ga.getCalls()[0].args[4].should.equal("0 results");
    ga.calledOnce.should.be.true; 

  });
});

describe("Given there has been a filter selected and I click the apply filter button and results have returned", function(){
  it("should send an event for each with results provided", function() {
    var ga = sinon.spy();
    
    googleAnalytics.sendResults(ga, ["<div class='result'>something</div>"]); 

    var noOfArgumentsPassed = ga.getCalls()[0].args.length;
    noOfArgumentsPassed.should.equal(5); 
    ga.getCalls()[0].args[0].should.equal("send");
    ga.getCalls()[0].args[1].should.equal("event");
    ga.getCalls()[0].args[3].should.not.equal("Without results");
    ga.getCalls()[0].args[4].should.not.equal("0 results");
    ga.calledOnce.should.be.true; 

  });
});

describe("Scroll index, page height to percentage value function", function() {
    var tests = [
        { args: [1,4], expected: "Baseline" },
        { args: [2,4], expected: "50%" },
        { args: [3,4], expected: "75%" },
        { args: [4,4], expected: "100%" },
        { args: [NaN,4], expected: "No value due to error" },
        { args: [undefined,4], expected: "No value due to error" },
        // { args: [1,NaN], expected: "No value due to error" },
        // { args: [1,undefined], expected: "No value due to error" },
        // { args: [0,0], expected: "Baseline" },
    ];

    tests.forEach(function(test) {
        it(test.args[0] + " and " + test.args[1] + " should return the " + test.expected, function() {
            var result = googleAnalytics.getPercentageText(test.args[0], test.args[1]).text;
            result.should.equal(test.expected);
        });
    });
})
