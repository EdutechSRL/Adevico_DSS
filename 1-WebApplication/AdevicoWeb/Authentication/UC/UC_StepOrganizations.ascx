<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="UC_StepOrganizations.ascx.vb"
    Inherits="Comunita_OnLine.UC_AuthenticationStepOrganizations" %>

<div class="StepOrgn">
<asp:MultiView ID="MLVorganizations" runat="server" ActiveViewIndex="0">
    <asp:View ID="VIWuser" runat="server">
        <div class="StepData">
            <span class="Fieldrow">
                <asp:Label ID="LBsubscribeTo" runat="server" CssClass="Titolo_campoSmall">Subscribe to:</asp:Label>
            </span>
            <span class="Fieldrow">
                <asp:RadioButtonList ID="RBLorganizations" runat="server" CssClass="testo_campoSmall"
                    RepeatLayout="Flow" RepeatDirection="Vertical" />
            </span>
        </div>
    </asp:View>
    <asp:View ID="VIWadministrator" runat="server">
        <div class="clearfix">
            <div class="FlotLeft, LeftCol">
                <asp:Label ID="LBsubscribeToDefault" runat="server" CssClass="Titolo_campoSmall">Default organization:</asp:Label>
            </div>
            <div class="FlotLeft">
                <asp:DropDownList ID="DDLorganizations" runat="server" CssClass="testo_campoSmall"
                    AutoPostBack="true">
                </asp:DropDownList>
            </div>
        </div>
        <div class="clearfix">
            <div class="FlotLeft, LeftCol">
                <asp:Label ID="LBsubscribeToAlso" runat="server" CssClass="Titolo_campoSmall">Subscribe also to:</asp:Label>
            </div>
            <div class="FlotLeft">
                <asp:CheckBoxList ID="CBLorganzations" runat="server" CssClass="testo_campoSmall"
                    RepeatLayout="Flow" RepeatDirection="Vertical">
                </asp:CheckBoxList>
            </div>
        </div>
    </asp:View>
</asp:MultiView>
</div>