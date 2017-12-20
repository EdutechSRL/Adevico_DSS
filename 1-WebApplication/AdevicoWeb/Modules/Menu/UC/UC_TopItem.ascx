<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="UC_TopItem.ascx.vb" Inherits="Comunita_OnLine.UC_TopItem" %>
<div>
    <div style="clear:both;">
        <div style="float: left;" class="Menu_FieldTitle">
            <asp:Label ID="LBitemName_t" runat="server" CssClass="Titolo_campo"  AssociatedControlID="TXBname">*Name</asp:Label>
        </div>
        <div style="float: left;" class="Menu_FieldContent">
            <asp:TextBox ID="TXBname" runat="server" CssClass="Testo_campo" Columns="50"></asp:TextBox>
            <asp:RequiredFieldValidator ID="RFVname" runat="server" ControlToValidate="TXBname"></asp:RequiredFieldValidator>
        </div>
    </div>
     <div style="clear:both;">
        <div style="float: left;" class="Menu_FieldTitle">
            <asp:Label ID="LBtextDisposition_t" runat="server" CssClass="Titolo_campo"  AssociatedControlID="DDLtextPosition">*Name</asp:Label>
        </div>
        <div style="float: left;" class="Menu_FieldContent">
            <asp:dropdownlist ID="DDLtextPosition"  CssClass="Testo_campo" runat="server">
      
            </asp:dropdownlist>
        </div>
    </div>
    <div style="clear:both;">
        <div style="float: left;" class="Menu_FieldTitle">
            <asp:Label ID="LBisEnabled_t" runat="server" CssClass="Titolo_campo" AssociatedControlID="CBXisEnabled">Is Enabled</asp:Label>
        </div>
        <div style="float: left;" class="Menu_FieldContent">
            <asp:CheckBox ID="CBXisEnabled" runat="server" Text="*Enabled"  CssClass="Testo_campo"/>
        </div>
    </div>
    <div style="clear:both;">
        <div style="float: left;" class="Menu_FieldTitle">
            <asp:Label ID="LBshowDisabledItems" runat="server" CssClass="Titolo_campo"  AssociatedControlID="RBLshowDisabledItems">*Name</asp:Label>
        </div>
        <div style="float: left;" class="Menu_FieldContent">
            <asp:RadioButtonList ID="RBLshowDisabledItems"  CssClass="Testo_campo" RepeatLayout="Flow" runat="server" RepeatDirection="Horizontal">
                <asp:ListItem Value="True"></asp:ListItem>
                <asp:ListItem Value="False" Selected="True"></asp:ListItem>
            </asp:RadioButtonList>
        </div>
    </div>
    <div style="clear:both;">
        <div style="float: left;" class="Menu_FieldTitle">
            <asp:Label ID="LBlink_t" runat="server" CssClass="Titolo_campo"  AssociatedControlID="TXBlink">*Name</asp:Label>
        </div>
        <div style="float: left;" class="Menu_FieldContent">
            <asp:TextBox ID="TXBlink" runat="server" CssClass="Testo_campo" Columns="50"></asp:TextBox>
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
            <asp:Label ID="LBdisplayOrder_t" runat="server" CssClass="Titolo_campo"  AssociatedControlID="TXBcssClass">Display order:</asp:Label>
        </div>
        <div style="float: left;" class="Menu_FieldContent">
            <asp:DropDownList ID="DDLposition" runat="server" CssClass="Testo_campo"></asp:DropDownList>
        </div>
    </div>
    <div style="clear:both;">
        <div style="float: left;">
            
        </div>
        <div style="float: left; text-align:right;">
             <asp:Button ID="BTNaddcolumn" runat="server" Text="Add separator" ToolTip="Add separator" CausesValidation="false"  CommandArgument="ItemColumn" />
        </div>
    </div>
</div>