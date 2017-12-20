<%@ Page Language="vb" AutoEventWireup="false" Codebehind="ChatBuffer_Ext.aspx.vb" Inherits="Comunita_OnLine.ChatBuffer_Ext" %>

<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<HTML>
	<head runat="server">
		<title>Comunità On Line - Chat</title>
		<meta name="GENERATOR" content="Microsoft Visual Studio .NET 7.1">
		<meta name="CODE_LANGUAGE" content="Visual Basic .NET 7.1">
		<meta name="vs_defaultClientScript" content="JavaScript">
		<meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5">
	</HEAD>
	<body >
		<form id="aspnetForm" method="post" runat="server">
		<asp:ScriptManager ID="SCMmanager" runat="server"></asp:ScriptManager>
			<asp:DataGrid id="DGMessaggi" style="Z-INDEX: 101; LEFT: 360px; POSITION: absolute; TOP: 80px"
				runat="server" Width="568px"></asp:DataGrid>
			<asp:DataGrid id="DGUtenti" style="Z-INDEX: 102; LEFT: 8px; POSITION: absolute; TOP: 80px" runat="server"
				Width="342px"></asp:DataGrid>
			<asp:Label id="Label1" style="Z-INDEX: 103; LEFT: 16px; POSITION: absolute; TOP: 8px" runat="server">ID Comunita: </asp:Label>
			<asp:Label id="LbWBSUrl" style="Z-INDEX: 104; LEFT: 16px; POSITION: absolute; TOP: 32px" runat="server">Indirizzo WebServer: </asp:Label>
		</form>
	</body>
</HTML>