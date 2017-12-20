
<%@ Page Language="vb" AutoEventWireup="false" MasterPageFile="~/Admin_Globale/AdminPortal.Master"
 Codebehind="AdminG_ModificaComunita.aspx.vb" Inherits="Comunita_OnLine.AdminG_ModificaComunita"%>
<%@ MasterType VirtualPath="~/Admin_Globale/AdminPortal.Master" %>


<asp:Content ContentPlaceHolderID="HeadContent" runat="server" ID="Content1">
	<script type="text/javascript" language="Javascript" src="./../jscript/generali.js"></script>
	<script type="text/javascript" language="javascript">
	    function SelectMe(Me) {
	        var HIDcheckbox;
	        //eval('HIDcheckbox= this.document.forms[0].HDabilitato')
	        //eval('HIDcheckbox=<%=Me.HDabilitato.ClientID%>')
	        HIDcheckbox = this.document.getElementById('<%=Me.HDabilitato.ClientID%>');
	        for (i = 0; i < document.forms[0].length; i++) {
	            e = document.forms[0].elements[i];
	            if (e.type == 'checkbox' && e.name.indexOf("CBabilitato") != -1) {
	                if (e.checked == true) {
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

	    function SelectAll(SelectAllBox) {
	        var actVar = SelectAllBox.checked;
	        var TBcheckbox;
	        //eval('HDabilitato= this.document.forms[0].HDabilitato')
	        //eval('HIDcheckbox=<%=Me.HDabilitato.ClientID%>')
	        HIDcheckbox = this.document.getElementById('<%=Me.HDabilitato.ClientID%>');
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


	        function Controlla(obj) {
	            if (obj.checked == true) {
	                window.document.forms[0].TXBmaxIscritti.disabled = true;
	                window.document.forms[0].TXBmaxIscritti.value = " ";
	            } else {
	                window.document.forms[0].TXBmaxIscritti.disabled = false;
	                window.document.forms[0].TXBmaxIscritti.value = "30";
	            }
	        }
	
		
	</script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="CPHservice" runat="server">
	<table cellSpacing="0" cellPadding="0" width="900px" border="0">
<%--		<tr>
			<td bgColor="#a3b2cd" height="1"></td>
		</tr>
		<tr>
			<td class="titolo" align="center"><asp:label id="LBTitolo" Runat="server" CssClass="TitoloServizio">- Modifica Comunità -</asp:label></td>
		</tr>
		<tr>
			<td bgColor="#a3b2cd" height="1"></td>
		</tr>--%>
		<tr>
			<td align="center"><asp:panel id="PNLpermessi" Runat="server" Visible="False">
					<table align="center">
						<tr>
							<td height="50">&nbsp;</td>
						</tr>
						<tr>
							<td align="center">
								<asp:Label id="LBNopermessi" CssClass="messaggio" Runat="server"></asp:Label></td>
						</tr>
						<tr>
							<td height="50">&nbsp;</td>
						</tr>
					</table>
				</asp:panel>
				<asp:panel id="PNLnoquery" Runat="server" Visible="False">
					<table align="center">
						<tr>
							<td height="50" colspan=2>&nbsp;</td>
						</tr>
						<tr>
							<td align="center" colspan=2>
								<asp:Label id="LBnoquery" CssClass="messaggio" Runat="server"></asp:Label></td>
						</tr>
						<tr>
							<td height="50">&nbsp;<asp:Button CssClass ="pulsante" ID="BTNlistacmnt" Text="Lista Comunità" runat="server"></asp:Button></td>
							<td height="50">&nbsp;<asp:Button CssClass ="pulsante" ID="BTNricercacmnt" Text="Ricerca per Persona" runat="server"></asp:Button></td>
						</tr>
										
					</table>
				</asp:panel>
				<asp:panel id="PNLcontenuto" Runat="server" HorizontalAlign="Center">
					<table align="center">
						<tr>
							<td align="center">
								<FIELDSET><LEGEND class="tableLegend">Modifica la comunità di:&nbsp;<B>
											<asp:label id="LBlegenda" CssClass="tableLegend" Runat="server"></asp:label></B></LEGEND><br/>
									<table cellSpacing="0" cellPadding="0" width="800" align="center" border="0">
										<tr>
											<td colSpan="3">
												<asp:panel id="PNLComunita" Runat="server" Visible="False">
													<table cellSpacing="0" cellPadding="0" align="center">
														<tr>
															<td class="Top" width="210">
																<asp:Label ID="LBnomeComunita_c" Runat=server cssclass="Titolo_campo">*Nome comunità:</asp:Label>
															</td>
															<td class="Top" width="450" colspan =2>
			                                        			<INPUT id="HDidPadre" type="hidden" runat="server" NAME="HDidPadre"/>
			                                                        <INPUT id="HDorgn" type="hidden" runat="server" NAME="HDorgn"/>
			                                                        <INPUT id="HDtpcm" type="hidden" runat="server" NAME="HDtpcm"/>
			                                                        <INPUT id="HDcrds" type="hidden" runat="server" NAME="HDcrds"/>
																<asp:textbox id="TXBCmntNome" CssClass="Testo_campo_obbligatorio" Runat="server" MaxLength="100"
																	Columns="40" Rows=3 TextMode=MultiLine ></asp:textbox>
																<asp:requiredfieldvalidator id="Requiredfieldvalidator" runat="server" CssClass="Validatori" text="*" ControlToValidate="TXBCmntNome"
																	Display="static" EnableClientScript="true"></asp:requiredfieldvalidator></td>
														</tr>
														<tr>
															<td class="Top" width="210" height=20>
																<asp:Label ID="LBtipoComunita_c" Runat=server cssclass="Titolo_campo">&nbsp; Tipo Comunità:</asp:Label>
															</td>
															<td class="Top" width="450" colspan =2>
																<asp:Label ID="LBtipoComunita" Runat=server cssclass="Testo_campo"></asp:Label>
															</td>
														</tr>
														<tr>
															<td class="Top" width="210">
																<asp:Label ID="LBdataTermine_c" Runat=server cssclass="Titolo_campo">&nbsp; Data termine comunità:</asp:Label>
															</td>
															<td class="Top" width="450" colspan =2>
																<asp:textbox id="TXBCmntdataTermine" CssClass="Testo_campo" Runat="server" MaxLength="10" Columns="22"></asp:textbox><SPAN class="Titolo_campo">&nbsp;(gg/mm/aaaa)</SPAN>
																&nbsp;
																<asp:comparevalidator id="datatermine" CssClass="Validatori" Runat="server" ControlToValidate="TXBCmntdataTermine"
																	Display="Static" EnableClientScript="true" Operator="GreaterThan" ControlToCompare="TXBCmntdataInIscriz"
																	Type="Date">*</asp:comparevalidator></td>
														</tr>
														<tr>
															<td class="Top" width="210">
																<asp:Label ID="LBdataInizioIscr" Runat=server cssclass="Titolo_campo">*Data inizio iscrizione:</asp:Label>
															</td>
															<td class="Top" width="450" colspan =2>
																<asp:textbox id="TXBCmntdataInIscriz" CssClass="Testo_campo_obbligatorio" Runat="server" MaxLength="10"
																	Columns="22"></asp:textbox><SPAN class="Titolo_campo">&nbsp;(gg/mm/aaaa)</SPAN>
																&nbsp;
																<asp:requiredfieldvalidator id="RFVdataInizioIscr" runat="server" CssClass="Validatori" ControlToValidate="TXBCmntdataInIscriz"
																	Display="static" EnableClientScript="true">*</asp:requiredfieldvalidator></td>
														</tr>
														<tr>
															<td class="Top" width="210">
																<asp:Label ID="LBdataFineIscr" Runat=server cssclass="Titolo_campo">&nbsp; Data fine iscrizione:</asp:Label>
															</td>
															<td class="Top" width="450" colspan =2>
																<asp:textbox id="TXBCmntdataFineIscriz" CssClass="Testo_campo" Runat="server" MaxLength="10"
																	Columns="22"></asp:textbox><SPAN class="Titolo_campo">&nbsp;(gg/mm/aaaa)</SPAN>
																&nbsp;
																<asp:comparevalidator id="RFVdataFineIscr" CssClass="Validatori" Runat="server" Display="dynamic" EnableClientScript="true"
																	Operator="GreaterThan" ControlToCompare="TXBCmntdataInIscriz" Type="Date" ControlTovalidate="TXBCmntdataFineIscriz">*</asp:comparevalidator>
																<asp:comparevalidator id="CMVdateIscrizione" CssClass="Validatori" Runat="server" Display="dynamic" EnableClientScript="true"
																	Operator="LessThan" ControlToCompare="TXBCmntdataTermine" Type="Date" ControlTovalidate="TXBCmntdataFineIscriz">*</asp:comparevalidator></td>
														</tr>
														<tr>
															<td class="Top" width="210">
																<asp:Label ID="LBstatuto_c" Runat=server cssclass="Titolo_campo">&nbsp; Statuto:</asp:Label>
															</td>
															<td class="Top" width="450" colspan =2>
																<asp:textbox id="TXBCmntStatuto" CssClass="Testo_campo" Runat="server" Columns="52" Height="120px"
																	TextMode="MultiLine" Rows="10"></asp:textbox><SPAN class="Titolo_campo">Max 
																	4000 char</SPAN>
															</td>
														</tr>
														<tr>
															<td class="Top" width="210">&nbsp;</td>
															<td class="Top" width="450" colspan =2>
																<asp:radiobuttonlist id="RBapertachiusa" CssClass="Titolo_campo" Runat="server" Repeatdirection="Horizontal">
																	<asp:ListItem Value="False">Aperta</asp:ListItem>
																	<asp:ListItem Value="True">Chiusa</asp:ListItem>
																</asp:radiobuttonlist></td>
														</tr>
														<tr>
															<td class="Top" width="210">
																<asp:Label ID="LBmaxIscritti_c" Runat=server cssclass="Titolo_campo">&nbsp; Max Iscritti:</asp:Label>
															</td>
															<td class="Top" width="450" colspan =2>
																<asp:textbox id="TXBmaxIscritti" Runat="server" Columns="3" MaxLength="3" CssClass="Testo_Campo"></asp:textbox>
																&nbsp;&nbsp;Illimitati<asp:CheckBox id="CHBillimitati" Runat="server" ></asp:CheckBox>
																<asp:RangeValidator id="Rangevalidator1" runat="server" CssClass="Validatori" Display="static"
																		ControlToValidate="TXBmaxIscritti" Type="Integer" MinimumValue="1" MaximumValue="999">*</asp:RangeValidator>
															</td>
														</tr>
													</table> 
												</asp:panel>
											</td>
										</tr>
										<tr>
											<td colSpan="3">
												<asp:panel id="PNLcorso" Runat="server" Visible="False">
													<table cellSpacing="0" cellPadding="0" align="center">
														<tr>
															<td class="Top" width="210">
																<DIV class="Titolo_campo">*Codice Corso:</DIV>
															</td>
															<td class="Top" width="450">
																<asp:Textbox id="TXBcodice" CssClass="Testo_campo_obbligatorio" Runat="server" MaxLength="20"
																	Columns="20"></asp:Textbox>
																<asp:requiredfieldvalidator id="Requiredfieldvalidator5" runat="server" CssClass="Validatori" ControlToValidate="TXBcodice"
																	Display="static">*</asp:requiredfieldvalidator></td>
														</tr>
														<tr>
															<td class="Top" width="210">
																<DIV class="Titolo_campo">*Ore:</DIV>
															</td>
															<td class="Top" width="450">
																<asp:Textbox id="TXBore" CssClass="Testo_campo_obbligatorio" Runat="server" MaxLength="3" Columns="10"></asp:Textbox>
																<asp:requiredfieldvalidator id="Requiredfieldvalidator3" runat="server" CssClass="Validatori" ControlToValidate="TXBore"
																	Display="dynamic" EnableClientScript="true">*</asp:requiredfieldvalidator>
																<asp:RangeValidator id="ore" runat="server" CssClass="Validatori" Display="dynamic" EnableClientScript="true"
																	Type="Integer" ControlTovalidate="TXBore" MaximumValue="10000" MinimumValue="0">*</asp:RangeValidator></td>
														</tr>
														<tr>
															<td class="Top" width="210">
																<DIV class="Titolo_campo">*Anno Accademico:</DIV>
															</td>
															<td class="Top" width="450">
																<asp:DropDownList id="DDLanno" CssClass="Testo_campo" Runat="server" Width="258px"></asp:DropDownList></td>
														</tr>
														<tr>
															<td class="Top" width="210">
																<DIV class="Titolo_campo">&nbsp; Periodo:</DIV>
															</td>
															<td class="Top" width="450">
																<asp:DropDownList id="DDLPeriodo" CssClass="Testo_campo" Runat="server" Width="258px"></asp:DropDownList></td>
														</tr>
																			
														<tr>
															<td class="Top" width="210">
																<DIV class="Titolo_campo">&nbsp; Prerequisiti:</DIV>
															</td>
															<td class="Top" width="450">
																<asp:Textbox id="TXBprerequisiti" Runat="server" CssClass="Testo_campo" Rows="4" TextMode="MultiLine" Columns=52></asp:Textbox></td>
														</tr>
														<tr>
															<td class="Top" width="210">
																<DIV class="Titolo_campo">&nbsp;Obbiettivo:</DIV>
															</td>
															<td width="450">
																<asp:Textbox id="TXBobbiettivo" Runat="server" CssClass="Testo_campo" Rows="4" TextMode="MultiLine" Columns=52></asp:Textbox>
																</td>
														</tr>
														<tr>
															<td class="Top" width="210">
																<DIV class="Titolo_campo">*Crediti:</DIV>
															</td>
															<td class="Top" width="450">
																<asp:Textbox id="TXBcrediti" CssClass="Testo_campo_obbligatorio" Runat="server" MaxLength="3"
																	Columns="40"></asp:Textbox>
																<asp:RangeValidator id="crediti" runat="server" CssClass="Validatori" ControlToValidate="TXBcrediti"
																	Display="static" Type="Integer" MaximumValue="20" MinimumValue="0">* 0 <= crediti <=30</asp:RangeValidator></td>
														</tr>
														<tr>
															<td class="Top" width="210">
																<DIV class="Titolo_campo">&nbsp; Descrizione Programma:</DIV>
															</td>
															<td class="Top" width="450">
																<asp:textbox id="TXBdescrizioneprogramma" CssClass="Testo_campo" Runat="server" Columns="52"
																	TextMode="MultiLine" Rows="10"></asp:textbox><SPAN class="Titolo_campo">Max&nbsp;200 
																	char</SPAN> &nbsp;
																</td>
														</tr>
													</table>
												</asp:panel></td>
										</tr>
										<tr>
											<td colSpan="3">
												<asp:panel id="PNLconferenza" Runat="server" Visible="False">
													<table cellSpacing="0" cellPadding="0" align="center">
														<tr>
															<td class="Top" width="210">
																<DIV class="Titolo_campo">*Inizio Conferenza:</DIV>
															</td>
															<td class="Top" width="450">
																<asp:textbox id="TXBconferenzaInizio" CssClass="Testo_campo_obbligatorio" Runat="server" MaxLength="10"
																	Columns="22"></asp:textbox><SPAN class="Titolo_campo">&nbsp;(gg/mm/aaaa)</SPAN>&nbsp;
																<asp:rangevalidator id="VLDconferenzaInizio" runat="server" CssClass="Validatori" ControlToValidate="TXBconferenzaInizio"
																	Display="static" Type="Date" MaximumValue="31/12/3000" MinimumValue="31/12/2000">*
																	</asp:rangevalidator></td>
														</tr>
														<tr>
															<td class="Top" width="210">
																<DIV class="Titolo_campo">*Fine Conferenza:</DIV>
															</td>
															<td class="Top" width="450">
																<asp:textbox id="TXBconferenzaFine" CssClass="Testo_campo_obbligatorio" Runat="server" MaxLength="10"
																	Columns="22"></asp:textbox><SPAN class="Titolo_campo">&nbsp;(gg/mm/aaaa)</SPAN>
																&nbsp;
																<asp:rangevalidator id="VLDconferenzaFine" runat="server" CssClass="Validatori" ControlToValidate="TXBconferenzaFine"
																	Display="Dynamic" Type="Date" MaximumValue="31/12/2010" MinimumValue="31/12/2000">*</asp:rangevalidator>
																<asp:CompareValidator id="VLDconferenzaInizioFine" CssClass="Validatori" Runat="server" Display="Dynamic"
																	EnableClientScript="true" Operator="LessThan" ControlToCompare="TXBconferenzaFine" ControlTovalidate="TXBconferenzaInizio"
																	type="Date">*</asp:CompareValidator></td>
														</tr>
														<tr>
															<td class="Top" width="210">
																<DIV class="Titolo_campo">*Inizio Articolo:</DIV>
															</td>
															<td class="Top" width="450">
																<asp:textbox id="TXBconferenzaInizioArticolo" CssClass="Testo_campo_obbligatorio" Runat="server"
																	MaxLength="10" Columns="22"></asp:textbox><SPAN class="Titolo_campo">&nbsp;(gg/mm/aaaa)</SPAN>
																&nbsp;
																<asp:rangevalidator id="VLDconferenzaInizioArticolo" runat="server" CssClass="Validatori" ControlToValidate="TXBconferenzaInizioArticolo"
																	Display="Static" Type="Date" MaximumValue="31/12/2010" MinimumValue="31/12/2000">*
														</asp:rangevalidator></td>
														</tr>
														<tr>
															<td class="Top" width="210">
																<DIV class="Titolo_campo">*Fine Articolo:</DIV>
															</td>
															<td class="Top" width="450">
																<asp:textbox id="TXBconferenzaFineArticolo" CssClass="Testo_campo_obbligatorio" Runat="server"
																	MaxLength="10" Columns="22"></asp:textbox><SPAN class="Titolo_campo">&nbsp;(gg/mm/aaaa)</SPAN>
																&nbsp;
																<asp:rangevalidator id="VLDconferenzaFineArticolo" runat="server" CssClass="Validatori" ControlToValidate="TXBconferenzaInizioArticolo"
																	Display="Dynamic" Type="Date" MaximumValue="31/12/2010" MinimumValue="31/12/2000">*
															</asp:rangevalidator>
																<asp:CompareValidator id="VLDconferenzaInizioFineArticolo" CssClass="Validatori" Runat="server" Display="dynamic"
																	EnableClientScript="true" Operator="LessThan" ControlToCompare="TXBconferenzaFineArticolo" ControlTovalidate="TXBconferenzaInizioArticolo"
																	type="Date">*</asp:CompareValidator></td>
														</tr>
														<tr>
															<td class="Top" width="210">
																<DIV class="Titolo_campo">*Fine Revisione:</DIV>
															</td>
															<td class="Top" width="450">
																<asp:textbox id="TXBconferenzaFineRevisione" CssClass="Testo_campo_obbligatorio" Runat="server"
																	MaxLength="10" Columns="22"></asp:textbox><SPAN class="Titolo_campo">&nbsp;(gg/mm/aaaa)</SPAN>
																&nbsp;
																<asp:rangevalidator id="VLDconferenzaFineRevisione" runat="server" CssClass="Validatori" ControlToValidate="TXBconferenzaInizioArticolo"
																	Display="static" Type="Date" MaximumValue="31/12/2010" MinimumValue="31/12/2000">*</asp:rangevalidator></td>
														</tr>
														<tr>
															<td class="Top" width="210">
																<DIV class="Titolo_campo">*Inizio definizione:</DIV>
															</td>
															<td class="Top" width="450">
																<asp:textbox id="TXBConferenzaInizioDefinizione" CssClass="Testo_campo_obbligatorio" Runat="server"
																	MaxLength="10" Columns="22"></asp:textbox><SPAN class="Titolo_campo">&nbsp;(gg/mm/aaaa)</SPAN>
																&nbsp;
																<asp:rangevalidator id="VLDconferenzaInizioDefinizione" runat="server" CssClass="Validatori" ControlToValidate="TXBConferenzaInizioDefinizione"
																	Display="static" Type="Date" MaximumValue="31/12/2010" MinimumValue="31/12/2000">*
															</asp:rangevalidator></td>
														</tr>
														<tr>
															<td class="Top" width="210">
																<DIV class="Titolo_campo">*Fine definizione:</DIV>
															</td>
															<td class="Top" width="450">
																<asp:textbox id="TXBConferenzaFineDefinizione" CssClass="Testo_campo_obbligatorio" Runat="server"
																	MaxLength="10" Columns="22"></asp:textbox><SPAN class="Titolo_campo">&nbsp;(gg/mm/aaaa)</SPAN>
																&nbsp;
																<asp:rangevalidator id="VLDConferenzaFineDefinizione" runat="server" CssClass="Validatori" ControlToValidate="TXBConferenzaFineArticolo"
																	Display="Dynamic" Type="Date" MaximumValue="31/12/2010" MinimumValue="31/12/2000">*
															</asp:rangevalidator>
																<asp:CompareValidator id="VLDConferenzaInizioFineDefinizione" CssClass="Validatori" Runat="server" Display="dynamic"
																	EnableClientScript="true" Operator="LessThan" ControlToCompare="TXBconferenzaFineDefinizione" ControlTovalidate="TXBConferenzaInizioDefinizione"
																	type="Date">*</asp:CompareValidator></td>
														</tr>
														<tr>
															<td class="Top" width="210">
																<DIV class="Titolo_campo">*Tempo moderatore articolo:</DIV>
															</td>
															<td class="Top" width="450">
																<asp:textbox id="TXBConferenzaTempoModeratore" CssClass="Testo_campo_obbligatorio" Runat="server"
																	MaxLength="5" Columns="22"></asp:textbox><SPAN class="Titolo_campo">&nbsp;(gg/mm/aaaa)</SPAN>
																&nbsp;
																<asp:rangevalidator id="VLDConferenzaTempoModeratore" runat="server" CssClass="Validatori" ControlToValidate="TXBConferenzaTempoModeratore"
																	Display="static" Type="Integer" MaximumValue="1000" MinimumValue="0">*
															</asp:rangevalidator></td>
														</tr>
														<tr>
															<td class="Top" width="210">
																<DIV class="Titolo_campo">*Tempo moderatore revisore:</DIV>
															</td>
															<td class="Top" width="450">
																<asp:textbox id="TXBConferenzaTempoRevisore" CssClass="Testo_campo_obbligatorio" Runat="server"
																	MaxLength="5" Columns="22"></asp:textbox><SPAN class="Titolo_campo">&nbsp;(gg/mm/aaaa)</SPAN>
																&nbsp;
																<asp:rangevalidator id="VLDConferenzaTempoRevisiore" runat="server" CssClass="Validatori" ControlToValidate="TXBConferenzaTempoRevisore"
																	Display="static" Type="Integer" MaximumValue="1000" MinimumValue="0">*
															</asp:rangevalidator></td>
														</tr>
													</table>
												</asp:panel></td>
										</tr>
										<tr>
											<td colSpan="3">
												<asp:panel id="PNLCorsoStudi" Runat="server" Visible="False">
													<table cellSpacing="0" cellPadding="0" align="center">
														<tr>
															<td class="Top" width="210">
																<DIV class="Titolo_campo">*Numero Esami:</DIV>
															</td>
															<td class="Top" width="450">
																<asp:textbox id="TXBCorsoStudioNumEsami" CssClass="Testo_campo_obbligatorio" Runat="server" MaxLength="3"
																	Columns="40"></asp:textbox>&nbsp;
																<asp:RangeValidator id="VLDCorsoStudioNumEsami" runat="server" CssClass="Validatori" ControlToValidate="TXBCorsoStudioNumEsami"
																	Display="static" Type="Integer" MaximumValue="50" MinimumValue="1">*</asp:RangeValidator></td>
														</tr>
														<tr>
															<td class="Top" width="210">
																<DIV class="Titolo_campo">*MIUR:</DIV>
															</td>
															<td class="Top" width="450">
																<asp:textbox id="TXBCorsoStudioMIUR" CssClass="Testo_campo_obbligatorio" Runat="server" MaxLength="50"
																	Columns="40"></asp:textbox>&nbsp;
																<asp:requiredfieldvalidator id="VLDCorsoStudioMIUR" runat="server" CssClass="Validatori" ControlToValidate="TXBCorsoStudioMIUR"
																	Display="static">*</asp:requiredfieldvalidator></td>
														</tr>
														<tr>
															<td class="Top" width="210">
																<DIV class="Titolo_campo">&nbsp; Attributo 1:</DIV>
															</td>
															<td class="Top" width="450">
																<asp:textbox id="TXBCorsoStudioAttributo1" CssClass="Testo_campo" Runat="server" MaxLength="50"
																	Columns="40"></asp:textbox></td>
														</tr>
														<tr>
															<td class="Top" width="210">
																<DIV class="Titolo_campo">&nbsp; Attributo 2:</DIV>
															</td>
															<td class="Top" width="450">
																<asp:textbox id="TXBCorsoStudioAttributo2" CssClass="Testo_campo" Runat="server" MaxLength="50"
																	Columns="40"></asp:textbox></td>
														</tr>
														<tr>
															<td class="Top" width="210">
																<DIV class="Titolo_campo">&nbsp; Tipo corso studi:</DIV>
															</td>
															<td class="Top" width="450">
																<asp:dropdownlist id="DDLCorsoStudiTipo" CssClass="Testo_campo" Runat="server" Width="258px"></asp:dropdownlist></td>
														</tr>
													</table>
												</asp:panel></td>
										</tr>
										<tr>
											<td align="center" colSpan="3" height="80">
												<asp:button id="BTNannulla" CssClass="pulsante" Runat="server" Text="Annulla"></asp:button>
												<asp:button id="BTNCmntModifica" CssClass="pulsante" Runat="server" Text="Modifica"></asp:button>
												<asp:label id="LBAvviso" CssClass="avviso" Runat="server" Visible="False"></asp:label>
												<asp:validationsummary id="VLDSum" runat="server" EnableClientScript="true" HeaderText="Attenzione! Sono state rilevate delle imprecisioni nella compilazione del form. Controlla i valori inseriti in corrisponsenza degli *"
													ShowSummary="false" ShowMessageBox="true" DisplayMode="BulletList"></asp:validationsummary><br/><br/>
												<asp:LinkButton id="LKBassociaPadri" runat="server" CausesValidation="False">AssociaPadri</asp:LinkButton></td>
										</tr>
										<tr vAlign="middle">
											<td align="center" colSpan="3">
												<asp:panel id="PNLmessaggio" Runat="server" Visible="False">
													<asp:Label id="LBmessaggio" CssClass="avviso" Runat="server"></asp:Label>
													<br/>
													<br/>
													<asp:Button id="BTNok" CssClass="pulsante" Runat="server" Text="ok" CausesValidation="False"></asp:Button>
												</asp:panel>
											</td>
										</tr>
										<tr>
											<td colSpan="3">
												<asp:panel id="PNLAssociazione" Runat="server" Visible="False">
													<table cellSpacing="0" cellPadding="0" align="center" border=0>
														<tr>
															<td>
																<P>Vuoi associare altre comunità?</P>
																<P>&nbsp;</P>
															</td>
														</tr>
														<tr>
															<td align="center">
																<asp:Button id="BTNsi" runat="server" CssClass="Pulsante" Width="30px" Text="Si"></asp:Button>
																<asp:Button id="BTNno" runat="server" CssClass="Pulsante" Width="30px" Text="No"></asp:Button></td>
														</tr>
													</table>
												</asp:panel>&nbsp;
											</td>
										</tr>
										<tr>
											<td colSpan="3">
												<asp:panel id="PNLAssociaComunita" Runat="server" Visible="False">
													<table cellSpacing="0" cellPadding="0" align="center" border=0>
														<tr>
															<td colspan=2>
																<p><asp:Label Runat= server ID="ElencoCmntPadri_t">Elenco delle comunità che sono già padri:</asp:Label> </p>
																<asp:datagrid 
																	id="DGComunitaAssociate" 
																	runat="server" 
																	DataKeyField="CMNT_id" 
																	AllowPaging="true"
																	AutoGenerateColumns="False" 
																	ShowFooter="false"
																	PageSize="20" 
																	AllowSorting="true"
																	CssClass="DataGrid_Generica">
																	<AlternatingItemStyle CssClass="Righe_Alternate_Center"></AlternatingItemStyle>
																	<HeaderStyle CssClass="Riga_Header"></HeaderStyle>
																	<ItemStyle CssClass="Righe_Normali_center" Height="22px"></ItemStyle>
																	<PagerStyle CssClass="Riga_Paginazione" Position="Bottom" Mode="NumericPages" Visible="true"
																		HorizontalAlign="Right" Height="25px" VerticalAlign="Bottom"></PagerStyle>
																	<Columns>
																		<asp:TemplateColumn runat="server" HeaderText="Tipo" ItemStyle-Width="40">
																			<ItemTemplate>
																				<img runat=server src='<%# DataBinder.Eval(Container.DataItem, "TPCM_icona") %>' alt='<%# DataBinder.Eval(Container.DataItem, "TPCM_descrizione") %>' ID="Img1"/>
																			</ItemTemplate>
																		</asp:TemplateColumn>
																		<asp:BoundColumn DataField="CMNT_Nome" HeaderText="Nome" SortExpression="CMNT_nome"></asp:BoundColumn>
																		<asp:BoundColumn DataField="CMNT_dataCreazione" HeaderText="Data Creazione" Visible="true" SortExpression="CMNT_dataCreazione"></asp:BoundColumn>
																		<asp:BoundColumn DataField="CMNT_id" Visible="false"></asp:BoundColumn>
																	</Columns>
																	<PagerStyle Width="600px" mode="NumericPages"></PagerStyle>
																</asp:datagrid>
																<asp:Label Runat="server" CssClass="avviso_normal" ID="LBnoRecordAssociate" Visible="False">
																	Nessuna comunità padre associata
																</asp:Label>
															</td>
														</tr>
														<tr>
															<td colspan=2>
																<br/>
																<INPUT id="HDabilitato" type="hidden" name="HDabilitato" runat="server"/>
																<INPUT id="HDattivato" type="hidden" name="HDattivato" runat="server"/>
																<INPUT id="HDtutti" type="hidden" name="HDtutti" runat="server"/>
																<table width=100% align=center >
																	<tr>
																		<td>
																			<asp:Label Runat= server ID="LBelencoCmntnonPadri">Seleziona le comunità per le quali vuoi far diventare figlia la comunità:</asp:Label>
																			<B><asp:Label id="LBNomeComunita" Runat="server"></asp:Label></B>
																		</td>
																	</tr>
																	<tr>
																		<td>
																			<asp:Table ID="TBLfiltro" CellPadding=2 CellSpacing=2 BorderStyle=None Runat=server>
																				<asp:TableRow>
																					<asp:TableCell>
																						<asp:Label ID="LBorganizzazione_t" Runat=server CssClass="FiltroVoce">Organizzazione:&nbsp;</asp:Label>
																					</asp:TableCell>
																					<asp:TableCell>
																						<asp:DropDownList ID="DDLfiltroOrganizzazione" Runat=server AutoPostBack=True CssClass="FiltroCampo"></asp:DropDownList>
																					</asp:TableCell>
																					<asp:TableCell>
																						<asp:Label ID="LBtipoComunita_t" Runat=server CssClass="FiltroVoce">Comunità:</asp:Label>
																					</asp:TableCell>
																					<asp:TableCell>
																						<asp:dropdownlist id="DDLfiltroTipocomunita" runat="server" CssClass="FiltroCampo" AutoPostBack="true">
																						</asp:dropdownlist>
																					</asp:TableCell>
																				</asp:TableRow>
																				<asp:TableRow id="TBRfiltroCorso" Visible=False >
																					<asp:TableCell>
																						<asp:Label ID="LBannoAccademico_t" Runat=server CssClass="FiltroVoce">A.A.:&nbsp;</asp:Label>
																					</asp:TableCell>
																					<asp:TableCell>
																						<asp:DropDownList ID="DDLfiltroAnnoAccademico" Runat=server AutoPostBack=True CssClass="FiltroCampo"></asp:DropDownList>
																					</asp:TableCell>
																					<asp:TableCell>
																						<asp:Label ID="LBperiodo_t" Runat=server CssClass="FiltroVoce">Periodo:&nbsp;</asp:Label>
																					</asp:TableCell>
																					<asp:TableCell>
																						<asp:DropDownList ID="DDLfiltroPeriodo" Runat=server AutoPostBack=True CssClass="FiltroCampo"></asp:DropDownList>
																					</asp:TableCell>
																				</asp:TableRow>
																				<asp:TableRow id="TBRfiltroCorsoDiStudi" Visible=False >
																					<asp:TableCell>
																						<asp:Label ID="LBcorsoDiStudi_t" Runat=server CssClass="FiltroVoce">Tipo Corso di studi:&nbsp;</asp:Label>
																					</asp:TableCell>
																					<asp:TableCell>
																						<asp:DropDownList ID="DDLfiltroTipoCorsoDiStudi" Runat=server AutoPostBack=True CssClass="FiltroCampo"></asp:DropDownList>
																					</asp:TableCell>
																					<asp:TableCell ColumnSpan=2>&nbsp;</asp:TableCell>
																				</asp:TableRow>
																			</asp:Table>
																		</td>
																	</tr>
																</table>
																					
																<asp:datagrid 
																	id="DGComunita" 
																	runat="server"
																	DataKeyField="CMNT_id" 
																	AllowPaging="true"
																	AutoGenerateColumns="False" 
																	ShowFooter="false"
																	PageSize="20" 
																	AllowSorting="true"
																	CssClass="DataGrid_Generica" >
																	<AlternatingItemStyle CssClass="Righe_Alternate_Center"></AlternatingItemStyle>
																	<HeaderStyle CssClass="Riga_Header"></HeaderStyle>
																	<ItemStyle CssClass="Righe_Normali_center" Height="22px"></ItemStyle>
																	<PagerStyle CssClass="Riga_Paginazione" Position="Bottom" Mode="NumericPages" Visible="true"
																		HorizontalAlign="Right" Height="25px" VerticalAlign="Bottom"></PagerStyle>
																	<Columns>
																		<asp:TemplateColumn runat="server" HeaderText="Tipo" ItemStyle-Width="40">
																			<ItemTemplate>
																				<img runat=server src='<%# DataBinder.Eval(Container.DataItem, "TPCM_icona") %>' alt='<%# DataBinder.Eval(Container.DataItem, "TPCM_descrizione") %>' ID="Img2"/>
																			</ItemTemplate>
																		</asp:TemplateColumn>
																		<asp:BoundColumn DataField="CMNT_Nome" HeaderText="Nome" SortExpression="CMNT_nome"></asp:BoundColumn>
																		<asp:BoundColumn DataField="CMNT_dataCreazione" HeaderText="Data Creazione" Visible="true" SortExpression="CMNT_dataCreazione"></asp:BoundColumn>
																		<asp:BoundColumn DataField="CMNT_id" Visible="false"></asp:BoundColumn>
																		<asp:TemplateColumn ItemStyle-Width="20px" ItemStyle-HorizontalAlign="Center">
																			<HeaderTemplate>
																				<input type="checkbox" id="SelectAll2" onclick="SelectAll(this);" runat="server" NAME="SelectAll"/>
																			</HeaderTemplate>
																			<ItemTemplate>
																					<input type="checkbox" value=<%# DataBinder.Eval(Container.DataItem, "CMNT_id") %> id="CBabilitato" <%# DataBinder.Eval(Container.DataItem, "oCheckAbilitato") %> name="CBabilitato" onclick="SelectMe(this);">
																			</ItemTemplate>
																		</asp:TemplateColumn>
																	</Columns>
																	<PagerStyle Width="600px" mode="NumericPages"></PagerStyle>
																</asp:datagrid>
																<asp:Label Runat="server" CssClass="avviso_normal" ID="LBnoCmnt" Visible="False"></asp:Label>
																<br/>
																</td>
														</tr>	
														<tr>
															<td align =left ><asp:LinkButton ID="LKBAnnulla" Runat="server" text="--Annulla/Fine--" CausesValidation="False"></asp:LinkButton></td>
															<td align=right ><asp:LinkButton id="LKBInserisci" Runat="server" text="--Inserisci--" CausesValidation="False"></asp:LinkButton><br/><br/></td>
														</tr>	
																			
													</table>
												</asp:panel>&nbsp;
											</td>
										</tr>
									</table>
								</FIELDSET>
							</td>
						</tr>
					</table>
				</asp:panel>
			</td>
		</tr>
	</table>
</asp:Content>



<%--<!DOCTYPE HTML PUBLIC "-//W3C//Dtd HTML 4.0 transitional//EN">
<HTML>
	<head runat="server">
		<title>Comunità On Line</title>
		<LINK href="./../Styles.css" type="text/css" rel="stylesheet"/>

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
					<td><HEADER:CtrLHEADER id="Intestazione" runat="server"></HEADER:CtrLHEADER></td>
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
