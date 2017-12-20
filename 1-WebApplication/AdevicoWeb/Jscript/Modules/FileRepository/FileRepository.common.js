var titleUploadtomoduleitem = "";
var titleUploadtomoduleitemandcommunity = "";
var titleLinkfromcommunity = "";
try {
    titleUploadtomoduleitem = title_uploadtomoduleitem;
}
catch (e) {
    if (e.name == "ReferenceError") {
        titleUploadtomoduleitem = "";
    }
}
try {
    titleUploadtomoduleitemandcommunity = title_uploadtomoduleitemandcommunity;
}
catch (e) {
    if (e.name == "ReferenceError") {
        titleUploadtomoduleitemandcommunity = "";
    }
}
try {
    titleLinkfromcommunity = title_linkfromcommunity;
}
catch (e) {
    if (e.name == "ReferenceError") {
        titleLinkfromcommunity = "";
    }
}



var resetUpload = function () {
    var current = $(this);
    try {
        $telerik.$('.RadAsyncUpload').each(function (index, value) {
            var id = $(value).attr('id');
            var upload = $find(id);
            try {
                var uploaded = upload.getUploadedFiles();
                for (i = uploaded.length - 1; i >= 0; i--) {
                    upload.deleteFileInputAt(i);
                }
            }
            catch (uploadError) {

            }
            try {
                var uploadingRows = $telerik.$(".RadAsyncUpload").find(".ruUploadProgress");
                for (var i = 0; i < uploadingRows.length; i++) {
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

function InitializeUploader() {
 /* for future use ? */
}

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
    $(".dropdown").each(function () {
        var $dropdown = $(this);
        $dropdown.find("input[type='hidden'].changetrigger").change(function () { $dropdown.find(".folderapply").click(); });
    });

    if (jQuery().dropdownList) {
        
        $(".dropdown.enabled").dropdownList({ changeWidth: true });
    }

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

    $(".openitemversiondlg").click(function () {
        $(".dialog.fileversion").dialog("open");
        return false;
    });

    $(".opendlguploadtomoduleitem").click(function () {
        $(".dialog.dlguploadtomoduleitem").dialog("open");
        $("input[type='hidden'].autoopendialog").first().val(".dialog.dlguploadtomoduleitem");
        InitializeUploader();
    });
    var dlg = $(".dialog.dlguploadtomoduleitem").dialog({
        appendTo: "form",
        autoOpen: false,
        closeOnEscape: false,
        modal: true,
        width: 800,
        height: 300,
        minHeight: 200,
        minWidth: 400,
        title: titleUploadtomoduleitem,
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

    $(".dialog.dlguploadtomoduleitem .close").click(function () {
        if (!uploadInProgress()) {
            $(this).parents(".dlguploadtomoduleitem").first().dialog("close");
            $(this).find("input[type='text']").val('');
            $(this).find("input[type='checkbox']").prop('checked', false);
            $("input[type='hidden'].autoopendialog").val("");
        }
        else {
            resetUpload();
        }
        return false;
    });

    $(".opendlguploadtomoduleitemandcommunity").click(function () {
        $(".dialog.dlguploadtomoduleitemandcommunity").dialog("open");
        $("input[type='hidden'].autoopendialog").first().val(".dialog.dlguploadtomoduleitemandcommunity");
        InitializeUploader();
    });
    var dlg = $(".dialog.dlguploadtomoduleitemandcommunity").dialog({
        appendTo: "form",
        autoOpen: false,
        closeOnEscape: false,
        modal: true,
        width: 800,
        height: 500,
        minHeight: 200,
        minWidth: 400,
        title: titleUploadtomoduleitemandcommunity,
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

    $(".dialog.dlguploadtomoduleitemandcommunity .close").click(function () {
        if (!uploadInProgress()) {
            $(this).parents(".dlguploadtomoduleitemandcommunity").first().dialog("close");
            $(this).find("input[type='text']").val('');
            $(this).find("input[type='checkbox']").prop('checked', false);
            $("input[type='hidden'].autoopendialog").val("");
        }
        else {
            resetUpload();
        }
        return false;
    });


    $(".opendlglinkfromcommunity").click(function () {
        $(".dlglinkfromcommunity").dialog("open");

        return false;
    });
    var dlg = $(".dialog.dlglinkfromcommunity").dialog({
        appendTo: "form",
        autoOpen: false,
        closeOnEscape: false,
        modal: true,
        width: 800,
        height: 500,
        minHeight: 400,
        minWidth: 200,
        title: titleLinkfromcommunity,
        open: function (type, data) {
            $(this).parent().children().children('.ui-dialog-titlebar-close').hide();

            $(this).parent().css("top", "50px");
            $(this).parent().css("position", "fixed");
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
            $(this).find("input[type='checkbox']").prop('checked', false);
            $("input[type='hidden'].autoopendialog").first().val('');
        }
    });

    $(".opendlglinkfromcommunity").click(function () {
        $(".dlglinkfromcommunity").dialog("open");

        return false;
    });
    $(".dialog.dlglinkfromcommunity .close").click(function () {
        $(this).find("input[type='checkbox']").prop('checked', false);
        $("input[type='hidden'].autoopendialog").val("");
        $(this).parents(".dlglinkfromcommunity").first().dialog("close");
        return false;
    });
 

    function uploadInProgress() {
        if (!(typeof $telerik === 'undefined')) {
            $t = $telerik.$;
        }
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
});