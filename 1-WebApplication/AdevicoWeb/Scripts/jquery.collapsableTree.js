(function ($) {
    $.fn.collapsableTree = function (options) {

        function CookieKey(id) {
            return config.cookiePrefix + "CollapsableStatus[" + id + "]";
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


                $.cookie(key, collapsed);
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
                if (el.hasClass(config.clsAutoClose)) {
                    collapsed = "true";
                }
                if (collapsed == "true") {
                    var $handle = $li.find(config.selHandle);
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
                    var $handle = $li.find(config.selHandle);
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

        var config = {
            selHandle: ".handle",
            clsCollapsed: "collapsed",
            selLi: "li",
            selUl: "ul",
            clsDisabledHandle: "disabled",
            clsAutoOpen: "autoOpen",
            clsKeepOpen: "keepOpen",
            clsKeepClose: "keepClose",
            clsAutoClose: "autoClose",
            onToggle: function (el, handle) { },
            onExpand: function (el, handle) { },
            onCollapse: function (el, handle) { },
            onSave: function (el) { },
            onLoad: function (el) { },
            preserve: false,
            debug: false,
            cookiePrefix: "",
            autoLoad: true,
            deleteCookies: false,
            selExtraCollapse: "",
            selfHandle: false,
            handleClickReturn: false
        };

        if (options) $.extend(config, options);

        if (config.selfHandle == true) {
            config.selHandle = ":parent";
        };

        return this.each(function () {
            Debug("Start");
            var $self = $(this);

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
                $handle = $li.find(config.selHandle);
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

            $self.find(config.selLi).filter("." + config.clsKeepClose).add("." + config.clsKeepOpen).each(function () {
                $li = $(this);
                $handle = $li.find(config.selHandle);
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
                }
                return config.handleClickReturn;
            });

            if (config.deleteCookies) {
                DeleteCookies($self);
            }

            if (config.autoLoad) {
                //Debug("AutoLoading...");
                $self.find(config.selLi).each(function () {
                    Load($(this));
                });
                //Debug("AutoLoading... finished");
            }
        });
    }
})(jQuery);