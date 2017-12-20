
/**
 * Created by roberto.maschio on 26/11/13.
 */
var resetUpload = function () {
    var current = $(this);
    try {
        $telerik.$('.RadAsyncUpload').each(function (index, value) {
            var id = $(value).attr('id');
            var upload = $find(id);
            try {
                var uploaded = upload.getUploadedFiles();
                console.log(uploaded);
                for (i = uploaded.length - 1; i >= 0; i--) {
                    upload.deleteFileInputAt(i);
                }
            }
            catch (uploadError) {

            }
            try {
                var uploadingRows = $telerik.$(".RadAsyncUpload").find(".ruUploadProgress");
                for (var i = 0; i < uploadingRows.length; i++) {
                    console.log(uploadingRows[i]);
                    if (!$telerik.$(uploadingRows[i]).hasClass("ruUploadCancelled") && !$telerik.$(uploadingRows[i]).hasClass("ruUploadFailure") && !$telerik.$(uploadingRows[i]).hasClass("ruUploadSuccess")) {
                        $telerik.$(".ruCancel").click();
                    }
                }
            }
            catch (inputError) {

            }
            try {
                var uploadingRows = $telerik.$(".RadAsyncUpload").find(".ruUploadProgress");
                for (var i = 0; i < uploadingRows.length; i++) {
                    $telerik.$(".ruRemove").click();
                }
            }
            catch (cancelError) {
            }
        });
    }
    catch (noTelerik) {

    }
};

/* FILE UPLOADING MANAGEMENT*/

function OnClientValidationFailed(sender, args) {
    var fileExtention = args.get_fileName().substring(args.get_fileName().lastIndexOf('.') + 1, args.get_fileName().length);
    if (args.get_fileName().lastIndexOf('.') != -1) {//this checks if the extension is correct
        if (sender.get_allowedFileExtensions().lenght > 0 && sender.get_allowedFileExtensions().indexOf(fileExtention) == -1) {
            alert("Wrong Extension!");
        }
        else {
            alert("Wrong file size!");
        }
    }
    else if (sender.get_allowedFileExtensions().lenght > 0) {
        alert("not correct extension!");
    }
}
/* TELERIK START   */

function OnClientValidationFailedInLine(sender, args) {
    var $row = $(args.get_row());
    var erorMessage = getErrorMessage(sender, args);
    var span = createError(erorMessage);
    $row.addClass("ruError");
    $row.append(span);
}

function getErrorMessage(sender, args) {
    var fileExtention = args.get_fileName().substring(args.get_fileName().lastIndexOf('.') + 1, args.get_fileName().length);
    if (args.get_fileName().lastIndexOf('.') != -1) {//this checks if the extension is correct
        if (sender.get_allowedFileExtensions().indexOf(fileExtention) == -1) {
            return itemError_NotSupported;
        }
        else {
            return itemError_Size;
        }
    }
    else {
        return itemError_Extension;
    }
}
function createError(erorMessage) {
    var input = '<span class="ruErrorMessage">' + erorMessage + ' </span>';
    return input;
}

/* TELERIK END   */

$(function () {

    'use strict';

    $(".ddbuttonlist.enabled").dropdownButtonList();

    $(".dropdown").each(function () {
        var $dropdown = $(this);
        $dropdown.find("input[type='hidden'].changetrigger").change(function () { $dropdown.find(".folderapply").click(); });
    });



    $(".groupedselector.normal .selectoricon, .groupedselector.normal .selectorlabel").click(function () {

        var $group = $(this).parents(".groupedselector").first();

        $(".groupedselector").not($group).removeClass("clicked");
        $group.toggleClass("clicked");

    });

    $(".groupedselector.normal .selectoritem").click(function () {

        var $group = $(this).parents(".groupedselector").first();
        $group.removeClass("clicked");

        $group.find(".selectoritem").removeClass("active");
        $(this).addClass("active");

        $group.find(".selectorgroup .selectorlabel").html($(this).find(".selectorlabel").html());

    });


    $(".groupedselector.noactive .selectoricon, .groupedselector.noactive .selectorlabel").click(function (event) {
        var $group = $(this).parents(".groupedselector").first();

        $(".groupedselector").not($group).removeClass("clicked");
        $group.toggleClass("clicked");
        event.stopPropagation();
    });

    $(".groupedselector.noactive .selectoritem").click(function () {
        var $group = $(this).parents(".groupedselector").first();
        $group.removeClass("clicked");

        $group.find(".selectoritem").removeClass("active");
        $(this).addClass("active");

        $group.find(".selectorgroup .selectorlabel").html($(this).find(".selectorlabel").html());

    });



    $(".groupedselector.noactive.clicked").mouseout(function () {
       
    });

    $(".groupedselector.noactive.clicked, .groupedselector.clicked .selectormenu").mouseenter(function () {

    });

    $("body").click(function () {
        $(".groupedselector.noactive.clicked").removeClass("clicked");
    });

    function closeMenu() {
        $(".groupedselector.noactive.clicked").removeClass("clicked");
    }

    function checkHeight() {

        var $tree = $(".section.tree");
        var $files = $(".section.files");
        $tree.data("height", 0);
        $files.data("height", 0);

        $tree.css("height", "auto");
        $files.css("height", "auto");
        $files.removeClass("height-modified");
        $tree.removeClass("height-modified");
    
        var h1 = $tree.outerHeight();
        var h2 = $files.outerHeight();
        var newh = Math.max(h1, h2);

        $tree.css("height", newh);
        $files.css("height", newh);
        $files.addClass("height-modified");
        $tree.addClass("height-modified");
    }



    $("ul.nestedtree.root").collapsableTreeAdv({
        selLi: 'li.treenode',
        selUl: 'ul.nestedtree',
        preserve: true,
        onResize: function () {
            checkHeight();
        }
    });

    $("li.treenode").each(function () {
        if ($(this).find("li.treenode").size() == 0) {
            $(this).find(".handle").css("visibility", "hidden");
        }
    })

    $.extend($.expr[':'], {
        unchecked: function (obj) {
            return ((obj.type == 'checkbox' || obj.type == 'radio') && !$(obj).is(':checked'));
        }
    });

    $("table thead tr th input:checkbox").click(function () {
        var $table = $(this).parents("table").first();
        var checkedStatus = this.checked;
        var index = $(this).parent().index() + 1;
        $table.find("tbody tr td:nth-child(" + index + ") input:checkbox").each(function () {
            this.checked = checkedStatus;
        });
    });

    $("table tbody tr td input:checkbox").click(function () {
        var $table = $(this).parents("table").first();
        var checkedStatus = this.checked;
        var index = $(this).parent().index() + 1;
        var size = $table.find("tbody tr td:nth-child(" + index + ") input:checkbox").size();

        var size1 = $table.find("tbody tr td:nth-child(" + index + ") input:checkbox").filter(":checked").size();

        if (size == size1) {
            $table.find("thead tr th:nth-child(" + index + ") input:checkbox").prop("checked", true);

        } else {
            $table.find("thead tr th:nth-child(" + index + ") input:checkbox").prop("checked", false);
        }
    });

    $(".treeselect .selection input:checkbox").on('change', function () {

        $(this).parents("li").first().find('ul').find('.selection input:checkbox').prop('checked', $(this).is(":checked"));
        $(this).parents("li").first().find('ul').find('.selection input:checkbox').prop('indeterminate', false);

        var $parentLI = $(this).parents("li").first();
        var $parentUL = $(this).parents("ul").first();
        var $parentULLI = $parentUL.parents("li").first();

        checkParents($parentUL, $parentULLI);



    });

    function checkParents(parentUL, parentULLI) {
        var $yes = parentUL.find("input:checkbox:checked");
        var yes = $yes.size();
        var $no = parentUL.find("input:checkbox:unchecked");
        var no = $no.size();

        if (yes != 0 && no != 0) {
            parentULLI.find("input:checkbox").first().prop('indeterminate', true);
        }

        if (no == 0) {
            parentULLI.find("input:checkbox").first().prop('indeterminate', false);
            parentULLI.find("input:checkbox").first().prop('checked', true);

        } else {
            parentULLI.find("input:checkbox").first().prop('indeterminate', true);
        }

        if (yes == 0) {
            parentULLI.find("input:checkbox").first().prop('indeterminate', false);
            parentULLI.find("input:checkbox").first().prop('checked', false);
        } else {

        }



        var $parentULnext = parentUL.parents("ul").first();
        var $parentULLInext = $parentULnext.parents("li").first();
        if (parentUL.is(":not(.root)")) {
            checkParents($parentULnext, $parentULLInext);
        }
    }




    var name = filerepository_cookiename;

    function CheckCookies() {
        var fullwidth = $.cookie(filerepository_cookiename + ".NarrowWideView");
        if (fullwidth == "true") {
            $(".page-width").addClass("fullwidth");
            $("div.filerepository").addClass("fullwidth");
        } else {
            $(".page-width").removeClass("fullwidth");
            $("div.filerepository").removeClass("fullwidth");
        }

        var extrainfo = $.cookie(filerepository_cookiename + ".Extrainfo");
        if (extrainfo == "true") {
            $("table.files").addClass("noextrainfo");
            $("div.filerepository").addClass("noextrainfo");
            $(".commands .command.info").addClass("off").removeClass("on");
        } else {
            $("table.files").removeClass("noextrainfo");
            $("div.filerepository").removeClass("noextrainfo");
            $(".commands .command.info").removeClass("off").addClass("on");
        }

        var stats = $.cookie(filerepository_cookiename + ".Statistics");
        if (stats == "true") {
            $("table.files").addClass("nostats");
            $("div.filerepository").addClass("nostats");
            $(".commands .command.stats").addClass("off").removeClass("on");
        } else {
            $("table.files").removeClass("nostats");
            $("div.filerepository").removeClass("nostats");
            $(".commands .command.stats").removeClass("off").addClass("on");
        }

        var dates = $.cookie(filerepository_cookiename + ".Date");
        if (dates == "true") {
            $("table.files").addClass("nodate");
            $("div.filerepository").addClass("nodate");
            $(".commands .command.date").addClass("off").removeClass("on");
        } else {
            $("table.files").removeClass("nodate");
            $("div.filerepository").removeClass("nodate");
            $(".commands .command.date").removeClass("off").addClass("on");
        }

        var showtree = $.cookie(filerepository_cookiename + ".Tree");
        if (showtree == "true") {
            $("div.filerepository").addClass("notree");

            $(".section.tree").hide();
            $(".section.files").addClass("grid_12");
            $(".section.files").addClass("alpha");

            $(".commands .command.tree").addClass("off").removeClass("on");
        } else {
            $("div.filerepository").removeClass("notree");

            $(".section.tree").show();
            $(".section.files").removeClass("grid_12");
            $(".section.files").removeClass("alpha");

            $(".commands .command.tree").removeClass("off").addClass("on");
        }

       
    }


    var type = $.cookie(filerepository_cookiename + ".viewtype");

    if (type === undefined || type === null) {
        type = filerepository_default.toLowerCase();
        if (type == "simple") {
            SetCookies(filerepository_simple);
            $.cookie(filerepository_cookiename + ".viewtype", "simple");
        } else if (type == "standard") {
            SetCookies(filerepository_standard);
            $.cookie(filerepository_cookiename + ".viewtype", "standard");
        } else if (type == "advanced") {
            SetCookies(filerepository_advanced);
            $.cookie(filerepository_cookiename + ".viewtype", "advanced");
        }
    };

    if (type == "simple") {
        $(".commands .command").removeClass("active");
        $(".commands .command.simple").addClass("active");

    } else if (type == "standard") {
        $(".commands .command").removeClass("active");
        $(".commands .command.standard").addClass("active");

    } else if (type == "advanced") {
        $(".commands .command").removeClass("active");
        $(".commands .command.advanced").addClass("active");

    } else if (type == "custom") {
        $(".commands .command").removeClass("active");
        $(".commands .command.custom").addClass("active");
    } else {
    }


    CheckCookies();




    checkHeight();

    if (jQuery().myProgressBar) {
        $(".progressbar").myProgressBar();
    }

    $(".commands .command.wide").click(function () {
        $(".page-width").toggleClass("fullwidth");
        $("div.filerepository").toggleClass("fullwidth");
        $.cookie(filerepository_cookiename + ".NarrowWideView", $(".page-width").is(".fullwidth"));

        if (jQuery().myProgressBar) {
            $(".progressbar").myProgressBar();
        }

        breadcrumb();
    });

    $(".commands .command.narrow").click(function () {
        $(".page-width").toggleClass("fullwidth");
        $("div.filerepository").toggleClass("fullwidth");
        $.cookie(filerepository_cookiename + ".NarrowWideView", $(".page-width").is(".fullwidth"));
        if (jQuery().myProgressBar) {
            $(".progressbar").myProgressBar();
        }

        breadcrumb();
    });

    $(".commands .command.info").click(function () {
        $("table.files").toggleClass("noextrainfo");
        $("div.filerepository").toggleClass("noextrainfo");
        $(".commands .command.info").toggleClass("on").toggleClass("off");
        $.cookie(filerepository_cookiename + ".Extrainfo", $("table.files").is(".noextrainfo"));

    });


    $(".commands .command.stats").click(function () {
        $("table.files").toggleClass("nostats");
        $("div.filerepository").toggleClass("nostats");
        $(".commands .command.stats").toggleClass("on").toggleClass("off");
        $.cookie(filerepository_cookiename + ".Statistics", $("table.files").is(".nostats"));
    });

    $(".commands .command.date").click(function () {
        $("table.files").toggleClass("nodate");
        $("div.filerepository").toggleClass("nodate");
        $(".commands .command.date").toggleClass("on").toggleClass("off");
        $.cookie(filerepository_cookiename + ".Date", $("table.files").is(".nodate"));
    });

    $(".commands .command.tree").click(function () {
        $("div.filerepository").toggleClass("notree");
        $(".commands .command.tree").toggleClass("on").toggleClass("off");

        $(".section.tree").toggle();
        $(".section.files").toggleClass("grid_12");
        $(".section.files").toggleClass("alpha");

        $.cookie(filerepository_cookiename + ".Tree", $("div.filerepository").is(".notree"));
    });

   
    function SetCookies(value) {
        var values = value.split(",");
        $.cookie(filerepository_cookiename + ".Tree", values[0] == "1");
        $.cookie(filerepository_cookiename + ".Statistics", values[1] == "1");
        $.cookie(filerepository_cookiename + ".Extrainfo", values[2] == "1");
        $.cookie(filerepository_cookiename + ".Date", values[3] == "1");
        $.cookie(filerepository_cookiename + ".NarrowWideView", values[4] == "1");
    }





    $(".commands .command.simple").click(function () {
        $(this).parents(".commands").first().find(".clicked").removeClass("clicked");
        $(this).parents(".commands").first().find(".active").removeClass("active");
        $(this).addClass("active");

        SetCookies(filerepository_simple);
        $.cookie(filerepository_cookiename + ".viewtype", "simple");

        CheckCookies();
        checkHeight();
        breadcrumb();
        return false;
    });
    $(".commands .command.standard").click(function () {
        $(this).parents(".commands").first().find(".clicked").removeClass("clicked");
        $(this).parents(".commands").first().find(".active").removeClass("active");
        $(this).addClass("active");

        $.cookie(filerepository_cookiename + ".viewtype", "standard");

        SetCookies(filerepository_standard);

        CheckCookies();
        checkHeight();
        breadcrumb();
        return false;
    });
    $(".commands .command.advanced").click(function () {
        $(this).parents(".commands").first().find(".clicked").removeClass("clicked");
        $(this).parents(".commands").first().find(".active").removeClass("active");
        $(this).addClass("active");

        $.cookie(filerepository_cookiename + ".viewtype", "advanced");

        SetCookies(filerepository_advanced);

        CheckCookies();
        checkHeight();
        breadcrumb();
        return false;
    });

    $("div.filerepository table.files tr.file").mouseenter(function () {
        var $grouped = $(this).find(".groupedselector");
        $(this).parents("table.files").first().find(".groupedselector.clicked").not($grouped).removeClass("clicked");
    });
    $("div.filerepository ul.directories li.directory .header").mouseenter(function () {
        var $grouped = $(this).find(".groupedselector");
        $(this).parents("ul.directories").first().find(".groupedselector.clicked").not($grouped).removeClass("clicked");
    });


    var group = $("ul.sortabletree").sortable({
        handle: ".text",

        onDrop: function (item, container, _super) {
            var ser = "";

            group.find("li.sortableitem").each(function () {

                var $parent = $(this).parents("li.sortableitem").first();
                var parentId = 0;
                if ($parent.size() > 0) {
                    parentId = $parent.attr("id")
                }

                ser = ser + $(this).attr("id") + ":" + parentId + ";";
            });
            $('.serialize_output').first().val(ser);


            _super(item, container);
        }

    });

    if (jQuery().dropdownList) {
        $(".dropdown.enabled").dropdownList({ changeWidth: true });
    }

    $(".toolbar .icon.upload").click(function () {

        $(".dialog.fileupload").dialog("open");
        $("input[type='hidden'].autoopendialog").first().val(".dialog.fileupload");
    });
    $(".toolbar .icon.share").click(function () {

        $(".dialog.fileshare").dialog("open");
    });
    $(".toolbar .icon.permission").click(function () {

        $(".dialog.filepermission").dialog("open");
    });
    $(".toolbar .icon.newfolder").click(function () {
        $(".dialog.createnewfolder").dialog("open");
    });
    $(".toolbar .icon.links").click(function () {

        $(".dialog.createnewlink").dialog("open");
    });

    var dlg = $(".dialog.createnewfolder").dialog({
        appendTo: "form",
        autoOpen: false,
        closeOnEscape: false,
        modal: true,
        width: 750,
        height: 450,
        minHeight: 200,
        minWidth: 300,
        open: function (type, data) {
            $(this).parent().css("top", "50px");
            $(this).parent().css("position", "fixed");
            $(this).find("input[type='file']").val(''); $(this).find("input[type='text']").val('');
            $(this).find("input[type='checkbox']").prop('checked', false);
        },
        resizeStart: function (event, ui) {
            var position = [(Math.floor(ui.position.left) - $(window).scrollLeft()),
                                (Math.floor(ui.position.top) - $(window).scrollTop())];
            $(event.target).parent().css('position', 'fixed');
            $(dlg).dialog('option', 'position', position);
        },
        resizeStop: function (event, ui) {
            var position = [(Math.floor(ui.position.left) - $(window).scrollLeft()),
                                (Math.floor(ui.position.top) - $(window).scrollTop())];
            $(event.target).parent().css('position', 'fixed');
            $(dlg).dialog('option', 'position', position);
        },
        close: function (type, data) {
            $(this).find("input[type='file']").val(''); $(this).find("input[type='text']").val('');
            $(this).find("input[type='checkbox']").prop('checked', false);
            $("input[type='hidden'].autoopendialog").first().val('');
        }
    });

    $(".dialog.createnewfolder .close").click(function () {
        $(this).parents(".createnewfolder").first().dialog("close");
        $(this).find("input[type='file']").val(''); $(this).find("input[type='text']").val('');
        $(this).find("input[type='checkbox']").prop('checked', false);
        $("input[type='hidden'].autoopendialog").val("");
        return false;
    });

    var dlg = $(".dialog.createnewlink").dialog({
        appendTo: "form",
        autoOpen: false,
        closeOnEscape: false,
        modal: true,
        width: 750,
        height: 450,
        minHeight: 200,
        minWidth: 300,
        open: function (type, data) {
            $(this).parent().css("top", "50px");
            $(this).parent().css("position", "fixed");
            $(this).find("input[type='file']").val(''); $(this).find("input[type='text']").val('');
            $(this).find("input[type='checkbox']").prop('checked', false);
        },
        resizeStart: function (event, ui) {
            var position = [(Math.floor(ui.position.left) - $(window).scrollLeft()),
                                (Math.floor(ui.position.top) - $(window).scrollTop())];
            $(event.target).parent().css('position', 'fixed');
            $(dlg).dialog('option', 'position', position);
        },
        resizeStop: function (event, ui) {
            var position = [(Math.floor(ui.position.left) - $(window).scrollLeft()),
                                (Math.floor(ui.position.top) - $(window).scrollTop())];
            $(event.target).parent().css('position', 'fixed');
            $(dlg).dialog('option', 'position', position);
        },
        close: function (type, data) {
            $(this).find("input[type='file']").val(''); $(this).find("input[type='text']").val('');
            $(this).find("input[type='checkbox']").prop('checked', false);
            $("input[type='hidden'].autoopendialog").first().val('');
        }
    });

    $(".dialog.createnewlink .close").click(function () {
        $(this).parents(".createnewlink").first().dialog("close");
        $(this).find("input[type='text']").val('');
        $(this).find("input[type='checkbox']").prop('checked', false);
        $("input[type='hidden'].autoopendialog").val("");
        return false;
    });

    var dlg = $(".dialog.fileupload").dialog({
        appendTo: "form",
        autoOpen: false,
        closeOnEscape: false,
        modal: true,
        width: 800,
        height: 600,
        minHeight: 200,
        minWidth: 300,
        open: function (type, data) {
            $(this).parent().css("top", "50px");
            $(this).parent().css("position", "fixed");
            $(this).find("input[type='file']").val(''); $(this).find("input[type='text']").val('');
            $(this).find("input[type='checkbox']").prop('checked', false);
        },
        resizeStart: function (event, ui) {
            var position = [(Math.floor(ui.position.left) - $(window).scrollLeft()),
                                (Math.floor(ui.position.top) - $(window).scrollTop())];
            $(event.target).parent().css('position', 'fixed');
            $(dlg).dialog('option', 'position', position);
        },
        resizeStop: function (event, ui) {
            var position = [(Math.floor(ui.position.left) - $(window).scrollLeft()),
                                (Math.floor(ui.position.top) - $(window).scrollTop())];
            $(event.target).parent().css('position', 'fixed');
            $(dlg).dialog('option', 'position', position);
        },
        close: function (type, data) {
            if (uploadInProgress()) {
                resetUpload();
            }
            $(this).find("input[type='file']").val(''); $(this).find("input[type='text']").val('');
            $(this).find("input[type='checkbox']").prop('checked', false);
            $("input[type='hidden'].autoopendialog").first().val('');
        }
    });

    $(".dialog.fileupload .close").click(function () {
        if (!uploadInProgress()) {
            $(this).parents(".fileupload").first().dialog("close");
            $(this).find("input[type='text']").val('');
            $(this).find("input[type='checkbox']").prop('checked', false);
            $("input[type='hidden'].autoopendialog").val("");
        }
        else
            resetUpload();
        return false;
    });

    var dlg = $(".dialog.fileversion").dialog({
        appendTo: "form",
        autoOpen: false,
        closeOnEscape: false,
        modal: true,
        width: 800,
        height: 400,
        minHeight: 200,
        minWidth: 300,
        open: function (type, data) {
            $(this).parent().css("top", "50px");
            $(this).parent().css("position", "fixed");
            $(this).find("input[type='file']").val(''); $(this).find("input[type='text']").val('');
            $(this).find("input[type='checkbox']").prop('checked', false);
        },
        resizeStart: function (event, ui) {
            var position = [(Math.floor(ui.position.left) - $(window).scrollLeft()),
                                (Math.floor(ui.position.top) - $(window).scrollTop())];
            $(event.target).parent().css('position', 'fixed');
            $(dlg).dialog('option', 'position', position);
        },
        resizeStop: function (event, ui) {
            var position = [(Math.floor(ui.position.left) - $(window).scrollLeft()),
                                (Math.floor(ui.position.top) - $(window).scrollTop())];
            $(event.target).parent().css('position', 'fixed');
            $(dlg).dialog('option', 'position', position);
        },
        close: function (type, data) {
            if (uploadInProgress()) {
                resetUpload();
            }
            $(this).find("input[type='file']").val(''); $(this).find("input[type='text']").val('');
            $(this).find("input[type='checkbox']").prop('checked', false);
            $("input[type='hidden'].autoopendialog").first().val('');
        }
    });

    $(".dialog.fileversion .close").click(function () {
        if (!uploadInProgress()) {
            $(this).parents(".fileversion").first().dialog("close");
            $(this).find("input[type='text']").val('');
            $(this).find("input[type='checkbox']").prop('checked', false);
            $("input[type='hidden'].autoopendialog").val("");
        }
        else
            resetUpload();
        return false;
    });

   
    function uploadInProgress() {
        if ($t != null) {
            var uploadingRows = $t(".RadAsyncUpload").find(".ruUploadProgress");
                        for (var i = 0; i < uploadingRows.length; i++) {
                if (!$t(uploadingRows[i]).hasClass("ruUploadCancelled") && !$t(uploadingRows[i]).hasClass("ruUploadFailure") && !$t(uploadingRows[i]).hasClass("ruUploadSuccess")) {
                    return true;
                }
            }
        }
        return false;
    }
    function submitUploadWindow() {
        if (!uploadInProgress())
            return true;
        else
            return false;
    }

    function removeUploadedFiles() {
        if ($t != null) {
            var uploadingRows = $t(".RadAsyncUpload").find(".ruUploadProgress");
            $('div.RadAsyncUpload').each(function (index, value) {
                var id = $(value).attr('id');
                uploads.push($find(id));
            });
        }
        return true;
    }


    function fileSelected(sender, args) {
        sender.set_enabled(false);
        $('.ruCancel').prop('disabled', false);
    }
    function fileUploaded(sender, args) {
        sender.set_enabled(true);
    }
   
    $.fn.hasOverflow = function () {
        var $this = $(this);
        return $this[0].scrollHeight > $this.outerHeight() ||
            $this[0].scrollWidth > $this.outerWidth();
    };



    $(".testbread").each(function () {

        var k = $(this).find(".item:not(.first,.last)").size();

        while ($(this).hasOverflow() && k > 0) {
            k--;
            $(this).find(".item:not(.hidden,.first,.last)").first().addClass("hidden");
        }
    });

    if ($('.testbread').size() > 0) {

        $('.testbread').find(".item.hidden").hover(function () {
            clearTimeout($(this).data('timeout'));
            $(this).parents(".testbread").first().find(".item.hover").addClass("hidden").removeClass("hover");
            $(this).removeClass("hidden").addClass("hover");

            var k = $(this).parents(".testbread").first().find(".item:not(.hidden,.first,.last,.hover)").size();

            while ($(this).parents(".testbread").first().hasOverflow() && k > 0) {
                k--;
                $(this).parents(".testbread").first().find(".item:not(.hidden,.first,.last,.hover)").first().addClass("hidden");
            }

        }, function () {
            var $this = $(this);
            var t = setTimeout(function () {
                $this.addClass("hidden").removeClass("hover");

                $this.parents(".testbread").first().find(".item").removeClass("hidden");
                var k = $this.parents(".testbread").first().find(".item:not(.first,.last)").size();

                while ($this.parents(".testbread").first().hasOverflow() && k > 0) {
                    k--;
                    $this.parents(".testbread").first().find(".item:not(.hidden,.first,.last,.hover)").first().addClass("hidden");
                }

            }, 300);
            $(this).data('timeout', t);
        });
    }


    function breadcrumb() {

        $(".breadcrumb").each(function () {

            $(this).find(".item").removeClass("hidden");

            var k = $(this).find(".item:not(.first,.last)").size();


            while ($(this).hasOverflow() && k > 0) {
                k--;
                $(this).find(".item:not(.hidden,.first,.last)").first().addClass("hidden");
            }
          
        });

        if ($('.breadcrumb').size() > 0) {

            $('.breadcrumb').find(".item.hidden").hover(function () {
                clearTimeout($(this).data('timeout'));
                $(this).parents(".breadcrumb").first().find(".item.hover").addClass("hidden");
                $(this).removeClass("hidden").addClass("hover");

                var k = $(this).parents(".breadcrumb").first().find(".item:not(.hidden,.first,.last,.hover)").size();

                while ($(this).parents(".breadcrumb").first().hasOverflow() && k > 0) {
                    k--;
                    $(this).parents(".breadcrumb").first().find(".item:not(.hidden,.first,.last,.hover)").first().addClass("hidden");
                }

            }, function () {
                var $this = $(this);
                var t = setTimeout(function () {
                    $this.addClass("hidden").removeClass("hover");

                    $this.parents(".breadcrumb").first().find(".item").removeClass("hidden");
                    var k = $this.parents(".breadcrumb").first().find(".item:not(.first,.last)").size();

                    while ($this.parents(".breadcrumb").first().hasOverflow() && k > 0) {
                        k--;
                        $this.parents(".breadcrumbwrapper").first().find(".item:not(.hidden,.first,.last,.hover)").first().addClass("hidden");
                    }

                }, 300);
                $(this).data('timeout', t);
            });
        }


    }

    breadcrumb();

    function doSomething() {
        if (jQuery().myProgressBar) {
            $(".progressbar").myProgressBar();

        }
        breadcrumb();
    };

    var resizeTimer;
    $(window).resize(function () {
        clearTimeout(resizeTimer);
        resizeTimer = setTimeout(doSomething, 50);
    });

    $("table.files tbody tr").hover(
            function () {
                $(this).addClass("hoverselected");
            }, function () {
                $(this).removeClass("hoverselected");
            }
        );
    $("table.files tbody tr").click(function () {
        $(this).parents("table.files").first().find(".hoverselected").removeClass("hoverselected");
        $(this).addClass("hoverselected");
    });


    $(".collapsable .expander:not(.treenode .expander)").click(function () {
        $(this).parents(".collapsable").first().toggleClass("collapsed").toggleClass("expanded");
    });

    $(".commands .groupedselector .selectoritem .command").click(function () {


        $(this).parents(".commands").first().children(".command").removeClass("active");
        $(this).parents(".groupedselector").addClass("active");
        $.cookie(filerepository_cookiename + ".viewtype", "custom");
        return false;
    });


    if (jQuery().fancybox) {
        $(".thumbnail a.image").fancybox();
        $(".thumbnail a.video").fancybox({});
        $(".thumbnail a.iframe").fancybox({
            type: "iframe",
            width: "75%",
            height: "75%"
        });
        $("a.lightbox.image").fancybox();
    }

    if (jQuery().tagit) {
        var tgit = $("input.tokeninputtag").tagit({
            removeConfirmation: true,
            triggerKeys: ['enter', 'comma', 'tab'],
            allowSpaces: true,
            /*autocomplete: {
				source: ['tagbello','tagbuono','tagbrutto']
			},*/
            availableTags: filerepository_tags
        });
    }

    $(".defaultservicecontainer.collapsable .itemheader .expander").click(function () {
        $(this).toggleClass("collapsed");
    });

    $(".nestedtree.calcsize .selection input[type='checkbox']").change(function () {
        var size = 0;
        var $tree = $(this).parents(".nestedtree.calcsize").first();
        var $selected = $tree.find(".selection input[type='checkbox']:checked");
        $selected.each(function () {
            var $this = $(this);
            var $parent = $this.parents(".treenode.directory.file");
            var current = $parent.data("size");
            if (current > 0) {
                size += current;
            }

        });

        $tree.data("size", size);

        var $community = $tree.parents(".community.defaultservicecontainer");
        var $sizeinfo = $community.find(".selectedsize");
        $sizeinfo.find(".number").html(formatBytes(size, 2));
        if (size != 0) {
            $sizeinfo.show();
        } else {
            $sizeinfo.hide();
        }

        CheckFileSize();
    });

    function CheckFileSize() {
        var totalsize = 0;

        $(".nestedtree.calcsize").each(function () {
            var size = 0;
            var $tree = $(this);
            var $selected = $tree.find(".selection input[type='checkbox']:checked");
            $selected.each(function () {
                var $this = $(this);
                var $parent = $this.parents(".treenode.directory.file");
                var current = $parent.data("size");
                if (current > 0) {
                    size += current;
                }

            });

            totalsize += size;

            var $community = $tree.parents(".community.defaultservicecontainer");
            var $sizeinfo = $community.find(".selectedsize");
            $sizeinfo.data("size", size);
            $sizeinfo.find(".number").html(formatBytes(size, 2));
            if (size != 0) {
                $sizeinfo.show();
            } else {
                $sizeinfo.hide();
            }
        });

        var $sizeinfo = $(".selectedsize.total");
        $sizeinfo.data("size", totalsize);
        $sizeinfo.find(".number").html(formatBytes(totalsize, 2));
        if (totalsize != 0) {
            $sizeinfo.show();
        } else {
            $sizeinfo.hide();
        }

        var $available = $(".selectedsize.available");
        var aval = $available.data("size");

        if (aval < totalsize) {
            $available.addClass("error");
        } else {
            $available.removeClass("error");
        }
    }

    CheckFileSize();

    function formatBytes(bytes, decimals) {
        if (bytes == 0) return '0 Byte';
        var k = 1000;
        var dm = decimals + 1 || 3;
        var sizes = ['Bytes', 'KB', 'MB', 'GB', 'TB', 'PB', 'EB', 'ZB', 'YB'];
        var i = Math.floor(Math.log(bytes) / Math.log(k));
        return (bytes / Math.pow(k, i)).toPrecision(dm) + ' ' + sizes[i];
    }



    $('.unselectradio input[type="radio"]').each(function () {
        var rel = $(this).data("rel");
        if (rel != "" && rel != "undefined") {
            if ($(this).is(":checked")) {
                $(rel).show();
            } else {
                $(rel).hide();
            }
        }
    })

    $('.unselectradio input[type="radio"]').bind('click', function () {
        $('input[name="' + $(this).attr('name') + '"]').not($(this)).trigger('deselect'); 
        var rel = $(this).data("rel");
        if (rel != "" && rel != "undefined") {
            $(rel).show();
        }
    });

    $('input[type="radio"]').bind('deselect', function () {
        var rel = $(this).data("rel");

        if (rel != "" && rel != "undefined") {
            $(rel).hide();
        }
    })



    $(".fieldrow.textslider div.slider.percentage").slider({
        min: 0,
        max: 100,
        value: 100,
        slide: function (event, ui) {
            $(this).parents(".fieldrow").first().find("input.inputtext").val(ui.value);
        },
        stop: function (event, ui) {
            checkPercent();
        }
    });

    $(".fieldrow.textslider div.slider.percentage").each(function () {

        var min = $(this).data("min");
        var max = $(this).data("max");
        var step = $(this).data("step");
        if (min != "" && typeof min != "undefined") {
            $(this).slider("option", "min", min);
        }
        if (max != "" && typeof max != "undefined") {
            $(this).slider("option", "max", max);
        }
        if (step != "" && typeof step != "undefined") {
            $(this).slider("option", "step", step);
        }

        var value = $(this).slider("value");
        $(this).parents(".fieldrow").first().find("input.inputtext").val(value);
    });


    $(".fieldrow.textslider input.inputtext").change(function () {
        var value = this.value;

        $(this).parents(".fieldrow").first().find("div.slider").slider("value", parseInt(value));
    });

    $(".fieldrow.textslider div.slider.megabytes").slider({
        min: 0,
        max: 1000000000000,
        value: 0,
        step: 1000000,
        slide: function (event, ui) {
            var value = formatBytes(ui.value, 2);
            var values = value.split(" ");
            var number = values[0];
            var measure = values[1];

            $(this).parents(".fieldrow").first().find("input.inputtext").val(number);
            $(this).parents(".fieldrow").first().find(".btnswitch").removeClass("active");
            $(this).parents(".fieldrow").first().find(".btnswitch." + measure.toLowerCase()).addClass("active");


        },
        stop: function (event, ui) {
            setMasterSlaves();
        }
    });

    function setMasterSlaves() {
        var master = $("div.slider.megabytes.master").slider("value");
        $("div.slider.megabytes.slave1").slider("option", "max", master);
        var slave1 = $("div.slider.megabytes.slave1").slider("value");
        if (slave1 > master) {
            slave1 = master;
            $("div.slider.megabytes.slave1").slider("value", slave1);
        }
        var slave2 = master - slave1;
        var slave3 = slave1;
        $("div.slider.megabytes.slave2").slider("option", "max", slave2);

        var max = $("div.slider.megabytes.slave3").data("max");
        if (max != "" && typeof max != "undefined") {
            slave3 = Math.min(slave3, max);
        }
        $("div.slider.megabytes.slave3").slider("option", "max", slave3);

        var quotamaster = $("div.slider.megabytes.quotamaster").slider("value");
        var quotaslave = $("div.slider.megabytes.quotaslave").slider("value");

        var quotamax = $("div.slider.megabytes.quotaslave").data("max");

        if (quotamax != "" && typeof quotamax != "undefined") {

            quotaslave = Math.min(quotamaster, quotamax);

        }

        $("div.slider.megabytes.quotaslave").slider("option", "max", quotaslave);

        checkPercent();

        eachMegabytes(false);
    }

    function checkPercent() {
        var pc1 = $("div.slider.percentage.slave1").slider("value");
        var abs1 = $("div.slider.megabytes.slave1").slider("value");
        var new1 = pc1 * abs1 / 100;
        var pc1text = $("div.slider.percentage.slave1").parents(".fieldrow").first().find(".calcmegabytes .number");
        pc1text.html(formatBytes(new1));



        var pc2 = $("div.slider.percentage.slave2").slider("value");
        var abs2 = $("div.slider.megabytes.slave3").slider("value");
        var new2 = pc2 * abs2 / 100;
        var pc2text = $("div.slider.percentage.slave2").parents(".fieldrow").first().find(".calcmegabytes .number");
        pc2text.html(formatBytes(new2));


        var pc3 = $("div.slider.percentage.quotaslave").slider("value");
        var abs3 = $("div.slider.megabytes.quotamaster").slider("value");
        var new3 = pc3 * abs3 / 100;
        var pc3text = $("div.slider.percentage.quotaslave").parents(".fieldrow").first().find(".calcmegabytes .number");
        pc3text.html(formatBytes(new3));
    }

    function eachMegabytes(check) {
        $(".fieldrow.textslider div.slider.megabytes").each(function () {
            if (check) {
                var min = $(this).data("min");
                var max = $(this).data("max");
                var step = $(this).data("step");
                var value = $(this).data("value");
                if (min != "" && typeof min != "undefined") {
                    $(this).slider("option", "min", min);
                }
                if (max != "" && typeof max != "undefined") {
                    $(this).slider("option", "max", max);
                }
                if (step != "" && typeof step != "undefined") {
                    $(this).slider("option", "step", step);
                }
                if (value != "" && typeof value != "undefined") {
                    $(this).slider("value", value);
                }
            }

            var value1 = $(this).slider("value");
            var value = formatBytes(value1, 2);
            var values = value.split(" ");
            var number = values[0];
            var measure = values[1];

            $(this).parents(".fieldrow").first().find("input.inputtext").val(number);
            $(this).parents(".fieldrow").first().find(".btnswitch").removeClass("active");
            $(this).parents(".fieldrow").first().find(".btnswitch." + measure.toLowerCase()).addClass("active");
            $(this).parents(".fieldrow").first().find("input.hidden").val(value1);

        });
    }

    eachMegabytes(true);
    setMasterSlaves();

    $(".fieldrow.textslider.megabytes .btnswitch:not(.disabled)").click(function () {

        $(this).parents(".btnswitchgroup").first().find(".btnswitch").removeClass("active");
        $(this).addClass("active");

        var value = $(this).parents(".fieldrow").first().find("input").val();
        var intvalue = parseInt(value);
        var active = $(this);
        if (active.is(".mb")) {
            intvalue *= 1000000;
        } else if (active.is(".gb")) {
            intvalue *= 1000000000;
        } else if (active.is(".tb")) {
            intvalue *= 1000000000000;
        }
        $(this).parents(".fieldrow").first().find("div.slider").slider("value", intvalue);
        value = $(this).parents(".fieldrow").first().find("div.slider").slider("value");

        value = formatBytes(value, 2);
        var values = value.split(" ");
        var number = values[0].replace(".00", "");
        var measure = values[1];

        $(this).parents(".fieldrow").first().find("input.inputtext").val(number);
        $(this).parents(".fieldrow").first().find(".btnswitch").removeClass("active");
        $(this).parents(".fieldrow").first().find(".btnswitch." + measure.toLowerCase()).addClass("active");

        setMasterSlaves();
        return false;
    });

    $(".fieldrow.textslider.megabytes input.inputtext").change(function () {
        var value = this.value;
        var intvalue = parseInt(value);
        var active = $(this).parents(".fieldrow").first().find(".btnswitch.active");
        if (active.is(".mb")) {
            intvalue *= 1000000;
        } else if (active.is(".gb")) {
            intvalue *= 1000000000;
        } else if (active.is(".tb")) {
            intvalue *= 1000000000000;
        }
        $(this).parents(".fieldrow").first().find("div.slider").slider("value", intvalue);

        value = $(this).parents(".fieldrow").first().find("div.slider").slider("value");
        //$(this).parents(".fieldrow").first().find("input.hidden").val(value);

        value = formatBytes(value, 2);
        var values = value.split(" ");
        var number = values[0];
        var measure = values[1];

        $(this).parents(".fieldrow").first().find("input.inputtext").val(number);
        $(this).parents(".fieldrow").first().find(".btnswitch").removeClass("active");
        $(this).parents(".fieldrow").first().find(".btnswitch." + measure.toLowerCase()).addClass("active");

        setMasterSlaves();

    });

    $(".coveredradio.enabled").each(function () {
        var $parent = $(this);
        var $checks = $parent.find(".wclist input[type='radio']");
        var $buttons = $parent.find(".btnswitch");
        $checks.each(function () {
            var idx = $(this).index() / 3;
            if ($(this).is(":checked")) {
                $($buttons.get(idx)).addClass("active");
                if ($(this).data("value") == "on") {
                    $parent.addClass("on").removeClass("off");
                } else if ($(this).data("value") == "off") {
                    $parent.addClass("off").removeClass("on");
                }
            } else {
                $($buttons.get(idx)).removeClass("active");
            }
        });

        var $disable = $parent.data("disable");
        if ($disable != "" && typeof $disable != "undefined") {
            if ($parent.is(".on")) {
                $parent.parents($disable).addClass("on").removeClass("off");
            } else {
                $parent.parents($disable).addClass("off").removeClass("on");
            }
        }
    });

    $(".coveredradio.enabled .btnswitch").click(function () {
        var $parent = $(this).parents(".coveredradio").first();
        $parent.find(".active").removeClass("active");
        var $checks = $parent.find(".wclist input[type='radio']");
        if ($parent.is(":not(.readonly)") && $(this).is(":not(.disabled)")) {
            $(this).toggleClass("active");
            var idx = $(this).index();
            $($checks.get(idx)).prop("checked", $(this).is(".active"));
        }
        if ($(this).is(".on")) {
            $parent.addClass("on");
            $parent.removeClass("off");
        } else if ($(this).is(".off")) {
            $parent.addClass("off");
            $parent.removeClass("on");
        }

        var $disable = $parent.data("disable");
        if ($disable != "" && typeof $disable != "undefined") {
            if ($parent.is(".on")) {
                $parent.parents($disable).addClass("on").removeClass("off");
            } else {
                $parent.parents($disable).addClass("off").removeClass("on");
            }
        }

        return false;
    });

    $(".coveredradio.enabled input[type='radio']").click(function () {
        var $parent = $(this).parents(".coveredradio").first();
        $parent.find(".active").removeClass("active");

        var $checks = $parent.find(".btnswitch");

        if ($parent.is(":not(.readonly)")) {
            var idx = $(this).index() / 3;

            if ($(this).is(":checked")) {
                $($checks.get(idx)).addClass("active");
                if ($(this).data("value") == "on") {
                    $parent.addClass("on").removeClass("off");
                } else if ($(this).data("value") == "off") {
                    $parent.addClass("off").removeClass("on");
                }
            } else {
                $($checks.get(idx)).removeClass("active");
            }
        }

        var $disable = $parent.data("disable");
        if ($disable != "" && typeof $disable != "undefined") {
            if ($parent.is(".on")) {
                $parent.parents($disable).addClass("on").removeClass("off");
            } else {
                $parent.parents($disable).addClass("off").removeClass("on");
            }
        }
    });

});