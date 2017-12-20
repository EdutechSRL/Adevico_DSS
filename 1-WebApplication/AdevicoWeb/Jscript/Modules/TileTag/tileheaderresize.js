$(function () {
    $.fn.hasOverflow = function () {
        var $this = $(this);
        return $this[0].scrollHeight > $this.outerHeight() ||
                $this[0].scrollWidth > $this.outerWidth();
    };

    $(".tileheader").each(function () {
        $(this).removeClass("hasoverflow");
        $(this).find(".overflowhandle").remove();
        if ($(this).hasOverflow()) {
            $(this).addClass("hasoverflow");
            $(this).append("<span class='overflowhandle'>...</span>");
        }
    });

    $(".overflowhandle").mouseover(function () {
        $(this).parents(".tileheader").addClass("hovered");
    }).mouseout(function () {
        $(this).parents(".tileheader").removeClass("hovered");
    });
});