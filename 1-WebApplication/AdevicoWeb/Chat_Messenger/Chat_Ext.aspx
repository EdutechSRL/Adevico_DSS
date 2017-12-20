<%@ Page Language="vb" AutoEventWireup="false" Codebehind="Chat_Ext.aspx.vb" Inherits="Comunita_OnLine.Chat_Ext" ValidateRequest="false" EnableViewState="true"%>
<%@ Register TagPrefix="UC" TagName="Chatcomunita" Src="UC_comunitaChat.ascx" %>
<%@ Register TagPrefix="UC" TagName="ChatUtenti" Src="UC_Partecipanti_Chat.ascx" %>
<%@ Register TagPrefix="UC" TagName="ChatFile" Src="UC_ChatFile.ascx" %>
<%@ Register TagPrefix="UC" TagName="ChatUtility" Src="UC_UtilityChat.ascx" %>
<%@ Register TagPrefix="UC" TagName="ChatSmile" Src="../UC/VisEmo.ascx" %>
<%@ Register TagPrefix="UC" TagName="ChatStili" Src="UC_StiliChat.ascx" %>
<!DOCTYPE HTML PUBLIC "-//W3C//Dtd HTML 4.0 Transitional//EN">
<html>
  <head runat="server">
		<title>Comunità On Line - Chat</title>
		<meta http-equiv="Content-Type" content="text/html; charset=windows-1252"/>
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR"/>
		<meta content="Visual Basic .NET 7.1" name="CODE_LANGUAGE"/>
		<meta content="JavaScript" name="vs_defaultClientScript"/>
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema"/>
		<link href="./../Styles.css" type="text/css" rel="stylesheet"/>
		
		<asp:Literal ID="Lit_Skin" runat="server"></asp:Literal>
		
</head>
<script language="javascript" type="text/javascript">
	function AggiornaFocus(){
		 try {
		     //		     document.FormChat.TBInput.focus();
		     TBInput.focus();
            }
		catch(e){
			return true;}
	}
</script>
<script language="javascript" type="text/javascript">
function Emot(TxtEmo) {
//    document.FormChat.TBInput.value += " " + TxtEmo;
    TBInput.value += " " + TxtEmo;
}
</script>
	<body onkeypress="if(event.keyCode==13){document.FormChat.BtSendAll.click()}" onload="AggiornaFocus()">
		<form id="FormChat" method="post" enctype="multipart/form-data" runat="server">
		<asp:ScriptManager ID="SCMmanager" runat="server"></asp:ScriptManager>
		    <asp:Panel ID="Pnl_Total" runat="server" Visible="true">
			<table class="chat_chiaro" height="100%" width="100%" cellspacing="0" cellpadding="0" border="0"
				runat="server" align="center">
				<tr>
					<td colspan="3" align="center">
						<br/>
						<asp:Label ID="LblComunita" Runat="server" CssClass="Chat_Comunita"></asp:Label><br/>
						<br/>
						<asp:Label ID="UtentiCom" Runat="server"></asp:Label>
						<asp:Label ID="LblNumUtenti" runat="server"></asp:Label>
					</td>
				</tr>
				<tr>
					<td>
						<asp:panel id="Pan_Write" Height="50" Runat="server">
      <table cellspacing="0" cellpadding="0" width="100%" border="0">
        <tr>
          <td width="100%" height="70px">
            <textarea cols="1" class="chat_input" id="TBInput" style="WIDTH:100%" name="TBInput" rows="4" runat="server"></textarea> 
          </td>
          <td width="100px">
<asp:button id="BtSendAll" runat="server" CssClass="Chat_Pulsante" Width="100px" Text="To all"></asp:button><br/>
<asp:button id="BtSendTo" accessKey="t" runat="server" CssClass="Chat_Pulsante" Width="100px" Text="To"></asp:button><br/>
<asp:button id="BtPrivate" accessKey="p" runat="server" CssClass="Chat_Pulsante" Width="100px" Text="Private"></asp:button></td></tr>
        <tr>
          <td>
<asp:label id="LBError" runat="server" CssClass="avviso" Visible="False" Font-Size="XX-Small">
						Please, select a User.
										</asp:label></td></tr></table>
						</asp:panel>&nbsp;
					</td>
				</tr>
				<tr>
					<td align="center" style="height:35px">
						<asp:Panel ID="PanPulsantiPannelli" Runat="server" >
						
						<br /><br />
						
      <table style="height:30px" align="left">
        <tr>
          <td width="16">&nbsp;</td>
          <td width="20">
<asp:imagebutton id="IBPan1_Chat" runat="server" ImageUrl="../images/Chat/Chat_Hot.gif"></asp:imagebutton></td>
          <td>&nbsp;</td>
          <td width="20">
<asp:imagebutton id="IBClear" accessKey="c" runat="server" ImageUrl="./../images/clear.gif" AlternateText="Clear"></asp:imagebutton></td>
          <td>&nbsp;</td>
          <td width="20">
<asp:imagebutton id="IBPan2_Stili" runat="server" ImageUrl="../images/Chat/Sty_HOT.gif"></asp:imagebutton></td>
          <td>&nbsp;</td>
          <td width="20">
<asp:imagebutton id="IBPan3_Smile" runat="server" ImageUrl="../images/Chat/Smi_Hot.gif"></asp:imagebutton></td>
          <td>&nbsp;</td>
          <td width="20">
<asp:imagebutton id="IBPan4_Tools" runat="server" ImageUrl="../images/Chat/Stru_HOT.gif"></asp:imagebutton></td>
          <td>&nbsp;</td>
          <td width="20">
<asp:imagebutton id="IBPan5_File" runat="server" ImageUrl="../images/Chat/File_Hot.gif"></asp:imagebutton></td>
          <td>&nbsp;</td>
          <td width="20">
<asp:imagebutton id="IBPan6_Utenti" runat="server" ImageUrl="../images/Chat/Ut_Hot.gif"></asp:imagebutton></td>
          <td>&nbsp;</td>
          <td width="20">
<asp:imagebutton id="IBPan7_Comunita" runat="server" ImageUrl="../images/Chat/Com_Hot.gif"></asp:imagebutton></td>
          <td>&nbsp;</td>
          <td width="20">
<asp:HyperLink id="HL_Help" Runat="server" Target="_blank" NavigateUrl="Chat_Help.aspx">
											<asp:Image ID="ImgLinkHelp" Runat="server" ImageUrl="../images/Chat/Hlp_HOT.gif"></asp:Image>
										</asp:HyperLink></td>
          <td>&nbsp;</td>
          <td width="20">
<asp:ImageButton id="IBExit" Runat="server" ImageUrl="../images/Chat/exit.gif"></asp:ImageButton></td>
          <td width="16">&nbsp;</td></tr></table>
						</asp:Panel>&nbsp;
					</td>
				</tr>
				<tr>
					<td align="center">
					
					
						<asp:panel id="Pan_Stili" Visible="false" Runat="server" Height="20">
<UC:ChatStili id="UC_ChatStili" runat="server"></UC:ChatStili>
						</asp:panel>
						<asp:panel id="Pan_Smile" Visible="false" Runat="server" Height="40">
<UC:ChatSmile id="UC_ChatSmile" runat="Server"></UC:ChatSmile>
						</asp:panel>
						<asp:panel id="Pan_Utility" Visible="false" Runat="server" Height="20">
      <table cellspacing="0" cellpadding="0" border="0">
        <tr>
          <td height="10">
<asp:label id="Lbl_Link_Help" Runat="server"></asp:label></td>
          <td>
<UC:ChatUtility id="UC_ChatUtility" Runat="server"></UC:ChatUtility></td></tr></table>
						</asp:panel>
						<asp:panel id="Pan_File" Visible="false" Runat="server" Height="260">
<UC:ChatFile id="UC_ChatFile" runat="Server"></UC:ChatFile>
						</asp:panel>
						<asp:panel id="Pan_Utenti" Visible="false" Runat="server">
<UC:ChatUtenti id="UC_ChatUtenti" runat="Server"></UC:ChatUtenti>
						</asp:panel>
						<asp:panel id="Pan_Comunita" Visible="false" Runat="server" Height="258" Width="450">
<UC:Chatcomunita id="UC_ChatComunita" runat="server"></UC:Chatcomunita>
						</asp:panel>
						
						
					</td>
				</tr>
				<tr>
					<td height="100%" width="100%" valign="top">
					
						<asp:panel id="Pan_Chat" Runat="server">
<asp:Label id="LblChatOut" Runat="server">
								<iframe id="Output" name="Output" marginwidth="0" marginheight="0" frameborder="1" width="99%"
									scrolling="yes" height="99%" runat="server" align="top" src="ChatOutput_Ext.aspx" style="height:100%;">
								</iframe>
							</asp:Label>
						</asp:panel>
						<asp:panel id="PNLpermessi" Width="179px" Visible="False" Runat="server">
      <table align="center">
      
        <tr>
          <td>&nbsp;</td></tr>
        <tr>
          <td align="center">
<asp:Label id="LBNopermessi" CssClass="messaggio" Runat="server"></asp:Label></td></tr>
        <tr>
          <td>&nbsp;</td></tr></table>
						</asp:panel>

					</td>
				</tr>
			</table>
			</asp:Panel>
			<asp:Panel ID="Pnl_NoService" runat="server" Visible="false">
			    <asp:Label ID="Lbl_NoService" runat="server">
			        Il servizio non è attualmente disponibile.<br />
			        Ci scusiamo per il disagio.
			    </asp:Label>
			</asp:Panel>
		</form>
	</body>
</html>
