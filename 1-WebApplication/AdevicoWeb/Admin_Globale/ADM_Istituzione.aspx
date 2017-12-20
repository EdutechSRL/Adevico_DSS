<%@ Page Language="vb" AutoEventWireup="false"  MasterPageFile="~/Admin_Globale/AdminPortal.Master"
Codebehind="ADM_Istituzione.aspx.vb" Inherits="Comunita_OnLine.ADM_Istituzione"%>
<%@ MasterType VirtualPath="~/Admin_Globale/AdminPortal.Master" %>

<asp:Content ContentPlaceHolderID="HeadContent" runat="server" ID="Content1">
	<script type="text/javascript" language="javascript" >	
		function AggiornaForm(){
			valore = document.forms[0].<%=me.DDLstato.ClientID%>.options[document.forms[0].<%=me.DDLstato.ClientID%>.selectedIndex].value
			if (valore==193){
				document.forms[0].<%=me.DDLprovincia.ClientID%>.disabled=false
				if (document.forms[0].<%=me.DDLprovincia.ClientID%>.options[document.forms[0].<%=me.DDLprovincia.ClientID%>.selectedIndex].value ==0)
					document.forms[0].<%=me.DDLprovincia.ClientID%>.value =92
				return false;
				}
			else{
				document.forms[0].<%=me.DDLprovincia.ClientID%>.value =0
				document.forms[0].<%=me.DDLprovincia.ClientID%>.disabled=true
				return false;
				}
		}
	</script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="CPHservice" runat="server">
	<table id="table1" cellSpacing="0" cellPadding="0" width="900px" align="center" border="0">
        <tr class="contenitore">
			<td colSpan="3">
				<table cellSpacing="0" cellPadding="0" width="100%" border="0">
<%--					<tr>
						<td class="RigaTitoloAdmin" align="left">
							<asp:label id="LBtitoloServizio" Runat="server">Gestione Istituzioni</asp:label>
						</td>
					</tr>--%>
					<tr>
						<td align=right>
							<asp:Panel ID="PNLmenu" Runat=server HorizontalAlign=Right>
								<asp:LinkButton id="LNBnuovo" runat="server" CausesValidation="False" CssClass="LINK_MENU">Nuova istituzione</asp:LinkButton>
							</asp:Panel>
							<asp:Panel ID="PNLmenuAzione" Runat=server HorizontalAlign=Right Visible=False >
								<asp:LinkButton id="LNBannulla" runat="server" CausesValidation="False" CssClass="LINK_MENU">Annulla</asp:LinkButton>
								&nbsp;
								<asp:LinkButton id="LNBinserisci" runat="server" CssClass="LINK_MENU">Salva</asp:LinkButton>
								<asp:LinkButton id="LNBmodifica" runat="server" CssClass="LINK_MENU">Salva</asp:LinkButton>
							</asp:Panel>
						</td>
					</tr>
					<tr>
						<td vAlign="top" align="center"><br/>
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
							</asp:panel><asp:panel id="PNLcontenuto" Runat="server" HorizontalAlign="Center">
								<asp:panel id="PNLlista" Runat="server">
									<table align="center">
										<tr>
											<td>
												<asp:datagrid id="DGIstituzione" runat="server"
													ShowFooter="false"
													AutoGenerateColumns="False" AllowPaging="True"
													DataKeyField="ISTT_id" AllowSorting="True"
													CssClass="DataGrid_Generica">
													<AlternatingItemStyle CssClass="ROW_Alternate_Small"></AlternatingItemStyle>
													<HeaderStyle CssClass="ROW_header_Small_Center"></HeaderStyle>
													<ItemStyle CssClass="ROW_Normal_Small" Height="22px"></ItemStyle>
													<PagerStyle CssClass="ROW_Page_Small" Position=Bottom Mode="NumericPages" Visible="true" HorizontalAlign="Right" Height="18px" VerticalAlign="Bottom"></PagerStyle>
													<Columns>
														<asp:TemplateColumn runat="server" HeaderText="" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="10">
															<ItemTemplate>
																<asp:ImageButton id="IMBmodifica" Runat="server" CausesValidation="False" CommandName="modifica" ImageUrl="../images/m.gif"></asp:ImageButton>
																<asp:ImageButton id="IMBCancella" Runat="server" CausesValidation="False" CommandName="elimina" ImageUrl="../images/x.gif"></asp:ImageButton>
															</ItemTemplate>
														</asp:TemplateColumn>
														<asp:BoundColumn HeaderText="RagioneSociale" DataField="ISTT_ragioneSociale" SortExpression="ISTT_ragioneSociale" headerStyle-CssClass=ROW_Header_Small_center ItemStyle-CssClass=ROW_TD_Small></asp:BoundColumn>
														<asp:BoundColumn DataField="ISTT_indirizzo" HeaderText="Indirizzo" Visible="true" headerStyle-CssClass=ROW_Header_Small_center ItemStyle-CssClass=ROW_TD_Small></asp:BoundColumn>
														<asp:BoundColumn DataField="ISTT_citta" HeaderText="Città" Visible="true" headerStyle-CssClass=ROW_Header_Small_center ItemStyle-CssClass=ROW_TD_Small></asp:BoundColumn>
														<asp:BoundColumn DataField="ISTT_telefono1" HeaderText="Telefono" Visible="true" headerStyle-CssClass=ROW_Header_Small_center ItemStyle-CssClass=ROW_TD_Small></asp:BoundColumn>
														<asp:BoundColumn DataField="ISTT_homePage" HeaderText="Home Page" Visible="true" headerStyle-CssClass=ROW_Header_Small_center ItemStyle-CssClass=ROW_TD_Small></asp:BoundColumn>
													</Columns>
												</asp:datagrid>
											</td>
										</tr>
									</table>
								</asp:panel>
								<asp:panel id="PNLnorecord" Runat="server" Visible="false">
									<table width="550" align="center">
										<tr>
											<td height="20">&nbsp;</td>
										</tr>
										<tr>
											<td align="center">
												<asp:Label id="LBnorecord" Runat="server" CssClass="smsStyle_Errore"></asp:Label></td>
										</tr>
										<tr>
											<td height="20">&nbsp;</td>
										</tr>
									</table>
								</asp:panel>
								<asp:panel id="PNLinsMod" Runat="server" Visible="False">
									<INPUT id="HDistt_id" type="hidden" runat="server" NAME="HDistt_id"/>
									<asp:table ID="Table1" cellSpacing="0" cellPadding="0" HorizontalAlign="center" Runat="server" Width=700px>
										<asp:tableRow>
											<asp:tableCell width="150px">
												<asp:Label ID="LBragionesociale_t" Runat=server CssClass="Titolo_campoSmall">*Ragione Sociale:</asp:Label>
											</asp:tableCell>
											<asp:tableCell>
												<asp:textbox id="TBragioneSociale" Runat="server" CssClass="Testo_Campo_obbligatorioSmall" MaxLength="100" Columns="60"></asp:textbox>
												<asp:requiredfieldvalidator id="Requiredfieldvalidator1" runat="server" CssClass="Validatori" ControlToValidate="TBragioneSociale" Display="static">*</asp:requiredfieldvalidator>
											</asp:tableCell>
										</asp:tableRow>
										<asp:tableRow>
											<asp:tableCell width="150px">
												<asp:Label ID="LBindirizzo_t" Runat=server CssClass="Titolo_campoSmall">*Indirizzo:</asp:Label>
											</asp:tableCell>
											<asp:tableCell>
												<asp:textbox id="TBindirizzo" Runat="server" CssClass="Testo_Campo_obbligatorioSmall" MaxLength="50" Columns="60"></asp:textbox>
												<asp:requiredfieldvalidator id="Requiredfieldvalidator3" runat="server" CssClass="Validatori" ControlToValidate="TBindirizzo" Display="Dynamic">*</asp:requiredfieldvalidator>
											</asp:tableCell>
										</asp:tableRow>
										<asp:tableRow>
											<asp:tableCell width="150px">
												<asp:Label ID="LBcap_t" Runat=server CssClass="Titolo_campoSmall">*Cap:</asp:Label>
											</asp:tableCell>
											<asp:tableCell>
												<asp:textbox id="TBcap" Runat="server" CssClass="Testo_Campo_obbligatorioSmall" MaxLength="5" Columns="7"></asp:textbox>
												<asp:requiredfieldvalidator id="Requiredfieldvalidator7" runat="server" CssClass="Validatori" ControlToValidate="TBcap" Display="dynamic">*</asp:requiredfieldvalidator>
												<asp:regularexpressionvalidator id="Cap" runat="server" CssClass="Validatori" ControlToValidate="TBcap" Display="dynamic" ValidationExpression="^\d{5}$">*</asp:regularexpressionvalidator>
											</asp:tableCell>
										</asp:tableRow>
										<asp:tableRow>
											<asp:tableCell width="150px">
												<asp:Label ID="LBcitta_t" Runat=server CssClass="Titolo_campoSmall">*Città:</asp:Label>
											</asp:tableCell>
											<asp:tableCell>
												<asp:textbox id="TBcitta" Runat="server" CssClass="Testo_Campo_obbligatorioSmall" MaxLength="25" Columns="40"></asp:textbox>
												<asp:requiredfieldvalidator id="Requiredfieldvalidator4" runat="server" CssClass="Validatori" ControlToValidate="TBcitta" Display="static">*</asp:requiredfieldvalidator>
											</asp:tableCell>
										</asp:tableRow>
										<asp:tableRow>
											<asp:tableCell width="150px">
												<asp:Label ID="LBstato_t" Runat=server CssClass="Titolo_campoSmall">*Stato:</asp:Label>
											</asp:tableCell>
											<asp:tableCell>
												<asp:dropdownlist id="DDLstato" Runat="server" CssClass="Testo_Campo_obbligatorioSmall" Width="258px" AutoPostBack="true"></asp:dropdownlist>
											</asp:tableCell>
										</asp:tableRow>
										<asp:tableRow>
											<asp:tableCell width="150px">
												<asp:Label ID="LBprovincia_t" Runat=server CssClass="Titolo_campoSmall">*Provincia:</asp:Label>
											</asp:tableCell>
											<asp:tableCell>
												<asp:dropdownlist id="DDLprovincia" Runat="server" CssClass="Testo_Campo_obbligatorioSmall" Width="258px"></asp:dropdownlist>
											</asp:tableCell>
										</asp:tableRow>
										<asp:tableRow>
											<asp:tableCell width="150px">
												<asp:Label ID="LBtelefono1_t" Runat=server CssClass="Titolo_campoSmall">*Telefono 1:</asp:Label>
											</asp:tableCell>
											<asp:tableCell>
												<asp:textbox id="TBtelefono1" Runat="server" CssClass="Testo_Campo_obbligatorioSmall" MaxLength="25" Columns="50"></asp:textbox>
												<asp:requiredfieldvalidator id="phoneReqVal" runat="server" CssClass="Validatori" ControlToValidate="TBtelefono1" Display="dynamic">*</asp:requiredfieldvalidator>
											</asp:tableCell>
										</asp:tableRow>
										<asp:tableRow>
											<asp:tableCell width="150px">
												<asp:Label ID="LBtelefono2_t" Runat=server CssClass="Titolo_campoSmall">*Telefono 2:</asp:Label>
											</asp:tableCell>
											<asp:tableCell>
												<asp:textbox id="TBtelefono2" Runat="server" CssClass="Testo_CampoSmall" MaxLength="25" Columns="50"></asp:textbox>
											</asp:tableCell>
										</asp:tableRow>
										<asp:tableRow>
											<asp:tableCell width="150px">
												<asp:Label ID="LBfax_t" Runat=server CssClass="Titolo_campoSmall">*Fax:</asp:Label>
											</asp:tableCell>
											<asp:tableCell>
												<asp:textbox id="TBfax" Runat="server" CssClass="Testo_CampoSmall" MaxLength="25" Columns="50"></asp:textbox>
											</asp:tableCell>
										</asp:tableRow>
										<asp:tableRow>
											<asp:tableCell width="150px">
												<asp:Label ID="LBhomepage_t" Runat=server CssClass="Titolo_campoSmall">Home page:</asp:Label>
											</asp:tableCell>
											<asp:tableCell>
												<asp:textbox id="TBhomePage" Runat="server" CssClass="Testo_CampoSmall" MaxLength="250" Columns="60"></asp:textbox>
											</asp:tableCell>
										</asp:tableRow>
										<asp:tableRow>
											<asp:tableCell width="150px">
												<asp:Label ID="LBuniversita_t" Runat=server CssClass="Titolo_campoSmall">Università:</asp:Label>
											</asp:tableCell>
											<asp:tableCell>
												<asp:CheckBox id="CHBisUniversità" Runat="server" CssClass="Testo_CampoSmall"></asp:CheckBox>
											</asp:tableCell>
										</asp:tableRow>
										<asp:tableRow id="TBRinserisciLogo">
											<asp:tableCell width="150px">
												<asp:Label ID="LBlogo_t" Runat="server" cssclass="Titolo_campoSmall">&nbsp; Logo:</asp:Label>
											</asp:tableCell>
											<asp:tableCell>
												<INPUT id="INlogo" type="file" name="INlogo" runat="server"/></asp:tableCell>
										</asp:tableRow>
										<asp:tableRow id="TBRmodificaLogo" Runat =server visible=False  >
											<asp:tableCell width="150" Height="22px" CssClass="top">
												<asp:label ID="LBlogoModifica_t" Runat=server CssClass="Titolo_campoSmall">&nbsp;Logo(356x43):</asp:Label>
											</asp:tableCell>
											<asp:tableCell>
												<fieldset>
													<table align =center >
														<tr>
															<td>
																<asp:image id="IMFoto" Runat="server" Visible="False" ToolTip="Immagine Personale" Width="356px" Height="43px"></asp:image>
															</td>
														</tr>
														<tr>
															<td>
																<input type=file id="FILElogoModifica" runat=server Class="Testo_campoSmall" size=60 NAME="FILElogoModifica"/>
																&nbsp;&nbsp;
																<asp:button id="BTNUploadFoto" runat="server" CssClass="pulsante" Text="Upload Foto" CausesValidation="False"></asp:button>&nbsp;
																<asp:button id="BTNCancella" runat="server" CssClass="pulsante" Text="Cancella Foto" CausesValidation="False"></asp:button>
															</td>
														</tr>
													</table>
												</fieldset>
											</asp:tableCell>
										</asp:tableRow>
									</asp:table>									
								</asp:panel>
								<asp:validationsummary id="VLDSum" runat="server" HeaderText="Attenzione! Sono state rilevate delle imprecisioni nella compilazione del form. Controlla i valori inseriti in corrisponsenza degli *" ShowSummary="false" ShowMessageBox="true" DisplayMode="BulletList"></asp:validationsummary>
							</asp:panel>
						</td>
					</tr>
				</table>
			</td>
		</tr>
	</table>
</asp:Content>


<%--<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<HTML>
	<head runat="server">
		<title>Comunità On Line</title>
		<script type="text/javascript" language="Javascript" src="./../jscript/Generali.js"></script>
		<meta content="Microsoft Visual Studio.NET 7.0" name="GENERATOR"/>
		<meta content="Visual Basic 7.0" name="CODE_LANGUAGE"/>
		<meta content="JavaScript" name="vs_defaultClientScript"/>
		<LINK href="./../Styles.css" type="text/css" rel="stylesheet"/>
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema"/>
		

	</HEAD>
	<body>
		<form id="aspnetForm" runat="server">
		<asp:ScriptManager ID="SCMmanager" runat="server"></asp:ScriptManager>
        	<tr>
				<td colSpan="3"><HEADER:CTRLHEADER id="Intestazione" runat="server"></HEADER:CTRLHEADER></td>
			</tr>


			<FOOTER:CTRLFOOTER id="CTRLFooter" runat="server"></FOOTER:CTRLFOOTER>
		</form>
	</body>
</HTML>
--%>