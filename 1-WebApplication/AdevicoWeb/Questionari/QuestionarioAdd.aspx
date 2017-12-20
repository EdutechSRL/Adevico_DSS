<%@ Page Language="vb" AutoEventWireup="false" MasterPageFile="~/AjaxPortal.Master"
    CodeBehind="QuestionarioAdd.aspx.vb" Inherits="Comunita_OnLine.QuestionarioAdd" %>

<%@ MasterType VirtualPath="~/AjaxPortal.Master" %>
<%@ Register Src="~/Modules/Common/Editor/UC_Editor.ascx" TagName="CTRLeditor" TagPrefix="CTRL" %>
<%@ Register Assembly="RadEditor.Net2" Namespace="Telerik.WebControls" TagPrefix="radE" %>
<%@ Register Assembly="RadCalendar.Net2" Namespace="Telerik.WebControls" TagPrefix="radCln" %>
<asp:Content ID="Content2" ContentPlaceHolderID="HeadContent" runat="Server">
   <link media="screen" href="stile.css?v=201604071200lm" type="text/css" rel="StyleSheet" />
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="CPHservice" runat="server">
    <script type="text/javascript">
        function JSvisualizzaRisposta(oCheck, controllo1, controllo2, controllo3) {
            var check;
            var unCheck;
            var unCheck3;
            check = document.getElementById(controllo1);
            unCheck = document.getElementById(controllo2);
            if (!controllo3) {
                unCheck3 = unCheck
            }
            else {
                unCheck3 = document.getElementById(controllo3);
            }
            if (!oCheck.checked) {
                check.checked = false;
                unCheck.checked = false;
                unCheck3.checked = false;
            }
        }
    </script>
    <script type="text/javascript">
        function JSvisualizzaCorrezione(oCheck, controlToCheck, controlToUnCheck, controlToUnCheckWUC) {
            var check;
            var unCheck;
            var unCheckWUC;
            check = document.getElementById(controlToCheck);
            unCheck = document.getElementById(controlToUnCheck);
            unCheckWUC = document.getElementById(controlToUnCheckWUC);
            if (oCheck.checked) {
                check.checked = true;
                unCheck.checked = false;
            }
            else {
                unCheckWUC.checked = false;
            }
        }
        
    </script>
    <script type="text/javascript">
        function JSvisualizzaSuggerimenti(oCheck, controlToCheck, controlToCheck2, controlToUnCheck) {
            var check;
            var check2;
            var unCheck;
            check = document.getElementById(controlToCheck);
            check2 = document.getElementById(controlToCheck2);
            unCheck = document.getElementById(controlToUnCheck);
            if (oCheck.checked) {
                check.checked = true;
                check2.checked = true;
                unCheck.checked = false;
            }
        }
    </script>
    <script type="text/javascript">
        function JSeditaRisposta(oCheck, controlToUnCheck, controlToUnCheck2, controlToCheck) {
            var check;
            var unCheck2;
            var unCheck;
            unCheck = document.getElementById(controlToUnCheck);
            unCheck2 = document.getElementById(controlToUnCheck2);
            check = document.getElementById(controlToCheck);
            if (oCheck.checked) {
                check.checked = true;
                unCheck.checked = false;
                unCheck2.checked = false;
            }
        }
    </script>
    <asp:MultiView runat="server" ID="MLVquestionari">
        <asp:View runat="server" ID="VIWSceltaQuestionario">
            <asp:Label runat="server" ID="LBSceltaQuestionario" Font-Bold="true" ForeColor="#00008B"></asp:Label><br />
            <hr style="color: #00008B;" />
            <table>
                <tr>
                    <td style="width: 20%">
                        <asp:Label runat="server" ID="LBQuestionario" Font-Bold="true"></asp:Label><br />
                        <asp:LinkButton runat="server" ID="LNBQuestionarioWIZ" CssClass="Link_Menu"></asp:LinkButton>
                        <asp:LinkButton runat="server" ID="LNBQuestionarioStandard" CssClass="Link_Menu"></asp:LinkButton>
                    </td>
                    <td class="bordato">
                        <asp:Label ID="LBQuestionarioDescrizione" runat="server" Font-Bold="true"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td style="width: 20%">
                        <asp:Label runat="server" ID="LBRandom" Font-Bold="true"></asp:Label><br />
                        <asp:LinkButton runat="server" ID="LNBRandomWIZ" CssClass="Link_Menu"></asp:LinkButton>
                        <asp:LinkButton runat="server" ID="LNBRandomStandard" CssClass="Link_Menu"></asp:LinkButton>
                    </td>
                    <td class="bordato">
                        <asp:Label ID="LBRandomDescrizione" runat="server" Font-Bold="true"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label runat="server" ID="LBAutovalutazione" Font-Bold="true"></asp:Label><br />
                        <asp:LinkButton runat="server" ID="LNBAutovalutazioneWIZ" CssClass="Link_Menu"></asp:LinkButton>
                        <asp:LinkButton runat="server" ID="LNBAutovalutazioneStandard" CssClass="Link_Menu"></asp:LinkButton>
                    </td>
                    <td class="bordato">
                        <asp:Label ID="LBAutovalutazioneDescrizione" runat="server" Font-Bold="true"></asp:Label>
                    </td>
                </tr>
                <tr id="TRpollsEmpty" runat="server"><td colspan="2">&nbsp;</td></tr>
                <tr id="TRpolls" runat="server">
                    <td style="width: 20%">
                        <asp:Label runat="server" ID="LBSondaggio" Font-Bold="true"></asp:Label><br />
                        <asp:LinkButton runat="server" ID="LNBSondaggioWIZ" CssClass="Link_Menu"></asp:LinkButton>
                        <asp:LinkButton runat="server" ID="LNBSondaggioStandard" CssClass="Link_Menu"></asp:LinkButton>
                    </td>
                    <td class="bordato">
                        <asp:Label ID="LBSondaggioDescrizione" runat="server" Font-Bold="true"></asp:Label>
                    </td>
                </tr>
                 <tr id="TRlibraryEmpty" runat="server"><td colspan="2">&nbsp;</td></tr>
                 <tr id="TRlibrary" runat="server">
                    <td style="width: 20%">
                        <asp:Label runat="server" ID="LBLibreria" Font-Bold="true"></asp:Label><br />
                        <asp:LinkButton ID="LNBlibreriaStandard" runat="server" CssClass="Link_Menu"></asp:LinkButton>
                    </td>
                    <td class="bordato">
                        <asp:Label ID="LBLibreriaDescrizione" runat="server" Font-Bold="true"></asp:Label>
                    </td>
                </tr>
                 <tr id="TRmodels" runat="server">
                    <td style="width: 20%">
                        <asp:Label runat="server" ID="LBModello" Font-Bold="true"></asp:Label><br />
                        <asp:LinkButton runat="server" ID="LNBmodelloStandard" CssClass="Link_Menu"></asp:LinkButton>
                    </td>
                    <td class="bordato">
                        <asp:Label ID="LBModelloDescrizione" runat="server" Font-Bold="true"></asp:Label>
                    </td>
                </tr>
            </table>
        </asp:View>
        <asp:View runat="server" ID="VIWWizardQuestionario">
            <asp:Wizard ID="WIZStatico" runat="server" ActiveStepIndex="0" Width="100%" BackColor="#EFF3FB"
                BorderColor="#B5C7DE" BorderWidth="1px" DisplaySideBar="false">
                <WizardSteps>
                    <asp:WizardStep ID="WizardStep1" runat="server" Title="Step 1">
                        <h4><asp:Label runat="server" ID="LBtitoloS1" CssClass="titolo"></asp:Label></h4>
                        <br />
                        <asp:Label runat="server" ID="LBdescrizioneS1"></asp:Label>
                        <br />
                        <br />
                        <asp:Label runat="server" ID="LBNome_WQ"></asp:Label><br />
                        <asp:TextBox ID="TXBNome_WQ" runat="server" Width="80%"></asp:TextBox><br />
                        <asp:CustomValidator ID="CUVNome_WQ" runat="server" OnServerValidate="CUVNome_ServerValidate"  CssClass="errore" ControlToValidate="TXBNome_WQ"></asp:CustomValidator>
                        <asp:RequiredFieldValidator runat="server" ID="RFVNome_WQ" ControlToValidate="TXBNome_WQ" CssClass="errore"></asp:RequiredFieldValidator>
                        <br />
                        <br />
                        <asp:Label ID="LBDescrizioneQuestionario_WQ" runat="server"></asp:Label>
                        <br />
                         <CTRL:CTRLeditor id="CTRLeditorDescrizioneQuestionario" runat="server" ContainerCssClass="containerclass" 
                            LoaderCssClass="loadercssclass" EditorHeight="300px" FontSizes="2" AllAvailableFontnames="true" AutoInitialize="true" ModuleCode="SRVQUST">
                            </CTRL:CTRLeditor>
                        <br />
                        <asp:Label ID="LBLinguaDefault_WQ" runat="server"></asp:Label>
                        <asp:DropDownList ID="DDLLingua_WQ" runat="server" DataTextField="nome" DataValueField="id">
                        </asp:DropDownList>
                        <br />
                        <asp:Label ID="LBDataInizioTitolo_WQ" runat="server"></asp:Label>
                        <radCln:RadDatePicker ID="RDPDataInizio_WQ" runat="server" Culture="Italian (Italy)">
                            <DateInput CatalogIconImageUrl="" Description="" DisplayPromptChar="_" PromptChar=" "
                                Title="" TitleIconImageUrl="" TitleUrl=""></DateInput>
                        </radCln:RadDatePicker>
                        <asp:DropDownList ID="DDLOraInizio_WQ" runat="server">
                        </asp:DropDownList>
                        <asp:DropDownList ID="DDLMinutiInizio_WQ" runat="server">
                        </asp:DropDownList>
                        <asp:Label ID="LBDataFineTitolo_WQ" runat="server"></asp:Label>
                        <radCln:RadDatePicker ID="RDPDataFine_WQ" runat="server" Culture="Italian (Italy)">
                            <DateInput CatalogIconImageUrl="" Description="" DisplayPromptChar="_" PromptChar=" "
                                Title="" TitleIconImageUrl="" TitleUrl=""></DateInput>
                        </radCln:RadDatePicker>
                        <asp:DropDownList ID="DDLOraFine_WQ" runat="server">
                        </asp:DropDownList>
                        <asp:DropDownList ID="DDLMinutiFine_WQ" runat="server">
                        </asp:DropDownList>
                        <br />
                        <asp:CustomValidator ID="CUVdate_WQ" runat="server" OnServerValidate="CUVdate_ServerValidate"
                            ControlToValidate="RDPDataFine_WQ"></asp:CustomValidator><br />
                    </asp:WizardStep>
                    <asp:WizardStep ID="WizardStep2" runat="server" Title="Step 2">
                        <h4><asp:Label runat="server" ID="LBtitoloS2" CssClass="titolo"></asp:Label></h4>
                        <br />
                        <asp:Label runat="server" ID="LBdescrizioneS2"></asp:Label>
                        <br />
                        <br />
                        <asp:Label ID="LBDurata_WQ" runat="server"></asp:Label>
                        <asp:TextBox ID="TBDurata_WQ" runat="server"></asp:TextBox>
                        <asp:Label ID="LBDurataDopo_WQ" runat="server"></asp:Label>
                        <asp:CompareValidator runat="server" ID="COVDurataInt_WQ" Operator="DataTypeCheck"
                            Type="Integer" Display="Dynamic" ControlToValidate="TBDurata_WQ" />
                        <br />
                        <br />
                        <asp:Label ID="LBScalaValutazione_WQ" runat="server"></asp:Label>
                        <asp:TextBox ID="TXBScalaValutazione_WQ" runat="server" Text="30"></asp:TextBox>
                        <asp:CompareValidator runat="server" ID="COVScalaValutazioneInt_WQ" Operator="DataTypeCheck"
                            Type="Integer" Display="Dynamic" ControlToValidate="TXBScalaValutazione_WQ" />
                        <asp:Label ID="LBOrdineDomandeRandom_WQ" runat="server"></asp:Label>
                        <asp:CheckBox runat="server" ID="CHKOrdineDomandeRandom_WQ" />
                        <br />
                        <br />
                        <asp:Label ID="LBanonymousResults_WQ" runat="server">BARRARE la casella se i risultati del questionario devono rimanere anonimi:</asp:Label>
                        <asp:CheckBox runat="server" ID="CBXanonymousResults_WQ" Checked='<%#Eval("risultatiAnonimi")%>'  />
                        <br />
                        <br />
                        <asp:Label runat="server" ID="LBvisualizzazione_WQ"></asp:Label>
                        <asp:CheckBox runat="server" ID="CHKvisualizzaRisposta_WQ" />
                        <asp:Label ID="LBvisualizzaRisposta_WQ" runat="server"></asp:Label>
                        <asp:CheckBox ID="CHKvisualizzaCorrezione_WQ" runat="server" />
                        <asp:Label ID="LBvisualizzaCorrezione_WQ" runat="server"></asp:Label>
                        <asp:CheckBox ID="CHKvisualizzaSuggerimenti_WQ" runat="server" />
                        <asp:Label ID="LBvisualizzaSuggerimenti_WQ" runat="server"></asp:Label>
                        <asp:CheckBox runat="server" ID="CHKeditaRisposta_WQ" />
                        <asp:Label ID="LBeditaRisposta_WQ" runat="server"></asp:Label>
                    </asp:WizardStep>
                    <asp:WizardStep ID="WizardStep3" runat="server" Title="Step 3">
                        <h4><asp:Label runat="server" ID="LBtitoloS3" CssClass="titolo"></asp:Label></h4>
                        <br />
                        <asp:Label runat="server" ID="LBdescrizioneS3"></asp:Label>
                        <br />
                        <br />
                        <asp:Label runat="server" ID="LBtitoloDestinatari_WQ"></asp:Label>
                        <asp:CheckBox runat="server" ID="CHKUtentiComunita_WQ" Checked="True" />
                        <br />
                        <asp:CheckBox runat="server" ID="CHKUtentiNonComunita_WQ" />
                        <br />
                        <asp:CheckBox ID="CHKUtentiInvitati_WQ" runat="server" />
                        <br />
                        <asp:CheckBox runat="server" ID="CHKUtentiEsterni_WQ" />
                        <br />
                        <br />
                        <br />
                        <asp:Label runat="server" ID="LBavvisoBloccato_WQ"></asp:Label>
                    </asp:WizardStep>
                </WizardSteps>
                <StepStyle Font-Size="0.8em" ForeColor="#333333" />
                <SideBarButtonStyle BackColor="#507CD1" ForeColor="White" />
                <NavigationButtonStyle BackColor="White" BorderColor="#507CD1" BorderStyle="Solid"
                    BorderWidth="1px"  Font-Size="0.8em" ForeColor="#284E98" />
                <SideBarStyle BackColor="#507CD1" Font-Size="0.9em" VerticalAlign="Top" />
                <HeaderStyle BackColor="#284E98" BorderColor="#EFF3FB" BorderStyle="Solid" BorderWidth="2px"
                    Font-Bold="True" Font-Size="0.9em" ForeColor="White" HorizontalAlign="Center" />
            </asp:Wizard>
        </asp:View>
        <asp:View runat="server" ID="VIWMessaggi">
            <br />
            <asp:Label runat="server" ID="LBErrore"></asp:Label>
        </asp:View>
    </asp:MultiView>
</asp:Content>
