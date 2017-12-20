<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/AjaxPortal.Master" CodeBehind="RecycleBin.aspx.vb" Inherits="Comunita_OnLine.TagRecycleBin" %>
<%@ MasterType VirtualPath="~/AjaxPortal.Master" %>
<%@ Register TagPrefix="CTRL" TagName="Header" Src="~/Modules/Tag//UC/UC_TagsListHeader.ascx" %>
<%@ Register TagPrefix="CTRL" TagName="List" Src="~/Modules/Tag/UC/UC_TagsList.ascx" %>

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
<div ng-app="ngtags"  ng-controller="tagcontroller">
    <div class="DivEpButton">
        <asp:HyperLink ID="HYPgoTo_TagList" runat="server" Text="*Back" CssClass="linkMenu" Visible="false"></asp:HyperLink>
    </div>
    <CTRL:list ID="CTRLtags" runat="server"></CTRL:list>
</div>
</asp:Content>