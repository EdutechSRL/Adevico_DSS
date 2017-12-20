<%@ Control Language="vb" AutoEventWireup="false" Codebehind="Legenda.ascx.vb" Inherits="Comunita_OnLine.Legenda" TargetSchema="http://schemas.microsoft.com/intellisense/ie5" %>
<table align=center border=1 style="border-color:#dedfdf" cellpadding=0 cellspacing=0>
	<tr>
		<td>
			<table width="100%" align=center bgcolor=#ffffff >
				<tr>
					<asp:DataList RepeatDirection="Vertical" RepeatColumns="7" Runat="server" ID="DTLlegenda" CellSpacing=2 CellPadding=0>
						<ItemTemplate>
							<td bgcolor="<%#databinder.eval(container.dataitem,"TPEV_icon")%>" width=10px height=10px>
								&nbsp;
							</td>
							<td bgcolor=#ffffff>
								<asp:Label Runat="server" Font-Name="Tahoma" Font-Size=8 ID="LBevento" Height=10px>
									&nbsp;&nbsp;<%#databinder.eval(container.dataitem,"TPEV_nome")%>&nbsp;&nbsp;
								</asp:Label>
							</td>
						</ItemTemplate>
					</asp:DataList>
				</tr>
			</table>
		</td>
	</tr>
</table>
