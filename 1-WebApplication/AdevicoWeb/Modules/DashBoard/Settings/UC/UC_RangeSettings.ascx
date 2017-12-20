<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="UC_RangeSettings.ascx.vb" Inherits="Comunita_OnLine.UC_RangeSettings" %>
<asp:Label ID="LBrangeItems_t" CssClass="fieldlabel" runat="server" AssociatedControlID="TXBrangeMin"></asp:Label> 
<span class="inputgroup">
    <asp:Label ID="LBrangeItemsFrom" CssClass="" AssociatedControlID="TXBrangeMin" runat="server">*</asp:Label>
    <asp:TextBox ID="TXBrangeMin" runat="server" MaxLength="3">15</asp:TextBox>
    <asp:Label ID="LBrangeItemsTo" CssClass="" AssociatedControlID="TXBrangeMax" runat="server">*</asp:Label>
    <asp:TextBox ID="TXBrangeMax" runat="server" MaxLength="3">24</asp:TextBox>
    <asp:Label ID="LBrangeItemsDisplay" CssClass="" AssociatedControlID="TXBrangeDisplayItems" runat="server">*:</asp:Label>
    <asp:TextBox ID="TXBrangeDisplayItems" runat="server" MaxLength="3">24</asp:TextBox>
</span>     