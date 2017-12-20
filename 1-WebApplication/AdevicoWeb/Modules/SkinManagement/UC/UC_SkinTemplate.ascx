<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="UC_SkinTemplate.ascx.vb" Inherits="Comunita_OnLine.UC_SkinTemplate" %>

<div class="dv_item_menu">
    <asp:Button ID="BTNsaveTemplate" runat="server" Text="Save" CssClass="Link_Menu" />
</div>
<br /><br />
<asp:Label ID="LBheaderTemplate_t" runat="server" CssClass="Titolo_campo">#Header template:</asp:Label>
<br /><br />
<div class="template">
    <asp:RadioButtonList ID="RBLheaderTemplate" runat="server" RepeatLayout="Flow" RepeatDirection="Horizontal" RepeatColumns="3">
    </asp:RadioButtonList>
</div>
<hr /><br />

<asp:Label ID="LBfooterTemplate_t" runat="server" CssClass="Titolo_campo">#Footer template:</asp:Label>

<br /><br />
<div class="template">
    <asp:RadioButtonList ID="RBLfooterTemplate" runat="server" RepeatLayout="Flow" RepeatDirection="Horizontal" RepeatColumns="3">
    </asp:RadioButtonList>
</div>