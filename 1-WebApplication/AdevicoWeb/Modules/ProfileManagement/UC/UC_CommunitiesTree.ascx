<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="UC_CommunitiesTree.ascx.vb"
    Inherits="Comunita_OnLine.UC_CommunitiesTree" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<asp:MultiView ID="MLVdata" runat="server">
    <asp:View ID="VIWtree" runat="server">
        <span class="Fieldrow">
            <asp:Label ID="LBcommunityStatus_t" runat="server" CssClass="Titolo_campo" AssociatedControlID="RBLstatus">Visualizza comunità:</asp:Label>
            <asp:RadioButtonList ID="RBLstatus" runat="server" RepeatDirection="Horizontal" RepeatLayout="Flow"
                CssClass="Testo_campo" AutoPostBack="true">
            </asp:RadioButtonList>

            <asp:Label ID="LBcommunityTypes_t" runat="server" CssClass="Titolo_campo" AssociatedControlID="DDLtypes">Tipi comunità:</asp:Label>
            <asp:DropDownList ID="DDLtypes" runat="server" AutoPostBack="true" CssClass="Testo_campo">
            </asp:DropDownList>
            <asp:Label ID="LBsubscriptions_t" runat="server" CssClass="Titolo_campo" AssociatedControlID="DDLsubscriptions"> e </asp:Label>
            <asp:DropDownList ID="DDLsubscriptions" runat="server" RepeatDirection="Horizontal" RepeatLayout="Flow"
                CssClass="Testo_campo" AutoPostBack="true">
            </asp:DropDownList>
        </span>
        <br />
        <telerik:RadTreeView ID="RDTcommunity" runat="server" Width="850px" AutoPostBack="false"
            CssFile="~/RadControls/TreeView/Skins/Materiale/StyleImport.css"
            CheckChildNodes="false" TriStateCheckBoxes="false" MultipleSelect="true" CheckBoxes="true"  OnClientNodeChecked ="AfterCheckHandler" />
    </asp:View>
    <asp:View ID="VIWempty" runat="server">
    </asp:View>
</asp:MultiView>
