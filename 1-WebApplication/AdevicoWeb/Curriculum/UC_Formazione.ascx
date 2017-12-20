<%@ Control Language="vb" AutoEventWireup="false" Codebehind="UC_Formazione.ascx.vb" Inherits="Comunita_OnLine.UC_Formazione" TargetSchema="http://schemas.microsoft.com/intellisense/ie5" %>
<input type="hidden" runat="server" id="HDNprsn_id" name="HDNprsn_id"/> 
<input type="hidden" runat="server" id="HDNcreu_id" name="HDNcreu_id"/>
<input type="hidden" runat="server" id="HDNisfr_id" name="HDNisfr_id"/>
<asp:Panel ID="PNLinserimento" Runat="server">
	<asp:Table ID="TBLinserimento" Runat="server" HorizontalAlign=Center >
		<asp:TableRow>
			<asp:TableCell>
				<asp:Label ID="LBinizio_t" Runat="server" CssClass="Titolo_campoSmall">*Data Inizio:</asp:Label>
			</asp:TableCell>
			<asp:TableCell>
				<asp:DropDownList ID="DDLinizio" Runat="server" CssClass="Testo_CampoSmall"></asp:DropDownList>
			</asp:TableCell>
			<asp:TableCell>
				<asp:Label ID="LBfine_t" Runat="server" CssClass="Titolo_campoSmall">*Data Fine:</asp:Label>
			</asp:TableCell>
			<asp:TableCell>
				<asp:DropDownList ID="DDLfine" Runat="server" CssClass="Testo_CampoSmall"></asp:DropDownList>
				<asp:Label ID="LBerrore" Runat="server" CssClass="erroreSmall"></asp:Label>&nbsp;&nbsp;
				<asp:CheckBox ID=CBXinCorso Runat="server" AutoPostBack="True" CssClass="Testo_CampoSmall" Text="Esperienza In Corso"></asp:CheckBox>
			</asp:TableCell>
		</asp:TableRow> 
	
		<asp:TableRow>
			<asp:TableCell ColumnSpan=2 CssClass="top">
				<asp:Label ID="LBnome_t" Runat="server" CssClass="Titolo_campoSmall">*Nome e Tipo Istituto:</asp:Label>
			</asp:TableCell>
			<asp:TableCell ColumnSpan=2 CssClass="top">
				<asp:textbox id="TXBnome" Runat="server" Columns="90" CssClass="Testo_CampoSmall_obbligatorio" MaxLength="1000" TextMode=MultiLine Rows=3></asp:textbox>
				<asp:requiredfieldvalidator id="Requiredfieldvalidator1" runat="server" CssClass="Validatori" ControlToValidate="TXBnome"
					Display="Static">*</asp:requiredfieldvalidator>
			</asp:TableCell>
		</asp:TableRow>
		<asp:TableRow>
			<asp:TableCell ColumnSpan=2 CssClass="top">
				<asp:Label ID="LBmaterie_t" Runat="server" CssClass="Titolo_campoSmall">&nbsp;Principali Materie o Abilità:</asp:Label>
			</asp:TableCell>
			<asp:TableCell ColumnSpan=2 CssClass="top">
				<asp:textbox id="TXBmaterie" Runat="server" Columns="90" CssClass="Testo_CampoSmall" MaxLength="1000" Rows=2></asp:textbox>
			</asp:TableCell>
		</asp:TableRow>
		<asp:TableRow>
			<asp:TableCell ColumnSpan=2 CssClass="top">
				<asp:Label ID="LBqualifica_t" Runat="server" CssClass="Titolo_campoSmall">&nbsp;Qualifica Conseguita:</asp:Label>
			</asp:TableCell>
			<asp:TableCell ColumnSpan=2 CssClass="top">
				<asp:textbox id="TXBqualifica" Runat="server" Columns="90" CssClass="Testo_CampoSmall" MaxLength="1000" TextMode=MultiLine Rows=3></asp:textbox>
			</asp:TableCell>
		</asp:TableRow>
		<asp:TableRow>
			<asp:TableCell ColumnSpan=2 CssClass="top">
				<asp:Label ID="LBclassificazione_t" Runat="server" CssClass="Titolo_campoSmall">&nbsp;Livello Classificazione Nazionale:</asp:Label>
			</asp:TableCell>
			<asp:TableCell ColumnSpan=2 CssClass="top">
				<asp:textbox id="TXBclassificazione" Runat="server" Columns="90" CssClass="Testo_CampoSmall" MaxLength="1000"></asp:textbox>
			</asp:TableCell>
		</asp:TableRow>
		<asp:TableRow>
			<asp:TableCell ColumnSpan=2>
				<asp:Label ID="LBrendiPubblico_t" Runat="server" cssclass="Titolo_campoSmall">&nbsp;Rendi Pubblico:&nbsp;</asp:Label>
			</asp:TableCell>
			<asp:TableCell ColumnSpan=2>
				<asp:CheckBox ID="CBXrendiPubblico" Runat="server" CssClass="Testo_CampoSmall" Checked="False"></asp:CheckBox>
			</asp:TableCell>
		</asp:TableRow>
		
	</asp:Table>
</asp:Panel>
<asp:Panel ID="PNLformazione" Runat="server">
	<asp:Repeater ID="RPTformazione" Runat="server">
		<ItemTemplate>
		<FIELDSET>
			<asp:Table Runat="server" ID=TBformazione HorizontalAlign =Left Width="100%" >
				<asp:TableRow runat="server" >
					<asp:TableCell  Wrap=False width=250px CssClass="top">
						<asp:Label ID="LBinizio_s" Runat="server" CssClass="Titolo_campoSmall">*Data Inizio:</asp:Label>
						
					</asp:TableCell>
					<asp:TableCell CssClass="top">
						<asp:Label ID="LBinizio" Runat="server" CssClass="Testo_CampoSmallSmall"><%#Container.DataItem("ISFR_inizio")%></asp:Label>
						&nbsp;&nbsp;&nbsp;
						<asp:Label ID="LBfine_s" Runat="server" CssClass="Titolo_campoSmall">*Data Fine:</asp:Label>
						&nbsp;
						<asp:Label ID="LBfine" Runat="server" CssClass="Testo_CampoSmallSmall"><%#Container.DataItem("ofine")%></asp:Label>
					</asp:TableCell>
				</asp:TableRow>
				<asp:TableRow>
					<asp:TableCell  Wrap=False width=250px CssClass="top">
						<asp:Label ID="LBnome_s" Runat="server" CssClass="Titolo_campoSmall">*Nome e Tipo Istituto:</asp:Label>
					</asp:TableCell>
					<asp:TableCell CssClass="top">
						<asp:Label ID="LBnome" Runat="server" CssClass="Testo_CampoSmallSmall" >
							<%#Container.DataItem("ISFR_nomeeTipoIstituto")%>
						</asp:Label>
					</asp:TableCell>
				</asp:TableRow>
				<asp:TableRow Runat=server ID="TBRmaterie">
					<asp:TableCell Wrap=False width=250px CssClass="top">
						<asp:Label ID="LBmaterie_s" Runat="server" CssClass="Titolo_campoSmall">&nbsp;Principali Materie o Abilità:</asp:Label>
					</asp:TableCell>
					<asp:TableCell CssClass="top">
						<asp:Label ID="LBmaterie" Runat="server" CssClass="Testo_CampoSmallSmall">
							<%#Container.DataItem("ISFR_principaliMaterieAbilita")%>
						</asp:Label>
					</asp:TableCell>
				</asp:TableRow>
				<asp:TableRow  Runat=server ID="TBRqualifica">
					<asp:TableCell Wrap=False width=250px CssClass="top">
						<asp:Label ID="LBqualifica_s" Runat="server" CssClass="Titolo_campoSmall">&nbsp;Qualifica Conseguita:</asp:Label>
					</asp:TableCell>
					<asp:TableCell CssClass="top">
						<asp:Label ID="LBqualifica" Runat="server" CssClass="Testo_CampoSmallSmall">
							<%#Container.DataItem("ISFR_qualificaConseguita")%>
						</asp:Label>
					</asp:TableCell>
				</asp:TableRow>
				<asp:TableRow Runat=server ID="TBRlivello">
					<asp:TableCell   Wrap=False width=250px CssClass="top">
						<asp:Label ID="LBclassificazione_s" Runat="server" CssClass="Titolo_campoSmall">&nbsp;Livello Classificazione Nazionale:</asp:Label>
					</asp:TableCell>
					<asp:TableCell CssClass="top">
						<asp:Label ID="LBclassificazione" Runat="server" CssClass="Testo_CampoSmallSmall">
							<%#Container.DataItem("ISFR_livelloClassificazioneNazionale")%>
						</asp:Label>
					</asp:TableCell>
				</asp:TableRow>
				<asp:TableRow>
					<asp:TableCell Wrap=False width=250px CssClass="top">
						<asp:Label ID="LBrendiPubblico_s" Runat="server" cssclass="Titolo_campoSmall">&nbsp;Rendi Pubblico:&nbsp;</asp:Label>
					</asp:TableCell>
					<asp:TableCell CssClass="top">
						<asp:Label ID="LBrendiPubblico" Runat="server" cssclass="Testo_CampoSmallSmall">
							<%#Container.DataItem("oRendiPubblico")%>
						</asp:Label>
					</asp:TableCell>
				</asp:TableRow>
				
				<asp:TableRow>
					<asp:TableCell ColumnSpan="2" HorizontalAlign=Right>
						<asp:LinkButton ID="LKBmodifica" Runat="server" CommandName="modifica" CommandArgument='<%#Container.DataItem("ISFR_id")%>' CssClass="Linksmall_Under">Modifica</asp:LinkButton>
						<asp:Image ID="IMpipe" ImageUrl="../images/pipe.gif" Runat=server ImageAlign=AbsMiddle></asp:Image>
						<asp:LinkButton ID="LKBelimina" Runat="server" CommandName="elimina" CommandArgument='<%#Container.DataItem("ISFR_id")%>' CssClass="Linksmall_Under">Elimina</asp:LinkButton>
					</asp:TableCell>
				</asp:TableRow>
				
			</asp:Table>
			</FIELDSET>
			<br>
		</ItemTemplate>
	</asp:Repeater>
	
		
</asp:Panel>
