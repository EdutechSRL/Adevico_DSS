<%@ Page Language="vb" AutoEventWireup="false" Codebehind="ChatFileManager_Ext.aspx.vb" Inherits="Comunita_OnLine.ChatFileManager_Ext" %>
<!DOCTYPE HTML PUBLIC "-//W3C//Dtd HTML 4.0 Transitional//EN">
<HTML>
	<head runat="server">
		<title>ChatFileManager</title>
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR"/>
		<meta content="Visual Basic .NET 7.1" name="CODE_LANGUAGE"/>
		<meta content="JavaScript" name="vs_defaultClientScript"/>
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema"/>
		<LINK href="./../Styles.css" type="text/css" rel="stylesheet"/>
	</HEAD>
	<body>
		<form id="aspnetForm" method="post" encType="multipart/form-data" runat="server">
		<asp:ScriptManager ID="SCMmanager" runat="server"></asp:ScriptManager>
			<table id="tableFileMan" cellSpacing="0" cellPadding="0" width="200" border="0" align="center"
				style="HEIGHT: 158px">
				<tr>
					<td class="chat_scuro" colSpan="2" style="WIDTH: 217px; HEIGHT: 12px">
						<asp:label id="Lab_FileDisp_t" runat="server" CssClass="chat_scuro" Width="85px" Height="13px"
							Font-Size="XX-Small" ForeColor="Lavender">File disponibili:</asp:label>
						<asp:label id="LblNumFile" runat="server" CssClass="chat_scuro" ForeColor="Lavender">0</asp:label>
					</td>
					<td class="chat_scuro" align="right" style="HEIGHT: 12px"></td>
				</tr>
				<tr>
					<td class="chat_chiaro" align="center" colSpan="2" style="WIDTH: 217px"><asp:listbox id="LBxFile" runat="server" CssClass="chat_chiaro" Height="110px" SelectionMode="Multiple"
							Width="138px"></asp:listbox></td>
					<td class="chat_scuro" align="center"><asp:button id="BtFileInfo" runat="server" CssClass="Chat_pulsante" Width="56px" Text="Info File"
							Font-Size="XX-Small"></asp:button><br/>
						<asp:button id="BtnDownload" runat="server" CssClass="Chat_pulsante" Width="56px" Text="Scarica"
							Font-Size="XX-Small"></asp:button><br/>
						<asp:button id="BtRemove" runat="server" CssClass="Chat_pulsante" Width="56px" Text="Rimuovi"
							Font-Size="XX-Small"></asp:button><br/>
						<asp:button id="BtHelp" runat="server" CssClass="Chat_pulsante" Width="56px" Text="Aiuto" Font-Size="XX-Small"></asp:button><br/>
						<asp:button id="BtSend" runat="server" CssClass="Chat_pulsante" Width="56px" Text="Invia File"
							Font-Size="XX-Small"></asp:button></td>
				</tr>
				<tr>
					<td colSpan="3">
						<P><input class="Chat_Pulsante" id="PostedFile" style="WIDTH: 200px; HEIGHT: 20px" type="file"
								size="23" name="InputFileName" runat="server"/></P>
					</td>
				</tr>
			</table>
			<table id="tableFileManStat" cellSpacing="0" cellPadding="2" width="200" border="0" align="center"
				style="HEIGHT: 136px">
				<tr>
					<td class="chat_scuro" vAlign="middle" align="center"><asp:image id="ImgOnOff" runat="server" AlternateText="Upload disponibile" ImageUrl="./../images/ON.gif"></asp:image></td>
					<td class="chat_scuro" vAlign="middle" align="center" style="WIDTH: 225px"><asp:label id="Lab_State" runat="server" CssClass="chat_scuro" Height="17px" Width="148px"
							ForeColor="Lavender" Font-Size="XX-Small"></asp:label></td>
					<td class="chat_scuro" style="WIDTH: 28px"><asp:imagebutton id="IBAggiorna" runat="server" Height="20px" AlternateText="Aggiorna" ImageUrl="./../Images/aggiorna.gif"></asp:imagebutton></td>
				</tr>
				<tr>
					<td colspan="3" style="WIDTH: 296px; HEIGHT: 101px" class="chat_chiaro">
						<p style="FONT-SIZE: xx-small">
							<asp:Label id="LblFileInfo1" runat="server" Width="192px" Height="12px" CssClass="chat_chiaro"
								Font-Size="XX-Small" Font-Bold="true">Attenzione!!!</asp:Label>
							<asp:Label id="LblFileInfo2" runat="server" Width="192px" Height="75px" CssClass="chat_chiaro"
								Font-Size="XX-Small">	
								<br/>Non ci assumiamo alcuna responsabilità sui contenuti dei file. Non sono consentiti file che violino le leggi vigenti. Si consiglia comunque di eseguire la scansione con un antivirus aggiornato prima di aprire i file scaricati. Fare riferimento all'Help per ulteriori informazioni.</asp:Label></p>
					</td>
				</tr>
			</table>
		</form>
	</body>
</HTML>
