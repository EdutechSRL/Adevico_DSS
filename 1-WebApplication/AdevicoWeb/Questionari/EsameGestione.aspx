<%@ Page Language="vb" AutoEventWireup="false" MasterPageFile="~/Ajaxportal.Master"
    CodeBehind="EsameGestione.aspx.vb" Inherits="Comunita_OnLine.EsameGestione" %>

<%@ MasterType VirtualPath="~/AjaxPortal.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="Server">
    <link media="screen" href="stile.css?v=201604071200lm" type="text/css" rel="StyleSheet" />
</asp:Content>
<asp:Content runat="server" ID="Content" ContentPlaceHolderID="CPHservice">
    <asp:Panel ID="PNLmenu" runat="server" Width="100%" HorizontalAlign="right">
        <asp:LinkButton ID="LNBQuestionarioAdmin" Visible="true" runat="server" CssClass="Link_Menu"
            CausesValidation="false"></asp:LinkButton>
    </asp:Panel>
    <br />
    <br />
</asp:Content>
