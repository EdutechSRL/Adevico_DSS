<%@ Page Language="vb" AutoEventWireup="false" Codebehind="DettaglioEvento.aspx.vb" Inherits="Comunita_OnLine.DettaglioEvento_aspx"%>
<%@ Register TagPrefix="radCTRL" Namespace="Telerik.WebControls" Assembly="RadEditor.net2" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<html>
	<head runat="server">
		<title>DETTAGLIO EVENTO</title>
		<meta name="GENERATOR" content="Microsoft Visual Studio .NET 7.1"/>
		<meta name="CODE_LANGUAGE" content="Visual Basic .NET 7.1"/>
		<meta name="vs_defaultClientScript" content="JavaScript"/>
		<LINK href="./../Styles.css" type="text/css" rel="stylesheet"/>
		<meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5"/>
		
		<META HTTP-EQUIV="CACHE-CONTROL" CONTENT="NO-CACHE"/>
		<META HTTP-EQUIV="PRAGMA" CONTENT="NO-CACHE"/>
		<meta http-equiv="expires" content="0"/>
	</head>
	<script language="javascript" type="text/javascript">
	        function ChiudiMi(){
			    this.window.close();
			}
		</script>
	<body>
		<form id="aspnetForm" method="post" runat="server">
		<asp:ScriptManager ID="SCMmanager" runat="server"></asp:ScriptManager>
			<table>
				<tr>
					<td >
						<asp:Table width="100%"  HorizontalAlign=Center Runat="server" CellSpacing="4">
							<asp:TableRow>
								<asp:TableCell ColumnSpan="2" BackColor="#c3e0e6">
									&nbsp;&nbsp;<asp:Label ID="LBgiorno" Runat="server" Font-Name="tahoma" Font-Size="9" Font-Bold="True"></asp:Label>
									&nbsp;&nbsp;<asp:Label ID="LBdata" Runat="server" Font-Name="tahoma" Font-Size="18" Font-Bold="True"></asp:Label>
									&nbsp;&nbsp;<asp:Label ID="LBmese" Runat="server" Font-Name="tahoma" Font-Size="9" Font-Bold="True"></asp:Label>
								</asp:TableCell>
							</asp:TableRow>
							<asp:TableRow>
								<asp:TableCell ColumnSpan=2><hr/></asp:TableCell>
							</asp:TableRow>
							<asp:TableRow BackColor=#f0f0d5>
								<asp:TableCell ColumnSpan="2"  BorderWidth="1" Width="400px" >
									<br/>&nbsp;&nbsp;<asp:Label ID="LBNomeEvento" Runat="server" Width="400px" Font-Bold="True" Font-Name="tahoma" Font-Size="12" ></asp:Label><br/>
								</asp:TableCell>
							</asp:TableRow>
							<asp:TableRow>
								<asp:TableCell ColumnSpan="2"><hr/></asp:TableCell>
							</asp:TableRow>
						</asp:Table>
						<asp:Table width="100%" Runat="server" CellSpacing="4">
							<asp:TableRow BackColor=#f5f5f5>
								<asp:TableCell Width="200px">
									<asp:Label ID="LBtipoEventoTesto" Runat="server" Font-Name="tahoma" ForeColor="#00008b" Font-Size="9">tipo evento</asp:Label>
								</asp:TableCell>
								<asp:TableCell>
									<asp:Label ID="LBtipoEvento" Runat="server" Width="400" Font-Bold="True" Font-Name="tahoma" Font-Size="9"></asp:Label>
								</asp:TableCell>
							</asp:TableRow>
							<asp:TableRow BackColor=#f0f0d5 ID="TBRcomunita">
								<asp:TableCell Width="200px">
									<asp:Label ID="LBcomunitaTesto" Runat="server" Font-Name="tahoma" ForeColor="#00008b" Font-Size="9">comunità</asp:Label>
								</asp:TableCell>
								<asp:TableCell>
									<asp:Label ID="LBcomunita" Runat="server" Width="400" Font-Bold="True" Font-Name="tahoma" Font-Size="9"></asp:Label>
								</asp:TableCell>
							</asp:TableRow>
							<asp:TableRow BackColor=#f5f5f5 id="TBRluogo">
								<asp:TableCell Width="200px">
									<asp:Label ID="LBluogoTesto" Runat="server" Font-Name="tahoma" ForeColor="#00008b" Font-Size="9">luogo</asp:Label>
								</asp:TableCell>
								<asp:TableCell>
									<asp:Label ID="LBluogo" Runat="server" Width="400" Font-Bold="True" Font-Name="tahoma" Font-Size="9"></asp:Label>
								</asp:TableCell>
							</asp:TableRow>
							<asp:TableRow BackColor=#f0f0d5 ID="TBRaula">
								<asp:TableCell Width="200px">
									<asp:Label ID="LBAulaTesto" Runat="server" Font-Name="tahoma" ForeColor="#00008b" Font-Size="9">aula</asp:Label>
								</asp:TableCell>
								<asp:TableCell>
									<asp:Label ID="LBaula" Runat="server" Width="400" Font-Bold="True" Font-Name="tahoma" Font-Size="9"></asp:Label>
								</asp:TableCell>
							</asp:TableRow>
							<asp:TableRow BackColor=#f5f5f5 ID="TBRinizio">
								<asp:TableCell Width="200px">
									<asp:Label ID="LBInizioTesto" Runat="server" Font-Name="tahoma" ForeColor="#00008b" Font-Size="9">Inizio</asp:Label>
								</asp:TableCell>
								<asp:TableCell>
									<asp:Label ID="LBInizio" Runat="server" Width="400" Font-Bold="True" Font-Name="tahoma" Font-Size="9"></asp:Label>
								</asp:TableCell>
							</asp:TableRow>
							<asp:TableRow BackColor=#f0f0d5 ID="TBRfine">
								<asp:TableCell Width="200px">
									<asp:Label ID="LBFineTesto" Runat="server" Font-Name="tahoma" ForeColor="#00008b" Font-Size="9">Fine</asp:Label>
								</asp:TableCell>
								<asp:TableCell>
									<asp:Label ID="LBFine" Runat="server" Width="400" Font-Bold="True" Font-Name="tahoma" Font-Size="9"></asp:Label>
								</asp:TableCell>
							</asp:TableRow>
							<asp:TableRow BackColor=#f5f5f5 ID="TBRanno">
								<asp:TableCell Width="200px">
									<asp:Label ID="LBAnnoAccTesto" Runat="server" Font-Name="tahoma" ForeColor="#00008b" Font-Size="9">Anno Acc.</asp:Label>
								</asp:TableCell>
								<asp:TableCell>
									<asp:Label ID="LBannoAccademico" Runat="server" Width="400" Font-Bold="True" Font-Name="tahoma"
										Font-Size="9"></asp:Label>
								</asp:TableCell>
							</asp:TableRow>
							<asp:TableRow BackColor=#f0f0d5 ID="TBRreferente">
								<asp:TableCell Width="200px">
									<asp:Label ID="LBReferenteTesto" Runat="server" Font-Name="tahoma" ForeColor="#00008b" Font-Size="9">Referente</asp:Label>
								</asp:TableCell>
								<asp:TableCell>
									<asp:Label ID="LBReferente" Runat="server" Width="400" Font-Bold="True" Font-Name="tahoma" Font-Size="9"></asp:Label>
								</asp:TableCell>
							</asp:TableRow>
							<asp:TableRow BackColor=#f5f5f5 ID="TBRlink">
								<asp:TableCell Width="200px">
									<asp:Label ID="LBLinkTesto" Runat="server" Font-Name="tahoma" ForeColor="#00008b" Font-Size="9">link</asp:Label>
								</asp:TableCell>
								<asp:TableCell>
									<asp:HyperLink Runat="server" ID="HLlink" Target="_blank" Font-Bold="True" Font-Name="tahoma" Font-Size="9"></asp:HyperLink>
								</asp:TableCell>
							</asp:TableRow>
							<asp:TableRow BackColor=#f0f0d5 ID="TBRvisibile">
								<asp:TableCell Width="200px">
									<asp:Label ID="LBvisibileTesto" Runat="server" Font-Name="tahoma" ForeColor="#00008b" Font-Size="9">visibile</asp:Label>
								</asp:TableCell>
								<asp:TableCell>
									<asp:Label ID="LBvisibile" Runat="server" Width="400" Font-Bold="True" Font-Name="tahoma" Font-Size="9"></asp:Label>
								</asp:TableCell>
							</asp:TableRow>
							<asp:TableRow BackColor=#f5f5f5 ID="TBRnote">
								<asp:TableCell Width="200px">
									<asp:Label ID="LBNoteTesto" Runat="server" Font-Name="tahoma" ForeColor="#00008b" Font-Size="9">note</asp:Label>
								</asp:TableCell>
								<asp:TableCell VerticalAlign="Top">
									<asp:Label ID="LBLnote" Runat=server ></asp:Label>
								</asp:TableCell>
							</asp:TableRow>
							<asp:TableRow BackColor=#f0f0d5 ID="TBRprogramma">
								<asp:TableCell Width="200px">
									<asp:Label ID="LBProgramma" Runat="server" Font-Name="tahoma" ForeColor="#00008b" Font-Size="9">programma</asp:Label>
								</asp:TableCell>
								<asp:TableCell VerticalAlign="Top">
									<asp:Label ID="LBLProgramma" Runat=server ></asp:Label>
								</asp:TableCell>
							</asp:TableRow>
							<asp:TableRow Height="50px">
								<asp:TableCell ColumnSpan="2" HorizontalAlign="Right" BackColor="#c3e0e6">
									<asp:Button ID="BTNChiudi" Runat="server" Text="Chiudi" CssClass="pulsante"></asp:Button>&nbsp;&nbsp;&nbsp;
								</asp:TableCell>
							</asp:TableRow>
						</asp:Table>
					</td>
				</tr>
			</table>
		</form>
	</body>
</html>