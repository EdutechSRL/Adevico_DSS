<%@ Control Language="vb" AutoEventWireup="false" Codebehind="UC_infoConoscenzaLingua.ascx.vb" Inherits="Comunita_OnLine.UC_infoConoscenzaLingua" TargetSchema="http://schemas.microsoft.com/intellisense/ie5" %>
<input type="hidden" runat="server" id="HDNprsn_id" name="HDNprsn_id"/> 
<asp:Panel ID="PNLlingua" Runat="server">
		<asp:Repeater ID="RPTlingua" Runat="server">
			<ItemTemplate>
			<fieldset>
			<Table align="center" width="100%">
				<TR>
					<td width="250px">
						<asp:Label ID="LBnome_s" Runat="server" CssClass="Titolo_campoSmall">&nbsp;Nome:</asp:Label>
					</td>
					<td>
						<asp:Label ID="LBnome" Runat="server" CssClass="TitoloServizio">
							<%#Container.DataItem("CNLN_nome")%>
						</asp:Label>
					</td>
				</TR>
				<TR>
					<td>
						<asp:Label ID="LBlettura_s" Runat="server" CssClass="Titolo_campoSmall">&nbsp;Abilità in Lettura:</asp:Label>
					</td>
					<td>
						<asp:Label ID="LBlettura" Runat="server" CssClass="Testo_campoSmall">
							<%#Container.DataItem("oLettura")%>
						</asp:Label>
					</td>
				</TR>
				<TR>
					<td>
						<asp:Label ID="LBscrittura_s" Runat="server" CssClass="Titolo_campoSmall">&nbsp;Abilità in Scrittura:</asp:Label>
					</td>
					<td>
						<asp:Label ID="LBscrittura" Runat="server" CssClass="Testo_campoSmall">
							<%#Container.DataItem("oScrittura")%>
						</asp:Label>
					</td>
				</TR>
				<TR>
					<td>
						<asp:Label ID="LBespressioneOrale_s" Runat="server" CssClass="Titolo_campoSmall">&nbsp;Abilità in Espressione Orale:</asp:Label>
					</td>
					<td>
						<asp:Label ID="LBespressioneOrale" Runat="server" CssClass="Testo_campoSmall">
							<%#Container.DataItem("oEspressioneOrale")%>
						</asp:Label>
					</td>
				</TR>

				</Table>
				</fieldset>
				<br>
			</ItemTemplate>
		</asp:Repeater>
</asp:Panel>