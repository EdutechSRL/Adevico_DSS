<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="UC_ModuleAssignment.ascx.vb" Inherits="Comunita_OnLine.UC_ItemModuleAssignment" %>
<div style="clear:both">
    <div style="clear:both">
        <div style="float: left;">
            <asp:Label ID="LBmodule_t" runat="server" CssClass="Titolo_campo">Servizio:</asp:Label>
        </div>
        <div style="float: left;">
            <asp:DropDownList ID="DDLmodules" CssClass="Testo_campo" runat="server" AutoPostBack="True">
            </asp:DropDownList>

            <br />
            <asp:CheckBoxList ID="CBLpermissions" CssClass="Testo_campo" runat="server" RepeatDirection="Horizontal" RepeatLayout="Flow" RepeatColumns="3">
            </asp:CheckBoxList>
        </div>
    </div>
</div>