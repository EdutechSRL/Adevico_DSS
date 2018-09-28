<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/AjaxPortal.Master"
    CodeBehind="UserListStatistics.aspx.vb" Inherits="Comunita_OnLine.UserListStatistics" %>

<%@ Register Src="UC/UC_TimeStat.ascx" TagName="SelectTime" TagPrefix="CTRL" %>
<%@ Register Src="UC/UC_TextAction.ascx" TagName="TextAction" TagPrefix="CTRL" %>
<%@ Register Src="UC/UC_CertificationAction.ascx" TagName="CertificationAction" TagPrefix="CTRL" %>
<%@ Register TagPrefix="CTRL" TagName="DisplayItem" Src="~/Modules/Repository/Common/UC_ModuleRenderAction.ascx" %>
<%@ Register TagPrefix="CTRL" TagName="QuestionnaireItem" Src="~/Modules/Questionnaire/UC/UC_ModuleQuizAction.ascx" %>
<%@ Register TagPrefix="CTRL" TagName="Messages" Src="~/Modules/Common/UC/UC_ActionMessages.ascx" %>
<%@ Register TagPrefix="CTRL" TagName="GridPager" Src="../../UC/UC_PagerControl.ascx" %>

<%@ MasterType VirtualPath="~/AjaxPortal.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <%--<link href="PfStyle.css" type="text/css" rel="stylesheet" />--%>
    <%--<link href="../../Graphics/Modules/Edupath/css/edupathstats.css" type="text/css" rel="stylesheet" />--%>
    <script src="../../Jscript/Modules/Common/jquery.ddbuttonlist.js" type="text/javascript"></script>
    <link href="../../Graphics/Modules/Edupath/css/<%=GetCssFileByType()%>edupath.css?v=201605041410lm" rel="Stylesheet" />
     <script type="text/javascript">
         var DisplayTitle = ""
         var DisplayMessage = "<h2><%=DisplayMessageToken%></h2>"
    </script>
    <script type="text/javascript" src="<%#ResolveUrl("~/Jscript/FileDownloadCookie.js")%>"></script>

    <script type="text/javascript">
        var TokenHiddenFieldId = "<% = HDNdownloadTokenValue.ClientID %>";
        var CookieStatisticsName = "<% = Me.CookieName %>";
        var DisplayStatisticsMessage = "<% = Me.DisplayMessageToken %>";
    </script>
    <script type="text/javascript">
        var fileDownloadCheckTimer;
        function blockUIForDownload() {
            var token = new Date().getTime(); //use the current timestamp as the token value
            $("input[id='" + TokenHiddenFieldId + "']").val(token);
            $.blockUI({ message: "'" + DisplayStatisticsMessage + "'", title: " ", draggable: false, theme: true });
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


            $('.updateVote').dialog({
                appendTo: "form",
                autoOpen: false,
                draggable: true,
                modal: true,
                title: "",
                width: 900,
                height: 600,
                minHeight: 400,
                minWidth: 600,
                zIndex: 99999,
                open: function (type, data) {
                    //$(this).parent().appendTo("form");
                }
            });

        });
    </script>
     <asp:Literal ID="LTcertificationScript" runat="server" Visible="false">
    </asp:Literal>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="CPHservice" runat="server">
    <asp:MultiView ID="MLVstat" runat="server" ActiveViewIndex="0">
        <asp:View ID="VIWstat" runat="server">
            <div class="DivEpButton">
                <div class="viewType">
                    <span class="label" id="SPANviewStatistics" runat="server">Statistiche*</span>
                    <asp:HyperLink ID="HYPuserStat" runat="server" CssClass="img_link ico_usr_perm_l"></asp:HyperLink>
                    <asp:HyperLink ID="HYPpathStat" runat="server" Text="*path stat"  CssClass="img_link ico_stat_l"></asp:HyperLink>
                </div>
                 <div runat="server" id="DIVexport" class="ddbuttonlist enabled" visible="true"><!--   
                    --><asp:LinkButton ID="LNBexportUsersStatisticToCsv" runat="server" Text="export2* " OnClientClick="blockUIForDownload();return true;" CssClass="linkMenu"></asp:LinkButton><!--
                    --><asp:LinkButton ID="LNBexportUsersStatisticToXml" runat="server" Text=" " OnClientClick="blockUIForDownload();return true;" CssClass="linkMenu"></asp:LinkButton><!--
                    --><asp:LinkButton ID="LNBexportUsersStatisticToPdf" runat="server" Text="exportPDF* " OnClientClick="blockUIForDownload();return true;" CssClass="linkMenu"></asp:LinkButton><!--
                    --><asp:LinkButton ID="LNBexportPSfullToCsv" runat="server" Text=" " OnClientClick="blockUIForDownload();return true;" CssClass="linkMenu"></asp:LinkButton><!--
                    --><asp:LinkButton ID="LNBexportPSfullToXml" runat="server" Text=" " OnClientClick="blockUIForDownload();return true;" CssClass="linkMenu"></asp:LinkButton><!--
                    --><asp:LinkButton ID="LNBexportPSfullCertificationToCsv" runat="server" Text=" " OnClientClick="blockUIForDownload();return true;" CssClass="linkMenu"></asp:LinkButton><!--
                    --><asp:LinkButton ID="LNBexportPSfullCertificationToXml" runat="server" Text="" OnClientClick="blockUIForDownload();return true;" CssClass="linkMenu"></asp:LinkButton><!--                    
                --></div>   
                <asp:LinkButton ID="LKBupdateList" runat="server" Text="*update user list" CssClass="Link_Menu" visible="false"></asp:LinkButton>
                <asp:HyperLink ID="HYPeduPathView" runat="server" Text="**edu view" CssClass="Link_Menu"></asp:HyperLink>
                <asp:LinkButton ID="LKBeduPathView" runat="server" Text="*eduview" CssClass="Link_Menu" Visible="false"></asp:LinkButton>
                <asp:Button ID="BTNevaluate" runat="server" Text="*eval" CssClass="Link_Menu" />
            </div>
            <div class="dateFilter">
                <ctrl:SelectTime id="CTRLselTime" runat="server"></ctrl:SelectTime>
                <br /><asp:CheckBox runat="server" ID="CHBshowall" Visible="true" Text="Show hidden" Checked="false" AutoPostBack="true" />
            </div>
            <asp:RadioButtonList ID="RBLfilter" runat="server" RepeatDirection="Horizontal" CssClass="Titolo_dettagliSmall" AutoPostBack="true" />
            <div class="pathTitle">
                <asp:Image ID="IMGmandatory" runat="server" Visible="false" />
                <asp:Label ID="LBitemType" runat="server" CssClass="Titolo_campo inline"></asp:Label>
                <asp:Label ID="LBitemName" runat="server" CssClass="label inline" Visible="false" style="margin-bottom:3px;"></asp:Label>
                <CTRL:QuestionnaireItem ID="CTRLquestionnaire" runat="server" EnableAnchor="true"  Visible="false"/>
                <CTRL:DisplayItem ID="CTRLdisplayItem" runat="server" EnableAnchor="true" DisplayExtraInfo="false" DisplayLinkedBy="false" DisplaySize="false"  Visible="false"/>
                <CTRL:TextAction runat="server" id="CTRLtextAction"/>
                <CTRL:CertificationAction runat="server" id="CTRLcertificationAction" visible="false" />
                <asp:Label ID="LBpoint" runat="server"></asp:Label>
                <asp:Image ID="IMGsubType" runat="server" Visible="false" />
                <asp:LinkButton ID="LKBblocked" runat="server" Text="Visible**" Visible="false"></asp:LinkButton>
                <asp:Image ID="IMGblocked" runat="server" Visible="false" />
            </div>
            <br />
            <CTRL:Messages ID="CTRLmessages" runat="server" Visible="false" />
            <div align="center">
                <asp:Label ID="LBnoPartecipant" runat="server" Visible="false" CssClass="Titolo_campo_Rosso" />
            </div>
            <div id="DIVpartecipants" runat="server">                
                <table class="stats users" id="">
                    <thead>
                        <tr>
                            <th class="description"><asp:Label ID="LBnameTit" runat="server" Text="*Nome"> </asp:Label></th>
                            <th class="timecompletion"><asp:Label ID="LBcompletionTit" runat="server" Text="*completion"></asp:Label></th>
                            <th id="TDmark" runat="server" class="mark">
                                <asp:Label ID="LBmarkTit" runat="server" Text="*mark"></asp:Label>
                            </th>
                            <th class="status"><asp:Label ID="LBstatusTit" runat="server" Text="*status"></asp:Label></th>
                            <th class="actions" id="TDdetail" runat="server"><asp:Label ID="LBactionsTit" runat="server" Text="**Actions"></asp:Label></th>
                        </tr>
                    </thead>
                    <tbody>
                        <asp:Repeater ID="RPitem" runat="server" OnItemCommand="RPsubAct_ItemCommand">

                            <ItemTemplate>
                                <tr class="userdetail <%# me.BackGroundItem(Container.ItemType) %>">
                                    <td class="description">
                                        <span class="iteminfo">
                                            <span class="name"><asp:Label ID="LBname" runat="server"> </asp:Label></span>
                                        </span>
                                    </td>
                                    <td class="timecompletion"><asp:Label ID="LBcompletion" runat="server" Text="-"></asp:Label>
                                        <asp:Image ID="IMGcompletion" runat="server" Visible="false" />
                                        <span class="flag <%# Me.FlagClass(Container.Dataitem) %>">&nbsp;</span>
                                        </td>
                                        <td id="TDmark" runat="server" class="mark">
                                        <asp:Label ID="LBmark" runat="server" Text="-" Visible="false"></asp:Label>
                                        <asp:Panel ID="PNLeval" runat="server" Visible="false">
                                            <asp:TextBox ID="TXBmark" runat="server" Widtd="30px" MaxLengtd="3" TextMode="SingleLine"></asp:TextBox>
                                            <asp:RangeValidator ID="RNVminCompletion" runat="server" ErrorMessage="*" Text=""
                                                ControlToValidate="TXBmark" MinimumValue="0" MaximumValue="100" Type="Integer"
                                                CssClass="validator"></asp:RangeValidator>
                                            <asp:CompareValidator ID="COVminCompletion" runat="server" ErrorMessage="*" Text=""
                                                ControlToValidate="TXBmark" Type="Integer" Operator="DataTypeCheck" CssClass="validator"></asp:CompareValidator>
                                        </asp:Panel>
                                        <asp:Image ID="IMGmark" runat="server" Visible="false" />
                                    </td>
		                            <td class="status"><asp:Label ID="LBstatus" runat="server" Text="*status" Widtd="75%" visible="false"></asp:Label>
                                    <span class="item <%# Me.SetStatus(Container.DataItem) %>"><%# Me.SetStatusString(Container.DataItem) %></span></td>
                                    <td class="actions" id="TDdetail" runat="server">
                                        <span class="actionbuttons">
                                             <asp:HyperLink ID="HYPdetail" runat="server" Text="" CssClass="actionbutton details" />     
                                             <CTRL:CertificationAction runat="server" ID="CTRLcertificationAction" Visible="false" onItemActionResult="ItemActionResult" />
                                             <asp:LinkButton ID="LNBexportCertification" runat="server" Text="" CssClass="actionbutton certificate" Visible="false" CommandName="certification" OnClientClick="blockUIForDownload();return true;"></asp:LinkButton>
                                            <asp:HyperLink ID="HYPexportCertification" Target="_blank" runat="server" Text="" CssClass="actionbutton certificate fileRepositoryCookieNoRefresh" Visible="false" />
                                            <asp:HyperLink ID="HYPrefreshCertificate" Target="_blank" runat="server" Text="" CssClass="actionbutton refresh fileRepositoryCookieNoRefresh" Visible="false" />
                                        </span>
                                    </td>
                                </tr>
                            </ItemTemplate>
                        </asp:Repeater>
                        
                    </tbody>
                </table>
            </div>
            <div style="width: 900px; text-align: right; padding-top: 5px; clear: both; height: 22px;">
                <CTRL:GridPager ID="PGgrid" runat="server" EnableQueryString="false"></CTRL:GridPager>
            </div>
            <asp:Literal ID="LTalert" runat="server"></asp:Literal>
        </asp:View>
        <asp:View ID="VIWerror" runat="server">
            <div id="DVerror">
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
    <asp:Literal ID="LTopenDialog" runat="server" Visible="false">
        <script type="text/javascript">
            $(document).ready(function () {
                $('.updateVote').dialog('open');
            });
        </script>
    </asp:Literal>
    <div class="dialog updateVote">
        <div>
            <asp:Label ID="LBsaveVote" runat="server" Text="save vote" />
        </div>
        <div>
            <asp:Button ID="BTNsave" runat="server" Text="*save" />
            <asp:Button ID="BTNnoSave" runat="server" Text="noSave" />
        </div>
    </div>
</asp:Content>
