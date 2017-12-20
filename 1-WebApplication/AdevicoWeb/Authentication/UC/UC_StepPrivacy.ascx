<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="UC_StepPrivacy.ascx.vb" Inherits="Comunita_OnLine.UC_AuthenticationStepPrivacy" %>

<%@ Register TagPrefix="CTRL" TagName="Policy" Src="~/Modules/ProfilePolicy/UC/UC_ProfilePolicy.ascx" %>

<div class="StepData">
    <CTRL:Policy id="CTRLpolicy" runat="Server"></CTRL:Policy>
</div>