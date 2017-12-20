<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/AjaxPortal.Master"
    CodeBehind="UserStatistics.aspx.vb" Inherits="Comunita_OnLine.UserStatistics" %>
<%@ Register Src="UC/UC_TextAction.ascx" TagName="TextAction" TagPrefix="CTRL" %>
<%@ Register Src="UC/UC_TimeStat.ascx" TagName="SelectTime" TagPrefix="CTRL" %>
<%@ Register TagPrefix="CTRL" TagName="ProgressBar" Src="UC/UC_ProgressBar.ascx" %>
<%@ Register Src="UC/UC_CertificationAction.ascx" TagName="CertificationAction" TagPrefix="CTRL" %>
<%@ Register TagPrefix="CTRL" TagName="Messages" Src="~/Modules/Common/UC/UC_ActionMessages.ascx" %>

<%@ MasterType VirtualPath="~/AjaxPortal.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
<script src="../../Scripts/jquery.treeTable.js" type="text/javascript"></script>
    <script type="text/javascript" src="../../Jscript/Modules/Common/jquery.progressbar.js"></script>
    <link href="../../Content/jquery.treeTable/jquery.treeTable.css" type="text/css" rel="Stylesheet" />
    <script src="../../Jscript/Modules/Common/jquery.ddbuttonlist.js" type="text/javascript"></script>
    <link href="../../Graphics/Modules/Edupath/css/edupath.css" rel="Stylesheet" />

    <script type="text/javascript">
         var DisplayTitle = ""
         var DisplayMessage = "<h2><%=DisplayMessageToken%></h2>"
    </script>
    <script type="text/javascript" src="<%#ResolveUrl("~/Jscript/FileDownloadCookie.js")%>"></script>

    <script type="text/javascript">
        var TokenHiddenFieldId = "<% = HDNdownloadTokenValue.ClientID %>";
        var CookieStatisticsName = "<% = Me.CookieName %>";
        var DisplayMessageStatistics = "<h2><% = Me.DisplayMessageToken %></h2>";
    </script>
    <script type="text/javascript">
        var fileDownloadCheckTimer;
        function blockUIForDownload() {
            var token = new Date().getTime(); //use the current timestamp as the token value
            $("input[id='" + TokenHiddenFieldId + "']").val(token);
            $.blockUI({ message: DisplayMessageStatistics , title: " ", draggable: false, theme: true });
            fileDownloadCheckTimer = window.setInterval(function () {
                var cookieValue = $.cookie(CookieStatisticsName);
                if (cookieValue == token)
                    finishDownload();
            }, 1000);
        }

        function finishDownload() {
            window.clearInterval(fileDownloadCheckTimer);
            $.cookie(CookieStatisticsName, null); //clears this cookie value
            $.unblockUI();
        }

        $(document).ready(function () {
            $(".ddbuttonlist.enabled").dropdownButtonList();
            $("#tree_table").treeTable({
                clickableNodeNames: false,
                initialState: "expanded",
                persist: false
            });
            
            $(".DialogDetail").dialog({
                autoOpen: false,
                width: '400px'
            });
            $(".openDetail_Path").click(function () {

                var row = $(this).parents("tr").first();
                //row.addClass("ui-state-highlight");

                var name = row.find(".ItemName").html();



                var value = row.find("span.hideDetail").html();

                var values = value.split('#');

                $(".DialogDetailPath").find("span.infoDetail").each(function (i, e) {
                    $(this).html(values[i]);
                });

                $(".DialogDetailPath").dialog("open");
                $(".DialogDetailPath").dialog("option", "title", "Stato");
                $(".DialogDetailPath").find(".ItemName").html(name);
                //row.removeClass("ui-state-highlight");

                return false;
            });
            $(".openDetail_Unit").click(function () {

                var row = $(this).parents("tr").first();
                //row.addClass("ui-state-highlight");

                var name = row.find(".ItemName").html();



                var value = row.find("span.hideDetail").html();
                var values = value.split('#');

                $(".DialogDetailUnit").find("span.infoDetail").each(function (i, e) {
                    $(this).html(values[i]);
                });

                //            for (i = 0; i < values.length; i++) {



                //                //                alert(values[i]);
                //            }
                $(".DialogDetailUnit").dialog("open");
                $(".DialogDetailUnit").dialog("option", "title", "Stato");
                $(".DialogDetailUnit").find(".ItemName").html(name);


                return false;
            });

            $(".openDetail_Activity").click(function () {

                var row = $(this).parents("tr").first();
                //row.addClass("ui-state-highlight");

                var name = row.find(".ItemName").html();



                var value = row.find("span.hideDetail").html();
                var values = value.split('#');

                $(".DialogDetailAct").find("span.infoDetail").each(function (i, e) {
                    $(this).html(values[i]);
                });
                $(".DialogDetailAct").dialog("open");
                $(".DialogDetailAct").dialog("option", "title", "Stato");
                $(".DialogDetailAct").find(".ItemName").html(name);
                return false;
            });

            $(".switchunitVis").click(function () {
                var x = $("[id^='unit-']").filter(".collapsed").size();
                var y = $("[id^='unit-']").size();
                if (((y - x) != y) & ((y - x) != 0)) {
                    $("[id^='unit-']").filter(".collapsed").toggleBranch();
                    $(".switchactivityVis").toggle();
                }

                $("[id^='unit-']").each(function () {
                    $(this).toggleBranch();
                });
                $(".switchactivityVis").toggle();
                return false;
            });

            $(".switchactivityVis").click(function () {
                var x = $("[id^='activity-']").filter(".collapsed").size();
                var y = $("[id^='activity-']").size();
                if (((y - x) != y) & ((y - x) != 0)) {
                    $("[id^='activity-']").filter(".collapsed").toggleBranch();
                    $(".switchsubactivityVis").show();
                }

                $("[id^='activity-']").each(function () {
                    try {
                        $(this).prev(".unit.collapsed").first().toggleBranch();
                    } catch (e) {

                    }
                    $(this).toggleBranch();
                });
                $(".switchsubactivityVis").toggle();
                return false;
            });

            $(".switchsubactivityVis").click(function () {
                $("[id^='subactivity-']").each(function () {
                    $(this).toggleBranch();
                });
                return false;
            });
        });
    </script>
    <%--<link href="PfStyle.css" type="text/css" rel="stylesheet" />--%>
    <%--<asp:Literal ID="LTcertificationScript" runat="server">
    </asp:Literal>--%>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="CPHservice" runat="server">
    <asp:MultiView ID="MLVstat" runat="server" ActiveViewIndex="0">
        <asp:View ID="VIWstat" runat="server">
            <div class="DivEpButton">
                <div class="viewType">
                    <span class="label" id="SPANviewStatistics" runat="server">Statistiche*</span>
                    <asp:HyperLink ID="HYPuserStat" runat="server" CssClass="img_link ico_usr_perm_l"  Visible="false"></asp:HyperLink>
                    <asp:HyperLink ID="HYPpathStat" runat="server" CssClass="img_link ico_stat_l" Visible="false"></asp:HyperLink>
                </div>
                 <div runat="server" id="DIVexport" class="ddbuttonlist enabled" visible="true"><!--   
                    --><asp:LinkButton ID="LNBexportUserStatisticToCsv" runat="server" Text="export2* " OnClientClick="blockUIForDownload();return true;" CssClass="linkMenu"></asp:LinkButton><!--
                    --><asp:LinkButton ID="LNBexportUserStatisticToXml" runat="server" Text=" " OnClientClick="blockUIForDownload();return true;" CssClass="linkMenu"></asp:LinkButton><!--
                    --><asp:LinkButton ID="LNBexportUserStatisticToPdf" runat="server" Text="exportPDF* " OnClientClick="blockUIForDownload();return true;" CssClass="linkMenu"></asp:LinkButton><!--
                    --><asp:LinkButton ID="LNBexportUserStatisticfullToCsv" runat="server" Text=" " OnClientClick="blockUIForDownload();return true;" CssClass="linkMenu" Visible="false"></asp:LinkButton><!--
                    --><asp:LinkButton ID="LNBexportUserStatisticfullToXml" runat="server" Text=" " OnClientClick="blockUIForDownload();return true;" CssClass="linkMenu" Visible="false"></asp:LinkButton><!--
                    --><asp:LinkButton ID="LNBexportUserStatisticfullCertificationToCsv" runat="server" Text=" " OnClientClick="blockUIForDownload();return true;" CssClass="linkMenu" Visible="false"></asp:LinkButton><!--
                    --><asp:LinkButton ID="LNBexportUserStatisticfullCertificationToXml" runat="server" Text="" OnClientClick="blockUIForDownload();return true;" CssClass="linkMenu" Visible="false"></asp:LinkButton><!--                    
                --></div>   
                <asp:HyperLink ID="HYPeduPathView" runat="server" Text="**edu view" CssClass="Link_Menu"></asp:HyperLink>
                <asp:HyperLink ID="HYPback" runat="server" Text="**back" CssClass="Link_Menu"></asp:HyperLink>
            </div>
            <div class="dateFilter" id="DVdateFilter" runat="server">
                <ctrl:SelectTime id="CTRLselTime" runat="server"></ctrl:SelectTime>
            </div>
            <div class="clearfix"></div>            
            <div class="EPprogressBar">
                <CTRL:ProgressBar ID="CTRLprogressBar" runat="server"></CTRL:ProgressBar>
            </div>
            <div>
                <asp:Label ID="LBuserName" runat="server" CssClass="Titolo_campo"></asp:Label>
            </div>
            <CTRL:Messages ID="CTRLmessages" runat="server" Visible="false" />
            <div class="visibilityNav">
                <%--<span class="label">Visibility:</span>--%>
                <ul class="expandbuttons">
                    <li class="expandbutton switchunitVis"><asp:Literal runat="server" ID="LTRNavUnitTop">Unit</asp:Literal>
                        <ul class="expandbuttons">
                            <li class="expandbutton switchactivityVis">&raquo; <asp:Literal runat="server" ID="LTRNavActivityTop">Unit</asp:Literal>
                                <ul class="expandbuttons">
                                    <li class="expandbutton switchsubactivityVis">&raquo; <asp:Literal runat="server" ID="LTRNavSubActivityTop">Unit</asp:Literal></li>
                                </ul>
                            </li>
                        </ul>
                    </li>
                </ul>
            </div>
            <table class="stats user" id="tree_table">
                <thead>
                    <tr>
                        <th class="description"><asp:Label ID="LBnameTit" runat="server" Text="*Nome"> </asp:Label></th>
                        <th class="timecompletion"><asp:Label ID="LBcompletionTit" runat="server" Text="*completion"></asp:Label></th>
                        <th id="TDmarkTit" runat="server" class="mark ">
                            <asp:Label ID="LBmarkTit" runat="server" Text="*mark"></asp:Label>
                        </th>
                        <th class="status"><asp:Label ID="LBstatusTit" runat="server" Text="*status"></asp:Label></th>
                        <th class="actions" runat="server" id="TDAction"><asp:Label ID="LBactionsTit" runat="server" Text="**Actions"></asp:Label></th>
                    </tr>
                </thead>
                <tbody>
                    <tr class="item path" id="path-1">
                        <td class="description">
                            <span class="iteminfo">
                                <span class="name"><asp:Label ID="LBname" runat="server" CssClass="ItemName"> </asp:Label></span>
                                <span class="itemdetail"><asp:Label ID="LBweight" runat="server"></asp:Label></span>
                            </span>
                            <span class="icons right">
                                <%--<span class="icon unlocked" title="Unlocked">&nbsp;</span>--%>
                                <asp:Image ID="IMGmandatory" runat="server" Visible="false" CssClass="icon openDetail_Path" />
                                <asp:Label ID="LBpathMandatoryItem" runat="server" CssClass="hideDetail"></asp:Label>
                                <span class="icon mandatory openDetail_Path" title="" runat="server" id ="SPANPathMandatory" visible="false">&nbsp;</span>
                                
                                <asp:LinkButton ID="LKBblocked" runat="server" Text="Visible**" Visible="false" CssClass="icon"></asp:LinkButton>
                                <asp:Label ID="IMGblocked" runat="server" Visible="false" CssClass="icon" ></asp:Label>
                            </span>
                        </td>
                        <td class="timecompletion">
                            <asp:Label ID="LBcompletion" runat="server" Text="-"></asp:Label>                            
                            <span class="flag" runat="server" id="SPANCompletion"></span>
                            <%--<asp:Image ID="IMGcompletion" runat="server" Visible="false" />--%>
                        </td>
                        <td id="TDmark" runat="server" class="mark ">
                            <asp:Label ID="LBmark" runat="server" Text="-"></asp:Label>
                            <asp:Image ID="IMGmark" runat="server" Visible="false" />
                        </td>
                        <td class="status">
                            <%--<span class="item" run>Started</span>--%>
                            <span class="item" runat="server" id="SPANStatus"></span>
                            <asp:Label ID="LBstatus" runat="server" Text="*status" Width="65%" Visible="false"></asp:Label>
                        </td>
                        <td class="actions" runat="server" id="TDActionP">
                            <span class="actionbuttons">
                                <%--<span class="actionbutton details" title="Details">&nbsp;</span>--%>   
                                &nbsp;                             
                            </span>
                        </td>
                    </tr>
                    <asp:Repeater ID="RPunit" runat="server" OnItemCommand="RP_ItemCommand">
                        <ItemTemplate>
                            <tr class="item unit" id="unit-<%# Container.Dataitem.Id %>">
                                <td class="description">
                                    <span class="iteminfo">
                                        <span class="name"><asp:Label ID="LBname" runat="server" CssClass="ItemName"> </asp:Label></span>
                                        <span class="itemdetail"><asp:Label ID="LBweight" runat="server"></asp:Label></span>
                                    </span>
                                    <span class="icons right">                
                                        <%--<span class="icon unlocked" title="Unlocked">&nbsp;</span>--%>
                                        
                                        <asp:Label ID="IMGisMandatory" runat="server" Visible="false" CssClass="icon mandatory" ></asp:Label>
                                        <asp:Image ID="IMGmandatory" runat="server" Visible="false" CssClass="openDetail_Activity icon mandatory" />                                        
                                        <asp:Label ID="LBmandatoryItem" runat="server" CssClass="hideDetail"></asp:Label>
                                        
                                        <asp:LinkButton ID="LKBblocked" runat="server" Text="Visible**" CommandName="blockedA"
                                            Visible="false"></asp:LinkButton>
                                        <asp:Label ID="IMGblocked" runat="server" Visible="false" ></asp:Label>
                                    </span>
                                </td>
                                <td class="timecompletion">
                                    <asp:Label ID="LBcompletion" runat="server" Text="-"></asp:Label>
                                    <%--<asp:Image ID="IMGcompletion" runat="server" Visible="false" />--%>
                                    <span class="flag" runat="server" id="SPANCompletion"></span>
                                </td>
                                <td id="TDmark" runat="server" visible="false" class="mark ">
                                    <asp:Label ID="LBmark" runat="server" Text="-"></asp:Label>
                                    <asp:Image ID="IMGmark" runat="server" Visible="false" />
                                </td>
                                <td class="status">
                                    <asp:Label ID="LBstatus" runat="server" Text="-" Width="65%" Visible="false"></asp:Label>
                                    <span class=" item <%# Me.HiddenStatus() %> <%# Me.SetStatus(Container.DataItem) %>"><%# Me.SetStatusString(Container.DataItem) %></span>
                                </td>
                                <td class="actions" runat="server" id="TDAction">
                                    <%--<span class="actionbuttons">
                                    </span>--%>
                                    &nbsp;
                                </td>
                            </tr>
                            <asp:Repeater ID="RPactivity" runat="server" OnItemCommand="RP_ItemCommand">
                            <ItemTemplate>
                                <tr class="item activity child-of-unit-<%# Container.Dataitem.parentId %> <%# Me.HiddenActivity(Container.Dataitem)%>" id="activity-<%# Container.Dataitem.Id %>">
                                    <td class="description">
                                        <span class="iteminfo">
                                            <span class="name"><asp:Label ID="LBname" runat="server" CssClass="ItemName"> </asp:Label></span>
                                            <span class="itemdetail"><asp:Label ID="LBweight" runat="server"></asp:Label></span>
                                        </span>
                                        <span class="icons right">
                                            <%--<span class="icon mandatory" title="Mandatory">&nbsp;</span>--%>
                                            <%--<span class="icon unlocked" title="Unlocked">&nbsp;</span>--%>

                                            <asp:Label ID="IMGisMandatory" runat="server" Visible="false" CssClass="icon mandatory" ></asp:Label>
                                        <asp:Image ID="IMGmandatory" runat="server" Visible="false" CssClass="icon openDetail_Activity" />
                                        <asp:Label ID="LBmandatoryItem" runat="server" CssClass="hideDetail"></asp:Label>
                                        
                                        <asp:LinkButton ID="LKBblocked" runat="server" Text="Visible**" CommandName="blockedA"
                                            Visible="false"></asp:LinkButton>
                                        <asp:label ID="IMGblocked" runat="server" Visible="false" ></asp:label> 
                                        </span>
                                    </td>
                                    <td class="timecompletion">
                                        <asp:Label ID="LBcompletion" runat="server" Text="-"></asp:Label>
                                        <%--<asp:Image ID="IMGcompletion" runat="server" Visible="false" />
                                        <span class="flag yes">&nbsp;</span>--%>
                                        <span class="flag" runat="server" id="SPANCompletion"></span>
                                    </td>
                                    <td id="TDmark" runat="server" class="mark ">
                                        <asp:Label ID="LBmark" runat="server" Text="-"></asp:Label>
                                        <asp:Image ID="IMGmark" runat="server" Visible="false" />
                                    </td>
		                            <td class="status">
                                        <asp:Label ID="LBstatus" runat="server" Text="*status" Width="65%" Visible="false"></asp:Label>
                                        <span class="item <%# Me.SetStatus(Container.DataItem) %>"><%# Me.SetStatusString(Container.DataItem) %></span>
                                    </td>
                                    <td class="actions" runat="server" id="TDAction" visible="true">
                                        <span class="actionbuttons">
                                            <%--<span class="actionbutton details" title="Details">&nbsp;</span>--%>
                                            <asp:HyperLink ID="HYPdetail" runat="server" Text="*detail" CssClass="Link_Menu" Visible="false" />
                                            &nbsp;
                                        </span>
                                    </td>
                                </tr>
                                <asp:Repeater runat="server" ID="RPSubActivity" OnItemCommand="RPSubactivity_ItemCommand">
                                    <ItemTemplate>
                                        <tr class="item subactivity<%# Me.isSingle(Container.Dataitem) %> child-of-<%# Me.ActivityUnit(Container.Dataitem) %>-<%# Container.Dataitem.parentId %>" id="subactivity-<%# Container.Dataitem.SubActivity.Id %>">
                                            <td class="description">
                                                <%--<span class="iteminfo">
                                                    <span class="name">--%>
                                                <CTRL:TextAction runat="server" id="CTRLtextAction" Visible="false"  />
                                                <CTRL:CertificationAction runat="server" id="CTRLcertificationAction" visible="false" onItemActionResult="ItemActionResult" />
                                                <asp:PlaceHolder runat="server" ID="PHLAction" Visible="false"></asp:PlaceHolder>
                                                    <%--</span>
                                                    <span class="itemdetail"><asp:Label ID="LBweight" runat="server"></asp:Label></span>--%>
                                                </span>
                                                <span class="icons right">
                                                    <%--<span class="icon mandatory" title="Mandatory">&nbsp;</span>--%>
                                                    <%--<span class="icon unlocked" title="Unlocked">&nbsp;</span>--%>

                                                    <asp:Label ID="IMGisMandatory" runat="server" Visible="false" CssClass="icon mandatory" ></asp:Label>
                                                <asp:Image ID="IMGmandatory" runat="server" Visible="false" CssClass="icon openDetail_Activity" />
                                                <asp:Label ID="LBmandatoryItem" runat="server" CssClass="hideDetail"></asp:Label>
                                        
                                                <asp:LinkButton ID="LKBblocked" runat="server" Text="Visible**" CommandName="blockedA"
                                                    Visible="false"></asp:LinkButton>
                                                <asp:label ID="IMGblocked" runat="server" Visible="false" ></asp:label> 
                                                </span>
                                            </td>
                                            <td class="timecompletion">
                                                <asp:Label ID="LBcompletion" runat="server" Text="-"></asp:Label>
                                                <%--<asp:Image ID="IMGcompletion" runat="server" Visible="false" />
                                                <span class="flag yes">&nbsp;</span>--%>
                                                <span class="flag" runat="server" id="SPANCompletion"></span>
                                            </td>
                                            <td id="TDmark" runat="server" class="mark ">
                                                <asp:Label ID="LBmark" runat="server" Text="-"></asp:Label>
                                                <asp:Image ID="IMGmark" runat="server" Visible="false" />
                                            </td>
		                                    <td class="status">
                                                <asp:Label ID="LBstatus" runat="server" Text="*status" Width="65%" Visible="false"></asp:Label>
                                                <span class="item <%# Me.SetStatusSubAct(Container.DataItem) %>"><%# Me.SetStatusStringSubAct(Container.DataItem) %></span>
                                            </td>
                                            <td class="actions" runat="server" id="TDAction">
                                                <span class="actionbuttons">
                                                    <%--<span class="actionbutton details" title="Details">&nbsp;</span>--%>
                                                    <asp:HyperLink ID="HYPdetail" runat="server" Text="*detail" CssClass="Link_Menu" Visible="false" />
                                                    <asp:HyperLink ID="HYPstats" runat="server" Text="" CssClass="actionbutton stats" Visible="false" />

                                                    <asp:LinkButton ID="LNBexportCertification" runat="server" Text="" CssClass="actionbutton certificate" Visible="false" CommandName="certification"></asp:LinkButton>
                                                    <asp:HyperLink ID="HYPexportCertification" Target="_blank" runat="server" Text="" CssClass="actionbutton certificate fileRepositoryCookieNoRefresh" Visible="false" />
                                                    <asp:HyperLink ID="HYPrefreshCertificate" Target="_blank" runat="server" Text="" CssClass="actionbutton refresh fileRepositoryCookieNoRefresh" Visible="false" />
                                                    &nbsp;
                                                </span>
                                            </td>
                                        </tr>
                                    </ItemTemplate>
                                </asp:Repeater>
                                <%--<tr class="ROW_Normal_Small activity">
                                    <td class="name ">                                        
                                        
                                        
                                    </td>
                                    <td class="completion ">
                                        
                                    </td>
                                    
                                    <td class="status ">
                                        
                                    </td>
                                    <td class="detail">
                                        
                                    </td>
                                </tr>--%>
                            </ItemTemplate>
                        </asp:Repeater>
                        </ItemTemplate>
                    </asp:Repeater>
                </tbody>
            </table>
            <div class="visibilityNav">
                <%--<span class="label">Visibility:</span>--%>
                <ul class="expandbuttons">
                    <li class="expandbutton switchunitVis"><asp:Literal runat="server" ID="LTRNavUnitBottom">Unit</asp:Literal>
                        <ul class="expandbuttons">
                            <li class="expandbutton switchactivityVis">&raquo; <asp:Literal runat="server" ID="LTRNavActivityBottom">Unit</asp:Literal>
                                <ul class="expandbuttons">
                                    <li class="expandbutton switchsubactivityVis">&raquo; <asp:Literal runat="server" ID="LTRNavSubActivityBottom">Unit</asp:Literal></li>
                                </ul>
                            </li>
                        </ul>
                    </li>
                </ul>
            </div> 
            <div class="DialogDetail DialogDetailPath">
                <%--<span>avanzamento delle unità del percorso </span><span class="ItemName"></span><br /><br />--%>
                <table>
                    <thead>
                        <tr>
                            <td>
                                <asp:Label ID="LbDialogPath" runat="server"></asp:Label><span class="ItemName"></span>
                            </td>
                        </tr>
                        <%--<tr><td>&nbsp;</td><td>&nbsp;</td></tr>--%>
                    </thead>
                    <tbody>
                        <tr>
                            <td>
                                <asp:Label ID="LbTotMandPath" runat="server"></asp:Label>
                            </td>
                            <td>
                                <span class="infoDetail"></span>
                            </td>
                        </tr>
                        <tr id="TRonlyCompletedPath" runat="server">
                            <td>
                                <asp:Label ID="LbOnlyCompPath" runat="server"></asp:Label>
                            </td>
                            <td>
                                <span class="infoDetail"></span>
                            </td>
                        </tr>
                        <tr id="TRonlyPassedPath" runat="server">
                            <td>
                                <asp:Label ID="LbOnlyPassPath" runat="server"></asp:Label>
                            </td>
                            <td>
                                <span class="infoDetail"></span>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="LbPassComplPath" runat="server"></asp:Label>
                            </td>
                            <td>
                                <span class="infoDetail"></span>
                            </td>
                        </tr>
                    </tbody>
                    <tfoot>
                        <tr>
                            <td>
                                ---
                            </td>
                        </tr>
                    </tfoot>
                </table>
            </div>
            <div class="DialogDetail DialogDetailUnit">
                <table>
                    <thead>
                        <tr>
                            <td>
                                <asp:Label ID="LbDialogUnit" runat="server"></asp:Label><span class="ItemName"></span>
                            </td>
                        </tr>
                        <%--<tr><td>&nbsp;</td><td>&nbsp;</td></tr>--%>
                    </thead>
                    <tbody>
                        <tr>
                            <td>
                                <asp:Label ID="LbTotMandUnit" runat="server"></asp:Label>
                            </td>
                            <td>
                                <span class="infoDetail"></span>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="LbOnlyCompUnit" runat="server"></asp:Label>
                            </td>
                            <td>
                                <span class="infoDetail"></span>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="LbOnlyPassUnit" runat="server"></asp:Label>
                            </td>
                            <td>
                                <span class="infoDetail"></span>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="LbPassComplUnit" runat="server"></asp:Label>
                            </td>
                            <td>
                                <span class="infoDetail"></span>
                            </td>
                        </tr>
                    </tbody>
                    <tfoot>
                        <tr>
                            <td>
                                ---
                            </td>
                        </tr>
                    </tfoot>
                </table>
            </div>
            <div class="DialogDetail DialogDetailAct">
                <table>
                    <thead>
                        <tr>
                            <td>
                                <asp:Label ID="LbDialogActivity" runat="server"></asp:Label><span class="ItemName"></span>
                            </td>
                        </tr>
                        <%--<tr><td>&nbsp;</td><td>&nbsp;</td></tr>--%>
                    </thead>
                    <tbody>
                        <tr>
                            <td>
                                <asp:Label ID="LbTotMandAct" runat="server"></asp:Label>
                            </td>
                            <td>
                                <span class="infoDetail"></span>
                            </td>
                        </tr>
                        <tr>
                            <td id="TRonlyCompletedAct" runat="server">
                                <asp:Label ID="LbOnlyCompAct" runat="server"></asp:Label>
                            </td>
                            <td>
                                <span class="infoDetail"></span>
                            </td>
                        </tr>
                        <tr id="TRonlyPassedAct" runat="server">
                            <td>
                                <asp:Label ID="LbOnlyPassAct" runat="server"></asp:Label>
                            </td>
                            <td>
                                <span class="infoDetail"></span>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="LbPassComplAct" runat="server"></asp:Label>
                            </td>
                            <td>
                                <span class="infoDetail"></span>
                            </td>
                        </tr>
                    </tbody>
                    <tfoot>
                        <tr>
                            <td>
                                ---
                            </td>
                        </tr>
                    </tfoot>
                </table>
            </div>
        </asp:View>
        <asp:View ID="VIWerror" runat="server">
            <div id="DVerror" align="center">
                <div class="DivEpButton">
                    <asp:HyperLink ID="HYPerror" runat="server" CssClass="Link_Menu" />
                </div>
                <div align="center">
                    <asp:Label ID="LBerror" runat="server" CssClass="messaggio"></asp:Label>
                </div>
            </div>
        </asp:View>
    </asp:MultiView>
     <asp:HiddenField runat="server" ID="HDNdownloadTokenValue" />
</asp:Content>