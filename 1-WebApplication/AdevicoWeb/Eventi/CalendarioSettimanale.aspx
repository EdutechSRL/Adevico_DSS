<%@ Page Language="vb" AutoEventWireup="false" MasterPageFile="~/AjaxPortal.Master" Codebehind="CalendarioSettimanale.aspx.vb" Inherits="Comunita_OnLine.CalendarioSettimanale"%>
<%@ MasterType VirtualPath="~/AjaxPortal.Master" %>

<%@ Register TagPrefix="CORPO2" TagName="CTRLLegenda" Src="Legenda.ascx" %>
<%--
--%>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <style type="text/css">
		A:hover  
		{
		    /*BACKGROUND-COLOR: white;*/ 
		    TEXT-DECORATION: underline overline;
		}
	</style>
    <script language="Javascript" src="./../jscript/generali.js" type="text/javascript"></script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="CPHservice" runat="server">
    <table cellspacing="0" cellpadding="0" width="100%" border="0">
<%--		<tr>
			<td class="RigaTitolo" align="left">
				<asp:label id="LBtitolo" Runat="server">Calendario Settimanale</asp:label>
			</td>
		</tr>--%>
		<tr>
			<td>&nbsp;</td>
		</tr>
		<tr>
			<td>
				<asp:panel id="PNLpermessi" Runat="server" Visible="False">
					<table align="center">
						<tr>
							<td height="50">&nbsp;</td>
						</tr>
						<tr>
							<td align="center">
								<asp:Label id="LBnopermessi" CssClass="messaggio" ForeColor="Blue" Runat="server"></asp:Label></td>
						</tr>
						<tr>
							<td valign="top" height="50" align="center">
								<asp:LinkButton id="LNBnascondi" Runat="server">OK</asp:LinkButton>&nbsp;
							</td>
						</tr>
					</table>
				</asp:panel>
				<table cellspacing="0" cellpadding="0" width="100%" border="0">
					<tr class="Eventi_tabella">
						<td class="Eventi_tabella" valign="top">
							<asp:panel id="PNLcontenuto" BackColor="#dfeae2" Runat="server">
								<table>
									<tr class="Eventi_tabella">
														
										<td class="Eventi_tabella" bgcolor="#dfeae2">
															
											<asp:Panel ID="PNLLeft" Height="100%" BackColor="#dfeae2" Runat="server" CssClass="Eventi_tabella">
												<asp:table ID="TBLsettimana" Runat="server" CssClass="Eventi_tabella">
													<asp:TableRow Runat="server" ID="Tablerow1">
														<asp:TableCell>
															<table align="left" border="0" style="border-color:#ffffff" cellpadding="0" cellspacing="1" bgcolor="#ffe7c6">
																<tr align="center">
																	<asp:Repeater ID="RPTpulsanti" Runat="server">
																		<ItemTemplate>
																			<td align="center" colspan="2" style="border-color:#ffffd9">
																				<asp:Button ID="BTNtemp" Runat="server" CssClass="Eventi_PulsanteGiorno" OnClick="ZoomGiorno"></asp:Button>
																			</td>
																		</ItemTemplate>
																	</asp:Repeater>
																</tr>
																<asp:Repeater ID="RPTore" Runat="server">
																	<ItemTemplate>
																		<tr valign="bottom">
																			<td align="center" valign="bottom" bgcolor="#fffee7">
																				<font class="Eventi_CellaOre">
																					<%#Container.DataItem("rigaOre")%>
																				</font>
																			</td>
																			<td width="1px"></td>
																			<asp:Repeater ID="RPTgiorni" Runat="server" OnItemCreated="RPTgiorni_ItemCreated">
																				<ItemTemplate>
																					<td id="CellaColore" runat="server" class="Eventi_CellaTipoEvento" bgcolor="#ffffff">&nbsp;</td>
																					<td id="CellaG" runat="server" class="Eventi_CellaGiorno" bgcolor="#fffee7">
																						<asp:LinkButton ID="LKB_temp" OnClick="DettaglioEvento" CssClass="Eventi_Linkbutton" Runat="server"
																							Visible="False"></asp:LinkButton>
																					</td>
																				</ItemTemplate>
																			</asp:Repeater>
																		</tr>
																	</ItemTemplate>
																</asp:Repeater>
															</table>
														</asp:TableCell>
													</asp:TableRow>
													<asp:TableRow>
														<asp:TableCell BackColor="AliceBlue">
															<CORPO2:CTRLLEGENDA id="Legenda1" runat="server"></CORPO2:CTRLLEGENDA>
														</asp:TableCell>
													</asp:TableRow>
												</asp:table>
											</asp:Panel>
										</td>
										<td class="Eventi_tabella">
											<asp:Panel ID="PNLRight" HorizontalAlign="Center" Runat="server" Width="200px" Height="100%"
												BackColor="#f1ffee">
												<table width="100%" cellpadding="2" cellspacing="2" border="1" style="border-color:#ffffff" bgcolor="#dfeae2"
													align="center">
													<tr align="center">
														<td colspan="4"><asp:Label Runat="server" CssClass="Eventi_Label" ID="LBGestEVNT">GESTIONE EVENTI</asp:Label></td>
													</tr>
													<tr align="center">
														<td><asp:linkbutton ID="LKBInserisciEvento" Runat="server" CssClass="Eventi_Label">inserisci</asp:linkbutton></td>
														<td><asp:linkbutton ID="LKBmodifica" Runat="server" CssClass="Eventi_Label">modifica</asp:linkbutton></td>
														<td><asp:linkbutton ID="LKBelimina" Runat="server" CssClass="Eventi_Label">elimina</asp:linkbutton></td>
														<td><asp:linkbutton ID="LKBtrova" Runat="server" CssClass="Eventi_Label">trova</asp:linkbutton></td>
													</tr>
												</table>
												<asp:Label ID="LBLMessaggio" CssClass="Eventi_Label" ForeColor="#dc143c" BackColor="#ffd700"
													Runat="server" Visible="False"></asp:Label>
												<asp:Table Visible="False" Runat="server" ID="TBLElimina" BackColor="#dfeae2" HorizontalAlign="Center">
													<asp:TableRow>
														<asp:TableCell ColumnSpan="2">
															<asp:radiobuttonlist id="RBLEliminaEventi" Runat="server" CssClass="Eventi_Label" RepeatLayout="Flow"
																Repeatdirection="Vertical">
																<asp:ListItem Value="0" Selected="True">solo questo evento</asp:ListItem>
																<asp:ListItem Value="1">tutte le ricorrenze</asp:ListItem>
															</asp:radiobuttonlist>
														</asp:TableCell>
													</asp:TableRow>
													<asp:TableRow>
														<asp:TableCell>
															<asp:Button ID="BTNElimina" Runat="server" Text="ELIMINA" CssClass="PulsantePiccolo"></asp:Button>
															<asp:Button ID="BTNModifica" Runat="server" Text="MODIFICA" Visible="False" CssClass="PulsantePiccolo"></asp:Button>
														</asp:TableCell>
														<asp:TableCell>
															<asp:Button ID="BTNAnnulla" Runat="server" Text="ANNULLA" CssClass="PulsantePiccolo"></asp:Button>
														</asp:TableCell>
													</asp:TableRow>
												</asp:Table>
												<asp:Panel ID="PNLcalendarioGiornaliero" Visible="False" Runat="server">
													<asp:LinkButton ID="LKBbackTOall" Runat="server" CssClass="Eventi_LabelBold"><-- vista settimanale</asp:LinkButton>
												</asp:Panel>
												<br/>
												<asp:Panel ID="PNLfiltriSettimana" HorizontalAlign="Center" Runat="server" Visible="True" Width="100%">
													<table align="center" cellspacing="0" width="100%">
														<tr bgcolor="#8399b2">
															<td colspan="2"></td>
														</tr>
														<tr align="center">
															<td colspan="2" bgcolor="#dfeae2">
																<asp:LinkButton ID="LKBSettPrec" Runat="server" CssClass="Eventi_LabelBold"><<</asp:LinkButton>
																&nbsp;&nbsp;&nbsp;<asp:Label ID="LBsettAttuale" Runat="server" CssClass="Eventi_LabelBold"></asp:Label>&nbsp;&nbsp;&nbsp;
																<asp:LinkButton ID="LKBsettSucc" Runat="server" CssClass="Eventi_LabelBold">>></asp:LinkButton>
															</td>
														</tr>
														<tr bgcolor="#8399b2">
															<td colspan="2"></td>
														</tr>
														<tr>
															<td align="center" colspan="2" bgcolor="#dfeae2">
																<asp:Label Runat="server" CssClass="Eventi_Label" ID="LBLFiltri">FILTRO CALENDARIO</asp:Label>
															</td>
														</tr>
														<tr align="center">
															<td class="Eventi_cella" bgcolor="#dfeae2">
																<asp:Label Runat="server" CssClass="Eventi_Label" ID="LBLfiltroORE">ore: </asp:Label>
																<asp:DropDownList ID="DDLfiltraOre" Runat="server" AutoPostBack="True" CssClass="Eventi_Label"></asp:DropDownList>
															</td>
															<td class="Eventi_cella" bgcolor="#dfeae2">
																<asp:Button ID="BTNfiltraGiorni" Runat="server" Text="giorni..." CssClass="PulsantePiccolo"></asp:Button>
															</td>
														</tr>
														<tr>
															<td colspan="2" bgcolor="#dfeae2">
																<asp:Panel ID="PNLfiltroGiorni" Runat="server" Visible="False">
																	<table cellspacing="0">
																		<tr>
																			<td width="80%">
																				<asp:CheckBoxList ID="CBXLfiltroGiorni" Runat="server" RepeatDirection="Vertical" RepeatLayout="Flow"
																					CssClass="Eventi_Label"></asp:CheckBoxList>
																			</td>
																			<td align="center" width="20%">
																				<asp:LinkButton ID="LKBtuttiIgiorni" Runat="server" CssClass="Eventi_label">&nbsp;&nbsp; tutti&nbsp;&nbsp;</asp:LinkButton><br/>
																				<br/>
																				&nbsp;&nbsp;<asp:Button ID="BTNApplicaFiltroG" Runat="server" Text="applica" CssClass="PulsantePiccolo"></asp:Button>&nbsp;&nbsp;
																			</td>
																		</tr>
																	</table>
																</asp:Panel>
															</td>
														</tr>
														<tr bgcolor="#8399b2">
															<td colspan="2"></td>
														</tr>
														<tr>
															<td align="center" colspan="2" bgcolor="#dfeae2">
																<asp:Label Runat="server" CssClass="Eventi_Label" ID="LBLfiltroEventi">FILTRO EVENTI</asp:Label>
															</td>
														</tr>
														<tr>
															<td align="left" bgcolor="#dfeae2">
																<asp:CheckBox ID="CBEventiTutti" AutoPostBack="True" Checked="True" Runat="server" CssClass="Eventi_Label"
																	Text="tutti"></asp:CheckBox>
															</td>
															<td align="center" bgcolor="#dfeae2">
																<asp:Button ID="BTNfiltroEventi" Runat="server" Text="eventi..." CssClass="PulsantePiccolo"></asp:Button>
															</td>
														</tr>
														<tr>
															<td bgcolor="#dfeae2" width="150px">
																<asp:CheckBoxList ID="CBXLFiltroEventi" Runat="server" CssClass="Eventi_Label" RepeatColumns="1" RepeatDirection="Vertical"
																	RepeatLayout="Flow" Visible="False"></asp:CheckBoxList>
															</td>
															<td bgcolor="#dfeae2" align="center" width="20%">
																<asp:Button ID="BTNApplicaFiltroEventi" Runat="server" Text="applica" CssClass="PulsantePiccolo"
																	Visible="False"></asp:Button>
															</td>
														</tr>
														<tr bgcolor="#8399b2">
															<td colspan="2"></td>
														</tr>
														<tr>
															<td colspan="2" align="center" bgcolor="#dfeae2">
																<asp:Label Runat="server"  CssClass="Eventi_Label" ID="LBFiltroComunita" Visible="False">FILTRO COMUNITA</asp:Label><br/>
																<asp:radiobuttonlist id="RBLFiltroComunita" Runat="server" CssClass="Eventi_Label" Visible="False" RepeatLayout="Flow"
																	Repeatdirection="Horizontal" Autopostback="true">
																	<asp:ListItem Value="-1">tutte</asp:ListItem>
																	<asp:ListItem Value="0">corrente</asp:ListItem>
																</asp:radiobuttonlist>
															</td>
														</tr>
														<tr bgcolor="#8399b2">
															<td colspan="2"></td>
														</tr>
													</table>
												</asp:Panel>
												<asp:Table ID="TBCalMini" Visible="True" Runat="server" CellPadding="0" CellSpacing="0" BorderWidth="1"
													HorizontalAlign="Center">
													<asp:TableRow BackColor="DarkGray" BorderWidth="1" BorderColor="DarkBlue">
														<asp:TableCell HorizontalAlign="Center">
															<asp:Label Runat="server" CssClass="Eventi_Label" ID="LBLCLNminiMESE">Mese </asp:Label>
															<br/>
															<asp:DropDownList ID="DDLVaiA_mesi" Runat="server" CssClass="Eventi_Label" AutoPostBack="True"></asp:DropDownList>
														</asp:TableCell>
														<asp:TableCell HorizontalAlign="Center">
															<asp:Label Runat="server" CssClass="Eventi_Label" ID="LBLCLNminiANNO">Anno </asp:Label>
															<br/>
															<asp:DropDownList ID="DDLVaiA_anni" Runat="server" CssClass="Eventi_Label" AutoPostBack="True"></asp:DropDownList>
														</asp:TableCell>
														<asp:TableCell HorizontalAlign="Center">
															<asp:Label Runat="server" CssClass="Eventi_Label" ID="LBLvaiA">vai a: </asp:Label>
															<br/>
															<asp:Button ID="BTNVaiA_oggi" Runat="server" CssClass="PulsantePiccolo" Text="oggi"></asp:Button>
														</asp:TableCell>
													</asp:TableRow>
													<asp:TableRow>
														<asp:TableCell ColumnSpan="3">
															<asp:Calendar ID="CLNmini" Runat="server" CellSpacing="0" CellPadding="0" DayStyle-Font-Size="8"
																DayHeaderStyle-Font-Size="7" NextPrevStyle-Font-Size="7" TitleStyle-Font-Size="7" WeekendDayStyle-ForeColor="#a32222"
																TitleStyle-Font-Name="tahoma" SelectionMode="DayWeek" DayNameFormat="FirstTwoLetters" DayStyle-BackColor="#e7e9ee"
																SelectedDayStyle-BackColor="white" SelectedDayStyle-ForeColor="black" TitleStyle-Font-Bold="true"
																TitleStyle-ForeColor="DarkBlue" OtherMonthDayStyle-ForeColor="DarkGray" TodayDayStyle-ForeColor="Firebrick"
																BorderColor="DarkBlue" TodayDayStyle-BackColor="#a6cade" SelectedDayStyle-BorderColor="CornflowerBlue"
																SelectedDayStyle-BorderWidth="1" TodayDayStyle-BorderColor="CornflowerBlue" TodayDayStyle-Font-Bold="true" FirstDayOfWeek=Monday
																TitleStyle-BorderWidth="1" PrevMonthText="mese precedente" NextMonthText="mese successivo"></asp:Calendar>
														</asp:TableCell>
													</asp:TableRow>
												</asp:Table>
												<asp:LinkButton ID="LKBvediCALmini" Visible="False" Runat="server" CssClass="Eventi_LabelBold">vedi calendario...</asp:LinkButton>
												<asp:Panel ID="PNLdettaglioEvento" Runat="server" Visible="False" HorizontalAlign="Center"
													Width="200px">
													<table align="center" bgcolor="#f1ffee" width="200px" style="BORDER-RIGHT: teal 1px solid; BORDER-TOP: teal 1px solid; BORDER-LEFT: teal 1px solid; BORDER-BOTTOM: teal 1px solid">
														<tr>
															<td align="left" colspan="2">
																<table width="100%" style="BORDER-BOTTOM: teal 1px solid">
																	<tr>
																		<td width="90%" align="center"><asp:Label text="DETTAGLIO EVENTO" Runat="server" CssClass="Eventi_LabelBold" ID="LBLdettEventoTitolo"></asp:Label></td>
																		<td width="10%" align="center" style="BORDER-LEFT: teal 1px solid; "><asp:linkbutton ID="LKBchiudiDettaglio" Runat="server" CssClass="Eventi_Label">X</asp:linkbutton></td>
																	</tr>
																</table>
															</td>
														</tr>
														<tr>
															<td align="left" class="Eventi_tabella" style="border-color:#f1ffee" width="30%">
																<asp:label Runat="server" text="Evento " CssClass="Eventi_LabelDettagliBold" ID="LBLdettEventoNome"></asp:label>
															</td>
															<td class="Eventi_tabella">
																<asp:label id="LBNomeEvento" Runat="server" CssClass="Eventi_Label"></asp:label>
															</td>
														</tr>
														<tr>
															<td align="left" class="Eventi_tabella">
																<asp:label ID="LBLdettEventoCNMT" Runat="server" text="Comunità " CssClass="Eventi_LabelDettagli"></asp:label>
															</td>
															<td class="Eventi_tabella">
																<asp:label id="LBComunita" Runat="server" CssClass="Eventi_Label"></asp:label>
															</td>
														</tr>
														<tr>
															<td align="left" class="Eventi_tabella">
																<asp:label ID="LBLdettEventoTipo" Runat="server" text="Tipo " CssClass="Eventi_LabelDettagli"></asp:label>
																&nbsp;&nbsp;
															</td>
															<td class="Eventi_tabella">
																<asp:label id="LBTipo" Runat="server" CssClass="Eventi_Label"></asp:label>
															</td>
														</tr>
														<tr>
															<td align="left" class="Eventi_tabella">
																<asp:label ID="LBLdettEventoInizio" Runat="server" text="Inizio " CssClass="Eventi_LabelDettagliBold"></asp:label>
																&nbsp;&nbsp;
															</td>
															<td class="Eventi_tabella">
																<asp:label id="LBInizio" Runat="server" CssClass="Eventi_Label"></asp:label>
															</td>
														</tr>
														<tr>
															<td align="left" class="Eventi_tabella">
																<asp:label ID="LBLdettEventoFine" Runat="server" text="Fine " CssClass="Eventi_LabelDettagliBold"></asp:label>
																&nbsp;&nbsp;
															</td>
															<td class="Eventi_tabella">
																<asp:label id="LBfine" Runat="server" CssClass="Eventi_Label"></asp:label>
															</td>
														</tr>
														<tr>
															<td align="left" class="Eventi_tabella">
																<asp:label ID="LBLdettEventoLink" Runat="server" text="Link " CssClass="Eventi_LabelDettagli"></asp:label>
																&nbsp;&nbsp;
															</td>
															<td class="Eventi_tabella">
																<asp:hyperlink ID="HLlink" Runat="server" Target="_blank"></asp:hyperlink>
															</td>
														</tr>
														<tr>
															<td align="left" class="Eventi_tabella">
																<asp:label ID="LBLdettEventoLuogo" Runat="server" text="luogo " CssClass="Eventi_LabelDettagli"></asp:label>
																&nbsp;&nbsp;
															</td>
															<td class="Eventi_tabella">
																<asp:label id="LBLuogo" Runat="server"></asp:label>
																<asp:label id="LBaula" Runat="server"></asp:label>
															</td>
														</tr>
														<tr>
															<td align="right" colspan="2" style="BORDER-TOP: teal 1px solid">
																|&nbsp;<asp:linkbutton ID="LKBzoom" Runat="server" CssClass="Eventi_Label"> ZOOM </asp:linkbutton>
																&nbsp;|&nbsp;<asp:linkbutton ID="LKBIcalendar" Runat="server" CssClass="Eventi_Label">get iCal</asp:linkbutton>
																&nbsp;|&nbsp;<asp:linkbutton ID="LKBcreaReminder" Runat="server" CssClass="Eventi_Label">crea reminder</asp:linkbutton>
															</td>
														</tr>
														<tr>
															<td colspan="2">
																<asp:label id="LBAvviso" Visible="False" Runat="server" CssClass="Eventi_Label" text="Tipo di Avviso: "></asp:label>
																<asp:dropdownlist id="DDLTipoAvviso" runat="server" Visible="false" CssClass="Eventi_Label"></asp:dropdownlist>
															</td>
														</tr>
														<tr>
														<td colspan=2 align=right>
																<asp:Button ID="BTNCreaReminder" Runat="server" Text="CREA" CssClass="PulsantePiccolo" Visible="False"></asp:Button>
																<asp:Button ID="BTNAnnullaCreazRem" Runat="server" Text="ANNULLA" CssClass="PulsantePiccolo" Visible="False"></asp:Button>														
														</td>
														</tr>
													</table>
												</asp:Panel>
												<table width="100%" cellpadding="2" cellspacing="2" border="1" style="border-color:#ffffff" bgcolor="#dfeae2"
													align="center">
													<tr align="center">
														<td colspan="2"><asp:Label Runat="server" CssClass="Eventi_Label" ID="LBLVisualizzazione">visualizzazione</asp:Label></td>
														<td><asp:linkbutton ID="LKBgoTOmensile" Runat="server" CssClass="Eventi_Label" Enabled="true">mensile</asp:linkbutton></td>
														<td><asp:linkbutton ID="LKBgoTOannuale" Runat="server" CssClass="Eventi_Label">annuale</asp:linkbutton></td>
													</tr>
													<tr>
														<td colspan="5" align="center">
															<asp:linkbutton ID="LKBexport" Runat="server" CssClass="Eventi_Label">EXPORT in EXCELL</asp:linkbutton>
														</td>
													</tr>
												</table>
											</asp:Panel>
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
    <asp:Table id="TBLExport" Runat="server"></asp:Table>
</asp:Content>


<%--<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<html>
	<head runat="server">
		<title>Comunità On Line - Calendario Settimanale</title>
		<meta name="GENERATOR" content="Microsoft Visual Studio .NET 7.1"/>
		<meta name="CODE_LANGUAGE" content="Visual Basic .NET 7.1"/>
		<script language="Javascript" src="./../jscript/generali.js" type="text/javascript"></script>
		<meta name="vs_defaultClientScript" content="JavaScript"/>
		<LINK href="./../Styles.css" type="text/css" rel="stylesheet"/>
		<meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5"/>

	</head>
	<body >
		<form id="aspnetForm" method="post" runat="server">
		<asp:ScriptManager ID="SCMmanager" runat="server"></asp:ScriptManager>
			<table id="table1" cellSpacing="1" align="center" cellPadding="1" width="780" border="0">
				<tr>
					<td align="center"><HEADER:CtrLHEADER id="Intestazione" runat="server"></HEADER:CtrLHEADER></td>
				</tr>
				<tr>
					<td>
						
					</td>
				</tr>
			</table>
			<FOOTER:CTRLFOOTER id="Piede" runat="server"></FOOTER:CTRLFOOTER>
			
		</form>
	</body>
</html>
--%>