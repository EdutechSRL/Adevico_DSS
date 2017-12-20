<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/AjaxPortal.Master" CodeBehind="RecycleBin.aspx.vb" Inherits="Comunita_OnLine.DashboardListRecycleBin" %>
<%@ MasterType VirtualPath="~/AjaxPortal.Master" %>
<%@ Register TagPrefix="CTRL" TagName="Messages" Src="~/Modules/Common/UC/UC_ActionMessages.ascx" %>
<%@ Register TagPrefix="CTRL" TagName="List" Src="~/Modules/Dashboard/Settings/UC/UC_SettingsList.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="PageTitleContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="CPHserviceLocalization" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="HeadContent" runat="server">
    <link href="<%=GetBaseUrl()%>Jscript/Modules/Common/Choosen/chosen.css?v=201604071200lm" rel="Stylesheet" />
    <script type="text/javascript" src="<%=GetBaseUrl()%>Jscript/Modules/Common/Choosen/chosen.jquery.js"></script>
    <link href="<%=GetBaseUrl()%>Graphics/Modules/TileTag/css/TileTag.css?v=201604071200lm" rel="Stylesheet" />
    <script src="<%=GetBaseUrl()%>Jscript/Modules/TileTag/TileTag.js" type="text/javascript"></script>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="TitleContent" runat="server">
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="CPHservice" runat="server">
<div class="DivEpButton">
    <asp:HyperLink ID="HYPgoTo_SettingsListTop" runat="server" Text="*Back" CssClass="linkMenu" Visible="false"></asp:HyperLink>
</div>
<CTRL:Messages ID="CTRLmessages" runat="server" Visible="false"/> 
<CTRL:list ID="CTRLsettings" runat="server"></CTRL:list>
<div class="DivEpButton" runat="server" id="DVbottomCommands" visible="false">
    <asp:HyperLink ID="HYPgoTo_SettingsListBottom" runat="server" Text="*Back" CssClass="linkMenu" Visible="false"></asp:HyperLink>
</div>
</asp:Content>
