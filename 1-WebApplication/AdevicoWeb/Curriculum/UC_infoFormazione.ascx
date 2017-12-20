<%@ Control Language="vb" AutoEventWireup="false" Codebehind="UC_infoFormazione.ascx.vb" Inherits="Comunita_OnLine.UC_infoFormazione" TargetSchema="http://schemas.microsoft.com/intellisense/ie5" %>
<input type="hidden" runat="server" id="HDNprsn_id" name="HDNprsn_id"/>
<asp:Panel ID="PNLformazione" Runat="server">
	<asp:Repeater ID="RPTformazione" Runat="server">
		<ItemTemplate>
		<FIELDSET>
			<asp:Table Runat="server" ID=TBformazione HorizontalAlign =Left Width=100%>
				<asp:TableRow runat="server">
					<asp:TableCell Wrap=False width=120px CssClass="top">
						<asp:Label ID="LBinizio_s" Runat="server" CssClass="Titolo_campoSmall">*Data Inizio:</asp:Label>
						&nbsp;&nbsp;
					</asp:TableCell>
					<asp:TableCell CssClass="top">
						<asp:Label ID="LBinizio" Runat="server" CssClass="Testo_campoSmall"><%#Container.DataItem("ISFR_inizio")%></asp:Label>
						&nbsp;&nbsp;&nbsp;
						<asp:Label ID="LBfine_s" Runat="server" CssClass="Titolo_campoSmall">*Data Fine:</asp:Label>
						&nbsp;
						<asp:Label ID="LBfine" Runat="server" CssClass="Testo_campoSmall"><%#Container.DataItem("ofine")%></asp:Label>
					</asp:TableCell>
				</asp:TableRow>
				<asp:TableRow>
					<asp:TableCell Wrap=False width=120px CssClass="top">
						<asp:Label ID="LBnome_s" Runat="server" CssClass="Titolo_campoSmall">*Nome e Tipo Istituto:</asp:Label>
					</asp:TableCell>
					<asp:TableCell CssClass="top">
						<asp:Label ID="LBnome" Runat="server" CssClass="Testo_campoSmall" >
							<%#Container.DataItem("ISFR_nomeeTipoIstituto")%>
						</asp:Label>
					</asp:TableCell>
				</asp:TableRow>
				<asp:TableRow Runat=server ID="TBRmaterie">
					<asp:TableCell Wrap=False width=120px CssClass="top">
						<asp:Label ID="LBmaterie_s" Runat="server" CssClass="Titolo_campoSmall">&nbsp;Principali Materie o Abilità:</asp:Label>
					</asp:TableCell>
					<asp:TableCell CssClass="top">
						<asp:Label ID="LBmaterie" Runat="server" CssClass="Testo_campoSmall">
							<%#Container.DataItem("ISFR_principaliMaterieAbilita")%>
						</asp:Label>
					</asp:TableCell>
				</asp:TableRow>
				<asp:TableRow  Runat=server ID="TBRqualifica" CssClass="top">
					<asp:TableCell Wrap=False width=250px>
						<asp:Label ID="LBqualifica_s" Runat="server" CssClass="Titolo_campoSmall">&nbsp;Qualifica Conseguita:</asp:Label>
					</asp:TableCell>
					<asp:TableCell CssClass="top">
						<asp:Label ID="LBqualifica" Runat="server" CssClass="Testo_campoSmall">
							<%#Container.DataItem("ISFR_qualificaConseguita")%>
						</asp:Label>
					</asp:TableCell>
				</asp:TableRow>
				<asp:TableRow Runat=server ID="TBRlivello">
					<asp:TableCell Wrap=False width=120px CssClass="top">
						<asp:Label ID="LBclassificazione_s" Runat="server" CssClass="Titolo_campoSmall">&nbsp;Livello Classificazione Nazionale:</asp:Label>
					</asp:TableCell>
					<asp:TableCell CssClass="top">
						<asp:Label ID="LBclassificazione" Runat="server" CssClass="Testo_campoSmall">
							<%#Container.DataItem("ISFR_livelloClassificazioneNazionale")%>
						</asp:Label>
					</asp:TableCell>
				</asp:TableRow>
			</asp:Table>
			</FIELDSET>
			<br>
		</ItemTemplate>
	</asp:Repeater>
</asp:Panel>