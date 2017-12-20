<%@ Control Language="vb" AutoEventWireup="false" Codebehind="UC_DatiServizio.ascx.vb" Inherits="Comunita_OnLine.UC_DatiServizio" TargetSchema="http://schemas.microsoft.com/intellisense/ie5" %>
<input type=hidden id="HDN_servizioID" runat=server/>
<input type=hidden id="HDN_nomeServizio" value="" runat=server NAME="HDN_nomeServizio"/>
<asp:Table HorizontalAlign=center Runat=server ID="TBLdati" Width=800px Visible=true >
	<asp:TableRow>
		<asp:TableCell ColumnSpan=2 Height=40px>&nbsp</asp:TableCell>
	</asp:TableRow>
	<asp:TableRow>
		<asp:TableCell width=120px>
			<asp:Label ID="LBnomeServizio_t" Runat=server CssClass="Titolo_CampoSmall">Nome:</asp:Label>
		</asp:TableCell>
		<asp:TableCell>
			<asp:TextBox ID="TXBNome" Runat=server CssClass="Testo_campo_obbligatorioSmall" MaxLength=50 Columns=80></asp:TextBox>
			<asp:requiredfieldvalidator id="RFVnome" runat="server" CssClass="Validatori" ControlToValidate="TXBNome" Display="Dynamic" EnableClientScript=True>*</asp:requiredfieldvalidator>
		</asp:TableCell>
	</asp:TableRow>
	<asp:TableRow>
		<asp:TableCell width="120px" >&nbsp;</asp:TableCell>
		<asp:TableCell CssClass=top>
			<table border=1 align=left bgcolor="#FFFBF7" style="border-color:#CCCCCC" cellpadding=0 cellspacing=0>
				<tr>
					<td>
						<table border=0 align=left bgcolor="#FFFBF7" cellpadding=0 cellspacing=0>
							<asp:Repeater id="RPTnome" Runat="server">
								<HeaderTemplate>
									<tr>
										<td colspan=2 height=20px>
											<asp:Label ID="LBlinguaNome_t" Runat=server CssClass="Titolo_campoSmall">&nbsp;Traduzioni(°):</asp:Label>
										</td>
									</tr>
								</HeaderTemplate>
								<ItemTemplate>
									<tr>
										<td align=right width=120px height=22px>
											<asp:Label id="LBlinguaID" Text='<%# Databinder.eval(Container.DataItem, "LNGU_ID")%>' runat="server" Visible=false />
											<asp:Label id="LBlingua_Nome" Text='<%# Databinder.eval(Container.DataItem, "LNGU_nome")%>' runat="server" Visible=true CssClass=Repeater_VoceLingua/>&nbsp;
										</td>
										<td align=left height=22px>
											<asp:TextBox ID="TXBtermine" Runat="server" CssClass="Testo_campoSmall" MaxLength="50" Text='<%# Databinder.eval(Container.DataItem, "Nome")%>' Columns="80"> </asp:TextBox>&nbsp;&nbsp;
										</td>
									</tr>
								</ItemTemplate>
								<FooterTemplate>
									<tr><td colspan=2 class=nosize0>&nbsp;</td></tr>
								</FooterTemplate>
							</asp:Repeater>
						</table>
					</td>
				</tr>
			</table>								
		</asp:TableCell>
	</asp:TableRow>
	<asp:TableRow>
		<asp:TableCell ColumnSpan=2>&nbsp;</asp:TableCell>
	</asp:TableRow>
	<asp:TableRow ID="TBR_2">
		<asp:TableCell width="120px" Height="35px">
			<asp:label ID="LBdescrizione_t" Runat=server CssClass="Titolo_campoSmall">&nbsp;Descrizione:</asp:Label>
		</asp:TableCell>
		<asp:TableCell>
			<asp:textbox id="TXBdescrizione" Runat="server" CssClass="Testo_campoSmall" MaxLength="200" Columns="80"></asp:textbox>
		</asp:TableCell>
	</asp:TableRow>
	<asp:TableRow>
		<asp:TableCell width="120px" >&nbsp;</asp:TableCell>
		<asp:TableCell CssClass=top>
			<table border=1 align=left bgcolor="#FFFBF7" style="border-color:#CCCCCC" cellpadding=0 cellspacing=0>
				<tr>
					<td>
						<table border=0 align=left bgcolor="#FFFBF7" cellpadding=0 cellspacing=0>
							<asp:Repeater id="RPTdescrizione" Runat="server">
								<HeaderTemplate>
									<tr>
										<td colspan=2 height=20px>
											<asp:Label ID="LBlinguaDescrizione_t" Runat=server CssClass="Titolo_campoSmall">&nbsp;Traduzioni(°):</asp:Label>
										</td>
									</tr>
								</HeaderTemplate>
								<ItemTemplate>
									<tr>
										<td align=right width=120px height=22px>
											<asp:Label id="LBlingua2ID" Text='<%# Databinder.eval(Container.DataItem, "LNGU_ID")%>' runat="server" Visible=false />
											<asp:Label id="LBlingua2_Nome" Text='<%# Databinder.eval(Container.DataItem, "LNGU_nome")%>' runat="server" Visible=true CssClass=Repeater_VoceLingua/>&nbsp;
										</td>
										<td align=left height=22px>
											<asp:TextBox ID="TXBtermine2" Runat="server" CssClass="Testo_campoSmall" Text='<%# Databinder.eval(Container.DataItem, "Descrizione")%>' MaxLength="200" Columns="80"> </asp:TextBox>&nbsp;&nbsp;
										</td>
									</tr>
								</ItemTemplate>
								<FooterTemplate>
									<tr><td colspan=2 class=nosize0>&nbsp;</td></tr>
								</FooterTemplate>
							</asp:Repeater>
						</table>
					</td>
				</tr>
			</table>								
		</asp:TableCell>
	</asp:TableRow>
	<asp:TableRow>
		<asp:TableCell width="120px" >
			<asp:Label ID="LBcodice_t" Runat=server CssClass="Titolo_campoSmall">Codice*:&nbsp;</asp:Label>
		</asp:TableCell>
		<asp:TableCell>
			<asp:textbox id="TXBcodice" Runat="server" CssClass="Testo_campo_obbligatorioSmall" MaxLength=15 Columns=20></asp:textbox>
			<asp:requiredfieldvalidator id="RFVcodice" runat="server" CssClass="Validatori" ControlToValidate="TXBcodice" Display="static" EnableClientScript="True">*</asp:requiredfieldvalidator>
		</asp:TableCell>
	</asp:TableRow>
	<asp:TableRow>
		<asp:TableCell width="120px" >
			<asp:Label ID="LBattiva_t" Runat=server CssClass="Titolo_campoSmall">Attivo:&nbsp;</asp:Label>
		</asp:TableCell>
		<asp:TableCell>
			<table cellpadding=0 cellspacing=0 border=0>
				<tr>
					<td>
						<asp:CheckBox id="CBXattiva" Runat="server" Checked="false" CssClass="Testo_campoSmall" Text="Si"></asp:CheckBox>		
					</td>
					<td width=30px>&nbsp;</td>
					<td>
						<asp:Label ID="LBnonDisattivabile_t" Runat=server CssClass="Titolo_campoSmall">Non disattivabile:&nbsp;</asp:Label>
						<asp:CheckBox id="CBXnoDisattiva" Runat="server" Checked="false" CssClass="Testo_campoSmall" Text="Si"></asp:CheckBox>
					</td>
					<td width=30px>&nbsp;</td>
					<td>
						<asp:Label ID="LBisNotificabile_t" Runat=server CssClass="Titolo_campoSmall">Notificabile:&nbsp;</asp:Label>
						<asp:CheckBox id="CBXnotificabile" Runat="server" Checked="false" CssClass="Testo_campoSmall" Text="Si"></asp:CheckBox>
					</td>
				</tr>
			</table>
		</asp:TableCell>
	</asp:TableRow>
</asp:Table>