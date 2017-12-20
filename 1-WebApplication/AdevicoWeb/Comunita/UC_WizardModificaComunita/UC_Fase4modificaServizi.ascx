<%@ Control Language="vb" AutoEventWireup="false" Codebehind="UC_Fase4modificaServizi.ascx.vb" Inherits="Comunita_OnLine.UC_Fase4ModificaServizi" TargetSchema="http://schemas.microsoft.com/intellisense/ie5" %>
<input id="HDN_ORGN_ID" type="hidden" name="HDN_ORGN_ID" runat="server"/>
<INPUT id="HDN_TipoComunitaID" type="hidden" name="HDN_TipoComunitaID" runat="server"/>
<INPUT id="HDN_PersonaID" type="hidden" name="HDN_PersonaID" runat="server"/>
<INPUT id="HDN_ResponsabileID" type="hidden" name="HDN_ResponsabileID" runat="server"/>
<input type=hidden id="HDNhasSetup" runat=server NAME="HDNhasSetup"/>
<input type=hidden id="HDNhasServizi" runat=server NAME="HDNhasServizi"/>
<input type=hidden id=HDNcmnt_ID runat=server NAME="HDNcmnt_ID"/>
<input type=hidden id="HDNserviziSelezionati" runat=server NAME="HDNserviziSelezionati"/>

<script language="javascript" type="text/javascript">
function SelectMe(Me){
		var HIDcheckbox,selezionati,LNBelimina;
		eval('HIDcheckbox= this.document.forms[0].<%=HDNserviziSelezionati.ClientID%>')
		selezionati = 0
		for(i=0;i< document.forms[0].length; i++){ 
			e=document.forms[0].elements[i];
			if ( e.type=='checkbox' && e.name.indexOf("CBXservizioAttivato") != -1 ){
				if (e.checked==true){
					selezionati++
					if (HIDcheckbox.value == ""){
						HIDcheckbox.value = ',' + e.value +','
					}	  
					else{
						pos1 = HIDcheckbox.value.indexOf(',' + e.value  +',')
						if (pos1==-1)
							HIDcheckbox.value = HIDcheckbox.value + e.value  +','
						}
				}
				else{
					valore = HIDcheckbox.value
					pos1 = HIDcheckbox.value.indexOf(',' + e.value  +',')
					if (pos1!=-1){
						stringa = ',' + e.value 
						HIDcheckbox.value = HIDcheckbox.value.substring(0,pos1)
						HIDcheckbox.value = HIDcheckbox.value + valore.substring(pos1+e.value.length+1,valore.length)
						}
				}
			}  
		}
		if (HIDcheckbox.value==",")
			HIDcheckbox.value = ""
			
	}
</script>

<asp:Panel ID="PNLservizi" Runat="server" Visible="true" HorizontalAlign=Center >								
	<asp:Table runat="server">
		<asp:TableRow ID="TBRutenteSelezionato" Visible=False >
			<asp:TableCell>
				<table border="1" align="center" width="800px" cellspacing=0 style="border-color=#cccccc; background-color:#fffbf7">
					<tr>
						<td>
							<asp:Table ID="TBLdatiPrincipali" Runat="server" Width="800px" CellPadding=0 CellSpacing=0 BorderStyle=none>
								<asp:TableRow>
									<asp:TableCell ColumnSpan=4>&nbsp;</asp:TableCell>
								</asp:TableRow>
								<asp:TableRow ID="TBRprofilo" BorderColor=White>
									<asp:TableCell>&nbsp;</asp:TableCell>
									<asp:TableCell>
										<table>
											<tr>
												<td>
													<asp:Label ID="LBsceltaServizio" Runat=server CssClass="Titolo_campoSmall"></asp:Label>
													<asp:RadioButtonList ID="RBLsceltaServizio" Runat=server RepeatLayout=Flow RepeatDirection=Horizontal CssClass="Testo_campoSmall" AutoPostBack=True>
														<asp:ListItem Value=0 Selected=True>Di sistema</asp:ListItem>	
														<asp:ListItem Value=1>Personale:</asp:ListItem>
													</asp:RadioButtonList>		
												</td>
												<td>
													<asp:DropDownList ID="DDLprofilo" Runat=server CssClass="Testo_campoSmall" AutoPostBack=True>
														
													</asp:DropDownList>
												</td>
											</tr>
										</table>
									</asp:TableCell>
									<asp:TableCell HorizontalAlign=Right>
										<asp:Button ID="BTNcambiaProfilo" Runat=server Text="Cambia profilo" CssClass=""></asp:Button>
										<asp:Button ID="BTNannullaModificheProfilo" Runat=server Text="Annulla" CssClass="" Visible=False ></asp:Button>
										<asp:Button ID="BTNsalvaModificheProfilo" Runat=server Text="Salva modifiche" CssClass="" Visible=False ></asp:Button>
									</asp:TableCell>
									<asp:TableCell>&nbsp;</asp:TableCell>
								</asp:TableRow>
								<asp:TableRow>
									<asp:TableCell ColumnSpan=4>&nbsp;</asp:TableCell>
								</asp:TableRow>
							</asp:Table>
						</td>
					</tr>
				</table>
			</asp:TableCell>
		</asp:TableRow>
	</asp:Table>
	<asp:datagrid 
	    id="DGServizi" Runat="server" 
	    ShowHeader="true" AllowSorting="true" 
		GridLines=Vertical AutoGenerateColumns="False" 
		DataKeyField="SRVZ_ID" AllowPaging="true" 
		PageSize=25 
		CssClass="DataGrid_Generica">
		<AlternatingItemStyle CssClass="ROW_Alternate_Small"></AlternatingItemStyle>
		<HeaderStyle CssClass="ROW_header_Small_Center"></HeaderStyle>
		<ItemStyle CssClass="ROW_Normal_Small" Height="22px"></ItemStyle>
		<PagerStyle CssClass="ROW_Page_Small" Position=TopAndBottom Mode="NumericPages" Visible="true" HorizontalAlign="Right" Height="18px" VerticalAlign="Bottom"></PagerStyle>
		<Columns>
			<asp:TemplateColumn headerStyle-CssClass=ROW_Header_Small_center>
				<ItemTemplate>
					<asp:Table ID="TBLdati" Runat=server>
						<asp:TableRow ID="TBRnome">
							<asp:TableCell>
								<asp:Label ID="LBnome" Runat=server><%# DataBinder.Eval(Container.DataItem, "SRVZ_nome") %></asp:Label>
							</asp:TableCell>
						</asp:TableRow>
						<asp:TableRow ID="TBRdescrizione">
							<asp:TableCell>
								<asp:Label ID="LBdescrizione" Runat=server><%# DataBinder.Eval(Container.DataItem, "SRVZ_Descrizione") %></asp:Label>
							</asp:TableCell>
						</asp:TableRow>
					</asp:Table>
				</ItemTemplate>
			</asp:TemplateColumn>
			<asp:TemplateColumn runat="server" HeaderText="Attivato" headerStyle-CssClass=ROW_Header_Small_center ItemStyle-CssClass=ROW_TD_Small_center ItemStyle-Width=100px >
				<ItemTemplate>
					<input runat=server  type="checkbox" id="CBXservizioAttivato" name="CBXservizioAttivato"  onclick="SelectMe(this);"/>
				</ItemTemplate>
			</asp:TemplateColumn>
			<asp:BoundColumn DataField="isNonDisattivabile" Visible=False ></asp:BoundColumn>
			<asp:BoundColumn DataField="isAbilitato" HeaderText="" Visible="False"></asp:BoundColumn>
			<asp:BoundColumn DataField="SRVZ_ID" HeaderText="" Visible="False"></asp:BoundColumn>
			<asp:BoundColumn DataField="isDefault" HeaderText="" Visible="False"></asp:BoundColumn>
		</Columns>
	</asp:datagrid>
</asp:Panel>