<%@ Page Language="vb" AutoEventWireup="false" MasterPageFile="~/AjaxPortal.Master" CodeBehind="GestioneIscritti_NEW.aspx.vb" Inherits="Comunita_OnLine.GestioneIscritti_NEW" %>
<%@ MasterType VirtualPath="~/AjaxPortal.Master" %>

<%--<%@ Register TagPrefix="FOOTER" TagName="CtrLFooter" Src="../UC/UC_Footer.ascx" %>
<%@ Register TagPrefix="HEADER" TagName="CtrLHeader" Src="../UC/UC_Header.ascx" %>--%>
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
            //eval('dF_HDazione=<%=Me.HDazione.ClientID%>')
            dF_HDazione = this.document.getElementById('<%=Me.HDazione.ClientID%>');
            var Selezionati
            Selezionati = DeselezionaNonEliminabili()
            //if (document.forms[0].HDazione.value == "," || document.forms[0].HDazione.value == "") {
            if (dF_HDazione.value == "," || dF_HDazione.value == "") {
                alert(messaggioSelezione);
                return false;
            }
            else {
                return confirm(messaggio);
            }
        }

        //Indica se è stato selezionato almeni un utente !!
        function UserSelezionati(messaggioSelezione) {
            //eval('dF_HDazione=<%=Me.HDazione.ClientID%>')
            dF_HDazione = this.document.getElementById('<%=Me.HDazione.ClientID%>');
            if (dF_HDazione.value == "," || dF_HDazione.value == "") {
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
                //OpenWin('./stampaiscritti.aspx?TPRL_id=' + document.forms[0].DDLTipoRuolo.value, '850', '600', 'yes', 'yes')
                OpenWin('./stampaiscritti.aspx?TPRL_id=' + <%=Me.DDLTipoRuolo.ClientID%>.value, '850', '600', 'yes', 'yes')
                return false;
            }



            function SubmitRicerca(event) {
                if (document.all) {
                    if (event.keyCode == 13) {
                        event.returnValue = false;
                        event.cancel = true;
                        try {
                            //document.forms[0].BTNCerca.click();
                            <%=Me.BTNCerca.ClientID%>.click();
                        }
                        catch (ex) {
                            return false;
                        }
                    }
                }
                else if (document.getElementById) {
                    if (event.which == 13) {
                        event.returnValue = false;
                        event.cancel = true;
                        try {
                            //document.forms[0].BTNCerca.click();
                            <%=Me.BTNCerca.ClientID%>.click();
                        }
                        catch (ex) {
                            return false;
                        }
                    }
                }
                else if (document.layers) {
                    if (event.which == 13) {
                        event.returnValue = false;
                        event.cancel = true;
                        try {
                            //document.forms[0].BTNCerca.click();
                            <%=Me.BTNCerca.ClientID%>.click();
                        }
                        catch (ex) {
                            return false;
                        }
                    }
                }
                else
                    return true;
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
							<asp:Panel ID="PNLmenuPrincipale" Runat="server" HorizontalAlign="Right" Wrap="False" width="300px">
								<asp:LinkButton ID="LNBgotoGestioneComunita" Runat="server" CssClass="LINK_MENU" CausesValidation="False"></asp:LinkButton>
								&nbsp;
								<asp:linkbutton id="LNBstampa" Visible="false" Runat="server" Text="Stampa" CssClass="LINK_MENU"></asp:linkbutton>
								<asp:linkbutton id="LNBexcel" Visible="false" Runat="server" Text="Esporta in Excel" CssClass="LINK_MENU"></asp:linkbutton>
							</asp:Panel>
						</td>
						<td nowrap="nowrap" align="right">
							<asp:Panel ID="PNLmenu" Runat="server" HorizontalAlign="Right" Wrap="False" width="300px">
								<asp:linkbutton id="LNBabilita" Visible="False" Runat="server" Text="Abilita" CssClass="LINK_MENU"></asp:linkbutton>
								<asp:linkbutton id="LNBdisabilita" Visible="False" Runat="server" Text="Disabilita" CssClass="LINK_MENU"></asp:linkbutton>
								<asp:linkbutton id="LNBelimina" Visible="False" Runat="server" Text="Elimina" CssClass="LINK_MENU"></asp:linkbutton>
								<asp:linkbutton id="LNBcancellaInAttesa" Visible="False" Runat="server" Text="Elimina in attesa" CssClass="LINK_MENU"></asp:linkbutton>
													
								<asp:Label ID="LBbuoto" runat="server">&nbsp;&nbsp;</asp:Label>
								<asp:linkbutton id="LNBiscrivi" Visible="False" Runat="server" Text="Iscrivi utente" CssClass="LINK_MENU"></asp:linkbutton>
							</asp:Panel>
							<asp:Panel ID="PNLmenuModifica" runat="server" HorizontalAlign="Right" Wrap="False" width="250px" Visible="false">
								<asp:LinkButton ID="LNBannulla" Runat="server" CssClass="LINK_MENU" CausesValidation="False" text="Annulla"></asp:LinkButton>
								<asp:linkbutton id="LNBsalva" Visible="true" Runat="server" Text="Salva" CssClass="LINK_MENU"></asp:linkbutton>
							</asp:Panel>
							<asp:Panel ID="PNLmenuDeIscrivi" runat="server" Visible="False" HorizontalAlign="Right" width="250px">
								<asp:LinkButton ID="LNBannullaDeiscrizione" Runat="server" CssClass="LINK_MENU" CausesValidation="False" text="Annulla"></asp:LinkButton>												
							</asp:Panel>
							<asp:Panel ID="PNLmenuDeIscriviMultiplo" runat="server" Visible="False" HorizontalAlign="Right" width="250px">
								<asp:LinkButton ID="LNBannulla_multi" Runat="server" CssClass="LINK_MENU" CausesValidation="False" text="Annulla"></asp:LinkButton>
								&nbsp;
								<asp:LinkButton ID="LNBdeIscriviTutte_multi" Runat="server" CssClass="LINK_MENU" CausesValidation="False" text="Deiscrivi da tutte"></asp:LinkButton>
								<asp:LinkButton ID="LNBdeIscriviCorrente_multi" Runat="server" CssClass="LINK_MENU" CausesValidation="False" text="Deiscrivi dalla corrente"></asp:LinkButton>
							</asp:Panel>
						</td>
					</tr>
				</table>
			</td>
		</tr>
		<tr>
			<td valign="top" align="center" colspan="2">
				<input id="HDazione" type="hidden" name="HDazione" runat="server"/>
				<input id="HDN_totale" type="hidden" name="HDN_totale" runat="server"/>
				<input id="HDN_TPCM_ID" type="hidden" name="HDN_TPCM_ID" runat="server"/>
									
				<input id="HDrlpc" type="hidden" name="HDrlpc" runat="server"/>
				<input id="HDNprsnID" type="hidden" name="HDNprsnID" runat="server"/>
				<input id="HDNrlpc_Attivato" type="hidden" name="HDNrlpc_Attivato" runat="server"/>
				<input id="HDNrlpc_Abilitato" type="hidden" name="HDNrlpc_Abilitato" runat="server"/>
				<input id="HDNrlpc_Responsabile" type="hidden" name="HDNrlpc_Responsabile" runat="server"/>
									
				<input type="hidden" id="HDNcmnt_ID" runat="server" name="HDNcmnt_ID"/>
									
				<input type="hidden" id="HDNcmnt_Path" runat="server" name="HDNcmnt_Path"/>
				<input type="hidden" id="HDNelencoID" runat="server" name="HDNelencoID"/>
				<input type="hidden" id="HDNnonEliminabili" runat="server" name="HDNnonEliminabili"/>
														
				<asp:panel id="PNLpermessi" Runat="server" Visible="False">
					<table align="center">
						<tr>
							<td height="50">&nbsp;</td>
						</tr>
						<tr>
							<td align="center">
								<asp:Label id="LBNopermessi" CssClass="messaggio" Runat="server"></asp:Label></td>
						</tr>
						<tr>
							<td valign="top" height="50">&nbsp;</td>
						</tr>
					</table>
				</asp:panel>
				<asp:panel id="PNLcontenuto" Runat="server" HorizontalAlign="Center" width="900px">										
					<asp:panel id="PNLiscritti" Runat="server" Visible="true" HorizontalAlign="Center">
						<table align="left" width="900px" >
							<tr>
								<td align="Left">
									<input type="hidden" runat="server" id="HDNselezionato" name="HDNselezionato"/>
									<input type="hidden" runat="server" id="HDN_filtroRuolo" name="HDN_filtroRuolo"/>
									<input type="hidden" runat="server" id="HDN_filtroTipoRicerca" name="HDN_filtroTipoRicerca"/>
									<input type="hidden" runat="server" id="HDN_filtroValore" name="HDN_filtroValore"/>
									<input type="hidden" runat="server" id="HDN_filtroIscrizione" name="HDN_filtroIscrizione"/>
									<asp:Table id="TBLfiltroNew" runat="server"  width="850px" CellPadding="0" CellSpacing="0" GridLines="none" HorizontalAlign="center">
										<asp:TableRow id="TBRchiudiFiltro" Height="22px">
											<asp:TableCell CssClass="Filtro_CellSelezionato" HorizontalAlign="Center" width="150px" Height="22px" VerticalAlign="Middle">
												<asp:LinkButton ID="LNBchiudiFiltro" runat="server" CssClass="Filtro_Link" CausesValidation="False">Chiudi Filtri</asp:LinkButton>
											</asp:TableCell>
											<asp:TableCell CssClass="Filtro_CellDeSelezionato" width="700px" Height="22px">&nbsp;
											</asp:TableCell>
										</asp:TableRow>
										<asp:TableRow id="TBRapriFiltro" Visible="False" Height="22px">
											<asp:TableCell CssClass="Filtro_CellApriFiltro" HorizontalAlign="Center" width="150px" Height="22px">
												<asp:LinkButton ID="LNBapriFiltro" runat="server" CssClass="Filtro_Link" CausesValidation="False">Apri Filtri</asp:LinkButton>
											</asp:TableCell>
											<asp:TableCell CssClass="Filtro_Cellnull" width="700px" Height="22px">&nbsp;
											</asp:TableCell>
										</asp:TableRow>
										<asp:TableRow ID="TBRfiltri" >
											<asp:TableCell CssClass="Filtro_CellFiltri" ColumnSpan="2" HorizontalAlign="center">
												<asp:Table runat="server" ID="TBLfiltro" CellPadding="1" CellSpacing="0" width="800px" HorizontalAlign="center" GridLines="none">
													<asp:TableRow>
														<asp:TableCell CssClass="FiltroVoceSmall" ColumnSpan="2">
															<table cellspacing="1" cellpadding="1"  border="0" width="800px" align="left">
																<tr>
																	<td>&nbsp;&nbsp;</td>
																	<td nowrap="nowrap" >
																		<asp:label Runat="server" ID="LBtipoRuolo_t" CssClass="FiltroVoceSmall">Tipo Ruolo:</asp:label>
																		&nbsp;
																		<asp:dropdownlist id="DDLTipoRuolo" runat="server" CssClass="FiltroCampoSmall" AutoPostBack="true"></asp:dropdownlist>	
																	</td>
																	<td>&nbsp;&nbsp;</td>
																	<td nowrap="nowrap">
																		<asp:label Runat="server" ID="LBtipoRicerca_t" CssClass="FiltroVoceSmall">Tipo Ricerca:</asp:label>
																		&nbsp;
																		<asp:dropdownlist id="DDLTipoRicerca" CssClass="FiltroCampoSmall" Runat="server" AutoPostBack="false">
																			<asp:ListItem Selected="true" Value="-2">Nome</asp:ListItem>
																			<asp:ListItem Value="-3">Cognome</asp:ListItem>
																			<asp:ListItem value="-4">Nome/Cognome</asp:ListItem>
																			<asp:ListItem value="-7">Login</asp:ListItem>
																			<asp:ListItem value="-6">Matricola</asp:ListItem>
																		</asp:dropdownlist>
																	</td>
																	<td nowrap="nowrap">
																		&nbsp;<asp:label Runat="server" ID="LBvalore_t" CssClass="FiltroVoceSmall">Valore:</asp:label>
																		&nbsp;
																		<asp:textbox id="TXBValore" CssClass="FiltroCampoSmall" Runat="server" MaxLength="100" Columns="40" Visible="false"></asp:textbox>
                                                                        <asp:TextBox ID="TXBValue" runat="server"></asp:TextBox>																	
																	</td>
																	<td>&nbsp;&nbsp;</td>
																</tr>
															</table>
														</asp:TableCell>
													</asp:TableRow>
													<asp:TableRow>
														<asp:TableCell CssClass="FiltroVoceSmall" >
															<table cellspacing="1" cellpadding="1"  border="0" align="left">
																<tr>
																	<td>&nbsp;&nbsp;</td>
																	<td nowrap="nowrap" >
																		<asp:Label ID="LBiscrizione_t" Runat="server" CssClass="FiltroVoceSmall">Visualizza:</asp:Label>&nbsp;
																		<asp:dropdownlist ID="DDLiscrizione" runat="server" CssClass="FiltroCampoSmall" AutoPostBack="True">
																			<asp:ListItem Value="4">Ultimi iscritti</asp:ListItem>
																			<asp:ListItem Value="-1">Tutti</asp:ListItem>
																			<asp:ListItem Value="1" Selected="true">Abilitati</asp:ListItem>
																			<asp:ListItem Value="0">In attesa di conferma</asp:ListItem>
																			<asp:ListItem Value="2">Bloccati</asp:ListItem>
																		</asp:dropdownlist>
																	</td>
																</tr>
															</table>
														</asp:TableCell>
														<asp:TableCell HorizontalAlign="Right">
															<asp:CheckBox ID="CBXautoUpdate" runat="server" Checked="true" AutoPostBack="True" CssClass="FiltroCampoSmall" Text="Aggiornamento automatico"></asp:CheckBox>
															&nbsp;&nbsp;
															<asp:button id="BTNCerca" CssClass="PulsanteFiltro" Runat="server" Text="Cerca"></asp:button>
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
																	<td align="center" nowrap="nowrap" >
																		<asp:linkbutton id="LKBtutti" Runat="server" CssClass="lettera" CommandArgument="-1" OnClick="FiltroLinkLettere_Click">Tutti</asp:linkbutton></td>
																	<td align="center" nowrap="nowrap">
																		<asp:linkbutton id="LKBaltro" Runat="server" CssClass="lettera" CommandArgument="0" OnClick="FiltroLinkLettere_Click">Altro</asp:linkbutton></td>
																	<td align="center">
																		<asp:linkbutton id="LKBa" Runat="server" CssClass="lettera" CommandArgument="1" OnClick="FiltroLinkLettere_Click">A</asp:linkbutton></td>
																	<td align="center">
																		<asp:linkbutton id="LKBb" Runat="server" CssClass="lettera" CommandArgument="2" OnClick="FiltroLinkLettere_Click">B</asp:linkbutton></td>
																	<td align="center">
																		<asp:linkbutton id="LKBc" Runat="server" CssClass="lettera" CommandArgument="3" OnClick="FiltroLinkLettere_Click">C</asp:linkbutton></td>
																	<td align="center">
																		<asp:linkbutton id="LKBd" Runat="server" CssClass="lettera" CommandArgument="4" OnClick="FiltroLinkLettere_Click">D</asp:linkbutton></td>
																	<td align="center">
																		<asp:linkbutton id="LKBe" Runat="server" CssClass="lettera" CommandArgument="5" OnClick="FiltroLinkLettere_Click">E</asp:linkbutton></td>
																	<td align="center">
																		<asp:linkbutton id="LKBf" Runat="server" CssClass="lettera" CommandArgument="6" OnClick="FiltroLinkLettere_Click">F</asp:linkbutton></td>
																	<td align="center">
																		<asp:linkbutton id="LKBg" Runat="server" CssClass="lettera" CommandArgument="7" OnClick="FiltroLinkLettere_Click">G</asp:linkbutton></td>
																	<td align="center">
																		<asp:linkbutton id="LKBh" Runat="server" CssClass="lettera" CommandArgument="8" OnClick="FiltroLinkLettere_Click">H</asp:linkbutton></td>
																	<td align="center">
																		<asp:linkbutton id="LKBi" Runat="server" CssClass="lettera" CommandArgument="9" OnClick="FiltroLinkLettere_Click">I</asp:linkbutton></td>
																	<td align="center">
																		<asp:linkbutton id="LKBj" Runat="server" CssClass="lettera" CommandArgument="10" OnClick="FiltroLinkLettere_Click">J</asp:linkbutton></td>
																	<td align="center">
																		<asp:linkbutton id="LKBk" Runat="server" CssClass="lettera" CommandArgument="11" OnClick="FiltroLinkLettere_Click">K</asp:linkbutton></td>
																	<td align="center">
																		<asp:linkbutton id="LKBl" Runat="server" CssClass="lettera" CommandArgument="12" OnClick="FiltroLinkLettere_Click">L</asp:linkbutton></td>
																	<td align="center">
																		<asp:linkbutton id="LKBm" Runat="server" CssClass="lettera" CommandArgument="13" OnClick="FiltroLinkLettere_Click">M</asp:linkbutton></td>
																	<td align="center">
																		<asp:linkbutton id="LKBn" Runat="server" CssClass="lettera" CommandArgument="14" OnClick="FiltroLinkLettere_Click">N</asp:linkbutton></td>
																	<td align="center">
																		<asp:linkbutton id="LKBo" Runat="server" CssClass="lettera" CommandArgument="15" OnClick="FiltroLinkLettere_Click">O</asp:linkbutton></td>
																	<td align="center">
																		<asp:linkbutton id="LKBp" Runat="server" CssClass="lettera" CommandArgument="16" OnClick="FiltroLinkLettere_Click">P</asp:linkbutton></td>
																	<td align="center">
																		<asp:linkbutton id="LKBq" Runat="server" CssClass="lettera" CommandArgument="17" OnClick="FiltroLinkLettere_Click">Q</asp:linkbutton></td>
																	<td align="center">
																		<asp:linkbutton id="LKBr" Runat="server" CssClass="lettera" CommandArgument="18" OnClick="FiltroLinkLettere_Click">R</asp:linkbutton></td>
																	<td align="center">
																		<asp:linkbutton id="LKBs" Runat="server" CssClass="lettera" CommandArgument="19" OnClick="FiltroLinkLettere_Click">S</asp:linkbutton></td>
																	<td align="center">
																		<asp:linkbutton id="LKBt" Runat="server" CssClass="lettera" CommandArgument="20" OnClick="FiltroLinkLettere_Click">T</asp:linkbutton></td>
																	<td align="center">
																		<asp:linkbutton id="LKBu" Runat="server" CssClass="lettera" CommandArgument="21" OnClick="FiltroLinkLettere_Click">U</asp:linkbutton></td>
																	<td align="center">
																		<asp:linkbutton id="LKBv" Runat="server" CssClass="lettera" CommandArgument="22" OnClick="FiltroLinkLettere_Click">V</asp:linkbutton></td>
																	<td align="center">
																		<asp:linkbutton id="LKBw" Runat="server" CssClass="lettera" CommandArgument="23" OnClick="FiltroLinkLettere_Click">W</asp:linkbutton></td>
																	<td align="center">
																		<asp:linkbutton id="LKBx" Runat="server" CssClass="lettera" CommandArgument="24" OnClick="FiltroLinkLettere_Click">X</asp:linkbutton></td>
																	<td align="center">
																		<asp:linkbutton id="LKBy" Runat="server" CssClass="lettera" CommandArgument="25" OnClick="FiltroLinkLettere_Click">Y</asp:linkbutton></td>
																	<td align="center">
																		<asp:linkbutton id="LKBz" Runat="server" CssClass="lettera" CommandArgument="26" OnClick="FiltroLinkLettere_Click">Z</asp:linkbutton></td>
																</tr>
															</table>
														</td>
														<td align="right">
															<asp:label ID="LBnumeroRecord_t" runat="server" cssclass="Filtro_TestoPaginazione">N° Record</asp:label>
															<asp:dropdownlist id="DDLNumeroRecord" CssClass="Filtro_RecordPaginazione" Runat="server" AutoPostBack="true">
																<asp:ListItem Value="15" ></asp:ListItem>
																<asp:ListItem Value="30" Selected="true"></asp:ListItem>
																<asp:ListItem value="45"></asp:ListItem>
																<asp:ListItem value="50"></asp:ListItem>
																<asp:ListItem value="70"></asp:ListItem>
																<asp:ListItem value="100"></asp:ListItem>
															</asp:dropdownlist>
														</td>
													</tr>
												</table>
											</asp:TableCell>
										</asp:TableRow>
										<asp:TableRow>
											<asp:TableCell ColumnSpan="2" HorizontalAlign="Center">
																
                                                <asp:gridview ID="GViscritti" runat="server" 
                                                    Width="900px" Font-Size="11px" BorderColor="#8080FF" PageSize="50" 
                                                    PagerSettings-Position="TopAndBottom" PagerSettings-Mode="Numeric"
                                                    AllowSorting="true" AllowPaging="true" AutoGenerateColumns="False" ShowFooter="false" DataKeyNames="RLPC_ID">
                                                    <Columns>
    
                                                        <asp:TemplateField ItemStyle-CssClass="ROW_TD_Small" >
			                                                <ItemTemplate>
			                                                    <asp:ImageButton ID="ImbInfo" runat="server" ImageUrl="~/images/proprieta.gif" AlternateText="Info" />
			                                                    <asp:ImageButton ID="ImbModifica" runat="server" ImageUrl="~/images/DG/m.gif" AlternateText="Info" />
			                                                    <%--CommandArgument="<%#Container.DataItem.Persona.Id%>" CommandName="Modifica"--%>
			                                                    <asp:ImageButton ID="ImbDeiscrivi" runat="server" ImageUrl="~/images/DG/x.gif" AlternateText="Info" CommandArgument="<%#Container.DataItem.Persona.Id%>"/>
			                                                </ItemTemplate>
		                                                </asp:TemplateField>
		                                                <asp:CommandField ButtonType="Image" EditImageUrl="~/images/DG/m.gif" CancelImageUrl="~/images/DG/x.gif" ShowDeleteButton="true" ShowEditButton="true" ShowCancelButton="true" ShowInsertButton="false" />
		
		                                                <asp:BoundField DataField="RLPC_ID" HeaderText="RLPC" Visible="false" />
		
                                                        <asp:TemplateField ItemStyle-CssClass="ROW_TD_Small" HeaderText="Cognome" SortExpression="Persona.Cognome">
			                                                <ItemTemplate>
				                                                &nbsp;<%#Container.DataItem.Persona.Cognome%>
			                                                </ItemTemplate>
		                                                </asp:TemplateField>
		
		                                                <asp:TemplateField ItemStyle-CssClass="ROW_TD_Small" HeaderText="Nome" SortExpression="Persona.Nome">
			                                                <ItemTemplate>
				                                                &nbsp;<%#Container.Dataitem.Persona.Nome%>
			                                                </ItemTemplate>
		                                                </asp:TemplateField>
		
		                                                <asp:TemplateField ItemStyle-CssClass="ROW_TD_Small" HeaderText="Nome" SortExpression="Persona.Anagrafica" Visible="False">
			                                                <ItemTemplate>
				                                                &nbsp;<%#Container.Dataitem.Persona.Anagrafica%>
			                                                </ItemTemplate>
		                                                </asp:TemplateField>
		
		                                                <asp:TemplateField runat="server" HeaderText="Mail" ItemStyle-HorizontalAlign="Center" 
			                                                ItemStyle-CssClass="ROW_TD_Small" HeaderStyle-CssClass="ROW_header_Small_Center">
			                                                <ItemTemplate>
				                                                &nbsp;<asp:HyperLink NavigateUrl='<%# "mailto:" & Container.Dataitem.Persona.Mail%>' text='<%# Container.Dataitem.Persona.Mail%>' Runat="server" ID="HYPMail" CssClass="ROW_ItemLink_Small" />&nbsp;
			                                                </ItemTemplate>
		                                                </asp:TemplateField>
		
		                                                <asp:TemplateField ItemStyle-CssClass="ROW_TD_Small" HeaderText="Nome" SortExpression="Ruolo.Id" Visible="False">
			                                                <ItemTemplate>
				                                                &nbsp;<%#Container.Dataitem.Ruolo.Id%>
			                                                </ItemTemplate>
		                                                </asp:TemplateField>
		
		                                                <asp:TemplateField runat="server" HeaderText="Ruolo" SortExpression="Ruolo.Nome" ItemStyle-CssClass="ROW_TD_Small">
			                                                <ItemTemplate>
				                                                &nbsp;<%#Container.DataItem.Ruolo.Nome%>
			                                                </ItemTemplate>
		                                                </asp:TemplateField>
		
		                                                <asp:BoundField DataField="RLPC_IscrittoIl" HeaderText="Iscritto il" SortExpression="RLPC_IscrittoIl" Visible="true" ItemStyle-width="120" ItemStyle-CssClass="ROW_TD_Small" headerStyle-CssClass="ROW_Header_Small_center" ItemStyle-HorizontalAlign="Center"></asp:BoundField>
	    
		                                                <asp:TemplateField visible="false">
			                                                <ItemTemplate>
				                                                &nbsp;<%#Container.DataItem.Ruolo.Gerarchia%>
			                                                </ItemTemplate>
		                                                </asp:TemplateField>
		
		                                                <asp:TemplateField visible="false">
			                                                <ItemTemplate>
				                                                &nbsp;<%#Container.DataItem.Persona.Id%>
			                                                </ItemTemplate>
		                                                </asp:TemplateField>
		
		                                                <asp:BoundField DataField="RLPC_attivato" visible="false"></asp:BoundField >
		
		                                                <asp:BoundField DataField="RLPC_abilitato" visible="false"></asp:BoundField >
		
		                                                <asp:BoundField DataField="RLPC_Responsabile" visible="false" ReadOnly="false" ></asp:BoundField >
	    
		                                                <asp:BoundField DataField="RLPC_ultimoCollegamento" HeaderText="Last visit" SortExpression="RLPC_ultimoCollegamento" ItemStyle-width="120" visible="true" ItemStyle-CssClass="ROW_TD_Small" headerStyle-CssClass="ROW_Header_Small_center" ItemStyle-HorizontalAlign="Center"></asp:BoundField >
		
		                                                <asp:TemplateField ItemStyle-CssClass="ROW_TD_Small" HeaderText="Login" SortExpression="PRSN_login" Visible="False">
			                                                <ItemTemplate>
				                                                &nbsp;<%#Container.DataItem.Persona.Login%>
			                                                </ItemTemplate>
		                                                </asp:TemplateField >
		
		                                                <asp:TemplateField  ItemStyle-width="20px" ItemStyle-HorizontalAlign="Center">
			                                                <HeaderTemplate>
				                                                <input type="checkbox" id="SelectAll" name="SelectAll" runat="server"/>
				
				                                                <%--onclick="SelectAll(this);"--%>
			                                                </HeaderTemplate>
			                                                <ItemTemplate>
				                                                <input type="checkbox" value="<%#Container.DataItem.Persona.Id%>" id="CBazione" name="CBazione"  onclick="SelectMe(this);">
	                                                            <%--<%# DataBinder.Eval(Container.DataItem, "oCheck") %>
				                                                <%# DataBinder.Eval(Container.DataItem, "oCheckDisabled") %> --%>
			                                                </ItemTemplate>
		                                                </asp:TemplateField>
		                                                <asp:BoundField DataField="LKPO_Default" Visible="False"/>
		                                                </Columns>
                                                </asp:gridview>

		                                        <asp:Panel ID="PNLmodifica" Runat="server" Visible="False">	
						                            <br/><br/><br/><br/>
						                            <- begin Tbl Modifica int ->
						                            <table border="1" align="center" width="400px" cellspacing="0" style="border-color:#CCCCCC; background-color:#fffbf7"> 
							                            <tr>
								                            <td align="center" bgcolor="#fffbf7">
									                            <table border="0" align="center">
										                            <tr>
											                            <td colspan="4" height="10px" class="nosize0">&nbsp;</td>
										                            </tr>
										                            <tr>
											                            <td width="5px" class="nosize0">&nbsp;</td>
											                            <td>
												                            <asp:Label ID="LBanagrafica_t" CssClass="Titolo_campo" Runat="server">Anagrafica:&nbsp;</asp:Label>
											                            </td>
											                            <td>
												                            <asp:label id="LBNomeCognome" Runat="server" CssClass="Testo_campo"></asp:label>
											                            </td>
											                            <td width="5px" class="nosize0">&nbsp;</td>
										                            </tr>
										                            <tr>
											                            <td width="5px" class="nosize0">&nbsp;</td>
											                            <td>
												                            <asp:Label ID="LBruolo_t" CssClass="Titolo_campo" Runat="server">Ruolo:&nbsp;</asp:Label>
											                            </td>
											                            <td>
												                            <asp:DropDownList id="DDLruolo" Runat="server" CssClass="Testo_campo"></asp:DropDownList>
											                            </td>
											                            <td width="5px" class="nosize0">&nbsp;</td>
										                            </tr>
										                            <tr>
											                            <td width="5px" class="nosize0">&nbsp;</td>
											                            <td>
												                            <asp:Label ID="LBresponsabile_t" CssClass="Titolo_campo" Runat="server">Responsabile:&nbsp;</asp:Label>
											                            </td>
											                            <td>
												                            <asp:CheckBox ID="CHBresponsabile" Runat="server" Text="Si" CssClass="Testo_campo"></asp:CheckBox>                               
											                            </td>
											                            <td width="5px" class="nosize0">&nbsp;</td>
										                            </tr>
										                            <tr>
											                            <td colspan="4">&nbsp;</td>
										                            </tr>
									                            </table>
									                            <- end Tbl Modifica int ->
									                            <br/>
								                            </td>
							                            </tr>
						                            </table>
					                            </asp:Panel>
										        <- end Tbl Modifica ext ->
                                                <br/>
												<asp:Label id="LBnoIscritti" Visible="False" Runat="server" CssClass="avviso"></asp:Label>
											</asp:TableCell>
										</asp:TableRow>
									</asp:Table>
								</td>
							</tr>
						</table>
					</asp:panel>
					<input type="hidden" id="HDNprsn_Id" runat="server" name="HDNprsn_Id"/>
				</asp:panel>
				<asp:Panel ID="PNLdeiscrivi" Visible="False" Runat="server">
					<br/><br/><br/><br/>
					<table border="1" align="center" width="600px" cellspacing="0" style="border-color:#CCCCCC; background-color:#fffbf7"> 
						<tr>
							<td>
								<table border="0" align="center">
									<tr>
										<td height="30">&nbsp;</td>
									</tr>
									<tr>
										<td>
											<asp:Label id="LBinfoDeIscrivi" Runat="server" CssClass="confirmDeleteComunita"></asp:Label>
										</td>
									</tr>
									<tr>
										<td>&nbsp;</td>
									</tr>
									<tr>
										<td align="right">
											<asp:LinkButton ID="LNBdeIscriviCorrente" Runat="server" CssClass="LINK_MENU" CausesValidation="False" text="Deiscrivi da corrente"></asp:LinkButton>
											<asp:linkbutton ID="LNBdeIscriviSelezionate" Runat="server" Text="Dalle selezionate" CausesValidation="false" CssClass="LINK_MENU"></asp:linkbutton>
											<asp:LinkButton ID="LNBdeIscriviTutte" Runat="server" CssClass="LINK_MENU" CausesValidation="False" text="Deiscrivi da tutte"></asp:LinkButton>	
										</td>
									</tr>
									<tr>
										<td>
											<radt:RadTreeView id="RDTcomunita" runat="server" align="left" width="750px" CausesValidation="False" 
												CssFile="~/RadControls/TreeView/Skins/Comunita/stylesDelete.css" PathToJavaScript="~/Jscript/RadTreeView_Client_3_1.js" ImagesBaseDir="~/RadControls/TreeView/Skins/Comunita/" >
											</radt:RadTreeView>
										</td>
									</tr>
									<tr>
										<td height="15px">&nbsp;</td>
									</tr>
								</table>													
							</td>
						</tr>
					</table>
				</asp:Panel>
				<asp:Panel ID="PNLdeiscriviMultiplo" Visible="False" Runat="server">
					<br/><br/><br/><br/><br/>
					<table border="1" align="center" width="600px" cellspacing="0" style="border-color:#CCCCCC; background-color:#fffbf7"> 
						<tr>
							<td align="left" bgcolor="#fffbf7">
								<br/>
								<asp:Label id="LBinfoDeIscrivi_multiplo" Runat="server" CssClass="confirmDelete"></asp:Label>
								<br/><br/><br/>
							</td>
						</tr>
					</table>
				</asp:Panel>
				<asp:Table ID="TBLexcel" runat="server" Visible="True"></asp:Table>
				<br />
				<asp:Label ID="LBLMessage" runat="server" Visible="false"></asp:Label>
				<asp:Label ID="LBLNumRow" runat="server"></asp:Label>
			</td>
		</tr>
	</table>
</asp:Content>

<%--<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Comunità On Line - Gestione Iscritti</title>
    
    <meta name="GENERATOR" content="Microsoft Visual Studio .NET 7.1"/>
	<meta name="CODE_LANGUAGE" content="Visual Basic .NET 7.1"/>
	<meta name="vs_defaultClientScript" content="JavaScript"/>
	<meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5"/>
	
	
	
    <link href="./../Styles.css" type="text/css" rel="stylesheet"/>

</head>

<body>
    <form id="aspnetForm" runat="server">
    <asp:ScriptManager ID="SCMmanager" runat="server"></asp:ScriptManager>
			<table cellspacing="1" cellpadding="1" width="900px" align="center" border="0">
				<tr>
					<td colspan="3"><HEADER:CTRLHEADER id="Intestazione" runat="server"></HEADER:CTRLHEADER></td>
				</tr>
				<tr class="contenitore">
					<td colspan="3">

						<FOOTER:CTRLFOOTER id="Piede" runat="server"></FOOTER:CTRLFOOTER>
					</td>
				</tr>
			</table>
		</form>
</body>
</html>--%>