<%@ Page Language="vb" AutoEventWireup="false" MasterPageFile="~/Admin_Globale/AdminPortal.Master"
 Codebehind="AdminG_RicercaComunita.aspx.vb" Inherits="Comunita_OnLine.AdminG_RicercaComunita"%>
<%@ MasterType VirtualPath="~/Admin_Globale/AdminPortal.Master" %>

<%--
<%@ Register TagPrefix="HEADER" TagName="CtrLHeader" Src="./UC/HeaderAdmin.ascx" %>--%>

<asp:Content ContentPlaceHolderID="HeadContent" runat="server" ID="Content1">
	<script type="text/javascript" language="Javascript" src="./../jscript/generali.js"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="CPHservice" runat="server">
	<table width="900px" align="center">
		<%--<tr>
			<td bgColor="#a3b2cd" height="1"></td>
		</tr>
		<tr>
			<td class="titolo" align="center"><asp:label id="LBtitolo" CssClass="TitoloServizio" Runat="server">- Scegli la persona per avere le sue comunità -</asp:label></td>
		</tr>
		<tr>
			<td bgColor="#a3b2cd" height="1"></td>
		</tr>--%>
		<tr>
			<td align="center">
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
					<asp:Panel ID=PNLsceltaPersona Runat=server >
						<asp:panel id="PNLfiltri" Runat="server">
							<table cellSpacing="0" cellPadding="0" width="100%" border="0">
								<tr>
									<td vAlign="top" align="center">
										<FIELDSET><LEGEND class="tableLegend">Cerca</LEGEND><br/>
											<asp:table id="TBLfiltro" Runat="server">
												<asp:TableRow>
													<asp:TableCell Width=210px>
														<asp:Label ID="LBorganizzazione" runat="server" CssClass="FiltroVoce">Organizzazione:</asp:Label>
													</asp:TableCell>
													<asp:TableCell Width=150px>
														<asp:Label ID="LBnome" runat="server" CssClass="FiltroVoce">Nome:</asp:Label>
													</asp:TableCell>
													<asp:TableCell>
														<asp:Label ID="LBcognome" runat="server" CssClass="FiltroVoce">Cognome:</asp:Label>
													</asp:TableCell>
												</asp:TableRow>
												<asp:TableRow>
													<asp:TableCell>
														<asp:DropDownList id="DDLorganizzazione" Runat="server" CssClass="FiltroCampo" AutoPostBack="True"></asp:DropDownList>
													</asp:TableCell>
													<asp:TableCell Width=150px>
														<asp:DropDownList id="DDLricercaPersonaBy" Runat="server" CssClass="FiltroCampo" AutoPostBack="true">
															<asp:ListItem Value=-1>tutti</asp:ListItem>
															<asp:ListItem Value=1>Nome</asp:ListItem>
															<asp:ListItem Value=2>Cognome</asp:ListItem>
															<asp:ListItem Value=3>Data di Nascita</asp:ListItem>
															<asp:ListItem Value=4>Matricola</asp:ListItem>
															<asp:ListItem Value=5>Mail</asp:ListItem>
															<asp:ListItem Value=6>Codice Fiscale</asp:ListItem>
															<asp:ListItem Value=7>Login</asp:ListItem>
														</asp:DropDownList>
													</asp:TableCell>
													<asp:TableCell>
														<asp:TextBox id="TXBvaloreSearch" Runat="server" MaxLength="300" CssClass="FiltroCampo" Width=150px></asp:TextBox>
													</asp:TableCell>
												</asp:TableRow>
												<asp:TableRow>
													<asp:TableCell ColumnSpan=3>
														<asp:Label ID="LBtipoPersona" runat="server" CssClass="FiltroVoce">Tipo Persona:</asp:Label>
													</asp:TableCell>
												</asp:TableRow>
												<asp:TableRow>
													<asp:TableCell ColumnSpan=3>
														<asp:DropDownList id="DDLtipoPersona" Runat="server" CssClass="FiltroCampo" AutoPostBack="True"></asp:DropDownList>
													</asp:TableCell>
												</asp:TableRow>
												<asp:TableRow>
													<asp:TableCell ColumnSpan="2">&nbsp;</asp:TableCell>
													<asp:TableCell HorizontalAlign="Right">
														<asp:Button id="BTNcerca" Runat="server" Text="Cerca" CssClass="Pulsante"></asp:Button>
													</asp:TableCell>
												</asp:TableRow>
												<asp:TableRow>
													<asp:TableCell ColumnSpan="3">
														<table width="100%">
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
											</asp:table>
										</FIELDSET>
									</td>
								</tr>
							</table>
						</asp:panel>
						<asp:panel id="PNLnorecordPrsn" Runat="server" Visible="false">
							<table width="450" align="center">
								<tr>
									<td height="40">&nbsp;</td>
								</tr>
								<tr align="center">
									<td>
										<asp:Label id="LBnorecordPrsn" Runat="server" CssClass="Errore"></asp:Label></td>
								</tr>
							</table>
						</asp:panel>
						<asp:panel id="PNLpersona" Runat="server">
							<asp:datagrid 
								id="DGpersona" 
								runat="server" 
								AllowCustomPaging="True" 
								PageSize="30" 
								DataKeyField="PRSN_ID"
								AllowPaging="true" 
								AutoGenerateColumns="False"
								ShowFooter="false"
								AllowSorting="True"
								CssClass="DataGrid_Generica" >
								<AlternatingItemStyle CssClass="Righe_Alternate_Center"></AlternatingItemStyle>
								<HeaderStyle CssClass="Riga_Header"></HeaderStyle>
								<ItemStyle CssClass="Righe_Normali_center" Height="22px"></ItemStyle>
								<PagerStyle CssClass="Riga_Paginazione" Position="Bottom" Mode="NumericPages" Visible="true"
									HorizontalAlign="Right" Height="25px" VerticalAlign="Bottom"></PagerStyle>
								<Columns>
									<asp:buttoncolumn DatatextField="PRSN_Anagrafica" HeaderText="Anagrafica" SortExpression="PRSN_Anagrafica"
										CommandName="VisualizzaComunita"></asp:buttoncolumn>
									<asp:TemplateColumn runat="server" HeaderText="Nato il" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="10"
										SortExpression="PRSN_datanascita" >
										<ItemTemplate>
											<asp:Label Runat="server" id="LBdataNascitaGriglia">
												<%# DataBinder.Eval(Container.DataItem, "oPRSN_datanascita") %>
											</asp:Label>
										</ItemTemplate>
									</asp:TemplateColumn>
									<asp:BoundColumn DataField="TPPR_descrizione" HeaderText="Tipo Persona" SortExpression="TPPR_descrizione"></asp:BoundColumn>
									<asp:BoundColumn DataField="PRSN_TPPR_id" HeaderText="idtipopersona" Visible="False"></asp:BoundColumn>
									<asp:BoundColumn DataField="PRSN_login" HeaderText="Login" SortExpression="PRSN_login"></asp:BoundColumn>
									<asp:BoundColumn DataField="PRSN_ID" Visible="False"></asp:BoundColumn>
								</Columns>
							</asp:datagrid>
							<asp:panel id="PNLpaginazione" Runat="server" Visible="false">
								<table width="100%" align="center">
									<tr>
										<td align="right"><SPAN class="Testo_paginazione">Record per Pagina:</SPAN>
											<asp:DropDownList id="DDLpaginazione" Runat="server" CssClass="testoNormale" AutoPostBack="true">
												<asp:ListItem Value="30"> 30</asp:ListItem>
												<asp:ListItem Value="50"> 50</asp:ListItem>
												<asp:ListItem Value="70"> 70</asp:ListItem>
												<asp:ListItem Value="85"> 85</asp:ListItem>
												<asp:ListItem Value="100">100</asp:ListItem>
											</asp:DropDownList>
										</td>
									</tr>
								</table>
							</asp:panel>
						</asp:panel>
					</asp:Panel>
							
					<asp:Panel ID="PNLsceltaComunita" Runat=server Visible =False >
						<asp:panel id="PNLfiltriComunita" Runat="server">
							<table cellSpacing="0" cellPadding="0" width="100%" border="0">
								<tr>
									<td vAlign="top" align="center">
										<FIELDSET><LEGEND class="tableLegend">Cerca</LEGEND><br/>
											<asp:Table Runat=server ID="TBLcercaCmnt" CellPadding=1 CellSpacing=1 Width="100%" BorderStyle=None>
												<asp:TableRow>
													<asp:TableCell CssClass="FiltroVoce">
														<asp:Label ID="LBtipoComunita_c" Runat=server CssClass="FiltroVoce">Tipo Comunita</asp:Label>
													</asp:TableCell>
													<asp:TableCell CssClass="FiltroVoce">
														<asp:Label ID="LBtipoRuolo_c" Runat=server CssClass="FiltroVoce">Tipo Ruolo</asp:Label>
													</asp:TableCell>
													<asp:TableCell CssClass="FiltroVoce">
														<asp:Label ID="LBnumeroRecord_c" Runat=server CssClass="FiltroVoce">Numero Record</asp:Label>
													</asp:TableCell>
													<asp:TableCell CssClass="FiltroVoce" ID="TBCtipoRicerca_c" Runat=server>
														<asp:Label ID="LBtipoRicerca_c" Runat=server CssClass="FiltroVoce">Tipo Ricerca</asp:Label>
													</asp:TableCell>
													<asp:TableCell CssClass="FiltroVoce" ID="TBCvalore_c">
														<asp:Label ID="LBvalore_c" Runat=server CssClass="FiltroVoce">Valore</asp:Label>
													</asp:TableCell>
													<asp:TableCell CssClass="FiltroVoce">
														<asp:Label ID="LBvuota_c" Runat=server CssClass="FiltroVoce">&nbsp;</asp:Label>
													</asp:TableCell>
												</asp:TableRow>
												<asp:TableRow>
													<asp:TableCell>
														<asp:dropdownlist id="DDLTipo" runat="server" CssClass="FiltroCampo" AutoPostBack="true"></asp:dropdownlist>
													</asp:TableCell>
													<asp:TableCell>
														<asp:dropdownlist id="DDLTipoRuolo" runat="server" CssClass="FiltroCampo" AutoPostBack="true"></asp:dropdownlist>
													</asp:TableCell>
													<asp:TableCell>
														<asp:dropdownlist id=DDLNumeroRecord Runat="server" CssClass="FiltroCampo" AutoPostBack="true">
															<asp:ListItem Value="15" Selected="true"></asp:ListItem>
															<asp:ListItem Value="30"></asp:ListItem>
															<asp:ListItem value="45"></asp:ListItem>
															<asp:ListItem value="50"></asp:ListItem>
														</asp:dropdownlist>
													</asp:TableCell>
													<asp:TableCell ID="TBCtipoRicerca" Runat=server>
														<asp:dropdownlist id=DDLTipoRicerca Runat="server" CssClass="FiltroCampo">
															<asp:ListItem Value=-2 Selected="true">Nome</asp:ListItem>
															<asp:ListItem Value=-3>Creata dopo il</asp:ListItem>
															<asp:ListItem Value=-4>Creata prima del</asp:ListItem>
															<asp:ListItem Value=-5>Data iscrizione dopo il</asp:ListItem>
															<asp:ListItem Value=-6>Data fine iscrizione prima del</asp:ListItem>
														</asp:dropdownlist>
													</asp:TableCell>
																		
													<asp:TableCell ID="TBCvalore">
														<asp:textbox id=TXBValore Runat="server" CssClass="FiltroCampo" MaxLength=100></asp:textbox>
													</asp:TableCell>
													<asp:TableCell>
														<asp:button id="BTNcercaComunita" Runat="server" CssClass="pulsante" Text="Cerca"></asp:button>
													</asp:TableCell>
												</asp:TableRow>
																	
												<asp:TableRow>
													<asp:TableCell HorizontalAlign=Left VerticalAlign=Bottom Height=25px ColumnSpan=4>
														<asp:RadioButtonList ID="RBLvisualizza" AutoPostBack=True Runat=server RepeatDirection=Horizontal >
															<asp:ListItem Value=1 Selected=True>Iscritto Semplice</asp:ListItem>
															<asp:ListItem Value=2>Responsabile</asp:ListItem>
															<asp:ListItem Value=3>Passante</asp:ListItem>
														</asp:RadioButtonList>&nbsp;
													</asp:TableCell>
													<asp:TableCell>&nbsp;</asp:TableCell>
												</asp:TableRow>
											</asp:Table>
										</FIELDSET>
									</td>
								</tr>
							</table>
						</asp:panel>
						<asp:panel id="PNLnorecordCmnt" Runat="server" Visible="false">
						<table width="450" align="center">
							<tr>
								<td height="40">&nbsp;</td>
							</tr>
							<tr align="center">
								<td>
									<asp:Label id="LBnorecordCmnt" Runat="server" CssClass="Errore"></asp:Label></td>
							</tr>
						</table>
					</asp:panel>
					<asp:Panel ID="PNLComunita" Runat=server Visible=true>
						<INPUT id="HDprsn_id" type="hidden" runat="server"/>
							<asp:datagrid 
								id="DGComunita" 
								runat="server" 
								DataKeyField="CMNT_id" 
								AllowPaging="true" 
								AutoGenerateColumns="False"
								AllowSorting="true"
								ShowFooter="false"
								PageSize=15 
								AllowCustomPaging=True
								CssClass="DataGrid_Generica">
										<AlternatingItemStyle CssClass="Righe_Alternate_Center"></AlternatingItemStyle>
										<HeaderStyle CssClass="Riga_Header"></HeaderStyle>
										<ItemStyle CssClass="Righe_Normali_center" Height="22px"></ItemStyle>
										<PagerStyle CssClass="Riga_Paginazione" Position="Bottom" Mode="NumericPages" Visible="true"
											HorizontalAlign="Right" Height="25px" VerticalAlign="Bottom"></PagerStyle>
										<Columns>
										<asp:TemplateColumn runat="server" ItemStyle-Width="80" >
											<ItemTemplate>
												<asp:ImageButton ID="IMGDettagli" CommandName="dettagli" ImageUrl="../images/proprieta.gif" Runat="server" OnClick =IMGDettagli_Click></asp:ImageButton>
												<asp:ImageButton id="IMGEdit" CommandName="modifica" ImageUrl="../images/m.gif" Runat="server" Enabled=True OnClick=IMGEdit_Click></asp:ImageButton>
												<asp:ImageButton ID="IMGDelete" CommandName="cancella" ImageUrl="../images/elimina.gif" Runat="server"></asp:ImageButton>
											</ItemTemplate>
										</asp:TemplateColumn>
											<asp:TemplateColumn runat="server" HeaderText="Tipo" ItemStyle-Width="40" SortExpression="TPCM_Descrizione">
												<ItemTemplate>
													<img runat=server src='<%# DataBinder.Eval(Container.DataItem, "TPCM_icona") %>' alt='<%# DataBinder.Eval(Container.DataItem, "TPCM_descrizione") %>' align=middle ID="Img1"/>
												</ItemTemplate>
											</asp:TemplateColumn>
											<asp:BoundColumn DataField="CMNT_Nome" HeaderText="Nome" SortExpression="CMNT_Nome"></asp:BoundColumn>
											<asp:BoundColumn DataField="AnagraficaCreatore" HeaderText="Creatore" SortExpression="AnagraficaCreatore"></asp:BoundColumn>
											<asp:BoundColumn DataField="CMNT_Responsabile" HeaderText="Responsabile" ></asp:BoundColumn>
											<asp:BoundColumn DataField="CMNT_dataCreazione" HeaderText="Creata il" sortExpression="CMNT_dataCreazione"></asp:BoundColumn>
															
										<asp:BoundColumn DataField="RLPC_TPRL_id"   Visible=False ></asp:BoundColumn>
										<asp:BoundColumn DataField="TPRL_nome" HeaderText="Tipo Ruolo" ItemStyle-Width="100" Visible=true ></asp:BoundColumn>
										<asp:TemplateColumn HeaderText="Gestione" runat="server"  ItemStyle-HorizontalAlign="Center">
										<ItemTemplate>
												<asp:LinkButton CommandName="servizi" Text="|-Servizi-|" Runat =server id=LKBservizi></asp:LinkButton>
												<asp:LinkButton CommandName="iscritti" Text="|-Iscritti-|" Runat=server ID=LKBiscritti></asp:LinkButton>
																	
											</ItemTemplate>
										</asp:TemplateColumn>
										</Columns>
									</asp:datagrid><br/>
					<asp:button id="BTNindietro" Runat="server" CssClass="pulsante" Text="Indietro"></asp:button>
										
					</asp:Panel>
										
										
					</asp:Panel>
										
									
										
				</asp:panel>
			</td>
		</tr>
	</table>
</asp:Content>



<%--<!DOCTYPE HTML PUBLIC "-//W3C//Dtd HTML 4.0 transitional//EN">
<HTML>
  <head runat="server">
		<title>Comunità On Line - Ricerca Comunita</title>

		<meta content="Microsoft Visual Studio.NET 7.0" name="GENERATOR"/>
		<meta content="Visual Basic 7.0" name="CODE_LANGUAGE"/>
		<meta content="JavaScript" name="vs_defaultClientScript"/>
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema"/>
		<LINK href="./../Styles.css" type="text/css" rel="stylesheet"/>
</HEAD>
	<body>
		<form id="aspnetForm" method="post" encType="multipart/form-data" runat="server">
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
