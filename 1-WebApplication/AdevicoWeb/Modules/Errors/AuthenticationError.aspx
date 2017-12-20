<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Authentication.master" CodeBehind="AuthenticationError.aspx.vb" Inherits="Comunita_OnLine.AuthenticationError" %>
<%@ MasterType VirtualPath="~/Authentication.Master" %>
<%@ Register TagPrefix="CTRL" TagName="Messages" Src="~/Modules/Common/UC/UC_ActionMessages.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="PageTitleContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="CPHmenu" runat="server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="CPHcontent" runat="server">
    <CTRL:Messages ID="CTRLmessage" runat="server" />
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="CPHbottomScripts" runat="server">
   
</asp:Content>