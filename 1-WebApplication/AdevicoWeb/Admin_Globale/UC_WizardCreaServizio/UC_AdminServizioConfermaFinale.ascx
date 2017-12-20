<%@ Control Language="vb" AutoEventWireup="false" Codebehind="UC_AdminServizioConfermaFinale.ascx.vb" Inherits="Comunita_OnLine.UC_AdminServizioConfermaFinale" TargetSchema="http://schemas.microsoft.com/intellisense/ie5" %>
<input type=hidden id="HDN_servizioID" runat=server NAME="HDN_servizioID"/>
<input type=hidden id="HDN_definito" runat=server NAME="HDN_definito"/>
<input type=hidden id="HDNhasSetupGenerali" runat=server NAME="HDNhasSetupGenerali"/>
<input type=hidden id="HDN_elencoPermessi" runat=server NAME="HDN_elencoPermessi"/>
<input type=hidden id="HDN_update" runat=server NAME="HDN_update"/>
<asp:Table HorizontalAlign=Center Runat=server ID="TBLtipoComunita" Width=800px Visible=true GridLines=none >
	<asp:TableRow>
		<asp:TableCell ColumnSpan=2 CssClass="nosize0" Height=10px>&nbsp;</asp:TableCell>
	</asp:TableRow>
	<asp:TableRow>
		<asp:TableCell ColumnSpan=2 HorizontalAlign=left >
			<asp:Label ID="LBfinale" Runat=server>
				ATTENZIONE: premendo il pulsante Conferma si procederà con l'impostazione del servizio
			</asp:Label>
		</asp:TableCell>
	</asp:TableRow>
	<asp:TableRow>
		<asp:TableCell ColumnSpan=2 CssClass="nosize0" Height=10px>&nbsp;</asp:TableCell>
	</asp:TableRow>
</asp:Table>