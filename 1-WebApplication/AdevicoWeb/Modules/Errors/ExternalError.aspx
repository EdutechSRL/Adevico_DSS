<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/ExternalService.Master" CodeBehind="ExternalError.aspx.vb" Inherits="Comunita_OnLine.ExternalError" %>
<%@ MasterType VirtualPath="~/ExternalService.Master" %>
<%@ Register TagPrefix="CTRL" TagName="Messages" Src="~/Modules/Common/UC/UC_ActionMessages.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="PageTitleContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="PreHeaderContent" runat="server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="TopBarContent" runat="server">
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="TitleContent" runat="server">
</asp:Content>
<asp:Content ID="Content6" ContentPlaceHolderID="CPHservice" runat="server">
    <CTRL:Messages ID="CTRLmessage" runat="server"/>
</asp:Content>