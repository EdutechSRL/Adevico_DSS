<%@ Page Language="vb" AutoEventWireup="false" MasterPageFile="~/AjaxPortal.Master" Codebehind="ModificaEvento.aspx.vb" Inherits="Comunita_OnLine.Modifica_Evento"%>
<%@ MasterType VirtualPath="~/AjaxPortal.Master" %>
<%@ Register Src="~/Modules/Common/Editor/UC_Editor.ascx" TagName="CTRLeditor" TagPrefix="CTRL" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <script language="Javascript" src="./../jscript/generali.js" type="text/javascript"></script>
	<style type=text/css>@import url(./../Jscript/Calendar/calendar-blue.css);</style>
	<script type=text/javascript src="./../Jscript/Calendar/calendar.js"></script>
		
	<script type=text/javascript src="./../Jscript/Calendar/calendar-setup.js"></script>
    <%=CalendarScript() %>
    <script language="javascript" type="text/javascript">
		    function AggiornaForm(){
			    valore = document.aspnetForm .<%=me.DDLCategoria.ClientID%>.options[document.aspnetForm .<%=me.DDLCategoria.ClientID%>.selectedIndex].value;
			    if (valore==1){
				    document.aspnetForm .<%=me.HDNselezionato.ClientID%>.value = valore
				    __doPostBack('DDLCategoria','');
				    return true;
				    }
			    else if (valore==0){
				    document.aspnetForm .<%=me.HDNselezionato.ClientID%>.value = valore
				    __doPostBack('DDLCategoria','');
				    return true;
				    }
			    else if (document.aspnetForm .<%=me.HDNselezionato.ClientID%>.value == 0){
				    document.aspnetForm .<%=me.HDNselezionato.ClientID%>.value = valore
				    __doPostBack('DDLCategoria','');
				    return true;
				    }
			    else if (document.aspnetForm .<%=me.HDNselezionato.ClientID%>.value == 1){
				    document.aspnetForm .<%=me.HDNselezionato.ClientID%>.value = valore
				    __doPostBack('DDLCategoria','');
				    return true;
				    }	
			    document.aspnetForm .<%=me.HDNselezionato.ClientID%>.value = valore
			    return false;
		    }
		</script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="CPHservice" runat="server">
    <asp:label ID="LBDataCreazione" Runat="server" Visible="False" />
    <table cellSpacing="0" cellPadding="0"  width="900px" border="0">
		<tr>
			<td Class="RigaTitolo" align=left>
				<asp:label id="LBtitolo" Runat="server">Modifica Evento</asp:label>
			</td>
		</tr>
		<tr>
			<td align=right>
				<asp:linkbutton ID="LNBcalendario" Runat="server" Text="Al calendario" CausesValidation="false" CssClass="LINK_MENU"></asp:linkbutton>
				<asp:linkbutton ID="LNBricerca" Runat="server" Text="Torna alla ricerca" CausesValidation="false" CssClass="LINK_MENU"></asp:linkbutton>
				&nbsp;
				<asp:linkbutton ID="LNBmodifica" Runat="server" Text="Salva" CssClass="LINK_MENU"></asp:linkbutton>
			</td>
		</tr>
		<tr>
			<td>
				<input type=hidden id="HDNselezionato" runat=server NAME="HDNselezionato"/>
				<asp:panel id="PNLpermessi" Runat="server" Visible="False">
					<table align="center">
						<tr>
							<td height="50">&nbsp;</td>
						</tr>
						<tr>
							<td align="center">
								<asp:Label id="LBnopermessi" CssClass="messaggio" ForeColor="Blue" Runat="server"></asp:Label></td>
						</tr>
						<tr>
							<td vAlign="top" align="center" height="50">&nbsp;</td>
						</tr>
					</table>
				</asp:panel>
				<asp:Panel ID="PNLcontenuto" Runat="server" HorizontalAlign="Center">
					<asp:Panel ID="PNLMain" Runat="server">
						<asp:Table ID="TBLdati" Runat=server Width=700px GridLines=None HorizontalAlign=Center>
							<asp:TableRow>
								<asp:TableCell Width=110px Wrap=false>
									<asp:label id="LBNomeEvento" Runat="server" CssClass="Titolo_campoSmall" text="Nome dell'evento:"></asp:label>
								</asp:TableCell>
								<asp:TableCell>
									<asp:textbox id="TXBNomeEvento" Runat="server" CssClass="Testo_campo_obbligatorioSmall" Columns=100 MaxLength=100></asp:textbox>
									<asp:RequiredFieldValidator runat="server" Display="Dynamic" controltovalidate="TXBNomeEvento" errormessage="Nome Evento Richiesto" ID="RFVNomeEvento">*</asp:RequiredFieldValidator>
									<asp:RegularExpressionValidator Runat="server" Display="Dynamic" ControlToValidate="TXBNomeEvento" ErrorMessage="Nome Evento troppo lungo (max 100 caratteri)" ID="REVNomeEvento" ValidationExpression=".{0,100}"></asp:RegularExpressionValidator>
								</asp:TableCell>
							</asp:TableRow>	
							<asp:TableRow>
								<asp:TableCell Width=110px Wrap=false>
									<asp:label id="LBCategoria" Runat="server" CssClass="Titolo_campoSmall" text="Categoria:"></asp:label>
								</asp:TableCell>
								<asp:TableCell>
									<table border=0 cellpadding=0 cellspacing=0>
										<tr>
											<td>
												<asp:dropdownlist id="DDLCategoria" runat="server" CssClass="Testo_campo_obbligatorioSmall" AutoPostBack="true"></asp:dropdownlist>		
											</td>
											<td>&nbsp;&nbsp;&nbsp;&nbsp;</td>
											<td>
												<asp:label id="LBvisualizzazione" Runat="server" CssClass="Titolo_campoSmall" text="Visualizzazione:"></asp:label>
												&nbsp;
												<asp:checkbox id="CBVisibile" runat="server" CssClass="Testo_campoSmall" Text="Visibile" Checked="True"></asp:checkbox>
											</td>
										</tr>
									</table>
								</asp:TableCell>
							</asp:TableRow>
							<asp:TableRow ID="TBRdate">
								<asp:TableCell Width=110px Wrap=false>
									<asp:label id="LBdataInizio" Runat="server" CssClass="Titolo_campoSmall" text="Data Inizio:"></asp:label>
								</asp:TableCell>
								<asp:TableCell>
									<table border=0 cellpadding=0 cellspacing=0>
										<tr>
											<td>
												<asp:label id="LBdataI" Runat="server" CssClass="Testo_campoSmall" text=""></asp:label>&nbsp;
												<asp:ImageButton ID="IMBapriInizio" ImageUrl="../images/cal.gif" Runat="server" CausesValidation="False"></asp:ImageButton>
												<input type=hidden id="HDNdataI" runat=server NAME="HDNdataI"/>
											</td>
											<td>
												&nbsp;&nbsp;&nbsp;&nbsp;
											</td>
											<td>
												<asp:label id="LBDataFine" Runat="server" CssClass="Titolo_campoSmall" text="Data Fine:"></asp:label>
												<asp:label id="LBFineRpetizione" Visible="false" Runat="server" CssClass="Titolo_campoSmall" text="Fine Ripetizione:"></asp:label>
											</td>
											<td>&nbsp;</td>
											<td>
												<asp:label id="LBdataF" Runat="server" CssClass="Testo_campoSmall" text=""></asp:label>&nbsp;
												<asp:ImageButton ID="IMBapriFine" ImageUrl="../images/cal.gif" Runat="server" CausesValidation="False"></asp:ImageButton>
												<input type=hidden id="HDNdataF" runat=server NAME="HDNdataF"/>
												<script type=text/javascript>
													function selectInit(calendar, date) {
														var esiste = false;
														if (calendar.dateClicked) {
															dataI = date
															dataF = document.getElementById("<%=me.HDNdataF.clientID%>").value
															dataIniziale = dataI.split("/")
															dataFinale = dataF.split("/")
															var dataInizio = new Date()
															var dataFine = new Date()
															dataInizio = Date.parse(dataIniziale[1] + '/' + dataIniziale[0] + '/' + dataIniziale[2])
															dataFine = Date.parse(dataFinale[1] + '/' + dataFinale[0] + '/' + dataFinale[2])
															if (dataInizio > dataFine) {
																document.getElementById("<%=me.HDNdataF.clientID%>").value = date
																document.getElementById("<%=me.LBdataF.clientID%>").innerHTML = date
															}
															document.getElementById("<%=me.HDNdataI.clientID%>").value = date
															document.getElementById("<%=me.LBdataI.clientID%>").innerHTML = date
															calendar.callCloseHandler()
														}

													}
													function selectEnd(calendar, date) {

														if (calendar.dateClicked) {
															dataI = document.getElementById("<%=me.HDNdataI.clientID%>").value;
															dataF = date;
															dataIniziale = dataI.split("/")
															dataFinale = dataF.split("/")
															var dataInizio = new Date()
															var dataFine = new Date()
															dataInizio = Date.parse(dataIniziale[1] + '/' + dataIniziale[0] + '/' + dataIniziale[2])
															dataFine = Date.parse(dataFinale[1] + '/' + dataFinale[0] + '/' + dataFinale[2])

															if (dataInizio > dataFine) {
																document.getElementById("<%=me.HDNdataI.clientID%>").value = date
																document.getElementById("<%=me.LBdataI.clientID%>").innerHTML = date
															}
															document.getElementById("<%=me.HDNdataF.clientID%>").value = date
															document.getElementById("<%=me.LBdataF.clientID%>").innerHTML = date
															calendar.callCloseHandler()
														}

													}
													Calendar.setup({
														ifFormat: "%d/%m/%Y",
														inputField: "<%=me.HDNdataI.clientID%>",
														displayArea: "<%=me.LBdataI.clientID%>",
														button: "<%=me.IMBapriInizio.clientID%>",
														firstDay: 1,
														onSelect: selectInit
													}
			);

													Calendar.setup({
														ifFormat: "%d/%m/%Y",
														inputField: "<%=me.HDNdataF.clientID%>",
														displayArea: "<%=me.LBdataF.clientID%>",
														button: "<%=me.IMBapriFine.clientID%>",
														firstDay: 1,
														onSelect: selectEnd


													}
			);
	</script>
											</td>
										</tr>
									</table>
								</asp:TableCell>
							</asp:TableRow>
							<asp:TableRow ID="TBRore">
								<asp:TableCell Width=110px Wrap=false>
									<asp:label id="LBLOraInizio" Runat="server" CssClass="Titolo_campoSmall" text="Ora Inizio:"></asp:label>
								</asp:TableCell>
								<asp:TableCell>
									<table border=0 cellpadding=0 cellspacing=0>
										<tr>
											<td>
												<asp:dropdownlist id="DDLOra1" runat="server" Visible="true" CssClass="Testo_campoSmall"></asp:dropdownlist>
												&nbsp;:&nbsp;<asp:dropdownlist id="DDLMinuti1" runat="server" Visible="true" CssClass="Testo_campoSmall"></asp:dropdownlist>
											</td>
											<td>
												&nbsp;&nbsp;&nbsp;&nbsp;
											</td>
											<td>
												<asp:label id="LBLOraFine" Runat="server" CssClass="Titolo_campoSmall" text="Ora Fine: "></asp:label>
											</td>
											<td>&nbsp;</td>
											<td>
												<asp:dropdownlist id="DDLOra2" runat="server" Visible="true" CssClass="Testo_campoSmall"></asp:dropdownlist>
												&nbsp;:&nbsp;
												<asp:dropdownlist id="DDLMinuti2" runat="server" Visible="true" CssClass="Testo_campoSmall"></asp:dropdownlist>
											</td>
										</tr>
									</table>
								</asp:TableCell>
							</asp:TableRow>
							<asp:TableRow ID="TBRluogo">
								<asp:TableCell>
									<asp:label id="LBLuogo" Runat="server" CssClass="Titolo_campoSmall" text="Luogo:"></asp:label>
								</asp:TableCell>
								<asp:TableCell ColumnSpan=3>
									<asp:textbox id="TBLuogo" Runat="server" CssClass="Testo_campoSmall" Width="100%"></asp:textbox>
									<asp:RegularExpressionValidator Runat="server" Display="Dynamic" ControlToValidate="TBLuogo" ErrorMessage="Luogo Evento troppo lungo (max 200 caratteri)" ID="REVLuogo" ValidationExpression=".{0,200}"></asp:RegularExpressionValidator>
								</asp:TableCell>
							</asp:TableRow>
							<asp:TableRow ID="TBRaula">
								<asp:TableCell Width=110px Wrap=false>
									<asp:label id="LBAula" Runat="server" CssClass="Titolo_campoSmall" text="Aula:"></asp:label>
								</asp:TableCell>
								<asp:TableCell>
									<asp:textbox id="TBAula" Runat="server" CssClass="Testo_campoSmall" Width="100%"></asp:textbox>
									<asp:RegularExpressionValidator Runat="server" Display="Dynamic" ControlToValidate="TBAula" ErrorMessage="Nome Aula troppo lungo (max 50 caratteri)" ID="REVAula" ValidationExpression=".{0,50}"></asp:RegularExpressionValidator>
								</asp:TableCell>
							</asp:TableRow>
							<asp:TableRow ID="TBReditorProgramma" Visible=False >
								<asp:TableCell Width=110px Wrap=false CssClass=top>
									<asp:label id="LBProgramma_t" Runat="server" CssClass="Titolo_campoSmall" text="Programma:"></asp:label>
								</asp:TableCell>
								<asp:TableCell CssClass=top>
									<table cellpadding=0 cellspacing=0 border=0>
										<tr>
											<td width="550" style="border-color:Black">
												<asp:Table ID="TBLprogramma" Runat=server GridLines=Both BorderColor=#000000 BorderWidth=1px height="100" width="550"  CellPadding=0 CellSpacing=0>
													<asp:TableRow>
														<asp:TableCell CssClass=top>
															<asp:Label ID="LBProgramma" Runat=server CssClass="Testo_campoSmall" height="100" width="550" BorderStyle=Solid BorderWidth=1px BorderColor=#000000></asp:Label>
														</asp:TableCell>
													</asp:TableRow>
												</asp:Table>
												  <CTRL:CTRLeditor id="CTRLeditorProgramma" runat="server" 
                                                                ContainerCssClass="containerclass" 
                                                                LoaderCssClass="loadercssclass" EditorHeight="100px" 
                                                                EditorWidth="550px"
                                                                AllAvailableFontnames="true" AutoInitialize="true" 
                                                                MaxHtmlLength="10000"
                                                                ModuleCode="SRVEVENTI">
                                                            </CTRL:CTRLeditor>
											</td>
											<td class=top nowrap="nowrap" >
												<asp:LinkButton ID="LNBmodificaProgramma" Runat=server CausesValidation=False CssClass="LinksmallNavy11">Modifica</asp:LinkButton>
												<asp:LinkButton ID="LNBsalvaProgramma" Runat=server CausesValidation=False CssClass="LinksmallNavy11">Salva</asp:LinkButton>
											</td>
										</tr>
									</table>
								</asp:TableCell>
							</asp:TableRow>
							<asp:TableRow ID="TBReditorNote"  Visible=False>
								<asp:TableCell Width=110px Wrap=false CssClass=top>
									<asp:label id="LBnote_t" Runat="server" CssClass="Titolo_campoSmall" text="Note:"></asp:label>
								</asp:TableCell>
								<asp:TableCell CssClass=top>
									<table cellpadding=0 cellspacing=0 border=0>
										<tr>
											<td width="550">
												<asp:Table ID="TBLnote" Runat=server GridLines=Both BorderColor=#000000 BorderWidth=1px height="100" width="550" CellPadding=0 CellSpacing=0>
													<asp:TableRow>
														<asp:TableCell CssClass=top>
															<asp:Label ID="LBNote" Runat=server CssClass="Testo_campoSmall" height="100" width="550"></asp:Label>
														</asp:TableCell>
													</asp:TableRow>
												</asp:Table>
												 <CTRL:CTRLeditor id="CTRLeditorNote" runat="server" 
                                                                ContainerCssClass="containerclass" 
                                                                LoaderCssClass="loadercssclass" EditorHeight="100px" 
                                                                EditorWidth="550px"
                                                                AllAvailableFontnames="true" AutoInitialize="true" 
                                                     MaxHtmlLength="4000"
                                                                ModuleCode="SRVEVENTI">
                                                            </CTRL:CTRLeditor>
											</td>
											<td class=top nowrap="nowrap" >
												<asp:LinkButton ID="LNBmodificaNote" Runat=server CausesValidation=False CssClass="LinksmallNavy11">Modifica</asp:LinkButton>
												<asp:LinkButton ID="LNBsalvaNote" Runat=server CausesValidation=False CssClass="LinksmallNavy11">Salva Note</asp:LinkButton>
											</td>
										</tr>
									</table>		
														
								</asp:TableCell>
							</asp:TableRow>
							<asp:TableRow id="TBRprogramma">
								<asp:TableCell Width=110px Wrap=false CssClass=top>
									<asp:label id="LBprogrammaNormale_t" Runat="server" CssClass="Titolo_campoSmall" text="Programma:"></asp:label>
								</asp:TableCell>
								<asp:TableCell ColumnSpan=3>
									<asp:textbox cssclass="Testo_campoSmall" id="TXBprogramma" TextMode="MultiLine" columns="80" runat="server"
										rows="5"></asp:textbox>
								</asp:TableCell>
							</asp:TableRow>
							<asp:TableRow id="TBRnote">
								<asp:TableCell Width=110px Wrap=false CssClass=top>
									<asp:label id="LBnoteNormale_t" Runat="server" CssClass="Titolo_campoSmall" text="Note:"></asp:label>
								</asp:TableCell>
								<asp:TableCell CssClass=top>
										<asp:textbox cssclass="Testo_campoSmall" id="TXBnote" TextMode="MultiLine" columns="80" runat="server"
										rows="3"></asp:textbox>
								</asp:TableCell>
							</asp:TableRow>
							<asp:TableRow>
								<asp:TableCell Width=110px Wrap=false CssClass=top>
									<asp:label id="LBLink" Runat="server" CssClass="Titolo_campoSmall" text="Link: "></asp:label>
								</asp:TableCell>
								<asp:TableCell CssClass=top>
									<asp:textbox id="TBLink" Runat="server" Width="100%" cssClass="Testo_campoSmall"></asp:textbox>
									<asp:RegularExpressionValidator Runat="server" Display="Dynamic" ControlToValidate="TBLink" ErrorMessage="Link troppo lungo (max 2500 caratteri)" ID="Regularexpressionvalidator5" ValidationExpression=".{0,2500}"></asp:RegularExpressionValidator>
								</asp:TableCell>
							</asp:TableRow>
							<asp:TableRow ID="TBRannoAccademico">
								<asp:TableCell Width=110px Wrap=false CssClass=top>
									<asp:label id="LBAnnoAccademico" Runat="server" CssClass="Titolo_campoSmall" text="Anno Accademico:"></asp:label>
								</asp:TableCell>
								<asp:TableCell CssClass=top>
									<asp:DropDownList id="DDLAnnoAccademico" runat="server" CssClass="Testo_campo_obbligatorioSmall"></asp:DropDownList>
								</asp:TableCell>
							</asp:TableRow>
							<asp:TableRow ID="TBRavvisoPersonale">
								<asp:TableCell Width=110px Wrap=false>
									<asp:label id="LBAvviso" Visible="False" Runat="server" CssClass="Titolo_campoSmall" text="Tipo di Avviso :"></asp:label>
								</asp:TableCell>
								<asp:TableCell>
									<asp:dropdownlist id="DDLTipoAvviso" runat="server" Visible="false" CssClass="Testo_campo_obbligatorioSmall"></asp:dropdownlist>
								</asp:TableCell>
							</asp:TableRow>
							<asp:TableRow ID="TBRmacro">
								<asp:TableCell Width=110px Wrap=false>
									<asp:label id="LBMacro" Runat="server" CssClass="Titolo_campoSmall" text="Macro<br/>(durata sul calendario non visibile):"></asp:label>
								</asp:TableCell>
								<asp:TableCell>
									<asp:checkbox id="CBMacro" runat="server" CssClass="Testo_campoSmall" Text="Macro" Checked=False></asp:checkbox>
								</asp:TableCell>
							</asp:TableRow>
						</asp:Table>
					</asp:Panel>
				</asp:Panel>
				<asp:Panel ID="PNLinfo" Runat="server" HorizontalAlign="Center" Visible="False">
					<table align="center" width="400px">
						<tr>
							<td height="30px" colspan="2">&nbsp;</td>
						</tr>
						<tr>
							<td colspan="2" align="center">
								<br/>
								<asp:Label ID="LBinfo" Width="400px" Height="50px" Runat="server" ForeColor="#ff0000" BorderColor="#ff6347"
									BorderWidth="1" Font-Bold="True" CssClass="avviso_normal" />
								<br/>
								<br/>
							</td>
						</tr>
						<tr>
							<td height="30px" colspan="2">&nbsp;</td>
						</tr>
					</table>
				</asp:Panel>
			</td>
		</tr>
	</table>
</asp:Content>


<%--<!DOCTYPE HTML PUBLIC "-//W3C//Dtd HTML 4.0 transitional//EN">
<HTML>
	<head runat="server">
		<title>Modifica Eventi</title>
		
		<meta content="Microsoft Visual Studio.NET 7.0" name="GENERATOR"/>
		<meta content="Visual Basic 7.0" name="CODE_LANGUAGE"/>
		<meta content="JavaScript" name="vs_defaultClientScript"/>
		

		
		<LINK href="./../Styles.css" type="text/css" rel="stylesheet"/>
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema"/>
		
	</HEAD>
	<%
		try
			Select Case Session("LinguaCode")
                Case "it-IT"
                    response.write("<script type=text/javascript src=" & """" & "./../Jscript/Calendar/lang/calendar-it.js" & """" &"></script>")
                Case "en-US"
                    response.write("<script type=text/javascript src=" & """" &  "./../Jscript/Calendar/lang/calendar-en.js" & """" &"></script>")
                Case "de-DE"
                    response.write("<script type=text/javascript src=" & """" &  "./../Jscript/Calendar/lang/calendar-de.js" & """" &"></script>")
                Case "fr-FR"
                   response.write("<script type=text/javascript src=" & """" &  "./../Jscript/Calendar/lang/calendar-fr.js" & """" &"></script>")
                Case "es-ES"
                    response.write("<script type=text/javascript src=" & """" &  "./../Jscript/Calendar/lang/calendar-es.js" & """" &"></script>")
                Case Else
                  response.write("<script type=text/javascript src=" & """" &  "./../Jscript/Calendar/lang/calendar-en.js" & """" &"></script>")
            End Select
		catch ex as exception
			response.write("<script type=text/javascript src=" & """" & "./../Jscript/Calendar/lang/calendar-en.js" & """" &"></script>")
		end try%>
		
	<body >
		<form id="aspnetForm " method="post" runat="server">
		<asp:ScriptManager ID="SCMmanager" runat="server"></asp:ScriptManager>
			LABEL DATA CREAZIONE - Visible=false
			<table id="table1" cellSpacing="1" cellPadding="1" width="780" align="center" border="0">
				<tr>
					<td>
						<HEADER:CtrLHEADER id="Intestazione" runat="server"></HEADER:CtrLHEADER>
					</td>
				</tr>
				<tr>
					<td>
						
					</td>
				</tr>
			</table>
			<FOOTER:CTRLFOOTER id="Piede" runat="server"></FOOTER:CTRLFOOTER>
		</form>
	</body>
</HTML>--%>
