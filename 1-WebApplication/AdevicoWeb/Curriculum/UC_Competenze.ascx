<%@ Control Language="vb" AutoEventWireup="false" Codebehind="UC_Competenze.ascx.vb" Inherits="Comunita_OnLine.UC_Competenze" TargetSchema="http://schemas.microsoft.com/intellisense/ie5" %>
<input type="hidden" runat="server" id="HDNprsn_id" name="HDNprsn_id"/>
<input type="hidden" runat="server" id="HDNcrev_id" name="HDNcrev_id"/>
<asp:Table ID="TBLcompetenze" Runat="server" HorizontalAlign=Center>
	<asp:TableRow Runat="server" ID="TBRmessaggio" Visible="False">
		<asp:TableCell ColumnSpan="2" HorizontalAlign="Center">
			<asp:label ID="LBmessaggio" Runat="server" CssClass="errore"></asp:label>
		</asp:TableCell>
	</asp:TableRow>
	<asp:TableRow>
		<asp:TableCell CssClass=top>
			<asp:Label ID="LBcompetenzeRelazionali_t" Runat="server" cssclass="Titolo_campoSmall">Competenze Relazionali:</asp:Label>
		</asp:TableCell>
		<asp:TableCell CssClass=top>
			<asp:TextBox id="TXBcompetenzeRelazionali" Runat="server" CssClass="Testo_campoSmall" MaxLength="5000"
			 Columns=90  Rows=5 TextMode="MultiLine"></asp:TextBox>
		</asp:TableCell>
	</asp:TableRow>
	<asp:TableRow>
		<asp:TableCell CssClass=top>
			<asp:Label ID="LBcompetenzeOrganizzative_t" Runat="server" cssclass="Titolo_campoSmall">Competenze Organizzative:</asp:Label>
		</asp:TableCell>
		<asp:TableCell CssClass=top>
			<asp:TextBox id="TXBcompetenzeOrganizzative" Runat="server" CssClass="Testo_campoSmall" MaxLength="5000" Columns=90  Rows=5 TextMode="MultiLine"></asp:TextBox>
		</asp:TableCell>
	</asp:TableRow>
	<asp:TableRow>
		<asp:TableCell CssClass=top>
			<asp:Label ID="LBcompetenzeTecniche_t" Runat="server" cssclass="Titolo_campoSmall">Competenze Tecniche:</asp:Label>
		</asp:TableCell>
		<asp:TableCell CssClass=top>
			<asp:TextBox id="TXBcompetenzeTecniche" Runat="server" CssClass="Testo_campoSmall" MaxLength="5000" Columns=90  Rows=5 TextMode="MultiLine"></asp:TextBox>
		</asp:TableCell>
	</asp:TableRow>
	<asp:TableRow>
		<asp:TableCell CssClass=top>
			<asp:Label ID="LBcompetenzeArtistiche_t" Runat="server" cssclass="Titolo_campoSmall">Competenze Artistiche:</asp:Label>
		</asp:TableCell>
		<asp:TableCell CssClass=top>
			<asp:TextBox id="TXBcompetenzeArtistiche" Runat="server" CssClass="Testo_campoSmall" MaxLength="5000" Columns=90  Rows=5 TextMode="MultiLine"></asp:TextBox>
		</asp:TableCell>
	</asp:TableRow>
	<asp:TableRow>
		<asp:TableCell CssClass=top>
			<asp:Label ID="LBaltreCompetenze_t" Runat="server" cssclass="Titolo_campoSmall">Altre Competenze:</asp:Label>
		</asp:TableCell>
		<asp:TableCell CssClass=top>
			<asp:TextBox id="TXBaltreCompetenze" Runat="server" CssClass="Testo_campoSmall" MaxLength="5000" Columns=90  Rows=5 TextMode="MultiLine"></asp:TextBox>
		</asp:TableCell>
	</asp:TableRow>
	<asp:TableRow>
		<asp:TableCell CssClass=top>
			<asp:Label ID="LBulterioriInfo_t" Runat="server" cssclass="Titolo_campoSmall">Ulteriori Info:</asp:Label>
		</asp:TableCell>
		<asp:TableCell CssClass=top>
			<asp:TextBox id="TXBulterioriInfo" Runat="server" CssClass="Testo_campoSmall" MaxLength="5000" Columns=90  Rows=5 TextMode="MultiLine"></asp:TextBox>
		</asp:TableCell>
	</asp:TableRow>
	
	</asp:Table> 