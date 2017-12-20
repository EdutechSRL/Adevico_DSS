<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="UC_AuthenticationProviderLoginFormat.ascx.vb" Inherits="Comunita_OnLine.UC_AuthenticationProviderLoginFormat" %>

<span class="Field_Row">
    <asp:Label ID="LBloginFormatName_t" runat="server" CssClass="Titolo_campo" AssociatedControlID="TXBname">Name:</asp:Label>
    <asp:TextBox ID="TXBname" runat="server" Columns="50" CssClass="Testo_Campo" MaxLength="200"></asp:TextBox>
    <asp:RequiredFieldValidator ID="RFVloginFormatName" runat="server" CssClass="Validatori" ControlToValidate="TXBname"
    Display="Dynamic">*</asp:RequiredFieldValidator>          
</span>
<span class="Field_Row">
    <asp:Label ID="LBloginFormatDefault_t" runat="server" CssClass="Titolo_campo" AssociatedControlID="CBXloginFormatDefault">Default format:</asp:Label>
    <asp:CheckBox ID="CBXloginFormatDefault" runat="server" CssClass="Testo_Campo" />
</span>
<span class="Field_Row">
    <asp:Label ID="LBloginFormatTextBefore_t" runat="server" CssClass="Titolo_campo" AssociatedControlID="TXBtextBefore">Text before login:</asp:Label>
    <asp:TextBox ID="TXBtextBefore" runat="server" Columns="50" CssClass="Testo_Campo" MaxLength="300"></asp:TextBox>
</span>
<span class="Field_Row">
    <asp:Label ID="LBloginFormatTextAfter_t" runat="server" CssClass="Titolo_campo" AssociatedControlID="TXBtextAfter">Text after login:</asp:Label>
    <asp:TextBox ID="TXBtextAfter" runat="server" Columns="50" CssClass="Testo_Campo" MaxLength="300"></asp:TextBox>
</span> 