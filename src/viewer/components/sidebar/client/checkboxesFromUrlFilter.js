module.exports = function sidebar() {
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
   reduce : function(arrayToGroup) {
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
   }
 };
};
