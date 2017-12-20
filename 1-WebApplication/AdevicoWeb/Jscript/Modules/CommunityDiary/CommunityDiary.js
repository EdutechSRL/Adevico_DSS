$(function () {
    $(".ddbuttonlist.enabled").dropdownButtonList();
    $(".print").click(function () {
        window.print();
        return false;
    });
});


function showDialog(id) {
    //            if ($.browser.msie && id =='addField') {
    //                $('#' + id).dialog("option", "height", 870);
    //            }
    //            else if ($.browser.msie && id =='addRequestedFile') {
    //                $('#' + id).dialog("option", "height", 350);
    //            }

    $('#' + id).dialog("open");
}

function closeDialog(id) {
    $('#' + id).dialog("close");
}