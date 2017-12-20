<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/AjaxPortal.Master" CodeBehind="Add.aspx.vb" Inherits="Comunita_OnLine.AddTile" %>
<%@ MasterType VirtualPath="~/AjaxPortal.Master" %>
<%@ Register TagPrefix="CTRL" TagName="Edit" Src="~/Modules/Tile/UC/UC_EditTile.ascx" %>
<%@ Register TagPrefix="CTRL" TagName="EditHeader" Src="~/Modules/Common/UC/UC_AsyncUploadHeader.ascx" %>
<%@ Register TagPrefix="CTRL" TagName="Messages" Src="~/Modules/Common/UC/UC_ActionMessages.ascx" %>
<%@ Register TagPrefix="CTRL" TagName="Header" Src="~/Modules/Tile/UC/UC_BaseTileHeader.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="PageTitleContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="CPHserviceLocalization" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="HeadContent" runat="server">
    <CTRL:Header ID="CTRLbaseHeader" runat="server"></CTRL:Header>
    <CTRL:EditHeader ID="CTRLheader" runat="server"></CTRL:EditHeader>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="TitleContent" runat="server">
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="CPHservice" runat="server">
    <div class="DivEpButton">
        <asp:LinkButton ID="LNBaddTileTop" runat="server" Visible="false" Text="*Save" CssClass="linkMenu"></asp:LinkButton>
        <asp:HyperLink ID="HYPgoTo_TileListTop" runat="server" Text="*Back to list" CssClass="linkMenu" Visible="false"></asp:HyperLink>
    </div>
    <CTRL:Messages ID="CTRLmessages" runat="server" Visible="false" />
    <CTRL:Edit id="CTRLedit" runat="server" />
    <div class="DivEpButton" id="DVmenuBottom" runat="server" Visible="false" >
        <asp:HyperLink ID="HYPgoTo_TileListBottom" runat="server" Text="*Back to list" CssClass="linkMenu" Visible="false"></asp:HyperLink>
        <asp:LinkButton ID="LNBaddTileBottom" runat="server" Visible="false" Text="*Save" CssClass="linkMenu"></asp:LinkButton>
    </div>
</asp:Content>