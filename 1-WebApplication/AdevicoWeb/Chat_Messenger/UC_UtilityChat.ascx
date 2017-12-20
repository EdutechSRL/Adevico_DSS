<%@ Control Language="vb" AutoEventWireup="false" Codebehind="UC_UtilityChat.ascx.vb" Inherits="Comunita_OnLine.UC_UtilityChat" TargetSchema="http://schemas.microsoft.com/intellisense/ie5" %>
<TABLE class="chat_chiaro" cellSpacing="0" cellPadding="0" width="360">
	<TR>
		<TD class="chat_chiaro" vAlign="middle" align="center" Height="20">
			<asp:Label ID="LblRef" Runat="server">Refresh</asp:Label>
			<asp:dropdownlist id="DDLTime" runat="server" Width="40px" CssClass="Chat_Pulsante" Font-Size="XX-Small"
				AutoPostBack="True">
				<asp:ListItem Value="5">5&quot;</asp:ListItem>
				<asp:ListItem Value="10" Selected="True">10&quot;</asp:ListItem>
				<asp:ListItem Value="15">15&quot;</asp:ListItem>
				<asp:ListItem Value="20">20&quot;</asp:ListItem>
				<asp:ListItem Value="30">30&quot;</asp:ListItem>
				<asp:ListItem Value="60">1'</asp:ListItem>
			</asp:dropdownlist>
		</TD>

		<td>
		</td>
		<td class="chat_chiaro" vAlign="middle" align="center" Height="20">
			<asp:Panel id="Pan_UtiAdm" runat="server">
				<TABLE class="chat_chiaro" style="height:20" cellSpacing="0" cellPadding="0" border="0">
					<TR>
						<TD class="chat_chiaro" vAlign="middle" align="center" height="20"><A onclick="window.open('./../Chat/ChatBuffer.aspx','ViewBuffer','width=450,height=200,menubar=yes,scrollbars=yes,resizable=yes')"
								href="javascript:;"><IMG id="IbViewAdm" height="20" alt="Visualizza il buffer" src="./../images/ViewServer.gif"
									width="20"/> </A>
						</TD>
						<TD>&nbsp;</TD>
						<TD class="chat_chiaro" vAlign="middle" align="center" height="20">
							<asp:ImageButton id="IBAClear" Width="20px" runat="server" AlternateText="Cancella buffer" ImageUrl="./../images/ClearServer.gif"
								Height="20"></asp:ImageButton></TD>
						<TD>&nbsp;</TD>
					</TR>
				</TABLE>
			</asp:Panel>
		</td>
	</TR>
</TABLE>
