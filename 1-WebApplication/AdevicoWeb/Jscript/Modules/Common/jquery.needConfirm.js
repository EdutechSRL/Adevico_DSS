(function ($) {
    var methods = {
        init: function (options) {

            var config = {
                /*Options*/
                msg: "Are you sure?",
                liveBinding: false,  /*false: .click() , true: .live("click")*/
                msgFunctionType: "static", /*static, title, attribute, parameter, custom*/
                /*Override functions*/
                msgFunction: function (item) { return msg(item); }, /*Override function for "custom"*/
                /*Events*/
                onConfirm: function (item) { },
                onNotConfirm: function (item) { },
                addConfirmClass: false,
                confirmClass: "confirmIt",
                confirmClassSelector: "self"
            };

            if (options) $.extend(config, options);

            function msg(item) {
                switch (config.msgFunctionType) {
                    case "static":
                        return config.msg;
                        break;
                    case "title":
                        return $(item).attr("title");
                        break;
                    case "attribute":
                        return $(item).data("msg");
                    case "parameter":
                        return config.msg.replace("{msg}", $(item).data("msg"));
                    case "custom":
                        break;
                }
            }

            function SetClass(item) {
                if (config.addConfirmClass) {
                    var el = item;
                    if (config.confirmClassSelector != "self") {
                        el = $(config.confirmClassSelector);
                    }
                    $(el).addClass(config.confirmClass);
                }
            }
            function RemoveClass(item) {
                if (config.addConfirmClass) {
                    var el = item;
                    if (config.confirmClassSelector != "self") {
                        el = $(config.confirmClassSelector);
                    }
                    $(el).removeClass(config.confirmClass);
                }
            }

            function confirmation(item) {
                SetClass(item);
                var result = confirm(config.msgFunction(item));
                RemoveClass(item);
                if (result) {
                    config.onConfirm(item);
                } else {
                    config.onNotConfirm(item);
                }

                return result;
            }

            return this.each(function () {
                var $this = $(this);

                if (config.liveBinding == true) {
                    $this.live("click", function (event) {
                        event.stopPropagation();
                        return confirmation($this);
                    });
                } else {
                    $this.click(function (event) {
                        event.stopPropagation();
                        return confirmation($this);
                    });
                }
            });
        }
    };

    $.fn.needConfirm = function (method) {
        if (methods[method]) {
            return methods[method].apply(this, Array.prototype.slice.call(arguments, 1));
        } else if (typeof method === 'object' || !method) {
            return methods.init.apply(this, arguments);
        } else {
            $.error('Method ' + method + ' does not exist on jQuery.Confirm');
        }
    }
})(jQuery);