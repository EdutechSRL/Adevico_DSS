<%@ Control Language="vb" AutoEventWireup="false" Codebehind="UC_RubricaMailGenerica.ascx.vb" Inherits="Comunita_OnLine.UC_RubricaMailGenerica" TargetSchema="http://schemas.microsoft.com/intellisense/ie5" %>
<%--OLD radtabstrip TELERIK--%>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>

<script language="javascript" type="text/javascript">
	function SelectMe(Me,setA,setCC,setCCN){
	 var HIDcheckbox;
	 //eval('HIDcheckbox= this.document.forms[0].<%=Me.HDazione.ClientID%>')
	 HIDcheckbox = this.document.getElementById('<%=Me.HDazione.ClientID%>');
		for(i=0;i< document.forms[0].length; i++){ 
			e=document.forms[0].elements[i];
			if ( e.type=='checkbox' && e.name.indexOf("CBazione") != -1 ){
				if (e.checked==true){
					if (HIDcheckbox.value == ""){
						HIDcheckbox.value = ',' + e.value+','
					}	  
					else{
						pos1 = HIDcheckbox.value.indexOf(',' + e.value+',')
						if (pos1==-1)
							HIDcheckbox.value = HIDcheckbox.value + e.value +','
						}
				}
				else{
					valore = HIDcheckbox.value
					pos1 = HIDcheckbox.value.indexOf(',' + e.value+',')
					if (pos1!=-1){
						stringa = ',' + e.value
						HIDcheckbox.value = HIDcheckbox.value.substring(0,pos1)
						HIDcheckbox.value = HIDcheckbox.value + valore.substring(pos1+e.value.length+1,valore.length)
						 }
				}
			}  
		}
		var HDazione_A,HDazione_CC,HDazione_CCN
		eval('HDazione_A= this.document.forms[0].<%=Me.HDazione_A.ClientID%>')
		eval('HDazione_CC= this.document.forms[0].<%=Me.HDazione_CC.ClientID%>')
		eval('HDazione_CCN= this.document.forms[0].<%=Me.HDazione_CCN.ClientID%>')
		if (HIDcheckbox.value==",")
			HIDcheckbox.value = ""
		if (setA =='True')
			HDazione_A.value =HIDcheckbox.value
		if (setCC =='True')
			HDazione_CC.value =HIDcheckbox.value
		if (setCCN =='True')
			HDazione_CCN.value =HIDcheckbox.value
	 }
	
			
			
		//Indica se è stato selezionato almeni un utente !!
		function UserSelezionati(){
			if (document.forms[0].HDazione.value == "," || document.forms[0].HDazione.value == "")
				return false;
			else
				return true;
			}
			

		</script>
<div class="content genericmail">
<asp:Table ID="TBLprincipale" Width="100%" Runat=server>
	<asp:TableRow ID="TBRselezioneComunita" Height=28px BackColor="#F2F2F2">
		<asp:TableCell>
			<asp:Label ID="LBselezionaComunita" Runat=server>Iscritti alla comunità:</asp:Label>
			<asp:RadioButtonList ID="RBLcomunita" Runat=server RepeatLayout=Flow AutoPostBack=True RepeatDirection=Horizontal >
				<asp:ListItem Value=0>corrente</asp:ListItem>
				<asp:ListItem Value=-1>corrente e relative sotto-comunità</asp:ListItem>
			</asp:RadioButtonList>
		</asp:TableCell>
	</asp:TableRow>
	<asp:TableRow ID="TBRselezioneDestinatari" Height=28px BackColor="#F2F2F2">
		<asp:TableCell>
			<asp:Label ID="LBselezionaDest" Runat=server>Destinatari:</asp:Label>
			<asp:RadioButtonList ID="RBLabilitazione" Runat=server RepeatLayout=Flow RepeatDirection=Horizontal AutoPostBack=True >
				<asp:ListItem Value=1>SOLO gli Abilitati</asp:ListItem>
				<asp:ListItem Value=5>SOLO i bloccati</asp:ListItem>
				<asp:ListItem Value=4>In Attesa di conferma</asp:ListItem>
				<asp:ListItem Value=0> Tutti</asp:ListItem>
			</asp:RadioButtonList>
			<br/>
		</asp:TableCell>
	</asp:TableRow>
	<asp:TableRow>
		<asp:TableCell>
             <telerik:radtabstrip id="TBSmenu" runat="server" align="Justify" Height="26px" SelectedIndex="0"
              causesvalidation="false" autopostback="true" skin="Outlook" enableembeddedskins="true">
                <tabs>
                    <telerik:RadTab text="Gruppi" value="Group" runat="server"></telerik:RadTab>
                    <telerik:RadTab text="Singoli" value="SingleUser" runat="server" ></telerik:RadTab>
                </tabs>
            </telerik:radtabstrip>
			<asp:panel id="PNLgruppi" Runat="server" BackColor="#F2F2F2" Width="100%">
                <asp:CheckBox id="CBXtutti" Runat="server" CssClass="smsStyle_Rubrica_sceltaPerGruppi" Text="Tutti"
								AutoPostBack="true"></asp:CheckBox>
				
				<asp:CheckBoxList id="CBLgruppi" Runat="server" CssClass="smsStyle_Rubrica_sceltaPerGruppi" RepeatLayout="table"
					Repeatdirection="Vertical"></asp:CheckBoxList>
			</asp:panel>
			<asp:panel id="PNLsingoli" Runat="server" Width="100%" Visible="False"  >
				<asp:panel id="PNLfiltri" Runat="server" >
                    <div class="filtercontainer container_12 clearfix">
                        <div class="filter grid_3">
                            <asp:label Runat="server" ID="LBtipoRuolo_t" CssClass="FiltroVoceSmall">Tipo Ruolo:</asp:label>
                            <asp:dropdownlist id="DDLTipoRuolo" runat="server" CssClass="FiltroCampoSmall" AutoPostBack="true"></asp:dropdownlist>
                        </div>
                        <div class="filter grid_3">
                            <asp:label Runat="server" ID="LBnumeroRecord_t" CssClass="FiltroVoceSmall">N° Record:</asp:label>
                            <asp:dropdownlist id="DDLNumeroRecord" CssClass="FiltroCampoSmall" Runat="server" AutoPostBack="true">
								<asp:ListItem Value="25" Selected="true"> 25</asp:ListItem>
								<asp:ListItem Value="40"> 40</asp:ListItem>
								<asp:ListItem Value="80"> 80</asp:ListItem>
								<asp:ListItem Value="100"> 100</asp:ListItem>
							</asp:dropdownlist>
                        </div>
                        <div class="filter grid_3">
                            <asp:Label ID="LBtipoRicerca_t" runat="server" CssClass="FiltroVoceSmall">Ricerca per:</asp:Label>
                            <asp:dropdownlist id="DDLTipoRicerca" CssClass="FiltroCampoSmall" Runat="server" AutoPostBack="false">
								<asp:ListItem Selected="true" Value="-2">Nome</asp:ListItem>
								<asp:ListItem Value="-3">Cognome</asp:ListItem>
								<asp:ListItem value="-4">Nome/Cognome</asp:ListItem>
							</asp:dropdownlist>
                        </div>
                        <div class="filter grid_3">
                            <asp:Label ID="LBvalore_t" runat="server" CssClass="FiltroVoceSmall">Valore:</asp:Label>
                            <asp:TextBox id="TXBvalore" Runat="server" MaxLength="300" CssClass="FiltroCampoSmall" Columns=30></asp:TextBox>
                        </div>
                    </div>
                    <div class="linewrapper filterletters clearfix">
                        <div class="left">
                            <asp:linkbutton id="LKBtutti" Runat="server" CssClass="lettera" CommandArgument="-1" OnClick="FiltroLink_Click">Tutti</asp:linkbutton>
                            <asp:linkbutton id="LKBaltro" Runat="server" CssClass="lettera" CommandArgument="0" OnClick="FiltroLink_Click">Altro</asp:linkbutton>
                            <asp:linkbutton id="LKBa" Runat="server" CssClass="lettera" CommandArgument="1" OnClick="FiltroLink_Click">A</asp:linkbutton>
                            <asp:linkbutton id="LKBb" Runat="server" CssClass="lettera" CommandArgument="2" OnClick="FiltroLink_Click">B</asp:linkbutton>
                            <asp:linkbutton id="LKBc" Runat="server" CssClass="lettera" CommandArgument="3" OnClick="FiltroLink_Click">C</asp:linkbutton>
                            <asp:linkbutton id="LKBd" Runat="server" CssClass="lettera" CommandArgument="4" OnClick="FiltroLink_Click">D</asp:linkbutton>
                            <asp:linkbutton id="LKBe" Runat="server" CssClass="lettera" CommandArgument="5" OnClick="FiltroLink_Click">E</asp:linkbutton>
                            <asp:linkbutton id="LKBf" Runat="server" CssClass="lettera" CommandArgument="6" OnClick="FiltroLink_Click">F</asp:linkbutton>
                            <asp:linkbutton id="LKBg" Runat="server" CssClass="lettera" CommandArgument="7" OnClick="FiltroLink_Click">G</asp:linkbutton>
                            <asp:linkbutton id="LKBh" Runat="server" CssClass="lettera" CommandArgument="8" OnClick="FiltroLink_Click">H</asp:linkbutton>
                            <asp:linkbutton id="LKBi" Runat="server" CssClass="lettera" CommandArgument="9" OnClick="FiltroLink_Click">I</asp:linkbutton>
                            <asp:linkbutton id="LKBj" Runat="server" CssClass="lettera" CommandArgument="10" OnClick="FiltroLink_Click">J</asp:linkbutton>
                            <asp:linkbutton id="LKBk" Runat="server" CssClass="lettera" CommandArgument="11" OnClick="FiltroLink_Click">K</asp:linkbutton>
                            <asp:linkbutton id="LKBl" Runat="server" CssClass="lettera" CommandArgument="12" OnClick="FiltroLink_Click">L</asp:linkbutton>
                            <asp:linkbutton id="LKBm" Runat="server" CssClass="lettera" CommandArgument="13" OnClick="FiltroLink_Click">M</asp:linkbutton>
                            <asp:linkbutton id="LKBn" Runat="server" CssClass="lettera" CommandArgument="14" OnClick="FiltroLink_Click">N</asp:linkbutton>
                            <asp:linkbutton id="LKBo" Runat="server" CssClass="lettera" CommandArgument="15" OnClick="FiltroLink_Click">O</asp:linkbutton>
                            <asp:linkbutton id="LKBp" Runat="server" CssClass="lettera" CommandArgument="16" OnClick="FiltroLink_Click">P</asp:linkbutton>
                            <asp:linkbutton id="LKBq" Runat="server" CssClass="lettera" CommandArgument="17" OnClick="FiltroLink_Click">Q</asp:linkbutton>
                            <asp:linkbutton id="LKBr" Runat="server" CssClass="lettera" CommandArgument="18" OnClick="FiltroLink_Click">R</asp:linkbutton>
                            <asp:linkbutton id="LKBs" Runat="server" CssClass="lettera" CommandArgument="19" OnClick="FiltroLink_Click">S</asp:linkbutton>
                            <asp:linkbutton id="LKBt" Runat="server" CssClass="lettera" CommandArgument="20" OnClick="FiltroLink_Click">T</asp:linkbutton>
                            <asp:linkbutton id="LKBu" Runat="server" CssClass="lettera" CommandArgument="21" OnClick="FiltroLink_Click">U</asp:linkbutton>
                            <asp:linkbutton id="LKBv" Runat="server" CssClass="lettera" CommandArgument="22" OnClick="FiltroLink_Click">V</asp:linkbutton>
                            <asp:linkbutton id="LKBw" Runat="server" CssClass="lettera" CommandArgument="23" OnClick="FiltroLink_Click">W</asp:linkbutton>
                            <asp:linkbutton id="LKBx" Runat="server" CssClass="lettera" CommandArgument="24" OnClick="FiltroLink_Click">X</asp:linkbutton>
                            <asp:linkbutton id="LKBy" Runat="server" CssClass="lettera" CommandArgument="25" OnClick="FiltroLink_Click">Y</asp:linkbutton>
                            <asp:linkbutton id="LKBz" Runat="server" CssClass="lettera" CommandArgument="26" OnClick="FiltroLink_Click">Z</asp:linkbutton>
                        </div>
                        <div class="right">
                            <asp:Button id="BTNcerca" Runat="server" Text="Cerca" CssClass="Pulsante"></asp:Button>
                        </div>
                    </div>
					<table cellSpacing="0" cellPadding="0" width="100%" border="0" bgColor="#F2F2F2">
						<tr>
							<td vAlign="top" align="center">
								
							</td>
						</tr>
						<tr>
							<td >
								<asp:DataList RepeatDirection=Horizontal RepeatColumns=3 ID="DTLiscritti" Runat=server >
									<HeaderTemplate>
										
									</HeaderTemplate>				
									<ItemTemplate>
									
										<table class="table minimal fullwidth userlists">
											<tr>
												<td width=30px>
													<span class="RubricaGenerale_DatiUtente">
													<input type="checkbox" value=<%# DataBinder.Eval(Container.DataItem, "PRSN_ID") %> id="CBazione" name="CBazione" <%# DataBinder.Eval(Container.DataItem, "oCheck") %> onclick="SelectMe(this,'<%=me.setA_Address%>','<%=me.setCC_Address%>','<%=me.setCCN_Address%>');" <%# DataBinder.Eval(Container.DataItem, "oCheckDisabled") %> >
													</span>
												</td>
												<td width=200px>
													<span class="RubricaGenerale_DatiUtenteBold"><%# DataBinder.Eval(Container.DataItem, "PRSN_Cognome") %></span>
													<span class="RubricaGenerale_DatiUtente">&nbsp;<%# DataBinder.Eval(Container.DataItem, "PRSN_Nome") %></span>
												</td>
											</tr>
										</table>
									</ItemTemplate>
								</asp:DataList>
								<asp:datagrid 
								    id="DGiscritti" 
								    runat="server" 
								    Width="100%" 
								    AllowSorting="true" 
								    ShowFooter="false" 
								    AutoGenerateColumns="False" 
								    AllowPaging="true" 
									DataKeyField="RLPC_ID" 
									PageSize="25" 
									AllowCustomPaging="True" 
									GridLines="None"
									>
									<PagerStyle  Position="Bottom" Mode="NumericPages" Visible="true"
										HorizontalAlign="left" Height="20px" VerticalAlign="Bottom"></PagerStyle>
									<Columns>
										<asp:BoundColumn DataField="RLPC_ID" HeaderText="RLPC" Visible="false"></asp:BoundColumn>								
									</Columns>
									<PagerStyle Width="100%" PageButtonCount="5" mode="NumericPages"></PagerStyle>
								</asp:datagrid>
								<br/>
								<asp:Label id="LBnoIscritti" Visible="False" Runat="server" CssClass="avviso"></asp:Label>
							</td>
						</tr>
					</table>
				</asp:panel>
			</asp:Panel>
		</asp:TableCell>
	</asp:TableRow>
	<asp:TableRow>
		<asp:TableCell ColumnSpan=2>&nbsp;</asp:TableCell>
	</asp:TableRow>
</asp:Table>
</div>

<INPUT id="HDNgruppi_A" type="hidden" name="HDNgruppi_A" runat="server"/>
<INPUT id="HDNgruppiNome_A" type="hidden" name="HDNgruppiNome_A" runat="server"/>
<INPUT id="HDNgruppiTutti_A" type="hidden" name="HDNgruppiTutti_A" runat="server"/>
<INPUT id="HDN_RBLcomunita_A" type="hidden" name="HDN_RBLcomunita_A" runat="server"/>
<INPUT id="HDN_RBLabilitazioneA" type="hidden" name="HDN_RBLabilitazioneA" runat="server"/>

<INPUT id="HDNgruppi_CC" type="hidden" name="HDNgruppi_CC" runat="server"/>
<INPUT id="HDNgruppiNome_CC" type="hidden" name="HDNgruppiNome_CC" runat="server"/>
<INPUT id="HDNgruppiTutti_CC" type="hidden" name="HDNgruppiTutti_CC" runat="server"/>
<INPUT id="HDN_RBLcomunita_CC" type="hidden" name="HDN_RBLcomunita_CC" runat="server"/>
<INPUT id="HDN_RBLabilitazioneCC" type="hidden" name="HDN_RBLabilitazioneCC" runat="server"/>

<INPUT id="HDNgruppi_CCN" type="hidden" name="HDNgruppi_CCN" runat="server"/>
<INPUT id="HDNgruppiNome_CCN" type="hidden" name="HDNgruppiNome_CCN" runat="server"/>
<INPUT id="HDNgruppiTutti_CCN" type="hidden" name="HDNgruppiTutti_CCN" runat="server"/>
<INPUT id="HDN_RBLcomunita_CCN" type="hidden" name="HDN_RBLcomunita_CCN" runat="server"/>
<INPUT id="HDN_RBLabilitazioneCCN" type="hidden" name="HDN_RBLabilitazioneCCN" runat="server"/>

<INPUT id="HDN_Path" type="hidden" name="HDN_Path" runat="server"/>
<INPUT id="HDN_CMNT_ID" type="hidden" name="HDN_CMNT_ID" runat="server"/>
<INPUT id="HDN_isLimbo" type="hidden" name="HDN_isLimbo" runat="server"/>
<INPUT id="HDN_setA" type="hidden" name="HDN_setA" runat="server"/>
<INPUT id="HDN_setCCN" type="hidden" name="HDN_setCCN" runat="server"/>
<INPUT id="HDN_setCC" type="hidden" name="HDN_setCC" runat="server"/>

<INPUT id="HDazione" type="hidden" name="HDN_setA" runat="server"/>
<INPUT id="HDazione_A" type="hidden" name="HDazione_A" runat="server"/>
<INPUT id="HDazione_CC" type="hidden" name="HDazione_CC" runat="server"/>
<INPUT id="HDazione_CCN" type="hidden" name="HDazione_CCN" runat="server"/>
