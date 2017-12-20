<%@ Control Language="vb" AutoEventWireup="false" Codebehind="UC_DatiCurriculum.ascx.vb" Inherits="Comunita_OnLine.UC_DatiCurriculum" TargetSchema="http://schemas.microsoft.com/intellisense/ie5" %>
<input type="hidden" runat="server" id="HDNprsn_id" name="HDNprsn_id"/> 
<input type="hidden" runat="server" id="HDNcrev_id" name="HDNcrev_id"/>

<asp:Table ID="TBLdati" Runat="server" GridLines=none width=700px HorizontalAlign=Center >
	<asp:TableRow Runat="server" ID="TBRmessaggio" Visible="False">
		<asp:TableCell ColumnSpan="4" HorizontalAlign="Center">
			<asp:label ID="LBmessaggio" Runat="server" CssClass="errore"></asp:label>
		</asp:TableCell>
	</asp:TableRow>
	<asp:TableRow Runat="server">
		<asp:TableCell Width=100px Wrap=False >
			<asp:Label ID="LBnome_t" Runat="server" CssClass="Titolo_campoSmall">*Nome:</asp:Label>
		</asp:TableCell>
		<asp:TableCell  Runat="server" ColumnSpan=2>
			<asp:textbox id="TXBnome" Runat="server" Columns="30" CssClass="Testo_CampoSmall" MaxLength="100"></asp:textbox>
			<asp:requiredfieldvalidator id="VLDLogin" runat="server" CssClass="Validatori" ControlToValidate="TXBnome" Display=Static>*</asp:requiredfieldvalidator>
		</asp:TableCell>
		<asp:TableCell VerticalAlign=top RowSpan=4 HorizontalAlign=Right CssClass="top">
			<table bgcolor="#6699ff" align=right >
				<tr align="center">
					<td colspan="2"><asp:label ID="LBnote" Runat="server">Note</asp:label></td>
				</tr>
				<tr>
					<td><asp:Label ID="LBrendiPubblico_t" Runat="server" cssclass="Titolo_campoSmall">Rendi Pubblico:&nbsp;</asp:Label> </td>
					<td><asp:CheckBox ID="CBXrendiPubblico" Runat="server" CssClass="Testo_campoSmall" Checked="False"></asp:CheckBox></td>
				</tr>
				<tr bgcolor=#99ccff>
					<td><asp:Label ID="LBmostraDatiSensibili_t" Runat="server" cssclass="Titolo_campoSmall">Mostra Dati Sensibili:&nbsp;</asp:Label></td>
					<td><asp:CheckBox ID="CBXmostraDatiSensibili" Runat="server"  BackColor=#99ccff Checked="False"></asp:CheckBox></td>
				</tr>
				<tr bgcolor=#99ff76>
					<td><asp:Label ID="LBmostraRecapiti_t" Runat="server" cssclass="Titolo_campoSmall">Mostra Recapiti:&nbsp;</asp:Label></td>
					<td><asp:CheckBox ID="CBXmostraRecapiti" Runat="server" BackColor=#99ff76 Checked="False"></asp:CheckBox></td>
				</tr>
			</table>
		</asp:TableCell>
	</asp:TableRow>
	<asp:TableRow>
		<asp:TableCell Width=100px>
			<asp:Label ID="LBcognome_t" Runat="server" CssClass="Titolo_campoSmall">*Cognome:</asp:Label>
		</asp:TableCell>
		<asp:TableCell ColumnSpan=2>
			<asp:textbox id="TXBcognome" Runat="server" Columns="30" CssClass="Testo_CampoSmall"
				MaxLength="100"></asp:textbox>
			<asp:requiredfieldvalidator id="Requiredfieldvalidator1" runat="server" CssClass="Validatori" ControlToValidate="TXBcognome"
				Display="Static">*</asp:requiredfieldvalidator>
		</asp:TableCell>
		
	</asp:TableRow>
	<asp:TableRow>
		<asp:TableCell Width=100px>
			<asp:Label ID="LBdatanascita_t" Runat="server" CssClass="Titolo_campoSmall">*Data Nascita(gg/mm/aaaa):</asp:Label>
		</asp:TableCell>
		<asp:TableCell ColumnSpan=2>
			<asp:textbox id="TXBdataNascita" Runat="server" Columns="20" CssClass="Testo_CampoSensibile" MaxLength="10"></asp:textbox>
			<asp:requiredfieldvalidator id="Requiredfieldvalidator6" runat="server" CssClass="Validatori" ControlToValidate="TXBdataNascita" Display="dynamic">*</asp:requiredfieldvalidator>
			<asp:rangevalidator id="rangeValDate" runat="server" CssClass="Validatori" ControlToValidate="TXBdataNascita" Display="Dynamic" Type="Date" MinimumValue="01/01/1111" MaximumValue="01/01/2999">*
			</asp:rangevalidator>
		</asp:TableCell>
	</asp:TableRow>
	<asp:TableRow Runat="server">
		<asp:TableCell Width=100px>
			<asp:Label ID="LBsesso_t" Runat="server" cssclass="Titolo_campoSmall">&nbsp;Sesso:</asp:Label>
		</asp:TableCell>
		<asp:TableCell ColumnSpan=2>
			<asp:radiobuttonlist id="RBLSesso" Runat="server" Repeatdirection="Horizontal" CssClass="Testo_CampoSensibile" RepeatLayout=Flow >
				<asp:listitem Value="1" Selected="true">Uomo&nbsp;&nbsp;&nbsp;</asp:listitem>
				<asp:ListItem Value="0">Donna</asp:ListItem>
			</asp:radiobuttonlist>

		</asp:TableCell>
	</asp:TableRow>
	<asp:TableRow>
		<asp:TableCell Width=100px>
			<asp:Label ID="LBindirizzo_t" Runat="server" CssClass="Titolo_campoSmall">&nbsp;Indirizzo:</asp:Label>
		</asp:TableCell>
		<asp:TableCell ColumnSpan="3">
			<asp:textbox id="TXBindirizzo" Runat="server" Columns="50" CssClass="Testo_CampoSensibile" MaxLength="200"></asp:textbox>
		</asp:TableCell>
	</asp:TableRow>
	<asp:TableRow>
		<asp:TableCell Width=100px>
			<asp:Label ID="LBcap_t" Runat="server" CssClass="Titolo_campoSmall">&nbsp;Cap:</asp:Label>
		</asp:TableCell>
		<asp:TableCell>
			<asp:textbox id="TXBcap" Runat="server" Columns="15" CssClass="Testo_CampoSensibile" MaxLength="10"></asp:textbox>
			<asp:regularexpressionvalidator id="Cap" runat="server" CssClass="Validatori" ControlToValidate="TXBcap" Display=Static
				ValidationExpression="^\d{1,10}$">*</asp:regularexpressionvalidator>
		</asp:TableCell>
		<asp:TableCell>
			<asp:Label ID="LBcitta_t" Runat="server" CssClass="Titolo_campoSmall">&nbsp;Città:</asp:Label>
		</asp:TableCell>
		<asp:TableCell >
			<asp:textbox id="TXBcitta" Runat="server" Columns="40" CssClass="Testo_CampoSensibile" MaxLength="25"></asp:textbox>
		</asp:TableCell>
	</asp:TableRow>
	<asp:TableRow>
		<asp:TableCell Width=100px>
			<asp:Label ID="LBtelefono_t" Runat="server" CssClass="Titolo_campoSmall">&nbsp;Telefono:</asp:Label>
		</asp:TableCell>
		<asp:TableCell >
			<asp:textbox id="TXBtelefono" Runat="server" Columns="30" CssClass="Testo_campoRecapitiTel" MaxLength="25"></asp:textbox>
		</asp:TableCell>
		<asp:TableCell>
			<asp:Label ID="LBcellulare_t" Runat="server" CssClass="Titolo_campoSmall">&nbsp;Cellulare:</asp:Label>
		</asp:TableCell>
		<asp:TableCell >
			<asp:textbox id="TXBcellulare" Runat="server" Columns="40" CssClass="Testo_campoRecapitiTel" MaxLength="25"></asp:textbox>
		</asp:TableCell>
	</asp:TableRow>
	<asp:TableRow>
		<asp:TableCell Width=100px>
			<asp:Label ID="LBfax_t" Runat="server" CssClass="Titolo_campoSmall">&nbsp;Fax:</asp:Label>
		</asp:TableCell>
		<asp:TableCell>
			<asp:textbox id="TXBfax" Runat="server" Columns="30" CssClass="Testo_campoRecapitiTel" MaxLength="25"></asp:textbox>
		</asp:TableCell>
		<asp:TableCell>
			<asp:Label ID="LBmail_t" Runat="server" CssClass="Titolo_campoSmall">*Mail:</asp:Label>
		</asp:TableCell>
		<asp:TableCell>
			<asp:textbox id="TXBmail" Runat="server" Columns="40" CssClass="Testo_CampoSmall" MaxLength="255"></asp:textbox>
			<asp:requiredfieldvalidator id="emailReqVal" Runat="server" CssClass="Validatori" ControlToValidate="TXBmail"
				Display="dynamic">*</asp:requiredfieldvalidator>
			<asp:regularexpressionvalidator id="emailRegexVal" runat="server" CssClass="Validatori" ControlToValidate="TXBmail"
				Display="dynamic" ValidationExpression="^([0-9a-zA-Z]([-.\w]*[0-9a-zA-Z])*@([0-9a-zA-Z][-\w]*[0-9a-zA-Z]\.)+[a-zA-Z]{2,9})$">*</asp:regularexpressionvalidator>
		</asp:TableCell>
	</asp:TableRow>
	<asp:TableRow>
		<asp:TableCell Width=100px>
			<asp:Label ID="LBnazionalita_t" Runat="server" CssClass="Titolo_campoSmall">&nbsp;Nazionalità:</asp:Label>
		</asp:TableCell>
		<asp:TableCell>
			<asp:textbox id="TXBnazionalita" Runat="server" Columns="40" CssClass="Testo_campoSensibile" MaxLength="250"></asp:textbox>
		</asp:TableCell>
		<asp:TableCell>
			<asp:Label ID="LBmadrelingua_t" Runat="server" CssClass="Titolo_campoSmall">&nbsp;Madrelingua:</asp:Label>
		</asp:TableCell>
		<asp:TableCell>
			<asp:textbox id="TXBmadrelingua" Runat="server" Columns="40" CssClass="Testo_campoSensibile" MaxLength="100"></asp:textbox>
			<input type="hidden" id="HDNMadrelingua" runat="server" NAME="HDNMadrelingua">
		</asp:TableCell>
	</asp:TableRow>
	
	<asp:TableRow>
		<asp:TableCell Width=100px>
			<asp:Label ID="LBpatente_t" Runat="server" CssClass="Titolo_campoSmall">&nbsp;Patente:</asp:Label>
		</asp:TableCell>
		<asp:TableCell ColumnSpan="3">
			<asp:textbox id="TXBpatente" Runat="server" Columns="40" CssClass="Testo_CampoSmall" MaxLength="250"></asp:textbox>
			<input type="hidden" id="HDNPatente" runat="server" name="HDNPatente">
		</asp:TableCell>
	</asp:TableRow>
	
</asp:Table>
