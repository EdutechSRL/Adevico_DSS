<%@ Page Language="vb" AutoEventWireup="false" Codebehind="ChatPrint.aspx.vb" Inherits="Comunita_OnLine.ChatPrint"%>

<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<HTML>
	<head runat="server">
		<title>ChatPrint</title>
		<meta name="GENERATOR" content="Microsoft Visual Studio .NET 7.1"/>
		<meta name="CODE_LANGUAGE" content="Visual Basic .NET 7.1"/>
		<meta name="vs_defaultClientScript" content="JavaScript"/>
		<meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5"/>
		<LINK href="./../Styles.css" type="text/css" rel="stylesheet"/>
	</HEAD>
	<body>
		<form id="aspnetForm" method="post" runat="server">
		<asp:ScriptManager ID="SCMmanager" runat="server"></asp:ScriptManager>
			<table class="chat_chiaro" cellSpacing="10" width=100% cellPadding="5" border="0" align="center" ID="Table1">
				<tr>
					<td align=center colspan=2>
						<asp:Label id="LblComunita" runat="server" CssClass="Chat_Comunita">Comunita</asp:Label>
					</td>
				</tr>
				<tr>
					<td>&nbsp;</td>
					<td>
						<asp:Label id="LblData_t" runat="server">Data: </asp:Label>
						<asp:Label id="LblTime" runat="server"></asp:Label>
					</td>
				</tr>
				<tr>
					<td>&nbsp;</td>
					<td>
						<asp:Label id="LblMessaggi" Runat="server"></asp:Label>
					</td>
				</tr>
			</table>
		</form>
	</body>
</HTML>
