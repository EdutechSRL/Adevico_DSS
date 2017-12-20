<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="UC_StepAuthentication.ascx.vb" Inherits="Comunita_OnLine.UC_AuthenticationStepAuthenticationTypes" %>

<div class="StepData">
    <span class="Fieldrow">
    <asp:Label ID="LBauthenticationProviders_t" runat="server" CssClass="Titolo_Campo full">Scegli il tipo di autenticazione</asp:Label>
    </span>
    <br />
    <span class="Fieldrow">
    <asp:RadioButtonList ID="RBLauthenticationProviders" runat="server" CssClass="Testo_Campo"
        RepeatLayout="Flow" RepeatDirection="Vertical">
    </asp:RadioButtonList>
    </span>
</div>