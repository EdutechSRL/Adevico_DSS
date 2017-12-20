/**
 * Created by roberto.maschio on 26/11/13.
 */


$(function () {

    $(".chzn-select").chosen();

    if ($.browser.msie && ($.browser.version === "7.0")) {

        var j = 500;

        $(".chzn-select").each(function () {

            $(this).parent().css({ 'position': 'relative', 'z-index': j });
            j = j - 1;

        });

    }

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


    $(".dialog.dlgtagedit").dialog(
    {
        autoOpen: false,
        width: 500

    });

    $(".dialog.dlgtileedit").dialog(
        {
            autoOpen: false,
            width: 500

        });

    $(".tabs").tabs({

    });

    $("table.tagslist tr.tag td.actions .icon.edit").click(function () {

        $(".dialog.dlgtagedit").dialog("open");
    })

    $("table.tileslist tr.tile td.actions .icon.edit").click(function () {

        $(".dialog.dlgtileedit").dialog("open");
    })

    $(".gallery:not(.onlyone) .icon, .gallery:not(.onlyone) .img").click(function () {
        var type = $(this).data("type");
        $(this).parents(".gallery").first().find(".active").removeClass("active");
        $(".gallery").find(".active").removeClass("active");
        $(this).addClass("active");
        $(".galleryvalue").val("");
        $(this).parents(".gallerywrapper").find(".galleryvalue").val(type);
    });

    /*$(".gallery .img").click(function(){
        var type = $(this).data("type");
        $(this).parents(".gallery").first().find(".active").removeClass("active");
        $(this).addClass("active");
        $(this).parents(".gallerywrapper").find(".galleryvalue").val(type);
    });*/


    $(".chzn-select").chosen();

    $("input[type='checkbox'][data-disable]").change(function () {
        var id = $(this).data("disable");
        if ($(this).is(":checked")) {
            $("[data-id='" + id + "']").attr("disabled", "disabled");
            if ($("[data-id='" + id + "']").is(".chzn-select")) {
                $("[data-id='" + id + "']").trigger("liszt:updated");
            }
        } else {
            $("[data-id='" + id + "']").removeAttr("disabled");
            if ($("[data-id='" + id + "']").is(".chzn-select")) {
                $("[data-id='" + id + "']").trigger("liszt:updated");
            }
        }
    });

    $("span[data-disable] input[type='checkbox']").change(function () {
        var id = $(this).parents("span[data-disable]").first().data("disable");
        if ($(this).is(":checked")) {
            $("[data-id='" + id + "']").attr("disabled", "disabled");
            if ($("[data-id='" + id + "']").is(".chzn-select")) {
                $("[data-id='" + id + "']").trigger("liszt:updated");
            }
        } else {
            $("[data-id='" + id + "']").removeAttr("disabled");
            if ($("[data-id='" + id + "']").is(".chzn-select")) {
                $("[data-id='" + id + "']").trigger("liszt:updated");
            }
        }
    });

    $(".coveredradio.enabled").each(function () {

        var $parent = $(this);
        var $checks = $parent.find(".wclist input[type='radio']");
        var $buttons = $parent.find(".btnswitch");
        $checks.each(function () {
            var idx = 0;
            if ($(this).parent().is("span.checkwrapper")) {
                idx = $(this).parents("span").first().index() / 2;
            } else {
                idx = $(this).index() / 3;
            }
            if ($(this).is(":checked")) {
                $($buttons.get(idx)).addClass("active");
                if ($(this).data("value") == "on" || $(this).parents("span").first().data("value") == "on") {
                    $parent.addClass("on").removeClass("off");
                } else if ($(this).data("value") == "off" || $(this).parents("span").first().data("value") == "off") {
                    $parent.addClass("off").removeClass("on");
                }
            } else {
                $($buttons.get(idx)).removeClass("active");
            }
        });

        var $disable = $parent.data("disable");
        if ($parent.is(".on")) {
            $parent.parents($disable).addClass("on").removeClass("off").removeClass("collapsed");
            $parent.parents($disable).find(".handle").removeClass("collapsed");
        } else {
            $parent.parents($disable).addClass("off").removeClass("on").addClass("collapsed");
            $parent.parents($disable).find(".handle").addClass("collapsed");
        }
    });

    $(".coveredradio.enabled .btnswitch").click(function () {
        var $parent = $(this).parents(".coveredradio").first();
        $parent.find(".active").removeClass("active");
        var $checks = $parent.find(".wclist input[type='radio']");
        //console.log($checks.size());
        if ($parent.is(":not(.readonly)") && $(this).is(":not(.disabled)")) {
            $(this).toggleClass("active");
            var idx = $(this).index();
            //console.log(idx);
            //console.log($(this).is(".active"));
            $($checks.get(idx)).prop("checked", $(this).is(".active"));

            //console.log($($checks.get(idx)).prop("checked"));
        }
        if ($(this).is(".on")) {
            $parent.addClass("on");
            $parent.removeClass("off");
        } else if ($(this).is(".off")) {
            $parent.addClass("off");
            $parent.removeClass("on");
        }

        var $disable = $parent.data("disable");
        if ($parent.is(".on")) {
            $parent.parents($disable).addClass("on").removeClass("off").removeClass("collapsed");
            $parent.parents($disable).find(".handle").removeClass("collapsed");
        } else {
            $parent.parents($disable).addClass("off").removeClass("on").addClass("collapsed");
            $parent.parents($disable).find(".handle").addClass("collapsed");
        }

        return false;
    });

    $(".coveredradio.enabled input[type='radio']").click(function () {
        var $parent = $(this).parents(".coveredradio").first();
        $parent.find(".active").removeClass("active");

        var $checks = $parent.find(".btnswitch");

        if ($parent.is(":not(.readonly)")) {
            //$(this).toggleClass("active");
            var idx = 0;
            if ($(this).parent().is("span.checkwrapper")) {
                idx = $(this).parents("span").first().index() / 2;
            } else {
                idx = $(this).index() / 3;
            }

            if ($(this).is(":checked")) {
                $($checks.get(idx)).addClass("active");
                if ($(this).data("value") == "on" || $(this).parents("span").first().data("value") == "on") {
                    $parent.addClass("on").removeClass("off");
                } else if ($(this).data("value") == "off" || $(this).parents("span").first().data("value") == "off") {
                    $parent.addClass("off").removeClass("on");
                }
            } else {
                $($checks.get(idx)).removeClass("active");
            }
        }

        var $disable = $parent.data("disable");
        if ($parent.is(".on")) {
            $parent.parents($disable).addClass("on").removeClass("off").removeClass("collapsed");
            $parent.parents($disable).find(".handle").removeClass("collapsed");
        } else {
            $parent.parents($disable).addClass("off").removeClass("on").addClass("collapsed");
            $parent.parents($disable).find(".handle").addClass("collapsed");
        }

    });

    /*
    $(".coveredradio.enabled").each(function () {
        var $parent = $(this);
        var $checks = $parent.find(".wclist input[type='radio']");
        var $buttons = $parent.find(".btnswitch");
        $checks.each(function () {
            var idx = $(this).index() / 3;
            if ($(this).is(":checked")) {
                $($buttons.get(idx)).addClass("active");
                if ($(this).val() == "true") {
                    $parent.addClass("on").removeClass("off");
                } else if ($(this).val() == "false") {
                    $parent.addClass("off").removeClass("on");
                }
            } else {
                $($buttons.get(idx)).removeClass("active");
            }
        });

        var $disable = $parent.data("disable");
        if ($parent.is(".on")) {
            $parent.parents($disable).addClass("on").removeClass("off");
        } else {
            $parent.parents($disable).addClass("off").removeClass("on");
        }
    });

    $(".coveredradio.enabled .btnswitch").click(function () {
        var $parent = $(this).parents(".coveredradio").first();
        $parent.find(".active").removeClass("active");
        var $checks = $parent.find(".wclist input[type='radio']");
        //console.log($checks.size());
        if ($parent.is(":not(.readonly)") && $(this).is(":not(.disabled)")) {
            $(this).toggleClass("active");
            var idx = $(this).index();
            //console.log(idx);
            //console.log($(this).is(".active"));
            $($checks.get(idx)).prop("checked", $(this).is(".active"));

            //console.log($($checks.get(idx)).prop("checked"));
        }
        if ($(this).is(".on")) {
            $parent.addClass("on");
            $parent.removeClass("off");
        } else if ($(this).is(".off")) {
            $parent.addClass("off");
            $parent.removeClass("on");
        }

        var $disable = $parent.data("disable");
        if ($parent.is(".on")) {
            $parent.parents($disable).addClass("on").removeClass("off");
        } else {
            $parent.parents($disable).addClass("off").removeClass("on");
        }

        return false;
    });

    $(".coveredradio.enabled input[type='radio']").click(function () {
        var $parent = $(this).parents(".coveredradio").first();
        $parent.find(".active").removeClass("active");

        var $checks = $parent.find(".btnswitch");

        if ($parent.is(":not(.readonly)")) {
            //$(this).toggleClass("active");
            var idx = $(this).index() / 3;

            if ($(this).is(":checked")) {
                $($checks.get(idx)).addClass("active");
                if ($(this).val()=="true") {
                    $parent.addClass("on").removeClass("off");
                } else if ($(this).val()=="false") {
                    $parent.addClass("off").removeClass("on");
                }
            } else {
                $($checks.get(idx)).removeClass("active");
            }
        }

        var $disable = $parent.data("disable");
        if ($parent.is(".on")) {
            $parent.parents($disable).addClass("on").removeClass("off");
        } else {
            $parent.parents($disable).addClass("off").removeClass("on");
        }

    });
    */
     $(".addbulk").click(function () {
        $(".dialog.dlgtagbulk").dialog("open");
        return false;
    });

    $(".dialog.dlgtagbulk").dialog(
    {
        autoOpen: false,
        width: 800,
        appendTo: "form",
        modal:true,
        open:function()
        {
            $(this).find(".edit textarea").val("");
        }
    });
    $(".collapsable").each(function () {
        var id = $(this).data("id");
        var cookie = $.cookie("collapsed-" + id);
        if (cookie != null) {
            if (cookie == "true") {
                $(this).addClass("collapsed");
            } else {
                $(this).removeClass("collapsed");
            }
        }
    })

    $(".collapsable .expander").click(function () {
        var $collapsable = $(this).parents(".collapsable").first();
        $collapsable.toggleClass("collapsed");

        var id = $collapsable.data("id");
        $.cookie("collapsed-" + id, $collapsable.is(".collapsed"), { expires: 1 });
    });
})