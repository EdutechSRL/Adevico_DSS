(function ($) {
    var methods = {
        init: function (options) {

            var config = {
                selCheckbox: "input[type=checkbox]",
                onToggle: function (el, handle) { },
                onBlock: function (el, handle) { },
                onUnblock: function (el, handle) { },
                blockedClass: "blocked",
                debug: false
            };

            if (options) $.extend(config, options);

            return this.each(function () {

                var $self = $(this);

                $self.data("config", config);

                var $fieldset = $self;
                var $legend = $fieldset.children("legend");
                var $checkbox = $legend.find(config.selCheckbox);

                var checked = $checkbox.is(":checked");
                inputcontrol($self, checked);

                $checkbox.click(function () {
                    checked = $(this).is(":checked");
                    inputcontrol($self, checked);
                });
                $checkbox.change(function () {
                    checked = $(this).is(":checked");
                    inputcontrol($self, checked);
                });
            });
        },
        unblock: function (selector) {
            checked = true;
            $checkbox.attr("checked", "checked");
            inputcontrol($(this), checked);
        },
        block: function (selector) {
            checked = false;
            $checkbox.removeAttr("checked", "checked");
            inputcontrol($(this), checked);
        },
        toggle: function (selector) {
            checked = !checked;
            if (!checked) {
                $checkbox.removeAttr("checked", "checked");
            } else {
                $checkbox.attr("checked", "checked");
            }
            inputcontrol($(this), checked);
        }
    };

    function inputcontrol($self, checked) {
        var config = $self.data("config");
        var $fieldset = $self;
        var $legend = $fieldset.children("legend");
        var $checkbox = $legend.find(config.selCheckbox);

        if (!checked) {
            $fieldset.addClass(config.blockedClass);
            $fieldset.find("input").not("legend input").attr("disabled", "disabled");
            $fieldset.find("select").attr("disabled", "disabled");
            //$legend.siblings("input").attr("disabled","disabled");
        } else {
            $fieldset.removeClass(config.blockedClass);
            $fieldset.find("input").not("legend input").removeAttr("disabled", "disabled");
            $fieldset.find("select").removeAttr("disabled", "disabled");
            //$legend.siblings("input").removeAttr("disabled");
        }
    }


    $.fn.blockableFieldset = function (method) {

        if (methods[method]) {
            return methods[method].apply(this, Array.prototype.slice.call(arguments, 1));
        } else if (typeof method === 'object' || !method) {
            return methods.init.apply(this, arguments);
        } else {
            $.error('Method ' + method + ' does not exist on jQuery.blockableFieldset');
        }


    }
})(jQuery);