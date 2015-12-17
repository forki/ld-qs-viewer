(function(){
// Determines whether js is enabled or not
    function isIE () {
        var myNav = navigator.userAgent.toLowerCase();
        return (myNav.indexOf('msie') != -1) ? parseInt(myNav.split('msie')[1]) : false;
    }

    function detectJs(){
        //classList no supported by IE...
        var nojs = document.getElementsByClassName('no-js');
        while (nojs.length > 0) {
            nojs[0].className = nojs[0].className.replace(/ *\bno-js\b/g, " js");
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
