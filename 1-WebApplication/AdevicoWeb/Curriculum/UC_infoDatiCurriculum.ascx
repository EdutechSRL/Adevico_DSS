<%@ Control Language="vb" AutoEventWireup="false" Codebehind="UC_infoDatiCurriculum.ascx.vb" Inherits="Comunita_OnLine.UC_infoDatiCurriculum" TargetSchema="http://schemas.microsoft.com/intellisense/ie5" %>
<input type="hidden" runat="server" id="HDNprsn_id" name="HDNprsn_id"/> 
<input type="hidden" runat="server" id="HDNcrev_id" name="HDNcrev_id"/>

<asp:Table ID="TBLdati" Runat="server" Width =100% HorizontalAlign =Center >
	<asp:TableRow Runat="server" ID="TBRmessaggio" Visible="False">
		<asp:TableCell ColumnSpan="4" HorizontalAlign="Center">
			<asp:label ID="LBmessaggio" Runat="server" CssClass="errore"></asp:label>
		</asp:TableCell>
	</asp:TableRow>
	<asp:TableRow Runat="server">
		<asp:TableCell>
			<asp:Label ID="LBnome_s" Runat="server" CssClass="Titolo_campo">Nome:</asp:Label>
		</asp:TableCell>
		<asp:TableCell  Runat="server" ColumnSpan=3>
			<asp:Label id="LBnome" Runat="server" CssClass="Testo_Campo"></asp:Label>
		</asp:TableCell>		
	</asp:TableRow>
	<asp:TableRow>
		<asp:TableCell>
			<asp:Label ID="LBcognome_s" Runat="server" CssClass="Titolo_campo">Cognome:</asp:Label>
		</asp:TableCell>
		<asp:TableCell ColumnSpan=3>
			<asp:label id="LBcognome" Runat="server"  CssClass="Testo_Campo"></asp:label>
		</asp:TableCell>	
	</asp:TableRow>
	<asp:TableRow Runat="server" ID="TBRdataNascitaSesso">
		<asp:TableCell Width="20%">
			<asp:Label ID="LBdatanascita_s" Runat="server" CssClass="Titolo_campo">Data Nascita(gg/mm/aaaa):</asp:Label>
		</asp:TableCell>
		<asp:TableCell Width="30%">
			<asp:Label id="LBdataNascita" Runat="server"  CssClass="Testo_Campo"></asp:Label>
		</asp:TableCell>
		<asp:TableCell Width="20%">
			<asp:Label ID="LBsesso_t" Runat="server" cssclass="Titolo_campo">&nbsp;Sesso:</asp:Label>
		</asp:TableCell>
		<asp:TableCell Width="30%">
			<asp:Label id="LBsesso" Runat="server"  CssClass="Testo_Campo"></asp:Label>
		</asp:TableCell>
	</asp:TableRow>
	<asp:TableRow ID="TBRindirizzo" Runat=server >
		<asp:TableCell>
			<asp:Label ID="LBindirizzo_t" Runat="server" CssClass="Titolo_campo">&nbsp;Indirizzo:</asp:Label>
		</asp:TableCell>
		<asp:TableCell ColumnSpan="3" >
			<asp:Label id="LBindirizzo" Runat="server" CssClass="Testo_Campo"></asp:Label>
		</asp:TableCell>
	</asp:TableRow>
	<asp:TableRow ID="TBRcapCitta" Runat=server>
		<asp:TableCell>
			<asp:Label ID="LBcap_t" Runat="server" CssClass="Titolo_campo">&nbsp;Cap:</asp:Label>
		</asp:TableCell>
		<asp:TableCell>
			<asp:Label id="LBcap" Runat="server" CssClass="Testo_Campo"></asp:Label>
		</asp:TableCell>
		<asp:TableCell>
			<asp:Label ID="LBcitta_t" Runat="server" CssClass="Titolo_campo">&nbsp;Città:</asp:Label>
		</asp:TableCell>
		<asp:TableCell >
			<asp:Label id="LBcitta" Runat="server"  CssClass="Testo_Campo"></asp:Label>
		</asp:TableCell>
	</asp:TableRow>
	<asp:TableRow ID="TBRtelFax" Runat=server >
		<asp:TableCell>
			<asp:Label ID="LBtelefono_t" Runat="server" CssClass="Titolo_campo">&nbsp;Telefono:</asp:Label>
		</asp:TableCell>
		<asp:TableCell >
			<asp:Label id="LBtelefono" Runat="server" CssClass="Testo_Campo"></asp:Label>
		</asp:TableCell>
		<asp:TableCell>
			<asp:Label ID="LBfax_t" Runat="server" CssClass="Titolo_campo">&nbsp;Fax:</asp:Label>
		</asp:TableCell>
		<asp:TableCell>
			<asp:Label id="LBfax" Runat="server" CssClass="Testo_Campo"></asp:Label>
		</asp:TableCell>
	</asp:TableRow>
	<asp:TableRow Runat=server ID="TBRcell">
		<asp:TableCell>
			<asp:Label ID="LBcellulare_t" Runat="server" CssClass="Titolo_campo">&nbsp;Cellulare:</asp:Label>
		</asp:TableCell>
		<asp:TableCell  ColumnSpan="3">
			<asp:Label id="LBcellulare" Runat="server" CssClass="Testo_Campo"></asp:Label>
		</asp:TableCell>
	</asp:TableRow>
	<asp:TableRow>
		<asp:TableCell >
			<asp:Label ID="LBmail_s" Runat="server" CssClass="Titolo_campo">Mail:</asp:Label>
		</asp:TableCell>
		<asp:TableCell  ColumnSpan="3">
			<asp:Label id="LBmail" Runat="server" CssClass="Testo_Campo"></asp:Label>
		</asp:TableCell>
	</asp:TableRow>
	
	<asp:TableRow  ID="TBRnazionalitaMadrelingua" Runat=server >
		<asp:TableCell>
			<asp:Label ID="LBnazionalita_t" Runat="server" CssClass="Titolo_campo">&nbsp;Nazionalità:</asp:Label>
		</asp:TableCell>
		<asp:TableCell>
			<asp:Label id="LBnazionalita" Runat="server" CssClass="Testo_Campo"></asp:Label>
		</asp:TableCell>
		<asp:TableCell>
			<asp:Label ID="LBmadrelingua_t" Runat="server" CssClass="Titolo_campo">&nbsp;Madrelingua:</asp:Label>
		</asp:TableCell>
		<asp:TableCell>
			<asp:Label id="LBmadrelingua" Runat="server" CssClass="Testo_Campo"></asp:Label>
		</asp:TableCell>
	</asp:TableRow>
	
	<asp:TableRow ID="TBRpatente" Runat=server>
		<asp:TableCell>
			<asp:Label ID="LBpatente_t" Runat="server" CssClass="Titolo_campo">&nbsp;Patente:</asp:Label>
		</asp:TableCell>
		<asp:TableCell ColumnSpan="3">
			<asp:Label id="LBpatente" Runat="server" CssClass="Testo_Campo"></asp:Label>
		</asp:TableCell>
	</asp:TableRow>
	
</asp:Table>