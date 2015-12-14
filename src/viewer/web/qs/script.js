(function(){
    <!--Doorbell User feedback module-->
    window.doorbellOptions = {
        strings: {
            'feedback-button-text': 'Leave feedback',

            'title': 'Your feedback',
            'feedback-textarea-placeholder': '',
            'feedback-label': 'Your comments, suggestions and issues (if applicable)...',
            'email-input-placeholder': '',
            'email-label': 'Your email address',
            'attach-a-screenshot': 'Include screenshot',
            'submit-button-text': 'Submit feedback',
            'add-attachments-label': 'Add an attachment'
        },
        appKey: '0Kr8cH0IhEL3L3W6mxNjQIKFxNP1Ic78RsDPYmxmle77ACqDCOq34CIWcVVnDpDp'
    };


    (function(d, t) {var g = d.createElement(t);g.id = 'doorbellScript';g.type = 'text/javascript';g.async = true;g.src = 'https://embed.doorbell.io/button/2274?t='+(new Date().getTime());(d.getElementsByTagName('head')[0]||d.getElementsByTagName('body')[0]).appendChild(g);}(document, 'script'));

    function ShowDoorbellForm() {
        doorbell.show();
    }

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
