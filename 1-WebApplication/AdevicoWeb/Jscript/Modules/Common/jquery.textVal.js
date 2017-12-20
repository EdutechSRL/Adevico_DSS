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
                textSelector: "self",
                skipWriteMinMax: false,
                charAvailable: "",
                charMax: "",
                applySelective: true
            };


            function elaborate($this, keypress, $parent, keypressed) {
                var max = 0;
                var current = 0;
                var avail = 0;

                if (keypress == true) {
                    max = parseInt($this.attr("maxlength"));
                    current = $this.val().length;

                    avail = max - current;
                    //x= Math.max(0,x);



                    $parent.find(config.charAvailable).html(avail);
                    $parent.find(config.charMax).html(max);



                } else {

                    if (config.textSelector == "self") {
                        max = parseInt($this.attr("maxlength"));
                        current = $this.val().length;

                        avail = max - current;

                        $this.siblings(config.charAvailable).html(avail);
                        $this.siblings(config.charMax).html(max);
                    } else {


                        if ($this.find(config.textSelector).size() > 0) {


                            max = parseInt($this.find(config.textSelector).attr("maxlength"));
                            current = $this.find(config.textSelector).val().length;

                            avail = max - current;

                            $this.find(config.charAvailable).html(avail);
                            $this.find(config.charMax).html(max);
                        }
                    }
                }

                return avail;
            }

            if (options) $.extend(config, options);

            return this.each(function () {
                var $this = $(this);

                if (config.checkOnStart) {
                    var c = elaborate($this);

                    if (c < 0) {
                        if (config.textSelector == "self") {
                            $this.addClass(config.errorClass);
                        } else if (config.errorSelector == "") {
                            $this.find(config.textSelector).addClass(config.errorClass);
                        } else {

                            $this.find(config.errorSelector).addClass(config.errorClass);
                        }
                    } else {
                        if (config.textSelector == "self") {
                            $this.removeClass(config.errorClass);
                        } else if (config.errorSelector == "") {
                            $this.find(config.textSelector).removeClass(config.errorClass);
                        } else {
                            $this.find(config.errorSelector).removeClass(config.errorClass);

                        }
                    }
                }

                if (config.textSelector == "self") {
                    $this.keydown(function () {
                        var c = elaborate($this, true);

                        if (c < 0) {
                            if (config.textSelector == "self") {
                                $this.addClass(config.errorClass);
                            } else if (config.errorSelector == "") {
                                $this.find(config.textSelector).addClass(config.errorClass);
                            } else {

                                $this.find(config.errorSelector).addClass(config.errorClass);
                            }
                        } else {
                            if (config.textSelector == "self") {
                                $this.removeClass(config.errorClass);
                            } else if (config.errorSelector == "") {
                                $this.find(config.textSelector).removeClass(config.errorClass);
                            } else {
                                $this.find(config.errorSelector).removeClass(config.errorClass);

                            }
                        }
                    });
                } else {


                    $this.find(config.textSelector).keyup(function (event) {
                        var c = elaborate($(this), true, $this, event.which);
                        if (c < 0) {
                            if (config.textSelector == "self") {
                                $this.addClass(config.errorClass);
                            } else if (config.errorSelector == "") {
                                $this.find(config.textSelector).addClass(config.errorClass);
                            } else {

                                $this.find(config.errorSelector).addClass(config.errorClass);
                            }
                        } else {
                            if (config.textSelector == "self") {
                                $this.removeClass(config.errorClass);
                            } else if (config.errorSelector == "") {
                                $this.find(config.textSelector).removeClass(config.errorClass);
                            } else {
                                $this.find(config.errorSelector).removeClass(config.errorClass);

                            }
                        }
                    });


                }

            });
        }
    };

    $.fn.textVal = function (method) {
        if (methods[method]) {
            return methods[method].apply(this, Array.prototype.slice.call(arguments, 1));
        } else if (typeof method === 'object' || !method) {
            return methods.init.apply(this, arguments);
        } else {
            $.error('Method ' + method + ' does not exist on jQuery.simpleEqualize');
        }
    }
})(jQuery);