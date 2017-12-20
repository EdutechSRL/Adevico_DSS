(function ($) {

    function inputcontrol(checked, config, $this) {
        var $input = $this.nextAll("input").first();  //.siblings("input").first("");
        if (!checked) {
            $input.addClass(config.disabledClass);
            $input.attr("disabled", "disabled");
            config.onDisable();
        } else {
            $input.removeClass(config.disabledClass);
            $input.removeAttr("disabled");
            config.onEnable();
            if (config.focusOnEnable) {
                $input.focus();
            }
        }
    }

    var methods = {
        init: function (options) {
            /*Debug("Init");*/

            var config = {
                onToggle: function () { },
                onDisable: function () { },
                onEnable: function () { },
                disabledClass: "disabled",
                focusOnEnable: true
            };

            if (options) $.extend(config, options);



            return this.each(function () {
                var $this = $(this);

                var checked = $this.is(":checked");

                inputcontrol(checked, config, $this);

                $this.change(function () {
                    checked = $this.is(":checked");
                    inputcontrol(checked, config, $this);
                    config.onToggle();
                });

            });
        }
    };

    $.fn.inputActivator = function (method) {
        if (methods[method]) {
            return methods[method].apply(this, Array.prototype.slice.call(arguments, 1));
        } else if (typeof method === 'object' || !method) {
            return methods.init.apply(this, arguments);
        } else {
            $.error('Method ' + method + ' does not exist on jQuery.simpleEqualize');
        }
    }
})(jQuery);