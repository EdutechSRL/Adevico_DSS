<%@ Page Language="vb" AutoEventWireup="false" MasterPageFile="~/Admin_Globale/AdminPortal.Master"
 Codebehind="AdminG_UtentiConnessi.aspx.vb" Inherits="Comunita_OnLine.AdminG_UtentiConnessi"%>
<%@ MasterType VirtualPath="~/Admin_Globale/AdminPortal.Master" %>
<%@ Register TagPrefix="cc1" Namespace="EeekSoft.Web" Assembly="EeekSoft.Web.PopupWin" %>

<asp:Content ContentPlaceHolderID="HeadContent" runat="server" ID="Content1">
	<script type="text/javascript" language="Javascript" src="./../jscript/generali.js"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="CPHservice" runat="server">
	<table Width="900px" align="center" border=0>
<%--		<tr class="RigaTitoloAdmin">
			<td align="left" class="RigaTitoloAdmin">
				<asp:label id="LBtitolo" Runat="server">Comunicazioni generali -</asp:label>
			</td>
		</tr>--%>
		<tr>
			<td align=right >
				<asp:Panel ID="PNLmenu" Runat=server>
					<asp:LinkButton ID="LNBlista" Runat=server CausesValidation=False CssClass="Link_Menu">Lista utenti</asp:LinkButton>
				</asp:Panel>
			</td>
		</tr>
		<tr>
			<td align="center">
				<asp:panel id="PNLpermessi" Runat="server" HorizontalAlign="Center" Visible="False">
					<table align=center>
						<tr>
							<td height=50>&nbsp;</td>
						</tr>
						<tr>
							<td align=center>
								<asp:Label id=LBNopermessi Runat="server" CssClass="messaggio">Spiacente, non dispone dei permessi neccessari per accedere a tale servizio</asp:Label>
							</td>
						</tr>
						<tr>
							<td height=50>&nbsp;</td>
						</tr>
					</table>
				</asp:panel>
				<asp:panel id="PNLcontenuto" Runat="server" HorizontalAlign="Center" Width="900px">
					<asp:Table ID="TBLcontenuto" Runat=server HorizontalAlign=Center CellPadding=4 CellSpacing=5>
						<asp:TableRow ID="TBRlista" Visible=False >
							<asp:TableCell HorizontalAlign=Center>
								<asp:panel id="PNLfiltri" Runat="server">
									<table cellSpacing="0" cellPadding="0" width="100%" border="0">
										<tr>
											<td vAlign="top" align="center">
												<asp:table id="TBLfiltro" Runat="server" width="100%">
													<asp:tableRow>
														<asp:tableCell>
															<asp:Label ID="LBorganizzazione" runat="server" CssClass="FiltroVoceSmall">Organizzazione:</asp:Label>
														</asp:tableCell>
														<asp:tableCell>
															<asp:DropDownList id="DDLorganizzazione" Runat="server" CssClass="FiltroCampoSmall" AutoPostBack="true"></asp:DropDownList>
														</asp:tableCell>
														<asp:tableCell ColumnSpan=2>
															<asp:Label ID="LBtipoPersona" runat="server" CssClass="FiltroVoceSmall">Tipo Persona:</asp:Label>
															&nbsp;
															<asp:DropDownList id="DDLtipoPersona" Runat="server" CssClass="FiltroCampoSmall" AutoPostBack="true"></asp:DropDownList>
														</asp:tableCell>
														<asp:tableCell ColumnSpan=2>
															<asp:Label ID="LBtipoAutenticazione" Runat=server CssClass="FiltroVoceSmall">Autenticazione</asp:Label>
															&nbsp;
															<asp:DropDownList id="DDLtipoAutenticazione" Runat="server" CssClass="FiltroCampoSmall" AutoPostBack="true"></asp:DropDownList>
														</asp:tableCell>
														</asp:tableRow>
													<asp:tableRow>
														<asp:tableCell>
															<asp:Label ID="LBtipoRicerca_t" runat="server" CssClass="FiltroVoceSmall">Ricerca per:</asp:Label>
														</asp:tableCell>
														<asp:tableCell ColumnSpan=3>
															<table>
																<tr>
																	<td>
																		<asp:DropDownList id="DDLtipoRicerca" Runat="server" CssClass="FiltroCampoSmall">
																			<asp:ListItem Value=-1>tutti</asp:ListItem>
																			<asp:ListItem Value=1>Nome</asp:ListItem>
																			<asp:ListItem Value=2>Cognome</asp:ListItem>
																			<asp:ListItem Value=3>Data di Nascita</asp:ListItem>
																			<asp:ListItem Value=4>Matricola</asp:ListItem>
																			<asp:ListItem Value=5>Mail</asp:ListItem>
																			<asp:ListItem Value=6>Codice Fiscale</asp:ListItem>
																			<asp:ListItem Value=7>Login</asp:ListItem>
																		</asp:DropDownList>
																	</td>
																	<td>&nbsp;</td>
																	<td>
																		<asp:Label ID="LBvalore_t" runat="server" CssClass="FiltroVoceSmall">Valore:</asp:Label>
																		&nbsp;
																		<asp:TextBox id="TXBvalore" Runat="server" MaxLength="300" CssClass="FiltroCampoSmall" Columns=40></asp:TextBox>
																	</td>
																</tr>
															</table>
														</asp:tableCell>
														<asp:tableCell HorizontalAlign="Right"  ColumnSpan=2>
															<asp:Button id="BTNcerca" Runat="server" Text="Cerca" CssClass="Pulsante"></asp:Button>
														</asp:tableCell>
													</asp:tableRow>
													<asp:tableRow>
														<asp:tableCell ColumnSpan=3  Wrap=False>
															<table width="100%">
																<tr>
																	<td align="center">
																		<asp:linkbutton id="LKBtutti" Runat="server" CssClass="lettera" CommandArgument="-1" OnClick="FiltroLink_Click">Tutti</asp:linkbutton></td>
																	<td align="center">
																		<asp:linkbutton id="LKBaltro" Runat="server" CssClass="lettera"  CommandArgument="0" OnClick="FiltroLink_Click">Altri caratteri</asp:linkbutton></td>
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
														</asp:tableCell>
														<asp:TableCell ColumnSpan=3 HorizontalAlign=Right>
															<table width="100%" border=0 align=right>
																<tr>
																	<td align= left >
																		<asp:CheckBox ID="CBXaggiorna" Runat=server AutoPostBack=True Text="Aggiorna risultati" CssClass="lettera"></asp:CheckBox>																						
																	</td>
																	<td align=right >
																		<asp:panel id="PNLpaginazione" Runat="server" Visible="false" HorizontalAlign=Right >
																			<table width="100%" align="right" cellpadding=0 cellspacing=0>
																				<tr>
																					<td >
																						<asp:Label ID="LBrecord" Runat=server cssclass="Filtro_TestoPaginazione">N° record:</asp:Label>
																					</td>
																					<td class=top>
																						<asp:DropDownList id="DDLpaginazione" Runat="server" CssClass="Filtro_RecordPaginazione" AutoPostBack="true">
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
																	</td>
																</tr>
															</table>
														</asp:TableCell>
													</asp:tableRow>
												</asp:table>
											</td>
										</tr>
									</table>
								</asp:panel>
								<asp:panel id="PNLpersona" Runat="server">
									<asp:datagrid 
										id="DGpersona" runat="server" 
										AllowCustomPaging="true" PageSize="30" 
										DataKeyField="PRSN_ID"
										AllowPaging="true" AutoGenerateColumns="False"
										ShowFooter="false"
										AllowSorting="true" 
										CssClass="DataGrid_Generica" >
										<AlternatingItemStyle CssClass="ROW_Alternate_Small"></AlternatingItemStyle>
										<HeaderStyle CssClass="ROW_header_Small"></HeaderStyle>
										<ItemStyle CssClass="ROW_Normal_Small" Height="18px"></ItemStyle>
										<PagerStyle CssClass="ROW_Page_Small" Position="Bottom" Mode="NumericPages" Visible="true"
											HorizontalAlign="Right" Height="25px" VerticalAlign="Bottom"></PagerStyle>
										<Columns>
											<asp:TemplateColumn runat="server" HeaderText="" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="10" >
												<ItemTemplate>
													<asp:ImageButton id="IMBinfo" Runat="server" CausesValidation="False" CommandName="infoPersona" ImageUrl="../images/proprieta.gif"></asp:ImageButton>
													<asp:ImageButton id="IMBcancella" Runat="server" CausesValidation="False" CommandName="disconnetti" ImageUrl="../images/x_d.gif"></asp:ImageButton>
												</ItemTemplate>
											</asp:TemplateColumn>
											<asp:TemplateColumn ItemStyle-CssClass=ROW_TD_Small HeaderText="Anagrafica" SortExpression="PRSN_Anagrafica">
												<ItemTemplate>
													&nbsp;<%#Container.Dataitem("PRSN_Anagrafica")%>
												</ItemTemplate>
											</asp:TemplateColumn>
											<asp:TemplateColumn runat="server" HeaderText="Nato il" ItemStyle-Width="10"
												SortExpression="PRSN_datanascita" Visible=False ItemStyle-CssClass=ROW_TD_Small>
												<ItemTemplate>
													<asp:Label Runat="server" id="LBdataNascitaGriglia">
														<%# DataBinder.Eval(Container.DataItem, "oPRSN_datanascita") %>
													</asp:Label>
												</ItemTemplate>
											</asp:TemplateColumn>
											<asp:TemplateColumn runat="server" HeaderText="Mail" ItemStyle-Width="10"
												SortExpression="PRSN_mail" ItemStyle-CssClass=ROW_TD_Small>
												<ItemTemplate>
													<asp:HyperLink NavigateUrl='<%# "mailto:" & Container.Dataitem("PRSN_mail")%>' text='<%# Container.Dataitem("PRSN_mail")%>' Runat="server" ID="HYPMail" />
												</ItemTemplate>
											</asp:TemplateColumn>
											<asp:TemplateColumn runat="server" HeaderText="Tipo" ItemStyle-Width="10"
												SortExpression="TPPR_descrizione" ItemStyle-CssClass=ROW_TD_Small>
												<ItemTemplate>
													<%# Container.Dataitem("TPPR_descrizione")%>
												</ItemTemplate>
											</asp:TemplateColumn>
											<asp:BoundColumn DataField="PRSN_TPPR_id" HeaderText="idtipopersona" Visible="false"></asp:BoundColumn>
											<asp:BoundColumn DataField="PRSN_login" HeaderText="Login" SortExpression="PRSN_login" ItemStyle-CssClass=ROW_TD_Small></asp:BoundColumn>
											<asp:BoundColumn DataField="PRSN_ID" Visible="False"></asp:BoundColumn>
											<asp:BoundColumn DataField="PRSN_AUTN_ID" Visible="False"></asp:BoundColumn>
											<asp:BoundColumn DataField="UTCN_IP" SortExpression="UTCN_IP" HeaderText="IndirizzoIP" ItemStyle-CssClass=ROW_TD_Small></asp:BoundColumn>
											<asp:TemplateColumn runat="server" HeaderText="Connessione" ItemStyle-Width="10"
												SortExpression="UTCN_DataConnessione" ItemStyle-CssClass=ROW_TD_Small_center >
												<ItemTemplate>
													<%# Container.Dataitem("UTCN_DataConnessione")%>
												</ItemTemplate>
											</asp:TemplateColumn>
											<asp:BoundColumn DataField="UTCN_Browser" SortExpression="UTCN_Browser" HeaderText="Browser" ItemStyle-CssClass=ROW_TD_Small HeaderStyle-Wrap=False ></asp:BoundColumn>
											<asp:BoundColumn DataField="UTCN_ID" Visible=False ></asp:BoundColumn>
										</Columns>
									</asp:datagrid>
									<br/>
														
								</asp:Panel>
								<asp:panel id="PNLnorecord" Runat="server" Visible="false">
									<table width="450" align="center">
										<tr>
											<td height="40">&nbsp;</td>
										</tr>
										<tr align="center">
											<td>
												<asp:Label id="LBnorecord" Runat="server" CssClass="Errore"></asp:Label></td>
										</tr>
									</table>
								</asp:panel>
							</asp:TableCell>
						</asp:TableRow>
					</asp:Table>
				</asp:panel>
			</td>
		</tr>
	</table>
</asp:Content>