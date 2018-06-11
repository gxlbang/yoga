var slide = (function() {
    var a = function(c) {
        return new b(c)
    };
    function b(c) {
        this.elem = c;
        this.oBox = document.querySelector(c);
        this.aLi = document.querySelectorAll(c + " [data-ul-child=child]");
        this.oUl = document.querySelector(c + " [data-slide-ul=firstUl]");
        this.now = 0;
        this.leftNum = 0;
        this.on0ff = false
    }
    b.prototype = {
        init: function(c) {
            var c = c || {},
            e = this.aLi;
            this.defaults = {
                startIndex: 0,
                loop: false,
                smallBtn: false,
                number: false,
                laseMoveFn: false,
                location: false,
                preDef: "lnr",
                autoPlay: false,
                autoHeight: false,
                preFn: null,
                lastImgSlider: false,
                playTime: 6000,
                mpingEvent: "",
                wareId: "",
                leftSlideNumCb: false
            };
            b.extend(this.defaults, c);
            this.now = this.defaults.startIndex;
            if (this.defaults.smallBtn) {
                this.oSmallBtn = document.querySelector(this.elem + ' [data-small-btn="smallbtn"]');
                this.oSmallBtn.innerHTML = this.addSmallBtn();
                this.btns = document.querySelectorAll(this.elem + ' [data-ol-btn="btn"]');
                for (var d = 0; d < this.btns.length; d++) {
                    this.btns[d].className = ""
                }
                this.btns[b.getNow(this.now, e.length)].className = "active"
            }
            if (this.defaults.number) {
                this.slideNub = document.getElementById("slide-nub");
                this.slideSum = document.getElementById("slide-sum");
                this.slideNub.innerHTML = this.now + 1;
                this.slideSum.innerHTML = this.aLi.length
            }
            if (this.aLi.length == 2) {
                if (this.defaults.loop) {
                    this.oUl.innerHTML += this.oUl.innerHTML
                }
                this.aLi = document.querySelectorAll(this.elem + " [data-ul-child=child]");
                this.need = true
            }
            this.liInit();
            this.bind();
            if (this.aLi.length < 2) {
                if (this.oSmallBtn) {
                    this.oSmallBtn.style.display = "none"
                }
                this.defaults.autoPlay = false;
                this.defaults.loop = false
            }
            if (this.defaults.autoPlay) {
                this.pause();
                this.play()
            }
        },
        bind: function() {
            var f = this.oBox,
            e = b._device();
            if (!e.hasTouch) {
                f.style.cursor = "pointer";
                f.ondragstart = function(g) {
                    if (g) {
                        return false
                    }
                    return true
                }
            }
            var d = "onorientationchange" in window;
            var c = d ? "orientationchange": "resize";
            window.addEventListener(c, this);
            if (this.aLi.length < 2) {
                if (this.oSmallBtn) {
                    this.oSmallBtn.style.display = "none"
                }
            }
            f.addEventListener(e.startEvt, this);
            window.addEventListener("touchcancel", this);
            if (navigator.userAgent.indexOf("baidubrowser")) {
                window.addEventListener("focusin", this);
                window.addEventListener("focusout", this)
            } else {
                window.addEventListener("blur", this);
                window.addEventListener("focus", this)
            }
        },
        liInit: function() {
            var d = this.aLi,
            f = d.length,
            m = this.oUl,
            l = this.oBox.offsetWidth,
            e = this.now,
            k = this;
            if (this.defaults.preFn) {
                this.defaults.preFn()
            }
            m.style.width = l * f + "px";
            for (var h = 0; h < f; h++) {
                b.setStyle(d[h], {
                    WebkitTransition: "all 0ms ease",
                    transition: "all 0ms ease",
                    height: "auto"
                })
            }
            if (this.defaults.autoHeight) {
                var c = this.oBox.offsetWidth;
                if (c >= 640) {
                    c = 640
                } else {
                    if (c <= 320) {
                        c = 320
                    }
                }
                for (var h = 0; h < f; h++) {
                    d[h].style.width = c + "px"
                }
                var j = d[0].getElementsByTagName("img")[0];
                if (j) {
                    var g = new Image();
                    g.onload = function() {
                        k.oBox.style.height = d[0].offsetHeight + "px";
                        k.oUl.style.height = d[0].offsetHeight + "px";
                        if (k.oBox.style.height.replace("px", "") >= (document.documentElement.clientHeight - 100)) {
                            k.oBox.parentNode.style.top = "50px";
                            k.oBox.parentNode.style.marginTop = 0
                        } else {
                            k.oBox.parentNode.style.top = "50%";
                            k.oBox.parentNode.style.marginTop = -d[0].offsetHeight / 2 + "px"
                        }
                        for (var n = 0; n < d.length; n++) {
                            d[n].style.height = d[0].offsetHeight + "px"
                        }
                    };
                    g.src = j.src
                } else {
                    this.oBox.style.height = d[0].offsetHeight + "px"
                }
            }
            if (this.defaults.loop) {
                for (var h = 0; h < f; h++) {
                    b.setStyle(d[h], {
                        position: "absolute",
                        left: 0,
                        top: 0
                    });
                    if (h == b.getNow(e, f)) {
                        b.setStyle(d[h], {
                            WebkitTransform: "translate3d(" + 0 + "px, 0px, 0px)",
                            transform: "translate3d(" + 0 + "px, 0px, 0px)",
                            zIndex: 10
                        })
                    } else {
                        if (h == b.getPre(e, f)) {
                            b.setStyle(d[h], {
                                WebkitTransform: "translate3d(" + -l + "px, 0px, 0px)",
                                transform: "translate3d(" + -l + "px, 0px, 0px)",
                                zIndex: 10
                            })
                        } else {
                            if (h == b.getNext(e, f)) {
                                b.setStyle(d[h], {
                                    WebkitTransform: "translate3d(" + l + "px, 0px, 0px)",
                                    transform: "translate3d(" + l + "px, 0px, 0px)",
                                    zIndex: 10
                                })
                            } else {
                                b.setStyle(d[h], {
                                    WebkitTransform: "translate3d(" + 0 + "px, 0px, 0px)",
                                    transform: "translate3d(" + 0 + "px, 0px, 0px)",
                                    zIndex: 9
                                })
                            }
                        }
                    }
                }
            } else {
                for (var h = 0; h < f; h++) {
                    b.setStyle(d[h], {
                        WebkitTransform: "translate3d(" + e * -l + "px, 0px, 0px)",
                        transform: "translate3d(" + e * -l + "px, 0px, 0px)"
                    })
                }
            }
        },
        handleEvent: function(d) {
            var c = b._device(),
            e = this.oBox;
            switch (d.type) {
            case c.startEvt:
                if (this.defaults.autoPlay) {
                    this.pause()
                }
                this.startHandler(d);
                break;
            case c.moveEvt:
                if (this.defaults.autoPlay) {
                    this.pause()
                }
                this.moveHandler(d);
                break;
            case c.endEvt:
                if (this.defaults.autoPlay) {
                    this.pause();
                    this.play()
                }
                this.endHandler(d);
                break;
            case "touchcancel":
                if (this.defaults.autoPlay) {
                    this.pause();
                    this.play()
                }
                this.endHandler(d);
                break;
            case "orientationchange":
                this.orientationchangeHandler();
                break;
            case "resize":
                this.orientationchangeHandler();
                break;
            case "focus":
                if (this.defaults.autoPlay) {
                    this.pause();
                    this.play()
                }
                break;
            case "blur":
                if (this.defaults.autoPlay) {
                    this.pause()
                }
                break;
            case "focusin":
                if (this.defaults.autoPlay) {
                    this.pause();
                    this.play()
                }
                break;
            case "focusout":
                if (this.defaults.autoPlay) {
                    this.pause()
                }
                break
            }
        },
        startHandler: function(e) {
            this.on0ff = true;
            var d = b._device(),
            f = d.hasTouch,
            h = this.oBox,
            c = this.now,
            g = this.aLi;
            h.addEventListener(d.moveEvt, this);
            h.addEventListener(d.endEvt, this);
            this.downTime = Date.now();
            this.downX = f ? e.targetTouches[0].pageX: e.clientX - h.offsetLeft;
            this.downY = f ? e.targetTouches[0].pageY: e.clientY - h.offsetTop;
            this.startT = b.getTranX(g[b.getNow(c, g.length)]);
            this.startNowT = b.getTranX(g[b.getNow(c, g.length)]);
            this.startPreT = b.getTranX(g[b.getPre(c, g.length)]);
            this.startNextT = b.getTranX(g[b.getNext(c, g.length)]);
            b.stopPropagation(e)
        },
        moveHandler: function(o) {
            var l = this.oBox,
            e = b._device();
            if (this.on0ff) {
                var m = e.hasTouch;
                var h = m ? o.targetTouches[0].pageX: o.clientX - l.offsetLeft;
                var g = m ? o.targetTouches[0].pageY: o.clientY - l.offsetTop;
                var c = this.aLi,
                f = c.length,
                d = this.now,
                q = c[0].offsetWidth;
                if (this.defaults.preDef == "all") {
                    b.stopDefault(o)
                }
                for (var k = 0; k < f; k++) {
                    b.setStyle(c[k], {
                        WebkitTransition: "all 0ms ease",
                        transition: "all 0ms ease"
                    })
                }
                if (Math.abs(h - this.downX) < Math.abs(g - this.downY)) {
                    if (this.defaults.preDef == "tnd" && this.defaults.preDef != "all") {
                        b.stopDefault(o)
                    }
                } else {
                    if (Math.abs(h - this.downX) > 10) {
                        if (this.defaults.preDef == "lnr" && this.defaults.preDef != "all") {
                            b.stopDefault(o)
                        }
                        if (this.defaults.loop) {
                            j = (this.startNowT + h - this.downX).toFixed(4);
                            preT = (this.startPreT + h - this.downX).toFixed(4);
                            nextT = (this.startNextT + h - this.downX).toFixed(4);
                            b.move(c[b.getNow(d, f)], j, 10);
                            b.move(c[b.getPre(d, f)], preT, 10);
                            b.move(c[b.getNext(d, f)], nextT, 10)
                        } else {
                            var j = b.getTranX(c[d]);
                            if (j > 0) {
                                var n = ((this.startT + h - this.downX) / 3).toFixed(4);
                                for (var k = 0; k < f; k++) {
                                    b.move(c[k], n)
                                }
                            } else {
                                if (Math.abs(j) >= Math.abs((f - 1) * q)) {
                                    var n = (this.startT + (h - this.downX) / 3).toFixed(4);
                                    for (var k = 0; k < f; k++) {
                                        b.move(c[k], n)
                                    }
                                    if (this.defaults.laseMoveFn && typeof this.defaults.laseMoveFn == "function") {
                                        var p = (n - this.startT).toFixed(4);
                                        this.defaults.laseMoveFn(p)
                                    }
                                } else {
                                    var n = (this.startT + h - this.downX).toFixed(4);
                                    for (var k = 0; k < f; k++) {
                                        b.move(c[k], n)
                                    }
                                }
                            }
                        }
                    }
                }
            } else {
                l.removeEventListener(e.moveEvt, this);
                l.removeEventListener(e.endEvt, this)
            }
            b.stopPropagation(o)
        },
        endHandler: function(j) {
            j.stopPropagation();
            this.on0ff = false;
            var f = Date.now(),
            e = b._device(),
            i = e.hasTouch,
            h = this.oBox,
            m = i ? j.changedTouches[0].pageX: j.clientX - h.offsetLeft,
            l = i ? j.changedTouches[0].pageY: j.clientY - h.offsetTop,
            c = this.aLi,
            k = c[0].offsetWidth,
            g = this,
            d = b.getTranX(c[b.getNow(this.now, c.length)]);
            if (m - this.downX < 30 && m - this.downX >= 0 && Math.abs(l - this.downY) < 30) {
                this.tab(d, "+=");
                return "click"
            } else {
                if (m - this.downX > -30 && m - this.downX <= 0 && Math.abs(l - this.downY) < 30) {
                    this.tab(d, "-=");
                    return "click"
                } else {
                    if (Math.abs(l - this.downY) - Math.abs(m - this.downX) > 30 && m - this.downX < 0) {
                        this.tab(d, "-=");
                        return
                    }
                    if (Math.abs(l - this.downY) - Math.abs(m - this.downX) > 30 && m - this.downX > 0) {
                        this.tab(d, "+=");
                        return
                    }
                    if (m < this.downX) {
                        if (this.downX - m > k / 3 || f - this.downTime < 200) {
                            this.now++;
                            this.tab(d, "++");
                            if (this.defaults.leftSlideNumCb && typeof this.defaults.leftSlideNumCb == "function") {
                                g.leftNum++;
                                this.defaults.leftSlideNumCb(g.leftNum)
                            }
                            pingClickWithLevel(this.defaults.mpingEvent, "", "", this.defaults.wareId, "");
                            return "left"
                        } else {
                            this.tab(d, "+=");
                            return "stay"
                        }
                    } else {
                        if (m - this.downX > k / 3 || f - this.downTime < 200) {
                            this.now--;
                            this.tab(d, "--");
                            pingClickWithLevel(this.defaults.mpingEvent, "", "", this.defaults.wareId, "");
                            return "right"
                        } else {
                            this.tab(d, "-=");
                            return "stay"
                        }
                    }
                }
            }
            b.stopPropagation(j);
            h.removeEventListener(e.moveEvt, this);
            h.removeEventListener(e.endEvt, this)
        },
        tab: function(e, l, f) {
            var c = this.aLi,
            k = c.length,
            r = c[0].offsetWidth,
            q = this.oBox,
            g = b._device(),
            p = this,
            d = this.now;
            if (this.defaults.loop) {
                if (d < 0) {
                    d = k - 1;
                    this.now = k - 1
                }
                for (var o = 0; o < k; o++) {
                    if (o == b.getPre(d, k)) {
                        var h;
                        switch (l) {
                        case "++":
                            h = 300;
                            break;
                        case "--":
                            h = 0;
                            break;
                        case "+=":
                            h = 0;
                            break;
                        case "-=":
                            h = 300;
                            break;
                        default:
                            break
                        }
                        b.setStyle(c[b.getPre(d, k)], {
                            WebkitTransform: "translate3d(" + -r + "px, 0px, 0px)",
                            transform: "translate3d(" + -r + "px, 0px, 0px)",
                            zIndex: 10,
                            WebkitTransition: "all " + h + "ms ease",
                            transition: "all " + h + "ms ease"
                        })
                    } else {
                        if (o == b.getNow(d, k)) {
                            b.setStyle(c[b.getNow(d, k)], {
                                WebkitTransform: "translate3d(" + 0 + "px, 0px, 0px)",
                                transform: "translate3d(" + 0 + "px, 0px, 0px)",
                                zIndex: 10,
                                WebkitTransition: "all " + 300 + "ms ease",
                                transition: "all " + 300 + "ms ease"
                            })
                        } else {
                            if (o == b.getNext(d, k)) {
                                var h;
                                switch (l) {
                                case "++":
                                    h = 0;
                                    break;
                                case "--":
                                    h = 300;
                                    break;
                                case "+=":
                                    h = 300;
                                    break;
                                case "-=":
                                    h = 0;
                                    break;
                                default:
                                    break
                                }
                                b.setStyle(c[b.getNext(d, k)], {
                                    WebkitTransform: "translate3d(" + r + "px, 0px, 0px)",
                                    transform: "translate3d(" + r + "px, 0px, 0px)",
                                    zIndex: 10,
                                    WebkitTransition: "all " + h + "ms ease",
                                    transition: "all " + h + "ms ease"
                                })
                            } else {
                                b.setStyle(c[o], {
                                    WebkitTransform: "translate3d(" + 0 + "px, 0px, 0px)",
                                    transform: "translate3d(" + 0 + "px, 0px, 0px)",
                                    zIndex: 9,
                                    WebkitTransition: "all " + 0 + "ms ease",
                                    transition: "all " + 0 + "ms ease"
                                })
                            }
                        }
                    }
                }
            } else {
                for (var o = 0; o < k; o++) {
                    b.setStyle(c[o], {
                        WebkitTransition: "all " + 300 + "ms ease",
                        transition: "all " + 300 + "ms ease"
                    })
                }
                if (d <= 0) {
                    d = 0;
                    this.now = 0
                }
                if (d > k - 1) {
                    if (f) {
                        d = 0;
                        this.now = 0
                    } else {
                        d = k - 1;
                        this.now = k - 1
                    }
                }
                for (var n = 0; n < k; n++) {
                    b.setStyle(c[n], {
                        WebkitTransform: "translate3d(" + ( - d * r) + "px, 0px, 0px)",
                        transform: "translate3d(" + ( - d * r) + "px, 0px, 0px)"
                    })
                }
            }
            if (this.defaults.smallBtn) {
                for (var o = 0; o < this.btns.length; o++) {
                    this.btns[o].className = ""
                }
                if (this.need) {
                    this.btns[b.getNow(d, k / 2)].className = "active"
                } else {
                    this.btns[b.getNow(d, k)].className = "active"
                }
            }
            if (this.defaults.number) {
                this.slideNub.innerHTML = b.getNow(d, k) + 1
            }
            c[b.getNow(d, k)].addEventListener("webkitTransitionEnd", m, false);
            c[b.getNow(d, k)].addEventListener("transitionend", m, false);
            function m() {
                if (p.defaults.location) {
                    if (e < -(k - 1) * r - r / 5) {
                        if (p.defaults.lastImgSlider && typeof p.defaults.lastImgSlider == "function") {
                            p.defaults.laseMoveFn(0);
                            p.defaults.lastImgSlider()
                        }
                    }
                }
                c[b.getNow(p.now, k)].removeEventListener("webkitTransitionEnd", m, false);
                c[b.getNow(p.now, k)].removeEventListener("transitionend", m, false)
            }
        },
        play: function() {
            var c = this;
            c.timer = setInterval(function() {
                c.now++;
                c.tab(null, "++", true)
            },
            this.defaults.playTime)
        },
        pause: function() {
            var c = this;
            clearInterval(c.timer)
        },
        orientationchangeHandler: function() {
            var c = this;
            setTimeout(function() {
                c.liInit()
            },
            300)
        },
        addSmallBtn: function() {
            var d = "",
            e = this.aLi;
            for (var c = 0; c < e.length; c++) {
                if (c == this.defaults.startIndex) {
                    d += '<span class="active" data-ol-btn="btn"></span>'
                } else {
                    d += '<span data-ol-btn="btn"></span>'
                }
            }
            return d
        },
        show: function() {
            this.oBox.style.display = "inline-block"
        },
        hide: function() {
            this.oBox.style.display = "none"
        }
    };
    b.extend = function(d, c) {
        for (name in c) {
            if (c[name] !== undefined) {
                d[name] = c[name]
            }
        }
    };
    b.extend(b, {
        setStyle: function(d, c) {
            for (name in c) {
                d.style[name] = c[name]
            }
        },
        getTranX: function(e) {
            var d = e.style.WebkitTransform || e.style.transform;
            var f = d.indexOf("translate3d(");
            var c = parseInt(d.substring(f + 12, d.length - 13));
            return c
        },
        _device: function() {
            var f = !!("ontouchstart" in window || window.DocumentTouch && document instanceof window.DocumentTouch);
            var d = "touchstart";
            var e = "touchmove";
            var c = "touchend";
            return {
                hasTouch: f,
                startEvt: d,
                moveEvt: e,
                endEvt: c
            }
        },
        getNow: function(d, c) {
            return d % c
        },
        getPre: function(e, c) {
            if (e % c - 1 < 0) {
                var d = c - 1
            } else {
                var d = e % c - 1
            }
            return d
        },
        getNext: function(e, d) {
            if (e % d + 1 > d - 1) {
                var c = 0
            } else {
                var c = e % d + 1
            }
            return c
        },
        move: function(e, c, d) {
            var f = d || null;
            if (f) {
                e.style.zIndex = f
            }
            b.setStyle(e, {
                WebkitTransform: "translate3d(" + c + "px, 0px, 0px)",
                transform: "translate3d(" + c + "px, 0px, 0px)"
            })
        },
        stopDefault: function(c) {
            if (c && c.preventDefault) {
                c.preventDefault()
            } else {
                window.event.returnValue = false
            }
            return false
        },
        stopPropagation: function(c) {
            if (c && c.stopPropagation) {
                c.stopPropagation()
            } else {
                c.cancelBubble = true
            }
        }
    });
    return a
})();
slide("#slide").init({
    startIndex: (parseInt($("#picInitNum").val()) - 1),
    number: true,
    laseMoveFn: jump,
    lastImgSlider: sliderJump,
    preDef: "lnr",
    location: true,
    autoPlay: false,
    autoHeight: true,
    mpingEvent: "MProductdetail_SlideBigPic",
    wareId: $("#wareId").val()
});
function jump(a) {
    var c = document.getElementById("tittup");
    var d = document.getElementById("slide");
    var b = d.offsetWidth;
    if (a < -b / 5) {
        c.children[0].classList.add("rotate"); 
        c.children[1].innerHTML = "\u771f\u6ca1\u6709\u5566"
    }
    if (a > -b / 5) {
        c.children[0].classList.remove("rotate");
        c.children[1].innerHTML = "\u6ca1\u6709\u5566"
    }
    c.style.WebkitTransform = "translate3d(" + a + "px, 0px ,0px)";
    c.style.transform = "translate3d(" + a + "px, 0px ,0px)"
}
function sliderJump() {
    var c = window.location.host;
    try {
        var b = window.localStorage;
        localStorage.removeItem("warePicShowGoback");
        localStorage.setItem("warePicShowGoback", "true");
    } catch(a) {}
}
var pingClick = function(g, d, c, a) {
    try {
        var f = new MPing.inputs.Click(g);
        f.event_param = d;
        f.page_param = a;
        var b = new MPing();
        b.send(f)
    } catch(h) {}
};
var pingClickWithLevel = function(h, d, c, a, g) {
    try {
        var f = new MPing.inputs.Click(h);
        f.event_param = d;
        f.page_param = a;
        f.event_level = g;
        var b = new MPing();
        b.send(f)
    } catch(i) {}
};
function imgError() {
    var a = event.srcElement;
    a.src = "/images/2014/ware/pic-error.png";
    a.onerror = null;
    a.parentNode.onclick = function() {
        location.reload()
    }
};