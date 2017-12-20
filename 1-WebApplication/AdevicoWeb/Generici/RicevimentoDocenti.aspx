<%@ Page Language="vb" AutoEventWireup="false" MasterPageFile="~/AjaxPortal.Master" Codebehind="RicevimentoDocenti.aspx.vb" Inherits="Comunita_OnLine.RicevimentoDocenti"%>
<%--<%@ Register TagPrefix="HEADER" TagName="CtrLHeader" Src="../UC/UC_Header.ascx" %>
<%@ Register TagPrefix="FOOTER" TagName="CtrLFooter" Src="../UC/UC_Footer.ascx" %>--%>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
		<script language="Javascript" src="../jscript/generali.js" type="text/javascript"></script>
        <%-- 
        <script language="javascript" type="text/javascript">
        < %= Me.BodyId() % >.onkeydown = SubmitRicerca(event);

	    function SubmitRicerca(event) {
	        if (document.all) {
	            if (event.keyCode == 13) {
	                event.returnValue = false;
	                event.cancel = true;
	                try {
	                    document.forms[0].BTNcerca.click();
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
		</script> --%>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="CPHservice" runat="server">
			<table class="contenitore" cellSpacing="0" cellPadding="0" align="center">
            	<tr>
					<td class="RigaTitolo" align="left">
						<asp:Label ID="LBTitolo" Runat="server">Orari Ricevimento</asp:Label>
					</td>
				</tr>
				<tr>
					<td>
						&nbsp;
					</td>
				</tr>
				<tr>
					<td>
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
								<asp:TableCell CssClass="Filtro_CellFiltri" ColumnSpan=2 Width=900px HorizontalAlign=left>
									<table>
										<tr>
											<td rowspan="2">&nbsp;</td>
											<td colspan="3">&nbsp;</td>
										</tr>
										<tr>
											<td>
												<asp:Label ID="LBorganizzazione_t" runat="server" CssClass="FiltroVoceSmall">Organizzazione:</asp:Label>
											&nbsp;&nbsp;
												<asp:DropDownList id="DDLorganizzazione" Runat="server" CssClass="FiltroCampoSmall" AutoPostBack=True ></asp:DropDownList>
											</td><td>
												&nbsp;&nbsp;
											</td><td>
												<asp:Label ID="LBtipoPersona_t" runat="server" CssClass="FiltroVoceSmall">Tipo Persona:</asp:Label>
											&nbsp;&nbsp;
												<asp:DropDownList id="DDLtipoPersona" Runat="server" CssClass="FiltroCampoSmall" AutoPostBack=True ></asp:DropDownList>
											</td><td>
												&nbsp;&nbsp;
											</td>
										</tr>
									</table>
									<table width="100%">
									
										<tr>
											<td>&nbsp;</td>
											<td>
												<asp:Label ID="LBtipoRicerca_t" runat="server" CssClass="FiltroVoceSmall">Ricerca per:</asp:Label>
											&nbsp;&nbsp;
												<asp:DropDownList id="DDLtipoRicerca" Runat="server" CssClass="FiltroCampoSmall">
													<asp:ListItem Value=-1>tutti</asp:ListItem>
													<asp:ListItem Value=1>Nome</asp:ListItem>
													<asp:ListItem Value=2>Cognome</asp:ListItem>
												</asp:DropDownList>
											&nbsp;&nbsp;&nbsp;&nbsp;
												<asp:Label ID="LBvalore_t" runat="server" CssClass="FiltroVoceSmall">Valore:</asp:Label>&nbsp;
											&nbsp;&nbsp;
												<asp:TextBox id="TXBvalore" Runat="server" MaxLength="300" CssClass="FiltroCampoSmall" Columns=40></asp:TextBox>
											</td><td>
												<asp:CheckBox ID="CBXautoUpdate" Runat=server AutoPostBack=True CssClass="FiltroCampoSmall" Text="Aggiornamento automatico" Checked="True"></asp:CheckBox>
												&nbsp;&nbsp;
												<asp:Button id="BTNcerca" Runat="server" Text="Cerca" CssClass="Link_Menu"></asp:Button>
											</td>
										</tr>
									</table>									
								</asp:TableCell>			
							</asp:TableRow>
						</asp:Table>
					</td>
				</tr>
				<tr>
					<td align="center" colSpan="3">
						<br/>
						<asp:Panel ID="PNLcontenuto" Runat=server HorizontalAlign=Center>
							<asp:Table ID="TBLdati" Runat="server">
								<asp:TableRow>
									<asp:TableCell HorizontalAlign=Center >
										
										
										<asp:table id="TBLfiltro" Runat="server" HorizontalAlign=left width="900">
											<asp:tableRow>
												<asp:tableCell ColumnSpan="5" HorizontalAlign=left>
													<table align=left>
														<tr>
															<td align="center">
																<asp:linkbutton id="LKBtutti" Runat="server" CssClass="lettera" CommandArgument="-1" OnClick="FiltroLink_Click">Tutti</asp:linkbutton></td>
															<td align="center">
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
												</asp:tableCell>
												<asp:TableCell HorizontalAlign=Right >&nbsp;
													<asp:label ID="LBnumeroRecord" Runat =server cssclass="Filtro_TestoPaginazione">N° Record</asp:label>&nbsp;
													<asp:dropdownlist id="DDLNumeroRecord" CssClass="Filtro_RecordPaginazione" Runat="server" AutoPostBack="true">
														<asp:ListItem Value="30" Selected="true">30</asp:ListItem>
														<asp:ListItem Value="50">50</asp:ListItem>
														<asp:ListItem Value="80">80</asp:ListItem>
														<asp:ListItem Value="100">100</asp:ListItem>
													</asp:dropdownlist>
												</asp:TableCell>
											</asp:tableRow>
										</asp:table>
									</asp:TableCell>
								</asp:TableRow>
								<asp:TableRow ID="TBRpersone">
									<asp:TableCell>
										<asp:datagrid 
										    id="DGpersona" 
										    runat="server" 
										    AllowSorting="true"
										    ShowFooter="false" 
										    AutoGenerateColumns="False"
											AllowPaging="true" AllowCustomPaging="true"
											DataKeyField="PRSN_id" PageSize="30"
											CssClass="DataGrid_Generica">
											<AlternatingItemStyle CssClass="ROW_Alternate_Small"></AlternatingItemStyle>
											<HeaderStyle CssClass="ROW_header_Small_Center"></HeaderStyle>
											<ItemStyle CssClass="ROW_Normal_Small" Height="22px"></ItemStyle>
											<PagerStyle CssClass="ROW_Page_Small" Position=TopAndBottom Mode="NumericPages" Visible="true"
											HorizontalAlign="Right" Height="18px" VerticalAlign="Bottom"></PagerStyle>
											<Columns>
												<asp:BoundColumn DataField="PRSN_Cognome" HeaderText="Cognome" SortExpression="PRSN_Cognome" ItemStyle-Width=120px ItemStyle-CssClass=ROW_TD_Small ></asp:BoundColumn>
												<asp:BoundColumn DataField="PRSN_Nome" HeaderText="Nome" SortExpression="PRSN_Nome" ItemStyle-CssClass=ROW_TD_Small ItemStyle-Width=120px></asp:BoundColumn>
												<asp:TemplateColumn HeaderText="E-mail" SortExpression="PRSN_mail" ItemStyle-CssClass=ROW_TD_Small ItemStyle-Width=245px>
													<ItemTemplate>
															<asp:HyperLink NavigateUrl='<%# "mailto:" & Container.Dataitem("PRSN_mail")%>' text='<%# Container.Dataitem("PRSN_mail")%>' Runat="server" ID="HYPMail" CssClass="ROW_ItemLink_Small" />
														</ItemTemplate>
												</asp:TemplateColumn>
												<asp:BoundColumn DataField="oRicevimento" HeaderText="Orario ricevimento" SortExpression="oRicevimento" ItemStyle-CssClass=ROW_TD_Small ></asp:BoundColumn>
											</Columns>
											
										</asp:datagrid>
									</asp:TableCell>
								</asp:TableRow>
								<asp:TableRow ID="TBRnorecord" Visible=False >
									<asp:TableCell HorizontalAlign=Center >
										<asp:Label id="LBnoRecord"  Runat="server" CssClass="avviso"></asp:Label>
									</asp:TableCell>
								</asp:TableRow>
							</asp:Table>
						
						
						</asp:Panel>
					</td>
				</tr>
			</table>

</asp:Content>
<%--

<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<html>
	<head runat="server">
		<title>Comunità On Line - Ricevimento Docenti</title>
		<meta name="GENERATOR" content="Microsoft Visual Studio .NET 7.1"/>
		<meta name="CODE_LANGUAGE" content="Visual Basic .NET 7.1"/>
		<meta name=vs_defaultClientScript content="JavaScript"/>
		<meta name=vs_targetSchema content="http://schemas.microsoft.com/intellisense/ie5"/>
		<LINK href="../Styles.css" type="text/css" rel="stylesheet"/>

		
		
	</head>

	<body   onkeydown="return SubmitRicerca(event);">
		 <form id="aspnetForm" method="post" runat="server">
		 <asp:ScriptManager ID="SCMmanager" runat="server"></asp:ScriptManager>
         		<tr>
					<td colSpan="3"><HEADER:CtrLHEADER id="Intestazione" runat="server"></HEADER:CtrLHEADER></td>
				</tr>
			<FOOTER:CtrLFOOTER id="CtrLFooter" runat="server"></FOOTER:CtrLFOOTER>
		</form>
	</body>
</html>
--%>