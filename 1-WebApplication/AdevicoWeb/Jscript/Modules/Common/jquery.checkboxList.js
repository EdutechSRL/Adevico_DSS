(function ($) {


    var methods = {
        init: function (options) {
            /*Debug("Init");*/

            var config = {
                onError: function (el, list) { },
                onClear: function (el, list) { },
                checkOnStart: true,
                errorClass: "error",
                errorSelector: "",
                listSelector: "self",
                skipWriteMinMax: false,
                prefix: {
                    max: "max",
                    min: "min"
                },
                placeholders: {
                    selector: "",
                    active: false,
                    min: "{min}",
                    max: "{max}"
                },
                applySelective: true,
                error: {
                    min: "",
                    max: ""
                }
            };

            function elaborate($this, $parent) {

                var $cbl = $parent;
                var cssclass = "";

                if (config.listSelector != "self") {
                    cssclass = $cbl.find(config.listSelector).attr("class");
                } else {

                    cssclass = $cbl.attr("class");

                }
                var max = extractMax(cssclass);
                var min = extractMin(cssclass);

                var current;

                current = $cbl.find("input[type='checkbox']:checked").size();



                if (config.placeholders.active == true) {

                    var error = $cbl.find(config.placeholders.selector);
                    error.html(error.html().replace(config.placeholders.min, min).replace(config.placeholders.max, max));
                } else {
                    if (config.skipWriteMinMax != true) {
                        var minEl = $cbl.find(config.error.min);
                        var maxEl = $cbl.find(config.error.max);
                        minEl.html(min);
                        maxEl.html(max);
                    }
                }

                if ((max > -1) ? (current > max || current < min) : current < min) {
                    if (config.listSelector == "self") {
                        $cbl.addClass(config.errorClass);
                    } else if (config.errorSelector == "") {
                        $cbl.find(config.listSelector).addClass(config.errorClass);

                    } else if (config.errorSelector == "self") {
                        $cbl.addClass(config.errorClass);
                    } else {
                        $cbl.find(config.errorSelector).addClass(config.errorClass);

                    }
                    if (config.applySelective == true) {

                        if (current < min) {
                            $cbl.find(config.error.min).addClass(config.errorClass);
                        } else {
                            $cbl.find(config.error.max).addClass(config.errorClass);
                        }
                    }
                    config.onError($this, $parent);
                } else {
                    if (config.listSelector == "self") {
                        $cbl.removeClass(config.errorClass);
                    } else if (config.errorSelector == "") {
                        $cbl.find(config.listSelector).removeClass(config.errorClass);
                    } else if (config.errorSelector == "self") {
                        $cbl.removeClass(config.errorClass);
                    } else {
                        $cbl.find(config.errorSelector).removeClass(config.errorClass);

                    }
                    if (config.applySelective == true) {

                        $cbl.find(config.error.min).removeClass(config.errorClass);

                        $cbl.find(config.error.max).removeClass(config.errorClass);
                    }
                    config.onClear($this, $parent);
                }
            }

            function extract(st, pattern, defaultValue) {
                var patt = new RegExp(pattern, "g");
                var result = patt.exec(st);
                if (result == null || result == "undefined") {
                    return defaultValue;
                }
                return result[1];
            }

            function extractMax(st) {
                return extract(st, config.prefix.max + "-(\\d*)", -1);
            }

            function extractMin(st) {
                return extract(st, config.prefix.min + "-(\\d*)", 0);
            }

            if (options) $.extend(config, options);

            return this.each(function () {
                var $this = $(this);
                if (config.checkOnStart) {

                    $this.find("input[type='checkbox']").each(function () {
                        elaborate($(this), $this);
                    });

                }

                $this.find("input[type='checkbox']").change(function () {
                    elaborate($(this), $this);
                });


            });
        }
    };

    $.fn.checkboxList = function (method) {
        if (methods[method]) {
            return methods[method].apply(this, Array.prototype.slice.call(arguments, 1));
        } else if (typeof method === 'object' || !method) {
            return methods.init.apply(this, arguments);
        } else {
            $.error('Method ' + method + ' does not exist on jQuery.simpleEqualize');
        }
    }
})(jQuery);