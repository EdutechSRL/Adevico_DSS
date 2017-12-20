<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="UC_CategoryTree.ascx.vb" Inherits="Comunita_OnLine.Uc_CategoryTree" %>
<%--Nomi Standard: OK--%>
<asp:Literal ID="LTulMainCssClass" runat="server" Visible="false">categories tree</asp:Literal>
<asp:Literal ID="LTliLeaftCssClass" runat="server" Visible="false">category leaf collapsed</asp:Literal>
<ul class="categories" id="UlMain" runat="server">
<asp:Repeater ID="RPTcategories" runat="server">
    <HeaderTemplate></HeaderTemplate>
    <ItemTemplate>
        <li class="category autoOpen" id="LiCategory" runat="server">
            <span class="handle"></span>
            <asp:HiddenField ID="HIFid" runat="server" />
            <asp:Label ID="LBname" runat="server" class="name" ></asp:Label>
            <asp:PlaceHolder ID="PHchildren" runat="server"></asp:PlaceHolder>
        </li>
    </ItemTemplate>
    <FooterTemplate></FooterTemplate>
</asp:Repeater>
</ul>