<%@ Page Language="vb" AutoEventWireup="false" MasterPageFile="~/AjaxPortal.Master"
    CodeBehind="MeetingWiz.aspx.vb" Inherits="Comunita_OnLine.MeetingWiz" %>

<%@ MasterType VirtualPath="~/AjaxPortal.Master" %>
<%@ Register Src="~/Modules/Common/Editor/UC_Editor.ascx" TagName="CTRLeditor" TagPrefix="CTRL" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="radCln" %>
<%@ Register Src="~/uc/UC_SearchUserByCommunities.ascx" TagName="UC_SearchUser" TagPrefix="uc1" %>
<%@ Register Src="~/Modules/Common/UC/UC_MailEditor.ascx" TagName="CTRLmailEditor" TagPrefix="CTRL" %>

<asp:Content ID="Head" ContentPlaceHolderID="HeadContent" runat="server">
    <link href="stile.css?v=201604071200lm" rel="stylesheet" type="text/css" />
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="CPHservice" runat="server">
    <script type="text/javascript">
        String.prototype.replaceAll = function (s1, s2) { return this.replace(new RegExp(s1, "g"), s2); }

       function TelerikEvents(editor, args) {
            editor.AttachEventHandler("onkeyup", function (e) {
                extractInfoTelerik();
            });
        }

        function getTextArea() {
            obj = document.getElementById('<%=me.CTRLmailEditor.EditorStandardClientId  %>');
            return obj;
        }

        function getPreviewTelerik() {
            obj = $(".preview");
            return obj;
        }

        function extractInfoTelerik() {
            try{
                var editor = $find("<%=  CTRLmailEditor.EditorHTMLClientId %>"); // 
                getPreviewTelerik().html(editor.getHtml());
                vars = arrayvars.split("|");
                for (i = 0; i < vars.length; i++) {
                    values = vars[i].split("=");
                    getPreviewTelerik().html(getPreviewTelerik().html().replaceAll(values[0], values[1]));
                }
             }
             catch (err){
             }
            return false;
        }

        function extractInfo() {
            getPreviewTelerik().html(getTextArea().value.replaceAll("\n","</br>"));
            vars = arrayvars.split("|");
            for (i = 0; i < vars.length; i++) {
                values = vars[i].split("=");
                getPreviewTelerik().html(getPreviewTelerik().html().replaceAll(values[0], values[1]));
            }
            return false;
        }

        function ChangeCheckBoxState(id, checkState) {
            var cb = document.getElementById(id);
            if (cb != null)
                cb.checked = checkState;
        }

        function ChangeAllCheckBoxStates(checkState) {
            // Toggles through all of the checkboxes defined in the CheckBoxIDs array
            // and updates their value to the checkState input parameter
            if (CheckBoxIDs != null) {
                for (var i = 0; i < CheckBoxIDs.length; i++)
                    ChangeCheckBoxState(CheckBoxIDs[i], checkState);
            }
        }

        function ChangeHeaderAsNeeded() {
            // Whenever a checkbox in the GridView is toggled, we need to
            // check the Header checkbox if ALL of the GridView checkboxes are
            // checked, and uncheck it otherwise
            if (CheckBoxIDs != null) {
                // check to see if all other checkboxes are checked
                for (var i = 1; i < CheckBoxIDs.length; i++) {
                    var cb = document.getElementById(CheckBoxIDs[i]);
                    if (!cb.checked) {
                        // Whoops, there is an unchecked checkbox, make sure
                        // that the header checkbox is unchecked
                        ChangeCheckBoxState(CheckBoxIDs[0], false);
                        return;
                    }
                }

                // If we reach here, ALL GridView checkboxes are checked
                ChangeCheckBoxState(CheckBoxIDs[0], true);
            }
        }

       /* function insertAtCursor(stringa) {
            var campo = document.getElementById('=me.TXBTestoMessaggio.clientID ');

            if (document.selection) {
                campo.focus();
                sel = document.selection.createRange();
                sel.text = stringa;
            }
            else if (campo.selectionStart || campo.selectionStart == '0') {
                var startPos = campo.selectionStart;
                var endPos = campo.selectionEnd;
                campo.value = campo.value.substring(0, startPos)
                          + stringa
                          + campo.value.substring(endPos, campo.value.length);
            } else {
                campo.value += stringa;
            }

            extractInfo();
        }*/
    </script>
    <asp:MultiView runat="server" ID="MLVquestionari">
        <asp:View runat="server" ID="VIWStep1">
            <asp:Label runat="server" ID="LBtitoloS1" CssClass="TitoloServizio"></asp:Label>
            <br />
            <asp:Label runat="server" ID="LBdescrizioneS1"></asp:Label>
            <br />
            <br />
            <asp:Label runat="server" ID="LBNome_WQ"></asp:Label><br />
            <asp:TextBox ID="TXBNome_WQ" runat="server" Width="80%"></asp:TextBox><br />
            <asp:CustomValidator ID="CUVNome_WQ" runat="server" OnServerValidate="CUVNome_ServerValidate"
                ControlToValidate="TXBNome_WQ"></asp:CustomValidator>
            <asp:RequiredFieldValidator runat="server" ID="RFVNome_WQ" ControlToValidate="TXBNome_WQ"></asp:RequiredFieldValidator>
            <br />
            <br />
            <asp:Label ID="LBDataInizioTitolo_WQ" runat="server"></asp:Label>
            <radCln:RadDatePicker ID="RDPDataInizio_WQ" Skin="Outlook" runat="server" Culture="Italian (Italy)">
                <DateInput CatalogIconImageUrl="" Description="" DisplayPromptChar="_" PromptChar=" "
                    Title="" TitleIconImageUrl="" TitleUrl="">
                </DateInput>
            </radCln:RadDatePicker>
            <asp:DropDownList ID="DDLOraInizio_WQ" runat="server">
            </asp:DropDownList>
            <asp:DropDownList ID="DDLMinutiInizio_WQ" runat="server">
            </asp:DropDownList>
            <asp:Label ID="LBDataFineTitolo_WQ" runat="server"></asp:Label>
            <radCln:RadDatePicker ID="RDPDataFine_WQ" Skin="Outlook" runat="server" Culture="Italian (Italy)">
                <DateInput CatalogIconImageUrl="" Description="" DisplayPromptChar="_" PromptChar=" "
                    Title="" TitleIconImageUrl="" TitleUrl="">
                </DateInput>
            </radCln:RadDatePicker>
            <asp:DropDownList ID="DDLOraFine_WQ" runat="server">
            </asp:DropDownList>
            <asp:DropDownList ID="DDLMinutiFine_WQ" runat="server">
            </asp:DropDownList>
            <br />
            <asp:CustomValidator ID="CUVdate_WQ" runat="server" OnServerValidate="CUVdate_ServerValidate"
                ControlToValidate="RDPDataFine_WQ"></asp:CustomValidator><br />
            <asp:Label runat="server" ID="LBtitoloS2" CssClass="TitoloServizio"></asp:Label>
            <br />
            <asp:Label runat="server" ID="LBvisualizzazione_WQ"></asp:Label>
            <asp:CheckBox runat="server" ID="CHKvisualizzaRisposta_WQ" Checked="true" />
            <asp:Label ID="LBvisualizzaRisposta_WQ" runat="server"></asp:Label>
            <asp:CheckBox runat="server" ID="CHKeditaRisposta_WQ" Checked="true" />
            <asp:Label ID="LBeditaRisposta_WQ" runat="server"></asp:Label>
            <br />
            <asp:LinkButton ID="LNBAvanti1" Visible="true" runat="server" CssClass="Link_Menu"
                Text="Avanti">
            </asp:LinkButton>
        </asp:View>
        <asp:View runat="server" ID="VIWStep2">
            <br />
            <asp:Label ID="LBTestoDomanda" runat="server" Text=""></asp:Label>
            <asp:Label runat="server" ID="LBErrorNoQuestion" CssClass="errore" Visible="false"></asp:Label>
            <br />
             <CTRL:CTRLeditor id="CTRLeditorTestoDomanda" runat="server" ContainerCssClass="containerclass" 
                LoaderCssClass="loadercssclass" EditorHeight="230px" AllAvailableFontnames="true" 
                 AutoInitialize="true"  CurrentType="telerik" Toolbar="simple" MaxHtmlLength="800000">
            </CTRL:CTRLeditor>
            <asp:Label ID="LBTitoloOpzioni" runat="server" Font-Bold="true"></asp:Label>
            <br />
            <radCln:RadCalendar runat="server" ID="RDCLCalendar" Skin="Web20" EnableMultiSelect="true"
                OnSelectionChanged="RDCLCalendar_SelectionChanged">
            </radCln:RadCalendar>
            <asp:Label runat="server" ID="LBErrorNoSelection" CssClass="errore" Visible="false"></asp:Label>
            <br />
            <asp:Label ID="LBOptionNumber" runat="server" Text=""></asp:Label>
            <asp:DropDownList ID="DDLOptionNumber" runat="server" AutoPostBack="true" OnSelectedIndexChanged="selectOption">
            </asp:DropDownList>
            <br />
            <asp:Label ID="LBNumeroOpzioni" runat="server" Text=""></asp:Label>
            <br />
            <asp:DropDownList ID="DDLNumeroOpzioni" runat="server" AutoPostBack="true" OnSelectedIndexChanged="selezionaNumeroOpzioni">
                <asp:ListItem>1</asp:ListItem>
                <asp:ListItem>2</asp:ListItem>
                <asp:ListItem>3</asp:ListItem>
                <asp:ListItem>4</asp:ListItem>
                <asp:ListItem>5</asp:ListItem>
                <asp:ListItem>6</asp:ListItem>
                <asp:ListItem>7</asp:ListItem>
                <asp:ListItem>8</asp:ListItem>
                <asp:ListItem>9</asp:ListItem>
                <asp:ListItem>10</asp:ListItem>
                <asp:ListItem>11</asp:ListItem>
                <asp:ListItem>12</asp:ListItem>
                <asp:ListItem>13</asp:ListItem>
                <asp:ListItem>14</asp:ListItem>
                <asp:ListItem>15</asp:ListItem>
                <asp:ListItem>16</asp:ListItem>
                <asp:ListItem>17</asp:ListItem>
                <asp:ListItem>18</asp:ListItem>
                <asp:ListItem>19</asp:ListItem>
                <asp:ListItem>20</asp:ListItem>
                <asp:ListItem>21</asp:ListItem>
                <asp:ListItem>22</asp:ListItem>
                <asp:ListItem>23</asp:ListItem>
                <asp:ListItem>24</asp:ListItem>
                <asp:ListItem>25</asp:ListItem>
                <asp:ListItem>26</asp:ListItem>
                <asp:ListItem>27</asp:ListItem>
                <asp:ListItem>28</asp:ListItem>
                <asp:ListItem>29</asp:ListItem>
                <asp:ListItem>30</asp:ListItem>
            </asp:DropDownList>
            <br />
            <asp:Label runat="server" ID="LBZone"></asp:Label>
            <br />
            <asp:Repeater ID="RPTZone" runat="server">
                <ItemTemplate>
                    <asp:TextBox runat="server" margin-right="5px" ID="TXBZone" Text='<%#Eval("Testo")%>'></asp:TextBox>
                </ItemTemplate>
            </asp:Repeater>
            <asp:PlaceHolder runat="server" ID="PHZone"></asp:PlaceHolder>
            <br />
            <asp:Label ID="LBTestoDopoDomanda" runat="server" Text=""></asp:Label>
            <asp:TextBox ID="TXBTestoDopoDomanda" runat="server" Width="100%" TextMode="MultiLine"
                MaxLength="250"></asp:TextBox>
            <asp:LinkButton ID="LNBAddDays" Visible="true" runat="server" CssClass="Link_Menu"
                Text="Aggiungi giorno"> </asp:LinkButton>
            <asp:LinkButton ID="LNBDeleteDays" Visible="true" runat="server" CssClass="Link_Menu"
                Text="Cancella giorno"> </asp:LinkButton>
            <asp:LinkButton ID="LNBIndietro2" Visible="true" runat="server" CssClass="Link_Menu"
                Text="Indietro">
            </asp:LinkButton>
            <asp:LinkButton ID="LNBAvanti2" Visible="true" runat="server" CssClass="Link_Menu"
                Text="Avanti">
            </asp:LinkButton>
        </asp:View>
        <asp:View runat="server" ID="VIWStep3">
            <asp:Label runat="server" ID="LBtitoloS3" CssClass="TitoloServizio"></asp:Label>
            <br />
            <div style="display: none;">
                <asp:Label runat="server" ID="LBdescrizioneS3"></asp:Label>
                <br />
                <br />
                <asp:Label runat="server" ID="LBtitoloDestinatari_WQ"></asp:Label>
                <asp:CheckBox runat="server" ID="CHKUtentiComunita_WQ" Checked="True" />
                <br />
                <asp:CheckBox runat="server" ID="CHKUtentiNonComunita_WQ" />
                <br />
                <asp:CheckBox ID="CHKUtentiInvitati_WQ" runat="server" Visible="false" />
                <br />
            </div>
            <asp:Label runat="server" ID="LBsubject" Text="*Oggetto:" CssClass="Titolo_campo"></asp:Label>
            <hr width="100%" style="color: Black; height: 2px;" />
            <asp:Label runat="server" ID="LBmessageTitle" CssClass="Titolo_campo"></asp:Label>
            <asp:Label runat="server" ID="LBmessage" Text="*CorpoMail:"></asp:Label>
            <asp:Label runat="server" ID="LBmessageTXBTitle" CssClass="Titolo_campo"></asp:Label>
            <asp:TextBox runat="server" ID="TXBmessage" Width="100%" TextMode="MultiLine" Rows="4"></asp:TextBox>
            <br />
            <hr width="100%" style="color: Black; height: 4px;" />
            <asp:Label runat="server" ID="LBmail" Text="*Sara` possibile inviare una e-mail di invito a:"></asp:Label>
            <asp:LinkButton ID="LNBaddExternalUser" Visible="true" runat="server" CssClass="Link_Menu"
                Text="Aggiungi utenti esterni"></asp:LinkButton>
            <asp:LinkButton ID="LNBaddSysUsers" Visible="true" runat="server" CssClass="Link_Menu"
                Text="Aggiungi utenti interni"></asp:LinkButton>
            <br />
            <br />
            <asp:Label runat="server" ID="LBnoRecipient" Visible="false" CssClass="avviso" Text="*Non e` stato selezionato alcun destinatario"></asp:Label>
            <asp:GridView Width="100%" ID="GRVElenco" runat="server" DataKeyNames="ID" AutoGenerateColumns="false"
                Font-Size="8" ShowFooter="false" BackColor="transparent"
                BorderColor="#8080FF" AllowPaging="True" PageSize="20" AllowSorting="True">
                <Columns>
                    <asp:TemplateField ItemStyle-Width="20" Visible="false">
                        <HeaderTemplate>
                            <asp:CheckBox runat="server" ID="CHKInvitaHeader" />
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:CheckBox runat="server" ID="CHKInvitaRow" Checked='<%#Eval("isSelezionato")%>' />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="cognome" />
                    <asp:BoundField DataField="nome" />
                    <asp:BoundField DataField="mail" />
                    <asp:TemplateField>
                        <ItemTemplate>
                            <asp:ImageButton runat="server" AlternateText="" ImageUrl="img/modifica-documento.gif"
                                ID="IMBGestione" CommandName="Modifica"></asp:ImageButton>
                            <asp:ImageButton runat="server" AlternateText="" ImageUrl="img/elimina.gif" ID="IMBElimina"
                                CommandName="Elimina"></asp:ImageButton>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
                <RowStyle CssClass="ROW_Normal_Small" Height="22px" />
                <EditRowStyle BackColor="#2461BF" />
                <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                <PagerStyle CssClass="ROW_Page_Small" HorizontalAlign="Right" Height="25px" VerticalAlign="Bottom" />
                <HeaderStyle CssClass="ROW_header_Small_Center" />
                <AlternatingRowStyle CssClass="ROW_Alternate_Small" />
            </asp:GridView>
            <br />
            <asp:LinkButton ID="LNBindietro3" Visible="true" runat="server" CssClass="Link_Menu"
                Text="Indietro">
            </asp:LinkButton>
            <asp:LinkButton ID="LNBAvanti3" Visible="true" runat="server" CssClass="Link_Menu"
                Text="*Anteprima Meeting">
            </asp:LinkButton>
            <asp:LinkButton ID="LKBsendMail" Visible="true" runat="server" CssClass="Link_Menu"
                Text="*Send Mail">
            </asp:LinkButton>
            <asp:LinkButton ID="LKBadvanced" Visible="true" runat="server" CssClass="Link_Menu"
                Text="*Advanced mail management">
            </asp:LinkButton>
        </asp:View>
        <asp:View ID="VIWimportaDaComunita" runat="server">
            <asp:Panel ID="PNLGestioneListe" runat="server">
                <uc1:UC_SearchUser ID="UCsearchUser" runat="server" />
                <br />
                <asp:LinkButton runat="server" ID="LNBconfirm" Text="Conferma selezione" CssClass="Link_Menu" />
                <asp:LinkButton ID="LNBCancelIDC" CssClass="Link_Menu" runat="server" CausesValidation="false"></asp:LinkButton>
            </asp:Panel>
        </asp:View>
        <asp:View ID="VIWaddExternalUser" runat="server">
            <asp:FormView ID="FRVUtente" runat="server" CellPadding="4" ForeColor="#333333" Width="450">
                <ItemTemplate>
                    <asp:Label ID="LBTitoloUtente" runat="server" Font-Bold="true"></asp:Label><br />
                    <br />
                    <asp:Label ID="LBCognome" runat="server"></asp:Label><br />
                    <asp:TextBox ID="TXBCognome" runat="server" Text='<%#Eval("cognome")%>' Width="300"></asp:TextBox>
                    <asp:RequiredFieldValidator runat="server" ID="RFVCognome" ControlToValidate="TXBCognome"></asp:RequiredFieldValidator>
                    <br />
                    <br />
                    <asp:Label ID="LBNome" runat="server"></asp:Label><br />
                    <asp:TextBox ID="TXBNome" runat="server" Text='<%#Eval("nome")%>' Width="300"></asp:TextBox>
                    <asp:RequiredFieldValidator runat="server" ID="RFVNome" ControlToValidate="TXBNome"></asp:RequiredFieldValidator>
                    <br />
                    <br />
                    <asp:Label ID="LBEmail" runat="server"></asp:Label><br />
                    <asp:TextBox ID="TXBEmail" runat="server" Text='<%#Eval("mail")%>' Width="300"></asp:TextBox>
                    <asp:RequiredFieldValidator runat="server" ID="RFVEmail" ControlToValidate="TXBEmail"></asp:RequiredFieldValidator>
                    <br />
                    <br />
                    <asp:Label ID="LBDescrizione" runat="server"></asp:Label><br />
                    <asp:TextBox ID="TXBDescrizione" runat="server" Width="300" Text='<%#Eval("descrizione")%>'
                        Rows="4" TextMode="MultiLine"></asp:TextBox>
                    <br />
                    <br />
                </ItemTemplate>
                <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                <EditRowStyle BackColor="#2461BF" />
                <RowStyle BackColor="#EFF3FB" />
                <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
                <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
            </asp:FormView>
            <asp:LinkButton ID="LNBSaveExternalUser" CssClass="Link_Menu" runat="server"></asp:LinkButton>
            <asp:LinkButton ID="LNBCancelAEU" CssClass="Link_Menu" runat="server" CausesValidation="false"></asp:LinkButton>
            <br />
            <br />
        </asp:View>
        <asp:View runat="server" ID="VIWRiepilogo">
            <asp:Label runat="server" ID="LBConfermaRiepilogo" CssClass="confermaVerde"></asp:Label>
            <asp:Label runat="server" ID="LBRiepilogo"></asp:Label>
            <%#Me.SmartTagsAvailable.TagAll(Eval("testo1"))%>
            <br />
            <asp:Label runat="server" ID="LBTestoMeeting"></asp:Label>
            <br />
            <asp:DataList ID="DLDomande" runat="server" DataKeyField="id" OnItemDataBound="loadDomandeOpzioni"
                Width="100%">
                <ItemTemplate>
                    <br />
                    <br />
                    <asp:PlaceHolder ID="PHOpzioni" runat="server" Visible="true"></asp:PlaceHolder>
                    <asp:LinkButton ID="LNBEdit" runat="server" CssClass="Link_Menu" Text="Modifica."
                        CommandName="Edit" Visible="false"></asp:LinkButton>
                    <asp:LinkButton ID="LNBDelete" runat="server" CssClass="Link_Menu" Text="Elimina."
                        CommandName="Delete"></asp:LinkButton>
                </ItemTemplate>
                <FooterStyle BackColor="WHITE" Font-Bold="True" ForeColor="White" />
                <SelectedItemStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                <AlternatingItemStyle BackColor="#EFF3FB" />
                <ItemStyle BackColor="WHITE" />
                <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
            </asp:DataList>
            <br />
            <asp:LinkButton ID="LNBBackPreview" runat="server" CssClass="Link_Menu" Text="indietro"></asp:LinkButton>
            <asp:LinkButton ID="LNBSaveAndManageMail" runat="server" CssClass="Link_Menu" Text="gestione inviti"></asp:LinkButton>
            <asp:LinkButton ID="LNBSaveAndUnlock" runat="server" CssClass="Link_Menu" Text="Sblocca e salva"></asp:LinkButton>
            <asp:LinkButton ID="LNBSave" runat="server" CssClass="Link_Menu" Text="Fine"></asp:LinkButton>
        </asp:View>
        <asp:View runat="server" ID="VIWMail">
            <asp:Label ID="LBErroreNoTag" runat="server" ForeColor="red" Font-Bold="true" Visible="false"></asp:Label>
            <asp:Label ID="LBErroreGenerico" runat="server" Font-Bold="true" ForeColor="red" Visible="false"></asp:Label>
            <asp:Label runat="server" ID="LBMsgQuestionarioBloccato" ForeColor="red"></asp:Label>
            <br />
            <asp:Panel runat="server" ID="PNLTemplate" BorderColor="black" Style="padding: 5px"
                BorderWidth="1">
                <asp:Label ID="LBTitoloTemplate" runat="server"></asp:Label><br />
                <br />
                <asp:DropDownList ID="DDLTemplate" runat="server" DataTextField="nome" DataValueField="id">
                </asp:DropDownList>
                <asp:Button ID="BTNLoadTemplate" runat="server" Text="OK" CssClass="Link_Menu" />
                <asp:Button ID="BTNElimina" runat="server" CssClass="Link_Menu" />
                <asp:Button ID="BTNNuovo" runat="server" CssClass="Link_Menu" />
            </asp:Panel>
            <br />
            <br />
            <asp:Label ID="LBDestinatario" runat="server"></asp:Label><br />
            <asp:TextBox ID="TXBDestinatario" Width="100%" TextMode="MultiLine" runat="server"
                Enabled="false" BackColor="LightGray" ForeColor="black"></asp:TextBox><br />
            <asp:LinkButton runat="server" ID="LKBSelezionaUtenti" Visible="false" CssClass="Link_Menu"></asp:LinkButton>
            <asp:LinkButton ID="LKBAggiungiNonCompletati" runat="server" CssClass="Link_Menu"></asp:LinkButton>
            &nbsp; &nbsp; &nbsp;&nbsp;
            <br />
            <asp:LinkButton runat="server" ID="LKBAggiungiTutti" CssClass="Link_Menu"></asp:LinkButton>
            &nbsp; &nbsp; &nbsp; &nbsp;
            <asp:LinkButton runat="server" ID="LKBAggiungiNonInvitati" CssClass="Link_Menu"></asp:LinkButton><br />
            <br /><br />

            <asp:Panel runat="server" ID="PNLMail" BorderWidth="1" Style="padding-left: 10px" BackColor="#EFF3FB">
                <br /><br />
                <CTRL:CTRLmailEditor ID="CTRLmailEditor" RaiseUpdateEvent="true" TelerikOnClientCommandExecuted="extractInfoTelerik();" HTMLMailOnKeyUpScript="extractInfoTelerik();"
                TelerikClientLoadScript="TelerikEvents(editor, args);"
                 ContainerLeft="" ContainerRight="" StandardMailOnKeyUpScript="extractInfo();"
                  AllowCopyToSender="False" AllowNotifyToSender="False" AllowValidation="True" runat="server" />
                <br />
                <asp:Literal ID="Literal1" runat="server"></asp:Literal>
                <table width="98%">
                    <tr>
                        <td>
                            <asp:Literal ID="LTRvariables" runat="server"></asp:Literal><br />
                            <asp:Label runat="server" ID="LBAnteprima" class="Titolo_Campo RowCellLeft"></asp:Label>
                            <asp:Label ID="LBpreviewDisplay" runat="server" CssClass="preview"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Button ID="BTNSalvaTemplate" runat="server" CssClass="Link_Menu" />
                            <asp:Button ID="BTNSalvaTemplateConNome" runat="server" CssClass="Link_Menu" />
                            <asp:TextBox ID="TXBNomeTemplate" runat="server" MaxLength="256"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td align="center">
                            <br />
                            <asp:CheckBox ID="CHKInoltraMittente" runat="server" />
                            <br />
                            <br />
                            <asp:Label runat="server" ID="LBMsgInvia"></asp:Label>
                            <p>
                                <asp:Button runat="server" ID="BTNInviaMail" CssClass="Link_Menu" />
                            </p>
                            <asp:Label runat="server" ID="LBMsgSbloccaInvia"></asp:Label>
                            <p>
                                <asp:Button runat="server" ID="BTNSbloccaInvia" CssClass="Link_Menu" />
                            </p>
                        </td>
                    </tr>
                </table>
            </asp:Panel>
            <br />
            <asp:LinkButton runat="server" ID="LNBCancelMail" CssClass="Link_Menu" CausesValidation="false"
                Text="#Cancel"></asp:LinkButton>
        </asp:View>
        <asp:View runat="server" ID="VIWMessaggi">
            <br />
            <asp:Label runat="server" ID="LBErrore" CssClass="errore"></asp:Label>
            <asp:Label runat="server" ID="LBConferma" CssClass="confermaVerde"></asp:Label>
            <br />
            <br />
            <asp:LinkButton runat="server" ID="LNBBackToStep2" Visible="false" CssClass="Link_Menu"></asp:LinkButton>
            <asp:LinkButton runat="server" ID="LNBBackToMail" Visible="false" CssClass="Link_Menu"></asp:LinkButton>
        </asp:View>
    </asp:MultiView>
    <asp:LinkButton runat="server" ID="LNBPreview" CssClass="Link_Menu"></asp:LinkButton>
</asp:Content>