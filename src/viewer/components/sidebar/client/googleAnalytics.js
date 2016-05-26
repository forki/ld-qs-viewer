var googleAnalytics = (function googleAnalytics() {
  return {
    sendFilters : function send(ga, events) {
        for (var i=0; i<events.length; i++) {

          var eventObj = {
            category : "Filters",
            action : "Applied",
            label : events[i]
          };

          ga.send(eventObj.category, eventObj.action, eventObj.label);
        }
      }
  };

})();
