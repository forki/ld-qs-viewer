(function() {
  if (jQuery) {
    jQuery(document).ready(function() {
      //track result count 
      var qs = document.location.search;
      if (qs !== ""){
        var results = jQuery(".result");
        googleAnalytics.sendResults(ga, results);
      }
      $(".card-list-wrapper").on('mousewheel DOMMouseScroll', function (event) {
          //console.log(event.originalEvent);
      });
      $.scrollDepth();

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

    sidebar.selectCheckboxes(document.location.search);

    //var Frequency = 10;
    //var _frequency = Frequency;
    //var _repentance = 100 / Frequency;
    //var _scrollMatrix = [];
    //for (ix = 0; ix < _repentance; ix++) {
        //_scrollMatrix[ix] = [_frequency, 'false'];
        //_frequency = Frequency + _frequency;
    //}
    //console.log(jQuery, jQuery("div.results"));

    //jQuery("div.results").scroll(function (e) {
      //console.log(e);
       //for (iz = 0; iz < _scrollMatrix.length; iz++) {
         //if ((jQuery(window).scrollTop() + jQuery(window).height() >= jQuery(document).height() * _scrollMatrix[iz][0] / 100)  && (_scrollMatrix[iz][1]== 'false')) {
           //console.log(_scrollMatrix[iz][0]);
           //_scrollMatrix[iz][1] = 'true';
             //ga('send', 'event', "Scroll depth", "Percetage of scroll", _scrollMatrix[iz][0]+'%');
         //}
       //}
    //});
  }
})(jQuery);
