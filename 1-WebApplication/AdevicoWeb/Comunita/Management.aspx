<%@ Page Language="vb" AutoEventWireup="false" MasterPageFile="~/AjaxPortal.Master" Codebehind="Management.aspx.vb" Inherits="Comunita_OnLine.Management"%>
<%@ MasterType VirtualPath="~/AjaxPortal.Master" %>

<%--
--%>
<%@ Register TagPrefix="radt" Namespace="Telerik.WebControls" Assembly="RadTreeView.Net2" %>
<%@ Register TagPrefix="DETTAGLI" Tagname="CTRLDettagli" Src="../UC/UC_dettaglicomunita.ascx" %>


<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <script language="Javascript" src="./../jscript/generali.js" type="text/javascript"></script>
    <script language=javascript type="text/javascript">
			
		function AggiornaForm(){
			valore = document.forms[0].<%=me.DDLTipoRicerca.ClientID%>.options[document.forms[0].<%=me.DDLTipoRicerca.ClientID%>.selectedIndex].value
			if (valore==-9){
				document.forms[0].<%=me.HDNselezionato.ClientID%>.value = valore
				__doPostBack('DDLTipoRicerca','');
				return true;
				}
			else if (document.forms[0].<%=me.HDNselezionato.ClientID%>.value == -9){
				document.forms[0].<%=me.HDNselezionato.ClientID%>.value = valore
				__doPostBack('DDLTipoRicerca','');
				return true;
				}
			else 
				return false;
		}		
			
		function UpdateAllChildren(nodes, checked){
			var i;
			for (i=0; i<nodes.length; i++){
				if (checked)
					nodes[i].Check()
				else
					nodes[i].UnCheck()

				if (nodes[i].Nodes.length > 0)
					UpdateAllChildren(nodes[i].Nodes, checked);   
			}
		}

		function CheckChildNodes(node){
			if (!node.Checked && node.Parent != null){
				node.Parent.UnCheck();   
				}
			UpdateAllChildren(node.Nodes, node.Checked);
			}
		
            //SubRicerca
	</script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="CPHservice" runat="server">
	<table width="900px" align="center">
<%--		<tr>
			<td Class="RigaTitolo" align=left >
				<asp:Label id="LBtitolo" Runat="server">Gestione sotto comunità</asp:Label>
			</td>
		</tr>--%>
		<tr>
			<td align=right>
				<asp:Panel ID="PNLmenuPrincipale" runat="server" HorizontalAlign=Right>
					<asp:LinkButton id="LNBtreeView" Runat="server" CssClass="LINK_MENU" CausesValidation=False >Visualizza albero sotto comunità</asp:LinkButton>&nbsp;
					<asp:LinkButton ID="LKBcrea" runat="server" Visible =true CssClass="LINK_MENU" CausesValidation=False >Crea SottoComunità</asp:LinkButton>
				</asp:Panel>
				<asp:Panel ID="PNLmenuTree" runat="server" HorizontalAlign=Right Visible=False >
					<asp:LinkButton ID="LNBelenco" runat="server" CssClass="LINK_MENU" CausesValidation=False>Torna all'elenco semplice</asp:LinkButton>
					<asp:LinkButton ID="LNBespandi" runat="server" CssClass="LINK_MENU" CausesValidation=False>Espandi</asp:LinkButton>
					<asp:LinkButton ID="LNBcomprimi" runat="server" CssClass="LINK_MENU" CausesValidation=False>Comprimi</asp:LinkButton>
				</asp:Panel>
				<asp:Panel ID="PNLmenuDettagli" runat="server" HorizontalAlign=Right Visible=False>
					<asp:LinkButton ID="LNBnascondiDettagli" runat="server" CssClass="LINK_MENU" CausesValidation=False>Torna all'elenco</asp:LinkButton>
					<asp:LinkButton ID="LNBentraDettagli" runat="server" CssClass="LINK_MENU" CausesValidation=False>Entra</asp:LinkButton>
					<asp:LinkButton ID="LNBiscriviDettagli" runat="server" CssClass="LINK_MENU" CausesValidation=False>Iscrivi</asp:LinkButton>
				</asp:Panel>
				<asp:Panel ID="PNLmenuCancella" runat="server" HorizontalAlign=Right Visible=False >
					<asp:LinkButton ID="LNBindietro" runat="server" CssClass="LINK_MENU" CausesValidation=False>Torna all'elenco semplice</asp:LinkButton>
					<asp:LinkButton ID="LNBelimina" runat="server" CssClass="LINK_MENU" CausesValidation=False>Espandi</asp:LinkButton>
				</asp:Panel>
			</td>
		</tr>
		<tr>
			<td align="center">
				<input type =hidden runat="server" id="HDNselezionato" name="HDNselezionato"/>
				<input type =hidden runat="server" id="HDNcomunitaSelezionate" name="HDNcomunitaSelezionate"/>
				<input type =hidden runat="server" id="HDN_filtroFacolta" name="HDN_filtroFacolta"/>
				<input type =hidden runat="server" id="HDN_filtroTipoRicerca" name="HDN_filtroTipoRicerca"/>
				<input type =hidden runat="server" id="HDN_filtroValore" name="HDN_filtroValore"/>
				<input type =hidden runat="server" id="HDN_filtroResponsabileID" name="HDN_filtroResponsabileID"/>
				<input type =hidden runat="server" id="HDN_filtroLaureaID" name="HDN_filtroLaureaID"/>
				<input type =hidden runat="server" id="HDN_filtroTipoCdl" name="HDN_filtroTipoCdl"/>
				<input type =hidden runat="server" id="HDN_filtroAnno" name="HDN_filtroAnno"/>
				<input type =hidden runat="server" id="HDN_filtroPeriodo" name="HDN_filtroPeriodo"/>
				<input type =hidden runat="server" id="HDN_filtroTipoComunitaID" name="HDN_filtroTipoComunitaID"/>
				<asp:Panel ID="PNLpermessi" Runat="server" Visible="False" HorizontalAlign="Center">
					<table align="center">
						<tr>
							<td height="50">&nbsp;</td>
						</tr>
						<tr>
							<td align="center">
								<asp:Label id="LBNopermessi" CssClass="messaggio" Runat="server"></asp:Label></td>
						</tr>
						<tr>
							<td height="50">&nbsp;</td>
						</tr>
					</table>
				</asp:Panel>
				<asp:Panel ID="PNLcontenuto" runat="server">
					<asp:panel id="PNLgriglia" Runat="server" HorizontalAlign="Center"> 
						<table width="100%" align="center">
							<tr>
								<td>
														
									<asp:Table id="TBLfiltroNew" runat="server"  Width="900px" CellPadding=0 CellSpacing=0 HorizontalAlign=Center>
										<asp:TableRow id="TBRchiudiFiltro" Height=22px>
											<asp:TableCell CssClass="Filtro_CellSelezionato" HorizontalAlign=Center Width=150px Height=22px VerticalAlign=Middle >
												<asp:LinkButton ID="LNBchiudiFiltro" runat="server" CssClass="Filtro_Link" CausesValidation=False>Chiudi Filtri</asp:LinkButton>
											</asp:TableCell>
											<asp:TableCell CssClass="Filtro_CellDeSelezionato" Width=750px Height=22px>&nbsp;
											</asp:TableCell>
										</asp:TableRow>
										<asp:TableRow id="TBRapriFiltro" Visible=False Height=22px>
											<asp:TableCell ColumnSpan=1 CssClass="Filtro_CellApriFiltro" HorizontalAlign=Center Width=150px Height=22px>
												<asp:LinkButton ID="LNBapriFiltro" runat="server" CssClass="Filtro_Link" CausesValidation=False >Apri Filtri</asp:LinkButton>
											</asp:TableCell>
											<asp:TableCell CssClass="Filtro_Cellnull" Width=750px Height=22px>&nbsp;
											</asp:TableCell>
										</asp:TableRow>
										<asp:TableRow ID="TBRfiltri">
											<asp:TableCell CssClass="Filtro_CellFiltri" ColumnSpan=2 Width=900px HorizontalAlign=center>
												<asp:Table runat="server" ID="TBLfiltro" CellPadding=1 CellSpacing=0 Width="900px" HorizontalAlign=center GridLines=none >
													<asp:TableRow>
														<asp:TableCell>&nbsp;</asp:TableCell>
														<asp:TableCell Height=24px HorizontalAlign=left ColumnSpan=2>
															<table border=0 cellpadding=0 cellspacing=0>
																<tr>
																	<td>
																		<asp:Label ID="LBtipoComunita_c" runat="server" CssClass="FiltroVoceSmall">Tipo Comunita</asp:Label>&nbsp;
																		<asp:dropdownlist id="DDLTipo" runat="server" CssClass="FiltroCampoSmall" AutoPostBack="true"></asp:dropdownlist>
																	</td>
																	<td width=20px>&nbsp;&nbsp;&nbsp;</td>
																	<td Height=22px>
																		<asp:Label ID="LBtipoRicerca_c" runat="server" CssClass="FiltroVoceSmall">Tipo Ricerca</asp:Label>&nbsp;
																		<asp:dropdownlist id="DDLTipoRicerca" Runat="server" CssClass="FiltroCampoSmall">
																			<asp:ListItem Value=-2 Selected="true">Nome</asp:ListItem>
																			<asp:ListItem Value=-7>Nome contiene</asp:ListItem>
																			<asp:ListItem Value=-9>Del responsabile</asp:ListItem>
																			<asp:ListItem Value=-3>Creata dopo il</asp:ListItem>
																			<asp:ListItem Value=-5>Data iscrizione dopo il</asp:ListItem>
																			<asp:ListItem Value=-6>Data fine iscrizione prima del</asp:ListItem>
																		</asp:dropdownlist>
																	</td>
																	<td width=20px>&nbsp;&nbsp;&nbsp;</td>
																	<td Height=22px>
																		<asp:Label ID="LBvalore_c" runat="server" CssClass="FiltroVoceSmall" Visible=true>Valore</asp:Label>
																		&nbsp;
																		<asp:textbox id="TXBValore" Runat="server" CssClass="FiltroCampoSmall" MaxLength=100 Columns=30></asp:textbox>
																		<asp:DropDownList ID="DDLresponsabile" runat="server" AutoPostBack=True CssClass="FiltroCampoSmall" Visible=False></asp:DropDownList>		
																	</td>
																</tr>
															</table>																				
														</asp:TableCell>
													</asp:TableRow>											
													<asp:TableRow runat="server" id="TBRtipiSpeciali">
														<asp:TableCell>&nbsp;</asp:TableCell>
														<asp:TableCell CssClass="FiltroVoceSmall" ID="TBCorganizzazione0" Visible=False Height=22px>
															<asp:Label ID="LBorganizzazione_c" runat="server" CssClass="FiltroVoceSmall">Organizzazione:&nbsp;</asp:Label>&nbsp;
															<asp:DropDownList ID="DDLorganizzazione" runat="server" AutoPostBack=True CssClass="FiltroCampoSmall"></asp:DropDownList>
														</asp:TableCell>
														<asp:TableCell ColumnSpan=2 HorizontalAlign=Left Width=450px>
															<asp:Table ID="TBLcorsi" CellPadding=2 CellSpacing=2 BorderStyle=None runat="server" Visible=False >
																<asp:TableRow>
																	<asp:TableCell>
																		<asp:Label ID="LBannoAccademico_c" runat="server" CssClass="FiltroVoceSmall">A.A.:&nbsp;</asp:Label>
																	</asp:TableCell>
																	<asp:TableCell>
																		<asp:DropDownList ID="DDLannoAccademico" runat="server" AutoPostBack=True CssClass="FiltroCampoSmall"></asp:DropDownList>
																	</asp:TableCell>
																	<asp:TableCell>&nbsp;&nbsp;&nbsp;</asp:TableCell>
																	<asp:TableCell>
																		<asp:Label ID="LBperiodo_c" runat="server" CssClass="FiltroVoceSmall">Periodo:&nbsp;</asp:Label>
																	</asp:TableCell>
																	<asp:TableCell>
																		<asp:DropDownList ID="DDLperiodo" runat="server" AutoPostBack=True CssClass="FiltroCampoSmall"></asp:DropDownList>
																	</asp:TableCell>
																</asp:TableRow>
															</asp:Table>
															<asp:Table ID="TBLcorsiDiStudio" CellPadding=2 CellSpacing=2 BorderStyle=None runat="server" Visible=False HorizontalAlign=Left>
																<asp:TableRow>
																	<asp:TableCell>
																		<asp:Label ID="LBcorsoDiStudi_t" runat="server" CssClass="FiltroVoceSmall">Tipologia:&nbsp;</asp:Label>
																	</asp:TableCell>
																	<asp:TableCell>
																		<asp:DropDownList ID="DDLtipoCorsoDiStudi" runat="server" AutoPostBack=True CssClass="FiltroCampoSmall"></asp:DropDownList>
																	</asp:TableCell>
																</asp:TableRow>
															</asp:Table>
															<asp:Label ID="LBnoCorsi" runat="server" >&nbsp;</asp:Label>
														</asp:TableCell>
													</asp:TableRow>
													<asp:TableRow  Height=30px>
														<asp:TableCell>&nbsp;</asp:TableCell>
														<asp:TableCell HorizontalAlign=left Height=30px ColumnSpan=1>
															<asp:Label ID="LBstatoComunita_t" Runat="server" CssClass="FiltroVoceSmall">Stato:</asp:Label>&nbsp;
															<asp:radiobuttonlist ID="RBLstatoComunita" runat="server" CssClass="FiltroCampoSmall" AutoPostBack=True RepeatDirection=Horizontal RepeatLayout=Flow >
																<asp:ListItem Value=-1>Tutte</asp:ListItem>
																<asp:ListItem Value=0 Selected=true>Attivate</asp:ListItem>
																<asp:ListItem Value=1>Archiviate</asp:ListItem>
																<asp:ListItem Value=2>Bloccate</asp:ListItem>
															</asp:radiobuttonlist>
														</asp:TableCell>
														<asp:TableCell HorizontalAlign=right Height=30px>
															<asp:CheckBox ID="CBXautoUpdate" runat="server" AutoPostBack=True CssClass="FiltroCampoSmall" Text="Aggiornamento automatico" Checked=True ></asp:CheckBox>&nbsp;&nbsp;
															<asp:button id=BTNCerca Runat="server" CssClass="PulsanteFiltro" Text="Cerca"></asp:button>&nbsp;
														</asp:TableCell>
													</asp:TableRow>
												</asp:Table>
											</asp:TableCell>
										</asp:TableRow>
										<asp:TableRow Visible=true >
											<asp:TableCell ColumnSpan=2 Width=900px HorizontalAlign=Center>
												<table cellpadding=0 cellspacing=0 align=center Width=900px border=0>
													<tr>
														<td>
															<table align="left" width=500px>
																<tr>
																	<td align="center" nowrap="nowrap" >
																		<asp:linkbutton id="LKBtutti" Runat="server" CssClass="lettera" CommandArgument="-1" OnClick="FiltroLink_Click">Tutti</asp:linkbutton></td>
																	<td align="center" nowrap="nowrap">
																		<asp:linkbutton id="LKBaltro" Runat="server" CssClass="lettera" CommandArgument="0" OnClick="FiltroLink_Click">Altro</asp:linkbutton></td>
																	<td align="center">
																		<asp:linkbutton id="LKBa" Runat="server" CssClass="lettera" CommandArgument="1" OnClick="FiltroLink_Click">A</asp:linkbutton></td>
																	<td align="center">
																		<asp:linkbutton id="LKBb" Runat="server" CssClass="lettera" CommandArgument="2" OnClick="FiltroLink_Click">B</asp:linkbutton></td>
																	<td align="center">
																		<asp:linkbutton id="LKBc" Runat="server" CssClass="lettera" CommandArgument="3" OnClick="FiltroLink_Click">C</asp:linkbutton></td>
																	<td align="center">
																		<asp:linkbutton id="LKBd" Runat="server" CssClass="lettera" CommandArgument="4" OnClick="FiltroLink_Click">D</asp:linkbutton></td>
																	<td align="center">
																		<asp:linkbutton id="LKBe" Runat="server" CssClass="lettera" CommandArgument="5" OnClick="FiltroLink_Click">E</asp:linkbutton></td>
																	<td align="center">
																		<asp:linkbutton id="LKBf" Runat="server" CssClass="lettera" CommandArgument="6" OnClick="FiltroLink_Click">F</asp:linkbutton></td>
																	<td align="center">
																		<asp:linkbutton id="LKBg" Runat="server" CssClass="lettera" CommandArgument="7" OnClick="FiltroLink_Click">G</asp:linkbutton></td>
																	<td align="center">
																		<asp:linkbutton id="LKBh" Runat="server" CssClass="lettera" CommandArgument="8" OnClick="FiltroLink_Click">H</asp:linkbutton></td>
																	<td align="center">
																		<asp:linkbutton id="LKBi" Runat="server" CssClass="lettera" CommandArgument="9" OnClick="FiltroLink_Click">I</asp:linkbutton></td>
																	<td align="center">
																		<asp:linkbutton id="LKBj" Runat="server" CssClass="lettera" CommandArgument="10" OnClick="FiltroLink_Click">J</asp:linkbutton></td>
																	<td align="center">
																		<asp:linkbutton id="LKBk" Runat="server" CssClass="lettera" CommandArgument="11" OnClick="FiltroLink_Click">K</asp:linkbutton></td>
																	<td align="center">
																		<asp:linkbutton id="LKBl" Runat="server" CssClass="lettera" CommandArgument="12" OnClick="FiltroLink_Click">L</asp:linkbutton></td>
																	<td align="center">
																		<asp:linkbutton id="LKBm" Runat="server" CssClass="lettera" CommandArgument="13" OnClick="FiltroLink_Click">M</asp:linkbutton></td>
																	<td align="center">
																		<asp:linkbutton id="LKBn" Runat="server" CssClass="lettera" CommandArgument="14" OnClick="FiltroLink_Click">N</asp:linkbutton></td>
																	<td align="center">
																		<asp:linkbutton id="LKBo" Runat="server" CssClass="lettera" CommandArgument="15" OnClick="FiltroLink_Click">O</asp:linkbutton></td>
																	<td align="center">
																		<asp:linkbutton id="LKBp" Runat="server" CssClass="lettera" CommandArgument="16" OnClick="FiltroLink_Click">P</asp:linkbutton></td>
																	<td align="center">
																		<asp:linkbutton id="LKBq" Runat="server" CssClass="lettera" CommandArgument="17" OnClick="FiltroLink_Click">Q</asp:linkbutton></td>
																	<td align="center">
																		<asp:linkbutton id="LKBr" Runat="server" CssClass="lettera" CommandArgument="18" OnClick="FiltroLink_Click">R</asp:linkbutton></td>
																	<td align="center">
																		<asp:linkbutton id="LKBs" Runat="server" CssClass="lettera" CommandArgument="19" OnClick="FiltroLink_Click">S</asp:linkbutton></td>
																	<td align="center">
																		<asp:linkbutton id="LKBt" Runat="server" CssClass="lettera" CommandArgument="20" OnClick="FiltroLink_Click">T</asp:linkbutton></td>
																	<td align="center">
																		<asp:linkbutton id="LKBu" Runat="server" CssClass="lettera" CommandArgument="21" OnClick="FiltroLink_Click">U</asp:linkbutton></td>
																	<td align="center">
																		<asp:linkbutton id="LKBv" Runat="server" CssClass="lettera" CommandArgument="22" OnClick="FiltroLink_Click">V</asp:linkbutton></td>
																	<td align="center">
																		<asp:linkbutton id="LKBw" Runat="server" CssClass="lettera" CommandArgument="23" OnClick="FiltroLink_Click">W</asp:linkbutton></td>
																	<td align="center">
																		<asp:linkbutton id="LKBx" Runat="server" CssClass="lettera" CommandArgument="24" OnClick="FiltroLink_Click">X</asp:linkbutton></td>
																	<td align="center">
																		<asp:linkbutton id="LKBy" Runat="server" CssClass="lettera" CommandArgument="25" OnClick="FiltroLink_Click">Y</asp:linkbutton></td>
																	<td align="center">
																		<asp:linkbutton id="LKBz" Runat="server" CssClass="lettera" CommandArgument="26" OnClick="FiltroLink_Click">Z</asp:linkbutton></td>
																</tr>
															</table>
														</td>
														<td align=right >
															<asp:label ID="LBnumeroRecord_c" Runat =server cssclass="Filtro_TestoPaginazione">N° Record</asp:label>
															<asp:dropdownlist id="DDLNumeroRecord" CssClass="Filtro_RecordPaginazione" Runat="server" AutoPostBack="true">
																<asp:ListItem Value="15" ></asp:ListItem>
																<asp:ListItem Value="30" Selected="true"></asp:ListItem>
																<asp:ListItem value="45"></asp:ListItem>
																<asp:ListItem value="50"></asp:ListItem>
																<asp:ListItem value="80"></asp:ListItem>
																<asp:ListItem value="100"></asp:ListItem>
															</asp:dropdownlist>
														</td>
													</tr>
												</table>	
											</asp:TableCell>
										</asp:TableRow>
									</asp:Table>
									<asp:datagrid id="DGComunita" 
										runat="server" DataKeyField="CMNT_id" 
										AllowPaging=true AutoGenerateColumns="False"
										AllowSorting="true" 
										ShowFooter="false" 
										PageSize=30
										CssClass="DataGrid_Generica">
										<HeaderStyle CssClass="ROW_header_Small_Center"></HeaderStyle>
										<ItemStyle CssClass="ROW_Normal_Small" Height="22px"></ItemStyle>
										<PagerStyle CssClass="ROW_Page_Small" Position=TopAndBottom Mode="NumericPages" Visible="true"
										HorizontalAlign="Right" Height="18px" VerticalAlign="Bottom"></PagerStyle>
										<Columns>
										<asp:TemplateColumn ItemStyle-Width="45px">
											<ItemTemplate>
												<table>
													<tr>
														<td align=center >
															<asp:ImageButton ID="IMGDettagli" Commandname="dettagli" ImageUrl="../images/proprieta.gif" Runat="server"></asp:ImageButton>		
														</td>
													</tr>
													<tr>
														<td>
															<asp:ImageButton id="IMGEdit" Commandname="modifica" ImageUrl="../images/m.gif" Runat="server"></asp:ImageButton>
															<asp:ImageButton ID="IMGDelete" Commandname="cancella" ImageUrl="../images/elimina.gif" Runat="server"></asp:ImageButton>
														</td>
													</tr>
												</table>												
											</ItemTemplate>
										</asp:TemplateColumn>
											<asp:TemplateColumn HeaderText="Tipo" ItemStyle-Width="50" SortExpression="TPCM_Descrizione" ItemStyle-CssClass=ROW_TD_Small_center HeaderStyle-CssClass="ROW_header_Small_Center">
												<ItemTemplate>
													<img runat="server" src='<%# DataBinder.Eval(Container.DataItem, "TPCM_icona") %>' alt='<%# DataBinder.Eval(Container.DataItem, "TPCM_descrizione") %>' align="middle" ID="Img1"/>
												</ItemTemplate>
											</asp:TemplateColumn>
											<asp:TemplateColumn  HeaderText="Nome" SortExpression="CMNT_Nome" ItemStyle-CssClass=ROW_TD_Small HeaderStyle-CssClass="ROW_header_Small_Center">
												<ItemTemplate>
													<asp:Table ID="TBLdati" runat="server">
														<asp:TableRow ID="TBRnome">
															<asp:TableCell>
																<asp:Image ID="IMGisChiusa" runat="server" Visible=False BorderStyle=None ></asp:Image>
															</asp:TableCell>
															<asp:TableCell CssClass=ROW_TD_Small ID="TBCnome">
																<%# DataBinder.Eval(Container.DataItem, "CMNT_Nome") %> &nbsp;(<asp:LinkButton ID="LNBiscrivi" runat="server" CausesValidation=False Commandname="iscrivi">Iscrivimi</asp:LinkButton><asp:LinkButton ID="LNBentra" runat="server" CausesValidation=False Commandname="entra">Entra</asp:LinkButton>)
															</asp:TableCell>
														</asp:TableRow>
														<asp:TableRow>
															<asp:TableCell>&nbsp;</asp:TableCell>
															<asp:TableCell>
																<asp:Label id="LBresponsabili" runat="server"></asp:Label>
															</asp:TableCell>
														</asp:TableRow>
														<asp:TableRow>
															<asp:TableCell>
																&nbsp;
															</asp:TableCell>
															<asp:TableCell>
																<asp:LinkButton ID="LNBiscritti" runat="server" CausesValidation=False Commandname="iscritti">Gestione Iscritti</asp:LinkButton>
																&nbsp;|&nbsp;
																<asp:LinkButton ID="LNBassociaPadri" runat="server" CausesValidation=False Commandname="associa">Associa A..</asp:LinkButton>
																&nbsp;|&nbsp;
																<asp:LinkButton ID="LNBservizi" runat="server" CausesValidation=False Commandname="servizi">Servizi</asp:LinkButton>
																<asp:Label ID="LBcoordina" runat="server" Visible=False >&nbsp;|&nbsp;</asp:Label>
																<asp:LinkButton ID="LNBcoordina" runat="server" CausesValidation=False Commandname="coordina" Visible=False>Coordina</asp:LinkButton>
																&nbsp;|&nbsp;
																<asp:LinkButton ID="LNBblocca" runat="server" CausesValidation=False Commandname="blocca" Visible=False>Blocca</asp:LinkButton>
																<asp:LinkButton ID="LNBsblocca" runat="server" CausesValidation=False Commandname="sblocca" Visible=False>Sblocca</asp:LinkButton>
																&nbsp;|&nbsp;
																<asp:LinkButton ID="LNBarchivia" runat="server" CausesValidation=False Commandname="archivia" Visible=False>Archivia</asp:LinkButton>
																<asp:LinkButton ID="LNBdeArchivia" runat="server" CausesValidation=False Commandname="deArchivia" Visible=False>De-Archivia</asp:LinkButton>
															</asp:TableCell>
														</asp:TableRow>
													</asp:Table>
												</ItemTemplate>
											</asp:TemplateColumn>
											<asp:BoundColumn DataField="AnagraficaCreatore" HeaderText="Creatore" SortExpression="AnagraficaCreatore" ItemStyle-CssClass=ROW_TD_Small Visible=False></asp:BoundColumn>
											<asp:BoundColumn DataField="CMNT_Responsabile" HeaderText="Responsabile"  ItemStyle-CssClass=ROW_TD_Small HeaderStyle-CssClass="ROW_header_Small_Center" Visible=False></asp:BoundColumn>
											<asp:BoundColumn DataField="CMNT_dataCreazioneText" HeaderText="Creata il" sortExpression="CMNT_dataCreazione" ItemStyle-CssClass=ROW_TD_Small_Center Visible=True ItemStyle-Width="100"></asp:BoundColumn>
											<asp:BoundColumn DataField="CMNT_dataCessazioneText" HeaderText="Cessazione" ItemStyle-CssClass=ROW_TD_Small_Center ItemStyle-Width="100" Visible=False ></asp:BoundColumn>
											<asp:BoundColumn DataField="ALCM_isDiretto" HeaderText="Link" Visible=False ></asp:BoundColumn>
											<asp:ButtonColumn ButtonType=LinkButton Text="Accedi" Commandname="accedi" Visible=False  ItemStyle-CssClass=ROW_TD_Small></asp:ButtonColumn>
											<asp:BoundColumn DataField="ALCM_Path" Visible="False"></asp:BoundColumn>
											<asp:BoundColumn DataField="ALCM_isDiretto" Visible=False ></asp:BoundColumn>
											<asp:BoundColumn DataField="ALCM_PadreVirtuale_ID"  Visible=False ></asp:BoundColumn>
											<asp:BoundColumn DataField="CMNT_PRSN_ID"  Visible=False ></asp:BoundColumn>
											<asp:BoundColumn DataField="RLPC_TPRL_ID"  Visible=False ></asp:BoundColumn>
											<asp:BoundColumn DataField="ALCM_HasFigli"  Visible=False ></asp:BoundColumn>
											<asp:BoundColumn DataField="CMNT_IsChiusa" Visible=False ></asp:BoundColumn>
											<asp:BoundColumn DataField="ALCM_isChiusaForPadre" Visible=False ></asp:BoundColumn>
											<asp:BoundColumn DataField="CMNT_TPCM_ID" Visible=False ></asp:BoundColumn>
											<asp:BoundColumn DataField="CMNT_Archiviata" Visible=False ></asp:BoundColumn>
											<asp:BoundColumn DataField="CMNT_Bloccata" Visible=False ></asp:BoundColumn>
										</Columns>
									</asp:datagrid><br/>
									<asp:Label id="LBmsgDG" CssClass="avviso_normal" Runat="server" Visible="False"></asp:Label>
								</td>
							</tr>
						</table>
					</asp:panel>
					<asp:Panel id="PNLtreeView" HorizontalAlign="center" Runat="server" Visible=False >
						<table align="left" width="100%">
							<tr>
								<td colspan=2>
									<asp:Label ID="LBcancellazione" runat="server" CssClass="errore"></asp:Label>
								</td>
							</tr>
							<tr>
								<td align="left" colspan="2">
									<radt:RadTreeView id="RDTcomunita" runat="server" align="left" width="100%"
											CausesValidation="False"  ContextMenuContentFile="~/RadControls/TreeView/Skins/Comunita/ContextMenus.xml"
										CssFile="~/RadControls/TreeView/Skins/Comunita/styles.css" PathToJavaScript="~/Jscript/RadTreeView_Client_3_1.js"
										ImagesBaseDir="~/RadControls/TreeView/Skins/Comunita/" skin="Comunita"  AfterClientCheck="CheckChildNodes">
									</radt:RadTreeView>
								</td>
							</tr>
							<tr>
								<td colspan=2>&nbsp;</td>
							</tr>
						</table>
					</asp:Panel>
					<asp:panel id="PNLdettagli" Runat="server" HorizontalAlign="Center" Visible="false">
						<table width=600 align=center border=0>
							<tr>
								<td align=center colspan=2>
									<FIELDSET>
									<LEGEND class=tableLegend>
									<asp:Label ID="LBlegendaDettagli" runat="server">Dettagli comunità</asp:Label>
									</LEGEND>
									<DETTAGLI:CTRLDettagli id="CTRLDettagli" runat="server"></DETTAGLI:CTRLDettagli>														
								</FIELDSET> 
								</td>
							</tr>
						</table>
					</asp:panel>
					<asp:Panel ID="PNLconfermaElimina" runat="server" HorizontalAlign=Center Visible=False>
						<table align=center>
							<tr>
								<td colspan=2 align=left>
									<br/><br/><br/>
									<asp:Label ID="LBconfermaElimina" runat="server" CssClass="avviso_normal"></asp:Label>
								</td>
							</tr>
							<tr>
								<td colspan=2>&nbsp;</td>
							</tr>
						</table>
					</asp:Panel>
					<asp:Panel ID="PNLdettagliElimina" runat="server" HorizontalAlign=Center Visible=False>
						<table align=center border=1>
							<tr>
								<td colspan=2>&nbsp;</td>
							</tr>
							<tr>
								<td colspan=2 align=left>
									<br/><br/><br/>
									<asp:Label ID="LBdettagliElimina" runat="server" CssClass="confirmDelete"></asp:Label>
								</td>
							</tr>
							<tr>
								<td colspan=2>&nbsp;</td>
							</tr>
						</table>
					</asp:Panel>
				</asp:Panel>
			</td>
		</tr>
	</table>
	<input type="hidden" id="HDNcmnt_ID" runat="server" name="HDNcmnt_ID"/>
	<input type="hidden" id="HDN_Path" runat="server" name="HDN_Path"/>
	<input type="hidden" id="HDNreturnTo" runat="server" name="HDNreturnTo"/>
	<input type="hidden" id="HDN_isDiretto" runat="server" name="HDN_isDiretto"/>
	<input type="hidden" id="HDN_idPadre_Link" runat="server" name="HDN_idPadre_Link"/>
	<input type="hidden" id="HDN_nodoSel" runat="server" name="HDN_nodoSel"/>
	<input type="hidden" id="HDN_nodoPadreSel" runat="server" name="HDN_nodoPadreSel"/>
</asp:Content>

<%--
<!DOCTYPE HTML PUBLIC "-//W3C//Dtd HTML 4.0 transitional//EN">
<HTML>
	<head runat="server">
		<title>Comunità On Line - Management Comunità</title>
		<meta name="GENERATOR" content="Microsoft Visual Studio .NET 7.1"/>
		<meta name="CODE_LANGUAGE" content="Visual Basic .NET 7.1"/>
		<meta name="vs_defaultClientScript" content="JavaScript"/>
		<meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5"/>
		<LINK href="./../Styles.css" type="text/css" rel="stylesheet"/>
			
	</HEAD>

	<body onkeydown="return SubmitRicerca(event);">
		<form id="aspnetForm" method="post" runat="server">
		<asp:ScriptManager ID="SCMmanager" runat="server"></asp:ScriptManager>
			<table cellspacing="0" cellpadding="0"  align="center" border="0" width="900px">
				<tr>
				    <td colspan="3" >
				    <div>
				        <HEADER:CtrLHEADER id="Intestazione" runat="server" ShowNews="false"></HEADER:CtrLHEADER>	
				    </div>
				    </td>
			    </tr>
				<tr>
					<td colSpan="3" align="center">

					</td>
				</tr>
			</table>
			<FOOTER:CtrLFOOTER id="CtrLFooter" runat="server"></FOOTER:CtrLFOOTER>
		</form>
	</body>
</HTML>
--%>