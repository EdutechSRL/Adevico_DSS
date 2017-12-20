<%@ Control Language="vb" AutoEventWireup="false" Codebehind="UC_StiliChat.ascx.vb" Inherits="Comunita_OnLine.UC_StiliChat" TargetSchema="http://schemas.microsoft.com/intellisense/ie5" %>
<TABLE class="chat_chiaro" cellSpacing="1" cellPadding="0" width="360" style="height:20">
	<tr>
		<TD class="chat_chiaro" align="center" width="89">
			<asp:checkbox id="CBBold" runat="server" CssClass="Chat_Pulsante" Text="Bold" Font-Size="XX-Small"
				Font-Bold="True"></asp:checkbox>
		</TD>
		<td class="chat_chiaro" align="center" width="89">
			<asp:checkbox id="CBItalic" runat="server" CssClass="Chat_Pulsante" Text="Italic" Font-Size="XX-Small"
				Font-Italic="True"></asp:checkbox>
		</td>
		<TD class="chat_chiaro" align="center" width="89">
			<asp:checkbox id="CBUnder" runat="server" CssClass="Chat_Pulsante" Text="Underline" Font-Size="XX-Small"
				Font-Underline="True"></asp:checkbox>
		</TD>
		<TD class="chat_chiaro" align="center" width="89">
			<SELECT class="Chat_Pulsante" id="SelColor" style="FONT-SIZE: 10px; WIDTH: 80px; FONT-FAMILY: Arial"
				name="SelColor" runat="server">
				<OPTION style="COLOR: #000000" selected="selected">Black</OPTION>
				<OPTION style="COLOR: #ff0000">Red</OPTION>
				<OPTION style="COLOR: #00ff00">Green</OPTION>
				<OPTION style="COLOR: #0000ff">Blue</OPTION>
				<OPTION style="COLOR: #00ffff">Light Blue</OPTION>
				<OPTION style="COLOR: #ffff00">Yellow</OPTION>
				<OPTION style="COLOR: #ff00ff">Violet</OPTION>
				<OPTION style="COLOR: #505050">Dark gray</OPTION>
				<OPTION style="COLOR: #afafaf">Gray</OPTION>
				<OPTION style="COLOR: #af5050">Brown</OPTION>
				<OPTION style="COLOR: #50af50">Dark green</OPTION>
				<OPTION style="COLOR: #5050af">Dark blue</OPTION>
				<OPTION style="COLOR: #50afaf">Grey-blue</OPTION>
				<OPTION style="COLOR: #afaf50">Dark yellow</OPTION>
				<OPTION style="COLOR: #af50af">Dark violet</OPTION>
				<OPTION>White</OPTION>
			</SELECT>
		</TD>
	</tr>
</TABLE>
