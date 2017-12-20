
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
if (!(typeof $telerik === 'undefined')) {
    var $t = $telerik.$;
}

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

function uploadInProgress() {
    if ($telerik.$ != null) {
        var uploadingRows = $telerik.$(".RadAsyncUpload").find(".ruUploadProgress");
        for (var i = 0; i < uploadingRows.length; i++) {
            if (!$telerik.$(uploadingRows[i]).hasClass("ruUploadCancelled") && !$telerik.$(uploadingRows[i]).hasClass("ruUploadFailure") && !$telerik.$(uploadingRows[i]).hasClass("ruUploadSuccess")) {
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
    $(".toolbar .icon.fileupload").click(function () {
        $(".dialog.fileupload").dialog("open");
        $("input[type='hidden'].autoopendialog").first().val(".dialog.fileupload");
        InitializeUploader();
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
    $(".toolbar .icon.newlink").click(function () {
        $(".dialog.createnewlink").dialog("open");
    });

    var dlg = $(".dialog.createnewfolder").dialog({
        appendTo: "form",
        autoOpen: false,
        closeOnEscape: false,
        modal: true,
        width: 900,
        height: 600,
        minHeight: 200,
        minWidth: 450,
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

        $(".dialog.createnewfolder").dialog("close");
        return false;
    });

    var dlg = $(".dialog.createnewlink").dialog({
        appendTo: "form",
        autoOpen: false,
        closeOnEscape: false,
        modal: true,
        width: 850,
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
        $(".dialog.createnewlink").dialog("close");
        return false;
    });

    var dlg = $(".dialog.fileupload").dialog({
        appendTo: "form",
        autoOpen: false,
        closeOnEscape: false,
        modal: true,
        width: 850,
        height: 530,
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

    $(".openitemversiondlg").click(function () {
        $(".dialog.fileversion").dialog("open");
        return false;
    });
    $(".openthumbnaildlg").click(function () {
        $(".dialog.filethumbnail").dialog("open");
        return false;
    });

    var dlg = $(".dialog.filethumbnail").dialog({
        appendTo: "form",
        autoOpen: false, closeOnEscape: false, modal: true,
        width: 600, height: 300, minHeight: 200, minWidth: 400,
        open: function (type, data) {
            $(this).parent().css("top", "50px");
            $(this).parent().css("position", "fixed");
            $(this).find("input[type='file']").val(''); $(this).find("input[type='text']").val('');
            $(this).find("input[type='checkbox']").prop('checked', false);
        },
        resizeStart: function (event, ui) {
            var position = [(Math.floor(ui.position.left) - $(window).scrollLeft()), (Math.floor(ui.position.top) - $(window).scrollTop())];
            $(event.target).parent().css('position', 'fixed');
            $(dlg).dialog('option', 'position', position);
        },
        resizeStop: function (event, ui) {
            var position = [(Math.floor(ui.position.left) - $(window).scrollLeft()), (Math.floor(ui.position.top) - $(window).scrollTop())];
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

    $(".dialog.filethumbnail .close").click(function () {
        if (!uploadInProgress()) {
            $(this).parents(".fileversion").first().dialog("close");
            $(this).find("input[type='text']").val('');
            $(this).find("input[type='checkbox']").prop('checked', false);
            $("input[type='hidden'].autoopendialog").val("");
        }
        else
            resetUpload();
        $(".dialog.filethumbnail").dialog("close");
        return false;
    });

    $(".openpermissionrolesdlg").click(function () {
        $(".dialog.permissionrolesdlg").dialog("open");
        return false;
    });

    var dlg = $(".dialog.permissionrolesdlg").dialog({
        appendTo: "form",
        autoOpen: false, closeOnEscape: false, modal: true,
        width: 600, height: 300, minHeight: 200, minWidth: 400,
        open: function (type, data) {
            $(this).parent().css("top", "50px");
            $(this).parent().css("position", "fixed");
            $(this).find("input[type='file']").val(''); $(this).find("input[type='text']").val('');
            $(this).find("input[type='checkbox']").prop('checked', false);
        },
        resizeStart: function (event, ui) {
            var position = [(Math.floor(ui.position.left) - $(window).scrollLeft()), (Math.floor(ui.position.top) - $(window).scrollTop())];
            $(event.target).parent().css('position', 'fixed');
            $(dlg).dialog('option', 'position', position);
        },
        resizeStop: function (event, ui) {
            var position = [(Math.floor(ui.position.left) - $(window).scrollLeft()), (Math.floor(ui.position.top) - $(window).scrollTop())];
            $(event.target).parent().css('position', 'fixed');
            $(dlg).dialog('option', 'position', position);
        },
        close: function (type, data) {
            $(this).find("input[type='file']").val(''); $(this).find("input[type='text']").val('');
            $(this).find("input[type='checkbox']").prop('checked', false);
            $("input[type='hidden'].autoopendialog").first().val('');
        }
    });

    $(".dialog.permissionrolesdlg .close").click(function () {
        $(this).find("input[type='checkbox']").prop('checked', false);
        $("input[type='hidden'].autoopendialog").val("");
        $(".dialog.permissionrolesdlg").dialog("close");
        return false;
    });

    $(".openpermissionprofiletypesdlg").click(function () {
        $(".dialog.permissionprofiletypesdlg").dialog("open");
        return false;
    });

    var dlg = $(".dialog.permissionprofiletypesdlg").dialog({
        appendTo: "form",
        autoOpen: false, closeOnEscape: false, modal: true,
        width: 600, height: 300, minHeight: 200, minWidth: 400,
        open: function (type, data) {
            $(this).parent().css("top", "50px");
            $(this).parent().css("position", "fixed");
            $(this).find("input[type='file']").val(''); $(this).find("input[type='text']").val('');
            $(this).find("input[type='checkbox']").prop('checked', false);
        },
        resizeStart: function (event, ui) {
            var position = [(Math.floor(ui.position.left) - $(window).scrollLeft()), (Math.floor(ui.position.top) - $(window).scrollTop())];
            $(event.target).parent().css('position', 'fixed');
            $(dlg).dialog('option', 'position', position);
        },
        resizeStop: function (event, ui) {
            var position = [(Math.floor(ui.position.left) - $(window).scrollLeft()), (Math.floor(ui.position.top) - $(window).scrollTop())];
            $(event.target).parent().css('position', 'fixed');
            $(dlg).dialog('option', 'position', position);
        },
        close: function (type, data) {
            $(this).find("input[type='file']").val(''); $(this).find("input[type='text']").val('');
            $(this).find("input[type='checkbox']").prop('checked', false);
            $("input[type='hidden'].autoopendialog").first().val('');
        }
    });

    $(".dialog.permissionprofiletypesdlg .close").click(function () {
        $(this).find("input[type='checkbox']").prop('checked', false);
        $("input[type='hidden'].autoopendialog").val("");
        $(".dialog.permissionprofiletypesdlg").dialog("close");
        return false;
    });


    $(".openpermissionusersdlg").click(function () {
        $(".dialog.permissionusersdlg").dialog("open");
        return false;
    });

    var dlg = $(".dialog.permissionusersdlg").dialog({
        appendTo: "form",
        autoOpen: false, closeOnEscape: false, modal: true,
        width: 900, height: 650, minHeight: 450, minWidth: 700,
        open: function (type, data) {
            $(this).parent().css("top", "50px");
            $(this).parent().css("position", "fixed");
            $(this).find("input[type='file']").val(''); $(this).find("input[type='text']").val('');
            $("input[type='hidden'].autoopendialog").val(".permissionusersdlg");
            $(this).find("input[type='checkbox']").prop('checked', false);
        },
        resizeStart: function (event, ui) {
            var position = [(Math.floor(ui.position.left) - $(window).scrollLeft()), (Math.floor(ui.position.top) - $(window).scrollTop())];
            $(event.target).parent().css('position', 'fixed');
            $(dlg).dialog('option', 'position', position);
        },
        resizeStop: function (event, ui) {
            var position = [(Math.floor(ui.position.left) - $(window).scrollLeft()), (Math.floor(ui.position.top) - $(window).scrollTop())];
            $(event.target).parent().css('position', 'fixed');
            $(dlg).dialog('option', 'position', position);
        },
        close: function (type, data) {
            $(this).find("input[type='file']").val(''); $(this).find("input[type='text']").val('');
            $(this).find("input[type='checkbox']").prop('checked', false);
            $("input[type='hidden'].autoopendialog").first().val('');
        }
    });

    $(".dialog.openpermissionusersdlg .close").click(function () {
        $(this).find("input[type='checkbox']").prop('checked', false);
        $("input[type='hidden'].autoopendialog").val("");
        $(".dialog.permissionusersdlg").dialog("close");
        return false;
    });



    $('.dialog.dlgapplyfolder').dialog({
        appendTo: "form",
        closeOnEscape: false,
        autoOpen: true,
        draggable: true,
        modal: true,
        title: "",
        width: 820,
        height: 400,
        minHeight: 200,
        //                minWidth: 700,
        zIndex: 1000,
        open: function (type, data) {
            $(".ui-dialog-titlebar-close", this.parentNode).hide();
        }

    });

    $(".closedlgapplyfolder").click(function () {
        $("input[type='hidden'].autoopendialog").val("");
        $(".dialog.dlgapplyfolder").dialog("close");
        return false;
    });

    
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
});