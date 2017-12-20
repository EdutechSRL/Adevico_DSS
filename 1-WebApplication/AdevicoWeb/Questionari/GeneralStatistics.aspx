<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/AjaxPopup.Master" CodeBehind="GeneralStatistics.aspx.vb" Inherits="Comunita_OnLine.GeneralStatistics" %>

<%@ MasterType VirtualPath="~/AjaxPopup.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="Server">
    <link media="screen" href="stile.css?v=201604071200lm" type="text/css" rel="StyleSheet" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="CPHservice" runat="Server">
    <asp:MultiView runat="server" ID="MLVquestionari">
        <asp:View ID="VIWdati" runat="server">
            <br />
            <asp:PlaceHolder ID="PHStat" runat="server"></asp:PlaceHolder>
            <br />
            <br />
            <asp:Label ID="LBerrorMSG" runat="server" CssClass="errore" Visible="false">No data</asp:Label>
        </asp:View>
        <asp:View runat="server" ID="VIWmessaggi">
            <asp:Label ID="LBerrore" runat="server" CssClass="errore">Non hai i permessi</asp:Label>
        </asp:View>
    </asp:MultiView>
</asp:Content>