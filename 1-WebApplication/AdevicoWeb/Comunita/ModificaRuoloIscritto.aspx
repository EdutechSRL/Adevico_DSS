<%@ Page Language="vb" AutoEventWireup="false" Codebehind="ModificaRuoloIscritto.aspx.vb" Inherits="Comunita_OnLine.ModificaRuoloIscritto"%>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<html>
  <head runat="server">
    <title>ModificaRuoloIscritto</title>
    <meta name="GENERATOR" content="Microsoft Visual Studio .NET 7.1"/>
    <meta name="CODE_LANGUAGE" content="Visual Basic .NET 7.1"/>
    <meta name=vs_defaultClientScript content="JavaScript"/>
    <meta name=vs_targetSchema content="http://schemas.microsoft.com/intellisense/ie5"/>
    <LINK href="../Styles.css" type="text/css" rel="stylesheet"/>
  </head>
  <body>

    <form id="aspnetForm" method="post" runat="server">
    <asp:ScriptManager ID="SCMmanager" runat="server"></asp:ScriptManager>
		<asp:Panel ID="PNLmodifica" Runat="server">
			<table border="0" align="center" width="400px">
				<tr>
					<td align=center >
						<FIELDSET><LEGEND class="tableLegend"><asp:label Runat=server id="LBlegenda">Ruolo nella comunità</asp:label></LEGEND><br/>
							<INPUT id="HDrlpc" type="hidden" name="HDrlpc" runat="server"/>
							<INPUT id="HDNprsnID" type="hidden" name="HDNprsnID" runat="server"/>
							<INPUT id="HDN_cmntID" type="hidden" name="HDN_cmntID" runat="server"/>
							<INPUT id="HDNrlpc_Attivato" type="hidden" name="HDNrlpc_Attivato" runat="server"/>
							<INPUT id="HDNrlpc_Abilitato" type="hidden" name="HDNrlpc_Abilitato" runat="server"/>
							<INPUT id="HDNrlpc_Responsabile" type="hidden" name="HDNrlpc_Responsabile" runat="server"/>
						<table border="0" align="center">
							<tr>
								<td colspan=4 height=10px class="nosize0">&nbsp;</td>
							</tr>
							<tr>
								<td width=5px class="nosize0">&nbsp;</td>
								<td>
									<asp:Label ID="LBcomunita_t" CssClass="Titolo_campo" Runat=server >Comunità:&nbsp;</asp:Label>
								</td>
								<td>
									<asp:label id="LBcomunita" Runat="server" CssClass="Testo_campo"></asp:label>
								</td>
								<td width=5px class="nosize0">&nbsp;</td>
							</tr>
							<tr>
								<td width=5px class="nosize0">&nbsp;</td>
								<td>
									<asp:Label ID="LBanagrafica_t" CssClass="Titolo_campo" Runat=server >Anagrafica:&nbsp;</asp:Label>
								</td>
								<td>
									<asp:label id="LBNomeCognome" Runat="server" CssClass="Testo_campo"></asp:label>
								</td>
								<td width=5px class="nosize0">&nbsp;</td>
							</tr>
							<tr>
								<td width=5px class="nosize0">&nbsp;</td>
								<td>
									<asp:Label ID="LBruolo_t" CssClass="Titolo_campo" Runat=server >Ruolo:&nbsp;</asp:Label>
								</td>
								<td>
									<asp:DropDownList id="DDLruolo" Runat="server" CssClass="Testo_campo"></asp:DropDownList>
								</td>
								<td width=5px class="nosize0">&nbsp;</td>
							</tr>
							<tr>
								<td width=5px class="nosize0">&nbsp;</td>
								<td>
									<asp:Label ID="LBabilitato_t" CssClass="Titolo_campo"  Runat=server>Abilitazione:&nbsp;</asp:Label>
								</td>
								<td>
									<asp:LinkButton ID="LNBabilita" Runat=server CausesValidation=False Visible=False >Enable</asp:LinkButton>
									<asp:LinkButton ID="LNBdisabilita" Runat=server CausesValidation=False Visible=False >Disable</asp:LinkButton>
									<asp:LinkButton ID="LNBattiva" Runat=server CausesValidation=False Visible=False >Accept Subscription</asp:LinkButton>
								</td> 
								<td width=5px class="nosize0">&nbsp;</td>
							</tr>
							<tr>
								<td width=5px class="nosize0">&nbsp;</td>
								<td>
									<asp:Label ID="LBresponsabile_t" CssClass="Titolo_campo"  Runat=server >Responsabile:&nbsp;</asp:Label>
								</td>
								<td>
									<asp:CheckBox ID=CHBresponsabile Runat=server Text =Si CssClass="Testo_campo" >
									</asp:CheckBox>
								</td> 
								<td width=5px class="nosize0">&nbsp;</td>
							</tr>
							<tr>
								<td colspan=4>&nbsp;</td>
							</tr>
							<tr align="center">
								<td width=5px class="nosize0">&nbsp;</td>
								<td align="left">
									&nbsp;
								</td>
								<td align="right">
									<asp:button id="BTNmodifica" Runat="server" Text="Modifica" CssClass="pulsante"></asp:Button>
								</td>
								<td width=5px class="nosize0">&nbsp;</td>
							</tr>
							<tr>
								<td colspan=4 height=10px class="nosize0">&nbsp;</td>
							</tr>
						</table>
						<br/>
						</fieldset> 
					</td>
				</tr>
			</table>
		</asp:Panel>
    </form>

  </body>
</html>
