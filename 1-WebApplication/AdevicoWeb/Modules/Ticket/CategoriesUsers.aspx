<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/AjaxPortal.Master" CodeBehind="CategoriesUsers.aspx.vb" Inherits="Comunita_OnLine.CategoriesUsers" %>
<%--Nomi Standard: OK--%>
<%@ MasterType VirtualPath="~/AjaxPortal.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="PageTitleContent" runat="server">

</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="TitleContent" runat="server">

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="CPHserviceLocalization" runat="server">

</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="HeadContent" runat="server">
    <link rel="Stylesheet" href="../../Graphics/Modules/Ticket/Css/tickets.css<%=CssVersion()%>" />


</asp:Content>

<asp:Content ID="Content5" ContentPlaceHolderID="CPHservice" runat="server">

<div class="fieldobject toolbar clearfix">
    <div class="fieldrow left">

    </div>
    <div class="fieldrow right">
        <span class="btnswitchgroup"><!--
            --><asp:LinkButton ID="LNBtable" runat="server" CssClass="btnswitch first">*Table</asp:LinkButton><!--
            --><asp:LinkButton ID="LNBtree" runat="server" CssClass="btnswitch">*Tree</asp:LinkButton><!--
            --><asp:LinkButton ID="LNBuser" runat="server" CssClass="btnswitch last active">*Users</asp:LinkButton><!--
        --></span>
    </div>
</div>

</asp:Content>
