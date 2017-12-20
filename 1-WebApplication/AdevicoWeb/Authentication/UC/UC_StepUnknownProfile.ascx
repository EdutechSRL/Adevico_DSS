<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="UC_StepUnknownProfile.ascx.vb" Inherits="Comunita_OnLine.UC_AuthenticationStepUnknownProfile" %>
<div class="StepData">
    <span class="Fieldrow">
        <asp:Label ID="LBlanguage_t" runat="server" CssClass="Titolo_campo full_row" AssociatedControlID="DDLlanguages">Language:</asp:Label>
        <asp:dropdownlist ID="DDLlanguages" runat="server" CssClass="Testo_campo" AutoPostBack="true"> </asp:dropdownlist>
    </span>
    <br />
    <span class="Fieldrow Testo_campo">
        <span class="half">
            <asp:RadioButton ID="RBnewProfile" runat="server" GroupName="provider" />
            <asp:Label ID="LBnewProfileRadio" runat="server" CssClass="Cbx_Title NewProfile" AssociatedControlID="RBnewProfile"></asp:Label>
            <br />
            <asp:Label ID="LBnewProfileRadioDescription" runat="server" CssClass="Cbx_Description" AssociatedControlID="RBnewProfile"></asp:Label>
        </span>
        <span class="half">
            <asp:RadioButton ID="RBinternalProfile" runat="server"  GroupName="provider" />
            <asp:Label ID="LBinternalProfileRadio" runat="server" CssClass="Cbx_Title OldProfile" AssociatedControlID="RBinternalProfile"></asp:Label>
            <br />
            <asp:Label ID="LBinternalProfileRadioDescription" runat="server" CssClass="Cbx_Description" AssociatedControlID="RBinternalProfile"></asp:Label>
        </span>
    </span>
</div>