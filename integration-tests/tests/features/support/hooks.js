var es = require('./elasticsearch.js');

module.exports = function () {
  this.Before(function() {
    this.driver.manage().window().setSize(1280, 1024);
     es.indexExists().then(function (exists) {
      if (exists) {
        return es.deleteIndex();
      }
    }).then(es.initIndex())
      .catch(function(err) {
      console.log("ERRROR INITIALISING", err) ;
      });
  });
  // this.After(function() {
  //   return es.deleteIndex();
  // });
  this.After(function() {
    return this.driver.quit();
  });
};
