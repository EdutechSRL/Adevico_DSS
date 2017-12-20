<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/AjaxPortal.Master" CodeBehind="PortalDashboardCombined.aspx.vb" Inherits="Comunita_OnLine.PortalDashboardCombined" %>
<%@ Register TagPrefix="CTRL" TagName="CTRLdashboardTopBar" Src="~/Modules/DashBoard/UC/UC_DashboardTopBar.ascx" %>
<%@ Register TagPrefix="CTRL" TagName="CTRLdashboardTopBarHeader" Src="~/Modules/DashBoard/UC/UC_DashboardTopBarHeader.ascx" %>
<%@ Register TagPrefix="CTRL" TagName="CTRLnoticeboardBlock" Src="~/Modules/DashBoard/UC/UC_NoticeboardBlock.ascx" %>
<%@ Register TagPrefix="CTRL" TagName="CTRLminiTile" Src="~/Modules/DashBoard/UC/UC_MiniTileList.ascx" %>

<%@ MasterType VirtualPath="~/AjaxPortal.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="PageTitleContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="CPHserviceLocalization" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="HeadContent" runat="server">
    <CTRL:CTRLdashboardTopBarHeader runat="server" ID="CTRLtopbarHeader"></CTRL:CTRLdashboardTopBarHeader>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="TitleContent" runat="server">
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="CPHservice" runat="server">
<div class="homepage">
    <CTRL:CTRLdashboardTopBar runat="server" ID="CTRLdashboardTopBar"></CTRL:CTRLdashboardTopBar>
    <div class="homecontent">
        <div class="list combined container_12 <%=GetNoticeboardPosition()%> clearfix">
            <ctrl:CTRLnoticeboardBlock id="CTRLnoticeboard" runat="server"></ctrl:CTRLnoticeboardBlock>
            <div class="maincontent grid_<%=GetContentItemColspan()%>">
                <ctrl:CTRLminiTile id="CTRLminiTile" runat="server" Visible="false"></ctrl:CTRLminiTile>
            </div>
        </div>
    </div>           
</div>
</asp:Content>