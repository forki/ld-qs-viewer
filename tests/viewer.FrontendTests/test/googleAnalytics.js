chai.should()

describe("Given there has been a filter selected", function(){
  it("should send an event for each filter selected", function() {
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
    ga.calledTwice.should.be.true; 

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
