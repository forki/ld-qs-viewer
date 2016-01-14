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
