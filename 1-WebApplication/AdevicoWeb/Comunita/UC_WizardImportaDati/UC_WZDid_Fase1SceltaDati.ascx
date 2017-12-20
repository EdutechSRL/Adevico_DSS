<%@ Control Language="vb" AutoEventWireup="false" Codebehind="UC_WZDid_Fase1SceltaDati.ascx.vb" Inherits="Comunita_OnLine.UC_WZDid_Fase1SceltaDati" TargetSchema="http://schemas.microsoft.com/intellisense/ie5" %>
<%@ Register TagPrefix="CTRL" TagName="CTRLsorgenteComunita" Src="./../../UC/UC_FiltroComunitaByServizio_NEW.ascx" %>

<asp:Table ID="TBLscelta" Runat=server HorizontalAlign=Center GridLines=none width="800px">
	<asp:TableRow>
		<asp:TableCell ColumnSpan=2>
			<asp:Label ID="LBinfoComunita" Runat="server">Scelta delle altre comunità in cui rendere disponibile quella che si intende creare:</asp:Label>
		</asp:TableCell>
	</asp:TableRow>
	<asp:TableRow ID="TBRcomunita">
		<asp:TableCell>
			<asp:Label ID="LBcomunita_t" Runat=server CssClass="Titolo_CampoSmall">Comunità:</asp:Label>
		</asp:TableCell>
		<asp:TableCell>
			<CTRL:CTRLsorgenteComunita id="CTRLsorgenteComunita" runat="server" Width="800px" LarghezzaFinestraAlbero="800px" ColonneNome=100></CTRL:CTRLsorgenteComunita>
		</asp:TableCell>
	</asp:TableRow>
	<asp:TableRow>
		<asp:TableCell ColumnSpan=2>
			<asp:CheckBoxList ID="CBLscelta" Runat=server RepeatLayout=Table RepeatColumns=3 RepeatDirection=Horizontal>
				<asp:ListItem Value=0>Impostazioni servizi/permessi</asp:ListItem>
				<asp:ListItem Value=1>Utenti iscritti</asp:ListItem>
				<asp:ListItem Value=2>Materiale</asp:ListItem>
				<asp:ListItem Value=3>Diario Lezione</asp:ListItem>
				<asp:ListItem Value=4>Eventi</asp:ListItem>
				<asp:ListItem Value=5>Link</asp:ListItem>
				<asp:ListItem Value=6>Bacheca</asp:ListItem>			
			</asp:CheckBoxList>
		</asp:TableCell>
	</asp:TableRow>
</asp:Table>