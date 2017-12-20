<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="UC_InternalLogin.ascx.vb"
    Inherits="Comunita_OnLine.UC_InternalLogin" %>
<div id="header">
    <div class="container">
        <div id="pseudoForm">
            <div class="left">
                <asp:Panel ID="PNLLoginInput" runat="server">
                    <table cellpadding="0" cellspacing="0">
                        <tr>
                            <td>
                                <asp:Label ID="LBlogin_t" runat="server">Login:</asp:Label>
                            </td>
                            <td class="inp">
                                <%--<input id="lgn" />--%>
                                <asp:TextBox ID="TXBuserLogin" runat="server" MaxLength="50" CssClass="Testo_campo_obbligatorio"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <%--<label for="pwd">Password</label>--%>
                                <asp:Label ID="LBpassword_t" runat="server">Password:</asp:Label>
                            </td>
                            <td class="inp">
                                <%--<input id="pwd" />--%>
                                <asp:TextBox ID="TXBuserPassword" runat="server" MaxLength="50" TextMode="Password"></asp:TextBox>
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
                <asp:MultiView ID="MLVAvvisi" runat="server">
                    <asp:View ID="V_AvvisoMail" runat="server">
                        <asp:Label ID="LBInvioMail" runat="server">Mail inviata correttamente.</asp:Label><br />
                        <br />
                        &nbsp;</asp:View>
                    <asp:View ID="V_PWDDimenticata" runat="server">
                        <asp:Label ID="LBmail_t" runat="server">Inserisci la mail con la quale sei iscritto al sistema</asp:Label>
                        <br />
                        <br />
                        <asp:TextBox ID="TXBEmail" runat="server" Columns="60" CssClass="Testo_campo_obbligatorioSmall"
                            MaxLength="255"></asp:TextBox>&nbsp;<br />
                        <br />
                    </asp:View>
                    <asp:View ID="V_NoUser" runat="server">
                        <asp:Label ID="LBuserNotFound" runat="server">Utente non trovato</asp:Label><br />
                        <asp:Label ID="LBadminContacts" runat="server">Per problemi contatta l'Amministratore</asp:Label><br />
                        <br />
                        &nbsp;</asp:View>
                    <asp:View ID="V_Errore" runat="server">
                        <asp:Label ID="LBerrore" runat="server"></asp:Label>
                    </asp:View>
                    <asp:View ID="V_UtenteDisable" runat="server">
                        <asp:Label ID="LBUserDisable" runat="server"></asp:Label>
                    </asp:View>
                </asp:MultiView>
            </div>
            <div class="right">
                <p class="submit">
                    <asp:ImageButton ID="IMBlogin" runat="server" CausesValidation="true" ImageUrl="~/Style/loginComol/bg-submit.png"
                        AlternateText="Login" />
                    <asp:Button ID="BTNGoLogin" runat="server" CausesValidation="False" Text="Ok" Visible="False">
                    </asp:Button>
                    <asp:Button ID="BTNPwdDimenticata" Text="Invia" runat="server" CausesValidation="false"
                        Visible="False"></asp:Button>
                    <asp:Button ID="BTNRiprova" Text="Ok" runat="server" Visible="False"></asp:Button>
                </p>
            </div>
            <asp:Label ID="LBpasswdDimenticata" runat="server">Per recuperare la password perduta </asp:Label>
            <asp:LinkButton ID="LNBPwdDimenticata" runat="server" CausesValidation="false" Text="Clicca QUI"></asp:LinkButton>
        </div>
        <!-- end pseudoFForm -->
        <asp:RequiredFieldValidator ID="VLDLogin" runat="server" ControlToValidate="TXBuserLogin"
            ErrorMessage="Login" Display="none"></asp:RequiredFieldValidator>
        <asp:RequiredFieldValidator ID="VLDPassword" runat="server" ControlToValidate="TXBuserPassword"
            ErrorMessage="Password" Display="none"></asp:RequiredFieldValidator>
        <asp:ValidationSummary ID="VLDSum" runat="server" DisplayMode="BulletList" ShowMessageBox="true"
            ShowSummary="false" HeaderText="Non hai inserito i seguenti valori:"></asp:ValidationSummary>
    </div>
</div>
