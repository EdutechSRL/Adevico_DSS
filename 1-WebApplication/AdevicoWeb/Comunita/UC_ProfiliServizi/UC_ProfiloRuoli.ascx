<%@ Control Language="vb" AutoEventWireup="false" Codebehind="UC_ProfiloRuoli.ascx.vb" Inherits="Comunita_OnLine.UC_ProfiloRuoli" TargetSchema="http://schemas.microsoft.com/intellisense/ie5" %>
<input type=hidden id="HDN_profiloID" runat=server NAME="HDN_profiloID"/>
<input type=hidden id="HDN_TPCM_ID" runat=server NAME="HDN_TPCM_ID"/>
<input type=hidden id="HDN_TPRL_ID" runat=server NAME="HDN_TPRL_ID"/>
<input type=hidden id="HDN_isDefinito" runat=server NAME="HDN_isDefinito"/>
<asp:Table HorizontalAlign=left Runat=server ID="TBLtipoRuolo" Width=800px GridLines=none>
	<asp:TableRow>
		<asp:TableCell ColumnSpan=2 Height=40px>&nbsp;</asp:TableCell>
	</asp:TableRow>
	<asp:TableRow>
		<asp:TableCell ColumnSpan=2>
			<asp:Label ID="LBinfoRuoli" Runat=server>
				ATTENZIONE: <br/>
				1) il ruolo di default NON può essere escluso;<br/>
				2) i ruoli di amministrazione NON possono essere esclusi;<br/>
				3) i ruoli esclusi saranno comunque disponibili nel caso in cui vi siano degli utenti iscritti alle comunità con tali ruoli;
			</asp:Label>
		</asp:TableCell>
	</asp:TableRow>
	<asp:TableRow>
		<asp:TableCell CssClass=top width=130px Wrap=False >
			<asp:Label ID="LBruoloDefault_t" Runat=server CssClass="Titolo_CampoSmall">Ruolo default:</asp:Label>
		</asp:TableCell>
		<asp:TableCell>
			<asp:CheckBox ID="CBXdefault" Runat=server Checked=True Enabled=False CssClass="Testo_campoSmall"></asp:CheckBox>
		</asp:TableCell>
	</asp:TableRow>
	<asp:TableRow ID="TBRruoloObbligatori">
		<asp:TableCell CssClass=top width=130px Wrap=False >
			<asp:Label ID="LBruoliNonDisattivabili_t" Runat=server CssClass="Titolo_CampoSmall">Ruoli obbligatori:</asp:Label>
		</asp:TableCell>
		<asp:TableCell>
			<asp:CheckBoxList ID="CBLtipoRuoloNonDisattivabili" Runat=server Enabled=False CssClass="Testo_campoSmall"  RepeatLayout=Table RepeatColumns=4 RepeatDirection=Horizontal></asp:CheckBoxList>
		</asp:TableCell>
	</asp:TableRow>
	<asp:TableRow>
		<asp:TableCell CssClass=top width=130px>
			<asp:Label ID="LBtipiRuolo_t" Runat=server CssClass="Titolo_CampoSmall">Altri ruoli associati:</asp:Label>
		</asp:TableCell>
		<asp:TableCell  CssClass="top">
			<asp:CheckBoxList ID="CBLtipoRuolo" Runat=server CssClass="Testo_campoSmall" DataValueField ="TPRL_ID" RepeatLayout=Table RepeatColumns=4 RepeatDirection=Horizontal>
			</asp:CheckBoxList>
		</asp:TableCell>
	</asp:TableRow>
</asp:Table>