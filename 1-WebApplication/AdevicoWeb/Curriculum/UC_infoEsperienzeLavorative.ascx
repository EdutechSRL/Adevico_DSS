<%@ Control Language="vb" AutoEventWireup="false" Codebehind="UC_infoEsperienzeLavorative.ascx.vb" Inherits="Comunita_OnLine.UC_infoEsperienzeLavorative" TargetSchema="http://schemas.microsoft.com/intellisense/ie5" %>
<input type="hidden" runat="server" id="HDNprsn_id" name="HDNprsn_id"/>
<asp:Panel ID="PNLesperienze" Runat="server">
	<asp:Repeater ID="RPTesperienze" Runat="server">
		<ItemTemplate>
			<FIELDSET>
				<asp:Table HorizontalAlign="center" width="100%" Runat="server" ID="TBesperienze" GridLines=none>
					<asp:TableRow runat="server" ID="TBRinizio" >
						<asp:TableCell Wrap=False width=120px CssClass="top" >
							<asp:Label ID="LBinizio_s" Runat="server" CssClass="Titolo_campoSmall">Data Inizio:</asp:Label>
						</asp:TableCell>
						<asp:TableCell CssClass="top">	
							<asp:Label ID="LBinizio" Runat="server" CssClass="Testo_campoSmall"><%#Container.DataItem("oInizio")%></asp:Label>
							&nbsp;&nbsp;&nbsp;
							<asp:Label ID="LBfine_s" Runat="server" CssClass="Titolo_campoSmall">Data Fine:</asp:Label>
							&nbsp;
							<asp:Label ID="LBfine" Runat="server" CssClass="Testo_campoSmall"><%#Container.DataItem("oFine")%></asp:Label>
						</asp:TableCell>	
					</asp:TableRow>
					<asp:TableRow>
						<asp:TableCell Wrap=False width=120px CssClass="top">
							<asp:Label ID="LBnome_s" Runat="server" CssClass="Titolo_campoSmall">&nbsp;Nome Datore:</asp:Label>
						</asp:TableCell>
						<asp:TableCell HorizontalAlign=Left CssClass="top" Width="650px">
							<asp:Label ID="LBnome" Runat="server" CssClass="Testo_campoSmall">
								<%#Container.DataItem("ESLV_nomeDatore")%>
							</asp:Label>
						</asp:TableCell>
					</asp:TableRow>
					<asp:TableRow Runat="server" ID="TBRsettore">
						<asp:TableCell Wrap=False width=120px CssClass="top">
							<asp:Label ID="LBsettore_s" Runat="server" CssClass="Titolo_campoSmall">&nbsp;Settore:</asp:Label>
						</asp:TableCell>
						<asp:TableCell HorizontalAlign=Left CssClass="top" Width="650px">
							<asp:Label ID="LBsettore" Runat="server" CssClass="Testo_campoSmall">
								<%#Container.DataItem("ESLV_settore")%>
							</asp:Label>
						</asp:TableCell>
					</asp:TableRow>
					<asp:TableRow Runat="server" ID="TBRtipoImpiego">
						<asp:TableCell Wrap=False width=120px CssClass="top">
							<asp:Label ID="LBtipoImpiego_s" Runat="server" CssClass="Titolo_campoSmall">&nbsp;Tipo Impiego:</asp:Label>
						</asp:TableCell>
						<asp:TableCell HorizontalAlign=Left CssClass="top" Width="650px">
							<asp:Label ID="LBtipoImpiego" Runat="server" CssClass="Testo_campoSmall">
								<%#Container.DataItem("ESLV_tipoImpiego")%>
							</asp:Label>
						</asp:TableCell>
					</asp:TableRow>
					<asp:TableRow Runat="server" ID="TBRmansione">
						<asp:TableCell Wrap=False width=120px CssClass="top">
							<asp:Label ID="LBmansione_s" Runat="server" CssClass="Titolo_campoSmall">&nbsp;Mansione:</asp:Label>
						</asp:TableCell>
						<asp:TableCell HorizontalAlign=Left CssClass="top" Width="650px">
							<asp:Label ID="LBmansione" Runat="server" CssClass="Testo_campoSmall">
								<%#Container.DataItem("ESLV_mansione")%>
							</asp:Label>
						</asp:TableCell>
					</asp:TableRow>				
				</asp:Table>
			</FIELDSET>
			<br>
		</ItemTemplate>
	</asp:Repeater>
</asp:Panel>
