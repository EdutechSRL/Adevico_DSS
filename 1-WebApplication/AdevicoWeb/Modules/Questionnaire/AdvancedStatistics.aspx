<%@ Page Title="" Language="vb" AutoEventWireup="false"  MasterPageFile="~/AjaxPortal.Master" CodeBehind="AdvancedStatistics.aspx.vb" Inherits="Comunita_OnLine.QuestionnaireAdvancedStatistics" %>
<%@ MasterType VirtualPath="~/AjaxPortal.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="PageTitleContent" runat="server">

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="HeadContent" runat="server">
    <link href="~/Questionari/Stile.css" type="text/css" rel="stylesheet" />
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="TitleContent" runat="server">

</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="CPHservice" runat="server">
    <div id="DVmenu" style="width: 900px; text-align: right; padding-top: 5px; margin: 0px auto;"
        runat="server">
        <asp:HyperLink ID="HYPbackToManagement" runat="server" CssClass="Link_Menu" Text="Back"
            Height="18px" CausesValidation="false"></asp:HyperLink>
    </div>
</asp:Content>