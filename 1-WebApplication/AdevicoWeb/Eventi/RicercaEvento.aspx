<%@ Page Language="vb" AutoEventWireup="false" MasterPageFile="~/AjaxPortal.Master" Codebehind="RicercaEvento.aspx.vb" Inherits="Comunita_OnLine.RicercaEvento"%>
<%@ MasterType VirtualPath="~/AjaxPortal.Master" %>

<%--
--%>


<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
	<script language="Javascript" src="./../jscript/generali.js"></script>
	<style type=text/css>@import url(./../Jscript/Calendar/calendar-blue.css);</style>
	<script type=text/javascript src="./../Jscript/Calendar/calendar.js"></script>
		
	<script type=text/javascript src="./../Jscript/Calendar/calendar-setup.js"></script>
    <%=CalendarScript() %>

    <script language="javascript" type="text/javascript">

        function Conferma(ConfermaAzione, ConfermaSelezione) {
            var HIDcheckbox, selezionati, LNBelimina;
            //eval('HIDcheckbox= this.document.forms[0].HDabilitato')
            HIDcheckbox = this.document.getElementById('<%=Me.HDabilitato.ClientID%>');
            if (HIDcheckbox.value == ",")
                HIDcheckbox.value = ""
            if (HIDcheckbox.value == ",,")
                HIDcheckbox.value = ""
            if (HIDcheckbox.value == "") {
                alert(ConfermaSelezione)
                return false;
            }
            else
                return confirm(ConfermaAzione);
        }

        function SelectMe(Me) {
            var HIDcheckbox, selezionati, LNBelimina;
            //eval('HIDcheckbox=this.document.forms[0].HDabilitato')
            //eval('HIDcheckbox=<%=Me.HDabilitato.ClientID%>')
            HIDcheckbox = this.document.getElementById('<%=Me.HDabilitato.ClientID%>');
            selezionati = 0
            for (i = 0; i < document.forms[0].length; i++) {
                e = document.forms[0].elements[i];
                if (e.type == 'checkbox' && e.name.indexOf("CBabilitato") != -1) {
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


        <%-- Probabilmente QUESTA NON FUNZIONA, ma non ho modo di testare... --%>
        function SelectAll(SelectAllBox) {
            var actVar = SelectAllBox.checked;
            var TBcheckbox;
            //eval('HDabilitato= this.document.forms[0].HDabilitato')
            //eval('HDabilitato=<%=Me.HDabilitato.ClientID%>')
            HDabilitato = this.document.getElementById('<%=Me.HDabilitato.ClientID%>');
            HDabilitato.value = ""
            for (i = 0; i < document.forms[0].length; i++) {
                e = document.forms[0].elements[i];
                if (e.type == 'checkbox' && e.name.indexOf("CBabilitato") != -1) {
                    e.checked = actVar;
                    if (e.checked == true)
                        if (HDabilitato.value == "")
                            HDabilitato.value = ',' + e.value + ','
                        else
                            HDabilitato.value = HDabilitato.value + e.value + ','
                    }
                }
            }

            function SubmitRicerca(event) {
                if (document.all) {
                    if (event.keyCode == 13) {
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
	</script>

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="CPHservice" runat="server">
    <asp:label ID="LBDataCreazione" Runat="server" Visible="False" />

    <table cellspacing="0" cellpadding="0" width="900px" border="0">
							<tr>
								<td Class="RigaTitolo" align=left>
									<asp:label id="LBtitolo" Runat="server">Trova Evento</asp:label>
								</td>
							</tr>
							<tr>
								<td>
									<asp:Panel ID="PNLmenu" Runat=server HorizontalAlign=Right>
										<asp:Label Runat="server" CssClass="Eventi_Label" ID="LBvisualizzazione">Vai alla visualizzazione:</asp:Label>
										&nbsp;<asp:linkbutton ID="LKBgoTOsettimanale" Runat="server" CssClass="LINK_MENU">settimanale</asp:linkbutton>
										<asp:linkbutton ID="LKBgoTOmensile" Runat="server" CssClass="LINK_MENU">mensile</asp:linkbutton>
										<asp:linkbutton ID="LKBgoTOannuale" Runat="server" CssClass="LINK_MENU">annuale</asp:linkbutton>
										<asp:linkbutton ID="LNBelimina" Runat="server" CssClass="LINK_MENU" Enabled=False >&nbsp;Cancella selezionati</asp:linkbutton>
									</asp:Panel>
								</td>
							</tr>
							<tr>
								<td>
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
												<td height="50">&nbsp;</td>
											</tr>
										</table>
									</asp:panel>
									<asp:Panel ID="PNLcontenuto" Runat=server HorizontalAlign=Center>
										<asp:Table id="TBLfiltroNew" Runat=server  Width="900px" CellPadding=0 CellSpacing=0>
											<asp:TableRow id="TBRchiudiFiltro" Height=22px>
												<asp:TableCell CssClass="Filtro_CellSelezionato" HorizontalAlign=Center Width=150px Height=22px VerticalAlign=Middle >
													<asp:Label ID="LBAzione_t" Runat=server CssClass="Filtro_LabelLink">Filtri</asp:Label>
												</asp:TableCell>
												<asp:TableCell CssClass="Filtro_CellDeSelezionato" Width=750px Height=22px>&nbsp;
												</asp:TableCell>
											</asp:TableRow>
											<asp:TableRow ID="TBRfiltri">
												<asp:TableCell CssClass="Filtro_CellFiltri" ColumnSpan=2 Width=900px HorizontalAlign=center>
													<asp:Table Runat=server ID="TBLfiltro" CellPadding=1 CellSpacing=0 Width="900px" HorizontalAlign=center >
														<asp:TableRow>
															<asp:TableCell CssClass="FiltroVoceSmall">
																<table cellspacing=0 border=0 align=left >
																	<tr>
																		<td height=25px>&nbsp;</td>
																		<td height=25px>
																			<asp:Label Runat="server" CssClass="FiltroVoceSmall" ID="LBtipoRicerca_t">Tipo ricerca:</asp:Label>&nbsp;&nbsp;
																		</td>
																		<td height=25px>
																			<table cellpadding=0 cellspacing=0 border=0>
																				<tr>
																					<td>
																						<asp:dropdownlist id="DDLricerca" Runat="server" Autopostback="true" CssClass=FiltroCampoSmall>
																							<asp:ListItem Value="4" Selected="true">Elenca tutti gli eventi memorizzati</asp:ListItem>
																							<asp:ListItem Value="3">Cerca gli eventi che cadono in una data</asp:ListItem>
																							<asp:ListItem Value="0">Cerca gli eventi compresi tra due date</asp:ListItem>
																							<asp:ListItem Value="5">Cerca tutti gli eventi, esclusi quelli compresi tra due date</asp:ListItem>
																							<asp:ListItem Value="1">Cerca gli eventi che cadono prima di una data</asp:ListItem>
																							<asp:ListItem Value="2">Cerca gli eventi che cadono dopo una data</asp:ListItem>
																							<asp:ListItem Value="6">Cerca gli eventi modificati dopo una data</asp:ListItem>
																						</asp:dropdownlist>
																					</td>
																					<td height=25px>&nbsp;</td>
																					<td height=25px>
																						<asp:label id="LBcategoria" Runat="server" text="Scegli la categoria:" CssClass="FiltroVoceSmall"></asp:label>&nbsp;
																						<asp:dropdownlist id="DDLCategoria" runat="server" CssClass="FiltroCampoSmall"></asp:dropdownlist>
																					</td>
																				</tr>
																			</table>
																		</td>
																	</tr>
																	<tr>
																		<td height=25px>&nbsp;</td>
																		<td>
																			<asp:Label Runat="server" CssClass="FiltroVoceSmall" ID="LBFiltroComunita" Visible="true">Filtro comunità:</asp:Label>
																		</td>
																		<td>
																			<table cellpadding=0 cellspacing=0 border=0>
																				<tr>
																					<td>
																						<asp:radiobuttonlist id="RBLFiltroComunita" Runat="server" CssClass="FiltroCampoSmall" Visible="true" RepeatLayout="Flow" Repeatdirection="Horizontal">
																							<asp:ListItem Value="-1">tutte</asp:ListItem>
																							<asp:ListItem Value="0" Selected="True">corrente</asp:ListItem>
																						</asp:radiobuttonlist>
																					</td>
																					<td height=25px width=30px>&nbsp;</td>
																					<td>
																						<asp:Table ID="TBLcalendari" Runat=server CellPadding=0 CellSpacing=0>
																							<asp:TableRow>
																								<asp:TableCell ID="TBCinizio">
																									<asp:label id="LBdataInizio" Runat="server" CssClass="FiltroVoceSmall" text="Data Inizio:"></asp:label>&nbsp;
																									<asp:TextBox ID="TXBdataI" Runat="server" ReadOnly="True" CssClass="FiltroCampoSmall" Columns=10></asp:TextBox>&nbsp;
																									<asp:ImageButton ID="IMBapriInizio" ImageUrl="../images/cal.gif" Runat="server" CausesValidation="False"></asp:ImageButton>
																								</asp:TableCell>
																								<asp:TableCell ID="TBCfine">
																									&nbsp;&nbsp;&nbsp;&nbsp;
																									<asp:label id="LBDataFine" Runat="server" CssClass="FiltroVoceSmall" text="Data Fine:"></asp:label>&nbsp;
																									<asp:TextBox ID="TXBdataF" Runat="server" ReadOnly="True" CssClass="FiltroCampoSmall" Columns=10></asp:TextBox>&nbsp;
																									<asp:ImageButton ID="IMBapriFine" ImageUrl="../images/cal.gif" Runat="server" CausesValidation="False"></asp:ImageButton>
																								</asp:TableCell>
																								<asp:TableCell ID="TBCscriptSingolo" Visible=False>
																								<script type=text/javascript>

																								    Calendar.setup({
																								        ifFormat: "%d/%m/%Y",
																								        inputField: "<%=me.TXBdataI.clientID%>",
																								        displayArea: "<%=me.TXBdataI.clientID%>",
																								        button: "<%=me.IMBapriInizio.clientID%>",
																								        firstDay: 1
																								    }
																											);
																									</script>
																								</asp:TableCell>
																								<asp:TableCell ID="TBCscript" Visible=true>&nbsp;
																									<script type=text/javascript>
																									    function selectInit(calendar, date) {
																									        if (calendar.dateClicked) {
																									            dataI = date
																									            dataF = document.getElementById("<%=me.TXBdataF.clientID%>").value
																									            dataIniziale = dataI.split("/")
																									            dataFinale = dataF.split("/")
																									            var dataInizio = new Date()
																									            var dataFine = new Date()
																									            dataInizio = Date.parse(dataIniziale[1] + '/' + dataIniziale[0] + '/' + dataIniziale[2])
																									            dataFine = Date.parse(dataFinale[1] + '/' + dataFinale[0] + '/' + dataFinale[2])

																									            if (dataInizio > dataFine) {
																									                document.getElementById("<%=me.TXBdataF.clientID%>").value = date
																									            }
																									            document.getElementById("<%=me.TXBdataI.clientID%>").value = date
																									            calendar.callCloseHandler()
																									        }

																									    }
																									    function selectEnd(calendar, date) {
																									        if (calendar.dateClicked) {
																									            dataI = document.getElementById("<%=me.TXBdataI.clientID%>").value
																									            dataF = date
																									            dataIniziale = dataI.split("/")
																									            dataFinale = dataF.split("/")
																									            var dataInizio = new Date()
																									            var dataFine = new Date()
																									            dataInizio = Date.parse(dataIniziale[1] + '/' + dataIniziale[0] + '/' + dataIniziale[2])
																									            dataFine = Date.parse(dataFinale[1] + '/' + dataFinale[0] + '/' + dataFinale[2])

																									            if (dataInizio > dataFine) {
																									                document.getElementById("<%=me.TXBdataI.clientID%>").value = date
																									            }
																									            document.getElementById("<%=me.TXBdataF.clientID%>").value = date
																									            calendar.callCloseHandler()
																									        }

																									    }
																									    Calendar.setup({
																									        ifFormat: "%d/%m/%Y",
																									        inputField: "<%=me.TXBdataI.clientID%>",
																									        displayArea: "<%=me.TXBdataI.clientID%>",
																									        button: "<%=me.IMBapriInizio.clientID%>",
																									        firstDay: 1,
																									        onSelect: selectInit
																									    }
																											);

																									    Calendar.setup({
																									        ifFormat: "%d/%m/%Y",
																									        inputField: "<%=me.TXBdataF.clientID%>",
																									        displayArea: "<%=me.TXBdataF.clientID%>",
																									        button: "<%=me.IMBapriFine.clientID%>",
																									        firstDay: 1,
																									        onSelect: selectEnd


																									    }
																											);
																									</script>
																								</asp:TableCell>
																							</asp:TableRow>
																						</asp:Table>
																					</td>
																				</tr>
																			</table>
																		</td>
																	</tr>
																</table>
															</asp:TableCell>
															<asp:TableCell HorizontalAlign=Left VerticalAlign=Bottom CssClass="bottom">
																<asp:button id=BTNCerca Runat="server" CssClass="PulsanteFiltro" Text="Cerca"></asp:button>&nbsp;
															</asp:TableCell>
														</asp:TableRow>
														<asp:TableRow>
															<asp:TableCell Height=10px ColumnSpan=2 CssClass="nosize0">&nbsp;</asp:TableCell>
														</asp:TableRow>
													</asp:Table>
												</asp:TableCell>			
											</asp:TableRow>
											<asp:TableRow ID="TBRdati">
												<asp:TableCell ColumnSpan=2 HorizontalAlign=Center >
													<br/>
													<INPUT id="HDabilitato" type="hidden" name="HDabilitato" runat="server"/>
													<INPUT id="HDattivato" type="hidden" name="HDattivato" runat="server"/>
													<INPUT id="HDtutti" type="hidden" name="HDtutti" runat="server"/>
													<asp:datagrid 
													    id="DGEventi" 
													    Visible="False" 
													    Runat="server" 
													    PagerStyle-Mode="NumericPages" 
													    PageSize="25" 
													    AllowSorting="true" 
													    AutoGenerateColumns="False" 
													    AllowPaging="true" 
													    DataKeyField="EVNT_id"
													    CssClass="DataGrid_Generica">
														<PagerStyle Position=Bottom HorizontalAlign=Right> </PagerStyle>
														<HeaderStyle Font-Size="11px"  BackColor="#c7d9ca" CssClass="ROW_headerEventi_SmallCenter"></HeaderStyle>
														<columns>
															<asp:TemplateColumn runat="server" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="20">
																<ItemTemplate>
																	<input runat=server  type="checkbox" id="CBabilitato" name="CBabilitato"  onclick="SelectMe(this);">
																</ItemTemplate>
															</asp:TemplateColumn>
															<asp:TemplateColumn runat="server" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="50px">
																<ItemTemplate>
																	<asp:ImageButton id="IMBmodifica" Runat="server" CausesValidation="False" CommandName="modifica" ImageUrl="./../images/m.gif"></asp:ImageButton>
																	<asp:ImageButton id="IMBCancella" Runat="server" CausesValidation="False" CommandName="elimina" ImageUrl="./../images/x.gif"></asp:ImageButton>
																</ItemTemplate>
															</asp:TemplateColumn>
															<asp:boundcolumn DataField="TPEV_id" Visible="False"></asp:boundcolumn>
															<asp:boundcolumn DataField="ORRI_id" Visible="False"></asp:boundcolumn>
															<asp:boundcolumn HeaderText="Categoria" ItemStyle-Width="160px" HeaderStyle-CssClass="ROW_headerEventi_SmallCenter" DataField="TPEV_nome" SortExpression="TPEV_nome"></asp:boundcolumn>
															<asp:buttoncolumn headertext="Nome Evento" ItemStyle-Width="220px" HeaderStyle-CssClass="ROW_headerEventi_SmallCenter" DatatextField="EVNT_nome" SortExpression="EVNT_nome" CommandName="vaiCalendario"></asp:buttoncolumn>
															<asp:boundcolumn headertext="Inizio" ItemStyle-Width="160px" HeaderStyle-CssClass="ROW_headerEventi_SmallCenter" DataField="ora_inizio" SortExpression="ORRI_inizio"></asp:boundcolumn>
															<asp:boundcolumn headertext="Fine" ItemStyle-Width="160px" HeaderStyle-CssClass="ROW_headerEventi_SmallCenter" DataField="ora_fine" SortExpression="ORRI_fine"></asp:boundcolumn>
															<asp:boundcolumn headertext="Anno Accademico" HeaderStyle-CssClass="ROW_headerEventi_SmallCenter" DataField="annoAccademico" SortExpression="annoAccademico" ItemStyle-Width="60px"></asp:boundcolumn>
															<asp:boundcolumn headertext="Comunità" ItemStyle-Width="220px" HeaderStyle-CssClass="ROW_headerEventi_SmallCenter" DataField="CMNT_nome" SortExpression="CMNT_nome"></asp:boundcolumn>
															<asp:boundcolumn headertext="Modificato il" HeaderStyle-CssClass="ROW_headerEventi_SmallCenter" DataField="ORRI_dataModifica" SortExpression="ORRI_dataModifica" Visible="False"></asp:boundcolumn>
															<asp:boundcolumn DataField="CMNT_id" Visible="False"></asp:boundcolumn>
														</columns>
													</asp:datagrid>
													<asp:Label id="LBnoRecord" CssClass="avviso" Runat="server" Visible="False"></asp:Label>
												</asp:TableCell>
											</asp:TableRow>
										</asp:Table>
									</asp:Panel>
								</td>
							</tr>
						</table>
</asp:Content>

<%--
<!DOCTYPE HTML PUBLIC "-//W3C//Dtd HTML 4.0 transitional//EN">
<HTML>
	<head runat="server">
		<title>Ricerca Eventi</title>

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
				
	<body  onkeydown="return SubmitRicerca(event);">
		<form id="aspnetForm" method="post" runat="server">
		<asp:ScriptManager ID="SCMmanager" runat="server"></asp:ScriptManager>
			LABEL DATA CREAZIONE
			<table id="table1" cellspacing="1" cellpadding="1" width="900px" align="center" border="0">
				<tr>
					<td>
						<div>
				            <HEADER:CtrLHEADER id="Intestazione" runat="server" HeaderNewsMemoHeight="85px"></HEADER:CtrLHEADER>
				        </div>
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