$(function () {    
    $(".needconfirm").needConfirm({
        msgFunction: function (item) { return ConfirmMsg(item) },
        addConfirmClass: true
    });


    $(".needconfirm-custom").needConfirm({
        // customize as you want...
        msgFunction: function (item) { return ConfirmMsg(item) },
        addConfirmClass: true
    });
});