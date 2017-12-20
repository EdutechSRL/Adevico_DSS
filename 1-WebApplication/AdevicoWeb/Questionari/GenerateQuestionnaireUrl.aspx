<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/AjaxPortal.Master" CodeBehind="GenerateQuestionnaireUrl.aspx.vb" Inherits="Comunita_OnLine.GenerateQuestionnaireUrl" %>
<%@ MasterType VirtualPath="~/AjaxPortal.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="PageTitleContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="CPHserviceLocalization" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="TitleContent" runat="server">
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="CPHservice" runat="server">
    <asp:MultiView runat="server" ID="MLVgenerate">
        <asp:View ID="VIWinfo" runat="server">
            <br /><br /><br /><br />
            <asp:Label ID="LBmessage" runat="server"></asp:Label>
            <br /><br /><br /><br />
        </asp:View>
    </asp:MultiView>
</asp:Content>