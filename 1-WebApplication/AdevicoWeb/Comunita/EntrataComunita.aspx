<%@ Page Language="vb" AutoEventWireup="false" MasterPageFile="~/AjaxPortal.Master" Codebehind="EntrataComunita.aspx.vb" Inherits="Comunita_OnLine.EntrataComunita" %>
<%@ MasterType VirtualPath="~/AjaxPortal.Master" %>

<%--
--%>
<%@ Register TagPrefix="DETTAGLI" Tagname="CTRLDettagli" Src="../UC/UC_dettaglicomunita.ascx" %>
<%@ Register TagPrefix="radt" Namespace="Telerik.WebControls" Assembly="RadTreeview.Net2" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
		<meta http-equiv="cache-control" content="no-cache" />
		<meta http-equiv="pragma" content="no-cache" />
		<meta http-equiv="expires" content="0"/>

        <script type="text/javascript" language="Javascript" src="./../jscript/generali.js"></script>

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
	
			function SubmitRicerca(event){
				 if (document.all){
					if (event.keyCode == 13){
						event.returnValue=false;
						event.cancel = true;
						try{
							document.forms[0].BTNCerca.click();}
						catch (ex){
							return false;}
						}
					}
				else if (document.getElementById){
					if (event.which == 13){
						event.returnValue=false;
						event.cancel = true;
						try{
							document.forms[0].BTNCerca.click();}
						catch(ex){
							return false;}
						}
					}
				else if(document.layers){
					if(event.which == 13){
						event.returnValue=false;
						event.cancel = true;
							try{
							document.forms[0].BTNCerca.click();}
						catch(ex){
							return false;}
						}
					}
				else
					return true;
			}
			
			function SelectMe(Me){
				var HIDcheckbox;
				for(i=0;i< document.forms[0].length; i++){ 
					e=document.forms[0].elements[i];
					if ( e.type=='checkbox' && e.name.indexOf("CBazione") != -1 ){
						if (e.checked==true){
							if (document.forms[0].<%= Me.HDNazione.ClientId %>.value == ""){
								document.forms[0].<%= Me.HDNazione.ClientId %>.value = ',' + e.value+','
							}	  
							else{
								pos1 = document.forms[0].<%= Me.HDNazione.ClientId %>.value.indexOf(',' + e.value+',')
								if (pos1==-1)
									document.forms[0].<%= Me.HDNazione.ClientId %>.value = document.forms[0].<%= Me.HDNazione.ClientId %>.value + e.value +','
								}
						}
						else{
							valore = document.forms[0].<%= Me.HDNazione.ClientId %>.value
							pos1 = document.forms[0].<%= Me.HDNazione.ClientId %>.value.indexOf(',' + e.value+',')
							if (pos1!=-1){
								stringa = ',' + e.value
								document.forms[0].<%= Me.HDNazione.ClientId %>.value = document.forms[0].<%= Me.HDNazione.ClientId %>.value.substring(0,pos1)
								document.forms[0].<%= Me.HDNazione.ClientId %>.value = document.forms[0].<%= Me.HDNazione.ClientId %>.value + valore.substring(pos1+e.value.length+1,valore.length)
								}
						}
					}  
				}
				if (document.forms[0].<%= Me.HDNazione.ClientId %>.value==",")
					document.forms[0].<%= Me.HDNazione.ClientId %>.value = ""
				}
	
			
		function ComunitaForCancella(forText){
			if (document.forms[0].<%= Me.HDNazione.ClientId %>.value == "," || document.forms[0].<%= Me.HDNazione.ClientId %>.value == "")
				return false;
			else
				return confirm(forText);
			}

		</script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="CPHservice" runat="server">
	<table width="900px" align="center" cellpadding=0 cellspacing=0>
<%--		<tr>
			<td class="RigaTitolo" align="left">
				<asp:label id="LBtitolo" Runat="server">Elenco comunità a cui si è iscritti</asp:label>
			</td>
		</tr>--%>
		<tr>
			<td align=right>
				<asp:Panel ID="PNLmenu" runat="server" HorizontalAlign=Right Visible=true>
					<asp:linkbutton ID="LNBalbero" Runat="server" Text="Albero comunità" CausesValidation="false" CssClass="LINK_MENU"></asp:linkbutton>
					<asp:linkbutton ID="LNBalberoGerarchico" Runat="server" Text="Albero gerarchico" CausesValidation="false" CssClass="LINK_MENU"></asp:linkbutton>
				</asp:Panel>
				<asp:Panel ID="PNLmenuDettagli" Runat="server" HorizontalAlign=Right Visible=False >
					<asp:linkbutton ID="LNBannullaDettagli" Runat="server" Text="Torna all'elenco" CausesValidation="false" CssClass="LINK_MENU"></asp:linkbutton>
					<asp:linkbutton ID="LNBentra" Runat="server" CssClass="LINK_MENU" CausesValidation="True" text="Entra"></asp:linkbutton>
				</asp:Panel>
				<asp:Panel ID="PNLmenuAccesso" runat="server" HorizontalAlign=Right Visible=False>
					<asp:linkbutton ID="LNBannulla" Runat="server" Text="Torna all'elenco" CausesValidation="false" CssClass="LINK_MENU"></asp:linkbutton>
				</asp:Panel>
				<asp:Panel ID="PNLmenuDeiscrivi" runat="server" HorizontalAlign=Right Visible=False>
					<asp:linkbutton ID="LNBannullaDeIscrizione" Runat="server" Text="Torna all'elenco" CausesValidation="false" CssClass="LINK_MENU"></asp:linkbutton>
				</asp:Panel>
			</td>
		</tr>
		<tr>
			<td align="center">
				<asp:panel id="PNLpermessi" Runat="server" HorizontalAlign="Center" Visible="False" width="900px">
					<table align=center cellpadding="0" cellspacing="0">
						<tr>
							<td height=50>&nbsp;</td>
						</tr>
						<tr>
							<td align=center>
								<asp:Label id=LBNopermessi Runat="server" CssClass="messaggio"></asp:Label>
							</td>
						</tr>
						<tr>
							<td vAlign=top height=50>
								&nbsp; 
							</td>
						</tr>
					</table>
				</asp:panel>
				<asp:panel id="PNLData" Runat="server" HorizontalAlign="Center" width="900px">
					<table align=center border="0" cellpadding="0" cellspacing="0">
						<tr>
							<td>
								<input type="hidden" runat="server" id="HDNselezionato" name="HDNselezionato"/>
								<input type="hidden" runat="server" id="HDN_filtroFacolta" name="HDN_filtroFacolta"/>
								<input type="hidden" runat="server" id="HDN_filtroTipoRicerca" name="HDN_filtroTipoRicerca"/>
								<input type="hidden" runat="server" id="HDN_filtroValore" name="HDN_filtroValore"/>
								<input type="hidden" runat="server" id="HDN_filtroResponsabileID" name="HDN_filtroResponsabileID"/>
								<input type="hidden" runat="server" id="HDN_filtroLaureaID" name="HDN_filtroLaureaID"/>
								<input type="hidden" runat="server" id="HDN_filtroTipoCdl" name="HDN_filtroTipoCdl"/>
								<input type="hidden" runat="server" id="HDN_filtroAnno" name="HDN_filtroAnno"/>
								<input type="hidden" runat="server" id="HDN_filtroPeriodo" name="HDN_filtroPeriodo"/>
								<input type="hidden" runat="server" id="HDN_filtroTipoComunitaID" name="HDN_filtroTipoComunitaID"/>
								<input type="hidden" runat="server" id="HDN_filtroStatus" name="HDN_filtroStatus"/>
										
								<asp:Table id="TBLfiltroNew" runat="server"  Width="900px" CellPadding=0 CellSpacing=0>
									<asp:TableRow id="TBRchiudiFiltro" Height=22px>
										<asp:TableCell CssClass="Filtro_CellSelezionato" HorizontalAlign=Center Width=150px Height=22px VerticalAlign=Middle >
											<asp:LinkButton ID="LNBchiudiFiltro" runat="server" CssClass="Filtro_Link" CausesValidation=False>Chiudi Filtri</asp:LinkButton>
										</asp:TableCell>
										<asp:TableCell CssClass="Filtro_CellDeSelezionato" Width=750px Height=22px>&nbsp;
										</asp:TableCell>
									</asp:TableRow>
									<asp:TableRow id="TBRapriFiltro" Visible=False Height=22px>
										<asp:TableCell ColumnSpan=1 CssClass="Filtro_CellApriFiltro" HorizontalAlign=Center Width=150px Height=22px>
											<asp:LinkButton ID="LNBapriFiltro" runat="server" CssClass="Filtro_Link" CausesValidation=False >Apri Filtri</asp:LinkButton>
										</asp:TableCell>
										<asp:TableCell CssClass="Filtro_Cellnull" Width=750px Height=22px>&nbsp;
										</asp:TableCell>
									</asp:TableRow>
									<asp:TableRow ID="TBRfiltri">
										<asp:TableCell CssClass="Filtro_CellFiltri" ColumnSpan=2 Width=900px HorizontalAlign=center>
											<asp:Table runat="server" ID="TBLfiltro" CellPadding=1 CellSpacing=0 Width="900px" HorizontalAlign=center >
												<asp:TableRow>
													<asp:TableCell CssClass="FiltroVoceSmall" ColumnSpan=2 >
														<table cellspacing=0 border=0 align=left >
															<tr>
																<td>
																	<asp:Label ID="LBtipoComunita_c" runat="server" CssClass="FiltroVoceSmall">Tipo Comunità</asp:Label>&nbsp;
																</td>
																<td>
																	<asp:dropdownlist id="DDLTipo" runat="server" CssClass="FiltroCampoSmall" AutoPostBack="true"></asp:dropdownlist>
																</td>
																<td>&nbsp;</td>
																<td>
																	<asp:Label ID="LBtipoRicerca_c" runat="server" CssClass="FiltroVoceSmall">Tipo Ricerca</asp:Label>&nbsp;
																	<asp:dropdownlist id=DDLTipoRicerca Runat="server" CssClass="FiltroCampoSmall">
																		<asp:ListItem Value=-2 Selected="true">Nome inizia per</asp:ListItem>
																		<asp:ListItem Value=-7>Nome contiene</asp:ListItem>
																		<asp:ListItem Value=-9>Del responsabile</asp:ListItem>
																	</asp:dropdownlist>
																</td>
																<td>&nbsp;</td>
																<td>
																	<asp:Label ID="LBvalore_c" runat="server" CssClass="FiltroVoceSmall" Visible=true >Valore</asp:Label>&nbsp;
																	<asp:textbox id=TXBValore Runat="server" CssClass="FiltroCampoSmall" MaxLength=100 Columns=30></asp:textbox>
																	<asp:DropDownList ID="DDLresponsabile" runat="server" AutoPostBack=True CssClass="FiltroCampoSmall" Visible=False></asp:DropDownList>
																</td>
															</tr>
														</table>
													</asp:TableCell>
												</asp:TableRow>
												<asp:TableRow runat="server" ID="TBRorgnCorsi">
													<asp:TableCell ID="TBCorganizzazione0" ColumnSpan=2 >
														<table cellspacing=0 cellpadding=0>
															<tr>
																<td>
																	<asp:Label ID="LBorganizzazione_c" runat="server" CssClass="FiltroVoceSmall">Organizzazione:&nbsp;</asp:Label>
																</td>
																<td>
																	<asp:DropDownList ID="DDLorganizzazione" runat="server" AutoPostBack=True CssClass="FiltroCampoSmall"></asp:DropDownList>		
																</td>
																<td>&nbsp;</td>
																<td>
																	<asp:Table ID="TBLcorsi" CellPadding=2 CellSpacing=2 BorderStyle=None runat="server" Visible=False >
																		<asp:TableRow>
																			<asp:TableCell>
																				<asp:Label ID="LBannoAccademico_c" runat="server" CssClass="FiltroVoceSmall">A.A.:&nbsp;</asp:Label>
																			</asp:TableCell>
																			<asp:TableCell>
																				<asp:DropDownList ID="DDLannoAccademico" runat="server" AutoPostBack=True CssClass="FiltroCampoSmall"></asp:DropDownList>
																			</asp:TableCell>
																			<asp:TableCell>
																				<asp:Label ID="LBperiodo_c" runat="server" CssClass="FiltroVoceSmall">Periodo:&nbsp;</asp:Label>
																			</asp:TableCell>
																			<asp:TableCell>
																				<asp:DropDownList ID="DDLperiodo" runat="server" AutoPostBack=True CssClass="FiltroCampoSmall"></asp:DropDownList>
																			</asp:TableCell>
																		</asp:TableRow>
																	</asp:Table>
																	<asp:Table ID="TBLcorsiDiStudio" CellPadding=2 CellSpacing=2 BorderStyle=None runat="server" Visible=False >
																		<asp:TableRow>
																			<asp:TableCell>
																				<asp:Label ID="LBcorsoDiStudi_t" runat="server" CssClass="FiltroVoceSmall">A.A.:&nbsp;</asp:Label>
																			</asp:TableCell>
																			<asp:TableCell>
																				<asp:DropDownList ID="DDLtipoCorsoDiStudi" runat="server" AutoPostBack=True CssClass="FiltroCampoSmall"></asp:DropDownList>
																			</asp:TableCell>
																		</asp:TableRow>
																	</asp:Table>
																	<asp:Label ID="LBnoCorsi" runat="server" >&nbsp;</asp:Label>
																</td>
															</tr>
														</table>
													</asp:TableCell>
												</asp:TableRow>
												<asp:TableRow>
													<asp:TableCell>
														<asp:Label ID="LBstatoComunita_t" Runat="server" CssClass="FiltroVoceSmall">Stato:</asp:Label>&nbsp;
														<asp:radiobuttonlist ID="RBLstatoComunita" runat="server" CssClass="FiltroCampoSmall" AutoPostBack=True RepeatDirection=Horizontal RepeatLayout=Flow >
															<asp:ListItem Value=-1>Tutte</asp:ListItem>
															<asp:ListItem Value=0 Selected=true>Attivate</asp:ListItem>
															<asp:ListItem Value=1>Archiviate</asp:ListItem>
															<asp:ListItem Value=2>Bloccate</asp:ListItem>
														</asp:radiobuttonlist>
													</asp:TableCell>
													<asp:TableCell HorizontalAlign=Right >
														<asp:CheckBox ID="CBXautoUpdate" runat="server" Checked=true AutoPostBack=True CssClass="FiltroCampoSmall" Text="Aggiornamento automatico"></asp:CheckBox>
														&nbsp;&nbsp;
														<asp:button id=BTNCerca Runat="server" CssClass="PulsanteFiltro" Text="Cerca"></asp:button>
													</asp:TableCell>
												</asp:TableRow>
												<asp:TableRow>
													<asp:TableCell ColumnSpan=2 Height=15px>
														&nbsp;
													</asp:TableCell>
												</asp:TableRow>
											</asp:Table>
										</asp:TableCell>			
									</asp:TableRow>
									<asp:TableRow Visible="true" >
										<asp:TableCell ColumnSpan="2" Width="850px" HorizontalAlign="Center" ID="TBCletters" runat="server" >
											<table cellpadding=0 cellspacing=0 align=center Width=900px border=0>
												<tr>
													<td>
														<table align="left" width=400px>
															<tr>
																<td align="center" nowrap="nowrap" >
																	<asp:linkbutton id="LKBtutti" Runat="server" CssClass="lettera" CommandArgument="-1" OnClick="FiltroLinkLettere_Click">Tutti</asp:linkbutton></td>
																<td align="center" nowrap="nowrap">
																	<asp:linkbutton id="LKBaltro" Runat="server" CssClass="lettera" CommandArgument="0" OnClick="FiltroLinkLettere_Click">Altro</asp:linkbutton></td>
																<td align="center">
																	<asp:linkbutton id="LKBa" Runat="server" CssClass="lettera" CommandArgument="1" OnClick="FiltroLinkLettere_Click">A</asp:linkbutton></td>
																<td align="center">
																	<asp:linkbutton id="LKBb" Runat="server" CssClass="lettera" CommandArgument="2" OnClick="FiltroLinkLettere_Click">B</asp:linkbutton></td>
																<td align="center">
																	<asp:linkbutton id="LKBc" Runat="server" CssClass="lettera" CommandArgument="3" OnClick="FiltroLinkLettere_Click">C</asp:linkbutton></td>
																<td align="center">
																	<asp:linkbutton id="LKBd" Runat="server" CssClass="lettera" CommandArgument="4" OnClick="FiltroLinkLettere_Click">D</asp:linkbutton></td>
																<td align="center">
																	<asp:linkbutton id="LKBe" Runat="server" CssClass="lettera" CommandArgument="5" OnClick="FiltroLinkLettere_Click">E</asp:linkbutton></td>
																<td align="center">
																	<asp:linkbutton id="LKBf" Runat="server" CssClass="lettera" CommandArgument="6" OnClick="FiltroLinkLettere_Click">F</asp:linkbutton></td>
																<td align="center">
																	<asp:linkbutton id="LKBg" Runat="server" CssClass="lettera" CommandArgument="7" OnClick="FiltroLinkLettere_Click">G</asp:linkbutton></td>
																<td align="center">
																	<asp:linkbutton id="LKBh" Runat="server" CssClass="lettera" CommandArgument="8" OnClick="FiltroLinkLettere_Click">H</asp:linkbutton></td>
																<td align="center">
																	<asp:linkbutton id="LKBi" Runat="server" CssClass="lettera" CommandArgument="9" OnClick="FiltroLinkLettere_Click">I</asp:linkbutton></td>
																<td align="center">
																	<asp:linkbutton id="LKBj" Runat="server" CssClass="lettera" CommandArgument="10" OnClick="FiltroLinkLettere_Click">J</asp:linkbutton></td>
																<td align="center">
																	<asp:linkbutton id="LKBk" Runat="server" CssClass="lettera" CommandArgument="11" OnClick="FiltroLinkLettere_Click">K</asp:linkbutton></td>
																<td align="center">
																	<asp:linkbutton id="LKBl" Runat="server" CssClass="lettera" CommandArgument="12" OnClick="FiltroLinkLettere_Click">L</asp:linkbutton></td>
																<td align="center">
																	<asp:linkbutton id="LKBm" Runat="server" CssClass="lettera" CommandArgument="13" OnClick="FiltroLinkLettere_Click">M</asp:linkbutton></td>
																<td align="center">
																	<asp:linkbutton id="LKBn" Runat="server" CssClass="lettera" CommandArgument="14" OnClick="FiltroLinkLettere_Click">N</asp:linkbutton></td>
																<td align="center">
																	<asp:linkbutton id="LKBo" Runat="server" CssClass="lettera" CommandArgument="15" OnClick="FiltroLinkLettere_Click">O</asp:linkbutton></td>
																<td align="center">
																	<asp:linkbutton id="LKBp" Runat="server" CssClass="lettera" CommandArgument="16" OnClick="FiltroLinkLettere_Click">P</asp:linkbutton></td>
																<td align="center">
																	<asp:linkbutton id="LKBq" Runat="server" CssClass="lettera" CommandArgument="17" OnClick="FiltroLinkLettere_Click">Q</asp:linkbutton></td>
																<td align="center">
																	<asp:linkbutton id="LKBr" Runat="server" CssClass="lettera" CommandArgument="18" OnClick="FiltroLinkLettere_Click">R</asp:linkbutton></td>
																<td align="center">
																	<asp:linkbutton id="LKBs" Runat="server" CssClass="lettera" CommandArgument="19" OnClick="FiltroLinkLettere_Click">S</asp:linkbutton></td>
																<td align="center">
																	<asp:linkbutton id="LKBt" Runat="server" CssClass="lettera" CommandArgument="20" OnClick="FiltroLinkLettere_Click">T</asp:linkbutton></td>
																<td align="center">
																	<asp:linkbutton id="LKBu" Runat="server" CssClass="lettera" CommandArgument="21" OnClick="FiltroLinkLettere_Click">U</asp:linkbutton></td>
																<td align="center">
																	<asp:linkbutton id="LKBv" Runat="server" CssClass="lettera" CommandArgument="22" OnClick="FiltroLinkLettere_Click">V</asp:linkbutton></td>
																<td align="center">
																	<asp:linkbutton id="LKBw" Runat="server" CssClass="lettera" CommandArgument="23" OnClick="FiltroLinkLettere_Click">W</asp:linkbutton></td>
																<td align="center">
																	<asp:linkbutton id="LKBx" Runat="server" CssClass="lettera" CommandArgument="24" OnClick="FiltroLinkLettere_Click">X</asp:linkbutton></td>
																<td align="center">
																	<asp:linkbutton id="LKBy" Runat="server" CssClass="lettera" CommandArgument="25" OnClick="FiltroLinkLettere_Click">Y</asp:linkbutton></td>
																<td align="center">
																	<asp:linkbutton id="LKBz" Runat="server" CssClass="lettera" CommandArgument="26" OnClick="FiltroLinkLettere_Click">Z</asp:linkbutton></td>
															</tr>
														</table>
													</td>
													<td align=right >
														<asp:CheckBox CssClass="FiltroCampoSmall" ID="CBXmostraPadre" runat="server" Checked=False Text=" Mostra comunità di appartenenza." TextAlign=Right AutoPostBack=True ></asp:CheckBox>
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
										<asp:TableCell horizontalAlign="center" ColumnSpan="2">
											<asp:datagrid 
												id="DGComunita" runat="server" 
												PageSize="30" 
												DataKeyField="CMNT_id" 
												AllowPaging="true" 
												AutoGenerateColumns="False" 
												AllowSorting="true" 
												ShowFooter="false" 
												CssClass="DataGrid_Generica">
												<AlternatingItemStyle CssClass="ROW_Alternate_Small"></AlternatingItemStyle>
												<HeaderStyle CssClass="ROW_header_Small_Center"></HeaderStyle>
												<ItemStyle CssClass="ROW_Normal_Small" Height="22px"></ItemStyle>
												<PagerStyle CssClass="ROW_Page_Small" Position=TopAndBottom Mode="NumericPages" Visible="true"
												HorizontalAlign="Right" Height="18px" VerticalAlign="Bottom"></PagerStyle>
												<Columns>
													<asp:TemplateColumn runat="server" HeaderText="Tipo" ItemStyle-Width="50" SortExpression="TPCM_Descrizione" ItemStyle-HorizontalAlign="Center">
														<ItemTemplate>
															<img runat="server" src='<%# DataBinder.Eval(Container.DataItem, "TPCM_icona") %>' alt='<%# DataBinder.Eval(Container.DataItem, "TPCM_descrizione") %>' ID="Img2"/>
														</ItemTemplate>
													</asp:TemplateColumn>
													<asp:TemplateColumn  HeaderText="Nome" SortExpression="CMNT_Nome">
														<ItemTemplate>
															<asp:Table ID="TBLnome" runat="server" HorizontalAlign=Left>
																<asp:TableRow ID="TBRnome"  runat="server">
																	<asp:TableCell Width="5px">&nbsp;</asp:TableCell>
																	<asp:TableCell ID="TBCchiusa"  runat="server" CssClass="top">
																		<asp:Image ID="IMGisChiusa" runat="server" Visible=False BorderStyle=None ></asp:Image>
																	</asp:TableCell>
																	<asp:TableCell ID="TBCnome"  runat="server">
																		<asp:Label ID="LBcomunitaNome" runat="server">
																			<%# DataBinder.Eval(Container.DataItem, "CMNT_Esteso") %>
																		</asp:Label>
																						
																		(<b><asp:LinkButton ID="LNBlogin" runat="server" Commandname="Login" CausesValidation=False>Entra</asp:LinkButton></b>
																		|
																		<asp:LinkButton ID="LNBdettagli" runat="server" Commandname="dettagli" CausesValidation=False>Dettagli</asp:LinkButton>
																		<asp:Label ID="LBseparatorNews" runat="server" Visible=False>|</asp:Label>
																		<asp:Literal ID="LThasnews" runat="server" Visible="false"></asp:Literal>
																		)
																	</asp:TableCell>
																</asp:TableRow>
															</asp:Table>															
														</ItemTemplate>
													</asp:TemplateColumn>
													<asp:ButtonColumn Text="Mostra" Commandname="dettagli" HeaderText="Dettagli" ItemStyle-Width="60" Visible=False headerStyle-CssClass=ROW_Header_Small_center ItemStyle-CssClass=ROW_TD_Small></asp:ButtonColumn>
													<asp:BoundColumn DataField="AnnoAccademico" HeaderText="A.A." Visible="false" headerStyle-CssClass=ROW_Header_Small_center ItemStyle-CssClass=ROW_TD_Small></asp:BoundColumn>
													<asp:BoundColumn DataField="Periodo" HeaderText="Periodo" Visible="false" SortExpression="Periodo" ItemStyle-CssClass=ROW_TD_Small></asp:BoundColumn>
													<asp:BoundColumn DataField="TPRL_nome" HeaderText="Ruolo" ItemStyle-Width="100" headerStyle-CssClass=ROW_Header_Small_center ItemStyle-CssClass=ROW_TD_Small></asp:BoundColumn>
													<asp:BoundColumn DataField="RLPC_attivato" Visible="false"></asp:BoundColumn>
													<asp:BoundColumn DataField="RLPC_abilitato" Visible="false"></asp:BoundColumn>
													<asp:TemplateColumn runat="server" HeaderText="Last Visit" ItemStyle-Width="100px" Visible="true" SortExpression="RLPC_UltimoCollegamento" ItemStyle-CssClass=ROW_TD_Small_Center>
														<ItemTemplate>
															<asp:label Runat="server" ID="LBultimocollegamento">
																<%# DataBinder.Eval(Container.DataItem, "RLPC_UltimoCollegamentoStringa") %>
															</asp:label>
														</ItemTemplate>
													</asp:TemplateColumn>
													<asp:BoundColumn DataField="ALCM_Path" Visible="False"></asp:BoundColumn>
													<asp:BoundColumn DataField="CMNT_Responsabile" Visible="False"></asp:BoundColumn>
													<asp:BoundColumn DataField="TPCM_icona" Visible="false"></asp:BoundColumn>
													<asp:BoundColumn DataField="CMNT_EstesoNoSpan" Visible="false"></asp:BoundColumn>
													<asp:TemplateColumn runat="server" ItemStyle-Width="30" Visible="true" ItemStyle-HorizontalAlign="Center" ItemStyle-CssClass=ROW_TD_Small>
														<ItemTemplate>
															<asp:ImageButton ID="IMBdeiscrivi" ImageUrl="./../images/x.gif" Commandname="deIscrivi" ImageAlign=AbsMiddle Visible=True runat="server" ></asp:ImageButton>
														</ItemTemplate>
													</asp:TemplateColumn>
													<asp:BoundColumn DataField="CMNT_id" Visible="false"></asp:BoundColumn>
													<asp:BoundColumn DataField="CMNT_TPCM_id" Visible="false"></asp:BoundColumn>
													<asp:BoundColumn DataField="HasNews" Visible="false"></asp:BoundColumn>
													<asp:BoundColumn DataField="CMNT_isChiusa" Visible="false"></asp:BoundColumn>
													<asp:BoundColumn DataField="CMNT_CanSubscribe" Visible="false"></asp:BoundColumn>
													<asp:BoundColumn DataField="CMNT_CanUnsubscribe" Visible="false"></asp:BoundColumn>
													<asp:BoundColumn DataField="CMNT_Archiviata" Visible="false"></asp:BoundColumn>
													<asp:BoundColumn DataField="CMNT_Bloccata" Visible="false"></asp:BoundColumn>
													<asp:BoundColumn DataField="RLPC_TPRL_id" Visible="false"></asp:BoundColumn>
												</Columns>
											</asp:datagrid>
										<br/>
										<asp:Label id=LBmsgDG Runat="server" CssClass="avviso_normal" Visible="False"></asp:Label>
										</asp:TableCell>
									</asp:TableRow>
								</asp:Table>
							</td>
						</tr>
					</table>	
				</asp:panel>
				<asp:panel id="PNLdettagli" Runat="server" HorizontalAlign="Center" Visible="false" width="900px">
					<table width=500 align=center border=0>
						<tr>
							<td align=center colspan=2>
								<FIELDSET><LEGEND class=tableLegend>
								<asp:Label ID="LBlegend" runat="server" CssClass="tableLegend">Dettagli comunità</asp:Label>
									</LEGEND>
								<input type="hidden" runat="server" id="HDNcmnt_ID"/>
								<input type="hidden" runat="server" id="HDNcmnt_Path"/>
								<DETTAGLI:CTRLDettagli id="CTRLDettagli" runat="server"></DETTAGLI:CTRLDettagli>	
													
							</FIELDSET> 
							</td>
						</tr>
					</table>
				</asp:panel>
				<asp:panel id="PNLmessaggi" Runat="server" Visible="False" width="900px">
					<table cellSpacing=0 cellPadding=0 align=center border=0>
						<tr>
							<td height=30>&nbsp;</td>
						</tr>
						<tr>
							<td>
							<asp:Label id=LBMessaggi Runat="server" CssClass="avviso12"></asp:Label>
							</td>
						</tr>
						<tr>
							<td height=30>&nbsp;</td>
						</tr>
					</table>
				</asp:panel>
				<asp:Panel ID="PNLdeiscrivi" Visible="False" runat="server" width="900px">
					<table cellSpacing=0 cellPadding=0 align=center border=0 width=750px>
						<tr >
							<td height=50 colspan=3>&nbsp;</td>
						</tr>
						<tr>
							<td colspan="3">
								<table style="border-color:#CCCCCC; background-color:#FFFBF7" border="1" cellspacing="0" cellpadding="2">
									<tr>
										<td>
											<table>
												<tr>
													<td>
														<asp:Label id="LBinfoDeIscrivi" Runat="server" CssClass="confirmDeleteComunita"></asp:Label>
													</td>
												</tr>
												<tr>
													<td>&nbsp;</td>
												</tr>
												<tr>
													<td align=right>
														<asp:linkbutton ID="LNBdeIscriviCorrente" Runat="server" Text="Dalla principale" CausesValidation="false" CssClass="LINK_MENU"></asp:linkbutton>
														<asp:linkbutton ID="LNBdeIscriviSelezionate" Runat="server" Text="Dalle selezionate" CausesValidation="false" CssClass="LINK_MENU"></asp:linkbutton>
														<asp:linkbutton ID="LNBdeIscriviDaTutte" Runat="server" Text="Da tutte" CausesValidation="false" CssClass="LINK_MENU"></asp:linkbutton>
													</td>
												</tr>
											</table>
															
										</td>
									</tr>
								</table>
								<radt:RadTreeView id="RDTcomunita" runat="server" align="left" width="750px" CausesValidation="False" 
									CssFile="~/RadControls/TreeView/Skins/Comunita/stylesDelete.css" PathToJavaScript="~/Jscript/RadTreeView_Client_3_1.js" ImagesBaseDir="~/RadControls/TreeView/Skins/Comunita/" >
								</radt:RadTreeView>
							</td>
						</tr>
						<tr>
							<td height=30 colspan=3>&nbsp;</td>
						</tr>
					</table>
				</asp:Panel>
			</td>
		</tr>
	</table>

    <input type=hidden id="HDNazione" runat="server"/>

</asp:Content>
<%--
<html>
  <head id="HEAD1" runat="server">

		<title>Comunità On Line</title>

		<meta content="Microsoft Visual Studio.NET 7.0" name="GENERATOR"/>
		<meta content="Visual Basic 7.0" name="CODE_LANGUAGE"/>
		<meta content="JavaScript" name="vs_defaultClientScript"/>
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema"/>
			
		<link href="./../Styles.css" type="text/css" rel="stylesheet"/>
	
</head>

		
<body onkeydown="return SubmitRicerca(event);">
	<form id="aspnetForm" method="post" runat="server">
	<asp:ScriptManager ID="SCMmanager" runat="server"></asp:ScriptManager>
		<table cellspacing="0" cellpadding="0"  align="center" border="0" width="900px">
			<tr>
				<td colspan="3" >
				<div>
				    <HEADER:CtrLHEADER id="Intestazione" runat="server" ShowNews="true" HeaderNewsMemoHeight="130px"></HEADER:CtrLHEADER>	
				</div>
				<br style="clear:both;" />
				</td>
			</tr>
			<tr>
				<td colspan="3">

				</td>
			</tr>
			<tr>
				<td colSpan="3"></td>
			</tr>
		</table>
		<FOOTER:CtrLFOOTER id="CtrLFooter" runat="server"></FOOTER:CtrLFOOTER>
		
	</form>
</body>
</HTML>--%>
