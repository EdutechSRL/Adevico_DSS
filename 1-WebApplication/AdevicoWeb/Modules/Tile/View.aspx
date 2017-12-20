<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/AjaxPortal.Master" CodeBehind="View.aspx.vb" Inherits="Comunita_OnLine.ViewTile" %>
<%@ MasterType VirtualPath="~/AjaxPortal.Master" %>
<%@ Register TagPrefix="CTRL" TagName="View" Src="~/Modules/Tile/UC/UC_ViewTile.ascx" %>
<%@ Register TagPrefix="CTRL" TagName="Messages" Src="~/Modules/Common/UC/UC_ActionMessages.ascx" %>
<%@ Register TagPrefix="CTRL" TagName="Header" Src="~/Modules/Tile/UC/UC_BaseTileHeader.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="PageTitleContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="CPHserviceLocalization" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="HeadContent" runat="server">
   <CTRL:Header ID="CTRLbaseHeader" runat="server"></CTRL:Header>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="TitleContent" runat="server">
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="CPHservice" runat="server">
    <div class="DivEpButton">
        <asp:HyperLink ID="HYPgoTo_TileListTop" runat="server" Text="*Back to list" CssClass="linkMenu" Visible="false"></asp:HyperLink>
    </div>
    <CTRL:Messages ID="CTRLmessages" runat="server" Visible="false" />
    <CTRL:View id="CTRLview" runat="server" Visible="true" />
    <div class="DivEpButton" id="DVmenuBottom" runat="server" Visible="false" >
        <asp:HyperLink ID="HYPgoTo_TileListBottom" runat="server" Text="*Back to list" CssClass="linkMenu" Visible="false"></asp:HyperLink>
    </div>
</asp:Content>