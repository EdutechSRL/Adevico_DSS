<%@ Control Language="vb" AutoEventWireup="false" Codebehind="UC_VisEmoLvl2.ascx.vb" Inherits="Comunita_OnLine.UC_VisEmoLvl2" TargetSchema="http://schemas.microsoft.com/intellisense/ie5" %>
<asp:Panel ID="PanEmoXML" Runat="server" Visible="False">
	<asp:datalist id="DLEmo" Font-Size="xx-small" Width="360px" CellSpacing="0" CellPadding="0" GridLines="Both"
		BorderStyle="Solid" RepeatColumns="16" BorderColor="#0000ff" BorderWidth="1" runat="server">
		<HeaderStyle CssClass="Emoticon_Header"></HeaderStyle>
		<ItemStyle CssClass="Emoticon_Normali" BorderWidth="0"></ItemStyle>
		<HeaderTemplate>
			&nbsp;&nbsp;Smile
		</HeaderTemplate>
		<ItemTemplate>
			<table border="0" cellpadding="0" cellspacing="1">
				<tr>
					<td width="25px" align="center">
					    <a href="javascript:Emot('<%# Container.DataItem("Short")%>')">
							<img src="./../../images/forum/smile/<%# Container.DataItem("Img")%>" border="0" alt="<%# Container.DataItem("Name")%> - <%# Container.DataItem("Short")%>"  align="middle" height=17px />
						</a>
					</td>
			</table>
		</ItemTemplate>
	</asp:datalist>
</asp:Panel>
<asp:Panel ID="PanEmo" Runat="server" Visible="True">
	<TABLE id="UC_ChatSmile_DLEmo" style="BORDER-RIGHT: blue 1px solid; BORDER-TOP: blue 1px solid; FONT-SIZE: xx-small; BORDER-LEFT: blue 1px solid; WIDTH: 360px; BORDER-BOTTOM: blue 1px solid; BORDER-COLLAPSE: collapse" cellSpacing="0" cellPadding="0" rules="all" border="1">
		<TR>
			<TD class="Emoticon_Header" colSpan="16">&nbsp;&nbsp;Smile
			</TD>
		</TR>
		<TR>
			<TD class="Emoticon_Normali">
				<TABLE cellSpacing="1" cellPadding="0" border="0">
					<TR>
						<TD align="center" width="25"><A href="javascript:Emot(':-)')"><IMG height="17" alt="smile - :-)" src="./../../images/forum/smile/smiley1.gif" align="middle"
									border="0"/> </A>
						</TD>
					</TR>
				</TABLE>
			</TD>
			<TD class="Emoticon_Normali">
				<TABLE cellSpacing="1" cellPadding="0" border="0">
					<TR>
						<TD align="center" width="25"><A href="javascript:Emot(':-O')"><IMG height="17" alt="Shocked - :-O" src="./../../images/forum/smile/smiley3.gif" align="middle"
									border="0"/> </A>
						</TD>
					</TR>
				</TABLE>
			</TD>
			<TD class="Emoticon_Normali">
				<TABLE cellSpacing="1" cellPadding="0" border="0">
					<TR>
						<TD align="center" width="25"><A href="javascript:Emot(';-)')"><IMG height="17" alt="Wink - ;-)" src="./../../images/forum/smile/smiley2.gif" align="middle"
									border="0"/> </A>
						</TD>
					</TR>
				</TABLE>
			</TD>
			<TD class="Emoticon_Normali">
				<TABLE cellSpacing="1" cellPadding="0" border="0">
					<TR>
						<TD align="center" width="25"><A href="javascript:Emot(':$')"><IMG height="17" alt="Embarassed - :$" src="./../../images/forum/smile/smiley9.gif" align="middle"
									border="0"/> </A>
						</TD>
					</TR>
				</TABLE>
			</TD>
			<TD class="Emoticon_Normali">
				<TABLE cellSpacing="1" cellPadding="0" border="0">
					<TR>
						<TD align="center" width="25"><A href="javascript:Emot(':(')"><IMG height="17" alt="Unhappy - :(" src="./../../images/forum/smile/smiley6.gif" align="middle"
									border="0"/> </A>
						</TD>
					</TR>
				</TABLE>
			</TD>
			<TD class="Emoticon_Normali">
				<TABLE cellSpacing="1" cellPadding="0" border="0">
					<TR>
						<TD align="center" width="25"><A href="javascript:Emot(':S')"><IMG height="17" alt="Confused - :S" src="./../../images/forum/smile/smiley5.gif" align="middle"
									border="0"/> </A>
						</TD>
					</TR>
				</TABLE>
			</TD>
			<TD class="Emoticon_Normali">
				<TABLE cellSpacing="1" cellPadding="0" border="0">
					<TR>
						<TD align="center" width="25"><A href="javascript:Emot(':@')"><IMG height="17" alt="Angry - :@" src="./../../images/forum/smile/smiley7.gif" align="middle"
									border="0"/> </A>
						</TD>
					</TR>
				</TABLE>
			</TD>
			<TD class="Emoticon_Normali">
				<TABLE cellSpacing="1" cellPadding="0" border="0">
					<TR>
						<TD align="center" width="25"><A href="javascript:Emot('(*)')"><IMG height="17" alt="Star - (*)" src="./../../images/forum/smile/smiley10.gif" align="middle"
									border="0"/> </A>
						</TD>
					</TR>
				</TABLE>
			</TD>
			<TD class="Emoticon_Normali">
				<TABLE cellSpacing="1" cellPadding="0" border="0">
					<TR>
						<TD align="center" width="25"><A href="javascript:Emot('(L)')"><IMG height="17" alt="Heart - (L)" src="./../../images/forum/smile/smiley27.gif" align="middle"
									border="0"/> </A>
						</TD>
					</TR>
				</TABLE>
			</TD>
			<TD class="Emoticon_Normali">
				<TABLE cellSpacing="1" cellPadding="0" border="0">
					<TR>
						<TD align="center" width="25"><A href="javascript:Emot('(U)')"><IMG height="17" alt="Brocken Heart - (U)" src="./../../images/forum/smile/smiley28.gif"
									align="Middle" border="0"/> </A>
						</TD>
					</TR>
				</TABLE>
			</TD>
			<TD class="Emoticon_Normali">
				<TABLE cellSpacing="1" cellPadding="0" border="0">
					<TR>
						<TD align="center" width="25"><A href="javascript:Emot('(Y)')"><IMG height="17" alt="Thumbs Up - (Y)" src="./../../images/forum/smile/smiley20.gif" align="middle"
									border="0"/> </A>
						</TD>
					</TR>
				</TABLE>
			</TD>
			<TD class="Emoticon_Normali">
				<TABLE cellSpacing="1" cellPadding="0" border="0">
					<TR>
						<TD align="center" width="25"><A href="javascript:Emot('(N)')"><IMG height="17" alt="Thumbs Down - (N)" src="./../../images/forum/smile/smiley21.gif" align="middle"
									border="0"/> </A>
						</TD>
					</TR>
				</TABLE>
			</TD>
			<TD class="Emoticon_Normali">
				<TABLE cellSpacing="1" cellPadding="0" border="0">
					<TR>
						<TD align="center" width="25"><A href="javascript:Emot('(pp)')"><IMG height="17" alt="Clap - (pp)" src="./../../images/forum/smile/smiley32.gif" align="middle"
									border="0"/> </A>
						</TD>
					</TR>
				</TABLE>
			</TD>
			<TD class="Emoticon_Normali">
				<TABLE cellSpacing="1" cellPadding="0" border="0">
					<TR>
						<TD align="center" width="25"><A href="javascript:Emot('8-)')"><IMG height="17" alt="Geek - 8-)" src="./../../images/forum/smile/smiley23.gif" align="middle"
									border="0"/> </A>
						</TD>
					</TR>
				</TABLE>
			</TD>
			<TD class="Emoticon_Normali">
				<TABLE cellSpacing="1" cellPadding="0" border="0">
					<TR>
						<TD align="center" width="25"><A href="javascript:Emot('(6)')"><IMG height="17" alt="Evil Smile - (6)" src="./../../images/forum/smile/smiley15.gif" align="middle"
									border="0"/> </A>
						</TD>
					</TR>
				</TABLE>
			</TD>
			<TD class="Emoticon_Normali">
				<TABLE cellSpacing="1" cellPadding="0" border="0">
					<TR>
						<TD align="center" width="25"><A href="javascript:Emot('(?)')"><IMG height="17" alt="Question - (?)" src="./../../images/forum/smile/smiley25.gif" align="middle"
									border="0"/> </A>
						</TD>
					</TR>
				</TABLE>
			</TD>
		</TR>
		<TR>
			<TD class="Emoticon_Normali">
				<TABLE cellSpacing="1" cellPadding="0" border="0">
					<TR>
						<TD align="center" width="25"><A href="javascript:Emot(':D')"><IMG height="17" alt="Big Smile - :D" src="./../../images/forum/smile/smiley4.gif" align="middle"
									border="0"/> </A>
						</TD>
					</TR>
				</TABLE>
			</TD>
			<TD class="Emoticon_Normali">
				<TABLE cellSpacing="1" cellPadding="0" border="0">
					<TR>
						<TD align="center" width="25"><A href="javascript:Emot(':-p')"><IMG height="17" alt="Tongue - :-p" src="./../../images/forum/smile/smiley17.gif" align="middle"
									border="0"/> </A>
						</TD>
					</TR>
				</TABLE>
			</TD>
			<TD class="Emoticon_Normali">
				<TABLE cellSpacing="1" cellPadding="0" border="0">
					<TR>
						<TD align="center" width="25"><A href="javascript:Emot('(H)')"><IMG height="17" alt="Cool - (H)" src="./../../images/forum/smile/smiley16.gif" align="middle"
									border="0"/> </A>
						</TD>
					</TR>
				</TABLE>
			</TD>
			<TD class="Emoticon_Normali">
				<TABLE cellSpacing="1" cellPadding="0" border="0">
					<TR>
						<TD align="center" width="25"><A href="javascript:Emot('|-)')"><IMG height="17" alt="Sleepy - |-)" src="./../../images/forum/smile/smiley12.gif" align="middle"
									border="0"/> </A>
						</TD>
					</TR>
				</TABLE>
			</TD>
			<TD class="Emoticon_Normali">
				<TABLE cellSpacing="1" cellPadding="0" border="0">
					<TR>
						<TD align="center" width="25"><A href="javascript:Emot(';-(')"><IMG height="17" alt="Cry - ;-(" src="./../../images/forum/smile/smiley19.gif" align="middle"
									border="0"/> </A>
						</TD>
					</TR>
				</TABLE>
			</TD>
			<TD class="Emoticon_Normali" colSpan="11">&nbsp;</TD>
		</TR>
	</TABLE>
</asp:Panel>