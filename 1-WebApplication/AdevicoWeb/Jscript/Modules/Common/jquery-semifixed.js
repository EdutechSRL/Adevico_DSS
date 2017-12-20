(function($) {
    var methods = {
        init : function( options ) {

            var $self=this;

            var config = {
                onFixed:function(el){},
                onRelative:function(el){},
                cssHasFloating:"hasFloating",
                cssFloatedElement:"floatingHeader",
                selFixedElement:".persist-header",
                checkLength:true,
                calcFixedElementHeight:true,
                debug:false
            };

            function getHeight() {
                var myWidth = 0, myHeight = 0;
                if( typeof( window.innerWidth ) == 'number' ) {
                    //Non-IE
                    myWidth = window.innerWidth;
                    myHeight = window.innerHeight;
                } else if( document.documentElement && ( document.documentElement.clientWidth || document.documentElement.clientHeight ) ) {
                    //IE 6+ in 'standards compliant mode'
                    myWidth = document.documentElement.clientWidth;
                    myHeight = document.documentElement.clientHeight;
                } else if( document.body && ( document.body.clientWidth || document.body.clientHeight ) ) {
                    //IE 4 compatible
                    myWidth = document.body.clientWidth;
                    myHeight = document.body.clientHeight;
                }

                return myHeight;
            }

            var waitForFinalEvent = (function () {
                var timers = {};
                return function (callback, ms, uniqueId) {
                    if (!uniqueId) {
                        uniqueId = "Don't call this twice without a uniqueId";
                    }
                    if (timers[uniqueId]) {
                        clearTimeout (timers[uniqueId]);
                    }
                    timers[uniqueId] = setTimeout(callback, ms);
                };
            })();

            function UpdateLength(){
                waitForFinalEvent(function(){
                    $(".persist-header").each(function(){
                        var height = $(this).height();
                        var windowHeight =getHeight();
                        if(height>windowHeight){
                            $(this).addClass("tooLong");
                        }else
                        {
                            $(this).removeClass("tooLong");
                        }
                    });
                }, 150, "resize");
            }

            function UpdateTableHeaders() {
                $self.each(function() {

                    var el             = $(this),
                        offset         = el.offset(),
                        scrollTop      = $(window).scrollTop(),
                        floatingHeader = $("."+config.cssFloatedElement , this)

                    if ((scrollTop> offset.top) && (scrollTop < offset.top + el.height())){
                        $(config.selFixedElement).addClass(config.cssFloatedElement);
                        if(!el.hasClass(config.cssHasFloating)){
                            config.onFixed(el);
                        }
                        el.addClass(config.cssHasFloating);
                        if(config.calcFixedElementHeight==true)
                        {
                            var height = $(config.selFixedElement).height();
                            el.css("padding-top",height+5);
                        }

                    } else {
                        $(config.selFixedElement).removeClass(config.cssFloatedElement);
                        if(el.hasClass(config.cssHasFloating)){
                            config.onRelative(el);
                        }
                        el.removeClass(config.cssHasFloating);
                        if(config.calcFixedElementHeight==true)
                        {
                            var height = $(config.selFixedElement).height();
                            el.removeAttr('style');
                        }

                    };
                });
            }

            if (options) $.extend(config, options);

            if(config.checkLength){
                $(window)
                    .scroll(UpdateTableHeaders)
                    .trigger("scroll")
                    .resize(UpdateLength)
                    .trigger("resize");
            }else
            {
                $(window)
                    .scroll(UpdateTableHeaders)
                    .trigger("scroll");
            }

            return this.each(function () {

            });
        }
    };

    $.fn.semiFixed = function(method) {

        if ( methods[method] ) {
            return methods[ method ].apply( this, Array.prototype.slice.call( arguments, 1 ));
        } else if ( typeof method === 'object' || ! method ) {
            return methods.init.apply( this, arguments );
        } else {
            $.error( 'Method ' +  method + ' does not exist on jQuery.semiFixed' );
        }


    }
})(jQuery);