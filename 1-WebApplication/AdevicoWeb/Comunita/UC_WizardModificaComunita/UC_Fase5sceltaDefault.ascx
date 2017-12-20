<%@ Control Language="vb" AutoEventWireup="false" Codebehind="UC_Fase5sceltaDefault.ascx.vb" Inherits="Comunita_OnLine.UC_Fase5sceltaDefault" TargetSchema="http://schemas.microsoft.com/intellisense/ie5" %>
<input type=hidden id="HDNhasSetup" runat=server NAME="HDNhasSetup"/>
<input type=hidden id="HDNhasServizi" runat=server NAME="HDNhasServizi"/>
<input type=hidden id=HDNcmnt_ID runat=server NAME="HDNcmnt_ID"/>

<asp:Panel ID="PNLservizi" Runat="server" Visible="true" HorizontalAlign=Center >				
	<asp:Table runat="server">
		<asp:TableRow ID="TBRutenteSelezionato" Visible="true">
			<asp:TableCell>
				<table border="0" align="center" width="800px" cellspacing=0>
					<tr>
						<td>
							<asp:Table ID="TBLdatiPrincipali" Runat="server" Width="800px" CellPadding=0 CellSpacing=0 BorderStyle=none>
								<asp:TableRow>
									<asp:TableCell ColumnSpan=4>&nbsp;</asp:TableCell>
								</asp:TableRow>
								<asp:TableRow>
									<asp:TableCell>&nbsp;</asp:TableCell>
									<asp:TableCell>
										<asp:Label ID="LBavviso" Runat=server CssClass ="avviso_normal">
										Se nella Comunità è attiva la "cover", cliccando su di essa di accede al servizio default.
										Se la Cover è disattivata si accede direttamente al servizio selezionato di default.
										</asp:Label>
										<br/><br/>
									</asp:TableCell>
									<asp:TableCell>&nbsp;</asp:TableCell>
								</asp:TableRow>
								<asp:TableRow ID="TBRdefault">
									<asp:TableCell>&nbsp;</asp:TableCell>
									<asp:TableCell>
										<asp:Label ID="LBpaginaDefault_t" Runat=server CssClass="Titolo_campoSmall">Attiva all'accesso il servizio:</asp:Label>&nbsp;
										<asp:DropDownList ID="DDLpagineDefault" Runat=server cssClass="Testo_campoSmall"></asp:DropDownList>
									</asp:TableCell>
									<asp:TableCell>&nbsp;</asp:TableCell>
								</asp:TableRow>
								<asp:TableRow>
									<asp:TableCell ColumnSpan=3>&nbsp;</asp:TableCell>
								</asp:TableRow>
							</asp:Table>
						</td>
					</tr>
				</table>
			</asp:TableCell>
		</asp:TableRow>
	</asp:Table>
</asp:Panel>