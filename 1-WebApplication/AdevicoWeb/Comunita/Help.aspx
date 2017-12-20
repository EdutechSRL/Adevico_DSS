<%@ Page Language="vb" AutoEventWireup="false" Codebehind="Help.aspx.vb" Inherits="Comunita_OnLine.Help"%>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<html>
  <head runat="server">
    <title>Help - Accesso Comunità</title>
    <meta name="GENERATOR" content="Microsoft Visual Studio .NET 7.1"/>
    <meta name="CODE_LANGUAGE" content="Visual Basic .NET 7.1"/>
    <meta name=vs_defaultClientScript content="JavaScript"/>
    <meta name=vs_targetSchema content="http://schemas.microsoft.com/intellisense/ie5"/>
    <LINK href="./../Styles.css" type="text/css" rel="stylesheet"/>
  </head>
  <body id="popup">
    <form id="aspnetForm" method="post" runat="server">
		<table align=center width=100%>
			<tr>
				<td align=center >
					<asp:Label ID="LBtitolo" Runat=server CssClass="HelpTitolo">Accesso/iscrizione comunità</asp:Label>
				</td>
			</tr>
			<tr>
				<td>
					&nbsp;
				</td>
			</tr>
			<tr>
				<td align=center>
					<table width=100%>
						<tr>
							<td class=top><span class="HelpNumero">1°&nbsp;</span></td>
							<td>
								<asp:Label Runat=server ID="LBcomunita" CssClass="HelpTesto">Selezionare la comunità desiderata</asp:Label>:
								<br/>
								<img src="./help/comunita.jpg" alt="" align=right border=0/>
							</td>
						</tr>
						<tr>
							<td class=top><span class="HelpNumero">2°&nbsp;</span></td>
							<td>
								<asp:Label ID="LBpulsanteDestro" Runat=server CssClass="HelpTesto">
									Premere il pulsante destro del mouse.
								</asp:Label>
							</td>
						</tr>
						<tr>
							<td class=top><span class="HelpNumero">3°&nbsp;</span></td>
							<td>
								<asp:Label ID="LBdati" Runat=server CssClass="HelpTesto">
									A seconda della comunità compariranno i seguenti menu contestuali:
								</asp:Label>
								<br/>
								<table align=right class=top>
									<tr>
										<td class=top>
											<img src="./help/aggiorna.jpg" alt=""/>
										</td>
										<td class=top>
											<img src="./help/Iscrivi.jpg" alt=""/>
										</td>
										<td class=top>
											<img src="./help/entra.jpg" alt=""/>
										</td>									
									</tr>
									<tr>
										<td colspan=3 class=top>
										<br/>
											<asp:Label ID="LBspiegazione" Runat=server CssClass="HelpTesto">
											con i quali sarà possibile aggiornare l'albero delle comunità,	
											entrare nella comunità prescelta oppure mostrarne i dettagli od infine
											iscriversi.
											</asp:Label>	
										</td>
									</tr>
								</table>
						
							</td>
						</tr>
					</table>
				</td>
			</tr>
			<tr>
				<td align=right >
					<a href="#" onclick="window.close();return false;">Close</a>
				</td>
			</tr>
		</table>
    </form>

  </body>
</html>
