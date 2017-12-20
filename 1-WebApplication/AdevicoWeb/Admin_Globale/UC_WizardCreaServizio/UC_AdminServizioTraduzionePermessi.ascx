<%@ Control Language="vb" AutoEventWireup="false" Codebehind="UC_AdminServizioTraduzionePermessi.ascx.vb" Inherits="Comunita_OnLine.UC_AdminServizioTraduzionePermessi" TargetSchema="http://schemas.microsoft.com/intellisense/ie5" %>
<input type=hidden id="HDN_servizioID" runat=server NAME="HDN_servizioID"/>
<input type=hidden id="HDN_associati" runat=server/>
<input type=hidden id="HDN_definito" runat=server NAME="HDN_definito"/>
<INPUT id="HDNhasSetup" type="hidden" runat="server" NAME="HDNhasSetup"/>
<asp:Table ID="TBLdati" Runat=server HorizontalAlign=Center Width=800px Visible=False >
	<asp:TableRow>
		<asp:TableCell ColumnSpan=2 Height=20px>&nbsp</asp:TableCell>
	</asp:TableRow>
	<asp:TableRow>
		<asp:TableCell ColumnSpan=2>
			
			<table border=0 align=left cellpadding=0 cellspacing=0>
				<asp:Repeater id="RPTpermessi" Runat="server">
					<ItemTemplate>
						<tr class="<%# Databinder.eval(Container.DataItem, "CssClass")%>">
							<td colspan=3 align=left class="<%# Databinder.eval(Container.DataItem, "CssClass")%>" height="22px">
								<asp:Label ID="LBpermesso" Runat=server CssClass="Titolo_CampoSmall"></asp:Label>
								<asp:Label ID="LBprms_ID" Runat=server Visible=False ></asp:Label>
								<asp:Label ID="LBAssociato" Runat=server Visible=False ><%# Databinder.eval(Container.DataItem, "Associato")%></asp:Label>
							</td>
						</tr>
						<tr class="<%# Databinder.eval(Container.DataItem, "CssClass")%>">
							<td class="<%# Databinder.eval(Container.DataItem, "CssClass")%>">&nbsp;</td>
							<td align=left class="<%# Databinder.eval(Container.DataItem, "CssClass")%>">
								<table cellpadding=0 cellspacing=0 border=0>
									<tr>
										<td>
											<table border=0 align=left bgcolor="#FFFBF7" cellpadding=0 cellspacing=0>
												<asp:Repeater id="RPTnome" Runat="server" >
													<HeaderTemplate>
														<tr>
															<td colspan=2 height=20px>
																<asp:Label ID="LBlinguaNome_t" Runat=server CssClass="Titolo_campoSmall">&nbsp;Nome:</asp:Label>
															</td>
														</tr>
													</HeaderTemplate>
													<ItemTemplate>
														<tr>
															<td align=right width=120px height=22px>
																<asp:Label id="LBlinguaID" Text='<%# Databinder.eval(Container.DataItem, "LNGU_ID")%>' runat="server" Visible=false />
																<asp:Label id="LBdefault" Text='<%# Databinder.eval(Container.DataItem, "LNGU_Default")%>' runat="server" Visible=false />
																<asp:Label id="LBlingua_Nome" Text='<%# Databinder.eval(Container.DataItem, "LNGU_nome")%>' runat="server" Visible=true CssClass=Repeater_VoceLingua/>&nbsp;
															</td>
															<td align=left height=22px>
																<asp:TextBox ID="TXBtermine" Runat="server" CssClass="Testo_campoSmall10" MaxLength="50" Columns="60" Text='<%# Databinder.eval(Container.DataItem, "Nome")%>'> </asp:TextBox>&nbsp;&nbsp;
															</td>
														</tr>
													</ItemTemplate>
													<FooterTemplate>
														<tr><td colspan=2 class=nosize0>&nbsp;</td></tr>
													</FooterTemplate>
												</asp:Repeater>
											</table>
										</td>
										<td>
											<table border=0 align=left bgcolor="#FFFBF7" cellpadding=0 cellspacing=0>
												<asp:Repeater id="RPTdescrizione" Runat="server">
													<HeaderTemplate>
														<tr>
															<td colspan=2 height=20px>
																<asp:Label ID="LBlinguaDescrizione_t" Runat=server CssClass="Titolo_campoSmall">Descrizione:</asp:Label>
															</td>
														</tr>
													</HeaderTemplate>
													<ItemTemplate>
														<tr>
															<td align=right width=120px height=22px>
																<asp:Label id="LBlingua2ID" Text='<%# Databinder.eval(Container.DataItem, "LNGU_ID")%>' runat="server" Visible=false />
																<asp:Label id="LBdefault2" Text='<%# Databinder.eval(Container.DataItem, "LNGU_Default")%>' runat="server" Visible=false />
																<asp:Label id="LBlingua2_Nome" Text='<%# Databinder.eval(Container.DataItem, "LNGU_nome")%>' runat="server" Visible=true CssClass=Repeater_VoceLingua/>&nbsp;
															</td>
															<td align=left height=22px>
																<asp:TextBox ID="TXBtermine2" Runat="server" CssClass="Testo_campoSmall10" Text='<%# Databinder.eval(Container.DataItem, "Descrizione")%>' MaxLength="200" Columns="80"> </asp:TextBox>&nbsp;&nbsp;
															</td>
														</tr>
													</ItemTemplate>
													<FooterTemplate>
														<tr><td colspan=2 class=nosize0>&nbsp;</td></tr>
													</FooterTemplate>
												</asp:Repeater>
											</table>
										</td>
									</tr>
								</table>
							</td>
							<td class="<%# Databinder.eval(Container.DataItem, "CssClass")%>">&nbsp;</td>
						</tr>
						<tr>
							<td class="<%# Databinder.eval(Container.DataItem, "CssClass")%>" colspan=3>&nbsp;</td>
						</tr>
					</ItemTemplate>
				</asp:Repeater>	
			</table>
		</asp:TableCell>
	</asp:TableRow>
</asp:Table>