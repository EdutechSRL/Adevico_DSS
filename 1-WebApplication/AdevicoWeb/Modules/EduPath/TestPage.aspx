<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/AjaxPortal.Master" CodeBehind="TestPage.aspx.vb" Inherits="Comunita_OnLine.TestPage" %>
<%@ Register Src="~/Modules/EduPath/UC/UC_OrderBy.ascx" TagName="OrderBy" TagPrefix="UC"%>
<asp:Content ID="Content1" ContentPlaceHolderID="PageTitleContent" runat="server">

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="CPHserviceLocalization" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="HeadContent" runat="server">
<link href="../../Graphics/Modules/Edupath/css/edupath.css" rel="Stylesheet" />
<style>
    span.btnOrderBy
    {        
        display:inline-block;
        *display:inline;
        zoom:1;
        vertical-align: middle;        
    }
    
</style>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="TitleContent" runat="server">
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="CPHservice" runat="server">


<table class="table light">
    <thead>
    <tr>
        <th>Column 1<UC:OrderBy runat="server" ID="OB1" Column="1" /></th>
        <th>Column 2<UC:OrderBy runat="server" ID="OB2" Column="2" /></th>
        <th>Column 3<UC:OrderBy runat="server" ID="OB3" Column="3" /></th>
    </tr>
    </thead>
    <tbody>
    <tr>
        <td>1</td>
        <td>2</td>
        <td>3</td>
    </tr>
    </tbody>
</table>
</asp:Content>
