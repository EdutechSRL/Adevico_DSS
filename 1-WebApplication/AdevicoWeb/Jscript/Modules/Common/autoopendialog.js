$(function () {
    var dialog = "";
    dialog = $("input[type='hidden'].autoopendialog:not(.disabled)").val();
    if (dialog != "" && dialog != "." && dialog != "#") {
        $(dialog).dialog("open");
    }
});