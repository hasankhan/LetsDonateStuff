/*! http://responsiveslides.com v1.25 by @viljamis */
(function (b, C, u) {
    b.fn.responsiveSlides = function (e) {
        var c = b.extend({ auto: !0, speed: 1E3, timeout: 4E3, pager: !1, nav: !1, prevText: "Previous", nextText: "Next", maxwidth: "", controls: "", namespace: "rslides" }, e); return this.each(function () {
            u++; var d = b(this), l, o, p, v, m, k = 0, f = d.children(), w = f.size(), x = parseFloat(c.speed), q = parseFloat(c.maxwidth), g = c.namespace, h = g + u, i = g + "_nav " + h + "_nav", r = g + "_here", j = h + "_on", y = h + "_s", n = b("<ul class='" + g + "_tabs " + h + "_tabs' />"), z = { "float": "left", position: "relative" }, D = {
                "float": "none",
                position: "absolute"
            }, s = function (a) { d.trigger(g + "-before"); f.stop().fadeOut(x, function () { b(this).removeClass(j).css(D) }).eq(a).fadeIn(x, function () { b(this).addClass(j).css(z).trigger(g + "-after"); k = a }) }; f.each(function (a) { this.id = y + a }); d.addClass(g + " " + h); e && e.maxwidth && d.css("max-width", q); f.hide().eq(0).addClass(j).css(z).show(); if (1 < f.size()) {
                if (!0 === c.pager) {
                    var t = []; f.each(function (a) { a += 1; t += "<li><a href='#' class='" + y + a + "'>" + a + "</a></li>" }); n.append(t); m = n.find("a"); e.controls ? b(c.controls).append(n) :
                    d.after(n); l = function (a) { m.closest("li").removeClass(r).eq(a).addClass(r) }
                } !0 === c.auto && (o = function () { v = setInterval(function () { var a = k + 1 < w ? k + 1 : 0; !0 === c.pager && l(a); s(a) }, parseFloat(c.timeout)) }, o()); p = function () { if (c.auto === true) { clearInterval(v); o() } }; !0 === c.pager && m.bind("click", function (a) { a.preventDefault(); p(); a = m.index(this); if (!(k === a || b("." + j + ":animated").length)) { l(a); s(a) } }).eq(0).closest("li").addClass(r); if (!0 === c.nav) {
                    i = "<a href='#' class='" + i + " prev'>" + c.prevText + "</a><a href='#' class='" +
                    i + " next'>" + c.nextText + "</a>"; e.controls ? b(c.controls).append(i) : d.after(i); var i = b("." + h + "_nav"), A = b("." + h + "_nav.prev"); i.bind("click", function (a) { a.preventDefault(); if (!b("." + j + ":animated").length) { var d = f.index(b("." + j)), a = d - 1, d = d + 1 < w ? k + 1 : 0; s(b(this)[0] === A[0] ? a : d); c.pager === true && l(b(this)[0] === A[0] ? a : d); p() } })
                }
            } if ("undefined" === typeof document.body.style.maxWidth && e && e.maxwidth) { var B = function () { d.css("width", "100%"); d.width() > q && d.css("width", q) }; B(); b(C).bind("resize", function () { B() }) }
        })
    }
})(jQuery,
this, 0);

/* end footer script */

/*The carousel*/

$(".rslides").responsiveSlides({
    auto: true,             // Boolean: Animate automatically, true or false
    speed: 900,            // Integer: Speed of the transition, in milliseconds
    timeout: 4000,          // Integer: Time between slide transitions, in milliseconds
    pager: false,           // Boolean: Show pager, true or false
    nav: false,             // Boolean: Show navigation, true or false
    prevText: "Previous",   // String: Text for the "previous" button
    nextText: "Next",       // String: Text for the "next" button
    maxwidth: "980",       // Integer: Max-width of the slideshow, in pixels
    controls: "",           // Selector: Where controls should be appended to, default is after the 'ul'
    namespace: "rslides"    // String: change the default namespace used
});

// Modal Window for Country Choice
$('#mymodal').modal('hide')
