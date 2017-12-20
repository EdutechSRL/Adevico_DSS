$(function () {
    $(".groupedselector .selectoricon, .groupedselector .selectorlabel").click(function () {
        var $group = $(this).parents(".groupedselector").first();

        $(".groupedselector").not($group).removeClass("clicked");
        $group.toggleClass("clicked");

    });

    $(".groupedselector .selectoritem").click(function () {
        var $group = $(this).parents(".groupedselector").first();
        $group.removeClass("clicked");

        $group.find(".selectoritem").removeClass("active");
        $(this).addClass("active");

        $group.find(".selectorgroup .selectorlabel").html($(this).find(".selectorlabel").html());

        $(this).find("a").click();

    });

    $(".groupedselector").mouseout(function () {
        //$(this).removeClass("clicked");
    });
    $("a.noticeboard").fancybox({
        type: "iframe",
        width: "75%",
        height: "75%"
    });

    $('.print').click(function () {
        $("iframe").get(0).contentWindow.focus();
        $("iframe").get(0).contentWindow.print();

        /*window.frames["frameright"].focus();
         window.frames["frameright"].print();*/
    });

    $(".collapsable").each(function () {
        var id = $(this).data("id");
        var cookie = $.cookie("collapsed-" + id);
        if (cookie != null)
        {
            if(cookie == "true")
            {
                $(this).addClass("collapsed");
            }else
            {                
                $(this).removeClass("collapsed");                
            }
        }        
    })

    $(".collapsable .expander").click(function () {
        var $collapsable=$(this).parents(".collapsable").first();
        $collapsable.toggleClass("collapsed");

        var id = $collapsable.data("id");
        $.cookie("collapsed-" + id, $collapsable.is(".collapsed"), { expires: 1 });
    });

    
    $(".needconfirm-unsubscribefromcommunity").needConfirm({
        // customize as you want...
        msgFunction: function (item) { return ConfirmMsg(item, "community", "unsubscribe") },
        addConfirmClass: true
    });

    $(".dialog .close").click(function () {
        $(this).parents(".dialog").first().dialog("close");
        return false;
    });

    $(".hiddendialog .close").click(function () {
        $(this).parents(".hiddendialog").first().dialog("close");
        $("input[type='hidden'].autoopendialog").val("");
        return false;
    });

    $(".dlgconfirmsubscription .fieldobject.subscription .seemore").click(function () {

        $(this).parents(".fieldobject.subscription").first().toggleClass("expanded");
    });
})