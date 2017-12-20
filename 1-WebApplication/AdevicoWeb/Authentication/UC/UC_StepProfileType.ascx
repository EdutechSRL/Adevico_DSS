<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="UC_StepProfileType.ascx.vb"
    Inherits="Comunita_OnLine.UC_AuthenticationStepProfileType" %>
<div class="StepData">
    <span class="Fieldrow">
    <asp:Label ID="LBprofileToSelect" runat="server" CssClass="Titolo_Campo"></asp:Label>
    </span>
    <br />
    <span class="Fieldrow">
    <asp:RadioButtonList ID="RBLuserTypes" runat="server" CssClass="Testo_Campo"
        RepeatLayout="Flow" RepeatDirection="Vertical">
    </asp:RadioButtonList>
    </span>
</div>