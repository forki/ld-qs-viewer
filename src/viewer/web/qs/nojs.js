(function(){
// Determines whether js is enabled or not
    function isIE () {
        var myNav = navigator.userAgent.toLowerCase();
        return (myNav.indexOf('msie') != -1) ? parseInt(myNav.split('msie')[1]) : false;
    }

    function detectJs(){
        var nojs = document.getElementsByClassName('no-js');
        while (nojs.length > 0) {
            nojs[0].classList.toggle('no-js');
        }
    };

    function notIEandHasJavascript()
    {
        if(!isIE() || !isIE() < 9)
        {
            detectJs();
        }
    }
    notIEandHasJavascript();
})();
