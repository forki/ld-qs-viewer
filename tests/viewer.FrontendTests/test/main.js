chai.should();

describe("Given there has been a filter selected", function(){
  it("should send an event for each filter selected", function() {
    jQuery.withArgs("input:submit[value='Apply filters']").calledOnce.should.be.true;
  });
});

describe("Given the clear all button has been clicked", function(){
  it("should send a clear event to GA", function() {
    jQuery.withArgs("a[type='reset']").calledOnce.should.be.true;
  });
});
