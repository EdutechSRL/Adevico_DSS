<%@ Page Language="vb" AutoEventWireup="false" MasterPageFile="~/AjaxPortal.Master"
    CodeBehind="GestioneIscritti.aspx.vb" Inherits="Comunita_OnLine.GestioneIscritti" %>

<%@ MasterType VirtualPath="~/AjaxPortal.Master" %>
<%--<%@ Register TagPrefix="FOOTER" TagName="CtrLFooter" Src="../UC/UC_Footer.ascx" %>
<%@ Register TagPrefix="HEADER" TagName="CtrLHeader" Src="../UC/UC_header.ascx" %>--%>
<%@ Register TagPrefix="radt" Namespace="Telerik.WebControls" Assembly="RadTreeView.Net2" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <script language="Javascript" src="./../jscript/generali.js" type="text/javascript"></script>
    <script language="javascript" type="text/javascript">
    	function SelectMe(Me) {
    	    var HIDcheckbox;
    	    //eval('HIDcheckbox= this.document.forms[0].HDazione')
    	    //eval('HIDcheckbox=<%=Me.HDazione.ClientID%>')
            HIDcheckbox = this.document.getElementById('<%=Me.HDazione.ClientID%>');
    	    for (i = 0; i < document.forms[0].length; i++) {
    	        e = document.forms[0].elements[i];
    	        if (e.type == 'checkbox' && e.name.indexOf("CBazione") != -1) {
    	            if (e.checked == true) {
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


    	function UserForCancella(messaggio, messaggioSelezione) {
    	    var Selezionati
    	    Selezionati = DeselezionaNonEliminabili()
    	    //if (document.forms[0].HDazione.value == "," || document.forms[0].HDazione.value == "") {
            if (<%=Me.HDazione.ClientID%>.value == "," || <%=Me.HDazione.ClientID%>.value == "") {
    	        alert(messaggioSelezione);
    	        return false;
    	    }
    	    else {
    	        return confirm(messaggio);
    	    }
    	}

    	//Indica se è stato selezionato almeni un utente !!
    	function UserSelezionati(messaggioSelezione) {
    	    //if (document.forms[0].HDazione.value == "," || document.forms[0].HDazione.value == "") {
            if (<%=Me.HDazione.ClientID%>.value == "," || <%=Me.HDazione.ClientID%>.value == "") {
    	        alert(messaggioSelezione);
    	        return false;
    	    }
    	    else
    	        return true;
    	}
    	function DeselezionaNonEliminabili() {
    	    //eval('HDNnonEliminabili= this.document.forms[0].HDNnonEliminabili')
    	    //eval('HDNnonEliminabili=<%=Me.HDNnonEliminabili.ClientID%>')
            HDNnonEliminabili = this.document.getElementById('<%=Me.HDNnonEliminabili.ClientID%>');
    	    //eval('HDazione= this.document.forms[0].HDazione')
    	    //eval('HDazione=<%=Me.HDazione.ClientID%>')
            HDazione = this.document.getElementById('<%=Me.HDazione.ClientID%>');
    	    for (i = 0; i < document.forms[0].length; i++) {
    	        e = document.forms[0].elements[i];
    	        if (e.type == 'checkbox' && e.name.indexOf("CBazione") != -1) {
    	            if (e.checked == true)
    	                if (HDNnonEliminabili.value.indexOf(',' + e.value + ',') > -1) {
    	                    e.checked = false;
    	                    HDazione.value = HDazione.value.replace("," + e.value + ",", ",")
    	                }
    	        }
    	    }
    	    return false;
    	}


    	function SelectAll(SelectAllBox) {
    	    var actVar = SelectAllBox.checked;
    	    var TBcheckbox;
    	    //eval('HDazione= this.document.forms[0].HDazione')
    	    //eval('HDazione=<%=Me.HDazione.ClientID%>')
            HDazione = this.document.getElementById('<%=Me.HDazione.ClientID%>');
    	    HDazione.value = ""
    	    for (i = 0; i < document.forms[0].length; i++) {
    	        e = document.forms[0].elements[i];
    	        if (e.type == 'checkbox' && e.name.indexOf("CBazione") != -1) {
    	            if (e.disabled == false)
    	                e.checked = actVar;
    	            if (e.checked == true)
    	                if (HDazione.value == "")
    	                    HDazione.value = ',' + e.value + ','
    	                else
    	                    HDazione.value = HDazione.value + e.value + ','
    	            }
    	        }
    	    }

    	function Stampa() {
            OpenWin('./stampaiscritti.aspx?TPRL_id=' + <%=Me.DDLTipoRuolo.ClientID%>.value, '850', '600', 'yes', 'yes')
    	    return false;
    	}

    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="CPHservice" runat="server">
    <table cellspacing="0" cellpadding="0" width="900px" border="0">
        <%--		<tr>
			<td class="RigaTitolo" align="left">
				<asp:label id="LBtitolo" Runat="server">Gestione Iscritti</asp:label>
			</td>
		</tr>--%>
        <tr>
            <td align="right" nowrap="nowrap">
                <table align="right" border="0">
                    <tr>
                        <td nowrap="nowrap" align="right">
                            <asp:Panel ID="PNLmenuPrincipale" runat="server" HorizontalAlign="Right" Wrap="False"
                                Width="550px">
                                <asp:LinkButton ID="LNBgotoGestioneComunita" runat="server" CssClass="LINK_MENU"
                                    CausesValidation="False"></asp:LinkButton>
                                &nbsp;
                                <asp:LinkButton ID="LNBstampa" Visible="false" runat="server" Text="Stampa" CssClass="LINK_MENU"></asp:LinkButton>
                                <asp:LinkButton ID="LNBexcel" Visible="false" runat="server" Text="Esporta in Excel"
                                    CssClass="LINK_MENU"></asp:LinkButton>
                            </asp:Panel>
                        </td>
                        <td nowrap="nowrap" align="right">
                            <asp:Panel ID="PNLmenu" runat="server" HorizontalAlign="Right" Wrap="False" Width="300px">
                                <asp:LinkButton ID="LNBabilita" Visible="False" runat="server" Text="Abilita" CssClass="LINK_MENU"></asp:LinkButton>
                                <asp:LinkButton ID="LNBdisabilita" Visible="False" runat="server" Text="Disabilita"
                                    CssClass="LINK_MENU"></asp:LinkButton>
                                <asp:LinkButton ID="LNBelimina" Visible="False" runat="server" Text="Elimina" CssClass="LINK_MENU"></asp:LinkButton>
                                <asp:LinkButton ID="LNBcancellaInAttesa" Visible="False" runat="server" Text="Elimina in attesa"
                                    CssClass="LINK_MENU"></asp:LinkButton>
                                <asp:Label ID="LBbuoto" runat="server">&nbsp;&nbsp;</asp:Label>
                                <asp:LinkButton ID="LNBiscrivi" Visible="False" runat="server" Text="Iscrivi utente"
                                    CssClass="LINK_MENU"></asp:LinkButton>
                            </asp:Panel>
                            <asp:Panel ID="PNLmenuModifica" runat="server" HorizontalAlign="Right" Wrap="False"
                                Width="250px" Visible="false">
                                <asp:LinkButton ID="LNBannulla" runat="server" CssClass="LINK_MENU" CausesValidation="False"
                                    Text="Annulla"></asp:LinkButton>
                                <asp:LinkButton ID="LNBsalva" Visible="true" runat="server" Text="Salva" CssClass="LINK_MENU"></asp:LinkButton>
                            </asp:Panel>
                            <asp:Panel ID="PNLmenuDeIscrivi" runat="server" Visible="False" HorizontalAlign="Right"
                                Width="250px">
                                <asp:LinkButton ID="LNBannullaDeiscrizione" runat="server" CssClass="LINK_MENU" CausesValidation="False"
                                    Text="Annulla"></asp:LinkButton>
                            </asp:Panel>
                            <asp:Panel ID="PNLmenuDeIscriviMultiplo" runat="server" Visible="False" HorizontalAlign="Right"
                                Width="250px">
                                <asp:LinkButton ID="LNBannulla_multi" runat="server" CssClass="LINK_MENU" CausesValidation="False"
                                    Text="Annulla"></asp:LinkButton>
                                &nbsp;
                                <asp:LinkButton ID="LNBdeIscriviTutte_multi" runat="server" CssClass="LINK_MENU"
                                    CausesValidation="False" Text="Deiscrivi da tutte"></asp:LinkButton>
                                <asp:LinkButton ID="LNBdeIscriviCorrente_multi" runat="server" CssClass="LINK_MENU"
                                    CausesValidation="False" Text="Deiscrivi dalla corrente"></asp:LinkButton>
                            </asp:Panel>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td valign="top" align="center" colspan="2">
                <input id="HDazione" type="hidden" name="HDazione" runat="server" />
                <input id="HDN_totale" type="hidden" name="HDN_totale" runat="server" />
                <input id="HDN_TPCM_ID" type="hidden" name="HDN_TPCM_ID" runat="server" />
                <input id="HDrlpc" type="hidden" name="HDrlpc" runat="server" />
                <input id="HDNprsnID" type="hidden" name="HDNprsnID" runat="server" />
                <input id="HDNrlpc_Attivato" type="hidden" name="HDNrlpc_Attivato" runat="server" />
                <input id="HDNrlpc_Abilitato" type="hidden" name="HDNrlpc_Abilitato" runat="server" />
                <input id="HDNrlpc_Responsabile" type="hidden" name="HDNrlpc_Responsabile" runat="server" />
                <input type="hidden" id="HDNcmnt_ID" runat="server" name="HDNcmnt_ID" />
                <input type="hidden" id="HDNprsn_Id" runat="server" name="HDNprsn_Id" />
                <input type="hidden" id="HDNcmnt_Path" runat="server" name="HDNcmnt_Path" />
                <input type="hidden" id="HDNelencoID" runat="server" name="HDNelencoID" />
                <input type="hidden" id="HDNnonEliminabili" runat="server" name="HDNnonEliminabili" />
                <asp:Panel ID="PNLpermessi" runat="server" Visible="False">
                    <table align="center">
                        <tr>
                            <td height="50">
                                &nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td align="center">
                                <asp:Label ID="LBNopermessi" CssClass="messaggio" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td valign="top" height="50">
                                &nbsp;
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
                <asp:Panel ID="PNLcontenuto" runat="server" HorizontalAlign="Center" Width="900px">
                    <asp:Panel ID="PNLiscritti" runat="server" Visible="true" HorizontalAlign="Center">
                        <table align="left" width="900px">
                            <tr>
                                <td align="Left">
                                    <input type="hidden" runat="server" id="HDNselezionato" name="HDNselezionato" />
                                    <input type="hidden" runat="server" id="HDN_filtroRuolo" name="HDN_filtroRuolo" />
                                    <input type="hidden" runat="server" id="HDN_filtroTipoRicerca" name="HDN_filtroTipoRicerca" />
                                    <input type="hidden" runat="server" id="HDN_filtroValore" name="HDN_filtroValore" />
                                    <input type="hidden" runat="server" id="HDN_filtroIscrizione" name="HDN_filtroIscrizione" />
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
                                                        <asp:TableCell CssClass="FiltroVoceSmall" ColumnSpan="2">
                                                            <table cellspacing="1" cellpadding="1" border="0" width="800px" align="left">
                                                                <tr>
                                                                    <td>
                                                                        &nbsp;&nbsp;
                                                                    </td>
                                                                    <td nowrap="nowrap">
                                                                        <asp:Label runat="server" ID="LBtipoRuolo_t" CssClass="FiltroVoceSmall">Tipo Ruolo:</asp:Label>
                                                                        &nbsp;
                                                                        <asp:DropDownList ID="DDLTipoRuolo" runat="server" CssClass="FiltroCampoSmall" AutoPostBack="true">
                                                                        </asp:DropDownList>
                                                                    </td>
                                                                    <td>
                                                                        &nbsp;&nbsp;
                                                                    </td>
                                                                    <td nowrap="nowrap">
                                                                        <asp:Label runat="server" ID="LBtipoRicerca_t" CssClass="FiltroVoceSmall">Tipo Ricerca:</asp:Label>
                                                                        &nbsp;
                                                                        <asp:DropDownList ID="DDLTipoRicerca" CssClass="FiltroCampoSmall" runat="server"
                                                                            AutoPostBack="false">
                                                                            <asp:ListItem Selected="true" Value="-2">Nome</asp:ListItem>
                                                                            <asp:ListItem Value="-3">Cognome</asp:ListItem>
                                                                            <asp:ListItem Value="-4">Nome/Cognome</asp:ListItem>
                                                                            <asp:ListItem Value="-7">Login</asp:ListItem>
                                                                            <asp:ListItem Value="-6">Matricola</asp:ListItem>
                                                                        </asp:DropDownList>
                                                                    </td>
                                                                    <td nowrap="nowrap">
                                                                        &nbsp;<asp:Label runat="server" ID="LBvalore_t" CssClass="FiltroVoceSmall">Valore:</asp:Label>
                                                                        &nbsp;
                                                                        <asp:TextBox ID="TXBValore" CssClass="FiltroCampoSmall" runat="server" MaxLength="100"
                                                                            Columns="40"></asp:TextBox>
                                                                    </td>
                                                                    <td>
                                                                        &nbsp;&nbsp;
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </asp:TableCell>
                                                    </asp:TableRow>
                                                    <asp:TableRow>
                                                        <asp:TableCell CssClass="FiltroVoceSmall">
                                                            <table cellspacing="1" cellpadding="1" border="0" align="left">
                                                                <tr>
                                                                    <td>
                                                                        &nbsp;&nbsp;
                                                                    </td>
                                                                    <td nowrap="nowrap">
                                                                        <asp:Label ID="LBiscrizione_t" runat="server" CssClass="FiltroVoceSmall">Visualizza:</asp:Label>&nbsp;
                                                                        <asp:DropDownList ID="DDLiscrizione" runat="server" CssClass="FiltroCampoSmall" AutoPostBack="True">
                                                                            <asp:ListItem Value="4">Ultimi iscritti</asp:ListItem>
                                                                            <asp:ListItem Value="-1">Tutti</asp:ListItem>
                                                                            <asp:ListItem Value="1" Selected="true">Abilitati</asp:ListItem>
                                                                            <asp:ListItem Value="0">In attesa di conferma</asp:ListItem>
                                                                            <asp:ListItem Value="2">Bloccati</asp:ListItem>
                                                                        </asp:DropDownList>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </asp:TableCell>
                                                        <asp:TableCell HorizontalAlign="Right">
                                                            <asp:CheckBox ID="CBXautoUpdate" runat="server" Checked="true" AutoPostBack="True"
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
                                                            <asp:Label ID="LBnumeroRecord_t" runat="server" CssClass="Filtro_TestoPaginazione">N° Record</asp:Label>
                                                            <asp:DropDownList ID="DDLNumeroRecord" CssClass="Filtro_RecordPaginazione" runat="server"
                                                                AutoPostBack="true">
                                                                <asp:ListItem Value="15"></asp:ListItem>
                                                                <asp:ListItem Value="30" Selected="true"></asp:ListItem>
                                                                <asp:ListItem Value="45"></asp:ListItem>
                                                                <asp:ListItem Value="50"></asp:ListItem>
                                                                <asp:ListItem Value="70"></asp:ListItem>
                                                                <asp:ListItem Value="100"></asp:ListItem>
                                                            </asp:DropDownList>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </asp:TableCell>
                                        </asp:TableRow>
                                        <asp:TableRow>
                                            <asp:TableCell ColumnSpan="2" HorizontalAlign="Center">
                                                <asp:DataGrid ID="DGiscritti" runat="server" Width="850px" AllowSorting="true" ShowFooter="false"
                                                    AutoGenerateColumns="False" DataKeyField="RLPC_ID" PageSize="50" AllowCustomPaging="True"
                                                    CssClass="DataGrid_Generica">
                                                    <AlternatingItemStyle CssClass="ROW_Alternate_Small"></AlternatingItemStyle>
                                                    <HeaderStyle CssClass="ROW_header_Small_Center"></HeaderStyle>
                                                    <ItemStyle CssClass="ROW_Normal_Small" Height="22px"></ItemStyle>
                                                    <PagerStyle CssClass="ROW_Page_Small" Position="TopAndBottom" Mode="NumericPages"
                                                        Visible="true" HorizontalAlign="Right" Height="18px" VerticalAlign="Bottom">
                                                    </PagerStyle>
                                                    <Columns>
                                                        <asp:BoundColumn DataField="RLPC_ID" HeaderText="RLPC" Visible="false"></asp:BoundColumn>
                                                        <asp:TemplateColumn runat="server" HeaderText="" ItemStyle-HorizontalAlign="Center"
                                                            ItemStyle-Width="10">
                                                            <ItemTemplate>
                                                                <asp:ImageButton ID="IMBCancella" runat="server" CausesValidation="False" CommandName="deiscrivi"
                                                                    ImageUrl="../images/x.gif"></asp:ImageButton>
                                                            </ItemTemplate>
                                                        </asp:TemplateColumn>
                                                        <asp:TemplateColumn runat="server" HeaderText="" ItemStyle-HorizontalAlign="Center"
                                                            ItemStyle-Width="10">
                                                            <ItemTemplate>
                                                                <asp:ImageButton ID="IMBmodifica" runat="server" CausesValidation="False" CommandName="modifica"
                                                                    ImageUrl="../images/m.gif"></asp:ImageButton>
                                                            </ItemTemplate>
                                                        </asp:TemplateColumn>
                                                        <asp:TemplateColumn runat="server" HeaderText="" ItemStyle-HorizontalAlign="Center"
                                                            ItemStyle-Width="10">
                                                            <ItemTemplate>
                                                                <asp:ImageButton ID="IMBinfo" runat="server" CausesValidation="False" CommandName="infoPersona"
                                                                    ImageUrl="../images/proprieta.gif"></asp:ImageButton>
                                                            </ItemTemplate>
                                                        </asp:TemplateColumn>
                                                        <asp:TemplateColumn ItemStyle-CssClass="ROW_TD_Small" HeaderText="Cognome" SortExpression="PRSN_Cognome">
                                                            <ItemTemplate>
                                                                &nbsp;<%#Container.Dataitem("PRSN_Cognome")%>
                                                            </ItemTemplate>
                                                        </asp:TemplateColumn>
                                                        <asp:TemplateColumn ItemStyle-CssClass="ROW_TD_Small" HeaderText="Nome" SortExpression="PRSN_Nome">
                                                            <ItemTemplate>
                                                                &nbsp;<%#Container.Dataitem("PRSN_Nome")%>
                                                            </ItemTemplate>
                                                        </asp:TemplateColumn>
                                                        <asp:BoundColumn DataField="PRSN_Anagrafica" HeaderText="Anagrafica" SortExpression="PRSN_Anagrafica"
                                                            Visible="False"></asp:BoundColumn>
                                                        <asp:TemplateColumn runat="server" HeaderText="Mail" ItemStyle-HorizontalAlign="Center"
                                                            ItemStyle-CssClass="ROW_TD_Small" HeaderStyle-CssClass="ROW_header_Small_Center">
                                                            <ItemTemplate>
                                                                &nbsp;<asp:HyperLink NavigateUrl='<%# "mailto:" & Container.Dataitem("PRSN_mail")%>'
                                                                    Text='<%# Container.Dataitem("PRSN_mail")%>' runat="server" ID="HYPMail" CssClass="ROW_ItemLink_Small" />&nbsp;
                                                            </ItemTemplate>
                                                        </asp:TemplateColumn>
                                                        <asp:BoundColumn DataField="TPRL_id" HeaderText="idRuolo" SortExpression="TPRL_id"
                                                            Visible="False"></asp:BoundColumn>
                                                        <asp:TemplateColumn runat="server" HeaderText="Ruolo" SortExpression="TPRL_nome"
                                                            ItemStyle-CssClass="ROW_TD_Small">
                                                            <ItemTemplate>
                                                                &nbsp;<%# Container.Dataitem("TPRL_nome")%>
                                                            </ItemTemplate>
                                                        </asp:TemplateColumn>
                                                        <asp:BoundColumn DataField="PRSN_TPPR_id" HeaderText="idtipopersona" SortExpression="PRSN_TPPR_id"
                                                            Visible="False"></asp:BoundColumn>
                                                        <asp:BoundColumn DataField="oIscrittoIl" HeaderText="Iscritto il" SortExpression="RLPC_IscrittoIl"
                                                            Visible="true" ItemStyle-Width="120" ItemStyle-CssClass="ROW_TD_Small" HeaderStyle-CssClass="ROW_Header_Small_center"
                                                            ItemStyle-HorizontalAlign="Center"></asp:BoundColumn>
                                                        <asp:BoundColumn DataField="TPRL_gerarchia" Visible="false"></asp:BoundColumn>
                                                        <asp:BoundColumn DataField="PRSN_ID" Visible="false"></asp:BoundColumn>
                                                        <asp:BoundColumn DataField="RLPC_Attivato" Visible="false"></asp:BoundColumn>
                                                        <asp:BoundColumn DataField="RLPC_Abilitato" Visible="false"></asp:BoundColumn>
                                                        <asp:BoundColumn DataField="RLPC_Responsabile" Visible="false"></asp:BoundColumn>
                                                        <asp:BoundColumn DataField="oUltimoCollegamento" HeaderText="Last visit" SortExpression="RLPC_ultimoCollegamento"
                                                            ItemStyle-Width="120" Visible="true" ItemStyle-CssClass="ROW_TD_Small" HeaderStyle-CssClass="ROW_Header_Small_center"
                                                            ItemStyle-HorizontalAlign="Center"></asp:BoundColumn>
                                                        <asp:TemplateColumn ItemStyle-CssClass="ROW_TD_Small" HeaderText="Login" SortExpression="PRSN_login"
                                                            Visible="False">
                                                            <ItemTemplate>
                                                                &nbsp;<%#Container.Dataitem("PRSN_login")%>
                                                            </ItemTemplate>
                                                        </asp:TemplateColumn>
                                                        <asp:TemplateColumn ItemStyle-Width="20px" ItemStyle-HorizontalAlign="Center">
                                                            <HeaderTemplate>
                                                                <input type="checkbox" id="SelectAll" name="SelectAll" onclick="SelectAll(this);"
                                                                    runat="server" />
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <input type="checkbox" value='<%# DataBinder.Eval(Container.DataItem, "PRSN_ID") %>'
                                                                    id="CBazione" name="CBazione" <%# DataBinder.Eval(Container.DataItem, "oCheck") %>
                                                                    onclick="SelectMe(this);" <%# DataBinder.Eval(Container.DataItem, "oCheckDisabled") %>>
                                                            </ItemTemplate>
                                                        </asp:TemplateColumn>
                                                        <asp:BoundColumn DataField="LKPO_Default" Visible="False"></asp:BoundColumn>
                                                    </Columns>
                                                    <PagerStyle Width="800px" PageButtonCount="5" Mode="NumericPages"></PagerStyle>
                                                </asp:DataGrid><br />
                                                <asp:Label ID="LBnoIscritti" Visible="False" runat="server" CssClass="avviso"></asp:Label>
                                            </asp:TableCell>
                                        </asp:TableRow>
                                    </asp:Table>
                                </td>
                            </tr>
                        </table>
                    </asp:Panel>
                    <asp:Panel ID="PNLmodifica" runat="server" Visible="False">
                        <br />
                        <br />
                        <br />
                        <br />
                        <table border="1" align="center" width="400px" cellspacing="0" style="border-color: #CCCCCC;
                            background-color: #fffbf7">
                            <tr>
                                <td align="center" bgcolor="#fffbf7">
                                    <table border="0" align="center">
                                        <tr>
                                            <td colspan="4" height="10px" class="nosize0">
                                                &nbsp;
                                            </td>
                                        </tr>
                                        <tr>
                                            <td width="5px" class="nosize0">
                                                &nbsp;
                                            </td>
                                            <td>
                                                <asp:Label ID="LBanagrafica_t" CssClass="Titolo_campo" runat="server">Anagrafica:&nbsp;</asp:Label>
                                            </td>
                                            <td>
                                                <asp:Label ID="LBNomeCognome" runat="server" CssClass="Testo_campo"></asp:Label>
                                            </td>
                                            <td width="5px" class="nosize0">
                                                &nbsp;
                                            </td>
                                        </tr>
                                        <tr>
                                            <td width="5px" class="nosize0">
                                                &nbsp;
                                            </td>
                                            <td>
                                                <asp:Label ID="LBruolo_t" CssClass="Titolo_campo" runat="server">Ruolo:&nbsp;</asp:Label>
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="DDLruolo" runat="server" CssClass="Testo_campo">
                                                </asp:DropDownList>
                                            </td>
                                            <td width="5px" class="nosize0">
                                                &nbsp;
                                            </td>
                                        </tr>
                                        <tr>
                                            <td width="5px" class="nosize0">
                                                &nbsp;
                                            </td>
                                            <td>
                                                <asp:Label ID="LBresponsabile_t" CssClass="Titolo_campo" runat="server">Responsabile:&nbsp;</asp:Label>
                                            </td>
                                            <td>
                                                <asp:CheckBox ID="CHBresponsabile" runat="server" Text="Si" CssClass="Testo_campo">
                                                </asp:CheckBox>
                                            </td>
                                            <td width="5px" class="nosize0">
                                                &nbsp;
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="4">
                                                &nbsp;
                                            </td>
                                        </tr>
                                    </table>
                                    <br />
                                </td>
                            </tr>
                        </table>
                    </asp:Panel>
                </asp:Panel>
                <asp:Panel ID="PNLdeiscrivi" Visible="False" runat="server">
                    <br />
                    <br />
                    <br />
                    <br />
                    <table border="1" align="center" width="600px" cellspacing="0" style="border-color: #CCCCCC;
                        background-color: #fffbf7">
                        <tr>
                            <td>
                                <table border="0" align="center">
                                    <tr>
                                        <td height="30">
                                            &nbsp;
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="LBinfoDeIscrivi" runat="server" CssClass="confirmDeleteComunita"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            &nbsp;
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="right">
                                            <asp:LinkButton ID="LNBdeIscriviCorrente" runat="server" CssClass="LINK_MENU" CausesValidation="False"
                                                Text="Deiscrivi da corrente"></asp:LinkButton>
                                            <asp:LinkButton ID="LNBdeIscriviSelezionate" runat="server" Text="Dalle selezionate"
                                                CausesValidation="false" CssClass="LINK_MENU"></asp:LinkButton>
                                            <asp:LinkButton ID="LNBdeIscriviTutte" runat="server" CssClass="LINK_MENU" CausesValidation="False"
                                                Text="Deiscrivi da tutte"></asp:LinkButton>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <radt:RadTreeView ID="RDTcomunita" runat="server" align="left" Width="750px" CausesValidation="False"
                                                CssFile="~/RadControls/TreeView/Skins/Comunita/stylesDelete.css" PathToJavaScript="~/Jscript/RadTreeView_Client_3_1.js"
                                                ImagesBaseDir="~/RadControls/TreeView/Skins/Comunita/">
                                            </radt:RadTreeView>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td height="15px">
                                            &nbsp;
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
                <asp:Panel ID="PNLdeiscriviMultiplo" Visible="False" runat="server">
                    <br />
                    <br />
                    <br />
                    <br />
                    <br />
                    <table border="1" align="center" width="600px" cellspacing="0" style="border-color: #CCCCCC;
                        background-color: #fffbf7">
                        <tr>
                            <td align="left" bgcolor="#fffbf7">
                                <br />
                                <asp:Label ID="LBinfoDeIscrivi_multiplo" runat="server" CssClass="confirmDelete"></asp:Label>
                                <br />
                                <br />
                                <br />
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
                <asp:Table ID="TBLexcel" runat="server" Visible="True">
                </asp:Table>
            </td>
        </tr>
    </table>
</asp:Content>
