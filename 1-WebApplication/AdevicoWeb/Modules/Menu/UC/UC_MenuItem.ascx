<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="UC_MenuItem.ascx.vb" Inherits="Comunita_OnLine.UC_MenuItem" %>
<div class="MenuItem">
    <div style="clear:both;">
        <div style="float: left;" class="Menu_FieldTitle">
            <asp:Label ID="LBitemType_t" runat="server" CssClass="Titolo_campo" AssociatedControlID="DDLtype">Type:</asp:Label>
        </div>
        <div style="float: left;" class="Menu_FieldContent">
            <asp:dropdownlist ID="DDLtype"  CssClass="Testo_campo" runat="server" AutoPostBack="true">
      
            </asp:dropdownlist>
        </div>
    </div>
    <div style="clear:both;"  runat="server" id="DVname">
        <div style="float: left;" class="Menu_FieldTitle">
            <asp:Label ID="LBitemName_t" runat="server" CssClass="Titolo_campo"  AssociatedControlID="TXBname">*Name</asp:Label>
        </div>
        <div style="float: left;" class="Menu_FieldContent">
            <asp:TextBox ID="TXBname" runat="server" CssClass="Testo_campo" Columns="50"></asp:TextBox>
            <asp:RequiredFieldValidator ID="RFVname" runat="server" ControlToValidate="TXBname"></asp:RequiredFieldValidator>
        </div>
    </div>
    <div style="clear:both;" runat="server" id="DVposition">
        <div style="float: left;" class="Menu_FieldTitle">
            <asp:Label ID="LBtextDisposition_t" runat="server" CssClass="Titolo_campo"  AssociatedControlID="DDLtextPosition">*Name</asp:Label>
        </div>
        <div style="float: left;" class="Menu_FieldContent">
            <asp:dropdownlist ID="DDLtextPosition"  CssClass="Testo_campo" runat="server">
      
            </asp:dropdownlist>
        </div>
    </div>
    <div style="clear:both;" runat="server" id="DVisEnabled">
        <div style="float: left;" class="Menu_FieldTitle_long">
            <asp:Label ID="LBisEnabled_t" runat="server" CssClass="Titolo_campo" AssociatedControlID="CBXisEnabled">Is Enabled</asp:Label>
        </div>
        <div style="float: left;" class="Menu_FieldContent">
            <asp:CheckBox ID="CBXisEnabled" runat="server" Text="*Enabled"  CssClass="Testo_campo"/>
        </div>
    </div>
    <div style="clear:both;" runat="server" id="DVdisabledItems">
        <div style="float: left;" class="Menu_FieldTitle_long">
            <asp:Label ID="LBshowDisabledItems" runat="server" CssClass="Titolo_campo"  AssociatedControlID="RBLshowDisabledItems">*Name</asp:Label>
        </div>
        <div style="float: left;" class="Menu_FieldContent">
            <asp:RadioButtonList ID="RBLshowDisabledItems"  CssClass="Testo_campo" RepeatLayout="Flow" runat="server" RepeatDirection="Horizontal">
                <asp:ListItem Value="True"></asp:ListItem>
                <asp:ListItem Value="False" Selected="True"></asp:ListItem>
            </asp:RadioButtonList>
        </div>
    </div>
    <div style="clear:both;" runat="server" id="DVlink">
        <div style="float: left;" class="Menu_FieldTitle">
            <asp:Label ID="LBlink_t" runat="server" CssClass="Titolo_campo"  AssociatedControlID="TXBlink">*Name</asp:Label>
        </div>
        <div style="float: left;" class="Menu_FieldContent">
            <asp:TextBox ID="TXBlink" runat="server" CssClass="Testo_campo" Columns="50"></asp:TextBox>
        </div>
    </div>
    <div style="clear:both;" runat="server" id="DVcssClass">
        <div style="float: left;" class="Menu_FieldTitle">
            <asp:Label ID="LBcssClass" runat="server" CssClass="Titolo_campo"  AssociatedControlID="TXBcssClass">*Css class</asp:Label>
        </div>
        <div style="float: left;" class="Menu_FieldContent">
            <asp:TextBox ID="TXBcssClass" runat="server" CssClass="Testo_campo" Columns="10"></asp:TextBox>
        </div>
    </div>
    <div style="clear:both;" id="DVdisplayOrder" runat="server">
        <div style="float: left;" class="Menu_FieldTitle_long">
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
            <asp:MultiView id="MLVaddButton" runat="server" ActiveViewIndex="0">
                <asp:View ID="VIWnoButtons"  runat="server">
                </asp:View>
                <asp:View ID="VIWcontainer" runat="server">
                    <asp:Button ID="BTNaddSeparator" runat="server" Text="Add separator" ToolTip="Add separator" CausesValidation="false" CommandArgument="Separator" />
                    <asp:Button ID="BTNaddText" runat="server" Text="Add text" ToolTip="Add text" CausesValidation="false" CommandArgument="Text" />
                    <asp:Button ID="BTNaddLink" runat="server" Text="Add link" ToolTip="Add link" CausesValidation="false" CommandArgument="Link" />
                </asp:View>
                <asp:View ID="VIWicons" runat="server">
                    <asp:Button ID="BTNaddIconNew" runat="server" Text="Add icon New" ToolTip="Add icon New" CausesValidation="false" CommandArgument="IconNewItem" />
                    <asp:Button ID="BTNaddIconStatistic" runat="server" Text="Add icon Statistic" ToolTip="Add icon Statistic" CausesValidation="false" CommandArgument="IconStatistic" />
                    <asp:Button ID="BTNaddIconManage" runat="server" Text="Add icon Manage" ToolTip="Add icon Manage" CausesValidation="false" CommandArgument="IconManage" />   
                </asp:View>
            </asp:MultiView>
        </div>
    </div>
</div>