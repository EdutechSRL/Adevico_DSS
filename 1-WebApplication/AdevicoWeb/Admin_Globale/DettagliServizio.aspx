<%@ Page Language="vb" AutoEventWireup="false" Codebehind="DettagliServizio.aspx.vb" Inherits="Comunita_OnLine.DettagliServizio"%>
<!DOCTYPE HTML PUBLIC "-//W3C//Dtd HTML 4.0 transitional//EN">
<HTML>
	<head runat="server">
		<title>Comunità On Line - Dettagli Servizio</title>
		<meta name="GENERATOR" content="Microsoft Visual Studio .NET 7.1"/>
		<meta name="CODE_LANGUAGE" content="Visual Basic .NET 7.1"/>
		<meta name="vs_defaultClientScript" content="JavaScript"/>
		<meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5"/>
		<LINK href="./../Styles.css" type="text/css" rel="stylesheet"/>
		<script type="text/javascript" language="Javascript" src="./../jscript/generali.js"></script>
	</HEAD>
	<body>
		<form id="aspnetForm" method="post" runat="server">
		<asp:ScriptManager ID="SCMmanager" runat="server"></asp:ScriptManager>
			<asp:Panel ID="PNLimpostazioni" Runat="server" Visible="true" HorizontalAlign="Center">
				<table align="center" border="0" width="100%" cellpadding=0 cellspacing=0>
					<tr>
						<td class="RigaTitolo" colspan=3>
							<asp:Label id="LBtitolo" Runat="server">Dettagli servizio</asp:Label>
						</td>
					</tr>
					<tr>
						<td align="right" colspan=3>
							<asp:LinkButton ID="LNBchiudi" Runat=server CssClass="LINK_MENU" CausesValidation=False>Chiudi Finestra(x)</asp:LinkButton>
						</td>
					</tr>
					<tr>
						<td class="nosize0">&nbsp;</td>
						<td align="center">
							<table border="0" cellpadding=4 width="550px">
								<tr>
									<td class="top" width=90px>
										<asp:Label id="LBinfoNome_t" Runat="server" CssClass="Titolo_CampoSmall">Nome:&nbsp;</asp:Label>
									</td>
									<td>
										<asp:Label id="LBnome" Runat="server" CssClass="Testo_CampoSmall"></asp:Label>
									</td>
								</tr>
								<tr>
									<td class="nosize0" height="1px">&nbsp;</td>
									<td class="dettagli_separatore" height="1px"><img src="./../images/separatore_small.gif" alt="" align="top" border="0" width="100%" height="1px"/></td>
								</tr>
								<tr>
									<td class="top" width=90px><asp:Label id="LBinfoDescrizione_t" Runat="server" CssClass="Titolo_CampoSmall">Descrizione:&nbsp;</asp:Label></td>
									<td class="Testo_CampoSmall"><asp:Label id="LBdescrizione" Runat="server" CssClass="Testo_CampoSmall"></asp:Label></td>
								</tr>
								<tr>
									<td class="nosize0" height="1px">&nbsp;</td>
									<td class="dettagli_separatore" height="1px"><img src="./../images/separatore_small.gif" alt="" align="top" border="0" width="100%" height="1px"/></td>
								</tr>
								<tr>
									<td class="top" width=90px><asp:Label id="LBinfoCodice_t" Runat="server" CssClass="Titolo_CampoSmall">Codice:&nbsp;</asp:Label></td>
									<td class="Testo_CampoSmall"><asp:Label id="LBcodice" Runat="server" CssClass="Testo_CampoSmall"></asp:Label></td>
								</tr>
								<tr>
									<td class="nosize0" height="1px">&nbsp;</td>
									<td class="dettagli_separatore" height="1px"><img src="./../images/separatore_small.gif" alt="" align="top" border="0" width="100%" height="1px"/></td>
								</tr>
								<tr>
									<td class="top" width=90px><asp:Label id="LBinfoAttivo_t" Runat="server" CssClass="Titolo_CampoSmall">Attivo:&nbsp;</asp:Label></td>
									<td class="Testo_CampoSmall"><asp:Label id="LBattivo" Runat="server" CssClass="Testo_CampoSmall"></asp:Label></td>
								</tr>
								<tr>
									<td class="nosize0" height="1px">&nbsp;</td>
									<td class="dettagli_separatore" height="1px"><img src="./../images/separatore_small.gif" alt="" align="top" border="0" width="100%" height="1px"/></td>
								</tr>
								<tr>
									<td class="top" width=90px><asp:Label id="LBinfoDisattivabile_t" Runat="server" CssClass="Titolo_CampoSmall">Disattivabile:&nbsp;</asp:Label></td>
									<td class="Testo_CampoSmall"><asp:Label id="LBdisattivabile" Runat="server" CssClass="Testo_CampoSmall"></asp:Label></td>
								</tr>
								<tr>
									<td class="nosize0" height="1px">&nbsp;</td>
									<td class="dettagli_separatore" height="1px"><img src="./../images/separatore_small.gif" alt="" align="top" border="0" width="100%" height="1px"/></td>
								</tr>
								<tr>
									<td class="top" width=90px><asp:Label id="LBinfoPermessi_t" Runat="server" CssClass="Titolo_CampoSmall">Permessi:&nbsp;</asp:Label></td>
									<td class="Testo_CampoSmall"><asp:Label id="LBpermessi" Runat="server" CssClass="Testo_CampoSmall"></asp:Label></td>
								</tr>
								<tr>
									<td class="nosize0" height="1px">&nbsp;</td>
									<td class="dettagli_separatore" height="1px"><img src="./../images/separatore_small.gif" alt="" align="top" border="0" width="100%" height="1px"/></td>
								</tr>
								<tr>
									<td class="top" width=90px  nowrap="nowrap" ><asp:Label id="LBinfoTipiComunita_t" Runat="server" CssClass="Titolo_CampoSmall">Associato a:&nbsp;</asp:Label></td>
									<td class="Testo_CampoSmall">
										<asp:Table ID=TBLtipicomunità Runat=server >
										
										</asp:Table>
									</td>
								</tr>
								<tr>
									<td class="nosize0" height="1px">&nbsp;</td>
									<td class="dettagli_separatore" height="1px"><img src="./../images/separatore_small.gif" alt="" align="top" border="0" width="100%" height="1px"/></td>
								</tr>
							</table>
						</td>
					</tr>
				</table>
			</asp:Panel>
		</form>
	</body>
</HTML>
