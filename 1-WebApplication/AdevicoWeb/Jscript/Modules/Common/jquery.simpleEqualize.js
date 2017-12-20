(function ($) {
    var methods = {
        init: function (options) {
            /*Debug("Init");*/

            var config = {
                onBeforeStart: function () { },
                copyThis: ".copyThis",
                resizeThis: ".resizeThis",
                resetHeight: "auto"
            };

            if (options) $.extend(config, options);

            config.onBeforeStart();

            return this.each(function () {
                var $this = $(this);

                var $copyThis = $this.find(config.copyThis).first();
                var $resizeThis = $this.find(config.resizeThis).first();

                $resizeThis.css("height", config.resetHeight);

                if ($resizeThis.height() < $copyThis.height()) {
                    $resizeThis.height($copyThis.height());
                }
            });
        }
    };

    $.fn.simpleEqualize = function (method) {
        if (methods[method]) {
            return methods[method].apply(this, Array.prototype.slice.call(arguments, 1));
        } else if (typeof method === 'object' || !method) {
            return methods.init.apply(this, arguments);
        } else {
            $.error('Method ' + method + ' does not exist on jQuery.simpleEqualize');
        }
    }
})(jQuery);