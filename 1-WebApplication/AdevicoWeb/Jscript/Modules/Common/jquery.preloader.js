(function($) {
    var methods = {
        init : function( options ) {

            var $self=this;

            var config = {
	              width:32,
	              //height:32,
	              speed:20,
	              frames:8,
	              startFrame:0,
	              offset:0
            };

            if (options) $.extend(config, options);




            return this.each(function () {
	              var idx = $(this).data("index");
	              if(idx==undefined)
	              {
		                idx = config.startFrame;
		                $(this).data("index",idx);
	              }
	              var $this = $(this);

	              var FPS = config.speed;

								var SECONDS_BETWEEN_FRAMES = 1 / FPS;
	              setInterval(function(){
		                var idx = $this.data("index")+1;
			              if(idx>=config.frames)
			              {
				              idx = 0;
			              }
			              $this.css("backgroundPosition", (-idx * config.width)+config.offset);
			              $this.data("index",idx);
	              }
		              , SECONDS_BETWEEN_FRAMES*1000);
            });
        }
    };

    $.fn.preloader = function(method) {

        if ( methods[method] ) {
            return methods[ method ].apply( this, Array.prototype.slice.call( arguments, 1 ));
        } else if ( typeof method === 'object' || ! method ) {
            return methods.init.apply( this, arguments );
        } else {
            $.error( 'Method ' +  method + ' does not exist on jQuery.preloader' );
        }


    }
})(jQuery);