<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/AjaxPortal.Master" CodeBehind="PortalDashboardList.aspx.vb" Inherits="Comunita_OnLine.PortalDashboardList" %>
<%@ Register TagPrefix="CTRL" TagName="CTRLdashboardTopBar" Src="~/Modules/DashBoard/UC/UC_DashboardTopBar.ascx" %>
<%@ Register TagPrefix="CTRL" TagName="CTRLdashboardTopBarHeader" Src="~/Modules/DashBoard/UC/UC_DashboardTopBarHeader.ascx" %>
<%@ Register TagPrefix="CTRL" TagName="CTRLnoticeboardBlock" Src="~/Modules/DashBoard/UC/UC_NoticeboardBlock.ascx" %>
<%@ Register TagPrefix="CTRL" TagName="CTRLlist" Src="~/Modules/DashBoard/UC/UC_ListView.ascx" %>


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
            <div class="list plain container_12 <%=GetNoticeboardPosition()%> clearfix">
                <ctrl:CTRLnoticeboardBlock id="CTRLnoticeboard" runat="server"></ctrl:CTRLnoticeboardBlock>
                <div class="maincontent grid_<%=GetContentItemColspan()%>">
                    <ctrl:CTRLlist id="CTRLlistMyCommunities" runat="server" TitleCssClass="community" Visible="false"></ctrl:CTRLlist>
                    <ctrl:CTRLlist id="CTRLlistMyOrganizations" runat="server" AllowAutoUpdateCookie="false" TitleCssClass="organization" IsCollapsable="true" Visible="false" DataIdForCollapse="cl-organizations" UseDefaultStartupItems="true" DefaultStartupItems="5"></ctrl:CTRLlist>
                </div>
            </div>
        </div>
    </div>
</asp:Content>