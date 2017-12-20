<%@ Page Language="vb" AutoEventWireup="false" MasterPageFile="~/AjaxPortal.Master" Codebehind="RicercaComunita.aspx.vb" Inherits="Comunita_OnLine.RicercaComunita"%>
<%@ MasterType VirtualPath="~/AjaxPortal.Master" %>

<%--
--%>
<%@ Register TagPrefix="DETTAGLI" TagName="CTRLDettagli" Src="../UC/UC_dettaglicomunita.ascx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    
    <script language="Javascript" src="./../jscript/generali.js" type="text/javascript"></script>

    <script language="javascript" type="text/javascript">
	    function AggiornaForm(){
		    valore = document.forms[0].<%=me.DDLTipoRicerca.ClientID%>.options[document.forms[0].<%=me.DDLTipoRicerca.ClientID%>.selectedIndex].value
		    if (valore==-9){
			    document.forms[0].<%=me.HDNselezionato.ClientID%>.value = valore
			    __doPostBack('DDLTipoRicerca','');
			    return true;
			    }
		    else if (document.forms[0].<%=me.HDNselezionato.ClientID%>.value == -9){
			    document.forms[0].<%=me.HDNselezionato.ClientID%>.value = valore
			    __doPostBack('DDLTipoRicerca','');
			    return true;
			    }
		    else 
			    return false;
	    }		
			
			
	    function ChiudiMi(){
	    this.window.close();
	    }
			
	    function SelectMe(Me){
	    var HIDcheckbox,selezionati;
        //eval('HIDcheckbox=<%=Me.HDNcomunitaSelezionate.ClientID%>')
        HIDcheckbox = this.document.getElementById('<%=Me.HDNcomunitaSelezionate.ClientID%>'); 

	    //eval('HIDcheckbox= this.document.forms[0].HDNcomunitaSelezionate')
        selezionati = 0
		    for(i=0;i< document.forms[0].length; i++){ 
			    e=document.forms[0].elements[i];
			    if ( e.type=='checkbox' && e.name.indexOf("CBcorso") != -1 ){
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
                        HIDcheckbox.value = HIDcheckbox.value.substring(0, pos1)
                        HIDcheckbox.value = HIDcheckbox.value + valore.substring(pos1 + e.value.length + 1, valore.length)
						    }
				    }
			    }  
		    }
		    if (HIDcheckbox.value==",")
                            HIDcheckbox.value = ""
	    }
			
	    function SelectAll( SelectAllBox ){
		    var actVar = SelectAllBox.checked ;
		    var TBcheckbox;
            //eval('HIDcheckbox=<%=Me.HDNcomunitaSelezionate.ClientID%>')
            HIDcheckbox = this.document.getElementById('<%=Me.HDNcomunitaSelezionate.ClientID%>');
		    //eval('HDNcomunitaSelezionate= this.document.forms[0].HDNcomunitaSelezionate')
		    HDNcomunitaSelezionate.value = ""
		    for(i=0;i< document.forms[0].length; i++){ 
			    e=document.forms[0].elements[i];
			    if ( e.type=='checkbox' && e.name.indexOf("CBcorso") != -1 ){
				    e.checked= actVar ;
				    if (e.checked==true)
					    if (HDNcomunitaSelezionate.value == "")
						    HDNcomunitaSelezionate.value = ',' + e.value+','
					    else
						    HDNcomunitaSelezionate.value = HDNcomunitaSelezionate.value + e.value +','
			    }
		    }
	    }
			
	    function HasComunitaSelezionate(conferma,Messaggio,MessaggioConferma){
		    var HIDcheckbox,selezionati;
            //eval('HIDcheckbox=<%=Me.HDNcomunitaSelezionate.ClientID%>')
            HIDcheckbox = this.document.getElementById('<%=Me.HDNcomunitaSelezionate.ClientID%>');
		    //eval('HIDcheckbox= this.document.forms[0].HDNcomunitaSelezionate')
		    if (HIDcheckbox.value=="," || HIDcheckbox.value==""){
			    alert(Messaggio)
			    return false;
			    }
		    else{
			    if (conferma==true){
				    return confirm(MessaggioConferma);
				    }
			    else
				    return true;
			    }
	    }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="CPHservice" runat="server">
	<table width="900px" align="center">
<%--		<tr>
			<td class="RigaTitolo" align="left">
				<asp:label id="LBtitolo" Runat="server">Ricerca comunità</asp:label>
			</td>
		</tr>--%>
		<tr>
			<td align=right >
				<table align=right>
					<tr>
						<td>&nbsp;</td>
						<td align=right>
							<asp:Panel ID="PNLmenu" Runat=server HorizontalAlign=Right Visible=true>
								<asp:linkbutton ID="LNBalbero" Runat="server" Text="Albero comunità" CausesValidation="false" CssClass="LINK_MENU"></asp:linkbutton>
								<asp:linkbutton ID="LNBalberoGerarchico" Runat="server" Text="Albero gerarchico" CausesValidation="false" CssClass="LINK_MENU"></asp:linkbutton>
								<asp:linkbutton ID="LNBiscriviMultipli" Enabled=False Runat="server"  CssClass="LINK_MENU" Text="Iscrivi ai selzionati"></asp:linkbutton>
							</asp:Panel>
							<asp:Panel ID="PNLmenuDettagli" Runat="server" HorizontalAlign=Right Visible=False >
								<asp:linkbutton ID="LNBannullaDettagli" Runat="server" Text="Torna all'elenco" CausesValidation="false" CssClass="LINK_MENU"></asp:linkbutton>
								<asp:linkbutton ID="LNBiscriviDettagli" Runat="server" CssClass="LINK_MENU" CausesValidation="True" text="Iscrivi"></asp:linkbutton>
								<asp:linkbutton ID="LNBentraDettagli" Runat="server" CssClass="LINK_MENU" CausesValidation="True" text="Entra" ></asp:linkbutton>
							</asp:Panel>
							<asp:Panel ID="PNLmenuConferma" Runat="server" HorizontalAlign=Right Visible=False >
								<asp:linkbutton ID="LNBannullaConferma" Runat="server" Text="Torna all'elenco" CausesValidation="false" CssClass="LINK_MENU"></asp:linkbutton>
								<asp:linkbutton ID="LNBiscriviConferma" Runat="server" CssClass="LINK_MENU" CausesValidation="True" text="Iscrivi"></asp:linkbutton>
							</asp:Panel>
							<asp:Panel ID="PNLmenuIscritto" Runat="server" HorizontalAlign=Right Visible=False >
								<asp:linkbutton ID="LNBelencoIscritte" Runat="server" Text="Torna alle iscritte" CausesValidation="false" CssClass="LINK_MENU"></asp:linkbutton>
								<asp:linkbutton ID="LNBiscriviAltre" Runat="server" CssClass="LINK_MENU" CausesValidation="True" text="Altra iscrizione"></asp:linkbutton>
							</asp:Panel>
							<asp:Panel ID="PNLmenuAccesso" Runat=server HorizontalAlign=Right Visible=False>
								<asp:linkbutton ID="LNBannulla" Runat="server" Text="Torna all'elenco" CausesValidation="false" CssClass="LINK_MENU"></asp:linkbutton>
							</asp:Panel>
						</td>
					</tr>
				</table>
			</td>
		</tr>
		<tr>
			<td align="center">
				<asp:Panel ID="PNLconferma" Runat="server" Visible="False" HorizontalAlign="Center">
					<input type =hidden id="HDisChiusa" runat=server NAME="HDisChiusa"/>
					<table align="center">
						<tr>
							<td height="50" colspan=2>&nbsp;</td>
						</tr>
						<tr>
							<td align="left"  colspan=2>
								<asp:Label id="LBconferma" CssClass="messaggioIscrizione" Runat="server">Conferma l'iscrizione alla comunità #nomeComunita# - #nomeResponsabile#</asp:Label>
								<asp:Label id="LBconfermaMultipla" CssClass="messaggioIscrizione" Runat="server" Visible=False></asp:Label>
							</td>
						</tr>
						<tr>
							<td height="50" colspan=2>&nbsp;</td>
						</tr>
					</table>
				</asp:Panel>
				<asp:panel id="PNLcontenuto" Runat="server" HorizontalAlign="Center">
					<input type =hidden runat=server id="HDNselezionato" NAME="HDNselezionato"/>
					<input type =hidden runat=server id="HDNcomunitaSelezionate" NAME="HDNcomunitaSelezionate"/>
					<input type =hidden runat=server id="HDN_filtroFacolta" NAME="HDN_filtroFacolta"/>
					<input type =hidden runat=server id="HDN_filtroTipoRicerca" NAME="HDN_filtroTipoRicerca"/>
					<input type =hidden runat=server id="HDN_filtroValore" NAME="HDN_filtroValore"/>
					<input type =hidden runat=server id="HDN_filtroResponsabileID" NAME="HDN_filtroResponsabileID"/>
					<input type =hidden runat=server id="HDN_filtroLaureaID" NAME="HDN_filtroLaureaID"/>
					<input type =hidden runat=server id="HDN_filtroTipoCdl" NAME="HDN_filtroTipoCdl"/>
					<input type =hidden runat=server id="HDN_filtroAnno" NAME="HDN_filtroAnno"/>
					<input type =hidden runat=server id="HDN_filtroPeriodo" NAME="HDN_filtroPeriodo"/>
					<input type =hidden runat=server id="HDN_filtroTipoComunitaID" NAME="HDN_filtroTipoComunitaID"/>
					<input type =hidden runat=server id="HDN_filtroRicercaByIscrizione" NAME="HDN_filtroRicercaByIscrizione"/>
					<input type =hidden runat=server id="HDN_filtroStatus" name="HDN_filtroStatus"/>
					<asp:Table id="TBLfiltroNew" Runat=server  Width="900px" CellPadding=0 CellSpacing=0>
						<asp:TableRow id="TBRchiudiFiltro" Height=22px>
							<asp:TableCell CssClass="Filtro_CellSelezionato" HorizontalAlign=Center Width=150px Height=22px VerticalAlign=Middle >
								<asp:LinkButton ID="LNBchiudiFiltro" Runat=server CssClass="Filtro_Link" CausesValidation=False>Chiudi Filtri</asp:LinkButton>
							</asp:TableCell>
							<asp:TableCell CssClass="Filtro_CellDeSelezionato" Width=750px Height=22px>&nbsp;
							</asp:TableCell>
						</asp:TableRow>
						<asp:TableRow id="TBRapriFiltro" Visible=False Height=22px>
							<asp:TableCell ColumnSpan=1 CssClass="Filtro_CellApriFiltro" HorizontalAlign=Center Width=150px Height=22px>
								<asp:LinkButton ID="LNBapriFiltro" Runat=server CssClass="Filtro_Link" CausesValidation=False >Apri Filtri</asp:LinkButton>
							</asp:TableCell>
							<asp:TableCell CssClass="Filtro_Cellnull" Width=750px Height=22px>&nbsp;
							</asp:TableCell>
						</asp:TableRow>
						<asp:TableRow ID="TBRfiltri">
							<asp:TableCell CssClass="Filtro_CellFiltri" ColumnSpan=2 Width=900px HorizontalAlign=center>
								<asp:Table Runat=server ID="TBLfiltro" CellPadding=1 CellSpacing=0 Width="900px" HorizontalAlign=center>
									<asp:TableRow>
										<asp:TableCell CssClass="FiltroVoceSmall" ColumnSpan=2>
											<table cellspacing=0 border=0 align=left >
												<tr>
													<td height=30px width=70px nowrap="nowrap" >
														<asp:Label ID="LBtipoComunita_c" Runat=server CssClass="FiltroVoceSmall">Tipo Comunità</asp:Label>&nbsp;
													</td>
													<td height=30px>
														<asp:dropdownlist id="DDLTipo" runat="server" CssClass="FiltroCampoSmall" AutoPostBack="true"></asp:dropdownlist>
													</td>
													<td height=30px>&nbsp;</td>
													<td height=30px>
														<asp:Label ID="LBtipoRicerca_c" Runat=server CssClass="FiltroVoceSmall">Tipo Ricerca</asp:Label>&nbsp;
														<asp:dropdownlist id=DDLTipoRicerca Runat="server" CssClass="FiltroCampoSmall">
															<asp:ListItem Value=-2 Selected="true">Nome inizia per</asp:ListItem>
															<asp:ListItem Value=-7>Nome contiene</asp:ListItem>
															<asp:ListItem Value=-9>Del responsabile</asp:ListItem>
															<asp:ListItem Value=-3>Creata dopo il</asp:ListItem>
															<asp:ListItem Value=-4>Creata prima del</asp:ListItem>
															<asp:ListItem Value=-5>Data iscrizione dopo il</asp:ListItem>
															<asp:ListItem Value=-6>Data fine iscrizione prima del</asp:ListItem>
														</asp:dropdownlist>
													</td>
													<td height=30px>&nbsp;</td>
													<td height=30px>
														<asp:Label ID="LBvalore_c" Runat=server CssClass="FiltroVoceSmall" Visible=true >Valore</asp:Label>&nbsp;
														<asp:textbox id=TXBValore Runat="server" CssClass="FiltroCampoSmall" MaxLength=100 Columns=30></asp:textbox>
														<asp:DropDownList ID="DDLresponsabile" Runat=server AutoPostBack=True CssClass="FiltroCampoSmall" Visible=False></asp:DropDownList>
													</td>
												</tr>
											</table>
										</asp:TableCell>
									</asp:TableRow>
									<asp:TableRow Runat=server ID="TBRorgnCorsi">
										<asp:TableCell ID="TBCorganizzazione0" ColumnSpan=2>
											<table cellspacing=0 cellpadding=0>
												<tr>
													<td width=70px nowrap="nowrap">
														<asp:Label ID="LBorganizzazione_c" Runat=server CssClass="FiltroVoceSmall">Organizzazione:&nbsp;</asp:Label>
													</td>
													<td>
														<asp:DropDownList ID="DDLorganizzazione" Runat=server AutoPostBack=True CssClass="FiltroCampoSmall"></asp:DropDownList>		
													</td>
													<td>&nbsp;</td>
													<td>
														<asp:Table ID="TBLcorsi" CellPadding=2 CellSpacing=2 BorderStyle=None Runat=server Visible=False >
															<asp:TableRow>
																<asp:TableCell>
																	<asp:Label ID="LBannoAccademico_c" Runat=server CssClass="FiltroVoceSmall">A.A.:&nbsp;</asp:Label>
																</asp:TableCell>
																<asp:TableCell>
																	<asp:DropDownList ID="DDLannoAccademico" Runat=server AutoPostBack=True CssClass="FiltroCampoSmall"></asp:DropDownList>
																</asp:TableCell>
																<asp:TableCell>
																	<asp:Label ID="LBperiodo_c" Runat=server CssClass="FiltroVoceSmall">Periodo:&nbsp;</asp:Label>
																</asp:TableCell>
																<asp:TableCell>
																	<asp:DropDownList ID="DDLperiodo" Runat=server AutoPostBack=True CssClass="FiltroCampoSmall"></asp:DropDownList>
																</asp:TableCell>
															</asp:TableRow>
														</asp:Table>
														<asp:Table ID="TBLcorsiDiStudio" CellPadding=2 CellSpacing=2 BorderStyle=None Runat=server Visible=False >
															<asp:TableRow>
																<asp:TableCell>
																	<asp:Label ID="LBcorsoDiStudi_t" Runat=server CssClass="FiltroVoceSmall">A.A.:&nbsp;</asp:Label>
																</asp:TableCell>
																<asp:TableCell>
																	<asp:DropDownList ID="DDLtipoCorsoDiStudi" Runat=server AutoPostBack=True CssClass="FiltroCampoSmall"></asp:DropDownList>
																</asp:TableCell>
															</asp:TableRow>
														</asp:Table>
														<asp:Label ID="LBnoCorsi" Runat=server >&nbsp;</asp:Label>
													</td>
												</tr>
											</table>
										</asp:TableCell>
									</asp:TableRow>
									<asp:TableRow  Height=30px>
										<asp:TableCell Height=30px>
											<asp:Label ID="LBricercaByIscrizione_c" Runat=server CssClass="FiltroVoceSmall">Comunità:</asp:Label>
											<asp:RadioButtonList id="RBLricercaByIscrizione" runat="server" CssClass="FiltroCampoSmall" AutoPostBack="true" RepeatDirection=Horizontal RepeatLayout=Flow >
												<asp:ListItem  Value=0 Selected=true>a cui iscriversi</asp:ListItem>
												<asp:ListItem Value=1>a cui si è iscritti</asp:ListItem>
											</asp:RadioButtonList>
											&nbsp;&nbsp;&nbsp;&nbsp;
											<asp:Label ID="LBstatoComunita_t" Runat=server CssClass="FiltroVoceSmall">Stato:</asp:Label>
											<asp:RadioButtonList id="RBLstatoComunita" runat="server" CssClass="FiltroCampoSmall" AutoPostBack="true" RepeatDirection=Horizontal RepeatLayout=Flow >
												<asp:ListItem Value=0 Selected=true>Attivate</asp:ListItem>
												<asp:ListItem Value=1>Archiviate</asp:ListItem>
												<asp:ListItem Value=2>Bloccate</asp:ListItem>
											</asp:RadioButtonList>
										</asp:TableCell>
										<asp:TableCell HorizontalAlign=right Height=30px >
											<asp:CheckBox ID="CBXautoUpdate" Runat=server AutoPostBack=True CssClass="FiltroCampoSmall" Text="Aggiornamento automatico"></asp:CheckBox>
											&nbsp;
											<asp:button id="BTNCerca" Runat="server" CssClass="PulsanteFiltro" Text="Cerca"></asp:button>
										</asp:TableCell>
									</asp:TableRow>
								</asp:Table>
							</asp:TableCell>
						</asp:TableRow>
						<asp:TableRow Visible=true ID="TBRfiltriGenerici">
							<asp:TableCell ColumnSpan=2 Width=900px HorizontalAlign=Center>
								<table cellpadding=0 cellspacing=0 align=center Width=100% border=0>
									<tr>
										<td>
											<table align="left" width=500px>
												<tr>
													<td align="center" nowrap="nowrap" >
														<asp:linkbutton id="LKBtutti" Runat="server" CssClass="lettera" CommandArgument="-1" OnClick="FiltroLink_Click">Tutti</asp:linkbutton></td>
													<td align="center" nowrap="nowrap">
														<asp:linkbutton id="LKBaltro" Runat="server" CssClass="lettera" CommandArgument="0" OnClick="FiltroLink_Click">Altro</asp:linkbutton></td>
													<td align="center">
														<asp:linkbutton id="LKBa" Runat="server" CssClass="lettera" CommandArgument="1" OnClick="FiltroLink_Click">A</asp:linkbutton></td>
													<td align="center">
														<asp:linkbutton id="LKBb" Runat="server" CssClass="lettera" CommandArgument="2" OnClick="FiltroLink_Click">B</asp:linkbutton></td>
													<td align="center">
														<asp:linkbutton id="LKBc" Runat="server" CssClass="lettera" CommandArgument="3" OnClick="FiltroLink_Click">C</asp:linkbutton></td>
													<td align="center">
														<asp:linkbutton id="LKBd" Runat="server" CssClass="lettera" CommandArgument="4" OnClick="FiltroLink_Click">D</asp:linkbutton></td>
													<td align="center">
														<asp:linkbutton id="LKBe" Runat="server" CssClass="lettera" CommandArgument="5" OnClick="FiltroLink_Click">E</asp:linkbutton></td>
													<td align="center">
														<asp:linkbutton id="LKBf" Runat="server" CssClass="lettera" CommandArgument="6" OnClick="FiltroLink_Click">F</asp:linkbutton></td>
													<td align="center">
														<asp:linkbutton id="LKBg" Runat="server" CssClass="lettera" CommandArgument="7" OnClick="FiltroLink_Click">G</asp:linkbutton></td>
													<td align="center">
														<asp:linkbutton id="LKBh" Runat="server" CssClass="lettera" CommandArgument="8" OnClick="FiltroLink_Click">H</asp:linkbutton></td>
													<td align="center">
														<asp:linkbutton id="LKBi" Runat="server" CssClass="lettera" CommandArgument="9" OnClick="FiltroLink_Click">I</asp:linkbutton></td>
													<td align="center">
														<asp:linkbutton id="LKBj" Runat="server" CssClass="lettera" CommandArgument="10" OnClick="FiltroLink_Click">J</asp:linkbutton></td>
													<td align="center">
														<asp:linkbutton id="LKBk" Runat="server" CssClass="lettera" CommandArgument="11" OnClick="FiltroLink_Click">K</asp:linkbutton></td>
													<td align="center">
														<asp:linkbutton id="LKBl" Runat="server" CssClass="lettera" CommandArgument="12" OnClick="FiltroLink_Click">L</asp:linkbutton></td>
													<td align="center">
														<asp:linkbutton id="LKBm" Runat="server" CssClass="lettera" CommandArgument="13" OnClick="FiltroLink_Click">M</asp:linkbutton></td>
													<td align="center">
														<asp:linkbutton id="LKBn" Runat="server" CssClass="lettera" CommandArgument="14" OnClick="FiltroLink_Click">N</asp:linkbutton></td>
													<td align="center">
														<asp:linkbutton id="LKBo" Runat="server" CssClass="lettera" CommandArgument="15" OnClick="FiltroLink_Click">O</asp:linkbutton></td>
													<td align="center">
														<asp:linkbutton id="LKBp" Runat="server" CssClass="lettera" CommandArgument="16" OnClick="FiltroLink_Click">P</asp:linkbutton></td>
													<td align="center">
														<asp:linkbutton id="LKBq" Runat="server" CssClass="lettera" CommandArgument="17" OnClick="FiltroLink_Click">Q</asp:linkbutton></td>
													<td align="center">
														<asp:linkbutton id="LKBr" Runat="server" CssClass="lettera" CommandArgument="18" OnClick="FiltroLink_Click">R</asp:linkbutton></td>
													<td align="center">
														<asp:linkbutton id="LKBs" Runat="server" CssClass="lettera" CommandArgument="19" OnClick="FiltroLink_Click">S</asp:linkbutton></td>
													<td align="center">
														<asp:linkbutton id="LKBt" Runat="server" CssClass="lettera" CommandArgument="20" OnClick="FiltroLink_Click">T</asp:linkbutton></td>
													<td align="center">
														<asp:linkbutton id="LKBu" Runat="server" CssClass="lettera" CommandArgument="21" OnClick="FiltroLink_Click">U</asp:linkbutton></td>
													<td align="center">
														<asp:linkbutton id="LKBv" Runat="server" CssClass="lettera" CommandArgument="22" OnClick="FiltroLink_Click">V</asp:linkbutton></td>
													<td align="center">
														<asp:linkbutton id="LKBw" Runat="server" CssClass="lettera" CommandArgument="23" OnClick="FiltroLink_Click">W</asp:linkbutton></td>
													<td align="center">
														<asp:linkbutton id="LKBx" Runat="server" CssClass="lettera" CommandArgument="24" OnClick="FiltroLink_Click">X</asp:linkbutton></td>
													<td align="center">
														<asp:linkbutton id="LKBy" Runat="server" CssClass="lettera" CommandArgument="25" OnClick="FiltroLink_Click">Y</asp:linkbutton></td>
													<td align="center">
														<asp:linkbutton id="LKBz" Runat="server" CssClass="lettera" CommandArgument="26" OnClick="FiltroLink_Click">Z</asp:linkbutton></td>
												</tr>
											</table>
										</td>
										<td align=right >
											<asp:CheckBox CssClass="FiltroCampoSmall" ID="CBXmostraPadre" Runat=server Checked=False Text=" Mostra comunità di appartenenza." TextAlign=Right AutoPostBack=True ></asp:CheckBox>
											<asp:label ID="LBnumeroRecord_c" Runat =server cssclass="Filtro_TestoPaginazione">N° Record</asp:label>
											<asp:dropdownlist id="DDLNumeroRecord" CssClass="Filtro_RecordPaginazione" Runat="server" AutoPostBack="true">
												<asp:ListItem Value="15" ></asp:ListItem>
												<asp:ListItem Value="30" Selected="true"></asp:ListItem>
												<asp:ListItem value="45"></asp:ListItem>
												<asp:ListItem value="50"></asp:ListItem>
											</asp:dropdownlist>
										</td>
									</tr>
								</table>	
							</asp:TableCell>
						</asp:TableRow>
						<asp:TableRow>
							<asp:TableCell ColumnSpan=2>
								<asp:datagrid 
									id="DGComunita" runat="server" 
									PageSize="30" DataKeyField="CMNT_id" 
									AllowPaging="true" AutoGenerateColumns="False" 
									AllowSorting="true" 
									ShowFooter="false" 
									CssClass="DataGrid_Generica">
									<AlternatingItemStyle CssClass="ROW_Alternate_Small"></AlternatingItemStyle>
									<HeaderStyle CssClass="ROW_header_Small_Center"></HeaderStyle>
									<ItemStyle CssClass="ROW_Normal_Small" Height="22px"></ItemStyle>
									<PagerStyle CssClass="ROW_Page_Small" Position=TopAndBottom Mode="NumericPages" Visible="true"
									HorizontalAlign="Right" Height="18px" VerticalAlign="Bottom"></PagerStyle>
									<Columns>
										<asp:TemplateColumn runat="server" HeaderText="Tipo" ItemStyle-Width="40" SortExpression="TPCM_Descrizione" ItemStyle-CssClass=ROW_TD_Small_Center>
											<ItemTemplate>
												<img runat=server src='<%# DataBinder.Eval(Container.DataItem, "TPCM_icona") %>' alt='<%# DataBinder.Eval(Container.DataItem, "TPCM_descrizione") %>' ID="Img2"/>
											</ItemTemplate>
										</asp:TemplateColumn>
										<asp:TemplateColumn  HeaderText="Nome" SortExpression="CMNT_Nome">
											<ItemTemplate>
												<asp:Table ID="TBLnome" Runat=server HorizontalAlign=Left>
													<asp:TableRow ID="TBRnome"  Runat=server>
														<asp:TableCell>
															&nbsp;
														</asp:TableCell>
														<asp:TableCell ID="TBCchiusa"  Runat=server>
															<asp:Image ID="IMGisChiusa" Runat=server Visible=False BorderStyle=None ></asp:Image>
														</asp:TableCell>
														<asp:TableCell ID="TBCnome"  Runat=server>
															<asp:Label ID="LBcomunitaNome" Runat="server">
																<%# DataBinder.Eval(Container.DataItem, "CMNT_Esteso") %>
															</asp:Label>
																				
															(<b><asp:LinkButton ID="LNBlogin" Runat=server CommandName="Login" CausesValidation=False>Entra</asp:LinkButton>
															<asp:LinkButton ID="LNBiscrivi" Runat=server CommandName="Iscrivi" Visible=False CausesValidation=False >Entra</asp:LinkButton></b>
															|
															<asp:LinkButton ID="LNBdettagli" Runat=server CommandName="dettagli" CausesValidation=False>Dettagli</asp:LinkButton>
															<asp:Label ID="LBseparatorNews" Runat=server Visible=False>|</asp:Label>
															<asp:Literal ID="LThasnews" runat="server" Visible="false"></asp:Literal>
															)
														</asp:TableCell>
													</asp:TableRow>
												</asp:Table>			
											</ItemTemplate>
										</asp:TemplateColumn>
										<asp:BoundColumn DataField="AnnoAccademico" HeaderText="A.A." Visible="false" SortExpression="CMNT_Anno" ItemStyle-CssClass=ROW_TD_Small_Center></asp:BoundColumn>
										<asp:BoundColumn DataField="Periodo" HeaderText="Periodo" Visible="false" SortExpression="CMNT_PRDO_descrizione" ItemStyle-CssClass=ROW_TD_Small></asp:BoundColumn>
										<asp:ButtonColumn Text="Mostra" CommandName="dettagli" HeaderText="Dettagli" ItemStyle-Width="60" Visible=False ></asp:ButtonColumn>
										<asp:BoundColumn DataField="AnagraficaResponsabile" HeaderText="Responsabile" ItemStyle-Width="150" SortExpression="CMNT_Responsabile" ItemStyle-CssClass=ROW_TD_Small></asp:BoundColumn>
										<asp:BoundColumn HeaderText="Iscritti" DataField="Iscritti" ItemStyle-CssClass=ROW_TD_Small HeaderStyle-CssClass="ROW_Header_Small"></asp:BoundColumn>
										<asp:BoundColumn DataField="CMNT_ID" Visible="False"></asp:BoundColumn>
										<asp:BoundColumn DataField="CMNT_Anno"  Visible="False"></asp:BoundColumn>
										<asp:BoundColumn DataField="CMNT_dataInizioIscrizione"  Visible="False"></asp:BoundColumn>
										<asp:BoundColumn DataField="CMNT_dataFineIscrizione"  Visible="False"></asp:BoundColumn>
																			
										<asp:BoundColumn DataField="ALCM_Path" Visible="false"></asp:BoundColumn>
										<asp:BoundColumn DataField="CMNT_Responsabile" Visible="False"></asp:BoundColumn>
										<asp:BoundColumn DataField="TPCM_icona" Visible="false"></asp:BoundColumn>
										<asp:BoundColumn DataField="CMNT_EstesoNoSpan" Visible="false"></asp:BoundColumn>
										<asp:BoundColumn DataField="CMNT_Iscritti" Visible=False></asp:BoundColumn>
										<asp:BoundColumn DataField="RLPC_attivato" Visible="false"></asp:BoundColumn>
										<asp:BoundColumn DataField="RLPC_abilitato" Visible="false"></asp:BoundColumn>
										<asp:BoundColumn DataField="RLPC_TPRL_ID" Visible="false"></asp:BoundColumn>
										<asp:BoundColumn DataField="CMNT_MaxIscritti" Visible=False></asp:BoundColumn>
										<asp:BoundColumn DataField="ALCM_isChiusaForPadre" Visible=False></asp:BoundColumn>
										<asp:BoundColumn DataField="CMNT_TPCM_id" Visible=False></asp:BoundColumn>
										<asp:BoundColumn DataField="CMNT_CanSubscribe" Visible="false"></asp:BoundColumn>
										<asp:BoundColumn DataField="CMNT_CanUnsubscribe" Visible="false"></asp:BoundColumn>
										<asp:BoundColumn DataField="CMNT_MaxIscrittiOverList" Visible="false"></asp:BoundColumn>
										<asp:BoundColumn DataField="CMNT_Nome" Visible="false"></asp:BoundColumn>
										<asp:BoundColumn DataField="CMNT_Archiviata" Visible="false"></asp:BoundColumn>
										<asp:BoundColumn DataField="CMNT_Bloccata" Visible="false"></asp:BoundColumn>
										<asp:TemplateColumn ItemStyle-Width="30px" ItemStyle-CssClass=ROW_TD_Small_center>
											<HeaderTemplate>
												<input type="checkbox" id="SelectAll2" onclick="SelectAll(this);" runat="server" NAME="SelectAll"/>
											</HeaderTemplate>
											<ItemTemplate>
												<input runat=server  type="checkbox" id="CBcorso" name="CBcorso"  onclick="SelectMe(this);"/>
											</ItemTemplate>
										</asp:TemplateColumn>
									</Columns>
								</asp:datagrid><br/>
								<asp:Label id=LBmsgDG Runat="server" CssClass="avviso_normal" Visible="False"></asp:Label>
								</asp:TableCell>
						</asp:TableRow>
					</asp:Table>
				</asp:panel>
				<asp:panel id="PNLdettagli" Runat="server" HorizontalAlign="Center" Visible="false">
					<table width=700 align=center border=0>
						<tr>
							<td align=center>
							<FIELDSET><LEGEND class=tableLegend>
								<asp:Label ID="LBlegenda" Runat=server cssclass=tableLegend>Dettagli comunità</asp:Label>
								</LEGEND>
								<input type =hidden runat=server id="HDNcmnt_ID" NAME="HDNcmnt_ID"/>
								<input type =hidden runat=server id="HDNtprl_id" NAME="HDNtprl_id"/>
								<input type =hidden runat=server id="HDNcmnt_Path" NAME="HDNcmnt_Path"/>
								<input type =hidden runat=server id="HDNisChiusaForPadre" NAME="HDNisChiusaForPadre"/>
								<asp:Label Visible="False" ID="LBtprl_id" Runat="server"></asp:Label>
								<DETTAGLI:CTRLDettagli id="CTRLDettagli" runat="server"></DETTAGLI:CTRLDettagli>	
														
							</FIELDSET> 
							</td>
						</tr>
						<tr>
							<td>&nbsp;</td>
						</tr>
					</table>
				</asp:panel>
				<asp:panel id="PNLmessaggi" Runat="server" Visible="False">
					<table cellSpacing=0 cellPadding=0 align=center border=0>
						<tr>
							<td height=30>&nbsp;</td>
						</tr>
						<tr>
							<td>
							<asp:Label id=LBMessaggi Runat="server" CssClass="avviso"></asp:Label>
							</td>
						</tr>
						<tr>
							<td height=30>&nbsp;</td>
						</tr>
					</table>
				</asp:panel>
				<asp:panel id="PNLiscrizioneAvvenuta" Runat="server" Visible="False">
					<table cellSpacing=0 cellPadding=0 align=center border=0>
						<tr>
							<td height=30>&nbsp;</td>
						</tr>
						<tr>
							<td>
								<asp:Label id="LBiscrizione" Runat="server" CssClass="avviso"></asp:Label>
								<br/><br/>
							</td>
						</tr>
						<tr>
							<td height=30>&nbsp;</td>
						</tr>
					</table>
				</asp:panel>
				<asp:panel id="PNLpermessi" Runat="server" HorizontalAlign="Center" Visible="False">
					<table align=center>
						<tr>
							<td height=50>&nbsp;</td>
						</tr>
						<tr>
							<td align=center>
								<asp:Label id=LBNopermessi Runat="server" CssClass="messaggio"></asp:Label>
							</td>
						</tr>
						<tr>
							<td vAlign=top height=50>&nbsp; </td>
						</tr>
					</table>
				</asp:panel>
			</td>
		</tr>
	</table>
</asp:Content>


<%--<!DOCTYPE HTML PUBLIC "-//W3C//Dtd HTML 4.0 transitional//EN">
<HTML>
  <head runat="server">
		<title>Comunità On Line - Ricerca Comunità</title>
		
		<meta content="Microsoft Visual Studio.NET 7.0" name="GENERATOR"/>
		<meta content="Visual Basic 7.0" name="CODE_LANGUAGE"/>
		<meta content="JavaScript" name="vs_defaultClientScript"/>
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema"/>
		<LINK href="./../Styles.css" type="text/css" rel="stylesheet"/>
</HEAD>

	<body onkeydown="return SubmitRicerca(event);">
		<form id="aspnetForm" method="post" runat="server">
		<asp:ScriptManager ID="SCMmanager" runat="server"></asp:ScriptManager>
			<table cellSpacing="0" cellPadding="0" width="900px" align="center" border="0">
				<tr>
					<td colSpan="3"><HEADER:CtrLHEADER id="Intestazione" runat="server"></HEADER:CtrLHEADER></td>
				</tr>
				<tr>
					<td colSpan="3">

					</td>
				</tr>
			</table>
			<FOOTER:CtrLFOOTER id="CtrLFooter" runat="server"></FOOTER:CtrLFOOTER>
		</form>
	</body>
</HTML>--%>
