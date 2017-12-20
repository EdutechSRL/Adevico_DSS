(function ($) {
    $.fn.anchor = function (options) {

        var config = {
            debug : false,
            baseColor : "#ffffff",
            highlightedColor:"#ffffaa",
            parameterA:1500,
            parameterB:3000,
            parameterC:1000,
            autoBaseColor:true,
            sel:""
        };

        if (options) { $.extend(config, options); }

        function Debug(msg) {
            if (config.debug) {
                console.log(msg);
            }
        }

        return this.each(function () {
            if (document.location.hash) {
                var elemSel=document.location.hash;
                if(config.sel!=""){
                    elemSel = $(elemSel).find(config.sel).first();
                }
                highlight(elemSel);
            }

            function highlight(elemSel){
                var elem = $(elemSel);
                if(config.autoBaseColor){
                    config.baseColor = elem.css("background-color");
                }
                elem.css("backgroundColor", config.baseColor); // hack for Safari
                elem.animate({ backgroundColor: config.highlightedColor }, config.parameterA);
                setTimeout(function(){$(elemSel).animate({ backgroundColor: config.baseColor }, config.parameterB)},config.parameterC);
            }

            $(this).find("a[href*='#']").live("click",function(){
                var elemSel = '#' + $(this).attr('href').split('#')[1];
                if(config.sel!=""){
                    elemSel = $(elemSel).find(config.sel).first();
                }
                highlight(elemSel);
            });
        });
    }
})(jQuery);