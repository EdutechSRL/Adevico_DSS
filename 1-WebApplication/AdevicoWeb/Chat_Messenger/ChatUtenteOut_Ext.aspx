<%@ Page Language="vb" AutoEventWireup="false" Codebehind="ChatUtenteOut_Ext.aspx.vb" Inherits="Comunita_OnLine.chatUtenteOut_Ext" validateRequest="false" %>


<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<HTML>
  <head runat="server">
		<title>Comunità On Line - Chat</title>
		<script type="text/javascript" language="Javascript" src="./../jscript/generali.js"></script>
		<meta content="Microsoft Visual Studio.NET 7.0" name="GENERATOR"/>
		<meta content="Visual Basic 7.0" name="CODE_LANGUAGE"/>
		<meta content="JavaScript" name="vs_defaultClientScript"/>
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema"/>
		<LINK href="./../Styles.css" type="text/css" rel="stylesheet"/>
  </HEAD>
	<body>
		<form id="aspnetForm" method="post" encType="multipart/form-data" runat="server">
		<asp:ScriptManager ID="SCMmanager" runat="server"></asp:ScriptManager>
			<TABLE class="contenitore" cellSpacing="0" cellPadding="0" align="center">
				<tr>
					<td class="Chat_Pulsante">
						<P>&nbsp;</P>
						<P align="center"><asp:Label ID="Lbl_Messaggio" cssclass=Chat_Pulsante Runat=server>Molto probabilmente l'amministratore della chat ha ritenuto opportuno impedirti l'accesso.
						</asp:Label></P>
						<P align="center">...</P>
						<P>&nbsp;</P>
					</td>
				</tr>
			</TABLE>
		</form>
	</body>
</HTML>