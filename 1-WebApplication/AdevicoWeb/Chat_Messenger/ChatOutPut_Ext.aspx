<%@ Page Language="vb" AutoEventWireup="false" Codebehind="ChatOutPut_Ext.aspx.vb" Inherits="Comunita_OnLine.ChatOutPut_Ext" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<html>
	<head runat="server">
		<title>Comunità On Line - Chat</title>
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR"/>
		<meta content="Visual Basic .NET 7.1" name="CODE_LANGUAGE"/>
		<meta content="JavaScript" name="vs_defaultClientScript"/>
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema"/>
<%--		<link href="./../Styles.css" type="text/css" rel="stylesheet"/>--%>		
		<style type="text/css">
		body {
			margin: 10px;
			padding:10px;
			font-size: small;
		}
	</style>
	</head>
	
	
	
	<body style="FONT-FAMILY:Arial" runat="server" id="Body">
		<form id="aspnetForm" method="post" runat="server">
		<asp:ScriptManager ID="SCMmanager" runat="server"></asp:ScriptManager>
			<table cellpadding="0" cellspacing="4" border="0" width="100%">
				<TR>
					<td valign="top">
						<asp:Label Runat="server" ID="lblout" CssClass="chat_output"></asp:Label>
					</td>
				</TR>
				<%--<tr>
					<td height="100%"><font size=xx-small>&nbsp;</font></td>
				</tr>--%>
			</table>
		</form>
		<script language="javascript" type="text/javascript">
			window.scrollTo(0, document.body.scrollHeight);
		</script>
	</body>
</HTML>
