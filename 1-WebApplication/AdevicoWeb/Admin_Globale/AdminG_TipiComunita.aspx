<%@ Page Language="vb" AutoEventWireup="false" MasterPageFile="~/Admin_Globale/AdminPortal.Master"
    CodeBehind="AdminG_TipiComunita.aspx.vb" Inherits="Comunita_OnLine.AdminG_TipiComunita" %>

<%@ MasterType VirtualPath="~/Admin_Globale/AdminPortal.Master" %>
<%@ Register TagPrefix="CTRL" TagName="CTRLmodifica" Src="./UC/UC_modificaTipoComunita.ascx" %>
<%--OLD radtabstrip TELERIK--%>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>

<asp:Content ContentPlaceHolderID="HeadContent" runat="server" ID="Content1">
    <script type="text/javascript" language="Javascript" src="./../jscript/generali.js"></script>
    <style type="text/css">
        td
        {
            font-size: 11px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="CPHservice" runat="server">
    <input type="hidden" id="HDNazione" value="gestioneTipo" runat="server" />
    <table cellspacing="0" cellpadding="0" width="900px" border="0">
        <%--		<tr>
			<td align="left" class="RigaTitoloAdmin">
				<asp:Label ID="LBTitolo" Runat="server">Gestione Tipo Comunità</asp:Label>
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
                    <asp:LinkButton ID="LNBaddRuolo" runat="server" Text="Aggiungi ruolo" CssClass="Link_Menu"
                        Visible="False"></asp:LinkButton>
                    <asp:LinkButton ID="LNBdefault" runat="server" CssClass="Link_Menu" Visible="False">Setta default</asp:LinkButton>
                    <asp:LinkButton ID="LNBtipocomunitaForAll" runat="server" CssClass="Link_Menu" Visible="False">Replica su tutte</asp:LinkButton>
                    <asp:LinkButton ID="LNBsetToAllCommunity" runat="server" CssClass="Link_Menu" Visible="False">Imposta per tutte le comunità</asp:LinkButton>
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
                                <asp:Panel ID="PNLlista" runat="server">
                                    <asp:DataGrid ID="DGtipoComunita" runat="server" ShowFooter="false" AutoGenerateColumns="False"
                                        AllowCustomPaging="True" DataKeyField="TPCM_id" AllowSorting="true" PageSize="20"
                                        CssClass="DataGrid_Generica">
                                        <AlternatingItemStyle CssClass="ROW_Alternate_Small"></AlternatingItemStyle>
                                        <HeaderStyle CssClass="ROW_header_Small_Center"></HeaderStyle>
                                        <ItemStyle CssClass="ROW_Normal_Small" Height="22px"></ItemStyle>
                                        <PagerStyle CssClass="ROW_Page_Small" Position="TopAndBottom" Mode="NumericPages"
                                            Visible="true" HorizontalAlign="Right" Height="18px" VerticalAlign="Bottom">
                                        </PagerStyle>
                                        <Columns>
                                            <asp:TemplateColumn runat="server" HeaderText="" ItemStyle-HorizontalAlign="Center"
                                                ItemStyle-Width="50">
                                                <ItemTemplate>
                                                    <asp:ImageButton ID="IMBmodifica" runat="server" CausesValidation="False" CommandName="modifica"
                                                        ImageUrl="../images/m.gif"></asp:ImageButton>
                                                    <asp:ImageButton ID="IMBCancella" runat="server" CausesValidation="False" CommandName="elimina"
                                                        ImageUrl="../images/x.gif"></asp:ImageButton>
                                                </ItemTemplate>
                                            </asp:TemplateColumn>
                                            <asp:BoundColumn HeaderText="Nome Tipo Comunità" DataField="TPCM_descrizione" SortExpression="TPCM_descrizione"
                                                HeaderStyle-CssClass="ROW_Header_Small_center" ItemStyle-CssClass="ROW_TD_Small">
                                            </asp:BoundColumn>
                                            <asp:BoundColumn DataField="ComunitaAssociate" HeaderText="Comunita Associate" Visible="True"
                                                ItemStyle-Width="120px" HeaderStyle-CssClass="ROW_Header_Small_center" ItemStyle-CssClass="ROW_TD_Small_center">
                                            </asp:BoundColumn>
                                            <asp:BoundColumn DataField="TPCM_noDelete" Visible="false" ItemStyle-Width="80px">
                                            </asp:BoundColumn>
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
                                <asp:Panel ID="PNLmodifica" runat="server" HorizontalAlign="Center" Visible="False">
                                    <table>
                                        <tr>
                                            <td>
                                                <telerik:RadTabStrip ID="TBSmenu" runat="server" Align="Justify" Width="700px" Height="26px"
                                                    SelectedIndex="0" CausesValidation="false" AutoPostBack="true" Skin="Outlook"
                                                    EnableEmbeddedSkins="true">
                                                    <Tabs>
                                                        <telerik:RadTab Text="Dati Tipologia" Value="TABtipologia" runat="server"/>
                                                        <telerik:RadTab Text="Categoria File" Value="TABcategoriaFile" runat="server"/>
                                                        <telerik:RadTab Text="Tipi Ruolo" Value="TABtipoRuolo"  runat="server"/>
                                                        <telerik:RadTab Text="Ruoli Profilo" Value="TABruoliProfilo" runat="server"/>
                                                        <telerik:RadTab Text="Modelli" Value="TABmodelli" runat="server"/>
                                                        <telerik:RadTab Text="Servizi" Value="TABservizio" runat="server"/>
                                                        <telerik:RadTab Text="Permessi" Value="TABpermessi" runat="server"/>
                                                    </Tabs>
                                                </telerik:RadTabStrip>	
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <CTRL:CTRLmodifica ID="CTRLmodifica" runat="server"></CTRL:CTRLmodifica>
                                            </td>
                                        </tr>
                                    </table>
                                </asp:Panel>
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
            </td>
        </tr>
    </table>
</asp:Content>