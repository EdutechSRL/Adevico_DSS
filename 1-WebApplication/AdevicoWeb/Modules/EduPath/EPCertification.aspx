<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/AjaxPortal.Master"
    CodeBehind="EPCertification.aspx.vb" Inherits="Comunita_OnLine.EPCertification" %>

<%@ MasterType VirtualPath="~/AjaxPortal.Master" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register Src="UC/UC_CertificationAction.ascx" TagName="CertificationAction" TagPrefix="CTRL" %>
<%@ Register TagPrefix="CTRL" TagName="Messages" Src="~/Modules/Common/UC/UC_ActionMessages.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="PageTitleContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="CPHserviceLocalization" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="HeadContent" runat="server">
    <link href="../../Graphics/Modules/Edupath/css/edupath.css" rel="Stylesheet" />
     <script type="text/javascript">
         var DisplayTitle = ""
         var DisplayMessage = "<h2><%=DisplayMessageToken%></h2>"
    </script>
    <script type="text/javascript" src="<%#ResolveUrl("~/Jscript/FileDownloadCookie.js")%>"></script>
   <%-- <script type="text/javascript">
        var TokenHiddenFieldId = "<% = HDNdownloadTokenValue.ClientID %>";
        var CookieName = "<% = Me.CookieName %>";
        var DisplayMessage = "<% = Me.DisplayMessageToken %>";
    </script>
    <script type="text/javascript">
        var fileDownloadCheckTimer;
        function blockUIForDownload() {
            var token = new Date().getTime(); //use the current timestamp as the token value
            $("input[id='" + TokenHiddenFieldId + "']").val(token);
            $.blockUI({ message: "'" + DisplayMessage + "'", title: " ", draggable: false, theme: true });
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

       
    </script>--%>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="TitleContent" runat="server">
    <asp:Label runat="server" ID="LBserviceCertification">**Certification</asp:Label>
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="CPHservice" runat="server">
    <asp:MultiView runat="server" ID="MLVcertification">
        <asp:View runat="server" ID="VIWcertification">
            <asp:HiddenField runat="server" ID="HDNdownloadTokenValue" />
            <div class="DivEpButton">
                <asp:HyperLink CssClass="linkMenu" runat="server" ID="HYPback"></asp:HyperLink>
            </div>
            <div class="pathcertification epsummary">
                <div class="fieldobject filters">
                    <div class="fieldrow filterstartdate">
                        <asp:Label runat="server" ID="LBfilterStartDateTitle" CssClass="fieldlabel">**Start date</asp:Label>
                        <telerik:RadDatePicker ID="RDPStartDate" runat="server">
                            <Calendar ID="Calendar1" UseColumnHeadersAsSelectors="False" UseRowHeadersAsSelectors="False"
                                ViewSelectorText="x" runat="server">
                            </Calendar>
                            <DateInput ID="DateInput1" DateFormat="dd/MM/yyyy" DisplayDateFormat="dd/MM/yyyy"
                                Width="" runat="server">
                            </DateInput>
                            <DatePopupButton CssClass="" HoverImageUrl="" ImageUrl="" />
                        </telerik:RadDatePicker>
                    </div>
                    <div class="fieldrow filterenddate">
                        <asp:Label runat="server" ID="LBfilterEndDateTitle" CssClass="fieldlabel">**End date</asp:Label>
                        <telerik:RadDatePicker ID="RDPEndDate" runat="server">
                            <Calendar ID="Calendar2" UseColumnHeadersAsSelectors="False" UseRowHeadersAsSelectors="False"
                                ViewSelectorText="x" runat="server">
                            </Calendar>
                            <DateInput ID="DateInput2" DateFormat="dd/MM/yyyy" DisplayDateFormat="dd/MM/yyyy"
                                Width="" runat="server">
                            </DateInput>
                            <DatePopupButton CssClass="" HoverImageUrl="" ImageUrl="" />
                        </telerik:RadDatePicker>
                    </div>
                    <div class="fieldrow">
                        <asp:Label runat="server" ID="LBfilterCertificateTitle" CssClass="fieldlabel">**Certificate</asp:Label>
                        <asp:DropDownList runat="server" ID="DDLCertificateFilters">
                        </asp:DropDownList>
                    </div>
                    <div class="fieldrow">
                        <asp:Label runat="server" ID="LBfilterUserTitle" CssClass="fieldlabel">**User</asp:Label>
                        <asp:TextBox runat="server" ID="TXBuserFilter"></asp:TextBox>
                    </div>
                    <div class="fieldrow">
                        <asp:Label runat="server" ID="LBfilterCommunityRoleTitle" CssClass="fieldlabel">**Community Role</asp:Label>
                        <asp:DropDownList runat="server" ID="DDLRolesFilters">
                        </asp:DropDownList>
                    </div>
                    <div class="fieldrow">
                        <asp:Button runat="server" ID="BTNupdate" Text="**update" />
                    </div>
                </div>
                <asp:CheckBox runat="server" ID="CHBshowall" Text="**all statistics" AutoPostBack="true" />
                <div class="certificateitem">
                    <asp:Label runat="server" ID="LBcertificateName" CssClass="certificatetitle">**Certificate</asp:Label>
                    <CTRL:Messages ID="CTRLmessages" runat="server" Visible="false" />
                    <table class="table light certificateslist fullwidth">
                        <thead>
                            <tr>
                                <th class="person">
                                    <asp:Label runat="server" ID="LBusernameHeader">**User</asp:Label>
                                </th>
                                <th class="date">
                                    <asp:Label runat="server" ID="LBdateHeader">**Date</asp:Label>
                                </th>
                                <th class="actions">
                                    <asp:Label runat="server" ID="LBactionsHeader">**Actions</asp:Label>
                                </th>
                            </tr>
                        </thead>
                        <tbody>
                            <asp:Repeater runat="server" ID="RPTcertifications">
                                <ItemTemplate>
                                    <tr>
                                        <td class="person">
                                            <asp:Label runat="server" ID="LBusername" Visible="false">**User</asp:Label>
                                            <asp:HyperLink runat="server" ID="HYPusername">**User</asp:HyperLink>
                                        </td>
                                        <td class="date">
                                            <asp:Label runat="server" ID="LBdate">**Date</asp:Label>
                                        </td>
                                        <td class="actions">
                                            <span class="icons">
                                                <%--<asp:HyperLink runat="server" ID="HYPdownload" CssClass="icon download">&nbsp;</asp:HyperLink>
                <asp:HyperLink runat="server" ID="HYPmail" CssClass="icon mail">&nbsp;</asp:HyperLink>--%>
                                               <%-- <asp:LinkButton runat="server" ID="LNBcertificate" CssClass="icon certificate" CommandName="certification"
                                                    OnClientClick="blockUIForDownload();return true;"></asp:LinkButton>--%>
                                                <asp:HyperLink ID="HYPexportCertification" Target="_blank" runat="server" Text="" CssClass="icon certificate fileRepositoryCookieNoRefresh" Visible="false"></asp:HyperLink>
                                                <asp:HyperLink ID="HYPrefreshCertificate" Target="_blank" runat="server" Text="" CssClass="icon refresh fileRepositoryCookieNoRefresh" Visible="false" />
                                                <CTRL:CertificationAction runat="server" ID="CTRLcertificationAction" Visible="false" onGetHiddenIdentifierValueEvent="GetHiddenIdentifierValueEvent" onItemActionResult="ItemActionResult" />
                                            </span>
                                        </td>
                                    </tr>
                                </ItemTemplate>
                            </asp:Repeater>
                            <tr runat="server" id="TRempty" visible="false" class="norecordrow">
                                <td colspan="3">
                                    <asp:Label runat="server" ID="LBLempty">**empty</asp:Label>
                                </td>
                            </tr>
                        </tbody>
                    </table>
                </div>
            </div>
        </asp:View>
        <asp:View runat="server" ID="VIWerror">
            <div id="DVerror" align="center">
                <div align="center">
                    <asp:Label ID="LBerror" runat="server" CssClass="messaggio">**error</asp:Label>
                </div>
            </div>
        </asp:View>
    </asp:MultiView>
</asp:Content>
