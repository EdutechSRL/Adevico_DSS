<%@ Page Language="vb" AutoEventWireup="false" MasterPageFile="~/Admin_Globale/AdminPortal.Master"
    CodeBehind="ADM_Organizzazione.aspx.vb" Inherits="Comunita_OnLine.ADM_Organizzazione" %>

<%@ MasterType VirtualPath="~/Admin_Globale/AdminPortal.Master" %>
<%@ Register TagPrefix="radt" Namespace="Telerik.WebControls" Assembly="RadTreeView.Net2" %>
<asp:Content ContentPlaceHolderID="HeadContent" runat="server" ID="Content1">
    <style type="text/css">
        td
        {
            font-size: 11px;
        }
    </style>
    <script type="text/javascript" language="javascript">	
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
    <table cellspacing="0" cellpadding="0" width="900px" border="0">
        <tr>
            <td align="right">
                <asp:Panel ID="PNLmenu" runat="server" HorizontalAlign="Right">
                    <asp:LinkButton ID="LNBnuovo" runat="server" CausesValidation="False" CssClass="LINK_MENU">Inserisci una nuova organizzazione</asp:LinkButton>
                </asp:Panel>
                <asp:Panel ID="PNLmenuInserimento" runat="server" HorizontalAlign="Right" Visible="False">
                    <asp:LinkButton ID="LNBannulla" runat="server" CausesValidation="False" CssClass="LINK_MENU">Annulla</asp:LinkButton>
                    &nbsp;
                    <asp:LinkButton ID="LNBassociaAutenticazione" runat="server" CausesValidation="False"
                        CssClass="LINK_MENU">Associa autenticazione</asp:LinkButton>
                    <asp:LinkButton ID="LNBinserisci" runat="server" CssClass="LINK_MENU">Salva</asp:LinkButton>
                    <asp:LinkButton ID="LNBmodifica" runat="server" CssClass="LINK_MENU">Salva</asp:LinkButton>
                </asp:Panel>
                <asp:Panel ID="PNLautenticazioni" runat="server" HorizontalAlign="Right" Visible="False">
                    <asp:LinkButton ID="LNBindietro" runat="server" CssClass="LINK_MENU" CausesValidation="False">Indietro</asp:LinkButton>
                    <asp:LinkButton ID="LNBautenticazioneIns" runat="server" CssClass="LINK_MENU">Associa</asp:LinkButton>
                </asp:Panel>
            </td>
        </tr>
        <tr>
            <td align="center" valign="top">
                <br />
                <asp:Panel ID="PNLpermessi" runat="server" Visible="False" Width="900px">
                    <table align="center">
                        <tr>
                            <td height="50">
                                &nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td align="center">
                                <asp:Label ID="LBNopermessi" runat="server" CssClass="messaggio"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td height="50">
                                &nbsp;
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
                <asp:Panel ID="PNLcontenuto" runat="server" HorizontalAlign="Center" Width="900px">
                    <table align="center">
                        <tr>
                            <td>
                                <table cellspacing="0" cellpadding="0" width="800" align="center" border="0">
                                    <tr>
                                        <td colspan="3">
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="3">
                                            <asp:Panel ID="PNLdgOrganizzazione" runat="server" Visible="true">
                                                <table cellspacing="0" cellpadding="0" align="center">
                                                    <tr>
                                                        <td class="FiltroVoceSmall" align="left">
                                                            <asp:Label ID="LBfiltroIstituzione_t" CssClass="FiltroVoceSmall" runat="server">Istituzione:</asp:Label>
                                                            <asp:DropDownList ID="DDLIstituzioni" runat="server" CssClass="FiltroCampoSmall"
                                                                AutoPostBack="true">
                                                            </asp:DropDownList>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <asp:DataGrid ID="DGOrganizzazione" runat="server" ShowFooter="false" AutoGenerateColumns="False"
                                                                AllowPaging="true" DataKeyField="ORGN_id" PageSize="15" CssClass="DataGrid_Generica">
                                                                <AlternatingItemStyle CssClass="ROW_Alternate_Small"></AlternatingItemStyle>
                                                                <HeaderStyle CssClass="ROW_header_Small_Center"></HeaderStyle>
                                                                <ItemStyle CssClass="ROW_Normal_Small" Height="22px"></ItemStyle>
                                                                <PagerStyle CssClass="ROW_Page_Small" Position="Bottom" Mode="NumericPages" Visible="true"
                                                                    HorizontalAlign="Right" Height="18px" VerticalAlign="Bottom"></PagerStyle>
                                                                <Columns>
                                                                    <asp:TemplateColumn runat="server" HeaderText="" ItemStyle-HorizontalAlign="Center"
                                                                        ItemStyle-Width="40">
                                                                        <ItemTemplate>
                                                                            <asp:ImageButton ID="IMBmodifica" runat="server" CausesValidation="False" CommandName="modifica"
                                                                                ImageUrl="../images/m.gif"></asp:ImageButton>
                                                                            <asp:ImageButton ID="IMBCancella" runat="server" CausesValidation="False" CommandName="cancella"
                                                                                ImageUrl="../images/x.gif"></asp:ImageButton>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateColumn>
                                                                    <asp:BoundColumn DataField="ORGN_ragioneSociale" HeaderText="Ragione Sociale" SortExpression="ORGN_ragioneSociale"
                                                                        HeaderStyle-CssClass="ROW_Header_Small_center" ItemStyle-CssClass="ROW_TD_Small">
                                                                    </asp:BoundColumn>
                                                                    <asp:BoundColumn DataField="ORGN_indirizzo" HeaderText="Indirizzo" HeaderStyle-CssClass="ROW_Header_Small_center"
                                                                        ItemStyle-CssClass="ROW_TD_Small"></asp:BoundColumn>
                                                                    <asp:BoundColumn DataField="ORGN_citta" HeaderText="Città" HeaderStyle-CssClass="ROW_Header_Small_center"
                                                                        ItemStyle-CssClass="ROW_TD_Small"></asp:BoundColumn>
                                                                    <asp:TemplateColumn runat="server" HeaderText="HomePage" ItemStyle-Width="40" HeaderStyle-CssClass="ROW_Header_Small_center"
                                                                        ItemStyle-CssClass="ROW_TD_Small">
                                                                        <ItemTemplate>
                                                                            <a href='http://<%# DataBinder.Eval(Container.DataItem, "ORGN_homepage") %>' target="_blank">
                                                                                <%# DataBinder.Eval(Container.DataItem, "ORGN_homepage") %>
                                                                            </a>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateColumn>
                                                                    <asp:TemplateColumn HeaderStyle-CssClass="ROW_Header_Small_center" ItemStyle-CssClass="ROW_TD_Small_Center">
                                                                        <ItemTemplate>
                                                                            <%# DataBinder.Eval(Container.DataItem, "isFacolta") %>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateColumn>
                                                                </Columns>
                                                                <PagerStyle Width="800px" Mode="NumericPages"></PagerStyle>
                                                            </asp:DataGrid>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </asp:Panel>
                                            <asp:Panel ID="PNLDatiOrganizzazione" runat="server" Visible="false">
                                                <asp:Table ID="TBLdati" runat="server" CellSpacing="0" CellPadding="0" HorizontalAlign="Center">
                                                    <asp:TableRow ID="TBR_0">
                                                        <asp:TableCell Width="130" Height="22px">
                                                            <asp:Label ID="LBistituzione_t" runat="server" CssClass="Titolo_campoSmall">*Istituzione:</asp:Label>
                                                        </asp:TableCell>
                                                        <asp:TableCell ColumnSpan="3">
                                                            <asp:DropDownList ID="DDLIstituzioneForm" runat="server" CssClass="Testo_campoSmall"
                                                                Width="258px">
                                                            </asp:DropDownList>
                                                        </asp:TableCell>
                                                    </asp:TableRow>
                                                    <asp:TableRow ID="TBR_1">
                                                        <asp:TableCell Width="130" Height="22px">
                                                            <asp:Label ID="LBragioneSociale_t" runat="server" CssClass="Titolo_campoSmall">*Ragione Sociale:</asp:Label>
                                                        </asp:TableCell>
                                                        <asp:TableCell ColumnSpan="3">
                                                            <asp:TextBox ID="TXBragioneSociale" runat="server" CssClass="Testo_campo_obbligatorioSmall"
                                                                MaxLength="100" Columns="60"></asp:TextBox>
                                                            <input id="TXBid_n" type="hidden" name="TXBid_n" runat="server" />
                                                            <asp:RequiredFieldValidator ID="Requiredfieldvalidator1" runat="server" CssClass="Validatori"
                                                                Display="Static" ControlToValidate="TXBragioneSociale">*</asp:RequiredFieldValidator>
                                                            &nbsp;&nbsp;&nbsp;
                                                            <asp:Label ID="LBisFacoltà" runat="server" CssClass="Titolo_campoSmall">Facoltà:</asp:Label>&nbsp;
                                                            <asp:CheckBox ID="CBXisFacolta" runat="server" CssClass="Testo_campoSmall"></asp:CheckBox>
                                                        </asp:TableCell>
                                                    </asp:TableRow>
                                                    <asp:TableRow ID="TBR_2">
                                                        <asp:TableCell Width="130" Height="22px">
                                                            <asp:Label ID="LBindirizzo_t" runat="server" CssClass="Titolo_campoSmall">*Indirizzo:</asp:Label>
                                                        </asp:TableCell>
                                                        <asp:TableCell ColumnSpan="3">
                                                            <asp:TextBox ID="TXBindirizzo" runat="server" CssClass="Testo_campo_obbligatorioSmall"
                                                                MaxLength="100" Columns="60"></asp:TextBox>
                                                            <asp:RequiredFieldValidator ID="Requiredfieldvalidator2" runat="server" CssClass="Validatori"
                                                                Display="static" ControlToValidate="TXBindirizzo">*</asp:RequiredFieldValidator></asp:TableCell>
                                                    </asp:TableRow>
                                                    <asp:TableRow ID="TBR_3">
                                                        <asp:TableCell Width="130" Height="22px">
                                                            <asp:Label ID="LBcitta_t" runat="server" CssClass="Titolo_campoSmall">*Città</asp:Label>
                                                        </asp:TableCell>
                                                        <asp:TableCell>
                                                            <asp:TextBox ID="TXBcitta" runat="server" CssClass="Testo_campo_obbligatorioSmall"
                                                                MaxLength="50" Columns="40"></asp:TextBox>
                                                            <asp:RequiredFieldValidator ID="Requiredfieldvalidator4" runat="server" CssClass="Validatori"
                                                                Display="static" ControlToValidate="TxBcitta">*</asp:RequiredFieldValidator>
                                                        </asp:TableCell>
                                                        <asp:TableCell Width="60" Height="22px">
                                                            <asp:Label ID="LBcap_t" runat="server" CssClass="Titolo_campoSmall">*CAP:</asp:Label>
                                                        </asp:TableCell>
                                                        <asp:TableCell>
                                                            <asp:TextBox ID="TXBcap" runat="server" CssClass="Testo_campo_obbligatorioSmall"
                                                                MaxLength="5" Columns="7"></asp:TextBox>
                                                            <asp:RequiredFieldValidator ID="Requiredfieldvalidator7" runat="server" CssClass="Validatori"
                                                                Display="dynamic" ControlToValidate="TXBcap">*</asp:RequiredFieldValidator>
                                                            <asp:RegularExpressionValidator ID="Cap" runat="server" CssClass="Validatori" Display="dynamic"
                                                                ControlToValidate="TxBcap" ValidationExpression="^\d{5}$">*</asp:RegularExpressionValidator></asp:TableCell>
                                                    </asp:TableRow>
                                                    <asp:TableRow ID="TBR_4">
                                                        <asp:TableCell Width="130" Height="22px">
                                                            <asp:Label ID="LBprovincia" runat="server" CssClass="Titolo_campoSmall">*Provincia:</asp:Label>
                                                        </asp:TableCell>
                                                        <asp:TableCell>
                                                            <asp:DropDownList ID="DDLprovincia" runat="server" CssClass="Testo_campoSmall" Width="258px">
                                                            </asp:DropDownList>
                                                        </asp:TableCell>
                                                        <asp:TableCell Width="60px">
                                                            <asp:Label ID="LBstato_t" runat="server" CssClass="Titolo_campoSmall">*Stato:</asp:Label>
                                                        </asp:TableCell>
                                                        <asp:TableCell>
                                                            <asp:DropDownList ID="DDLstato" runat="server" CssClass="Testo_campoSmall" AutoPostBack="True">
                                                            </asp:DropDownList>
                                                        </asp:TableCell>
                                                    </asp:TableRow>
                                                    <asp:TableRow ID="TBR_5">
                                                        <asp:TableCell Width="130" Height="22px">
                                                            <asp:Label ID="LBtelefono1_t" runat="server" CssClass="Titolo_campoSmall">&nbsp;Tel. 1:</asp:Label>
                                                        </asp:TableCell>
                                                        <asp:TableCell>
                                                            <asp:TextBox ID="TXBtelefono1" runat="server" CssClass="Testo_campoSmall" MaxLength="25"
                                                                Columns="30"></asp:TextBox>
                                                        </asp:TableCell>
                                                        <asp:TableCell Width="60">
                                                            <asp:Label ID="LBtelefono2_t" runat="server" CssClass="Titolo_campoSmall">&nbsp;Tel. 2:</asp:Label>
                                                        </asp:TableCell>
                                                        <asp:TableCell>
                                                            <asp:TextBox ID="TXBtelefono2" runat="server" CssClass="Testo_campoSmall" MaxLength="25"
                                                                Columns="30"></asp:TextBox>
                                                        </asp:TableCell>
                                                    </asp:TableRow>
                                                    <asp:TableRow ID="TBR_6">
                                                        <asp:TableCell Width="130" Height="22px">
                                                            <asp:Label ID="LBfax_t" runat="server" CssClass="Titolo_campoSmall">&nbsp;Fax:</asp:Label>
                                                        </asp:TableCell>
                                                        <asp:TableCell ColumnSpan="3">
                                                            <asp:TextBox ID="TXBfax" runat="server" CssClass="Testo_campoSmall" MaxLength="25"
                                                                Columns="30"></asp:TextBox>
                                                        </asp:TableCell>
                                                    </asp:TableRow>
                                                    <asp:TableRow ID="TBR_7">
                                                        <asp:TableCell Width="130" Height="22px">
                                                            <asp:Label ID="LBhomePage_t" runat="server" CssClass="Titolo_campoSmall">&nbsp;Home Page:</asp:Label>
                                                        </asp:TableCell>
                                                        <asp:TableCell ColumnSpan="3">
                                                            <asp:TextBox ID="TXBhomePage" runat="server" CssClass="Testo_campoSmall" MaxLength="250"
                                                                Columns="60"></asp:TextBox></asp:TableCell>
                                                    </asp:TableRow>
                                                    <asp:TableRow ID="TBR_8">
                                                        <asp:TableCell Width="130" Height="22px">
                                                            <asp:Label ID="LBisChiusa" runat="server" CssClass="Titolo_campoSmall">&nbsp;Chiusa:</asp:Label>
                                                        </asp:TableCell>
                                                        <asp:TableCell ColumnSpan="3">
                                                            <table border="0" cellpadding="0" cellspacing="0">
                                                                <tr>
                                                                    <td>
                                                                        <asp:RadioButtonList ID="RBLisChiusa" runat="server" RepeatDirection="Horizontal"
                                                                            CssClass="Testo_campoSmall">
                                                                            <asp:ListItem Value="0" Selected="true">No</asp:ListItem>
                                                                            <asp:ListItem Value="1">Si</asp:ListItem>
                                                                        </asp:RadioButtonList>
                                                                    </td>
                                                                    <td width="30px">
                                                                        &nbsp;
                                                                    </td>
                                                                    <td nowrap="nowrap">
                                                                        <asp:Label ID="LBiscrivimi" runat="server" CssClass="Titolo_campoSmall">Iscrivimi:</asp:Label>&nbsp;
                                                                    </td>
                                                                    <td>
                                                                        <asp:CheckBox ID="CBXiscrivimi" Text="Iscrivimi" runat="server" CssClass="Testo_campoSmall">
                                                                        </asp:CheckBox>&nbsp;
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </asp:TableCell>
                                                    </asp:TableRow>
                                                    <asp:TableRow ID="TBR_12">
                                                        <asp:TableCell Width="130" Height="22px" CssClass="top">
                                                            <asp:Label ID="LBlimiti_t" runat="server" CssClass="Titolo_campoSmall">&nbsp;Limita tesi:</asp:Label>
                                                        </asp:TableCell>
                                                        <asp:TableCell ColumnSpan="3">
                                                            <asp:CheckBoxList ID="CBLlimiti" runat="server" CssClass="Testo_campoSmall" RepeatLayout="Table"
                                                                RepeatColumns="5" RepeatDirection="Horizontal">
                                                            </asp:CheckBoxList>
                                                        </asp:TableCell>
                                                    </asp:TableRow>
                                                    <asp:TableRow ID="TBRiscrizioniAutonome">
                                                        <asp:TableCell Width="130" Height="22px" CssClass="top">
                                                            <asp:Label ID="LBiscrizioniAutonome" runat="server" CssClass="Titolo_campoSmall">&nbsp;Iscrizioni autonome:</asp:Label>
                                                        </asp:TableCell>
                                                        <asp:TableCell ColumnSpan="3">
                                                            <asp:CheckBoxList ID="CBLiscrizioniAutonome" runat="server" CssClass="Testo_campoSmall"
                                                                RepeatLayout="Table" RepeatColumns="5" RepeatDirection="Horizontal">
                                                            </asp:CheckBoxList>
                                                        </asp:TableCell>
                                                    </asp:TableRow>
                                                    <asp:TableRow ID="TBR_9">
                                                        <asp:TableCell Width="130" Height="22px" CssClass="top">
														    &nbsp;
                                                        </asp:TableCell>
                                                        <asp:TableCell HorizontalAlign="center" ColumnSpan="4">
                                                            <asp:DataGrid ID="DGautenticazione" runat="server" Font-Size="8" 
                                                                ShowFooter="false" BackColor="transparent" BorderColor="#8080FF" AutoGenerateColumns="False"
                                                                AllowPaging="false" DataKeyField="LKAO_id" CellPadding="4" Width="600px">
                                                                <AlternatingItemStyle CssClass="Righe_Alternate_Center"></AlternatingItemStyle>
                                                                <AlternatingItemStyle CssClass="ROW_Alternate_Small"></AlternatingItemStyle>
                                                                <HeaderStyle CssClass="ROW_header_Small_Center"></HeaderStyle>
                                                                <ItemStyle CssClass="ROW_Normal_Small" Height="22px"></ItemStyle>
                                                                <PagerStyle CssClass="ROW_Page_Small" Position="Bottom" Mode="NumericPages" Visible="true"
                                                                    HorizontalAlign="Right" Height="18px" VerticalAlign="Bottom"></PagerStyle>
                                                                <Columns>
                                                                    <asp:TemplateColumn runat="server" HeaderText="" HeaderStyle-CssClass="ROW_Header_Small_center"
                                                                        ItemStyle-CssClass="ROW_TD_Small" ItemStyle-Width="40">
                                                                        <ItemTemplate>
                                                                            <asp:ImageButton ID="IMBModifica2" runat="server" CausesValidation="False" CommandName="modifica"
                                                                                ImageUrl="../images/m.gif"></asp:ImageButton>
                                                                            <asp:ImageButton ID="IMBCancella2" runat="server" CausesValidation="False" CommandName="cancella"
                                                                                ImageUrl="../images/x.gif"></asp:ImageButton>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateColumn>
                                                                    <asp:BoundColumn DataField="AUTN_nome" HeaderText="Autenticazione" HeaderStyle-CssClass="ROW_Header_Small_center"
                                                                        ItemStyle-CssClass="ROW_TD_Small"></asp:BoundColumn>
                                                                    <asp:BoundColumn DataField="LKAO_descrizione" HeaderText="Descrizione" HeaderStyle-CssClass="ROW_Header_Small_center"
                                                                        ItemStyle-CssClass="ROW_TD_Small"></asp:BoundColumn>
                                                                    <asp:BoundColumn DataField="LKAO_parametro_1" HeaderText="Sorgente" HeaderStyle-CssClass="ROW_Header_Small_center"
                                                                        ItemStyle-CssClass="ROW_TD_Small"></asp:BoundColumn>
                                                                    <asp:BoundColumn DataField="LKAO_parametro_2" HeaderText="Server" Visible="False"
                                                                        HeaderStyle-CssClass="ROW_Header_Small_center" ItemStyle-CssClass="ROW_TD_Small">
                                                                    </asp:BoundColumn>
                                                                    <asp:BoundColumn DataField="LKAO_parametro_3" HeaderText="Porta" Visible="False"
                                                                        HeaderStyle-CssClass="ROW_Header_Small_center" ItemStyle-CssClass="ROW_TD_Small">
                                                                    </asp:BoundColumn>
                                                                    <asp:BoundColumn DataField="LKAO_parametro_4" HeaderText="Altro" Visible="False"
                                                                        HeaderStyle-CssClass="ROW_Header_Small_center" ItemStyle-CssClass="ROW_TD_Small">
                                                                    </asp:BoundColumn>
                                                                    <asp:BoundColumn DataField="LKAO_id" HeaderText="LKAO_id" Visible="false"></asp:BoundColumn>
                                                                    <asp:BoundColumn DataField="LKAO_AUTN_id" HeaderText="LKAO_AUTN_id" Visible="false">
                                                                    </asp:BoundColumn>
                                                                </Columns>
                                                            </asp:DataGrid>
                                                            <asp:Label ID="LBnoAutenticaz" runat="server" CssClass="AVVISO"></asp:Label>
                                                        </asp:TableCell>
                                                    </asp:TableRow>
                                                    <asp:TableRow ID="TBR_11" Visible="False">
                                                        <asp:TableCell ColumnSpan="4" HorizontalAlign="Center">
                                                            <table class="TableUcFile" border="1" cellspacing="0" cellpadding="0">
                                                                <tr>
                                                                    <td align="center">
                                                                        <asp:Table ID="Table1" HorizontalAlign="Center" runat="server">
                                                                            <asp:TableRow>
                                                                                <asp:TableCell CssClass="nosize0" ColumnSpan="4" Height="15px">&nbsp;</asp:TableCell>
                                                                            </asp:TableRow>
                                                                            <asp:TableRow>
                                                                                <asp:TableCell Width="5px" CssClass="nosize0">&nbsp;</asp:TableCell>
                                                                                <asp:TableCell>
                                                                                    <asp:Label ID="LBtipoAutenticazione" runat="server" CssClass="Titolo_campoSmall">Tipo Autenticazione:</asp:Label>
                                                                                </asp:TableCell>
                                                                                <asp:TableCell>
                                                                                    <asp:DropDownList ID="DDLtipoAutenticazione" runat="server" CssClass="Testo_campoSmall">
                                                                                    </asp:DropDownList>
                                                                                    <input type="hidden" id="HDNlkao_ID" runat="server" />
                                                                                </asp:TableCell>
                                                                                <asp:TableCell Width="5px" CssClass="nosize0">&nbsp;</asp:TableCell>
                                                                            </asp:TableRow>
                                                                            <asp:TableRow>
                                                                                <asp:TableCell Width="5px" CssClass="nosize0">&nbsp;</asp:TableCell>
                                                                                <asp:TableCell>
                                                                                    <asp:Label ID="LBdescrizione" runat="server" CssClass="Titolo_campoSmall">Descrizione:</asp:Label>
                                                                                </asp:TableCell>
                                                                                <asp:TableCell>
                                                                                    <asp:TextBox ID="TXBdescrizione" runat="server" CssClass="Testo_campo_obbligatorioSmall"
                                                                                        Rows="3" Columns="60" MaxLength="250" TextMode="MultiLine"></asp:TextBox>
                                                                                    <asp:RequiredFieldValidator ID="RFVdescrizione" ControlToValidate="TXBdescrizione"
                                                                                        Text="*" Display="Dynamic" runat="server"></asp:RequiredFieldValidator>
                                                                                </asp:TableCell>
                                                                                <asp:TableCell Width="5px" CssClass="nosize0">&nbsp;</asp:TableCell>
                                                                            </asp:TableRow>
                                                                            <asp:TableRow>
                                                                                <asp:TableCell Width="5px" CssClass="nosize0">&nbsp;</asp:TableCell>
                                                                                <asp:TableCell>
                                                                                    <asp:Label ID="LBparametro_1" runat="server" CssClass="Titolo_campoSmall">Filtro DB:</asp:Label>
                                                                                </asp:TableCell>
                                                                                <asp:TableCell>
                                                                                    <asp:TextBox ID="TXBparametro_1" runat="server" CssClass="Testo_campoSmall" Columns="60"
                                                                                        MaxLength="150"></asp:TextBox>
                                                                                </asp:TableCell>
                                                                                <asp:TableCell Width="5px" CssClass="nosize0">&nbsp;</asp:TableCell>
                                                                            </asp:TableRow>
                                                                            <asp:TableRow>
                                                                                <asp:TableCell Width="5px" CssClass="nosize0">&nbsp;</asp:TableCell>
                                                                                <asp:TableCell>
                                                                                    <asp:Label ID="LBparametro_2" runat="server" CssClass="Titolo_campoSmall">Server:</asp:Label>
                                                                                </asp:TableCell>
                                                                                <asp:TableCell>
                                                                                    <asp:TextBox ID="TXBparametro_2" runat="server" CssClass="Testo_campoSmall" Columns="60"
                                                                                        MaxLength="150"></asp:TextBox>
                                                                                </asp:TableCell>
                                                                                <asp:TableCell Width="5px" CssClass="nosize0">&nbsp;</asp:TableCell>
                                                                            </asp:TableRow>
                                                                            <asp:TableRow>
                                                                                <asp:TableCell Width="5px" CssClass="nosize0">&nbsp;</asp:TableCell>
                                                                                <asp:TableCell>
                                                                                    <asp:Label ID="LBparametro_3" runat="server" CssClass="Titolo_campoSmall">Porta:</asp:Label>
                                                                                </asp:TableCell>
                                                                                <asp:TableCell>
                                                                                    <asp:TextBox ID="TXBparametro_3" runat="server" CssClass="Testo_campoSmall" Columns="60"
                                                                                        MaxLength="150"></asp:TextBox>
                                                                                </asp:TableCell>
                                                                                <asp:TableCell Width="5px" CssClass="nosize0">&nbsp;</asp:TableCell>
                                                                            </asp:TableRow>
                                                                            <asp:TableRow>
                                                                                <asp:TableCell Width="5px" CssClass="nosize0">&nbsp;</asp:TableCell>
                                                                                <asp:TableCell>
                                                                                    <asp:Label ID="LBparametro_4" runat="server" CssClass="Titolo_campoSmall">Altro:</asp:Label>
                                                                                </asp:TableCell>
                                                                                <asp:TableCell>
                                                                                    <asp:TextBox ID="TXBparametro_4" runat="server" CssClass="Testo_campoSmall" Columns="60"
                                                                                        MaxLength="150"></asp:TextBox>
                                                                                </asp:TableCell>
                                                                                <asp:TableCell Width="5px" CssClass="nosize0">&nbsp;</asp:TableCell>
                                                                            </asp:TableRow>
                                                                            <asp:TableRow>
                                                                                <asp:TableCell CssClass="nosize0" ColumnSpan="4" Height="15px">&nbsp;</asp:TableCell>
                                                                            </asp:TableRow>
                                                                        </asp:Table>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </asp:TableCell>
                                                    </asp:TableRow>
                                                    <asp:TableRow>
                                                        <asp:TableCell ColumnSpan="4">&nbsp;</asp:TableCell>
                                                    </asp:TableRow>
                                                </asp:Table>
                                            </asp:Panel>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
            </td>
        </tr>
    </table>
</asp:Content>