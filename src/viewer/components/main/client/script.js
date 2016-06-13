(function() {
    var qs ="";
  if (jQuery) {
      jQuery(document).ready(function() {
        //track result count 
        qs = document.location.search;
        if (qs !== ""){
            var results = jQuery(".result");
            googleAnalytics.sendResults(ga, results);
        }


    });

    var scrollTracker = [];
    jQuery(".card-list-wrapper").on('mousewheel DOMMouseScroll', function (event) {
        var percentage = 0;
        var eventValue = 0;
        var scrollIndex = parseInt(jQuery(".card-list-wrapper").scrollTop());
        var heightOfResults = parseInt(jQuery(".results").height() - 600);
        var percentageText = "Baseline";
        var scrollDepth = googleAnalytics.getScrollDepth(scrollIndex, heightOfResults);
        var qs = document.location.search;
        if (scrollDepth && !scrollTracker[scrollDepth.value] && qs !== "") {
            console.log(scrollDepth);
            googleAnalytics.sendScrollDepth(ga, scrollDepth.text, scrollDepth.value); 
            scrollTracker[scrollDepth.value] = true;
        }
    });
    jQuery("input:submit[value='Apply filters']").click(function(evt) {
      var qs = sidebar.getSearchLocation(); 
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
