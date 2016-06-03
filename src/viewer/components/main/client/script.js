(function() {
  if (jQuery) {
      jQuery(document).ready(function() {
      //track result count 
      var qs = document.location.search;
      if (qs !== ""){
        var results = jQuery(".result");
        googleAnalytics.sendResults(ga, results);
      }

      var scrollTracker = [];
      function getEventValue(percentage) {
        if (percentage < 25) {
          return 0;
        }
        if (percentage > 25 && percentage < 50) {
          return 1;
        }
        if (percentage > 50 && percentage < 75) {
          return 2;
        }
        if (percentage > 75 && percentage < 100) {
          return 3;
        }
        if (percentage > 100) {
          return 4;
        }
          return 0;
      }
      $(".card-list-wrapper").on('mousewheel DOMMouseScroll', function (event) {
          var percentage = 0;
          var eventValue = 0;
          var scrollIndex = parseInt(jQuery("card-list-wrapper").scrollTop());
          var heightOfResults = parseInt(jQuery(".results").height() - 600);
          var percentageText = "Baseline";

          percentage = (scrollIndex / heightOfResults) * 100;
          if (percentage < 25) {
            percentageText = "Baseline";
          }
          if (percentage > 25 && percentage < 50) {
            percentageText = "25%";
          }
          if (percentage > 51 && percentage < 75) {
            percentageText = "50%";
          }
          if (percentage > 75 && percentage < 100) {
            percentageText = "75%";
          }
          if (percentage > 100) {
            percentageText = "100%";
          }
          if (!scrollTracker[getEventValue(percentage)] && qs !== "") {
             googleAnalytics.sendScrollDepth(ga, percentageText, getEventValue(percentage));
             scrollTracker[getEventValue(percentage)] = true;
          }
      });

    });

    jQuery("input:submit[value='Apply filters']").click(function() {
      var qs = document.location.search;
      var result = sidebar.extractKeysAndValuesFromUrl(qs);
      var values = sidebar.extractValues(result);
      var uniqueValues = sidebar.groupBy(values);
      googleAnalytics.sendFilters(ga, uniqueValues);

    });

    jQuery("a[type='reset']").click(function() {
      googleAnalytics.sendClearFilters(ga);
    });

    jQuery("#old-standards").click(function() {
        googleAnalytics.sendOutboundLink(ga, "Previous QS NICE website");
    });
    jQuery("#mailFeedback").click(function() {
        googleAnalytics.sendOutboundLink(ga, "Feedback");
    });

    sidebar.selectCheckboxes(document.location.search);
  }
})(jQuery);
