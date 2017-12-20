<%@ Page Language="vb" AutoEventWireup="false" MasterPageFile="~/AjaxPortal.Master" Codebehind="Management_ProfiliServizi.aspx.vb" Inherits="Comunita_OnLine.Management_ProfiliServizi"%>
<%@ MasterType VirtualPath="~/AjaxPortal.Master" %>

<%--
--%>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <script language="Javascript" src="./../jscript/generali.js" type="text/javascript"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="CPHservice" runat="server">
	<table cellSpacing="0" cellPadding="0" width="900px" border="0">
<%--		<tr>
			<td class="RigaTitolo" align="left">
				<asp:Label ID="LBtitolo" Runat="server">Gestione Profili Personali Servizi Comunità</asp:Label>
			</td>
		</tr>--%>
		<tr>
			<td align=right>
				&nbsp;<asp:linkbutton id="LNBinserisci" Runat="server" text="Inserisci" CausesValidation=False CssClass=Link_Menu></asp:linkbutton>
			</td>
		</tr>
		<tr>
			<td align="center" valign="top">
				<br/>
				<asp:Panel ID="PNLpermessi" Runat="server" Visible="False" HorizontalAlign=Center>
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
							<td height="50">&nbsp;</td>
						</tr>
					</table>
				</asp:Panel>
				<asp:Panel ID="PNLcontenuto" Runat=server HorizontalAlign=Center Width=900px>
					<table cellSpacing="0" cellPadding="0" width="800px" border="0">
						<tr>
							<td vAlign="top" align="center">
								<asp:datagrid 
									id="DGprofilo" Runat="server" 
									ShowHeader="true" AllowSorting="true" 
									GridLines=Vertical AutoGenerateColumns="False" 
									DataKeyField="PRFS_ID" AllowPaging="true" PageSize=25
									CssClass="DataGrid_Generica">
									<AlternatingItemStyle CssClass="ROW_Alternate_Small"></AlternatingItemStyle>
									<HeaderStyle CssClass="ROW_header_Small_Center"></HeaderStyle>
									<ItemStyle CssClass="ROW_Normal_Small" Height="22px"></ItemStyle>
									<PagerStyle CssClass="ROW_Page_Small" Position=TopAndBottom Mode="NumericPages" Visible="true" HorizontalAlign="Right" Height="18px" VerticalAlign="Bottom"></PagerStyle>
									<Columns>
										<asp:BoundColumn DataField="PRFS_ID" HeaderText="" Visible="False"></asp:BoundColumn>
										<asp:TemplateColumn headerStyle-CssClass=ROW_Header_Small_center ItemStyle-CssClass="ROW_TD_Small" ItemStyle-Width=30px>
											<ItemTemplate>
												<asp:ImageButton ID="IMBmodifica" runat="server" Commandname="modifica" CausesValidation=False ImageUrl="../images/m.gif" Visible=True></asp:ImageButton>
												<asp:ImageButton ID="IMBelimina" runat="server" CommandName="elimina" CausesValidation=False ImageUrl="../images/x_d.gif" Visible=True></asp:ImageButton>
											</ItemTemplate>
										</asp:TemplateColumn>
										<asp:TemplateColumn headerStyle-CssClass=ROW_Header_Small_center HeaderText="Tipo comunità" SortExpression="TPCM_Descrizione" ItemStyle-CssClass=ROW_TD_Small>
											<ItemTemplate>
												&nbsp;<asp:Label ID="Label1" CssClass="ROW_TD_Small" Runat=server><%# DataBinder.Eval(Container.DataItem, "TPCM_Descrizione") %></asp:Label>
											</ItemTemplate>
										</asp:TemplateColumn>
										<asp:TemplateColumn headerStyle-CssClass=ROW_Header_Small_center HeaderText="Profilo" SortExpression="PRFS_Nome">
											<ItemTemplate>
												<asp:Table ID="TBLdati" Runat=server>
													<asp:TableRow ID="TBRnome">
														<asp:TableCell Width="10px">&nbsp;</asp:TableCell>
														<asp:TableCell>
															<asp:Label ID="LBnome" CssClass="ROW_TD_Small" Runat=server><%# DataBinder.Eval(Container.DataItem, "ProfiloNome") %></asp:Label>
														</asp:TableCell>
													</asp:TableRow>
													<asp:TableRow ID="TBRdescrizione">
														<asp:TableCell Width="10px">&nbsp;</asp:TableCell>
														<asp:TableCell>
															<asp:Label ID="LBdescrizione" CssClass="ROW_TD_Small" Runat=server><i><%# DataBinder.Eval(Container.DataItem, "PRFS_Descrizione") %></i></asp:Label>
														</asp:TableCell>
													</asp:TableRow>
													<asp:TableRow>
														<asp:TableCell Width="10px">&nbsp;</asp:TableCell>
														<asp:TableCell>
															<asp:LinkButton ID="LNBdefinisciRuoli" Runat="server" CausesValidation="False" CommandName=ruoli>Cambia ruoli</asp:LinkButton>
															&nbsp;|&nbsp;
															<asp:LinkButton ID="LNDdefinisciServizi" Runat="server" CausesValidation="False" CommandName=servizi>Cambia servizi</asp:LinkButton>
															&nbsp;|&nbsp;
															<asp:LinkButton ID="LNBdefinisciPermessi" Runat="server" CausesValidation="False" CommandName=permessi>Gestione permessi</asp:LinkButton>
														</asp:TableCell>
													</asp:TableRow>
												</asp:Table>
											</ItemTemplate>
										</asp:TemplateColumn>
									</Columns>
								</asp:datagrid>
								<br/>
								<asp:Label ID="LBnoRecord" Runat=server CssClass="avviso11"></asp:Label>
							</td>
						</tr>
					</table>
				</asp:Panel>
			</td>
		</tr>
	</table>	
</asp:Content>
<%--
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<html>
  <head runat="server">
		<title>Comunità On Line - Gestione Profili Servizi</title>
		<meta name="GENERATOR" content="Microsoft Visual Studio .NET 7.1"/>
		<meta name="CODE_LANGUAGE" content="Visual Basic .NET 7.1"/>
		<meta name="vs_defaultClientScript" content="JavaScript"/>
		<meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5"/>
		<LINK href="./../Styles.css" type="text/css" rel="stylesheet"/>
		
  </head>
 <body >
    <form id="aspnetForm" method="post" runat="server">
    <asp:ScriptManager ID="SCMmanager" runat="server"></asp:ScriptManager>
		<table width="900px" align="center" border="0">
			<tr>
				<td colSpan="3">
					<HEADER:CtrLHEADER id="Intestazione" runat="server"></HEADER:CtrLHEADER>
				</td>
			</tr>
			<tr>
				<td colSpan="3">

				</td>
			</tr>
		</table>
		<FOOTER:CTRLFOOTER id="CTRLFooter" runat="server"></FOOTER:CTRLFOOTER>
    </form>
 </body>
</html>--%>