<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/AjaxPortal.Master" CodeBehind="RecycleBin.aspx.vb" Inherits="Comunita_OnLine.TileRecycleBin" %>
<%@ MasterType VirtualPath="~/AjaxPortal.Master" %>
<%@ Register TagPrefix="CTRL" TagName="Header" Src="~/Modules/Tile/UC/UC_TileListHeader.ascx" %>
<%@ Register TagPrefix="CTRL" TagName="List" Src="~/Modules/Tile/UC/UC_TileList.ascx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="PageTitleContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="CPHserviceLocalization" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="HeadContent" runat="server">
   <CTRL:Header ID="CTRLheader" runat="server"></CTRL:Header>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="TitleContent" runat="server">
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="CPHservice" runat="server">
<div class="DivEpButton">
    <asp:HyperLink ID="HYPgotoDashboardSettings" runat="server" Text="*Back to dashboard settings" CssClass="linkMenu" Visible="false"></asp:HyperLink>
    <asp:HyperLink ID="HYPgoTo_TileList" runat="server" Text="*Back" CssClass="linkMenu" Visible="false"></asp:HyperLink>
</div>
<CTRL:list ID="CTRLtiles" runat="server"></CTRL:list>
</asp:Content>