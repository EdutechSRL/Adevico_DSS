function UpdateTableHeaders() {
    $(".persist-area").each(function () {
        var el = $(this),
            offset = el.offset(),
            scrollTop = $(window).scrollTop(),
            floatingHeader = $(".floatingHeader", this)

        if ((scrollTop > offset.top) && (scrollTop < offset.top + el.height())) {
            $(".persist-header").addClass("floatingHeader");
            el.addClass("hasFloating");
        } else {
            $(".persist-header").removeClass("floatingHeader");
            el.removeClass("hasFloating");
        };
    });
}

function getHeight() {
    var myWidth = 0, myHeight = 0;
    if (typeof (window.innerWidth) == 'number') {
        //Non-IE
        myWidth = window.innerWidth;
        myHeight = window.innerHeight;
    } else if (document.documentElement && (document.documentElement.clientWidth || document.documentElement.clientHeight)) {
        //IE 6+ in 'standards compliant mode'
        myWidth = document.documentElement.clientWidth;
        myHeight = document.documentElement.clientHeight;
    } else if (document.body && (document.body.clientWidth || document.body.clientHeight)) {
        //IE 4 compatible
        myWidth = document.body.clientWidth;
        myHeight = document.body.clientHeight;
    }

    return myHeight;
}

var waitForFinalEvent = (function () {
    var timers = {};
    return function (callback, ms, uniqueId) {
        if (!uniqueId) {
            uniqueId = "Don't call this twice without a uniqueId";
        }
        if (timers[uniqueId]) {
            clearTimeout(timers[uniqueId]);
        }
        timers[uniqueId] = setTimeout(callback, ms);
    };
})();

function UpdateLength() {
    waitForFinalEvent(function () {
        $(".persist-header").each(function () {
            var height = $(this).height();
            var windowHeight = getHeight();
            if (height > windowHeight) {
                $(this).addClass("tooLong");
            } else {
                $(this).removeClass("tooLong");
            }
        });
    }, 150, "resize");
}

// DOM Ready
$(function () {
    var clonedHeaderRow;
    $(window)
        .scroll(UpdateTableHeaders)
        .trigger("scroll")
        .resize(UpdateLength)
        .trigger("resize");




    var timeout = null;
    var clear = function () {
        if (timeout) {
            clearTimeout(timeout);
            timeout = null;
        }
    }


//  REMOVED FROM SOLUTION VS2010
//    $(".dialog").dialog({
//        //appendTo:"form",
//        width: 400,
//        autoOpen: false
//        /*buttons: { "Ok": function () { $(this).dialog("close"); }, "Cancel": function () { $(this).dialog("close"); } }*/
//    });

    $("body").simpleEqualize();


});

$(function(){
   $("table .headercheckbox input[type='checkbox']").change(function(){
                var $checkbox=$(this);
                var $table=$checkbox.parents("div.tablewrapper table");
                var ischecked=$checkbox.is(":checked");
                var $rows = $table.children("tbody").children("tr").find(".submittercheckbox input[type='checkbox']").attr("checked",ischecked);
            });

            $("table .submittercheckbox input[type='checkbox']").change(function(){
                var $checkbox=$(this);
                var $table=$checkbox.parents("div.tablewrapper table");
                var checked = $table.find(".submittercheckbox input[type='checkbox']:checked").size();
                var total = $table.find(".submittercheckbox input[type='checkbox']").size();

                if(total!=checked)
                {
                    $table.find(".headercheckbox input[type='checkbox']").attr("checked",false);
                }else
                {
                    $table.find(".headercheckbox input[type='checkbox']").attr("checked",true);
                }

            });

            $(".table th input[type='checkbox']").change(function(){
                var $this=$(this);
                $(this).parents("table").first().find("td input[type='checkbox']").prop("checked",$this.is(":checked"));

                var $el;
                var $elout;

                var ultrafast = 1;
                var fast = 200;
                var slow = 3000;

                if($this.is(":checked"))
                {
                    $el = $this.siblings(".selectorpopup.checkall");
                    $elout = $this.siblings(".selectorpopup.checknone");
                }else
                {
                    $el = $this.siblings(".selectorpopup.checknone");
                    $elout = $this.siblings(".selectorpopup.checkall");
                }

                if($el.size()>0){
                    $el.fadeIn(fast).addClass("open");
                    var ovt = setTimeout(function(){$el.fadeOut(fast,function(){$el.removeClass("open");}); clearTimeout(ovt);},slow);
                }
                if($elout.size()>0){
                    $elout.fadeOut(ultrafast, function(){$elout.removeClass("open");});
                }

            });
                $("table.expandable").each(function(){
                var $table = $(this);
                var max = $table.data("max")-1;



                $table.find("tbody > tr").removeClass("hidden");
                $table.find("tbody > tr:gt("+max+")").addClass("hidden");

                var $showextra = $table.find("tfoot .showextra:not(.first)");
                var $hideextra = $table.find("tfoot .hideextra:not(.last)");

                if($showextra.size()>0){
                    $showextra.html( $showextra.html().replace("{0}",$table.find("tbody > tr").size()));
                }
                if($hideextra.size()>0){
                    $hideextra.html( $hideextra.html().replace("{0}",max+1));
                }

                if ($table.find("tbody > tr").size()<=max)
                {
                    $table.find("tfoot").hide();
                }else
                {
                    $table.find("tfoot").show();
                }

            });

            $("table.expandable tfoot .showextra").click(function(){
                var $table = $(this).parents("table.expandable").first();
                $table.removeClass("compressed");
            });

            $("table.expandable tfoot .hideextra").click(function(){
                var $table = $(this).parents("table.expandable").first();
                $table.addClass("compressed");
            });

    $(".addprofile").hover(function(){
                $("ul.navlist li.nav.profile").switchClass("","highlighted",100);
            },function(){
                $("ul.navlist li.nav.profile").switchClass("highlighted","",500);
            });
            $(".addcommunity").hover(function(){
                $("ul.navlist li.nav.community").switchClass("","highlighted",100);
            },function(){
                $("ul.navlist li.nav.community").switchClass("highlighted","",500);
            });
            $(".adduser").hover(function(){
                $("ul.navlist li.nav.user").switchClass("","highlighted",100);
            },function(){
                $("ul.navlist li.nav.user").switchClass("highlighted","",500);
            });
});


//  REMOVED FROM SOLUTION VS2010 ???
$(function () {
//    var dlg = $(".dialog").dialog({
//        //appendTo:"form",
//        autoOpen: false,
//        modal: false,
//        open: function (type, data) {
//            $(this).data("open", true);
//            $(this).parent().appendTo("form");
//        },
//        close: function (event, ui) {
//            var name = $(this).data("name");
//            var old = $(this).data("old");
//            var value = $(this).data("value");
//            var element = $(this).data("element");

//            $(this).data("open", false);

//            if ($(element).is(".RadPicker")) {

//                var id = $(element).children().attr("id");
//                var obj = $find(id);

//                obj.set_selectedDate(new Date(old));
//                //obj.clear();

//            } else {
//                $(element).val(old);
//            }

//        }
//    });

//    $(".coherencecheck").not(".RadPicker").each(function () {
//        $(this).data("value", $(this).val());
//    });

//    $(".coherencecheck.RadPicker").each(function () {
//        var value = $(this).find("input[type='text']").val();


//        $(this).data("value", new Date(value).toDateString());
//    });


//    $(".RadPicker.coherencecheck").change(function () {

//        var $this = $(this);
//        var name = $this.data("name");
//        var old = $this.data("value");
//        var id = $(this).children().attr("id");

//        var obj = $find(id);

//        var value = new Date(obj.get_selectedDate().toString()).toDateString();

//        var dlg = $this.data("dlg");

//        $(dlg).data("element", $this);
//        $(dlg).data("name", name);
//        $(dlg).data("value", value);
//        $(dlg).data("old", old);

//        if (old != value) {

//            $(dlg).dialog("open");
//        }

//    });

//    $(".coherencecheck").blur(function () {

//        var name = $(this).data("name");
//        var old = $(this).data("value");
//        var value = $(this).val();
//        var dlg = $(this).data("dlg");

//        $(dlg).data("element", $(this));
//        $(dlg).data("name", name);
//        $(dlg).data("value", value);
//        $(dlg).data("old", old);

//        if (old != value) {

//            $(dlg).dialog("open");
//        }

//    });

    $(".dialog .close").click(function () {
        $(this).parents(".dialog").first().dialog("close");
        return false;
    });



    $(".chzn-select").chosen({});

    $(".ddbuttonlist.enabled").dropdownButtonList();



    $("table.treetable.taskmap").treeTable({ initialState: "expanded", treeColumn: 1 });

    $("table.treetable.projects").treeTable({ initialState: "expanded" });

    $("table.treetable").treeTable({ initialState: "expanded" });

    $(".coveredcheck.enabled").each(function () {
        var $parent = $(this);
        var $checks = $parent.find(".wclist input[type='checkbox']");
        var $buttons = $parent.find(".btnswitch");
        $checks.each(function () {
            var idx = $(this).index() / 3;
            if ($(this).is(":checked")) {
                $($buttons.get(idx)).addClass("active");
            } else {
                $($buttons.get(idx)).removeClass("active");
            }
        });
    });

    $(".coveredcheck.enabled .btnswitch").click(function () {
        var $parent = $(this).parents(".coveredcheck").first();

        var $checks = $parent.find(".wclist input[type='checkbox']");
        //console.log($checks.size());
        if ($parent.is(":not(.readonly)") && $(this).is(":not(.disabled)")) {
            $(this).toggleClass("active");
            var idx = $(this).index();
            //console.log(idx);
            //console.log($(this).is(".active"));
            $($checks.get(idx)).prop("checked", $(this).is(".active"));
            //console.log($($checks.get(idx)).prop("checked"));
        }
        return false;
    });

    $(".coveredcheck.enabled input[type='checkbox']").click(function () {
        var $parent = $(this).parents(".coveredcheck").first();

        var $checks = $parent.find(".btnswitch");

        if ($parent.is(":not(.readonly)")) {
            //$(this).toggleClass("active");
            var idx = $(this).index() / 3;

            if ($(this).is(":checked")) {
                $($checks.get(idx)).addClass("active");
            } else {
                $($checks.get(idx)).removeClass("active");
            }
        }

    });

    $(".coveredradio.enabled").each(function () {
        var $parent = $(this);
        var $checks = $parent.find(".wclist input[type='radio']");
        var $buttons = $parent.find(".btnswitch");
        $checks.each(function () {
            var idx = $(this).index() / 3;
            if ($(this).is(":checked")) {
                $($buttons.get(idx)).addClass("active");
            } else {
                $($buttons.get(idx)).removeClass("active");
            }
        });
    });

    $(".coveredradio.enabled .btnswitch").click(function () {
        var $parent = $(this).parents(".coveredradio").first();


        $parent.find(".btnswitch").removeClass("active");

        var $checks = $parent.find(".wclist input[type='radio']");
        //console.log($checks.size());
        if ($parent.is(":not(.readonly)") && $(this).is(":not(.disabled)")) {
            $(this).toggleClass("active");
            var idx = $(this).index();
            //console.log(idx);
            //console.log($(this).is(".active"));
            $($checks.get(idx)).prop("checked", $(this).is(".active"));
            //console.log($($checks.get(idx)).prop("checked"));
        }
        return false;
    });

    $(".coveredradio.enabled input[type='radio']").click(function () {
        var $parent = $(this).parents(".coveredradio").first();

        var $checks = $parent.find(".btnswitch");

        if ($parent.is(":not(.readonly)")) {
            //$(this).toggleClass("active");
            var idx = $(this).index() / 3;

            if ($(this).is(":checked")) {
                $($checks.get(idx)).addClass("active");
            } else {
                $($checks.get(idx)).removeClass("active");
            }
        }

    });


    $(".weekcalendar.enabled").each(function () {
        var $parent = $(this);
        var $checks = $parent.find(".wclist input[type='checkbox']");
        var $buttons = $parent.find(".btnswitch");
        $checks.each(function () {
            var idx = $(this).index() / 3;
            if ($(this).is(":checked")) {
                $($buttons.get(idx)).addClass("active");
            } else {
                $($buttons.get(idx)).removeClass("active");
            }
        });
    });

    $(".weekcalendar.enabled .btnswitch").click(function () {
        var $parent = $(this).parents(".weekcalendar").first();

        var $checks = $parent.find(".wclist input[type='checkbox']");
        //console.log($checks.size());
        if ($parent.is(":not(.readonly)") && $(this).is(":not(.disabled)")) {
            $(this).toggleClass("active");
            var idx = $(this).index();
            //console.log(idx);
            //console.log($(this).is(".active"));
            $($checks.get(idx)).prop("checked", $(this).is(".active"));
            //console.log($($checks.get(idx)).prop("checked"));
        }
        return false;
    });

    $(".weekcalendar.enabled input[type='checkbox']").click(function () {
        var $parent = $(this).parents(".weekcalendar").first();

        var $checks = $parent.find(".btnswitch");

        if ($parent.is(":not(.readonly)")) {
            //$(this).toggleClass("active");
            var idx = $(this).index() / 3;

            if ($(this).is(":checked")) {
                $($checks.get(idx)).addClass("active");
            } else {
                $($checks.get(idx)).removeClass("active");
            }
        }

    });




    //    $("div.tabs").tabs();

    /*temp*/
    $("table.taskmap .resourceslist").click(function () {

        var $row = $(this).parents("tr");

        var id = $(this).parents("tr").first().attr("id");

        var name = $row.find("td.name span.editable .edit input[type='text']").val();

        $(".dialog.dlgresources").find("input[type='hidden']").val(id);

        $(".dialog.dlgresources").find(".taskname .task").html(name);

        $(".dialog.dlgresources").dialog("open");

    });

    $(".dialog.dlgresources").dialog({
        appendTo: "form",
        autoOpen: false,
        width: 600,
        height: 400,
        open: function () {
            //$(this).find(".chzn-select").chosen({});
            var $dialog = $(this);
            $(this).find("select option").prop("selected",false);
            $(this).find(".choseselect").find(".chzn-select").trigger("liszt:updated");
            $(this).find(".choseselect").find(".chzn-select").trigger("change");

            var id = $(this).find("input[type='hidden']").first().val();

            var $row = $("table.taskmap tr#"+id);


            $row.find(".resource").each(function(){
                var id = $(this).data("id");

                $dialog.find("select.resources option[value='"+id+"']").prop("selected",true);

                $dialog.find(".chzn-select option[value='"+id+"']").prop("selected", true);
                $dialog.find(".chzn-select").trigger("liszt:updated");
                $dialog.find(".chzn-select").trigger("change");
            });

        }
    });


    $(".dlgedit div.tabs").tabs({
        activate: function( event, ui ) {
            $(this).parents(".dlgedit").find("input.selectedtab").val(ui.newTab.index());
        }
    });

    $(this).find("div.tabs").tabs({

    });
    //  REMOVED FROM SOLUTION VS2010
//    /*Fix Asp.net*/
//    $(".dlgedit").dialog({
//        modal: true,
//        /*width:"auto",
//        height:"auto",*/
//        width: 700,
//        height: 500,
//        open: function () {
//            //$(this).find(".chzn-select").chosen({});
//            //$(this)
//            var val = $(this).find("input.selectedtab").first().val();
//            if(val!=""){
//                $(this).find("div.tabs").tabs( "option", "active", val );
//            }

//            var $selres = $(this).find(".chzn-select.resources");
//            var $options = $selres.find("option");
//            var $tableres=$(this).find(".table.resourcescompletion");
//            $options.each(function(){
//                var id = $(this).val();
//                if($(this).is(":selected"))
//                {
//                    $tableres.find("tr#"+id).removeClass("disabled");
//                }else
//                {
//                    $tableres.find("tr#"+id).addClass("disabled");
//                }
//            });
//        }
//    });

    $(".dlgedit .chzn-select.resources").change(function(){
        var $options = $(this).find("option");
        var $tableres=$(this).parents(".dlgedit").find(".table.resourcescompletion");
        $options.each(function(){
            var id = $(this).val();
            if($(this).is(":selected"))
            {
                $tableres.find("tr#"+id).removeClass("disabled").show();
            }else
            {
                $tableres.find("tr#"+id).addClass("disabled").hide();
            }
        });
    });


    $(".dialog.dlglinkshelp").dialog({
        appendTo: "form",
        autoOpen: false,
        width: "auto",
        height: "auto",
        open: function () {
            //$(this).find(".chzn-select").chosen({});
        }
    });

    $(".dialog.dlgprojectresources").dialog({
        //appendTo:"form",
        modal: true,
        width: 600,
        height: 450,
        open: function () {
            //$(this).find(".chzn-select").chosen({});
            //$(this)
        }
    })
    

    $(".dialog.dlgprojectattachments").dialog({
        //appendTo:"form",
        autoOpen: false,
        modal: true,
        width: 600,
        height: 450,
        open: function () {
            //$(this).find(".chzn-select").chosen({});
            //$(this)
        },
        close: function (event, ui) {
            $("input[type='hidden'].autoopendialog").val("");
        }
    })
   
    $("span.editable:not(.disabled)").each(function () {
        var $editable = $(this);
        var $edit = $editable.children(".edit");
        var $inputh = $edit.find("input[type='hidden']").first();
        /*console.log($inputh);
        console.log($inputh.size());
        console.log($inputh.val());*/
       /* if ($inputh.val() != "init") {
            alert($inputh.attr('id'));
            alert($inputh.val());
        }*/
        if ($inputh.val() == "edit") {
            $(this).removeClass("viewmode").addClass("editmode");
            $inputh.val("edit");
        } else if ($inputh.val() == "") {
            $(this).addClass("viewmode").removeClass("editmode");
            $inputh.val("");
        }
     /*   if ($inputh.val() != "init")
            alert($inputh.val());*/
    });

    $("span.editable:not(.disabled) .view").click(function () {
        var $view = $(this);
        var $editable = $(this).parents("span.editable").first();
        var $edit = $editable.children(".edit");
        var $input = $edit.find("input[type='text']");
        $input.val($(this).html().replace("&nbsp;", ""));
        $editable.removeClass("viewmode").addClass("editmode");
        $editable.find("input[type='hidden']").val("edit");
        $input.focus();
        //$(".editablehelp").removeClass("hidden");
    });

    $("span.editable:not(.disabled) .edit .icon.cancel").click(function () {
        //$(".editablehelp").addClass("hidden");
        $(this).parents("span.editable").first().removeClass("editmode").addClass("viewmode");
        $(this).parents("span.editable").first().find("input[type='hidden']").val("");
        $(this).parents(".error,.linkserror").removeClass("error").removeClass("linkserror");
    });

    $("span.editable:not(.disabled) .edit .icon.ok").click(function () {
        //$(".editablehelp").addClass("hidden");

        /*$.blockUI({ message: '<h1>Just a moment...</h1>' });

        setTimeout(function(){
        $.unblockUI;
        $(".savesubmit").click();
        }, 200);*/

        $(".savesubmit").click();
    });

    /*$("form").submit(function(){
    $(this).parents("span.editable").first().removeClass("editmode").addClass("viewmode");
    $.blockUI({ message: '<h1><img src="busy.gif" /> Just a moment...</h1>' });

    setTimeout($.unblockUI, 2000);
    });*/

    $("span.editable:not(.disabled) .edit input").keyup(function (e) {

        var $editable = $(this).parents("span.editable").first();
        var $edit = $editable.children(".edit");

        if (e.which == 13) { $editable.find('.ok').click(); }

        if (e.which == 27) { $editable.find('.cancel').click(); }

    });

    $("span.editable:not(.disabled) .edit .icon").click(function () {
        $(this).parents("span.editable").first().removeClass("editmode").addClass("viewmode");
    });

    /*temp*/
    $(".projectresources").click(function () {
        $(".dialog.dlgprojectresources").dialog("open");
        return false;
    });

    $("table.users tr.user .delete").click(function () {
        $(".dialog.dlgdelete").dialog("open");
    });

    /*temp*/
    $(".customresource").click(function () {
        $(".dialog.dlgcustomresource").dialog("open");
        return false;
    });

    $(".links .openlegend").click(function () {
        $(".dialog.dlglinkshelp").dialog("open");
        return false;
    });


    //  REMOVED FROM SOLUTION VS2010
//    /*temp*/
//    $("table.table.taskmap tr.task .listwrapper .edit").click(function () {
//        $(".dlgedit").dialog("open");
//        return false;
//    });


    /*end temp*/

    /*$("table.table.taskmap tr.task:not(.actions) *:not(.expander)").click(function(){
    var $row = $(this).parents("tr.task").first();
    var $table = $(this).parents("table.table.taskmap").first();
    var $actionrow = $table.find("tr.task.actions");
    var $actionrowhidid=$actionrow.find(".idhid");

    $actionrowhidid.val($row.attr("id"));
    $table.find("tr.task.selectedrow").removeClass("selectedrow");
    $row.addClass("selectedrow");

    $row.after($actionrow.removeClass("invisible"));
    });*/

    /*temp*/
    /*$("table.table.taskmap tr.task:not(.actions) td.id").dblclick(function(){
    $(".dlgedit").dialog("open");
    });*/

    $("table.table.taskmap tr.task.actions .close").click(function () {
        var $row = $(this).parents("tr.task").first();
        var $table = $(this).parents("table.table.taskmap").first();
        $row.addClass("invisible");
        $table.find("tr.selectedrow").removeClass("selectedrow");

        return false;
    });

    $("table.table.taskmap tr.task .expandresources").click(function () {
        var $row = $(this).parents("tr.task").first();
        var $table = $(this).parents("table.table.taskmap").first();
        $row.find(".resources .resourceslist").toggleClass("expanded");

        return false;
    });

    $(".commands .command.toggleresources").click(function () {
        var $table = $(this).parents("table.table").first();
        $table.find(".resources .resourceslist").toggleClass("expanded");
    });

    $(".commands .command.togglestatus").click(function () {
        var $table = $(this).parents("table.table").first();
        $table.find("td.status").toggleClass("expanded");
        $table.find("th.status").toggleClass("expanded");
    });

    $(".commands .command.toggleinfo").click(function () {
        var $table = $(this).parents("table.table").first();
        $table.toggleClass("showinfo");
    });

    $(".commands .command.toggleroles").click(function () {
        var $table = $(this).parents("table.table.projects").first();
        $table.find(".roles .roleslist").toggleClass("expanded");
    });

    $(".commands .command.togglecompleteness").click(function () {
        var $table = $(this).parents("table.table").first();
        $table.toggleClass("totalcompleteness");
    });

    $(".listwrapper .grouptitle").each(function () {
        $(this).data("title", $(this).html());
    });

    $(".listwrapper .group.graph").each(function () {
        var $title = $(this).prev(".grouptitle");
        var st = "" + $title.data("title");
        $title.html(st.replace("{n}", ""));
    });

    /*Temp*/
    $(".dialog.dlgtask").dialog({
        appendTo:"form",
        width:600
    });

    $("table.tasks tr.task td.name a.opendlgtask").click(function () {
        $(".dialog.dlgtask").dialog("open");
        $(".dialog.dlgtask").find(".tabs").tabs({active:0});
        return false;
    });

    $("table.tasks tr.task td.name span.icon.attacchment").click(function () {
        $(".dialog.dlgtask").dialog("open");
        $(".dialog.dlgtask").find(".tabs").tabs({active:1});
        return false;
    });

    $(".projectname span.icon.attacchment, .projectnametitle span.icon.attacchment").click(function () {
        $(".dialog.dlgprojectattachments").dialog("open");
        return false;
    });
    /*End Temp*/

    $(".listwrapper .group.graph .item").each(function () {
        $(this).html("&nbsp;");
    });


    $(".listwrapper .group.graph .item").hover(function () {
        var $el = $(this);
        var $group = $el.parents(".group").first();
        var $list = $el.parents(".listwrapper").first();
        var $title = $group.prev(".grouptitle");
        /*$el.html("&nbsp;");*/
        var idx = $el.index();
        var n = $group.find(".item").size();

        $group.addClass("hovered");

        var st = "" + $title.data("title");
        $title.html(st.replace("{n}", idx + 1));

        $list.find(".group:not(.hovered)").find(".item").removeClass("selected");

        $list.find(".group:not(.hovered)").each(function () {
            var $title = $(this).prev(".grouptitle");
            var st = "" + $title.data("title");
            $title.html(st.replace("{n}", ""));
        });

        $group.removeClass("hovered");

        for (var i = 0; i <= n; i++) {
            if (i <= idx) {
                $($group.children(".item").get(i)).addClass("selected");
            } else {
                //$list.find(".item").removeClass("selected");

                $($group.children(".item").get(i)).removeClass("selected");
            }
        }

    });



    if (jQuery.hoverIntent) {
        $(".listwrapper .group.graph").hoverIntent({
            over: function () {
                var $list = $(this).parents(".listwrapper").first();


            },
            out: function () {
                $(this).find(".item").removeClass("selected");
                var $title = $(this).prev(".grouptitle");
                var st = "" + $title.data("title");
                $title.html(st.replace("{n}", ""));
            }
        });
    } else {
        $(".listwrapper .group.graph").hover(function () { }, function () {
            //$(this).find(".item").removeClass("selected");
            /*var $title = $(this).prev(".grouptitle");
            var st = ""+$title.data("title");
            $title.html(st.replace("{n}",0));*/
        });
    }

    /*$(".listwrapper .group.graph").hover(function(){},function(){
    $(this).find(".item").removeClass("selected");
    var $title = $(this).prev(".grouptitle");
    var st = ""+$title.data("title");
    $title.html(st.replace("{n}",0));
    });*/

    /*$("fieldset.expandable").blockableFieldset({
    blockedClass: "disabled"
    });*/


    /*$(".btnswitchgroup.fullwidth .btnswitch.hide").click(function(){

    var $parent = $(this).parents(".btnswitchgroup").first();
    var $data = $parent.data("rel");
    var $name = $parent.data("name");

    $($data).removeClass($name);
    $parent.find(".btnswitch").removeClass("active");
    $(this).addClass("active");

    });

    $(".btnswitchgroup.fullwidth .btnswitch.show").click(function(){

    var $parent = $(this).parents(".btnswitchgroup").first();
    var $data = $parent.data("rel");
    var $name = $parent.data("name");

    $($data).addClass($name);
    $parent.find(".btnswitch").removeClass("active");
    $(this).addClass("active");

    });*/



    $(".btnswitchgroup .btnswitch.hide").click(function () {
        var $parent = $(this).parents(".btnswitchgroup").first();
        var $data = $parent.data("rel");
        var $table = $parent.data("table");
        var $name = $parent.data("name");
        $($data).addClass("invisible");

        if ($table != "") {
            var tablename = $($table).data("name");
            $.cookie(tablename + "-no-" + $data, true);

            $($table).addClass("no-" + $name);
            $($table).find("td[colspan]").each(function () {
                var delta = parseInt($(this).data("colspan"));
                var n = $($table).find("th:not(.invisible)").size();
                $(this).attr("colspan", n + delta);
            });

            if($(this).is(":not(.active)")){
           $($table).find("th.resizablecol").each(function () {
               $(this).attr("style", "");
                var size = $(this).css("width");
                var columnname = $(this).data("name");
                //$(this).css("width",size);
                $(this).children("span.th").css("width", "100%");

                $.cookie(tablename + "-" + columnname, size, { expires: 1 });
            });
        }
        }

        $parent.find(".btnswitch").removeClass("active");
        $(this).addClass("active");
    });

    $(".btnswitchgroup .btnswitch.show").click(function () {
        var $parent = $(this).parents(".btnswitchgroup").first();
        var $data = $parent.data("rel");
        $($data).removeClass("invisible");

        var $table = $parent.data("table");
        var $name = $parent.data("name");
        if ($table != "") {
            var tablename = $($table).data("name");
            $.cookie(tablename + "-no-" + $data, false);

            $($table).removeClass("no-" + $name);
            $($table).find("td[colspan]").each(function () {
                var delta = parseInt($(this).data("colspan"));
                var n = $($table).find("th:not(.invisible)").size();
                $(this).attr("colspan", n + delta);
            });

            if($(this).is(":not(.active)")){
            $($table).find("th.resizablecol").each(function () {
                $(this).attr("style", "");
                var size = $(this).css("width");
                var columnname = $(this).data("name");
                //$(this).css("width",size);
                $(this).children("span.th").css("width", "100%");
                //$(this).attr("style", "");
                $.cookie(tablename + "-" + columnname, size, { expires: 1 });
            });
        }
        }

        $parent.find(".btnswitch").removeClass("active");
        $(this).addClass("active");
    });

    /*$(".btnswitchgroup .btnswitch.hide.active").each(function () {
        var $self = $(this);
        var $parent = $(this).parents(".btnswitchgroup").first();
        var $data = $parent.data("rel");
        var $table = $parent.data("table");
        var $name = $parent.data("name");

        $self.click();


    });*/


    $(".btnswitchgroup .btnswitch.hide").each(function () {
        var $self = $(this);
        var $parent = $(this).parents(".btnswitchgroup").first();
        var $data = $parent.data("rel");
        var $table = $parent.data("table");
        var $name = $parent.data("name");

        if ($table != "") {
            var tablename = $($table).data("name");
            var invisible = $.cookie(tablename + "-no-" + $data);

            if (invisible == "true") {
                $self.click();

                /*$($data).addClass("invisible");
                $($table).addClass("no-"+$name);
                $($table).find("td[colspan]").each(function(){
                var delta = parseInt($(this).data("colspan"));
                var n = $($table).find("th:not(.invisible)").size();
                $(this).attr("colspan",n+delta);
                });

                $parent.find(".btnswitch").removeClass("active");
                $(this).addClass("active");*/
            }
        }

    });

    //$("table.table.taskmap span.icons.float").hide();
    if (jQuery().hoverIntent) {
        $("table.table.taskmap tr.task").hoverIntent({
            over: function () {
                $(this).addClass("hovered");
            },
            out: function () {

                $(this).removeClass("hovered");
                $(this).find("span.icons.floating").removeClass("clicked");
            },
            timeout: 200
        }
    );
    } else {
        $("table.table.taskmap tr.task").hover(function () {
            $(this).addClass("hovered");
        },
            function () {
                $(this).removeClass("hovered")
                $(this).find("span.icons.floating").removeClass("clicked");

            });
    }

    $("span.icons.floating span.icon").click(function () {
        $(this).parents("span.icons.floating").first().toggleClass("clicked");
    });

    /*$("table.table.taskmap tr.task").bind("contextmenu",function(e){

    return false;

    });*/

    /*$("table.table.taskmap").colResizable({
    postbackSafe:true,
    flush:false
    });*/


    //$("input[type='text']").widtherize();
    /*
    $(".editable.editmode .edit input[type='text']")

    $("input[type='text'].resizable").autoResize({
    minHeight:"original",
    maxHeight:"original",
    maxWidth:300

    });

    $(".editable .edit input[type='text']").autoResize({
    minHeight:"original",
    maxHeight:"original",
    maxWidth:300

    });*/

    if (jQuery().autoResize) {
        $(".editable .edit input[type='text']").autoResize({
            minHeight: "original",
            maxHeight: "original",
            minWidth: 50,
            maxWidth: 300,
            extraSpace: 20
        });
    }
    /*$("table.taskmap th").each(function(){
    $(this).data("id",$(this).index());
    });*/

    $("table.taskmap th.resizablecol").each(function () {
        var tablename = $(this).parents("table.taskmap").data("name");
        var columnname = $(this).data("name");
        var size = $.cookie(tablename + "-" + columnname);
        if (size != null) {
            $(this).data("original", $(this).children("span.th").width);
            $(this).children("span.th").css("width", size);
            //$(this).css("width",size);
            $(this).css("width", size);
        }
        //$.cookie(tablename+"-"+columnname,size,{expires:-1});
    });

    $("table.taskmap .restore").click(function () {
        var $table = $(this).parents("table.taskmap");
        var tablename = $(this).parents("table.taskmap").data("name");
        $table.find("th.resizablecol").each(function () {
            var size = $(this).data("original");
            var columnname = $(this).data("name");
            //$(this).css("width",size);
            $(this).children("span.th").css("width", size);
            $(this).attr("style", "");
            $.cookie(tablename + "-" + columnname, size, { expires: -1 });
        });
    });

    /* $("table.taskmap th.resizablecol.last span.th").resizable({
    handles:"w",
    minWidth:50,

    resize: function(event, ui) {

    ui.originalElement.parents("th").first().css("width",ui.size.width);

    ui.size.height = ui.originalSize.height;
    },
    stop:function(event, ui){

    var tablename = ui.originalElement.parents("table.taskmap").data("name");
    var columnname = ui.originalElement.parents("th").first().data("name");

    var size = ui.size.width;

    $.cookie(tablename+"-"+columnname,size,{expires:1});

    }

    });*/


    $("table.taskmap th.resizablecol span.th").resizable({
        handles: "e",
        minWidth: 50,

        resize: function (event, ui) {

            ui.originalElement.parents("th").first().css("width", ui.size.width);
            //ui.originalElement.parents("th").first().css("width","");
            //alert(ui.element.class);
            //var res=  ui.originalElement.data("resize");
            /*if(res=="no")
            {
            ui.size.height = ui.originalSize.height;
            ui.size.width = ui.originalSize.width;
            }*/
            ui.size.height = ui.originalSize.height;
        },
        stop: function (event, ui) {
            //ui.originalElement.index();
            var tablename = ui.originalElement.parents("table.taskmap").data("name");
            var columnname = ui.originalElement.parents("th").first().data("name");

            var size = ui.size.width;
            //ui.originalElement.parents("th").first().css("width",size);
            //ui.originalElement.css("width","");
            $.cookie(tablename + "-" + columnname, size, { expires: 1 });

        }

    });

    if (jQuery().hoverIntent) {

        $("table.taskmap tfoot tr.toolbar").hoverIntent({
            over: function () {

            },
            out: function () {
                $(this).find(".icons.floating").removeClass("clicked");
                $(this).find("span.icons.floating .group.graph .item").removeClass("selected");
                $(this).find("span.icons.floating .group.graph").each(function () {
                    var $title = $(this).prev(".grouptitle");
                    var st = "" + $title.data("title");
                    $title.html(st.replace("{n}", ""));
                });
            },
            timeout: 200
        });

    } else {

        $("table.taskmap tfoot tr.toolbar").hover(function () { }, function () {
            $(this).find(".icons.floating").removeClass("clicked");
            $(this).find("span.icons.floating .group.graph .item").removeClass("selected");
        });
    }

    $(".editable .edit.datepicker input[type='text']").datepicker({
        //showOn:"button",
        constrainInput: true,
        dateFormat: "dd/mm/yy",
        changeMonth: true,
        changeYear: true,
        showOtherMonths: true,
        selectOtherMonths: true
    });

    $(".editable .edit.datepicker input[type='text']").datepicker("option", $.datepicker.regional["en-GB"]);
    //$(".editable .edit.datepicker input").datepicker("option",$.datepicker.regional["it"] );
    //$(".editable .edit.datepicker input").datepicker("option",$.datepicker.regional["de"] );
    //$(".editable .edit.datepicker input").datepicker("option",$.datepicker.regional["es"] );

    $(".commands .command.wide").click(function () {
        $(".page-width").toggleClass("fullwidth");
        var name = $("table.taskmap").data("name");
        $.cookie(name + ".page-width", $(".page-width").is(".fullwidth"));
    });

    $(".commands .command.narrow").click(function () {
        $(".page-width").toggleClass("fullwidth");
        var name = $("table.taskmap").data("name");
        $.cookie(name + ".page-width", $(".page-width").is(".fullwidth"));
    });

    $(".commands .command.expandnodes").click(function () {
        //$(this).parents("table.treetable").first().treeTable("expandAll");
        $(this).parents("table.treetable").find("tr").each(function () { $(this).expand(); });
    });
    $(".commands .command.compressnodes").click(function () {
        $(this).parents("table.treetable").find("tr").each(function () { $(this).collapse(); });
    });

    var name = $("table.taskmap").data("name");

    var fullwidth = $.cookie(name + ".page-width");
    if (fullwidth == "true") {
        $(".page-width").addClass("fullwidth");
    } else {
        $(".page-width").removeClass("fullwidth");
    }

    $(".commands .command.save").click(function () {

    });

    $("table.taskmap .restore").click(function () {
        var $table = $(this).parents("table.taskmap");
        var tablename = $(this).parents("table.taskmap").data("name");
        $table.find("th.resizablecol").each(function () {
            var size = $(this).data("original");
            var columnname = $(this).data("name");
            $(this).css("width", size);
            $(this).attr("style", "");
            $.cookie(tablename + "-" + columnname, size, { expires: -1 });
        });

        $(".btnswitchgroup .btnswitch.show").click();
    });


    $(".hidecontent input[type='checkbox']").each(function () {
        if ($(this).is(":checked")) {
            $(this).parents(".hidecontent").first().find(".hideme").show();
        } else {
            $(this).parents(".hidecontent").first().find(".hideme").hide();
            $(this).parents(".hidecontent").first().find(".hideme").hide();
        }
    });

    $(".hidecontent input[type='checkbox']").click(function () {
        if ($(this).is(":checked")) {
            $(this).parents(".hidecontent").first().find(".hideme").show();
        } else {
            $(this).parents(".hidecontent").first().find(".hideme").hide();
        }
    });

    /*$("table.taskmap").resizable({
    //handles:"e, w",
    resize: function(event, ui) {
    //ui.size.height = ui.originalSize.height;
    }
    });*/

    $("div.summary.forceopen").each(function(){
        $.cookie("div.summary", true,{expires:1});
    });

    $("div.summary.forceclose").each(function(){
        $.cookie("div.summary", false,{expires:1});
    });

    var value = $.cookie("div.summary");
    //alert(value);
    if(value=="true")
    {
        //alert("TRUE: "+value);
        $("div.summary").removeClass("compressed").addClass("expanded");
    }else
    {
        //alert("FALSE: "+value);
        $("div.summary").removeClass("expanded").addClass("compressed");
    }

    $("div.summary.compressed").click(function(){
       $(this).removeClass("compressed").addClass("expanded");
        $.cookie("div.summary", true,{expires:1});
        return false;
    });

    $("div.summary .icon.close").click(function(){
        $(this).parents(".summary").removeClass("expanded").addClass("compressed");
        $.cookie("div.summary", false,{expires:1});
        return false;
    });

    $("div.summary .summaryitem, div.summary .summaryitem *").click(function () {
        if ($(this).find("a").size() > 0) {
            $(this).find("a")[0].click("click");
        }
    });
//dlgupload
    $(".dialog.dlgupload").dialog({
        appendTo:"form",
        modal:true,
        autoOpen:false, // put false
        width:600
    });
    $(".dialog.dlguploadsave").dialog({
        appendTo:"form",
        modal:true,
        autoOpen:false, // put false
        width:600
    });
    $(".dialog.dlgimport").dialog({
        appendTo:"form",
        modal:true,
        autoOpen:false, // put false
        width:600
    });
    $(".dialog.dlgurl").dialog({
        appendTo:"form",
        modal:true,
        autoOpen:false, // put false
        width:600
    });

    if(jQuery().dropdownList) {

        $(".dropdown.enabled").dropdownList({changeWidth: true});
    }


    $(".icon.sharedto").click(function(){
        var dlg = $(".dialog.dlgsharedto").dialog("open");
        var data = $(this).data("sharedto");

        var splitted = data.split(';');
        //console.log(splitted);

        var $shares = dlg.find("ul.shares");

        //$shares.html("");
        for(i=0;i<splitted.length;i++)
        {
            var item = splitted[i].split(':');
            var text = item[0];
            var link = item[1];

            $shares.append($("<li class='share'><a href='"+link+"'>"+text+"</a></li>"));

        }

        $(".dialog.dlgsharedto li.share a").click(function(){
           dlg.dialog("close");
        });
    });

});

$(document).ready(function () {
    $(".dialog.dlgviewprojectattachments").dialog({
        //appendTo:"form",
        autoOpen: true,
        modal: true,
        width: 600,
        height: 450,
        open: function () {
            //$(this).find(".chzn-select").chosen({});
            //$(this)
        },
        close: function (event, ui) {
            $("input[type='hidden'].autoopendialog").val("");
        }
    });
});