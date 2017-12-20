<%@ Page Language="vb" AutoEventWireup="false" MasterPageFile="~/AjaxPortal.Master" Codebehind="InserimentoEvento.aspx.vb" Inherits="Comunita_OnLine.Inserimento_Evento"%>
<%@ MasterType VirtualPath="~/AjaxPortal.Master" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register Src="~/Modules/Common/Editor/UC_Editor.ascx" TagName="CTRLeditor" TagPrefix="CTRL" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <style type=text/css>@import url(./../Jscript/Calendar/calendar-blue.css);</style>
	<script type=text/javascript src="./../Jscript/Calendar/calendar.js"></script>
	<script type=text/javascript src="./../Jscript/Calendar/calendar-setup.js"></script>
    
    <script language="Javascript" src="./../jscript/generali.js" type="text/javascript"></script>
    <%=CalendarScript() %>

	<script language="javascript" type="text/javascript">
		function AggiornaForm()
		{
		    valore = document.forms[0].<%=me.DDLCategoria.ClientID%>.options[document.forms[0].<%=me.DDLCategoria.ClientID%>.selectedIndex].value;
		    if (valore==1)
		    {
		        document.forms[0].<%=me.HDNselezionato.ClientID%>.value = valore;
		        __doPostBack('DDLCategoria','');
		        return true;
		    }
		    else if (valore==0)
		    {
		        document.forms[0].<%=me.HDNselezionato.ClientID%>.value = valore;
		        __doPostBack('DDLCategoria','');
		        return true;
		    }
		    else if (document.forms[0].<%=me.HDNselezionato.ClientID%>.value == 0)
		    {
		        document.forms[0].<%=me.HDNselezionato.ClientID%>.value = valore;
		        __doPostBack('DDLCategoria','');
		        return true;
		    }
		    else if (document.forms[0].<%=me.HDNselezionato.ClientID%>.value == 1)
		    {
		        document.forms[0].<%=me.HDNselezionato.ClientID%>.value = valore;
		        __doPostBack('DDLCategoria','');
		        return true;
		    }	
			document.forms[0].<%=me.HDNselezionato.ClientID%>.value = valore;
    		return false;
		}
	</script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="CPHservice" runat="server">
	<table cellSpacing="0" cellPadding="0"  width="900px" border="0">
		<tr>
			<td Class="RigaTitolo" align=left>
				<asp:label id="LBtitolo" Runat="server">Inserimento Evento</asp:label>
			</td>
		</tr>
		<tr>
			<td align=right>
				<asp:linkbutton ID="LNBcalendario" Runat="server" Text="Al calendario" CausesValidation="false" CssClass="LINK_MENU"></asp:linkbutton>
				&nbsp;
				<asp:linkbutton ID="LNBcrea" Runat="server" Text="Crea" CssClass="LINK_MENU"></asp:linkbutton>
				<asp:linkbutton ID="LNBcreaNuovo" Runat="server" Text="Crea nuovo" CausesValidation="false" CssClass="LINK_MENU" Visible=False></asp:linkbutton>
			</td>
		</tr>
		<tr>
			<td vAlign="top" align="center">
									
				<asp:panel id="PNLpermessi" Runat="server" Visible="False">
					<table align="center">
						<tr>
							<td height="50">&nbsp;</td>
						</tr>
						<tr>
							<td align="center"><asp:Label id="LBNopermessi" CssClass="messaggio" ForeColor="Blue" Runat="server"></asp:Label></td>
						</tr>
						<tr>
							<td vAlign="top" align="center" height="50">&nbsp;</td>
						</tr>
					</table>
				</asp:panel>
				<asp:panel id="PNLcontenuto" Runat="server" HorizontalAlign="Center">
					<br/>
					<table align=center>
						<tr>
							<td>
								<asp:Panel ID="PNLMain" Runat="server">
									<asp:Table ID="TBLevento" Runat=server GridLines=none HorizontalAlign=Center Width=700px>
										<asp:TableRow>
											<asp:TableCell Width=110px Wrap=false>
												<asp:label id="LBNomeEvento" Runat="server" CssClass="Titolo_campoSmall" text="Nome dell'evento:"></asp:label>
											</asp:TableCell>
											<asp:TableCell>
												<asp:textbox id="TXBNomeEvento" Runat="server" CssClass="Testo_campo_obbligatorioSmall"  Columns=100 MaxLength=100></asp:textbox>
												<asp:RequiredFieldValidator runat="server" Display="Dynamic" controltovalidate="TXBNomeEvento" errormessage="INSERIRE IL NOME DELL'EVENTO" ID="RFVNomeEvento"></asp:RequiredFieldValidator>
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
										<asp:TableRow>
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
																});

																Calendar.setup({
																	ifFormat: "%d/%m/%Y",
																	inputField: "<%=me.HDNdataF.clientID%>",
																	displayArea: "<%=me.LBdataF.clientID%>",
																	button: "<%=me.IMBapriFine.clientID%>",
																	firstDay: 1,
																	onSelect: selectEnd


																});
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
										<asp:TableRow ID="TBRripetizione">
											<asp:TableCell Width=110px Wrap=false>
												<asp:label id="LBRipetizione" Runat="server" CssClass="Titolo_campoSmall" text="Ripetizione:"></asp:label>
											</asp:TableCell>
											<asp:TableCell BackColor="#CEFFCE">
												<asp:radiobuttonlist id="RBLRipetizione" Runat="server" CssClass="Testo_campoSmall" Repeatdirection="Horizontal" Autopostback="true" RepeatLayout=Flow>
													<asp:ListItem Value="0">nessuna</asp:ListItem>
													<asp:ListItem Value="1">giornaliera</asp:ListItem>
													<asp:ListItem Value="2">settimanale</asp:ListItem>
													<asp:ListItem Value="3">mensile</asp:ListItem>
													<asp:ListItem Value="4">annuale</asp:ListItem>
													<asp:ListItem Value="5">perpetua</asp:ListItem>
												</asp:radiobuttonlist>
											</asp:TableCell>
										</asp:TableRow>
										<asp:TableRow ID="TBRripetizioni" Runat=server >
											<asp:TableCell Width=110px Wrap=false>&nbsp;</asp:TableCell>
											<asp:TableCell >
												<asp:panel id="PNLCriteriGiornalieri" Visible="False" Runat="server" BorderWidth="2" BorderStyle="Inset" BackColor="white">
													<table border=0 cellspacing=0 cellpadding=0 >
														<tr>
															<td height=25px nowrap="nowrap" >
																<asp:radiobutton id="RDBferiali" Runat="server" CssClass="Titolo_campoSmall" text="Ogni giorno feriale" GroupName="CriterioGiornaliero"></asp:radiobutton>&nbsp;
															</td>
															<td height=25px nowrap="nowrap"> 
																<asp:radiobutton id="RDBfestivi" Runat="server" CssClass="Titolo_campoSmall" text="Ogni giorno festivo" GroupName="CriterioGiornaliero"></asp:radiobutton>		
															</td>
															<td height=25px colspan=2 nowrap="nowrap">
																<asp:radiobutton id="RBCriteriGiornalieri1" Runat="server" CssClass="Titolo_campoSmall" text="Ogni" GroupName="CriterioGiornaliero"></asp:radiobutton>&nbsp;
																<asp:textbox id="TXBnumeroGiorni" Runat="server" CssClass="Testo_campo_obbligatorioSmall" Width="25">1</asp:textbox>
																<asp:Label ID="LBCGgiorno" Runat="server" CssClass="Testo_campoSmall">giorno/i</asp:Label>
																<asp:RequiredFieldValidator ControlToValidate="TXBnumeroGiorni" text="inserire numero giorni" Runat="server" ID="RFVnumeroGiorni" Display=Dynamic></asp:RequiredFieldValidator>
																<asp:RangeValidator ControlToValidate="TXBnumeroGiorni" MinimumValue="1" MaximumValue="365" Runat="server" Type="Integer" text="*  (1- 365)" ID="RVnumeroGiorni" Display=Dynamic></asp:RangeValidator>
															</td>
														</tr>
													</table>																		
												</asp:panel>
												<asp:panel id="PNLCriteriSettimanali" Visible="False" Runat="server" BorderWidth="2" BorderStyle="Inset" BackColor="white">
													<table border=0 cellspacing=0 cellpadding=0 width=100%>
														<tr>
															<td height=25px>
																<asp:Label ID="LBCSOgni" Runat="server" CssClass="Titolo_campoSmall">Ogni</asp:Label>
																<asp:textbox id="TXBnumeroSettimane" Runat="server" CssClass="Testo_campo_obbligatorioSmall" Width="25">1</asp:textbox>
																<asp:Label ID="LBCSSettimane" Runat="server" CssClass="Titolo_campoSmall">settimana/e di:</asp:Label>
																<asp:RequiredFieldValidator ControlToValidate="TXBnumeroSettimane" text="inserire numero settimane" Runat="server" ID="RFVnumeroSettimane"></asp:RequiredFieldValidator>
																<asp:RangeValidator ControlToValidate="TXBnumeroSettimane" MinimumValue="1" MaximumValue="55" Runat="server" Type="Integer" text="*  (1- 54)" ID="RVnumeroSettimane"></asp:RangeValidator>
															</td>
														</tr>
														<tr>
															<td>
																<asp:Table ID="TBLripetizioni" Runat=server GridLines=Vertical CellSpacing=0 HorizontalAlign=Center>
																	<asp:TableRow>
																		<asp:TableCell width="45px" height="18px" cssclass="nosize0" BackColor="#000084">&nbsp;</asp:TableCell>
																		<asp:TableCell width="140px" height="18px" cssclass="nosize0" BackColor="#000084">&nbsp;</asp:TableCell>
																		<asp:TableCell width="140px" height="18px" cssclass="nosize0" BackColor="#000084" ColumnSpan=2 HorizontalAlign=Center>
																			<asp:Label ID="LBdal" Runat="server" CssClass="ROW_header_Small_Center">Dalle</asp:Label>
																		</asp:TableCell>
																		<asp:TableCell width="140px" height="18px" cssclass="nosize0" BackColor="#000084" ColumnSpan=2 HorizontalAlign=Center>
																			<asp:Label ID="LBal" Runat="server" CssClass="ROW_header_Small_Center">Alle</asp:Label>
																		</asp:TableCell>
																		<asp:TableCell height="18px" cssclass="nosize0" BackColor="#000084">&nbsp;</asp:TableCell>
																	</asp:TableRow>
																	<asp:TableRow BackColor="#CEFFCE">
																		<asp:TableCell BackColor=#CEFFCE width="45px" height="18px" HorizontalAlign="center"><asp:CheckBox id="CBGiorno1" Runat="server" CssClass="Testo_campoSmall"></asp:CheckBox></asp:TableCell>
																		<asp:TableCell BackColor=#CEFFCE width="140px" height="18px"><asp:Label ID="LBLCSLunedi" Runat="server">Lunedì</asp:Label></asp:TableCell>
																		<asp:TableCell BackColor=#CEFFCE width="70px" height="18px" HorizontalAlign="center"><asp:dropdownlist id="DDLOraInizio1" runat="server" CssClass="Testo_campoSmall"></asp:dropdownlist></asp:TableCell>
																		<asp:TableCell BackColor=#CEFFCE width="70px" height="18px" HorizontalAlign="center"><asp:dropdownlist id="DDLMinutiInizio1" runat="server" CssClass="Testo_campoSmall"></asp:dropdownlist></asp:TableCell>
																		<asp:TableCell BackColor=#CEFFCE width="70px" height="18px" HorizontalAlign="center"><asp:dropdownlist id="DDLOraFine1" runat="server" CssClass="Testo_campoSmall"></asp:dropdownlist></asp:TableCell>
																		<asp:TableCell BackColor=#CEFFCE width="70px" height="18px" HorizontalAlign="center"><asp:dropdownlist id="DDLMinutiFine1" runat="server" CssClass="Testo_campoSmall"></asp:dropdownlist></asp:TableCell>
																		<asp:TableCell BackColor=#CEFFCE width="70px" height="18px" HorizontalAlign="left"><asp:Label ID="LBLerrDLL1" Runat="server" CssClass="Titolo_campoSmall" Visible="False" ForeColor="Red" Font-Bold="True">errore</asp:Label></asp:TableCell>
																	</asp:TableRow>
																	<asp:TableRow BackColor="white">
																		<asp:TableCell width="45px" height="18px" HorizontalAlign="center"><asp:CheckBox id="CBGiorno2" Runat="server" CssClass="Testo_campoSmall"></asp:CheckBox></asp:TableCell>
																		<asp:TableCell width="140px" height="18px"><asp:Label ID="LBLCSMartedi" Runat="server">Martedì</asp:Label></asp:TableCell>
																		<asp:TableCell width="70px" height="18px" HorizontalAlign=center><asp:dropdownlist id="DDLOraInizio2" runat="server" CssClass="Testo_campoSmall"></asp:dropdownlist></asp:TableCell>
																		<asp:TableCell width="70px" height="18px" HorizontalAlign=center><asp:dropdownlist id="DDLMinutiInizio2" runat="server" CssClass="Testo_campoSmall"></asp:dropdownlist></asp:TableCell>
																		<asp:TableCell width="70px" height="18px" HorizontalAlign=center><asp:dropdownlist id="DDLOraFine2" runat="server" CssClass="Testo_campoSmall"></asp:dropdownlist></asp:TableCell>
																		<asp:TableCell width="70px" height="18px" HorizontalAlign=center><asp:dropdownlist id="DDLMinutiFine2" runat="server" CssClass="Testo_campoSmall"></asp:dropdownlist></asp:TableCell>
																		<asp:TableCell width="70px" height="18px" HorizontalAlign=Left><asp:Label ID="LBLerrDLL2" Runat="server" CssClass="Titolo_campoSmall" Visible="False" ForeColor="Red" Font-Bold="True">errore</asp:Label> </asp:TableCell> 
																	</asp:TableRow>
																	<asp:TableRow BackColor="#CEFFCE">
																		<asp:TableCell width="45px" height="18px" HorizontalAlign="center"><asp:CheckBox id="CBGiorno3" Runat="server" CssClass="Testo_campoSmall"></asp:CheckBox></asp:TableCell>
																		<asp:TableCell width="140px" height="18px"><asp:Label ID="LBLCSMercoledi" Runat="server">Mercoledì</asp:Label></asp:TableCell>
																		<asp:TableCell width="70px" height="18px" horizontalalign=center><asp:dropdownlist id="DDLOraInizio3" runat="server" CssClass="Testo_campoSmall"></asp:dropdownlist></asp:TableCell>
																		<asp:TableCell width="70px" height="18px" horizontalalign=center><asp:dropdownlist id="DDLMinutiInizio3" runat="server" CssClass="Testo_campoSmall"></asp:dropdownlist></asp:TableCell>
																		<asp:TableCell width="70px" height="18px" horizontalalign=center><asp:dropdownlist id="DDLOraFine3" runat="server" CssClass="Testo_campoSmall"></asp:dropdownlist></asp:TableCell>
																		<asp:TableCell width="70px" height="18px" horizontalalign=center><asp:dropdownlist id="DDLMinutiFine3" runat="server" CssClass="Testo_campoSmall"></asp:dropdownlist></asp:TableCell>
																		<asp:TableCell width="70px" height="18px" horizontalalign="left"><asp:Label ID="LBLerrDLL3" Runat="server" CssClass="Titolo_campoSmall" Visible="False" ForeColor="Red" Font-Bold="True">errore</asp:Label></asp:TableCell>
																	</asp:TableRow>
																	<asp:TableRow BackColor="white">
																		<asp:TableCell width="45px" height="18px" HorizontalAlign="center"><asp:CheckBox id="CBGiorno4" Runat="server" CssClass="Testo_campoSmall"></asp:CheckBox></asp:TableCell>
																		<asp:TableCell width="140px" height="18px"><asp:Label ID="LBLCSGiovedi" Runat="server">Giovedì</asp:Label></asp:TableCell>
																		<asp:TableCell width="70px" height="18px" horizontalalign=center><asp:dropdownlist id="DDLOraInizio4" runat="server" CssClass="Testo_campoSmall"></asp:dropdownlist></asp:TableCell>
																		<asp:TableCell width="70px" height="18px" horizontalalign=center><asp:dropdownlist id="DDLMinutiInizio4" runat="server" CssClass="Testo_campoSmall"></asp:dropdownlist></asp:TableCell>
																		<asp:TableCell width="70px" height="18px" horizontalalign=center><asp:dropdownlist id="DDLOraFine4" runat="server" CssClass="Testo_campoSmall"></asp:dropdownlist></asp:TableCell>
																		<asp:TableCell width="70px" height="18px" horizontalalign=center><asp:dropdownlist id="DDLMinutiFine4" runat="server" CssClass="Testo_campoSmall"></asp:dropdownlist></asp:TableCell>
																		<asp:TableCell width="70px" height="18px" horizontalalign="left"><asp:Label ID="LBLerrDLL4" Runat="server" CssClass="Titolo_campoSmall" Visible="False" ForeColor="Red" Font-Bold="True">errore</asp:Label></asp:TableCell>
																	</asp:TableRow>
																	<asp:TableRow BackColor="#CEFFCE">
																		<asp:TableCell width="45px" height="18px" HorizontalAlign="center"><asp:CheckBox id="CBGiorno5" Runat="server" CssClass="Testo_campoSmall"></asp:CheckBox></asp:TableCell>
																		<asp:TableCell width="140px" height="18px"><asp:Label ID="LBLCSVenerdi" Runat="server">Venerdì</asp:Label></asp:TableCell>
																		<asp:TableCell width="70px" height="18px" horizontalalign=center><asp:dropdownlist id="DDLOraInizio5" runat="server" CssClass="Testo_campoSmall"></asp:dropdownlist></asp:TableCell>
																		<asp:TableCell width="70px" height="18px" horizontalalign=center><asp:dropdownlist id="DDLMinutiInizio5" runat="server" CssClass="Testo_campoSmall"></asp:dropdownlist></asp:TableCell>
																		<asp:TableCell width="70px" height="18px" horizontalalign=center><asp:dropdownlist id="DDLOraFine5" runat="server" CssClass="Testo_campoSmall"></asp:dropdownlist></asp:TableCell>
																		<asp:TableCell width="70px" height="18px" horizontalalign=center><asp:dropdownlist id="DDLMinutiFine5" runat="server" CssClass="Testo_campoSmall"></asp:dropdownlist></asp:TableCell>
																		<asp:TableCell width="70px" height="18px" horizontalalign="left"><asp:Label ID="LBLerrDLL5" Runat="server" CssClass="Titolo_campoSmall" Visible="False" ForeColor="Red" Font-Bold="True">errore</asp:Label></asp:TableCell>
																	</asp:TableRow>
																	<asp:TableRow BackColor="white">
																		<asp:TableCell BackColor="white" width="45px" height="18px" HorizontalAlign="center"><asp:CheckBox id="CBGiorno6" Runat="server" CssClass="Testo_campoSmall"></asp:CheckBox></asp:TableCell>
																		<asp:TableCell BackColor="white" width="140px" height="18px"><asp:Label ID="LBLCSSabato" Runat="server">Sabato</asp:Label></asp:TableCell>
																		<asp:TableCell BackColor="white" width="70px" height="18px" horizontalalign=center><asp:dropdownlist id="DDLOraInizio6" runat="server" CssClass="Testo_campoSmall"></asp:dropdownlist></asp:TableCell>
																		<asp:TableCell BackColor="white" width="70px" height="18px" horizontalalign=center><asp:dropdownlist id="DDLMinutiInizio6" runat="server" CssClass="Testo_campoSmall"></asp:dropdownlist></asp:TableCell>
																		<asp:TableCell BackColor="white" width="70px" height="18px" horizontalalign=center><asp:dropdownlist id="DDLOraFine6" runat="server" CssClass="Testo_campoSmall"></asp:dropdownlist></asp:TableCell>
																		<asp:TableCell BackColor="white" width="70px" height="18px" horizontalalign=center><asp:dropdownlist id="DDLMinutiFine6" runat="server" CssClass="Testo_campoSmall"></asp:dropdownlist></asp:TableCell>
																		<asp:TableCell BackColor="white" width="70px" height="18px" horizontalalign="left"><asp:Label ID="LBLerrDLL6" Runat="server" CssClass="Titolo_campoSmall" Visible="False" ForeColor="Red" Font-Bold="True">errore</asp:Label></asp:TableCell>
																	</asp:TableRow>
																	<asp:TableRow BackColor="#CEFFCE">
																		<asp:TableCell BackColor="#CEFFCE" width="45px" height="18px" HorizontalAlign="center"><asp:CheckBox id="CBGiorno0" Runat="server" CssClass="Testo_campoSmall"></asp:CheckBox></asp:TableCell>
																		<asp:TableCell BackColor="#CEFFCE" width="140px" height="18px"><asp:Label ID="LBLCSDomenica" Runat="server">Domenica</asp:Label></asp:TableCell>
																		<asp:TableCell BackColor="#CEFFCE" width="70px" height="18px" horizontalalign=center><asp:dropdownlist id="DDLOraInizio7" runat="server" CssClass="Testo_campoSmall"></asp:dropdownlist></asp:TableCell>
																		<asp:TableCell BackColor="#CEFFCE" width="70px" height="18px" horizontalalign=center><asp:dropdownlist id="DDLMinutiInizio7" runat="server" CssClass="Testo_campoSmall"></asp:dropdownlist></asp:TableCell>
																		<asp:TableCell BackColor="#CEFFCE" width="70px" height="18px" horizontalalign=center><asp:dropdownlist id="DDLOraFine7" runat="server" CssClass="Testo_campoSmall"></asp:dropdownlist></asp:TableCell>
																		<asp:TableCell BackColor="#CEFFCE" width="70px" height="18px" horizontalalign=center><asp:dropdownlist id="DDLMinutiFine7" runat="server" CssClass="Testo_campoSmall"></asp:dropdownlist></asp:TableCell>
																		<asp:TableCell BackColor="#CEFFCE" width="70px" height="18px" horizontalalign="left"><asp:Label ID="LBLerrDLL7" Runat="server" CssClass="Titolo_campoSmall" Visible="False" ForeColor="Red" Font-Bold="True">errore</asp:Label></asp:TableCell>
																	</asp:TableRow>
																</asp:Table>
															</td>
														</tr>
													</table>
												</asp:panel>
												<asp:panel id="PNLCriteriMensili" Visible="False" Runat="server" BorderWidth="2" BorderStyle="Inset" BackColor="white">
													<table border=0 cellspacing=0 cellpadding=0 >
														<tr>
															<td width=40px nowrap="nowrap" height=25px>
																<asp:radiobutton id="RBCriterioMensile1" Runat="server" CssClass="Titolo_campoSmall" Text="Il giorno" GroupName="CriterioMensili"></asp:radiobutton>
																					
															</td>
															<td height=25px nowrap="nowrap">
																&nbsp;
																<asp:textbox id="TBGiorno1" Runat="server" CssClass="Testo_campo_obbligatorioSmall" Width="25">1</asp:textbox>
																<asp:RequiredFieldValidator ControlToValidate="TBGiorno1" text="inserire numero giorni" Runat="server" ID="RfvCMnumeroGiorni" Display=Dynamic></asp:RequiredFieldValidator>
																<asp:RangeValidator ControlToValidate="TBGiorno1" MinimumValue="1" MaximumValue="31" Runat="server" Type="Integer" text="*  (1- 31)" ID="RVCMNumeroGiorni" Display=Dynamic></asp:RangeValidator>
															</td>
															<td height=25px nowrap="nowrap">
																&nbsp;&nbsp;&nbsp;&nbsp;
																<asp:Label ID="LBLCMOgni" Runat="server" CssClass="Titolo_campoSmall"></asp:Label>
																<asp:textbox id="TBMese1" Runat="server" CssClass="Testo_campo_obbligatorioSmall" Width="25">1</asp:textbox>
																<asp:Label ID="LBLCMMesi" Runat="server" CssClass="Testo_campoSmall">mese/i</asp:Label>
																<asp:RequiredFieldValidator ControlToValidate="TBMese1" text="inserire numero mesi" Runat="server" ID="RFVCMNumeroMesi"></asp:RequiredFieldValidator>
																<asp:RangeValidator ControlToValidate="TBMese1" MinimumValue="1" MaximumValue="24" Runat="server" Type="Integer" text="*  (1- 24)" ID="RVCMNumeroMesi"></asp:RangeValidator>
															</td>
														</tr>
														<tr>
															<td width=40px nowrap="nowrap" height=25px>
																<asp:radiobutton id="RBCriterioMensile2" Runat="server" CssClass="Titolo_campoSmall" Text="Ogni" GroupName="Criteriomensili"></asp:radiobutton>
															</td>
															<td colspan=2 height=25px nowrap="nowrap">
																&nbsp;
																<asp:dropdownlist id="DDLRicorrenzaMensile1" runat="server" CssClass="Testo_campo_obbligatorioSmall">
																	<asp:Listitem Value="1">primo</asp:Listitem>
																	<asp:Listitem Value="2">secondo</asp:Listitem>
																	<asp:Listitem Value="3">terzo</asp:Listitem>
																	<asp:Listitem Value="4">quarto</asp:Listitem>
																	<asp:Listitem Value="5">ultimo</asp:Listitem>
																</asp:dropdownlist>
																<asp:dropdownlist id="DDLRicorrenzaMensile2" runat="server" CssClass="Testo_campo_obbligatorioSmall">
																	<asp:Listitem Value="1">Lunedi</asp:Listitem>
																	<asp:Listitem Value="2">Martedi</asp:Listitem>
																	<asp:Listitem Value="3">Mercoledi</asp:Listitem>
																	<asp:Listitem Value="4">Giovedi</asp:Listitem>
																	<asp:Listitem Value="5">Venerdi</asp:Listitem>
																	<asp:Listitem Value="6">Sabato</asp:Listitem>
																	<asp:Listitem Value="7">Domenica</asp:Listitem>
																</asp:dropdownlist>
																&nbsp;&nbsp;&nbsp;
																<asp:Label ID="LBLCMOgni2" Runat="server" CssClass="Titolo_campoSmall">Ogni</asp:Label>
																<asp:textbox id="TBRicorrenzaMensile" runat="server" CssClass="Testo_campo_obbligatorioSmall" Width="25">1</asp:textbox>
																<asp:Label ID="LBLCMMesi2" Runat="server" CssClass="Testo_campoSmall">mese/i</asp:Label>
															</td>
														</tr>
													</table>
												</asp:panel>
												<asp:panel id="PNLCriteriAnnuali" Visible="False" Runat="server" BorderWidth="2" BorderStyle="Inset" BackColor="white">
													<table border=0 cellspacing=0 cellpadding=0 >
														<tr>
															<td height=25px>
																<asp:radiobutton id="RBLCriterioAnnuale1" Runat="server" CssClass="Titolo_campoSmall" Text="Ogni" GroupName="CriterioAnnuale"></asp:radiobutton>&nbsp;	
															</td>
															<td height=25px>
																<asp:textbox id="TBGiornoAnno" Runat="server" CssClass="Testo_campo_obbligatorioSmall" width="25"></asp:textbox>
																<asp:dropdownlist id="DDLMeseAnno" Runat="server" CssClass="Testo_campo_obbligatorioSmall">
																	<asp:listitem Value="1">Gennaio</asp:listitem>
																	<asp:listitem Value="2">Febbraio</asp:listitem>
																	<asp:listitem Value="3">Marzo</asp:listitem>
																	<asp:listitem Value="4">Aprile</asp:listitem>
																	<asp:listitem Value="5">Maggio</asp:listitem>
																	<asp:listitem Value="6">Giugno</asp:listitem>
																	<asp:listitem Value="7">Luglio</asp:listitem>
																	<asp:listitem Value="8">Agosto</asp:listitem>
																	<asp:listitem Value="9">Settembre</asp:listitem>
																	<asp:listitem Value="10">Ottobre</asp:listitem>
																	<asp:listitem Value="11">Novembre</asp:listitem>
																	<asp:listitem Value="12">Dicembre</asp:listitem>
																</asp:dropdownlist>
																<asp:Label ID="LBLCAOgni" Runat="server" CssClass="Testo_campoSmall">di ogni</asp:Label>
																<asp:textbox id="TBAnni1" Runat="server" CssClass="Testo_campo_obbligatorioSmall" width="25">1</asp:textbox>
																<asp:Label ID="LBLCAAnni" Runat="server" CssClass="Testo_campoSmall">anno/i</asp:Label>
															</td>
														</tr>
														<tr>
															<td height=25px>
																<asp:radiobutton id="RBLCriterioAnnuale2" Runat="server" CssClass="Titolo_campoSmall" Text="Ogni" GroupName="CriterioAnnuale"></asp:radiobutton>&nbsp;
															</td>
															<td height=25px>
																<asp:dropdownlist id="DDLRicorrenzaAnnuale1" runat="server" CssClass="Testo_campo_obbligatorioSmall">
																	<asp:Listitem Value="1">primo</asp:Listitem>
																	<asp:Listitem Value="2">secondo</asp:Listitem>
																	<asp:Listitem Value="3">terzo</asp:Listitem>
																	<asp:Listitem Value="4">quarto</asp:Listitem>
																	<asp:Listitem Value="5">ultimo</asp:Listitem>
																</asp:dropdownlist>
																<asp:dropdownlist id="DDLRicorrenzaAnnuale2" runat="server" CssClass="Testo_campo_obbligatorioSmall">
																	<asp:Listitem Value="1">Lunedi</asp:Listitem>
																	<asp:Listitem Value="2">Martedi</asp:Listitem>
																	<asp:Listitem Value="3">Mercoledi</asp:Listitem>
																	<asp:Listitem Value="4">Giovedi</asp:Listitem>
																	<asp:Listitem Value="5">Venerdi</asp:Listitem>
																	<asp:Listitem Value="6">Sabato</asp:Listitem>
																	<asp:Listitem Value="0">Domenica</asp:Listitem>
																</asp:dropdownlist>
																<asp:Label ID="LBLCAdi" Runat="server" CssClass="Testo_campoSmall">di</asp:Label>
																<asp:dropdownlist id="DDLRicorrenzaAnnuale3" runat="server" CssClass="Testo_campo_obbligatorioSmall">
																	<asp:listitem Value="1">Gennaio</asp:listitem>
																	<asp:listitem Value="2">Febbraio</asp:listitem>
																	<asp:listitem Value="3">Marzo</asp:listitem>
																	<asp:listitem Value="4">Aprile</asp:listitem>
																	<asp:listitem Value="5">Maggio</asp:listitem>
																	<asp:listitem Value="6">Giugno</asp:listitem>
																	<asp:listitem Value="7">Luglio</asp:listitem>
																	<asp:listitem Value="8">Agosto</asp:listitem>
																	<asp:listitem Value="9">Settembre</asp:listitem>
																	<asp:listitem Value="10">Ottobre</asp:listitem>
																	<asp:listitem Value="11">Novembre</asp:listitem>
																	<asp:listitem Value="12">Dicembre</asp:listitem>
																</asp:dropdownlist>
																<asp:Label ID="LBLCAdiOgni" Runat="server" CssClass="Testo_campoSmall">di ogni</asp:Label>
																<asp:textbox id="TBAnni2" Runat="server" CssClass="Testo_campo_obbligatorioSmall" width="25">1</asp:textbox>
																<asp:Label ID="LBLCAanno2" Runat="server" CssClass="Testo_campoSmall">anno/i</asp:Label>
															</td>
														</tr>
													</table>																	
												</asp:panel>
											</asp:TableCell>
										</asp:TableRow>
										<asp:TableRow ID="TBRluogo">
											<asp:TableCell Width=110px Wrap=false>
												<asp:label id="LBLuogo" Runat="server" CssClass="Titolo_campoSmall" text="Luogo:"></asp:label>
											</asp:TableCell>
											<asp:TableCell>
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
															<asp:Table ID="TBLprogramma" Runat=server GridLines=Both style="border-color:#000000; height:100" BorderWidth=1px height="100" CellPadding=0 CellSpacing=0>
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
                                                                  MaxHtmlLength="4000"
                                                                AllAvailableFontnames="true" AutoInitialize="true" 
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
											<asp:TableCell CssClass=top>
												<asp:textbox cssclass="Testo_campoSmall" id="TXBprogramma" TextMode="MultiLine" columns="80" runat="server" rows="5"></asp:textbox>
											</asp:TableCell>
										</asp:TableRow>
										<asp:TableRow id="TBRnote">
											<asp:TableCell Width=110px Wrap=false CssClass=top>
												<asp:label id="LBnoteNormale_t" Runat="server" CssClass="Titolo_campoSmall" text="Note:"></asp:label>
											</asp:TableCell>
											<asp:TableCell CssClass=top>
												<asp:textbox cssclass="Testo_campoSmall" id="TXBnote" TextMode="MultiLine" columns="80" runat="server" rows="3"></asp:textbox>
											</asp:TableCell>
										</asp:TableRow>
										<asp:TableRow>
											<asp:TableCell Width=110px Wrap=false>
												<asp:label id="LBLink" Runat="server" CssClass="Titolo_campoSmall" text="Link: "></asp:label>
											</asp:TableCell>
											<asp:TableCell>
												<asp:textbox id="TBLink" Runat="server" Width="100%" cssClass="Testo_campoSmall"></asp:textbox>
												<asp:RegularExpressionValidator Runat="server" Display="Dynamic" ControlToValidate="TBLink" ErrorMessage="Link troppo lungo (max 2500 caratteri)" ID="Regularexpressionvalidator5" ValidationExpression=".{0,2500}"></asp:RegularExpressionValidator>
											</asp:TableCell>
										</asp:TableRow>
										<asp:TableRow ID="TBRannoAccademico">
											<asp:TableCell Width=110px Wrap=false>
												<asp:label id="LBAnnoAccademico" Runat="server" CssClass="Titolo_campoSmall" text="Anno Accademico:"></asp:label>
											</asp:TableCell>
											<asp:TableCell>
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
							</td>
						</tr>
					</table>
				</asp:panel>
				<asp:Panel ID="PNLinfo" Runat="server" HorizontalAlign="Center" Visible="False">
					<br/><br/><br/><br/><br/>
					<table  class="TableUcFile" border=1 cellspacing=0 cellpadding=0 width=690px>
						<tr>
							<td>
								<table width=680px style="height:150px">
									<tr>
										<td colspan=3>&nbsp;</td>
									</tr>
									<tr>
										<td>&nbsp;</td>
										<td>
											<asp:Label ID="LBinfo" CssClass="avviso11_black" Runat=server ></asp:Label>
										</td>
										<td>&nbsp;</td>
									</tr>
									<tr>
										<td colspan=3>&nbsp;</td>
									</tr>
								</table>
							</td>
						</tr>
					</table>
				</asp:Panel>
				<input type=hidden id="HDNselezionato" runat=server/>
			</td>
		</tr>
	</table>
</asp:Content>

<%--<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<html>
	<head runat="server">
		<title>Inserimento Evento</title>
		<meta name="GENERATOR" content="Microsoft Visual Studio .NET 7.1"/>
		<meta name="CODE_LANGUAGE" content="Visual Basic .NET 7.1"/>
		<meta name="vs_defaultClientScript" content="JavaScript"/>
		<meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5"/>
		
		<LINK href="../Styles.css" type="text/css" rel="stylesheet"/>
		
		
	</head>

	<body>
		<form id="aspnetForm" runat="server">
		<asp:ScriptManager ID="SCMmanager" runat="server"></asp:ScriptManager>
			<table cellSpacing="1" cellPadding="1" width="780" align="center" border="0">
				<tr>
					<td colSpan="3"><HEADER:CTRLHEADER id="Intestazione" runat="server"></HEADER:CTRLHEADER></td>
				</tr>
				<tr class="contenitore" align=center>
					<td colSpan="3" align=center>

					</td>
				</tr>
			</table>
			<FOOTER:CTRLFOOTER id="Piede" runat="server"></FOOTER:CTRLFOOTER>
		</form>
	</body>
</html>
--%>