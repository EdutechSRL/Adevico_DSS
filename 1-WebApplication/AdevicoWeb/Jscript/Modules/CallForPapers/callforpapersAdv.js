// DOM Ready
$(function () {
    $(".fieldsAdv").sortable({
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
            $(this).sortable("refresh");
            $(ui.item).addClass("dragging");
        },
        stop: function (event, ui) {
            $(ui.item).removeClass("dragging");
        }
    });
   
});