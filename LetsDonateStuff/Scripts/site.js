/* ---------------------------------------------------------------------- */
/*	Min-height for Footer to stick to the bottom of the page
/* ---------------------------------------------------------------------- */
(function () {

    // Set minimum height so footer will stay at the bottom of the window, even if there isn't enough content
    $('#content').css('min-height', $(window).outerHeight(true) - parseInt($('body').css('border-top-width')) - $('#header').outerHeight(true) - $('#footer').outerHeight(true) - $('#footer-bottom').outerHeight(true) + 11);

})();

/* TinyNavigation for small screen devices*//*! http://tinynav.viljamis.com v1.03 by @viljamis */
$(function () {

    // TinyNav.js 
    $('#main').tinyNav({
        //header: 'Navigation'
    });

});

// Script for making user voice feedback link functional
var uvOptions = {};
(function () {
    var uv = document.createElement('script'); uv.type = 'text/javascript'; uv.async = true;
    uv.src = ('https:' == document.location.protocol ? 'https://' : 'http://') + 'widget.uservoice.com/abcdefghijk.js';
    var s = document.getElementsByTagName('script')[0]; s.parentNode.insertBefore(uv, s);
})();