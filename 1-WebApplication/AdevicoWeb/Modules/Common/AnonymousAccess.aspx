<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Authentication.master" CodeBehind="AnonymousAccess.aspx.vb" Inherits="Comunita_OnLine.AnonymousAccess" %>
<%@ MasterType VirtualPath="~/Authentication.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="PageTitleContent" runat="server">

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="HeadContent" runat="server">

</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="CPHmenu" runat="server">

</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="CPHcontent" runat="server">
  <div id="form" class="section">
        <h2><asp:Literal ID="LTtitle" runat="server"></asp:Literal></h2>
        <span class="info-label">
            <asp:Literal ID="LTdisplayInfo" runat="server"></asp:Literal>
        </span> 
    </div>
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="CPHbottomScripts" runat="server">

</asp:Content>