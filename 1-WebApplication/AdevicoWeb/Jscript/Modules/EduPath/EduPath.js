$(function () {
    $(".collapsable .expander").click(function () {
        $(this).parents(".collapsable").first().toggleClass("collapsed");
    });

    $(".defaultservicecontainer.collapsable .expander").click(function () {
        $(this).toggleClass("collapsed");
    });

    if (jQuery().chosen) {
        $("select.chzn-select").chosen();

        $(".selectall").click(function () {
            var dis = $(this).parents(".choseselect").find(".chzn-select").attr("disabled");
            if (dis != "disabled") {
                $(this).parents(".choseselect").find(".chzn-select option:not(:disabled)").attr("selected", true);
                $(this).parents(".choseselect").find(".chzn-select").trigger("liszt:updated");
                $(this).parents(".choseselect").find(".chzn-select").trigger("change");
            }
        });
        $(".selectnone").click(function () {
            var dis = $(this).parents(".choseselect").find(".chzn-select").attr("disabled");
            if (dis != "disabled") {
                //$(this).parents(".choseselect").find(".chzn-select option").attr("selected", false);
                $(this).parents(".choseselect").find(".chzn-select").val("");
                $(this).parents(".choseselect").find(".chzn-select").trigger("liszt:updated");
                $(this).parents(".choseselect").find(".chzn-select").trigger("change");
            }
        });
    };
});