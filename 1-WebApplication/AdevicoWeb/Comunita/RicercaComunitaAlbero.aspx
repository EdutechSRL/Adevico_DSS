<%@ Page Language="vb" AutoEventWireup="false" MasterPageFile="~/AjaxPortal.Master" Codebehind="RicercaComunitaAlbero.aspx.vb" Inherits="Comunita_OnLine.RicercaComunitaAlbero"%>
<%@ MasterType VirtualPath="~/AjaxPortal.Master" %>


<%@ Register TagPrefix="radt" Namespace="Telerik.WebControls" Assembly="RadTreeView.Net2" %>
<%@ Register TagPrefix="DETTAGLI" TagName="CTRLDettagli" Src="../UC/UC_dettaglicomunita.ascx" %>

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
			
			
		function ExpandAll(){
			var i;
			for (i=0; i<RDTcomunita.AllNodes.length; i++){
				var node = RDTcomunita.AllNodes[i];
				if (node.Nodes.length > 0){  
					node.Expand();
				}
			} 
		}
				 
		function CollapseAll(){
			var i;
			for (i=0; i<RDTcomunita.AllNodes.length; i++){
				var node = RDTcomunita.AllNodes[i];
				if (node.Nodes.length > 0){  
					node.Collapse();
				}
			} 
		}
			

	</script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="CPHservice" runat="server">
	<table align="center" width="900px">
		<tr>
			<td align=right>
				<asp:Panel ID="PNLmenu" Runat=server HorizontalAlign=Right Visible=true>
					<asp:linkbutton ID="LNBlista" Runat="server" Text="Lista comunità" CausesValidation="false" CssClass="LINK_MENU"></asp:linkbutton>
					<asp:linkbutton ID="LNBalbero" Runat="server" Text="Albero comunità" CausesValidation="false" CssClass="LINK_MENU" Visible=False ></asp:linkbutton>
					<asp:linkbutton ID="LNBalberoGerarchico" Runat="server" Text="Albero gerarchico" CausesValidation="false" CssClass="LINK_MENU" Visible=False></asp:linkbutton>
				</asp:Panel>
				<asp:Panel ID="PNLmenuDettagli" Runat="server" HorizontalAlign=Right Visible=False >
					<asp:linkbutton ID="LNBannullaDettagli" Runat="server" Text="Torna all'elenco" CausesValidation="false" CssClass="LINK_MENU"></asp:linkbutton>
					<asp:linkbutton ID="LNBentra" Runat="server" CssClass="LINK_MENU" CausesValidation="True" text="Entra" Visible=False></asp:linkbutton>
					<asp:linkbutton ID="LNBiscrivi" Runat="server" CssClass="LINK_MENU" CausesValidation="True" text="Iscrivi" Visible=False ></asp:linkbutton>
				</asp:Panel>
				<asp:Panel ID="PNLmenuMessaggio" Runat=server HorizontalAlign=Right Visible=False>
					<asp:linkbutton ID="LNBannullaMessaggio" Runat="server" Text="Torna all'elenco" CausesValidation="false" CssClass="LINK_MENU"></asp:linkbutton>
				</asp:Panel>
				<asp:Panel ID="PNLmenuConferma" Runat="server" HorizontalAlign=Right Visible=False >
					<asp:linkbutton ID="LNBannullaConferma" Runat="server" Text="Torna all'elenco" CausesValidation="false" CssClass="LINK_MENU"></asp:linkbutton>
					<asp:linkbutton ID="LNBiscriviConferma" Runat="server" CssClass="LINK_MENU" CausesValidation="True" text="Iscrivi"></asp:linkbutton>
				</asp:Panel>
			</td>
		</tr>
		<tr>
			<td align=center >
				<asp:Panel ID="PNLcontenuto" Runat="server" HorizontalAlign="Center">										
					<table align="left" width="100%" cellpadding=0 cellspacing=0 border=0>
						<tr>
							<td align="center">
								<input type =hidden runat=server id="HDNselezionato" name="HDNselezionato"/>
								<input type =hidden runat=server id="HDN_filtroFacolta" name="HDN_filtroFacolta"/>
								<input type =hidden runat=server id="HDN_filtroTipoRicerca" name="HDN_filtroTipoRicerca"/>
								<input type =hidden runat=server id="HDN_filtroValore" name="HDN_filtroValore"/>
								<input type =hidden runat=server id="HDN_filtroResponsabileID" name="HDN_filtroResponsabileID"/>
								<input type =hidden runat=server id="HDN_filtroLaureaID" name="HDN_filtroLaureaID"/>
								<input type =hidden runat=server id="HDN_filtroTipoCdl" name="HDN_filtroTipoCdl"/>
								<input type =hidden runat=server id="HDN_filtroAnno" name="HDN_filtroAnno"/>
								<input type =hidden runat=server id="HDN_filtroPeriodo" name="HDN_filtroPeriodo"/>
								<input type =hidden runat=server id="HDN_filtroTipoComunitaID" name="HDN_filtroTipoComunitaID"/>
								<input type =hidden runat=server id="HDN_filtroRicercaByIscrizione" name="HDN_filtroRicercaByIscrizione"/>
								<input type =hidden runat=server id="HDN_filtroStatus" name="HDN_filtroStatus"/>
													
								<INPUT id="HDNcmnt_ID" type="hidden" name="HDNcmnt_ID" runat="server"/>
								<INPUT id="HDN_Path" type="hidden" name="HDN_Path" runat="server"/>
								<INPUT id="HDNisChiusa" type="hidden" name="HDNisChiusa" runat="server"/>
								<asp:Panel id="PNLtreeView" HorizontalAlign="center" Runat="server">
									<asp:Table Runat=server ID="Table1" CellPadding=0 CellSpacing=0>
										<asp:TableRow ID="TBRfiltro">
											<asp:TableCell ColumnSpan=2 HorizontalAlign=Center>														
												<asp:Table id="TBLfiltroNew" Runat=server  Width="900px" CellPadding=0 CellSpacing=0>
													<asp:TableRow id="TBRchiudiFiltro" Height=22px>
														<asp:TableCell CssClass="Filtro_CellSelezionato" HorizontalAlign=Center Width=150px Height=22px VerticalAlign=Middle >
															<asp:LinkButton ID="LNBchiudiFiltro" Runat=server CssClass="Filtro_Link" CausesValidation=False>Chiudi Filtri</asp:LinkButton>
														</asp:TableCell>
														<asp:TableCell CssClass="Filtro_CellDeSelezionato" Width=750px Height=22px HorizontalAlign=Right >&nbsp;
															&nbsp;
														</asp:TableCell>
													</asp:TableRow>
													<asp:TableRow id="TBRapriFiltro" Visible=False Height=22px>
														<asp:TableCell ColumnSpan=1 CssClass="Filtro_CellApriFiltro" HorizontalAlign=Center Width=150px Height=22px>
															<asp:LinkButton ID="LNBapriFiltro" Runat=server CssClass="Filtro_Link" CausesValidation=False >Apri Filtri</asp:LinkButton>
														</asp:TableCell>
														<asp:TableCell CssClass="Filtro_Cellnull" Width=750px Height=22px>&nbsp;
														</asp:TableCell>
													</asp:TableRow>
													<asp:TableRow ID="TBRfiltri">
														<asp:TableCell CssClass="Filtro_CellFiltri" ColumnSpan=2 Width=900px HorizontalAlign=center>
															<asp:Table Runat=server ID="TBLfiltro" CellPadding=1 CellSpacing=0 Width="900px" HorizontalAlign=center >
																<asp:TableRow height=30px>
																	<asp:TableCell height=30px ColumnSpan=2>
																		<table>
																			<tr>
																				<td height=30px>
																					<asp:Label ID="LBtipoComunita_c" Runat=server CssClass="FiltroVoceSmall">Tipo Comunità</asp:Label>&nbsp;
																				</td>
																				<td height=30px>
																					<asp:dropdownlist id="DDLTipo" runat="server" CssClass="FiltroCampoSmall" AutoPostBack="true"></asp:dropdownlist>
																				</td>
																				<td height=30px>&nbsp;</td>
																				<td height=30px>
																					<asp:Label ID="LBtipoRicerca_c" Runat=server CssClass="FiltroVoceSmall">Tipo Ricerca</asp:Label>&nbsp;
																					<asp:dropdownlist id=DDLTipoRicerca Runat="server" CssClass="FiltroCampoSmall">
																						<asp:ListItem Value=-2 Selected="true">Nome inizia per</asp:ListItem>
																						<asp:ListItem Value=-7>Nome contiene</asp:ListItem>
																						<asp:ListItem Value=-9>Del responsabile</asp:ListItem>
																						<asp:ListItem Value=-3>Creata dopo il</asp:ListItem>
																						<asp:ListItem Value=-4>Creata prima del</asp:ListItem>
																						<asp:ListItem Value=-5>Data iscrizione dopo il</asp:ListItem>
																						<asp:ListItem Value=-6>Data fine iscrizione prima del</asp:ListItem>
																					</asp:dropdownlist>
																				</td>
																				<td height=30px>&nbsp;</td>
																				<td height=30px>
																					<asp:Label ID="LBvalore_c" Runat=server CssClass="FiltroVoceSmall" Visible=true >Valore</asp:Label>&nbsp;
																					<asp:textbox id=TXBValore Runat="server" CssClass="FiltroCampoSmall" MaxLength=100 Columns=30></asp:textbox>
																					<asp:DropDownList ID="DDLresponsabile" Runat=server AutoPostBack=True CssClass="FiltroCampoSmall" Visible=False></asp:DropDownList>
																				</td>
																			</tr>
																		</table>
																	</asp:TableCell>
																</asp:TableRow>
																<asp:TableRow Runat=server ID="TBRorgnCorsi">
																	<asp:TableCell ID="TBCorganizzazione0" ColumnSpan=2>
																		<table cellspacing=0 cellpadding=0>
																			<tr>
																				<td width=70px nowrap="nowrap">
																					<asp:Label ID="LBorganizzazione_c" Runat=server CssClass="FiltroVoceSmall">Organizzazione:&nbsp;</asp:Label>
																				</td>
																				<td>
																					<asp:DropDownList ID="DDLorganizzazione" Runat=server AutoPostBack=True CssClass="FiltroCampoSmall"></asp:DropDownList>		
																				</td>
																				<td>&nbsp;&nbsp;</td>
																				<td>
																					<asp:Table ID="TBLcorsi" CellPadding=0 CellSpacing=0 BorderStyle=None Runat=server Visible=False >
																						<asp:TableRow>
																							<asp:TableCell>
																								<asp:Label ID="LBannoAccademico_c" Runat=server CssClass="FiltroVoceSmall">A.A.:&nbsp;</asp:Label>
																							</asp:TableCell>
																							<asp:TableCell>
																								<asp:DropDownList ID="DDLannoAccademico" Runat=server AutoPostBack=True CssClass="FiltroCampoSmall"></asp:DropDownList>
																							</asp:TableCell>
																							<asp:TableCell>
																								&nbsp;&nbsp;<asp:Label ID="LBperiodo_c" Runat=server CssClass="FiltroVoceSmall">Periodo:&nbsp;</asp:Label>
																							</asp:TableCell>
																							<asp:TableCell>
																								<asp:DropDownList ID="DDLperiodo" Runat=server AutoPostBack=True CssClass="FiltroCampoSmall"></asp:DropDownList>
																							</asp:TableCell>
																						</asp:TableRow>
																					</asp:Table>
																					<asp:Table ID="TBLtipoCorsoDiStudi" CellPadding=0 CellSpacing=0 BorderStyle=None Runat=server Visible=False >
																						<asp:TableRow>
																							<asp:TableCell>
																								<asp:Label ID="LBcorsoDiStudi_t" Runat=server CssClass="FiltroVoceSmall">A.A.:&nbsp;</asp:Label>
																							</asp:TableCell>
																							<asp:TableCell>
																								<asp:DropDownList ID="DDLtipoCorsoDiStudi" Runat=server AutoPostBack=True CssClass="FiltroCampoSmall"></asp:DropDownList>
																							</asp:TableCell>
																						</asp:TableRow>
																					</asp:Table>
																					<asp:Label ID="LBnoCorsi" Runat=server Visible=False >&nbsp;</asp:Label>
																				</td>
																			</tr>
																		</table>
																	</asp:TableCell>
																</asp:TableRow>
																<asp:TableRow  Height=30px>
																	<asp:TableCell Height=30px>
																		<asp:Label ID="LBricercaByIscrizione_c" Runat=server CssClass="FiltroVoceSmall">Comunità:</asp:Label>
																		<asp:RadioButtonList id="RBLricercaByIscrizione" runat="server" CssClass="FiltroCampoSmall" AutoPostBack="true" RepeatDirection=Horizontal RepeatLayout=Flow >
																			<asp:ListItem  Value=0 Selected=true>a cui iscriversi</asp:ListItem>
																			<asp:ListItem Value=1>a cui si è iscritti</asp:ListItem>
																		</asp:RadioButtonList>
																		&nbsp;&nbsp;&nbsp;&nbsp;
																		<asp:Label ID="LBstatoComunita_t" Runat=server CssClass="FiltroVoceSmall">Stato:</asp:Label>
																		<asp:RadioButtonList id="RBLstatoComunita" runat="server" CssClass="FiltroCampoSmall" AutoPostBack="true" RepeatDirection=Horizontal RepeatLayout=Flow >
																			<asp:ListItem Value=0 Selected=true>Attivate</asp:ListItem>
																			<asp:ListItem Value=1>Archiviate</asp:ListItem>
																			<asp:ListItem Value=2>Bloccate</asp:ListItem>
																		</asp:RadioButtonList>
																	</asp:TableCell>
																	<asp:TableCell HorizontalAlign=right Height=30px >
																		<asp:CheckBox ID="CBXautoUpdate" Runat=server AutoPostBack=True CssClass="FiltroCampoSmall" Text="Aggiornamento automatico"></asp:CheckBox>
																		&nbsp;
																		<asp:button id="BTNCerca" Runat="server" CssClass="PulsanteFiltro" Text="Cerca"></asp:button>
																	</asp:TableCell>
																</asp:TableRow>
															</asp:Table>
														</asp:TableCell>
													</asp:TableRow>
												</asp:Table>
											</asp:TableCell>
										</asp:TableRow>
										<asp:TableRow id=TBRsimple>
											<asp:TableCell HorizontalAlign=left  cssclass="top" ColumnSpan=2>
												<asp:LinkButton ID="LNBespandi" Runat=server>Espandi</asp:LinkButton>
												&nbsp;|&nbsp;
												<asp:LinkButton ID="LNBcomprimi" Runat=server>Comprimi</asp:LinkButton>
												&nbsp;|&nbsp;															
												<a href="#" onclick="Javascript: window.open('./help.aspx','Help','menubar=no,resizable=false,location=no,toolbar=no,scrollbars=true,width=450,height=400');window.status='';return false;" onmouseover="window.status='';return true" onmouseout="window.status='';return true;">
													<img src="./../images/help.gif" alt="Help" align="middle"/>
												</a>
											</asp:TableCell>
										</asp:TableRow>
										<asp:TableRow>
											<asp:TableCell ColumnSpan=2 HorizontalAlign=Left >
												<radt:RadTreeView id="RDTcomunita" runat="server" align="left" width="900px"
													CausesValidation="False"  ContextMenuContentFile="~/RadControls/TreeView/Skins/Comunita/ContextMenus.xml"
													CssFile="~/RadControls/TreeView/Skins/Comunita/styles.css" PathToJavaScript="~/Jscript/RadTreeView_Client_3_1.js"
													ImagesBaseDir="~/RadControls/TreeView/Skins/Comunita/" skin="Comunita">
												</radt:RadTreeView>
											</asp:TableCell>
										</asp:TableRow>
									</asp:Table>
								</asp:Panel>
								<asp:Panel id="PNLdettagli" HorizontalAlign="Center" Visible="False" Runat="server">
									<table width="700" align="center" border="0">
										<tr>
											<td align="center">
												<FIELDSET><LEGEND class="tableLegend"><asp:Label ID="LBlegenda" Runat=server cssclass=tableLegend>Dettagli comunità</asp:Label></LEGEND>
													<asp:table id="TBLdettagli" Runat="server" Width="500px">
														<asp:TableRow>
															<asp:tableCell CssClass="nosize" width="5px" height="5px">&nbsp;</asp:tableCell>
															<asp:TableCell  HorizontalAlign=Center >
																<DETTAGLI:CTRLDettagli id="CTRLDettagli" runat="server" width=650px></DETTAGLI:CTRLDettagli>
															</asp:TableCell>
															<asp:tableCell CssClass="nosize" width="5px" height="5px">&nbsp;</asp:tableCell>
														</asp:TableRow>
														<asp:tableRow>
															<asp:tableCell CssClass="nosize" width="5px" height="5px">&nbsp;</asp:tableCell>
															<asp:tableCell  HorizontalAlign="left">
																&nbsp;
															</asp:tableCell>
															<asp:tableCell CssClass="nosize" width="5px" height="5px">&nbsp;</asp:tableCell>
														</asp:tableRow>
													</asp:table>
												</FIELDSET>
											</td>
										</tr>
									</table>
								</asp:Panel>
								<asp:Panel ID="PNLmessaggio" Runat=server HorizontalAlign=Center Visible=False >
									<table border=0 align=center >
										<tr>
											<td height=50px>&nbsp;</td>
										</tr>
										<tr>
											<td>
												<asp:Label ID="LBmessaggio" Runat=server CssClass="avviso12"></asp:Label>
											</td>
										</tr>
										<tr>
											<td height=50px>&nbsp;</td>
										</tr>
									</table>
								</asp:Panel>
								<asp:Panel ID="PNLconferma" Runat="server" Visible="False" HorizontalAlign="Center">
									<table align="center">
										<tr>
											<td height="50" colspan=2>&nbsp;
											<input type =hidden id="HDcmnt_id" runat=server NAME="HDcmnt_id"/>
											<input type =hidden id="HDpath" runat=server NAME="HDpath"/>
											<input type =hidden id="HDisChiusa" runat=server NAME="HDisChiusa"/>
											</td>
										</tr>
										<tr>
											<td align="center"  colspan=2>
												<asp:Label id="LBconferma" CssClass="messaggioIscrizione" Runat="server">Conferma l'iscrizione alla comunità #nomeComunita# - #nomeResponsabile#</asp:Label>
											</td>
										</tr>
									</table>
								</asp:Panel>
							</td>
						</tr>
					</table>
				</asp:Panel>
				<asp:Panel ID="PNLpermessi" Runat="server" Visible="False" HorizontalAlign="Center">
					<table align="center">
						<tr>
							<td height="50">&nbsp;</td>
						</tr>
						<tr>
							<td align="center">
								<asp:Label id="LBNopermessi" Runat="server" CssClass="avviso12"></asp:Label></td>
						</tr>
						<tr>
							<td vAlign="top" height="50">
								&nbsp;
							</td>
						</tr>
					</table>
				</asp:Panel>
			</td>
		</tr>
	</table>
</asp:Content>
<%--
<!DOCTYPE HTML PUBLIC "-//W3C//Dtd HTML 4.0 transitional//EN">
<HTML>
	<head runat="server">
		<title>Comunità On Line - Ricerca comunita:visualizzazione ad albero</title>
		<meta name="GENERATOR" content="Microsoft Visual Studio .NET 7.1"/>
		<meta name="CODE_LANGUAGE" content="Visual Basic .NET 7.1"/>
		<meta name="vs_defaultClientScript" content="JavaScript"/>
		<meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5"/>
		<LINK href="./../Styles.css" type="text/css" rel="stylesheet"/>
		
	</HEAD>

	<body  bgcolor="#ffffff" onkeydown="return SubmitRicerca(event);">
		<form id="aspnetForm" method="post" runat="server">
		<asp:ScriptManager ID="SCMmanager" runat="server"></asp:ScriptManager>
			<table cellSpacing="0" cellPadding="0" width="780" align="center" border="0">
				<tr>
					<td colSpan="3">
						<HEADER:CtrLHEADER id="CTRLintestazione" runat="server"></HEADER:CtrLHEADER>
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
</HTML>--%>
