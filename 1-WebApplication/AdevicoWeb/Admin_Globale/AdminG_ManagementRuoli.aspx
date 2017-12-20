<%@ Page Language="vb" AutoEventWireup="false" MasterPageFile="~/Admin_Globale/AdminPortal.Master"
    CodeBehind="AdminG_ManagementRuoli.aspx.vb" Inherits="Comunita_OnLine.AdminG_ManagementRuoli" %>

<%@ MasterType VirtualPath="~/Admin_Globale/AdminPortal.Master" %>

<asp:Content ContentPlaceHolderID="HeadContent" runat="server" ID="Content1">
    <script type="text/javascript" language="javascript" src="./../jscript/generali.js"></script>
    <style type="text/css">
        td
        {
            font-size: 11px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="CPHservice" runat="server">
    <table cellspacing="0" cellpadding="0" width="900px" border="0">
        <%--		<tr>
			<td align="left" class="RigaTitoloAdmin">
				<asp:Label ID="LBTitolo" Runat="server">Management Ruoli</asp:Label>
			</td>
		</tr>--%>
        <tr>
            <td align="right">
                <asp:Panel ID="PNLmenu" runat="server" HorizontalAlign="Right">
                    &nbsp;<asp:LinkButton ID="LNBinserisci" runat="server" Text="Crea nuovo" CssClass="Link_Menu"></asp:LinkButton>
                </asp:Panel>
                <asp:Panel ID="PNLmenuAzione" runat="server" HorizontalAlign="Right" Visible="False">
                    &nbsp;<asp:LinkButton ID="LNBindietro" runat="server" Text="Torna all'elenco" CssClass="Link_Menu"
                        CausesValidation="False"></asp:LinkButton>
                    <asp:HyperLink ID="HYPtoSettings" runat="server" Text="Gestione permessi" NavigateUrl="#"
                        CssClass="Link_Menu"></asp:HyperLink>
                    <asp:LinkButton ID="LNBsalvaDati" runat="server" Text="Salva" CssClass="Link_Menu"></asp:LinkButton>
                </asp:Panel>
            </td>
        </tr>
        <tr>
            <td align="center" valign="top">
                <asp:Panel ID="PNLpermessi" runat="server" Visible="False">
                    <br />
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
                <asp:Panel ID="PNLcontenuto" runat="server" HorizontalAlign="Center">
                    <br />
                    <table align="center" width="500px">
                        <tr>
                            <td align="center">
                                <asp:Panel ID="PNLlista" runat="server" HorizontalAlign="Center">
                                    <asp:DataGrid ID="DGtipoRuolo" runat="server" ShowFooter="false" AutoGenerateColumns="False"
                                        DataKeyField="TPRL_ID" CssClass="DataGrid_Generica" AllowSorting="true" PageSize="20"
                                        AllowPaging="True">
                                        <AlternatingItemStyle CssClass="ROW_Alternate_Small"></AlternatingItemStyle>
                                        <HeaderStyle CssClass="ROW_header_Small_Center"></HeaderStyle>
                                        <ItemStyle CssClass="ROW_Normal_Small" Height="22px"></ItemStyle>
                                        <PagerStyle CssClass="ROW_Page_Small" Position="TopAndBottom" Mode="NumericPages"
                                            Visible="true" HorizontalAlign="Right" Height="18px" VerticalAlign="Bottom">
                                        </PagerStyle>
                                        <Columns>
                                            <asp:TemplateColumn runat="server" HeaderText="" ItemStyle-Width="50" HeaderStyle-CssClass="ROW_Header_Small_center"
                                                ItemStyle-CssClass="ROW_TD_Small_center">
                                                <ItemTemplate>
                                                    <asp:ImageButton ID="IMBmodifica" runat="server" CausesValidation="False" CommandName="modifica"
                                                        ImageUrl="../images/m.gif"></asp:ImageButton>
                                                    <asp:ImageButton ID="IMBCancella" runat="server" CausesValidation="False" CommandName="elimina"
                                                        ImageUrl="../images/x.gif"></asp:ImageButton>
                                                </ItemTemplate>
                                            </asp:TemplateColumn>
                                            <asp:TemplateColumn HeaderStyle-CssClass="ROW_Header_Small_center" ItemStyle-CssClass="ROW_TD_Small"
                                                HeaderText="Ruolo" SortExpression="TPRL_nome">
                                                <ItemTemplate>
                                                    <asp:Table ID="TBLrecord" runat="server" CellPadding="0" CellSpacing="0" Width="700px"
                                                        GridLines="none" HorizontalAlign="Left">
                                                        <asp:TableRow>
                                                            <asp:TableCell Width="5px">&nbsp;</asp:TableCell>
                                                            <asp:TableCell>
                                                                <asp:Label ID="LBruolo" runat="server" CssClass="ROW_TD_Small">
																	<%#Container.Dataitem("TPRL_nome")%>
                                                                </asp:Label>
                                                            </asp:TableCell>
                                                            <asp:TableCell HorizontalAlign="Right">
                                                                <asp:HyperLink ID="HYPsettings" runat="server">Definizione permessi/servizi</asp:HyperLink>
                                                            </asp:TableCell>
                                                            <asp:TableCell Width="5px">&nbsp;</asp:TableCell>
                                                        </asp:TableRow>
                                                        <asp:TableRow ID="TBRdescrizione">
                                                            <asp:TableCell Width="5px">&nbsp;</asp:TableCell>
                                                            <asp:TableCell ColumnSpan="2">
                                                                <table cellpadding="0" cellspacing="0" border="0" align="left">
                                                                    <tr>
                                                                        <td>
                                                                            <b>
                                                                                <asp:Label ID="LBdescrz_t" runat="server" CssClass="ROW_TD_Small"></asp:Label></b>&nbsp;
                                                                        </td>
                                                                        <td>
                                                                            <asp:Label ID="LBdescrizione" runat="server" CssClass="ROW_TD_Small">
																				<%#Container.Dataitem("TPRL_Descrizione")%>
                                                                            </asp:Label>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </asp:TableCell>
                                                            <asp:TableCell Width="5px">&nbsp;</asp:TableCell>
                                                        </asp:TableRow>
                                                    </asp:Table>
                                                </ItemTemplate>
                                            </asp:TemplateColumn>
                                            <asp:BoundColumn DataField="ComunitaAssociate" HeaderText="Comunita Associate" Visible="True"
                                                ItemStyle-Width="120px" HeaderStyle-CssClass="ROW_Header_Small_center" ItemStyle-CssClass="ROW_TD_Small_center">
                                            </asp:BoundColumn>
                                            <asp:BoundColumn DataField="TPRL_noDelete" Visible="false" ItemStyle-Width="80px">
                                            </asp:BoundColumn>
                                            <asp:BoundColumn DataField="associato" Visible="False"></asp:BoundColumn>
                                            <asp:BoundColumn DataField="TPRL_Descrizione" Visible="False"></asp:BoundColumn>
                                        </Columns>
                                    </asp:DataGrid>
                                </asp:Panel>
                                <asp:Panel ID="PNLnorecord" runat="server" Visible="false" HorizontalAlign="Center">
                                    <table width="600" align="center">
                                        <tr>
                                            <td height="20">
                                                &nbsp;
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="center">
                                                <asp:Label ID="LBnorecord" runat="server" CssClass="info_blackMedium"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td height="20">
                                                &nbsp;
                                            </td>
                                        </tr>
                                    </table>
                                </asp:Panel>
                                <asp:Panel ID="PNLdati" runat="server" Visible="false" HorizontalAlign="Center">
                                    <input type="hidden" id="HDNtprl_ID" runat="server" name="HDNtprl_ID" />
                                    <input type="hidden" id="HDNsetup" runat="server" name="HDNsetup" />
                                    <asp:Table ID="TBLdati" runat="server" CellSpacing="0" CellPadding="0" HorizontalAlign="Center"
                                        Width="600px">
                                        <asp:TableRow ID="TBR_1">
                                            <asp:TableCell Width="120px" Height="35px" Wrap="False">
                                                <asp:Label ID="LBnome_t" runat="server" CssClass="Titolo_campoSmall">*Nome Ruolo:</asp:Label>
                                            </asp:TableCell>
                                            <asp:TableCell>
                                                <asp:TextBox ID="TXBnome" runat="server" CssClass="Testo_campo_obbligatorioSmall"
                                                    MaxLength="50" Columns="60"></asp:TextBox>
                                                <asp:RequiredFieldValidator ID="Requiredfieldvalidator1" runat="server" CssClass="Validatori"
                                                    Display="Static" ControlToValidate="TXBnome">*</asp:RequiredFieldValidator>
                                            </asp:TableCell>
                                        </asp:TableRow>
                                        <asp:TableRow>
                                            <asp:TableCell Width="120px">&nbsp;</asp:TableCell>
                                            <asp:TableCell CssClass="top">
                                                <table border="1" align="left" bgcolor="#FFFBF7" style="border-color: #CCCCCC" cellpadding="0"
                                                    cellspacing="0">
                                                    <tr>
                                                        <td>
                                                            <table border="0" align="left" bgcolor="#FFFBF7" cellpadding="0" cellspacing="0">
                                                                <asp:Repeater ID="RPTnome" runat="server">
                                                                    <HeaderTemplate>
                                                                        <tr>
                                                                            <td colspan="2" height="20px">
                                                                                <asp:Label ID="LBlinguaNome_t" runat="server" CssClass="Titolo_campoSmall">&nbsp;Traduzioni(°):</asp:Label>
                                                                            </td>
                                                                        </tr>
                                                                    </HeaderTemplate>
                                                                    <ItemTemplate>
                                                                        <tr>
                                                                            <td align="right" width="120px" height="22px">
                                                                                <asp:Label ID="LBlinguaID" Text='<%# Databinder.eval(Container.DataItem, "Language.ID")%>'
                                                                                    runat="server" Visible="false" />
                                                                                <asp:Label ID="LBlingua_Nome" Text='<%# Databinder.eval(Container.DataItem, "Language.Nome")%>'
                                                                                    runat="server" Visible="true" CssClass="Repeater_VoceLingua" />&nbsp;
                                                                            </td>
                                                                            <td align="left" height="22px">
                                                                                <asp:TextBox ID="TXBtermine" runat="server" CssClass="Testo_campoSmall" MaxLength="50"
                                                                                    Columns="60" Text='<%# Databinder.eval(Container.DataItem, "RoleType.Name")%>'> </asp:TextBox>&nbsp;&nbsp;
                                                                            </td>
                                                                        </tr>
                                                                    </ItemTemplate>
                                                                    <FooterTemplate>
                                                                        <tr>
                                                                            <td colspan="2" class="nosize0">
                                                                                &nbsp;
                                                                            </td>
                                                                        </tr>
                                                                    </FooterTemplate>
                                                                </asp:Repeater>
                                                            </table>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </asp:TableCell>
                                        </asp:TableRow>
                                        <asp:TableRow>
                                            <asp:TableCell ColumnSpan="2">&nbsp;</asp:TableCell>
                                        </asp:TableRow>
                                        <asp:TableRow ID="TBRtipocomunita" Visible="False">
                                            <asp:TableCell Width="120px" CssClass="top">
                                                <asp:Label ID="LBtipoComunitaIns_t" runat="server" CssClass="Titolo_campoSmall">&nbsp;Tipo comunità:</asp:Label>
                                            </asp:TableCell>
                                            <asp:TableCell CssClass="top">
                                                <asp:CheckBoxList ID="CBLtipoComunita" runat="server" CssClass="Testo_campoSmall"
                                                    RepeatLayout="Table" RepeatDirection="Horizontal" RepeatColumns="3">
                                                </asp:CheckBoxList>
                                            </asp:TableCell>
                                        </asp:TableRow>
                                        <asp:TableRow ID="TBR_2">
                                            <asp:TableCell Width="120px" Height="35px">
                                                <asp:Label ID="LBdescrizione_t" runat="server" CssClass="Titolo_campoSmall">&nbsp;Descrizione:</asp:Label>
                                            </asp:TableCell>
                                            <asp:TableCell>
                                                <asp:TextBox ID="TXBdescrizione" runat="server" CssClass="Testo_campoSmall" MaxLength="50"
                                                    Columns="60"></asp:TextBox>
                                            </asp:TableCell>
                                        </asp:TableRow>
                                        <asp:TableRow>
                                            <asp:TableCell Width="120px">&nbsp;</asp:TableCell>
                                            <asp:TableCell CssClass="top">
                                                <table border="1" align="left" bgcolor="#FFFBF7" style="border-color: #CCCCCC" cellpadding="0"
                                                    cellspacing="0">
                                                    <tr>
                                                        <td>
                                                            <table border="0" align="left" bgcolor="#FFFBF7" cellpadding="0" cellspacing="0">
                                                                <asp:Repeater ID="RPTdescrizione" runat="server">
                                                                    <HeaderTemplate>
                                                                        <tr>
                                                                            <td colspan="2" height="20px">
                                                                                <asp:Label ID="LBlinguaDescrizione_t" runat="server" CssClass="Titolo_campoSmall">&nbsp;Traduzioni(°):</asp:Label>
                                                                            </td>
                                                                        </tr>
                                                                    </HeaderTemplate>
                                                                    <ItemTemplate>
                                                                        <tr>
                                                                            <td align="right" width="120px" height="22px">
                                                                                <asp:Label ID="LBlingua2ID" Text='<%# Databinder.eval(Container.DataItem, "Language.ID")%>'
                                                                                    runat="server" Visible="false" />
                                                                                <asp:Label ID="LBlingua2_Nome" Text='<%# Databinder.eval(Container.DataItem, "Language.Nome")%>'
                                                                                    runat="server" Visible="true" CssClass="Repeater_VoceLingua" />&nbsp;
                                                                            </td>
                                                                            <td align="left" height="22px">
                                                                                <asp:TextBox ID="TXBtermine2" runat="server" CssClass="Testo_campoSmall" Text='<%# Databinder.eval(Container.DataItem, "RoleType.Description")%>'
                                                                                    MaxLength="200" Columns="60"> </asp:TextBox>&nbsp;&nbsp;
                                                                            </td>
                                                                        </tr>
                                                                    </ItemTemplate>
                                                                    <FooterTemplate>
                                                                        <tr>
                                                                            <td colspan="2" class="nosize0">
                                                                                &nbsp;
                                                                            </td>
                                                                        </tr>
                                                                    </FooterTemplate>
                                                                </asp:Repeater>
                                                            </table>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </asp:TableCell>
                                        </asp:TableRow>
                                        <asp:TableRow ID="TBR_3">
                                            <asp:TableCell Width="120px">
                                                <asp:Label ID="LBlivelloRuolo_t" runat="server" CssClass="Titolo_campoSmall">&nbsp;Livello:</asp:Label>
                                            </asp:TableCell>
                                            <asp:TableCell>
                                                <asp:RadioButtonList ID="RBlivello" runat="server" RepeatDirection="Horizontal" CssClass="Testo_campoSmall">
                                                </asp:RadioButtonList>
                                            </asp:TableCell>
                                        </asp:TableRow>
                                        <asp:TableRow ID="TBR_4">
                                            <asp:TableCell Width="120px">&nbsp;</asp:TableCell>
                                            <asp:TableCell Width="100%" Height="35px" HorizontalAlign="left">
                                                <asp:Table ID="TBLlivelli" runat="server" CellSpacing="0" CellPadding="0" BorderColor="#3300cc"
                                                    BorderWidth="1">
                                                    <asp:TableRow>
                                                        <asp:TableCell CssClass="ROW_header_Small_Center" Width="100px">
                                                            <asp:Label ID="LBlivello_t" runat="server" CssClass="Titolo_campo">LIVELLO</asp:Label></asp:TableCell>
                                                        <asp:TableCell CssClass="ROW_header_Small_Center" Width="400px">
                                                            <asp:Label ID="LBruoli_t" runat="server" CssClass="Titolo_campo">RUOLI</asp:Label></asp:TableCell>
                                                    </asp:TableRow>
                                                </asp:Table>
                                            </asp:TableCell>
                                        </asp:TableRow>
                                    </asp:Table>
                                </asp:Panel>
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
            </td>
        </tr>
    </table>
</asp:Content>
