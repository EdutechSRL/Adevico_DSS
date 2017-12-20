<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Authentication.master"
    CodeBehind="ActivateUserProfile.aspx.vb" Inherits="Comunita_OnLine.ActivateUserProfile" %>

<%@ MasterType VirtualPath="~/Authentication.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="PageTitleContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="CPHmenu" runat="server">
    <asp:Literal ID="LTinternalLoginPage" runat="server" Visible="false" />
    <asp:Literal ID="LTexternalWebLogon" runat="server" Visible="false" />
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="CPHcontent" runat="server">
    <div id="form" class="section">
        <h2>
            <asp:Literal ID="LTtitleActivateUserProfile" runat="server"></asp:Literal></h2>
       <br /><br /><br /><br /><br />

       <asp:label ID="LBmessage" runat="server"></asp:label>
       <br /><br /><br /><br /><br />
    </div>
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="CPHbottomScripts" runat="server">

</asp:Content>