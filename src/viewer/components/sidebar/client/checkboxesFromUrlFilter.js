  var sidebar = (function () {
 return {
   extractKeysAndValuesFromUrl : function (searchOptions) {
      searchOptions = searchOptions.slice(1); //take out the ? mark from the beginning
      return searchOptions.split(/=([^&#]*)/); 
   },
   extractValues : function(keysAndValues) {
     var values = [];
     for (var i=0;i<keysAndValues.length-1;i++){
       if ((i%2)!==0) {
           values.push(keysAndValues[i]);
       }
     }
     return values;
   },
   groupBy : function(arrayToGroup) {
     var grouped = [];
     var hist = {};

     arrayToGroup.forEach(function (a) { if (a in hist) hist[a] = a; else hist[a] = a; });

     for (var key in hist) {
       if (hist.hasOwnProperty(key)) {
            grouped.push(key);
         }
     }

     return grouped;
    },
   extractKeys : function(keysAndValues) {
     var keys = [];
     for (var i=0;i<keysAndValues.length-1;i++){
       if ((i+2)%2==0) {
         if (keysAndValues[i][0] === '&') {
           keys.push(keysAndValues[i].slice(1));
         } else {
           keys.push(keysAndValues[i]);
         }
       }
     }
     return keys;
   },
   escapeColon : function (stringWithColon){
     return stringWithColon.replace(":", "\\:");
   },
   selectCheckboxes : function (qs) {
     if (qs && qs!=="")  {
        var result = sidebar.extractKeysAndValuesFromUrl(qs);
        var keys = sidebar.extractKeys(result);
        var values = sidebar.extractValues(result);
        var uniqueKeys = sidebar.groupBy(keys);
        var uniqueValues = sidebar.groupBy(values);

        uniqueKeys.forEach(function (key) {
            $("#" + sidebar.escapeColon(decodeURIComponent(key.replace(/\+/g,'%20')))).click();
        });
        uniqueValues.forEach(function (value) {
          var selector ="input:checkbox[value='" + decodeURIComponent(value.replace(/\+/g, '%20')) + "']";
          if ($(selector + ":checked").length === 0) {
            $(selector).click();
          }
        });
     }
    }
 };

})();

