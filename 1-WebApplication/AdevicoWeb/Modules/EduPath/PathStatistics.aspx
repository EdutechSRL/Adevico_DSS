<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/AjaxPortal.Master" CodeBehind="PathStatistics.aspx.vb" Inherits="Comunita_OnLine.PathStatistics" %>
<%@ Register Src="UC/UC_TimeStat.ascx" TagName="SelectTime" TagPrefix="CTRL" %>
<%@ Register Src="~/Modules/Common/UC/UC_StackedBar.ascx" TagName="StackedBar" TagPrefix="CTRL" %>
<%@ Register Src="UC/UC_TextAction.ascx" TagName="TextAction" TagPrefix="CTRL" %>
<%@ Register Src="UC/UC_CertificationAction.ascx" TagName="CertificationAction" TagPrefix="CTRL" %>
<%@ MasterType VirtualPath="~/AjaxPortal.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <script src="../../Scripts/jquery.treeTable.js" type="text/javascript"></script>
    <script src="../../Jscript/Modules/Common/jquery.ddbuttonlist.js" type="text/javascript"></script>
    <script type="text/javascript" src="../../Jscript/Modules/Common/jquery.progressbar.js"></script>
    <link href="../../Content/jquery.treeTable/jquery.treeTable.css" type="text/css" rel="Stylesheet" />
    
    <link href="../../Graphics/Modules/Edupath/css/edupath.css" rel="Stylesheet" />
    <script type="text/javascript">
        var TokenHiddenFieldId = "<% = HDNdownloadTokenValue.ClientID %>";
        var CookieName = "<% = Me.CookieName %>";
        var DisplayMessage = "<% = Me.DisplayMessageToken %>";
        var DisplayTitle = "<% = Me.DisplayTitleToken %>";
    </script>
     <script type="text/javascript">
         var fileDownloadCheckTimer;
         function blockUIForDownload() {
             var token = new Date().getTime(); //use the current timestamp as the token value
             $("input[id='" + TokenHiddenFieldId + "']").val(token);
             $.blockUI({ message: DisplayMessage, title: DisplayTitle, draggable: false, theme: true });
             fileDownloadCheckTimer = window.setInterval(function () {
                 var cookieValue = $.cookie(CookieName);
                 if (cookieValue == token)
                     finishDownload();
             }, 1000);
         }

         function finishDownload() {
             window.clearInterval(fileDownloadCheckTimer);
             $.cookie(CookieName, null); //clears this cookie value
             $.unblockUI();
         }
    </script>
    <style>
        tr.hiddenActivity
        {
            display:none;
        }
        table.stats tr.single
        {
            background-color:#EEE;
        }
    </style>
    <script type="text/javascript">
        function ExpandAllCookie(selector) {
            $(selector).find("tr.item").each(function () {

            });
        }

        $(document).ready(function () {
            $(".ddbuttonlist.enabled").dropdownButtonList();
            $(".progressbar").myProgressBar();

            $("#tree_table").treeTable({
                clickableNodeNames: false,
                initialState: "expanded",
                persist: false
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
    
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="CPHservice" runat="server">
    <asp:MultiView ID="MLVstat" runat="server" ActiveViewIndex="0">
        <asp:View ID="VIWstat" runat="server">
            <div class="DivEpButton ">
                <div class="viewType">
                    <span class="label" id="SPANviewStatistics" runat="server">Statistiche*</span>
                    <asp:HyperLink ID="HYPuserStat" runat="server" CssClass="img_link ico_usr_perm_l"></asp:HyperLink>
                </div>
                 <div runat="server" id="DIVexport" class="ddbuttonlist enabled" visible="true"><!--   
                    --><asp:LinkButton ID="LNBexportPSstandardToCsv" runat="server" Text="export2* " OnClientClick="blockUIForDownload();return true;" CssClass="linkMenu"></asp:LinkButton><!--
                    --><asp:LinkButton ID="LNBexportPSstandardToXml" runat="server" Text=" " OnClientClick="blockUIForDownload();return true;" CssClass="linkMenu"></asp:LinkButton><!--
                    --><asp:LinkButton ID="LNBexportPSstandardToPdf" runat="server" Text="exportPDF* " OnClientClick="blockUIForDownload();return true;" CssClass="linkMenu"></asp:LinkButton><!--
                    --><asp:LinkButton ID="LNBexportPSfullToCsv" runat="server" Text=" " OnClientClick="blockUIForDownload();return true;" CssClass="linkMenu"></asp:LinkButton><!--
                    --><asp:LinkButton ID="LNBexportPSfullToXml" runat="server" Text=" " OnClientClick="blockUIForDownload();return true;" CssClass="linkMenu"></asp:LinkButton><!--
                    --><asp:LinkButton ID="LNBexportPSfullCertificationToCsv" runat="server" Text=" " OnClientClick="blockUIForDownload();return true;" CssClass="linkMenu"></asp:LinkButton><!--
                    --><asp:LinkButton ID="LNBexportPSfullCertificationToXml" runat="server" Text="" OnClientClick="blockUIForDownload();return true;" CssClass="linkMenu"></asp:LinkButton><!--                    
                --></div>        
                <%--<asp:HyperLink runat="server" ID="HYPcertificates" CssClass="Link_Menu"></asp:HyperLink>--%>
                <asp:HyperLink ID="HYPeduPathView" runat="server" Text="**edu view" CssClass="Link_Menu"></asp:HyperLink>
                <asp:HyperLink ID="HYPeduPathList" runat="server" Text="**edu list" CssClass="Link_Menu"></asp:HyperLink>
            </div>
            <%--<div class="exportButtons">
                <span class="label">Esporta statistiche</span>
                <span class="exportButton">Csv</span>
                <span class="exportButton">Xml</span>
                <span class="exportButton">Pdf</span>
            </div>--%>
            <div class="dateFilter">
                <%--<span class="label"></span>--%>
                <ctrl:SelectTime id="CTRLselTime" runat="server"></ctrl:SelectTime>
                <%--<div class="dateTime">
                    <input class="date">&nbsp;
                    <input />hh&nbsp;<input />mm
                    <input type="button" value="show" />
                </div>--%>
                <br />
                <asp:CheckBox runat="server" ID="CHBshowall" Visible="true" Text="Show hidden" Checked="false" AutoPostBack="true" />
            </div>
            <asp:Label ID="LBuserName" runat="server" CssClass="Titolo_campo" Visible="false"></asp:Label>
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
            <table class="stats <%# Me.PathTypeClass %>" id="tree_table">
                <thead>
                    <tr>
                        <th class="description"><asp:Label ID="LBnameTit" runat="server" Text="*Nome"> </asp:Label></th>
                        <th class="completedpassed"><asp:Label ID="LBcomplPassTit" runat="server" Text="-"> </asp:Label></th>
                        <th id="THcompleted" runat="server" class="completed ">
                            <asp:Label ID="LBcompletedTit" runat="server" Text="-"></asp:Label>
                        </th>
                        <th id="THpassed" runat="server" class="passed ">
                            <asp:Label ID="LBpassedTit" runat="server" Text="-"> </asp:Label>
                        </th>
                        <th class="started"><asp:Label ID="LBstartedTit" runat="server" Text="-"></asp:Label></th>
                        <th class="notstarted"><asp:Label ID="LBnotStartedTit" runat="server" Text="-"></asp:Label></th>
                        <th class="status"><asp:Label ID="LBstatusTit" runat="server" Text="**Status"></asp:Label></th>
                        <th class="actions"><asp:Label ID="LBactionsTit" runat="server" Text="**Actions"></asp:Label></th>
                    </tr>
                </thead>
                <tbody>                    
                    <tr class="item path" id="path-<%# Me.PathIdString %>">
                        <td class="description">
                            <span class="iteminfo">
                                <span class="name"><asp:Label ID="LBname" runat="server" Text="-" CssClass="___Bold"> </asp:Label></span>
                                <span class="itemdetail"><asp:Label ID="LBweight" runat="server"></asp:Label></span>
                            </span>
                            <span class="icons right">
                                <span class="icon" title="Unlocked" id="ImgPathLock" runat="server" visible="false">&nbsp;</span>
                                <asp:LinkButton runat="server" ID="LBPathLock" ToolTip="Unlocked" CssClass="icon" Visible="false"></asp:LinkButton>
                            </span>
                        </td>
                        <td class="completedpassed"><asp:Label ID="LBcomplPass" runat="server" Text="-"> </asp:Label></td>
                        <td id="TDcompleted" runat="server" class="completed">
                            <asp:Label ID="LBcompleted" runat="server" Text="-"></asp:Label>
                        </td>
                        <td id="TDpassed" runat="server" class="passed">
                            <asp:Label ID="LBpassed" runat="server" Text="-"> </asp:Label>
                        </td>
                        <td class="started"><asp:Label ID="LBstarted" runat="server" Text="-"></asp:Label></td>
                        <td class="notstarted"><asp:Label ID="LBnotStarted" runat="server" Text="-"></asp:Label></td>
                        <td class="status">
                            <%--<span class="progressbar"><span class="completed" title="Completed and Passed: 50%">&nbsp;</span><span class="notpassed" title="Not Passed: 0%">&nbsp;</span><span class="started" title="Started: 24%">&nbsp;</span><span class="notstarted" title="Not Started: 26%">&nbsp;</span></span>--%>
                            <CTRL:StackedBar id="CTRLStackedBar" runat="server"></CTRL:StackedBar>
                        </td>
                        <td class="actions">
                            <span class="icons">
                                <%--<span class="actionbutton details" title="Details">&nbsp;</span>--%>
                                <asp:HyperLink ID="HYPdetail" runat="server" Text="" CssClass="icon infoalt"
                            Visible="false" />                        
                                <%--<span class="actionbutton stats" title="Stats">&nbsp;</span>--%>
                                <%--<span class="actionbutton">&nbsp;</span>--%>

                                <asp:HyperLink runat="server" ID="HYPcertificates" CssClass="icon certificate"></asp:HyperLink>
                            </span>
                        </td>
                    </tr>

                    <asp:Repeater ID="RPunit" runat="server" OnItemCommand="RP_ItemCommand">                        
                        <ItemTemplate>
                            <tr class="item unit" id="unit-<%# Container.Dataitem.itemId %>">
                                <td class="description">
                                    <span class="iteminfo">
                                        <span class="name"><asp:Label ID="LBname" runat="server" Text="*Nome" CssClass="___Bold"> </asp:Label></span>
                                        <span class="itemdetail"><asp:Label ID="LBnameDetail" runat="server"></asp:Label></span>
                                    </span>
                                    <span class="icons right">
                                        <span class="icon mandatory" runat="server" id="ImgMandatory" visible="false" title="<%# Me.UnitMandatory %>">&nbsp;</span>
                                        <span class="icon" id="ImgUnitLock" runat="server" visible="false" title="<%# Me.ItemBlocked(Container.Dataitem) %>">&nbsp;</span>
                                        <asp:LinkButton runat="server" ID="LBUnitLock" CssClass="icon" ToolTip="<%# Me.ItemBlocked(Container.Dataitem) %>" Visible="false" CommandName="blockedU"></asp:LinkButton>
                                    </span>
                                </td>
                                <td id="TDcompleted" runat="server" class="completed ">
                                    <asp:Label ID="LBcompleted" runat="server" Text="-"></asp:Label>
                                </td>
                                <td id="TDpassed" runat="server" class="passed ">
                                    <asp:Label ID="LBpassed" runat="server" Text="-"> </asp:Label>
                                </td>
                                <td class="completedpassed"><asp:Label ID="LBcomplPass" runat="server" Text="-"> </asp:Label></td>
                                <td class="started"><asp:Label ID="LBstarted" runat="server" Text="-"></asp:Label></td>
                                <td class="notstarted"><asp:Label ID="LBnotStarted" runat="server" Text="-"></asp:Label></td>
                                <td class="status">
                                    <CTRL:StackedBar id="CTRLStackedBar" runat="server" Visible="false"></CTRL:StackedBar>
                                </td>
                                <td class="actions">
                                    <span class="actionbuttons">                                        
                                        <asp:HyperLink ID="HYPdetail" runat="server" Text="" CssClass="actionbutton details" Visible="false" />                                        
                                        <asp:HyperLink ID="HYPstats" runat="server" Text="" CssClass="actionbutton stats" Visible="false" />
                                        <%--<span class="actionbutton evaluate" title="Evaluate">&nbsp;</span>--%>
                                    </span>
                                </td>
                            </tr>

                            <asp:Repeater ID="RPactivity" runat="server" OnItemCommand="RP_ItemCommand">
                                <ItemTemplate>                                    
                                    <tr class="item activity child-of-unit-<%# Container.Dataitem.parentId %> <%# Me.HiddenActivity(Container.Dataitem)%>" id="activity-<%# Container.Dataitem.itemId %>">
                                        <td class="description">
                                            <span class="iteminfo">
                                                <span class="name"><asp:Label ID="LBname" runat="server" Text="*Nome"> </asp:Label></span>
                                                <span class="itemdetail"><asp:Label ID="LBnameDetail" runat="server" Text=""> </asp:Label></span></span>
                                            </span>
                                            <span class="icons right">
                                                <span class="icon mandatory" runat="server" id="ImgMandatory" visible="false" title="<%# Me.ActivityMandatory %>">&nbsp;</span>
                                                <span class="icon"  id="ImgActivityLock" runat="server" title="<%# Me.ItemBlocked(Container.Dataitem) %>"  Visible="false">&nbsp;</span>
                                                <asp:LinkButton runat="server" ID="LBActivityLock"  ToolTip="<%# Me.ItemBlocked(Container.Dataitem) %>" CssClass="icon" Visible="false" CommandName="blockedA"></asp:LinkButton>
                                            </span>
                                        </td>
                                        <td id="TDcompleted" runat="server" class="completed ">
                                            <asp:Label ID="LBcompleted" runat="server" Text=""></asp:Label>
                                        </td>
                                        <td id="TDpassed" runat="server" class="passed ">
                                            <asp:Label ID="LBpassed" runat="server" Text=""> </asp:Label>
                                        </td>
                                        <td class="completedpassed"><asp:Label ID="LBcomplPass" runat="server" Text=""> </asp:Label></td>
                                        <td class="started"><asp:Label ID="LBstarted" runat="server" Text="*All Items"></asp:Label></td>
                                        <td class="notstarted"><asp:Label ID="LBnotStarted" runat="server"></asp:Label></td>
                                        <td class="status">
                                            <CTRL:StackedBar id="CTRLStackedBar" runat="server"></CTRL:StackedBar>
                                        </td>
                                        <td class="actions">
                                            <span class="actionbuttons">
                                                <asp:HyperLink ID="HYPdetail" runat="server" Text="" CssClass="actionbutton details" Visible="false" />
                                                <asp:HyperLink ID="HYPstats" runat="server" Text="" CssClass="actionbutton stats" Visible="false" />
                                                <%--<span class="actionbutton">&nbsp;</span>--%>
                                            </span>
                                        </td>
                                    </tr>

                                    <asp:Repeater ID="RPsubAct" runat="server" OnItemCommand="RP_ItemCommand">
                                        <ItemTemplate>
                                            <tr class="item subactivity<%# Me.isSingle(Container.Dataitem) %> child-of-<%# Me.ActivityUnit(Container.Dataitem) %>-<%# Container.Dataitem.parentId %>" id="subactivity-<%# Container.Dataitem.itemId %>">
                                                <td class="description">
                                                    <CTRL:TextAction runat="server" id="CTRLtextAction" Visible="false"  />
                                                    <CTRL:CertificationAction runat="server" id="CTRLcertificationAction" visible="false" />
                                                    <asp:PlaceHolder runat="server" ID="PHLAction"></asp:PlaceHolder>
                                                    <span class="icons right">
                                                        <span class="icon mandatory" runat="server" id="ImgMandatory" visible="false"  title="<%# Me.SubActivityMandatory %>">&nbsp;</span>
                                                        <span class="icon" id="ImgSubActivityLock" runat="server" title="<%# Me.ItemBlocked(Container.Dataitem) %>" visible="false">&nbsp;</span>
                                                        <asp:LinkButton runat="server" ID="LBSubActivityLock" ToolTip="<%# Me.ItemBlocked(Container.Dataitem) %>" CssClass="icon" Visible="false" CommandName="blockedS"></asp:LinkButton>
                                                    </span>
                                                </td>
                                                <td id="TDcompleted" runat="server" class="completed ">
                                                    <asp:Label ID="LBcompleted" runat="server" Text="-"></asp:Label>
                                                </td>
                                                <td id="TDpassed" runat="server" class="passed ">
                                                    <asp:Label ID="LBpassed" runat="server" Text="-"> </asp:Label>
                                                </td>
                                                <td class="completedpassed"><asp:Label ID="LBcomplPass" runat="server" Text="-"> </asp:Label></td>
                                                <td class="started"><asp:Label ID="LBstarted" runat="server" Text="-"></asp:Label></td>
                                                <td class="notstarted"><asp:Label ID="LBnotStarted" runat="server" Text="-"></asp:Label></td>
                                                <td class="status">
                                                    <CTRL:StackedBar id="CTRLStackedBar" runat="server"></CTRL:StackedBar>
                                                </td>
                                                <td class="actions">
                                                    <span class="actionbuttons">
                                                        <asp:HyperLink ID="HYPdetail" runat="server" Text="" CssClass="actionbutton details" Visible="false" />
                                                        <asp:HyperLink ID="HYPstats" runat="server" Text="" CssClass="actionbutton stats" Visible="false" />
                                                        <asp:HyperLink ID="HYPquizstats" runat="server" Text="" CssClass="actionbutton charts" Visible="false" />
                                                        <asp:HyperLink ID="HYPeditMetadata" runat="server" Text="" CssClass="actionbutton editmetadata" Visible="false" />
                                                        <%--<span class="actionbutton">&nbsp;</span>--%>
                                                    </span>
                                                </td> 
                                            </tr>
                                        </ItemTemplate>
                                    </asp:Repeater>
                                </ItemTemplate>
                            </asp:Repeater>

                        </ItemTemplate>                        
                    </asp:Repeater>
</tbody>
<tfoot>
<tr>
    <td class="description">Name</td>
    <td class="completedpassed">Completed and Passed</td>
    <td class="started">Started</td>
    <td class="notstarted">Not Started</td>
    <td class="status">Status</td>
    <td class="actions">Actions</td>
</tr>
</tfoot>
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