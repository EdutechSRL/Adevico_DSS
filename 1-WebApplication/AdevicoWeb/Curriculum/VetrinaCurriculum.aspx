<%@ Page Language="vb" AutoEventWireup="false" MasterPageFile="~/AjaxPortal.Master" Codebehind="VetrinaCurriculum.aspx.vb" Inherits="Comunita_OnLine.VetrinaCurriculum"%>
<%@ MasterType VirtualPath="~/AjaxPortal.Master" %>
<%--OLD radtabstrip TELERIK--%>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>

<%@ Register TagPrefix="DATI" TagName="CTRLdati" Src="./UC_infodatiCurriculum.ascx" %>
<%@ Register TagPrefix="LINGUA" TagName="CTRLlingua" Src="./UC_infoConoscenzaLingua.ascx" %>
<%@ Register TagPrefix="LAVORO" TagName="CTRLlavoro" Src="./UC_infoEsperienzeLavorative.ascx" %>
<%@ Register TagPrefix="FORMAZIONE" TagName="CTRLformazione" Src="./UC_infoformazione.ascx" %>
<%@ Register TagPrefix="COMPETENZE" TagName="CTRLcompetenze" Src="./UC_infoCompetenze.ascx" %>


<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    		<script language="Javascript" src="../jscript/generali.js" type="text/javascript"></script>
		<script language="javascript" type="text/javascript">

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
		                    document.aspnetForm .BTNCerca.click();
		                }
		                catch (ex) {
		                    return false;
		                }
		            }
		        }
		        else
		            return true;
		    }	
		</script>
</asp:Content>


<asp:Content ID="Content2" ContentPlaceHolderID="CPHservice" runat="server">
	<div id="DVmenu" style="width: 900px; text-align:right;" align="center">
        <asp:Button id="BTNcerca" Runat="server" Text="Cerca" CssClass="Link_Menu"></asp:Button>
		<asp:Panel id=PNLmenu Visible=False Runat=server HorizontalAlign=Right>
			<asp:linkbutton id="LKBtornaLista" Runat="server" Text="Torna alla Lista" CssClass="Link_Menu"></asp:linkbutton>
		</asp:Panel>
    </div>
    <div align="left" style="width: 900px;  padding-top:5px; ">
        
        <asp:Panel ID="PNLcontenuto" Runat="server" HorizontalAlign="Center" Width=800px>
			<asp:Panel ID="PNLvetrina" Runat="server">
				<asp:Table ID="TBLvetrina" Runat="server" Width="800px">
					<asp:TableRow>
						<asp:TableCell HorizontalAlign="Center">
							<asp:Table id="TBLfiltroNew" Runat=server  Width="800px" CellPadding=0 CellSpacing=0 GridLines=none>
								<asp:TableRow id="TBRchiudiFiltro" Height=22px>
									<asp:TableCell CssClass="Filtro_CellSelezionato" HorizontalAlign=Center Width=150px Height=22px VerticalAlign=Middle >
										<asp:LinkButton ID="LNBchiudiFiltro" Runat=server CssClass="Filtro_Link" CausesValidation=False>Chiudi Filtri</asp:LinkButton>
									</asp:TableCell>
									<asp:TableCell CssClass="Filtro_CellDeSelezionato" Width=650px Height=22px>&nbsp;
									</asp:TableCell>
								</asp:TableRow>
								<asp:TableRow id="TBRapriFiltro" Visible=False Height=22px>
									<asp:TableCell ColumnSpan=1 CssClass="Filtro_CellApriFiltro" HorizontalAlign=Center Width=150px Height=22px>
										<asp:LinkButton ID="LNBapriFiltro" Runat=server CssClass="Filtro_Link" CausesValidation=False >Apri Filtri</asp:LinkButton>
									</asp:TableCell>
									<asp:TableCell CssClass="Filtro_Cellnull" Width=650px Height=22px>&nbsp;
									</asp:TableCell>
								</asp:TableRow>
								<asp:TableRow ID="TBRfiltri">
									<asp:TableCell CssClass="Filtro_CellFiltri" ColumnSpan=2 Width=800px HorizontalAlign=center Height=20px>
										<asp:table id="TBLfiltro" Runat="server" HorizontalAlign="left" Width=800px GridLines=none>
											<asp:TableRow>
												<asp:TableCell Width=90px>
													<asp:Label ID="LBorganizzazione_t" runat="server" CssClass="FiltroVoceSmall">Organizzazione:</asp:Label>
												</asp:TableCell>
												<asp:TableCell ColumnSpan=4>
													<table align=left cellpadding=0 cellspacing=0 border=0>
														<tr>
															<td>
																<asp:DropDownList id="DDLorganizzazione" Runat="server" CssClass="FiltroCampoSmall" AutoPostBack=True ></asp:DropDownList>
															</td>
															<td width=50px>&nbsp;</td>
															<td>
																<asp:Label ID="LBtipoPersona_t" runat="server" CssClass="FiltroVoceSmall">Tipo Persona:</asp:Label>&nbsp;&nbsp;
															</td>
															<td>
																<asp:DropDownList id="DDLtipoPersona" Runat="server" CssClass="FiltroCampoSmall" AutoPostBack=True ></asp:DropDownList>
															</td>
														</tr>
													</table>
												</asp:TableCell>
												<asp:TableCell>
													&nbsp;
												</asp:TableCell>
											</asp:TableRow>
											<asp:TableRow>
												<asp:TableCell Width=90px>
													<asp:Label ID="LBtipoRicerca_t" runat="server" CssClass="FiltroVoceSmall">Ricerca per:</asp:Label>
												</asp:TableCell>
												<asp:TableCell ColumnSpan=5>
													<table align=left cellpadding=0 cellspacing=0 border=0>
														<tr>
															<td>
																<asp:DropDownList id="DDLtipoRicerca" Runat="server" CssClass="FiltroCampoSmall">
																	<asp:ListItem Value=-1>tutti</asp:ListItem>
																	<asp:ListItem Value=1>Nome</asp:ListItem>
																	<asp:ListItem Value=2>Cognome</asp:ListItem>
																</asp:DropDownList>
															</td>
															<td width=50px>&nbsp;</td>
															<td>
																<asp:Label ID="LBvalore_t" runat="server" CssClass="FiltroVoceSmall">Valore:</asp:Label>&nbsp;
															</td>
															<td>
																<asp:TextBox id="TXBvalore" Runat="server" MaxLength="300" CssClass="FiltroCampoSmall" Columns=40></asp:TextBox>
															</td>
														</tr>
													</table>
												</asp:TableCell>
											</asp:TableRow>
										</asp:Table>
									</asp:TableCell>
								</asp:TableRow>
								<asp:tableRow >
									<asp:tableCell ColumnSpan=2>
										<table width=800px>
											<tr>
												<td>
													<table width="400px" align="left">
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
												</td>
												<td align=right >
													&nbsp;<asp:label ID="LBnumeroRecord" Runat =server cssclass="Filtro_TestoPaginazione">N° Record</asp:label>&nbsp;
													<asp:dropdownlist id="DDLNumeroRecord" CssClass="Filtro_RecordPaginazione" Runat="server" AutoPostBack="true">
														<asp:ListItem Value="30" Selected="true">30</asp:ListItem>
														<asp:ListItem Value="50">50</asp:ListItem>
														<asp:ListItem Value="80">80</asp:ListItem>
														<asp:ListItem Value="100">100</asp:ListItem>
													</asp:dropdownlist>
												</td>
											</tr>
										</table>
														
									</asp:tableCell>
								</asp:tableRow>
							</asp:table>
						</asp:TableCell>
					</asp:TableRow>
					<asp:TableRow ID="TBRpersone" >
						<asp:TableCell ColumnSpan=2 HorizontalAlign=Center >
							<asp:datagrid 
								id="DGpersona" 
								runat="server" 
								AllowSorting="true" 
								ShowFooter="false" 
								AutoGenerateColumns="False"
								AllowPaging="true" 
								AllowCustomPaging="true" 
								DataKeyField="PRSN_id" 
								PageSize="20"
								CssClass="DataGrid_Generica">
									<AlternatingItemStyle CssClass="ROW_Alternate_Small"></AlternatingItemStyle>
							<HeaderStyle CssClass="ROW_header_Small_Center"></HeaderStyle>
							<ItemStyle CssClass="ROW_Normal_Small" Height="22px"></ItemStyle>
							<PagerStyle CssClass="ROW_Page_Small" Position=TopAndBottom Mode="NumericPages" Visible="true"
							HorizontalAlign="Right" Height="18px" VerticalAlign="Bottom"></PagerStyle>
								<Columns>
									<asp:TemplateColumn HeaderText="Cognome" SortExpression="PRSN_Cognome" ItemStyle-CssClass=ROW_TD_Small>
										<ItemTemplate>
											<table>
												<tr>
													<td Class=ROW_TD_Small>&nbsp;</td>
													<td Class=ROW_TD_Small><%# Container.Dataitem("PRSN_Cognome")%></td>
												</tr>
											</table>
										</ItemTemplate>
									</asp:TemplateColumn>
									<asp:TemplateColumn HeaderText="Nome" SortExpression="PRSN_Nome" ItemStyle-CssClass=ROW_TD_Small>
										<ItemTemplate>
											<table>
												<tr>
													<td Class=ROW_TD_Small>&nbsp;</td>
													<td Class=ROW_TD_Small><%# Container.Dataitem("PRSN_Nome")%></td>
												</tr>
											</table>
										</ItemTemplate>
									</asp:TemplateColumn>
									<asp:TemplateColumn HeaderText="Tipo utente" SortExpression="TPPR_Descrizione" ItemStyle-CssClass=ROW_TD_Small>
										<ItemTemplate>
											<table>
												<tr>
													<td Class=ROW_TD_Small>&nbsp;</td>
													<td Class=ROW_TD_Small><%# Container.Dataitem("TPPR_Descrizione")%></td>
												</tr>
											</table>
										</ItemTemplate>
									</asp:TemplateColumn>
									<asp:ButtonColumn CommandName="curriculum" ButtonType="LinkButton" Text="Visualizza Curriculum" ItemStyle-Width=150px ItemStyle-CssClass=ROW_TD_Small_Center></asp:ButtonColumn>
									<asp:BoundColumn Visible=False DataField="PRSN_Nome"></asp:BoundColumn>
									<asp:BoundColumn Visible=False DataField="PRSN_Cognome"></asp:BoundColumn>
								</Columns>
							</asp:datagrid>
						</asp:TableCell>
					</asp:TableRow>
					<asp:TableRow ID="TBRnorecord" Visible="False">
						<asp:TableCell HorizontalAlign="Center">
							<asp:Label id="LBnoRecord" Runat="server" CssClass="avviso"></asp:Label>
						</asp:TableCell>
					</asp:TableRow>
				</asp:Table>
			</asp:Panel>
			<asp:Panel ID="PNLcurriculum" runat="server" Visible="False">
				<asp:Table ID="TBLdati" Runat="server" Width=100%>
					<asp:TableRow>
						<asp:TableCell CssClass=RigaTab>
							<telerik:radtabstrip id="TBSmenu" runat="server" Skin="ClassicBlue" align="justify" SelectedIndex="0" Width="100%" Height="26px"
								CausesValidation="False" autopostback="true">
								<Tabs>
									<telerik:RadTab id="TABdati" Text="Dati" ToolTip="Dati personali"></telerik:RadTab>
									<telerik:RadTab id="TABcompetenze" Text="Competenze" ToolTip="Competenze"></telerik:RadTab>
									<telerik:RadTab id="TABformazione" Text="Istruzione" ToolTip="Istruzione"></telerik:RadTab>
									<telerik:RadTab id="TABlingua" Text="Lingue" ToolTip="Conoscenza Lingue"></telerik:RadTab>
									<telerik:RadTab id="TABesperienze" Text="Esperienze Lavorative" ToolTip="Esperienze Lavorative"></telerik:RadTab>
								</Tabs>
							 </telerik:radtabstrip>
						</asp:TableCell>
					</asp:TableRow>
					<asp:TableRow Runat="server" ID="TBRdati">
						<asp:TableCell >
							<DATI:CTRLdati id="CTRLdati" runat="server" ></DATI:CTRLdati>
						</asp:TableCell>
					</asp:TableRow>
					<asp:TableRow Runat="server" ID="TBRcompetenze" Visible="False">
						<asp:TableCell>
							<COMPETENZE:CTRLcompetenze id="CTRLcompetenze" runat="server" Width="550px"></COMPETENZE:CTRLcompetenze>
						</asp:TableCell>
					</asp:TableRow>
					<asp:TableRow Runat="server" ID="TBRformazione" Visible="False">
						<asp:TableCell>
							<FORMAZIONE:CTRLformazione id="CTRLformazione" runat="server" Width="550px"></FORMAZIONE:CTRLformazione>
						</asp:TableCell>
					</asp:TableRow>
					<asp:TableRow Runat="server" ID="TBRlingua" Visible="False">
						<asp:TableCell>
							<LINGUA:CTRLlingua id="CTRLlingua" runat="server" Width="550px"></LINGUA:CTRLlingua>
						</asp:TableCell>
					</asp:TableRow>
					<asp:TableRow Runat="server" ID="TBResperienze" Visible="False">
						<asp:TableCell>
							<LAVORO:CTRLlavoro id="CTRLlavoro" runat="server" Width="550px"></LAVORO:CTRLlavoro>
						</asp:TableCell>
					</asp:TableRow>
				</asp:Table>
			</asp:Panel>
		</asp:Panel>
    </div>
</asp:Content>