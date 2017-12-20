(function ($) {

    var $self;

    var config = {
        selHandle: ".handle",
        clsCollapsed: "collapsed",
        selLi: "li",
        selUl: "ul",
        clsDisabledHandle: "disabled",
        clsAutoOpen: "autoOpen",
        clsKeepOpen: "keepOpen",
        clsKeepClose: "keepClose",
        onToggle: function (el, handle) { },
        onExpand: function (el, handle) { },
        onCollapse: function (el, handle) { },
        onSave: function (el) { },
        onLoad: function (el) { },
        onResize: function () { },
        preserve: false,
        debug: false,
        cookiePrefix: "comol_",
        autoLoad: true,
        selExtraCollapse: "",
        selfHandle: false,
        handleClickReturn: false
    };

    function ShowHideToggle(selector) {
        $self.find(selector).each(function () {
            var $li = $(this);

            var collapsed = !$li.hasClass(config.clsCollapsed);

            if (collapsed == true) {
                var $handle = $li.find(config.selHandle).first();
                $li.addClass(config.clsCollapsed);
                $handle.addClass(config.clsCollapsed);
                var $ul = $li.find(config.selUl).first();
                $ul.hide();
                if (config.selExtraCollapse != "") {
                    $collapsable = $li.find(config.selExtraCollapse);
                    $collapsable.hide();
                }
                config.onCollapse($li, $handle);

            } else if (collapsed == false) {
                var $handle = $li.find(config.selHandle).first();
                $li.removeClass(config.clsCollapsed);
                $handle.removeClass(config.clsCollapsed);
                var $ul = $li.find(config.selUl).first();
                $ul.show();
                if (config.selExtraCollapse != "") {
                    $collapsable = $li.find(config.selExtraCollapse);
                    $collapsable.show();
                }
                config.onExpand($li, $handle);

            }
        });

        config.onResize();
    }


    function ShowHide(selector, collapsed) {
        $self.find(selector).each(function () {
            var $li = $(this);

            if (collapsed == true) {
                var $handle = $li.find(config.selHandle).first();
                $li.addClass(config.clsCollapsed);
                $handle.addClass(config.clsCollapsed);
                var $ul = $li.find(config.selUl).first();
                $ul.hide();
                if (config.selExtraCollapse != "") {
                    $collapsable = $li.find(config.selExtraCollapse);
                    $collapsable.hide();
                }
                config.onCollapse($li, $handle);

            } else if (collapsed == false) {
                var $handle = $li.find(config.selHandle).first();
                $li.removeClass(config.clsCollapsed);
                $handle.removeClass(config.clsCollapsed);
                var $ul = $li.find(config.selUl).first();
                $ul.show();
                if (config.selExtraCollapse != "") {
                    $collapsable = $li.find(config.selExtraCollapse);
                    $collapsable.show();
                }
                config.onExpand($li, $handle);

            }
        });
        config.onResize();
    }

    function DeleteCookie(el) {
        var id = Id(el);
        var key = CookieKey(id);
        $.cookie(key, null);
    }
    function DeleteCookies(el) {
        el.find(config.selLi).each(function () {
            DeleteCookie($(this));
        });
    }

    var methods = {
        init: function (options) {
            function CookieKey(id) {
                var prefix = $self.data("cookie-prefix");
                if (typeof prefix != 'undefined') {
                    return config.cookiePrefix + $self.data("cookie-prefix") + "CollapsableStatus[" + id + "]";
                } else {
                    return config.cookiePrefix + "CollapsableStatus[" + id + "]";
                }

            }



            function Debug(msg) {
                if (config.debug) {
                    console.log(msg);
                }
            }

            function Id(el) {
                return el.attr("id");
            }

            function Save(el) {
                if (config.preserve) {
                    var id = Id(el);
                    var key = CookieKey(id);

                    var collapsed = el.hasClass(config.clsCollapsed);
                    Debug(id + " : " + collapsed);


                    $.cookie(key, collapsed, { expires: 1 });
                    Debug("Cookie (" + key + "): " + collapsed);

                    config.onSave(el);
                }
            }
            function Load(el) {
                if (config.preserve) {
                    var id = Id(el);
                    var key = CookieKey(id);

                    var collapsed = $.cookie(key);
                    Debug("Cookie (" + key + "): " + collapsed);

                    var $li = el;

                    if (collapsed == "true") {
                        var $handle = $li.find(config.selHandle).first();
                        $li.addClass(config.clsCollapsed);
                        $handle.addClass(config.clsCollapsed);
                        var $ul = $li.find(config.selUl).first();
                        $ul.hide();
                        if (config.selExtraCollapse != "") {
                            $collapsable = $li.find(config.selExtraCollapse);
                            $collapsable.hide();
                        }
                        config.onCollapse($li, $handle);

                    } else if (collapsed == "false") {
                        var $handle = $li.find(config.selHandle).first();
                        $li.removeClass(config.clsCollapsed);
                        $handle.removeClass(config.clsCollapsed);
                        var $ul = $li.find(config.selUl).first();
                        $ul.show();
                        if (config.selExtraCollapse != "") {
                            $collapsable = $li.find(config.selExtraCollapse);
                            $collapsable.show();
                        }
                        config.onExpand($li, $handle);

                    }


                    config.onLoad(el);
                }
            }



            if (options) $.extend(config, options);

            if (config.selfHandle == true) {
                config.selHandle = ":parent";
            };

            return this.each(function () {
                Debug("Start");
                $self = $(this);

                $self.find(config.selHandle).removeClass(config.clsDisabledHandle);



                /*$self.find(config.selLi).filter("."+config.clsAutoOpen).each(function(){
                 var $li = $(this);
                 var $parents = $li.parents();
                 alert($parents.size());
                 $parents.each(function(){
                 alert("ok");
                 var $parentLi = $(this);
                 var $handle = $parentLi.find(config.selHandle);
                 $parentLi.removeClass(config.clsCollapsed);
                 $handle.removeClass(config.clsCollapsed);
                 var $ul = $li.find(config.selUl);
                 $ul.show();
                 });
                 });*/

                $self.find(config.selLi).not("." + config.clsAutoOpen).not("." + config.clsKeepOpen).each(function () {
                    $li = $(this);
                    $handle = $li.find(config.selHandle).first();
                    $li.addClass(config.clsCollapsed);
                    $handle.addClass(config.clsCollapsed);
                    $ul = $li.find(config.selUl);
                    $ul.hide();

                    if (config.selExtraCollapse != "") {
                        $collapsable = $li.find(config.selExtraCollapse);
                        $collapsable.hide();
                    }

                    config.onCollapse($li, $handle);

                });
                config.onResize();

                $self.find(config.selLi).filter("." + config.clsKeepClose).add("." + config.clsKeepOpen).each(function () {
                    $li = $(this);
                    $handle = $li.find(config.selHandle).first();
                    $handle.addClass(config.clsDisabledHandle);
                });

                /*$self.find(config.selLi).not("."+config.clsAutoOpen).not("."+config.clsKeepOpen).each(function(){
                 $li = $(this);
                 $handle = $li.find(config.selHandle);
                 $li.addClass(config.clsCollapsed);
                 $handle.addClass(config.clsCollapsed);
                 $ul = $li.find(config.selUl);
                 $ul.hide();
                 });*/

                $self.find(config.selHandle).click(function () {
                    $handle = $(this);
                    $li = $handle.parents(config.selLi).first();
                    if (!$li.hasClass(config.clsKeepOpen) && !$li.hasClass(config.clsKeepClose)) {
                        $li.toggleClass(config.clsCollapsed);
                        $handle.toggleClass(config.clsCollapsed);
                        $ul = $li.find(config.selUl).first();
                        $ul.toggle();

                        if (config.selExtraCollapse != "") {
                            $collapsable = $li.find(config.selExtraCollapse);
                            $collapsable.toggle();
                        }

                        Save($li);
                        config.onToggle($li, $handle);
                        config.onResize();
                    }
                    return config.handleClickReturn;
                });


                if (config.autoLoad) {
                    //Debug("AutoLoading...");
                    $self.find(config.selLi).each(function () {
                        Load($(this));
                    });
                    //Debug("AutoLoading... finished");
                }
            });
        },
        expand: function (selector) {
            // IS
            ShowHide(selector, false);
        },
        collapse: function (selector) {
            // GOOD
            ShowHide(selector, true);
        },
        collapseAll: function () {
            // IS
            ShowHide(config.selLi, true);
        },
        expandAll: function () {
            // GOOD
            ShowHide(config.selLi, false);
        },
        toggleAll: function () {
            // !!!
            ShowHideToggle(config.selLi);
        },
        toggle: function (selector) {
            // !!!
            ShowHideToggle(selector);
        },
        deleteCookie: function (selector) {
            DeleteCookies(selector);
        },
        deleteCookies: function () {
            DeleteCookies($self);
        }
    };


    $.fn.collapsableTreeAdv = function (method) {

        if (methods[method]) {
            return methods[method].apply(this, Array.prototype.slice.call(arguments, 1));
        } else if (typeof method === 'object' || !method) {
            return methods.init.apply(this, arguments);
        } else {
            $.error('Method ' + method + ' does not exist on jQuery.tooltip');
        }


    }
})(jQuery);