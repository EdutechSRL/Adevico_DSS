<%@ Page Language="vb" AutoEventWireup="false" MasterPageFile="~/Admin_Globale/AdminPortal.Master"
 Codebehind="AdminG_CategoriaFile.aspx.vb" Inherits="Comunita_OnLine.AdminG_CategoriaFile"%>
<%@ MasterType VirtualPath="~/Admin_Globale/AdminPortal.Master" %>

<asp:Content ContentPlaceHolderID="HeadContent" runat="server" ID="Content1">
	<script type="text/javascript" language="Javascript" src="./../jscript/generali.js"></script>
    <style type="text/css">
		td{
	        font-size: 11px;
		}
	</style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="CPHservice" runat="server">
	<table cellSpacing="0" cellPadding="0" width="900px" border="0">
<%--		<tr>
			<td align="left" class="RigaTitoloAdmin">
				<asp:Label iD="LBTitolo" Runat="server">Management Categoria File</asp:Label>
			</td>
		</tr>--%>
		<tr>
			<td align=right>
				<asp:Panel ID="PNLmenu" Runat=server HorizontalAlign=Right>
					&nbsp;<asp:linkbutton id="LNBinserisci" Runat="server" Text="Crea nuovo" CssClass=Link_Menu></asp:linkbutton>
				</asp:Panel>
				<asp:Panel ID="PNLmenuAzione" Runat=server HorizontalAlign=Right Visible=False>
					&nbsp;<asp:linkbutton id="LNBindietro" Runat="server" Text="Torna all'elenco" CssClass=Link_Menu CausesValidation=False></asp:linkbutton>
					<asp:linkbutton id="LNBsalvaDati" Runat="server" Text="Salva" CssClass=Link_Menu></asp:linkbutton>
				</asp:Panel>
			</td>
		</tr>
		<tr>
			<td align="center" valign="top">
				<br/>
				<asp:panel id="PNLpermessi" Runat="server" Visible="False">
					<table align="center">
						<tr>
							<td height="50">&nbsp;</td>
						</tr>
						<tr>
							<td align="center">
								<asp:Label id="LBNopermessi" Runat="server" CssClass="messaggio"></asp:Label></td>
						</tr>
						<tr>
							<td height="50">&nbsp;</td>
						</tr>
					</table>
				</asp:panel>
				<asp:Panel ID="PNLcontenuto" Runat="server" HorizontalAlign="Center">
					<table align=center width=700px>
						<tr>
							<td>
								<asp:panel id="PNLlista" Runat="server">
									<asp:datagrid 
										id="DGcategoria" runat="server"
										ShowFooter="false"
										AutoGenerateColumns="False" AllowPaging="true"
										DataKeyField="CTGR_id"
										AllowSorting="true" PageSize=20
										CssClass="DataGrid_Generica" >
										<AlternatingItemStyle CssClass="ROW_Alternate_Small"></AlternatingItemStyle>
										<HeaderStyle CssClass="ROW_header_Small_Center"></HeaderStyle>
										<ItemStyle CssClass="ROW_Normal_Small" Height="22px"></ItemStyle>
										<PagerStyle CssClass="ROW_Page_Small" Position=TopAndBottom Mode="NumericPages" Visible="true" HorizontalAlign="Right" Height="18px" VerticalAlign="Bottom"></PagerStyle>
										<Columns>
											<asp:TemplateColumn runat="server" HeaderText="" headerStyle-CssClass=ROW_Header_Small_center ItemStyle-CssClass=ROW_TD_Small_center ItemStyle-Width="50px">
												<ItemTemplate>
													<asp:ImageButton id="IMBmodifica" Runat="server" CausesValidation="False" CommandName="modifica"
														ImageUrl="../images/m.gif" ToolTip = "Modifica Categoria"></asp:ImageButton>
													<asp:ImageButton id="IMBCancella" Runat="server" CausesValidation="False" CommandName="elimina"
													ImageUrl="../images/x.gif"></asp:ImageButton>
												</ItemTemplate>
											</asp:TemplateColumn>
											<asp:BoundColumn HeaderText="Nome Categoria" DataField="CTGR_nome" ItemStyle-Width=570px SortExpression="CTGR_nome" headerStyle-CssClass=ROW_Header_Small_center ItemStyle-CssClass=ROW_TD_Small></asp:BoundColumn>
											<asp:BoundColumn DataField="FileAssociati" HeaderText="File Associati" Visible=True ItemStyle-Width=100px  headerStyle-CssClass=ROW_Header_Small_center ItemStyle-CssClass=ROW_TD_Small_center></asp:BoundColumn>
											<asp:BoundColumn DataField="Totale" Visible=false HeaderText="File Associati" ItemStyle-Width=80 headerStyle-CssClass=ROW_Header_Small_center ItemStyle-CssClass=ROW_TD_Small></asp:BoundColumn>
											<asp:BoundColumn DataField="CTGR_noDelete" Visible=false></asp:BoundColumn>
										</Columns>
									</asp:datagrid>
								</asp:panel>
								<asp:panel id="PNLnorecord" Runat="server" Visible="false" HorizontalAlign=Center >
									<table width="600px" align="center">
										<tr>
											<td height="20">&nbsp;</td>
										</tr>
										<tr>
											<td align="center">
												<asp:Label id="LBnorecord" Runat="server" CssClass="info_blackMedium"></asp:Label></td>
										</tr>
										<tr>
											<td height="20">&nbsp;</td>
										</tr>
									</table>
								</asp:panel>
								<asp:panel id="PNLdati" Runat="server" Visible="False" HorizontalAlign=Center>
									<INPUT id="HDctgr_id" type="hidden" runat="server" NAME="HDctgr_id"/>
									<table cellSpacing="0" cellPadding="0" align="center" width=750px>
										<tr>
											<td>
												<asp:Label ID="LBnome_t" CssClass="Titolo_campoSmall" Runat=server >*Nome:</asp:Label>
											</td>
											<td>
												<asp:textbox id="TXBnome" Runat="server" CssClass="Testo_Campo_obbligatorioSmall" MaxLength="100" Columns="70"></asp:textbox>
												<asp:requiredfieldvalidator id="Requiredfieldvalidator3" runat="server" CssClass="Validatori" ControlToValidate="TXBnome"
													Display="Dynamic">*</asp:requiredfieldvalidator>
											</td>
										</tr>
										<tr>
											<td>&nbsp;</td>
											<td>
												<table border=1 align=left bgcolor="#FFFBF7" style="border-color:#CCCCCC" cellpadding=0 cellspacing=0>
													<tr>
														<td>
															<table border=0 align=left bgcolor="#FFFBF7" cellpadding=0 cellspacing=0>
																<asp:Repeater id="RPTnome" Runat="server">
																	<HeaderTemplate>
																		<tr>
																			<td colspan=2 height=20px>
																				<asp:Label ID="LBlinguaNome_t" Runat=server CssClass="Titolo_campoSmall">&nbsp;Traduzioni(°):</asp:Label>
																			</td>
																		</tr>
																	</HeaderTemplate>
																	<ItemTemplate>
																		<tr>
																			<td align=right width=120px height=22px>
																				<asp:Label id="LBlinguaID" Text='<%# Databinder.eval(Container.DataItem, "LNGU_ID")%>' runat="server" Visible=false />
																				<asp:Label id="LBlingua_Nome" Text='<%# Databinder.eval(Container.DataItem, "LNGU_nome")%>' runat="server" Visible=true CssClass=Repeater_Voce/>&nbsp;
																			</td>
																			<td align=left height=22px>
																				<asp:TextBox ID="TXBtermine" Runat="server" CssClass="Testo_campoSmall" MaxLength="50" Columns="60" Text='<%# Databinder.eval(Container.DataItem, "Nome")%>'> </asp:TextBox>&nbsp;&nbsp;
																			</td>
																		</tr>
																	</ItemTemplate>
																	<FooterTemplate>
																		<tr><td colspan=2 class=nosize0>&nbsp;</td></tr>
																	</FooterTemplate>
																</asp:Repeater>
															</table>
														</td>
													</tr>
												</table>			
											</td>	
										</tr>
										<tr>
											<td colspan=2 height=15px class="Nosize0">&nbsp;</td>
										</tr>
										<tr>
											<td class=top>
												<asp:Label ID="LBelencoTipiComunita_t"  Runat=server CssClass="Titolo_campoSmall" >Tipi Comunità associati:</asp:Label>
											</td>
											<td class=top>
												<asp:CheckBoxList ID=CBLtipoComunita Runat=server CssClass="Testo_campoSmall" RepeatDirection=Horizontal RepeatColumns=3 RepeatLayout=Table></asp:CheckBoxList>
											</td>
										</tr>
									</table>
								</asp:panel>
							</td>
						</tr>
					</table>
					<asp:validationsummary id="VLDSum" runat="server" HeaderText="Attenzione! Sono state rilevate delle imprecisioni nella compilazione del form. Controlla i valori inseriti in corrisponsenza degli *"
						ShowSummary="false" ShowMessageBox="true" DisplayMode="BulletList"></asp:validationsummary>
				</asp:Panel>
			</td>
		</tr>
	</table>
</asp:Content>


<%--

<!DOCTYPE HTML PUBLIC "-//W3C//Dtd HTML 4.0 transitional//EN">
<HTML>
	<head runat="server">
		<title>Comunità On Line</title>
		<LINK href="./../Styles.css" type="text/css" rel="stylesheet"/>
		<script type="text/javascript" language="Javascript" src="./../jscript/generali.js"></script>
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR"/>
		<meta content="Visual Basic .NET 7.1" name="CODE_LANGUAGE"/>
		<meta content="JavaScript" name="vs_defaultClientScript"/>
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema"/>

	</HEAD>
	<body>
		<form id="aspnetForm" method="post" runat="server">
		<asp:ScriptManager ID="SCMmanager" runat="server"></asp:ScriptManager>
			<table class="contenitore" align="center">
				<tr class="contenitore">
					<td colSpan="3"><HEADER:CtrLHEADER id="Intestazione" runat="server"></HEADER:CtrLHEADER></td>
				</tr>
				<tr class="contenitore">
					<td colSpan="3">

					</td>
				</tr>
			</table>
			<FOOTER:CtrLFOOTER id="CtrLFooter" runat="server"></FOOTER:CtrLFOOTER>
		</form>
	</body>
</HTML>--%>