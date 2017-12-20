<%@ Page Language="vb" AutoEventWireup="false" MasterPageFile="~/AjaxPortal.Master"
    CodeBehind="GruppoAdmin.aspx.vb" Inherits="Comunita_OnLine.GruppoAdmin" Title="Pagina senza titolo" %>

<%@ MasterType VirtualPath="~/AjaxPortal.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="Server">
    <link media="screen" href="stile.css?v=201604071200lm" type="text/css" rel="StyleSheet" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="CPHservice" runat="Server">
    <asp:MultiView runat="server" ID="MLVquestionari">
        <asp:View ID="VIWdati" runat="server">
            <br />
            <br />
            <asp:Panel ID="PNLmenu" runat="server" Width="100%" HorizontalAlign="right">
                <asp:LinkButton ID="LNBCartellaPrincipale" Visible="true" runat="server" CssClass="Link_Menu"
                    CausesValidation="false"></asp:LinkButton>&nbsp;
                <asp:LinkButton ID="LNBGestioneDomande" Visible="true" runat="server" CssClass="Link_Menu"></asp:LinkButton>
            </asp:Panel>
            <asp:Label ID="LBNuovoGruppo" runat="server" Text=""></asp:Label><br />
            <asp:TextBox ID="TXBNomeGruppo" runat="server"></asp:TextBox>
            <br />
            <br />
            <asp:Label ID="LBDescrizione" runat="server" Text=""></asp:Label>
            <br />
            <asp:TextBox ID="TXBDescrizione" runat="server" Height="56px" TextMode="MultiLine"></asp:TextBox><br />
            <br />
            <asp:Button ID="BTNSalva" runat="server" Text="" />
        </asp:View>
        <asp:View runat="server" ID="VIWmessaggi">
            <asp:Label ID="LBerrore" runat="server"></asp:Label>
        </asp:View>
    </asp:MultiView>
</asp:Content>
