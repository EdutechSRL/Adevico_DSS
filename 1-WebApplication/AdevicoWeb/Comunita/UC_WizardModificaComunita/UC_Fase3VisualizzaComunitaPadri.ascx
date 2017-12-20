<%@ Control Language="vb" AutoEventWireup="false" Codebehind="UC_Fase3VisualizzaComunitaPadri.ascx.vb" Inherits="Comunita_OnLine.UC_Fase3VisualizzaComunitaPadri" TargetSchema="http://schemas.microsoft.com/intellisense/ie5" %>
<input id="HDN_ComunitaPadriElenco" type=hidden runat=server NAME="HDN_ComunitaPadriElenco"/>
<INPUT id="HDN_ComunitaAttualePadreID" type="hidden" name="HDN_ComunitaAttualePadreID" runat="server"/>
<INPUT id="HDN_ComunitaAttualeID" type="hidden" name="HDN_ComunitaAttualeID" runat="server"/>
<INPUT id="HDN_ComunitaAttualePercorso" type="hidden" name="HDN_ComunitaAttualePercorso" runat="server"/>
<INPUT id="HDN_Livello" type="hidden" name="HDN_Livello" runat="server"/>
<input id="HDNhasSetup" type=hidden runat=server NAME="HDNhasSetup"/>
<input id="HDN_OrganizzazioneID" type="hidden" name="HDN_OrganizzazioneID" runat="server"/>

<asp:Table ID="TBLelencoComunita" Runat=server HorizontalAlign=Center GridLines=none width="800px">
	<asp:TableRow>
		<asp:TableCell>
			<asp:Label ID="LBinfoComunitaPadri" Runat="server">La comunità attuale è accessibile dalle seguenti comunità:</asp:Label>
		</asp:TableCell>
	</asp:TableRow>
	<asp:TableRow>
		<asp:TableCell HorizontalAlign=Center>
			<asp:datagrid 
			    id="DGComunitaAssociate" 
			    runat="server" 
			    DataKeyField="CMNT_id" 
			    AllowPaging="true"
				AutoGenerateColumns="False" 
				ShowFooter="false"
				PageSize="20" AllowSorting="true"
				CssClass="DataGrid_Generica">
				<AlternatingItemStyle CssClass="ROW_Alternate_Small"></AlternatingItemStyle>
				<HeaderStyle CssClass="ROW_header_Small_Center"></HeaderStyle>
				<ItemStyle CssClass="ROW_Normal_Small" Height="22px"></ItemStyle>
				<PagerStyle CssClass="ROW_Page_Small" Position=Bottom Mode="NumericPages" Visible="true"
				HorizontalAlign="Right" Height="18px" VerticalAlign="Bottom" Width="700px"></PagerStyle>
				<Columns>
					<asp:TemplateColumn runat="server" HeaderText="Tipo" ItemStyle-Width="40" ItemStyle-CssClass=ROW_TD_Small_center HeaderStyle-CssClass="ROW_header_Small_Center" >
						<ItemTemplate>
							<img runat=server src='<%# DataBinder.Eval(Container.DataItem, "TPCM_icona") %>' alt='<%# DataBinder.Eval(Container.DataItem, "TPCM_descrizione") %>' ID="Img1"/>
						</ItemTemplate>
					</asp:TemplateColumn>
					<asp:TemplateColumn HeaderText="Nome" SortExpression="CMNT_nome" ItemStyle-CssClass=ROW_TD_Small>
						<ItemTemplate >
							<table>
								<tr>
									<td Class=ROW_TD_Small>&nbsp;</td>
									<td Class=ROW_TD_Small>
										<asp:Label id="LBcomunita" Runat="server"><%# DataBinder.Eval(Container.DataItem, "CMNT_Nome") %></asp:Label>
									</td>
								</tr>
								<tr>
									<td Class=ROW_TD_Small>&nbsp;</td>
									<td Class=ROW_TD_Small>
										<asp:Label id="LBresponsabile" Runat="server"><%# DataBinder.Eval(Container.DataItem, "Responsabile") %></asp:Label>
									</td>
								</tr>
							</table>
						</ItemTemplate>
					</asp:TemplateColumn>
					<asp:BoundColumn DataField="CMNT_status" HeaderText="Status" Visible="true" SortExpression="CMNT_status" ItemStyle-CssClass=ROW_TD_Small_Center ItemStyle-Width=100px></asp:BoundColumn>
					<asp:TemplateColumn ItemStyle-CssClass=ROW_TD_Small_center >
						<ItemTemplate>
							<asp:Button ID="BTNremove" Runat="server" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "CMNT_ID") %>' CommandName="remove" CausesValidation=False CssClass="Pulsante_AzioneGriglia" Visible=False Text="Rimuovi"></asp:Button>
						</ItemTemplate>
					</asp:TemplateColumn>
					<asp:BoundColumn DataField="CMNT_id" Visible="false" ItemStyle-CssClass=ROW_TD_Small></asp:BoundColumn>
					<asp:BoundColumn DataField="CMNT_Bloccata" Visible="false" ItemStyle-CssClass=ROW_TD_Small></asp:BoundColumn>
					<asp:BoundColumn DataField="CMNT_Archiviata" Visible="false" ItemStyle-CssClass=ROW_TD_Small></asp:BoundColumn>
				</Columns>
			</asp:datagrid>
			<asp:Label Runat="server" CssClass="avviso_normal" ID="LBnoRecordAssociate" Visible="False">Questa comunità non appartiene a nessun'altra comunità presente nel sistema</asp:Label>
		</asp:TableCell>
	</asp:TableRow>
</asp:Table>