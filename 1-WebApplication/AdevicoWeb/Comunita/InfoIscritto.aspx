<%@ Page Language="vb" AutoEventWireup="false" Codebehind="InfoIscritto.aspx.vb" Inherits="Comunita_OnLine.InfoIscritto"%>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<HTML>
	<head runat="server">
		<title>Comunità on Line</title>
		<meta name="GENERATOR" content="Microsoft Visual Studio .NET 7.1"/>
		<meta name="CODE_LANGUAGE" content="Visual Basic .NET 7.1"/>
		<meta name="vs_defaultClientScript" content="JavaScript"/>
		<meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5"/>
		<LINK href="../Styles.css" type="text/css" rel="stylesheet"/>
	</HEAD>
	<script language="javascript" type="text/javascript">
			function ChiudiMi(){
			this.window.close();
			}
		</script>
	<body id="popup">
		<form id="aspnetForm" method="post" runat="server">
		<asp:ScriptManager ID="SCMmanager" runat="server"></asp:ScriptManager>
			<FIELDSET><LEGEND class="">
				<asp:Label ID="LBinfo" Runat=server CssClass="tableLegend">Info</asp:Label>
				</LEGEND>
				<br/>
				<asp:Table CellPadding=0 CellSpacing=0 Width=100% Runat=server ID="TBLDettagli">
				
					<asp:TableRow>
						<asp:TableCell Width=5px CssClass="nosize0">&nbsp;</asp:TableCell>
						<asp:TableCell cssclass="Top" >
							<asp:Label ID="LBanagrafica" Runat=server cssclass="Titolo_campo_Rosso">Anagrafica</asp:Label>
						</asp:TableCell>
						<asp:TableCell Width=5px CssClass="nosize0" ColumnSpan=3>&nbsp;</asp:TableCell>
					</asp:TableRow>
					
					<asp:tableRow Height=22px>
						<asp:TableCell Width=5px CssClass="nosize0">&nbsp;</asp:TableCell>
						<asp:tableCell CssClass="dettagli_separatore" ColumnSpan=3>
						<img src="./../images/separatore_small.gif" alt="" align="top" border="0" width="100%"
							height="2px" /></asp:tableCell>
						<asp:TableCell Width=5px CssClass="nosize0">&nbsp;</asp:TableCell>
					</asp:tableRow>
							
					<asp:TableRow Height=22px>
						<asp:TableCell Width=5px CssClass="nosize0">&nbsp;</asp:TableCell>
						<asp:TableCell cssclass="Top">
							<asp:Label ID="LBlogin_c" Runat=server cssclass="Titolo_campo">Login:</asp:Label>
						</asp:TableCell>
						<asp:TableCell cssclass="Top" >
							<asp:Label Runat="server" ID="LBlogin" cssclass="Testo_campo"></asp:Label>
						</asp:TableCell>
						
						<asp:TableCell cssclass="Top" RowSpan=6>
							<asp:image id="IMFoto" Runat="server" Visible="False" Width="85px" ToolTip="Immagine Personale"></asp:image>
						</asp:TableCell>
						<asp:TableCell Width=5px CssClass="nosize0">&nbsp;</asp:TableCell>
						
					</asp:TableRow>
					
					<asp:TableRow Height=22px>
						<asp:TableCell Width=5px CssClass="nosize0">&nbsp;</asp:TableCell>
						<asp:TableCell cssclass="Top" >
							<asp:Label ID="LBnome_t" Runat=server cssclass="Titolo_campo">Nome:</asp:Label>
						</asp:TableCell>
						<asp:TableCell cssclass="Top" >
							<asp:Label Runat="server" ID="LBNome" cssclass="Testo_campo"></asp:Label>
						</asp:TableCell>
						<asp:TableCell Width=5px CssClass="nosize0" ColumnSpan=2>&nbsp;</asp:TableCell>
					</asp:TableRow>
					
					<asp:TableRow Height=22px>
					<asp:TableCell Width=5px CssClass="nosize0">&nbsp;</asp:TableCell>
						<asp:TableCell cssclass="Top" >
							<asp:Label ID="LBcognome_t" Runat=server cssclass="Titolo_campo">Cognome:</asp:Label>
						</asp:TableCell>
						<asp:TableCell cssclass="Top" >
							<asp:Label Runat="server" ID="LBCognome" cssclass="Testo_campo"></asp:Label>
						</asp:TableCell>
						<asp:TableCell Width=5px CssClass="nosize0" ColumnSpan=2>&nbsp;</asp:TableCell>
					</asp:TableRow>
					
					<asp:TableRow Height=22px>
						<asp:TableCell Width=5px CssClass="nosize0">&nbsp;</asp:TableCell>
						<asp:TableCell cssclass="Top" >
							<asp:Label ID="LBorganizzazione_t" Runat=server cssclass="Titolo_campo">Facoltà:</asp:Label>
						</asp:TableCell>
						<asp:TableCell cssclass="Top">
							<asp:Label Runat="server" ID="LBorganizzazione" cssclass="Testo_campo"></asp:Label>
						</asp:TableCell>
						<asp:TableCell Width=5px CssClass="nosize0" ColumnSpan=2>&nbsp;</asp:TableCell>
					</asp:TableRow>
					
					<asp:TableRow Height=22px>
						<asp:TableCell Width=5px CssClass="nosize0">&nbsp;</asp:TableCell>
						<asp:TableCell cssclass="Top" >
							<asp:Label ID="LBtipoPersona_t" Runat=server cssclass="Titolo_campo">Tipo Persona:</asp:Label>
						</asp:TableCell>
						<asp:TableCell cssclass="Top">
							<asp:Label Runat="server" ID="LBtipoPersona" cssclass="Testo_campo"></asp:Label>
						</asp:TableCell>
						<asp:TableCell Width=5px CssClass="nosize0" ColumnSpan=2>&nbsp;</asp:TableCell>
					</asp:TableRow>
					
					<asp:TableRow Height=22px>
						<asp:TableCell Width=5px CssClass="nosize0">&nbsp;</asp:TableCell>
						<asp:TableCell cssclass="Top" >
							<asp:Label ID="LBdataIscrizione_t" Runat=server cssclass="Titolo_campo">Data Iscrizione:</asp:Label>
						</asp:TableCell>
						<asp:TableCell cssclass="Top">
							<asp:Label Runat="server" ID="LBdataIscrizione" cssclass="Testo_campo"></asp:Label>
						</asp:TableCell>
						<asp:TableCell Width=5px CssClass="nosize0" ColumnSpan=2>&nbsp;</asp:TableCell>
					</asp:TableRow>
					
					<asp:TableRow Height=22px>
						<asp:TableCell Width=5px CssClass="nosize0">&nbsp;</asp:TableCell>
						<asp:TableCell cssclass="Top" >
							<asp:Label ID="LBlingua_c" Runat=server cssclass="Titolo_campo">Lingua:</asp:Label>
						</asp:TableCell>
						<asp:TableCell cssclass="Top">
							<asp:Label Runat="server" ID="LBlingua" cssclass="Testo_campo"></asp:Label>
						</asp:TableCell>
						<asp:TableCell Width=5px CssClass="nosize0" ColumnSpan=2>&nbsp;</asp:TableCell>
					</asp:TableRow>
					
					
					<asp:TableRow Height=22px ID="TBRmail">
						<asp:TableCell Width=5px CssClass="nosize0">&nbsp;</asp:TableCell>
						<asp:TableCell cssclass="Top" >
							<asp:Label ID="LBMail_c" Runat=server cssclass="Titolo_campo">Mail:</asp:Label>
						</asp:TableCell>
						<asp:TableCell cssclass="Top" >
							<asp:HyperLink ID="HLmail" Runat=server ><asp:Label Runat="server" ID="LBMail" cssclass="Testo_campo"></asp:Label></asp:HyperLink>
						</asp:TableCell>
						<asp:TableCell Width=5px CssClass="nosize0" ColumnSpan=2>&nbsp;</asp:TableCell>
					</asp:TableRow>
					
					<asp:TableRow ID="TBRhomePage" Height=22px>
						<asp:TableCell Width=5px CssClass="nosize0">&nbsp;</asp:TableCell>
						<asp:TableCell cssclass="Top" >
							<asp:Label ID="LBHomePage_c" Runat=server cssclass="Titolo_campo">Home Page:</asp:Label>
						</asp:TableCell>
						<asp:TableCell cssclass="Top">
							<asp:Label Runat="server" ID="LBHomePage" cssclass="Testo_campo"></asp:Label>
						</asp:TableCell>
						<asp:TableCell Width=5px CssClass="nosize0" ColumnSpan=2>&nbsp;</asp:TableCell>
					</asp:TableRow>
					
					<asp:TableRow ID="TBRdettagliRuolo">
						<asp:TableCell Width=5px CssClass="nosize0">&nbsp;</asp:TableCell>
						<asp:TableCell cssclass="Top" >
							<asp:Label ID="LBdettagliRuolo" Runat=server cssclass="Titolo_campo_Rosso">Dettagli Ruolo</asp:Label>
						</asp:TableCell>
						<asp:TableCell >&nbsp;</asp:TableCell>
						<asp:TableCell Width=5px CssClass="nosize0" ColumnSpan=2>&nbsp;</asp:TableCell>
					</asp:TableRow>
					
					<asp:tableRow Height=22px>
						<asp:TableCell Width=5px CssClass="nosize0">&nbsp;</asp:TableCell>
						<asp:tableCell CssClass="dettagli_separatore" ColumnSpan=3>
							<img src="./../images/separatore_small.gif" alt="" align="top" border="0" width="100%"
								height="2px" />
						</asp:tableCell>
						<asp:TableCell Width=5px CssClass="nosize0" ColumnSpan=2>&nbsp;</asp:TableCell>
					</asp:tableRow>
					<asp:TableRow ID="TBRmansione" Height=22px>
						<asp:TableCell Width=5px CssClass="nosize0">&nbsp;</asp:TableCell>
						<asp:TableCell cssclass="Top" >
							<asp:Label ID="LBMansione_c" Runat=server cssclass="Titolo_campo">Mansione:</asp:Label>
						</asp:TableCell>
						<asp:TableCell cssclass="Top" >
							<asp:Label Runat="server" ID="LBMansione" cssclass="Testo_campo"></asp:Label>
						</asp:TableCell>
						<asp:TableCell Width=5px CssClass="nosize0">&nbsp;</asp:TableCell>
					</asp:TableRow>
				</asp:Table>
				<br/>
			</FIELDSET>
			<table align="center" border="0">
				<tr>
					<td><asp:label ID="LBdisclaimer" Runat=server>"Tutte le altre informazioni dell'utente non sono visibile secondo il Dlgs 196/2003 - Codice in materia di protezione dei dati personali"</asp:label></td>
				</tr>
				<tr>
					<td align="right"><asp:Button Runat="server" ID="BTNOk" Text="Chiudi" CssClass="pulsante"></asp:Button></td>
				</tr>
			</table>
		</form>
	</body>
</HTML>
