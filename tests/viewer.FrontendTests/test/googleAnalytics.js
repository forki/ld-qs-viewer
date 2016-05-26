chai.should()

describe("Given there has been a filter selected", function(){
  it("should send an event for each filter selected", function() {
    var qs = "?key1=value&key1=value";
    var result = sidebar.extractKeysAndValuesFromUrl(qs);
    var keys = sidebar.extractKeys(result);
    var values = sidebar.extractValues(result);
    var uniqueKeys = sidebar.groupBy(keys);


    var ga = { send: sinon.spy() };
    
    googleAnalytics.sendFilters(ga, values); 

    var noOfArgumentsPassed = ga.send.getCalls()[0].args.length;
    noOfArgumentsPassed.should.equal(3); 
    ga.send.calledTwice.should.be.true; 

  });
});
