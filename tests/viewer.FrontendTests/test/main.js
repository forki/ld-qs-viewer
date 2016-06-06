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

describe("Links in the site", function(){
    it("should send an event to GA", function() {
        jQuery.withArgs("#mailFeedback").calledOnce.should.be.true;
        jQuery.withArgs("#old-standards").calledOnce.should.be.true;
    });
});

describe("scroll in results div", function(){
    it("should send an event to GA", function() {
        jQuery.withArgs(".card-list-wrapper").calledOnce.should.be.true;
    });
});
