<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Authentication.master"
    CodeBehind="RetrievePassword.aspx.vb" Inherits="Comunita_OnLine.RetrievePassword" %>

    <%@ MasterType VirtualPath="~/Authentication.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="PageTitleContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="CPHmenu" runat="server">
    <asp:Literal ID="LTbackToLoginPage" runat="server" Visible="false" />
    <asp:Literal ID="LTshibbolethLogon" runat="server" Visible="false"/>
    <asp:Literal ID="LTexternalWebLogon" runat="server" Visible="false"/>
    <asp:Literal ID="LTsubscription" runat="server" Visible="false"/>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="CPHcontent" runat="server">
    <div id="form" class="section">
        <h2>
            <asp:Literal ID="LTtitleInternalLoginRetrieve" runat="server"></asp:Literal></h2>
        <span class="info-label">
            <asp:Literal ID="LTmailInfo" runat="server">Inserisci la email con la quale sei iscritto al sistema.</asp:Literal></span>
        <asp:TextBox ID="TXBmail" CssClass="textbox" runat="server" Text="Mail"></asp:TextBox>
        <asp:Button ID="BTNretrievePassword" runat="server" CssClass="submit" Text="Richiesta" />
        <div id="submit-feedback">
            <span class="invisible" runat="server" id="SPNmessages">
                <asp:Literal ID="LTretrieveError" runat="server"></asp:Literal><br />
                <asp:Literal ID="LTretrieveErrorSubscription" runat="server" />
                <asp:Literal ID="LTretrieveErrorShibbolethAccount" runat="server" />
            </span>
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="CPHbottomScripts" runat="server">
</asp:Content>
