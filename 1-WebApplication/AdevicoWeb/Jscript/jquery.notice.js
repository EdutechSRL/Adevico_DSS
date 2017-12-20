(function (jQuery) {
    jQuery.extend({
        noticeAdd: function (options, callback) {
            var defaults = {
                inEffect: { opacity: 'show' }, // in effect
                inEffectDuration: 600, 			// in effect duration in miliseconds
                stayTime: 3000, 			// time in miliseconds before the item has to disappear
                text: '', 				// content of the item
                stay: false, 			// should the notice item stay or not?
                type: 'notice', 			// could also be error, succes
                keepOpen: false
            }

            // declare varaibles
            var options, noticeWrapAll, noticeItemOuter, noticeItemInner, noticeItemClose;

            options = jQuery.extend({}, defaults, options);
            noticeWrapAll = (!jQuery('.notice-wrap').length) ? jQuery('<div></div>').addClass('notice-wrap').appendTo('body') : jQuery('.notice-wrap');
            noticeItemOuter = jQuery('<div></div>').addClass('notice-item-wrapper');
            noticeItemInner = jQuery('<div></div>').hide().addClass('notice-item ' + options.type).appendTo(noticeWrapAll).html('<p>' + options.text + '</p>').animate(options.inEffect, options.inEffectDuration).wrap(noticeItemOuter);
            if (!options.keepOpen) {
                noticeItemClose = jQuery('<div></div>').addClass('notice-item-close').prependTo(noticeItemInner).html('x').click(function () { jQuery.noticeRemove(noticeItemInner, options.keepOpen, callback) });
                noticeItemHide = jQuery('<div></div>').addClass('notice-item-hide').prependTo(noticeItemInner).html('v').click(function () { jQuery.noticeRemove(noticeItemInner, false, null) });
            }
            else {
                noticeItemHide = jQuery('<div></div>').addClass('notice-item-hide').prependTo(noticeItemInner).html('v').click(function () { jQuery.noticeRemove(noticeItemInner, false, null) });
            }


            // hmmmz, zucht
            if (navigator.userAgent.match(/MSIE 6/i)) {
                noticeWrapAll.css({ top: document.documentElement.scrollTop });
            }

            if (!options.stay) {
                setTimeout(function () {
                    jQuery.noticeRemove(noticeItemInner, false, null);
                },
				options.stayTime);
            }
        },

        noticeRemove: function (obj, keepOpen, callback) {
            if (callback) {
                var ret = callback();
            }
            if (!keepOpen) {
                obj.animate({ opacity: '0' }, 600, function () {
                    obj.parent().animate({ height: '0px' }, 300, function () {
                        obj.parent().remove();
                    });
                });
            }

        }
    });
})(jQuery);