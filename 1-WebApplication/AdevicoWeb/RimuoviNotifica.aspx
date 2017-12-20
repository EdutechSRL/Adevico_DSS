<%@ Page Language="vb" AutoEventWireup="false" Codebehind="RimuoviNotifica.aspx.vb" Inherits="Comunita_OnLine.RimuoviNotifica"%>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<HTML>
	<head runat="server">
		<title>Comunità On Line - Elimina notifica automatica</title>
		<meta name="GENERATOR" content="Microsoft Visual Studio .NET 7.1"/>
		<meta name="CODE_LANGUAGE" content="Visual Basic .NET 7.1"/>
		<meta name="vs_defaultClientScript" content="JavaScript"/>
		<meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5"/>
		<LINK href="./Styles.css" type="text/css" rel="stylesheet"/>
	</HEAD>
	<body bgcolor="#ffffff" onkeydown="return SubmitRicerca(event);">
		 <form id="aspnetForm" method="post" runat="server">
			<table align=center width=100% style="height:100%">
				<tr>
					<td align=center valign=middle >
						<asp:Panel ID="PNLInfo" Runat=server Visible=true >
							<table  align=center width=700px>
								<tr>
									<td colspan=2>
										<asp:Label ID="LBinfoRimuoviNotifica" Runat=server></asp:Label>
									</td>
								</tr>
								<tr>
									<td colspan=2 align=center>&nbsp;</td>
								</tr>
								<tr>
									<td>
										<asp:Button ID="BTNhome" Runat=server CssClass="Pulsante_Menu" Text="Comunità On Line"></asp:Button>
									</td>
									<td align=right >
										<asp:Button id="BTNchiudi" Runat=server CssClass="Pulsante_Menu" Text="Chiudi Finestra"></asp:Button>&nbsp;
										<asp:Button id="BTNconferma" Runat=server CssClass="Pulsante_Menu" Text="Conferma"></asp:Button>
										<input type=hidden id="HDN_PersonaID" runat=server NAME="HDN_PersonaID"/>
										<input type=hidden id="HDN_ForumID" runat=server NAME="HDN_ForumID"/>
										<input type=hidden id="HDN_TopicID" runat=server NAME="HDN_TopicID"/>
										<input type=hidden id="HDN_ComunitaID" runat=server NAME="HDN_ComunitaID"/>
										<input type=hidden id="HDN_PostID" runat=server NAME="HDN_PostID"/>
									</td>
								</tr>
							</table>
						</asp:Panel>
					</td>
				</tr>
			</table>
		</form>
	</body>
</HTML>
