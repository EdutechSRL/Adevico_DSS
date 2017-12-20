//Edit.aspx + Editskin.aspx + List.aspx + Preview.aspx (download)

var fileDownloadCheckTimer;
function blockUIForDownload() {
    var token = new Date().getTime(); //use the current timestamp as the token value
    $("input[id='" + TokenHiddenFieldId + "']").val(token);
    $.blockUI({ message: DisplayMessage, title: DisplayTitle, draggable: false, theme: true });
    fileDownloadCheckTimer = window.setInterval(function () {
        var cookieValue = $.cookie(CookieName);
        if (cookieValue == token)
            finishDownload();
    }, 1000);
}

function finishDownload() {
    window.clearInterval(fileDownloadCheckTimer);
    $.cookie(CookieName, null); //clears this cookie value
    $.unblockUI();
}



$(document).ready(function () {




    //        $.blockUI.defaults.css.cursor = 'default';
    //        $.blockUI({ message: $('.view-modal'), css: { width: '850px'} }); //Class view-modal
    //        $('.view-modal').appendTo('form:first');
    $(".view-modal").dialog({
        appendTo: "form",
        closeOnEscape: false,
        modal: true,
        width: 850,
        height: 450,
        minHeight: 400,
        minWidth: 840,
        open: function (type, data) {
            //$(this).parent().appendTo("form");
            $(this).parent().children().children('.ui-dialog-titlebar-close').hide();
        }
    });



    $(".sections").collapsableTreeAdv({
        preserve: true, //Attivazione Cookie
        cookiePrefix: "dTp-sections-",
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

//            var $radEditor = $(ui.item).find("*[id$='RDEtext']").first();
//            var editor = $find($radEditor.attr('id'));
//            editor.css("display", "none");


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

            var $radEditor = $(ui.item).find("*[id$='RDEtext']").first();
            var editor = $find($radEditor.attr('id'));
            editor.onParentNodeChanged();

            //$(".sections").css("padding-bottom", "0px");
            $(ui.item).removeClass("dragging");
            /*$(ui.item).parents(".fields").removeClass("dragging");*/
            /*$(ui.item).parents(".section").removeClass("activitieIndragging");
            $(".section").removeClass("nodragging");*/


        }

    });


    $("ul.fields").sortable({

        update: function (event, ui) {

            //            var $radEditor = $(ui.item).find("*[id$='RDEtext']").first();

            //            var editor = $find($radEditor.attr('id'));
            //            editor.onParentNodeChanged();

            var Data = $(this).sortable("serialize");

            var $section = $(this).parents("li.section").first();

            var sectionId = $section.attr("id");

            $(this).find(".hiddensort").val(sectionId);
            var x = 0;
            $(this).find(".hiddendisplayorder").each(function () {
                x += 1;
                $(this).val(x);
            });

            getOrder()
        }
    });




    //  DropDownList bottoni
    $(".ddbuttonlist.enabled").dropdownButtonList();
});


// UC BODY

$.fn.insertAtCaret = function (tagName) {
    return this.each(function () {
        if (document.selection) {
            //IE support
            this.focus();
            sel = document.selection.createRange();
            sel.text = tagName;
            this.focus();
        } else if (this.selectionStart || this.selectionStart == '0') {
            //MOZILLA/NETSCAPE support
            startPos = this.selectionStart;
            endPos = this.selectionEnd;
            scrollTop = this.scrollTop;
            this.value = this.value.substring(0, startPos) + tagName + this.value.substring(endPos, this.value.length);
            this.focus();
            this.selectionStart = startPos + tagName.length;
            this.selectionEnd = startPos + tagName.length;
            this.scrollTop = scrollTop;
        } else {
            this.value += tagName;
            this.focus();
        }
    });
};


// Edit Version
function checkAll(chb) {

    if (chb.checked) {
        $("span.cbxsel input").prop("checked", true);
    } else {
        $("span.cbxsel input").prop("checked", false);
    }
}

// List.aspx
$(function () {

    //DA fare:
    // 1. Aggiungere le classi ai vari elementi
    // 2. Togliere l'attuale internazionalizzazione sugli elementi altrimenti ridondanti
    // 3. Aggiungere i vari elementi nei .js di internazionalizzazione...

    $("table.template").treeTable({
        clickableNodeNames: false,
        initialState: "collapsed",
        persist: false
    });

    $(".needconfirm-versionActivate").needConfirm({
        // customize as you want...
        msgFunction: function (item) { return ConfirmMsg(item, "template", "versionActivate") },
        addConfirmClass: true
    });

    $(".needconfirm-versionEnable").needConfirm({
        // customize as you want...
        msgFunction: function (item) { return ConfirmMsg(item, "template", "versionEnable") },
        addConfirmClass: true
    });

    $(".needconfirm-versionDisable").needConfirm({
        // customize as you want...
        msgFunction: function (item) { return ConfirmMsg(item, "template", "versionDisable") },
        addConfirmClass: true
    });

    $(".needconfirm-templateDisable").needConfirm({
        // customize as you want...
        msgFunction: function (item) { return ConfirmMsg(item, "template", "templateDisable") },
        addConfirmClass: true
    });

});