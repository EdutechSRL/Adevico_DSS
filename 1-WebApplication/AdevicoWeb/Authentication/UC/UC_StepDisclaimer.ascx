<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="UC_StepDisclaimer.ascx.vb"
    Inherits="Comunita_OnLine.UC_AuthenticationStepDisclaimer" %>

<div class="StepData">
    <span class="Fieldrow">
        <asp:Label ID="LBlanguage_t" runat="server" CssClass="Titolo_campoSmall">Language:</asp:Label>
    </span><span class="Fieldrow">
        <asp:RadioButtonList ID="RBLlanguages" runat="server" CssClass="testo_campoSmall"
            RepeatLayout="Flow" RepeatDirection="Vertical" AutoPostBack="true">
        </asp:RadioButtonList>
    </span>
</div>