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
    noOfArgumentsPassed.should.equal(4); 
    ga.getCalls()[0].args[0].should.equal("send");
    ga.calledTwice.should.be.true; 

  });
});
