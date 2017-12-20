<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/AjaxPortal.Master" CodeBehind="EnrollTo.aspx.vb" Inherits="Comunita_OnLine.EnrollTo" %>
<%@ Register TagPrefix="CTRL" TagName="CTRLdashboardTopBarHeader" Src="~/Modules/DashBoard/UC/UC_DashboardTopBarHeader.ascx" %>
<%@ Register TagPrefix="CTRL" TagName="CTRLdashboardEnrollHeader" Src="~/Modules/DashBoard/UC/UC_EnrollToCommunitiesHeader.ascx" %>
<%@ Register TagPrefix="CTRL" TagName="CTRLdashboardEnroll" Src="~/Modules/DashBoard/UC/UC_EnrollToCommunities.ascx" %>
<%@ MasterType VirtualPath="~/AjaxPortal.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="PageTitleContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="CPHserviceLocalization" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="HeadContent" runat="server">
    <CTRL:CTRLdashboardEnrollHeader runat="server" ID="CTRLdashboardEnrollHeader"></CTRL:CTRLdashboardEnrollHeader>
    <CTRL:CTRLdashboardTopBarHeader runat="server" ID="CTRLtopbarHeader"></CTRL:CTRLdashboardTopBarHeader>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="TitleContent" runat="server">
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="CPHservice" runat="server">
    <div class="homepage">
        <div class="homecontent">
            <CTRL:CTRLdashboardEnroll id="CTRLenroll" runat="server"></CTRL:CTRLdashboardEnroll>
        </div>
    </div>
</asp:Content>