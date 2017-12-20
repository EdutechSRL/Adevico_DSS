$(function () {
    $(".ddbuttonlist").dropdownButtonList();

    $('.command.print').click(function () {
        $("iframe").get(0).contentWindow.focus();
        $("iframe").get(0).contentWindow.print();

        /*window.frames["frameright"].focus();
        window.frames["frameright"].print();*/
    });
    $('.print').click(function () {
        $("iframe").get(0).contentWindow.focus();
        $("iframe").get(0).contentWindow.print();
    });
})