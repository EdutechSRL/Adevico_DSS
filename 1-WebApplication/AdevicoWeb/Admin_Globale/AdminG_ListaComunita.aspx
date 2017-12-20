<%@ Page Language="vb" AutoEventWireup="false" MasterPageFile="~/Admin_Globale/AdminPortal.Master"
 Codebehind="AdminG_ListaComunita.aspx.vb" Inherits="Comunita_OnLine.AdminG_ListaComunita" %>
<%@ MasterType VirtualPath="~/Admin_Globale/AdminPortal.Master" %>

<%@ Register TagPrefix="DETTAGLI" TagName="CTRLDettagli" Src="../UC/UC_dettaglicomunita.ascx" %>

<asp:Content ContentPlaceHolderID="HeadContent" runat="server" ID="Content1">
	<script type="text/javascript" language="Javascript" src="./../jscript/generali.js"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="CPHservice" runat="server">
	<table width="900px" align="center">
		<%--<tr>
			<td Align=Left Class=RigaTitoloAdmin>
				<asp:label id="LBtitolo" Runat="server">- Elenco comunità -</asp:label>
			</td>
		</tr>--%>
		<tr>
			<td align="center">
				<INPUT id="HDN_Azione" type="hidden" name="HDN_Azione" runat="server"/>
				<INPUT id="HDN_Path" type="hidden" name="HDN_Path" runat="server"/>
				<asp:panel id="PNLpermessi" Runat="server" HorizontalAlign="Center" Visible="False">
					<table align=center>
						<tr>
							<td height=50>&nbsp;</td>
						</tr>
						<tr>
							<td align=center>
								<asp:Label id=LBNopermessi Runat="server" CssClass="messaggio"></asp:Label>
							</td>
						</tr>
						<tr>
							<td vAlign=top height=50>
								<asp:LinkButton id=LNBnascondi Runat="server">Indietro</asp:LinkButton>&nbsp; 
							</td>
						</tr>
					</table>
				</asp:panel>
				<asp:panel id="PNLcontenuto" Runat="server" HorizontalAlign="Center">
					<table width="100%" align=center>
						<tr>
							<td>
								<asp:Table Runat=server ID="TBLfiltro" CellPadding=1 CellSpacing=1 Width="900px" BorderStyle=None>
									<asp:TableRow ID="TBRorganizzazione">
										<asp:TableCell>
											<asp:Label ID="LBorganizzazione_c" Runat=server CssClass="FiltroVoceSmall">Organizzazione:&nbsp;</asp:Label>
										</asp:TableCell>
										<asp:TableCell ID="TBCorganizzazione">
												<asp:DropDownList ID="DDLorganizzazione" Runat=server AutoPostBack=True CssClass="FiltroCampoSmall"></asp:DropDownList>
										</asp:TableCell>
										<asp:TableCell CssClass="FiltroVoceSmall">
											<asp:Label ID="LBtipoComunita_c" Runat=server CssClass="FiltroVoceSmall">Tipo Comunita:&nbsp;</asp:Label>
										</asp:TableCell>
										<asp:TableCell>
											<asp:dropdownlist id="DDLTipo" runat="server" CssClass="FiltroCampoSmall" AutoPostBack="true"></asp:dropdownlist>
										</asp:TableCell>
										<asp:TableCell CssClass="FiltroVoceSmall">
											<asp:Label ID="LBnumeroRecord_c" Runat=server CssClass="FiltroVoceSmall">N° Record:&nbsp;</asp:Label>
										</asp:TableCell>
										<asp:TableCell>
											<asp:dropdownlist id=DDLNumeroRecord Runat="server" CssClass="FiltroCampoSmall" AutoPostBack="true">
												<asp:ListItem Value="15" Selected="true"></asp:ListItem>
												<asp:ListItem Value="30"></asp:ListItem>
												<asp:ListItem value="45"></asp:ListItem>
												<asp:ListItem value="50"></asp:ListItem>
											</asp:dropdownlist>
										</asp:TableCell>
									</asp:TableRow>
									<asp:TableRow>				
										<asp:TableCell CssClass="FiltroVoceSmall" ID="TBCtipoRicerca_c" Runat=server>
											<asp:Label ID="LBtipoRicerca_c" Runat=server CssClass="FiltroVoceSmall">Tipo Ricerca:&nbsp;</asp:Label>
										</asp:TableCell>
										<asp:TableCell ID="TBCtipoRicerca" Runat=server>
											<asp:dropdownlist id=DDLTipoRicerca Runat="server" CssClass="FiltroCampoSmall">
												<asp:ListItem Value=-2 Selected="true">Nome</asp:ListItem>
												<asp:ListItem Value=-3>Creata dopo il</asp:ListItem>
												<asp:ListItem Value=-4>Creata prima del</asp:ListItem>
												<asp:ListItem Value=-5>Data iscrizione dopo il</asp:ListItem>
												<asp:ListItem Value=-6>Data fine iscrizione prima del</asp:ListItem>
											</asp:dropdownlist>
										</asp:TableCell>
										<asp:TableCell CssClass="FiltroVoceSmall" ID="TBCvalore_c">
											<asp:Label ID="LBvalore_c" Runat=server CssClass="FiltroVoceSmall">Valore:&nbsp;</asp:Label>
										</asp:TableCell>
										<asp:TableCell ID="TBCvalore">
											<asp:textbox id=TXBValore Runat="server" CssClass="FiltroCampoSmall" MaxLength=100></asp:textbox>
										</asp:TableCell>
										<asp:TableCell CssClass="FiltroVoceSmall">
											<asp:Label ID="LBvuota_c" Runat=server CssClass="FiltroVoceSmall">&nbsp;</asp:Label>
										</asp:TableCell>
										<asp:TableCell>
											<asp:button id=BTNCerca Runat="server" CssClass="pulsante" Text="Cerca"></asp:button>
										</asp:TableCell>
									</asp:TableRow>
									<asp:TableRow ID= "TBRcorsi" Runat=server Visible=False>
										<asp:TableCell ColumnSpan=6 Height=25px>
											<asp:Table ID="TBLcorsi" CellPadding=2 CellSpacing=2 BorderStyle=None Runat=server>
												<asp:TableRow>
													<asp:TableCell>
														<asp:Label ID="LBannoAccademico_c" Runat=server CssClass="FiltroVoceSmall">A.A.:&nbsp;</asp:Label>
													</asp:TableCell>
													<asp:TableCell>
														<asp:DropDownList ID="DDLannoAccademico" Runat=server AutoPostBack=True CssClass="FiltroCampoSmall"></asp:DropDownList>
													</asp:TableCell>
													<asp:TableCell>
														<asp:Label ID="LBperiodo_c" Runat=server CssClass="FiltroVoceSmall">Periodo:&nbsp;</asp:Label>
													</asp:TableCell>
													<asp:TableCell>
														<asp:DropDownList ID="DDLperiodo" Runat=server AutoPostBack=True CssClass="FiltroCampoSmall"></asp:DropDownList>
													</asp:TableCell>
												</asp:TableRow>
											</asp:Table>
										</asp:TableCell>
									</asp:TableRow>
									<asp:TableRow>
										<asp:TableCell ColumnSpan=6>
											<table align=center cellspacing=5>
												<tr>
													<td align="center">
														<asp:linkbutton id="LKBtutti" Runat="server" CssClass="lettera" CommandArgument="-1" OnClick="FiltroLink_Click">Tutti</asp:linkbutton></td>
													<td align="center">
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
										</asp:TableCell>
									</asp:TableRow>
									<asp:TableRow>
										<asp:TableCell HorizontalAlign=Left VerticalAlign=Bottom Height=25px ColumnSpan=6>
											<asp:RadioButtonList ID="RBLvisualizza" AutoPostBack=True Runat=server RepeatDirection=Horizontal >
												<asp:ListItem Value=3 Selected=True>elenco semplice</asp:ListItem>
												<asp:ListItem Value=1>albero comunità</asp:ListItem>
												<asp:ListItem Value=2>albero comunità organizzativo</asp:ListItem>
											</asp:RadioButtonList>&nbsp;
										</asp:TableCell>
										<asp:TableCell>&nbsp;</asp:TableCell>
									</asp:TableRow>
								</asp:Table>
								<table align=center>
									<tr>
										<td align=center >
											<asp:datagrid 
												id=DGComunita runat="server" 
												PageSize="15" DataKeyField="CMNT_id" 
												AllowPaging="true" AutoGenerateColumns="False"
                                                AllowCustomPaging=True  AllowSorting="true"
                                                ShowFooter="false" 
                                                CssClass="DataGrid_Generica">
												<AlternatingItemStyle CssClass="Righe_Alternate_Center"></AlternatingItemStyle>
												<HeaderStyle CssClass="Riga_Header"></HeaderStyle>
												<ItemStyle CssClass="Righe_Normali_center" Height="22px"></ItemStyle>
												<PagerStyle CssClass="Riga_Paginazione" Position="Bottom" Mode="NumericPages" Visible="true"
													HorizontalAlign="Right" Height="25px" VerticalAlign="Bottom"></PagerStyle>
												<Columns>
													<asp:TemplateColumn runat="server" ItemStyle-Width="80">
														<ItemTemplate>
															<asp:ImageButton ID="IMGDettagli" CommandName="dettagli" ImageUrl="../images/proprieta.gif" Runat="server"></asp:ImageButton>
															<asp:ImageButton id="IMGEdit" CommandName="modifica" ImageUrl="../images/m.gif" Runat="server"></asp:ImageButton>
															<asp:ImageButton ID="IMGDelete" CommandName="cancella" ImageUrl="../images/elimina.gif" Runat="server"></asp:ImageButton>
														</ItemTemplate>
													</asp:TemplateColumn>
													<asp:TemplateColumn runat="server" HeaderText="Tipo" ItemStyle-Width="40" SortExpression="TPCM_Descrizione" >
														<ItemTemplate>
															<img runat=server src='<%# DataBinder.Eval(Container.DataItem, "TPCM_icona") %>' alt='<%# DataBinder.Eval(Container.DataItem, "TPCM_descrizione") %>' align=middle ID="Img1"/>
														</ItemTemplate>
													</asp:TemplateColumn>
													<asp:BoundColumn DataField="CMNT_Livello" HeaderText="Livello" Visible=true SortExpression="CMNT_Livello"></asp:BoundColumn>
													<asp:BoundColumn DataField="CMNT_Esteso" HeaderText="Nome" SortExpression="CMNT_Nome"></asp:BoundColumn>
													<asp:BoundColumn DataField="AnnoAccademico" HeaderText="A.A." Visible="false" SortExpression="CMNT_Anno"></asp:BoundColumn>
													<asp:BoundColumn DataField="Periodo" HeaderText="Periodo" Visible="false" SortExpression="CMNT_PRDO_descrizione"></asp:BoundColumn>
													<asp:BoundColumn DataField="CMNT_Responsabile" Visible="false" SortExpression="CMNT_Responsabile" HeaderText="Responsabile"></asp:BoundColumn>
													<asp:BoundColumn DataField="AnagraficaCreatore" Visible="true" SortExpression="AnagraficaCreatore" HeaderText="Creatore"></asp:BoundColumn>
													<asp:BoundColumn DataField="TPCM_icona" Visible="false"></asp:BoundColumn>
													<asp:ButtonColumn CommandName="LogonAs" Text="Logon as.." ButtonType=LinkButton Visible="false"></asp:ButtonColumn>
													<asp:ButtonColumn CommandName="servizi" Text="Gestione" ButtonType=LinkButton HeaderText="Servizi"></asp:ButtonColumn>
													<asp:ButtonColumn CommandName="iscritti" Text="Gestione" ButtonType=LinkButton HeaderText="Utenti"></asp:ButtonColumn>
													<asp:BoundColumn DataField="CMNT_EstesoNoSpan" Visible="false"></asp:BoundColumn>
													<asp:BoundColumn DataField ="CMNT_path" Visible =True ></asp:BoundColumn>
												</Columns>
											</asp:datagrid><br/>
											<asp:Label id=LBmsgDG Runat="server" CssClass="avviso_normal" Visible="False"></asp:Label></td></tr></table>
										</td>
									</tr>
									<tr>
										<td align=left >
											<asp:LinkButton ID="LNBvisualizzazione" Runat=server>Colonne visibili..</asp:LinkButton>
										</td>	
									</tr>
								</table>
				</asp:panel>
				<asp:Panel ID="PNLvisualizzazione" Runat=server HorizontalAlign=Center Visible=False >
					<table width=500 align=center border=0>
						<tr>
							<td>
								<fieldset>
									<legend class=tableLegend><asp:Label ID="LBvisualizzazione" Runat=server>Colonne da visualizzare</asp:Label></legend>
									<table align=center>
										<tr>
											<td colspan=3 height="10px" class="nosize0">&nbsp;</td>
										</tr>
										<tr>
											<td width ="10px" class="nosize0">&nbsp;</td>
											<td>
												<asp:CheckBoxList ID="CBXvisualizza" Runat=server CssClass="FiltroVoceSmall" RepeatColumns=5 RepeatDirection=Horizontal >
													<asp:ListItem Value=1 Selected=True>Tipo Comunità</asp:ListItem>
													<asp:ListItem Value=2 >Livello</asp:ListItem>
													<asp:ListItem Value=3 Selected=True>Nome</asp:ListItem>
													<asp:ListItem Value=4 >Anno accademico</asp:ListItem>
													<asp:ListItem Value=5 >Periodo</asp:ListItem>
													<asp:ListItem Value=6 >Responsabile</asp:ListItem>
													<asp:ListItem Value=7 Selected=True>Creatore</asp:ListItem>
													<asp:ListItem Value=9 >Accedi</asp:ListItem>
													<asp:ListItem Value=10 Selected=True>Servizi</asp:ListItem>
													<asp:ListItem Value=11 Selected=True>Iscritti</asp:ListItem>
												</asp:CheckBoxList>
											</td>
											<td width ="10px" class="nosize0">&nbsp;</td>
										</tr>
										<tr>
											<td colspan=3 height="10px" class="nosize0">&nbsp;</td>
										</tr>
										<tr>
											<td width ="10px" class="nosize0">&nbsp;</td>
											<td>
												[<asp:LinkButton ID="LNBchiudi" Runat=server >Nascondi & Salva</asp:LinkButton>]
											</td>
											<td width ="10px" class="nosize0">&nbsp;</td>
										</tr>
										<tr>
											<td colspan=3 height="10px" class="nosize0">&nbsp;</td>
										</tr>
									</table>
								</fieldset>
							</td>
						</tr>
					</table>
				</asp:Panel>
				<asp:panel id="PNLdettagli" Runat="server" HorizontalAlign="Center" Visible="False">
					<table width=500 align=center border=0>
						<tr>
							<td align=center colspan=2>
							<FIELDSET><LEGEND class=tableLegend>Dettagli comunità</LEGEND>
												<input type="hidden" id="HDNcmnt_ID" runat="server"/>
												<input type="hidden" id="HDNcmnt_Path" runat="server"/>
																
										<DETTAGLI:CTRLDettagli id="CTRLDettagli" runat="server"></DETTAGLI:CTRLDettagli>	
								</FIELDSET> 
							</td>
						</tr>
						<tr>
							<td><asp:Button ID="BTNnascondi" Runat="server" Text="Indietro" CssClass="Pulsante"></asp:Button></td>
							<td align=right><asp:Button ID="BTNentra" Runat="server" Text="Entra" CssClass="Pulsante" Visible=False ></asp:Button></td>
						</tr>
					</table>
				</asp:panel>
				<asp:panel id="PNLmessaggi" Runat="server" Visible="False">
					<table cellSpacing=0 cellPadding=0 align=center border=0>
						<tr>
							<td height=30>&nbsp;</td>
						</tr>
						<tr>
							<td>
							<asp:Label id=LBMessaggi Runat="server" CssClass="avviso"></asp:Label>
							</td>
						</tr>
						<tr>
							<td align=right>
								<asp:Button id=BTNMessaggi Runat="server" CssClass="Pulsante" Text="Seleziona Ancora"></asp:Button>&nbsp;&nbsp;&nbsp; 
							</td>
						</tr>
					</table>
				</asp:panel>
			</td>
		</tr>
	</table>
</asp:Content>


<%--<!DOCTYPE HTML PUBLIC "-//W3C//Dtd HTML 4.0 transitional//EN">
<HTML>
  <head runat="server">
		<title>Comunità On Line - Lista Comunita</title>

		<meta content="Microsoft Visual Studio.NET 7.0" name="GENERATOR"/>
		<meta content="Visual Basic 7.0" name="CODE_LANGUAGE"/>
		<meta content="JavaScript" name="vs_defaultClientScript"/>
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema"/>
		<LINK href="./../Styles.css" type="text/css" rel="stylesheet"/>
</HEAD>
	<body>
		<form id="aspnetForm" method="post" runat="server">
		<asp:ScriptManager ID="SCMmanager" runat="server"></asp:ScriptManager>
			<table cellSpacing="0" cellPadding="0" width="780" align="center" border="0">
				<tr>
					<td colSpan="3"><HEADER:CtrLHEADER id="Intestazione" runat="server"></HEADER:CtrLHEADER></td>
				</tr>
				<tr>
					<td colSpan="3">

					</td>
				</tr>
			</table>
			<FOOTER:CtrLFOOTER id="CtrLFooter" runat="server"></FOOTER:CtrLFOOTER>
		</form>
	</body>
</HTML>--%>
