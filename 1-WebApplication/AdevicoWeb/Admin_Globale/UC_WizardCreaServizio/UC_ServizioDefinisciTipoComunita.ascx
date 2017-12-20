<%@ Control Language="vb" AutoEventWireup="false" Codebehind="UC_ServizioDefinisciTipoComunita.ascx.vb" Inherits="Comunita_OnLine.UC_ServizioDefinisciTipoComunita" TargetSchema="http://schemas.microsoft.com/intellisense/ie5" %>
<input type=hidden id="HDN_servizioID" runat=server NAME="HDN_servizioID"/>
<input type=hidden id="HDN_definito" runat=server NAME="HDN_definito"/>
<input type=hidden id="HDNhasSetupGenerali" runat=server NAME="HDNhasSetupGenerali"/>
<input type=hidden id="HDNhasSetupMultiplo" runat=server NAME="HDNhasSetupMultiplo"/>

<asp:Table HorizontalAlign=Center Runat=server ID="TBLtipoComunita" Width=750px Visible=true GridLines=none >
	<asp:TableRow>
		<asp:TableCell>
			<asp:Label ID="LBsceltaTipodefinizione_t" Runat="server">Definisci servizi</asp:Label>
			&nbsp;
			<asp:RadioButtonList ID="RBLsceltaTipodefinizione" RepeatDirection=Horizontal RepeatLayout=Flow Runat="server" AutoPostBack=True>
				<asp:ListItem Value=0 Selected=True >In generale</asp:ListItem>
				<asp:ListItem Value=1>Organizzazione per organizzazione</asp:ListItem>
			</asp:RadioButtonList>
		</asp:TableCell>
	</asp:TableRow>
	<asp:TableRow>
		<asp:TableCell ColumnSpan=2 CssClass="nosize0" Height=10px>&nbsp;</asp:TableCell>
	</asp:TableRow>
	<asp:TableRow>
		<asp:TableCell ColumnSpan=2>
			<div id="divDestinatario" style="position:absolute; border: solid 1px; width:800px; background: white; height: 400px; z-index: 1; overflow: auto;">
			<table cellpadding=0 cellspacing=0 border=0>
				
					<asp:Repeater id="RPTtipoComunitaMultiplo" Runat="server" >
						<HeaderTemplate>
							<tr>
								<td class="Header_Repeater" width=150px align=left nowrap="nowrap" >
									&nbsp;
								</td>
								<asp:Repeater id="RPTorganizzazioneHEADER" Runat="server">
									<ItemTemplate>
										<td nowrap="nowrap" class="<%# Databinder.eval(Container.DataItem, "CssClass")%>" align="center">
											<asp:Label id="LBorganizzazione_t" CssClass="titolo_campoSmall" Text='<%# Databinder.eval(Container.DataItem, "ORGN_Ragionesociale")%>' runat="server" Visible=true />
										</td>
									</ItemTemplate>
								</asp:Repeater>
							</tr>	
						</HeaderTemplate>
						<ItemTemplate>
						<tr>
							<td class="Header_Repeater" width=150px align=left nowrap="nowrap" >
								<asp:Label ID="LBtipoComunita_t" Runat=server CssClass="titolo_campoSmall" Text='<%# Databinder.eval(Container.DataItem, "TPCM_descrizione")%>'></asp:Label>
								<asp:Label id="LBtipoComunitaID" Text='<%# Databinder.eval(Container.DataItem, "TPCM_ID")%>' runat="server" Visible=false />
							</td>
								<asp:Repeater id="RPTorganizzazione" Runat="server">
									<ItemTemplate>
										<td nowrap="nowrap" class="<%# Databinder.eval(Container.DataItem, "CssClass")%>">
											<table border=0 align=center>
												<tr>
													<td align=right>
														<asp:CheckBox ID="CBXtipoComunitaAssociato" Runat=server Text="associa" CssClass="ROW_TD_Small" Checked=<%# DataBinder.Eval(Container.DataItem, "oCheckAssociato") %> ></asp:CheckBox>
													</td>
													<td>&nbsp;</td>
													<td align=left>
														<asp:Label id="LBorganizzazioneID" Text='<%# Databinder.eval(Container.DataItem, "ORGN_ID")%>' runat="server" Visible=false />
														<asp:CheckBox ID="CBXtipoComunitaAttiva" Runat=server Text="Attiva" CssClass="ROW_TD_Small" Checked=<%# DataBinder.Eval(Container.DataItem, "oCheckDefault") %> ></asp:CheckBox>
													</td>
												</tr>
											</table>
										</td>	
									</ItemTemplate>
								</asp:Repeater>	
							</tr>							
						</ItemTemplate>
					</asp:Repeater>
					<asp:Repeater id="RPTgenericoTipoComunita" Runat="server" Visible=False>
						<HeaderTemplate>
							<tr>
								<td class="Header_Repeater" width=150px align=left nowrap="nowrap" >
									<asp:Label ID="LBgenericotipoComunita_t" Runat=server CssClass="titolo_campoSmall">Tipo comunità:&nbsp;&nbsp;</asp:Label>
								</td>
								<td align=center class="Header_Repeater" width=70px>
									&nbsp;
								</td>
							</tr>
						</HeaderTemplate>
						<ItemTemplate>
							<tr>
								<td width=150px align=left nowrap="nowrap" >
									<asp:Label ID="LBgenericoTipoComunita" Runat=server CssClass="ROW_TD_Small" Text='<%# Databinder.eval(Container.DataItem, "TPCM_descrizione")%>'></asp:Label>
									<asp:Label id="LBgenericotipoComunitaID" Text='<%# Databinder.eval(Container.DataItem, "TPCM_ID")%>' runat="server" Visible=false />
								</td>
								<td width=250px>
									<asp:CheckBox ID="CBXgenericoTipoComunitaAssociato" Runat=server Text="Si" CssClass="ROW_TD_Small" Checked=<%# DataBinder.Eval(Container.DataItem, "oCheckAssociato") %> ></asp:CheckBox>
									&nbsp;&nbsp;
									<asp:CheckBox ID="CBXgenericoTipoComunitaAttiva" Runat=server Text="Si" CssClass="ROW_TD_Small" Checked=<%# DataBinder.Eval(Container.DataItem, "oCheckDefault") %> ></asp:CheckBox>
								</td>
							</tr>
						</ItemTemplate>
					</asp:Repeater>
			</table>
			</div>
		</asp:TableCell>
	</asp:TableRow>
</asp:Table>