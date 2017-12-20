/**
 * Created by roberto.maschio on 09/02/2015.
 */

$(function () {

    /*$(".collapsable .expander").click(function(){
        $(this).parents(".collapsable").first().toggleClass("collapsed");
    });*/

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



});