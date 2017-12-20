<%@ Control Language="vb" AutoEventWireup="false" Codebehind="UC_ChatFile.ascx.vb" Inherits="Comunita_OnLine.UC_ChatFile" TargetSchema="http://schemas.microsoft.com/intellisense/ie5" %>
<table id="tableFileMan" cellSpacing="0" cellPadding="0" width="200" border="0" align="center">
	<tr>
		<td class="chat_scuro" colSpan="2" width="360">
			<asp:label id="Label1" runat="server" CssClass="chat_scuro" Width="85px" Height="13px" Font-Size="XX-Small"
				ForeColor="Lavender">File disponibili:</asp:label>
			<asp:label id="LblNumFile" runat="server" CssClass="chat_scuro" ForeColor="Lavender">0</asp:label>
		</td>
		<td class="chat_scuro" align="right"></td>
	</tr>
	<tr>
		<td class="chat_chiaro" align="center" colSpan="2" style="WIDTH: 217px">
			<asp:listbox id="LBxFile" runat="server" CssClass="chat_chiaro" Height="110px" SelectionMode="Multiple"
				Width="300px"></asp:listbox>
		</td>
		<td class="chat_scuro" align="center">
			<asp:button id="BtFileInfo" runat="server" CssClass="Chat_pulsante" Width="56px" Text="Info File"
				Font-Size="XX-Small"></asp:button><br/>
			<asp:button id="BtnDownload" runat="server" CssClass="Chat_pulsante" Width="56px" Text="Scarica"
				Font-Size="XX-Small"></asp:button><br/>
			<asp:button id="BtRemove" runat="server" CssClass="Chat_pulsante" Width="56px" Text="Rimuovi"
				Font-Size="XX-Small"></asp:button><br/>
			<asp:button id="BtSend" runat="server" CssClass="Chat_pulsante" Width="56px" Text="Invia File"
				Font-Size="XX-Small"></asp:button><br/>
			<asp:HyperLink ID="HL_Help" Runat="server" NavigateUrl="Chat_FileHelp.aspx" Target="_blank" BackColor="#8399b1"
				Font-Size="x-small">Aiuto</asp:HyperLink>
		</td>
	</tr>
	<tr>
		<td colSpan="3">
			<P>
				<input class="Chat_Pulsante" style="FONT-SIZE: xx-small" type="file" size="30" name="InputFileName"
					id="InputFileName" runat="server"/></P>
		</td>
	</tr>
</table>
<table id="tableFileManStat" cellSpacing="0" cellPadding="2" width="200" border="0" align="center">
	<tr>
		<td class="chat_scuro" vAlign="middle" align="center">
			<asp:image id="ImgOnOff" runat="server" AlternateText="Upload disponibile" ImageUrl="./../images/ON.gif"></asp:image>
		</td>
		<td class="chat_scuro" vAlign="middle" align="center" style="WIDTH: 225px">
			<asp:label id="Lab_State" runat="server" CssClass="chat_scuro" Height="17px" Width="148px"
				ForeColor="Lavender" Font-Size="XX-Small"></asp:label>
		</td>
		<td class="chat_scuro" style="WIDTH: 28px" align="center">
			<asp:imagebutton id="IBAggiorna" runat="server" Height="20px" AlternateText="Aggiorna" ImageUrl="./../Images/aggiorna.gif"></asp:imagebutton>
		</td>
	</tr>
	<tr>
		<td colspan="3" style="WIDTH: 296px; HEIGHT: 101px" class="chat_chiaro">
			<asp:Label id="LblFileInfo1" runat="server" Width="352px" Height="12px" CssClass="chat_chiaro"
				Font-Size="XX-Small" Font-Bold="true">Attenzione!!!</asp:Label>
			<asp:Label id="LblFileInfo2" runat="server" Width="354px" Height="56px" CssClass="chat_chiaro"
				Font-Size="XX-Small">	
					<br/>Non ci assumiamo alcuna responsabilità sui contenuti dei file. Non sono consentiti file che violino le leggi vigenti. Si consiglia comunque di eseguire la scansione con un antivirus aggiornato prima di aprire i file scaricati. Fare riferimento all'Help per ulteriori informazioni.
			</asp:Label>
		</td>
	</tr>
</table>
