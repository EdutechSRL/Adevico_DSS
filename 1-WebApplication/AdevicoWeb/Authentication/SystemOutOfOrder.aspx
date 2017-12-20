<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Authentication.master"
    CodeBehind="SystemOutOfOrder.aspx.vb" Inherits="Comunita_OnLine.SystemOutOfOrder" %>

<%@ MasterType VirtualPath="~/Authentication.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="PageTitleContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="CPHmenu" runat="server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="CPHcontent" runat="server">
    <div id="form" class="section">
        <h2>
            <asp:Literal ID="LTtitleSystemOutOforder" runat="server">Accesso momentaneamente non disponibile</asp:Literal></h2>
        <span class="info-label">
            <asp:Literal ID="LToutOfOrderInfo" runat="server">Adevico è attualmente in manutenzione. L'accesso sar&agrave; ripristinato a breve.</asp:Literal>
        </span>
        <span class="info-label">
            <asp:Literal ID="LTsupportInfo" runat="server"></asp:Literal>
        </span>
        <span class="info-label admin-access">
            <asp:Literal ID="LTaccess" runat="server">
                Gli amministratori di sistema possono accedere tramite autenticazione <a href="#">Comol</a>
            </asp:Literal>
        </span>
    </div>
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="CPHbottomScripts" runat="server">
</asp:Content>
