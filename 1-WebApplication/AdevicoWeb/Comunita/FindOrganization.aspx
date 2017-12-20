<%@ Page Language="vb" AutoEventWireup="false" MasterPageFile="~/AjaxPortal.Master" CodeBehind="FindOrganization.aspx.vb" Inherits="Comunita_OnLine.FindOrganization" %>
<%@ MasterType VirtualPath="~/AjaxPortal.Master" %>

<%--<%@ Register TagPrefix="FOOTER" Tagname="CtrLFooter" Src="../UC/UC_Footer.ascx" %>
<%@ Register TagPrefix="HEADER" Tagname="CtrLHeader" Src="../UC/UC_Header.ascx" %>--%>
<%@ Register TagPrefix="DETTAGLI" Tagname="CTRLDettagli" Src="../UC/UC_dettaglicomunita.ascx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">

	<script language="Javascript" src="./../jscript/generali.js" type="text/javascript"></script>

    <script language="javascript" type="text/javascript">
        
        <%-- = Me.BodyId() % >.onkeydown = return SubmitRicerca(event); --%>

        function ChiudiMi() {
            this.window.close();
        }

        <%-- 
        function SubmitRicerca(event) {
            if (document.all) {
                if (event.keyCode == 13) {
                    event.returnValue = false;
                    event.cancel = true;
                    try {
                        document.forms[0].BTNCerca.click();
                    }
                    catch (ex) {
                        return false;
                    }
                }
            }
            else if (document.getElementById) {
                if (event.which == 13) {
                    event.returnValue = false;
                    event.cancel = true;
                    try {
                        document.forms[0].BTNCerca.click();
                    }
                    catch (ex) {
                        return false;
                    }
                }
            }
            else if (document.layers) {
                if (event.which == 13) {
                    event.returnValue = false;
                    event.cancel = true;
                    try {
                        document.forms[0].BTNCerca.click();
                    }
                    catch (ex) {
                        return false;
                    }
                }
            }
            else
                return true;
        }
        --%>

        function SelectMe(Me) {
            var HIDcheckbox, selezionati;
            //eval('HIDcheckbox= this.document.forms[0].HDNcomunitaSelezionate')
            //eval('HIDcheckbox=<%=Me.HDNcomunitaSelezionate.ClientID%>')
            HIDcheckbox = this.document.getElementById('<%=Me.HDNcomunitaSelezionate.ClientID%>');
            selezionati = 0
            for (i = 0; i < document.forms[0].length; i++) {
                e = document.forms[0].elements[i];
                if (e.type == 'checkbox' && e.name.indexOf("CBcorso") != -1) {
                    if (e.checked == true) {
                        selezionati++
                        if (HIDcheckbox.value == "") {
                            HIDcheckbox.value = ',' + e.value + ','
                        }
                        else {
                            pos1 = HIDcheckbox.value.indexOf(',' + e.value + ',')
                            if (pos1 == -1)
                                HIDcheckbox.value = HIDcheckbox.value + e.value + ','
                        }
                    }
                    else {
                        valore = HIDcheckbox.value
                        pos1 = HIDcheckbox.value.indexOf(',' + e.value + ',')
                        if (pos1 != -1) {
                            stringa = ',' + e.value
                            HIDcheckbox.value = HIDcheckbox.value.substring(0, pos1)
                            HIDcheckbox.value = HIDcheckbox.value + valore.substring(pos1 + e.value.length + 1, valore.length)
                        }
                    }
                }
            }
            if (HIDcheckbox.value == ",")
                HIDcheckbox.value = ""
        }

        function SelectAll(SelectAllBox) {
            var actVar = SelectAllBox.checked;
            var TBcheckbox;
            //eval('HDNcomunitaSelezionate= this.document.forms[0].HDNcomunitaSelezionate')
            //eval('HDNcomunitaSelezionate=<%=Me.HDNcomunitaSelezionate.ClientID%>')
            HDNcomunitaSelezionate = this.document.getElementById('<%=Me.HDNcomunitaSelezionate.ClientID%>');
            HDNcomunitaSelezionate.value = ""
            for (i = 0; i < document.forms[0].length; i++) {
                e = document.forms[0].elements[i];
                if (e.type == 'checkbox' && e.name.indexOf("CBcorso") != -1) {
                    e.checked = actVar;
                    if (e.checked == true)
                        if (HDNcomunitaSelezionate.value == "")
                            HDNcomunitaSelezionate.value = ',' + e.value + ','
                        else
                            HDNcomunitaSelezionate.value = HDNcomunitaSelezionate.value + e.value + ','
                    }
                }
            }

            function HasComunitaSelezionate(conferma, Messaggio, MessaggioConferma) {
                var HIDcheckbox, selezionati;
                //eval('HIDcheckbox= this.document.forms[0].HDNcomunitaSelezionate')
                //eval('HIDcheckbox=<%=Me.HDNcomunitaSelezionate.ClientID%>')
                HIDcheckbox = this.document.getElementById('<%=Me.HDNcomunitaSelezionate.ClientID%>');
                if (HIDcheckbox.value == "," || HIDcheckbox.value == "") {
                    alert(Messaggio)
                    return false;
                }
                else {
                    if (conferma == true) {
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
							<asp:Panel ID="PNLmenu" Runat="server" HorizontalAlign=Right Visible=true>
								<asp:linkbutton ID="LNBiscriviMultipli" Enabled=False Runat="server"  CssClass="LINK_MENU" Text="Iscrivi ai selzionati"></asp:linkbutton>
							</asp:Panel>
							<asp:Panel ID="PNLmenuDettagli" Runat="server" HorizontalAlign=Right Visible=False >
								<asp:linkbutton ID="LNBannullaDettagli" Runat="server" Text="Torna all'elenco" CausesValidation="false" CssClass="LINK_MENU"></asp:linkbutton>
								<asp:linkbutton ID="LNBiscriviDettagli" Runat="server" CssClass="LINK_MENU" CausesValidation="True" text="Iscrivi"></asp:linkbutton>
							</asp:Panel>
							<asp:Panel ID="PNLmenuConferma" Runat="server" HorizontalAlign=Right Visible=False >
								<asp:linkbutton ID="LNBannullaConferma" Runat="server" Text="Torna all'elenco" CausesValidation="false" CssClass="LINK_MENU"></asp:linkbutton>
								<asp:linkbutton ID="LNBiscriviConferma" Runat="server" CssClass="LINK_MENU" CausesValidation="True" text="Iscrivi"></asp:linkbutton>
							</asp:Panel>
							<asp:Panel ID="PNLmenuIscritto" Runat="server" HorizontalAlign=Right Visible=False >
								<asp:linkbutton ID="LNBelencoIscritte" Runat="server" Text="Torna alle iscritte" CausesValidation="false" CssClass="LINK_MENU"></asp:linkbutton>
								<asp:linkbutton ID="LNBiscriviAltre" Runat="server" CssClass="LINK_MENU" CausesValidation="True" text="Altra iscrizione"></asp:linkbutton>
							</asp:Panel>
							<asp:Panel ID="PNLmenuAccesso" Runat="server" HorizontalAlign=Right Visible=False>
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
					<input type ="hidden" id="HDisChiusa" Runat="server" name="HDisChiusa"/>
					<table align="center">
						<tr>
							<td height="50">&nbsp;</td>
						</tr>
						<tr>
							<td align="left">
								<asp:Label id="LBconferma" CssClass="messaggioIscrizione" Runat="server">Conferma l'iscrizione alla comunità #nomeComunita# - #nomeResponsabile#</asp:Label>
								<asp:Label id="LBconfermaMultipla" CssClass="messaggioIscrizione" Runat="server" Visible=False></asp:Label>
							</td>
						</tr>
						<tr>
							<td height="50">&nbsp;</td>
						</tr>
					</table>
				</asp:Panel>
				<asp:panel id="PNLcontenuto" Runat="server" HorizontalAlign="Center">
					<input type ="hidden" Runat="server" id="HDNselezionato" name="HDNselezionato"/>
					<input type ="hidden" Runat="server" id="HDNcomunitaSelezionate" name="HDNcomunitaSelezionate"/>
					<input type ="hidden" Runat="server" id="HDN_filtroFacolta" name="HDN_filtroFacolta"/>
					<input type ="hidden" Runat="server" id="HDN_filtroValore" name="HDN_filtroValore"/>
					<input type ="hidden" Runat="server" id="HDN_filtroResponsabileID" name="HDN_filtroResponsabileID"/>
					<asp:Table id="TBLfiltroNew" Runat="server"  Width="900px" cellpadding="0" cellspacing="0">
						<asp:TableRow id="TBRchiudiFiltro" Height=22px>
							<asp:TableCell CssClass="Filtro_CellSelezionato" Horizontalalign="center" Width=150px Height=22px VerticalAlign=Middle >
								&nbsp;
							</asp:TableCell>
							<asp:TableCell CssClass="Filtro_CellDeSelezionato" Width=750px Height=22px>&nbsp;
							</asp:TableCell>
						</asp:TableRow>

						<asp:TableRow ID="TBRfiltri">
							<asp:TableCell CssClass="Filtro_CellFiltri" ColumnSpan=2 Width=900px Horizontalalign="center">
								<asp:Table Runat="server" ID="TBLfiltro" CellPadding="1" cellspacing="0" Width="900px" Horizontalalign="center">
									<asp:TableRow>
										<asp:TableCell CssClass="FiltroVoceSmall" ColumnSpan="3">
											<table cellspacing="0" border="0" align="left" >
												<tr>
													<td height="30px">&nbsp;</td>
													<td nowrap="nowrap" height="30px">
														<asp:Label ID="LBnomeComunita_t" Runat="server" CssClass="FiltroVoceSmall">Nome comunità:</asp:Label>&nbsp;
														<asp:textbox id="TXBValore" Runat="server" CssClass="FiltroCampoSmall" MaxLength="100" Columns="30"></asp:textbox>
													</td>
													<td height="30px">&nbsp;&nbsp;</td>
													<td align="right" style=" width:150px">
														<asp:button id="BTNCerca" Runat="server" CssClass="PulsanteFiltro" Text="Cerca"></asp:button>
													</td>
												</tr>
											</table>
										</asp:TableCell>
									</asp:TableRow>
								</asp:Table>
							</asp:TableCell>
						</asp:TableRow>
						<asp:TableRow Visible=true ID="TBRfiltriGenerici">
							<asp:TableCell ColumnSpan=2 Width=900px Horizontalalign="center">
								<table cellpadding="0" cellspacing="0" align="center" Width=100% border="0">
									<tr>
										<td align="right">
											<asp:label ID="LBnumeroRecord_c" Runat ="server" cssclass="Filtro_TestoPaginazione">N° Record</asp:label>
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
												<img Runat="server" src='<%# DataBinder.Eval(Container.DataItem, "TPCM_icona") %>' alt='<%# DataBinder.Eval(Container.DataItem, "TPCM_descrizione") %>' ID="Img2"/>
											</ItemTemplate>
										</asp:TemplateColumn>
										<asp:TemplateColumn  HeaderText="Nome" SortExpression="CMNT_Nome">
											<ItemTemplate>
												<asp:Table ID="TBLnome" Runat="server" Horizontalalign="left">
													<asp:TableRow ID="TBRnome"  Runat="server">
														<asp:TableCell>
															&nbsp;
														</asp:TableCell>
														<asp:TableCell ID="TBCchiusa"  Runat="server">
															<asp:Image ID="IMGisChiusa" Runat="server" Visible=False BorderStyle=None ></asp:Image>
														</asp:TableCell>
														<asp:TableCell ID="TBCnome"  Runat="server">
															<asp:Label ID="LBcomunitaNome" Runat="server">
																<%# DataBinder.Eval(Container.DataItem, "CMNT_Esteso") %>
															</asp:Label>
																				
															(<b>
															<asp:LinkButton ID="LNBiscrivi" Runat="server" Commandname="Iscrivi" Visible=False CausesValidation=False >Iscrivi</asp:LinkButton></b>
															|
															<asp:LinkButton ID="LNBdettagli" Runat="server" Commandname="dettagli" CausesValidation=False>Dettagli</asp:LinkButton>

															)
														</asp:TableCell>
													</asp:TableRow>
												</asp:Table>			
											</ItemTemplate>
										</asp:TemplateColumn>
										<asp:BoundColumn DataField="AnnoAccademico" HeaderText="A.A." Visible="false" SortExpression="CMNT_Anno" ItemStyle-CssClass=ROW_TD_Small_Center></asp:BoundColumn>
										<asp:BoundColumn DataField="Periodo" HeaderText="Periodo" Visible="false" SortExpression="CMNT_PRDO_descrizione" ItemStyle-CssClass=ROW_TD_Small></asp:BoundColumn>
										<asp:ButtonColumn Text="Mostra" Commandname="dettagli" HeaderText="Dettagli" ItemStyle-Width="60" Visible=False ></asp:ButtonColumn>
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
												<input type="checkbox" id="SelectAll2" onclick="SelectAll(this);" runat="server" name="SelectAll"/>
											</HeaderTemplate>
											<ItemTemplate>
												<input Runat="server"  type="checkbox" id="CBcorso" name="CBcorso"  onclick="SelectMe(this);"/>
											</ItemTemplate>
										</asp:TemplateColumn>
									</Columns>
								</asp:datagrid><br/>
								<asp:Label id="LBmessageFind" Runat="server" CssClass="avviso_normal" Visible="False"></asp:Label>
								</asp:TableCell>
						</asp:TableRow>
					</asp:Table>
				</asp:panel>
				<asp:panel id="PNLdettagli" Runat="server" HorizontalAlign="Center" Visible="false">
					<table width=700 align="center" border="0">
						<tr>
							<td align="center">
							<FIELDSET><LEGEND class=tableLegend>
								<asp:Label ID="LBlegenda" Runat="server" cssclass=tableLegend>Dettagli comunità</asp:Label>
								</LEGEND>
								<input type ="hidden" Runat="server" id="HDNcmnt_ID" name="HDNcmnt_ID"/>
								<input type ="hidden" Runat="server" id="HDNtprl_id" name="HDNtprl_id"/>
								<input type ="hidden" Runat="server" id="HDNcmnt_Path" name="HDNcmnt_Path"/>
								<input type ="hidden" Runat="server" id="HDNisChiusaForPadre" name="HDNisChiusaForPadre"/>
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
					<table cellspacing="0" cellPadding=0 align="center" border="0">
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
					<table cellspacing="0" cellPadding=0 align="center" border="0">
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
					<table align="center">
						<tr>
							<td height="50">&nbsp;</td>
						</tr>
						<tr>
							<td align="center">
								<asp:Label id="LBNopermessi" Runat="server" CssClass="messaggio"></asp:Label>
							</td>
						</tr>
						<tr>
							<td vAlign="top" height="50">&nbsp; </td>
						</tr>
					</table>
				</asp:panel>
			</td>
		</tr>
	</table>
</asp:Content>
<%--

<!DOCTYPE HTML PUBLIC "-//W3C//Dtd HTML 4.0 transitional//EN">
<html>
  <head id="Head1" runat="server">
		<title>Comunità On Line - Ricerca Comunità</title>

		<meta content="Microsoft Visual Studio.NET 7.0" name="GENERATOR"/>
		<meta content="Visual Basic 7.0" name="CODE_LANGUAGE"/>
		<meta content="JavaScript" name="vs_defaultClientScript"/>
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema"/>
		<link href="./../Styles.css" type="text/css" rel="stylesheet"/>
</head>

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
</html>--%>