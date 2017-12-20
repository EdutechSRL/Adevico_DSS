/**
 * Created by roberto.maschio on 26/11/13.
 */
$(function () {
    'use strict';
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

    $(".collapsable .expander").click(function () {
        $(this).parents(".collapsable").first().toggleClass("collapsed");
    });

    $(".defaultservicecontainer.collapsable .expander").click(function () {
        $(this).toggleClass("collapsed");
    });

    $(".groupedselector .selectoricon, .groupedselector .selectorlabel").click(function () {
        var $group = $(this).parents(".groupedselector").first();

        $(".groupedselector").not($group).removeClass("clicked");
        $group.toggleClass("clicked");

    });

    $(".groupedselector .selectoritem").click(function () {
        var $group = $(this).parents(".groupedselector").first();
        $group.removeClass("clicked");

        $group.find(".selectoritem").removeClass("active");
        $(this).addClass("active");

        $group.find(".selectorgroup .selectorlabel").html($(this).find(".selectorlabel").html());

    });

    $(".groupedselector").mouseout(function () {
        //$(this).removeClass("clicked");
    });


    /*var group=$("ol.sortabletree").sortable({
     handle: ".text",
     exclude:".default",
     onDrop: function (item, container, _super) {
     var ser="";
     group.find("li.sortableitem").each(function(){
     var $parent =$(this).parents("li.sortableitem").first();
     var parentId=0;
     if ($parent.size()>0)
     {
     parentId = $parent.attr("id")
     }
     ser= ser + $(this).attr("id")+":"+parentId+";";
     });
     $('.serialize_output').val(ser);
     _super(item, container);
     }
     });*/

    $("ol.sortabletree").sortable({
        handle: ".text",
        items: "li",
        cancel: "li.default",
        tolerance: 'pointer',
        placeholder: 'ui-state-highlightHelper',
        forcePlaceholderSize: true,
        forceHelperSize: true,
        dragOnEmpty: true,
        refreshPositions: true,
        axis: 'y',
        helper: "original",
        start: function (event, ui) {
            $(ui.item).addClass("dragging");
        },
        stop: function (event, ui) {
            $(ui.item).removeClass("dragging");
            $(".serialize_output").val($(ui.item).parents("ol.sortabletree").first().sortable("serialize"));
        }
    });


    var cookiename = "glossary-new";

    $(".glossarywrapper.view:not(.viewterm)").find("dl.terms dt.term").each(function () {
        var $term = $(this);

        var $handle = $term.find(".handle");
        var $definition = $term.next("dd.definition");
        var id = $term.attr("id");
        var expanded = $.cookie(cookiename + "-term-" + id);
        if (expanded == "true") {
            $handle.addClass("expanded").removeClass("collapsed");
            $term.addClass("expanded").removeClass("collapsed");
            $definition.addClass("expanded").removeClass("collapsed");
        } else {
            $handle.removeClass("expanded").addClass("collapsed");
            $term.removeClass("expanded").addClass("collapsed");
            $definition.removeClass("expanded").addClass("collapsed");
        }

    });

    $(".glossarywrapper.view:not(.viewterm)").find("dl.terms dt.term .handle").click(function () {
        var $handle = $(this);
        var $term = $handle.parents("dt.term");

        var $definition = $term.next("dd.definition");
        //var $terms = $term.parents("dl.terms");

        $handle.toggleClass("expanded").toggleClass("collapsed");
        $term.toggleClass("expanded").toggleClass("collapsed");
        //$definition.toggleClass("expanded").toggleClass("collapsed");
        $definition.slideToggle(300, "swing").toggleClass("expanded").toggleClass("collapsed");

        var id = $term.attr("id");
        $.cookie(cookiename + "-term-" + id, $handle.is(".expanded"));
    });
    $(".glossarywrapper.view:not(.viewterm)").find("dl.terms dt.term .termtitle").click(function () {

        var $term = $(this).parents("dt.term");
        var $handle = $term.find(".handle");
        var $definition = $term.next("dd.definition");
        //var $terms = $term.parents("dl.terms");

        $handle.toggleClass("expanded").toggleClass("collapsed");
        $term.toggleClass("expanded").toggleClass("collapsed");
        //$definition.toggleClass("expanded").toggleClass("collapsed");     
        $definition.slideToggle(300, "swing").toggleClass("expanded").toggleClass("collapsed");

        var id = $term.attr("id");
        $.cookie(cookiename + "-term-" + id, $handle.is(".expanded"));
    });
    if (jQuery().dropdownButtonList) {
        $(".ddbuttonlist.enabled").dropdownButtonList();
    }


    $(".fieldrow.defaultview input:radio").each(function () {

        var $input = $(this);
        var $group = $input.parents(".inputgroup").first();
        var $row = $group.parents(".fieldrow").first();

        var checked = $input.is(":checked");

        //$row.find(".inputgroup").removeClass("active");

        if (checked) {

            $group.addClass("active");
        }
    });

    $(".fieldrow.defaultview input:radio").click(function () {
        var $input = $(this);
        var $group = $input.parents(".inputgroup").first();
        var $row = $group.parents(".fieldrow").first();

        var checked = $input.is(":checked");

        $row.find(".inputgroup").removeClass("active");

        if (checked) {
            $group.addClass("active");
        }
    });

    $(".dialog").dialog({
        autoOpen: false,
        width: 800,
        height: 600,
        appendTo: "form"
    });

    $(".openuploadfile").click(function () {
        $(".dialog.fileupload").dialog("open");
        return false;
    });

    $(".itemcontent .commands .selectall").click(function () {
        $(this).parents(".itemcontent").first().find(".radiobuttonlist input[type='checkbox']").prop("checked", true);
    });
    $(".itemcontent .commands .selectnone").click(function () {
        $(this).parents(".itemcontent").first().find(".radiobuttonlist input[type='checkbox']").prop("checked", false);
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
});