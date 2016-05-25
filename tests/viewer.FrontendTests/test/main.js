chai.should()

describe("Given there has been a filter selected", function(){
  it("should send an event for each filter selected", function() {
    jQuery.calledOnce.should.be.true;
  });
});
