var googleAnalytics = (function googleAnalytics() {
  function figureOut(resultCount) {
    if (resultCount > 50 && resultCount < 101) {
      return '51 - 100 results';
    }
    if (resultCount > 100 && resultCount < 201) {
      return '101 - 200 results';
    }
    if (resultCount > 200 && resultCount < 301) {
      return '201 - 300 results';
    }
    if (resultCount > 300 && resultCount < 401) {
      return '301 - 400 results';
    }
    if (resultCount > 400 && resultCount < 501) {
      return '401 - 500 results';
    }
    if (resultCount > 500) {
      return '500+ results';
    }
    return '1 - 50 results';
  }

  function figureOutPercentageText(scrollIndex, heightOfResults) {
    var percentage = (scrollIndex / heightOfResults) * 100;
    var percentageText = "Baseline";
    if (scrollIndex === 0 || heightOfResults === 0)  {
        return {text: percentageText};
    }
    if (isNaN(scrollIndex) || isNaN(heightOfResults) || !scrollIndex || !heightOfResults) {
        return {text: "No value due to error"};
    }
        
    if (percentage < 25) {
        percentageText = "Baseline";
    }
    if (percentage > 25 && percentage < 49) {
        percentageText = "25%";
    }
    if (percentage > 49 && percentage < 74) {
        percentageText = "50%";
    }
    if (percentage > 74 && percentage < 99) {
        percentageText = "75%";
    }
    if (percentage > 99) {
        percentageText = "100%";
    }
    return {text: percentageText};
  }


  return {
    sendFilters : function send(ga, events) {
        var eventObj = {
            category : 'Filters',
            action : 'Applied',
            label : "" 
        };

        for (var i=0; i<events.length; i++) {
            eventObj.label += events[i].split("%23")[1] + ",";
        }
        ga('send', 'event', eventObj.category, eventObj.action, eventObj.label);
      },
    sendClearFilters : function send(ga) {
      ga('send', 'event', 'Filters', 'Cleared');
    },
    sendResults : function send(ga, results) {
      if (results && results.length === 0) {
        ga('send', 'event', 'Results provided', 'Without results', '0 results');
      } else{
        var resultCount = results.length;
        var resultCountText = figureOut(resultCount);

        ga('send', 'event', 'Results provided', 'With results', resultCountText);
      };
    },
    sendScrollDepth : function send(ga, label, value){
      ga('send', 'event', 'Scroll depth', 'Percentage of scroll', label, value);
    },
    sendOutboundLink : function send (ga, label) {
      ga('send', 'event', 'Outbound links', 'clicked', label);
    },
    getScrollDepth : function getPercentageText(scrollIndex, pageHeight) {
      return figureOutPercentageText(scrollIndex, pageHeight);
    }
  };
  })();
