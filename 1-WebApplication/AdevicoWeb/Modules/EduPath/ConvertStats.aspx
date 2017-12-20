<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/AjaxPortal.Master" CodeBehind="ConvertStats.aspx.vb" Inherits="Comunita_OnLine.ConvertStats" %>
<asp:Content ID="Content1" ContentPlaceHolderID="PageTitleContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="TitleContent" runat="server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="CPHservice" runat="server">

<div>
  
 PathID: <asp:TextBox ID="TXTpathId" runat="server"></asp:TextBox>
</div>

<div>
 UserId: <asp:TextBox ID="TXTuserId" runat="server"></asp:TextBox>
</div>
<div>
    <asp:LinkButton ID="LKBsubmit" runat="server">Submit</asp:LinkButton>
</div>
</asp:Content>
