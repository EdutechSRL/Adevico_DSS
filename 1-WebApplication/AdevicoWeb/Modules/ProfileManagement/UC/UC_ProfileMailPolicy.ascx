<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="UC_ProfileMailPolicy.ascx.vb" Inherits="Comunita_OnLine.UC_ProfileMailPolicy" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<div class="StepData">
<asp:MultiView ID="MLVdata" runat="server">
    <asp:View ID="VIWtree" runat="server">
        <span class="Fieldrow">
            <asp:Label ID="LBmailPolicyInfo" runat="server" CssClass="TestoCampo">
            </asp:Label>
        </span>
        <br />
        <span class="Fieldrow">
            <asp:Label ID="LBcommunityStatus_t" runat="server" CssClass="Titolo_campo" AssociatedControlID="RBLstatus">Visualizza comunità:</asp:Label>
            <asp:RadioButtonList ID="RBLstatus" runat="server" RepeatDirection="Horizontal" RepeatLayout="Flow"
                CssClass="Testo_campo" AutoPostBack="true">
            </asp:RadioButtonList>

            <asp:Label ID="LBcommunityname_t" runat="server" CssClass="Titolo_campo" AssociatedControlID="TXBcontains">Nome:</asp:Label>
            <asp:TextBox ID="TXBcontains" runat="server" CssClass="Testo_campo"></asp:TextBox>
            <asp:Button ID="BTNfilter" runat="server"  Text="Search"/>
        </span>
        <br />
        <telerik:RadTreeView ID="RDTcommunity" runat="server" Width="850px" AutoPostBack="false"
            CssFile="~/RadControls/TreeView/Skins/Materiale/StyleImport.css" OnClientNodeChecked ="AfterCheckHandler"
            CheckChildNodes="false" TriStateCheckBoxes="false" MultipleSelect="true" CheckBoxes="true" />
    </asp:View>
    <asp:View ID="VIWempty" runat="server">
    </asp:View>
</asp:MultiView>
</div>