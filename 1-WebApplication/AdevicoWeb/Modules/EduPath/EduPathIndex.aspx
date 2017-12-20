<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/AjaxPortal.Master"
    CodeBehind="EduPathIndex.aspx.vb" Inherits="Comunita_OnLine.EduPathIndex" %>
    <%@ MasterType VirtualPath="~/AjaxPortal.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="PageTitleContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="HeadContent" runat="server">
    <script type="text/javascript">

        $(document).ready(function () {

            //            $("li span.switch.autoOpen").addClass("open").removeClass("close");


            //            $("li span.switch").not(".autoOpen").each(function () {
            //                var li_el = $(this).parent("li");
            //                var child_ul = $(li_el).children("ul");
            //                var sib_li = $(li_el).siblings("li");
            //                child_ul.hide();
            //                if ($(child_ul).is(":visible")) {
            //                    var childSib = $(sib_li).children("ul");
            //                    childSib.hide();
            //                    childSib.parent("li").children("span.switch").removeClass("open").addClass("close");

            //                    $(this).addClass("open").removeClass("close");
            //                } else {
            //                    $(this).removeClass("open").addClass("close");
            //                }
            //            });

            //            $("li span.switch").click(function () {

            //                var li_el = $(this).parent("li");
            //                var child_ul = $(li_el).children("ul");
            //                var sib_li = $(li_el).siblings("li");
            //                child_ul.slideToggle();
            //                if ($(child_ul).is(":visible")) {
            //                    var childSib = $(sib_li).children("ul");
            //                    childSib.slideUp();
            //                    childSib.parent("li").children("span.switch").removeClass("open").addClass("close");
            //                    //$(this).addClass("open").removeClass("close");
            //                } else {
            //                    //$(this).removeClass("open").addClass("close");
            //                }

            //                $(this).toggleClass("open").toggleClass("close");
            //            });

            $("li span.switch.autoOpen").addClass("open").removeClass("close");

            $("li span.switch").not(".keepOpen").not(".autoOpen").each(function () {
                var li_el = $(this).parent("li");
                var child_ul = $(li_el).children("ul");
                var sib_li = $(li_el).siblings("li");
                child_ul.hide();
                if ($(child_ul).is(":visible")) {
                    var childSib = $(sib_li).children("ul");
                    childSib.hide();
                    childSib.parent("li").children("span.switch").removeClass("open").addClass("close");



                    $(this).addClass("open").removeClass("close");

                } else {

                    $(this).removeClass("open").addClass("close");

                }

            });



            $("li span.switch").not(".keepOpen").click(function () {



                var li_el = $(this).parent("li");

                var child_ul = $(li_el).children("ul");

                var sib_li = $(li_el).siblings("li");


                child_ul.slideToggle();

                if ($(child_ul).is(":visible")) {

                    var childSib = $(sib_li).not(".keepOpen").children("ul");

                    childSib.slideUp();

                    childSib.parent("li").children("span.switch").removeClass("open").addClass("close");

                    //$(this).addClass("open").removeClass("close");

                } else {

                    //$(this).removeClass("open").addClass("close");

                }


                $(this).toggleClass("open").toggleClass("close");

            });


            $(this).find("a[href*='#']").live("click", function () {
                var elemSel = '[name=' + $(this).attr('href').split('#')[1]+']';
                var $elemSel = $(elemSel);
                var $li_el = $elemSel.parents("li").first();
                var $child_ul = $li_el.children("ul");
                var $sib_li = $li_el.siblings("li");

                $child_ul.slideDown();

                var $childSib = $sib_li.not(".keepOpen").children("ul");

                $childSib.slideUp();

                $childSib.parent("li").children("span.switch").removeClass("open").addClass("close");
                $li_el.children("span.switch").addClass("open").removeClass("close");

            });


        });
    </script>

    <script type="text/javascript">
        $(function () {
            $(".requestGlossary").click(function () {

                var title = $(this).attr("title");
                //var url = $(this).data("url");
                var html = "";

                var baseurl = "";
                var ItemId = $(this).data("id");

                $.ajax({
                    type: "POST",
                    url: "../FaqGlossaryHelper/Glossary.asmx/GetGlossaryString",
                    data: "{'ItemId':'" + ItemId + "', 'Htmltype':'2'}",
                    processData: false,
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",

                    success: function (msg) {
                        html = msg.d;

                        $(".dialog.dialogGlossary").parents(".ui-dialog").find("span.ui-dialog-title").html(title);
                        $(".dialog.dialogGlossary").find("div.itemcontent").html(html);

                        $(".dialog.dialogGlossary").dialog("open");
                    },
                    error: function (result) {
                        html = result.status + "<br>" + result.statusText;

                        $(".dialog.dialogGlossary").parents(".ui-dialog").find("span.ui-dialog-title").html(title);
                        $(".dialog.dialogGlossary").find("div.itemcontent").html(html);

                        $(".dialog.dialogGlossary").dialog("open");
                    }
                });



                return false;
            });

            $(".requestFaq dt").live("click", function () {
                $(this).toggleClass("expanded");
                $(this).next("dd").toggleClass("expanded");
            });

            $(".requestFaq").each(function () {
                //var url = $(this).data("url");

                var baseurl = "";
                var ItemId = $(this).data("id");

                var $this = $(this);

                $this.data("html", "");

                if (ItemId.indexOf(";") > 0) {

                    var Ids = ItemId.split(";");

                    for (var _ids in Ids) {

                        var value = Ids[_ids];
                        if (value != "") {

                            $.ajax({
                                type: "POST",
                                url: "../FaqGlossaryHelper/Faq.asmx/GetFaqString",
                                data: "{'ItemId':'" + value + "', 'Htmltype':'4'}",
                                processData: false,
                                contentType: "application/json; charset=utf-8",
                                dataType: "json",

                                success: function (msg) {
                                    $this.data("html", $this.data("html") + msg.d);
                                    $this.html("<dl>" + $this.data("html") + "</dl>");
                                },
                                error: function (result) {
                                    //                                html += result.status + "<br>" + result.statusText;
                                    //                                $this.html(html);
                                    $this.data("html", $this.data("html") + "<dt>" + result.statusText + "</dt><dd>ErrorCode: " + result.status + "</dd>");
                                    $this.html("<dl>" + $this.data("html") + "</dl>");


                                }
                            });
                        }

                    }
                } else {

                    $.ajax({
                        type: "POST",
                        url: baseurl + "Faq.asmx/GetFaqString",
                        data: "{'ItemId':'" + ItemId + "', 'Htmltype':'2'}",
                        processData: false,
                        contentType: "application/json; charset=utf-8",
                        dataType: "json",

                        success: function (msg) {
                            $this.data("html", msg.d);
                            $this.html($this.data("html"));
                        },
                        error: function (result) {
                            $this.data("html", result.status + "<br>" + result.statusText);
                            $this.html($this.data("html"));
                        }
                    });
                }


            });

            $(".dialog").dialog({
                autoOpen: false,
                height: 400,
                width: 600,
                minHeight: 200,
                minWidth: 500,
                maxWidth: 900,
                maxHeight: 700
            });
        });
    </script>

    
    <%--<link href="../../Graphics/Modules/Edupath/css/tree.css" rel="Stylesheet" />--%>
    <link href="tree.css" type="text/css" rel="stylesheet" />
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="TitleContent" runat="server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="CPHservice" runat="server">        
    <asp:Literal ID="LTrender" runat="server"></asp:Literal>
    <div class="dialog dialogGlossary">
            <div class="itemcontent"></div>
        </div>
     <asp:UpdatePanel ID="UPTempo" runat="server">
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="TMsession" EventName="Tick" />
        </Triggers>
    </asp:UpdatePanel>
    <asp:Timer ID="TMsession" runat="server">
    </asp:Timer>
</asp:Content>