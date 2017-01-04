var seleniumWebdriver = require('selenium-webdriver'),
    By = seleniumWebdriver.By,
    until = seleniumWebdriver.until;
    driver = seleniumWebdriver.driver;
var chai = require('chai');
chai.should();
var es = require('../support/elasticsearch.js');

module.exports = function () {
  this.Given(/^I have published quality statements annotated with vocabulary term 1$/, function (done) {

    var createStatement = new this.createStatement();
    createStatement.addqsidentifier("1");
    createStatement.addstidentifier("1");
    createStatement.addserviceArea("https://nice.org.uk/ontologies/servicearea/41def0db_ac59_4e68_a61b_ad1183bced27");
    // createStatement.addSetting("https://nice.org.uk/ontologies/setting/e517e9af_3c12_4f8b_a320_9323dfdf2510");
    var esStatement = createStatement.build();

    var createStatement2 = new this.createStatement();
    createStatement2.addqsidentifier("1");
    createStatement2.addstidentifier("2");
    createStatement2.addserviceArea("https://nice.org.uk/ontologies/servicearea/41def0db_ac59_4e68_a61b_ad1183bced27");
    var esStatement2 = createStatement2.build();

    es.createStatement(esStatement)
      .then(es.createStatement(esStatement2))
      .then(es.refreshIndex())
      .then(done());
  });

  this.Given(/^I have published a quality statement annotated with vocabulary term 2$/, function (done) {
    var createStatement = new this.createStatement();
    createStatement.addqsidentifier("1");
    createStatement.addstidentifier("3");
    createStatement.addserviceArea("https://nice.org.uk/ontologies/servicearea/81bc76de_3bdd_48bc_ac3c_3381f45875a4");
    var esStatement = createStatement.build();

    es.createStatement(esStatement)
      .then(es.refreshIndex())
      .then(done());

  });

  this.Given(/^I have published quality statements annotated with multiple vocabulary terms$/, function (done) {

    var createStatement = new this.createStatement();
    createStatement.addqsidentifier("2");
    createStatement.addstidentifier("1");
    createStatement.addserviceArea("https://nice.org.uk/ontologies/servicearea/41def0db_ac59_4e68_a61b_ad1183bced27");
    createStatement.addSetting("https://nice.org.uk/ontologies/setting/e517e9af_3c12_4f8b_a320_9323dfdf2510");
    var esStatement = createStatement.build();

    var createStatement2 = new this.createStatement();
    createStatement2.addqsidentifier("2");
    createStatement2.addstidentifier("2");
    createStatement2.addserviceArea("https://nice.org.uk/ontologies/servicearea/41def0db_ac59_4e68_a61b_ad1183bced27");
    createStatement2.addSetting("https://nice.org.uk/ontologies/setting/e517e9af_3c12_4f8b_a320_9323dfdf2510");
    var esStatement2 = createStatement2.build();

    var createStatement3 = new this.createStatement();
    createStatement3.addqsidentifier("2");
    createStatement3.addstidentifier("3");
    createStatement3.addserviceArea("https://nice.org.uk/ontologies/servicearea/41def0db_ac59_4e68_a61b_ad1183bced27");
    createStatement3.addSetting("https://nice.org.uk/ontologies/setting/e517e9af_3c12_4f8b_a320_9323dfdf2510");
    var esStatement3 = createStatement3.build();

    es.createStatement(esStatement)
      .then(es.createStatement(esStatement2))
      .then(es.createStatement(esStatement3))
      .then(es.refreshIndex())
      .then(done());
  });

this.Given(/^I have published some Quality Statements with different Standard and Statement numbers$/, function (done) {

    var createStatement = new this.createStatement();
    createStatement.addqsidentifier("4");
    createStatement.addstidentifier("4");
    createStatement.addserviceArea("https://nice.org.uk/ontologies/servicearea/41def0db_ac59_4e68_a61b_ad1183bced27");
    createStatement.addAbstract("Third");
    var esStatement = createStatement.build();

    var createStatement2 = new this.createStatement();
    createStatement2.addqsidentifier("21");
    createStatement2.addstidentifier("4");
    createStatement2.addserviceArea("https://nice.org.uk/ontologies/servicearea/41def0db_ac59_4e68_a61b_ad1183bced27");
    createStatement2.addAbstract("First");
    var esStatement2 = createStatement2.build();

    var createStatement3 = new this.createStatement();
    createStatement3.addqsidentifier("21");
    createStatement3.addstidentifier("21");
    createStatement3.addserviceArea("https://nice.org.uk/ontologies/servicearea/41def0db_ac59_4e68_a61b_ad1183bced27");
    createStatement3.addAbstract("Second");
    var esStatement3 = createStatement3.build();

    es.createStatement(esStatement)
      .then(es.createStatement(esStatement2))
      .then(es.createStatement(esStatement3))
      .then(es.refreshIndex())
      .then(done());
  });

  this.When(/^I visit the statement finder homepage$/, function (done) {
    var url = this.statementFinderUrl;
    return this.driver.sleep(1000)  // Had to wait here, think it's elastic not being ready yet ERE BE DRAGINS!!
                  .then(this.visit(url))
                  .then(done());
  });

  this.When(/^I refresh the index$/, function (done) {
    return es.refreshIndex().then(done());
  });

  this.When(/^I select the vocabulary Service Area$/, function () {
    var vocab = this.driver.findElement(By.xpath('//h4[contains(text(),"Service area")]'));
    vocab.click();
  });

  this.When(/^I select the vocabulary Setting$/, function () {
    var vocab = this.driver.findElement(By.xpath('//h4[contains(text(),"Setting")]'));
    vocab.click();
  });

  this.When(/^I select this single vocabulary term from the Service Area filters$/, function (done) {
    var checkbox1 = this.driver.findElement(By.xpath('//label[contains(text(),"Community health care")]'));
    var condition = until.elementIsVisible(checkbox1, 5000);
    this.driver.wait(condition, 25000).then(function() {
      checkbox1.click();
      done();
    });
  });

  this.When(/^I select this single vocabulary term from the Setting filters$/, function (done) {
    var checkbox1 = this.driver.findElement(By.xpath('//label[contains(text(),"Community")]'));
    var condition = until.elementIsVisible(checkbox1, 5000);
    this.driver.wait(condition, 25000).then(function() {
      checkbox1.click();
      done();
    });
  });

  this.When(/^I select multiple vocabulary terms from the Service Area filters$/, function (done) {
    var checkbox1 = this.driver.findElement(By.xpath('//label[contains(text(),"Community health care")]'));
    var checkbox2 = this.driver.findElement(By.xpath('//label[contains(text(),"Critical care")]'));
    var condition = until.elementIsVisible(checkbox1, checkbox2, 5000);
    this.driver.wait(condition, 25000).then(function() {
      checkbox1.click();
        }).then(function() {
            checkbox2.click();
            done();
    });
  });

  this.When(/^I perform a search$/, function () {
    this.driver.findElement(By.className('filters')).then(function(form) {
      form.submit();
    });
  });

  this.Then(/^I should see the total count as "([^"]*)"$/, function (text, done) {
    var counterTextElement = this.driver.findElement(By.className('counter'));
    counterTextElement.then(function(element) {
      element.getText().then(function(innerText) {
        innerText.should.equal(text);
        done();
      });
    });
  });

  this.Then(/^I should see the quality statements that are annotated with that single term "([^"]*)"$/, function (text,done) {
  var counterTextElement = this.driver.findElement(By.className('counter'));
  counterTextElement.then(function(element) {
      element.getText().then(function(innerText) {
        innerText.should.equal(text);
        done();
      });
    });
  });

  this.Then(/^I should see the results ordered by Standard number then Statement number$/, function (callback) {
    var expectedText = ["First", "Second", "Third"];

    this.driver.findElements(By.className("abstract")).then(function(elements) {
        elements.forEach(function (element, index) {
            element.getText().then(function(text){
               expectedText[index].should.equal(text);
            });
        });
    });
    callback();
    });

};
