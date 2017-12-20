<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="UC_StepInternalCredentials.ascx.vb"
    Inherits="Comunita_OnLine.UC_AuthenticationStepInternalCredentials" %>
<div class="StepData">
    <br /><br /><br /><br />
    <span class="Fieldrow">
        <asp:Label ID="LBlogin_t" runat="server" CssClass="Titolo_campo" AssociatedControlID="TXBlogin">(*)Login:</asp:Label>
        <asp:TextBox ID="TXBlogin" runat="server" Columns="40" CssClass="textbox Testo_Campo"
            MaxLength="50"></asp:TextBox>
        <asp:RequiredFieldValidator ID="RFVlogin" runat="server" CssClass="Validatori" ControlToValidate="TXBlogin"
            Display="Dynamic">*</asp:RequiredFieldValidator>
    </span><span class="Fieldrow">
        <asp:Label ID="LBpwd" runat="server" CssClass="Titolo_campo" AssociatedControlID="TXBpassword">(*)Password:</asp:Label>
        <asp:TextBox ID="TXBpassword" runat="server" TextMode="Password" CssClass="textbox Testo_Campo"></asp:TextBox>
    </span>
    <div id="submit-feedback">
        <span class="invisible" runat="server" id="SPNmessages">
            <asp:Literal ID="LTloginError" runat="server"></asp:Literal>
        </span>
    </div>
    <br /><br /><br /><br />
</div>
