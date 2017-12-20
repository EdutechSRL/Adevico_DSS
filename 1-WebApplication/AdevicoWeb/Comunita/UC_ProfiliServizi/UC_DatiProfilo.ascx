<%@ Control Language="vb" AutoEventWireup="false" Codebehind="UC_DatiProfilo.ascx.vb" Inherits="Comunita_OnLine.UC_DatiProfilo" TargetSchema="http://schemas.microsoft.com/intellisense/ie5" %>
<input type=hidden id="HDN_profiloID" runat=server/>
<input type=hidden id="HDN_nomeProfilo" value="" runat=server NAME="HDN_nomeProfilo"/>
<asp:Table HorizontalAlign=left Runat=server ID="TBLdati" Width=800px Visible=true >
	<asp:TableRow>
		<asp:TableCell ColumnSpan=2 Height=40px>&nbsp</asp:TableCell>
	</asp:TableRow>
	<asp:TableRow>
		<asp:TableCell width=120px>
			<asp:Label ID="LBnome_t" Runat=server CssClass="Titolo_CampoSmall">Nome:</asp:Label>
		</asp:TableCell>
		<asp:TableCell>
			<asp:TextBox ID="TXBNome" Runat=server CssClass="Testo_campo_obbligatorioSmall" MaxLength=50 Columns=80></asp:TextBox>
			<asp:requiredfieldvalidator id="RFVnome" runat="server" CssClass="Validatori" ControlToValidate="TXBNome" Display="Dynamic" EnableClientScript=True>*</asp:requiredfieldvalidator>
		</asp:TableCell>
	</asp:TableRow>
	<asp:TableRow ID="TBR_2">
		<asp:TableCell width="120px" Height="35px">
			<asp:label ID="LBdescrizione_t" Runat=server CssClass="Titolo_campoSmall">Descrizione:</asp:Label>
		</asp:TableCell>
		<asp:TableCell>
			<asp:textbox id="TXBdescrizione" Runat="server" CssClass="Testo_campoSmall" MaxLength="200" Columns="80"></asp:textbox>
		</asp:TableCell>
	</asp:TableRow>
	<asp:TableRow>
		<asp:TableCell width="120px" >
			<asp:Label ID="LBtipoComunita_t" Runat=server CssClass="Titolo_campoSmall">Tipo Comunità:&nbsp;</asp:Label>
		</asp:TableCell>
		<asp:TableCell>
			<asp:DropDownList ID="DDLtipoComunita" Runat=server CssClass="Testo_campoSmall"></asp:DropDownList>
			<asp:Label ID="LBinfo" Runat=server CssClass="Testo_campoSmall"></asp:Label>
		</asp:TableCell>
	</asp:TableRow>
	<asp:TableRow ID="TBRcreatore">
		<asp:TableCell width="120px" >
			<asp:Label ID="LBcreatore_t" Runat=server CssClass="Titolo_campoSmall">Creato da:&nbsp;</asp:Label>
		</asp:TableCell>
		<asp:TableCell>
			<asp:Label ID="LBcreatore" Runat=server CssClass="Testo_campoSmall"></asp:Label>
		</asp:TableCell>
	</asp:TableRow>
	<asp:TableRow ID="TBRmodifica">
		<asp:TableCell width="120px" >
			<asp:Label ID="LBultimaModifica_t" Runat=server CssClass="Titolo_campoSmall">Modificato da:&nbsp;</asp:Label>
		</asp:TableCell>
		<asp:TableCell>
			<asp:Label ID="LBultimaModifica" Runat=server CssClass="Testo_campoSmall"></asp:Label>
		</asp:TableCell>
	</asp:TableRow>
</asp:Table>