<%@ Page Language="vb" AutoEventWireup="false" Codebehind="Chat_Ext_Login.aspx.vb" Inherits="Comunita_OnLine.Chat_Ext_Login"%>
<%@ Register TagPrefix="UC" TagName="ChatLogin" Src="UC_LoginChat.ascx" %>
<%@ Register TagPrefix="FOOTER" TagName="CTRLFooter" Src="UC_ChatFoot.ascx" %>
<%@ Register TagPrefix="HEAD" TagName="CTRLHeadChat" Src="UC_ChatHead.ascx" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<HTML>
	<head runat="server">
		<title>Chat_Ext_Login</title>
		<meta name="GENERATOR" content="Microsoft Visual Studio .NET 7.1"/>
		<meta name="CODE_LANGUAGE" content="Visual Basic .NET 7.1"/>
		<meta name="vs_defaultClientScript" content="JavaScript"/>
		<meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5"/>
		<LINK href="./../Styles.css" type="text/css" rel="stylesheet"/>
	</HEAD>
	<body >
		<form id="aspnetForm" method="post" runat="server">
		<asp:ScriptManager ID="SCMmanager" runat="server"></asp:ScriptManager>
			<table align="center" border=0 cellspacing=0 cellpadding=0>
			<tr>
				<td>
				<HEAD:CTRLHEADCHAT id="UC_ChatHead" runat="server"></HEAD:CTRLHEADCHAT>
				</td>
			</tr>
			<%--<tr>
				<td>
				<UC:ChatLogin id="UC_ChatLogin" runat="Server"></UC:ChatLogin>
				<asp:Label Visible=False ID="LblGestioneWEB" Runat=server>Accesso non disponibile: aggiornamenti in corso</asp:Label>
				</td>
			</tr>--%>
			<tr>
				<td align=center>
					<a href="javascript:window.close();" id="HYLchiudi" runat="server" class="notaRecuperoPwd">Chiudi finestra</a>
					<br/><br/>
				</td>
			</tr>
			<tr>
				<td>
				<FOOTER:CTRLFOOTER id="CTRLFooter" runat="server"></FOOTER:CTRLFOOTER>
				</td>
			</tr>
			</table>
			
		</form>
	</body>
</HTML>