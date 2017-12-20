<%@ Control Language="vb" AutoEventWireup="false" Codebehind="UC_RubricaMailGlobale.ascx.vb" Inherits="Comunita_OnLine.UC_RubricaMailGlobale" TargetSchema="http://schemas.microsoft.com/intellisense/ie5" %>
<%--OLD radtabstrip TELERIK--%>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>

<script type="text/javascript" language="javascript">
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
		<td height="28">
			<asp:Label ID="LBselezionaFacolta" CssClass="titolo_campoSmall" Runat=server>Iscritti alla Facoltà/Organizzazione:</asp:Label>
			<asp:DropDownList ID=DDLorganizzazione Runat=server CssClass ="smsStyle_testoCampo"  AutoPostBack=True ></asp:DropDownList>
		</td>
	</tr>
	
	<tr>
		<td height="28" colSpan="5">
			<asp:Label ID="LBselezionaDest" CssClass="titolo_campoSmall" Runat=server>Destinatari:</asp:Label>
			<asp:RadioButtonList ID="RBLabilitazione" Runat=server RepeatLayout=Flow RepeatDirection=Horizontal AutoPostBack=True>
				<asp:ListItem Value=1>SOLO gli Abilitati</asp:ListItem>
				<asp:ListItem Value=5>SOLO i bloccati</asp:ListItem>
				<asp:ListItem Value=9>In Attesa di conferma</asp:ListItem>
				<asp:ListItem Value=0> Tutti</asp:ListItem>
			</asp:RadioButtonList>
		</td>
	</tr>
	
	<tr>
		<td>
            <telerik:radtabstrip id="TBSmenu" runat="server" align="Justify" Width="650px" Height="26px" SelectedIndex="0"
              causesvalidation="false" autopostback="true" skin="Outlook" enableembeddedskins="true">
                <tabs>
                    <telerik:RadTab text="Gruppi" value="RTABGruppi" runat="server"></telerik:RadTab>
                    <telerik:RadTab text="Singoli" value="RTABSingoli" runat="server" ></telerik:RadTab>
                </tabs>
            </telerik:radtabstrip>
			<INPUT id="HDN_PanView" type="hidden" name="HDN_PanView" runat="server"/>
		</td>
	</tr>
	
	<tr>
		<td>
			<asp:panel id="PNLgruppi" Runat="server" Width="100%" BorderWidth=1 BackColor="#FFa0a0"> 
				<asp:CheckBox id="CBXtutti" Runat="server" CssClass="smsStyle_Rubrica_sceltaPerGruppi" Text="Tutti" AutoPostBack="true"></asp:CheckBox> <br/>
				<asp:CheckBoxList id="CBLgruppi" Runat="server" CssClass="smsStyle_Rubrica_sceltaPerGruppi" RepeatLayout="table" Repeatdirection="Vertical"></asp:CheckBoxList>
			</asp:panel>
										
			<asp:panel id="PNLsingoli" Runat="server" Width="100%" Visible="False" BorderWidth=1>
					
				<table cellSpacing="0" cellPadding="0" width="100%" border="0">
					<tr bgcolor="#FFa0a0">
						<td>
							<table cellPadding="2" width="100%" bgColor="#FFa0a0" border="0">
								<tr>
									<td colSpan="4">
										<asp:Label  ID="LBfiltraTipo" CssClass="titolo_campoSmall" Runat=server >&nbsp;Filtra per tipologia:</asp:Label>
										&nbsp;&nbsp;&nbsp;
										<asp:DropDownList id="DDLfiltroTipoPersona" Runat="server" CssClass="smsStyle_testoCampo" AutoPostBack="true"></asp:DropDownList>
									</td>
								</tr>
								<tr>
									<td>
										<INPUT class="smsStyle_testoCampo" id="TBsearch" type="text" size="30" name="TBsearch"/>
									&nbsp;
										<asp:button id="BTNfind" Runat="server" CssClass="Pulsante" Text="trova"></asp:button>
									</td><td>
										<asp:label ID="LBseleziona" CssClass="titolo_campoSmall" Runat=server >&nbsp;Seleziona:&nbsp;&nbsp;</asp:label>
										&nbsp;
										<asp:dropdownlist id="DDLfiltro" Runat="server" CssClass="smsStyle_testoCampo">
											<asp:ListItem Value="-1">Nessuno</asp:ListItem>
											<asp:ListItem Value="0">Tutti</asp:ListItem>
										</asp:dropdownlist>
									</td>
								</tr>
							</table>
						</td>
					</tr><tr>
						<td align="center"  bgColor="#FFC8C8">
							<table cellPadding="5" bgColor="#FFC8C8" border="0">
								<tr>
									<td rowSpan="3" align=right>
										<asp:listbox id="LBXelenco" Runat="server" CssClass="smsStyle_Rubrica_Elenco" EnableViewState="true" Rows="15" width="200px" SelectionMode="Multiple"></asp:listbox>
									</td>
									<td vAlign="bottom" align="center" width="50" height="50">
										<asp:button id="BTNscelta" Runat="server" CssClass="Pulsante" Text="-->"></asp:button>
									</td>
									<td align="center" width="130" rowSpan="3">
										<asp:listbox id="LSB_A" Runat="server" CssClass="smsStyle_Rubrica_Elenco" EnableViewState="true" Rows="15" width="200px" SelectionMode="Multiple"></asp:listbox>
										<asp:listbox id="LSB_CC" Runat="server" CssClass="smsStyle_Rubrica_Elenco" EnableViewState="true" Rows="15" width="200px" SelectionMode="Multiple" Visible=False ></asp:listbox>
										<asp:listbox id="LSB_CCN" Runat="server" CssClass="smsStyle_Rubrica_Elenco" EnableViewState="true" Rows="15" width="200px" SelectionMode="Multiple" Visible=False></asp:listbox>
									</td>
								</tr>
								<tr>
									<td vAlign="top" align="center" width="50" height="50">
										<asp:button id="BTNrimuovi" Runat="server" CssClass="Pulsante" Text="<--"></asp:button></td>
								</tr>
								<tr>
									<td vAlign="top" align="center" width="50">&nbsp;</td>
								</tr>
							</table>
						</td>
					</tr>
				</table>
				
				<INPUT id="HDNgruppi" type="hidden" name="HDNgruppi" runat="server"/>
				<INPUT id="HDNgruppiNome" type="hidden" name="HDNgruppiNome" runat="server"/>
				<INPUT id="HDNgruppiTotale" type="hidden" name="HDNgruppiTotale" runat="server"/>
				
				<INPUT id="HDNgruppi_A" type="hidden" name="HDNgruppi_A" runat="server"/>
				<INPUT id="HDNgruppiNome_A" type="hidden" name="HDNgruppiNome_A" runat="server"/>
				<INPUT id="HDNgruppiTutti_A" type="hidden" name="HDNgruppiTutti_A" runat="server"/>
				<INPUT id="HDN_RBLabilitazioneA" type="hidden" name="HDN_RBLabilitazioneA" runat="server"/>
				
				<INPUT id="HDNgruppi_CC" type="hidden" name="HDNgruppi_CC" runat="server"/>
				<INPUT id="HDNgruppiNome_CC" type="hidden" name="HDNgruppiNome_CC" runat="server"/>
				<INPUT id="HDNgruppiTutti_CC" type="hidden" name="HDNgruppiTutti_CC" runat="server"/>
				<INPUT id="HDN_RBLabilitazioneCC" type="hidden" name="HDN_RBLabilitazioneCC" runat="server"/>
				
				<INPUT id="HDNgruppi_CCN" type="hidden" name="HDNgruppi_CCN" runat="server"/>
				<INPUT id="HDNgruppiNome_CCN" type="hidden" name="HDNgruppiNome_CCN" runat="server"/>
				<INPUT id="HDNgruppiTutti_CCN" type="hidden" name="HDNgruppiTutti_CCN" runat="server"/>
				<INPUT id="HDN_RBLabilitazioneCCN" type="hidden" name="HDN_RBLabilitazioneCCN" runat="server"/>
				
				<INPUT id="HDN_setA" type="hidden" name="HDN_setA" runat="server"/>
				<INPUT id="HDN_setCCN" type="hidden" name="HDN_setCCN" runat="server"/>
				<INPUT id="HDN_setCC" type="hidden" name="HDN_setCC" runat="server"/>
				
			</asp:panel>
		</td>
	</tr>
</table>