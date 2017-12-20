<%@ Control Language="vb" AutoEventWireup="false" Codebehind="UC_ProfiloServizi.ascx.vb" Inherits="Comunita_OnLine.UC_ProfiloServizi" TargetSchema="http://schemas.microsoft.com/intellisense/ie5" %>
<input type=hidden id="HDN_profiloID" runat=server NAME="HDN_profiloID"/>
<input type=hidden id="HDN_TPCM_ID" runat=server NAME="HDN_TPCM_ID"/>
<input type=hidden id="HDN_checkbox" runat=server NAME="HDN_checkbox"/>
<input type=hidden id="HDN_isDefinito" runat=server NAME="HDN_isDefinito"/>
<script language="javascript" type="text/javascript">	
	function SelectMe(Me){
		var HDN_checkbox,selezionati,LNBelimina;
		eval('HDN_checkbox= this.document.forms[0].<%=HDN_checkbox.ClientID%>')
		selezionati = 0
		for(i=0;i< document.forms[0].length; i++){ 
			e=document.forms[0].elements[i];
			if ( e.type=='checkbox' && e.name.indexOf("CBXservizioAttivato") != -1 ){
				if (e.checked==true){
					selezionati++
					if (HDN_checkbox.value == ""){
						HDN_checkbox.value = ',' + e.value +','
					}	  
					else{
						pos1 = HDN_checkbox.value.indexOf(',' + e.value  +',')
						if (pos1==-1)
							HDN_checkbox.value = HDN_checkbox.value + e.value  +','
						}
				}
				else{
					valore = HDN_checkbox.value
					pos1 = HDN_checkbox.value.indexOf(',' + e.value  +',')
					if (pos1!=-1){
						stringa = ',' + e.value 
						HDN_checkbox.value = HDN_checkbox.value.substring(0,pos1)
						HDN_checkbox.value = HDN_checkbox.value + valore.substring(pos1+e.value.length+1,valore.length)
						}
				}
			}  
		}
		if (HDN_checkbox.value==",")
			HDN_checkbox.value = ""
			
	}
	</script>
		
<asp:Table ID="TBLservizio" Runat=server Width=800px GridLines=none HorizontalAlign=left>
	<asp:TableRow>
		<asp:TableCell ColumnSpan=2 Height=40px>&nbsp;</asp:TableCell>
	</asp:TableRow>
	<asp:TableRow>
		<asp:TableCell ColumnSpan=2>
			<asp:Label ID="LBinfoServizi" Runat=server>
				ATTENZIONE: alcuni servizi NON sono disattivabili per garantire il corretto funzionamento del sistema.
			</asp:Label>
		</asp:TableCell>
	</asp:TableRow>
	<asp:TableRow Visible=False >
		<asp:TableCell  CssClass="top" Width=150px Wrap=False >
			<asp:Label ID="LBserviziAttivi_t" Runat=server CssClass="Titolo_CampoSmall">Servizi:</asp:Label>
		</asp:TableCell>
	</asp:TableRow>
	<asp:TableRow>
		<asp:TableCell>
			<asp:datagrid 
			    id="DGServizi" 
			    Runat="server" 
			    ShowHeader="true" 
			    AllowSorting="true" 
				GridLines=Vertical 
				AutoGenerateColumns="False" 
				DataKeyField="SRVZ_ID" 
				AllowPaging="true" 
				PageSize=25 
				Width="600px"
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
					<asp:TemplateColumn runat="server" HeaderText="Default" headerStyle-CssClass=ROW_Header_Small_center ItemStyle-CssClass=ROW_TD_Small_center ItemStyle-Width=130px>
						<ItemTemplate>
							<input runat=server  type="checkbox" id="CBXservizioAttivato" name="CBXservizioAttivato"  onclick="SelectMe(this);">
						</ItemTemplate>
					</asp:TemplateColumn>
					<asp:BoundColumn DataField="SRVZ_nonDisattivabile" Visible=False ></asp:BoundColumn>
					<asp:BoundColumn DataField="SRVZ_Attivato" HeaderText="" Visible="False"></asp:BoundColumn>
					<asp:BoundColumn DataField="SRVZ_ID" HeaderText="" Visible="False"></asp:BoundColumn>
					<asp:BoundColumn DataField="isDefault" HeaderText="" Visible="False"></asp:BoundColumn>
				</Columns>
			</asp:datagrid>
			<asp:Label ID="LBnoRecord" Runat=server CssClass="avviso11"></asp:Label>
		</asp:TableCell>
	</asp:TableRow>
</asp:Table>