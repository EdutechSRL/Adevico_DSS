<%@ Page Language="vb" AutoEventWireup="false" Codebehind="StampaIscrittiAvanzata.aspx.vb" Inherits="Comunita_OnLine.StampaIscrittiAvanzata"%>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<html>
	<head runat="server">
		<title>StampaIscrittiAvanzata</title>
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
			<table align="left" width="100%" cellpadding=0 cellspacing=0 border=0>
				<tr>
					<td colspan="2" align="left" class=RigaTitolo>
						<asp:Label ID="LBtitolo" Runat="server"></asp:Label>
					</td>
				</tr>
				<tr>
					<td colspan="2" align="right">
						<asp:Button Runat="server" ID="BTNOk" Text="Chiudi" CssClass="PulsantiMenu_Bold"></asp:Button>
						<asp:Button Runat="server" ID="BTNstampa" Text="Stampa" CssClass="PulsantiMenu_Bold"></asp:Button>
					</td>
				</tr>
				<tr align=left>
					<td colspan="2" align="left">
						<table>
							<tr>
								<td class=top>
									<asp:Label ID=LBcolonne runat=server CssClass=FiltroVoceSmall>Colonne Visualizzate:</asp:Label>		
								</td>
								<td class=top>
									<asp:CheckBoxList ID=CHLcolonne Runat=server CssClass="FiltroCampoSmall" RepeatColumns =4  RepeatDirection=Horizontal AutoPostBack=True RepeatLayout=Flow >
										<asp:ListItem Value=1 Selected=True >Matricola</asp:ListItem>
										<asp:ListItem Value=2 Selected=True >Anagrafica</asp:ListItem>
										<asp:ListItem Value=3 Selected=True >Login</asp:ListItem>
										<asp:ListItem Value=4 Selected=True >E-mail</asp:ListItem>
										<asp:ListItem Value=5 Selected=True >Ruolo</asp:ListItem>
										<asp:ListItem Value=6 Selected=false >Tipo Persona</asp:ListItem>
										<asp:ListItem Value=7 Selected=false >Ultimo Collegamento</asp:ListItem>
										<asp:ListItem Value=8 Selected=True >Iscritto Il</asp:ListItem>
									</asp:CheckBoxList>
								</td>
							</tr>
						</table>
					</td>
				</tr>
				<tr>
					<td align="left" colspan="2">
					
						<asp:datagrid 
						    id="DGiscritti" runat="server" 
						    AllowSorting="true" 
						    ShowFooter="false" 
							AutoGenerateColumns="False"
							AllowPaging="true" 
							DataKeyField="RLPC_ID" 
							PageSize="30"
							CssClass="DataGrid_Generica">
							<HeaderStyle CssClass="ROW_header_Small_Center"></HeaderStyle>
							<ItemStyle CssClass="ROW_Normal_Small" Height="22px"></ItemStyle>
							<PagerStyle CssClass="ROW_Page_Small" Position=TopAndBottom Mode="NumericPages" Visible="true"
															HorizontalAlign="Right" Height="18px" VerticalAlign="Bottom"></PagerStyle>
							<Columns>
								<asp:BoundColumn DataField="RLPC_ID" HeaderText="" Visible="false"></asp:BoundColumn>
								<asp:BoundColumn DataField="Matricola" HeaderText="Matricola" ItemStyle-CssClass=ROW_TD_Small headerStyle-CssClass=ROW_header_Small></asp:BoundColumn>
								<asp:BoundColumn DataField="PRSN_Anagrafica" HeaderText="Anagrafica" SortExpression="PRSN_Anagrafica" ItemStyle-CssClass=ROW_TD_Small></asp:BoundColumn>
								<asp:BoundColumn DataField="PRSN_login" HeaderText="Login" SortExpression="PRSN_login" ItemStyle-CssClass=ROW_TD_Small></asp:BoundColumn>
								<asp:TemplateColumn runat="server" HeaderText="Mail" ItemStyle-CssClass=ROW_TD_Small
									SortExpression="PRSN_mail">
									<ItemTemplate>
										<asp:HyperLink NavigateUrl='<%# "mailto:" & Container.Dataitem("PRSN_mail")%>' text='<%# Container.Dataitem("PRSN_mail")%>' Runat="server" ID="HYPMail" />
									</ItemTemplate>
								</asp:TemplateColumn>
								<asp:BoundColumn DataField="TPRL_nome" HeaderText="Ruolo" SortExpression="TPRL_nome" ItemStyle-CssClass=ROW_TD_Small></asp:BoundColumn>
								<asp:BoundColumn DataField="TPPR_descrizione" HeaderText="Tipo Persona" SortExpression="TPPR_descrizione"
									ItemStyle-CssClass=ROW_TD_Small></asp:BoundColumn>
								<asp:BoundColumn DataField="oRLPC_ultimoCollegamento" HeaderText="Last visit" SortExpression="RLPC_ultimoCollegamento"
									  ItemStyle-CssClass=ROW_TD_Small></asp:BoundColumn>
								<asp:BoundColumn DataField="oIscrittoIl" HeaderText="Iscritto il" SortExpression="RLPC_IscrittoIl"
									visible="true" ItemStyle-CssClass=ROW_TD_Small></asp:BoundColumn>
								<asp:BoundColumn DataField="PRSN_TPPR_id" HeaderText="idtipopersona" SortExpression="PRSN_TPPR_id"
									Visible="False"></asp:BoundColumn>
								<asp:BoundColumn DataField="TPRL_gerarchia" visible="false"></asp:BoundColumn>
								<asp:BoundColumn DataField="PRSN_ID" visible="false"></asp:BoundColumn>
								<asp:BoundColumn DataField="RLPC_Attivato" visible="false"></asp:BoundColumn>
								<asp:BoundColumn DataField="RLPC_Abilitato" visible="false"></asp:BoundColumn>
								<asp:BoundColumn DataField="RLPC_Responsabile" visible="false"></asp:BoundColumn>
								<asp:BoundColumn DataField="TPRL_id" HeaderText="idRuolo" Visible="False"></asp:BoundColumn>
							</Columns>
							<PagerStyle Width="700px" PageButtonCount="5" mode="NumericPages"></PagerStyle>
						</asp:datagrid>
					</td>
				</tr>
				
			</table>
		</form>
	</body>
</html>
