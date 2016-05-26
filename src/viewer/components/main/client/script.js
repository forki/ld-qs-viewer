(function() {
  if (jQuery) {
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

    sidebar.selectCheckboxes(document.location.search);
  }
})(jQuery);
