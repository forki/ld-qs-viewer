$(function() {
    $('.filter-checkbox').change(function(e) {
        ShowInferenceInTree(this);
    });

    function anyChildrenSelected (container) {
      return ($(container).find('input[type="checkbox"]').filter(function (ix, itm) {
        return $(itm).prop("checked");
      }).length > 0);
    }

    function highlight(container, upTree) {
      let children = container.find('input[type="checkbox"]');
      
      children.each(function(index, item){
        let thisChecked = $(item).prop("checked");
        let highlight = true;

        if (upTree) {
          let thisContainer = $(item).parent();
          highlight = anyChildrenSelected(thisContainer);
        }

        if (thisChecked) {
          $(item).prop( {indeterminate: false});
        } else {
          $(item).prop( {indeterminate: highlight});
        }
      });
    }

    function ShowInferenceInTree (context) {
      var container = $(context).parent();
      
      while (container.parent().parent("li").length > 0){
        container = container.parent().parent("li");
      }
      
      highlight(container, true);
      
      $(container).find('input[type="checkbox"]').filter(function (ix, itm) {
        return $(itm).prop("checked");
      }).each(function(index, item){
        highlight($(item).parent(), false);
      })

    }

});
