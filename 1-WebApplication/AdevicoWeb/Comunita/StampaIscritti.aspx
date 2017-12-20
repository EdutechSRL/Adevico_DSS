<%@ Page Language="vb" AutoEventWireup="false" Codebehind="StampaIscritti.aspx.vb" Inherits="Comunita_OnLine.StampaIscritti"%>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<html>
	<head runat="server">
		<title>Comunità on Line</title>
		<script language="Javascript" src="./../jscript/generali.js" type="text/javascript"></script>
		<meta name="GENERATOR" content="Microsoft Visual Studio .NET 7.1"/>
		<meta name="CODE_LANGUAGE" content="Visual Basic .NET 7.1"/>
		<meta name="vs_defaultClientScript" content="JavaScript"/>
		<meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5"/>
		<LINK href="../Styles.css" type="text/css" rel="stylesheet"/>
		
	</head>
	<script language="javascript" type="text/javascript">
			function ChiudiMi(){
			this.window.close();
			}
			
			function stampa(){
			this.window.print();
			}
		</script>
	<body>
		<form id="aspnetForm" method="post" runat="server">
		<asp:ScriptManager ID="SCMmanager" runat="server"></asp:ScriptManager>
			<table align="center" width=100%>
				<tr>
					<td colspan="3" align="center" class=RigaTitolo>
						<asp:Label ID="LBtitolo" Runat="server"></asp:Label>
					</td>
				</tr>
				<tr >
					<td><asp:Button Runat="server" ID="BTNOk" Text="Chiudi" CssClass="PulsantiMenu_Bold"></asp:Button></td>
					<td class="size0" align=center>&nbsp;</td>
					<td align="right">
						<asp:Button Runat="server" ID="BTNexcel" Text="Esporta in Excel" CssClass="PulsantiMenu_Bold"></asp:Button>
						<asp:Button Runat="server" ID="BTNstampa" Text="Stampa" CssClass="PulsantiMenu_Bold"></asp:Button>
					</td>
				</tr>
				
				<tr>
					<td align="center" colspan="3">
						<br/>
						<asp:Table CellPadding="0" CellSpacing="0" Runat="server" ID="TBLDettagli" Width=750px>
							<asp:TableRow ID="TBRiscritti">
								<asp:TableCell>
									<asp:Label ID="LBabilitati" Runat="server" cssclass="Titolo_campo_Rosso">Utenti Abilitati</asp:Label>
								</asp:TableCell>
							</asp:TableRow>
							<asp:TableRow Height="22px" ID="TBRiscrittiSeparatore">
								<asp:tablecell CssClass="dettagli_separatore">
									<img src="./../images/separatore_small.gif" alt="" align="top" border="0" width="100%"
										height="2px"/></asp:tablecell>
							</asp:TableRow>
							<asp:TableRow>
								<asp:tablecell>
									<asp:Label id="LBnoIscritti" Visible="False" Runat="server" CssClass="avviso_normal"></asp:Label>
								</asp:tablecell>
							</asp:TableRow>
							<asp:TableRow>
								<asp:tablecell>
									<asp:datagrid 
									    id="DGiscritti" runat="server" 
									    AllowSorting="true" 
									    ShowFooter="false" 
										AutoGenerateColumns="False" 
										AllowPaging="false"
										DataKeyField="RLPC_ID"
										CssClass="DataGrid_Generica">
										<HeaderStyle CssClass="ROW_header_Small_Center"></HeaderStyle>
										<ItemStyle CssClass="ROW_Normal_Small" Height="22px"></ItemStyle>
										<Columns>
											<asp:TemplateColumn runat="server" HeaderText="#" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="10" HeaderStyle-CssClass=ROW_TD_Small>
												<ItemTemplate>
													<asp:Label Runat="server" id="LBnumero">
														&nbsp;<%# DataBinder.Eval(Container.DataItem, "oNumero") %>
													</asp:Label>
												</ItemTemplate>
											</asp:TemplateColumn>
											<asp:TemplateColumn runat="server" HeaderText="Matricola" ItemStyle-CssClass=ROW_TD_Small ItemStyle-Width="15" HeaderStyle-CssClass=ROW_TD_Small>
												<ItemTemplate>
													<asp:Label Runat="server" id="LBmatricola">
														&nbsp;<%# DataBinder.Eval(Container.DataItem, "oMatricola") %>
													</asp:Label>
												</ItemTemplate>
											</asp:TemplateColumn>
											<asp:BoundColumn DataField="PRSN_cognome" HeaderText="Cognome" ItemStyle-CssClass=ROW_TD_Small HeaderStyle-CssClass=ROW_TD_Small></asp:BoundColumn>
											<asp:BoundColumn DataField="PRSN_nome" HeaderText="Nome" ItemStyle-CssClass=ROW_TD_Small HeaderStyle-CssClass=ROW_TD_Small></asp:BoundColumn>
											<asp:BoundColumn DataField="PRSN_mail" HeaderText="E-Mail" ItemStyle-CssClass=ROW_TD_Small HeaderStyle-CssClass=ROW_TD_Small></asp:BoundColumn>
											<asp:BoundColumn DataField="PRSN_login" HeaderText="Login" Visible="False" ItemStyle-CssClass=ROW_TD_Small HeaderStyle-CssClass=ROW_TD_Small></asp:BoundColumn>
											<asp:TemplateColumn HeaderText="Ruolo" ItemStyle-CssClass=ROW_TD_Small HeaderStyle-CssClass=ROW_TD_Small>
												<ItemTemplate>
													&nbsp;<%# DataBinder.Eval(Container.DataItem, "TPRL_nome") %>
												</ItemTemplate>
											</asp:TemplateColumn>
											<asp:BoundColumn DataField="oIscritto" HeaderText="Iscritto Il" ItemStyle-CssClass=ROW_TD_Small HeaderStyle-CssClass=ROW_TD_Small></asp:BoundColumn>
										</Columns>
									</asp:datagrid>
									<br/>
									<br/>
								</asp:tablecell>
							</asp:TableRow>
							<asp:TableRow ID="TBRbloccati">
								<asp:tablecell CssClass="Top">
									<asp:Label ID="LButentiBloccati" Runat="server" cssclass="Titolo_campo_Rosso">Utenti Bloccati</asp:Label>
								</asp:tablecell>
							</asp:TableRow>
							<asp:TableRow Height="22px" ID="TBRbloccatiSeparatore">
								<asp:tablecell CssClass="dettagli_separatore">
									<img src="./../images/separatore_small.gif" alt="" align="top" border="0" width="100%"
										height="2px"/></asp:tablecell>
							</asp:TableRow>
							<asp:TableRow ID="TBRbloccati1">
								<asp:tablecell>
									<asp:Label id="LBnoIscrittiBloccati" Visible="False" Runat="server" CssClass="avviso_normal"></asp:Label>
								</asp:tablecell>
							</asp:TableRow>
							<asp:TableRow ID="TBRbloccati2">
								<asp:tablecell>
									<asp:datagrid 
									    id="DGiscrittiBloccati" runat="server" 
									    AllowSorting="true" 
									    ShowFooter="false" 
										AutoGenerateColumns="False"
										AllowPaging="false" DataKeyField="RLPC_ID"
										CssClass="DataGrid_Generica">
										<HeaderStyle CssClass="ROW_header_Small_Center"></HeaderStyle>
										<ItemStyle CssClass="ROW_Normal_Small" Height="22px"></ItemStyle>
										<Columns>
											<asp:TemplateColumn runat="server" HeaderText="#" ItemStyle-CssClass=ROW_TD_Small ItemStyle-Width="10" HeaderStyle-CssClass=ROW_TD_Small>
												<ItemTemplate>
													<asp:Label Runat="server" id="LBnumeroBloccati">
														&nbsp;<%# DataBinder.Eval(Container.DataItem, "oNumero") %>
													</asp:Label>
												</ItemTemplate>
											</asp:TemplateColumn>
											<asp:TemplateColumn runat="server" HeaderText="Matricola" ItemStyle-CssClass=ROW_TD_Small ItemStyle-Width="15" HeaderStyle-CssClass=ROW_TD_Small>
												<ItemTemplate>
													<asp:Label Runat="server" id="LBmatricolaBloccati">
														&nbsp;<%# DataBinder.Eval(Container.DataItem, "oMatricola") %>
													</asp:Label>
												</ItemTemplate>
											</asp:TemplateColumn>
											<asp:BoundColumn DataField="PRSN_cognome" HeaderText="Cognome" ItemStyle-CssClass=ROW_TD_Small HeaderStyle-CssClass=ROW_TD_Small ></asp:BoundColumn>
											<asp:BoundColumn DataField="PRSN_nome" HeaderText="Nome" ItemStyle-CssClass=ROW_TD_Small HeaderStyle-CssClass=ROW_TD_Small></asp:BoundColumn>
											<asp:BoundColumn DataField="PRSN_mail" HeaderText="E-Mail" ItemStyle-CssClass=ROW_TD_Small HeaderStyle-CssClass=ROW_TD_Small></asp:BoundColumn>
											<asp:BoundColumn DataField="PRSN_login" HeaderText="Login" Visible="False" HeaderStyle-CssClass=ROW_TD_Small></asp:BoundColumn>
											<asp:TemplateColumn HeaderText="Ruolo" ItemStyle-CssClass=ROW_TD_Small HeaderStyle-CssClass=ROW_TD_Small>
												<ItemTemplate>
													&nbsp;<%# DataBinder.Eval(Container.DataItem, "TPRL_nome") %>
												</ItemTemplate>
											</asp:TemplateColumn>
											<asp:BoundColumn DataField="oIscritto" HeaderText="Iscritto Il" ItemStyle-CssClass=ROW_TD_Small HeaderStyle-CssClass=ROW_TD_Small></asp:BoundColumn>
										</Columns>
									</asp:datagrid>
									<br/>
									<br/>
								</asp:tablecell>
							</asp:TableRow>
							<asp:TableRow ID="TBRinAttesa">
								<asp:tablecell CssClass="Top">
									<asp:Label ID="LBinAttesa" Runat="server" cssclass="Titolo_campo_Rosso">Utenti inAttesa</asp:Label>
								</asp:tablecell>
							</asp:TableRow>
							<asp:TableRow Height="22px" ID="TBRinAttesaSeparatore">
								<asp:tablecell CssClass="dettagli_separatore">
									<img src="./../images/separatore_small.gif" alt="" align="top" border="0" width="100%"
										height="2px"/></asp:tablecell>
							</asp:TableRow>
							<asp:TableRow ID="TBRinAttesa1">
								<asp:tablecell>
									<asp:Label id="LBnoIscrittiInAttesa" Visible="False" Runat="server" CssClass="avviso_normal"></asp:Label>
								</asp:tablecell>
							</asp:TableRow>
							<asp:TableRow ID="TBRinAttesa2">
								<asp:tablecell>
									<asp:datagrid 
									    id="DGiscrittiInAttesa" runat="server" 
									    AllowSorting="true" 
									    ShowFooter="false" 
										AutoGenerateColumns="False"
										AllowPaging="false" 
										DataKeyField="RLPC_ID"
										CssClass="DataGrid_Generica">
										<HeaderStyle CssClass="ROW_header_Small_Center"></HeaderStyle>
										<ItemStyle CssClass="ROW_Normal_Small" Height="22px"></ItemStyle>
										<Columns>
											<asp:TemplateColumn runat="server" HeaderText="#" ItemStyle-CssClass=ROW_TD_Small ItemStyle-Width="10" HeaderStyle-CssClass=ROW_TD_Small>
												<ItemTemplate>
													<asp:Label Runat="server" id="LBnumeroInAttesa">
														&nbsp;<%# DataBinder.Eval(Container.DataItem, "oNumero") %>
													</asp:Label>
												</ItemTemplate>
											</asp:TemplateColumn>
											<asp:TemplateColumn runat="server" HeaderText="Matricola" ItemStyle-CssClass=ROW_TD_Small ItemStyle-Width="15" HeaderStyle-CssClass=ROW_TD_Small>
												<ItemTemplate>
													<asp:Label Runat="server" id="LBmatricolaInAttesa">
														&nbsp;<%# DataBinder.Eval(Container.DataItem, "oMatricola") %>
													</asp:Label>
												</ItemTemplate>
											</asp:TemplateColumn>
											<asp:BoundColumn DataField="PRSN_cognome" HeaderText="Cognome" ItemStyle-CssClass=ROW_TD_Small HeaderStyle-CssClass=ROW_TD_Small></asp:BoundColumn>
											<asp:BoundColumn DataField="PRSN_nome" HeaderText="Nome" ItemStyle-CssClass=ROW_TD_Small HeaderStyle-CssClass=ROW_TD_Small></asp:BoundColumn>
											<asp:BoundColumn DataField="PRSN_mail" HeaderText="E-Mail" ItemStyle-CssClass=ROW_TD_Small HeaderStyle-CssClass=ROW_TD_Small></asp:BoundColumn>
											<asp:BoundColumn DataField="PRSN_login" HeaderText="Login" Visible="False" ItemStyle-CssClass=ROW_TD_Small HeaderStyle-CssClass=ROW_TD_Small></asp:BoundColumn>
											<asp:TemplateColumn HeaderText="Ruolo" ItemStyle-CssClass=ROW_TD_Small HeaderStyle-CssClass=ROW_TD_Small>
												<ItemTemplate>
													&nbsp;<%# DataBinder.Eval(Container.DataItem, "TPRL_nome") %>
												</ItemTemplate>
											</asp:TemplateColumn>
											<asp:BoundColumn DataField="oIscritto" HeaderText="Iscritto Il" ItemStyle-CssClass=ROW_TD_Small HeaderStyle-CssClass=ROW_TD_Small></asp:BoundColumn>
										</Columns>
									</asp:datagrid>
									<br/>
									<br/>
								</asp:tablecell>
							</asp:TableRow>
						</asp:Table>
					</td>
				</tr>
			</table>
		</form>
	</body>
</html>
