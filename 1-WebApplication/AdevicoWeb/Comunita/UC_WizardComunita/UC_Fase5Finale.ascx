<%@ Control Language="vb" AutoEventWireup="false" Codebehind="UC_Fase5Finale.ascx.vb" Inherits="Comunita_OnLine.UC_Fase5Finale" TargetSchema="http://schemas.microsoft.com/intellisense/ie5" %>
<asp:Table ID="TBLfinale" Runat=server HorizontalAlign=Center GridLines=none width="800px">
	<asp:TableRow>
		<asp:TableCell ColumnSpan=2>
			&nbsp;
		</asp:TableCell>
	</asp:TableRow>
	<asp:TableRow>
		<asp:TableCell ColumnSpan=2>
			<asp:Label ID="LBfinale" Runat=server>
				ATTENZIONE: premendo il pulsante Conferma si procederà con la creazione della comunita
			</asp:Label>
		</asp:TableCell>
	</asp:TableRow>
	<asp:TableRow>
		<asp:TableCell ColumnSpan=2>
			&nbsp;
		</asp:TableCell>
	</asp:TableRow>
	<asp:TableRow>
		<asp:TableCell>
			<asp:Label ID="LBcomunita_t" Runat=server CssClass="Titolo_campoSmall">Nome:</asp:Label>&nbsp;
		</asp:TableCell>
		<asp:TableCell>
			<asp:Label ID="LBcomunita" Runat=server CssClass="Testo_campoSmall"></asp:Label>
		</asp:TableCell>
	</asp:TableRow>
	<asp:TableRow>
		<asp:TableCell>
			<asp:Label ID="LBresponsabile_t" Runat=server CssClass="Titolo_campoSmall">Responsabile:</asp:Label>&nbsp;
		</asp:TableCell>
		<asp:TableCell>
			<asp:Label ID="LBresponsabile" Runat=server CssClass="Testo_campoSmall"></asp:Label>
		</asp:TableCell>
	</asp:TableRow>
	<asp:TableRow ID="TBRpadri">
		<asp:TableCell VerticalAlign=Top >
			<asp:Label ID="LBpadri_t" Runat=server CssClass="Titolo_campoSmall">Disponibile per le comunità:</asp:Label>
		</asp:TableCell>
		<asp:TableCell VerticalAlign=Top >
			<asp:Label ID="LBpadri" Runat=server CssClass="Testo_campoSmall"></asp:Label>
		</asp:TableCell>
	</asp:TableRow>
	<asp:TableRow ID="TBRservizi">
		<asp:TableCell VerticalAlign=Top >
			<asp:Label ID="LBservizi_t" Runat=server CssClass="Titolo_campoSmall">Servizi attivati:</asp:Label>
		</asp:TableCell>
		<asp:TableCell VerticalAlign=Top >
			<asp:Label ID="LBservizi" Runat=server CssClass="Testo_campoSmall"></asp:Label>
		</asp:TableCell>	
	</asp:TableRow>
	<asp:TableRow>
		<asp:TableCell ColumnSpan=2>
			&nbsp;
		</asp:TableCell>
	</asp:TableRow>
</asp:Table>