<%@ Control Language="vb" AutoEventWireup="false" Codebehind="UC_EsperienzeLavorative.ascx.vb" Inherits="Comunita_OnLine.UC_EsperienzeLavorative" TargetSchema="http://schemas.microsoft.com/intellisense/ie5" %>
<input type="hidden" runat="server" id="HDNprsn_id" name="HDNprsn_id"/> 
<input type="hidden" runat="server" id="HDNcreu_id" name="HDNcreu_id"/>
<input type="hidden" runat="server" id="HDNeslv_id" name="HDNeslv_id"/>


		
<asp:Panel ID="PNLinserimento" Runat="server">
	<asp:Table ID="TBLinserimento" Runat="server" HorizontalAlign=Center >
		<asp:TableRow>
			<asp:TableCell ColumnSpan=4>
				<asp:Table Runat=server HorizontalAlign=Left Width=100%>
					<asp:TableRow>
						<asp:TableCell Width=120px>
							<asp:Label ID="LBinizio_t" Runat="server" CssClass="Titolo_campoSmall">*Data Inizio:</asp:Label>
						</asp:TableCell>
						<asp:TableCell CssClass ="Top">
							<asp:label id="LBdataI" Runat="server" CssClass="Testo_campoSmall" text=""></asp:label>&nbsp;
	   						<asp:ImageButton ID="IMBapriInizio" ImageUrl="../images/cal.gif" Runat="server" CausesValidation="False"></asp:ImageButton>
	   						<input type=hidden id="HDNdataI" runat=server NAME="HDNdataI"/>
						</asp:TableCell>
						<asp:TableCell>
							<asp:Label ID="LBfine_t" Runat="server" CssClass="Titolo_campoSmall">*Data Fine:</asp:Label>
						</asp:TableCell>
						<asp:TableCell CssClass ="Top">
							 <asp:label id="LBdataF" Runat="server" CssClass="Testo_campoSmall" text=""></asp:label>&nbsp;
							<asp:ImageButton ID="IMBapriFine" ImageUrl="../images/cal.gif" Runat="server" CausesValidation="False"></asp:ImageButton>
							<asp:CheckBox ID=CBXinCorso Runat="server" AutoPostBack="True" CssClass="Testo_CampoSmall" Text="Esperienza In Corso"></asp:CheckBox>
							<br><asp:label id="LBLErrData" Runat="server" CssClass="messaggio" ForeColor="Red" text="Data Fine NON VALIDA"
								Visible="False"></asp:label>
								<input type=hidden id="HDNdataF" runat=server NAME="HDNdataF"/>
								
								<script type=text/javascript>
							function selectInit(calendar,date){
								var esiste = false;
									if (calendar.dateClicked){
										dataI = date
										dataF = document.getElementById("<%=me.HDNdataF.clientID%>").value
										dataIniziale = dataI.split("/")
										dataFinale = dataF.split("/")
										var dataInizio = new Date()
										var dataFine = new Date()
										dataInizio = Date.parse(dataIniziale[1] + '/' + dataIniziale[0] + '/' + dataIniziale[2])
										dataFine =  Date.parse(dataFinale[1] + '/' + dataFinale[0] + '/' + dataFinale[2])
										if (dataInizio>dataFine){
											document.getElementById("<%=me.HDNdataF.clientID%>").value = date
											document.getElementById("<%=me.LBdataF.clientID%>").innerHTML = date
											}
										document.getElementById("<%=me.HDNdataI.clientID%>").value = date																
										document.getElementById("<%=me.LBdataI.clientID%>").innerHTML = date
										calendar.callCloseHandler()
									}
									
								}
							function selectEnd(calendar,date){
									
									if (calendar.dateClicked){
										dataI  = document.getElementById("<%=me.HDNdataI.clientID%>").value;
										dataF = date;
										dataIniziale = dataI.split("/")
										dataFinale = dataF.split("/")
										var dataInizio = new Date()
										var dataFine = new Date()
										dataInizio = Date.parse(dataIniziale[1] + '/' + dataIniziale[0] + '/' + dataIniziale[2])
										dataFine =  Date.parse(dataFinale[1] + '/' + dataFinale[0] + '/' + dataFinale[2])

										if (dataInizio>dataFine){
											document.getElementById("<%=me.HDNdataI.clientID%>").value = date
											document.getElementById("<%=me.LBdataI.clientID%>").innerHTML = date
											}
										document.getElementById("<%=me.HDNdataF.clientID%>").value = date																
										document.getElementById("<%=me.LBdataF.clientID%>").innerHTML = date
										calendar.callCloseHandler()
									}
									
								}
							Calendar.setup({
								ifFormat : "%d/%m/%Y",
								inputField : "<%=me.HDNdataI.clientID%>",
								displayArea: "<%=me.LBdataI.clientID%>",
								button : "<%=me.IMBapriInizio.clientID%>",
								firstDay : 1,
								onSelect : selectInit
							}
								);
								
							Calendar.setup({
								ifFormat : "%d/%m/%Y",
								inputField : "<%=me.HDNdataF.clientID%>",
								displayArea: "<%=me.LBdataF.clientID%>",
								button : "<%=me.IMBapriFine.clientID%>",
								firstDay : 1,
								onSelect : selectEnd
							
								
							}
								);
						</script>
						</asp:TableCell>
					</asp:TableRow>
				</asp:Table>
			</asp:TableCell>			
		</asp:TableRow>
		<asp:TableRow>
			<asp:TableCell ColumnSpan="2" Wrap=False Width=120px>
				<asp:Label ID="LBnome_t" Runat="server" CssClass="Titolo_campoSmall">*Nome Datore:</asp:Label>
			</asp:TableCell>
			<asp:TableCell ColumnSpan="2">
				<asp:textbox id="TXBnome" Runat="server" Columns="90" CssClass="Testo_campo_obbligatorioSmall" MaxLength="2000"></asp:textbox>
				<asp:requiredfieldvalidator id="Requiredfieldvalidator1" runat="server" CssClass="Validatori" ControlToValidate="TXBnome"
					Display="Static">*</asp:requiredfieldvalidator>
			</asp:TableCell>
		</asp:TableRow>
		<asp:TableRow>
			<asp:TableCell ColumnSpan="2" CssClass="top" Wrap=False Width=120px>
				<asp:Label ID="LBsettore_t" Runat="server" CssClass="Titolo_campoSmall">&nbsp;Settore:</asp:Label>
			</asp:TableCell>
			<asp:TableCell ColumnSpan="2">
				<asp:textbox id="TXBsettore" Runat="server" Columns="90" TextMode="MultiLine" Rows="5" CssClass="Testo_CampoSmall"
					MaxLength="2000"></asp:textbox>
			</asp:TableCell>
		</asp:TableRow>
		<asp:TableRow>
			<asp:TableCell ColumnSpan="2" CssClass="top" Wrap=False Width=120px>
				<asp:Label ID="LBtipoImpiego_t" Runat="server" CssClass="Titolo_campoSmall">&nbsp;Tipo Impiego:</asp:Label>
			</asp:TableCell>
			<asp:TableCell ColumnSpan="2">
				<asp:textbox id="TXBtipoImpiego" Runat="server" Columns="90" TextMode="MultiLine" Rows="5" CssClass="Testo_CampoSmall"
					MaxLength="2000"></asp:textbox>
			</asp:TableCell>
		</asp:TableRow>
		<asp:TableRow>
			<asp:TableCell ColumnSpan="2" CssClass="top" Wrap=False Width=120px>
				<asp:Label ID="LBmansione_t" Runat="server" CssClass="Titolo_campoSmall">&nbsp;Mansione:</asp:Label>
			</asp:TableCell>
			<asp:TableCell ColumnSpan="2">
				<asp:textbox id="TXBmansione" Runat="server" Columns="90" TextMode="MultiLine" Rows="5" CssClass="Testo_CampoSmall"
					MaxLength="2000"></asp:textbox>
			</asp:TableCell>
		</asp:TableRow>
		<asp:TableRow>
			<asp:TableCell ColumnSpan="2" Wrap=False Width=120px>
				<asp:Label ID="LBrendiPubblico_t" Runat="server" cssclass="Titolo_campoSmall">&nbsp;Rendi Pubblico:&nbsp;</asp:Label>
			</asp:TableCell>
			<asp:TableCell ColumnSpan="2">
				<asp:CheckBox ID="CBXrendiPubblico" Runat="server" CssClass="Testo_CampoSmall" Checked="False"></asp:CheckBox>
			</asp:TableCell>
		</asp:TableRow>
		
	</asp:Table>
</asp:Panel>
<asp:Panel ID="PNLesperienze" Runat="server">
	<asp:Repeater ID="RPTesperienze" Runat="server">
		<ItemTemplate>
			<FIELDSET>
				<asp:Table HorizontalAlign="center" width="100%" Runat="server" ID="TBesperienze">
					<asp:TableRow runat="server" ID="TBRinizio" CssClass="top">
						<asp:TableCell Wrap=False width=180px>
							<asp:Label ID="LBinizio_s" Runat="server" CssClass="Titolo_campoSmall">Data Inizio:</asp:Label>							
						</asp:TableCell>
						<asp:TableCell CssClass="top">		
							<asp:Label ID="LBinizio" Runat="server" CssClass="Testo_campoSmall"><%#Container.DataItem("oInizio")%></asp:Label>
							&nbsp;&nbsp;&nbsp;
							<asp:Label ID="LBfine_s" Runat="server" CssClass="Titolo_campoSmall">Data Fine:</asp:Label>
							&nbsp;	<asp:Label ID="LBfine" Runat="server" CssClass="Testo_campoSmall"><%#Container.DataItem("oFine")%></asp:Label>
						</asp:TableCell>
					</asp:TableRow>
					<asp:TableRow>
						<asp:TableCell Wrap=False width=120px CssClass="top">
							<asp:Label ID="LBnome_s" Runat="server" CssClass="Titolo_campoSmall">&nbsp;Nome Datore:</asp:Label>
						</asp:TableCell>
						<asp:TableCell HorizontalAlign=Left CssClass="top" Width="600px">
							<asp:Label ID="LBnome" Runat="server" CssClass="Testo_campoSmall">
								<%#Container.DataItem("ESLV_nomeDatore")%>
							</asp:Label>
						</asp:TableCell>
					</asp:TableRow>
					<asp:TableRow Runat="server" ID="TBRsettore">
						<asp:TableCell Wrap=False width=120px CssClass="top">
							<asp:Label ID="LBsettore_s" Runat="server" CssClass="Titolo_campoSmall">&nbsp;Settore:</asp:Label>
						</asp:TableCell>
						<asp:TableCell HorizontalAlign=Left CssClass="top" Width="600px">
							<asp:Label ID="LBsettore" Runat="server" CssClass="Testo_campoSmall">
								<%#Container.DataItem("ESLV_settore")%>
							</asp:Label>
						</asp:TableCell>
					</asp:TableRow>
					<asp:TableRow Runat="server" ID="TBRtipoImpiego">
						<asp:TableCell Wrap=False width=120px CssClass="top">
							<asp:Label ID="LBtipoImpiego_s" Runat="server" CssClass="Titolo_campoSmall">&nbsp;Tipo Impiego:</asp:Label>
						</asp:TableCell>
						<asp:TableCell HorizontalAlign=Left CssClass="top" Width="600px">
							<asp:Label ID="LBtipoImpiego" Runat="server" CssClass="Testo_campoSmall">
								<%#Container.DataItem("ESLV_tipoImpiego")%>
							</asp:Label>
						</asp:TableCell>
					</asp:TableRow>
					<asp:TableRow Runat="server" ID="TBRmansione">
						<asp:TableCell Wrap=False width=120px CssClass="top">
							<asp:Label ID="LBmansione_s" Runat="server" CssClass="Titolo_campoSmall">&nbsp;Mansione:</asp:Label>
						</asp:TableCell>
						<asp:TableCell HorizontalAlign=Left CssClass="top" Width="600px">
							<asp:Label ID="LBmansione" Runat="server" CssClass="Testo_campoSmall">
								<%#Container.DataItem("ESLV_mansione")%>
							</asp:Label>
						</asp:TableCell>
					</asp:TableRow>
					<asp:TableRow>
						<asp:TableCell  Wrap=False width=180px CssClass="top">
							<asp:Label ID="LBrendiPubblico_s" Runat="server" cssclass="Titolo_campoSmall">&nbsp;Rendi Pubblico:&nbsp;</asp:Label>
						</asp:TableCell>
						<asp:TableCell HorizontalAlign=Left CssClass="top" Width="600px">
							<asp:Label ID="LBrendiPubblico" Runat="server" cssclass="Testo_campoSmall">
								<%#Container.DataItem("oRendiPubblico")%>
							</asp:Label>
						</asp:TableCell>
					</asp:TableRow>
					<asp:TableRow>
						<asp:TableCell ColumnSpan="2" HorizontalAlign=Right>
							<asp:LinkButton ID="LKBmodifica" Runat="server" CommandName="modifica" CommandArgument='<%#Container.DataItem("ESLV_id")%>' CssClass="Linksmall_Under">Modifica</asp:LinkButton>
							<asp:Image ID="IMpipe" ImageUrl="../images/pipe.gif" Runat="server" ImageAlign=AbsMiddle></asp:Image>
							<asp:LinkButton ID="LKBelimina" Runat="server" CommandName="elimina" CommandArgument='<%#Container.DataItem("ESLV_id")%>' CssClass="Linksmall_Under">Elimina</asp:LinkButton>
						</asp:TableCell>
					</asp:TableRow>
				</asp:Table>
			</FIELDSET>
			<br>
		</ItemTemplate>
	</asp:Repeater>
</asp:Panel>
