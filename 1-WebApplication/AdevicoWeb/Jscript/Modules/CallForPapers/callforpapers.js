function UpdateTableHeaders() {
    $(".persist-area").each(function () {
        var el = $(this),
            offset = el.offset(),
            scrollTop = $(window).scrollTop(),
            floatingHeader = $(".floatingHeader", this)

        if ((scrollTop > offset.top) && (scrollTop < offset.top + el.height())) {
            $(".persist-header").addClass("floatingHeader");
            el.addClass("hasFloating");
        } else {
            $(".persist-header").removeClass("floatingHeader");
            el.removeClass("hasFloating");
        };
    });
}

function getHeight() {
    var myWidth = 0, myHeight = 0;
    if (typeof (window.innerWidth) == 'number') {
        //Non-IE
        myWidth = window.innerWidth;
        myHeight = window.innerHeight;
    } else if (document.documentElement && (document.documentElement.clientWidth || document.documentElement.clientHeight)) {
        //IE 6+ in 'standards compliant mode'
        myWidth = document.documentElement.clientWidth;
        myHeight = document.documentElement.clientHeight;
    } else if (document.body && (document.body.clientWidth || document.body.clientHeight)) {
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
            clearTimeout(timers[uniqueId]);
        }
        timers[uniqueId] = setTimeout(callback, ms);
    };
})();

function UpdateLength() {
    waitForFinalEvent(function () {
        $(".persist-header").each(function () {
            var height = $(this).height();
            var windowHeight = getHeight();
            if (height > windowHeight) {
                $(this).addClass("tooLong");
            } else {
                $(this).removeClass("tooLong");
            }
        });
    }, 150, "resize");
}

// DOM Ready
$(function () {
    var clonedHeaderRow;
    $(window)
        .scroll(UpdateTableHeaders)
        .trigger("scroll")
        .resize(UpdateLength)
        .trigger("resize");


    $(".sections").sortable({
        handle: "span.movesection",
        tolerance: 'pointer',
        placeholder: 'ui-state-highlightHelper',
        forcePlaceholderSize: true,
        forceHelperSize: true,
        axis: "y",
        start: function (event, ui) {
            //un-comment this
            /*height = $(ui.item).outerHeight();
            width = $(ui.item).outerWidth();
            $(".sections").css("padding-bottom", height);
            $(".ui-state-highlightHelper").css("height", height);
            $(".ui-state-highlightHelper").css("width", width);*/

            $(ui.item).addClass("dragging");
            $(ui.item).parents(".sections").addClass("dragging");

            $(this).sortable("refresh");
        },
        stop: function (event, ui) {
            //$(".sections").css("padding-bottom", "0px");
            $(ui.item).removeClass("dragging");
            $(ui.item).parents(".sections").removeClass("dragging");
        }
    });

    var timeout = null;
    var clear = function () {
        if (timeout) {
            clearTimeout(timeout);
            timeout = null;
        }
    }

    $(".fields:not(.items)").sortable({
        handle: "span.movecfield",
        tolerance: 'pointer',
        placeholder: 'ui-state-highlightHelper',
        forcePlaceholderSize: true,
        forceHelperSize: true,
        connectWith: ".fields",
        dragOnEmpty: true,
        items: "li.cfield",
        refreshPositions: true,
        axis: "y",
        preserve: true, /*Attivazione Cookie*/
        cookiePrefix: "c4p-fields-",
        start: function (event, ui) {
            /* height = $(ui.item).outerHeight();
            width = $(ui.item).outerWidth();
            $(".sections").css("padding-bottom", height);
            $(".ui-state-highlightHelper").css("height", height);
            $(".ui-state-highlightHelper").css("width", width); */
            $(this).sortable("refresh");
            $(ui.item).addClass("dragging");
            /*$(ui.item).parents(".fields").addClass("dragging");*/
            /*$(ui.item).parents(".section").addClass("activitieIndragging");
            $(".section").not(".activitieIndragging").addClass("nodragging");*/

            /*$(".section.collapsed").droppable({
            accept:function(el){

            return true;
            },
            hoverClass: "ui-state-hover",
            tolerance:"touch",
            refreshPositions: true,
            greedy:false,
            over:function(event,ui){
            $(this).find(".fields").sortable("refresh");
            clear();
            var el=$(this);
            timeout = setTimeout(function() {
            $(".sections").collapsableTreeAdv("expand",el);
            timeout = null;
            }, 1000);
            },
            out:function(event,ui){
            clear();

            }
            });*/
        },
        stop: function (event, ui) {
            //$(".sections").css("padding-bottom", "0px");
            $(ui.item).removeClass("dragging");
            /*$(ui.item).parents(".fields").removeClass("dragging");*/
            /*$(ui.item).parents(".section").removeClass("activitieIndragging");
            $(".section").removeClass("nodragging");*/
        }

    });

    $(".items").sortable({
        handle: "span.movecfield",
        tolerance: 'pointer',
        placeholder: 'ui-state-highlightHelper',
        forcePlaceholderSize: true,
        forceHelperSize: true,
        dragOnEmpty: true,
        items: "li.cfield",
        refreshPositions: true,
        axis: "y",
        start: function (event, ui) {
            /* height = $(ui.item).outerHeight();
            width = $(ui.item).outerWidth();
            $(".sections").css("padding-bottom", height);
            $(".ui-state-highlightHelper").css("height", height);
            $(".ui-state-highlightHelper").css("width", width); */
            $(this).sortable("refresh");
            $(ui.item).addClass("dragging");
            /*$(ui.item).parents(".fields").addClass("dragging");*/
            /*$(ui.item).parents(".section").addClass("activitieIndragging");
            $(".section").not(".activitieIndragging").addClass("nodragging");*/

            /*$(".section.collapsed").droppable({
            accept:function(el){

            return true;
            },
            hoverClass: "ui-state-hover",
            tolerance:"touch",
            refreshPositions: true,
            greedy:false,
            over:function(event,ui){
            $(this).find(".fields").sortable("refresh");
            clear();
            var el=$(this);
            timeout = setTimeout(function() {
            $(".sections").collapsableTreeAdv("expand",el);
            timeout = null;
            }, 1000);
            },
            out:function(event,ui){
            clear();

            }
            });*/
        },
        stop: function (event, ui) {
            //$(".sections").css("padding-bottom", "0px");
            $(ui.item).removeClass("dragging");
            /*$(ui.item).parents(".fields").removeClass("dragging");*/
            /*$(ui.item).parents(".section").removeClass("activitieIndragging");
            $(".section").removeClass("nodragging");*/
        }

    });


    $("ul.fielditems").sortable({
        handle: "span.movecfielditem",
        tolerance: 'pointer',
        placeholder: 'ui-state-highlightHelper',
        forcePlaceholderSize: true,
        forceHelperSize: true,
        dragOnEmpty: true,
        items: "li.fielditem:not(.fixed)",
        refreshPositions: true,
        axis: "y",
        start: function (event, ui) {
            /* height = $(ui.item).outerHeight();
            width = $(ui.item).outerWidth();
            $(".sections").css("padding-bottom", height);
            $(".ui-state-highlightHelper").css("height", height);
            $(".ui-state-highlightHelper").css("width", width); */
            $(this).sortable("refresh");
            $(ui.item).addClass("dragging");
            /*$(ui.item).parents(".fields").addClass("dragging");*/
            /*$(ui.item).parents(".section").addClass("activitieIndragging");
            $(".section").not(".activitieIndragging").addClass("nodragging");*/

            /*$(".section.collapsed").droppable({
            accept:function(el){

            return true;
            },
            hoverClass: "ui-state-hover",
            tolerance:"touch",
            refreshPositions: true,
            greedy:false,
            over:function(event,ui){
            $(this).find(".fields").sortable("refresh");
            clear();
            var el=$(this);
            timeout = setTimeout(function() {
            $(".sections").collapsableTreeAdv("expand",el);
            timeout = null;
            }, 1000);
            },
            out:function(event,ui){
            clear();

            }
            });*/
        },
        stop: function (event, ui) {
            //$(".sections").css("padding-bottom", "0px");
            $(ui.item).removeClass("dragging");
            /*$(ui.item).parents(".fields").removeClass("dragging");*/
            /*$(ui.item).parents(".section").removeClass("activitieIndragging");
            $(".section").removeClass("nodragging");*/
        }

    });

    /*<Reorder>*/








    $("ul.fielditems").sortable({

        update: function (event, ui) {

            var Data = $(this).sortable("serialize");
            var x = 0;

            $(this).find(".hiddendisplayorderoption").each(function () {
                x += 1;
                $(this).val(x);
            });



            $.ajax({
                type: "POST",
                url: "CallReordering.asmx/OptionsReorder",
                data: "{'position':'" + Data + "'}",
                processData: false,
                contentType: "application/json; charset=utf-8",
                dataType: "json",

                success: function (msg) {
                    //alert(msg.d);
                },
                error: function (result) {
                    //alert("Error: (" + result.status + ') [' + result.statusText + ']');

                }
            });

        }
    });
    $("ul.sections").sortable({

        update: function (event, ui) {

            var Data = $(this).sortable("serialize");
            var x = 0;

            $(this).find(".hiddendisplayordersection").each(function () {
                x += 1;
                $(this).val(x);
            });

            $.ajax({
                type: "POST",
                url: "CallReordering.asmx/SectionsReorder",
                data: "{'position':'" + Data + "'}",
                processData: false,
                contentType: "application/json; charset=utf-8",
                dataType: "json",

                success: function (msg) {
                    //alert(msg.d);
                },
                error: function (result) {
                    //alert("Error: (" + result.status + ') [' + result.statusText + ']');

                }
            });

        }
    });
    $("ul.fields").sortable({

        update: function (event, ui) {

            var Data = $(this).sortable("serialize");

            var $section = $(this).parents("li.section").first();

            var sectionId = $section.attr("id");

            $(this).find(".hiddensort").val(sectionId);
            var x = 0;
            $(this).find(".hiddendisplayorder").each(function () {
                x += 1;
                $(this).val(x);
            });

            $.ajax({
                type: "POST",
                url: "CallReordering.asmx/FieldsReorder",
                data: "{'position':'" + Data + "', 'sectionId':'" + sectionId + "'}",
                processData: false,
                contentType: "application/json; charset=utf-8",
                dataType: "json",

                success: function (msg) {
                    //alert(msg.d);
                },
                error: function (result) {
                    //alert("Error: (" + result.status + ') [' + result.statusText + ']');

                }
            });

        }
    });

    $("ul.committees").sortable({

        update: function (event, ui) {

            var Data = $(this).sortable("serialize");
            var x = 0;

            $(this).find(".hiddendisplayordersection").each(function () {
                x += 1;
                $(this).val(x);
            });

            $.ajax({
                type: "POST",
                url: "EvaluationReordering.asmx/CommitteesReorder",
                data: "{'position':'" + Data + "'}",
                processData: false,
                contentType: "application/json; charset=utf-8",
                dataType: "json",

                success: function (msg) {
                    //alert(msg.d);
                },
                error: function (result) {
                    //alert("Error: (" + result.status + ') [' + result.statusText + ']');

                }
            });

        }
    });
    $("ul.criteria").sortable({

        update: function (event, ui) {

            var Data = $(this).sortable("serialize");

            var $section = $(this).parents("li.section").first();

            var sectionId = $section.attr("id");

            $(this).find(".hiddensort").val(sectionId);
            var x = 0;
            $(this).find(".hiddendisplayorder").each(function () {
                x += 1;
                $(this).val(x);
            });

            $.ajax({
                type: "POST",
                url: "EvaluationReordering.asmx/CriteriaReorder",
                data: "{'position':'" + Data + "', 'idCommittee':'" + sectionId + "'}",
                processData: false,
                contentType: "application/json; charset=utf-8",
                dataType: "json",

                success: function (msg) {
                    //alert(msg.d);
                },
                error: function (result) {
                    //alert("Error: (" + result.status + ') [' + result.statusText + ']');

                }
            });

        }
    });

    $("ul.criterionoptions").sortable({

        update: function (event, ui) {

            var Data = $(this).sortable("serialize");
            var x = 0;

            $(this).find(".hiddendisplayorderoption").each(function () {
                x += 1;
                $(this).val(x);
            });



            $.ajax({
                type: "POST",
                url: "EvaluationReordering.asmx/OptionsReorder",
                data: "{'position':'" + Data + "'}",
                processData: false,
                contentType: "application/json; charset=utf-8",
                dataType: "json",

                success: function (msg) {
                    //alert(msg.d);
                },
                error: function (result) {
                    //alert("Error: (" + result.status + ') [' + result.statusText + ']');

                }
            });

        }
    });
    $("ul.submitters").sortable({

        update: function (event, ui) {

            var Data = $(this).sortable("serialize");
            var x = 0;

            $(this).find(".hiddendisplayorder").each(function () {
                x += 1;
                $(this).val(x);
            });

            $.ajax({
                type: "POST",
                url: "CallReordering.asmx/SubmittersReorder",
                data: "{'position':'" + Data + "'}",
                processData: false,
                contentType: "application/json; charset=utf-8",
                dataType: "json",

                success: function (msg) {
                    //alert(msg.d);
                },
                error: function (result) {
                    //alert("Error: (" + result.status + ') [' + result.statusText + ']');

                }
            });

        }
    });

    $("ul.filesattached").sortable({

        update: function (event, ui) {

            var Data = $(this).sortable("serialize");
            var x = 0;

            $(this).find(".hiddendisplayorder").each(function () {
                x += 1;
                $(this).val(x);
            });

            $.ajax({
                type: "POST",
                url: "CallReordering.asmx/AttachmentsReorder",
                data: "{'position':'" + Data + "'}",
                processData: false,
                contentType: "application/json; charset=utf-8",
                dataType: "json",

                success: function (msg) {
                    //alert(msg.d);
                },
                error: function (result) {
                    //alert("Error: (" + result.status + ') [' + result.statusText + ']');

                }
            });

        }
    });

    $("ul.requiredfiles").sortable({

        update: function (event, ui) {

            var Data = $(this).sortable("serialize");
            var x = 0;

            $(this).find(".hiddendisplayorder").each(function () {
                x += 1;
                $(this).val(x);
            });

            $.ajax({
                type: "POST",
                url: "CallReordering.asmx/RequestedFilesReorder",
                data: "{'position':'" + Data + "'}",
                processData: false,
                contentType: "application/json; charset=utf-8",
                dataType: "json",

                success: function (msg) {
                    //alert(msg.d);
                },
                error: function (result) {
                    //alert("Error: (" + result.status + ') [' + result.statusText + ']');

                }
            });

        }
    });
    /*</Reorder>*/

    /*$(".switchsection").click(function () {
    $(this).toggleClass("collapsed");
    $parent = $(this).parents("li.section");
    $parent.find(".collapsable").toggle();
    $parent.toggleClass("collapsed");
    });

    $(".switchcfield").click(function () {
    $(this).toggleClass("collapsed");
    $parent = $(this).parents("li.cfield");
    $parent.find(".collapsable").toggle();
    $parent.toggleClass("collapsed");
    :not(.committees)});*/

    $(".sections").collapsableTreeAdv({
        preserve: true, /*Attivazione Cookie*/
        cookiePrefix: "c4p-sections-",
        selUl: "ul.fields, ul.subfields, div.fielddetails",
        selLi: "li:not(li.sectiondesc)",
        onToggle: function (el, handle) {
            $("div.column.left, div.column.right").attr("style", "");
            if (el.hasClass(".section")) {
                el.find(".collapsable").not(".cfield .collapsable").toggle();
            } else {
                el.find(".collapsable").toggle();
            }
            //$("div.column.left, div.column.right").equalize();
            //checkHeight();
        },
        onResize: function () {
            $("body").simpleEqualize();
        },
        onExpand: function (el, handle) {
            $("div.column.left, div.column.right").attr("style", "");
            if (el.hasClass(".section")) {
                el.find(".collapsable").not(".cfield .collapsable").show();
            } else {
                el.find(".collapsable").show();
            }
            //$("div.column.left, div.column.right").equalize();
            //checkHeight();
        },
        onCollapse: function (el, handle) {
            $("div.column.left, div.column.right").attr("style", "");
            if (el.hasClass(".section")) {
                el.find(".collapsable").not(".cfield .collapsable").hide();
            } else {
                el.find(".collapsable").hide();
            }
            //$("div.column.left, div.column.right").equalize();
            //checkHeight();
        }
    });
    //    $(".sections.committees").collapsableTreeAdv({
    //        preserve: true, /*Attivazione Cookie*/
    //        cookiePrefix: "c4p-committees-",
    //        selUl: "ul.fields, ul.subfields, div.fielddetails",
    //        selLi: "li:not(li.sectiondesc)",
    //        onToggle: function (el, handle) {
    //            $("div.column.left, div.column.right").attr("style", "");
    //            if (el.hasClass(".section")) {
    //                el.find(".collapsable").not(".cfield .collapsable").toggle();
    //            } else {
    //                el.find(".collapsable").toggle();
    //            }
    //            //$("div.column.left, div.column.right").equalize();
    //            //checkHeight();
    //        },
    //        onResize: function () {
    //            $("body").simpleEqualize();
    //        },
    //        onExpand: function (el, handle) {
    //            $("div.column.left, div.column.right").attr("style", "");
    //            if (el.hasClass(".section")) {
    //                el.find(".collapsable").not(".cfield .collapsable").show();
    //            } else {
    //                el.find(".collapsable").show();
    //            }
    //            //$("div.column.left, div.column.right").equalize();
    //            //checkHeight();
    //        },
    //        onCollapse: function (el, handle) {
    //            $("div.column.left, div.column.right").attr("style", "");
    //            if (el.hasClass(".section")) {
    //                el.find(".collapsable").not(".cfield .collapsable").hide();
    //            } else {
    //                el.find(".collapsable").hide();
    //            }
    //            //$("div.column.left, div.column.right").equalize();
    //            //checkHeight();
    //        }
    //    });
    /*$(".fields").collapsableTree({
    selExtraCollapse:".collapsable",
    preserve:true,
    selUl:"ul.subfields, ul.rules"});*/

    $(".subfields").each(function () {
        $(this).find(".subcfield").last().addClass("last");
    });

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
            $(this).parents(".choseselect").find(".chzn-select option").attr("selected", false);
            $(this).parents(".choseselect").find(".chzn-select").trigger("liszt:updated");
            $(this).parents(".choseselect").find(".chzn-select").trigger("change");
        }
    });

    $(".fieldsHide").click(function () {
        //$(".sections").collapsableTreeAdv("hide","li.cfield");
        $(".sections").collapsableTreeAdv("collapse", "li.cfield");
    });

    $(".fieldsShow").click(function () {
        //$(".sections").collapsableTreeAdv("hide","li.cfield");
        $(".sections").collapsableTreeAdv("expand", "li.cfield");
    });

    $(".collapseAll").click(function () {
        //$(".sections").collapsableTreeAdv("hide","li.cfield");
        $(".sections").collapsableTreeAdv("collapseAll");
    });

    $(".expandAll").click(function () {
        //$(".sections").collapsableTreeAdv("hide","li.cfield");
        $(".sections").collapsableTreeAdv("expandAll");
    });

    $("select.chzn-select.partecipants.filtered").change(function () {
        var $all = $("select.chzn-select.partecipants");
        var $allOptions = $all.find("option");
        $allOptions.each(function () {
            $m = $(this);
            $m.prop("disabled", false);
        });
        var $selected = $all.find("option:selected");
        /*  

        $all.each(function () {
        alert($(this));
        alert($(this).val());
        var $options = $(this).find("option");
        $options.each(function () {
        alert("opzioni");
        alert("opzione: " + $(this));
        alert("opzione: " + $(this).val());
        alert("disabled=" + $(this).prop("disabled"));
        alert("selected=" + $(this).prop("selected"));
        });
        });
        alert($selected.val());
        alert($(this).val());*/
        $selected.each(function () {
            $m = $(this);
            $all.find('option').each(function () {
                if ($(this).val() == $m.val()) {
                    $(this).prop("disabled", true);
                }
            });
            $m.prop("disabled", false);
        });

        $all.trigger("liszt:updated");
    });


    $(".dialog").dialog({
        appendTo: "form",
        width: 400,
        autoOpen: false
        /*buttons: { "Ok": function () { $(this).dialog("close"); }, "Cancel": function () { $(this).dialog("close"); } }*/
    });

    $(".dialog.listedit").dialog({
        appendTo: "form",
        width: 700,
        modal: true
    });

    $(".dialog.addnewfield").dialog({
        appendTo: "form",
        width: 700,
        height: 800,
        modal: true,
        closeOnEscape: false,
        open: function (type, data) {
            //$(this).parent().appendTo("form");
            $(".ui-dialog-titlebar-close", this.parentNode).hide();
        }

    });

    $("a.advanced").click(function () {
        $(".dialog.regexedit").dialog("open");
        return false;
    });

    $("span.listedit").click(function () {
        $(".dialog.listedit").dialog("open");
        return false;
    });

    $(".addfield").click(function () {
        var $section = $(this).parents("li.section").first();

        //$("ul.sections").collapsableTreeAdv("deleteCookie", $section);
        $("ul.sections").collapsableTreeAdv("expand", $section);

        return true;
    });

    //    $(".addfield").click(function () {
    //        var $section = $(this).parents("li.section").first();
    //        var sectionId = $section.attr("id");
    //        $(".hiddencurrentsection").val(sectionId);
    //        $(".dialog.addnewfield").dialog("open");
    //        return false;
    //    });

    //    $(".fieldtype").hover(function () {
    //        var id = $(this).attr("id");
    //        $(".divpreview").filter("#preview-" + id).show();
    //        $(".divpreview").not("#preview-" + id).hide();
    //    },
    //        function () {
    //            $(".divpreview").hide();
    //            $(".fieldtype input:checked").each(function () {
    //                var id = $(this).parents(".fieldtype").attr("id");
    //                $(".divpreview").filter("#preview-" + id).show();
    //            });
    //        });

    //    $(".fieldtype input:checked").each(function () {
    //        var id = $(this).attr("id");
    //        //var id = $(this).parents(".fieldtype").attr("id");
    //            $(".hiddenselectedtype").val(id);
    //        $(".divpreview").filter("#preview-" + id).show();
    //    });

    //    $(".fieldtype").change(function () {
    //        if ($(this).filter(":checked")) {
    //            var id = $(this).attr("id");
    //            $(".hiddenselectedtype").val(id);
    //            $(".divpreview").filter("#preview-" + id).show();
    //        }
    //    });

    $("fieldset.expandable").blockableFieldset({
        blockedClass: "disabled"
    });

    $("body").simpleEqualize();

    //    $(".dialog").simpleEqualize({
    //        copyThis: ".leftfield",
    //        resizeThis: ".divpreview"
    //    });

    //$("div.column.left, div.column.right").equalize();
    /*$("div.column.left, div.column.right").resize(function(){

    });*/


    $("input.activator").inputActivator();


    $(".fieldrow.attachmentforall input[type='checkbox']").change(function () {

        var $check = $(this);

        var checked = $check.is(":checked");

        var $li = $check.parents("li.cfield.fileattached");
        var $select = $li.find(".fieldfooter .choseselect select");

        if (!checked) {
            $select.removeAttr("disabled");
        } else {
            $select.attr("disabled", "disabled");
        }

        //var $all = $("select.chzn-select.partecipants");
        $select.trigger("liszt:updated");
    });

    //attivare dopo aver importato il link al file jquery.checkboxList.js nelle pagine del CallForPaper
    //    $(".fieldobject.checkboxlist").checkboxList({
    //        listSelector: "span.checkboxlist",
    //        errorSelector: ".fieldrow.fieldinput label",
    //        checkOnStart: true,
    //        error: {
    //            min: ".minmax .min",
    //            max: ".minmax .max"
    //        }
    //    })

    //jquery.textVal.js

    //    $(".fieldobject.singleline .fieldrow.fieldinput").textVal({
    //        textSelector: "input.inputtext",
    //        charAvailable: ".fieldinfo .maxchar .availableitems",
    //        errorSelector: ".fieldrow.fieldinput label, .fieldinfo",
    //        charMax: ".fieldinfo .maxchar .totalitems"

    //    });

    //    $(".fieldobject.multiline .fieldrow.fieldinput").textVal({
    //        textSelector: "textarea.textarea",
    //        charAvailable: ".fieldinfo .maxchar .availableitems",
    //        errorSelector: ".fieldrow.fieldinput label, .fieldinfo",
    //        charMax: ".fieldinfo .maxchar .totalitems"
    //    });

    $(".closepopup").click(function () {
        window.close();
    });

    $(".accordion .accordion-group:not(.expanded)").each(function () {
        $(this).addClass("compressed");
    });
    $(".accordion").each(function () {
        if ($(this).find(".accordion-group.expanded").size() == 0) {
            $(this).find(".accordion-group:first").removeClass("compressed").addClass("expanded");
        }
    });

    $(".accordion .accordion-group .accordion-handle").live("click", function () {
        $(this).parents(".accordion").find(".accordion-group").addClass("compressed").removeClass("expanded");
        $(this).parents(".accordion-group").removeClass("compressed").addClass("expanded");
    });


    $(".fuzzyinput .coveredradio.enabled").each(function () {
        var $input = $(this).parents(".fuzzyinput").first();
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
                $input.data("value") = $(this).parents("span.checkwrapper").data("value");
            } else {
                $($buttons.get(idx)).removeClass("active");
            }
        });

        var $disable = $parent.data("disable");
    });

    $(".coveredradio.enabled .btnswitch").click(function () {

        var $input = $(this).parents(".fuzzyinput").first();

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
            $input.attr("data-value", $($checks.get(idx)).parents("span.checkwrapper").data("value"));
            //console.log($($checks.get(idx)).prop("checked"));
        }

        return false;
    });

    $(".coveredradio.enabled input[type='radio']").click(function () {
        var $input = $(this).parents(".fuzzyinput").first();
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
                if ($(this).is(":checked")) {
                    $($buttons.get(idx)).addClass("active");
                    $input.data("value") = $(this).parents("span.checkwrapper").data("value");
                } else {
                    $($buttons.get(idx)).removeClass("active");
                }

            }

            var $disable = $parent.data("disable");


        }
    });

    $("span.editable:not(.disabled)").each(function () {
        var $editable = $(this);
        var $edit = $editable.children(".edit");
        var $inputh = $edit.find("input[type='hidden'].view").first();

        if ($inputh.val() == "edit") {
            $(this).removeClass("viewmode").addClass("editmode");
            $inputh.val("edit");
        } else if ($inputh.val() == "") {
            $(this).addClass("viewmode").removeClass("editmode");
            $inputh.val("");
        }
        /*   if ($inputh.val() != "init")
               alert($inputh.val());*/
    });

    $("span.editable:not(.disabled) .view").click(function () {
        var $view = $(this);
        var $editable = $(this).parents("span.editable").first();
        var $edit = $editable.children(".edit");
        var $input = $edit.find("input[type='text'].view");
        $input.val($(this).html().replace("&nbsp;", ""));
        $editable.removeClass("viewmode").addClass("editmode");
        $editable.find("input[type='hidden'].view").val("edit");
        $input.focus();
        //$(".editablehelp").removeClass("hidden");
    });

    $("span.editable:not(.disabled) .edit .icon.cancel").click(function () {
        //$(".editablehelp").addClass("hidden");
        $(this).parents("span.editable").first().removeClass("editmode").addClass("viewmode");
        $(this).parents("span.editable").first().find("input[type='hidden'].view").val("");
        $(this).parents(".error,.linkserror").removeClass("error").removeClass("linkserror");
    });

    $("span.editable:not(.disabled) .edit .icon.ok").click(function () {


        //$(".savesubmit").click();

        $parent = $(this).parents("span.editable").first();
        if ($parent.is(".fuzzy")) {
            var $type = $parent.find("input.fuzzytype");
            /console.log($type);*/
            var id = $type.val();
            var $div = $parent.find(id);
            var template = $div.data("template");
            var fuzzytype = $div.data("fuzzy");
            var fuzzy = fuzzytype + ":";
            $div.find("select").each(function () {
                var mapping = $(this).data("mapping");
                var text = $(this).find("option[value='" + $(this).val() + "']").html();
                template = template.replace(mapping, text);
                fuzzy += $(this).val() + ";";
            });

            $parent.find("span.view").html(template);
            $parent.find(".fuzzyvalue").val(fuzzy);
        }
    });


    $("span.editable:not(.disabled) .edit input").keyup(function (e) {

        var $editable = $(this).parents("span.editable").first();
        var $edit = $editable.children(".edit");

        if (e.which == 13) { $editable.find('.ok').click(); }

        if (e.which == 27) { $editable.find('.cancel').click(); }

    });

    $("span.editable:not(.disabled) .edit .icon").click(function () {
        $(this).parents("span.editable").first().removeClass("editmode").addClass("viewmode");
    });

    $("span.editable.fuzzy .fuzzytabs").tabs({
        activate: function (event, ui) {
            //console.log(ui);
            var id = $(ui.newPanel).attr("id");
            $(this).parents("span.editable.fuzzy").find(".fuzzytype").val("#" + id);
        }
    });
    $("span.editable.fuzzy .fuzzytabs").each(function () {
        var id = $(this).parents("span.editable.fuzzy").find(".fuzzytype").val();
        $(this).find("[href='" + id + "']").click();
    });



});