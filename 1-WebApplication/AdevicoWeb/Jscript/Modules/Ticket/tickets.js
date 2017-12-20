$(function () {

    $("div.dialog").dialog({
        autoOpen: false,
        modal: true,
        appendTo: "form",
        closeOnEscape: true,
        modal: true,
        width: 890,
        height: 450,
        minHeight: 300,
        minWidth: 700
    });

    $("input.activator").inputActivator();

    $("ul.tree.collapsable").collapsableTreeAdv();

    $(".ddbuttonlist.enabled").dropdownButtonList();

    $(".openfakedropdown").click(function () {
        var $drop = $(this).next(".fakedropdown");
        $drop.toggleClass("open");
    });

    $(".dropdown.enabled").dropdownList({ changeWidth: true });

    $("ul.tree.selectable.enabled").find(".name").click(function () {
        var id = $(this).parents("li").first().attr("id");
        $(this).parents(".selectable").first().find(".name").removeClass("selected");
        $(this).addClass("selected");
        $(this).parents("ul.selectable").next("input[type='hidden']").val(id);
        if ($(this).parents("ul.selectable").is(".fakedropdown")) {

            $(this).parents("ul.selectable").prev(".openfakedropdown").html($(this).html());
            $(this).parents("ul.selectable").removeClass("open");
        }
    });

    /*$("ul.tree").selectable({
     filter:".name",
     selected:function(event, ui)
     {
     var $selected = $(ui.selected);

     var id = $selected.parents("li").first().attr("id");

     $selected.parents("ul.tree").next("input[type='hidden']").val(id);
     }
     });*/

    $("table.table th .selectall").click(function () {
        var $table = $(this).parents("table.table").first();
        var $th = $(this).parents("th").first();

        var datacol = $th.data("col");
        $table.find("td[data-col='" + datacol + "'] input[type='checkbox']:not(:disabled)").prop("checked", true);
    });

    /*$("table.table th .selectnone").click(function(){
     var $table =$(this).parents("table.table").first();
     var $th = $(this).parents("th").first();
     var thclass = $th.attr("class").trim().replace(new RegExp(" ", 'g'),".");
     $table.find("td."+thclass+" input[type='checkbox']:not(:disabled)").prop("checked",false);
     });*/

    $("table.table th .selectnone").click(function () {
        var $table = $(this).parents("table.table").first();
        var $th = $(this).parents("th").first();

        var datacol = $th.data("col");
        $table.find("td[data-col='" + datacol + "'] input[type='checkbox']:not(:disabled)").prop("checked", false);
    });

    $("table.table td .selectall").click(function () {
        //var $table =$(this).parents("table.table").first();
        var $tr = $(this).parents("tr").first();
        $tr.find("td:not(.check) input[type='checkbox']").prop("checked", true);
    });

    $("table.table td .selectnone").click(function () {
        //var $table =$(this).parents("table.table").first();
        var $tr = $(this).parents("tr").first();
        $tr.find("td:not(.check) input[type='checkbox']").prop("checked", false);
    });

    $(".expandlistwrapper:visible:not(.initialized)").each(function () {
        InitializeExpandList($(this));
    });

    function InitializeExpandList(el) {
        if (!el.is(".initialized")) {
            el.addClass("initialized");
            var $children = el.find("ul.expandlist");
            var $content = $children.children().wrapAll('<div class="overflow">');

            //$children.wrapInner('<div class="clearfix" />');
            var delta = 3;

            var $el = el.find("div.overflow");
            var HasOverflow = $children.height() + delta < $el.height();

            if (!HasOverflow) {
                el.addClass("disabled");
                el.removeClass("compressed");
            } else {
                el.removeClass("disabled");
            }
        }
    }

    $(".expandlistwrapper .command.expand").click(function () {
        $(this).parents(".expandlistwrapper").first().removeClass("compressed");
        return false;
    });

    $(".expandlistwrapper .command.compress").click(function () {
        $(this).parents(".expandlistwrapper").first().addClass("compressed");
        return false;
    });

    $("table.expandable").each(function () {
        var $table = $(this);
        var max = $table.data("max") - 1;


        $table.find("tbody > tr").removeClass("hidden");
        $table.find("tbody > tr:gt(" + max + ")").addClass("hidden");

        var $showextra = $table.find("tfoot .showextra:not(.first)");
        var $hideextra = $table.find("tfoot .hideextra:not(.last)");

        if ($showextra.size() > 0) {
            $showextra.html($showextra.html().replace("{0}", $table.find("tbody > tr").size()));
        }
        if ($hideextra.size() > 0) {
            $hideextra.html($hideextra.html().replace("{0}", max + 1));
        }

        if ($table.find("tbody > tr").size() <= max) {
            $table.find("tfoot").hide();
        } else {
            $table.find("tfoot").show();
        }

    });

    $("table.expandable tfoot .showextra").click(function () {
        var $table = $(this).parents("table.expandable").first();
        $table.removeClass("compressed");
    });

    $("table.expandable tfoot .hideextra").click(function () {
        var $table = $(this).parents("table.expandable").first();
        $table.addClass("compressed");
    });

    $(".numberedlist.roleusers").click(function () {
        $(".dialog.dlgresources").dialog("open");
    });


    if (jQuery().treeTable) {

        $("table.treetable.userrolemap").treeTable({ initialState: "collapsed" });

        $("table.treetable").treeTable({ initialState: "expanded" });
    }

    $("span.icon.showdesc").click(function () {

        //alert($(this).parents("td.name").first().size().find(".dlgdescription"));

        var html = $(this).parents("td.name").first().find("div.description").html();

        $(".dlgdescription").find("div.description").html(html);
        $(".dlgdescription").dialog("open");
    });

    $(".icon.delete").click(function () {
        $(".dialog.dlgdelcategory").dialog("open");
        $(".dialog.dlgdelcategory").dialog({
            width: 600,
            autoOpen: false
        });
    });

    $(".fieldobject.collapsable .fieldrow.title").click(function () {
        $(this).parents(".fieldobject.collapsable").toggleClass("collapsed");
    });

    $(".dialog .close").click(function () {
        $(this).parents(".dialog").first().dialog("close");
        return false;
    });

    if (jQuery().chosen) {
        $("select.chzn-select").chosen();

        $(".selectall").click(function () {
            var dis = $(this).parents(".choseselect").find(".chzn-select").attr("disabled");
            if (dis != "disabled") {
                $(this).parents(".choseselect").find(".chzn-select option:not(:disabled)").attr("selected", true);
                $(this).parents(".choseselect").find(".chzn-select").trigger("liszt:updated");
                $(this).parents(".choseselect").find(".chzn-select").trigger("change");
            }
        });
        $(".selectnone").click(function () {
            var dis = $(this).parents(".choseselect").find(".chzn-select").attr("disabled");
            if (dis != "disabled") {
                //$(this).parents(".choseselect").find(".chzn-select option").attr("selected", false);
                $(this).parents(".choseselect").find(".chzn-select").val("");
                $(this).parents(".choseselect").find(".chzn-select").trigger("liszt:updated");
                $(this).parents(".choseselect").find(".chzn-select").trigger("change");
            }
        });
    };

    if (jQuery().collapsableTreeAdv) {
        $("ul.nestedtree.root").collapsableTreeAdv({
            selLi: 'li.treenode',
            selUl: 'ul.nestedtree',
            preserve: true,
            cookiePrefix: "communitytree-"
        });

        $("li.treenode").each(function () {
            if ($(this).find("li.treenode").size() == 0) {
                //$(this).find(".handle").css("visibility","hidden");
                $(this).find(".handle").addClass("disabled");
                $(this).addClass("leaf").removeClass("node");
            } else {
                $(this).addClass("node").removeClass("leaf");
            }
        })

        /*$("ul.nestedtree .selection input[type='checkbox']").change(function(){
         var $this = $(this);
         var $li = $this.parents("li.treenode").first();
         var value = $this.is(':checked');
         $li.find("ul.nestedtree li.treenode .selection input[type='checkbox']").prop("checked",value);
         var $root = $this.parents("ul.nestedtree.root").first();
         var $parents =$this.parents("li.treenode").first();

         });*/

        $.extend($.expr[':'], {
            unchecked: function (obj) {
                return ((obj.type == 'checkbox' || obj.type == 'radio') && !$(obj).is(':checked'));
            }
        });

        $(".treeselect .selection input:checkbox").on('change', function () {

            $(this).parents("li").first().find('ul').find('.selection input:checkbox').prop('checked', $(this).is(":checked"));

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
                //$parentULLI.find("input:checkbox").first().prop('indeterminate',true);
            }

            if (yes == 0) {
                parentULLI.find("input:checkbox").first().prop('indeterminate', false);
                parentULLI.find("input:checkbox").first().prop('checked', false);
                //$parentULLI.find("input:checkbox").first().prop('indeterminate',false);
            } else {
                //$parentULLI.find("input:checkbox").first().prop('indeterminate',true);
            }



            var $parentULnext = parentUL.parents("ul").first();
            var $parentULLInext = $parentULnext.parents("li").first();
            if (parentUL.is(":not(.root)")) {
                checkParents($parentULnext, $parentULLInext);
            }
        }

        $(".collapsable .expander").click(function () {
            $(this).parents(".collapsable").first().toggleClass("collapsed");
        });

        $(".defaultservicecontainer.collapsable .expander").click(function () {
            $(this).toggleClass("collapsed");
        });
    }
});