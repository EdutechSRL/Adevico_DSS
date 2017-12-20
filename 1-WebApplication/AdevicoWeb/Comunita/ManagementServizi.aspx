<%@ Page Language="vb" AutoEventWireup="false"  MasterPageFile="~/AjaxPortal.Master" Codebehind="ManagementServizi.aspx.vb" Inherits="Comunita_OnLine.ManagementServizi"%>
<%@ MasterType VirtualPath="~/AjaxPortal.Master" %>

<%--
--%>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <script language="Javascript" src="./../jscript/generali.js" type="text/javascript"></script>
	<style type="text/css">
				
		#LayerLegenda
        {
	        border-right: black 1px solid;
	        border-top: black 1px solid;
	        font-size: xx-small;
	        left: 10px;
	        visibility: hidden;
	        margin: 2px;
	        border-left: black 1px solid;
	        width: 550px;
	        border-bottom: black 1px solid;
	        position: absolute;
	        top: 10px;
	        height: 100px;
	        background-color: lightblue;
	        text-align: left;
        }


		#layer1
        {
	        border-right: black 1px solid;
	        border-top: black 1px solid;
	        font-size: x-small;
	        left: 10px;
	        visibility: hidden;
	        margin: 2px;
	        border-left: black 1px solid;
	        width: 200px;
	        border-bottom: black 1px solid;
	        position: absolute;
	        top: 10px;
	        height: 100px;
	        background-color: lightblue;
	        margin-top: 2px;
	        margin-right: 2px;
	        margin-bottom: 2px;
	        margin-left: 2px;

        }
	
	</style>
    	<script language="javascript" type="text/javascript">
    	    function SelectMe(Me) {
    	        var HIDcheckbox, selezionati, LNBelimina;
    	        //eval('HIDcheckbox= this.document.forms[0].<%=HIDcheckbox.ClientID%>')
    	        HIDcheckbox = this.document.getElementById('<%=Me.HIDcheckbox.ClientID%>'); 
    	        selezionati = 0
    	        for (i = 0; i < document.forms[0].length; i++) {
    	            e = document.forms[0].elements[i];
    	            if (e.type == 'checkbox' && e.name.indexOf("CBXservizioAttivato") != -1) {
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
	</script>
	<script type="text/javascript" language="JavaScript" >
			<!--
	    function ChangeState(evt, layerRef, state, valore) {
	        var e = (window.event) ? window.event : evt;
	        var PosX, PosY;

	        PosX = e.clientX;
	        PosY = e.clientY + 10;
	        document.getElementById(layerRef).innerHTML = valore
	        document.getElementById(layerRef).style.visibility = state;
	        document.getElementById(layerRef).style.left = PosX;
	        document.getElementById(layerRef).style.top = PosY;
	    }
		//-->
	</script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="CPHservice" runat="server">
	<input type="hidden" id="HIDcheckbox" runat="server" NAME="HIDcheckbox"/>
	<input type="hidden" id="HDNpermessi" runat="server" NAME="HDNpermessi"/>
	<input type="hidden" id="HDNsrvz_ID" runat="server" NAME="HDNsrvz_ID"/>
	<div id="layer1"></div>
	<div id="LayerLegenda"></div>
	<table align=center width=900px>
<%--		<tr>
			<td class="RigaTitolo" align="left">
				<asp:Label id="LBtitolo" Runat="server" >Gestione Servizi</asp:Label>
			</td>
		</tr>--%>
		<tr>
			<td align="right" valign=middle>
				<asp:Panel ID="PNLmenu" Runat=server Visible=true HorizontalAlign=Right >
					<asp:LinkButton ID="LNBtoGestione" CausesValidation=False  Text="Alla pagina di gestione" Visible=False Runat=server CssClass="LINK_MENU"></asp:LinkButton>
					<asp:LinkButton id="LNBsalvaServizi" Runat="server" CssClass="LINK_MENU">Salva impostazioni</asp:LinkButton>
				</asp:Panel>
				<asp:Panel ID="PNLmenuSecondario" Runat=server Visible=False HorizontalAlign=Right>
					&nbsp;
					<asp:LinkButton id="LNBindietro" Runat="server" CssClass="LINK_MENU">Indietro</asp:LinkButton>&nbsp;
					<asp:LinkButton id="LNBsalvaImpostazioni" Runat="server" CssClass="LINK_MENU">Salva modifiche</asp:LinkButton>
					<asp:LinkButton id="LNBsalvaImpostazioniIndietro" Runat="server" CssClass="LINK_MENU">Salva e torna all'elenco</asp:LinkButton>
				</asp:Panel>
			</td>
		</tr>
		<tr>
			<td align=center >
				<asp:Panel ID="PNLpermessi" Runat="server" Visible="False" HorizontalAlign=Center >
					<br/><br/><br/>
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
				<asp:Panel ID="PNLservizi" Runat="server" Visible="true" HorizontalAlign=Center >
									
					<asp:Table runat="server" ID="TBLgenerica">
						<asp:TableRow ID="TBRutenteSelezionato" Visible=False >
							<asp:TableCell>
								<table border="1" align="center" width="800px" cellspacing=0 style="border-color:#CCCCCC; background-color:#fffbf7">
									<tr>
										<td>
											<asp:Table ID="TBLdatiPrincipali" Runat="server" Width="800px" CellPadding=0 CellSpacing=0 BorderStyle=none>
												<asp:TableRow>
													<asp:TableCell ColumnSpan=4>&nbsp;</asp:TableCell>
												</asp:TableRow>
												<asp:TableRow>
													<asp:TableCell>&nbsp;</asp:TableCell>
													<asp:TableCell ColumnSpan=2>
														<asp:Label ID="LBavviso" Runat=server CssClass ="avviso_normal">
														Se nella Comunità è attiva la "cover", cliccando su di essa di accede al servizio default.
														Se la Cover è disattivata si accede direttamente al servizio selezionato di default.
														</asp:Label>
													</asp:TableCell>
													<asp:TableCell>&nbsp;</asp:TableCell>
												</asp:TableRow>
												<asp:TableRow ID="TBRdefault">
													<asp:TableCell>&nbsp;</asp:TableCell>
													<asp:TableCell>
														<asp:Label ID="LBpaginaDefault_t" Runat=server CssClass="Titolo_campoSmall">Attiva all'accesso il servizio:</asp:Label>&nbsp;
														<asp:DropDownList ID="DDLpagineDefault" Runat=server cssClass="Testo_campoSmall"></asp:DropDownList>
													</asp:TableCell>
													<asp:TableCell HorizontalAlign=Right>
														<asp:Button ID="BTNmodifica" Runat=server Text="Modifica" CssClass=""></asp:Button>
														<asp:Button ID="BTNsalvaDefault" Runat=server Text="Rendi default" CssClass="" Visible=False ></asp:Button>
													</asp:TableCell>
													<asp:TableCell>&nbsp;</asp:TableCell>
												</asp:TableRow>
												<asp:TableRow ID="TBRprofilo" BorderColor=White>
													<asp:TableCell>&nbsp;</asp:TableCell>
													<asp:TableCell>
														<table>
															<tr>
																<td>
																	<asp:Label ID="LBsceltaServizio" Runat=server CssClass="Titolo_campoSmall"></asp:Label>
																	<asp:RadioButtonList ID="RBLsceltaServizio" Runat=server RepeatLayout=Flow RepeatDirection=Horizontal CssClass="Testo_campoSmall" AutoPostBack=True>
																		<asp:ListItem Value=0 Selected=True>Di sistema</asp:ListItem>	
																		<asp:ListItem Value=1>Personale:</asp:ListItem>
																	</asp:RadioButtonList>		
																</td>
																<td>
																	<asp:DropDownList ID="DDLprofilo" Runat=server CssClass="Testo_campoSmall" AutoPostBack=True>
																							
																	</asp:DropDownList>
																</td>
															</tr>
														</table>
													</asp:TableCell>
													<asp:TableCell HorizontalAlign=Right>
														<asp:Button ID="BTNcambiaProfilo" Runat=server Text="Cambia profilo" CssClass=""></asp:Button>
														<asp:Button ID="BTNannullaModificheProfilo" Runat=server Text="Annulla" CssClass="" Visible=False ></asp:Button>
														<asp:Button ID="BTNsalvaModificheProfilo" Runat=server Text="Salva modifiche" CssClass="" Visible=False ></asp:Button>
													</asp:TableCell>
													<asp:TableCell>&nbsp;</asp:TableCell>
												</asp:TableRow>
												<asp:TableRow>
													<asp:TableCell ColumnSpan=4>&nbsp;</asp:TableCell>
												</asp:TableRow>
											</asp:Table>
										</td>
									</tr>
								</table>
							</asp:TableCell>
						</asp:TableRow>
					</asp:Table>
					<asp:datagrid 
						id="DGServizi" Runat="server" 
						ShowHeader="true" AllowSorting="true" 
						GridLines=Vertical AutoGenerateColumns="False" 
						DataKeyField="SRVZ_ID" AllowPaging="true" PageSize=25 
						CssClass="DataGrid_Generica">
						<AlternatingItemStyle CssClass="ROW_Alternate_Small"></AlternatingItemStyle>
						<HeaderStyle CssClass="ROW_header_Small_Center"></HeaderStyle>
						<ItemStyle CssClass="ROW_Normal_Small" Height="22px"></ItemStyle>
						<PagerStyle CssClass="ROW_Page_Small" Position=TopAndBottom Mode="NumericPages" Visible="true" HorizontalAlign="Right" Height="18px" VerticalAlign="Bottom"></PagerStyle>
						<Columns>
							<asp:TemplateColumn headerStyle-CssClass=ROW_Header_Small_center>
								<ItemTemplate>
									<asp:Table ID="TBLdati" Runat=server>
										<asp:TableRow ID="TBRnome">
											<asp:TableCell>
												<asp:Label ID="LBnome" Runat=server><%# DataBinder.Eval(Container.DataItem, "SRVZ_nome") %></asp:Label>
												<asp:Label id="LBseparatore" Runat=server>&nbsp;|&nbsp;</asp:Label>
												<asp:LinkButton ID="LNBimpostazioni" CommandName="impostazioni" Runat="server" CssClass="ROW_ItemLink_Small">Impostazioni servizio</asp:LinkButton>
											</asp:TableCell>
										</asp:TableRow>
										<asp:TableRow ID="TBRdescrizione">
											<asp:TableCell>
												<asp:Label ID="LBdescrizione" Runat=server><%# DataBinder.Eval(Container.DataItem, "SRVZ_Descrizione") %></asp:Label>
											</asp:TableCell>
										</asp:TableRow>
									</asp:Table>
								</ItemTemplate>
							</asp:TemplateColumn>
							<asp:TemplateColumn runat="server" HeaderText="Active" headerStyle-CssClass=ROW_Header_Small_center ItemStyle-CssClass=ROW_TD_Small_center ItemStyle-Width=100px>
								<ItemTemplate>
									<input runat=server  type="checkbox" id="CBXservizioAttivato" name="CBXservizioAttivato"  onclick="SelectMe(this);"/>
								</ItemTemplate>
							</asp:TemplateColumn>
							<asp:BoundColumn DataField="isNonDisattivabile" Visible=False ></asp:BoundColumn>
							<asp:BoundColumn DataField="isAbilitato" HeaderText="" Visible="False"></asp:BoundColumn>
							<asp:BoundColumn DataField="SRVZ_ID" HeaderText="" Visible="False"></asp:BoundColumn>
							<asp:BoundColumn DataField="isDefault" HeaderText="" Visible="False"></asp:BoundColumn>
						</Columns>
					</asp:datagrid>
				</asp:Panel>
				<asp:Panel ID="PNLimpostazioni" Runat="server" Visible="False">
					<table width="850px" align="center">
						<tr>
							<td align="center">
								<table align="center">
									<tr>
										<td class="nosize" width="10">&nbsp;</td>
										<td align="left" colspan=2>
											<asp:RadioButtonList ID="RBLruoli" RepeatDirection=Horizontal Runat=server CssClass="FiltroCampoSmallSmall" AutoPostBack=True RepeatLayout=Flow >
												<asp:ListItem Value=0 Selected=True>Ruoli/permessi definiti</asp:ListItem>
												<asp:ListItem Value=1 Selected=True>Lista completa ruoli</asp:ListItem>
												<asp:ListItem Value=2 Selected=True>Ruoli/permessi di default</asp:ListItem>
											</asp:RadioButtonList>		
										</td>
										<td class="nosize" width="10">&nbsp;</td>
									</tr>
									<tr>
										<td class="nosize" width="10">&nbsp;</td>
										<td align="left" colspan=2>
											<table align="left">
												<tr>
													<td>
														<asp:Label ID="LBlegendaRuoli" Runat=server CssClass="LegendaSmall"></asp:Label>&nbsp;|&nbsp;
														<asp:Label ID="LBlegendaPermessi" Runat=server CssClass="LegendaSmall"></asp:Label>
													</td>
												</tr>
											</table>
										</td>
										<td class="nosize" width="10">&nbsp;</td>
									</tr>
									<tr>
										<td class="nosize" width="10">&nbsp;</td>
										<td align="center" colspan=2>
											<asp:Table Runat=server HorizontalAlign=left ID="TBLpermessiRuoli" GridLines=Both  CellSpacing="0" CellPadding="3" BorderColor="#2A4DA1" Width=850px>
																				
											</asp:Table>
										</td>
										<td class="nosize" width="10">&nbsp;</td>
									</tr>
									<tr>
										<td class="nosize" colSpan="4" height="5">&nbsp;</td>
									</tr>
								</table>
							</td>
						</tr>
					</table>
				</asp:Panel>
				<br/>
			</td>
		</tr>
	</table>
</asp:Content>

<%--<!DOCTYPE HTML PUBLIC "-//W3C//Dtd HTML 4.0 transitional//EN">
<HTML>
	<head runat="server">
		<title>Comunità On Line - Gestione Servizi</title>
		<meta name="GENERATOR" content="Microsoft Visual Studio .NET 7.1"/>
		<meta name="CODE_LANGUAGE" content="Visual Basic .NET 7.1"/>
		<meta name="vs_defaultClientScript" content="JavaScript"/>
		<meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5"/>
		<LINK href="./../Styles.css" type="text/css"rel="stylesheet"/>
		
		
	</HEAD>

	<body >
		<form id="aspnetForm" method="post" runat="server">
		<asp:ScriptManager ID="SCMmanager" runat="server"></asp:ScriptManager>
			<table cellSpacing="0" cellPadding="0" width="900px" align="center" border="0">
				<tr>
					<td colSpan="3">
						<HEADER:CtrLHEADER id="Intestazione" runat="server"></HEADER:CtrLHEADER>
					</td>
				</tr>
				<tr>
					<td colSpan="3" align="center">

					</td>
				</tr>
			</table>
			<FOOTER:CtrLFOOTER id="CtrLFooter" runat="server"></FOOTER:CtrLFOOTER>
		</form>
	</body>
</HTML>--%>