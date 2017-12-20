<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="UC_Fase1DatiComunita.ascx.vb" Inherits="Comunita_OnLine.UC_Fase1DatiComunita" %>
<%@ Register TagPrefix="CTRL" TagName="CTRLtags" Src="~/Modules/Tag/UC/UC_TagsSelectorForCommunity.ascx" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>

<script type="text/javascript">
    function validateFunction(sender, eventArgs) {
        eventArgs.IsValid = true;
      
        var firstPicker = $find("<%=RDPsubscriptionsStart.ClientID %>");
        var secondPicker = $find("<%=RDPsubscriptionsEnd.ClientID %>");
        if (secondPicker.get_selectedDate() <= firstPicker.get_selectedDate()) {
            eventArgs.IsValid = false;
        }
    }
    function validateEndDateFunction(sender, eventArgs) {
        eventArgs.IsValid = true;

        var firstPicker = $find("<%=RDPsubscriptionsStart.ClientID %>");
        var secondPicker = $find("<%=RDPdatetimeEnd.ClientID %>");
        if (secondPicker.get_selectedDate() <= firstPicker.get_selectedDate()) {
            eventArgs.IsValid = false;
        }
    }
</script>

<asp:Table ID="TBLcomunita" runat="server" HorizontalAlign="Center" Width="850px"
    GridLines="none" CellSpacing="0">
    <asp:TableRow ID="TBRstatus" CssClass="StatusAttivo_Riga">
        <asp:TableCell>
            <asp:Label ID="LBstatus_t" runat="server" CssClass="StatusAttivo_Titolo_campoSmall">&nbsp;Status Comunità:</asp:Label>
        </asp:TableCell>
        <asp:TableCell ColumnSpan="3">
            <asp:Image runat="server" Visible="false" ImageAlign="absMiddle" ImageUrl="./../../images/Attenzione.png"
                ID="IMGavviso" />
            <asp:Label ID="LBavvisoStatus" runat="server" Visible="false"></asp:Label>
            <asp:RadioButtonList ID="RBLstatus" runat="server" CssClass="StatusAttivo_Testo_CampoSmall"
                RepeatDirection="Horizontal" RepeatLayout="Flow">
                <asp:ListItem Value="0" Selected="true">Attivata</asp:ListItem>
                <asp:ListItem Value="1">Archiviata</asp:ListItem>
                <asp:ListItem Value="2">Bloccata</asp:ListItem>
            </asp:RadioButtonList>
        </asp:TableCell>
    </asp:TableRow>
    <asp:TableRow ID="TBRstart" CssClass="StatusAttivo_Titolo_campoSmall">
        <asp:TableCell>
            <asp:Label ID="LBtipoComunita_t" runat="server" CssClass="StatusAttivo_Titolo_campoSmall">*Tipo Comunità:</asp:Label>
        </asp:TableCell>
        <asp:TableCell ColumnSpan="3">
            <asp:DropDownList ID="DDLtipoComunita" runat="server" CssClass="StatusAttivo_Riga"
                Width="258px" AutoPostBack="true">
            </asp:DropDownList>
        </asp:TableCell>
    </asp:TableRow>
    <asp:TableRow>
        <asp:TableCell CssClass="top" Width="150px">
            <asp:Label ID="LBtags_t" runat="server" CssClass="Titolo_campoSmall">&nbsp;*Select tags:</asp:Label>
        </asp:TableCell>
        <asp:TableCell CssClass="top" ColumnSpan="3">            
            <CTRL:CTRLtags ID="CTRLtagsSelector" runat="server" InLineStyle="width:581px" />
        </asp:TableCell>
    </asp:TableRow>

    <asp:TableRow>
        <asp:TableCell CssClass="top" Width="150px">
            <asp:Label ID="LBnome_t" runat="server" CssClass="Titolo_campoSmall">*Nome:</asp:Label>
        </asp:TableCell>
        <asp:TableCell CssClass="top" ColumnSpan="3">
            <asp:TextBox ID="TXBCmntNome" runat="server" CssClass="Testo_campo_obbligatorioSmall"
                Columns="80" Rows="3" TextMode="MultiLine" MaxLength="200"></asp:TextBox>
            <asp:RequiredFieldValidator ID="RFVnomeComunita" runat="server" CssClass="Validatori"
                EnableClientScript="true" Display="static" ControlToValidate="TXBCmntNome" Text="*"></asp:RequiredFieldValidator>
        </asp:TableCell>
    </asp:TableRow>
    <asp:TableRow ID="TBRdataTermine">
        <asp:TableCell CssClass="top" Width="150px">
            <asp:Label ID="LBdatatermine_t" runat="server" CssClass="Titolo_campoSmall">&nbsp;Data termine:</asp:Label>
        </asp:TableCell>
        <asp:TableCell CssClass="top" ColumnSpan="3" Wrap="False">
           <telerik:RadDateTimePicker id="RDPdatetimeEnd" runat="server" ></telerik:RadDateTimePicker>
            &nbsp;
            <asp:CustomValidator runat="server" ID="CSVdatatermine" ClientValidationFunction="validateEndDateFunction" ErrorMessage="*"
            EnableClientScript="true" ControlToValidate="RDPdatetimeEnd" ></asp:CustomValidator>
        </asp:TableCell>
    </asp:TableRow>
    <asp:TableRow ID="TBRdataIscrizione">
        <asp:TableCell Width="150px">
            <asp:Label ID="LBdataInizioIscr_t" runat="server" CssClass="Titolo_campoSmall">*Data inizio iscrizione:</asp:Label>
        </asp:TableCell>
        <asp:TableCell Wrap="False">
            <telerik:RadDateTimePicker id="RDPsubscriptionsStart" runat="server" ></telerik:RadDateTimePicker>
            &nbsp;
            <asp:RequiredFieldValidator runat="server" CssClass="Validatori" EnableClientScript="true"
                Display="static" ControlToValidate="RDPsubscriptionsStart" ID="RFVdataInIscriz">*</asp:RequiredFieldValidator>
        </asp:TableCell>
        <asp:TableCell ColumnSpan="2" Wrap="False">
            <asp:Label ID="LBdataFineIscr_t" runat="server" CssClass="Titolo_campoSmall">&nbsp;Data fine iscrizione:</asp:Label>
            &nbsp;&nbsp;
           <telerik:RadDateTimePicker id="RDPsubscriptionsEnd" runat="server" ></telerik:RadDateTimePicker>
            &nbsp;
            <asp:CustomValidator runat="server" ID="CSVdate" ClientValidationFunction="validateFunction" ErrorMessage="End date must be after start date"
            EnableClientScript="true" ControlToValidate="RDPsubscriptionsEnd" ></asp:CustomValidator>
        </asp:TableCell>
    </asp:TableRow>
    <asp:TableRow ID="TBRisAperta">
        <asp:TableCell CssClass="top" Width="150px">
            <asp:Label ID="LBisAperta_t" runat="server" CssClass="Titolo_campoSmall">&nbsp;Stato:</asp:Label>
        </asp:TableCell>
        <asp:TableCell CssClass="top" ColumnSpan="3">
            <asp:RadioButtonList ID="RBapertachiusa" runat="server" CssClass="Testo_campoSmall"
                RepeatDirection="Horizontal" RepeatLayout="Flow">
                <asp:ListItem Value="False" Selected="true">Aperta</asp:ListItem>
                <asp:ListItem Value="True">Chiusa</asp:ListItem>
            </asp:RadioButtonList>
        </asp:TableCell>
    </asp:TableRow>
    <asp:TableRow ID="TBRiscritti">
        <asp:TableCell Width="150px">
            <asp:Label ID="LBnumIscrizioni" runat="server" CssClass="Titolo_campoSmall">&nbsp;N° iscritti:</asp:Label>
        </asp:TableCell>
        <asp:TableCell ColumnSpan="3">
            <asp:RadioButtonList ID="RBLnumeroIscritti" runat="server" RepeatDirection="Horizontal"
                CssClass="Testo_campoSmall" RepeatLayout="Flow" AutoPostBack="true">
                <asp:ListItem Selected="True" Value="0" Text="Illimitati"></asp:ListItem>
                <asp:ListItem Value="1" Text="Numero chiuso"></asp:ListItem>
            </asp:RadioButtonList>
            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
            <asp:Label ID="LBmaxIscritti" runat="server" CssClass="Titolo_campoSmall">&nbsp;Max Iscritti:</asp:Label>
            &nbsp;&nbsp;<asp:TextBox ID="TXBmaxIscritti" runat="server" Columns="3" MaxLength="3"
                CssClass="Testo_campoSmall"></asp:TextBox>
            <asp:RangeValidator ID="Rangevalidator1" runat="server" CssClass="Validatori" Display="static"
                ControlToValidate="TXBmaxIscritti" Type="Integer" MinimumValue="1" MaximumValue="999">*</asp:RangeValidator>
            &nbsp;&nbsp;
            <asp:Label ID="LBmaxIscrittiOver_t" runat="server" CssClass="Titolo_campoSmall">&nbsp;Max Iscritti:</asp:Label>
            &nbsp;&nbsp;
            <asp:TextBox ID="TXBmaxIscrittiOver" runat="server" Columns="3" MaxLength="3" CssClass="Testo_campoSmall"></asp:TextBox>
            <asp:RangeValidator ID="Rangevalidator2" runat="server" CssClass="Validatori" Display="static"
                ControlToValidate="TXBmaxIscrittiOver" Type="Integer" MinimumValue="0" MaximumValue="999">*</asp:RangeValidator>
        </asp:TableCell>
    </asp:TableRow>
    <asp:TableRow ID="TBRiscrizioni">
        <asp:TableCell CssClass="top" Width="150px">
            <asp:Label ID="LBiscrizioni_t" runat="server" CssClass="Titolo_campoSmall">&nbsp;Iscrizioni:</asp:Label>
        </asp:TableCell>
        <asp:TableCell CssClass="top" ColumnSpan="3">
            <asp:CheckBox ID="CBXcanSubscribe" runat="server" CssClass="Testo_campoSmall"></asp:CheckBox>
            <asp:CheckBox ID="CBXcanUnSubscribe" runat="server" CssClass="Testo_campoSmall">
            </asp:CheckBox>
        </asp:TableCell>
    </asp:TableRow>
    <asp:TableRow ID="TBRiscrivimi">
        <asp:TableCell CssClass="top" Width="150px">
            <asp:Label ID="LBiscrivimi_t" runat="server" CssClass="Titolo_campoSmall">&nbsp;Iscrivimi:</asp:Label>
        </asp:TableCell>
        <asp:TableCell CssClass="top" ColumnSpan="3">
            <asp:CheckBox ID="CBXiscrivimi" runat="server" CssClass="Testo_CampoSmall" Checked="True">
            </asp:CheckBox>
        </asp:TableCell>
    </asp:TableRow>
    <asp:TableRow ID="TBRaccessiSpeciali">
        <asp:TableCell CssClass="top" Width="150px">
            <asp:Label ID="LBaccessiSpeciali_t" runat="server" CssClass="Titolo_campoSmall">&nbsp;Accessi speciali:</asp:Label>
        </asp:TableCell>
        <asp:TableCell CssClass="top" ColumnSpan="3">
            <asp:CheckBox ID="CBXaccessoCopisteria" runat="server" CssClass="Testo_CampoSmall"
                Checked="True"></asp:CheckBox>
            &nbsp;&nbsp;&nbsp;
            <asp:CheckBox ID="CBXaccessoLibero" runat="server" CssClass="Testo_CampoSmall" Text="Accesso libero"
                Checked="false"></asp:CheckBox>
        </asp:TableCell>
    </asp:TableRow>
    <asp:TableRow>
        <asp:TableCell CssClass="top" Width="150px">
            <asp:Label ID="LBstatuto_t" runat="server" CssClass="Titolo_campoSmall">&nbsp;Statuto:</asp:Label>
        </asp:TableCell>
        <asp:TableCell CssClass="top" ColumnSpan="3">
            <asp:TextBox ID="TXBCmntStatuto" runat="server" CssClass="Testo_campoSmall" Columns="80"
                Rows="6" TextMode="MultiLine" Height="120px"></asp:TextBox>
            <asp:Label ID="LBmaxChar" runat="server" CssClass="Titolo_campoSmall">Max 4000 char</asp:Label>
        </asp:TableCell>
    </asp:TableRow>
    <asp:TableRow>
        <asp:TableCell HorizontalAlign="Center" ColumnSpan="4">
            &nbsp;
            <asp:Label ID="LBAvviso" CssClass="avviso" runat="server" Visible="False"></asp:Label>
            <asp:ValidationSummary ID="VLDSum" runat="server" EnableClientScript="true" HeaderText="Attenzione! Sono state rilevate delle imprecisioni nella compilazione del form. Controlla i valori inseriti in corrisponsenza degli *"
                ShowSummary="false" ShowMessageBox="true" DisplayMode="BulletList"></asp:ValidationSummary>
        </asp:TableCell>
    </asp:TableRow>
</asp:Table>