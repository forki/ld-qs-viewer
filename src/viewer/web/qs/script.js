<!-- Classie js -->
!function(s){"use strict";function e(s){return new RegExp("(^|\\s+)"+s+"(\\s+|$)")}function n(s,e){var n=t(s,e)?c:a;n(s,e)}var t,a,c;"classList"in document.documentElement?(t=function(s,e){return s.classList.contains(e)},a=function(s,e){s.classList.add(e)},c=function(s,e){s.classList.remove(e)}):(t=function(s,n){return e(n).test(s.className)},a=function(s,e){t(s,e)||(s.className=s.className+" "+e)},c=function(s,n){s.className=s.className.replace(e(n)," ")});var o={hasClass:t,addClass:a,removeClass:c,toggleClass:n,has:t,add:a,remove:c,toggle:n};"function"==typeof define&&define.amd?define(o):"object"==typeof exports?module.exports=o:s.classie=o}(window);

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

<!-- Responsive menu -->
var menuLeft = document.getElementById('filters');
var showLeft = document.getElementById('toggle-filters');
var body = document.body;

showLeft.addEventListener('click', function(){
    classie.toggle(this, 'active');
    classie.toggle(body, 'nav-menu-open');
    classie.toggle(menuLeft, 'filters-open');

//    this.className = this.className + ' active';
//    body.className = body.className + ' nav-menu-open';
//    menuLeft.className = menuLeft.className + ' filters-open';
});

function setupAccordionEvents(){

    function getChildren(n, skipMe){
        var r = [];
        for ( ; n; n = n.nextSibling )
            if ( n.nodeType == 1 && n != skipMe)
                r.push( n );
        return r;
    };

    function getSiblings(n) {
        return getChildren(n.parentNode.firstChild, n);
    }

    var accordians = document.getElementsByClassName("accordion-trigger");
    for (var i = 0; i < accordians.length; i++) {
        accordians[i].addEventListener('click', function() {
            this.classList.toggle('open');
            var siblings = getSiblings(this);
            for(var s = 0; s < siblings.length; s++){
                siblings[s].classList.toggle('open');
            }
        });
    }

};

setupAccordionEvents();
