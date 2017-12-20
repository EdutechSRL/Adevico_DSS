<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="UC_Measure.ascx.vb" Inherits="Comunita_OnLine.UC_Measure" %>

<asp:Panel ID="PNLMeasure" runat="server">
    <span class="uc_measure">
        <asp:Label ID="LBLfield_t" runat="server" CssClass="Titolo_campo">Left</asp:Label>
        <asp:TextBox ID="TXBvalue" Columns="5" runat="server" CssClass="Testo_campo"></asp:TextBox>
        <asp:DropDownList ID="DDLunit" runat="server" CssClass="Testo_campo">
        </asp:DropDownList>
    </span>
</asp:Panel>