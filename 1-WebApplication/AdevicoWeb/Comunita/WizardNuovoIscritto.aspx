<%@ Page Language="vb" AutoEventWireup="false" MasterPageFile="~/AjaxPortal.Master"
    CodeBehind="WizardNuovoIscritto.aspx.vb" Inherits="Comunita_OnLine.WizardNuovoIscritto" %>
<%@ MasterType VirtualPath="~/AjaxPortal.Master" %>
<%@ Register TagPrefix="CTRL" TagName="Messages" Src="~/Modules/Common/UC/UC_ActionMessages.ascx" %>
<%@ Register TagPrefix="CTRL" TagName="CTRLsorgenteComunita" Src="./../UC/UC_FiltroComunitaByServizio_NEW.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <script language="Javascript" src="./../jscript/generali.js" type="text/javascript"></script>
    <script language="javascript" type="text/javascript">
        function SelectMe(Me) {
            var HIDcheckbox, selezionati;
            //eval('HIDcheckbox= this.document.forms[0].HDabilitato')

            //eval('HIDcheckbox=<%=Me.HDabilitato.ClientID%>')
            HIDcheckbox = this.document.getElementById('<%=Me.HDabilitato.ClientID%>');
            selezionati = 0
            for (i = 0; i < document.forms[0].length; i++) {
                e = document.forms[0].elements[i];
                if (e.type == 'checkbox' && e.name.indexOf("CBabilitato") != -1) {
                    if (e.checked == true) {
                        selezionati++
                        if (HIDcheckbox.value == "") {
                            HIDcheckbox.value = ',' + e.value + ','
                        }
                        else {
                            pos1 = HIDcheckbox.value.indexOf(',' + e.value + ',')
                            if (pos1 == -1)
                                HIDcheckbox.value = HIDcheckbox.value + e.value + ','
                        }
                    }
                    else {
                        valore = HIDcheckbox.value
                        pos1 = HIDcheckbox.value.indexOf(',' + e.value + ',')
                        if (pos1 != -1) {
                            stringa = ',' + e.value
                            HIDcheckbox.value = HIDcheckbox.value.substring(0, pos1)
                            HIDcheckbox.value = HIDcheckbox.value + valore.substring(pos1 + e.value.length + 1, valore.length)
                        }
                    }
                }
            }
            if (HIDcheckbox.value == ",")
                HIDcheckbox.value = ""

        }

        function SelectAll(SelectAllBox) {
            var actVar = SelectAllBox.checked;
            var TBcheckbox;
            //eval('HDabilitato= this.document.forms[0].HDabilitato')
            //eval('HDabilitato=<%=Me.HDabilitato.ClientID%>')
            HDabilitato = this.document.getElementById('<%=Me.HDabilitato.ClientID%>');
            HDabilitato.value = ""
            for (i = 0; i < document.forms[0].length; i++) {
                e = document.forms[0].elements[i];
                if (e.type == 'checkbox' && e.name.indexOf("CBabilitato") != -1) {
                    e.checked = actVar;
                    if (e.checked == true)
                        if (HDabilitato.value == "")
                            HDabilitato.value = ',' + e.value + ','
                        else
                            HDabilitato.value = HDabilitato.value + e.value + ','
                    }
                }
            }

            function UserSelezionati(messaggioSelezione) {
                try {
                    if (document.forms[0].HDabilitato.value == "," || document.forms[0].HDabilitato.value == "") {
                        alert(messaggioSelezione);
                        return false;
                    }
                    else
                        return true;
                }
                catch (ex) {
                    return true;
                }
            }
					
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="CPHservice" runat="server">
    <input type="hidden" id="HDNazione" value="gestioneTipo" runat="server" name="HDNazione" />
    <asp:Table ID="TBLprincipale" runat="server" CellPadding="0" GridLines="None" Width="900px"
        CellSpacing="0">
<%--        <asp:TableRow>
            <asp:TableCell HorizontalAlign="Left" CssClass="RigaTitolo">
                <asp:Label ID="LBTitolo" runat="server">Iscrizione utente/i</asp:Label>
            </asp:TableCell>
        </asp:TableRow>--%>
        <asp:TableRow ID="TBRmenu">
            <asp:TableCell HorizontalAlign="Right">
                &nbsp;
                <asp:Button ID="BTNgoToManagement" runat="server" CssClass="PulsanteFiltro" Text="Vai al management"
                    CausesValidation="False"></asp:Button>
                <asp:Button ID="BTNtornaPaginaElenco" runat="server" CssClass="PulsanteFiltro" Text="Torna agli iscritti"
                    CausesValidation="False"></asp:Button>
            </asp:TableCell>
        </asp:TableRow>
        <asp:TableRow>
             
            <asp:TableCell CssClass="top">
                <br />
                <asp:Panel ID="PNLpermessi" runat="server" Visible="False">
                    <br />
                    <table align="center">
                        <tr>
                            <td height="50">
                                &nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td align="center">
                                <asp:Label ID="LBNopermessi" runat="server" CssClass="messaggio"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td height="50">
                                &nbsp;
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
                <asp:Panel ID="PNLnavigazione2" runat="server" HorizontalAlign="Right" Width="100%"
                    BorderWidth="1">
                    <table cellspacing="0" cellpadding="0" border="0" align="right">
                        <tr>
                            <td align="left" width="590px">
                                <asp:Button ID="BTNgoToManagementAlto" runat="server" CssClass="PulsanteFiltro" Text="Vai al management"
                                    CausesValidation="False"></asp:Button>
                                <asp:Button ID="BTNtornaPaginaElencoAlto" runat="server" CssClass="PulsanteFiltro"
                                    Text="Torna agli iscritti" CausesValidation="False"></asp:Button>
                            </td>
                            <td width="35">
                                &nbsp;
                            </td>
                            <td width="100" nowrap="nowrap">
                                <asp:Button ID="BTNindietro2" runat="server" CssClass="PulsanteFiltro" Text="< Indietro"
                                    CausesValidation="False"></asp:Button>
                            </td>
                            <td width="5">
                                &nbsp;
                            </td>
                            <td width="150px" nowrap="nowrap">
                                <asp:Button ID="BTNavanti2" runat="server" CssClass="PulsanteFiltro" Text="Avanti >"
                                    CausesValidation="True"></asp:Button>
                                <asp:Button ID="BTNconferma2" runat="server" CssClass="PulsanteFiltro" Text="Iscrivi">
                                </asp:Button>
                            </td>
                            <td width="20">
                                &nbsp;
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
                <asp:Panel ID="PNLcontenuto" runat="server" HorizontalAlign="Center" Width="900px"
                    BorderWidth="1">
                    <br />
                    <asp:Table runat="server" ID="TBLinserimento" CellPadding="0" CellSpacing="0" Width="900px"
                        Height="450px">
                        <asp:TableRow>
                            <asp:TableCell>&nbsp;</asp:TableCell>
                            <asp:TableCell HorizontalAlign="left" CssClass="top">
                                <asp:Table HorizontalAlign="center" runat="server" ID="TBLsorgente" Width="800px"
                                    Visible="true">
                                    <asp:TableRow>
                                        <asp:TableCell ColumnSpan="2">&nbsp;</asp:TableCell>
                                    </asp:TableRow>
                                    <asp:TableRow>
                                        <asp:TableCell ColumnSpan="2">
                                            <asp:Label ID="LBinfoSorgente" runat="server">
											Scegliere la comunità da dove importare gli utenti
                                            </asp:Label>
                                        </asp:TableCell>
                                    </asp:TableRow>
                                    <asp:TableRow>
                                        <asp:TableCell ColumnSpan="2">
                                            <asp:Label ID="LBscelta_t" runat="server" CssClass="Titolo_CampoSmall">Importa utenti da:</asp:Label>
                                            &nbsp;
                                            <asp:RadioButtonList ID="RBLimporta" CssClass="Testo_CampoSmall" RepeatDirection="Horizontal"
                                                RepeatLayout="Flow" runat="server" AutoPostBack="True">
                                                <asp:ListItem Value="0">Dal sistema</asp:ListItem>
                                                <asp:ListItem Value="1">Da una comunità</asp:ListItem>
                                            </asp:RadioButtonList>
                                        </asp:TableCell>
                                    </asp:TableRow>
                                    <asp:TableRow ID="TBRcomunita">
                                        <asp:TableCell>
                                            <asp:Label ID="LBcomunita_t" runat="server" CssClass="Titolo_CampoSmall">Comunità:</asp:Label>
                                        </asp:TableCell>
                                        <asp:TableCell>
                                            <CTRL:CTRLsorgenteComunita ID="CTRLsorgenteComunita" runat="server" Width="800px"
                                                LarghezzaFinestraAlbero="800px" ColonneNome="100"></CTRL:CTRLsorgenteComunita>
                                        </asp:TableCell>
                                    </asp:TableRow>
                                </asp:Table>
                                <input type="hidden" runat="server" id="HDN_filtroRuolo" name="HDN_filtroRuolo" />
                                <input type="hidden" runat="server" id="HDN_filtroTipoRicerca" name="HDN_filtroTipoRicerca" />
                                <input type="hidden" runat="server" id="HDN_filtroValore" name="HDN_filtroValore" />
                                <asp:Table runat="server" ID="TBLutenti" Width="850px" Visible="False" HorizontalAlign="Center">
                                    <asp:TableRow>
                                        <asp:TableCell>
                                            <CTRL:Messages id="CTRLmessages" runat="server" Visible="false" />
                                            <input id="HDabilitato" type="hidden" name="HDabilitato" runat="server" />
                                            <asp:Table ID="TBLfiltroNew" runat="server" Width="850px" CellPadding="0" CellSpacing="0"
                                                GridLines="none" HorizontalAlign="center">
                                                <asp:TableRow ID="TBRchiudiFiltro" Height="22px">
                                                    <asp:TableCell CssClass="Filtro_CellSelezionato" HorizontalAlign="Center" Width="150px"
                                                        Height="22px" VerticalAlign="Middle">
                                                        <asp:LinkButton ID="LNBchiudiFiltro" runat="server" CssClass="Filtro_Link" CausesValidation="False">Chiudi Filtri</asp:LinkButton>
                                                    </asp:TableCell>
                                                    <asp:TableCell CssClass="Filtro_CellDeSelezionato" Width="700px" Height="22px">&nbsp;
                                                    </asp:TableCell>
                                                </asp:TableRow>
                                                <asp:TableRow ID="TBRapriFiltro" Visible="False" Height="22px">
                                                    <asp:TableCell CssClass="Filtro_CellApriFiltro" HorizontalAlign="Center" Width="150px"
                                                        Height="22px">
                                                        <asp:LinkButton ID="LNBapriFiltro" runat="server" CssClass="Filtro_Link" CausesValidation="False">Apri Filtri</asp:LinkButton>
                                                    </asp:TableCell>
                                                    <asp:TableCell CssClass="Filtro_Cellnull" Width="700px" Height="22px">&nbsp;
                                                    </asp:TableCell>
                                                </asp:TableRow>
                                                <asp:TableRow ID="TBRfiltri">
                                                    <asp:TableCell CssClass="Filtro_CellFiltri" ColumnSpan="2" HorizontalAlign="center">
                                                        <asp:Table runat="server" ID="TBLfiltro" CellPadding="1" CellSpacing="0" Width="800px"
                                                            HorizontalAlign="center" GridLines="none">
                                                            <asp:TableRow>
                                                                <asp:TableCell ColumnSpan="2">
                                                                    <table cellspacing="1" cellpadding="1" border="0">
                                                                        <tr>
                                                                            <td class="FiltroVoceSmall" nowrap="nowrap">
                                                                                <asp:Label ID="LBtipoRuolo_t" runat="server">Role:</asp:Label>
                                                                                <asp:Label ID="LBtipoPersona_t" runat="server" Visible="False">Type:</asp:Label>
                                                                                <asp:DropDownList ID="DDLTipoRuolo" runat="server" CssClass="FiltroCampoSmall" AutoPostBack="true">
                                                                                </asp:DropDownList>
                                                                                <asp:DropDownList ID="DDLTipoPersona" runat="server" CssClass="FiltroCampoSmall"
                                                                                    AutoPostBack="true" Visible="False">
                                                                                </asp:DropDownList>
                                                                            </td>
                                                                            <td width="5">
                                                                                &nbsp;
                                                                            </td>
                                                                            <td class="FiltroVoceSmall" nowrap="nowrap">
                                                                                <asp:Label ID="LBtipoRicerca_t" runat="server">Cerca per:</asp:Label>
                                                                                <asp:DropDownList ID="DDLTipoRicerca" CssClass="FiltroCampoSmall" runat="server"
                                                                                    AutoPostBack="false">
                                                                                    <asp:ListItem Value="-2" Selected="true">Nome</asp:ListItem>
                                                                                    <asp:ListItem Value="-3">Cognome</asp:ListItem>
                                                                                    <asp:ListItem Value="-4">Nome/Cognome</asp:ListItem>
                                                                                    <asp:ListItem Value="-7">Login</asp:ListItem>
                                                                                </asp:DropDownList>
                                                                            </td>
                                                                            <td width="5">
                                                                                &nbsp;
                                                                            </td>
                                                                            <td class="FiltroVoceSmall" nowrap="nowrap">
                                                                                <asp:Label ID="LBvalore_t" runat="server">Valore:</asp:Label>&nbsp;
                                                                                <asp:TextBox ID="TXBValore" CssClass="FiltroCampoSmall" runat="server" Columns="30"
                                                                                    MaxLength="100"></asp:TextBox>
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                </asp:TableCell>
                                                            </asp:TableRow>
                                                            <asp:TableRow>
                                                                <asp:TableCell runat="server">
                                                                    <asp:Panel runat="server" ID="PNLuserEnabled" Visible="true">
                                                                        <asp:Label ID="LBLuserEnabled_t" runat="server" CssClass="FiltroVoceSmall">Abilitato: </asp:Label>
                                                                        <asp:DropDownList runat="server" ID="DDLuserEnabled" CssClass="FiltroCampoSmall">
                                                                            <asp:ListItem Text="All" Value="-1" Selected="True"></asp:ListItem>    
                                                                            <asp:ListItem Text="Hide unavailables" Value="1"></asp:ListItem>    
                                                                            <asp:ListItem Text="Only unavailables" Value="2"></asp:ListItem>    
                                                                            <asp:ListItem Text="Only wainting" Value="3"></asp:ListItem>    
                                                                            <asp:ListItem Text="Only disabled" Value="4"></asp:ListItem>   
                                                                        </asp:DropDownList>
                                                                    </asp:Panel>
                                                                </asp:TableCell>
                                                                <asp:TableCell HorizontalAlign="Right" ColumnSpan="1">
                                                                    <asp:CheckBox ID="CBXaggiorna" runat="server" Checked="true" AutoPostBack="True"
                                                                        CssClass="FiltroCampoSmall" Text="Aggiornamento automatico"></asp:CheckBox>
                                                                    &nbsp;&nbsp;
                                                                    <asp:Button ID="BTNCerca" CssClass="PulsanteFiltro" runat="server" Text="Cerca">
                                                                    </asp:Button>
                                                                </asp:TableCell>
                                                            </asp:TableRow>
                                                            <asp:TableRow>
                                                                <asp:TableCell ColumnSpan="2" Height="10px">&nbsp;</asp:TableCell>
                                                            </asp:TableRow>
                                                        </asp:Table>
                                                    </asp:TableCell>
                                                </asp:TableRow>
                                                <asp:TableRow Visible="true">
                                                    <asp:TableCell ColumnSpan="2" HorizontalAlign="Center">
                                                        <table cellpadding="0" cellspacing="0" align="center" width="850px" border="0">
                                                            <tr>
                                                                <td>
                                                                    <table align="left" width="400px">
                                                                        <tr>
                                                                            <td align="center" nowrap="nowrap">
                                                                                <asp:LinkButton ID="LKBtutti" runat="server" CssClass="lettera" CommandArgument="-1"
                                                                                    OnClick="FiltroLinkLettere_Click">Tutti</asp:LinkButton>
                                                                            </td>
                                                                            <td align="center" nowrap="nowrap">
                                                                                <asp:LinkButton ID="LKBaltro" runat="server" CssClass="lettera" CommandArgument="0"
                                                                                    OnClick="FiltroLinkLettere_Click">Altro</asp:LinkButton>
                                                                            </td>
                                                                            <td align="center">
                                                                                <asp:LinkButton ID="LKBa" runat="server" CssClass="lettera" CommandArgument="1" OnClick="FiltroLinkLettere_Click">A</asp:LinkButton>
                                                                            </td>
                                                                            <td align="center">
                                                                                <asp:LinkButton ID="LKBb" runat="server" CssClass="lettera" CommandArgument="2" OnClick="FiltroLinkLettere_Click">B</asp:LinkButton>
                                                                            </td>
                                                                            <td align="center">
                                                                                <asp:LinkButton ID="LKBc" runat="server" CssClass="lettera" CommandArgument="3" OnClick="FiltroLinkLettere_Click">C</asp:LinkButton>
                                                                            </td>
                                                                            <td align="center">
                                                                                <asp:LinkButton ID="LKBd" runat="server" CssClass="lettera" CommandArgument="4" OnClick="FiltroLinkLettere_Click">D</asp:LinkButton>
                                                                            </td>
                                                                            <td align="center">
                                                                                <asp:LinkButton ID="LKBe" runat="server" CssClass="lettera" CommandArgument="5" OnClick="FiltroLinkLettere_Click">E</asp:LinkButton>
                                                                            </td>
                                                                            <td align="center">
                                                                                <asp:LinkButton ID="LKBf" runat="server" CssClass="lettera" CommandArgument="6" OnClick="FiltroLinkLettere_Click">F</asp:LinkButton>
                                                                            </td>
                                                                            <td align="center">
                                                                                <asp:LinkButton ID="LKBg" runat="server" CssClass="lettera" CommandArgument="7" OnClick="FiltroLinkLettere_Click">G</asp:LinkButton>
                                                                            </td>
                                                                            <td align="center">
                                                                                <asp:LinkButton ID="LKBh" runat="server" CssClass="lettera" CommandArgument="8" OnClick="FiltroLinkLettere_Click">H</asp:LinkButton>
                                                                            </td>
                                                                            <td align="center">
                                                                                <asp:LinkButton ID="LKBi" runat="server" CssClass="lettera" CommandArgument="9" OnClick="FiltroLinkLettere_Click">I</asp:LinkButton>
                                                                            </td>
                                                                            <td align="center">
                                                                                <asp:LinkButton ID="LKBj" runat="server" CssClass="lettera" CommandArgument="10"
                                                                                    OnClick="FiltroLinkLettere_Click">J</asp:LinkButton>
                                                                            </td>
                                                                            <td align="center">
                                                                                <asp:LinkButton ID="LKBk" runat="server" CssClass="lettera" CommandArgument="11"
                                                                                    OnClick="FiltroLinkLettere_Click">K</asp:LinkButton>
                                                                            </td>
                                                                            <td align="center">
                                                                                <asp:LinkButton ID="LKBl" runat="server" CssClass="lettera" CommandArgument="12"
                                                                                    OnClick="FiltroLinkLettere_Click">L</asp:LinkButton>
                                                                            </td>
                                                                            <td align="center">
                                                                                <asp:LinkButton ID="LKBm" runat="server" CssClass="lettera" CommandArgument="13"
                                                                                    OnClick="FiltroLinkLettere_Click">M</asp:LinkButton>
                                                                            </td>
                                                                            <td align="center">
                                                                                <asp:LinkButton ID="LKBn" runat="server" CssClass="lettera" CommandArgument="14"
                                                                                    OnClick="FiltroLinkLettere_Click">N</asp:LinkButton>
                                                                            </td>
                                                                            <td align="center">
                                                                                <asp:LinkButton ID="LKBo" runat="server" CssClass="lettera" CommandArgument="15"
                                                                                    OnClick="FiltroLinkLettere_Click">O</asp:LinkButton>
                                                                            </td>
                                                                            <td align="center">
                                                                                <asp:LinkButton ID="LKBp" runat="server" CssClass="lettera" CommandArgument="16"
                                                                                    OnClick="FiltroLinkLettere_Click">P</asp:LinkButton>
                                                                            </td>
                                                                            <td align="center">
                                                                                <asp:LinkButton ID="LKBq" runat="server" CssClass="lettera" CommandArgument="17"
                                                                                    OnClick="FiltroLinkLettere_Click">Q</asp:LinkButton>
                                                                            </td>
                                                                            <td align="center">
                                                                                <asp:LinkButton ID="LKBr" runat="server" CssClass="lettera" CommandArgument="18"
                                                                                    OnClick="FiltroLinkLettere_Click">R</asp:LinkButton>
                                                                            </td>
                                                                            <td align="center">
                                                                                <asp:LinkButton ID="LKBs" runat="server" CssClass="lettera" CommandArgument="19"
                                                                                    OnClick="FiltroLinkLettere_Click">S</asp:LinkButton>
                                                                            </td>
                                                                            <td align="center">
                                                                                <asp:LinkButton ID="LKBt" runat="server" CssClass="lettera" CommandArgument="20"
                                                                                    OnClick="FiltroLinkLettere_Click">T</asp:LinkButton>
                                                                            </td>
                                                                            <td align="center">
                                                                                <asp:LinkButton ID="LKBu" runat="server" CssClass="lettera" CommandArgument="21"
                                                                                    OnClick="FiltroLinkLettere_Click">U</asp:LinkButton>
                                                                            </td>
                                                                            <td align="center">
                                                                                <asp:LinkButton ID="LKBv" runat="server" CssClass="lettera" CommandArgument="22"
                                                                                    OnClick="FiltroLinkLettere_Click">V</asp:LinkButton>
                                                                            </td>
                                                                            <td align="center">
                                                                                <asp:LinkButton ID="LKBw" runat="server" CssClass="lettera" CommandArgument="23"
                                                                                    OnClick="FiltroLinkLettere_Click">W</asp:LinkButton>
                                                                            </td>
                                                                            <td align="center">
                                                                                <asp:LinkButton ID="LKBx" runat="server" CssClass="lettera" CommandArgument="24"
                                                                                    OnClick="FiltroLinkLettere_Click">X</asp:LinkButton>
                                                                            </td>
                                                                            <td align="center">
                                                                                <asp:LinkButton ID="LKBy" runat="server" CssClass="lettera" CommandArgument="25"
                                                                                    OnClick="FiltroLinkLettere_Click">Y</asp:LinkButton>
                                                                            </td>
                                                                            <td align="center">
                                                                                <asp:LinkButton ID="LKBz" runat="server" CssClass="lettera" CommandArgument="26"
                                                                                    OnClick="FiltroLinkLettere_Click">Z</asp:LinkButton>
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                </td>
                                                                <td align="right">
                                                                    <asp:Label ID="LBnumeroRecord_t" runat="server" CssClass="Filtro_TestoPaginazione">N° Record:</asp:Label>&nbsp;
                                                                    <asp:DropDownList ID="DDLNumeroRecord" CssClass="Filtro_RecordPaginazioneIndex" runat="server"
                                                                        AutoPostBack="true">
                                                                        <asp:ListItem Value="25"></asp:ListItem>
                                                                        <asp:ListItem Value="50" Selected="true"></asp:ListItem>
                                                                        <asp:ListItem Value="75"></asp:ListItem>
                                                                        <asp:ListItem Value="100"></asp:ListItem>
                                                                        <asp:ListItem Value="150"></asp:ListItem>
                                                                    </asp:DropDownList>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </asp:TableCell>
                                                </asp:TableRow>
                                                <asp:TableRow Visible="true">
                                                    <asp:TableCell ColumnSpan="2" HorizontalAlign="Center">
                                                        <asp:DataGrid ID="DGPersone" runat="server" DataKeyField="PRSN_id" AllowPaging="true"
                                                            AutoGenerateColumns="False" ShowFooter="false" AllowSorting="true" CssClass="DataGrid_Generica">
                                                            <AlternatingItemStyle CssClass="ROW_Alternate_Small"></AlternatingItemStyle>
                                                            <HeaderStyle CssClass="ROW_header_Small_Center"></HeaderStyle>
                                                            <ItemStyle CssClass="ROW_Normal_Small" Height="22px"></ItemStyle>
                                                            <PagerStyle CssClass="ROW_Page_Small" Position="TopAndBottom" Mode="NumericPages"
                                                                Visible="true" HorizontalAlign="Right" Height="18px" VerticalAlign="Bottom">
                                                            </PagerStyle>
                                                            <Columns>
                                                                <asp:BoundColumn DataField="PRSN_id" Visible="False"></asp:BoundColumn>
                                                                <asp:TemplateColumn HeaderText="" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="10">
                                                                    <ItemTemplate>
                                                                        <asp:ImageButton ID="IMBinfo" runat="server" CausesValidation="False" CommandName="infoPersona"
                                                                            ImageUrl="../images/proprieta.gif"></asp:ImageButton>
                                                                    </ItemTemplate>
                                                                </asp:TemplateColumn>
                                                                <asp:TemplateColumn HeaderText="Matricola" ItemStyle-CssClass="ROW_TD_Small" HeaderStyle-CssClass="ROW_header_Small_Center">
                                                                    <ItemTemplate>
                                                                        <asp:Label runat="server" ID="LBmatricola">
																			&nbsp;<%# DataBinder.Eval(Container.DataItem, "oSTDN_Matricola") %>
                                                                        </asp:Label>
                                                                    </ItemTemplate>
                                                                </asp:TemplateColumn>
                                                                <asp:TemplateColumn ItemStyle-CssClass="ROW_TD_Small" HeaderText="Cognome" SortExpression="PRSN_Cognome">
                                                                    <ItemTemplate>
                                                                        &nbsp;<%# DataBinder.Eval(Container.DataItem, "PRSN_Cognome") %>
                                                                    </ItemTemplate>
                                                                </asp:TemplateColumn>
                                                                <asp:TemplateColumn ItemStyle-CssClass="ROW_TD_Small" HeaderText="Nome" SortExpression="PRSN_Nome">
                                                                    <ItemTemplate>
                                                                        &nbsp;<%# DataBinder.Eval(Container.DataItem, "PRSN_Nome") %>
                                                                    </ItemTemplate>
                                                                </asp:TemplateColumn>
                                                                <asp:BoundColumn DataField="PRSN_Anagrafica" HeaderText="Anagrafica" Visible="False">
                                                                </asp:BoundColumn>
                                                                <asp:TemplateColumn HeaderText="Mail" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="160px"
                                                                    ItemStyle-CssClass="ROW_TD_Small" HeaderStyle-CssClass="ROW_header_Small_Center">
                                                                    <ItemTemplate>
                                                                        <table border="0" align="left">
                                                                            <tr>
                                                                                <td>
                                                                                    &nbsp;
                                                                                </td>
                                                                                <td>
                                                                                    <asp:HyperLink NavigateUrl='<%# "mailto:" & Container.Dataitem("PRSN_mail")%>' Text='<%# server.HtmlEncode(replace(Container.Dataitem("PRSN_mail") ,"-","&ndash;"))%>'
                                                                                        runat="server" ID="HYPMail" CssClass="ROW_ItemLink_Small" />
                                                                                </td>
                                                                            </tr>
                                                                        </table>
                                                                    </ItemTemplate>
                                                                </asp:TemplateColumn>
                                                                <asp:TemplateColumn ItemStyle-CssClass="ROW_TD_Small" HeaderText="Tipo Ruolo" SortExpression="TPRL_nome">
                                                                    <ItemTemplate>
                                                                         <span class="sysdisabled" style="display: none;"><%# GetDisabled(Container.DataItem) %></span>
                                                                        &nbsp;<%# DataBinder.Eval(Container.DataItem, "TPRL_nome") %>
                                                                    </ItemTemplate>
                                                                </asp:TemplateColumn>
                                                                <asp:BoundColumn DataField="PRSN_TPPR_id" Visible="False"></asp:BoundColumn>
                                                                <asp:TemplateColumn ItemStyle-CssClass="ROW_TD_Small" HeaderText="Tipo Persona" SortExpression="TPPR_descrizione">
                                                                    <ItemTemplate>
                                                                        &nbsp;<%# DataBinder.Eval(Container.DataItem, "TPPR_descrizione") %>
                                                                    </ItemTemplate>
                                                                </asp:TemplateColumn>
                                                                <asp:TemplateColumn ItemStyle-Width="20px" ItemStyle-CssClass="ROW_TD_Small_center">
                                                                    <HeaderTemplate>
                                                                        <input type="checkbox" id="SelectAll2" onclick="SelectAll(this);" runat="server"
                                                                            name="SelectAll" />
                                                                    </HeaderTemplate>
                                                                    <ItemTemplate>
                                                                        <input runat="server" type="checkbox" id="CBabilitato" name="CBabilitato" onclick="SelectMe(this);" />
                                                                    </ItemTemplate>
                                                                </asp:TemplateColumn>
                                                                <asp:TemplateColumn ItemStyle-CssClass="ROW_TD_Small" HeaderText="Login" SortExpression="PRSN_login"
                                                                    Visible="False">
                                                                    <ItemTemplate>
                                                                        &nbsp;<%# DataBinder.Eval(Container.DataItem, "PRSN_login") %>
                                                                    </ItemTemplate>
                                                                </asp:TemplateColumn>
                                                                <asp:BoundColumn DataField="PRSN_Cognome" Visible="False"></asp:BoundColumn>
                                                                <asp:BoundColumn DataField="PRSN_Nome" Visible="False"></asp:BoundColumn>
                                                                <asp:BoundColumn DataField="oSTDN_Matricola" Visible="False"></asp:BoundColumn>
                                                            </Columns>
                                                            <PagerStyle Width="850px" PageButtonCount="5" Mode="NumericPages"></PagerStyle>
                                                        </asp:DataGrid>
                                                        <asp:Panel ID="PNLnessunUtente" runat="server" Visible="False">
                                                            <table>
                                                                <tr>
                                                                    <td>
                                                                        <br />
                                                                        <br />
                                                                        <asp:Label ID="LBnessunUtente" CssClass="avviso" runat="server"></asp:Label>
                                                                        <br />
                                                                        <br />
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </asp:Panel>
                                                    </asp:TableCell>
                                                </asp:TableRow>
                                            </asp:Table>
                                        </asp:TableCell>
                                    </asp:TableRow>
                                </asp:Table>
                                <asp:Table HorizontalAlign="center" runat="server" ID="TBLruolo" Width="850px" Visible="False">
                                    <asp:TableRow>
                                        <asp:TableCell>
                                            <br />
                                            <br />
                                            <br />
                                            <br />
                                            <table border="1" align="center" width="400px" cellspacing="0" style="border-color: #cccccc;
                                                background-color: #fffbf7">
                                                <tr>
                                                    <td align="center" bgcolor="#fffbf7">
                                                        <asp:Table ID="TBLdatiRuolo" runat="server" Width="500px" HorizontalAlign="Center"
                                                            BackColor="#FFFBF7" BorderStyle="none" GridLines="none">
                                                            <asp:TableRow>
                                                                <asp:TableCell>&nbsp;</asp:TableCell>
                                                                <asp:TableCell ColumnSpan="2">&nbsp</asp:TableCell>
                                                                <asp:TableCell>&nbsp;</asp:TableCell>
                                                            </asp:TableRow>
                                                            <asp:TableRow>
                                                                <asp:TableCell>&nbsp;</asp:TableCell>
                                                                <asp:TableCell ColumnSpan="2">
                                                                    <asp:Label ID="LBdescrizione" runat="server" CssClass="avviso11"></asp:Label>
                                                                </asp:TableCell>
                                                                <asp:TableCell>&nbsp;</asp:TableCell>
                                                            </asp:TableRow>
                                                            <asp:TableRow ID="TBRprofilo" runat="server" Visible="False">
                                                                <asp:TableCell>&nbsp;</asp:TableCell>
                                                                <asp:TableCell>
                                                                    <asp:Label ID="LBsceltaRuoli_t" runat="server" CssClass="Titolo_campoSmall">Scelta ruoli:&nbsp;</asp:Label>
                                                                </asp:TableCell>
                                                                <asp:TableCell>
                                                                    <asp:RadioButtonList ID="RBLsceltaRuoli" runat="server" CssClass="FiltroCampoSmall"
                                                                        RepeatDirection="Horizontal" AutoPostBack="True">
                                                                        <asp:ListItem Value="1" Selected="True">ruoli standard</asp:ListItem>
                                                                        <asp:ListItem Value="5">profilo comunità</asp:ListItem>
                                                                    </asp:RadioButtonList>
                                                                </asp:TableCell>
                                                                <asp:TableCell>&nbsp;</asp:TableCell>
                                                            </asp:TableRow>
                                                            <asp:TableRow>
                                                                <asp:TableCell>&nbsp;</asp:TableCell>
                                                                <asp:TableCell>
                                                                    <asp:Label ID="LBtiporuoloAggiungi_t" runat="server" CssClass="Titolo_campoSmall">Ruolo di:&nbsp;</asp:Label>
                                                                </asp:TableCell>
                                                                <asp:TableCell>
                                                                    <asp:DropDownList ID="DDLtipoRuoloAggiungi" runat="server" CssClass="FiltroCampoSmall">
                                                                    </asp:DropDownList>
                                                                </asp:TableCell>
                                                                <asp:TableCell>&nbsp;</asp:TableCell>
                                                            </asp:TableRow>
                                                            <asp:TableRow ID="TBRresponsabile">
                                                                <asp:TableCell>&nbsp;</asp:TableCell>
                                                                <asp:TableCell>
                                                                    <asp:Label ID="LBresponsabile_t" runat="server" CssClass="Titolo_campoSmall">Responsabile:</asp:Label>
                                                                </asp:TableCell>
                                                                <asp:TableCell>
                                                                    <asp:CheckBox ID="CBXresponsabile" runat="server" CssClass="Testo_CampoSmall" Text="Si">
                                                                    </asp:CheckBox>
                                                                </asp:TableCell>
                                                                <asp:TableCell>&nbsp;</asp:TableCell>
                                                            </asp:TableRow>
                                                            <asp:TableRow>
                                                                <asp:TableCell>&nbsp;</asp:TableCell>
                                                                <asp:TableCell Height="20px" ColumnSpan="2">&nbsp;</asp:TableCell>
                                                                <asp:TableCell>&nbsp;</asp:TableCell>
                                                            </asp:TableRow>
                                                        </asp:Table>
                                                        <br />
                                                    </td>
                                                </tr>
                                            </table>
                                        </asp:TableCell>
                                    </asp:TableRow>
                                </asp:Table>
                            </asp:TableCell>
                            <asp:TableCell Width="5px">&nbsp;</asp:TableCell>
                        </asp:TableRow>
                    </asp:Table>
                </asp:Panel>
                <asp:Panel ID="PNLnavigazione" runat="server" HorizontalAlign="Right" Width="100%"
                    BorderWidth="1">
                    <table cellspacing="0" cellpadding="0" border="0" align="right">
                        <tr>
                            <td align="left" width="590px">
                                <asp:Button ID="BTNgoToManagementBasso" runat="server" CssClass="PulsanteFiltro"
                                    Text="Vai al management" CausesValidation="False"></asp:Button>
                                <asp:Button ID="BTNtornaPaginaElencoBasso" runat="server" CssClass="PulsanteFiltro"
                                    Text="Torna agli iscritti" CausesValidation="False"></asp:Button>
                            </td>
                            <td width="35">
                                &nbsp;
                            </td>
                            <td width="100" nowrap="nowrap">
                                <asp:Button ID="BTNindietro" runat="server" CssClass="PulsanteFiltro" Text="< Indietro"
                                    CausesValidation="False"></asp:Button>
                            </td>
                            <td width="5">
                                &nbsp;
                            </td>
                            <td width="150px" nowrap="nowrap">
                                <asp:Button ID="BTNavanti" runat="server" CssClass="PulsanteFiltro" Text="Avanti >"
                                    CausesValidation="True"></asp:Button>
                                <asp:Button ID="BTNconferma" runat="server" CssClass="PulsanteFiltro" Text="Iscrivi">
                                </asp:Button>
                            </td>
                            <td width="20">
                                &nbsp;
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
            </asp:TableCell>
        </asp:TableRow>
    </asp:Table>
    <input type="hidden" id="HDN_ComunitaPadreID" runat="server" name="HDN_ComunitaPadreID" />
    <input id="HDattivato" type="hidden" name="HDattivato" runat="server" />
    <input id="HDtutti" type="hidden" name="HDtutti" runat="server" />
</asp:Content>
<%--
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<html>
  <head runat="server">
		<title>Comunità On Line - Wizard Nuovo iscritto</title>
		
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR"/>
		<meta content="Visual Basic .NET 7.1" name="CODE_LANGUAGE"/>
		<meta content="JavaScript" name="vs_defaultClientScript"/>
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema"/>
		<LINK href="./../Styles.css" type="text/css" rel="stylesheet"/>
		<style type="text/css">
			td{
			font-size: 11px;
			}
		</style>
</head>

	<body onkeydown="return SubmitRicerca(event);">
		<form id="aspnetForm" method="post" runat="server">
		<asp:ScriptManager ID="SCMmanager" runat="server"></asp:ScriptManager>
			<table cellspacing="0" cellpadding="0"  align="center" border="0" width="900px">
				<tr>
				    <td colspan="3" >
				    <div>
				        <HEADER:CtrLHEADER id="Intestazione" runat="server"></HEADER:CtrLHEADER>	
				    </div>
				    </td>
			    </tr>
				<tr class="contenitore">
					<td colSpan="3">

					</td>
				</tr>
				<tr class="contenitore">
					<td colSpan="3"></td>
				</tr>
			</table>
			<FOOTER:CtrLFOOTER id="CtrLFooter" runat="server"></FOOTER:CtrLFOOTER>
		</form>
	</body>
</html>



function SubmitRicerca(event){
					if (document.all){
						if (event.keyCode == 13){
							event.returnValue=false;
							event.cancel = true;
							try{
								document.forms[0].BTNCerca.click();}
							catch (ex){
								return false;}
							}
						}
					else if (document.getElementById){
						if (event.which == 13){
							event.returnValue=false;
							event.cancel = true;
							try{
								document.forms[0].BTNCerca.click();}
							catch(ex){
								return false;}
							}
						}
					else if(document.layers){
						if(event.which == 13){
							event.returnValue=false;
							event.cancel = true;
								try{
								document.forms[0].BTNCerca.click();}
							catch(ex){
								return false;}
							}
						}
					else
						return true;
				}	--%>