/// <reference path="jquery-1.4.1.min.js" />
/// <reference path="jquery-ui-1.8.11.custom.min.js" />

function SelectTab(IdTabContainer, IdTab) 
{
    $("#" + IdTabContainer).tabs("select", IdTab);
}

function SelectTab(IdTab)
{
    $("#tabs").tabs("select",IdTab);
}

$(document).ready(function () {

    $(".dialog").dialog({
        appendTo: "form",
        autoOpen: false,
        modal: true,
        open: function (event, ui) {
            //$(this).parent().appendTo(jQuery("form:first"));

            tinyMCE.settings = {
                theme: "advanced",
                mode: "none",
                plugins: "safari,pagebreak,style,layer,table,save,advhr,advimage,advlink,emotions,iespell,inlinepopups,insertdatetime,preview,media,searchreplace,print,contextmenu,paste,directionality,fullscreen,noneditable,visualchars,nonbreaking,xhtmlxtras,template,example",
                //                    theme_advanced_buttons1 : "bold,italic,underline,separator,strikethrough,justifyleft,justifycenter,justifyright, justifyfull,bullist,numlist,undo,redo,link,unlink",
                //                    theme_advanced_buttons2 : "",
                //                    theme_advanced_buttons3 : "",
                theme_advanced_buttons1: "bold,italic,underline,strikethrough,|,justifyleft,justifycenter,justifyright,justifyfull,styleselect,formatselect,fontselect,fontsizeselect",
                theme_advanced_buttons2: "cut,copy,paste,pastetext,pasteword,|,search,replace,|,bullist,numlist,|,outdent,indent,blockquote,|,undo,redo,|,link,unlink,anchor,image,cleanup,help,code,|,insertdate,inserttime,preview,|,forecolor,backcolor",
                theme_advanced_buttons3: "tablecontrols,|,hr,removeformat,visualaid,|,sub,sup,|,charmap,emotions,iespell,media,advhr,|,fullscreen",
                theme_advanced_buttons4: '',
                //                    theme_advanced_buttons4: "insertlayer,moveforward,movebackward,absolute,|,styleprops,|,cite,abbr,acronym,del,ins,|,nonbreaking,pagebreak",
                theme_advanced_toolbar_location: "top",
                theme_advanced_toolbar_align: "left"
            };

            $(this).find('textarea.editorSimple').each(function (index) {
                tinyMCE.execCommand('mceAddControl', false, $(this).attr('id'));

            });
        },
        beforeclose: function (event, ui) {
            $(this).find('textarea.editorSimple').each(function (index) {
                tinyMCE.execCommand('mceRemoveControl', false, $(this).attr('id'));
            });
        }
    });

    $(".dialog").each(function () {
        var title = $(this).find("label.dialog-title-hidden").first().html();
        var width = $(this).css("width");

        $(this).dialog("option", "title", title);

        //define the dialog width against the css width
        $(this).dialog("option", "width", width);
    });

    $(".dialog-cancel").live("click", function (e) {
        $(".dialog").dialog("close");
        return false;
    });

    //Re-put Dialog inside a form after dialog-declaration
    //    jQuery(".dialog").parent().appendTo(jQuery("form:first"));

    $(".containerSkinList .see-more").live("click", function () {
        var id = $(this).parents("li.skin").attr("id").replace("ItemSkin_", "");
        $(".see-more-dialog").find(".skinId").first().val(id);
        $(".see-more-dialog").dialog("open");
        return false;
    });

    $(".containerSkinList .share").live("click", function () {
        var id = $(this).parents("li.skin").attr("id").replace("ItemSkin_", "");
        $(".share-dialog").find(".skinId").first().val(id);
        $(".share-dialog").dialog("open");
        return false;
    });

    $(".containerSkinList .copy").live("click", function () {
        var id = $(this).parents("li.skin").attr("id").replace("ItemSkin_", "");
        $(".copy-dialog").find(".skinId").first().val(id);
        $(".copy-dialog").dialog("open");
        return false;
    });

    $(".needConfirm").live("click", function () {
        var txt = $(this).attr("title");
        return confirm(txt);
    });

    $("#tabs").hide();
    $("#tabs").tabs({ cookie: { expires: 1} });

    $("#tabs").show();

    $(".containerSkinEdit .add-logo").live("click", function () {

        $(".add-logo-dialog").find(".item-url").attr("src", "");
        $(".add-logo-dialog").find(".item-link").val("");
        $(".add-logo-dialog").find(".item-title").val("");
        $(".add-logo-dialog").find(".item-size-1").val("");
        $(".add-logo-dialog").find(".item-size-2").val("");

        $(".add-logo-dialog").find(".add-logo-hidden").val("");

        $(".add-logo-dialog").find(".only-new").show();
        $(".add-logo-dialog").find(".only-edit").hide();

        $(".add-logo-dialog").dialog("open");

        return false;
    });
    $(".containerSkinEdit .edit-logo").live("click", function () {
        var url = $(this).parents("tr").first().find("[axis=url]").children("span").html();
        var link = $(this).parents("tr").first().find("[axis=link]").children("span").html();
        var title = $(this).parents("tr").first().find("[axis=title]").children("span").html();
        var size = $(this).parents("tr").first().find("[axis=size]").children("span").html();

        $(".add-logo-dialog").find(".add-logo-hidden").val(url);

        $(".add-logo-dialog").find(".only-new").hide();
        $(".add-logo-dialog").find(".only-edit").show();

        $(".add-logo-dialog").find(".item-url").attr("src", url);
        $(".add-logo-dialog").find(".item-link").val(link);
        $(".add-logo-dialog").find(".item-title").val(title);
        var sizeArray = size.split("x");
        $(".add-logo-dialog").find(".item-size-1").val(sizeArray[0].replace(" ", ""));
        $(".add-logo-dialog").find(".item-size-2").val(sizeArray[1].replace(" ", ""));

        $(".add-logo-dialog").dialog("open");
        return false;
    });
    $(".containerSkinEdit .add-text").live("click", function () {

        $(".add-text-dialog").find(".item-text").val("");
        $(".add-text-dialog").find(".item-language").val("it-IT");

        var hasOptions = $(".add-text-dialog").find("select.item-language option").length > 0;

        if (hasOptions) {

            $(".add-text-dialog").find(".only-new").show();
            $(".add-text-dialog").find(".only-edit").hide();

            $(".add-text-dialog").find(".add-text-hidden").val("");



            $(".add-text-dialog").dialog("open");
        } else {
            alert("No Languages available");
        }
        return false;
    });
    $(".containerSkinEdit .edit-text").live("click", function () {

        var text = $(this).parents("tr").first().find("[axis=text]").children("span").html();
        var language = $(this).parents("tr").first().find("[axis=language]").children("span").html();
        var languageName = $(this).parents("tr").first().find("[axis=language-name]").children("span").html();
        $(".add-text-dialog").find(".item-text").val(text);
        //$(".add-text-dialog").find(".item-language option[value='" + language + "']").attr("selected", "selected");
        $(".add-text-dialog").find(".item-language-name").html(languageName + " [" + language + "]");

        $(".add-text-dialog").find(".only-new").hide();
        $(".add-text-dialog").find(".only-edit").show();

        $(".add-text-dialog").find(".add-text-hidden").val(language);

        $(".add-text-dialog").dialog("open");
        return false;
    });
    var fixHelper = function (e, ui) {
        ui.children().each(function () {
            $(this).width($(this).width());
        });
        return ui;
    };
    $("table.Sortable").tableDnD({
        onDrop: function (table, row) {
            //alert($.tableDnD.serialize());
            $.ajax({
                type: "POST",
                contentType: "application/json; charset=utf-8",
                url: "OSReordering.asmx/LogoReordering",
                data: "{'position':'" + $.tableDnD.serialize() + "'}",
                dataType: "json",
                error: function (result) {
                    //alert("ko: " + result.status + " " + result.statusText);
                    jQuery.noticeAdd({
                        text: "Reordering failure: " + result.status + " " + result.statusText,
                        stay: true
                    });
                },
                success: function (msg) {
                    jQuery.noticeAdd({
                        text: 'Correctly Reordered',
                        stay: false                        
                    });
                }
            });

            var pos = 1;
            //            alert(pos);
            //            alert($(table).find("tr").size());
            $(table).find("tr").not("tr[id='Order_header']").each(function () {
                $(this).attr("id", "Order_" + pos);
                pos += 1;

            });
        },
        dragHandle: "dragHandle"
    });

    //    $(".ellipsis").each(function () {
    //        var html = $(this).html();
    //        var newHtml=
    //    });



    $(".ShowMoreLess").add(".Ellipsis").click(function () {
        var parent = $(this).parents("li.skin");
        parent.find(".SeeMoreList").toggleClass("SeeMoreList-Compacted");
        parent.find(".ShowMoreLess").toggleClass("Close");
        var items = parent.find(".SeeMoreList-Compacted").find(".SeeMoreItem").not(".Visible").size();
        if (items > 0) {
            parent.find(".Ellipsis").show();
        } else {
            parent.find(".Ellipsis").hide();
        }
    });

    CheckEllipsis();

    function CheckEllipsis() {
        $(".Ellipsis").hide();

        $(".SeeMoreList-Compacted").each(function () {
            var parent = $(this).parents("li.skin");
            var items = parent.find(".SeeMoreList-Compacted").find(".SeeMoreItem").not(".Visible").size();
            if (items > 0) {
                parent.find(".Ellipsis").show();
            } else {
                parent.find(".Ellipsis").hide();
            }
        });


    }

});