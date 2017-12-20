<%@ Page Language="vb" AutoEventWireup="false" MasterPageFile="~/Admin_Globale/AdminPortal.Master"
 Codebehind="AdminG_AlberoComunita.aspx.vb" Inherits="Comunita_OnLine.AdminG_AlberoComunita" %>
<%@ MasterType VirtualPath="~/Admin_Globale/AdminPortal.Master" %>

<%@ Register TagPrefix="radt" Namespace="Telerik.WebControls" Assembly="RadTreeView.Net2" %>
<%@ Register TagPrefix="DETTAGLI" TagName="CTRLDettagli" Src="../UC/UC_dettaglicomunita.ascx" %>

<asp:Content ContentPlaceHolderID="HeadContent" runat="server" ID="Content1">
	<script type="text/javascript" language="Javascript" src="./../jscript/generali.js"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="CPHservice" runat="server">
	<table width="900px" align="center">
		<tr>
			<td bgColor="#a3b2cd" height="1"></td>
		</tr>
<%--		<tr>
			<td class="titolo" align="center"><asp:label id="Label1" CssClass="TitoloServizio" Runat="server">- Elenco comunità -</asp:label></td>
		</tr>--%>
		<tr>
			<td bgColor="#a3b2cd" height="1"></td>
		</tr>
		<tr>
			<td align="center">
				<asp:panel id="Panel1" Runat="server" HorizontalAlign="Center" Visible="False">
					<table align=center>
						<tr>
							<td height=50>&nbsp;</td>
						</tr>
						<tr>
							<td align=center>
								<asp:Label id=Label2 Runat="server" CssClass="messaggio"></asp:Label>
							</td>
						</tr>
						<tr>
							<td vAlign=top height=50>
								<asp:LinkButton id=LinkButton1 Runat="server">Indietro</asp:LinkButton>&nbsp; 
							</td>
						</tr>
					</table>
				</asp:panel>
				<asp:panel id="Panel2" Runat="server" HorizontalAlign="Center">
					<table align="left" width="100%">
						<tr>
							<td align="center">
								<INPUT id="Hidden1" type="hidden" name="HDN_Azione" runat="server">
								<INPUT id="Hidden2" type="hidden" name="HDN_Path" runat="server">
								<asp:Panel id="Panel3" HorizontalAlign="center" Runat="server">
									<table align="left" width="100%">
										<tr>
											<td colspan=2 align=center >
												<asp:Table Runat=server ID="Table2">
													<asp:TableRow ID="TableRow1">
														<asp:TableCell ColumnSpan=2 HorizontalAlign=Center>
															<fieldset>
															<legend><label class="tableLegend">Filtro</label></legend>
																				
															<asp:Table ID="Table3" Runat=server HorizontalAlign=Center>
																<asp:TableRow Height=25px>
																	<asp:TableCell ColumnSpan=5>
																		<asp:Table ID="Table4" Runat=server CellPadding=2 CellSpacing=2 BorderStyle=None>
																			<asp:TableRow>
																				<asp:TableCell ID="TableCell1">
																				<asp:Label ID="Label3" Runat=server CssClass="FiltroVoce">Organizzazione:&nbsp;</asp:Label>
																				</asp:TableCell>
																				<asp:TableCell ID="TableCell2">
																					<asp:DropDownList ID="DropDownList1" Runat=server AutoPostBack=True CssClass="FiltroCampo"></asp:DropDownList>
																				</asp:TableCell>
																				<asp:TableCell>
																					<asp:Label ID="Label4" Runat=server CssClass="FiltroVoce">Tipo Comunità:&nbsp;</asp:Label>
																				</asp:TableCell>
																				<asp:TableCell>
																					<asp:dropdownlist id="Dropdownlist2" runat="server" CssClass="FiltroCampo" AutoPostBack="true"></asp:dropdownlist>
																				</asp:TableCell>
																			</asp:TableRow>
																		</asp:Table>														
																	</asp:TableCell>
																</asp:TableRow>
																<asp:TableRow ID= "TableRow2" Runat=server Visible=False>
																	<asp:TableCell ColumnSpan=5 Height=25px>
																		<asp:Table ID="Table5" CellPadding=2 CellSpacing=2 BorderStyle=None Runat=server>
																			<asp:TableRow>
																				<asp:TableCell>
																					<asp:Label ID="Label5" Runat=server CssClass="FiltroVoce">A.A.:&nbsp;</asp:Label>
																				</asp:TableCell>
																				<asp:TableCell>
																					<asp:DropDownList ID="DropDownList3" Runat=server Runat=server AutoPostBack=True CssClass="FiltroCampo"></asp:DropDownList>
																				</asp:TableCell>
																				<asp:TableCell>
																					<asp:Label ID="Label6" Runat=server CssClass="FiltroVoce">Periodo:&nbsp;</asp:Label>
																				</asp:TableCell>
																				<asp:TableCell>
																					<asp:DropDownList ID="DropDownList4" Runat=server Runat=server AutoPostBack=True CssClass="FiltroCampo"></asp:DropDownList>
																				</asp:TableCell>
																			</asp:TableRow>
																		</asp:Table>
																	</asp:TableCell>
																</asp:TableRow>
															</asp:Table>
															</fieldset>
														</asp:TableCell>
													</asp:TableRow>
													<asp:TableRow >
														<asp:TableCell>
															<asp:RadioButtonList ID="RadioButtonList1" AutoPostBack=True Runat=server RepeatDirection=Horizontal >
																<asp:ListItem Value=3>elenco semplice</asp:ListItem>
																<asp:ListItem Selected=True Value=1>albero comunità</asp:ListItem>
																<asp:ListItem Value=2>albero comunità organizzativo</asp:ListItem>
															</asp:RadioButtonList>
														</asp:TableCell>
														<asp:TableCell HorizontalAlign=Right  cssclass="top">
															<img src="./../images/search.gif" alt="" align=absmiddle >
															<asp:LinkButton ID="LinkButton2" Runat=server>Aggiorna albero</asp:LinkButton>
															&nbsp;|&nbsp;
															<asp:LinkButton ID="LinkButton3" Runat=server>Espandi</asp:LinkButton>
															&nbsp;|&nbsp;
															<asp:LinkButton ID="LinkButton4" Runat=server>Comprimi</asp:LinkButton>
														</asp:TableCell>
													</asp:TableRow>
																		
													<asp:TableRow>
														<asp:TableCell ColumnSpan=2 HorizontalAlign=Left >
															<radt:RadTreeView id="RadTreeView1" runat="server" align="left" width="100%"
																CausesValidation="False"  ContextMenuContentFile="~/RadControls/TreeView/Skins/Comunita/ContextMenus.xml"
																CssFile="~/RadControls/TreeView/Skins/Comunita/StyleAdmin.css" PathToJavaScript="~/Jscript/RadTreeView_Client_3_1.js"
																ImagesBaseDir="~/RadControls/TreeView/Skins/Comunita/" skin="Comunita">
															</radt:RadTreeView>
														</asp:TableCell>
													</asp:TableRow>
												</asp:Table>
											</td>
										</tr>
									</table>
								</asp:Panel>
							</td>
						</tr>
					</table>
				</asp:panel>
				<asp:panel id="Panel4" Runat="server" HorizontalAlign="Center" Visible="False">
					<table width=500 align=center border=0>
						<tr>
							<td align=center colspan=2>
							<FIELDSET>
								<LEGEND class=tableLegend>Dettagli comunità</LEGEND>
								<input type="hidden" id="Hidden3" runat="server" NAME="HDNcmnt_ID">
								<input type="hidden" id="Hidden4" runat="server" NAME="HDNcmnt_Path">
																
								<DETTAGLI:CTRLDettagli id="CTRLDettagli1" runat="server"></DETTAGLI:CTRLDettagli>	
								</FIELDSET> 
							</td>
						</tr>
						<tr>
							<td><asp:Button ID="Button1" Runat="server" Text="Indietro" CssClass="Pulsante"></asp:Button></td>
							<td align=right><asp:Button ID="Button2" Runat="server" Text="Entra" CssClass="Pulsante" Visible=False ></asp:Button></td>
						</tr>
					</table>
				</asp:panel>
				<asp:panel id="Panel5" Runat="server" Visible="False">
					<table cellSpacing=0 cellPadding=0 align=center border=0>
						<tr>
							<td height=30>&nbsp;</td>
						</tr>
						<tr>
							<td>
							<asp:Label id=Label7 Runat="server" CssClass="avviso"></asp:Label>
							</td>
						</tr>
						<tr>
							<td align=right>
								<asp:Button id=Button3 Runat="server" CssClass="Pulsante" Text="Seleziona Ancora"></asp:Button>&nbsp;&nbsp;&nbsp; 
							</td>
						</tr>
					</table>
				</asp:panel>
			</td>
		</tr>
	</table>
</asp:Content>


<%--<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<html>
  <head runat="server">
    <title>Comunità On Line - Albero Comunità</title>
    <meta name="GENERATOR" content="Microsoft Visual Studio .NET 7.1"/>
    <meta name="CODE_LANGUAGE" content="Visual Basic .NET 7.1"/>
    <meta name=vs_defaultClientScript content="JavaScript"/>
    <meta name=vs_targetSchema content="http://schemas.microsoft.com/intellisense/ie5"/>
	<LINK href="./../Styles.css" type="text/css" rel="stylesheet"/>

  </head>
  <body >

		<form id="aspnetForm" method="post" runat="server">
		<asp:ScriptManager ID="SCMmanager" runat="server"></asp:ScriptManager>
			<table cellSpacing="0" cellPadding="0" width="780" align="center" border="0">
				<tr>
					<td colSpan="3"><HEADER:CtrLHEADER id="Intestazione" runat="server"></HEADER:CtrLHEADER></td>
				</tr>
				<tr>
					<td colSpan="3">
						<table width="900px" align="center">
							<tr>
								<td bgColor="#a3b2cd" height="1"></td>
							</tr>
							<tr>
								<td class="titolo" align="center"><asp:label id="LBtitolo" CssClass="TitoloServizio" Runat="server">- Elenco comunità -</asp:label></td>
							</tr>
							<tr>
								<td bgColor="#a3b2cd" height="1"></td>
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
										<table align="left" width="100%">
											<tr>
												<td align="center">
													<INPUT id="HDN_Azione" type="hidden" name="HDN_Azione" runat="server">
													<INPUT id="HDN_Path" type="hidden" name="HDN_Path" runat="server">
													<asp:Panel id="PNLtreeView" HorizontalAlign="center" Runat="server">
														<table align="left" width="100%">
															<tr>
																<td colspan=2 align=center >
																	<asp:Table Runat=server ID="Table1">
																		<asp:TableRow ID="TBRfiltro">
																			<asp:TableCell ColumnSpan=2 HorizontalAlign=Center>
																				<fieldset>
																				<legend><label class="tableLegend">Filtro</label></legend>
																				
																				<asp:Table ID="TBLfiltro" Runat=server HorizontalAlign=Center>
																					<asp:TableRow Height=25px>
																						<asp:TableCell ColumnSpan=5>
																							<asp:Table Runat=server CellPadding=2 CellSpacing=2 BorderStyle=None>
																								<asp:TableRow>
																									<asp:TableCell ID="TBCorganizzazione_c">
																									<asp:Label ID="LBorganizzazione_c" Runat=server CssClass="FiltroVoce">Organizzazione:&nbsp;</asp:Label>
																									</asp:TableCell>
																									<asp:TableCell ID="TBCorganizzazione">
																										<asp:DropDownList ID="DDLorganizzazione" Runat=server AutoPostBack=True CssClass="FiltroCampo"></asp:DropDownList>
																									</asp:TableCell>
																									<asp:TableCell>
																										<asp:Label ID="LBtipoFiltro" Runat=server CssClass="FiltroVoce">Tipo Comunità:&nbsp;</asp:Label>
																									</asp:TableCell>
																									<asp:TableCell>
																										<asp:dropdownlist id="DDLTipo" runat="server" CssClass="FiltroCampo" AutoPostBack="true"></asp:dropdownlist>
																									</asp:TableCell>
																								</asp:TableRow>
																							</asp:Table>														
																						</asp:TableCell>
																					</asp:TableRow>
																					<asp:TableRow ID= "TBRcorsi" Runat=server Visible=False>
																						<asp:TableCell ColumnSpan=5 Height=25px>
																							<asp:Table ID="TBLcorsi" CellPadding=2 CellSpacing=2 BorderStyle=None Runat=server>
																								<asp:TableRow>
																									<asp:TableCell>
																										<asp:Label ID="LBannoAccademico_c" Runat=server CssClass="FiltroVoce">A.A.:&nbsp;</asp:Label>
																									</asp:TableCell>
																									<asp:TableCell>
																										<asp:DropDownList ID="DDLannoAccademico" Runat=server Runat=server AutoPostBack=True CssClass="FiltroCampo"></asp:DropDownList>
																									</asp:TableCell>
																									<asp:TableCell>
																										<asp:Label ID="LBperiodo_c" Runat=server CssClass="FiltroVoce">Periodo:&nbsp;</asp:Label>
																									</asp:TableCell>
																									<asp:TableCell>
																										<asp:DropDownList ID="DDLperiodo" Runat=server Runat=server AutoPostBack=True CssClass="FiltroCampo"></asp:DropDownList>
																									</asp:TableCell>
																								</asp:TableRow>
																							</asp:Table>
																						</asp:TableCell>
																					</asp:TableRow>
																				</asp:Table>
																				</fieldset>
																			</asp:TableCell>
																		</asp:TableRow>
																		<asp:TableRow >
																			<asp:TableCell>
																				<asp:RadioButtonList ID="RBLvisualizza" AutoPostBack=True Runat=server RepeatDirection=Horizontal >
																					<asp:ListItem Value=3>elenco semplice</asp:ListItem>
																					<asp:ListItem Selected=True Value=1>albero comunità</asp:ListItem>
																					<asp:ListItem Value=2>albero comunità organizzativo</asp:ListItem>
																				</asp:RadioButtonList>
																			</asp:TableCell>
																			<asp:TableCell HorizontalAlign=Right  cssclass="top">
																				<img src="./../images/search.gif" alt="" align=absmiddle >
																				<asp:LinkButton ID="LNBaggiorna" Runat=server>Aggiorna albero</asp:LinkButton>
																				&nbsp;|&nbsp;
																				<asp:LinkButton ID="LNBespandi" Runat=server>Espandi</asp:LinkButton>
																				&nbsp;|&nbsp;
																				<asp:LinkButton ID="LNBcomprimi" Runat=server>Comprimi</asp:LinkButton>
																			</asp:TableCell>
																		</asp:TableRow>
																		
																		<asp:TableRow>
																			<asp:TableCell ColumnSpan=2 HorizontalAlign=Left >
																				<radt:RadTreeView id="RDTcomunita" runat="server" align="left" width="100%"
																					CausesValidation="False"  ContextMenuContentFile="~/RadControls/TreeView/Skins/Comunita/ContextMenus.xml"
																					CssFile="~/RadControls/TreeView/Skins/Comunita/StyleAdmin.css" PathToJavaScript="~/Jscript/RadTreeView_Client_3_1.js"
																					ImagesBaseDir="~/RadControls/TreeView/Skins/Comunita/" skin="Comunita">
																				</radt:RadTreeView>
																			</asp:TableCell>
																		</asp:TableRow>
																	</asp:Table>
																</td>
															</tr>
														</table>
													</asp:Panel>
												</td>
											</tr>
										</table>
									</asp:panel>
									<asp:panel id="PNLdettagli" Runat="server" HorizontalAlign="Center" Visible="False">
										<table width=500 align=center border=0>
											<tr>
												<td align=center colspan=2>
												<FIELDSET>
													<LEGEND class=tableLegend>Dettagli comunità</LEGEND>
													<input type="hidden" id="HDNcmnt_ID" runat="server" NAME="HDNcmnt_ID">
													<input type="hidden" id="HDNcmnt_Path" runat="server" NAME="HDNcmnt_Path">
																
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
									<asp:panel id="PNLmessaggio" Runat="server" Visible="False">
										<table cellSpacing=0 cellPadding=0 align=center border=0>
											<tr>
												<td height=30>&nbsp;</td>
											</tr>
											<tr>
												<td>
												<asp:Label id=LBMessaggio Runat="server" CssClass="avviso"></asp:Label>
												</td>
											</tr>
											<tr>
												<td align=right>
													<asp:Button id=BTNindietro Runat="server" CssClass="Pulsante" Text="Seleziona Ancora"></asp:Button>&nbsp;&nbsp;&nbsp; 
												</td>
											</tr>
										</table>
									</asp:panel>
								</td>
							</tr>
						</table>
					</td>
				</tr>
			</table>
			<FOOTER:CtrLFOOTER id="CtrLFooter" runat="server"></FOOTER:CtrLFOOTER>
		</form>
  </body>
</html>--%>
