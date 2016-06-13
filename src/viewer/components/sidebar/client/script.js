  var sidebar = (function () {
 return {
     getSearchLocation : function () {
         return document.location.search;  
     },
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

(function(){
 
    function setupAccordionEvents(){

        addEvent( document.getElementById( 'filters' ), 'click', function(e) {
            if (e.target && e.target.className && ~e.target.className.indexOf('accordion-trigger')) {
                toggleAccordionState.call( e.target, e );
            }
        });

        function toggleAccordionState( e ) {
            var className = (this.className || '').split(' ');
            if ( ~className.indexOf( 'open' ) ) {
                removeClass( this, 'open' );
                var siblings = getSiblings(this);
                for(var s = 0; s < siblings.length; s++){
                    addClass( siblings[s], 'closed' );
                }
                return;
            }

            addClass( this, 'open' );
            var siblings = getSiblings(this);
            for(var s = 0; s < siblings.length; s++){
                removeClass( siblings[s], 'closed' );
            }
        }

        function addClass( el, cls ) {
            var className = ( el.className || '' ).split( ' ' );
            if ( !~className.indexOf( cls ) ) {
                className.push( cls );
                el.className = className.join( ' ' );
            }
            return el;
        }

        function removeClass( el, cls ) {
            var className = ( el.className || '' ).split(' ');
            if ( ~className.indexOf( cls ) ) {
                className.splice( className.indexOf( cls ), 1 );
                el.className = className.join( ' ' );
            }
            return el;
        }

        function getChildren(n, skipMe){
            var r = [];
            n = n.firstChild;
            for ( ; n; n = n.nextSibling )
                if ( n.nodeType == 1 && n != skipMe)
                    r.push( n );
            return r;
        };

        function getSiblings(n) {
            return getChildren(n.parentNode, n);
        }

        function addEvent(elem, evnt, func) {
            if (elem.addEventListener)  // W3C DOM
                elem.addEventListener(evnt, func, false);
            else if (elem.attachEvent) { // IE DOM
                elem.attachEvent( "on"+ evnt, func );
            }
            else { // No much to do
                elem[ evnt ] = func;
            }
        }


    };
    setupAccordionEvents();
})();
