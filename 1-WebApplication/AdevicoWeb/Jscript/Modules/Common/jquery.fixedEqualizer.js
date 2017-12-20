(function($) {
    var methods = {
        init : function( options ) {
            /*Debug("Init");*/

            var config = {
	            children:".child"
            };

            if (options) $.extend(config, options);

            this.each(function () {
                var $this = $(this);

	              var $sameHeightChildren = $this.find(config.children);
				        var maxHeight = 0;
								console.log($sameHeightChildren.size());
				        $sameHeightChildren.each(function() {
					        //if($(this).height()>maxHeight){maxHeight = $(this).height()}
				             maxHeight = Math.max(maxHeight, $(this).outerHeight());
				        });
								console.log(maxHeight);
				        $sameHeightChildren.css({ height: maxHeight + 'px' });
            });
        }
    };

    $.fn.fixedEqualizer = function(method) {
        if ( methods[method] ) {
            return methods[ method ].apply( this, Array.prototype.slice.call( arguments, 1 ));
        } else if ( typeof method === 'object' || ! method ) {
            return methods.init.apply( this, arguments );
        } else {
            $.error( 'Method ' +  method + ' does not exist on jQuery.fixedEqualizer' );
        }
    }
})(jQuery);