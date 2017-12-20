/*globals $, jquery*/
/*jslint vars:true */
/**
 * Created by roberto.maschio on 26/11/13.
 */

$(".toolbar .icon.shadowcopy").click(function () {
    event.preventDefault();
    return false;
});


$(function () {
    'use strict';

    $(".ddbuttonlist.enabled").dropdownButtonList();

    $(".dropdown").each(function () {
        var $dropdown = $(this);
        $dropdown.find("input[type='hidden'].changetrigger").change(function () { $dropdown.find(".folderapply").click(); });
    });

    $(".groupedselector.normal .selectoricon, .groupedselector.normal .selectorlabel").click(function () {

        var $group = $(this).parents(".groupedselector").first();

        $(".groupedselector").not($group).removeClass("clicked");
        $group.toggleClass("clicked");

    });

    $(".groupedselector.normal .selectoritem").click(function () {

        var $group = $(this).parents(".groupedselector").first();
        $group.removeClass("clicked");

        $group.find(".selectoritem").removeClass("active");
        $(this).addClass("active");

        $group.find(".selectorgroup .selectorlabel").html($(this).find(".selectorlabel").html());

    });


    $(".groupedselector.noactive .selectoricon, .groupedselector.noactive .selectorlabel").click(function (event) {
        var $group = $(this).parents(".groupedselector").first();

        $(".groupedselector").not($group).removeClass("clicked");
        $group.toggleClass("clicked");
        //clearTimeout(timer);
        event.stopPropagation();
    });

    $(".groupedselector.noactive .selectoritem").click(function () {
        var $group = $(this).parents(".groupedselector").first();
        $group.removeClass("clicked");

        $group.find(".selectoritem").removeClass("active");
        $(this).addClass("active");

        $group.find(".selectorgroup .selectorlabel").html($(this).find(".selectorlabel").html());

    });

    // var timer = null;


    $(".groupedselector.noactive.clicked").mouseout(function () {
        //timer = setTimeout(closeMenu,3000);        
    });

    $(".groupedselector.noactive.clicked, .groupedselector.clicked .selectormenu").mouseenter(function () {
        // clearTimeout(timer);
    });

    $("body").click(function () {
        $(".groupedselector.noactive.clicked").removeClass("clicked");
    });

    function closeMenu() {
        //clearTimeout(timer);
        $(".groupedselector.noactive.clicked").removeClass("clicked");
    }

    function checkHeight() {

        var $tree = $(".section.tree"),
            $files = $(".section.files");
        //var $info = $(".section.info");

        $tree.data("height", 0);
        $files.data("height", 0);
        //alert();
        //$info.data("height",h3);

        $tree.css("height", "auto");
        $files.css("height", "auto");
        //$info.css("height","auto");

        $files.removeClass("height-modified");
        $tree.removeClass("height-modified");
        //$info.removeClass("height-modified");

        //var h1 = $tree.outerHeight(true);
        //var h2 = $files.outerHeight(true);


        //var max = 0;


        /* $files.find("tbody tr.file td.actions").each(function(){
             var hm1 = $(this).height();
             var hm2 = $('span.td span.icons', this).height();        
             max = Math.max(max,hm2);
            $(this).css('height', Math.max(hm1,hm2));            
         });*/

        var h1 = $tree.outerHeight(),
            h2 = $files.outerHeight();

        //$("td span.td span.icons").css("position","absolute");

        /*$files.find("tbody tr.file td.actions").each(function(){
           $(this).css('height', "auto");            
        });*/


        //var h3 = $info.outerHeight();
        //var newh = Math.max(h1,h2,h3);
        var newh = Math.max(h1, h2);

        $tree.css("height", newh);
        $files.css("height", newh);
        //$info.css("height",newh);

        $files.addClass("height-modified");
        $tree.addClass("height-modified");
        //$info.addClass("height-modified");
    }



    $("ul.nestedtree.root").collapsableTreeAdv({
        selLi: 'li.treenode',
        selUl: 'ul.nestedtree',
        preserve: true,
        onResize: function () {
            checkHeight();
        }
    });

    $("li.treenode").each(function () {
        if ($(this).find("li.treenode").size() === 0) {
            $(this).find(".handle").css("visibility", "hidden");
        }
    });

    $.extend($.expr[':'], {
        unchecked: function (obj) {
            return ((obj.type === 'checkbox' || obj.type === 'radio') && !$(obj).is(':checked'));
        }
    });

    $("table thead tr th input:checkbox").click(function () {
        var $table = $(this).parents("table").first(),
            checkedStatus = this.checked,
            index = $(this).parent().index() + 1;
        $table.find("tbody tr td:nth-child(" + index + ") input:checkbox").each(function () {
            this.checked = checkedStatus;
        });
    });

    $("table tbody tr td input:checkbox").click(function () {
        var $table = $(this).parents("table").first(),
            checkedStatus = this.checked,
            index = $(this).parent().index() + 1,
            size = $table.find("tbody tr td:nth-child(" + index + ") input:checkbox").size(),
            size1 = $table.find("tbody tr td:nth-child(" + index + ") input:checkbox").filter(":checked").size();

        if (size === size1) {
            $table.find("thead tr th:nth-child(" + index + ") input:checkbox").prop("checked", true);

        } else {
            $table.find("thead tr th:nth-child(" + index + ") input:checkbox").prop("checked", false);
        }
    });


    function checkParents(parentUL, parentULLI) {
        //console.log("start");
        var $yes = parentUL.find("input:checkbox:checked");
        var yes = $yes.size();
        var $no = parentUL.find("input:checkbox:unchecked");
        var no = $no.size();

        if (yes !== 0 && no !== 0) {
            //console.log("yes!=0 && no!=0");
            parentULLI.find("input:checkbox").first().prop('indeterminate', true);
        }

        if (no === 0) {
            //console.log("no==0");
            parentULLI.find("input:checkbox").first().prop('indeterminate', false);
            parentULLI.find("input:checkbox").first().prop('checked', true);

        } else {
            //console.log("no!=0");
            parentULLI.find("input:checkbox").first().prop('indeterminate', true);
        }

        if (yes === 0) {
            //console.log("yes==0");
            parentULLI.find("input:checkbox").first().prop('indeterminate', false);
            parentULLI.find("input:checkbox").first().prop('checked', false);
            //$parentULLI.find("input:checkbox").first().prop('indeterminate',false);
        } /*else {
            //console.log("yes!=0");
            //$parentULLI.find("input:checkbox").first().prop('indeterminate',true);
        }*/

        var $parentULnext = parentUL.parents("ul").first();
        var $parentULLInext = $parentULnext.parents("li").first();
        if (parentUL.is(":not(.root)")) {
            checkParents($parentULnext, $parentULLInext);
        }

        //console.log("stop");
    }

    $(".treeselect .selection input:checkbox").on('change', function () {

        $(this).parents("li").first().find('ul').find('.selection input:checkbox').prop('checked', $(this).is(":checked"));
        $(this).parents("li").first().find('ul').find('.selection input:checkbox').prop('indeterminate', false);

        var $parentLI = $(this).parents("li").first();
        var $parentUL = $(this).parents("ul").first();
        var $parentULLI = $parentUL.parents("li").first();

        checkParents($parentUL, $parentULLI);

    });





    var name = filerepository_cookiename;

    function CheckCookies() {
        var fullwidth = $.cookie(filerepository_cookiename + ".NarrowWideView");
        if (fullwidth === "true") {
            $(".page-width").addClass("fullwidth");
            $("div.filerepository").addClass("fullwidth");
        } else {
            $(".page-width").removeClass("fullwidth");
            $("div.filerepository").removeClass("fullwidth");
        }

        var extrainfo = $.cookie(filerepository_cookiename + ".Extrainfo");
        if (extrainfo === "true") {
            $("table.files").addClass("noextrainfo");
            $("div.filerepository").addClass("noextrainfo");
            $(".commands .command.info").addClass("off").removeClass("on");
        } else {
            $("table.files").removeClass("noextrainfo");
            $("div.filerepository").removeClass("noextrainfo");
            $(".commands .command.info").removeClass("off").addClass("on");
        }

        var stats = $.cookie(filerepository_cookiename + ".Statistics");
        if (stats === "true") {
            $("table.files").addClass("nostats");
            $("div.filerepository").addClass("nostats");
            $(".commands .command.stats").addClass("off").removeClass("on");
        } else {
            $("table.files").removeClass("nostats");
            $("div.filerepository").removeClass("nostats");
            $(".commands .command.stats").removeClass("off").addClass("on");
        }

        var dates = $.cookie(filerepository_cookiename + ".Date");
        if (dates === "true") {
            $("table.files").addClass("nodate");
            $("div.filerepository").addClass("nodate");
            $(".commands .command.date").addClass("off").removeClass("on");
        } else {
            $("table.files").removeClass("nodate");
            $("div.filerepository").removeClass("nodate");
            $(".commands .command.date").removeClass("off").addClass("on");
        }

        var showtree = $.cookie(filerepository_cookiename + ".Tree");
        if (showtree === "true") {
            $("div.filerepository").addClass("notree");

            $(".section.tree").hide();
            $(".section.files").addClass("grid_12");
            $(".section.files").addClass("alpha");

            $(".commands .command.tree").addClass("off").removeClass("on");
        } else {
            $("div.filerepository").removeClass("notree");

            $(".section.tree").show();
            $(".section.files").removeClass("grid_12");
            $(".section.files").removeClass("alpha");

            $(".commands .command.tree").removeClass("off").addClass("on");
        }

        /*var author = $.cookie(filerepository_cookiename + ".author");
        if (author == "true") {
            $("table.files").addClass("noauthor");
            $("div.filerepository").addClass("noauthor");
            $(".commands .command.author").addClass("off").removeClass("on");
        } else {
            $("table.files").removeClass("noauthor");
            $("div.filerepository").removeClass("noauthor");
            $(".commands .command.author").removeClass("off").addClass("on");
        }*/

        /*var actions = $.cookie(filerepository_cookiename + ".actions");
        if (actions == "true") {
            $("table.files").addClass("noactions");
            $("div.filerepository").addClass("noactions");
            $(".commands .command.actions").addClass("off").removeClass("on");
        } else {
            $("table.files").removeClass("noactions");
            $("div.filerepository").removeClass("noactions");
            $(".commands .command.actions").removeClass("off").addClass("on");
        }*/







    }

    //$.cookie(filerepository_cookiename + ".viewtype", 'value', { expires: -1 });

    var type = $.cookie(filerepository_cookiename + ".viewtype");

    if (type === undefined || type === null) {
        type = filerepository_default.toLowerCase();
        if (type === "simple") {
            SetCookies(filerepository_simple);
            $.cookie(filerepository_cookiename + ".viewtype", "simple");
        } else if (type === "standard") {
            SetCookies(filerepository_standard);
            $.cookie(filerepository_cookiename + ".viewtype", "standard");
        } else if (type === "advanced") {
            SetCookies(filerepository_advanced);
            $.cookie(filerepository_cookiename + ".viewtype", "advanced");
        }

        //checkHeight();
        //breadcrumb();
    }

    if (type === "simple") {
        $(".commands .command").removeClass("active");
        $(".commands .command.simple").addClass("active");

    } else if (type === "standard") {
        $(".commands .command").removeClass("active");
        $(".commands .command.standard").addClass("active");

    } else if (type === "advanced") {
        $(".commands .command").removeClass("active");
        $(".commands .command.advanced").addClass("active");

    } else if (type === "custom") {
        $(".commands .command").removeClass("active");
        $(".commands .command.custom").addClass("active");
    } /*else {        
    }*/


    CheckCookies();




    checkHeight();

    if (jQuery().myProgressBar) {
        $(".progressbar").myProgressBar();
    }

    $(".commands .command.wide").click(function () {
        $(".page-width").toggleClass("fullwidth");
        $("div.filerepository").toggleClass("fullwidth");
        //var name = $("table.taskmap").data("name");
        $.cookie(filerepository_cookiename + ".NarrowWideView", $(".page-width").is(".fullwidth"));

        if (jQuery().myProgressBar) {
            $(".progressbar").myProgressBar();
        }

        breadcrumb();
    });

    $(".commands .command.narrow").click(function () {
        $(".page-width").toggleClass("fullwidth");
        $("div.filerepository").toggleClass("fullwidth");
        //var name = $("table.taskmap").data("name");
        $.cookie(filerepository_cookiename + ".NarrowWideView", $(".page-width").is(".fullwidth"));
        if (jQuery().myProgressBar) {
            $(".progressbar").myProgressBar();
        }

        breadcrumb();
    });

    $(".commands .command.info").click(function () {
        $("table.files").toggleClass("noextrainfo");
        $("div.filerepository").toggleClass("noextrainfo");
        $(".commands .command.info").toggleClass("on").toggleClass("off");
        $.cookie(filerepository_cookiename + ".Extrainfo", $("table.files").is(".noextrainfo"));

    });

    /*$(".commands .command.author").click(function () {
        $("table.files").toggleClass("noauthor");
        $("div.filerepository").toggleClass("noauthor");
        $(".commands .command.author").toggleClass("on").toggleClass("off");
        $.cookie(filerepository_cookiename + ".author", $("table.files").is(".noauthor"));

    });*/

    /*$(".commands .command.actions").click(function () {
        $("table.files").toggleClass("noactions");
        $("div.filerepository").toggleClass("noactions");
        $(".commands .command.actions").toggleClass("on").toggleClass("off");
        $.cookie(filerepository_cookiename + ".actions", $("table.files").is(".noactions"));

    });*/

    $(".commands .command.stats").click(function () {
        $("table.files").toggleClass("nostats");
        $("div.filerepository").toggleClass("nostats");
        $(".commands .command.stats").toggleClass("on").toggleClass("off");
        $.cookie(filerepository_cookiename + ".Statistics", $("table.files").is(".nostats"));
    });

    $(".commands .command.date").click(function () {
        $("table.files").toggleClass("nodate");
        $("div.filerepository").toggleClass("nodate");
        $(".commands .command.date").toggleClass("on").toggleClass("off");
        $.cookie(filerepository_cookiename + ".Date", $("table.files").is(".nodate"));
    });

    $(".commands .command.tree").click(function () {
        $("div.filerepository").toggleClass("notree");
        $(".commands .command.tree").toggleClass("on").toggleClass("off");

        $(".section.tree").toggle();
        $(".section.files").toggleClass("grid_12");
        $(".section.files").toggleClass("alpha");

        $.cookie(filerepository_cookiename + ".Tree", $("div.filerepository").is(".notree"));
    });

    /*    $(".commands .command.restore").click(function () {
    
            $.cookie(filerepository_cookiename + ".Tree", false);
            $.cookie(filerepository_cookiename + ".Statistics", false);
            //$.cookie(filerepository_cookiename + ".actions", false);
            //$.cookie(filerepository_cookiename + ".author", false);
            $.cookie(filerepository_cookiename + ".Extrainfo", false);
            $.cookie(filerepository_cookiename + ".NarrowWideView", false);
            $.cookie(filerepository_cookiename + ".Date", false);
    
            CheckCookies();
    
        });*/

    /*$(".commands .command.restore").click(function () {

        $.cookie(filerepository_cookiename + ".tree", false);
        $.cookie(filerepository_cookiename + ".stats", false);
        $.cookie(filerepository_cookiename + ".actions", false);
        $.cookie(filerepository_cookiename + ".author", false);
        $.cookie(filerepository_cookiename + ".extrainfo", false);
        $.cookie(filerepository_cookiename + ".page-width", false);

        CheckCookies();

    });*/

    function SetCookies(value) {
        var values = value.split(",");
        $.cookie(filerepository_cookiename + ".Tree", values[0] == "1");
        $.cookie(filerepository_cookiename + ".Statistics", values[1] == "1");
        $.cookie(filerepository_cookiename + ".Extrainfo", values[2] == "1");
        $.cookie(filerepository_cookiename + ".Date", values[3] == "1");
        $.cookie(filerepository_cookiename + ".NarrowWideView", values[4] == "1");

        /*$.cookie(filerepository_cookiename + ".author", values[2]=="1");*/
    }





    $(".commands .command.simple").click(function () {
        $(this).parents(".commands").first().find(".clicked").removeClass("clicked");
        $(this).parents(".commands").first().find(".active").removeClass("active");
        $(this).addClass("active");

        SetCookies(filerepository_simple);
        $.cookie(filerepository_cookiename + ".viewtype", "simple");

        CheckCookies();
        checkHeight();
        breadcrumb();
        return false;
    });
    $(".commands .command.standard").click(function () {
        $(this).parents(".commands").first().find(".clicked").removeClass("clicked");
        $(this).parents(".commands").first().find(".active").removeClass("active");
        $(this).addClass("active");

        $.cookie(filerepository_cookiename + ".viewtype", "standard");

        SetCookies(filerepository_standard);

        CheckCookies();
        checkHeight();
        breadcrumb();
        return false;
    });
    $(".commands .command.advanced").click(function () {
        $(this).parents(".commands").first().find(".clicked").removeClass("clicked");
        $(this).parents(".commands").first().find(".active").removeClass("active");
        $(this).addClass("active");

        $.cookie(filerepository_cookiename + ".viewtype", "advanced");

        SetCookies(filerepository_advanced);

        CheckCookies();
        checkHeight();
        breadcrumb();
        return false;
    });

    /*$("div.filerepository table.files tr.file").mouseout(function(){
       //$(this).find(".groupedselector.clicked").removeClass("clicked");
    });*/
    $("div.filerepository table.files tr.file").mouseenter(function () {
        var $grouped = $(this).find(".groupedselector");
        //$(this).find(".groupedselector.clicked").removeClass("clicked");
        $(this).parents("table.files").first().find(".groupedselector.clicked").not($grouped).removeClass("clicked");
    });
    $("div.filerepository ul.directories li.directory .header").mouseenter(function () {
        var $grouped = $(this).find(".groupedselector");
        //$(this).find(".groupedselector.clicked").removeClass("clicked");
        $(this).parents("ul.directories").first().find(".groupedselector.clicked").not($grouped).removeClass("clicked");
    });


    var group = $("ul.sortabletree").sortable({
        handle: ".text",

        onDrop: function (item, container, _super) {
            //$('#serialize_output').val();
            //console.log(group.sortable("serialize").get());

            //group.sortable("serialize").get()

            var ser = "";

            group.find("li.sortableitem").each(function () {

                var $parent = $(this).parents("li.sortableitem").first();
                var parentId = 0;
                if ($parent.size() > 0) {
                    parentId = $parent.attr("id")
                }

                ser = ser + $(this).attr("id") + ":" + parentId + ";";
            });
            $('.serialize_output').first().val(ser);


            _super(item, container);
        }//,
        /* serialize: function (parent, children, isContainer) {
         return isContainer ? children.join() : parent.attr("id");
         }*/

        /*serialize:function ($parent, $children, parentIsContainer){
         var result = $.extend({}, $parent.attr("id"))

         if(parentIsContainer)
         return $children
         else if ($children[0]){
         result.children = $children
         delete result.subContainer
         }
         }*/
    });

    if (jQuery().dropdownList) {
        $(".dropdown.enabled").dropdownList({ changeWidth: true });
    }

   


    $.fn.hasOverflow = function () {
        var $this = $(this);
        return $this[0].scrollHeight > $this.outerHeight() ||
            $this[0].scrollWidth > $this.outerWidth();
    };



    $(".testbread").each(function () {

        var k = $(this).find(".item:not(.first,.last)").size();

        while ($(this).hasOverflow() && k > 0) {
            k--;
            $(this).find(".item:not(.hidden,.first,.last)").first().addClass("hidden");
            //$(".breadcrumb").find(".separator:not(.hidden,.first,.last)").first().addClass("hidden").hide();
        }
    });

    if ($('.testbread').size() > 0) {

        $('.testbread').find(".item.hidden").hover(function () {
            clearTimeout($(this).data('timeout'));
            $(this).parents(".testbread").first().find(".item.hover").addClass("hidden").removeClass("hover");
            $(this).removeClass("hidden").addClass("hover");

            var k = $(this).parents(".testbread").first().find(".item:not(.hidden,.first,.last,.hover)").size();

            while ($(this).parents(".testbread").first().hasOverflow() && k > 0) {
                k--;
                $(this).parents(".testbread").first().find(".item:not(.hidden,.first,.last,.hover)").first().addClass("hidden");
                //$(".breadcrumb").find(".separator:not(.hidden,.first,.last)").first().addClass("hidden").hide();
            }

        }, function () {
            var $this = $(this);
            var t = setTimeout(function () {
                $this.addClass("hidden").removeClass("hover");

                $this.parents(".testbread").first().find(".item").removeClass("hidden");
                var k = $this.parents(".testbread").first().find(".item:not(.first,.last)").size();

                while ($this.parents(".testbread").first().hasOverflow() && k > 0) {
                    k--;
                    $this.parents(".testbread").first().find(".item:not(.hidden,.first,.last,.hover)").first().addClass("hidden");
                    //$(".breadcrumb").find(".separator:not(.hidden,.first,.last)").first().addClass("hidden").hide();
                }

            }, 300);
            $(this).data('timeout', t);
        });
    }


    function breadcrumb() {

        $(".breadcrumb").each(function () {

            $(this).find(".item").removeClass("hidden");

            var k = $(this).find(".item:not(.first,.last)").size();

            //$(this).filter(":not(.single)").find(".item.last").css("width","auto");

            while ($(this).hasOverflow() && k > 0) {
                k--;
                $(this).find(".item:not(.hidden,.first,.last)").first().addClass("hidden");
                //$(".breadcrumb").find(".separator:not(.hidden,.first,.last)").first().addClass("hidden").hide();
            }
            /*var width=0;
           while(!$(this).filter(":not(.single)").hasOverflow()){
               width=$(this).find(".item.last").width();
               width+=1;
               $(this).find(".item.last").width(width);
           }

           $(this).filter(":not(.single)").find(".item.last").width(width-1);*/

        });

        if ($('.breadcrumb').size() > 0) {

            $('.breadcrumb').find(".item.hidden").hover(function () {
                clearTimeout($(this).data('timeout'));
                $(this).parents(".breadcrumb").first().find(".item.hover").addClass("hidden").removeClass("hover");
                //$(this).parents(".breadcrumb").first().find(".item.hover").addClass("hidden");
                $(this).removeClass("hidden").addClass("hover");

                var k = $(this).parents(".breadcrumb").first().find(".item:not(.hidden,.first,.last,.hover)").size();

                while ($(this).parents(".breadcrumb").first().hasOverflow() && k > 0) {
                    k--;
                    $(this).parents(".breadcrumb").first().find(".item:not(.hidden,.first,.last,.hover)").first().addClass("hidden");
                    //$(".breadcrumb").find(".separator:not(.hidden,.first,.last)").first().addClass("hidden").hide();
                }

            }, function () {
                var $this = $(this);
                var t = setTimeout(function () {
                    $this.addClass("hidden").removeClass("hover");

                    $this.parents(".breadcrumb").first().find(".item").removeClass("hidden");
                    var k = $this.parents(".breadcrumb").first().find(".item:not(.first,.last)").size();

                    while ($this.parents(".breadcrumb").first().hasOverflow() && k > 0) {
                        k--;
                        $this.parents(".breadcrumb").first().find(".item:not(.hidden,.first,.last,.hover)").first().addClass("hidden");
                        //$this.parents(".breadcrumbwrapper").first().find(".item:not(.hidden,.first,.last,.hover)").first().addClass("hidden");
                        //$(".breadcrumb").find(".separator:not(.hidden,.first,.last)").first().addClass("hidden").hide();
                    }

                }, 300);
                $(this).data('timeout', t);
            });
        }


    }

    breadcrumb();

    function doSomething() {
        if (jQuery().myProgressBar) {
            $(".progressbar").myProgressBar();

        }
        breadcrumb();
    };

    var resizeTimer;
    $(window).resize(function () {
        clearTimeout(resizeTimer);
        resizeTimer = setTimeout(doSomething, 50);
    });

    $("table.files tbody tr").hover(
            function () {
                $(this).addClass("hoverselected");
            }, function () {
                $(this).removeClass("hoverselected");
            }
        );
    $("table.files tbody tr").click(function () {
        $(this).parents("table.files").first().find(".hoverselected").removeClass("hoverselected");
        $(this).addClass("hoverselected");
    });


    $(".collapsable .expander:not(.treenode .expander)").click(function () {
        $(this).parents(".collapsable").first().toggleClass("collapsed").toggleClass("expanded");
    });

    $(".commands .groupedselector .selectoritem .command").click(function () {

        //$(this).parents(".groupedselector").first().addClass("clicked").addClass("active");    
        $(this).parents(".commands").first().children(".command").removeClass("active");
        $(this).parents(".groupedselector").addClass("active");
        $.cookie(filerepository_cookiename + ".viewtype", "custom");
        return false;
    });


    if (jQuery().fancybox) {
        $(".thumbnail a.image").fancybox();
        $(".thumbnail a.video").fancybox({});
        $(".thumbnail a.iframe").fancybox({
            type: "iframe",
            width: "75%",
            height: "75%"
        });
        $("a.lightbox.image").fancybox();
    }

    if (jQuery().tagit) {
        var tgit = $("input.tokeninputtag").tagit({
            removeConfirmation: true,
            triggerKeys: ['enter', 'comma', 'tab'],
            allowSpaces: true,
            /*autocomplete: {
				source: ['tagbello','tagbuono','tagbrutto']
			},*/
            availableTags: filerepository_tags
        });
    }

    $(".defaultservicecontainer.collapsable .itemheader .expander").click(function () {
        $(this).toggleClass("collapsed");
    });

    $(".nestedtree.calcsize .selection input[type='checkbox']").change(function () {
        var size = 0;
        var $tree = $(this).parents(".nestedtree.calcsize").first();
        var $selected = $tree.find(".selection input[type='checkbox']:checked");
        $selected.each(function () {
            var $this = $(this);
            var $parent = $this.parents(".treenode.directory.file");
            var current = $parent.data("size");
            if (current > 0) {
                size += current;
            }

        });

        $tree.data("size", size);

        var $community = $tree.parents(".community.defaultservicecontainer");
        var $sizeinfo = $community.find(".selectedsize");
        $sizeinfo.find(".number").html(formatBytes(size, 2));
        if (size != 0) {
            $sizeinfo.show();
        } else {
            $sizeinfo.hide();
        }

        CheckFileSize();
    });

    function CheckFileSize() {
        var totalsize = 0;

        $(".nestedtree.calcsize").each(function () {
            var size = 0;
            var $tree = $(this);
            var $selected = $tree.find(".selection input[type='checkbox']:checked");
            $selected.each(function () {
                var $this = $(this);
                var $parent = $this.parents(".treenode.directory.file");
                var current = $parent.data("size");
                if (current > 0) {
                    size += current;
                }

            });

            totalsize += size;

            var $community = $tree.parents(".community.defaultservicecontainer");
            var $sizeinfo = $community.find(".selectedsize");
            $sizeinfo.data("size", size);
            $sizeinfo.find(".number").html(formatBytes(size, 2));
            if (size != 0) {
                $sizeinfo.show();
            } else {
                $sizeinfo.hide();
            }
        });

        var $sizeinfo = $(".selectedsize.total");
        $sizeinfo.data("size", totalsize);
        $sizeinfo.find(".number").html(formatBytes(totalsize, 2));
        if (totalsize != 0) {
            $sizeinfo.show();
        } else {
            $sizeinfo.hide();
        }

        var $available = $(".selectedsize.available");
        //alert($available.data("size"));
        var aval = $available.data("size");

        if (aval < totalsize) {
            $available.addClass("error");
        } else {
            $available.removeClass("error");
        }
    }

    CheckFileSize();

    function formatBytes(bytes, decimals) {
        if (bytes == 0) return '0 Byte';
        var k = 1000;
        var dm = decimals + 1 || 3;
        var sizes = ['Bytes', 'KB', 'MB', 'GB', 'TB', 'PB', 'EB', 'ZB', 'YB'];
        var i = Math.floor(Math.log(bytes) / Math.log(k));
        return (bytes / Math.pow(k, i)).toPrecision(dm) + ' ' + sizes[i];
    }



    $('.unselectradio input[type="radio"]').each(function () {
        var rel = $(this).data("rel");
        if (rel != "" && rel != "undefined") {
            if ($(this).is(":checked")) {
                $(rel).show();
            } else {
                $(rel).hide();
            }
        }
    })

    $('.unselectradio input[type="radio"]').bind('click', function () {
        // Processing only those that match the name attribute of the currently clicked button...
        $('input[name="' + $(this).attr('name') + '"]').not($(this)).trigger('deselect'); // Every member of the current radio group except the clicked one...
        var rel = $(this).data("rel");
        if (rel != "" && rel != "undefined") {
            $(rel).show();
        }
    });

    $('input[type="radio"]').bind('deselect', function () {
        //console.log($(this));
        var rel = $(this).data("rel");

        if (rel != "" && rel != "undefined") {
            $(rel).hide();
        }
    })



    $(".fieldrow.textslider div.slider.percentage").slider({
        min: 0,
        max: 100,
        value: 100,
        slide: function (event, ui) {
            $(this).parents(".fieldrow").first().find("input.inputtext").val(ui.value);
        },
        stop: function (event, ui) {
            checkPercent();
        }
    });

    $(".fieldrow.textslider div.slider.percentage").each(function () {

        var min = $(this).data("min");
        var max = $(this).data("max");
        var step = $(this).data("step");
        if (min != "" && typeof min != "undefined") {
            $(this).slider("option", "min", min);
        }
        if (max != "" && typeof max != "undefined") {
            $(this).slider("option", "max", max);
        }
        if (step != "" && typeof step != "undefined") {
            $(this).slider("option", "step", step);
        }

        var value = $(this).slider("value");
        $(this).parents(".fieldrow").first().find("input.inputtext").val(value);
    });


    $(".fieldrow.textslider input.inputtext").change(function () {
        var value = this.value;

        $(this).parents(".fieldrow").first().find("div.slider").slider("value", parseInt(value));
    });

    $(".fieldrow.textslider div.slider.megabytes").slider({
        min: 0,
        max: 1000000000000,
        value: 0,
        step: 1000000,
        slide: function (event, ui) {
            var value = formatBytes(ui.value, 2);
            var values = value.split(" ");
            var number = values[0];
            var measure = values[1];

            $(this).parents(".fieldrow").first().find("input.inputtext").val(number);
            $(this).parents(".fieldrow").first().find(".btnswitch").removeClass("active");
            $(this).parents(".fieldrow").first().find(".btnswitch." + measure.toLowerCase()).addClass("active");


        },
        stop: function (event, ui) {
            setMasterSlaves();
        }
    });

    function setMasterSlaves() {
        var master = $("div.slider.megabytes.master").slider("value");
        $("div.slider.megabytes.slave1").slider("option", "max", master);
        var slave1 = $("div.slider.megabytes.slave1").slider("value");
        if (slave1 > master) {
            slave1 = master;
            $("div.slider.megabytes.slave1").slider("value", slave1);
        }
        var slave2 = master - slave1;
        var slave3 = slave1;
        $("div.slider.megabytes.slave2").slider("option", "max", slave2);

        var max = $("div.slider.megabytes.slave3").data("max");
        if (max != "" && typeof max != "undefined") {
            slave3 = Math.min(slave3, max);
        }
        $("div.slider.megabytes.slave3").slider("option", "max", slave3);

        var quotamaster = $("div.slider.megabytes.quotamaster").slider("value");
        var quotaslave = $("div.slider.megabytes.quotaslave").slider("value");

        var quotamax = $("div.slider.megabytes.quotaslave").data("max");

        if (quotamax != "" && typeof quotamax != "undefined") {

            quotaslave = Math.min(quotamaster, quotamax);

        }

        $("div.slider.megabytes.quotaslave").slider("option", "max", quotaslave);

        checkPercent();

        eachMegabytes(false);
    }

    function checkPercent() {
        var pc1 = $("div.slider.percentage.slave1").slider("value");
        var abs1 = $("div.slider.megabytes.slave1").slider("value");
        var new1 = pc1 * abs1 / 100;
        var pc1text = $("div.slider.percentage.slave1").parents(".fieldrow").first().find(".calcmegabytes .number");
        pc1text.html(formatBytes(new1));



        var pc2 = $("div.slider.percentage.slave2").slider("value");
        var abs2 = $("div.slider.megabytes.slave3").slider("value");
        var new2 = pc2 * abs2 / 100;
        var pc2text = $("div.slider.percentage.slave2").parents(".fieldrow").first().find(".calcmegabytes .number");
        pc2text.html(formatBytes(new2));


        var pc3 = $("div.slider.percentage.quotaslave").slider("value");
        var abs3 = $("div.slider.megabytes.quotamaster").slider("value");
        var new3 = pc3 * abs3 / 100;
        var pc3text = $("div.slider.percentage.quotaslave").parents(".fieldrow").first().find(".calcmegabytes .number");
        pc3text.html(formatBytes(new3));
    }

    function eachMegabytes(check) {
        $(".fieldrow.textslider div.slider.megabytes").each(function () {
            if (check) {
                var min = $(this).data("min");
                var max = $(this).data("max");
                var step = $(this).data("step");
                var value = $(this).data("value");
                if (min != "" && typeof min != "undefined") {
                    $(this).slider("option", "min", min);
                }
                if (max != "" && typeof max != "undefined") {
                    $(this).slider("option", "max", max);
                }
                if (step != "" && typeof step != "undefined") {
                    $(this).slider("option", "step", step);
                }
                if (value != "" && typeof value != "undefined") {
                    $(this).slider("value", value);
                }
            }

            var value1 = $(this).slider("value");
            var value = formatBytes(value1, 2);
            var values = value.split(" ");
            var number = values[0];
            var measure = values[1];

            $(this).parents(".fieldrow").first().find("input.inputtext").val(number);
            $(this).parents(".fieldrow").first().find(".btnswitch").removeClass("active");
            $(this).parents(".fieldrow").first().find(".btnswitch." + measure.toLowerCase()).addClass("active");
            $(this).parents(".fieldrow").first().find("input.hidden").val(value1);

        });
    }

    eachMegabytes(true);
    setMasterSlaves();

    $(".fieldrow.textslider.megabytes .btnswitch:not(.disabled)").click(function () {

        $(this).parents(".btnswitchgroup").first().find(".btnswitch").removeClass("active");
        $(this).addClass("active");

        var value = $(this).parents(".fieldrow").first().find("input").val();
        var intvalue = parseInt(value);
        var active = $(this);
        if (active.is(".mb")) {
            intvalue *= 1000000;
        } else if (active.is(".gb")) {
            intvalue *= 1000000000;
        } else if (active.is(".tb")) {
            intvalue *= 1000000000000;
        }
        $(this).parents(".fieldrow").first().find("div.slider").slider("value", intvalue);
        value = $(this).parents(".fieldrow").first().find("div.slider").slider("value");

        value = formatBytes(value, 2);
        var values = value.split(" ");
        var number = values[0].replace(".00", "");
        var measure = values[1];

        $(this).parents(".fieldrow").first().find("input.inputtext").val(number);
        $(this).parents(".fieldrow").first().find(".btnswitch").removeClass("active");
        $(this).parents(".fieldrow").first().find(".btnswitch." + measure.toLowerCase()).addClass("active");

        setMasterSlaves();
        return false;
    });

    $(".fieldrow.textslider.megabytes input.inputtext").change(function () {
        var value = this.value;
        var intvalue = parseInt(value);
        var active = $(this).parents(".fieldrow").first().find(".btnswitch.active");
        if (active.is(".mb")) {
            intvalue *= 1000000;
        } else if (active.is(".gb")) {
            intvalue *= 1000000000;
        } else if (active.is(".tb")) {
            intvalue *= 1000000000000;
        }
        $(this).parents(".fieldrow").first().find("div.slider").slider("value", intvalue);

        value = $(this).parents(".fieldrow").first().find("div.slider").slider("value");
        //$(this).parents(".fieldrow").first().find("input.hidden").val(value);

        value = formatBytes(value, 2);
        var values = value.split(" ");
        var number = values[0];
        var measure = values[1];

        $(this).parents(".fieldrow").first().find("input.inputtext").val(number);
        $(this).parents(".fieldrow").first().find(".btnswitch").removeClass("active");
        $(this).parents(".fieldrow").first().find(".btnswitch." + measure.toLowerCase()).addClass("active");

        setMasterSlaves();

    });

    $(".coveredradio.enabled:not(.scormsettingtype)").each(function () {
        var $parent = $(this);
        var $checks = $parent.find(".wclist input[type='radio']");
        var $buttons = $parent.find(".btnswitch");
        $checks.each(function () {
            var idx = $(this).index() / 3;
            if ($(this).is(":checked")) {
                $($buttons.get(idx)).addClass("active");
                if ($(this).data("value") == "on") {
                    $parent.addClass("on").removeClass("off");
                } else if ($(this).data("value") == "off") {
                    $parent.addClass("off").removeClass("on");
                }
            } else {
                $($buttons.get(idx)).removeClass("active");
            }
        });

        var $disable = $parent.data("disable");
        if ($disable != "" && typeof $disable != "undefined") {
            if ($parent.is(".on")) {
                $parent.parents($disable).addClass("on").removeClass("off");
            } else {
                $parent.parents($disable).addClass("off").removeClass("on");
            }
        }
    });

    $(".coveredradio.enabled:not(.scormsettingtype) .btnswitch").click(function () {
        var $parent = $(this).parents(".coveredradio").first();
        $parent.find(".active").removeClass("active");
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
        if ($(this).is(".on")) {
            $parent.addClass("on");
            $parent.removeClass("off");
        } else if ($(this).is(".off")) {
            $parent.addClass("off");
            $parent.removeClass("on");
        }

        var $disable = $parent.data("disable");
        if ($disable != "" && typeof $disable != "undefined") {
            if ($parent.is(".on")) {
                $parent.parents($disable).addClass("on").removeClass("off");
            } else {
                $parent.parents($disable).addClass("off").removeClass("on");
            }
        }

        return false;
    });

    $(".coveredradio.enabled:not(.scormsettingtype) input[type='radio']").click(function () {
        var $parent = $(this).parents(".coveredradio").first();
        $parent.find(".active").removeClass("active");

        var $checks = $parent.find(".btnswitch");

        if ($parent.is(":not(.readonly)")) {
            //$(this).toggleClass("active");
            var idx = $(this).index() / 3;

            if ($(this).is(":checked")) {
                $($checks.get(idx)).addClass("active");
                if ($(this).data("value") == "on") {
                    $parent.addClass("on").removeClass("off");
                } else if ($(this).data("value") == "off") {
                    $parent.addClass("off").removeClass("on");
                }
            } else {
                $($checks.get(idx)).removeClass("active");
            }
        }

        var $disable = $parent.data("disable");
        if ($disable != "" && typeof $disable != "undefined") {
            if ($parent.is(".on")) {
                $parent.parents($disable).addClass("on").removeClass("off");
            } else {
                $parent.parents($disable).addClass("off").removeClass("on");
            }
        }
    });



    //$(".iteminfo .text").each(function () {
    //    var text = $(this).text();
    //    $(this).attr("title", text);
    //});

    //$('.iteminfo .text').each(function () {
    //    var path = $(this).attr("title").split(".");
    //    if (path.length > 1) {
    //        var name = path.pop();
    //        $(this).html('<span class="filenameonly">' + path.join('.') + '</span>' + '<span class="extension"><span class="dot">.</span>' + name + '</span>');
    //    }
    //});

    //if (jQuery().dotdotdot) {
    //    $(".iteminfo:not(.nodot)").dotdotdot({
    //        //	configuration goes here
    //        ellipsis: ' ... ',
    //        after: 'span.extension',
    //        watch: 'window',
    //        height: 40,
    //        wrap: "letter",
    //        callback: function (isTruncated, orgContent) {
    //            if (isTruncated) {
    //                $(this).addClass("truncated");
    //            } else {
    //                $(this).removeClass("truncated");
    //            }
    //        },

    //    });
    //    $(".iteminfo:not(.truncated) .text").each(function () {
    //        $(this).attr("title", "");
    //    });
    //}

    $('.timeinput .seconds').spinner({
        spin: function (event, ui) {
            var $parent = $(this).parents(".timeinput").first();

            if (ui.value >= 60) {
                $(this).spinner('value', ui.value - 60);
                $parent.find('.minutes').spinner('stepUp');

                return false;
            } else if (ui.value < 0) {
                $(this).spinner('value', ui.value + 60);
                $parent.find('.minutes').spinner('stepDown');

                return false;
            }
        },
        change: function (event, ui) {
            var $parent = $(this).parents(".timeinput").first();
            CheckSpinner($parent);
        }
    });
    $('.timeinput .minutes').spinner({

        spin: function (event, ui) {
            var $parent = $(this).parents(".timeinput").first();

            if (ui.value >= 60) {
                $(this).spinner('value', ui.value - 60);
                $parent.find('.hours').spinner('stepUp');

                return false;
            } else if (ui.value < 0) {
                $(this).spinner('value', ui.value + 60);
                $parent.find('.hours').spinner('stepDown');

                return false;
            }
        },
        change: function (event, ui) {
            var $parent = $(this).parents(".timeinput").first();
            CheckSpinner($parent);
        }
    });
    $('.timeinput .hours').spinner({
        min: 0,
        change: function (event, ui) {
            var $parent = $(this).parents(".timeinput").first();
            CheckSpinner($parent);
        }
    });

    $(".coveredradio.enabled.scormsettingtype").each(function () {
        var $parent = $(this);
        var $checks = $parent.find(".wclist input[type='radio']");
        var $buttons = $parent.find(".btnswitch");
        var $dataparent = $($parent.data("parent"));
        $checks.each(function () {
            var idx = $(this).index() / 3;
            if ($(this).is(":checked")) {
                $($buttons.get(idx)).addClass("active");
                $parent.removeClass("original");
                $dataparent.removeClass("original");
                $parent.removeClass("package");
                $dataparent.removeClass("package");
                $parent.removeClass("activity");
                $dataparent.removeClass("activity");

                if ($($buttons.get(idx)).is(".original ")) {
                    $parent.addClass("original");
                    $dataparent.addClass("original");
                }
                if ($($buttons.get(idx)).is(".package")) {
                    $parent.addClass("package");
                    $dataparent.addClass("package");
                }
                if ($($buttons.get(idx)).is(".activity")) {
                    $parent.addClass("activity");
                    $dataparent.addClass("activity");
                }
            } else {
                $($buttons.get(idx)).removeClass("active");
            }
        });


    });

    $(".coveredradio.enabled.scormsettingtype:not(.readonly) .btnswitch").click(function () {

        var $parent = $(this).parents(".coveredradio").first();

        $parent.find(".active").removeClass("active");
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

        var $dataparent = $($parent.data("parent"));

        $parent.removeClass("original");
        $dataparent.removeClass("original");
        $parent.removeClass("package");
        $dataparent.removeClass("package");
        $parent.removeClass("activity");
        $dataparent.removeClass("activity");

        if ($(this).is(".original ")) {
            $parent.addClass("original");
            $dataparent.addClass("original");
        }
        if ($(this).is(".package")) {
            $parent.addClass("package");
            $dataparent.addClass("package");
        }
        if ($(this).is(".activity")) {
            $parent.addClass("activity");
            $dataparent.addClass("activity");
        }

        coveredradiochange($parent);

        return false;
    });

    $(".coveredradio.enabled.scormsettingtype:not(.readonly) input[type='radio']").click(function () {
        var $parent = $(this).parents(".coveredradio").first();
        $parent.find(".active").removeClass("active");

        var $checks = $parent.find(".btnswitch");
        var $dataparent = $($parent.data("parent"));
        if ($parent.is(":not(.readonly)")) {
            //$(this).toggleClass("active");
            var idx = $(this).index() / 3;

            if ($(this).is(":checked")) {
                $($checks.get(idx)).addClass("active");
                $dataparent.removeClass("original");
                $parent.removeClass("package");
                $dataparent.removeClass("package");
                $parent.removeClass("activity");
                $dataparent.removeClass("activity");

                if ($($checks.get(idx)).is(".original ")) {
                    $parent.addClass("original");
                    $dataparent.addClass("original");
                }
                if ($($checks.get(idx)).is(".package")) {
                    $parent.addClass("package");
                    $dataparent.addClass("package");
                }
                if ($($checks.get(idx)).is(".activity")) {
                    $parent.addClass("activity");
                    $dataparent.addClass("activity");
                }
            } else {
                $($checks.get(idx)).removeClass("active");
            }
        }

        var $disable = $parent.data("disable");
        if ($disable != "" && typeof $disable != "undefined") {
            if ($parent.is(".on")) {
                $parent.parents($disable).addClass("on").removeClass("off");
            } else {
                $parent.parents($disable).addClass("off").removeClass("on");
            }
        }

        coveredradiochange($parent);
    });

    /*$(".fieldobject.scormsettings .nestedtree .namelink").click(function () {
        $(this).parents(".nestedtree.root").first().find(".header").removeClass("active");
        $(this).parents(".header").first().addClass("active");
        var org = $(this).data("org");
        if (org == "-1") {
            $(".block.activitysettings").removeClass("hidden");
        } else {
            $(".block.activitysettings").addClass("hidden");
            $(".block.activitysettings[data-org='" + org + "']").removeClass("hidden");
        }
    });*/

    AlphaOmega();

    $(".fieldobject.scormsettings .nestedtree .header.active .namelink").each(function () {
        $(this).parents(".nestedtree.root").first().find(".header").removeClass("active");
        $(this).parents(".header").first().addClass("active");
        var children = $(this).data("children");

        $(".block.activitysettings:not(.alwayson)").addClass("hidden");

        var ids = children.split(",");
        for (var i = 0; i < ids.length; i++) {
            var id = ids[i];

            $(".block.activitysettings[data-id='" + id + "']").removeClass("hidden");
        }

        AlphaOmega();

        /*if (org == "-1") {
            $(".block.activitysettings").removeClass("hidden");
        } else {
            $(".block.activitysettings").addClass("hidden");
            $(".block.activitysettings[data-org='" + org + "']").removeClass("hidden");
        }*/

    });

    $(".fieldobject.scormsettings .nestedtree .namelink").click(function () {
        $(this).parents(".nestedtree.root").first().find(".header").removeClass("active");
        $(this).parents(".header").first().addClass("active");
        var children = $(this).data("children");

        $(".block.activitysettings:not(.alwayson)").addClass("hidden");

        var ids = children.split(",");
        for (var i = 0; i < ids.length; i++) {
            var id = ids[i];

            $(".block.activitysettings[data-id='" + id + "']").removeClass("hidden");
        }

        AlphaOmega();

        /*if (org == "-1") {
            $(".block.activitysettings").removeClass("hidden");
        } else {
            $(".block.activitysettings").addClass("hidden");
            $(".block.activitysettings[data-org='" + org + "']").removeClass("hidden");
        }*/

        return false;
    });

    function AlphaOmega() {
        $(".block.activitysettings").removeClass("alpha").removeClass("omega");
        var idx = 0;
        $(".block.activitysettings:not(.hidden,.alwayson)").each(function () {
            idx += 1;
            if (idx % 2 === 0) {
                $(this).addClass("omega");
            } else {
                $(this).addClass("alpha");
            }
        });

        $(".block.activitysettings.blockmessage").addClass("hidden");
        if ($(".block.activitysettings.hidden:not(.blockmessage)").size() > 0) {
            $(".block.activitysettings.blockmessage").removeClass("hidden");
        }
    }

    $(".block.activitysettings.blockmessage a.seeall").click(function () {

        $(".namelink.package").click();
        return false;
    });

    $(".timeinput").each(function () {

        var value = $(this).find(".hours").val() + ":" + $(this).find(".minutes").val() + ":" + $(this).find(".seconds").val()

        $(this).data("default", value);
        $(this).data("current", value);
    });

    $('.checkchange').each(function (index, element) {
        var $element = $(this);

        if ($element.is("input:text")) {
            var defaultValue = $element.prop("defaultValue");
            $element.data("defaultValue", defaultValue);
        }

        if ($element.is("input:radio")) {
            var defaultChecked = $element.prop("defaultChecked");

            $element.data("defaultChecked", defaultChecked);
        }

        if ($element.is(".coveredradio")) {
            var defaultChecked = $element.find("input:radio:checked").val();

            $element.data("defaultChecked", defaultChecked);
        }

        if ($element.is("input:checkbox")) {
            var defaultChecked = $element.prop("defaultChecked");

            $element.data("defaultChecked", defaultChecked);
        }

        $element.parents(".fieldrow").first().data("changes", 0);
        $element.parents(".fieldobject").first().data("changes", 0);
        $element.parents(".block").first().data("changes", 0);
    });

    function CheckSpinner(element) {

        var $element = element;
        var changed = false;

        element.find(".hours, .minutes, .seconds").each(function () {
            var defaultValue = $(this).data("defaultValue");

            var newValue = $(this).val();

            changed = changed | (defaultValue != newValue);


        });

        var value = $element.find(".hours").val() + ":" + $element.find(".minutes").val() + ":" + $element.find(".seconds").val()

        var defaultvalue = $element.data("default");
        $element.data("current", value);


        if (defaultvalue != value) {

            $element.addClass("changed");
            $element.parents(".fieldrow").first().addClass("changed_by_spinner");
            $element.parents(".fieldobject").first().addClass("changed_by_spinner");
            $element.parents(".block").first().addClass("changed_by_spinner");
            /*ApplyChanged($element.parents(".fieldrow").first(),+1);
            ApplyChanged($element.parents(".fieldobject").first(),+1);
            ApplyChanged($element.parents(".block").first(),+1);*/
        } else {

            $element.removeClass("changed");
            $element.parents(".fieldrow").first().removeClass("changed_by_spinner");
            $element.parents(".fieldobject").first().removeClass("changed_by_spinner");
            $element.parents(".block").first().removeClass("changed_by_spinner");
            /*ApplyChanged($element.parents(".fieldrow").first(),-1);
            ApplyChanged($element.parents(".fieldobject").first(),-1);
            ApplyChanged($element.parents(".block").first(),-1);*/

        }

    }

    function ApplyChanged(element, delta) {
        var val = element.data("changes");

        val += delta;

        element.data("changes", val);
        if (val != 0) {
            element.addClass("changed");
        } else {
            element.removeClass("changed");
        }
    }

    function coveredradiochange(element) {
        var $element = $(element);
        var defaultChecked = $element.data("defaultChecked");
        var newChecked = $element.find("input:radio:checked").val();
        if (newChecked != defaultChecked) {
            $element.addClass("changed");
            //$element.parents(".fieldrow").first().addClass("changed_by_radio");
            $element.parents(".fieldobject").first().addClass("changed_by_radio");
            $element.parents(".block").first().addClass("changed_by_radio");
            /*ApplyChanged($element.parents(".fieldrow").first(),+1);
            ApplyChanged($element.parents(".fieldobject").first(),+1);
            ApplyChanged($element.parents(".block").first(),+1);*/
        } else {
            $element.removeClass("changed");
            //$element.parents(".fieldrow").first().removeClass("changed_by_radio");
            $element.parents(".fieldobject").first().removeClass("changed_by_radio");
            $element.parents(".block").first().removeClass("changed_by_radio");
            /*ApplyChanged($element.parents(".fieldrow").first(),-1);
            ApplyChanged($element.parents(".fieldobject").first(),-1);
            ApplyChanged($element.parents(".block").first(),-1);*/
        }
        return false;

    }
    $('.checkchange').change(function (element) {
        var $element = $(this);

        if ($element.is("input[type='text']")) {

            var defaultValue = $element.data("defaultValue");
            var newValue = $element.val();
            if (newValue != defaultValue) {
                $element.addClass("changed");
                $element.parents(".fieldrow").first().addClass("changed_by_text");
                $element.parents(".fieldobject").first().addClass("changed_by_text");
                $element.parents(".block").first().addClass("changed_by_text");
                /*ApplyChanged($element.parents(".fieldrow").first(),+1);
                ApplyChanged($element.parents(".fieldobject").first(),+1);
                ApplyChanged($element.parents(".block").first(),+1);*/
            } else {
                $element.removeClass("changed");
                $element.parents(".fieldrow").first().removeClass("changed_by_text");
                $element.parents(".fieldobject").first().removeClass("changed_by_text");
                $element.parents(".block").first().removeClass("changed_by_text");
                /*ApplyChanged($element.parents(".fieldrow").first(),-1);
                ApplyChanged($element.parents(".fieldobject").first(),-1);
                ApplyChanged($element.parents(".block").first(),-1);*/
            }
        }

        if ($element.is("input[type='radio']")) {
            var defaultChecked = $element.data("defaultChecked");
            var newChecked = $element.is(":checked");
            if (newChecked != defaultChecked) {
                $element.addClass("changed");
                $element.parents(".fieldrow").first().addClass("changed_by_radio");
                $element.parents(".fieldobject").first().addClass("changed_by_radio");
                $element.parents(".block").first().addClass("changed_by_radio");
                /*ApplyChanged($element.parents(".fieldrow").first(),+1);
                ApplyChanged($element.parents(".fieldobject").first(),+1);
                ApplyChanged($element.parents(".block").first(),+1);*/
            } else {
                $element.removeClass("changed");
                $element.parents(".fieldrow").first().removeClass("changed_by_radio");
                $element.parents(".fieldobject").first().removeClass("changed_by_radio");
                $element.parents(".block").first().removeClass("changed_by_radio");
                /*ApplyChanged($element.parents(".fieldrow").first(),-1);
                ApplyChanged($element.parents(".fieldobject").first(),-1);
                ApplyChanged($element.parents(".block").first(),-1);*/
            }
        }

        if ($element.is("input[type='checkbox']")) {

            var defaultChecked = $element.data("defaultChecked");
            var newChecked = $element.is(":checked");
            if (newChecked != defaultChecked) {
                $element.addClass("changed");
                $element.parents(".fieldrow").first().addClass("changed_by_checkbox" + "_" + $element.data("changed"));
                $element.parents(".fieldobject").first().addClass("changed_by_checkbox" + "_" + $element.data("changed"));
                $element.parents(".block").first().addClass("changed_by_checkbox" + "_" + $element.data("changed"));
                /*ApplyChanged($element.parents(".fieldrow").first(),+1);
                ApplyChanged($element.parents(".fieldobject").first(),+1);
                ApplyChanged($element.parents(".block").first(),+1);*/
            } else {
                $element.removeClass("changed");
                $element.parents(".fieldrow").first().removeClass("changed_by_checkbox" + "_" + $element.data("changed"));
                $element.parents(".fieldobject").first().removeClass("changed_by_checkbox" + "_" + $element.data("changed"));
                $element.parents(".block").first().removeClass("changed_by_checkbox" + "_" + $element.data("changed"));
                /*ApplyChanged($element.parents(".fieldrow").first(),-1);
                ApplyChanged($element.parents(".fieldobject").first(),-1);
                ApplyChanged($element.parents(".block").first(),-1);*/
            }
        }

    });

    $(".multimediasettings .file .namelink").click(function () {
        $(this).parents(".nestedtree.root").find(".active").removeClass("active");
        $(this).parents(".header").first().addClass("active");
        $(".hidselectedfile").val($(this).parents("li.treenode.file").first().attr("id"));
        return false;
    });

    $(".scormstat .collapseall").click(function () {
        $(".collapsable").removeClass("expanded").addClass("collapsed");
    });

    $(".scormstat .expandall").click(function () {
        $(".collapsable").addClass("expanded").removeClass("collapsed");
    });

    $(".fieldobject.scormsettings.readonly").find("input").prop('readonly', true).prop('disabled', true);
    $(".fieldobject.scormsettings.readonly").find(".timeinput").addClass("readonly");
    $(".fieldobject.scormsettings.readonly").find(".timeinput input").spinner("disable");
    $(".timeinput.readonly input").spinner("disable");
    //$(".fieldobject.scormsettings.modal").dialog({
    //    width: 900,
    //    heigh: 600,
    //    modal: true
    //});

    $("a.openmodalscormsettings").click(function () {
        var url = $(this).attr("href");

        var $dialog = $("<div class='dialog withiframe'><iframe id='dialogiframe' src=''></iframe></div>");
        $("form").first().append($dialog);

        $dialog.dialog({
            width: 1000,
            heigh: 800,
            modal: true,
            dialogClass: "alert",
            open: function (ev, ui) {
                $('#dialogiframe').attr('src', url);
            }
        });
        return false;
    });

    $(".toolbar a[data-command]").each(function () {
        var $icon = $(this).find(".icon");
        var cssclass = $icon.attr("class");
        var id = $(this).data("command");

        var $new = $("<a class='aicon shadowcopy enabled-icon' data-command='" + id + "'><span class='" + cssclass + "'>&nbsp;</span></a>");
        $new.data("command", id);

        $(this).after($new);
    });


    function commands(check) {
        var $table = $(check).parents("table.files").first();
        var $checkboxes = $table.find("input:checkbox:checked");

        if ($checkboxes.size() > 0) {

            var commands = "";

            $checkboxes.each(function () {
                var $tr = $(this).parents("tr.file").first();
                var aval = $tr.data("commands");


                commands += aval + ",";

            });

            var avals = commands.split(",");
            console.log(avals);

            var $allcommands = $(".toolbar [data-command]");
            $allcommands.each(function () {
                var id = "" + $(this).data("command");
                console.log(id);
                if (avals.indexOf(id) > -1) {
                    $(this).addClass("enabled-icon").removeClass("disabled-icon");
                } else {
                    $(this).addClass("disabled-icon").removeClass("enabled-icon");
                }
            });
        } else {
            var $allcommands = $(".toolbar [data-command]").addClass("enabled-icon").removeClass("disabled-icon");
        }
    }

    $("tr.file input:checkbox").click(function () {
        commands($(this));
    });

    $("th input:checkbox").click(function () {
        commands($(this));
    });
});