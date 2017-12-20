<%@ Control Language="vb" AutoEventWireup="false" Codebehind="UC_infoCompetenze.ascx.vb" Inherits="Comunita_OnLine.UC_infoCompetenze" TargetSchema="http://schemas.microsoft.com/intellisense/ie5" %>
<input type="hidden" runat="server" id="HDNprsn_id" name="HDNprsn_id"/>
<asp:Table ID="TBLcompetenze" Runat="server">
<asp:TableRow Runat="server" ID="TBRmessaggio" Visible="False">
		<asp:TableCell ColumnSpan="2" HorizontalAlign="Center">
			<asp:label ID="LBmessaggio" Runat="server" CssClass="errore"></asp:label>
		</asp:TableCell>
	</asp:TableRow>
	<asp:TableRow ID="TBRrelazionali" Runat=server >
		<asp:TableCell CssClass=top >
			<asp:Label ID="LBcompetenzeRelazionali_t" Runat="server" cssclass="Titolo_campoSmall">Competenze Relazionali:</asp:Label>
		</asp:TableCell>
		<asp:TableCell CssClass=top>
			<asp:Label id="LBcompetenzeRelazionali" Runat="server" CssClass="Testo_campoSmall" Width="600px" ></asp:Label>
		</asp:TableCell>
	</asp:TableRow>
	<asp:TableRow ID="TBRorganizzative" Runat=server >
		<asp:TableCell CssClass=top>
			<asp:Label ID="LBcompetenzeOrganizzative_t" Runat="server" cssclass="Titolo_campoSmall">Competenze Organizzative:</asp:Label>
		</asp:TableCell>
		<asp:TableCell CssClass=top>
			<asp:Label id="LBcompetenzeOrganizzative" Runat="server" CssClass="Testo_campoSmall" Width="600px"></asp:Label>
		</asp:TableCell>
	</asp:TableRow>
	<asp:TableRow ID="TBRtecniche" Runat=server >
		<asp:TableCell CssClass=top>
			<asp:Label ID="LBcompetenzeTecniche_t" Runat="server" cssclass="Titolo_campoSmall">Competenze Tecniche:</asp:Label>
		</asp:TableCell>
		<asp:TableCell CssClass=top>
			<asp:Label id="LBcompetenzeTecniche" Runat="server" CssClass="Testo_campoSmall"  Width="600px"></asp:Label>
		</asp:TableCell>
	</asp:TableRow>
	<asp:TableRow ID="TBRartistiche" Runat=server >
		<asp:TableCell CssClass=top>
			<asp:Label ID="LBcompetenzeArtistiche_t" Runat="server" cssclass="Titolo_campoSmall">Competenze Artistiche:</asp:Label>
		</asp:TableCell>
		<asp:TableCell CssClass=top>
			<asp:Label id="LBcompetenzeArtistiche" Runat="server" CssClass="Testo_campoSmall" Width="600px"></asp:Label>
		</asp:TableCell>
	</asp:TableRow>
	<asp:TableRow ID="TBRaltre" Runat=server >
		<asp:TableCell CssClass=top>
			<asp:Label ID="LBaltreCompetenze_t" Runat="server" cssclass="Titolo_campoSmall">Altre Competenze:</asp:Label>
		</asp:TableCell>
		<asp:TableCell CssClass=top>
			<asp:Label id="LBaltreCompetenze" Runat="server" CssClass="Testo_campoSmall" Width="600"></asp:Label>
		</asp:TableCell>
	</asp:TableRow>
	<asp:TableRow ID="TBRulterioriInfo" Runat=server >
		<asp:TableCell CssClass=top>
			<asp:Label ID="LBulterioriInfo_t" Runat="server" cssclass="Titolo_campoSmall">Ulteriori Info:</asp:Label>
		</asp:TableCell>
		<asp:TableCell CssClass=top>
			<asp:Label id="LBulterioriInfo" Runat="server" CssClass="Testo_campoSmall" Width="600px"></asp:Label>
		</asp:TableCell>
	</asp:TableRow>
	
	</asp:Table> 