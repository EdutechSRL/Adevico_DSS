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
            $prbar = $(this);
            newSize = parseInt($prbar.css(config.variableSize));
            calcSize = 0;
            $prbar.children(config.internalElements).each(function () {
                value = $(this).attr(config.valueAttrib);
                if (value != "") {
                    var match = value.match(config.regex); //Extract numeric info
                    value = parseInt(match[0]);
                    calcSize += value * newSize / 100;
                    $(this).css(config.variableSize, value * newSize / 100);
                }
            });
            if (newSize - calcSize != 0) {
                $last = $prbar.children(config.internalElements).last();
                var change = parseInt($last.css(config.variableSize)) + newSize - calcSize;
                $last.css(config.variableSize, change);
            }
        });
    }
})(jQuery);
