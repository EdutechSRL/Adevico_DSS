(function ($) {
    $.fn.dropdownList = function (options) {

        var config = {
            debug: false,
            widthOffset: 30,
            changeWidth: true
        };

        if (options) {
            $.extend(config, options);
        }

        function Debug(msg) {
            if (config.debug) {
                //console.log(msg);
            }
        }

        function getUnvisibleDimensions(obj) {
            if ($(obj).length == 0) {
                return false;
            }

            var clone = obj.clone();
            clone.css({
                visibility: 'hidden',
                width: '',
                height: '',
                maxWidth: '',
                maxHeight: ''
            });
            $('body').append(clone);
            var width = clone.outerWidth(),
                height = clone.outerHeight();
            clone.remove();
            return {w: width, h: height};
        }

        function getRealDimensions(obj, outer) {
            if ($(obj).length == 0) {
                return false;
            }

            var width, height, offsetTop, offsetLeft, offsets;

            var clone = obj.clone();
            clone.show();
            clone.css({
                visibility: 'hidden'
            });
            $('body').append(clone);
            if (outer === true) {
                width = clone.outerWidth();
                height = clone.outerHeight();
            } else {
                width = clone.innerWidth();
                height = clone.innerHeight();
            }

            offsets = clone.offset();
            offsetTop = clone.offsets.top;
            offsetLeft = clone.offsets.left;
            clone.remove();
            return {
                width: width,
                height: height,
                offsetTop: offsetTop,
                offsetLeft: offsetLeft
            };
        }

        return this.each(function () {
            
            var $self = $(this);
            if ($self.is(".enabled")) {
                /*$currentChildren = $self.children();

                 $currentChildren.addClass("ddbutton");
                 if ($currentChildren.filter(".active").size() == 0) {
                 $currentChildren.first().addClass("active");
                 }*/

                /*var $newSelector = $("<span class='selector'><span class='selectoricon'>&nbsp;</span><span class='listwrapper'><span class='arrow'></span></span></span>");

                 if ($self.children(".selector").size() == 0) {
                 $newSelector.appendTo($self);
                 }*/
                $selector = $self.find("span.selector");
                $container = $selector.find("span.listwrapper");

                $container.data("maxWidth", 0);
                /*$self.find(".ddbutton").each(function () {
                 $menuHelper = $("<a></a>");
                 $menuHelper.addClass("ddbutton");
                 $menuHelper.html($(this).html());
                 if ($(this).is(".active")) {
                 $menuHelper.addClass("activeselected");
                 }
                 $icon = $("<span class='icon'>&nbsp;</span>");
                 $icon.prependTo($menuHelper);
                 $menuHelper.data("relative", $(this));
                 $menuHelper.appendTo($container);

                 if (config.changeWidth) {
                 var elementWidth = getUnvisibleDimensions($(this)).w;
                 $menuHelper.data("width", elementWidth);
                 Debug("Width: " + elementWidth);
                 if (elementWidth >= $container.data("maxWidth")) {
                 $container.data("maxWidth", elementWidth);
                 }
                 }
                 });*/
                if (config.changeWidth) {
                    Debug("Max Width: " + $container.data("maxWidth"));
                    Debug("New Width: " + ($container.data("maxWidth") + config.widthOffset));
                    $container.children(".ddbutton").width($container.data("maxWidth") + config.widthOffset);
                }
                $container.find(".ddbutton").first().addClass("first");
                $container.find(".ddbutton").last().addClass("last");
            }


            $self.filter(".enabled").find("span.selector a.ddbutton").click(function () {
                $parent = $self;
                //console.log($self.attr("id"));
                //$parent.children(".ddbutton.active").removeClass("active");
                $parent.find(".ddselector").html($(this).data("text"));
                $parent.find("input[type='hidden']").val($(this).data("id"));
                $parent.find("input[type='hidden']").trigger("change");
                $selector = $(this).parents("span.selector");
                $selector.removeClass("clicked");
                /*$el = $(this).data("relative");
                 $el.addClass("active");*/
                $(this).parents(".listwrapper").find(".ddbutton").removeClass("activeselected");
                $(this).addClass("activeselected");
                return false;
            });
            $self.filter(".enabled").find(".ddselector").click(function () {
                $(this).next(".selector").toggleClass("clicked");
            });
            $self.filter(".enabled").find("span.selector").click(function (evt) {
                evt.stopImmediatePropagation();
                $(this).toggleClass("clicked");
            });

        });
    }
})(jQuery);