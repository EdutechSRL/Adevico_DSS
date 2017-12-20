$(function () {
    $(".collapsablerows .row .handle").click(function () {
        $(this).parents(".row").first().toggleClass("collapsed").toggleClass("expanded");
        $(this).toggleClass("collapsed").toggleClass("expanded");
        var collapsed = $(this).parents(".row").first().is(".collapsed");
        var id = $(this).parents(".row").first().attr("id");
        //cookie
    });
    $(".collapsablerows .collapseall").click(function () {
        $(this).parents(".collapsablerows").first().find(".row").each(function () {
            if ($(this).find(".handle").size() > 0) {
                $(this).find(".handle").addClass("collapsed").removeClass("expanded");
                $(this).addClass("collapsed").removeClass("expanded");
                var id = $(this).attr("id");
                //cookie
            }
        });
    });
    $(".collapsablerows .expandall").click(function () {
        //$(this).parents(".collapsablerows").first().find(".row").removeClass("collapsed");
        $(this).parents(".collapsablerows").first().find(".row").each(function () {
            if ($(this).find(".handle").size() > 0) {

                $(this).find(".handle").removeClass("collapsed").addClass("expanded");
                $(this).removeClass("collapsed").addClass("expanded");
                var id = $(this).attr("id");
                //cookie
            }
        });
    });
});