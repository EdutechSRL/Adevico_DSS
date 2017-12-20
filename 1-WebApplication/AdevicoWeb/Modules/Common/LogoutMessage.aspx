<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Authentication.master" CodeBehind="LogoutMessage.aspx.vb" Inherits="Comunita_OnLine.LogoutMessage" %>
<%@ MasterType VirtualPath="~/Authentication.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="PageTitleContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="CPHmenu" runat="server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="CPHcontent" runat="server">
    <div id="form" class="section Wizard">
        <h2><asp:Literal ID="LTlogoutMessageTitle" runat="server"></asp:Literal></h2>
    </div>
    <div id="data_content">
        <div id="Wizard">
            <br /><br /><br /><br /><br /><br /><br />
            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
            <asp:Label ID="LBlogoutMessage" runat="server" CssClass="Titolo_Campo"></asp:Label>
            <br /><br /><br /><br />
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="CPHbottomScripts" runat="server">
</asp:Content>
