<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/AjaxPortal.Master"
    CodeBehind="EPUserCertification.aspx.vb" Inherits="Comunita_OnLine.EPUserCertification" %>
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
    <script type="text/javascript">
     <%--   var TokenHiddenFieldId = "<% = HDNdownloadTokenValue.ClientID %>";
        var CookieName = "<% = Me.CookieName %>";
        var DisplayMessage = "<% = Me.DisplayMessageToken %>";--%>
    </script>
    <script type="text/javascript">
        //var fileDownloadCheckTimer;
        //function blockUIForDownload() {
        //    var token = new Date().getTime(); //use the current timestamp as the token value
        //    $("input[id='" + TokenHiddenFieldId + "']").val(token);
        //    $.blockUI({ message: "'" + DisplayMessage + "'", title: " ", draggable: false, theme: true });
        //    fileDownloadCheckTimer = window.setInterval(function () {
        //        var cookieValue = $.cookie(CookieName);
        //        if (cookieValue == token)
        //            finishDownload();
        //    }, 1000);
        //}

        //function finishDownload() {
        //    window.clearInterval(fileDownloadCheckTimer);
        //    $.cookie(CookieName, null); //clears this cookie value
        //    $.unblockUI();
        //}

       
    </script>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="TitleContent" runat="server">
    <asp:Label runat="server" ID="LBserviceCertification">**Certification</asp:Label>
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="CPHservice" runat="server">
    <asp:HiddenField runat="server" ID="HDNdownloadTokenValue" />
    <asp:MultiView runat="server" ID="MLVcertification">
        <asp:View runat="server" ID="VIWcertification">
            <div class="DivEpButton">
                <asp:HyperLink CssClass="linkMenu" runat="server" ID="HYPback"></asp:HyperLink>
            </div>
            <div class="pathcertification pathcertificationuser epsummary">
                
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
                    <%--<div class="fieldrow">        
        <asp:Label runat="server" ID="LBfilterCertificateTitle" CssClass="fieldlabel">**Certificate</asp:Label>
        <asp:DropDownList runat="server" id="DDLCertificateFilters">        
        </asp:DropDownList>
    </div>
    <div class="fieldrow">        
        <asp:Label runat="server" ID="LBfilterUserTitle" CssClass="fieldlabel">**User</asp:Label>        
        <asp:TextBox runat="server" ID="TXBuserFilter"></asp:TextBox>
    </div>
    <div class="fieldrow">        
        <asp:Label runat="server" ID="LBfilterCommunityRoleTitle" CssClass="fieldlabel">**Community Role</asp:Label>
        <asp:DropDownList runat="server" id="DDLRolesFilters">        
        </asp:DropDownList>
    </div>--%>
                    <div class="fieldrow">
                        <asp:Button runat="server" ID="BTNupdate" Text="**update" />
                    </div>
                </div>
                <div class="certificateitem">
                    <div class="fieldobject details">
                        <div class="fieldrow">
                            <asp:Label runat="server" ID="LBuserInfo" CssClass="user">**User name</asp:Label>
                            <span class="divider">- </span>
                            <asp:Label runat="server" ID="LBpathInfo" CssClass="path">**Path name</asp:Label>
                        </div>
                    </div>
                    <CTRL:Messages ID="CTRLmessages" runat="server" Visible="false" />
                    <table class="certificatetable table light">
                        <thead>
                            <tr>
                                <th class="certificate">
                                    <asp:Label runat="server" ID="LBcertificateNameHeader">**Certificate</asp:Label>
                                </th>
                                <th class="date">
                                    <asp:Label runat="server" ID="LBdateHeader">**Date</asp:Label>
                                </th>
                                <th class="actions">
                                    <asp:Label runat="server" ID="LBactionsHeader">**Action</asp:Label>
                                </th>
                            </tr>
                        </thead>
                        <tbody>
                            <asp:Repeater runat="server" ID="RPTcertificates">
                                <ItemTemplate>
                                    <tr>
                                        <td class="certificate">
                                            <asp:Label runat="server" ID="LBcertificate" Visible="false">**Certificate</asp:Label>
                                            <asp:HyperLink runat="server" ID="HYPcertificate">**Certificate</asp:HyperLink>
                                        </td>
                                        <td class="date">
                                            <asp:Label runat="server" ID="LBdate">**Date</asp:Label>
                                        </td>
                                        <td class="actions">
                                            <span class="icons">
                                                <%--<span class="icon download">&nbsp;</span>--%>
                                            <%--    <asp:LinkButton runat="server" ID="LNBcertificate" CssClass="icon certificate" CommandName="certification"
                                                    OnClientClick="blockUIForDownload();return true;"></asp:LinkButton>--%>
                                                <asp:HyperLink ID="HYPexportCertification" Target="_blank" runat="server" Text="" CssClass="icon certificate fileRepositoryCookieNoRefresh" Visible="false"></asp:HyperLink>
                                                 <asp:HyperLink ID="HYPrefreshCertificate" Target="_blank" runat="server" Text="" CssClass="icon refresh fileRepositoryCookie" Visible="false" />
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
                        <tfoot>
                        </tfoot>
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
