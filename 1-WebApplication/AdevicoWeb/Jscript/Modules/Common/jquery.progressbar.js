(function ($) {
    $.fn.myProgressBar = function (options) {

        var config = {
            'internalElements': 'span',
            'variableSize': 'width',
            'valueAttrib': 'title',
            'regex': /\d+/g
        };

        if (options) $.extend(config, options);

        return this.each(function () {
            var $prbar = $(this);
            var newSize = 0;
            //console.log($prbar.css(config.variableSize));
            if ($prbar.css(config.variableSize).indexOf("%") > 0) {
                if (config.variableSize == "width") {
                    newSize = $prbar.width();
//                    console.log($prbar.css(config.variableSize));
//                    console.log($prbar.parent().width());
//                    newSize = parseFloat($prbar.parents().first().css(config.variableSize)) * parseFloat($prbar.css(config.variableSize)) / 100;
                    //console.log(newSize);
                } else {
                    newSize = $prbar.height();
                }
            } else {
                newSize = parseInt($prbar.css(config.variableSize));
            }

            var calcSize = 0;
            $prbar.children(config.internalElements).each(function () {
                var value = $(this).attr(config.valueAttrib);
                if (value != "") {
                    var match = value.match(config.regex); //Extract numeric info

                    if (!match || !match[0]) {
                        value = 0;
                    } else {
                        value = parseInt(match[0]);
                    }
                    //value = parseInt(match[0]);
                    calcSize += value * newSize / 100;
                    $(this).css(config.variableSize, value * newSize / 100-0.3);
                }
            });

            if (newSize - calcSize != 0) {
                var $last = $prbar.children(config.internalElements).last();
                var change = parseInt($last.css(config.variableSize)) + newSize - calcSize - 0.5;
                $last.css(config.variableSize, change);
            }
        });
    }
})(jQuery);
