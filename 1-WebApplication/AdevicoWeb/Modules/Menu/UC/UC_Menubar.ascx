<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="UC_Menubar.ascx.vb" Inherits="Comunita_OnLine.UC_Menubar" %>
<div>
    <div style="clear:both;">
        <div style="float: left;" class="Menu_FieldTitle">
            <asp:Label ID="LBmenubarName" runat="server" CssClass="Titolo_campo"  AssociatedControlID="TXBname">*Name</asp:Label>
        </div>
        <div style="float: left;" class="Menu_FieldContent">
            <asp:TextBox ID="TXBname" runat="server" CssClass="Testo_campo" Columns="50"></asp:TextBox>
            <asp:RequiredFieldValidator ID="RFVname" runat="server" ControlToValidate="TXBname"></asp:RequiredFieldValidator>
        </div>
    </div>
    <div style="clear:both;">
        <div style="float: left;" class="Menu_FieldTitle">
            <asp:Label ID="LBcssClass" runat="server" CssClass="Titolo_campo"  AssociatedControlID="TXBcssClass">*Css class</asp:Label>
        </div>
        <div style="float: left;" class="Menu_FieldContent">
            <asp:TextBox ID="TXBcssClass" runat="server" CssClass="Testo_campo" Columns="10"></asp:TextBox>
        </div>
    </div>
    <div style="clear:both;">
        <div style="float: left;" class="Menu_FieldTitle">
            <asp:Label ID="LBmenubarType" runat="server" CssClass="Titolo_campo">*Type</asp:Label>
        </div>
        <div style="float: left;" class="Menu_FieldContent">
             <asp:DropDownList ID="DDLtype" runat="server" CssClass="Testo_campo">
        </asp:DropDownList>
        </div>
    </div>
    <div style="clear:both;">
        <div style="float: left;">
            
        </div>
        <div style="float: left; text-align:right;" class="Menu_FieldContent">
             <asp:Button ID="BTNaddTopItem" runat="server" Text="Add menu" ToolTip="Add menu" CausesValidation="false"  />
             &nbsp;

             <asp:Button ID="BTNsaveBottom" runat="server" Text="Save" ToolTip="Save"  />
        </div>
    </div>
</div>