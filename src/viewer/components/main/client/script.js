(function() {
  if (jQuery) {
    jQuery("input:submit[value='Apply filters']").click(function() {
      ga('send', 'event', 'Filters', 'Applied', 'Adults');
      console.log(sidebar);
    });
    sidebar.selectCheckboxes(document.location.search);
  }
})(jQuery);
