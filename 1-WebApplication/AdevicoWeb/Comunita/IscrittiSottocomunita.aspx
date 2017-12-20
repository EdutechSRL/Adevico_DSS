<%@ Page Language="vb" AutoEventWireup="false" MasterPageFile="~/AjaxPortal.Master" Codebehind="IscrittiSottocomunita.aspx.vb" Inherits="Comunita_OnLine.IscrittiSottocomunita"%>
<%@ MasterType VirtualPath="~/AjaxPortal.Master" %>

<%--<%@ Register TagPrefix="FOOTER" TagName="CtrLFooter" Src="../UC/UC_Footer.ascx" %>
<%@ Register TagPrefix="HEADER" TagName="CtrLHeader" Src="../UC/UC_header.ascx" %>--%>
<%@ Register TagPrefix="CTRL" TagName="CTRLfiltroComunita" Src="./../UC/UC_FiltroComunitaGenerale.ascx" %>
<%@ Register TagPrefix="CTRL" TagName="CTRLfiltroUtenti" Src="./../UC/UC_FiltriUtente.ascx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <script language="Javascript" src="./../jscript/generali.js" type="text/javascript"></script>

    <script language="Javascript" type="text/javascript">
    	function SelectMe(Me) {
    	    var HIDcheckbox;
    	    //eval('HIDcheckbox= this.document.forms[0].HDazione')
    	    //eval('HIDcheckbox=<%=Me.HDazione.ClientID%>')
    	    HIDcheckbox = this.document.getElementById('<%=Me.HDazione.ClientID%>');

    	    for (i = 0; i < document.forms[0].length; i++) {
    	        e = document.forms[0].elements[i];
    	        if (e.type == 'checkbox' && e.name.indexOf("CBazione") != -1) {
    	            if (e.checked == true) {
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


    	function UserForCancella() {
    	    //if (document.forms[0].HDazione.value == "," || document.forms[0].HDazione.value == "")
    	    //eval('HDazione=<%=Me.HDazione.ClientID%>')
    	    HDazione = this.document.getElementById('<%=Me.HDazione.ClientID%>');
    	    if (HDazione.value == "," || HDazione.value == "")
    	        return false;
    	    else
    	        return confirm('Sei sicuro di cancellare l\'iscrizione degli utenti selezionati?');
    	}

    	//Indica se è stato selezionato almeni un utente !!
    	function UserSelezionati() {
    	    //if (document.forms[0].HDazione.value == "," || document.forms[0].HDazione.value == "")
    	    //eval('HDazione=<%=Me.HDazione.ClientID%>')
    	    HDazione = this.document.getElementById('<%=Me.HDazione.ClientID%>');
    	    if (HDazione.value == "," || HDazione.value == "")
    	        return false;
    	    else
    	        return true;
    	}

    	function SelectAll(SelectAllBox) {
    	    var actVar = SelectAllBox.checked;
    	    var TBcheckbox;
    	    //eval('HDazione= this.document.forms[0].HDazione')
    	    //eval('HDazione=<%=Me.HDazione.ClientID%>')
    	    HDazione = this.document.getElementById('<%=Me.HDazione.ClientID%>');
    	    HDazione.value = ""
    	    for (i = 0; i < document.forms[0].length; i++) {
    	        e = document.forms[0].elements[i];
    	        if (e.type == 'checkbox' && e.name.indexOf("CBazione") != -1) {
    	            e.checked = actVar;
    	            if (e.checked == true)
    	                if (HDazione.value == "")
    	                    HDazione.value = ',' + e.value + ','
    	                else
    	                    HDazione.value = HDazione.value + e.value + ','
    	            }
    	        }
    	    }
		
	</script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="CPHservice" runat="server">
	<table align="center" width="100%">
<%--		<tr>	
			<td class="RigaTitolo" align="left">
				<asp:Label id="LBtitolo" Runat="server">Gestione avanzata Iscritti</asp:Label>
			</td>
		</tr>--%>
		<tr>
			<td  align="Right">
				<asp:Panel ID="PNLmenu" Runat=server HorizontalAlign=Right Wrap=False>
					<asp:linkbutton id="LNBstampa" Runat="server" Text="Stampa" CssClass="LINK_MENU"></asp:linkbutton>
					<asp:linkbutton id="LNBexcel" Runat="server" Text="Esporta in Excel" CssClass="LINK_MENU"></asp:linkbutton>
					&nbsp;
					<asp:LinkButton ID="LNBfind" Runat="server" CssClass="LINK_MENU" CausesValidation=False></asp:LinkButton>
										
				</asp:Panel>
			</td>
		</tr>
	</table>
	<asp:Panel ID="PNLpermessi" Runat="server" Visible="False" HorizontalAlign="Center">
		<table align="center">
			<tr>
				<td height="50">&nbsp;</td>
			</tr>
			<tr>
				<td align="center">
					<asp:Label id="LBNopermessi" Runat="server" CssClass="messaggio"></asp:Label></td>
			</tr>
			<tr>
				<td vAlign="top" height="50">
					&nbsp;
				</td>
			</tr>
		</table>
	</asp:Panel>
	<asp:Panel ID="PNLcontenuto" Runat="server" HorizontalAlign="Center">
		<table align="left" width="100%" border="0">
			<tr>
				<td Class="top" width="250px">&nbsp;</td>
				<td align="center" colspan="2">
					<CTRL:CTRLfiltroComunita id="CTRLfiltroComunita" runat="server" width="800px" LarghezzaFinestraAlbero=700px ColonneNome="90"></CTRL:CTRLfiltroComunita>
				</td>
			</tr>
			<tr>
				<td Class="top" width="250px">
					<CTRL:CTRLfiltroUtenti id="CTRLfiltroUtenti" runat="server"></CTRL:CTRLfiltroUtenti>
				</td>
				<td Class="top">
					<INPUT id="HDazione" type="hidden" name="HDazione" runat="server"/>
					<table align="center" cellpadding="0" cellspacing="0" border="0">
						<tr>
							<td>
								<table cellSpacing="1" cellPadding="1" Width="800px" border="0" align="center">
									<tr>
										<td align="left">
											<table cellSpacing="1" cellPadding="1" width="500px" border="0" align="left">
												<tr>
													<td class="FiltroVoceSmall">
														<asp:Label ID="LBtipoRicerca_t" runat="server" CssClass="FiltroVoceSmall">Ricerca per:</asp:Label>
														&nbsp;
														<asp:DropDownList id="DDLtipoRicerca" Runat="server" CssClass="FiltroCampoSmall">
															<asp:ListItem Value="-1">tutti</asp:ListItem>
															<asp:ListItem Value="1">Nome</asp:ListItem>
															<asp:ListItem Value="2">Cognome</asp:ListItem>
															<asp:ListItem Value="4">Matricola</asp:ListItem>
															<asp:ListItem Value="5">Mail</asp:ListItem>
															<asp:ListItem Value="6">Codice Fiscale</asp:ListItem>
															<asp:ListItem Value="7">Login</asp:ListItem>
														</asp:DropDownList>
													</td>
													<td class="FiltroVoceSmall">
														<asp:Label ID="LBvalore_t" runat="server" CssClass="FiltroVoceSmall">Valore:</asp:Label>
														&nbsp;
														<asp:textbox id="TXBValore" CssClass="FiltroCampoSmall" Runat="server" MaxLength="50"></asp:textbox>
													</td>
												</tr>
												<tr>
													<td colspan="2" align="left">
														<table width="400px" align="left">
															<tr>
																<td align="center">
																	<asp:linkbutton id="LKBtutti" Runat="server" CssClass="lettera" CommandArgument="-1" OnClick="FiltroLinkLettere_Click">Tutti</asp:linkbutton></td>
																<td align="center">
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
												</tr>
											</table>
										</td>
									</tr>
									<tr>
										<td align="left" >
											<asp:datagrid 
												id="DGiscritti" 
												runat="server" 
												AllowSorting="true" 
												ShowFooter="false" 
												AutoGenerateColumns="False"
												AllowPaging="true" DataKeyField="RLPC_ID" 
												PageSize="20" AllowCustomPaging="True"
												CssClass="DataGrid_Generica">
												<AlternatingItemStyle CssClass="ROW_Alternate_Small"></AlternatingItemStyle>
													<HeaderStyle CssClass="ROW_header_Small_Center"></HeaderStyle>
													<ItemStyle CssClass="ROW_Normal_Small" Height="22px"></ItemStyle>
													<PagerStyle CssClass="ROW_Page_Small" Position=TopAndBottom Mode="NumericPages" Visible="true"
													HorizontalAlign="Right" Height="18px" VerticalAlign="Bottom"></PagerStyle>
												<Columns>
													<asp:TemplateColumn ItemStyle-Width="15px" ItemStyle-HorizontalAlign="Center" Visible="false">
														<HeaderTemplate>
															<input type="checkbox" id="SelectAll" name="SelectAll" onclick="SelectAll(this);" runat="server"/>
														</HeaderTemplate>
														<ItemTemplate>
															<input type="checkbox" value=<%# DataBinder.Eval(Container.DataItem, "PRSN_ID") %> id="CBazione" name="CBazione" <%# DataBinder.Eval(Container.DataItem, "oCheck") %> onclick="SelectMe(this);" <%# DataBinder.Eval(Container.DataItem, "oCheckDisabled") %>>
														</ItemTemplate>
													</asp:TemplateColumn>
													<asp:BoundColumn DataField="RLPC_ID" HeaderText="" Visible="false"></asp:BoundColumn>
													<asp:TemplateColumn runat="server" HeaderText="" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="40">
														<ItemTemplate>
															<asp:ImageButton id="IMBinfo" Runat="server" CausesValidation="False" CommandName="infoPersona" ImageUrl="../images/proprieta.gif"></asp:ImageButton>
															<asp:ImageButton id="IMBmodifica" Runat="server" CausesValidation="False" CommandName="modifica"
																ImageUrl="../images/m.gif"></asp:ImageButton>
															<asp:ImageButton id="IMBCancella" Runat="server" CausesValidation="False" CommandName="deiscrivi"
																ImageUrl="../images/x.gif"></asp:ImageButton>
														</ItemTemplate>
													</asp:TemplateColumn>
													<asp:TemplateColumn HeaderText="Matricola" ItemStyle-CssClass=ROW_TD_Small HeaderStyle-CssClass="ROW_header_Small_Center">
														<ItemTemplate>
															<table>
																<tr>
																	<td class="ROW_TD_Small">&nbsp;</td>
																	<td class="ROW_TD_Small"><%# DataBinder.Eval(Container.DataItem, "Matricola") %></td>
																</tr>
															</table>
														</ItemTemplate>
													</asp:TemplateColumn>
													<asp:TemplateColumn HeaderText="Cognome" SortExpression="PRSN_Cognome" ItemStyle-CssClass=ROW_TD_Small >
														<ItemTemplate>
															<table>
																<tr>
																	<td class="ROW_TD_Small">&nbsp;</td>
																	<td class="ROW_TD_Small"><%# DataBinder.Eval(Container.DataItem, "PRSN_Cognome") %></td>
																</tr>
															</table>
														</ItemTemplate>
													</asp:TemplateColumn>
													<asp:TemplateColumn HeaderText="Nome" SortExpression="PRSN_Nome" ItemStyle-CssClass=ROW_TD_Small>
														<ItemTemplate>
															<table>
																<tr>
																	<td class="ROW_TD_Small">&nbsp;</td>
																	<td class="ROW_TD_Small"><%# DataBinder.Eval(Container.DataItem, "PRSN_Nome") %></td>
																</tr>
															</table>
														</ItemTemplate>
													</asp:TemplateColumn>
													<asp:BoundColumn DataField="PRSN_Anagrafica" HeaderText="Anagrafica" Visible=false></asp:BoundColumn>
													<asp:BoundColumn DataField="PRSN_login" HeaderText="Login" SortExpression="PRSN_login" ItemStyle-CssClass=ROW_TD_Small visible=False ></asp:BoundColumn>
													<asp:TemplateColumn runat="server" HeaderText="Mail" ItemStyle-CssClass=ROW_TD_Small  ItemStyle-Width="10"
														SortExpression="PRSN_mail">
														<ItemTemplate>
															<asp:HyperLink NavigateUrl='<%# "mailto:" & Container.Dataitem("PRSN_mail")%>' text='<%# Container.Dataitem("PRSN_mail")%>' Runat="server" ID="HYPMail" />
														</ItemTemplate>
													</asp:TemplateColumn>
													<asp:BoundColumn DataField="TPRL_nome" HeaderText="Ruolo" SortExpression="TPRL_nome" ItemStyle-CssClass=ROW_TD_Small ></asp:BoundColumn>
													<asp:BoundColumn DataField="TPPR_descrizione" HeaderText="Tipo Persona" SortExpression="TPPR_descrizione"
														ItemStyle-CssClass=ROW_TD_Small  Visible="False"></asp:BoundColumn>
													<asp:BoundColumn DataField="RLPC_ultimoCollegamento" HeaderText="Last visit" SortExpression="RLPC_ultimoCollegamento"
														Visible="false" ItemStyle-Width="150" ItemStyle-CssClass=ROW_TD_Small ></asp:BoundColumn>
													<asp:BoundColumn DataField="oIscrittoIl" HeaderText="Iscritto il" SortExpression="RLPC_IscrittoIl"
														visible="true" ItemStyle-CssClass=ROW_TD_Small ItemStyle-Width=120px></asp:BoundColumn>
													<asp:BoundColumn DataField="PRSN_TPPR_id" HeaderText="idtipopersona" SortExpression="PRSN_TPPR_id"
														Visible="False"></asp:BoundColumn>
													<asp:BoundColumn DataField="TPRL_gerarchia" visible="false"></asp:BoundColumn>
													<asp:BoundColumn DataField="PRSN_ID" visible="false"></asp:BoundColumn>
													<asp:BoundColumn DataField="RLPC_Attivato" visible="false"></asp:BoundColumn>
													<asp:BoundColumn DataField="RLPC_Abilitato" visible="false"></asp:BoundColumn>
													<asp:BoundColumn DataField="RLPC_Responsabile" visible="false"></asp:BoundColumn>
													<asp:BoundColumn DataField="TPRL_id" HeaderText="idRuolo" SortExpression="TPRL_id" Visible="False"></asp:BoundColumn>
												</Columns>
												<PagerStyle Width="800px" PageButtonCount="5" mode="NumericPages"></PagerStyle>
											</asp:datagrid><br/>
											<asp:Label id="LBnoIscritti" Visible="False" Runat="server" CssClass="avviso"></asp:Label>
										</td>
									</tr>
								</table>
							</td>
						</tr>
					</table>
				</td>
			</tr>
		</table>
	</asp:Panel>

    <asp:Table ID="TBLexcel" Runat=server Visible=True ></asp:Table>
	<asp:datagrid 
		id="DGiscrittiBis" runat="server" 
		AllowSorting="true" 
		ShowFooter="false" 
		AutoGenerateColumns="False"
		AllowPaging="true" 
		DataKeyField="RLPC_ID" PageSize="20" 
		AllowCustomPaging="True" Visible="false"
		CssClass="DataGrid_Generica">
		<AlternatingItemStyle CssClass="Righe_Alternate_Center"></AlternatingItemStyle>
		<HeaderStyle CssClass="Riga_Header"></HeaderStyle>
		<ItemStyle CssClass="Righe_Normali_center" Height="22px"></ItemStyle>
		<PagerStyle CssClass="Riga_Paginazione" Position="Bottom" Mode="NumericPages" Visible="true"
			HorizontalAlign="Right" Height="25px" VerticalAlign="Bottom"></PagerStyle>
		<Columns>
			<asp:BoundColumn DataField="RLPC_ID" HeaderText="" Visible="false"></asp:BoundColumn>
			<asp:BoundColumn DataField="Matricola" HeaderText="Matricola"></asp:BoundColumn>
			<asp:BoundColumn DataField="PRSN_Anagrafica" HeaderText="Anagrafica" SortExpression="PRSN_Anagrafica"></asp:BoundColumn>
			<asp:BoundColumn DataField="PRSN_login" HeaderText="Login" SortExpression="PRSN_login" ItemStyle-HorizontalAlign="Center"></asp:BoundColumn>
			<asp:BoundColumn DataField="PRSN_mail" HeaderText="Mail" SortExpression="PRSN_mail" ItemStyle-HorizontalAlign="Center"></asp:BoundColumn>
			<asp:BoundColumn DataField="TPRL_nome" HeaderText="Ruolo" SortExpression="TPRL_nome" ItemStyle-HorizontalAlign="Center"></asp:BoundColumn>
			<asp:BoundColumn DataField="TPPR_descrizione" HeaderText="Tipo Persona" SortExpression="TPPR_descrizione"
				ItemStyle-HorizontalAlign="Center" Visible="False"></asp:BoundColumn>
			<asp:BoundColumn DataField="RLPC_ultimoCollegamento" HeaderText="Last visit" SortExpression="RLPC_ultimoCollegamento"
				Visible="false" ItemStyle-Width="150" ItemStyle-HorizontalAlign="Center"></asp:BoundColumn>
			<asp:BoundColumn DataField="oIscrittoIl" HeaderText="Iscritto il" SortExpression="RLPC_IscrittoIl"
				visible="true" ItemStyle-HorizontalAlign="Center"></asp:BoundColumn>
			<asp:BoundColumn DataField="PRSN_TPPR_id" HeaderText="idtipopersona" SortExpression="PRSN_TPPR_id"
				Visible="False"></asp:BoundColumn>
			<asp:BoundColumn DataField="TPRL_gerarchia" visible="false"></asp:BoundColumn>
			<asp:BoundColumn DataField="PRSN_ID" visible="false"></asp:BoundColumn>
			<asp:BoundColumn DataField="RLPC_Attivato" visible="false"></asp:BoundColumn>
			<asp:BoundColumn DataField="RLPC_Abilitato" visible="false"></asp:BoundColumn>
			<asp:BoundColumn DataField="RLPC_Responsabile" visible="false"></asp:BoundColumn>
			<asp:BoundColumn DataField="TPRL_id" HeaderText="idRuolo" SortExpression="TPRL_id" Visible="False"></asp:BoundColumn>
		</Columns>
		<PagerStyle Width="600px" PageButtonCount="5" mode="NumericPages"></PagerStyle>
	</asp:datagrid>
</asp:Content>

<%--
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<html>
	<head runat="server">
		<title>Comunità On Line - Elenco Iscritti Sottocomunità</title>
		<meta name="GENERATOR" content="Microsoft Visual Studio .NET 7.1"/>
		<meta name="CODE_LANGUAGE" content="Visual Basic .NET 7.1"/>
		<meta name="vs_defaultClientScript" content="JavaScript"/>
		<meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5"/>
		<LINK href="../Styles.css" type="text/css" rel="stylesheet"/>
		
	</head>

	<body >
		<form id="aspnetForm" method="post" runat="server">
		<asp:ScriptManager ID="SCMmanager" runat="server"></asp:ScriptManager>
			<table cellSpacing="0" cellPadding="0" width="780" align="center" border="0">
				<tr>
					<td colSpan="3">
						<HEADER:CtrLHEADER id="Intestazione" runat="server"></HEADER:CtrLHEADER>
					</td>
				</tr>
				<tr>
					<td colSpan="3" align="center">

					</td>
				</tr>
				<tr>
					<td colSpan="3">
						<FOOTER:CtrLFOOTER id="CtrLFooter" runat="server"></FOOTER:CtrLFOOTER>
					</td>
				</tr>
			</table>

		</form>
	</body>
</html>--%>
