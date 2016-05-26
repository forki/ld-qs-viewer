chai.should()

describe("Given there has been a filter selected", function(){
  it("should send an event for each filter selected", function() {
      jQuery.withArgs("input:submit[value='Apply filters']").returns(sinon.stub({click: function(){}}));
    jQuery.calledOnce.should.be.true;
  });
});
