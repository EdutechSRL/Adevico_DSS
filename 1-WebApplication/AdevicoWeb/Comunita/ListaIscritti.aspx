<%@ Page Language="vb" AutoEventWireup="false" MasterPageFile="~/AjaxPortal.Master" Codebehind="ListaIscritti.aspx.vb" Inherits="Comunita_OnLine.ListaIscritti"%>
<%@ MasterType VirtualPath="~/AjaxPortal.Master" %>

<%--<%@ Register TagPrefix="FOOTER" TagName="CtrLFooter" Src="../UC/UC_Footer.ascx" %>
<%@ Register TagPrefix="HEADER" TagName="CtrLHeader" Src="../UC/UC_Header.ascx" %>--%>


<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <script language="Javascript" src="./../jscript/generali.js" type="text/javascript"></script>

    <script language="javascript" type="text/javascript">
    	function Stampa() {
    	    OpenWin('./stampaiscritti.aspx?TPRL_id=' + <%=Me.DDLTipoRuolo.ClientId %>.value, '700', '600', 'yes', 'yes')
    	    return false;
    	}
        
       
	</script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="CPHservice" runat="server">
	<table cellSpacing="0" cellPadding="0" width="900px" border="0">
<%--		<tr>
			<td class="RigaTitolo" align="left">
				<asp:label id="LBtitolo" Runat="server">Lista Iscritti</asp:label>
			</td>
		</tr>--%>
		<tr>
			<td align=right height=26px class=top>
				<asp:Panel ID="PNLmenu" Runat=server HorizontalAlign=Right >
					<asp:LinkButton ID="LKBgestioneIscritti" Runat=server CssClass="LINK_MENU" CausesValidation=False>Vai a "Gestione Iscritti"</asp:LinkButton>
					<asp:LinkButton ID="LNBstampa" Runat=server CssClass="LINK_MENU" CausesValidation=False>Stampa Iscritti</asp:LinkButton>
				</asp:Panel>
			</td>
		</tr>
		<tr>
			<td vAlign="top" align="center">
				<asp:panel id="PNLpermessi" Runat="server" Visible="False">
					<table align="center">
						<tr>
							<td height="50">&nbsp;</td>
						</tr>
						<tr>
							<td align="center">
								<asp:Label id="LBNopermessi" CssClass="messaggio" Runat="server"></asp:Label></td>
						</tr>
						<tr>
							<td vAlign="top" height="50">
								<br/>
							</td>
						</tr>
					</table>
				</asp:panel>
				<asp:panel id="PNLcontenuto" Runat="server" HorizontalAlign="Center">
					<input type =hidden runat=server id="HDN_filtroRuolo" NAME="HDN_filtroRuolo"/>
					<input type =hidden runat=server id="HDN_filtroTipoRicerca" NAME="HDN_filtroTipoRicerca"/>
					<input type =hidden runat=server id="HDN_filtroValore" NAME="HDN_filtroValore"/>
					<input type =hidden runat=server id="HDN_filtroIscrizione" NAME="HDN_filtroIscrizione"/>
														
														
					<asp:Table id="TBLfiltroNew" Runat=server  Width="850px" CellPadding=0 CellSpacing=0 GridLines=none HorizontalAlign=center>
						<asp:TableRow id="TBRchiudiFiltro" Height=22px>
							<asp:TableCell CssClass="Filtro_CellSelezionato" HorizontalAlign=Center Width=150px Height=22px VerticalAlign=Middle >
								<asp:LinkButton ID="LNBchiudiFiltro" Runat=server CssClass="Filtro_Link" CausesValidation=False>Chiudi Filtri</asp:LinkButton>
							</asp:TableCell>
							<asp:TableCell CssClass="Filtro_CellDeSelezionato" Width=700px Height=22px>&nbsp;
							</asp:TableCell>
						</asp:TableRow>
						<asp:TableRow id="TBRapriFiltro" Visible=False Height=22px>
							<asp:TableCell CssClass="Filtro_CellApriFiltro" HorizontalAlign=Center Width=150px Height=22px>
								<asp:LinkButton ID="LNBapriFiltro" Runat=server CssClass="Filtro_Link" CausesValidation=False >Apri Filtri</asp:LinkButton>
							</asp:TableCell>
							<asp:TableCell CssClass="Filtro_Cellnull" Width=700px Height=22px>&nbsp;
							</asp:TableCell>
						</asp:TableRow>
						<asp:TableRow ID="TBRfiltri" >
							<asp:TableCell CssClass="Filtro_CellFiltri" ColumnSpan=2 HorizontalAlign=center>
								<table cellSpacing="2" cellPadding="2" width="850px" border="0">
									<tr>
										<td height=22px>&nbsp;</td>
										<td class="FiltroVoceSmall" height=22px>
											<asp:label ID="LBtipoRuolo" Runat =server cssclass="FiltroVoceSmall">Tipo Ruolo</asp:label>
											&nbsp;
											<asp:dropdownlist id="DDLTipoRuolo" runat="server" CssClass="FiltroCampoSmall" AutoPostBack="true"></asp:dropdownlist>
										</td>
										<td class="FiltroVoceSmall" height=22px>
											<asp:label ID="LBtipoRicerca" Runat =server cssclass="FiltroVoceSmall">Tipo Ricerca</asp:label>
											&nbsp;
											<asp:dropdownlist id="DDLTipoRicerca" CssClass="FiltroCampoSmall" Runat="server" AutoPostBack="false">
												<asp:ListItem  Selected="true" Value=-2>Nome</asp:ListItem>
												<asp:ListItem Value=-3>Cognome</asp:ListItem>
												<asp:ListItem value = -4>Nome/Cognome</asp:ListItem>
												<asp:ListItem value = -7>Login</asp:ListItem>
											</asp:dropdownlist>
										</td>
										<td class="FiltroVoceSmall" height=22px>
											<asp:label ID="LBvalore" Runat =server cssclass="FiltroVoceSmall">Valore</asp:label>
											&nbsp;
											<asp:textbox id="TXBValore" CssClass="FiltroCampoSmall" Runat="server" MaxLength ="50" Columns=30></asp:textbox>
										</td>															
										<td height=22px>&nbsp;</td>
									</tr>
									<tr>
										<td Height=8px class="nosize0">&nbsp;</td>
										<td colspan=2>
											<asp:Label ID="LBiscrizione_t" Runat="server" CssClass="FiltroVoceSmall">Visualizza:</asp:Label>&nbsp;
											<asp:dropdownlist ID="DDLiscrizione" Runat=server CssClass="FiltroCampoSmall" AutoPostBack=True>
												<asp:ListItem Value=4>Ultimi iscritti</asp:ListItem>
												<asp:ListItem Value=-1>Tutti</asp:ListItem>
												<asp:ListItem Value=1 Selected=true>Abilitati</asp:ListItem>
												<asp:ListItem Value=0>In attesa di conferma</asp:ListItem>
												<asp:ListItem Value=2>Bloccati</asp:ListItem>
											</asp:dropdownlist>
										</td>
										<td align=right>
											<asp:CheckBox ID="CBXautoUpdate" Runat=server Checked=true AutoPostBack=True CssClass="FiltroCampoSmall" Text="Aggiornamento automatico"></asp:CheckBox>
											&nbsp;&nbsp;
											<asp:button id="BTNCerca" CssClass="PulsanteFiltro" Runat="server" Text="Cerca"></asp:button>
										</td>
										<td Height=8px class="nosize0">&nbsp;</td>
									</tr>
									<tr>
										<td Height=8px class="nosize0">&nbsp;</td>
										<td colspan=3 Height=8px class="nosize0">&nbsp;</td>
										<td Height=8px class="nosize0">&nbsp;</td>
									</tr>
								</table>
							</asp:TableCell>
						</asp:TableRow>
						<asp:TableRow Visible=true >
							<asp:TableCell ColumnSpan=2 HorizontalAlign=Center>
								<table width=100%  border=0 cellpadding=0 cellspacing=0>
									<tr>
										<td>
											<table width="400px" align=left border=0 cellpadding=0 cellspacing=0>
												<tr>
														<td align="center" nowrap="nowrap"  >
															<asp:linkbutton id="LKBtutti" Runat="server" CssClass="lettera" CommandArgument="-1" OnClick="FiltroLinkLettere_Click">Tutti</asp:linkbutton></td>
														<td align="center" nowrap="nowrap">
															<asp:linkbutton id="LKBaltro" Runat="server" CssClass="lettera" CommandArgument="0" OnClick="FiltroLinkLettere_Click">Altro</asp:linkbutton></td>
														<td>&nbsp;</td>
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
											<asp:label ID="LBnumeroRecord" Runat =server cssclass="Filtro_TestoPaginazione">N° Record</asp:label>&nbsp;
											<asp:dropdownlist id="DDLNumeroRecord" CssClass="Filtro_RecordPaginazione" Runat="server" AutoPostBack="true">
												<asp:ListItem Value="25"></asp:ListItem>
												<asp:ListItem Value="50" Selected="true"></asp:ListItem>
												<asp:ListItem Value="75"></asp:ListItem>
												<asp:ListItem Value="100"></asp:ListItem>
												<asp:ListItem Value="150"></asp:ListItem>
											</asp:dropdownlist>
										</td>
									</tr>
								</table>
							</asp:TableCell>
						</asp:TableRow>
						<asp:TableRow>
							<asp:TableCell ColumnSpan=2>
								<INPUT id="HDazione" type="hidden" name="HDazione" runat="server"/>
								<asp:datagrid 
									id="DGiscritti" runat="server" 
									AllowSorting="true" 
									ShowFooter="false" 
									AutoGenerateColumns="False" 
									AllowPaging="true" DataKeyField="RLPC_ID" 
									PageSize ="50" AllowCustomPaging="True"
									CssClass="DataGrid_Generica">
														
									<AlternatingItemStyle CssClass="ROW_Alternate_Small"></AlternatingItemStyle>
									<HeaderStyle CssClass="ROW_header_Small_Center"></HeaderStyle>
									<ItemStyle CssClass="ROW_Normal_Small" Height="22px"></ItemStyle>
									<PagerStyle CssClass="ROW_Page_Small" Position="TopAndBottom"  Mode="NumericPages" Visible="true"
									HorizontalAlign="Right" Height="25px" VerticalAlign="Bottom"></PagerStyle>
									<Columns>
										<asp:BoundColumn DataField="RLPC_ID" HeaderText="RLPC" Visible="false"></asp:BoundColumn>
										<asp:TemplateColumn runat="server" HeaderText="" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="10">
											<ItemTemplate>
												<asp:ImageButton id="IMBinfo" Runat="server" CausesValidation="False" CommandName="infoPersona" ImageUrl="../images/proprieta.gif"></asp:ImageButton>
											</ItemTemplate>
										</asp:TemplateColumn>
										<asp:TemplateColumn ItemStyle-CssClass=ROW_TD_Small HeaderText="Cognome" SortExpression="PRSN_Cognome" ItemStyle-Width="140px">
											<ItemTemplate>
												&nbsp;<%#Container.Dataitem("PRSN_Cognome")%>
											</ItemTemplate>
										</asp:TemplateColumn>
										<asp:TemplateColumn ItemStyle-CssClass=ROW_TD_Small HeaderText="Nome" SortExpression="PRSN_Nome" ItemStyle-Width="140px">
											<ItemTemplate>
												&nbsp;<%#Container.Dataitem("PRSN_Nome")%>
											</ItemTemplate>
										</asp:TemplateColumn>
										<asp:BoundColumn DataField="PRSN_Anagrafica" HeaderText="Anagrafica" visible=False ></asp:BoundColumn>
										<asp:TemplateColumn ItemStyle-CssClass=ROW_TD_Small HeaderText="Login" SortExpression="PRSN_login" Visible=False >
											<ItemTemplate>
												&nbsp;<%#Container.Dataitem("PRSN_login")%>
											</ItemTemplate>
										</asp:TemplateColumn>
										<asp:TemplateColumn runat="server" HeaderText="Mail" ItemStyle-CssClass=ROW_TD_Small ItemStyle-Width="120px"
											SortExpression="PRSN_mail">
											<ItemTemplate>
												<table border=0 align=left >
													<tr>
														<td>&nbsp;</td>
														<td>
															<asp:HyperLink NavigateUrl='<%# "mailto:" & Container.Dataitem("PRSN_mail")%>' text='<%# server.HtmlEncode(replace(Container.Dataitem("PRSN_mail") ,"-","&ndash;"))%>' Runat="server" ID="HYPMail" CssClass="ROW_ItemLink_Small"  />		
															<asp:Label ID="LBnoMail" Visible=True Runat=server>--</asp:Label>
														</td>
													</tr>
												</table>																
											</ItemTemplate>
										</asp:TemplateColumn>
										<asp:BoundColumn DataField="TPRL_id" HeaderText="idRuolo" SortExpression="TPRL_id" Visible="False"></asp:BoundColumn>
										<asp:TemplateColumn runat="server" HeaderText="Ruolo" SortExpression="TPRL_nome" ItemStyle-CssClass=ROW_TD_Small ItemStyle-Width="140px">
											<ItemTemplate>
												&nbsp;<%# Container.Dataitem("TPRL_nome")%>
											</ItemTemplate>
										</asp:TemplateColumn>
										<asp:BoundColumn DataField="PRSN_TPPR_id" HeaderText="idtipopersona" SortExpression="PRSN_TPPR_id"  Visible="False"></asp:BoundColumn>
										<asp:BoundColumn DataField="oUltimoCollegamento" HeaderText="Last visit" SortExpression="RLPC_ultimoCollegamento" Visible="true" ItemStyle-Width="110PX" ItemStyle-CssClass=ROW_TD_Small_center></asp:BoundColumn>
										<asp:BoundColumn DataField="TPRL_gerarchia" visible=false></asp:BoundColumn>
										<asp:BoundColumn DataField="PRSN_ID" visible=false></asp:BoundColumn>
										<asp:BoundColumn DataField="RLPC_Attivato" visible=false></asp:BoundColumn>
										<asp:BoundColumn DataField="RLPC_Abilitato" visible=false></asp:BoundColumn>
										<asp:BoundColumn DataField="RLPC_Responsabile" visible=false></asp:BoundColumn>
										<asp:BoundColumn DataField="oIscrittoIl" HeaderText="Iscritto il" SortExpression="RLPC_IscrittoIl" visible=false ItemStyle-CssClass=ROW_TD_Small></asp:BoundColumn>
										<asp:BoundColumn DataField="RLPC_PRSN_mostraMail" visible=false></asp:BoundColumn>
										<asp:BoundColumn DataField="PRSN_mostraMail" visible=false></asp:BoundColumn>
									</Columns>
									<PagerStyle Width="600px" PageButtonCount="5" mode="NumericPages"></PagerStyle>
								</asp:datagrid>
								<br/><br/>
								<asp:Label id="LBnoIscritti" Visible="False" Runat="server" CssClass="avviso"></asp:Label>
								</asp:TableCell>
						</asp:TableRow>
					</asp:Table>
				</asp:Panel> 
			</td>
		</tr>
	</table>
</asp:Content>



<%--<!DOCTYPE HTML PUBLIC "-//W3C//Dtd HTML 4.0 transitional//EN">
<HTML>
	<head runat="server">
		<title>Comunità On Line - Lista Iscritti</title>
		
		<meta content="Microsoft Visual Studio.NET 7.0" name="GENERATOR"/>
		<meta content="Visual Basic 7.0" name="CODE_LANGUAGE"/>
		<meta content="JavaScript" name="vs_defaultClientScript"/>
		<LINK href="../Styles.css" type="text/css" rel="stylesheet"/>
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema"/>
		
	</HEAD>
    
	<body onkeydown="return SubmitRicerca(event);">
		<form id="aspnetForm" runat="server">
		<asp:ScriptManager ID="SCMmanager" runat="server"></asp:ScriptManager>
			<table cellSpacing="1" cellPadding="1" width="780" align="center" border="0" >
				<tr>
					<td colSpan="3"><HEADER:CTRLHEADER id="Intestazione" runat="server"></HEADER:CTRLHEADER></td>
				</tr>
				<tr class="contenitore">
					<td colSpan="3">

					</td>
				</tr>
				<tr>
					<td colSpan="3"></td>
				</tr>
			</table>
			<FOOTER:CTRLFOOTER id="Piede" runat="server"></FOOTER:CTRLFOOTER>
		</form>
	</body>
</HTML>--%>