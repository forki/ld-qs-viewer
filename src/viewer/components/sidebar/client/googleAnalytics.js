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
    var value = 0;

    if (scrollIndex === 0 || heightOfResults === 0)  {
        return {text: percentageText, value: 0};
    }
    if (isNaN(scrollIndex) || isNaN(heightOfResults) || !scrollIndex || !heightOfResults) {
        return {text: "No value due to error", value: 0};
    }
        
    if (percentage < 25) {
        percentageText = "Baseline";
        value = 0;
    }
    if (percentage > 24 && percentage < 49) {
        percentageText = "25%";
        value = 1;
    }
    if (percentage > 49 && percentage < 74) {
        percentageText = "50%";
        value = 2;
    }
    if (percentage > 74 && percentage < 99) {
        percentageText = "75%";
        value = 3;
    }
    if (percentage > 99) {
        percentageText = "100%";
        value = 4;
    }
      return {text: percentageText, value: value};
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
