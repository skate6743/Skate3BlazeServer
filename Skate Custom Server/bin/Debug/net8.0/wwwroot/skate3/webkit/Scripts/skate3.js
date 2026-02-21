/*
 * jQuery JavaScript Library v1.3.2
 *
 * Copyright (c) 2009 John Resig, http://jquery.com/
 * 
 * Permission is hereby granted, free of charge, to any person obtaining
 * a copy of this software and associated documentation files (the
 * "Software"), to deal in the Software without restriction, including
 * without limitation the rights to use, copy, modify, merge, publish,
 * distribute, sublicense, and/or sell copies of the Software, and to
 * permit persons to whom the Software is furnished to do so, subject to
 * the following conditions:
 * 
 * The above copyright notice and this permission notice shall be
 * included in all copies or substantial portions of the Software.
 * 
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
 * EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
 * MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
 * NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE
 * LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION
 * OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION
 * WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
 *
 * Date: 2009-02-19 17:34:21 -0500 (Thu, 19 Feb 2009)
 * Revision: 6246
 */
(function() {
    var m = this,
        g, B = m.jQuery,
        r = m.$,
        q = m.jQuery = m.$ = function(H, I) {
            return new q.fn.init(H, I)
        },
        G = /^[^<]*(<(.|\s)+>)[^>]*$|^#([\w-]+)$/,
        f = /^.[^:#\[\.,]*$/;
    q.fn = q.prototype = {
        init: function(H, K) {
            H = H || document;
            if (H.nodeType) {
                this[0] = H;
                this.length = 1;
                this.context = H;
                return this
            }
            if (typeof H === "string") {
                var J = G.exec(H);
                if (J && (J[1] || !K)) {
                    if (J[1]) {
                        H = q.clean([J[1]], K)
                    } else {
                        var L = document.getElementById(J[3]);
                        if (L && L.id != J[3]) {
                            return q().find(H)
                        }
                        var I = q(L || []);
                        I.context = document;
                        I.selector = H;
                        return I
                    }
                } else {
                    return q(K).find(H)
                }
            } else {
                if (q.isFunction(H)) {
                    return q(document).ready(H)
                }
            }
            if (H.selector && H.context) {
                this.selector = H.selector;
                this.context = H.context
            }
            return this.setArray(q.isArray(H) ? H : q.makeArray(H))
        },
        selector: "",
        jquery: "1.3.2",
        size: function() {
            return this.length
        },
        get: function(H) {
            return H === g ? Array.prototype.slice.call(this) : this[H]
        },
        pushStack: function(I, K, H) {
            var J = q(I);
            J.prevObject = this;
            J.context = this.context;
            if (K === "find") {
                J.selector = this.selector + (this.selector ? " " : "") + H
            } else {
                if (K) {
                    J.selector = this.selector + "." + K + "(" + H + ")"
                }
            }
            return J
        },
        setArray: function(H) {
            this.length = 0;
            Array.prototype.push.apply(this, H);
            return this
        },
        each: function(I, H) {
            return q.each(this, I, H)
        },
        index: function(H) {
            return q.inArray(H && H.jquery ? H[0] : H, this)
        },
        attr: function(I, K, J) {
            var H = I;
            if (typeof I === "string") {
                if (K === g) {
                    return this[0] && q[J || "attr"](this[0], I)
                } else {
                    H = {};
                    H[I] = K
                }
            }
            return this.each(function(L) {
                for (I in H) {
                    q.attr(J ? this.style : this, I, q.prop(this, H[I], J, L, I))
                }
            })
        },
        css: function(H, I) {
            if ((H == "width" || H == "height") && parseFloat(I) < 0) {
                I = g
            }
            return this.attr(H, I, "curCSS")
        },
        text: function(I) {
            if (typeof I !== "object" && I != null) {
                return this.empty().append((this[0] && this[0].ownerDocument || document).createTextNode(I))
            }
            var H = "";
            q.each(I || this, function() {
                q.each(this.childNodes, function() {
                    if (this.nodeType != 8) {
                        H += this.nodeType != 1 ? this.nodeValue : q.fn.text([this])
                    }
                })
            });
            return H
        },
        wrapAll: function(H) {
            if (this[0]) {
                var I = q(H, this[0].ownerDocument).clone();
                if (this[0].parentNode) {
                    I.insertBefore(this[0])
                }
                I.map(function() {
                    var J = this;
                    while (J.firstChild) {
                        J = J.firstChild
                    }
                    return J
                }).append(this)
            }
            return this
        },
        wrapInner: function(H) {
            return this.each(function() {
                q(this).contents().wrapAll(H)
            })
        },
        wrap: function(H) {
            return this.each(function() {
                q(this).wrapAll(H)
            })
        },
        append: function() {
            return this.domManip(arguments, true, function(H) {
                if (this.nodeType == 1) {
                    this.appendChild(H)
                }
            })
        },
        prepend: function() {
            return this.domManip(arguments, true, function(H) {
                if (this.nodeType == 1) {
                    this.insertBefore(H, this.firstChild)
                }
            })
        },
        before: function() {
            return this.domManip(arguments, false, function(H) {
                this.parentNode.insertBefore(H, this)
            })
        },
        after: function() {
            return this.domManip(arguments, false, function(H) {
                this.parentNode.insertBefore(H, this.nextSibling)
            })
        },
        end: function() {
            return this.prevObject || q([])
        },
        push: [].push,
        sort: [].sort,
        splice: [].splice,
        find: function(H) {
            if (this.length === 1) {
                var I = this.pushStack([], "find", H);
                I.length = 0;
                q.find(H, this[0], I);
                return I
            } else {
                return this.pushStack(q.unique(q.map(this, function(J) {
                    return q.find(H, J)
                })), "find", H)
            }
        },
        clone: function(J) {
            var H = this.map(function() {
                if (!q.support.noCloneEvent && !q.isXMLDoc(this)) {
                    var L = this.outerHTML;
                    if (!L) {
                        var M = this.ownerDocument.createElement("div");
                        M.appendChild(this.cloneNode(true));
                        L = M.innerHTML
                    }
                    return q.clean([L.replace(/ jQuery\d+="(?:\d+|null)"/g, "").replace(/^\s*/, "")])[0]
                } else {
                    return this.cloneNode(true)
                }
            });
            if (J === true) {
                var K = this.find("*").andSelf(),
                    I = 0;
                H.find("*").andSelf().each(function() {
                    if (this.nodeName !== K[I].nodeName) {
                        return
                    }
                    var L = q.data(K[I], "events");
                    for (var N in L) {
                        for (var M in L[N]) {
                            q.event.add(this, N, L[N][M], L[N][M].data)
                        }
                    }
                    I++
                })
            }
            return H
        },
        filter: function(H) {
            return this.pushStack(q.isFunction(H) && q.grep(this, function(J, I) {
                return H.call(J, I)
            }) || q.multiFilter(H, q.grep(this, function(I) {
                return I.nodeType === 1
            })), "filter", H)
        },
        closest: function(H) {
            var J = q.expr.match.POS.test(H) ? q(H) : null,
                I = 0;
            return this.map(function() {
                var K = this;
                while (K && K.ownerDocument) {
                    if (J ? J.index(K) > -1 : q(K).is(H)) {
                        q.data(K, "closest", I);
                        return K
                    }
                    K = K.parentNode;
                    I++
                }
            })
        },
        not: function(H) {
            if (typeof H === "string") {
                if (f.test(H)) {
                    return this.pushStack(q.multiFilter(H, this, true), "not", H)
                } else {
                    H = q.multiFilter(H, this)
                }
            }
            var I = H.length && H[H.length - 1] !== g && !H.nodeType;
            return this.filter(function() {
                return I ? q.inArray(this, H) < 0 : this != H
            })
        },
        add: function(H) {
            return this.pushStack(q.unique(q.merge(this.get(), typeof H === "string" ? q(H) : q.makeArray(H))))
        },
        is: function(H) {
            return !!H && q.multiFilter(H, this).length > 0
        },
        hasClass: function(H) {
            return !!H && this.is("." + H)
        },
        val: function(N) {
            if (N === g) {
                var H = this[0];
                if (H) {
                    if (q.nodeName(H, "option")) {
                        return (H.attributes.value || {}).specified ? H.value : H.text
                    }
                    if (q.nodeName(H, "select")) {
                        var L = H.selectedIndex,
                            O = [],
                            P = H.options,
                            K = H.type == "select-one";
                        if (L < 0) {
                            return null
                        }
                        for (var I = K ? L : 0, M = K ? L + 1 : P.length; I < M; I++) {
                            var J = P[I];
                            if (J.selected) {
                                N = q(J).val();
                                if (K) {
                                    return N
                                }
                                O.push(N)
                            }
                        }
                        return O
                    }
                    return (H.value || "").replace(/\r/g, "")
                }
                return g
            }
            if (typeof N === "number") {
                N += ""
            }
            return this.each(function() {
                if (this.nodeType != 1) {
                    return
                }
                if (q.isArray(N) && /radio|checkbox/.test(this.type)) {
                    this.checked = (q.inArray(this.value, N) >= 0 || q.inArray(this.name, N) >= 0)
                } else {
                    if (q.nodeName(this, "select")) {
                        var Q = q.makeArray(N);
                        q("option", this).each(function() {
                            this.selected = (q.inArray(this.value, Q) >= 0 || q.inArray(this.text, Q) >= 0)
                        });
                        if (!Q.length) {
                            this.selectedIndex = -1
                        }
                    } else {
                        this.value = N
                    }
                }
            })
        },
        html: function(H) {
            return H === g ? (this[0] ? this[0].innerHTML.replace(/ jQuery\d+="(?:\d+|null)"/g, "") : null) : this.empty().append(H)
        },
        replaceWith: function(H) {
            return this.after(H).remove()
        },
        eq: function(H) {
            return this.slice(H, +H + 1)
        },
        slice: function() {
            return this.pushStack(Array.prototype.slice.apply(this, arguments), "slice", Array.prototype.slice.call(arguments).join(","))
        },
        map: function(H) {
            return this.pushStack(q.map(this, function(J, I) {
                return H.call(J, I, J)
            }))
        },
        andSelf: function() {
            return this.add(this.prevObject)
        },
        domManip: function(M, P, O) {
            if (this[0]) {
                var L = (this[0].ownerDocument || this[0]).createDocumentFragment(),
                    I = q.clean(M, (this[0].ownerDocument || this[0]), L),
                    K = L.firstChild;
                if (K) {
                    for (var J = 0, H = this.length; J < H; J++) {
                        O.call(N(this[J], K), this.length > 1 || J > 0 ? L.cloneNode(true) : L)
                    }
                }
                if (I) {
                    q.each(I, C)
                }
            }
            return this;

            function N(Q, R) {
                return P && q.nodeName(Q, "table") && q.nodeName(R, "tr") ? (Q.getElementsByTagName("tbody")[0] || Q.appendChild(Q.ownerDocument.createElement("tbody"))) : Q
            }
        }
    };
    q.fn.init.prototype = q.fn;

    function C(H, I) {
        if (I.src) {
            q.ajax({
                url: I.src,
                async: false,
                dataType: "script"
            })
        } else {
            q.globalEval(I.text || I.textContent || I.innerHTML || "")
        }
        if (I.parentNode) {
            I.parentNode.removeChild(I)
        }
    }

    function e() {
        return +new Date
    }
    q.extend = q.fn.extend = function() {
        var M = arguments[0] || {},
            K = 1,
            L = arguments.length,
            H = false,
            J;
        if (typeof M === "boolean") {
            H = M;
            M = arguments[1] || {};
            K = 2
        }
        if (typeof M !== "object" && !q.isFunction(M)) {
            M = {}
        }
        if (L == K) {
            M = this;
            --K
        }
        for (; K < L; K++) {
            if ((J = arguments[K]) != null) {
                for (var I in J) {
                    var N = M[I],
                        O = J[I];
                    if (M === O) {
                        continue
                    }
                    if (H && O && typeof O === "object" && !O.nodeType) {
                        M[I] = q.extend(H, N || (O.length != null ? [] : {}), O)
                    } else {
                        if (O !== g) {
                            M[I] = O
                        }
                    }
                }
            }
        }
        return M
    };
    var b = /z-?index|font-?weight|opacity|zoom|line-?height/i,
        s = document.defaultView || {},
        u = Object.prototype.toString;
    q.extend({
        noConflict: function(H) {
            m.$ = r;
            if (H) {
                m.jQuery = B
            }
            return q
        },
        isFunction: function(H) {
            return u.call(H) === "[object Function]"
        },
        isArray: function(H) {
            return u.call(H) === "[object Array]"
        },
        isXMLDoc: function(H) {
            return H.nodeType === 9 && H.documentElement.nodeName !== "HTML" || !!H.ownerDocument && q.isXMLDoc(H.ownerDocument)
        },
        globalEval: function(J) {
            if (J && /\S/.test(J)) {
                var I = document.getElementsByTagName("head")[0] || document.documentElement,
                    H = document.createElement("script");
                H.type = "text/javascript";
                if (q.support.scriptEval) {
                    H.appendChild(document.createTextNode(J))
                } else {
                    H.text = J
                }
                I.insertBefore(H, I.firstChild);
                I.removeChild(H)
            }
        },
        nodeName: function(I, H) {
            return I.nodeName && I.nodeName.toUpperCase() == H.toUpperCase()
        },
        each: function(J, N, I) {
            var H, K = 0,
                L = J.length;
            if (I) {
                if (L === g) {
                    for (H in J) {
                        if (N.apply(J[H], I) === false) {
                            break
                        }
                    }
                } else {
                    for (; K < L;) {
                        if (N.apply(J[K++], I) === false) {
                            break
                        }
                    }
                }
            } else {
                if (L === g) {
                    for (H in J) {
                        if (N.call(J[H], H, J[H]) === false) {
                            break
                        }
                    }
                } else {
                    for (var M = J[0]; K < L && N.call(M, K, M) !== false; M = J[++K]) {}
                }
            }
            return J
        },
        prop: function(K, L, J, I, H) {
            if (q.isFunction(L)) {
                L = L.call(K, I)
            }
            return typeof L === "number" && J == "curCSS" && !b.test(H) ? L + "px" : L
        },
        className: {
            add: function(H, I) {
                q.each((I || "").split(/\s+/), function(J, K) {
                    if (H.nodeType == 1 && !q.className.has(H.className, K)) {
                        H.className += (H.className ? " " : "") + K
                    }
                })
            },
            remove: function(H, I) {
                if (H.nodeType == 1) {
                    H.className = I !== g ? q.grep(H.className.split(/\s+/), function(J) {
                        return !q.className.has(I, J)
                    }).join(" ") : ""
                }
            },
            has: function(I, H) {
                return I && q.inArray(H, (I.className || I).toString().split(/\s+/)) > -1
            }
        },
        swap: function(K, J, L) {
            var H = {};
            for (var I in J) {
                H[I] = K.style[I];
                K.style[I] = J[I]
            }
            L.call(K);
            for (var I in J) {
                K.style[I] = H[I]
            }
        },
        css: function(K, I, M, H) {
            if (I == "width" || I == "height") {
                var O, J = {
                        position: "absolute",
                        visibility: "hidden",
                        display: "block"
                    },
                    N = I == "width" ? ["Left", "Right"] : ["Top", "Bottom"];

                function L() {
                    O = I == "width" ? K.offsetWidth : K.offsetHeight;
                    if (H === "border") {
                        return
                    }
                    q.each(N, function() {
                        if (!H) {
                            O -= parseFloat(q.curCSS(K, "padding" + this, true)) || 0
                        }
                        if (H === "margin") {
                            O += parseFloat(q.curCSS(K, "margin" + this, true)) || 0
                        } else {
                            O -= parseFloat(q.curCSS(K, "border" + this + "Width", true)) || 0
                        }
                    })
                }
                if (K.offsetWidth !== 0) {
                    L()
                } else {
                    q.swap(K, J, L)
                }
                return Math.max(0, Math.round(O))
            }
            return q.curCSS(K, I, M)
        },
        curCSS: function(L, I, J) {
            var O, H = L.style;
            if (I == "opacity" && !q.support.opacity) {
                O = q.attr(H, "opacity");
                return O == "" ? "1" : O
            }
            if (I.match(/float/i)) {
                I = z
            }
            if (!J && H && H[I]) {
                O = H[I]
            } else {
                if (s.getComputedStyle) {
                    if (I.match(/float/i)) {
                        I = "float"
                    }
                    I = I.replace(/([A-Z])/g, "-$1").toLowerCase();
                    var P = s.getComputedStyle(L, null);
                    if (P) {
                        O = P.getPropertyValue(I)
                    }
                    if (I == "opacity" && O == "") {
                        O = "1"
                    }
                } else {
                    if (L.currentStyle) {
                        var M = I.replace(/\-(\w)/g, function(Q, R) {
                            return R.toUpperCase()
                        });
                        O = L.currentStyle[I] || L.currentStyle[M];
                        if (!/^\d+(px)?$/i.test(O) && /^\d/.test(O)) {
                            var K = H.left,
                                N = L.runtimeStyle.left;
                            L.runtimeStyle.left = L.currentStyle.left;
                            H.left = O || 0;
                            O = H.pixelLeft + "px";
                            H.left = K;
                            L.runtimeStyle.left = N
                        }
                    }
                }
            }
            return O
        },
        clean: function(I, N, L) {
            N = N || document;
            if (typeof N.createElement === "undefined") {
                N = N.ownerDocument || N[0] && N[0].ownerDocument || document
            }
            if (!L && I.length === 1 && typeof I[0] === "string") {
                var K = /^<(\w+)\s*\/?>$/.exec(I[0]);
                if (K) {
                    return [N.createElement(K[1])]
                }
            }
            var J = [],
                H = [],
                O = N.createElement("div");
            q.each(I, function(S, V) {
                if (typeof V === "number") {
                    V += ""
                }
                if (!V) {
                    return
                }
                if (typeof V === "string") {
                    V = V.replace(/(<(\w+)[^>]*?)\/>/g, function(X, Y, W) {
                        return W.match(/^(abbr|br|col|img|input|link|meta|param|hr|area|embed)$/i) ? X : Y + "></" + W + ">"
                    });
                    var R = V.replace(/^\s+/, "").substring(0, 10).toLowerCase();
                    var T = !R.indexOf("<opt") && [1, "<select multiple='multiple'>", "</select>"] || !R.indexOf("<leg") && [1, "<fieldset>", "</fieldset>"] || R.match(/^<(thead|tbody|tfoot|colg|cap)/) && [1, "<table>", "</table>"] || !R.indexOf("<tr") && [2, "<table><tbody>", "</tbody></table>"] || (!R.indexOf("<td") || !R.indexOf("<th")) && [3, "<table><tbody><tr>", "</tr></tbody></table>"] || !R.indexOf("<col") && [2, "<table><tbody></tbody><colgroup>", "</colgroup></table>"] || !q.support.htmlSerialize && [1, "div<div>", "</div>"] || [0, "", ""];
                    O.innerHTML = T[1] + V + T[2];
                    while (T[0]--) {
                        O = O.lastChild
                    }
                    if (!q.support.tbody) {
                        var U = /<tbody/i.test(V),
                            Q = !R.indexOf("<table") && !U ? O.firstChild && O.firstChild.childNodes : T[1] == "<table>" && !U ? O.childNodes : [];
                        for (var P = Q.length - 1; P >= 0; --P) {
                            if (q.nodeName(Q[P], "tbody") && !Q[P].childNodes.length) {
                                Q[P].parentNode.removeChild(Q[P])
                            }
                        }
                    }
                    if (!q.support.leadingWhitespace && /^\s/.test(V)) {
                        O.insertBefore(N.createTextNode(V.match(/^\s*/)[0]), O.firstChild)
                    }
                    V = q.makeArray(O.childNodes)
                }
                if (V.nodeType) {
                    J.push(V)
                } else {
                    J = q.merge(J, V)
                }
            });
            if (L) {
                for (var M = 0; J[M]; M++) {
                    if (q.nodeName(J[M], "script") && (!J[M].type || J[M].type.toLowerCase() === "text/javascript")) {
                        H.push(J[M].parentNode ? J[M].parentNode.removeChild(J[M]) : J[M])
                    } else {
                        if (J[M].nodeType === 1) {
                            J.splice.apply(J, [M + 1, 0].concat(q.makeArray(J[M].getElementsByTagName("script"))))
                        }
                        L.appendChild(J[M])
                    }
                }
                return H
            }
            return J
        },
        attr: function(M, J, N) {
            if (!M || M.nodeType == 3 || M.nodeType == 8) {
                return g
            }
            var K = !q.isXMLDoc(M),
                O = N !== g;
            J = K && q.props[J] || J;
            if (M.tagName) {
                var I = /href|src|style/.test(J);
                if (J == "selected" && M.parentNode) {
                    M.parentNode.selectedIndex
                }
                if (J in M && K && !I) {
                    if (O) {
                        if (J == "type" && q.nodeName(M, "input") && M.parentNode) {
                            throw "type property can't be changed"
                        }
                        M[J] = N
                    }
                    if (q.nodeName(M, "form") && M.getAttributeNode(J)) {
                        return M.getAttributeNode(J).nodeValue
                    }
                    if (J == "tabIndex") {
                        var L = M.getAttributeNode("tabIndex");
                        return L && L.specified ? L.value : M.nodeName.match(/(button|input|object|select|textarea)/i) ? 0 : M.nodeName.match(/^(a|area)$/i) && M.href ? 0 : g
                    }
                    return M[J]
                }
                if (!q.support.style && K && J == "style") {
                    return q.attr(M.style, "cssText", N)
                }
                if (O) {
                    M.setAttribute(J, "" + N)
                }
                var H = !q.support.hrefNormalized && K && I ? M.getAttribute(J, 2) : M.getAttribute(J);
                return H === null ? g : H
            }
            if (!q.support.opacity && J == "opacity") {
                if (O) {
                    M.zoom = 1;
                    M.filter = (M.filter || "").replace(/alpha\([^)]*\)/, "") + (parseInt(N) + "" == "NaN" ? "" : "alpha(opacity=" + N * 100 + ")")
                }
                return M.filter && M.filter.indexOf("opacity=") >= 0 ? (parseFloat(M.filter.match(/opacity=([^)]*)/)[1]) / 100) + "" : ""
            }
            J = J.replace(/-([a-z])/ig, function(P, Q) {
                return Q.toUpperCase()
            });
            if (O) {
                M[J] = N
            }
            return M[J]
        },
        trim: function(H) {
            return (H || "").replace(/^\s+|\s+$/g, "")
        },
        makeArray: function(J) {
            var H = [];
            if (J != null) {
                var I = J.length;
                if (I == null || typeof J === "string" || q.isFunction(J) || J.setInterval) {
                    H[0] = J
                } else {
                    while (I) {
                        H[--I] = J[I]
                    }
                }
            }
            return H
        },
        inArray: function(J, K) {
            for (var H = 0, I = K.length; H < I; H++) {
                if (K[H] === J) {
                    return H
                }
            }
            return -1
        },
        merge: function(K, H) {
            var I = 0,
                J, L = K.length;
            if (!q.support.getAll) {
                while ((J = H[I++]) != null) {
                    if (J.nodeType != 8) {
                        K[L++] = J
                    }
                }
            } else {
                while ((J = H[I++]) != null) {
                    K[L++] = J
                }
            }
            return K
        },
        unique: function(N) {
            var I = [],
                H = {};
            try {
                for (var J = 0, K = N.length; J < K; J++) {
                    var M = q.data(N[J]);
                    if (!H[M]) {
                        H[M] = true;
                        I.push(N[J])
                    }
                }
            } catch (L) {
                I = N
            }
            return I
        },
        grep: function(I, M, H) {
            var J = [];
            for (var K = 0, L = I.length; K < L; K++) {
                if (!H != !M(I[K], K)) {
                    J.push(I[K])
                }
            }
            return J
        },
        map: function(H, M) {
            var I = [];
            for (var J = 0, K = H.length; J < K; J++) {
                var L = M(H[J], J);
                if (L != null) {
                    I[I.length] = L
                }
            }
            return I.concat.apply([], I)
        }
    });
    var F = navigator.userAgent.toLowerCase();
    q.browser = {
        version: (F.match(/.+(?:rv|it|ra|ie)[\/: ]([\d.]+)/) || [0, "0"])[1],
        safari: /webkit/.test(F),
        opera: /opera/.test(F),
        msie: /msie/.test(F) && !/opera/.test(F),
        mozilla: /mozilla/.test(F) && !/(compatible|webkit)/.test(F)
    };
    q.each({
        parent: function(H) {
            return H.parentNode
        },
        parents: function(H) {
            return q.dir(H, "parentNode")
        },
        next: function(H) {
            return q.nth(H, 2, "nextSibling")
        },
        prev: function(H) {
            return q.nth(H, 2, "previousSibling")
        },
        nextAll: function(H) {
            return q.dir(H, "nextSibling")
        },
        prevAll: function(H) {
            return q.dir(H, "previousSibling")
        },
        siblings: function(H) {
            return q.sibling(H.parentNode.firstChild, H)
        },
        children: function(H) {
            return q.sibling(H.firstChild)
        },
        contents: function(H) {
            return q.nodeName(H, "iframe") ? H.contentDocument || H.contentWindow.document : q.makeArray(H.childNodes)
        }
    }, function(H, I) {
        q.fn[H] = function(J) {
            var K = q.map(this, I);
            if (J && typeof J == "string") {
                K = q.multiFilter(J, K)
            }
            return this.pushStack(q.unique(K), H, J)
        }
    });
    q.each({
        appendTo: "append",
        prependTo: "prepend",
        insertBefore: "before",
        insertAfter: "after",
        replaceAll: "replaceWith"
    }, function(H, I) {
        q.fn[H] = function(J) {
            var M = [],
                O = q(J);
            for (var N = 0, K = O.length; N < K; N++) {
                var L = (N > 0 ? this.clone(true) : this).get();
                q.fn[I].apply(q(O[N]), L);
                M = M.concat(L)
            }
            return this.pushStack(M, H, J)
        }
    });
    q.each({
        removeAttr: function(H) {
            q.attr(this, H, "");
            if (this.nodeType == 1) {
                this.removeAttribute(H)
            }
        },
        addClass: function(H) {
            q.className.add(this, H)
        },
        removeClass: function(H) {
            q.className.remove(this, H)
        },
        toggleClass: function(I, H) {
            if (typeof H !== "boolean") {
                H = !q.className.has(this, I)
            }
            q.className[H ? "add" : "remove"](this, I)
        },
        remove: function(H) {
            if (!H || q.filter(H, [this]).length) {
                q("*", this).add([this]).each(function() {
                    q.event.remove(this);
                    q.removeData(this)
                });
                if (this.parentNode) {
                    this.parentNode.removeChild(this)
                }
            }
        },
        empty: function() {
            q(this).children().remove();
            while (this.firstChild) {
                this.removeChild(this.firstChild)
            }
        }
    }, function(H, I) {
        q.fn[H] = function() {
            return this.each(I, arguments)
        }
    });

    function k(H, I) {
        return H[0] && parseInt(q.curCSS(H[0], I, true), 10) || 0
    }
    var h = "jQuery" + e(),
        y = 0,
        D = {};
    q.extend({
        cache: {},
        data: function(I, H, J) {
            I = I == m ? D : I;
            var K = I[h];
            if (!K) {
                K = I[h] = ++y
            }
            if (H && !q.cache[K]) {
                q.cache[K] = {}
            }
            if (J !== g) {
                q.cache[K][H] = J
            }
            return H ? q.cache[K][H] : K
        },
        removeData: function(I, H) {
            I = I == m ? D : I;
            var K = I[h];
            if (H) {
                if (q.cache[K]) {
                    delete q.cache[K][H];
                    H = "";
                    for (H in q.cache[K]) {
                        break
                    }
                    if (!H) {
                        q.removeData(I)
                    }
                }
            } else {
                try {
                    delete I[h]
                } catch (J) {
                    if (I.removeAttribute) {
                        I.removeAttribute(h)
                    }
                }
                delete q.cache[K]
            }
        },
        queue: function(I, H, K) {
            if (I) {
                H = (H || "fx") + "queue";
                var J = q.data(I, H);
                if (!J || q.isArray(K)) {
                    J = q.data(I, H, q.makeArray(K))
                } else {
                    if (K) {
                        J.push(K)
                    }
                }
            }
            return J
        },
        dequeue: function(K, J) {
            var H = q.queue(K, J),
                I = H.shift();
            if (!J || J === "fx") {
                I = H[0]
            }
            if (I !== g) {
                I.call(K)
            }
        }
    });
    q.fn.extend({
        data: function(H, J) {
            var K = H.split(".");
            K[1] = K[1] ? "." + K[1] : "";
            if (J === g) {
                var I = this.triggerHandler("getData" + K[1] + "!", [K[0]]);
                if (I === g && this.length) {
                    I = q.data(this[0], H)
                }
                return I === g && K[1] ? this.data(K[0]) : I
            } else {
                return this.trigger("setData" + K[1] + "!", [K[0], J]).each(function() {
                    q.data(this, H, J)
                })
            }
        },
        removeData: function(H) {
            return this.each(function() {
                q.removeData(this, H)
            })
        },
        queue: function(H, I) {
            if (typeof H !== "string") {
                I = H;
                H = "fx"
            }
            if (I === g) {
                return q.queue(this[0], H)
            }
            return this.each(function() {
                var J = q.queue(this, H, I);
                if (H == "fx" && J.length == 1) {
                    J[0].call(this)
                }
            })
        },
        dequeue: function(H) {
            return this.each(function() {
                q.dequeue(this, H)
            })
        }
    });
    /*
     * Sizzle CSS Selector Engine - v0.9.3
     *  Copyright 2009, The Dojo Foundation
     *  More information: http://sizzlejs.com/
     *
     * Permission is hereby granted, free of charge, to any person obtaining a copy
     * of this software and associated documentation files (the "Software"), to deal
     * in the Software without restriction, including without limitation the rights
     * to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
     * copies of the Software, and to permit persons to whom the Software is
     * furnished to do so, subject to the following conditions:
     *
     * The above copyright notice and this permission notice shall be included in
     * all copies or substantial portions of the Software.
     *
     * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
     * IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
     * FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
     * AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
     * LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
     * OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
     * THE SOFTWARE.
     *
     */
    (function() {
        var U = /((?:\((?:\([^()]+\)|[^()]+)+\)|\[(?:\[[^[\]]*\]|['"][^'"]*['"]|[^[\]'"]+)+\]|\\.|[^ >+~,(\[\\]+)+|[>+~])(\s*,\s*)?/g,
            O = 0,
            K = Object.prototype.toString;
        var I = function(ab, X, ae, af) {
            ae = ae || [];
            X = X || document;
            if (X.nodeType !== 1 && X.nodeType !== 9) {
                return []
            }
            if (!ab || typeof ab !== "string") {
                return ae
            }
            var ac = [],
                Z, ai, al, W, ag, Y, aa = true;
            U.lastIndex = 0;
            while ((Z = U.exec(ab)) !== null) {
                ac.push(Z[1]);
                if (Z[2]) {
                    Y = RegExp.rightContext;
                    break
                }
            }
            if (ac.length > 1 && P.exec(ab)) {
                if (ac.length === 2 && L.relative[ac[0]]) {
                    ai = M(ac[0] + ac[1], X)
                } else {
                    ai = L.relative[ac[0]] ? [X] : I(ac.shift(), X);
                    while (ac.length) {
                        ab = ac.shift();
                        if (L.relative[ab]) {
                            ab += ac.shift()
                        }
                        ai = M(ab, ai)
                    }
                }
            } else {
                var ah = af ? {
                    expr: ac.pop(),
                    set: H(af)
                } : I.find(ac.pop(), ac.length === 1 && X.parentNode ? X.parentNode : X, T(X));
                ai = I.filter(ah.expr, ah.set);
                if (ac.length > 0) {
                    al = H(ai)
                } else {
                    aa = false
                }
                while (ac.length) {
                    var ak = ac.pop(),
                        aj = ak;
                    if (!L.relative[ak]) {
                        ak = ""
                    } else {
                        aj = ac.pop()
                    }
                    if (aj == null) {
                        aj = X
                    }
                    L.relative[ak](al, aj, T(X))
                }
            }
            if (!al) {
                al = ai
            }
            if (!al) {
                throw "Syntax error, unrecognized expression: " + (ak || ab)
            }
            if (K.call(al) === "[object Array]") {
                if (!aa) {
                    ae.push.apply(ae, al)
                } else {
                    if (X.nodeType === 1) {
                        for (var ad = 0; al[ad] != null; ad++) {
                            if (al[ad] && (al[ad] === true || al[ad].nodeType === 1 && N(X, al[ad]))) {
                                ae.push(ai[ad])
                            }
                        }
                    } else {
                        for (var ad = 0; al[ad] != null; ad++) {
                            if (al[ad] && al[ad].nodeType === 1) {
                                ae.push(ai[ad])
                            }
                        }
                    }
                }
            } else {
                H(al, ae)
            }
            if (Y) {
                I(Y, X, ae, af);
                if (J) {
                    hasDuplicate = false;
                    ae.sort(J);
                    if (hasDuplicate) {
                        for (var ad = 1; ad < ae.length; ad++) {
                            if (ae[ad] === ae[ad - 1]) {
                                ae.splice(ad--, 1)
                            }
                        }
                    }
                }
            }
            return ae
        };
        I.matches = function(W, X) {
            return I(W, null, null, X)
        };
        I.find = function(ad, W, ae) {
            var ac, aa;
            if (!ad) {
                return []
            }
            for (var Z = 0, Y = L.order.length; Z < Y; Z++) {
                var ab = L.order[Z],
                    aa;
                if ((aa = L.match[ab].exec(ad))) {
                    var X = RegExp.leftContext;
                    if (X.substr(X.length - 1) !== "\\") {
                        aa[1] = (aa[1] || "").replace(/\\/g, "");
                        ac = L.find[ab](aa, W, ae);
                        if (ac != null) {
                            ad = ad.replace(L.match[ab], "");
                            break
                        }
                    }
                }
            }
            if (!ac) {
                ac = W.getElementsByTagName("*")
            }
            return {
                set: ac,
                expr: ad
            }
        };
        I.filter = function(ag, af, aj, Z) {
            var Y = ag,
                al = [],
                ad = af,
                ab, W, ac = af && af[0] && T(af[0]);
            while (ag && af.length) {
                for (var ae in L.filter) {
                    if ((ab = L.match[ae].exec(ag)) != null) {
                        var X = L.filter[ae],
                            ak, ai;
                        W = false;
                        if (ad == al) {
                            al = []
                        }
                        if (L.preFilter[ae]) {
                            ab = L.preFilter[ae](ab, ad, aj, al, Z, ac);
                            if (!ab) {
                                W = ak = true
                            } else {
                                if (ab === true) {
                                    continue
                                }
                            }
                        }
                        if (ab) {
                            for (var aa = 0;
                                (ai = ad[aa]) != null; aa++) {
                                if (ai) {
                                    ak = X(ai, ab, aa, ad);
                                    var ah = Z ^ !!ak;
                                    if (aj && ak != null) {
                                        if (ah) {
                                            W = true
                                        } else {
                                            ad[aa] = false
                                        }
                                    } else {
                                        if (ah) {
                                            al.push(ai);
                                            W = true
                                        }
                                    }
                                }
                            }
                        }
                        if (ak !== g) {
                            if (!aj) {
                                ad = al
                            }
                            ag = ag.replace(L.match[ae], "");
                            if (!W) {
                                return []
                            }
                            break
                        }
                    }
                }
                if (ag == Y) {
                    if (W == null) {
                        throw "Syntax error, unrecognized expression: " + ag
                    } else {
                        break
                    }
                }
                Y = ag
            }
            return ad
        };
        var L = I.selectors = {
            order: ["ID", "NAME", "TAG"],
            match: {
                ID: /#((?:[\w\u00c0-\uFFFF_-]|\\.)+)/,
                CLASS: /\.((?:[\w\u00c0-\uFFFF_-]|\\.)+)/,
                NAME: /\[name=['"]*((?:[\w\u00c0-\uFFFF_-]|\\.)+)['"]*\]/,
                ATTR: /\[\s*((?:[\w\u00c0-\uFFFF_-]|\\.)+)\s*(?:(\S?=)\s*(['"]*)(.*?)\3|)\s*\]/,
                TAG: /^((?:[\w\u00c0-\uFFFF\*_-]|\\.)+)/,
                CHILD: /:(only|nth|last|first)-child(?:\((even|odd|[\dn+-]*)\))?/,
                POS: /:(nth|eq|gt|lt|first|last|even|odd)(?:\((\d*)\))?(?=[^-]|$)/,
                PSEUDO: /:((?:[\w\u00c0-\uFFFF_-]|\\.)+)(?:\((['"]*)((?:\([^\)]+\)|[^\2\(\)]*)+)\2\))?/
            },
            attrMap: {
                "class": "className",
                "for": "htmlFor"
            },
            attrHandle: {
                href: function(W) {
                    return W.getAttribute("href")
                }
            },
            relative: {
                "+": function(ad, W, ac) {
                    var aa = typeof W === "string",
                        ae = aa && !/\W/.test(W),
                        ab = aa && !ae;
                    if (ae && !ac) {
                        W = W.toUpperCase()
                    }
                    for (var Z = 0, Y = ad.length, X; Z < Y; Z++) {
                        if ((X = ad[Z])) {
                            while ((X = X.previousSibling) && X.nodeType !== 1) {}
                            ad[Z] = ab || X && X.nodeName === W ? X || false : X === W
                        }
                    }
                    if (ab) {
                        I.filter(W, ad, true)
                    }
                },
                ">": function(ac, X, ad) {
                    var aa = typeof X === "string";
                    if (aa && !/\W/.test(X)) {
                        X = ad ? X : X.toUpperCase();
                        for (var Y = 0, W = ac.length; Y < W; Y++) {
                            var ab = ac[Y];
                            if (ab) {
                                var Z = ab.parentNode;
                                ac[Y] = Z.nodeName === X ? Z : false
                            }
                        }
                    } else {
                        for (var Y = 0, W = ac.length; Y < W; Y++) {
                            var ab = ac[Y];
                            if (ab) {
                                ac[Y] = aa ? ab.parentNode : ab.parentNode === X
                            }
                        }
                        if (aa) {
                            I.filter(X, ac, true)
                        }
                    }
                },
                "": function(Z, X, ab) {
                    var Y = O++,
                        W = V;
                    if (!X.match(/\W/)) {
                        var aa = X = ab ? X : X.toUpperCase();
                        W = S
                    }
                    W("parentNode", X, Y, Z, aa, ab)
                },
                "~": function(Z, X, ab) {
                    var Y = O++,
                        W = V;
                    if (typeof X === "string" && !X.match(/\W/)) {
                        var aa = X = ab ? X : X.toUpperCase();
                        W = S
                    }
                    W("previousSibling", X, Y, Z, aa, ab)
                }
            },
            find: {
                ID: function(X, Y, Z) {
                    if (typeof Y.getElementById !== "undefined" && !Z) {
                        var W = Y.getElementById(X[1]);
                        return W ? [W] : []
                    }
                },
                NAME: function(Y, ab, ac) {
                    if (typeof ab.getElementsByName !== "undefined") {
                        var X = [],
                            aa = ab.getElementsByName(Y[1]);
                        for (var Z = 0, W = aa.length; Z < W; Z++) {
                            if (aa[Z].getAttribute("name") === Y[1]) {
                                X.push(aa[Z])
                            }
                        }
                        return X.length === 0 ? null : X
                    }
                },
                TAG: function(W, X) {
                    return X.getElementsByTagName(W[1])
                }
            },
            preFilter: {
                CLASS: function(Z, X, Y, W, ac, ad) {
                    Z = " " + Z[1].replace(/\\/g, "") + " ";
                    if (ad) {
                        return Z
                    }
                    for (var aa = 0, ab;
                        (ab = X[aa]) != null; aa++) {
                        if (ab) {
                            if (ac ^ (ab.className && (" " + ab.className + " ").indexOf(Z) >= 0)) {
                                if (!Y) {
                                    W.push(ab)
                                }
                            } else {
                                if (Y) {
                                    X[aa] = false
                                }
                            }
                        }
                    }
                    return false
                },
                ID: function(W) {
                    return W[1].replace(/\\/g, "")
                },
                TAG: function(X, W) {
                    for (var Y = 0; W[Y] === false; Y++) {}
                    return W[Y] && T(W[Y]) ? X[1] : X[1].toUpperCase()
                },
                CHILD: function(W) {
                    if (W[1] == "nth") {
                        var X = /(-?)(\d*)n((?:\+|-)?\d*)/.exec(W[2] == "even" && "2n" || W[2] == "odd" && "2n+1" || !/\D/.test(W[2]) && "0n+" + W[2] || W[2]);
                        W[2] = (X[1] + (X[2] || 1)) - 0;
                        W[3] = X[3] - 0
                    }
                    W[0] = O++;
                    return W
                },
                ATTR: function(aa, X, Y, W, ab, ac) {
                    var Z = aa[1].replace(/\\/g, "");
                    if (!ac && L.attrMap[Z]) {
                        aa[1] = L.attrMap[Z]
                    }
                    if (aa[2] === "~=") {
                        aa[4] = " " + aa[4] + " "
                    }
                    return aa
                },
                PSEUDO: function(aa, X, Y, W, ab) {
                    if (aa[1] === "not") {
                        if (aa[3].match(U).length > 1 || /^\w/.test(aa[3])) {
                            aa[3] = I(aa[3], null, null, X)
                        } else {
                            var Z = I.filter(aa[3], X, Y, true ^ ab);
                            if (!Y) {
                                W.push.apply(W, Z)
                            }
                            return false
                        }
                    } else {
                        if (L.match.POS.test(aa[0]) || L.match.CHILD.test(aa[0])) {
                            return true
                        }
                    }
                    return aa
                },
                POS: function(W) {
                    W.unshift(true);
                    return W
                }
            },
            filters: {
                enabled: function(W) {
                    return W.disabled === false && W.type !== "hidden"
                },
                disabled: function(W) {
                    return W.disabled === true
                },
                checked: function(W) {
                    return W.checked === true
                },
                selected: function(W) {
                    W.parentNode.selectedIndex;
                    return W.selected === true
                },
                parent: function(W) {
                    return !!W.firstChild
                },
                empty: function(W) {
                    return !W.firstChild
                },
                has: function(Y, X, W) {
                    return !!I(W[3], Y).length
                },
                header: function(W) {
                    return /h\d/i.test(W.nodeName)
                },
                text: function(W) {
                    return "text" === W.type
                },
                radio: function(W) {
                    return "radio" === W.type
                },
                checkbox: function(W) {
                    return "checkbox" === W.type
                },
                file: function(W) {
                    return "file" === W.type
                },
                password: function(W) {
                    return "password" === W.type
                },
                submit: function(W) {
                    return "submit" === W.type
                },
                image: function(W) {
                    return "image" === W.type
                },
                reset: function(W) {
                    return "reset" === W.type
                },
                button: function(W) {
                    return "button" === W.type || W.nodeName.toUpperCase() === "BUTTON"
                },
                input: function(W) {
                    return /input|select|textarea|button/i.test(W.nodeName)
                }
            },
            setFilters: {
                first: function(X, W) {
                    return W === 0
                },
                last: function(Y, X, W, Z) {
                    return X === Z.length - 1
                },
                even: function(X, W) {
                    return W % 2 === 0
                },
                odd: function(X, W) {
                    return W % 2 === 1
                },
                lt: function(Y, X, W) {
                    return X < W[3] - 0
                },
                gt: function(Y, X, W) {
                    return X > W[3] - 0
                },
                nth: function(Y, X, W) {
                    return W[3] - 0 == X
                },
                eq: function(Y, X, W) {
                    return W[3] - 0 == X
                }
            },
            filter: {
                PSEUDO: function(ac, Y, Z, ad) {
                    var X = Y[1],
                        aa = L.filters[X];
                    if (aa) {
                        return aa(ac, Z, Y, ad)
                    } else {
                        if (X === "contains") {
                            return (ac.textContent || ac.innerText || "").indexOf(Y[3]) >= 0
                        } else {
                            if (X === "not") {
                                var ab = Y[3];
                                for (var Z = 0, W = ab.length; Z < W; Z++) {
                                    if (ab[Z] === ac) {
                                        return false
                                    }
                                }
                                return true
                            }
                        }
                    }
                },
                CHILD: function(W, Z) {
                    var ac = Z[1],
                        X = W;
                    switch (ac) {
                        case "only":
                        case "first":
                            while (X = X.previousSibling) {
                                if (X.nodeType === 1) {
                                    return false
                                }
                            }
                            if (ac == "first") {
                                return true
                            }
                            X = W;
                        case "last":
                            while (X = X.nextSibling) {
                                if (X.nodeType === 1) {
                                    return false
                                }
                            }
                            return true;
                        case "nth":
                            var Y = Z[2],
                                af = Z[3];
                            if (Y == 1 && af == 0) {
                                return true
                            }
                            var ab = Z[0],
                                ae = W.parentNode;
                            if (ae && (ae.sizcache !== ab || !W.nodeIndex)) {
                                var aa = 0;
                                for (X = ae.firstChild; X; X = X.nextSibling) {
                                    if (X.nodeType === 1) {
                                        X.nodeIndex = ++aa
                                    }
                                }
                                ae.sizcache = ab
                            }
                            var ad = W.nodeIndex - af;
                            if (Y == 0) {
                                return ad == 0
                            } else {
                                return (ad % Y == 0 && ad / Y >= 0)
                            }
                    }
                },
                ID: function(X, W) {
                    return X.nodeType === 1 && X.getAttribute("id") === W
                },
                TAG: function(X, W) {
                    return (W === "*" && X.nodeType === 1) || X.nodeName === W
                },
                CLASS: function(X, W) {
                    return (" " + (X.className || X.getAttribute("class")) + " ").indexOf(W) > -1
                },
                ATTR: function(ab, Z) {
                    var Y = Z[1],
                        W = L.attrHandle[Y] ? L.attrHandle[Y](ab) : ab[Y] != null ? ab[Y] : ab.getAttribute(Y),
                        ac = W + "",
                        aa = Z[2],
                        X = Z[4];
                    return W == null ? aa === "!=" : aa === "=" ? ac === X : aa === "*=" ? ac.indexOf(X) >= 0 : aa === "~=" ? (" " + ac + " ").indexOf(X) >= 0 : !X ? ac && W !== false : aa === "!=" ? ac != X : aa === "^=" ? ac.indexOf(X) === 0 : aa === "$=" ? ac.substr(ac.length - X.length) === X : aa === "|=" ? ac === X || ac.substr(0, X.length + 1) === X + "-" : false
                },
                POS: function(aa, X, Y, ab) {
                    var W = X[2],
                        Z = L.setFilters[W];
                    if (Z) {
                        return Z(aa, Y, X, ab)
                    }
                }
            }
        };
        var P = L.match.POS;
        for (var R in L.match) {
            L.match[R] = RegExp(L.match[R].source + /(?![^\[]*\])(?![^\(]*\))/.source)
        }
        var H = function(X, W) {
            X = Array.prototype.slice.call(X);
            if (W) {
                W.push.apply(W, X);
                return W
            }
            return X
        };
        try {
            Array.prototype.slice.call(document.documentElement.childNodes)
        } catch (Q) {
            H = function(aa, Z) {
                var X = Z || [];
                if (K.call(aa) === "[object Array]") {
                    Array.prototype.push.apply(X, aa)
                } else {
                    if (typeof aa.length === "number") {
                        for (var Y = 0, W = aa.length; Y < W; Y++) {
                            X.push(aa[Y])
                        }
                    } else {
                        for (var Y = 0; aa[Y]; Y++) {
                            X.push(aa[Y])
                        }
                    }
                }
                return X
            }
        }
        var J;
        if (document.documentElement.compareDocumentPosition) {
            J = function(X, W) {
                var Y = X.compareDocumentPosition(W) & 4 ? -1 : X === W ? 0 : 1;
                if (Y === 0) {
                    hasDuplicate = true
                }
                return Y
            }
        } else {
            if ("sourceIndex" in document.documentElement) {
                J = function(X, W) {
                    var Y = X.sourceIndex - W.sourceIndex;
                    if (Y === 0) {
                        hasDuplicate = true
                    }
                    return Y
                }
            } else {
                if (document.createRange) {
                    J = function(Z, X) {
                        var Y = Z.ownerDocument.createRange(),
                            W = X.ownerDocument.createRange();
                        Y.selectNode(Z);
                        Y.collapse(true);
                        W.selectNode(X);
                        W.collapse(true);
                        var aa = Y.compareBoundaryPoints(Range.START_TO_END, W);
                        if (aa === 0) {
                            hasDuplicate = true
                        }
                        return aa
                    }
                }
            }
        }(function() {
            var X = document.createElement("form"),
                Y = "script" + (new Date).getTime();
            X.innerHTML = "<input name='" + Y + "'/>";
            var W = document.documentElement;
            W.insertBefore(X, W.firstChild);
            if (!!document.getElementById(Y)) {
                L.find.ID = function(aa, ab, ac) {
                    if (typeof ab.getElementById !== "undefined" && !ac) {
                        var Z = ab.getElementById(aa[1]);
                        return Z ? Z.id === aa[1] || typeof Z.getAttributeNode !== "undefined" && Z.getAttributeNode("id").nodeValue === aa[1] ? [Z] : g : []
                    }
                };
                L.filter.ID = function(ab, Z) {
                    var aa = typeof ab.getAttributeNode !== "undefined" && ab.getAttributeNode("id");
                    return ab.nodeType === 1 && aa && aa.nodeValue === Z
                }
            }
            W.removeChild(X)
        })();
        (function() {
            var W = document.createElement("div");
            W.appendChild(document.createComment(""));
            if (W.getElementsByTagName("*").length > 0) {
                L.find.TAG = function(X, ab) {
                    var aa = ab.getElementsByTagName(X[1]);
                    if (X[1] === "*") {
                        var Z = [];
                        for (var Y = 0; aa[Y]; Y++) {
                            if (aa[Y].nodeType === 1) {
                                Z.push(aa[Y])
                            }
                        }
                        aa = Z
                    }
                    return aa
                }
            }
            W.innerHTML = "<a href='#'></a>";
            if (W.firstChild && typeof W.firstChild.getAttribute !== "undefined" && W.firstChild.getAttribute("href") !== "#") {
                L.attrHandle.href = function(X) {
                    return X.getAttribute("href", 2)
                }
            }
        })();
        if (document.querySelectorAll) {
            (function() {
                var W = I,
                    X = document.createElement("div");
                X.innerHTML = "<p class='TEST'></p>";
                if (X.querySelectorAll && X.querySelectorAll(".TEST").length === 0) {
                    return
                }
                I = function(ab, aa, Y, Z) {
                    aa = aa || document;
                    if (!Z && aa.nodeType === 9 && !T(aa)) {
                        try {
                            return H(aa.querySelectorAll(ab), Y)
                        } catch (ac) {}
                    }
                    return W(ab, aa, Y, Z)
                };
                I.find = W.find;
                I.filter = W.filter;
                I.selectors = W.selectors;
                I.matches = W.matches
            })()
        }
        if (document.getElementsByClassName && document.documentElement.getElementsByClassName) {
            (function() {
                var W = document.createElement("div");
                W.innerHTML = "<div class='test e'></div><div class='test'></div>";
                if (W.getElementsByClassName("e").length === 0) {
                    return
                }
                W.lastChild.className = "e";
                if (W.getElementsByClassName("e").length === 1) {
                    return
                }
                L.order.splice(1, 0, "CLASS");
                L.find.CLASS = function(X, Y, Z) {
                    if (typeof Y.getElementsByClassName !== "undefined" && !Z) {
                        return Y.getElementsByClassName(X[1])
                    }
                }
            })()
        }

        function S(X, ac, ab, ag, ad, af) {
            var ae = X == "previousSibling" && !af;
            for (var Z = 0, Y = ag.length; Z < Y; Z++) {
                var W = ag[Z];
                if (W) {
                    if (ae && W.nodeType === 1) {
                        W.sizcache = ab;
                        W.sizset = Z
                    }
                    W = W[X];
                    var aa = false;
                    while (W) {
                        if (W.sizcache === ab) {
                            aa = ag[W.sizset];
                            break
                        }
                        if (W.nodeType === 1 && !af) {
                            W.sizcache = ab;
                            W.sizset = Z
                        }
                        if (W.nodeName === ac) {
                            aa = W;
                            break
                        }
                        W = W[X]
                    }
                    ag[Z] = aa
                }
            }
        }

        function V(X, ac, ab, ag, ad, af) {
            var ae = X == "previousSibling" && !af;
            for (var Z = 0, Y = ag.length; Z < Y; Z++) {
                var W = ag[Z];
                if (W) {
                    if (ae && W.nodeType === 1) {
                        W.sizcache = ab;
                        W.sizset = Z
                    }
                    W = W[X];
                    var aa = false;
                    while (W) {
                        if (W.sizcache === ab) {
                            aa = ag[W.sizset];
                            break
                        }
                        if (W.nodeType === 1) {
                            if (!af) {
                                W.sizcache = ab;
                                W.sizset = Z
                            }
                            if (typeof ac !== "string") {
                                if (W === ac) {
                                    aa = true;
                                    break
                                }
                            } else {
                                if (I.filter(ac, [W]).length > 0) {
                                    aa = W;
                                    break
                                }
                            }
                        }
                        W = W[X]
                    }
                    ag[Z] = aa
                }
            }
        }
        var N = document.compareDocumentPosition ? function(X, W) {
            return X.compareDocumentPosition(W) & 16
        } : function(X, W) {
            return X !== W && (X.contains ? X.contains(W) : true)
        };
        var T = function(W) {
            return W.nodeType === 9 && W.documentElement.nodeName !== "HTML" || !!W.ownerDocument && T(W.ownerDocument)
        };
        var M = function(W, ad) {
            var Z = [],
                aa = "",
                ab, Y = ad.nodeType ? [ad] : ad;
            while ((ab = L.match.PSEUDO.exec(W))) {
                aa += ab[0];
                W = W.replace(L.match.PSEUDO, "")
            }
            W = L.relative[W] ? W + "*" : W;
            for (var ac = 0, X = Y.length; ac < X; ac++) {
                I(W, Y[ac], Z)
            }
            return I.filter(aa, Z)
        };
        q.find = I;
        q.filter = I.filter;
        q.expr = I.selectors;
        q.expr[":"] = q.expr.filters;
        I.selectors.filters.hidden = function(W) {
            return W.offsetWidth === 0 || W.offsetHeight === 0
        };
        I.selectors.filters.visible = function(W) {
            return W.offsetWidth > 0 || W.offsetHeight > 0
        };
        I.selectors.filters.animated = function(W) {
            return q.grep(q.timers, function(X) {
                return W === X.elem
            }).length
        };
        q.multiFilter = function(Y, W, X) {
            if (X) {
                Y = ":not(" + Y + ")"
            }
            return I.matches(Y, W)
        };
        q.dir = function(Y, X) {
            var W = [],
                Z = Y[X];
            while (Z && Z != document) {
                if (Z.nodeType == 1) {
                    W.push(Z)
                }
                Z = Z[X]
            }
            return W
        };
        q.nth = function(aa, W, Y, Z) {
            W = W || 1;
            var X = 0;
            for (; aa; aa = aa[Y]) {
                if (aa.nodeType == 1 && ++X == W) {
                    break
                }
            }
            return aa
        };
        q.sibling = function(Y, X) {
            var W = [];
            for (; Y; Y = Y.nextSibling) {
                if (Y.nodeType == 1 && Y != X) {
                    W.push(Y)
                }
            }
            return W
        };
        return;
        m.Sizzle = I
    })();
    q.event = {
        add: function(L, I, K, N) {
            if (L.nodeType == 3 || L.nodeType == 8) {
                return
            }
            if (L.setInterval && L != m) {
                L = m
            }
            if (!K.guid) {
                K.guid = this.guid++
            }
            if (N !== g) {
                var J = K;
                K = this.proxy(J);
                K.data = N
            }
            var H = q.data(L, "events") || q.data(L, "events", {}),
                M = q.data(L, "handle") || q.data(L, "handle", function() {
                    return typeof q !== "undefined" && !q.event.triggered ? q.event.handle.apply(arguments.callee.elem, arguments) : g
                });
            M.elem = L;
            q.each(I.split(/\s+/), function(P, Q) {
                var R = Q.split(".");
                Q = R.shift();
                K.type = R.slice().sort().join(".");
                var O = H[Q];
                if (q.event.specialAll[Q]) {
                    q.event.specialAll[Q].setup.call(L, N, R)
                }
                if (!O) {
                    O = H[Q] = {};
                    if (!q.event.special[Q] || q.event.special[Q].setup.call(L, N, R) === false) {
                        if (L.addEventListener) {
                            L.addEventListener(Q, M, false)
                        } else {
                            if (L.attachEvent) {
                                L.attachEvent("on" + Q, M)
                            }
                        }
                    }
                }
                O[K.guid] = K;
                q.event.global[Q] = true
            });
            L = null
        },
        guid: 1,
        global: {},
        remove: function(N, K, M) {
            if (N.nodeType == 3 || N.nodeType == 8) {
                return
            }
            var J = q.data(N, "events"),
                I, H;
            if (J) {
                if (K === g || (typeof K === "string" && K.charAt(0) == ".")) {
                    for (var L in J) {
                        this.remove(N, L + (K || ""))
                    }
                } else {
                    if (K.type) {
                        M = K.handler;
                        K = K.type
                    }
                    q.each(K.split(/\s+/), function(P, R) {
                        var T = R.split(".");
                        R = T.shift();
                        var Q = RegExp("(^|\\.)" + T.slice().sort().join(".*\\.") + "(\\.|$)");
                        if (J[R]) {
                            if (M) {
                                delete J[R][M.guid]
                            } else {
                                for (var S in J[R]) {
                                    if (Q.test(J[R][S].type)) {
                                        delete J[R][S]
                                    }
                                }
                            }
                            if (q.event.specialAll[R]) {
                                q.event.specialAll[R].teardown.call(N, T)
                            }
                            for (I in J[R]) {
                                break
                            }
                            if (!I) {
                                if (!q.event.special[R] || q.event.special[R].teardown.call(N, T) === false) {
                                    if (N.removeEventListener) {
                                        N.removeEventListener(R, q.data(N, "handle"), false)
                                    } else {
                                        if (N.detachEvent) {
                                            N.detachEvent("on" + R, q.data(N, "handle"))
                                        }
                                    }
                                }
                                I = null;
                                delete J[R]
                            }
                        }
                    })
                }
                for (I in J) {
                    break
                }
                if (!I) {
                    var O = q.data(N, "handle");
                    if (O) {
                        O.elem = null
                    }
                    q.removeData(N, "events");
                    q.removeData(N, "handle")
                }
            }
        },
        trigger: function(L, N, K, H) {
            var J = L.type || L;
            if (!H) {
                L = typeof L === "object" ? L[h] ? L : q.extend(q.Event(J), L) : q.Event(J);
                if (J.indexOf("!") >= 0) {
                    L.type = J = J.slice(0, -1);
                    L.exclusive = true
                }
                if (!K) {
                    L.stopPropagation();
                    if (this.global[J]) {
                        q.each(q.cache, function() {
                            if (this.events && this.events[J]) {
                                q.event.trigger(L, N, this.handle.elem)
                            }
                        })
                    }
                }
                if (!K || K.nodeType == 3 || K.nodeType == 8) {
                    return g
                }
                L.result = g;
                L.target = K;
                N = q.makeArray(N);
                N.unshift(L)
            }
            L.currentTarget = K;
            var M = q.data(K, "handle");
            if (M) {
                M.apply(K, N)
            }
            if ((!K[J] || (q.nodeName(K, "a") && J == "click")) && K["on" + J] && K["on" + J].apply(K, N) === false) {
                L.result = false
            }
            if (!H && K[J] && !L.isDefaultPrevented() && !(q.nodeName(K, "a") && J == "click")) {
                this.triggered = true;
                try {
                    K[J]()
                } catch (O) {}
            }
            this.triggered = false;
            if (!L.isPropagationStopped()) {
                var I = K.parentNode || K.ownerDocument;
                if (I) {
                    q.event.trigger(L, N, I, true)
                }
            }
        },
        handle: function(N) {
            var M, H;
            N = arguments[0] = q.event.fix(N || m.event);
            N.currentTarget = this;
            var O = N.type.split(".");
            N.type = O.shift();
            M = !O.length && !N.exclusive;
            var L = RegExp("(^|\\.)" + O.slice().sort().join(".*\\.") + "(\\.|$)");
            H = (q.data(this, "events") || {})[N.type];
            for (var J in H) {
                var K = H[J];
                if (M || L.test(K.type)) {
                    N.handler = K;
                    N.data = K.data;
                    var I = K.apply(this, arguments);
                    if (I !== g) {
                        N.result = I;
                        if (I === false) {
                            N.preventDefault();
                            N.stopPropagation()
                        }
                    }
                    if (N.isImmediatePropagationStopped()) {
                        break
                    }
                }
            }
        },
        props: "altKey attrChange attrName bubbles button cancelable charCode clientX clientY ctrlKey currentTarget data detail eventPhase fromElement handler keyCode metaKey newValue originalTarget pageX pageY prevValue relatedNode relatedTarget screenX screenY shiftKey srcElement target toElement view wheelDelta which".split(" "),
        fix: function(K) {
            if (K[h]) {
                return K
            }
            var I = K;
            K = q.Event(I);
            for (var J = this.props.length, M; J;) {
                M = this.props[--J];
                K[M] = I[M]
            }
            if (!K.target) {
                K.target = K.srcElement || document
            }
            if (K.target.nodeType == 3) {
                K.target = K.target.parentNode
            }
            if (!K.relatedTarget && K.fromElement) {
                K.relatedTarget = K.fromElement == K.target ? K.toElement : K.fromElement
            }
            if (K.pageX == null && K.clientX != null) {
                var L = document.documentElement,
                    H = document.body;
                K.pageX = K.clientX + (L && L.scrollLeft || H && H.scrollLeft || 0) - (L.clientLeft || 0);
                K.pageY = K.clientY + (L && L.scrollTop || H && H.scrollTop || 0) - (L.clientTop || 0)
            }
            if (!K.which && ((K.charCode || K.charCode === 0) ? K.charCode : K.keyCode)) {
                K.which = K.charCode || K.keyCode
            }
            if (!K.metaKey && K.ctrlKey) {
                K.metaKey = K.ctrlKey
            }
            if (!K.which && K.button) {
                K.which = (K.button & 1 ? 1 : (K.button & 2 ? 3 : (K.button & 4 ? 2 : 0)))
            }
            return K
        },
        proxy: function(I, H) {
            H = H || function() {
                return I.apply(this, arguments)
            };
            H.guid = I.guid = I.guid || H.guid || this.guid++;
            return H
        },
        special: {
            ready: {
                setup: E,
                teardown: function() {}
            }
        },
        specialAll: {
            live: {
                setup: function(H, I) {
                    q.event.add(this, I[0], c)
                },
                teardown: function(J) {
                    if (J.length) {
                        var H = 0,
                            I = RegExp("(^|\\.)" + J[0] + "(\\.|$)");
                        q.each((q.data(this, "events").live || {}), function() {
                            if (I.test(this.type)) {
                                H++
                            }
                        });
                        if (H < 1) {
                            q.event.remove(this, J[0], c)
                        }
                    }
                }
            }
        }
    };
    q.Event = function(H) {
        if (!this.preventDefault) {
            return new q.Event(H)
        }
        if (H && H.type) {
            this.originalEvent = H;
            this.type = H.type
        } else {
            this.type = H
        }
        this.timeStamp = e();
        this[h] = true
    };

    function l() {
        return false
    }

    function x() {
        return true
    }
    q.Event.prototype = {
        preventDefault: function() {
            this.isDefaultPrevented = x;
            var H = this.originalEvent;
            if (!H) {
                return
            }
            if (H.preventDefault) {
                H.preventDefault()
            }
            H.returnValue = false
        },
        stopPropagation: function() {
            this.isPropagationStopped = x;
            var H = this.originalEvent;
            if (!H) {
                return
            }
            if (H.stopPropagation) {
                H.stopPropagation()
            }
            H.cancelBubble = true
        },
        stopImmediatePropagation: function() {
            this.isImmediatePropagationStopped = x;
            this.stopPropagation()
        },
        isDefaultPrevented: l,
        isPropagationStopped: l,
        isImmediatePropagationStopped: l
    };
    var a = function(I) {
        var H = I.relatedTarget;
        while (H && H != this) {
            try {
                H = H.parentNode
            } catch (J) {
                H = this
            }
        }
        if (H != this) {
            I.type = I.data;
            q.event.handle.apply(this, arguments)
        }
    };
    q.each({
        mouseover: "mouseenter",
        mouseout: "mouseleave"
    }, function(I, H) {
        q.event.special[H] = {
            setup: function() {
                q.event.add(this, I, a, H)
            },
            teardown: function() {
                q.event.remove(this, I, a)
            }
        }
    });
    q.fn.extend({
        bind: function(I, J, H) {
            return I == "unload" ? this.one(I, J, H) : this.each(function() {
                q.event.add(this, I, H || J, H && J)
            })
        },
        one: function(J, K, I) {
            var H = q.event.proxy(I || K, function(L) {
                q(this).unbind(L, H);
                return (I || K).apply(this, arguments)
            });
            return this.each(function() {
                q.event.add(this, J, H, I && K)
            })
        },
        unbind: function(I, H) {
            return this.each(function() {
                q.event.remove(this, I, H)
            })
        },
        trigger: function(H, I) {
            return this.each(function() {
                q.event.trigger(H, I, this)
            })
        },
        triggerHandler: function(H, J) {
            if (this[0]) {
                var I = q.Event(H);
                I.preventDefault();
                I.stopPropagation();
                q.event.trigger(I, J, this[0]);
                return I.result
            }
        },
        toggle: function(J) {
            var H = arguments,
                I = 1;
            while (I < H.length) {
                q.event.proxy(J, H[I++])
            }
            return this.click(q.event.proxy(J, function(K) {
                this.lastToggle = (this.lastToggle || 0) % I;
                K.preventDefault();
                return H[this.lastToggle++].apply(this, arguments) || false
            }))
        },
        hover: function(H, I) {
            return this.mouseenter(H).mouseleave(I)
        },
        ready: function(H) {
            E();
            if (q.isReady) {
                H.call(document, q)
            } else {
                q.readyList.push(H)
            }
            return this
        },
        live: function(J, I) {
            var H = q.event.proxy(I);
            H.guid += this.selector + J;
            q(document).bind(j(J, this.selector), this.selector, H);
            return this
        },
        die: function(I, H) {
            q(document).unbind(j(I, this.selector), H ? {
                guid: H.guid + this.selector + I
            } : null);
            return this
        }
    });

    function c(K) {
        var H = RegExp("(^|\\.)" + K.type + "(\\.|$)"),
            J = true,
            I = [];
        q.each(q.data(this, "events").live || [], function(L, M) {
            if (H.test(M.type)) {
                var N = q(K.target).closest(M.data)[0];
                if (N) {
                    I.push({
                        elem: N,
                        fn: M
                    })
                }
            }
        });
        I.sort(function(M, L) {
            return q.data(M.elem, "closest") - q.data(L.elem, "closest")
        });
        q.each(I, function() {
            if (this.fn.call(this.elem, K, this.fn.data) === false) {
                return (J = false)
            }
        });
        return J
    }

    function j(I, H) {
        return ["live", I, H.replace(/\./g, "`").replace(/ /g, "|")].join(".")
    }
    q.extend({
        isReady: false,
        readyList: [],
        ready: function() {
            if (!q.isReady) {
                q.isReady = true;
                if (q.readyList) {
                    q.each(q.readyList, function() {
                        this.call(document, q)
                    });
                    q.readyList = null
                }
                q(document).triggerHandler("ready")
            }
        }
    });
    var A = false;

    function E() {
        if (A) {
            return
        }
        A = true;
        if (document.addEventListener) {
            document.addEventListener("DOMContentLoaded", function() {
                document.removeEventListener("DOMContentLoaded", arguments.callee, false);
                q.ready()
            }, false)
        } else {
            if (document.attachEvent) {
                document.attachEvent("onreadystatechange", function() {
                    if (document.readyState === "complete") {
                        document.detachEvent("onreadystatechange", arguments.callee);
                        q.ready()
                    }
                });
                if (document.documentElement.doScroll && m == m.top) {
                    (function() {
                        if (q.isReady) {
                            return
                        }
                        try {
                            document.documentElement.doScroll("left")
                        } catch (H) {
                            setTimeout(arguments.callee, 0);
                            return
                        }
                        q.ready()
                    })()
                }
            }
        }
        q.event.add(m, "load", q.ready)
    }
    q.each(("blur,focus,load,resize,scroll,unload,click,dblclick,mousedown,mouseup,mousemove,mouseover,mouseout,mouseenter,mouseleave,change,select,submit,keydown,keypress,keyup,error").split(","), function(I, H) {
        q.fn[H] = function(J) {
            return J ? this.bind(H, J) : this.trigger(H)
        }
    });
    q(m).bind("unload", function() {
        for (var H in q.cache) {
            if (H != 1 && q.cache[H].handle) {
                q.event.remove(q.cache[H].handle.elem)
            }
        }
    });
    (function() {
        q.support = {};
        var I = document.documentElement,
            J = document.createElement("script"),
            N = document.createElement("div"),
            M = "script" + (new Date).getTime();
        N.style.display = "none";
        N.innerHTML = '   <link/><table></table><a href="/a" style="color:red;float:left;opacity:.5;">a</a><select><option>text</option></select><object><param/></object>';
        var K = N.getElementsByTagName("*"),
            H = N.getElementsByTagName("a")[0];
        if (!K || !K.length || !H) {
            return
        }
        q.support = {
            leadingWhitespace: N.firstChild.nodeType == 3,
            tbody: !N.getElementsByTagName("tbody").length,
            objectAll: !!N.getElementsByTagName("object")[0].getElementsByTagName("*").length,
            htmlSerialize: !!N.getElementsByTagName("link").length,
            style: /red/.test(H.getAttribute("style")),
            hrefNormalized: H.getAttribute("href") === "/a",
            opacity: H.style.opacity === "0.5",
            cssFloat: !!H.style.cssFloat,
            scriptEval: false,
            noCloneEvent: true,
            boxModel: null
        };
        J.type = "text/javascript";
        try {
            J.appendChild(document.createTextNode("window." + M + "=1;"))
        } catch (L) {}
        I.insertBefore(J, I.firstChild);
        if (m[M]) {
            q.support.scriptEval = true;
            delete m[M]
        }
        I.removeChild(J);
        if (N.attachEvent && N.fireEvent) {
            N.attachEvent("onclick", function() {
                q.support.noCloneEvent = false;
                N.detachEvent("onclick", arguments.callee)
            });
            N.cloneNode(true).fireEvent("onclick")
        }
        q(function() {
            var O = document.createElement("div");
            O.style.width = O.style.paddingLeft = "1px";
            document.body.appendChild(O);
            q.boxModel = q.support.boxModel = O.offsetWidth === 2;
            document.body.removeChild(O).style.display = "none"
        })
    })();
    var z = q.support.cssFloat ? "cssFloat" : "styleFloat";
    q.props = {
        "for": "htmlFor",
        "class": "className",
        "float": z,
        cssFloat: z,
        styleFloat: z,
        readonly: "readOnly",
        maxlength: "maxLength",
        cellspacing: "cellSpacing",
        rowspan: "rowSpan",
        tabindex: "tabIndex"
    };
    q.fn.extend({
        _load: q.fn.load,
        load: function(J, M, N) {
            if (typeof J !== "string") {
                return this._load(J)
            }
            var L = J.indexOf(" ");
            if (L >= 0) {
                var H = J.slice(L, J.length);
                J = J.slice(0, L)
            }
            var K = "GET";
            if (M) {
                if (q.isFunction(M)) {
                    N = M;
                    M = null
                } else {
                    if (typeof M === "object") {
                        M = q.param(M);
                        K = "POST"
                    }
                }
            }
            var I = this;
            q.ajax({
                url: J,
                type: K,
                dataType: "html",
                data: M,
                complete: function(P, O) {
                    if (O == "success" || O == "notmodified") {
                        I.html(H ? q("<div/>").append(P.responseText.replace(/<script(.|\s)*?\/script>/g, "")).find(H) : P.responseText)
                    }
                    if (N) {
                        I.each(N, [P.responseText, O, P])
                    }
                }
            });
            return this
        },
        serialize: function() {
            return q.param(this.serializeArray())
        },
        serializeArray: function() {
            return this.map(function() {
                return this.elements ? q.makeArray(this.elements) : this
            }).filter(function() {
                return this.name && !this.disabled && (this.checked || /select|textarea/i.test(this.nodeName) || /text|hidden|password|search/i.test(this.type))
            }).map(function(H, I) {
                var J = q(this).val();
                return J == null ? null : q.isArray(J) ? q.map(J, function(L, K) {
                    return {
                        name: I.name,
                        value: L
                    }
                }) : {
                    name: I.name,
                    value: J
                }
            }).get()
        }
    });
    q.each("ajaxStart,ajaxStop,ajaxComplete,ajaxError,ajaxSuccess,ajaxSend".split(","), function(H, I) {
        q.fn[I] = function(J) {
            return this.bind(I, J)
        }
    });
    var t = e();
    q.extend({
        get: function(H, J, K, I) {
            if (q.isFunction(J)) {
                K = J;
                J = null
            }
            return q.ajax({
                type: "GET",
                url: H,
                data: J,
                success: K,
                dataType: I
            })
        },
        getScript: function(H, I) {
            return q.get(H, null, I, "script")
        },
        getJSON: function(H, I, J) {
            return q.get(H, I, J, "json")
        },
        post: function(H, J, K, I) {
            if (q.isFunction(J)) {
                K = J;
                J = {}
            }
            return q.ajax({
                type: "POST",
                url: H,
                data: J,
                success: K,
                dataType: I
            })
        },
        ajaxSetup: function(H) {
            q.extend(q.ajaxSettings, H)
        },
        ajaxSettings: {
            url: location.href,
            global: true,
            type: "GET",
            contentType: "application/x-www-form-urlencoded",
            processData: true,
            async: true,
            xhr: function() {
                return m.ActiveXObject ? new ActiveXObject("Microsoft.XMLHTTP") : new XMLHttpRequest()
            },
            accepts: {
                xml: "application/xml, text/xml",
                html: "text/html",
                script: "text/javascript, application/javascript",
                json: "application/json, text/javascript",
                text: "text/plain",
                _default: "*/*"
            }
        },
        lastModified: {},
        ajax: function(P) {
            P = q.extend(true, P, q.extend(true, {}, q.ajaxSettings, P));
            var Z, I = /=\?(&|$)/g,
                U, Y, J = P.type.toUpperCase();
            if (P.data && P.processData && typeof P.data !== "string") {
                P.data = q.param(P.data)
            }
            if (P.dataType == "jsonp") {
                if (J == "GET") {
                    if (!P.url.match(I)) {
                        P.url += (P.url.match(/\?/) ? "&" : "?") + (P.jsonp || "callback") + "=?"
                    }
                } else {
                    if (!P.data || !P.data.match(I)) {
                        P.data = (P.data ? P.data + "&" : "") + (P.jsonp || "callback") + "=?"
                    }
                }
                P.dataType = "json"
            }
            if (P.dataType == "json" && (P.data && P.data.match(I) || P.url.match(I))) {
                Z = "jsonp" + t++;
                if (P.data) {
                    P.data = (P.data + "").replace(I, "=" + Z + "$1")
                }
                P.url = P.url.replace(I, "=" + Z + "$1");
                P.dataType = "script";
                m[Z] = function(aa) {
                    Y = aa;
                    L();
                    O();
                    m[Z] = g;
                    try {
                        delete m[Z]
                    } catch (ab) {}
                    if (K) {
                        K.removeChild(W)
                    }
                }
            }
            if (P.dataType == "script" && P.cache == null) {
                P.cache = false
            }
            if (P.cache === false && J == "GET") {
                var H = e();
                var X = P.url.replace(/(\?|&)_=.*?(&|$)/, "$1_=" + H + "$2");
                P.url = X + ((X == P.url) ? (P.url.match(/\?/) ? "&" : "?") + "_=" + H : "")
            }
            if (P.data && J == "GET") {
                P.url += (P.url.match(/\?/) ? "&" : "?") + P.data;
                P.data = null
            }
            if (P.global && !q.active++) {
                q.event.trigger("ajaxStart")
            }
            var T = /^(\w+:)?\/\/([^\/?#]+)/.exec(P.url);
            if (P.dataType == "script" && J == "GET" && T && (T[1] && T[1] != location.protocol || T[2] != location.host)) {
                var K = document.getElementsByTagName("head")[0];
                var W = document.createElement("script");
                W.src = P.url;
                if (P.scriptCharset) {
                    W.charset = P.scriptCharset
                }
                if (!Z) {
                    var R = false;
                    W.onload = W.onreadystatechange = function() {
                        if (!R && (!this.readyState || this.readyState == "loaded" || this.readyState == "complete")) {
                            R = true;
                            L();
                            O();
                            W.onload = W.onreadystatechange = null;
                            K.removeChild(W)
                        }
                    }
                }
                K.appendChild(W);
                return g
            }
            var N = false;
            var M = P.xhr();
            if (P.username) {
                M.open(J, P.url, P.async, P.username, P.password)
            } else {
                M.open(J, P.url, P.async)
            }
            try {
                if (P.data) {
                    M.setRequestHeader("Content-Type", P.contentType)
                }
                if (P.ifModified) {
                    M.setRequestHeader("If-Modified-Since", q.lastModified[P.url] || "Thu, 01 Jan 1970 00:00:00 GMT")
                }
                M.setRequestHeader("X-Requested-With", "XMLHttpRequest");
                M.setRequestHeader("Accept", P.dataType && P.accepts[P.dataType] ? P.accepts[P.dataType] + ", */*" : P.accepts._default)
            } catch (V) {}
            if (P.beforeSend && P.beforeSend(M, P) === false) {
                if (P.global && !--q.active) {
                    q.event.trigger("ajaxStop")
                }
                M.abort();
                return false
            }
            if (P.global) {
                q.event.trigger("ajaxSend", [M, P])
            }
            var Q = function(aa) {
                if (M.readyState == 0) {
                    if (S) {
                        clearInterval(S);
                        S = null;
                        if (P.global && !--q.active) {
                            q.event.trigger("ajaxStop")
                        }
                    }
                } else {
                    if (!N && M && (M.readyState == 4 || aa == "timeout")) {
                        N = true;
                        if (S) {
                            clearInterval(S);
                            S = null
                        }
                        U = aa == "timeout" ? "timeout" : !q.httpSuccess(M) ? "error" : P.ifModified && q.httpNotModified(M, P.url) ? "notmodified" : "success";
                        if (U == "success") {
                            try {
                                Y = q.httpData(M, P.dataType, P)
                            } catch (ac) {
                                U = "parsererror"
                            }
                        }
                        if (U == "success") {
                            var ab;
                            try {
                                ab = M.getResponseHeader("Last-Modified")
                            } catch (ac) {}
                            if (P.ifModified && ab) {
                                q.lastModified[P.url] = ab
                            }
                            if (!Z) {
                                L()
                            }
                        } else {
                            q.handleError(P, M, U)
                        }
                        O();
                        if (aa) {
                            M.abort()
                        }
                        if (P.async) {
                            M = null
                        }
                    }
                }
            };
            if (P.async) {
                var S = setInterval(Q, 13);
                if (P.timeout > 0) {
                    setTimeout(function() {
                        if (M && !N) {
                            Q("timeout")
                        }
                    }, P.timeout)
                }
            }
            try {
                M.send(P.data)
            } catch (V) {
                q.handleError(P, M, null, V)
            }
            if (!P.async) {
                Q()
            }

            function L() {
                if (P.success) {
                    P.success(Y, U)
                }
                if (P.global) {
                    q.event.trigger("ajaxSuccess", [M, P])
                }
            }

            function O() {
                if (P.complete) {
                    P.complete(M, U)
                }
                if (P.global) {
                    q.event.trigger("ajaxComplete", [M, P])
                }
                if (P.global && !--q.active) {
                    q.event.trigger("ajaxStop")
                }
            }
            return M
        },
        handleError: function(I, K, H, J) {
            if (I.error) {
                I.error(K, H, J)
            }
            if (I.global) {
                q.event.trigger("ajaxError", [K, I, J])
            }
        },
        active: 0,
        httpSuccess: function(I) {
            try {
                return !I.status && location.protocol == "file:" || (I.status >= 200 && I.status < 300) || I.status == 304 || I.status == 1223
            } catch (H) {}
            return false
        },
        httpNotModified: function(J, H) {
            try {
                var K = J.getResponseHeader("Last-Modified");
                return J.status == 304 || K == q.lastModified[H]
            } catch (I) {}
            return false
        },
        httpData: function(M, K, J) {
            var I = M.getResponseHeader("content-type"),
                H = K == "xml" || !K && I && I.indexOf("xml") >= 0,
                L = H ? M.responseXML : M.responseText;
            if (H && L.documentElement.tagName == "parsererror") {
                throw "parsererror"
            }
            if (J && J.dataFilter) {
                L = J.dataFilter(L, K)
            }
            if (typeof L === "string") {
                if (K == "script") {
                    q.globalEval(L)
                }
                if (K == "json") {
                    L = m["eval"]("(" + L + ")")
                }
            }
            return L
        },
        param: function(H) {
            var J = [];

            function K(L, M) {
                J[J.length] = encodeURIComponent(L) + "=" + encodeURIComponent(M)
            }
            if (q.isArray(H) || H.jquery) {
                q.each(H, function() {
                    K(this.name, this.value)
                })
            } else {
                for (var I in H) {
                    if (q.isArray(H[I])) {
                        q.each(H[I], function() {
                            K(I, this)
                        })
                    } else {
                        K(I, q.isFunction(H[I]) ? H[I]() : H[I])
                    }
                }
            }
            return J.join("&").replace(/%20/g, "+")
        }
    });
    var n = {},
        o, d = [
            ["height", "marginTop", "marginBottom", "paddingTop", "paddingBottom"],
            ["width", "marginLeft", "marginRight", "paddingLeft", "paddingRight"],
            ["opacity"]
        ];

    function w(I, H) {
        var J = {};
        q.each(d.concat.apply([], d.slice(0, H)), function() {
            J[this] = I
        });
        return J
    }
    q.fn.extend({
        show: function(M, O) {
            if (M) {
                return this.animate(w("show", 3), M, O)
            } else {
                for (var K = 0, I = this.length; K < I; K++) {
                    var H = q.data(this[K], "olddisplay");
                    this[K].style.display = H || "";
                    if (q.css(this[K], "display") === "none") {
                        var J = this[K].tagName,
                            N;
                        if (n[J]) {
                            N = n[J]
                        } else {
                            var L = q("<" + J + " />").appendTo("body");
                            N = L.css("display");
                            if (N === "none") {
                                N = "block"
                            }
                            L.remove();
                            n[J] = N
                        }
                        q.data(this[K], "olddisplay", N)
                    }
                }
                for (var K = 0, I = this.length; K < I; K++) {
                    this[K].style.display = q.data(this[K], "olddisplay") || ""
                }
                return this
            }
        },
        hide: function(K, L) {
            if (K) {
                return this.animate(w("hide", 3), K, L)
            } else {
                for (var J = 0, I = this.length; J < I; J++) {
                    var H = q.data(this[J], "olddisplay");
                    if (!H && H !== "none") {
                        q.data(this[J], "olddisplay", q.css(this[J], "display"))
                    }
                }
                for (var J = 0, I = this.length; J < I; J++) {
                    this[J].style.display = "none"
                }
                return this
            }
        },
        _toggle: q.fn.toggle,
        toggle: function(J, I) {
            var H = typeof J === "boolean";
            return q.isFunction(J) && q.isFunction(I) ? this._toggle.apply(this, arguments) : J == null || H ? this.each(function() {
                var K = H ? J : q(this).is(":hidden");
                q(this)[K ? "show" : "hide"]()
            }) : this.animate(w("toggle", 3), J, I)
        },
        fadeTo: function(H, J, I) {
            return this.animate({
                opacity: J
            }, H, I)
        },
        animate: function(L, I, K, J) {
            var H = q.speed(I, K, J);
            return this[H.queue === false ? "each" : "queue"](function() {
                var N = q.extend({}, H),
                    P, O = this.nodeType == 1 && q(this).is(":hidden"),
                    M = this;
                for (P in L) {
                    if (L[P] == "hide" && O || L[P] == "show" && !O) {
                        return N.complete.call(this)
                    }
                    if ((P == "height" || P == "width") && this.style) {
                        N.display = q.css(this, "display");
                        N.overflow = this.style.overflow
                    }
                }
                if (N.overflow != null) {
                    this.style.overflow = "hidden"
                }
                N.curAnim = q.extend({}, L);
                q.each(L, function(R, V) {
                    var U = new q.fx(M, N, R);
                    if (/toggle|show|hide/.test(V)) {
                        U[V == "toggle" ? O ? "show" : "hide" : V](L)
                    } else {
                        var T = V.toString().match(/^([+-]=)?([\d+-.]+)(.*)$/),
                            W = U.cur(true) || 0;
                        if (T) {
                            var Q = parseFloat(T[2]),
                                S = T[3] || "px";
                            if (S != "px") {
                                M.style[R] = (Q || 1) + S;
                                W = ((Q || 1) / U.cur(true)) * W;
                                M.style[R] = W + S
                            }
                            if (T[1]) {
                                Q = ((T[1] == "-=" ? -1 : 1) * Q) + W
                            }
                            U.custom(W, Q, S)
                        } else {
                            U.custom(W, V, "")
                        }
                    }
                });
                return true
            })
        },
        stop: function(I, H) {
            var J = q.timers;
            if (I) {
                this.queue([])
            }
            this.each(function() {
                for (var K = J.length - 1; K >= 0; K--) {
                    if (J[K].elem == this) {
                        if (H) {
                            J[K](true)
                        }
                        J.splice(K, 1)
                    }
                }
            });
            if (!H) {
                this.dequeue()
            }
            return this
        }
    });
    q.each({
        slideDown: w("show", 1),
        slideUp: w("hide", 1),
        slideToggle: w("toggle", 1),
        fadeIn: {
            opacity: "show"
        },
        fadeOut: {
            opacity: "hide"
        }
    }, function(H, I) {
        q.fn[H] = function(J, K) {
            return this.animate(I, J, K)
        }
    });
    q.extend({
        speed: function(J, K, I) {
            var H = typeof J === "object" ? J : {
                complete: I || !I && K || q.isFunction(J) && J,
                duration: J,
                easing: I && K || K && !q.isFunction(K) && K
            };
            H.duration = q.fx.off ? 0 : typeof H.duration === "number" ? H.duration : q.fx.speeds[H.duration] || q.fx.speeds._default;
            H.old = H.complete;
            H.complete = function() {
                if (H.queue !== false) {
                    q(this).dequeue()
                }
                if (q.isFunction(H.old)) {
                    H.old.call(this)
                }
            };
            return H
        },
        easing: {
            linear: function(J, K, H, I) {
                return H + I * J
            },
            swing: function(J, K, H, I) {
                return ((-Math.cos(J * Math.PI) / 2) + 0.5) * I + H
            }
        },
        timers: [],
        fx: function(I, H, J) {
            this.options = H;
            this.elem = I;
            this.prop = J;
            if (!H.orig) {
                H.orig = {}
            }
        }
    });
    q.fx.prototype = {
        update: function() {
            if (this.options.step) {
                this.options.step.call(this.elem, this.now, this)
            }(q.fx.step[this.prop] || q.fx.step._default)(this);
            if ((this.prop == "height" || this.prop == "width") && this.elem.style) {
                this.elem.style.display = "block"
            }
        },
        cur: function(I) {
            if (this.elem[this.prop] != null && (!this.elem.style || this.elem.style[this.prop] == null)) {
                return this.elem[this.prop]
            }
            var H = parseFloat(q.css(this.elem, this.prop, I));
            return H && H > -10000 ? H : parseFloat(q.curCSS(this.elem, this.prop)) || 0
        },
        custom: function(L, K, J) {
            this.startTime = e();
            this.start = L;
            this.end = K;
            this.unit = J || this.unit || "px";
            this.now = this.start;
            this.pos = this.state = 0;
            var H = this;

            function I(M) {
                return H.step(M)
            }
            I.elem = this.elem;
            if (I() && q.timers.push(I) && !o) {
                o = setInterval(function() {
                    var N = q.timers;
                    for (var M = 0; M < N.length; M++) {
                        if (!N[M]()) {
                            N.splice(M--, 1)
                        }
                    }
                    if (!N.length) {
                        clearInterval(o);
                        o = g
                    }
                }, 13)
            }
        },
        show: function() {
            this.options.orig[this.prop] = q.attr(this.elem.style, this.prop);
            this.options.show = true;
            this.custom(this.prop == "width" || this.prop == "height" ? 1 : 0, this.cur());
            q(this.elem).show()
        },
        hide: function() {
            this.options.orig[this.prop] = q.attr(this.elem.style, this.prop);
            this.options.hide = true;
            this.custom(this.cur(), 0)
        },
        step: function(K) {
            var J = e();
            if (K || J >= this.options.duration + this.startTime) {
                this.now = this.end;
                this.pos = this.state = 1;
                this.update();
                this.options.curAnim[this.prop] = true;
                var H = true;
                for (var I in this.options.curAnim) {
                    if (this.options.curAnim[I] !== true) {
                        H = false
                    }
                }
                if (H) {
                    if (this.options.display != null) {
                        this.elem.style.overflow = this.options.overflow;
                        this.elem.style.display = this.options.display;
                        if (q.css(this.elem, "display") == "none") {
                            this.elem.style.display = "block"
                        }
                    }
                    if (this.options.hide) {
                        q(this.elem).hide()
                    }
                    if (this.options.hide || this.options.show) {
                        for (var L in this.options.curAnim) {
                            q.attr(this.elem.style, L, this.options.orig[L])
                        }
                    }
                    this.options.complete.call(this.elem)
                }
                return false
            } else {
                var M = J - this.startTime;
                this.state = M / this.options.duration;
                this.pos = q.easing[this.options.easing || (q.easing.swing ? "swing" : "linear")](this.state, M, 0, 1, this.options.duration);
                this.now = this.start + ((this.end - this.start) * this.pos);
                this.update()
            }
            return true
        }
    };
    q.extend(q.fx, {
        speeds: {
            slow: 600,
            fast: 200,
            _default: 400
        },
        step: {
            opacity: function(H) {
                q.attr(H.elem.style, "opacity", H.now)
            },
            _default: function(H) {
                if (H.elem.style && H.elem.style[H.prop] != null) {
                    H.elem.style[H.prop] = H.now + H.unit
                } else {
                    H.elem[H.prop] = H.now
                }
            }
        }
    });
    if (document.documentElement.getBoundingClientRect) {
        q.fn.offset = function() {
            if (!this[0]) {
                return {
                    top: 0,
                    left: 0
                }
            }
            if (this[0] === this[0].ownerDocument.body) {
                return q.offset.bodyOffset(this[0])
            }
            var J = this[0].getBoundingClientRect(),
                M = this[0].ownerDocument,
                I = M.body,
                H = M.documentElement,
                O = H.clientTop || I.clientTop || 0,
                N = H.clientLeft || I.clientLeft || 0,
                L = J.top + (self.pageYOffset || q.boxModel && H.scrollTop || I.scrollTop) - O,
                K = J.left + (self.pageXOffset || q.boxModel && H.scrollLeft || I.scrollLeft) - N;
            return {
                top: L,
                left: K
            }
        }
    } else {
        q.fn.offset = function() {
            if (!this[0]) {
                return {
                    top: 0,
                    left: 0
                }
            }
            if (this[0] === this[0].ownerDocument.body) {
                return q.offset.bodyOffset(this[0])
            }
            q.offset.initialized || q.offset.initialize();
            var M = this[0],
                J = M.offsetParent,
                I = M,
                R = M.ownerDocument,
                P, K = R.documentElement,
                N = R.body,
                O = R.defaultView,
                H = O.getComputedStyle(M, null),
                Q = M.offsetTop,
                L = M.offsetLeft;
            while ((M = M.parentNode) && M !== N && M !== K) {
                P = O.getComputedStyle(M, null);
                Q -= M.scrollTop, L -= M.scrollLeft;
                if (M === J) {
                    Q += M.offsetTop, L += M.offsetLeft;
                    if (q.offset.doesNotAddBorder && !(q.offset.doesAddBorderForTableAndCells && /^t(able|d|h)$/i.test(M.tagName))) {
                        Q += parseInt(P.borderTopWidth, 10) || 0, L += parseInt(P.borderLeftWidth, 10) || 0
                    }
                    I = J, J = M.offsetParent
                }
                if (q.offset.subtractsBorderForOverflowNotVisible && P.overflow !== "visible") {
                    Q += parseInt(P.borderTopWidth, 10) || 0, L += parseInt(P.borderLeftWidth, 10) || 0
                }
                H = P
            }
            if (H.position === "relative" || H.position === "static") {
                Q += N.offsetTop, L += N.offsetLeft
            }
            if (H.position === "fixed") {
                Q += Math.max(K.scrollTop, N.scrollTop), L += Math.max(K.scrollLeft, N.scrollLeft)
            }
            return {
                top: Q,
                left: L
            }
        }
    }
    q.offset = {
        initialize: function() {
            if (this.initialized) {
                return
            }
            var O = document.body,
                I = document.createElement("div"),
                K, J, Q, L, P, H, M = O.style.marginTop,
                N = '<div style="position:absolute;top:0;left:0;margin:0;border:5px solid #000;padding:0;width:1px;height:1px;"><div></div></div><table style="position:absolute;top:0;left:0;margin:0;border:5px solid #000;padding:0;width:1px;height:1px;" cellpadding="0" cellspacing="0"><tr><td></td></tr></table>';
            P = {
                position: "absolute",
                top: 0,
                left: 0,
                margin: 0,
                border: 0,
                width: "1px",
                height: "1px",
                visibility: "hidden"
            };
            for (H in P) {
                I.style[H] = P[H]
            }
            I.innerHTML = N;
            O.insertBefore(I, O.firstChild);
            K = I.firstChild, J = K.firstChild, L = K.nextSibling.firstChild.firstChild;
            this.doesNotAddBorder = (J.offsetTop !== 5);
            this.doesAddBorderForTableAndCells = (L.offsetTop === 5);
            K.style.overflow = "hidden", K.style.position = "relative";
            this.subtractsBorderForOverflowNotVisible = (J.offsetTop === -5);
            O.style.marginTop = "1px";
            this.doesNotIncludeMarginInBodyOffset = (O.offsetTop === 0);
            O.style.marginTop = M;
            O.removeChild(I);
            this.initialized = true
        },
        bodyOffset: function(H) {
            q.offset.initialized || q.offset.initialize();
            var J = H.offsetTop,
                I = H.offsetLeft;
            if (q.offset.doesNotIncludeMarginInBodyOffset) {
                J += parseInt(q.curCSS(H, "marginTop", true), 10) || 0, I += parseInt(q.curCSS(H, "marginLeft", true), 10) || 0
            }
            return {
                top: J,
                left: I
            }
        }
    };
    q.fn.extend({
        position: function() {
            var L = 0,
                K = 0,
                I;
            if (this[0]) {
                var J = this.offsetParent(),
                    M = this.offset(),
                    H = /^body|html$/i.test(J[0].tagName) ? {
                        top: 0,
                        left: 0
                    } : J.offset();
                M.top -= k(this, "marginTop");
                M.left -= k(this, "marginLeft");
                H.top += k(J, "borderTopWidth");
                H.left += k(J, "borderLeftWidth");
                I = {
                    top: M.top - H.top,
                    left: M.left - H.left
                }
            }
            return I
        },
        offsetParent: function() {
            var H = this[0].offsetParent || document.body;
            while (H && (!/^body|html$/i.test(H.tagName) && q.css(H, "position") == "static")) {
                H = H.offsetParent
            }
            return q(H)
        }
    });
    q.each(["Left", "Top"], function(I, H) {
        var J = "scroll" + H;
        q.fn[J] = function(K) {
            if (!this[0]) {
                return null
            }
            return K !== g ? this.each(function() {
                this == m || this == document ? m.scrollTo(!I ? K : q(m).scrollLeft(), I ? K : q(m).scrollTop()) : this[J] = K
            }) : this[0] == m || this[0] == document ? self[I ? "pageYOffset" : "pageXOffset"] || q.boxModel && document.documentElement[J] || document.body[J] : this[0][J]
        }
    });
    q.each(["Height", "Width"], function(L, J) {
        var H = L ? "Left" : "Top",
            K = L ? "Right" : "Bottom",
            I = J.toLowerCase();
        q.fn["inner" + J] = function() {
            return this[0] ? q.css(this[0], I, false, "padding") : null
        };
        q.fn["outer" + J] = function(N) {
            return this[0] ? q.css(this[0], I, false, N ? "margin" : "border") : null
        };
        var M = J.toLowerCase();
        q.fn[M] = function(N) {
            return this[0] == m ? document.compatMode == "CSS1Compat" && document.documentElement["client" + J] || document.body["client" + J] : this[0] == document ? Math.max(document.documentElement["client" + J], document.body["scroll" + J], document.documentElement["scroll" + J], document.body["offset" + J], document.documentElement["offset" + J]) : N === g ? (this.length ? q.css(this[0], M) : null) : this.css(M, typeof N === "string" ? N : N + "px")
        }
    })
})();
/*
 * jQuery Cycle Plugin (with Transition Definitions)
 * Examples and documentation at: http://jquery.malsup.com/cycle/
 * Copyright (c) 2007-2009 M. Alsup
 * Version: 2.72 (09-SEP-2009)
 * Dual licensed under the MIT and GPL licenses:
 * http://www.opensource.org/licenses/mit-license.php
 * http://www.gnu.org/licenses/gpl.html
 * Requires: jQuery v1.2.6 or later
 *
 * Originally based on the work of:
 *	1) Matt Oakes
 *	2) Torsten Baldes (http://medienfreunde.com/lab/innerfade/)
 *	3) Benjamin Sterling (http://www.benjaminsterling.com/experiments/jqShuffle/)
 */
(function(j) {
    var m = "2.72";
    if (j.support == undefined) {
        j.support = {
            opacity: !(j.browser.msie)
        }
    }

    function a(r) {
        if (j.fn.cycle.debug) {
            f(r)
        }
    }

    function f() {
        if (window.console && window.console.log) {
            window.console.log("[cycle] " + Array.prototype.join.call(arguments, " "))
        }
    }
    j.fn.cycle = function(s, r) {
        var t = {
            s: this.selector,
            c: this.context
        };
        if (this.length === 0 && s != "stop") {
            if (!j.isReady && t.s) {
                f("DOM not ready, queuing slideshow");
                j(function() {
                    j(t.s, t.c).cycle(s, r)
                });
                return this
            }
            f("terminating; zero elements found by selector" + (j.isReady ? "" : " (DOM not ready)"));
            return this
        }
        return this.each(function() {
            var y = n(this, s, r);
            if (y === false) {
                return
            }
            if (this.cycleTimeout) {
                clearTimeout(this.cycleTimeout)
            }
            this.cycleTimeout = this.cyclePause = 0;
            var z = j(this);
            var A = y.slideExpr ? j(y.slideExpr, this) : z.children();
            var w = A.get();
            if (w.length < 2) {
                f("terminating; too few slides: " + w.length);
                return
            }
            var u = l(z, A, w, y, t);
            if (u === false) {
                return
            }
            var x = u.continuous ? 10 : h(u.currSlide, u.nextSlide, u, !u.rev);
            if (x) {
                x += (u.delay || 0);
                if (x < 10) {
                    x = 10
                }
                a("first timeout: " + x);
                this.cycleTimeout = setTimeout(function() {
                    e(w, u, 0, !u.rev)
                }, x)
            }
        })
    };

    function n(r, u, s) {
        if (r.cycleStop == undefined) {
            r.cycleStop = 0
        }
        if (u === undefined || u === null) {
            u = {}
        }
        if (u.constructor == String) {
            switch (u) {
                case "stop":
                    r.cycleStop++;
                    if (r.cycleTimeout) {
                        clearTimeout(r.cycleTimeout)
                    }
                    r.cycleTimeout = 0;
                    j(r).removeData("cycle.opts");
                    return false;
                case "pause":
                    r.cyclePause = 1;
                    return false;
                case "resume":
                    r.cyclePause = 0;
                    if (s === true) {
                        u = j(r).data("cycle.opts");
                        if (!u) {
                            f("options not found, can not resume");
                            return false
                        }
                        if (r.cycleTimeout) {
                            clearTimeout(r.cycleTimeout);
                            r.cycleTimeout = 0
                        }
                        e(u.elements, u, 1, 1)
                    }
                    return false;
                case "prev":
                case "next":
                    var w = j(r).data("cycle.opts");
                    if (!w) {
                        f('options not found, "prev/next" ignored');
                        return false
                    }
                    j.fn.cycle[u](w);
                    return false;
                default:
                    u = {
                        fx: u
                    }
            }
            return u
        } else {
            if (u.constructor == Number) {
                var t = u;
                u = j(r).data("cycle.opts");
                if (!u) {
                    f("options not found, can not advance slide");
                    return false
                }
                if (t < 0 || t >= u.elements.length) {
                    f("invalid slide index: " + t);
                    return false
                }
                u.nextSlide = t;
                if (r.cycleTimeout) {
                    clearTimeout(r.cycleTimeout);
                    r.cycleTimeout = 0
                }
                if (typeof s == "string") {
                    u.oneTimeFx = s
                }
                e(u.elements, u, 1, t >= u.currSlide);
                return false
            }
        }
        return u
    }

    function b(r, s) {
        if (!j.support.opacity && s.cleartype && r.style.filter) {
            try {
                r.style.removeAttribute("filter")
            } catch (t) {}
        }
    }

    function l(A, L, x, u, G) {
        var E = j.extend({}, j.fn.cycle.defaults, u || {}, j.metadata ? A.metadata() : j.meta ? A.data() : {});
        if (E.autostop) {
            E.countdown = E.autostopCount || x.length
        }
        var s = A[0];
        A.data("cycle.opts", E);
        E.$cont = A;
        E.stopCount = s.cycleStop;
        E.elements = x;
        E.before = E.before ? [E.before] : [];
        E.after = E.after ? [E.after] : [];
        E.after.unshift(function() {
            E.busy = 0
        });
        if (!j.support.opacity && E.cleartype) {
            E.after.push(function() {
                b(this, E)
            })
        }
        if (E.continuous) {
            E.after.push(function() {
                e(x, E, 0, !E.rev)
            })
        }
        o(E);
        if (!j.support.opacity && E.cleartype && !E.cleartypeNoBg) {
            g(L)
        }
        if (A.css("position") == "static") {
            A.css("position", "relative")
        }
        if (E.width) {
            A.width(E.width)
        }
        if (E.height && E.height != "auto") {
            A.height(E.height)
        }
        if (E.startingSlide) {
            E.startingSlide = parseInt(E.startingSlide)
        }
        if (E.random) {
            E.randomMap = [];
            for (var J = 0; J < x.length; J++) {
                E.randomMap.push(J)
            }
            E.randomMap.sort(function(N, w) {
                return Math.random() - 0.5
            });
            E.randomIndex = 0;
            E.startingSlide = E.randomMap[0]
        } else {
            if (E.startingSlide >= x.length) {
                E.startingSlide = 0
            }
        }
        E.currSlide = E.startingSlide = E.startingSlide || 0;
        var z = E.startingSlide;
        L.css({
            position: "absolute",
            top: 0,
            left: 0
        }).hide().each(function(w) {
            var N = z ? w >= z ? x.length - (w - z) : z - w : x.length - w;
            j(this).css("z-index", N)
        });
        j(x[z]).css("opacity", 1).show();
        b(x[z], E);
        if (E.fit && E.width) {
            L.width(E.width)
        }
        if (E.fit && E.height && E.height != "auto") {
            L.height(E.height)
        }
        var F = E.containerResize && !A.innerHeight();
        if (F) {
            var y = 0,
                D = 0;
            for (var H = 0; H < x.length; H++) {
                var r = j(x[H]),
                    M = r[0],
                    C = r.outerWidth(),
                    K = r.outerHeight();
                if (!C) {
                    C = M.offsetWidth
                }
                if (!K) {
                    K = M.offsetHeight
                }
                y = C > y ? C : y;
                D = K > D ? K : D
            }
            if (y > 0 && D > 0) {
                A.css({
                    width: y + "px",
                    height: D + "px"
                })
            }
        }
        if (E.pause) {
            A.hover(function() {
                this.cyclePause++
            }, function() {
                this.cyclePause--
            })
        }
        if (c(E) === false) {
            return false
        }
        var t = false;
        u.requeueAttempts = u.requeueAttempts || 0;
        L.each(function() {
            var P = j(this);
            this.cycleH = (E.fit && E.height) ? E.height : P.height();
            this.cycleW = (E.fit && E.width) ? E.width : P.width();
            if (P.is("img")) {
                var N = (j.browser.msie && this.cycleW == 28 && this.cycleH == 30 && !this.complete);
                var Q = (j.browser.mozilla && this.cycleW == 34 && this.cycleH == 19 && !this.complete);
                var O = (j.browser.opera && ((this.cycleW == 42 && this.cycleH == 19) || (this.cycleW == 37 && this.cycleH == 17)) && !this.complete);
                var w = (this.cycleH == 0 && this.cycleW == 0 && !this.complete);
                if (N || Q || O || w) {
                    if (G.s && E.requeueOnImageNotLoaded && ++u.requeueAttempts < 100) {
                        f(u.requeueAttempts, " - img slide not loaded, requeuing slideshow: ", this.src, this.cycleW, this.cycleH);
                        setTimeout(function() {
                            j(G.s, G.c).cycle(u)
                        }, E.requeueTimeout);
                        t = true;
                        return false
                    } else {
                        f("could not determine size of image: " + this.src, this.cycleW, this.cycleH)
                    }
                }
            }
            return true
        });
        if (t) {
            return false
        }
        E.cssBefore = E.cssBefore || {};
        E.animIn = E.animIn || {};
        E.animOut = E.animOut || {};
        L.not(":eq(" + z + ")").css(E.cssBefore);
        if (E.cssFirst) {
            j(L[z]).css(E.cssFirst)
        }
        if (E.timeout) {
            E.timeout = parseInt(E.timeout);
            if (E.speed.constructor == String) {
                E.speed = j.fx.speeds[E.speed] || parseInt(E.speed)
            }
            if (!E.sync) {
                E.speed = E.speed / 2
            }
            while ((E.timeout - E.speed) < 250) {
                E.timeout += E.speed
            }
        }
        if (E.easing) {
            E.easeIn = E.easeOut = E.easing
        }
        if (!E.speedIn) {
            E.speedIn = E.speed
        }
        if (!E.speedOut) {
            E.speedOut = E.speed
        }
        E.slideCount = x.length;
        E.currSlide = E.lastSlide = z;
        if (E.random) {
            E.nextSlide = E.currSlide;
            if (++E.randomIndex == x.length) {
                E.randomIndex = 0
            }
            E.nextSlide = E.randomMap[E.randomIndex]
        } else {
            E.nextSlide = E.startingSlide >= (x.length - 1) ? 0 : E.startingSlide + 1
        }
        if (!E.multiFx) {
            var I = j.fn.cycle.transitions[E.fx];
            if (j.isFunction(I)) {
                I(A, L, E)
            } else {
                if (E.fx != "custom" && !E.multiFx) {
                    f("unknown transition: " + E.fx, "; slideshow terminating");
                    return false
                }
            }
        }
        var B = L[z];
        if (E.before.length) {
            E.before[0].apply(B, [B, B, E, true])
        }
        if (E.after.length > 1) {
            E.after[1].apply(B, [B, B, E, true])
        }
        if (E.next) {
            j(E.next).bind(E.prevNextEvent, function() {
                return q(E, E.rev ? -1 : 1)
            })
        }
        if (E.prev) {
            j(E.prev).bind(E.prevNextEvent, function() {
                return q(E, E.rev ? 1 : -1)
            })
        }
        if (E.pager) {
            d(x, E)
        }
        k(E, x);
        return E
    }

    function o(r) {
        r.original = {
            before: [],
            after: []
        };
        r.original.cssBefore = j.extend({}, r.cssBefore);
        r.original.cssAfter = j.extend({}, r.cssAfter);
        r.original.animIn = j.extend({}, r.animIn);
        r.original.animOut = j.extend({}, r.animOut);
        j.each(r.before, function() {
            r.original.before.push(this)
        });
        j.each(r.after, function() {
            r.original.after.push(this)
        })
    }

    function c(y) {
        var w, t, s = j.fn.cycle.transitions;
        if (y.fx.indexOf(",") > 0) {
            y.multiFx = true;
            y.fxs = y.fx.replace(/\s*/g, "").split(",");
            for (w = 0; w < y.fxs.length; w++) {
                var x = y.fxs[w];
                t = s[x];
                if (!t || !s.hasOwnProperty(x) || !j.isFunction(t)) {
                    f("discarding unknown transition: ", x);
                    y.fxs.splice(w, 1);
                    w--
                }
            }
            if (!y.fxs.length) {
                f("No valid transitions named; slideshow terminating.");
                return false
            }
        } else {
            if (y.fx == "all") {
                y.multiFx = true;
                y.fxs = [];
                for (p in s) {
                    t = s[p];
                    if (s.hasOwnProperty(p) && j.isFunction(t)) {
                        y.fxs.push(p)
                    }
                }
            }
        }
        if (y.multiFx && y.randomizeEffects) {
            var u = Math.floor(Math.random() * 20) + 30;
            for (w = 0; w < u; w++) {
                var r = Math.floor(Math.random() * y.fxs.length);
                y.fxs.push(y.fxs.splice(r, 1)[0])
            }
            a("randomized fx sequence: ", y.fxs)
        }
        return true
    }

    function k(s, r) {
        s.addSlide = function(u, w) {
            var t = j(u),
                x = t[0];
            if (!s.autostopCount) {
                s.countdown++
            }
            r[w ? "unshift" : "push"](x);
            if (s.els) {
                s.els[w ? "unshift" : "push"](x)
            }
            s.slideCount = r.length;
            t.css("position", "absolute");
            t[w ? "prependTo" : "appendTo"](s.$cont);
            if (w) {
                s.currSlide++;
                s.nextSlide++
            }
            if (!j.support.opacity && s.cleartype && !s.cleartypeNoBg) {
                g(t)
            }
            if (s.fit && s.width) {
                t.width(s.width)
            }
            if (s.fit && s.height && s.height != "auto") {
                $slides.height(s.height)
            }
            x.cycleH = (s.fit && s.height) ? s.height : t.height();
            x.cycleW = (s.fit && s.width) ? s.width : t.width();
            t.css(s.cssBefore);
            if (s.pager) {
                j.fn.cycle.createPagerAnchor(r.length - 1, x, j(s.pager), r, s)
            }
            if (j.isFunction(s.onAddSlide)) {
                s.onAddSlide(t)
            } else {
                t.hide()
            }
        }
    }
    j.fn.cycle.resetState = function(s, r) {
        r = r || s.fx;
        s.before = [];
        s.after = [];
        s.cssBefore = j.extend({}, s.original.cssBefore);
        s.cssAfter = j.extend({}, s.original.cssAfter);
        s.animIn = j.extend({}, s.original.animIn);
        s.animOut = j.extend({}, s.original.animOut);
        s.fxFn = null;
        j.each(s.original.before, function() {
            s.before.push(this)
        });
        j.each(s.original.after, function() {
            s.after.push(this)
        });
        var t = j.fn.cycle.transitions[r];
        if (j.isFunction(t)) {
            t(s.$cont, j(s.elements), s)
        }
    };

    function e(z, r, y, A) {
        if (y && r.busy && r.manualTrump) {
            j(z).stop(true, true);
            r.busy = false
        }
        if (r.busy) {
            return
        }
        var w = r.$cont[0],
            C = z[r.currSlide],
            B = z[r.nextSlide];
        if (w.cycleStop != r.stopCount || w.cycleTimeout === 0 && !y) {
            return
        }
        if (!y && !w.cyclePause && ((r.autostop && (--r.countdown <= 0)) || (r.nowrap && !r.random && r.nextSlide < r.currSlide))) {
            if (r.end) {
                r.end(r)
            }
            return
        }
        if (y || !w.cyclePause) {
            var x = r.fx;
            C.cycleH = C.cycleH || j(C).height();
            C.cycleW = C.cycleW || j(C).width();
            B.cycleH = B.cycleH || j(B).height();
            B.cycleW = B.cycleW || j(B).width();
            if (r.multiFx) {
                if (r.lastFx == undefined || ++r.lastFx >= r.fxs.length) {
                    r.lastFx = 0
                }
                x = r.fxs[r.lastFx];
                r.currFx = x
            }
            if (r.oneTimeFx) {
                x = r.oneTimeFx;
                r.oneTimeFx = null
            }
            j.fn.cycle.resetState(r, x);
            if (r.before.length) {
                j.each(r.before, function(D, E) {
                    if (w.cycleStop != r.stopCount) {
                        return
                    }
                    E.apply(B, [C, B, r, A])
                })
            }
            var t = function() {
                j.each(r.after, function(D, E) {
                    if (w.cycleStop != r.stopCount) {
                        return
                    }
                    E.apply(B, [C, B, r, A])
                })
            };
            if (r.nextSlide != r.currSlide) {
                r.busy = 1;
                if (r.fxFn) {
                    r.fxFn(C, B, r, t, A)
                } else {
                    if (j.isFunction(j.fn.cycle[r.fx])) {
                        j.fn.cycle[r.fx](C, B, r, t)
                    } else {
                        j.fn.cycle.custom(C, B, r, t, y && r.fastOnEvent)
                    }
                }
            }
            r.lastSlide = r.currSlide;
            if (r.random) {
                r.currSlide = r.nextSlide;
                if (++r.randomIndex == z.length) {
                    r.randomIndex = 0
                }
                r.nextSlide = r.randomMap[r.randomIndex]
            } else {
                var u = (r.nextSlide + 1) == z.length;
                r.nextSlide = u ? 0 : r.nextSlide + 1;
                r.currSlide = u ? z.length - 1 : r.nextSlide - 1
            }
            if (r.pager) {
                j.fn.cycle.updateActivePagerLink(r.pager, r.currSlide)
            }
        }
        var s = 0;
        if (r.timeout && !r.continuous) {
            s = h(C, B, r, A)
        } else {
            if (r.continuous && w.cyclePause) {
                s = 10
            }
        }
        if (s > 0) {
            w.cycleTimeout = setTimeout(function() {
                e(z, r, 0, !r.rev)
            }, s)
        }
    }
    j.fn.cycle.updateActivePagerLink = function(r, s) {
        j(r).find("a").removeClass("activeSlide").filter("a:eq(" + s + ")").addClass("activeSlide")
    };

    function h(x, u, w, s) {
        if (w.timeoutFn) {
            var r = w.timeoutFn(x, u, w, s);
            while ((r - w.speed) < 250) {
                r += w.speed
            }
            a("calculated timeout: " + r + "; speed: " + w.speed);
            if (r !== false) {
                return r
            }
        }
        return w.timeout
    }
    j.fn.cycle.next = function(r) {
        q(r, r.rev ? -1 : 1)
    };
    j.fn.cycle.prev = function(r) {
        q(r, r.rev ? 1 : -1)
    };

    function q(s, w) {
        var r = s.elements;
        var u = s.$cont[0],
            t = u.cycleTimeout;
        if (t) {
            clearTimeout(t);
            u.cycleTimeout = 0
        }
        if (s.random && w < 0) {
            s.randomIndex--;
            if (--s.randomIndex == -2) {
                s.randomIndex = r.length - 2
            } else {
                if (s.randomIndex == -1) {
                    s.randomIndex = r.length - 1
                }
            }
            s.nextSlide = s.randomMap[s.randomIndex]
        } else {
            if (s.random) {
                if (++s.randomIndex == r.length) {
                    s.randomIndex = 0
                }
                s.nextSlide = s.randomMap[s.randomIndex]
            } else {
                s.nextSlide = s.currSlide + w;
                if (s.nextSlide < 0) {
                    if (s.nowrap) {
                        return false
                    }
                    s.nextSlide = r.length - 1
                } else {
                    if (s.nextSlide >= r.length) {
                        if (s.nowrap) {
                            return false
                        }
                        s.nextSlide = 0
                    }
                }
            }
        }
        if (j.isFunction(s.prevNextClick)) {
            s.prevNextClick(w > 0, s.nextSlide, r[s.nextSlide])
        }
        e(r, s, 1, w >= 0);
        return false
    }

    function d(s, t) {
        var r = j(t.pager);
        j.each(s, function(u, w) {
            j.fn.cycle.createPagerAnchor(u, w, r, s, t)
        });
        j.fn.cycle.updateActivePagerLink(t.pager, t.startingSlide)
    }
    j.fn.cycle.createPagerAnchor = function(w, x, t, u, y) {
        var s;
        if (j.isFunction(y.pagerAnchorBuilder)) {
            s = y.pagerAnchorBuilder(w, x)
        } else {
            s = '<a href="#">' + (w + 1) + "</a>"
        }
        if (!s) {
            return
        }
        var z = j(s);
        if (z.parents("body").length === 0) {
            var r = [];
            if (t.length > 1) {
                t.each(function() {
                    var A = z.clone(true);
                    j(this).append(A);
                    r.push(A)
                });
                z = j(r)
            } else {
                z.appendTo(t)
            }
        }
        z.bind(y.pagerEvent, function(C) {
            C.preventDefault();
            y.nextSlide = w;
            var B = y.$cont[0],
                A = B.cycleTimeout;
            if (A) {
                clearTimeout(A);
                B.cycleTimeout = 0
            }
            if (j.isFunction(y.pagerClick)) {
                y.pagerClick(y.nextSlide, u[y.nextSlide])
            }
            e(u, y, 1, y.currSlide < w);
            return false
        });
        if (y.pagerEvent != "click") {
            z.click(function() {
                return false
            })
        }
        if (y.pauseOnPagerHover) {
            z.hover(function() {
                y.$cont[0].cyclePause++
            }, function() {
                y.$cont[0].cyclePause--
            })
        }
    };
    j.fn.cycle.hopsFromLast = function(u, t) {
        var s, r = u.lastSlide,
            w = u.currSlide;
        if (t) {
            s = w > r ? w - r : u.slideCount - r
        } else {
            s = w < r ? r - w : r + u.slideCount - w
        }
        return s
    };

    function g(t) {
        function s(u) {
            u = parseInt(u).toString(16);
            return u.length < 2 ? "0" + u : u
        }

        function r(x) {
            for (; x && x.nodeName.toLowerCase() != "html"; x = x.parentNode) {
                var u = j.css(x, "background-color");
                if (u.indexOf("rgb") >= 0) {
                    var w = u.match(/\d+/g);
                    return "#" + s(w[0]) + s(w[1]) + s(w[2])
                }
                if (u && u != "transparent") {
                    return u
                }
            }
            return "#ffffff"
        }
        t.each(function() {
            j(this).css("background-color", r(this))
        })
    }
    j.fn.cycle.commonReset = function(y, u, x, s, t, r) {
        j(x.elements).not(y).hide();
        x.cssBefore.opacity = 1;
        x.cssBefore.display = "block";
        if (s !== false && u.cycleW > 0) {
            x.cssBefore.width = u.cycleW
        }
        if (t !== false && u.cycleH > 0) {
            x.cssBefore.height = u.cycleH
        }
        x.cssAfter = x.cssAfter || {};
        x.cssAfter.display = "none";
        j(y).css("zIndex", x.slideCount + (r === true ? 1 : 0));
        j(u).css("zIndex", x.slideCount + (r === true ? 0 : 1))
    };
    j.fn.cycle.custom = function(D, x, r, t, s) {
        var C = j(D),
            y = j(x);
        var u = r.speedIn,
            B = r.speedOut,
            w = r.easeIn,
            A = r.easeOut;
        y.css(r.cssBefore);
        if (s) {
            if (typeof s == "number") {
                u = B = s
            } else {
                u = B = 1
            }
            w = A = null
        }
        var z = function() {
            y.animate(r.animIn, u, w, t)
        };
        C.animate(r.animOut, B, A, function() {
            if (r.cssAfter) {
                C.css(r.cssAfter)
            }
            if (!r.sync) {
                z()
            }
        });
        if (r.sync) {
            z()
        }
    };
    j.fn.cycle.transitions = {
        fade: function(s, t, r) {
            t.not(":eq(" + r.currSlide + ")").css("opacity", 0);
            r.before.push(function(x, u, w) {
                j.fn.cycle.commonReset(x, u, w);
                w.cssBefore.opacity = 0
            });
            r.animIn = {
                opacity: 1
            };
            r.animOut = {
                opacity: 0
            };
            r.cssBefore = {
                top: 0,
                left: 0
            }
        }
    };
    j.fn.cycle.ver = function() {
        return m
    };
    j.fn.cycle.defaults = {
        fx: "fade",
        timeout: 4000,
        timeoutFn: null,
        continuous: 0,
        speed: 1000,
        speedIn: null,
        speedOut: null,
        next: null,
        prev: null,
        prevNextClick: null,
        prevNextEvent: "click",
        pager: null,
        pagerClick: null,
        pagerEvent: "click",
        pagerAnchorBuilder: null,
        before: null,
        after: null,
        end: null,
        easing: null,
        easeIn: null,
        easeOut: null,
        shuffle: null,
        animIn: null,
        animOut: null,
        cssBefore: null,
        cssAfter: null,
        fxFn: null,
        height: "auto",
        startingSlide: 0,
        sync: 1,
        random: 0,
        fit: 0,
        containerResize: 1,
        pause: 0,
        pauseOnPagerHover: 0,
        autostop: 0,
        autostopCount: 0,
        delay: 0,
        slideExpr: null,
        cleartype: !j.support.opacity,
        cleartypeNoBg: false,
        nowrap: 0,
        fastOnEvent: 0,
        randomizeEffects: 1,
        rev: 0,
        manualTrump: true,
        requeueOnImageNotLoaded: true,
        requeueTimeout: 250
    }
})(jQuery);
/*
 * jQuery Cycle Plugin Transition Definitions
 * This script is a plugin for the jQuery Cycle Plugin
 * Examples and documentation at: http://malsup.com/jquery/cycle/
 * Copyright (c) 2007-2008 M. Alsup
 * Version:	 2.72
 * Dual licensed under the MIT and GPL licenses:
 * http://www.opensource.org/licenses/mit-license.php
 * http://www.gnu.org/licenses/gpl.html
 */
(function(a) {
    a.fn.cycle.transitions.none = function(c, d, b) {
        b.fxFn = function(g, e, f, h) {
            a(e).show();
            a(g).hide();
            h()
        }
    };
    a.fn.cycle.transitions.scrollUp = function(d, e, c) {
        d.css("overflow", "hidden");
        c.before.push(a.fn.cycle.commonReset);
        var b = d.height();
        c.cssBefore = {
            top: b,
            left: 0
        };
        c.cssFirst = {
            top: 0
        };
        c.animIn = {
            top: 0
        };
        c.animOut = {
            top: -b
        }
    };
    a.fn.cycle.transitions.scrollDown = function(d, e, c) {
        d.css("overflow", "hidden");
        c.before.push(a.fn.cycle.commonReset);
        var b = d.height();
        c.cssFirst = {
            top: 0
        };
        c.cssBefore = {
            top: -b,
            left: 0
        };
        c.animIn = {
            top: 0
        };
        c.animOut = {
            top: b
        }
    };
    a.fn.cycle.transitions.scrollLeft = function(d, e, c) {
        d.css("overflow", "hidden");
        c.before.push(a.fn.cycle.commonReset);
        var b = d.width();
        c.cssFirst = {
            left: 0
        };
        c.cssBefore = {
            left: b,
            top: 0
        };
        c.animIn = {
            left: 0
        };
        c.animOut = {
            left: 0 - b
        }
    };
    a.fn.cycle.transitions.scrollRight = function(d, e, c) {
        d.css("overflow", "hidden");
        c.before.push(a.fn.cycle.commonReset);
        var b = d.width();
        c.cssFirst = {
            left: 0
        };
        c.cssBefore = {
            left: -b,
            top: 0
        };
        c.animIn = {
            left: 0
        };
        c.animOut = {
            left: b
        }
    };
    a.fn.cycle.transitions.scrollHorz = function(c, d, b) {
        c.css("overflow", "hidden").width();
        b.before.push(function(h, f, g, e) {
            a.fn.cycle.commonReset(h, f, g);
            g.cssBefore.left = e ? (f.cycleW - 1) : (1 - f.cycleW);
            g.animOut.left = e ? -h.cycleW : h.cycleW
        });
        b.cssFirst = {
            left: 0
        };
        b.cssBefore = {
            top: 0
        };
        b.animIn = {
            left: 0
        };
        b.animOut = {
            top: 0
        }
    };
    a.fn.cycle.transitions.scrollVert = function(c, d, b) {
        c.css("overflow", "hidden");
        b.before.push(function(h, f, g, e) {
            a.fn.cycle.commonReset(h, f, g);
            g.cssBefore.top = e ? (1 - f.cycleH) : (f.cycleH - 1);
            g.animOut.top = e ? h.cycleH : -h.cycleH
        });
        b.cssFirst = {
            top: 0
        };
        b.cssBefore = {
            left: 0
        };
        b.animIn = {
            top: 0
        };
        b.animOut = {
            left: 0
        }
    };
    a.fn.cycle.transitions.slideX = function(c, d, b) {
        b.before.push(function(g, e, f) {
            a(f.elements).not(g).hide();
            a.fn.cycle.commonReset(g, e, f, false, true);
            f.animIn.width = e.cycleW
        });
        b.cssBefore = {
            left: 0,
            top: 0,
            width: 0
        };
        b.animIn = {
            width: "show"
        };
        b.animOut = {
            width: 0
        }
    };
    a.fn.cycle.transitions.slideY = function(c, d, b) {
        b.before.push(function(g, e, f) {
            a(f.elements).not(g).hide();
            a.fn.cycle.commonReset(g, e, f, true, false);
            f.animIn.height = e.cycleH
        });
        b.cssBefore = {
            left: 0,
            top: 0,
            height: 0
        };
        b.animIn = {
            height: "show"
        };
        b.animOut = {
            height: 0
        }
    };
    a.fn.cycle.transitions.shuffle = function(e, f, d) {
        var c, b = e.css("overflow", "visible").width();
        f.css({
            left: 0,
            top: 0
        });
        d.before.push(function(j, g, h) {
            a.fn.cycle.commonReset(j, g, h, true, true, true)
        });
        if (!d.speedAdjusted) {
            d.speed = d.speed / 2;
            d.speedAdjusted = true
        }
        d.random = 0;
        d.shuffle = d.shuffle || {
            left: -b,
            top: 15
        };
        d.els = [];
        for (c = 0; c < f.length; c++) {
            d.els.push(f[c])
        }
        for (c = 0; c < d.currSlide; c++) {
            d.els.push(d.els.shift())
        }
        d.fxFn = function(n, k, m, g, j) {
            var h = j ? a(n) : a(k);
            a(k).css(m.cssBefore);
            var l = m.slideCount;
            h.animate(m.shuffle, m.speedIn, m.easeIn, function() {
                var q = a.fn.cycle.hopsFromLast(m, j);
                for (var r = 0; r < q; r++) {
                    j ? m.els.push(m.els.shift()) : m.els.unshift(m.els.pop())
                }
                if (j) {
                    for (var s = 0, o = m.els.length; s < o; s++) {
                        a(m.els[s]).css("z-index", o - s + l)
                    }
                } else {
                    var t = a(n).css("z-index");
                    h.css("z-index", parseInt(t) + 1 + l)
                }
                h.animate({
                    left: 0,
                    top: 0
                }, m.speedOut, m.easeOut, function() {
                    a(j ? this : n).hide();
                    if (g) {
                        g()
                    }
                })
            })
        };
        d.cssBefore = {
            display: "block",
            opacity: 1,
            top: 0,
            left: 0
        }
    };
    a.fn.cycle.transitions.turnUp = function(c, d, b) {
        b.before.push(function(g, e, f) {
            a.fn.cycle.commonReset(g, e, f, true, false);
            f.cssBefore.top = e.cycleH;
            f.animIn.height = e.cycleH
        });
        b.cssFirst = {
            top: 0
        };
        b.cssBefore = {
            left: 0,
            height: 0
        };
        b.animIn = {
            top: 0
        };
        b.animOut = {
            height: 0
        }
    };
    a.fn.cycle.transitions.turnDown = function(c, d, b) {
        b.before.push(function(g, e, f) {
            a.fn.cycle.commonReset(g, e, f, true, false);
            f.animIn.height = e.cycleH;
            f.animOut.top = g.cycleH
        });
        b.cssFirst = {
            top: 0
        };
        b.cssBefore = {
            left: 0,
            top: 0,
            height: 0
        };
        b.animOut = {
            height: 0
        }
    };
    a.fn.cycle.transitions.turnLeft = function(c, d, b) {
        b.before.push(function(g, e, f) {
            a.fn.cycle.commonReset(g, e, f, false, true);
            f.cssBefore.left = e.cycleW;
            f.animIn.width = e.cycleW
        });
        b.cssBefore = {
            top: 0,
            width: 0
        };
        b.animIn = {
            left: 0
        };
        b.animOut = {
            width: 0
        }
    };
    a.fn.cycle.transitions.turnRight = function(c, d, b) {
        b.before.push(function(g, e, f) {
            a.fn.cycle.commonReset(g, e, f, false, true);
            f.animIn.width = e.cycleW;
            f.animOut.left = g.cycleW
        });
        b.cssBefore = {
            top: 0,
            left: 0,
            width: 0
        };
        b.animIn = {
            left: 0
        };
        b.animOut = {
            width: 0
        }
    };
    a.fn.cycle.transitions.zoom = function(c, d, b) {
        b.before.push(function(g, e, f) {
            a.fn.cycle.commonReset(g, e, f, false, false, true);
            f.cssBefore.top = e.cycleH / 2;
            f.cssBefore.left = e.cycleW / 2;
            f.animIn = {
                top: 0,
                left: 0,
                width: e.cycleW,
                height: e.cycleH
            };
            f.animOut = {
                width: 0,
                height: 0,
                top: g.cycleH / 2,
                left: g.cycleW / 2
            }
        });
        b.cssFirst = {
            top: 0,
            left: 0
        };
        b.cssBefore = {
            width: 0,
            height: 0
        }
    };
    a.fn.cycle.transitions.fadeZoom = function(c, d, b) {
        b.before.push(function(g, e, f) {
            a.fn.cycle.commonReset(g, e, f, false, false);
            f.cssBefore.left = e.cycleW / 2;
            f.cssBefore.top = e.cycleH / 2;
            f.animIn = {
                top: 0,
                left: 0,
                width: e.cycleW,
                height: e.cycleH
            }
        });
        b.cssBefore = {
            width: 0,
            height: 0
        };
        b.animOut = {
            opacity: 0
        }
    };
    a.fn.cycle.transitions.blindX = function(d, e, c) {
        var b = d.css("overflow", "hidden").width();
        c.before.push(function(h, f, g) {
            a.fn.cycle.commonReset(h, f, g);
            g.animIn.width = f.cycleW;
            g.animOut.left = h.cycleW
        });
        c.cssBefore = {
            left: b,
            top: 0
        };
        c.animIn = {
            left: 0
        };
        c.animOut = {
            left: b
        }
    };
    a.fn.cycle.transitions.blindY = function(d, e, c) {
        var b = d.css("overflow", "hidden").height();
        c.before.push(function(h, f, g) {
            a.fn.cycle.commonReset(h, f, g);
            g.animIn.height = f.cycleH;
            g.animOut.top = h.cycleH
        });
        c.cssBefore = {
            top: b,
            left: 0
        };
        c.animIn = {
            top: 0
        };
        c.animOut = {
            top: b
        }
    };
    a.fn.cycle.transitions.blindZ = function(e, f, d) {
        var c = e.css("overflow", "hidden").height();
        var b = e.width();
        d.before.push(function(j, g, h) {
            a.fn.cycle.commonReset(j, g, h);
            h.animIn.height = g.cycleH;
            h.animOut.top = j.cycleH
        });
        d.cssBefore = {
            top: c,
            left: b
        };
        d.animIn = {
            top: 0,
            left: 0
        };
        d.animOut = {
            top: c,
            left: b
        }
    };
    a.fn.cycle.transitions.growX = function(c, d, b) {
        b.before.push(function(g, e, f) {
            a.fn.cycle.commonReset(g, e, f, false, true);
            f.cssBefore.left = this.cycleW / 2;
            f.animIn = {
                left: 0,
                width: this.cycleW
            };
            f.animOut = {
                left: 0
            }
        });
        b.cssBefore = {
            width: 0,
            top: 0
        }
    };
    a.fn.cycle.transitions.growY = function(c, d, b) {
        b.before.push(function(g, e, f) {
            a.fn.cycle.commonReset(g, e, f, true, false);
            f.cssBefore.top = this.cycleH / 2;
            f.animIn = {
                top: 0,
                height: this.cycleH
            };
            f.animOut = {
                top: 0
            }
        });
        b.cssBefore = {
            height: 0,
            left: 0
        }
    };
    a.fn.cycle.transitions.curtainX = function(c, d, b) {
        b.before.push(function(g, e, f) {
            a.fn.cycle.commonReset(g, e, f, false, true, true);
            f.cssBefore.left = e.cycleW / 2;
            f.animIn = {
                left: 0,
                width: this.cycleW
            };
            f.animOut = {
                left: g.cycleW / 2,
                width: 0
            }
        });
        b.cssBefore = {
            top: 0,
            width: 0
        }
    };
    a.fn.cycle.transitions.curtainY = function(c, d, b) {
        b.before.push(function(g, e, f) {
            a.fn.cycle.commonReset(g, e, f, true, false, true);
            f.cssBefore.top = e.cycleH / 2;
            f.animIn = {
                top: 0,
                height: e.cycleH
            };
            f.animOut = {
                top: g.cycleH / 2,
                height: 0
            }
        });
        b.cssBefore = {
            left: 0,
            height: 0
        }
    };
    a.fn.cycle.transitions.cover = function(f, g, e) {
        var j = e.direction || "left";
        var b = f.css("overflow", "hidden").width();
        var c = f.height();
        e.before.push(function(k, d, h) {
            a.fn.cycle.commonReset(k, d, h);
            if (j == "right") {
                h.cssBefore.left = -b
            } else {
                if (j == "up") {
                    h.cssBefore.top = c
                } else {
                    if (j == "down") {
                        h.cssBefore.top = -c
                    } else {
                        h.cssBefore.left = b
                    }
                }
            }
        });
        e.animIn = {
            left: 0,
            top: 0
        };
        e.animOut = {
            opacity: 1
        };
        e.cssBefore = {
            top: 0,
            left: 0
        }
    };
    a.fn.cycle.transitions.uncover = function(f, g, e) {
        var j = e.direction || "left";
        var b = f.css("overflow", "hidden").width();
        var c = f.height();
        e.before.push(function(k, d, h) {
            a.fn.cycle.commonReset(k, d, h, true, true, true);
            if (j == "right") {
                h.animOut.left = b
            } else {
                if (j == "up") {
                    h.animOut.top = -c
                } else {
                    if (j == "down") {
                        h.animOut.top = c
                    } else {
                        h.animOut.left = -b
                    }
                }
            }
        });
        e.animIn = {
            left: 0,
            top: 0
        };
        e.animOut = {
            opacity: 1
        };
        e.cssBefore = {
            top: 0,
            left: 0
        }
    };
    a.fn.cycle.transitions.toss = function(e, f, d) {
        var b = e.css("overflow", "visible").width();
        var c = e.height();
        d.before.push(function(j, g, h) {
            a.fn.cycle.commonReset(j, g, h, true, true, true);
            if (!h.animOut.left && !h.animOut.top) {
                h.animOut = {
                    left: b * 2,
                    top: -c / 2,
                    opacity: 0
                }
            } else {
                h.animOut.opacity = 0
            }
        });
        d.cssBefore = {
            left: 0,
            top: 0
        };
        d.animIn = {
            left: 0
        }
    };
    a.fn.cycle.transitions.wipe = function(u, n, e) {
        var s = u.css("overflow", "hidden").width();
        var k = u.height();
        e.cssBefore = e.cssBefore || {};
        var g;
        if (e.clip) {
            if (/l2r/.test(e.clip)) {
                g = "rect(0px 0px " + k + "px 0px)"
            } else {
                if (/r2l/.test(e.clip)) {
                    g = "rect(0px " + s + "px " + k + "px " + s + "px)"
                } else {
                    if (/t2b/.test(e.clip)) {
                        g = "rect(0px " + s + "px 0px 0px)"
                    } else {
                        if (/b2t/.test(e.clip)) {
                            g = "rect(" + k + "px " + s + "px " + k + "px 0px)"
                        } else {
                            if (/zoom/.test(e.clip)) {
                                var q = parseInt(k / 2);
                                var f = parseInt(s / 2);
                                g = "rect(" + q + "px " + f + "px " + q + "px " + f + "px)"
                            }
                        }
                    }
                }
            }
        }
        e.cssBefore.clip = e.cssBefore.clip || g || "rect(0px 0px 0px 0px)";
        var m = e.cssBefore.clip.match(/(\d+)/g);
        var x = parseInt(m[0]),
            c = parseInt(m[1]),
            o = parseInt(m[2]),
            j = parseInt(m[3]);
        e.before.push(function(y, h, t) {
            if (y == h) {
                return
            }
            var d = a(y),
                b = a(h);
            a.fn.cycle.commonReset(y, h, t, true, true, false);
            t.cssAfter.display = "block";
            var r = 1,
                l = parseInt((t.speedIn / 13)) - 1;
            (function w() {
                var A = x ? x - parseInt(r * (x / l)) : 0;
                var B = j ? j - parseInt(r * (j / l)) : 0;
                var C = o < k ? o + parseInt(r * ((k - o) / l || 1)) : k;
                var z = c < s ? c + parseInt(r * ((s - c) / l || 1)) : s;
                b.css({
                    clip: "rect(" + A + "px " + z + "px " + C + "px " + B + "px)"
                });
                (r++ <= l) ? setTimeout(w, 13): d.css("display", "none")
            })()
        });
        e.cssBefore = {
            display: "block",
            opacity: 1,
            top: 0,
            left: 0
        };
        e.animIn = {
            left: 0
        };
        e.animOut = {
            left: 0
        }
    }
})(jQuery);
jQuery.cookie = function(b, j, m) {
    if (typeof j != "undefined") {
        m = m || {};
        if (j === null) {
            j = "";
            m.expires = -1
        }
        var e = "";
        if (m.expires && (typeof m.expires == "number" || m.expires.toUTCString)) {
            var f;
            if (typeof m.expires == "number") {
                f = new Date();
                f.setTime(f.getTime() + (m.expires * 24 * 60 * 60 * 1000))
            } else {
                f = m.expires
            }
            e = "; expires=" + f.toUTCString()
        }
        var l = m.path ? "; path=" + (m.path) : "";
        var g = m.domain ? "; domain=" + (m.domain) : "";
        var a = m.secure ? "; secure" : "";
        document.cookie = [b, "=", encodeURIComponent(j), e, l, g, a].join("")
    } else {
        var d = null;
        if (document.cookie && document.cookie != "") {
            var k = document.cookie.split(";");
            for (var h = 0; h < k.length; h++) {
                var c = jQuery.trim(k[h]);
                if (c.substring(0, b.length + 1) == (b + "=")) {
                    d = decodeURIComponent(c.substring(b.length + 1));
                    break
                }
            }
        }
        return d
    }
};
jQuery.fn.extend({
    everyTime: function(a, b, c, d) {
        return this.each(function() {
            jQuery.timer.add(this, a, b, c, d)
        })
    },
    oneTime: function(a, b, c) {
        return this.each(function() {
            jQuery.timer.add(this, a, b, c, 1)
        })
    },
    stopTime: function(a, b) {
        return this.each(function() {
            jQuery.timer.remove(this, a, b)
        })
    }
});
jQuery.extend({
    timer: {
        global: [],
        guid: 1,
        dataKey: "jQuery.timer",
        regex: /^([0-9]+(?:\.[0-9]*)?)\s*(.*s)?$/,
        powers: {
            ms: 1,
            cs: 10,
            ds: 100,
            s: 1000,
            das: 10000,
            hs: 100000,
            ks: 1000000
        },
        timeParse: function(c) {
            if (c == undefined || c == null) {
                return null
            }
            var a = this.regex.exec(jQuery.trim(c.toString()));
            if (a[2]) {
                var b = parseFloat(a[1]);
                var d = this.powers[a[2]] || 1;
                return b * d
            } else {
                return c
            }
        },
        add: function(d, b, c, f, h) {
            var a = 0;
            if (jQuery.isFunction(c)) {
                if (!h) {
                    h = f
                }
                f = c;
                c = b
            }
            b = jQuery.timer.timeParse(b);
            if (typeof b != "number" || isNaN(b) || b < 0) {
                return
            }
            if (typeof h != "number" || isNaN(h) || h < 0) {
                h = 0
            }
            h = h || 0;
            var g = jQuery.data(d, this.dataKey) || jQuery.data(d, this.dataKey, {});
            if (!g[c]) {
                g[c] = {}
            }
            f.timerID = f.timerID || this.guid++;
            var e = function() {
                if ((++a > h && h !== 0) || f.call(d, a) === false) {
                    jQuery.timer.remove(d, c, f)
                }
            };
            e.timerID = f.timerID;
            if (!g[c][f.timerID]) {
                g[c][f.timerID] = window.setInterval(e, b)
            }
            this.global.push(d)
        },
        remove: function(c, b, d) {
            var e = jQuery.data(c, this.dataKey),
                a;
            if (e) {
                if (!b) {
                    for (b in e) {
                        this.remove(c, b, d)
                    }
                } else {
                    if (e[b]) {
                        if (d) {
                            if (d.timerID) {
                                window.clearInterval(e[b][d.timerID]);
                                delete e[b][d.timerID]
                            }
                        } else {
                            for (var d in e[b]) {
                                window.clearInterval(e[b][d]);
                                delete e[b][d]
                            }
                        }
                        for (a in e[b]) {
                            break
                        }
                        if (!a) {
                            a = null;
                            delete e[b]
                        }
                    }
                }
                for (a in e) {
                    break
                }
                if (!a) {
                    jQuery.removeData(c, this.dataKey)
                }
            }
        }
    }
});
jQuery(window).bind("unload", function() {
    jQuery.each(jQuery.timer.global, function(a, b) {
        jQuery.timer.remove(b)
    })
});
jQuery.fn.replaceClass = function(a, b) {
    this.each(function() {
        var c = $(this);
        c.addClass(b);
        c.removeClass(a)
    })
};
(function(f) {
    function h(j, k, l) {
        j.siblings(".prev-arrow").remove();
        j.siblings(".next-arrow").remove();
        j.before('<a href="#" class="prev-arrow" onMouseOver="showSelectButton();" >' + l + "</a>");
        j.before('<a href="#" class="next-arrow" onMouseOver="showSelectButton();" >' + k + "</a>");
        j.parent().children(".prev-arrow").bind("click", function(m) {
            return e(j)
        });
        j.parent().children(".next-arrow").bind("click", function(m) {
            return d(j)
        })
    }

    function e(k) {
        var j = true;
        var o = null;
        var n = null;
        f.each(k.children("li"), function(q, r) {
            var s = f(this);
            if (s.css("display") === "none") {
                if (j) {
                    o = s
                }
            }
            if (s.css("display") == "block" || s.css("display") == "list-item") {
                n = s.css("display");
                j = false
            }
        });
        g(k);
        var l = false;
        if (o !== null) {
            o.css("display", n);
            var m = o.parent().height() - o.height();
            l = c(k, o.prevAll(), m, n)
        }
        b(k, true, l);
        if (!l) {
            DummyAjaxRequest()
        }
        return false
    }

    function d(k) {
        var j = false;
        var o = null;
        var n = null;
        f.each(k.children("li"), function(q, r) {
            var s = f(this);
            if (s.css("display") === "block" || s.css("display") === "list-item") {
                n = s.css("display");
                j = true
            }
            if (j) {
                if (s.css("display") == "none") {
                    if (o === null) {
                        o = s
                    }
                }
            }
        });
        g(k);
        var l = false;
        if (o !== null) {
            o.css("display", n);
            var m = o.parent().height() - o.height();
            l = c(k, o.nextAll(), m, n)
        } else {}
        b(k, l, true);
        if (!l) {
            DummyAjaxRequest()
        }
        return false
    }

    function g(j) {
        f.each(j.children("li"), function(k, l) {
            var m = f(this);
            m.css("display", "none")
        })
    }

    function a(j) {
        return c(j, j.children("li"), j.height(), "block")
    }

    function c(m, q, r, j) {
        var l = false;
        var k = m.parent().children(".next-arrow");
        var n = k.outerHeight();
        var o = r - n;
        f.each(q, function(t, u) {
            var w = f(this);
            var s = w.outerHeight();
            if (!l && s <= o) {
                o = o - s;
                if (j !== undefined) {
                    w.css("display", j)
                }
            } else {
                w.css("display", "none");
                l = true
            }
        });
        return l
    }

    function b(k, l, j) {
        if (!l && !j) {
            k.parent().children(".prev-arrow").css("display", "none");
            k.parent().children(".next-arrow").css("display", "none")
        } else {
            k.parent().children(".prev-arrow").css("visibility", j ? "visible" : "hidden");
            k.parent().children(".next-arrow").css("visibility", l ? "visible" : "hidden");
            f("#pagination-next-button").removeClass("navigation_jumpto");
            f("#pagination-prev-button").removeClass("navigation_jumpto");
            if (l ^ j) {
                k.parent().children((l) ? "#pagination-next-button" : "#pagination-prev-button").addClass("navigation_jumpto")
            }
        }
    }
    f.fn.extend({
        paginate: function(j, k) {
            return this.each(function() {
                var m = f(this);
                g(m);
                h(m, j, k);
                var l = a(m);
                b(m, l, false)
            })
        }
    })
})(jQuery);
(function(a) {
    a.extend({
        flicker: function(b) {
            a(b).live("mouseover", function(d) {
                var c = a(this);
                c.everyTime(50, function(f) {
                    var e = 4;
                    var g = Math.floor(Math.random() * e) + 1;
                    for (f = 1; f <= e; f++) {
                        c.removeClass("frame" + f)
                    }
                    c.addClass("frame" + g)
                })
            });
            a(b).live("mouseleave", function(f) {
                var d = a(this);
                d.stopTime();
                var c = 4;
                for (i = 1; i <= c; i++) {
                    d.removeClass("frame" + i)
                }
            })
        }
    })
})(jQuery);
(function(c) {
    function a(e) {
        e = c(e);
        if (e.css("position") == "absolute") {
            return
        }
        var g = e.offset();
        var j = g[1];
        var h = g[0];
        var f = e.clientWidth;
        var d = e.clientHeight;
        e._originalLeft = h - parseFloat(e.css("left") || 0);
        e._originalTop = j - parseFloat(e.css("top") || 0);
        e._originalWidth = e.width();
        e._originalHeight = e.height();
        e.css("position", "absolute");
        e.css("top", j + "px");
        e.css("left", h + "px");
        e.width(f + "px");
        e.height(d + "px");
        return e
    }

    function b(e, d) {
        var f = this;
        this.options = d;
        this.useCaptions = this.options.captions;
        this.elem = e;
        c(this.elem).css({
            overflow: "hidden",
            position: "relative"
        });
        this.stack = c(this.elem).children().get();
        this.stackCount = this.stack.length;
        this.currPos = this.options.startIndex - 1;
        this.currIndex = this.currPos;
        this.timer = 0;
        c(this.stack).click(function() {
            f.handleClick(this)
        });
        if (this.stackCount <= 1) {
            c("#next").css("visibility", "hidden");
            c("#previous").css("visibility", "hidden")
        } else {
            c("#next").click(function() {
                f.handleNext()
            });
            c("#previous").click(function() {
                f.handlePrevious()
            })
        }
        this.goTo(this.currPos);
        this.goTo(0);
        this.autoplayer = null
    }
    b.prototype = {
        autoPlay: function() {
            if ((this.currIndex + 2) > this.stackCount) {
                this.currIndex = 0
            }
            this.currIndex = this.currIndex + 1;
            this.goTo(this.currIndex)
        },
        handleWindowResize: function(d) {},
        handleWheel: function(f) {
            var e = this.currIndex + f;
            if (e >= 0 && e < this.stackCount) {
                this.goTo(e)
            }
        },
        handleNext: function() {
            this.goTo((this.currIndex + 1) % this.stackCount);
            return false
        },
        handlePrevious: function() {
            this.goTo((this.currIndex - 1 + this.stackCount) % this.stackCount);
            return false
        },
        handleSliderChange: function(d) {
            this.goTo(d)
        },
        handleKey: function(e) {
            var d = this.currIndex;
            if (e.keyCode == 39) {
                if ((d + 1) >= 0 && (d + 1) < this.stackCount) {
                    this.goTo(d + 1)
                }
            } else {
                if (e.keyCode == 37) {
                    if ((d - 1) >= 0 && (d - 1) < this.stackCount) {
                        this.goTo(d - 1)
                    }
                }
            }
        },
        handleSlider: function(d) {
            if (typeof(d) !== "undefined") {
                this.goTo(d)
            }
        },
        handleClick: function(d) {
            v = d.getAttribute("index");
            this.goTo(v)
        },
        getCurrentPos: function() {
            return this.currPos
        },
        goTo: function(d) {
            console.log("goto : " + d);
            this.currIndex = d;
            this.slideTo(d * this.options.flex * -1)
        },
        step: function() {
            console.log("step : target=" + this.target + " currPos=" + this.currPos);
            if (this.target < this.currPos - 1 || this.target > this.currPos + 1) {
                this.moveTo(this.currPos + (this.target - this.currPos) / 5);
                var d = this;
                window.setTimeout(function() {
                    d.step()
                }, this.options.interval);
                this.timer = 1
            } else {
                console.log("end");
                this.timer = 0
            }
        },
        slideTo: function(d) {
            this.target = d;
            if (this.timer == 0) {
                var e = this;
                window.setTimeout(function() {
                    e.step()
                }, this.options.interval);
                this.timer = 1
            }
        },
        moveTo: function(h) {
            var m = h;
            this.currPos = h;
            var d = c(this.elem).width();
            var n = c(this.elem).height();
            var k = this.elem.offsetTop;
            var o = d * 0.5;
            var e = n;
            var j = this.stackCount;
            var g = this.currIndex;
            var l = this.stackCount;
            var f = this;
            c(this.stack).each(function(t) {
                var r = Math.abs(g - t);
                var q = l - r;
                a(this);
                this.setAttribute("index", t);
                var u = Math.sqrt(10000 + m * m * 1) + 100;
                var s = m / u * o + o;
                c(this).css({
                    left: (s - 40 / u * e) + "px",
                    top: (30 / u * o + 0) + "px",
                    width: (74 / u * e) + "px",
                    height: (130 / u * e) + "px",
                    zIndex: q
                });
                m += f.options.flex
            })
        },
        getStackCount: function() {
            return this.stackCount
        }
    };
    jQuery.fn.coverflow = function(d) {
        d = c.extend({
            startIndex: 3,
            interval: 60,
            flex: 100
        }, d);
        return this.each(function() {
            new b(this, d)
        })
    }
})(jQuery);
(function(b) {
    function a(c) {
        c.children().css("display", "none");
        c(":first-child").css("display", "block")
    }
    b.fn.extend({
        shrink: function() {
            return this.each(function() {
                var d = b(this);
                var f = d.width();
                var c = d.css("font-size");
                var e = parseFloat(c);
                while (f > maxWidth) {
                    e = e * 0.9;
                    d.css("font-size", e);
                    f = d.width()
                }
            })
        },
    })
})(jQuery);
(function(b) {
    function a(c) {
        c.children().css("display", "none");
        c(":first-child").css("display", "block")
    }
    b.fn.extend({
        rotate: function() {
            return this.each(function() {
                var c = b(this)
            })
        },
    })
})(jQuery);

function hideSelectButton() {
    $("#footer_select_button").css("display", "none");
    $("#footer_select_button").addClass("fake_cross_button")
}

function showSelectButton() {
    $("#footer_select_button").css("display", "block");
    $("#footer_select_button").removeClass("fake_cross_button")
}

function bindDPADToLogoSelector() {
    $("#previousLogoButton").addClass("dpad_left_button");
    $("#nextLogoButton").addClass("dpad_right_button")
}

function unbindDPADToLogoSelector() {
    $("#previousLogoButton").removeClass("dpad_left_button");
    $("#nextLogoButton").removeClass("dpad_right_button")
}

function ResetCursor() {
    try {
        EA.ResetCursor()
    } catch (a) {}
}

function MessageBox(a) {
    try {
        EA.MessageBox(a)
    } catch (b) {
        alert(a)
    }
}

function TagInputs() {
    try {
        EA.TagInputs()
    } catch (a) {}
}

function DummyAjaxRequest() {
    try {
        EA.UpdateNav()
    } catch (a) {}
}

function UpdateNavigation() {
    try {
        EA.UpdateNav()
    } catch (a) {}
}

function PlayAudio(a) {
    try {
        EA.PlayAudio(a)
    } catch (b) {}
}

function PlayAudioError() {
    PlayAudio("core_error")
}
$(document).ready(function() {
    $("#rotator").cycle({
        fx: "fade"
    });
    $.flicker(".menu-item");
    $.flicker(".roster-item");
    $.flicker(".thumb-item");
    $.flicker(".feed-item");
    $.flicker(".attribute");
    $.flicker(".table .row a");
    $.flicker(".simple-users-list li a");
    $.flicker(".button-link");
    $.flicker(".list-item");
    $.flicker(".page-link");
    $.flicker("#user-team-info");
    $.flicker(".leaderboard-arrow");
    $.flicker(".board-selector");
    PlayAudioOnErrors()
});

function PlayAudioOnErrors() {
    var a = false;
    $(".field-validation-error").each(function() {
        var b = $(this);
        if (b.html() !== "") {
            a = true
        }
    });
    if (a) {
        PlayAudioError()
    }
}

function SwapThumb(b, a) {
    $("#" + b).attr("src", a)
}

function ShowPopup() {
    ResetCursor();
    $("#container").css("display", "none");
    $("#content").css("visibility", "hidden");
    $("#popup").css("visibility", "visible");
    if ($("#page_back_button").is(".circle_button")) {
        $("#footer #page_back_button.circle_button").removeClass("circle_button").addClass("circle_button_disabled")
    }
    if ($("#page_back_button").is(".fake_circle_button")) {
        $("#footer #page_back_button.fake_circle_button").removeClass("fake_circle_button").addClass("fake_circle_button_disabled")
    }
    $("#flag_button").removeClass("select_button");
    $("#skatefeed-helper-button").removeClass("square_button");
    $("#popup-cancel-button").addClass("circle_button");
    DisableTabNavigation();
    TagInputs();
    DummyAjaxRequest()
}

function HidePopup() {
    ResetCursor();
    $("#popup-content").html("");
    $("#popup").css("visibility", "hidden");
    $("#content").css("visibility", "visible");
    $("#container").css("display", "block");
    if ($("#page_back_button").is(".circle_button_disabled")) {
        $("#footer #page_back_button.circle_button_disabled").addClass("circle_button").removeClass("circle_button_disabled")
    }
    if ($("#page_back_button").is(".fake_circle_button_disabled")) {
        $("#footer #page_back_button.fake_circle_button_disabled").addClass("fake_circle_button").removeClass("fake_circle_button_disabled")
    }
    $("#flag_button").addClass("select_button");
    $("#skatefeed-helper-button").addClass("square_button");
    $("#popup-cancel-button").removeClass("circle_button");
    EnableTabNavigation();
    DummyAjaxRequest()
}

function ShowMessage(a) {
    ShowMessageBox("Skate Feed Message", $(a).children(".fullbody").html())
}

function ShowMessageBox(b, a) {
    $("#messagebox-title").html(b);
    $("#messagebox-message").html(a);
    $("#content").css("visibility", "hidden");
    $("#messagebox").css("visibility", "visible");
    $("#messagebox_back_button").addClass("fake_circle_button");
    $("#close_message_box_button").addClass("fake_cross_button");
    $("#close_message_box_button").addClass("navigation_jumpto");
    DisableTabNavigation();
    DummyAjaxRequest()
}

function HideMessageBox() {
    $("#close_message_box_button").removeClass("fake_cross_button");
    $("#close_message_box_button").removeClass("navigation_jumpto");
    $("#messagebox_back_button").removeClass("fake_circle_button");
    $("#messagebox-message").html("");
    $("#messagebox").css("visibility", "hidden");
    $("#content").css("visibility", "visible");
    $("#messagebox #circle_button").attr("id", "circle_button_disabled");
    EnableTabNavigation();
    DummyAjaxRequest()
}

function DisableTabNavigation() {
    $(".l2_button").replaceClass("l2_button", "disabled_l2_button");
    $(".r2_button").replaceClass("r2_button", "disabled_r2_button")
}

function EnableTabNavigation() {
    $(".disabled_l2_button").replaceClass("disabled_l2_button", "l2_button");
    $(".disabled_r2_button").replaceClass("disabled_r2_button", "r2_button")
}

function WebKitHistoryback() {
    var a = document.URL;
    parent.history.back();
    if (document.URL == a) {
        parent.history.back()
    }
}

function ShowConfirmDialogBox(c, a, b) {
    $("#content-jumpto").removeClass("navigation_jumpto");
    $("#close_confirm").addClass("navigation_jumpto");
    $("#yes_confirm").attr("href", b);
    $("#confirmdialog-title").html(c);
    $("#confirmdialog-message").html(a);
    $("#confirmdialog").css("visibility", "visible");
    $("#content").css("visibility", "hidden");
    $("#dialogbox_back_button").addClass("circle_button");
    DisableTabNavigation();
    DummyAjaxRequest()
}

function HideConfirmDialogBox() {
    $("#content-jumpto").addClass("navigation_jumpto");
    $("#yes_confirm").attr("href", "#");
    $("#close_confirm").removeClass("navigation_jumpto");
    $("#confirmdialog-message").html("");
    $("#confirmdialog").css("visibility", "hidden");
    $("#content").css("visibility", "visible");
    $("#dialogbox_back_button").removeClass("circle_button");
    EnableTabNavigation();
    DummyAjaxRequest()
}

function OnWebRequestCompleted(b, a) {
    HidePopup()
}

function tryActivateSubmitButton(b) {
    var c = b.form;
    $("#SubmitButton").css("display", "block");
    for (var a = 0; a < c.elements.length; a++) {
        var d = c.elements[a];
        if (d.type == "text") {
            var e = jQuery.trim(d.value);
            if (e == "") {
                $("#SubmitButton").css("display", "none")
            }
        }
    }
}

function tryActivateAssignRoleButton(b) {
    var c = b.form;
    $(b).attr("navigationdown", "SubmitButton");
    $("#SubmitButton").css("visibility", "visible");
    for (var a = 0; a < c.elements.length; a++) {
        var d = c.elements[a];
        if (d.type == "text" && d.value != null) {
            var e = jQuery.trim(d.value);
            if (e.length == 0) {
                $("#SubmitButton").css("visibility", "hidden");
                $(b).attr("navigationdown", "ignore")
            }
        }
    }
}
var currentLogo = 0;

function resetLogo(a) {
    currentLogo = a
}

function nextLogo(c) {
    console.log("currentLogo = " + currentLogo);
    if (currentLogo < c - 1) {
        $("#logo" + currentLogo).css("display", "none");
        currentLogo++;
        $("#logo" + currentLogo).css("display", "inline");
        var a = document.getElementById("logo" + currentLogo).getAttribute("src");
        var b = $("#logoId" + currentLogo).html();
        document.getElementById("logoURL").setAttribute("value", a);
        document.getElementById("logoId").setAttribute("value", b);
        $("#nextLogoButton").css("opacity", "1");
        $("#previousLogoButton").css("opacity", "1");
        if (currentLogo == c - 1) {
            $("#nextLogoButton").css("opacity", "0")
        }
    }
}

function previousLogo(c) {
    if (currentLogo > 0) {
        $("#logo" + currentLogo).css("display", "none");
        currentLogo--;
        $("#logo" + currentLogo).css("display", "inline");
        var a = document.getElementById("logo" + currentLogo).getAttribute("src");
        var b = $("#logoId" + currentLogo).html();
        document.getElementById("logoURL").setAttribute("value", a);
        document.getElementById("logoId").setAttribute("value", b);
        $("#nextLogoButton").css("opacity", "1");
        $("#previousLogoButton").css("opacity", "1");
        if (currentLogo === 0) {
            $("#previousLogoButton").css("opacity", "0")
        }
    }
}

function setAttribute(c, a, e, b, d) {
    $("#" + b).html(d);
    $.post(c, a, function() {
        HidePopup()
    })
}

function setAttribute2(b, a) {
    $.post(b, a, function() {
        HidePopup()
    })
}

function setSearchType(a, b, c) {
    if (a != "ID_TEAMS_SEARCH_DEFAULT_VALUE") {
        $("#" + c).val(a);
        $("#" + c + "_p").html(b)
    } else {
        $("#" + c).val("");
        $("#" + c + "_p").html(b)
    }
    HidePopup()
}

function setrole(c, b, g, f, d) {
    var e = $(c).html();
    $("#customrole").val(e);
    var a = "&id=" + f + "&clubid=" + d + "&role=" + g;
    $.post(b, a, function() {
        HidePopup()
    })
}

function setrole2(b, e, c) {
    var d = $("#createrole").val();
    var a = "&id=" + e + "&clubid=" + c + "&role=" + d;
    $.post(b, a, function() {
        HidePopup()
    })
}

function setChallenges(b, a, d, c) {
    setAttribute(b, "enabled=" + a + "&id=" + d, c, "challenges", a ? "Yes" : "No")
}

function setStyle(a, c, b, e, d) {
    setAttribute(a, "style=" + c + "&id=" + e, d, "style", b)
}

function setRecruiting(a, c, b, e, d) {
    setAttribute(a, "recruiting=" + c + "&id=" + e, d, "recruiting", b)
}

function setRegion(a, b, c, e, d) {
    setAttribute(a, "region=" + b + "&id=" + e, d, "region", c)
}

function setSpecialization(a, b, c, e, d) {
    setAttribute(a, "specialization=" + b + "&id=" + e, d, "specialization", c)
}

function setWallpaper(c, b, a, e, d) {
    setAttribute(c, "wallpaper=" + b + "&id=" + e, d, "wallpaper", a)
}

function setCustomColour(a, b, c, e, d) {
    setAttribute(a, "customcolour=" + b + "&id=" + e, d, "customcolour", c)
}

function setLogo(a, d) {
    var c = $("#logoURL").val();
    var b = $("#logoId").val();
    document.getElementById("logo").setAttribute("src", c);
    setAttribute2(a, "logoId=" + b + "&id=" + d + "&logoURL=" + c);
    HidePopup()
}

function setWallpaper(a, d) {
    var c = $("#logoURL").val();
    var b = $("#logoId").val();
    document.getElementById("wallpaper").setAttribute("src", c);
    setAttribute2(a, "logoId=" + b + "&id=" + d + "&logoURL=" + c);
    HidePopup()
}

function SubmitForm(a) {
    var b = FindOwningForm(a);
    b.submit()
}

function SubmitForm(a) {
    var b = FindOwningForm(a);
    b.submit()
}

function FindOwningForm(a) {
    if (a.tagName === "FORM") {
        return a
    } else {
        return FindOwningForm(a.parentNode)
    }
}

function EditPlayerQuote(c) {
    var b = $("form").attr("action");
    var e = $("#id").attr("value");
    var d = $("#message").val();
    var a = "id=" + e + "&message=" + d;
    $.post(b, a, function(f) {
        HidePopup();
        $.globalEval(f)
    })
}

function SendTeamMessage(c) {
    var b = $("form").attr("action");
    var e = $("#id").attr("value");
    var d = $("#message").val();
    var a = "id=" + e + "&message=" + d;
    $.post(b, a, function() {
        HidePopup()
    })
}

function UpdateTeamMotto(c) {
    var b = $("form").attr("action");
    var e = $("#id").attr("value");
    var d = $("#motto").val();
    var a = "id=" + e + "&motto=" + d;
    $.post(b, a, function(f) {
        HidePopup();
        $.globalEval(f)
    })
}
var currentPage = 0;
var newPage = 0;

function OutroPage() {
    if (direction == "prev") {
        newPage -= 1;
        if ($("#user_page_" + newPage).size() == 0) {
            newPage = numPages - 1
        }
        var a = "810px"
    } else {
        newPage += 1;
        if ($("#user_page_" + newPage).size() == 0) {
            newPage = 0
        }
        var a = "-810px"
    }
    $("#user_page_" + currentPage).animate({
        left: a
    }, {
        duration: 150,
        complete: function() {
            IntroPage()
        }
    })
}

function IntroPage() {
    var a = "810px";
    if (direction == "prev") {
        a = "-810px"
    }
    $("#user_page_" + currentPage).css({
        display: "none"
    });
    $("#user_page_" + newPage).css({
        left: a,
        display: "block"
    }).animate({
        left: "0"
    }, {
        duration: 200
    });
    currentPage = newPage
}

function AnimateOut(b, d, c) {
    if (c == "next-arrow") {
        var a = "-810px"
    } else {
        var a = "810px"
    }
    $("#" + d + " ." + c).removeClass("navigation_jumpto");
    $("#" + d).animate({
        left: a
    }, {
        duration: 150,
        complete: function() {
            AnimateIn(b, d, c)
        }
    })
}

function AnimateIn(a, d, c) {
    if (c == "next-arrow") {
        var b = "810px"
    } else {
        var b = "-810px"
    }
    $("#" + d).css({
        display: "none"
    });
    $("#" + a).css({
        left: b,
        display: "block"
    }).animate({
        left: "0"
    }, {
        duration: 200,
        complete: function() {
            SetPageIn(a, c)
        }
    })
}

function SetPageIn(a, b) {
    $("#" + a + " ." + b).addClass("navigation_jumpto");
    DummyAjaxRequest()
}

function TabSetProfile() {
    TabSet("profile");
    ShowButtons("profile");
    HideButtons("roster");
    HideButtons("content")
}

function TabSetRoster() {
    TabSet("roster");
    HideButtons("profile");
    ShowButtons("roster");
    HideButtons("content")
}

function TabSetContent() {
    TabSet("content");
    HideButtons("profile");
    HideButtons("roster");
    ShowButtons("content")
}

function TabSet2(a, b) {
    TabSet("tab" + a);
    for (i = 0; i < b; i++) {
        HideButtons("tab_" + i)
    }
    ShowButtons("tab_" + a)
}

function doAlert() {
    alert("bye")
}

function SetTabContent(a) {
    $.get(a, function(b) {
        $("#tabbed-content").html(b)
    })
}

function TabRight() {}

function TabLeft() {}

function SizeScroller() {
    var b = 400 / 5;
    var a = b * 5;
    if (a >= 400) {
        $("#scroller img").css("visibility", "hidden")
    } else {
        $("#scroller img").attr("height", a)
    }
}

function limitText(b, a) {
    if (b.value.length > a) {
        b.value = b.value.substring(0, a)
    }
}

function shrinkTitleWidth(a, f) {
    var c = $("#" + a);
    var e = c.width();
    var b = c.css("font-size");
    var d = parseFloat(b);
    while (e > f) {
        d = d * 0.9;
        c.css("font-size", d);
        e = c.width()
    }
}
$(document).ready(function() {
    shrinkTitleWidth("main-title", 600)
});

function dynamicalUpdateElement(c, a) {
    var b = $("#" + c);
    if (b.length > 0) {
        if (b.css("visibility") != "hidden") {
            if (b.css("display") != "none") {
                if (a.indexOf("?") == -1) {
                    a = a + "?disableloadingupdate"
                } else {
                    a = a + "&disableloadingupdate"
                }
                $.get(a, function(d) {
                    b.html(d)
                })
            }
        }
    }
}

function showExceptions() {
    $("#content").css("visibility", "hidden");
    $("#exceptions").css("visibility", "visible");
    $("#exceptions .close_button").addClass("circle_button");
    DisableTabNavigation()
}

function closeExceptions() {
    $("#exceptions").css("visibility", "hidden");
    $("#content").css("visibility", "visible");
    $("#exceptions .close_button").removeClass("circle_button");
    EnableTabNavigation();
    DummyAjaxRequest()
}

function previousLeaderboardPage(b, a, d, c) {
    updateLeaderboardPage(b, a, d, c, rankOffset[c] - 10)
}

function nextLeaderboardPage(b, a, d, c) {
    updateLeaderboardPage(b, a, d, c, rankOffset[c] + 10)
}

function updateLeaderboardPage(b, a, d, c, e) {
    b = b + "?disableloadingupdate&offset=" + e;
    $.get(b, function(f) {
        if (f !== "") {
            $("#leaderboard-view-" + a).html(f);
            rankOffset[c] = e;
            toggleLeaderboardNextPrevious(d, c)
        }
    })
}

function toggleLeaderboardNextPrevious(e, d) {
    var b = $();
    var c = $("#leaderboard-view-" + d + " #leaderboard .row").size();
    var a = $("#leaderboard-view-" + d + " #leaderboard :first.row .col1").html();
    var f = $("#leaderboard-view-" + d + " #leaderboard .row:last .col1").html();
    b = $("#leaderboard-view-" + d).parent();
    prevArrow = b.children(".prev-arrow");
    nextArrow = b.children(".next-arrow");
    prevArrowVisible = true;
    nextArrowVisible = true;
    if (c == 0 || a == "1" || (d == "Teammates" && rankOffset[d] < 10) || (d == "Friends" && rankOffset[d] < 10)) {
        prevArrowVisible = false;
        prevArrow.css("visibility", "hidden")
    } else {
        prevArrow.css("visibility", "visible")
    }
    if (c < 10 || f == e) {
        nextArrowVisible = false;
        nextArrow.css("visibility", "hidden")
    } else {
        nextArrow.css("visibility", "visible")
    }
    if (nextArrowVisible ^ prevArrowVisible) {
        $((nextArrowVisible) ? "#pagination-next-button" : "#pagination-prev-button").addClass("navigation_jumpto")
    }
    if (!nextArrowVisible || !prevArrowVisible) {
        UpdateNavigation()
    }
}

function toggleFlagButton(a) {
    if (a) {
        $("#footer #flag_label").css("visibility", "visible");
        $("#footer #flag_button").css("visibility", "visible");
        $("#footer #flag_button").addClass("select_button")
    } else {
        $("#footer #flag_button").css("visibility", "hidden");
        $("#footer #flag_label").css("visibility", "hidden");
        $("#footer #flag_button").removeClass("select_button")
    }
}

function toggleSelectButton(a) {
    if (a) {
        $("#footer_select_button").show()
    } else {
        $("#footer_select_button").hide()
    }
}

function setCookie(b, c, a) {
    var d = new Date();
    d.setDate(d.getDate() + a);
    document.cookie = b + "=" + escape(c) + ((a == null) ? "" : ";expires=" + d.toGMTString()) + ";path=/"
}

function removeTabCookie(a) {
    setCookie("TabPosition_" + a, "0", -1)
}

function cancelRequest(b, a) {
    $.post(a, null, function(c) {
        $(b).parent().remove()
    })
}

function rejectInvitation(b, a) {
    $.post(a, null, function(c) {
        $(b).parent().remove()
    })
}

function sendInvitation(b, a) {
    $.post(a, null, function(c) {
        $(b).parent().remove()
    })
}

function cancelInvitation(b, a) {
    $.post(a, null, function(c) {
        $(b).parent().remove()
    })
}

function acceptRequest(b, a) {
    $.post(a, null, function(c) {
        $(b).parent().remove()
    })
}

function rejectRequest(b, a) {
    $.post(a, null, function(c) {
        $(b).parent().remove()
    })
}

function paginateSkateFeedTabs(a, b, c) {
    if (a == 0) {
        $("#feed-items-news").paginate(b, c)
    } else {
        if (a == 1) {
            $("#feed-items-Content").paginate(b, c)
        } else {
            if (a == 2) {
                $("#feed-items-Contacts").paginate(b, c)
            }
        }
    }
}

function paginateInvitePlayersTabs(a, b, c) {
    if (a == 0) {
        $("#friends-list").paginate(b, c)
    } else {
        if (a == 1) {
            $("#recentplayers-list").paginate(b, c)
        }
    }
}

function paginatePlayerProfileTabs(a, b, c) {
    if (a == 3) {
        $("#awards-list").paginate(b, c)
    }
}

function swapPlayerProfileInvite(a, b) {
    $(a).before("<p>" + b + "</p>");
    $(a).before().addClass("menu-item item-dimmed");
    $(a).remove()
}
var tabs = {
    currentTabIndex: 0,
    totalTabs: 0,
    onChangeTab: null,
    name: "",
    Setup: function(c, a, b, d) {
        this.currentTabIndex = a;
        this.totalTabs = b;
        this.name = c;
        this.onChangeTab = d;
        this._updateTabs()
    },
    JumpToTab: function(a) {
        this.currentTabIndex = a;
        this._updateTabs();
        return false
    },
    NextTab: function() {
        this.currentTabIndex = this.currentTabIndex + 1;
        if (this.currentTabIndex >= this.totalTabs) {
            this.currentTabIndex = 0
        }
        this._updateTabs();
        return false
    },
    PreviousTab: function() {
        this.currentTabIndex = this.currentTabIndex - 1;
        if (this.currentTabIndex < 0) {
            this.currentTabIndex = this.totalTabs - 1
        }
        this._updateTabs();
        return false
    },
    _updateTabs: function() {
        ResetCursor();
        this._setHeader("tab" + this.currentTabIndex);
        this._updateTabContent();
        this._updateCookie();
        DummyAjaxRequest();
        this.onChangeTab(this.currentTabIndex)
    },
    _updateCookie: function() {
        setCookie("TabPosition_" + this.name, this.currentTabIndex, 1)
    },
    _setHeader: function(a) {
        $("#tabs .tab.current").removeClass("current");
        $("#" + a + "-tab").addClass("current")
    },
    _updateTabContent: function() {
        for (i = 0; i < this.totalTabs; i++) {
            $("#tabbed-content" + i).css("display", "none");
            $("#tabbed-content" + i).css("visibility", "hidden")
        }
        $("#tabbed-content" + this.currentTabIndex).css("display", "block");
        $("#tabbed-content" + this.currentTabIndex).css("visibility", "visible")
    },
    _showButtons: function(a) {
        $("#r_trigger_" + a).addClass("showing");
        $("#r_trigger_" + a).removeClass("hiding");
        $("#l_trigger_" + a).addClass("showing");
        $("#l_trigger_" + a).removeClass("hiding");
        $("#r_trigger_" + a).addClass("r2_button");
        $("#l_trigger_" + a).addClass("l2_button")
    },
    _hideButtons: function(a) {
        $("#r_trigger_" + a).addClass("hiding");
        $("#r_trigger_" + a).removeClass("showing");
        $("#l_trigger_" + a).addClass("hiding");
        $("#l_trigger_" + a).removeClass("showing");
        $("#r_trigger_" + a).removeClass("r2_button");
        $("#l_trigger_" + a).removeClass("l2_button")
    }
};