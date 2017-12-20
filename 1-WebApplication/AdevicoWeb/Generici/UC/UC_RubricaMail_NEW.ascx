<%@ Control Language="vb" AutoEventWireup="false" Codebehind="UC_RubricaMail_NEW.ascx.vb" Inherits="Comunita_OnLine.UC_RubricaMail_NEW" TargetSchema="http://schemas.microsoft.com/intellisense/ie5" %>
<%--OLD radtabstrip TELERIK--%>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>

<script language="javascript" type="text/javascript">
	var iscr;
	iscr = new Array()
	
	function hasSelezionati(){
	
		if (document.forms[0].<%=Me.LBXelenco.ClientID%>.selectedIndex > -1)
			return true;
		else
			return false;
	}
	function selezionaFiltrato(obj){
		var valore,stringa,posStart,posEnd,posVal,telefono
		valore = obj.options[obj.selectedIndex].value;
		var LBXelenco
		eval('LBXelenco= this.document.forms[0].<%=Me.LBXelenco.ClientID%>')
		for(i=0;i<LBXelenco.options.length;i++){
			if (valore==-1){
				LBXelenco.options[i].selected = false}
			else{
				if (valore==0)
					{LBXelenco.options[i].selected = true}
				else{
					stringa = LBXelenco.options[i].value
					if(getTipo_Ruolo(stringa)==valore)
						LBXelenco.options[i].selected = true
					else
						LBXelenco.options[i].selected = false
				}
			}
		}
		return true;
	}
	
	
	function getrLPC_ID(stringa){
		posStart = stringa.indexOf(",")
		
		if (posStart < 0)
			RLPC_ID = stringa
		else{
			RLPC_ID = stringa.substring(0,posStart)}
				
		return RLPC_ID
	}
	
	function getIndice(stringa){
		posStart = stringa.indexOf(",")
		posMid = stringa.indexOf(",",posStart+1)
		posMid2 = stringa.indexOf(",",posMid+1)
		posEnd = stringa.indexOf(",",posMid2+1)
				
		Tipo_Ruolo = stringa.substring(posStart+1,posMid)
		cellulare = stringa.substring(posMid+1,posMid2)
		ricezione = stringa.substring(posMid2+1,posEnd)
		indice = stringa.substring(posEnd+1,stringa.length)
		
		return indice
	}
	
	function getTipo_Ruolo(stringa){
		posStart = stringa.indexOf(",")
		posMid = stringa.indexOf(",",posStart+1)	
		Tipo_Ruolo = stringa.substring(posStart+1,posMid)
		return Tipo_Ruolo
	}

	function getCellulare(stringa){
		posStart = stringa.indexOf(",")
		posMid = stringa.indexOf(",",posStart+1)
		posMid2 = stringa.indexOf(",",posMid+1)
		
		cellulare = stringa.substring(posMid+1,posMid2)
		return cellulare
	}

	function getriceviSMS(stringa){
		posStart = stringa.indexOf(",")
		posMid = stringa.indexOf(",",posStart+1)
		posMid2 = stringa.indexOf(",",posMid+1)
		posEnd = stringa.indexOf(",",posMid2+1)
						
		ricezione = stringa.substring(posMid2+1,posEnd)
		return ricezione
	}

	function findDestinatari(){
		var stringa;
		var LBXelenco,LBXsms;
		eval('LBXelenco= document.forms[0].<%=Me.LBXelenco.ClientID%>')
		//eval('LBXsms= document.forms[0].<%=Me.LSB_A.ClientID%>')
		
		for(i=0;i < LBXelenco.options.length;i++){
			stringa = LBXelenco.options[i].text.toLowerCase()		
			
			if (this.document.forms[0].TBsearch.value != "" ){
				posStart = stringa.indexOf(this.document.forms[0].TBsearch.value.toLowerCase())
				if (posStart >=0)
					LBXelenco.options[i].selected = true
				else
					LBXelenco.options[i].selected = false
				}
		}
		
		return false
	}	

</script>
<table cellSpacing="0" cellPadding="0" width="100%" border="0" bgcolor="<%=Me.BackColor%>">
	<tr>
		<td height="28" colSpan="5">
			<asp:Label ID="LBselezionaComunita" Runat=server CssClass="titolo_campoSmall">Iscritti alla comunità:</asp:Label>
			<asp:RadioButtonList ID="RBLcomunita" Runat=server RepeatLayout=Flow AutoPostBack=True RepeatDirection=Horizontal CssClass="testo_campo">
				<asp:ListItem Value=0>corrente</asp:ListItem>
				<asp:ListItem Value=-1>corrente e relative sotto-comunità</asp:ListItem>
			</asp:RadioButtonList>
		</td>
	</tr>
	<tr>
		<td height="28" colSpan="5">
			<asp:Label ID="LBselezionaDest" Runat=server CssClass="titolo_campoSmall">Destinatari:</asp:Label>
			<asp:RadioButtonList ID="RBLabilitazione" Runat=server RepeatLayout=Flow RepeatDirection=Horizontal AutoPostBack=True CssClass="testo_campo">
				<asp:ListItem Value=1>SOLO gli Abilitati</asp:ListItem>
				<asp:ListItem Value=5>SOLO i bloccati</asp:ListItem>
				<asp:ListItem Value=4>In Attesa di conferma</asp:ListItem>
				<asp:ListItem Value=0> Tutti</asp:ListItem>
			</asp:RadioButtonList>
			<br/><br/>
		</td>
	</tr>
	<tr>
		<td>
             <telerik:radtabstrip id="TBSmenu" runat="server" align="Justify" Width="650px" Height="26px" SelectedIndex="0"
              causesvalidation="false" autopostback="true" skin="Outlook" enableembeddedskins="true">
                <tabs>
                    <telerik:RadTab text="Gruppi" value="Group" runat="server"></telerik:RadTab>
                    <telerik:RadTab text="Singoli" value="SingleUser" runat="server" ></telerik:RadTab>
                </tabs>
            </telerik:radtabstrip>
			<INPUT id="HDN_PanView" type="hidden" name="HDN_PanView" runat="server"/>
		</td>
	</tr>
	
	<tr>
		<td colSpan="5">
			<asp:panel id="PNLgruppi" Runat="server" BackColor="#A5C9EB" Width="100%" BorderWidth="1">
				<table>
					<tr>
						<td><br/>
							<asp:CheckBox id="CBXtutti" Runat="server" CssClass="smsStyle_Rubrica_sceltaPerGruppi" Text="Tutti"
								AutoPostBack="true"></asp:CheckBox>
						</td>
					</tr>
				</table>
				<asp:CheckBoxList id="CBLgruppi" Runat="server" CssClass="smsStyle_Rubrica_sceltaPerGruppi" RepeatLayout="table"
					Repeatdirection="Vertical"></asp:CheckBoxList>
				<br/>
			</asp:panel>&nbsp;
			<asp:panel id="PNLsingoli" Runat="server" Width="100%" Visible="False" BorderWidth="1">
				<table cellSpacing="0" cellPadding="0" width="100%" bgColor="#A5C9EB" border="0">
					<tr>
						<td colSpan="3" height="22">
							<table cellPadding="2" width="100%" bgColor="#A5C9EB" border="0">
								<tr>
									<td class="smsStyle_testoVoce" width="150">
										<asp:Label  ID="LBfiltraTipo" Runat=server CssClass="titolo_campoSmall">
											&nbsp;Filtra per tipologia:
										</asp:Label>
									</td>
									<td colSpan="2">
										<asp:DropDownList id="DDLfiltroTipoRuolo" Runat="server" CssClass="smsStyle_testoCampo" AutoPostBack="true"></asp:DropDownList>
									</td>
								</tr>
							</table>
						</td>

					</tr>
					<tr>

						<td vAlign="middle" nowrap="nowrap" bgColor="#A5C9EB">&nbsp; <INPUT class="smsStyle_testoCampo" id="TBsearch" type="text" size="30" name="TBsearch"/>
							&nbsp;
							<asp:button id="BTNfind" Runat="server" CssClass="Pulsante" Text="trova"></asp:button>
						</td>
						<td class="smsStyle_testoVoce" vAlign="middle" nowrap="nowrap" bgColor="#A5C9EB" colSpan="2">
							<asp:label ID="LBseleziona" Runat=server CssClass="titolo_campoSmall">&nbsp;Seleziona:&nbsp;&nbsp;</asp:label>
							<asp:dropdownlist id="DDLfiltro" Runat="server" CssClass="smsStyle_testoCampo">
								<asp:ListItem Value="-1">Nessuno</asp:ListItem>
								<asp:ListItem Value="0">Tutti</asp:ListItem>
							</asp:dropdownlist></td>

					</tr>
					<tr>

						<td class="NoSize" bgColor="#A5C9EB" colSpan="3" height="10">&nbsp;</td>

					</tr>
					<tr>

						<td align="center" bgColor="#CFE3F5" colSpan="3">
							<table style="border-Color:#CFE3F5" cellPadding="5" bgColor="#CFE3F5" border="0">
								<tr>
									<td width="120" rowSpan="3">
										<asp:listbox id="LBXelenco" Runat="server" CssClass="smsStyle_Rubrica_Elenco" EnableViewState="true"
											Rows="15" width="200px" SelectionMode="Multiple"></asp:listbox>
										<br/>
									</td>
									<td vAlign="bottom" align="center" width="50" height="50">
										<br/>
										<asp:button id="BTNscelta" Runat="server" CssClass="Pulsante" Text="-->"></asp:button></td>
									<td align="center" width="130" rowSpan="3">
										<asp:listbox id="LSB_A" Runat="server" CssClass="smsStyle_Rubrica_Elenco" EnableViewState="true"
											Rows="15" width="200px" SelectionMode="Multiple"></asp:listbox>
										<asp:listbox id="LSB_CC" Runat="server" CssClass="smsStyle_Rubrica_Elenco" EnableViewState="true"
											Rows="15" width="200px" SelectionMode="Multiple" Visible=False></asp:listbox>
										<asp:listbox id="LSB_CCN" Runat="server" CssClass="smsStyle_Rubrica_Elenco" EnableViewState="true"
											Rows="15" width="200px" SelectionMode="Multiple" Visible=False></asp:listbox>
										<br/>
									</td>
								</tr>
								<tr>
									<td vAlign="top" align="center" width="50" height="50">
										<asp:button id="BTNrimuovi" Runat="server" CssClass="Pulsante" Text="<--"></asp:button></td>
								</tr>
								<tr>
									<td vAlign="top" align="center" width="50">
										&nbsp;
									</td>
								</tr>
							</table>
						</td>

					</tr>
				</table>
				
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
				
				<INPUT id="HDN_CMNT_ID" type="hidden" name="HDN_CMNT_ID" runat="server"/>
				<INPUT id="HDN_isLimbo" type="hidden" name="HDN_isLimbo" runat="server"/>
				<INPUT id="HDN_setA" type="hidden" name="HDN_setA" runat="server"/>
				<INPUT id="HDN_setCCN" type="hidden" name="HDN_setCCN" runat="server"/>
				<INPUT id="HDN_setCC" type="hidden" name="HDN_setCC" runat="server"/>
				<INPUT id="HDN_Path" type="hidden" name="HDN_Path" runat="server"/>
			</asp:panel>
		</td>
	</tr>
</table>
<br/>

