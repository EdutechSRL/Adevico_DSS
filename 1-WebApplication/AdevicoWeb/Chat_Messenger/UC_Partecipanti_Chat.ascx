<%@ Control Language="vb" AutoEventWireup="false" Codebehind="UC_Partecipanti_Chat.ascx.vb" Inherits="Comunita_OnLine.UC_Partecipanti_Chat" TargetSchema="http://schemas.microsoft.com/intellisense/ie5" %>
<TABLE cellSpacing="0" cellPadding="0" border="0">
	<TR>
		<TD colSpan="4">
			<asp:listbox id="LBUtenti" SelectionMode="Multiple" Font-Size="XX-Small" Width="360px" Height="88px"
				CssClass="Chat_Pulsante" runat="server"></asp:listbox></TD>
	</TR>
	<TR>
		<TD class="chat_chiaro" valign="middle" align="center">
			<asp:imagebutton id="IMBaggiorna" runat="server" AlternateText="Aggiorna" ImageUrl="./../Images/aggiorna.gif"></asp:imagebutton>
		</TD>
		<TD class="chat_chiaro" valign="middle" align="center">
			<asp:imagebutton id="IMBblocca" accessKey="l" runat="server" AlternateText="Blocca" ImageUrl="./../Images/lucchetto_closed.gif"></asp:imagebutton>
		</TD>
		<TD class="chat_chiaro" valign="middle" align="center">
			<asp:imagebutton id="IMBsblocca" accessKey="u" runat="server" AlternateText="Sblocca" ImageUrl="./../Images/lucchetto_open.gif"></asp:imagebutton>
		</TD>
		<TD class="chat_chiaro" valign="middle" align="center">
			<asp:Panel ID="Pan_UTAdmin" Runat="server" HorizontalAlign="Center">
<asp:Button id="BAMSetLvl" accessKey="s" runat="server" CssClass="Chat_Pulsante" Height="20"
					Font-Size="XX-Small" Text="Set Level"></asp:Button>&nbsp;&nbsp;&nbsp; 
<asp:DropDownList id="DDLLivello" runat="server" CssClass="Chat_Pulsante" Height="20" Width="80px"
					Font-Size="XX-Small">
					<asp:ListItem Value="0">OUT</asp:ListItem>
					<asp:ListItem Value="1" Selected="True">Reader</asp:ListItem>
					<asp:ListItem Value="2">Writer</asp:ListItem>
					<asp:ListItem Value="4">Moderator</asp:ListItem>
				</asp:DropDownList>
			</asp:Panel>
		</TD>
	</TR>
</TABLE>