<%@ Control Language="vb" AutoEventWireup="false" Codebehind="UC_Fase4DefinizioneServizi.ascx.vb" Inherits="Comunita_OnLine.UC_Fase4DefinizioneServizi" TargetSchema="http://schemas.microsoft.com/intellisense/ie5" %>

<input id="HDN_ORGN_ID" type="hidden" name="HDN_ORGN_ID" runat="server"/>
<INPUT id="HDN_TipoComunitaID" type="hidden" name="HDN_TipoComunitaID" runat="server"/>
<INPUT id="HDN_PersonaID" type="hidden" name="HDN_PersonaID" runat="server"/>
<input type=hidden id="HDNhasSetup" runat=server NAME="HDNhasSetup"/>
<input type=hidden id="HDNhasServizi" runat=server NAME="HDNhasServizi"/>
<input type=hidden id=HDNcmnt_ID runat=server NAME="HDNcmnt_ID"/>
<input type=hidden id="HDNserviziSelezionati" runat=server NAME="HDNserviziSelezionati"/>
<input type=hidden id="HDN_ResponsabileID" runat=server NAME="HDN_ResponsabileID"/>

<script language="javascript" type="text/javascript">
 function SelectFromNameAndAssocia(Nome,value){
		var HDNserviziSelezionati;
		eval('HDNserviziSelezionati= this.document.forms[0].<%=Me.HDNserviziSelezionati.clientID%>');
   		for(i=0;i< document.forms[0].length; i++){ 
			e=document.forms[0].elements[i];
			if ( e.type=='checkbox' && e.name ==Nome.name) {//"CBXassocia"
				if (e.checked==true){
					if (HDNserviziSelezionati.value == "")
						HDNserviziSelezionati.value = ',' + value +','
					else{
						pos1 = HDNserviziSelezionati.value.indexOf(',' + value+',')
						if (pos1==-1)
						HDNserviziSelezionati.value = HDNserviziSelezionati.value + value +','
						}
					}
				else{
					valore = HDNserviziSelezionati.value;
					pos1 = HDNserviziSelezionati.value.indexOf(',' + value+',')
					if (pos1!=-1){
						stringa = ',' + value+','
						HDNserviziSelezionati.value = HDNserviziSelezionati.value.substring(0,pos1)
						HDNserviziSelezionati.value = HDNserviziSelezionati.value + valore.substring(pos1+value.length+1,valore.length)
						}
					}
				}
		}

		if (HDNserviziSelezionati.value==",")
			HDNserviziSelezionati.value = "";
	}
</script>

<asp:Table ID="TBLdefinizioni" Runat=server HorizontalAlign=Center GridLines=none width="800px">
	<asp:TableRow>
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
	</asp:TableRow>
	<asp:TableRow>
		<asp:TableCell>
			<asp:CheckBoxList id="CBLservizi" Runat=server RepeatColumns=2 RepeatDirection=Vertical RepeatLayout="Table" CssClass="Testo_campoSmall">
			
			</asp:CheckBoxList>
		
			<table cellpadding=0 cellspacing=0 border=0>
				<asp:Repeater id="RPTservizio" Runat="server">
					<HeaderTemplate>
						<tr>
							<td align=center class="Header_Repeater" width=100px>
								<asp:Label ID="LBattiva_t" Runat=server CssClass="titolo_campoSmall">Attiva</asp:Label>
							</td>
							<td class="Header_Repeater" width=200px align=left nowrap="nowrap" >
								<asp:Label ID="LBservizio_t" Runat=server CssClass="titolo_campoSmall">Servizio:&nbsp;&nbsp;</asp:Label>
							</td>
						</tr>
					</HeaderTemplate>
					<ItemTemplate>
						<tr>
							<td align=center width=100px>
								<asp:CheckBox id="CBXservizioAttiva" cssClass="ROW_TD_Small" Runat="server"></asp:CheckBox>
								<asp:Label id="LBservizioID" Text='<%# Databinder.eval(Container.DataItem, "SRVZ_ID")%>' runat="server" Visible=false />
							</td>
							<td align=left width=200px nowrap="nowrap" >
								<asp:Label id="LBservizio" Text='<%# Databinder.eval(Container.DataItem, "SRVZ_Nome")%>' runat="server" Visible=true CssClass=ROW_TD_Small/>
							</td>						
						</tr>
					</ItemTemplate>
				</asp:Repeater>
			</table>
		</asp:TableCell>
	</asp:TableRow>
</asp:Table>