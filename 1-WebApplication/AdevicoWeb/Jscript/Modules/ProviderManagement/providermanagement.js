$(function () {

    var autoOpenDialog = false;

    $(".dialog").dialog({
        appendTo:"form",
        autoOpen: autoOpenDialog,
        width: "500px",
        open: function () {

        }
    }); //.parent().appendTo("#content .DIV_MP_Content"); ;

    $(".openAddDynamicField").click(function () {
        $(".dialog.addDynamicField").dialog("open");
        return false;
    });

    $("select.expandoption").change(function () {
        var x = $(this).children("option.other:selected").size() == 1;
        if (x == 1) {
            $(this).parents(".details").first().find(".Field_Row.other").removeClass("hidden");
        } else {
            $(this).parents(".details").first().find(".Field_Row.other").addClass("hidden");
        }
    });

    $(".divswitch").hide();

    $("select.divswitcher").each(function () {
        var x = $(this).children("option:selected").first();
        var id = "#" + x.attr("class");

        $(this).parents(".Field_Row").first().find(".divswitch").hide();
        $(id).show();
    });

    $("select.divswitcher").change(function () {
        var x = $(this).children("option:selected").first();
        var id = "#" + x.attr("class");

        $(this).parents(".Field_Row").first().find(".divswitch").hide();
        $(id).show();
    });

    /*

    fielditems

    */

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

    $(".macattribute ul.fielditems").sortable({

        update: function (event, ui) {

            var Data = $(this).sortable("serialize");
            var x = 0;

            $(this).find(".hiddendisplayorderoption").each(function () {
                x += 1;
                $(this).val(x);
            });

            $.ajax({
                type: "POST",
                url: "AttributeReorder.asmx/MacAttributeItemReorder",
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


    $(".inputActivator").inputActivator();



});