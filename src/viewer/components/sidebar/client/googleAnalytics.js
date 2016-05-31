var googleAnalytics = (function googleAnalytics() {
  function figureOut(resultCount) {
    if (resultCount > 50 && resultCount < 101) {
      return "51 - 100 results";
    }
    if (resultCount > 100 && resultCount < 201) {
      return "101 - 200 results";
    }
    if (resultCount > 200 && resultCount < 301) {
      return "201 - 300 results";
    }
    if (resultCount > 300 && resultCount < 401) {
      return "301 - 400 results";
    }
    if (resultCount > 400 && resultCount < 501) {
      return "401 - 500 results";
    }
    if (resultCount > 500) {
      return "500+ results";
    }
    return "1 - 50 results";
  }
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
    },
    sendResults : function send(ga, results) {
      console.log(results.length);
      if (results && results.length === 0) {
        ga('send', 'event', "Results provided", "Without results", "0 results");
      } else{
        var resultCount = results.length;
        var resultCountText = figureOut(resultCount);

        ga('send', 'event', "Results provided", "With results", resultCountText);
      };
    }
  };
})();
