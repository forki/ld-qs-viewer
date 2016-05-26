var googleAnalytics = (function googleAnalytics() {
  return {
    sendFilters : function send(ga, events) {
      for (var i=0; i<events.length; i++) {

          var eventObj = {
            category : "Filters",
            action : "Applied",
            label : events[i]
          };

          ga('send', 'event', eventObj.category, eventObj.action, eventObj.label);
        }
      },
    sendClearFilters : function send(ga) {
      ga('send', 'event', "Filters", "Cleared");
    }
  };
})();
