<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/AjaxPortal.Master"
    CodeBehind="EduPathView.aspx.vb" Inherits="Comunita_OnLine.EduPathView" %>

<%@ Register Src="~/Modules/Common/UC/UC_ActionMessages.ascx" TagPrefix="CTRL" TagName="Messages" %>
<%@ Register TagPrefix="CTRL" TagName="ProgressBar" Src="UC/UC_ProgressBar.ascx" %>
<%@ Register TagPrefix="COL" Assembly="Comunita_OnLine" Namespace="Comunita_OnLine.MyUC" %>
<%@ Register Src="UC/UC_HelpStatus.ascx" TagName="helpStatus" TagPrefix="CTRL" %>
<%@ MasterType VirtualPath="~/AjaxPortal.Master" %>
<%@ Register TagPrefix="CTRL" TagName="EditCertificationAction" Src="~/Modules/EduPath/UC/UC_EditCertificationAction.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <!-- HEADER EDUPATH VIEW START-->
    <script type="text/javascript">
        var EnabledSort = <%# me.sortVisibility()%>;
    </script>
     <link href="../../Graphics/Modules/Edupath/css/<%=GetCssFileByType()%>pfstyle.css?v=201605041410lm" rel="Stylesheet" />
    <script type="text/javascript" language="javascript">
        var hash = new Array();
        var code = "ep-pathview";

        //        function cookieName(id) {
        //            return code + "-item-" + id;
        //        }

        //        function createCookie(name, value, days) {
        //            if (days) {
        //                var date = new Date();
        //                date.setTime(date.getTime() + (days * 24 * 60 * 60 * 1000));
        //                var expires = "; expires=" + date.toGMTString();
        //            }
        //            else var expires = "";
        //            document.cookie = name + "=" + value + expires + "; path=/";
        //        }

        //        function readCookie(name) {
        //            var nameEQ = name + "=";
        //            var ca = document.cookie.split(';');
        //            for (var i = 0; i < ca.length; i++) {
        //                var c = ca[i];
        //                while (c.charAt(0) == ' ') c = c.substring(1, c.length);
        //                if (c.indexOf(nameEQ) == 0) return c.substring(nameEQ.length, c.length);
        //            }
        //            return null;
        //        }

        //        function eraseCookie(name) {
        //            createCookie(name, "", -1);
        //        }

        //        function WriteOpenCloseStatus(ItemId, current) {
        //            createCookie(cookieName(ItemId), current);
        //        }

        //        function OpenFromCookie(External, Internal, Class) {
        //            $(External).each(function () {
        //                var itemId = $(this).attr("id");
        //                var value = readCookie(cookieName(itemId));

        //                if (value != "true") {
        //                    //$(this).removeClass(Class).children(Internal).show();
        //                    $(this).find("span.switch").removeClass("collapsed");
        //                    $(this).find("ul.activities").toggleClass("hidden");
        //                    //$(this).parents("li.unit").children("ul.rules").toggleClass("hidden");

        //                }
        //            });
        //        }

        function OpenClose(el, External, Internal, Class) {
            var itemParent = el.parents(External);
            var itemParentId = itemParent.attr("id");
            var itemChildren = itemParent.children(Internal);

            var currentClosed = itemParent.find("span.switch").hasClass(Class);

            //WriteOpenCloseStatus(itemParentId, currentClosed);
        }

        function UpdateUnitOrder(event, ui) {
            CheckOrderButtons();
            var Data = $(this).sortable("serialize");

            $.ajax({
                type: "POST",
                url: "Reordering.asmx/UnitsReorder",
                data: "{'position':'" + Data + "'}",
                processData: false,
                contentType: "application/json; charset=utf-8",
                dataType: "json",

                success: function (msg) {
                    //alert(msg.d);
                },
                error: function (result) {
                    alert("Error: (" + result.status + ') [' + result.statusText + ']');

                }
            });
        }

        function UpdateActivityOrder(event, ui) {
            CheckOrderButtons();
            //alert($(this).sortable("serialize"));

            var Data = $(this).sortable("serialize");

            $.ajax({
                type: "POST",
                url: "Reordering.asmx/ActivitiesReorder",
                data: "{'position':'" + Data + "'}",
                processData: false,
                contentType: "application/json; charset=utf-8",
                dataType: "json",

                success: function (msg) {
                    //alert(msg.d);
                },
                error: function (result) {
                    alert("Error: (" + result.status + ') [' + result.statusText + ']');

                }
            });
        }

        $(document).ready(function () {
            function Change(el) {
                el.prevAll("input[type=radio]").attr("checked", true);
                el.prevAll("input[type=radio]").change();
            }

            $(".amount").live("keyup", function () {
                var wrapper = $(this).parents(".slider-wrapper").first();
                var amountLow = wrapper.find(".amountLow");
                var amountUp = wrapper.find(".amountUp");
                var slider = wrapper.find(".slider");
                var checkbox = wrapper.find("input[type='checkbox']");
                checkbox.attr("checked", "true");
                //wrapper.removeClass("gray");                

                var low = amountLow.val();
                var up = amountUp.val();

                if (low == 0 && up == 100) {
                    checkbox.attr("checked", "");
                }

                slider.slider("values", 0, amountLow.val());
                slider.slider("values", 1, amountUp.val());

                if (low > 100 || low < 0) {
                    amountLow.addClass("ui-state-error");
                } else {
                    amountLow.removeClass("ui-state-error");
                }
                if (up > 100 || up < 0) {
                    amountUp.addClass("ui-state-error");
                } else {
                    amountUp.removeClass("ui-state-error");
                }
            });

            $(".enabler").live("click", function () {
                var wrapper = $(this).parents(".slider-wrapper").first();
                var amountLow = wrapper.find(".amountLow");
                var amountUp = wrapper.find(".amountUp");
                var slider = wrapper.find(".slider");

                var checkValue = $(this).is(':checked');
                var id = slider.attr("id");

                if (!checkValue) {

                    hash[id + "_low"] = amountLow.val();
                    hash[id + "_up"] = amountUp.val();

                    amountLow.val(0);
                    amountUp.val(100);
                    slider.slider("values", 0, 0);
                    slider.slider("values", 1, 100);
                } else {
                    amountLow.val(hash[id + "_low"]);
                    amountUp.val(hash[id + "_up"]);
                    //slider.attr("enable", true);
                    slider.slider("values", 0, amountLow.val());
                    slider.slider("values", 1, amountUp.val());
                }
            });

            $(".Act2").live("change", function () {                
                $(".Act2-lbl").html($(".Act2").children("option:selected").text());
                $("input[id$='HIDact2']").val($(".Act2").first().val().replace("ACT-", ""));                
            });

            $(".rangeTypes input[type=text]").live("focus", function () {
                Change($(this));
            });

            $(".rangeTypes input[type=text]").live("blur", function () {
                Change($(this));
            });

            $(".rangeTypes input[type=text]").live("click", function () {
                Change($(this));
            });

            $(".rangeTypes input[type=text]").live("keyup", function () {
                Change($(this));
            });

            $(".rangeTypes input[type=radio]").live("change", function () {
                var value = $(this).val();
                var values = new Array();
                var operators = new Array();
                var html = "";
                var k = 0;
                var j = 0;

                $(this).nextUntil(".rangeTypes input[type=radio]").filter("input[type=text]").each(function () {

                    var c = $(this).val();

                    values[k] = c;
                    k++;

                });

                $(this).nextUntil(".rangeTypes input[type=radio]").filter(".operator").each(function () {
                    var c = $(this).html();
                    operators[j] = c;
                    j++;
                    //alert('operators');
                });

                switch (values.length) {
                    case 1:
                        html = "x " + operators[0] + " " + values[0];
                        //alert('case1');
                        break;
                    case 2:
                        html = values[0] + " " + operators[1] + " x " + operators[1] + " " + values[1];
                        //alert('case2');
                        break;
                    default:
                }

                $(".condition").each(function () {
                    $(this).html(html);
                    //alert('condition');
                });
            });

            $("ul.units>li.unit>span.buttons>span.switch").live("click", function () {
                $(this).parents("li.unit").children("ul.activities").toggleClass("hidden");
                $(this).toggleClass("collapsed");

                OpenClose($(this), "li.unit", "ul.activities", "collapsed");
            });

            $("#addRule").dialog({ autoOpen: false,
                appendTo: "form",
                width: 600,
                height: 400,
                minHeight: 400,
                minWidth: 600,
                //modal: true,
                title: "Add Rule",
                open: function (type, data) {
                    //$(this).parent().appendTo("form");
                }
            });

            $('.switchAct').dialog({
                appendTo: "form",
                autoOpen: false,
                draggable: true,
                modal: true,
                title: "",
                width: 380,
                height: 180,
                minHeight: 110,
                minWidth: 300,
                zIndex: 99999,

                open: function (type, data) {
                    //$(this).parent().appendTo("form");
                }
            });

            $('.switchMessage').dialog({
                appendTo: "form",
                autoOpen: false,
                draggable: true,
                modal: false,
                title: "",
                width: 350,
                height: 140,
                minHeight: 110,
                minWidth: 300,
                zIndex: 99999,
                open: function (type, data) {
                    //$(this).parent().appendTo("form");
                }
            });

            $(".closeDialog").live("click", function () {
                $(this).parents(".dialog").first().dialog("close");
                return false;
            });

            $(".switchOnOff").live("click", function () {
                if ($(this).is(".enable")) {
                    $(this).switchClass("enable", "disable", 0);
                } else {
                    $(this).switchClass("disable", "enable", 0);
                }
            });

            $(".needConfirm").live("click", function () {
                $(this).parents("li").first().addClass("ui-state-highlight").addClass("ui-state-disabled");
                var ret = confirm("Sei sicuro di voler cancellare?");
                $(this).parents("li").first().removeClass("ui-state-highlight").removeClass("ui-state-disabled");
                return ret;
            });


            $('.dialog.dlgeditoptions').dialog({
                appendTo:"form",
                closeOnEscape: false,
                autoOpen: true,
                draggable: true,
                modal: true,
                title: "",
                width: 820,
                height: 400,
                minHeight: 200,
                //                minWidth: 700,
                zIndex: 1000,
                open: function (type, data) {
                    //                $(this).dialog('option', 'width', 700);
                    //                $(this).dialog('option', 'height', 600);
                    //$(this).parent().appendTo("form");
                    $(".ui-dialog-titlebar-close", this.parentNode).hide();
                }

            });

            $(".closedlgeditoptions").click(function () {
                $(".dialog.dlgeditoptions").dialog("close");
                return false;
            });

            InitializeAll();
            //$('#addRule').parent().appendTo('#dialog_target');
        });

        function SetupSliders() {
            $(".slider").each(function () {
                var id = $(this).attr("id");

                var wrapper = $(this).parents(".slider-wrapper").first();

                var amountLow = wrapper.find(".amountLow").first();
                var amountUp = wrapper.find(".amountUp").first();
                var checkbox = wrapper.find("input[type='checkbox']").first();




                var low = amountLow.val();
                var up = amountUp.val();

                if (low > 0 || up < 100) {
                    checkbox.attr("checked", "true");
                } else {
                    checkbox.attr("checked", "");
                }
                hash[id + "_low"] = low;
                hash[id + "_up"] = up;

                $(this).slider("values", 0, low);
                $(this).slider("values", 1, up);
            });
        }

        function InitializeAll() {
            $(".slider").each(function () {
                var id = $(this).attr("id");

                hash[id + "_low"] = 0;
                hash[id + "_up"] = 100;
            });

            $(".slider").slider({
                range: true,
                min: 0,
                max: 100,
                values: [0, 100],
                slide: function (event, ui) {
                    var wrapper = $(this).parents(".slider-wrapper").first();

                    var amountLow = wrapper.find(".amountLow").first();
                    var amountUp = wrapper.find(".amountUp").first();
                    var checkbox = wrapper.find("input[type='checkbox']").first();
                    checkbox.attr("checked", "true");
                    amountLow.val(ui.values[0]);
                    amountUp.val(ui.values[1]);

                    var low = amountLow.val();
                    var up = amountUp.val();

                    if (low == 0 && up == 100) {
                        checkbox.attr("checked", "");
                    }

                    amountLow.removeClass("ui-state-error");
                    amountUp.removeClass("ui-state-error");
                }
            });

            $("ul.activities").not(":has(li.activity)").not(":has(li.note)").parents("li.unit").addClass("empty");

            $("ul.units").sortable({
                item: '.unit',
                handle: 'span.moveUnit',
                update: UpdateUnitOrder,

                helper: 'clone',
                tolerance: 'pointer',
                axis: 'y',
                opacity: 0.5,
                scroll: true,
                forceHelperSize: true,
                placeholder: 'ui-state-highlight short',
                forcePlaceholderSize: true
            });

            $("ul.activities").sortable({
                item: '.activity',
                handle: 'span.moveActivity',
                update: UpdateActivityOrder,

                helper: 'clone',
                tolerance: 'pointer',
                axis: 'y',
                opacity: 0.5,
                scroll: true,
                forceHelperSize: true,
                placeholder: 'ui-state-highlight short',
                forcePlaceholderSize: true
            });

            $("li.unit").not(".current").children("ul.activities").addClass("hidden");
            //$("li.unit").not(".current").children("ul.rules").addClass("hidden");
            $("li.unit").not(".current").find("span.switch").toggleClass("collapsed");

            //TreeStatus();

            //OpenFromCookie("li.unit", "ul.activities", "hidden");

            CheckOrderButtons();
        }

        function CheckOrderButtons() {
            //attenzione: se si ripristina, le classi css han cambiato nome
            //            $(".moveUp").removeClass("invisible");
            //            $(".moveDown").removeClass("invisible");
            ////                        $("li.unit").first().children().children().children(".moveUp").addClass("invisible");
            ////                        $("li.unit").last().children().children().children(".moveDown").addClass("invisible");

            //                        //$("li.unit:first>div.title>span.buttons>.moveUp").first().addClass("invisible");
            //                        //$("li.unit:last>div.title>span.buttons>.moveDown").first().addClass("invisible");

            ////            $("li.unit:last>div.title>span.buttons>a.moveDown>img")
            ////            $("li.unit:first>div.title>span.buttons>a.moveUp>img")
            ////                        $("li.unit:not(:last)>div.title>span.buttons>a.moveDown").first().click(function () {
            ////                            return true;
            ////                        });
            ////                        $("li.unit:not(:first)>div.title>span.buttons>.moveUp").first().click(function () {
            ////                            return false;
            ////                        });
            ////                        $("li.unit:last>div.title>span.buttons>a.moveDown").first().click(function () {                            
            ////                            return false;
            ////                        });
            ////                        $("li.unit:first>div.title>span.buttons>.moveUp").first().click(function () {
            ////                            return false;
            ////                        });
            ////            $("li.unit:first>div.title>span.buttons>.moveUp").addClass("invisible");
            ////            $("li.unit:last>div.title>span.buttons>.moveDown").addClass("invisible");
            //            $("li.unit").each(function () {
            //                $(this).find("li.activity").first().find(".moveUp").addClass("invisible");
            //                $(this).find("li.activity").last().find(".moveDown").addClass("invisible");
            //            });
        }

        function showDialog(id) {          
            InitializeAll();       
            $('#' + id).dialog("open");       
        }

        function closeDialog(id) {
            InitializeAll();
            $('#' + id).dialog("close");
        }

        function showDialogSender(id, sender) {
            InitializeAll();
            
            SetupSliders();

            var values = sender.split(";");

            var unit = values[0];
            var activity = values[1];

            //alert(unit + " ; " + activity);

            //verificare:
            //            if (unit == null && activity != null) {
            //                unit = activity;
            //                activity = null;

            //                //unit rule

            //            } else {
            //                //activity rule                

            //                var value = $("#ACT-" + activity).html();
            //                alert(value);
            //                $(".Act1").val(value);

            //                $(".Act2-lbl").html($(".Act2").children("option:selected").text());
            //                $(".Act1-lbl").html($(".Act1").val());
            //                $("input[id$='HIDact2']").val($(".Act2").first().val().replace("ACT-", ""));
            //                alert(activity);
            //                $("input[id$='HIDact1']").val(activity);

            //                        }


            //fine verifica
            if (unit != null && activity == null) {
                //unit = activity;
                activity = unit;

                //unit rule

                var value = $("#UNIT_" + activity).html();
                    
                $(".Act1").val(value);

                $(".Act2-lbl").html($(".Act2").children("option:selected").text());
                $(".Act1-lbl").html($(".Act1").val());
                $("input[id$='HIDact2']").val($(".Act2").first().val().replace("ACT-", ""));
                    
                $("input[id$='HIDact1']").val(activity);

                            


            } else {
                //activity rule                

                var value = $("#ACT-" + activity).html();
                    
                $(".Act1").val(value);

                $(".Act2-lbl").html($(".Act2").children("option:selected").text());
                $(".Act1-lbl").html($(".Act1").val());
                $("input[id$='HIDact2']").val($(".Act2").first().val().replace("ACT-", ""));
                    
                $("input[id$='HIDact1']").val(activity);

            }

            


            //            alert($("input[id$='HIDact1']").val());
            //            alert($("input[id$='HIDact2']").val());


            //$("li.unit[id$='-" + unit + "']").children("ul.activities").removeClass("hidden");
            //$("li.unit[id$='-" + unit + "']").find("span.switch").removeClass("collapsed");


            if ($.browser.msie) {
                $('#' + id).dialog("option", "height", 820);
            }
            $('#' + id).dialog("open");

        }

        function closeDialogSender(id, sender) {
            $('#' + id).dialog("close");
        }


    </script>
    <%--<script>
        $(document).ready(function () {

            var hash = new Array();

            $(".slider").each(function () {
                var id = $(this).attr("id");

                hash[id + "_low"] = 0;
                hash[id + "_up"] = 100;
            });

            //$(".dialog").dialog({ title: "Add Rule", autoOpen: true, width: "auto" });

            $(".slider").slider({
                range: true,
                min: 0,
                max: 100,
                values: [0, 100],
                slide: function (event, ui) {
                    var wrapper = $(this).parents(".slider-wrapper").first();
                    var amountLow = wrapper.find(".amountLow");
                    var amountUp = wrapper.find(".amountUp");
                    var checkbox = wrapper.find("input[type='checkbox']");
                    checkbox.attr("checked", "true");
                    amountLow.val(ui.values[0]);
                    amountUp.val(ui.values[1]);

                    var low = amountLow.val();
                    var up = amountUp.val();

                    if (low == 0 && up == 100) {
                        checkbox.attr("checked", "");
                    }

                    amountLow.removeClass("ui-state-error");
                    amountUp.removeClass("ui-state-error");
                }

            });

            $(".openDialog").click(function () {
                $(".dialog").dialog("open");
                return false;
            });

            $(".amount").live("keyup",function () {
                var wrapper = $(this).parents(".slider-wrapper").first();
                var amountLow = wrapper.find(".amountLow");
                var amountUp = wrapper.find(".amountUp");
                var slider = wrapper.find(".slider");
                var checkbox = wrapper.find("input[type='checkbox']");
                checkbox.attr("checked", "true");
                //wrapper.removeClass("gray");                

                var low = amountLow.val();
                var up = amountUp.val();

                if (low == 0 && up == 100) {
                    checkbox.attr("checked", "");
                }

                slider.slider("values", 0, amountLow.val());
                slider.slider("values", 1, amountUp.val());

                if (low > 100 || low < 0) {
                    amountLow.addClass("ui-state-error");
                } else {
                    amountLow.removeClass("ui-state-error");
                }
                if (up > 100 || up < 0) {
                    amountUp.addClass("ui-state-error");
                } else {
                    amountUp.removeClass("ui-state-error");
                }
            });


            $(".enabler").live("click",function () {
                var wrapper = $(this).parents(".slider-wrapper").first();
                var amountLow = wrapper.find(".amountLow");
                var amountUp = wrapper.find(".amountUp");
                var slider = wrapper.find(".slider");

                var checkValue = $(this).is(':checked');
                var id = slider.attr("id");

                if (!checkValue) {

                    hash[id + "_low"] = amountLow.val();
                    hash[id + "_up"] = amountUp.val();

                    amountLow.val(0);
                    amountUp.val(100);
                    slider.slider("values", 0, 0);
                    slider.slider("values", 1, 100);
                    slider.attr("enable", false);
                } else {

                    //                    if (hash[id + "_low"] < 0 || hash[id + "_low"] > 100 || hash[id + "_low"]=undefined) {
                    //                        hash[id + "_low"] = 0;
                    //                    }
                    //                    if (hash[id + "_up"] < 0 || hash[id + "_up"] > 100 || hash[id + "_up"]=undefined) {
                    //                        hash[id + "_up"] = 100;
                    //                    }
                    amountLow.val(hash[id + "_low"]);
                    amountUp.val(hash[id + "_up"]);
                    slider.attr("enable", true);
                    slider.slider("values", 0, amountLow.val());
                    slider.slider("values", 1, amountUp.val());
                }
            });
        });
    </script>--%>
    <!-- HEADER EDUPATH VIEW END-->
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="CPHservice" runat="server">
    <asp:UpdatePanel ID="UDPpath" UpdateMode="Conditional" ChildrenAsTriggers="true"
        runat="server">
        <ContentTemplate>
            <asp:MultiView ID="MLVeduPathView" runat="server" ActiveViewIndex="0">
                <asp:View ID="VIWeduPathView" runat="server">
                    <div class="DivEpButton">
                        <asp:LinkButton ID="LKBdeleteEP" runat="server" Visible="false" Text="del**"></asp:LinkButton>
                        <asp:HyperLink ID="HYPnewUnit" runat="server" Text="**new unit" CssClass="Link_Menu"
                            Visible="false"></asp:HyperLink>
                        <asp:HyperLink ID="HYPnewUnitNote" runat="server" Visible="false" Text="*newUnitNote"
                            CssClass="Link_Menu" />
                        <asp:HyperLink ID="HYPvalidateByAct" runat="server" Visible="false" CssClass="Link_Menu" />
                        <asp:HyperLink ID="HYPlistEduPath" runat="server" Text="**list edu path" CssClass="Link_Menu"></asp:HyperLink>
                    </div>
                    <CTRL:Messages runat="server" ID="CTRLmoduleStatusMessage" Visible="false" />
                    <CTRL:Messages runat="server" ID="CTRLgenericMessage" Visible="false" />
                    <div id="DIVprogressBar" runat="server">
                        <CTRL:ProgressBar ID="CTRLprogressBar" runat="server"></CTRL:ProgressBar>
                    </div>
                    <br />
                    <asp:Panel class="<%# Me.PlayerCssVisibility()%>" runat="server" ID="SPANactivitiesInProgress">
                        <span class="titleInProgress ">
                            <asp:Label ID="LBplayerTitle" runat="server" CssClass="" Text="***attività in corso"></asp:Label>
                        </span><span class="activitiesInProgress"><span class="activityInProgressTitle">
                            <asp:Label ID="LBcurrentActivityName" runat="server" CssClass="Titolo_dettagliSmall"></asp:Label>
                        </span>
                            <br />
                            <asp:Label ID="LBdateTitle" runat="server" Text="**date:" CssClass="Titolo_dettagliSmall"></asp:Label>
                            <asp:Label ID="LBdate" runat="server" Text="**da fare tra .. e .." CssClass="dettagli_CampoSmall"></asp:Label><br />
                            <asp:Label ID="LBdescriptionTitle" runat="server" Text="**Description:" CssClass="Titolo_dettagliSmall"></asp:Label>
                            <asp:Label ID="LBdescription" runat="server" Text="" CssClass="dettagli_CampoSmall"></asp:Label><br />
                            <asp:Label ID="LBstatusPlayerTitle" runat="server" Text="**Status:" CssClass="Titolo_dettagliSmall"></asp:Label>
                            <asp:Label ID="LBstatusPlayer" runat="server" CssClass="dettagli_CampoSmall" />
                            <asp:Image ID="IMGstatus" runat="server" Visible="false" />
                            <span class="openStatus inline img_span ico_help_s"></span>

                        </span>
                    </asp:Panel>
                    <br />
                    <span class="titleFuture">
                        <asp:Label ID="LBunitList" runat="server" Text="**UnitList" Visible="false"></asp:Label></span>
                    <asp:Repeater ID="RPunit" runat="server" OnItemCommand="RPunit_ItemCommand">
                        <HeaderTemplate>
                            <ul class="units <%#Me.SetIfUnitIsUnmovable()%>>">
                        </HeaderTemplate>
                        <ItemTemplate>
                            <li id="UNIT_<%# DataBinder.Eval(Container.DataItem,"Id")%>" class="unit  current">
                                <span class="buttons floatLeftIE "><span id="SPswitch" runat="server" class="button switch">S</span><asp:Literal runat="server"
                                    ID="LITmoveUnit"></asp:Literal>
                                    <asp:Image ID="IMGmandatory" runat="server" Visible="false" />
                                    <asp:LinkButton ID="LKBmandatory" runat="server" Visible="false" CommandName="mandatory"></asp:LinkButton>
                                </span>
                                <div class="title FloatRightIE " runat="server" id="DIVunitName">
                                    <span class="leftSide">
                                        <asp:Label ID="LBunitNumber" runat="server" Visible="false" />
                                        <asp:Label ID="LBunit" runat="server" Visible="false" CssClass="Titolo_campoSmall"
                                            Text="description***"></asp:Label>
                                        <asp:Label ID="LBunitWeight" runat="server" />
                                        <asp:Label ID="LBdescription" runat="server" Visible="false" CssClass="renderedtext" />
                                    </span>
                                    <span class="buttons rightSide leftBordered">
                                        <asp:LinkButton ID="LKBdeleteUnit" runat="server" Text="del**" CommandName="unitDelete"
                                            Visible="false" CssClass="img_link ico_delete_m "></asp:LinkButton>
                                        <asp:HyperLink ID="HYPupdateUnit" runat="server" Text="**Stat" Visible="false" CssClass="img_link ico_edit_m"></asp:HyperLink>
                                        <asp:LinkButton ID="LKBvisibUnit" runat="server" Text="Visible**" CommandName="visibility"
                                            Visible="false"></asp:LinkButton>
                                        <asp:Image ID="IMGvisibility" runat="server" Visible="false" />
                                        <asp:LinkButton ID="LKBaddUnitRule" runat="server" Text="Add Rule**" CommandName="addUnitRule"
                                            CommandArgument='<%# DataBinder.Eval(Container.DataItem,"Id")%>' Visible="false"></asp:LinkButton>
                                        <asp:LinkButton ID="LKBupU" runat="server" Visible="false" CommandName="moveUpU"
                                            CssClass=" img_link ico_moveup_s"></asp:LinkButton>
                                        <asp:LinkButton ID="LKBdownU" runat="server" Visible="false" CommandName="moveDownU"
                                            CssClass=" img_link ico_movedown_s"></asp:LinkButton>
                                        <asp:Image ID="IMGstatus" runat="server" Visible="false" />
                                    </span><span class="expireDate rightSide leftBordered">&nbsp; </span>
                                    <div id="DIVunitDescription" runat="server" class="renderedtext" visible="false">
                                        <asp:Label ID="LBunitDesc" runat="server"></asp:Label>
                                    </div>
                                </div>
                                <asp:Repeater ID="RPactivity" runat="server" OnItemCommand="RPactivity_ItemCommand">
                                    <HeaderTemplate>
                                        <ul class="activities <%#Me.SetIfActivityIsUnmovable()%>">
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <li id="ACT-<%# DataBinder.Eval(Container.DataItem,"Id")%>" class="<%#Me.GetActivityCssClass(DataBinder.Eval(Container.DataItem,"Status")) %>">
                                            <span class="buttons floatLeftIE ">
                                                <asp:Literal runat="server" ID="LITmoveActivity"></asp:Literal>
                                                <asp:Image ID="IMGmandatory" runat="server" Visible="false" />
                                                <asp:LinkButton ID="LKBmandatory" runat="server" Visible="false" CommandName="mandatory"></asp:LinkButton>
                                            </span>
                                            <div class="title FloatRightIE " runat="server" id="DIVactName">
                                                <span class="leftSide">
                                                    <asp:Label ID="LBactNumber" runat="server" />
                                                    <asp:HyperLink ID="HYPactName" runat="server" CssClass="Titolo_campoSmall" Text="description***"
                                                        Visible="false"></asp:HyperLink>
                                                    <asp:Label ID="LBactName" runat="server" CssClass="Titolo_campoSmall" Text="name"
                                                        Visible="false" />
                                                    <asp:Label ID="LBactWeight" runat="server" Visible="false" />
                                                    <asp:Label ID="LBdescription" runat="server" CssClass="renderedtext" Visible="false" />
                                                </span><span class="buttons rightSide leftBordered">
                                                    <asp:LinkButton ID="LKBdeleteAct" runat="server" Visible="false" Text="del**" CommandName="activityDelete" CssClass="img_link ico_delete_s"></asp:LinkButton>
                                                    <asp:HyperLink ID="HYPupdateAct" runat="server" Text="**Update" Visible="false" CssClass="img_link ico_edit_s"></asp:HyperLink>
                                                    <asp:LinkButton ID="LKBvisibActivity" runat="server" Text="Visible**" CommandName="visibility"
                                                        Visible="false"></asp:LinkButton>
                                                    <asp:Image ID="IMGvisibility" runat="server" Visible="false" />
                                                    <%--<%# me.TempUnitId%>--%>
                                                    <asp:LinkButton ID="LKBaddActivityRule" runat="server" Text="Add Rule**" CommandName="addActivityRule"
                                                        CommandArgument='<%# DataBinder.Eval(Container.DataItem,"Id")%>' Visible="false"></asp:LinkButton>
                                                    <asp:LinkButton ID="LKBupA" runat="server" Visible="false" CommandName="moveUpA"
                                                        CssClass="img_link ico_moveup_s"></asp:LinkButton>
                                                    <asp:LinkButton ID="LKBdownA" runat="server" Visible="false" CommandName="moveDownA"
                                                        CssClass="img_link ico_movedown_s"></asp:LinkButton>
                                                    <asp:Image ID="IMGstatus" runat="server" Visible="false" />
                                                </span><span class="expireDate rightSide leftBordered">
                                                    <asp:Label ID="LBdate" runat="server" CssClass="dettagli_CampoSmall"></asp:Label>
                                                </span>
                                            </div>
                                            <ul id="ULactRules" runat="server" class=" rules ">
                                                <asp:Repeater ID="RPrule" runat="server" OnItemCommand="RPrule_ItemCommand">
                                                    <ItemTemplate>
                                                        <li class="rule"><span class="buttons">
                                                            <asp:LinkButton ID="LKBdeleteRule" runat="server" Visible="false" Text="del**" CommandName="ruleDelete"
                                                                CommandArgument="" CssClass="img_link ico_delete_s"></asp:LinkButton>
                                                            <asp:LinkButton ID="LKBeditRule" runat="server" Visible="false" Text="edit**" CommandName="ruleEdit"
                                                                CommandArgument="" CssClass="img_link ico_edit_m"></asp:LinkButton>
                                                        </span>
                                                            <asp:Label ID="LBrule" runat="server" CssClass="Titolo_campoSmall" Text="rule***"></asp:Label>
                                                        </li>
                                                    </ItemTemplate>
                                                </asp:Repeater>
                                            </ul>
                                        </li>
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        </ul>
                                    </FooterTemplate>
                                </asp:Repeater>
                                <span class="activity addActivity clearfix">
                                    <asp:HyperLink ID="HYPnewAct" runat="server" Text="**new act" CssClass="Link_Menu"
                                        Visible="false"></asp:HyperLink>
                                    <asp:HyperLink ID="HYPnewActNote" runat="server" Visible="false" Text="*newUnitNote"
                                        CssClass="Link_Menu" />
                                </span>
                                <ul id="ULunitRules" runat="server" class=" rules ">
                                    <asp:Repeater ID="RPunitrule" runat="server" OnItemCommand="RPunitrule_ItemCommand">
                                        <ItemTemplate>
                                            <li class="rule"><span class="buttons">
                                                <asp:LinkButton ID="LKBunitDeleteRule" runat="server" Visible="false" Text="del**"
                                                    CommandName="ruleDelete" CommandArgument="" CssClass="img_link ico_delete_m"></asp:LinkButton>
                                                <asp:LinkButton ID="LKBunitEditRule" runat="server" Visible="false" Text="edit**"
                                                    CommandName="ruleEdit" CommandArgument=""></asp:LinkButton>
                                            </span>
                                                <asp:Label ID="LBunitrule" runat="server" CssClass="Titolo_campoSmall" Text="rule***"></asp:Label>
                                            </li>
                                        </ItemTemplate>
                                    </asp:Repeater>
                                </ul>
                            </li>
                            </li>
                        </ItemTemplate>
                        <FooterTemplate>
                            </ul>
                        </FooterTemplate>
                    </asp:Repeater>
                </asp:View>
                <asp:View ID="VIWerror" runat="server">
                    <div id="DVerror" align="center">
                        <div class="DivEpButton">
                            <asp:HyperLink ID="HYPerror" runat="server" CssClass="Link_Menu" />
                        </div>
                        <CTRL:Messages runat="server" ID="CTRLerrorMessage" />
                    </div>
                </asp:View>
                <asp:View ID="VIWmessages" runat="server">
                    <CTRL:Messages runat="server" ID="CTRLmessages" />
                </asp:View>
            </asp:MultiView>
        </ContentTemplate>
    </asp:UpdatePanel>
    <div class="dialog dlgeditoptions" id="DVconfirmEditing" runat="server" visible="false">
        <div class="fieldobject">
            <div class="fieldrow title">
                <div class="description">
                    <asp:Label ID="LBeditingOptions" runat="server">*Si sta cercando di accedere ad un mooc con statistiche già registrate, selezionare una delle opzioni sottostanti prima di procedere</asp:Label>
                </div>
            </div>
            <div class="fieldrow commandoptions clearfix">
                <div class="commandoption left">
                    <asp:Button Text="* Visualizza in sola lettura" runat="server" CssClass="commandbutton editoption1 closedlgeditoptions" ID="BTNreadonlyOption" />
                    <asp:Label ID="LBreadonlyOption" runat="server" CssClass="commanddescription">* Visualizza in sola lettura: visualizza il moocs in sola lettura. Non saranno consentite modifiche alla struttura del mooc.</asp:Label>
                </div>
                <div class="commandoption right">
                    <asp:Button Text="* Consenti modifiche" runat="server" CssClass="commandbutton editoption2" ID="BTNeditOption" />
                    <asp:Label ID="LBeditOption" runat="server" CssClass="commanddescription">* Modifica: verranno rimosse tutte le statistiche utente raccolte ad oggi, il mooc sarà bloccato e le modifiche saranno abilitate. Per consentire agli utenti di riprendere il mooc dovrai successivamente togliere il blocco impostato automaticamente.</asp:Label>
                </div>
            </div>
        </div>
    </div>
    <div id="dialog_target">
    </div>
    <div id="addRule" class="addRule">
        <%--<img src="img/addrule_bgnd.png" height="40px" class="background" />--%>
        <asp:UpdatePanel ID="UDPaddRule" UpdateMode="Conditional" ChildrenAsTriggers="true"
            runat="server">
            <ContentTemplate>
                <div class="divblock">
                    <asp:HiddenField runat="server" ID="HIDact1" />
                    <asp:HiddenField runat="server" ID="HIDact2" />
                    <span class="Act1-lbl">
                        <asp:Literal runat="server" ID="LITactivity1c" Text="*Regole di attivazione di {0}"></asp:Literal></span>
                    <ul id="shownRules" class="shownrules">
                        <asp:Repeater ID="RPshownRules" runat="server" Visible="true">
                            <ItemTemplate>
                                <li>
                                    <asp:Label runat="server" ID="LBrule" Visible="true"></asp:Label></li>
                            </ItemTemplate>
                        </asp:Repeater>
                    </ul>
                    <hr />
                    <span class="activitiesChoose"><span class="activityChoose"><span>
                        <asp:Literal runat="server" ID="LITactivity3" Text="*L'attività"></asp:Literal></span>
                        <asp:Literal runat="server" ID="LITactivity1" Text=""></asp:Literal>
                        <%--<select id="CAct1" class="Act1" disabled="disabled">
                    <asp:Repeater ID="RPactivity1" runat="server" Visible="true">
                        <ItemTemplate>
                            <option id='<asp:Literal runat="server" ID="LITactivity1Id"></asp:Literal>'><asp:Literal runat="server" ID="LITactivity1"></asp:Literal></option>
                        </ItemTemplate>
                    </asp:Repeater>
                </select> --%>
                    </span><span class="activityChoose"><span>
                        <asp:Literal runat="server" ID="LITactivity4" Text="*è attiva se"></asp:Literal></span>
                        <select id="CAct2" class="Act2">
                            <asp:Repeater ID="RPactivity2" runat="server" Visible="true">
                                <ItemTemplate>
                                    <%--<option value='<asp:Literal runat="server" ID="LITactivity2Id"></asp:Literal>' <% %> >--%>
                                    <asp:Literal runat="server" ID="LITactivity2"></asp:Literal>
                                    </option>
                                </ItemTemplate>
                            </asp:Repeater>
                        </select>
                    </span></span>
                    <ul class="Layout">
                        <li>
                            <div class="slider-wrapper" id="DIVruleSlider" runat="server">
                                <input type="checkbox" class="enabler" id="checkboxA" name="checkboxA" />Completamento<br />
                                <asp:TextBox runat="server" ID="TXBcompleteLow" CssClass="amount amountLow FloatLeft"
                                    Text="0"></asp:TextBox>
                                <%--<input type="text" class="amount amountLow FloatLeft" id="amountLowA" value="0" />--%>
                                <div id="sliderA" class="slider">
                                </div>
                                <%--<input type="text" class="amount amountUp FloatRight" id="amountUpA" value="100" />--%>
                                <asp:TextBox runat="server" ID="TXBcompleteUp" CssClass="amount amountUp FloatRight"
                                    Text="100"></asp:TextBox>
                            </div>
                        </li>
                        <li></li>
                        <li>
                            <div class="slider-wrapper" runat="server" id="DIVmarkSlider">
                                <input type="checkbox" class="enabler" id="checkboxB" name="checkboxB" />Voto<br />
                                <asp:TextBox runat="server" ID="TXBmarkLow" CssClass="amount amountLow FloatLeft"
                                    Text="0"></asp:TextBox>
                                <%--<input type="text" class="amount amountLow FloatLeft"  id="amountLowB" value="0" />--%>
                                <div id="sliderB" class="slider">
                                </div>
                                <%--<input type="text" class="amount amountUp FloatRight"  id="amountUpB"value="100" />--%>
                                <asp:TextBox runat="server" ID="TXBmarkUp" CssClass="amount amountUp FloatRight"
                                    Text="100"></asp:TextBox>
                            </div>
                        </li>
                    </ul>
                    <hr />
                    <div id="Div1" runat="server" visible="false">
                        <asp:Literal runat="server" ID="LITactivity5" Text="*è completata con un valore percentuale: "></asp:Literal>
                        <span class="rangeTypes"><span class="rangeType">
                            <asp:RadioButton GroupName="rangeType" ID="RBrangeTypeLT" runat="server" />
                            <%--<input type="radio" name="rangeType" value="a" id="r1" />--%>
                            <span class="operator">&le;</span>
                            <asp:TextBox runat="server" ID="TXBlowerThan" Text="100"></asp:TextBox>
                            <%--<input type="text" />--%>% </span>
                            <br />
                            <span class="rangeType">
                                <asp:RadioButton GroupName="rangeType" ID="RBrangeTypeGT" runat="server" />
                                <%--<input type="radio" name="rangeType" value="b" id="r2" />--%>
                                <span class="operator">&ge;</span>
                                <asp:TextBox runat="server" ID="TXBgreaterThan" Text="0"></asp:TextBox>
                                <%--<input type="text" />--%>% </span>
                            <br />
                            <span class="rangeType">
                                <asp:RadioButton GroupName="rangeType" ID="RBrangeTypeBT" runat="server" />
                                <%--<input type="radio" name="rangeType" value="c" id="r3" />--%>
                                <span class="operator">&ge;</span>
                                <asp:TextBox runat="server" ID="TXBlowerBound" Text="0"></asp:TextBox>
                                <%--<input type="text" />--%>% e <span class="operator">&le;</span>
                                <asp:TextBox runat="server" ID="TXBupperBound" Text="100"></asp:TextBox>
                                <%--<input type="text" />--%>% </span></span>
                        <asp:CompareValidator runat="server" ID="COVbetween" ControlToValidate="TXBlowerBound"
                            ControlToCompare="TXBupperBound" Display="Dynamic" Type="String" Operator="LessThanEqual"
                            ErrorMessage="*">
                        </asp:CompareValidator>
                        <asp:CompareValidator runat="server" ID="COVlowerThan" Operator="DataTypeCheck" Type="Integer"
                            Display="Dynamic" ControlToValidate="TXBlowerThan" ErrorMessage="no lettere!*" />
                        <asp:RangeValidator runat="server" ID="RVlowerThan" MinimumValue="0" MaximumValue="100"
                            Display="Dynamic" Type="Integer" ControlToValidate="TXBlowerThan" ErrorMessage="fuori range*" />
                        <asp:CompareValidator runat="server" ID="COVgreaterThan" Operator="DataTypeCheck"
                            Type="Integer" Display="Dynamic" ControlToValidate="TXBgreaterThan" ErrorMessage="no lettere!*" />
                        <asp:RangeValidator runat="server" ID="RVgreaterThan" MinimumValue="0" MaximumValue="100"
                            Display="Dynamic" Type="Integer" ControlToValidate="TXBgreaterThan" ErrorMessage="fuori range*" />
                        <asp:CompareValidator runat="server" ID="COVlowerBound" Operator="DataTypeCheck"
                            Type="Integer" Display="Dynamic" ControlToValidate="TXBlowerBound" ErrorMessage="no lettere!*" />
                        <asp:RangeValidator runat="server" ID="RVlowerBound" MinimumValue="0" MaximumValue="100"
                            Display="Dynamic" Type="Integer" ControlToValidate="TXBlowerBound" ErrorMessage="fuori range*" />
                        <asp:CompareValidator runat="server" ID="COVupperBound" Operator="DataTypeCheck"
                            Type="Integer" Display="Dynamic" ControlToValidate="TXBupperBound" ErrorMessage="no lettere!*" />
                        <asp:RangeValidator runat="server" ID="RVupperBound" MinimumValue="0" MaximumValue="100"
                            Display="Dynamic" Type="Integer" ControlToValidate="TXBupperBound" ErrorMessage="fuori range*" />
                    </div>
                    <span class="rightSide">
                        <asp:LinkButton runat="server" ID="LNBcreateActivityRule" Text="Salva **" CssClass="Link_Menu"
                            Visible="true"> </asp:LinkButton>
                        <asp:LinkButton runat="server" ID="LNBcreateUnitRule" Text="Salva **" CssClass="Link_Menu"
                            Visible="true"> </asp:LinkButton>
                    </span>
                    <br />
                    <span class="ruleDescription">
                        <asp:Literal runat="server" ID="LITexplanation" Text="*SPIEGAZIONE"></asp:Literal>
                    </span>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
    <div class="dialog switchAct" id="switchAct">
        <div>
            <asp:Label ID="LBswitchCombo" runat="server" Text="*msg combo regole + controlla pesi" />
            <asp:Label ID="LBswitch" runat="server" Text="save vote" />
        </div>
        <div>
            <asp:Button ID="BTNswitch" runat="server" CssClass="Link_Menu" />
            <asp:HyperLink ID="HYPannulSwitch" runat="server" CssClass="Link_Menu closeDialog" />
        </div>
    </div>

    <div class="dialog switchMessage" id="switchMessage">
        <asp:Label ID="LbSwitchFirst" runat="server"></asp:Label>
    </div>
    <CTRL:helpStatus ID="CTRLhelpStatus" runat="server" />
</asp:Content>
