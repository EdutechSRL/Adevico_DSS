<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="UC_ProfileMailEditor.ascx.vb" Inherits="Comunita_OnLine.UC_ProfileMailEditor" %>

<asp:MultiView ID="MLVmail" runat="server">
    <asp:View ID="VIWerrors" runat="server">
        <span class="Fieldrow">&nbsp;</span>
        <span class="Fieldrow">
            <asp:Label ID="LBgenericMessage" runat="server"></asp:Label>
        </span>
        <span class="Fieldrow">&nbsp;</span>
        <span>
            <asp:Button ID="BTNcloseMailWindowFromErrors" runat="server" Text="Chiudi" CssClass="Testo_Campo" CausesValidation="false"></asp:Button>
            <asp:Button ID="BTNbackToMailEditorStep" runat="server" Text="Indietro" CssClass="Testo_Campo" CausesValidation="false"></asp:Button>
        </span>
    </asp:View>
    <asp:View ID="VIWeditor" runat="server">
        <span class="Fieldrow">
            <asp:Label ID="LBmailEditorInfo_t" runat="server" CssClass="Titolo_campo" AssociatedControlID="LBmailEditorInfo">*Attenzione</asp:Label>
        </span>
        <span class="Fieldrow">
            <asp:Label ID="LBmailEditorInfo" runat="server" CssClass="Testo_Campo">
                Al nuovo indirizzo e-mail inserito verrà spedito un messaggio contenente un link di conferma.
                Solo dopo conferma avvenuta l'indirizzo sarà cambiato.
            </asp:Label>
        </span>
        <span class="Fieldrow">&nbsp;</span>
        <span class="Fieldrow">
            <asp:Label ID="LBmail_t" runat="server" CssClass="Titolo_campo" AssociatedControlID="TXBmail">Mail address:</asp:Label>
            <asp:TextBox ID="TXBmail" Runat="server" CssClass="Testo_Campo"  MaxLength="255" Columns="60"></asp:TextBox>
	        <%--<asp:regularexpressionvalidator id="REVmail" runat="server" CssClass="errore" ControlToValidate="TXBmail"
		        Display="dynamic" ValidationExpression="^([0-9a-zA-Z]([-.\w]*[0-9a-zA-Z])*@([0-9a-zA-Z][-\w]*[0-9a-zA-Z]\.)+[a-zA-Z]{2,9})$">*</asp:regularexpressionvalidator>--%>
             <asp:regularexpressionvalidator id="REVmail" runat="server" CssClass="errore" ControlToValidate="TXBmail"
		        Display="dynamic" ValidationExpression="[a-zA-Z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-zA-Z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-zA-Z0-9](?:[a-zA-Z0-9-]*[a-z0-9])?\.)+[a-zA-Z0-9](?:[a-zA-Z0-9-]*[a-zA-Z0-9])?">*</asp:regularexpressionvalidator>
        </span>
        <span class="Fieldrow">
            <asp:Label ID="LBmailConfirm_t" runat="server" CssClass="Titolo_campo" AssociatedControlID="TXBconfirmMail">Confirm mail address:</asp:Label>
            <asp:TextBox ID="TXBconfirmMail" Runat="server" CssClass="Testo_Campo"  MaxLength="255" Columns="60"></asp:TextBox>
	        <%--<asp:regularexpressionvalidator id="REVmailConfirm" runat="server" CssClass="errore" ControlToValidate="TXBconfirmMail"
		        Display="dynamic" ValidationExpression="^([0-9a-zA-Z]([-.\w]*[0-9a-zA-Z])*@([0-9a-zA-Z][-\w]*[0-9a-zA-Z]\.)+[a-zA-Z]{2,9})$">*</asp:regularexpressionvalidator>--%>
            <asp:regularexpressionvalidator id="REVmailConfirm" runat="server" CssClass="errore" ControlToValidate="TXBconfirmMail"
		        Display="dynamic" ValidationExpression="[a-zA-Z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-zA-Z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-zA-Z0-9](?:[a-zA-Z0-9-]*[a-z0-9])?\.)+[a-zA-Z0-9](?:[a-zA-Z0-9-]*[a-zA-Z0-9])?">*</asp:regularexpressionvalidator>


            <asp:CompareValidator id="CVmail" Runat="server" ControlToValidate="TXBconfirmMail" ControlToCompare ="TXBmail"
					            Display="dynamic" Type="String" CssClass="errore">I due indirizzi non corrispondono!</asp:CompareValidator>
        </span>
        <span class="Fieldrow">&nbsp;</span>
        <span class="Fieldrow">
            <asp:Button ID="BTNcloseMailWindowFromEditor" runat="server" Text="Chiudi" CssClass="Testo_Campo" CausesValidation="false"></asp:Button>
            <asp:Button ID="BTNsaveMail" runat="server" Text="Save" CssClass="Testo_Campo" CausesValidation="true"></asp:Button>
        </span>
    </asp:View>
    <asp:View ID="VIWcode" runat="server">
        <span class="Fieldrow">
            <asp:Label ID="LBmailCodeInfo_t" runat="server" CssClass="Titolo_campo" AssociatedControlID="LBmailCodeInfo">ATTENTION</asp:Label>
            <asp:Label ID="LBmailCodeInfo" runat="server" CssClass="Testo_Campo">
                Al nuovo indirizzo e-mail inserito verrà spedito un messaggio contenente un link di conferma.
                Solo dopo conferma avvenuta l'indirizzo sarà cambiato.
            </asp:Label>
        </span>
        <span class="Fieldrow">&nbsp;</span>
        <span class="Fieldrow">
            <asp:Label ID="LBmailCode_t" runat="server" CssClass="Titolo_campo" AssociatedControlID="LBmailCodeInfo">Code:</asp:Label>
            <asp:TextBox ID="TXBmailCode" Runat="server" CssClass="Testo_Campo"  MaxLength="10" Columns="15"></asp:TextBox>
        </span>
        <span class="Fieldrow">&nbsp;</span>
        <span class="Fieldrow">
            <asp:Button ID="BTNcloseMailWindowFromCode" runat="server" Text="Chiudi" CssClass="Testo_Campo" CausesValidation="false"></asp:Button>
            <asp:Button ID="BTNnewMail" runat="server" Text="Change mail" CssClass="Testo_Campo" CausesValidation="false"></asp:Button>
            <asp:Button ID="BTNsendCode" runat="server" Text="New code" CssClass="Testo_Campo" CausesValidation="false"></asp:Button>
            &nbsp;
            <asp:Button ID="BTNinsertCode" runat="server" Text="Activate" CssClass="Testo_Campo" CausesValidation="true"></asp:Button>
        </span>
    </asp:View>
</asp:MultiView>