<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="UC_Column.ascx.vb" Inherits="Comunita_OnLine.UC_ColumnItem" %>
<div>
    <div style="clear:both;">
        <div style="float: left;" class="Menu_FieldTitle">
            <asp:Label ID="LBisEnabled_t" runat="server" CssClass="Titolo_campo" AssociatedControlID="CBXisEnabled">Is Enabled</asp:Label>
        </div>
        <div style="float: left;">
            <asp:CheckBox ID="CBXisEnabled" runat="server" Text="*Enabled"/>
        </div>
    </div>
    <div style="clear:both;">
        <div style="float: left;" class="Menu_FieldTitle">
            <asp:Label ID="LBwidth_t" runat="server" CssClass="Titolo_campo" AssociatedControlID="TXBwidth">Width (px)</asp:Label>
        </div>
        <div style="float: left;">
            <asp:TextBox ID="TXBwidth" runat="server" CssClass="Testo_campo" Columns="5" MaxLength="5"></asp:TextBox>
            <asp:RangeValidator ID="RNVwidth" SetFocusOnError="true" ControlToValidate="TXBwidth" MinimumValue="0" MaximumValue="1024" Type="Integer" runat="server"></asp:RangeValidator>
        </div>
    </div>
    <div style="clear:both;">
        <div style="float: left;" class="Menu_FieldTitle">
            <asp:Label ID="LBheight_t" runat="server" CssClass="Titolo_campo" AssociatedControlID="TXBheight">*Type</asp:Label>
        </div>
        <div style="float: left;">
             <asp:TextBox ID="TXBheight" runat="server" CssClass="Testo_campo" Columns="5" MaxLength="5"></asp:TextBox>
             <asp:RangeValidator ID="RNVheight" SetFocusOnError="true" ControlToValidate="TXBheight" MinimumValue="0" MaximumValue="1024" Type="Integer" runat="server"></asp:RangeValidator>
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
            <asp:Label ID="LBhasSeparator_t" runat="server" CssClass="Titolo_campo" AssociatedControlID="CBXseparator">Has Separator:</asp:Label>
        </div>
        <div style="float: left;" class="Menu_FieldContent">
            <asp:CheckBox ID="CBXseparator" runat="server" Text="*Enabled"/>
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
        <div style="float: left; text-align:right;" class="Menu_FieldContent">
             <asp:Button ID="BTNaddSeparator" runat="server" Text="Add separator" ToolTip="Add separator" CausesValidation="false"  CommandArgument="Separator" />
             <asp:Button ID="BTNaddTextContainer" runat="server" Text="Add separator" ToolTip="Add separator" CausesValidation="false"  CommandArgument="TextContainer" />
             <asp:Button ID="BTNaddLinkContainer" runat="server" Text="Add separator" ToolTip="Add separator" CausesValidation="false"  CommandArgument="LinkContainer" />
             <asp:Button ID="BTNaddText" runat="server" Text="Add separator" ToolTip="Add separator" CausesValidation="false"  CommandArgument="Text" />
             <asp:Button ID="BTNaddLink" runat="server" Text="Add separator" ToolTip="Add separator" CausesValidation="false"  CommandArgument="Link" />
             &nbsp;
             <asp:Button ID="BTNdelete" runat="server" Text="Delete" ToolTip="Delete" CausesValidation="false" CssClass="needconfirm"/>
             <asp:Button ID="BTNvirtualDelete" runat="server" Text="Virtual delete" ToolTip="Virtual delete" CausesValidation="false"  CssClass="needconfirm"/>
             <asp:Button ID="BTNsaveBottom" runat="server" Text="Save" ToolTip="Save"  />
        </div>
    </div>
</div>