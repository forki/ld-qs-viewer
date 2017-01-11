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

describe("Given I have searched and retrieved some results, When I select a result", function() {
  it("should send the index of the result to GA", function(done) {
    var ga = sinon.spy();
    var index = 1;
    
    googleAnalytics.sendSearchIndex(ga, index); 

    var noOfArgumentsPassed = ga.getCalls()[0].args.length;
    noOfArgumentsPassed.should.equal(4); 
    ga.getCalls()[0].args[0].should.equal("send");
    ga.getCalls()[0].args[1].should.equal("event");
    ga.getCalls()[0].args[2].should.equal("Search Result Index");
    ga.getCalls()[0].args[3].should.equal(index);
    ga.calledOnce.should.be.true; 

    done(); 
  })
})

describe("Scroll index, page height to percentage value function", function() {
    var tests = [
        { args: [0,4], expected: { text: "Baseline", value: 0 } },
        { args: [1,4], expected: { text: "25%", value: 1 } },
        { args: [2,4], expected: { text: "50%", value: 2 }},
        { args: [3,4], expected: { text: "75%", value: 3 }},
        { args: [4,4], expected: { text: "100%", value: 4 }},
        { args: [NaN,4], expected: { text: "No value due to error", value: 0 }},
        { args: [undefined,4], expected:{ text: "No value due to error", value: 0 }},
        { args: [1,NaN], expected: { text: "No value due to error", value: 0 }},
        { args: [1,undefined], expected:{ text: "No value due to error", value: 0 }},
        { args: [0,0], expected: { text: "Baseline", value: 0 }},
    ];

    tests.forEach(function(test) {
        it(test.args[0] + " and " + test.args[1] + " should return the " + test.expected, function() {
            var result = "";
            var arg1 = test.args[0];
            var arg2 = test.args[1];
            result = googleAnalytics.getScrollDepth(arg1, arg2);
            result.text.should.equal(test.expected.text);
            result.value.should.equal(test.expected.value);
        });
    });
})
