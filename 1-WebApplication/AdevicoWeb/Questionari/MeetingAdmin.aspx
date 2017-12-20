<%@ Page Language="vb" AutoEventWireup="false" MasterPageFile="~/AjaxPortal.Master"
    CodeBehind="MeetingAdmin.aspx.vb" Inherits="Comunita_OnLine.MeetingAdmin" ValidateRequest="false"
    Culture="IT-it" %>

<%@ MasterType VirtualPath="~/AjaxPortal.Master" %>
<%@ Register Src="~/Modules/Common/Editor/UC_Editor.ascx" TagName="CTRLeditor" TagPrefix="CTRL" %>
<%@ Register Src="UserControls/ucDomandaMeetingEdit.ascx" TagName="CTRLDomandaMeetingEdit"
    TagPrefix="CTRL" %>
<%@ Register Assembly="RadCalendar.Net2" Namespace="Telerik.WebControls" TagPrefix="radCln" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="Server">
    <link media="screen" href="stile.css?v=201604071200lm" type="text/css" rel="StyleSheet" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="CPHservice" runat="Server">
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
        <asp:View ID="VIWdati" runat="server">
            <asp:Panel ID="PNLmenu" runat="server" Width="100%" HorizontalAlign="right">
                <asp:LinkButton ID="LNBCartellaPrincipale" Visible="true" runat="server" CssClass="Link_Menu"
                    CausesValidation="false"></asp:LinkButton>&nbsp;
                <asp:LinkButton ID="LNBSalva" Visible="true" runat="server" CssClass="Link_Menu">
                </asp:LinkButton>
                &nbsp;
            </asp:Panel>
            <br />
            <br />
            <asp:Panel runat="server" ID="PNLQuestionario" BackColor="white" Width="100%">
                <asp:FormView ID="FRVQuestionario" runat="server" CellPadding="0" Width="100%" Font-Size="8"
                    BackColor="transparent" BorderColor="#8080FF" HorizontalAlign="Left">
                    <RowStyle CssClass="ROW_Normal_Small" Height="22px" />
                    <EditRowStyle BackColor="#2461BF" />
                    <PagerStyle CssClass="ROW_Page_Small" HorizontalAlign="Right" Height="25px" VerticalAlign="Bottom" />
                    <HeaderStyle CssClass="ROW_header_Small_Center" />
                    <ItemTemplate>
                        <asp:Label runat="server" ID="LBDatiQuestionario" Font-Bold="true"></asp:Label>
                        <br />
                        <table border="1" bordercolor="black" width="100%" cellpadding="5" cellspacing="0">
                            <tr>
                                <td valign="middle" bordercolor="white">
                                    <asp:Label ID="LBNome" runat="server"></asp:Label>
                                    <asp:TextBox ID="TXBnome" runat="server" Text='<%#Eval("nome")%>' Width="800"> </asp:TextBox>
                                    <asp:CustomValidator ID="CUVNome" runat="server" OnServerValidate="CUVNome_ServerValidate"
                                        ControlToValidate="TXBnome"></asp:CustomValidator>
                                    <asp:RequiredFieldValidator runat="server" ID="RFVNome" ControlToValidate="TXBNome"></asp:RequiredFieldValidator><br />
                                </td>
                                <td valign="middle" align="right" bordercolor="white" class="DIVHelp">
                                    <asp:Label runat="server" ID="LBAiuto"></asp:Label>
                                </td>
                                <td width="30px" bordercolor="white" class="DIVHelp">
                                    <asp:ImageButton ID="IMBHelp" runat="server" ImageUrl="img/Help.png" />
                                </td>
                            </tr>
                            <tr>
                                <td colspan="3" bordercolor="white">
                                    <asp:Label ID="LBdescrizioneQuestionario" runat="server"></asp:Label>
                                    <br />
                                      <CTRL:CTRLeditor id="CTRLeditorDescrizioneQuestionario" runat="server" ContainerCssClass="containerclass" 
                                        LoaderCssClass="loadercssclass" EditorHeight="300px" EditorWidth="100%" FontSizes="2" AllAvailableFontnames="true"
                                        HTML='<%#Eval("descrizione")%>'>
                                    </CTRL:CTRLeditor>
                                    <br />
                                    <br />
                                    <asp:Label ID="LBTitoloDataCreazione" runat="server"></asp:Label>
                                    <asp:Label ID="LBDataCreazione" runat="server" Text='<%#Eval("dataCreazione")%>'
                                        Width="248px"></asp:Label>
                                    <br />
                                    <br />
                                    <asp:Label ID="LBDataInizioTitolo" runat="server" Text=""></asp:Label>
                                    <radCln:RadDatePicker ID="RDPDataInizio" runat="server" AllowEmpty="true" Culture="Italian (Italy)"
                                        SelectedDate='<%#Databinder.Eval(Container.DataItem,"dataInizio")%>'>
                                    </radCln:RadDatePicker>
                                    <asp:DropDownList ID="DDLOraInizio" runat="server">
                                    </asp:DropDownList>
                                    <asp:DropDownList ID="DDLMinutiInizio" runat="server">
                                    </asp:DropDownList>
                                    <asp:Label ID="LBDataFineTitolo" runat="server" Text=""></asp:Label>
                                    <radCln:RadDatePicker ID="RDPDataFine" runat="server" AllowEmpty="true" Culture="Italian (Italy)"
                                        SelectedDate='<%#Databinder.Eval(Container.DataItem,"dataInizio")%>'>
                                    </radCln:RadDatePicker>
                                    <asp:DropDownList ID="DDLOraFine" runat="server">
                                    </asp:DropDownList>
                                    <asp:DropDownList ID="DDLMinutiFine" runat="server">
                                    </asp:DropDownList>
                                    <br />
                                    <asp:CustomValidator ID="CUVdate" runat="server" OnServerValidate="CUVdate_ServerValidate"
                                        ControlToValidate="RDPDataFine" ErrorMessage=""></asp:CustomValidator><br />
                                    <asp:CheckBox runat="server" ID="CKisBloccato" Checked='<%#Eval("isBloccato")%>' /><asp:Label
                                        runat="server" ID="LBisBloccato" Text=""></asp:Label>
                                    <asp:CheckBox runat="server" ID="CKisChiuso" Checked='<%#Eval("isReadOnly")%>' /><asp:Label
                                        runat="server" ID="LBisChiuso" Text=""></asp:Label><br />
                                    <br />
                                </td>
                            </tr>
                        </table>
                        <br />
                        <asp:Label runat="server" ID="LBModalita" Font-Bold="true"></asp:Label><br />
                        <asp:Panel runat="server" ID="PNLModalita" BackColor="white" BorderColor="black">
                            <table border="1" bordercolor="black" width="100%" cellpadding="0" cellspacing="0"
                                style="margin-bottom: 15px">
                                <tr>
                                    <td bordercolor="white" style="padding: 7px;">
                                        <div runat="server" id="DIVDurata" visible='<%#Eval("isVisibleDurata")%>'>
                                            <asp:Label ID="LBDurata" runat="server" Text=""></asp:Label>
                                            <br />
                                            <asp:TextBox ID="TBDurata" runat="server" Text='<%#Eval("durata")%>'></asp:TextBox>
                                            <asp:CompareValidator runat="server" ID="COVDurataInt" Operator="DataTypeCheck" Type="Integer"
                                                Display="Dynamic" ControlToValidate="TBDurata" />
                                            <br />
                                            <br />
                                            <asp:Label ID="LBScalaValutazione" runat="server" Text=""></asp:Label><br />
                                            <asp:TextBox ID="TXBScalaValutazione" runat="server" Text='<%#Eval("scalaValutazione")%>'></asp:TextBox>
                                            <asp:CompareValidator runat="server" ID="COVScalaValutazioneInt" Operator="DataTypeCheck"
                                                Type="Integer" Display="Dynamic" ControlToValidate="TXBScalaValutazione" />
                                            <br />
                                            <br />
                                        </div>
                                        <asp:CheckBox runat="server" Checked='<%#Eval("isRandomOrder")%>' ID="CHKOrdineDomandeRandom" />
                                        <asp:Label ID="LBOrdineDomandeRandom" runat="server"></asp:Label>
                                        <asp:Label ID="LBpermessiCompilatore" runat="server"></asp:Label>
                                        <asp:CheckBox runat="server" ID="CHKvisualizzaRisposta" Checked='<%#Eval("visualizzaRisposta")%>' />
                                        <asp:Label ID="LBvisualizzaRisposta" runat="server"></asp:Label>
                                        <asp:CheckBox ID="CHKvisualizzaCorrezione" runat="server" Checked='<%#Eval("visualizzaCorrezione")%>' />
                                        <asp:Label ID="LBvisualizzaCorrezione" runat="server"></asp:Label>
                                        <asp:CheckBox ID="CHKvisualizzaSuggerimenti" runat="server" Checked='<%#Eval("visualizzaSuggerimenti")%>' />
                                        <asp:Label ID="LBvisualizzaSuggerimenti" runat="server"></asp:Label>
                                        <asp:CheckBox runat="server" ID="CHKeditaRisposta" Checked='<%#Eval("editaRisposta")%>' />
                                        <asp:Label ID="LBeditaRisposta" runat="server"></asp:Label>
                                        <br />
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                    </ItemTemplate>
                </asp:FormView>
            </asp:Panel>
            <br />
            <asp:Label runat="server" ID="LBDestinatari" Font-Bold="true"></asp:Label><br />
            <asp:Panel runat="server" ID="PNLDestinatari" BackColor="white" BorderColor="black"
                BorderWidth="1px" CssClass="panelAdmin">
                <div runat="server" id="DIVutentiComunita">
                    <asp:Label runat="server" ID="LBDescrizione"></asp:Label><br />
                    <br />
                    <asp:CheckBox runat="server" ID="CHKUtentiComunita" Text="" /><br />
                </div>
                <div runat="server" id="DIVutentiNonComunita" style="display: none">
                    <br />
                    <asp:CheckBox runat="server" ID="CHKUtentiNonComunita" Text="" />
                    <br />
                </div>
                <br />
                <asp:CheckBox runat="server" ID="CHKUtentiEsterni" Text="" />
                <asp:Label ID="LBTitoloUrl" runat="server"></asp:Label>
                <asp:HyperLink Target="_blank" Text="link" runat="server" ID="HYPUrl" NavigateUrl='<%#Eval("url")%>'></asp:HyperLink>
                <br />
                <br />
                <asp:CheckBox runat="server" ID="CHKUtentiInvitati" Text="" />
                &nbsp;
                <asp:Button CssClass="Link_Menu" runat="server" ID="BTNInvitaUtenti" Text="" /><br />
            </asp:Panel>
            <asp:PlaceHolder runat="server" ID="PHOpzioni"></asp:PlaceHolder>
            <br />
            <asp:Label runat="server" ID="LBGestioneAvanzata" Font-Bold="true"></asp:Label>
            <asp:Panel runat="server" ID="PNLCopiaQuestionario" BackColor="white" BorderColor="black"
                BorderWidth="1px" CssClass="panelAdmin">
                <asp:Label runat="server" ID="LBAltraComunita"></asp:Label>
                <asp:DropDownList ID="DDLComunita" runat="server" DataTextField="nome" DataValueField="id">
                </asp:DropDownList>
                <asp:Button ID="BTNComunitaGruppo" runat="server" CommandName="" Text="" CssClass="Link_Menu" />
                <asp:DropDownList ID="DDLComunitaGruppo" runat="server" DataTextField="nome" DataValueField="id"
                    Visible="false">
                </asp:DropDownList>
                <asp:Button ID="BTNCopiaComunita" runat="server" Text="" CssClass="Link_Menu" />
                <br />
                <br />
                <asp:Label ID="LBNuovaLingua" runat="server" Text=""></asp:Label>
                <asp:DropDownList ID="DDLNuovaLingua" runat="server" DataTextField="nome" DataValueField="id">
                </asp:DropDownList>
                <asp:Button ID="BTNSalvaAltraLingua" runat="server" Text="" CssClass="Link_Menu" /><br />
                <br />
                <asp:Label ID="LBGruppo" runat="server" Text=""></asp:Label>
                <div style="display: none">
                    <asp:DropDownList ID="DDLGruppo" runat="server" DataTextField="nome" DataValueField="id">
                    </asp:DropDownList>
                    <br />
                    <br />
                    <asp:Label runat="server" ID="LBAltraCartella"> </asp:Label>
                    <asp:DropDownList ID="DDLCartelle" runat="server" DataTextField="nome" DataValueField="id">
                    </asp:DropDownList>
                    <asp:Button ID="BTNCopiaCartella" runat="server" Text="" CssClass="Link_Menu" />
                    <br />
                    <br />
                </div>
            </asp:Panel>
            <br />
            <asp:Label ID="LBinserireNome" runat="server" Font-Bold="True" ForeColor="Red" Height="25px"
                Text="" Visible="False"></asp:Label>&nbsp;
            <br />
            <asp:LinkButton ID="LNBSalvaBottom" Visible="true" runat="server" CssClass="Link_Menu">
            </asp:LinkButton>
        </asp:View>
        <asp:View runat="server" ID="VIWmessaggi">
            <asp:LinkButton ID="LNBTornaGestioneQuestionari" Visible="true" runat="server" CssClass="Link_Menu"
                CausesValidation="false" HorizontalAlign="right"></asp:LinkButton>&nbsp;
            <br />
            <br />
            <asp:Label ID="LBerrore" runat="server" Visible="True"></asp:Label>
            <asp:Label ID="LBCopiaSottocartella" runat="server" Visible="false"></asp:Label>
            <asp:Label ID="LBCopiaComunita" runat="server" Visible="false"></asp:Label>
        </asp:View>
    </asp:MultiView>
</asp:Content>
