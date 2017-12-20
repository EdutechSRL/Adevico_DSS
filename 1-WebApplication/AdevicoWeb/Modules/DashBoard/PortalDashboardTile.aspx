<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/AjaxPortal.Master" CodeBehind="PortalDashboardTile.aspx.vb" Inherits="Comunita_OnLine.PortalDashboardTile" %>
<%@ Register TagPrefix="CTRL" TagName="CTRLdashboardTopBar" Src="~/Modules/DashBoard/UC/UC_DashboardTopBar.ascx" %>
<%@ Register TagPrefix="CTRL" TagName="CTRLdashboardTopBarHeader" Src="~/Modules/DashBoard/UC/UC_DashboardTopBarHeader.ascx" %>
<%@ Register TagPrefix="CTRL" TagName="CTRLtiles" Src="~/Modules/DashBoard/UC/UC_Tiles.ascx" %>
<%@ Register TagPrefix="CTRL" TagName="CTRLnoticeboard" Src="~/Modules/DashBoard/UC/UC_NoticeboardTile.ascx" %>
<%@ Register TagPrefix="CTRL" TagName="Messages" Src="~/Modules/Common/UC/UC_ActionMessages.ascx" %>

<%@ MasterType VirtualPath="~/AjaxPortal.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="PageTitleContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="CPHserviceLocalization" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="HeadContent" runat="server">
    <CTRL:CTRLdashboardTopBarHeader runat="server" ID="CTRLtopbarHeader"></CTRL:CTRLdashboardTopBarHeader>
</asp:Content><asp:Content ID="Content4" ContentPlaceHolderID="TitleContent" runat="server">
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="CPHservice" runat="server">
<div class="homepage">
    <CTRL:CTRLdashboardTopBar runat="server" ID="CTRLdashboardTopBar"></CTRL:CTRLdashboardTopBar>
    <div class="homecontent">
        <CTRL:Messages ID="CTRLmessages"  runat="server" Visible="false" />
        <div class="list tiles container_12 <%=GetNoticeboardPosition()%> clearfix">
           <CTRL:CTRLtiles id="CTRLtiles" runat="server"></CTRL:CTRLtiles>
        </div>
    </div>    
</div>
</asp:Content>