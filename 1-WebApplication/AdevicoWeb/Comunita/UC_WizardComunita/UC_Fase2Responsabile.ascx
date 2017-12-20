<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="UC_Fase2Responsabile.ascx.vb"
    Inherits="Comunita_OnLine.UC_Fase2Responsabile" TargetSchema="http://schemas.microsoft.com/intellisense/ie5" %>
<input type="hidden" id="HDNcmnt_ID" runat="server" name="HDNcmnt_ID" />
<input id="HDNidPadre" type="hidden" runat="server" name="HDNidPadre" />
<input id="HDN_ORGN_ID" type="hidden" name="HDN_ORGN_ID" runat="server" />
<input id="HDNprsn_id" type="hidden" name="HDNprsn_id" runat="server" />
<input type="hidden" id="HDNhasSetup" runat="server" name="HDNhasSetup" />
<input type="hidden" id="HDN_TipoComunita" runat="server" name="HDN_TipoComunita" />
<input type="hidden" id="HDN_ForInsert" runat="server" name="HDN_ForInsert" />
<asp:Table ID="TBLresponsabili" runat="server" HorizontalAlign="Center" GridLines="none"
    Width="850px">
    <asp:TableRow ID="TBRutenteSelezionato" Visible="False">
        <asp:TableCell>
            <table border="1" align="center" width="850px" cellspacing="0" style="border-color: #cccccc;
                height: 55px; background-color: #fffbf7">
                <tr>
                    <td>
                        <table border="0" cellpadding="0" cellspacing="0" width="800px">
                            <tr>
                                <td colspan="4">
                                    &nbsp;
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    &nbsp;
                                </td>
                                <td>
                                    <asp:Label ID="LBpersona_t" runat="server" CssClass="Titolo_campoSmall">Responsabile:</asp:Label>&nbsp;
                                    <asp:Label ID="LBpersona" runat="server" CssClass="Testo_CampoSmall"></asp:Label>&nbsp;
                                </td>
                                <td align="right">
                                    <asp:Button ID="BTNmodifica" runat="server" Text="Modifica" CssClass=""></asp:Button>
                                </td>
                                <td>
                                    &nbsp;
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    &nbsp;
                                </td>
                                <td colspan="2">
                                    <asp:Label ID="LBruoloResponsabile_t" runat="server" CssClass="Titolo_campoSmall">Ruolo responsabile:</asp:Label>&nbsp;
                                    <asp:DropDownList ID="DDLruoloResponsabile" runat="server" CssClass="Testo_CampoSmall">
                                    </asp:DropDownList>
                                </td>
                                <td>
                                    &nbsp;
                                </td>
                            </tr>
                            <tr>
                                <td colspan="4">
                                    &nbsp;
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
        </asp:TableCell>
    </asp:TableRow>
    <asp:TableRow ID="TBRsceltaUtenti">
        <asp:TableCell>
            <table border="1" align="center" width="850px" cellspacing="0" style="border-color: #cccccc;
                height: 55px; background-color: #fffbf7">
                <tr>
                    <td>
                        <table border="0" cellpadding="0" cellspacing="0" width="800px">
                            <tr>
                                <td>
                                    <asp:Label ID="LBscelta" runat="server" CssClass="Titolo_campoSmall">Scelta responsabile:</asp:Label>&nbsp;
                                    <asp:RadioButtonList ID="RBLscelta" runat="server" RepeatDirection="Horizontal" CssClass="Testo_CampoSmall"
                                        AutoPostBack="True" RepeatLayout="Flow">
                                        <asp:ListItem Value="1">dall'intero sistema</asp:ListItem>
                                        <asp:ListItem Value="2" Selected="True">da una organizzazione</asp:ListItem>
                                        <asp:ListItem Value="3">dalla comunità corrente</asp:ListItem>
                                    </asp:RadioButtonList>
                                </td>
                                <td align="right">
                                    <asp:Button ID="BNTannullaModifica" runat="server" Text="Annulla modifica" CssClass="">
                                    </asp:Button>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
        </asp:TableCell>
    </asp:TableRow>
    <asp:TableRow ID="TBRelencoUtenti">
        <asp:TableCell>
            <asp:Table ID="TBLfiltroNew" runat="server" Width="850px" CellPadding="0" CellSpacing="0"
                GridLines="none" HorizontalAlign="center">
                <asp:TableRow ID="TBRchiudiFiltro" Height="22px">
                    <asp:TableCell CssClass="Filtro_CellSelezionato" HorizontalAlign="Center" Width="150px"
                        Height="22px" VerticalAlign="Middle">
                        <asp:LinkButton ID="LNBchiudiFiltro" runat="server" CssClass="Filtro_Link" CausesValidation="False">Chiudi Filtri</asp:LinkButton>
                    </asp:TableCell>
                    <asp:TableCell CssClass="Filtro_CellDeSelezionato" Width="650px" Height="22px">&nbsp;</asp:TableCell>
                </asp:TableRow>
                <asp:TableRow ID="TBRapriFiltro" Visible="False" Height="22px">
                    <asp:TableCell CssClass="Filtro_CellApriFiltro" HorizontalAlign="Center" Width="150px"
                        Height="22px">
                        <asp:LinkButton ID="LNBapriFiltro" runat="server" CssClass="Filtro_Link" CausesValidation="False">Apri Filtri</asp:LinkButton>
                    </asp:TableCell>
                    <asp:TableCell CssClass="Filtro_Cellnull" Width="650px" Height="22px">&nbsp;
                    </asp:TableCell>
                </asp:TableRow>
                <asp:TableRow ID="TBRfiltri">
                    <asp:TableCell CssClass="Filtro_CellFiltri" ColumnSpan="2" HorizontalAlign="center">
                        <asp:Table runat="server" GridLines="None">
                            <asp:TableRow ID="TBRfiltroFacoltà">
                                <asp:TableCell CssClass="FiltroVoceSmall">
                                    <asp:Label ID="LBfacolta_t" runat="server">Facoltà:</asp:Label>&nbsp;
                                    <asp:DropDownList ID="DDLfacolta" runat="server" CssClass="FiltroCampoSmall" AutoPostBack="true">
                                    </asp:DropDownList>
                                </asp:TableCell>
                            </asp:TableRow>
                            <asp:TableRow>
                                <asp:TableCell>
                                    <table cellspacing="0" cellpadding="1" width="750px" border="0">
                                        <tr>
                                            <td class="FiltroVoceSmall" nowrap="nowrap">
                                                <asp:Label ID="LBtipoRuolo_t" runat="server" CssClass="FiltroCampoSmall">Tipo Ruolo:</asp:Label>
                                                <asp:Label ID="LBtipoPersona_t" runat="server" Visible="False">Tipo Persona:</asp:Label>&nbsp;
                                                <asp:DropDownList ID="DDLTipoRuolo" runat="server" CssClass="FiltroCampoSmall" AutoPostBack="true">
                                                </asp:DropDownList>
                                                <asp:DropDownList ID="DDLTipoPersona" runat="server" CssClass="FiltroCampoSmall"
                                                    AutoPostBack="true" Visible="False">
                                                </asp:DropDownList>
                                            </td>
                                            <td class="FiltroVoceSmall" nowrap="nowrap">
                                                <asp:Label ID="LBtipoRicerca_t" runat="server">Tipo Ricerca</asp:Label>&nbsp;
                                                <asp:DropDownList ID="DDLTipoRicerca" CssClass="FiltroCampoSmall" runat="server"
                                                    AutoPostBack="false">
                                                    <asp:ListItem Selected="true" Value="-2">Nome</asp:ListItem>
                                                    <asp:ListItem Value="-3">Cognome</asp:ListItem>
                                                    <asp:ListItem Value="-4">Nome/Cognome</asp:ListItem>
                                                    <asp:ListItem Value="-7">Login</asp:ListItem>
                                                </asp:DropDownList>
                                            </td>
                                            <td class="FiltroVoceSmall">
                                                <asp:Label ID="LBvalore_t" runat="server">Valore</asp:Label>&nbsp;
                                                <asp:TextBox ID="TXBValore" CssClass="FiltroCampoSmall" runat="server"></asp:TextBox>
                                            </td>
                                            <td class="FiltroVoceSmall" align="right">
                                                <asp:Button ID="BTNCerca" CssClass="PulsanteFiltro" runat="server" Text="Cerca">
                                                </asp:Button>
                                            </td>
                                        </tr>
                                    </table>
                                </asp:TableCell>
                            </asp:TableRow>
                        </asp:Table>
                    </asp:TableCell>
                </asp:TableRow>
                <asp:TableRow Visible="true">
                    <asp:TableCell ColumnSpan="2" HorizontalAlign="Center">
                        <table cellpadding="0" cellspacing="0" align="center" width="850px" border="0">
                            <tr>
                                <td>
                                    <table align="left" width="400px">
                                        <tr>
                                            <td align="center" nowrap="nowrap">
                                                <asp:LinkButton ID="LKBtutti" runat="server" CssClass="lettera" CommandArgument="-1"
                                                    OnClick="FiltroLinkLettere_Click">Tutti</asp:LinkButton>
                                            </td>
                                            <td align="center" nowrap="nowrap">
                                                <asp:LinkButton ID="LKBaltro" runat="server" CssClass="lettera" CommandArgument="0"
                                                    OnClick="FiltroLinkLettere_Click">Altro</asp:LinkButton>
                                            </td>
                                            <td align="center">
                                                <asp:LinkButton ID="LKBa" runat="server" CssClass="lettera" CommandArgument="1" OnClick="FiltroLinkLettere_Click">A</asp:LinkButton>
                                            </td>
                                            <td align="center">
                                                <asp:LinkButton ID="LKBb" runat="server" CssClass="lettera" CommandArgument="2" OnClick="FiltroLinkLettere_Click">B</asp:LinkButton>
                                            </td>
                                            <td align="center">
                                                <asp:LinkButton ID="LKBc" runat="server" CssClass="lettera" CommandArgument="3" OnClick="FiltroLinkLettere_Click">C</asp:LinkButton>
                                            </td>
                                            <td align="center">
                                                <asp:LinkButton ID="LKBd" runat="server" CssClass="lettera" CommandArgument="4" OnClick="FiltroLinkLettere_Click">D</asp:LinkButton>
                                            </td>
                                            <td align="center">
                                                <asp:LinkButton ID="LKBe" runat="server" CssClass="lettera" CommandArgument="5" OnClick="FiltroLinkLettere_Click">E</asp:LinkButton>
                                            </td>
                                            <td align="center">
                                                <asp:LinkButton ID="LKBf" runat="server" CssClass="lettera" CommandArgument="6" OnClick="FiltroLinkLettere_Click">F</asp:LinkButton>
                                            </td>
                                            <td align="center">
                                                <asp:LinkButton ID="LKBg" runat="server" CssClass="lettera" CommandArgument="7" OnClick="FiltroLinkLettere_Click">G</asp:LinkButton>
                                            </td>
                                            <td align="center">
                                                <asp:LinkButton ID="LKBh" runat="server" CssClass="lettera" CommandArgument="8" OnClick="FiltroLinkLettere_Click">H</asp:LinkButton>
                                            </td>
                                            <td align="center">
                                                <asp:LinkButton ID="LKBi" runat="server" CssClass="lettera" CommandArgument="9" OnClick="FiltroLinkLettere_Click">I</asp:LinkButton>
                                            </td>
                                            <td align="center">
                                                <asp:LinkButton ID="LKBj" runat="server" CssClass="lettera" CommandArgument="10"
                                                    OnClick="FiltroLinkLettere_Click">J</asp:LinkButton>
                                            </td>
                                            <td align="center">
                                                <asp:LinkButton ID="LKBk" runat="server" CssClass="lettera" CommandArgument="11"
                                                    OnClick="FiltroLinkLettere_Click">K</asp:LinkButton>
                                            </td>
                                            <td align="center">
                                                <asp:LinkButton ID="LKBl" runat="server" CssClass="lettera" CommandArgument="12"
                                                    OnClick="FiltroLinkLettere_Click">L</asp:LinkButton>
                                            </td>
                                            <td align="center">
                                                <asp:LinkButton ID="LKBm" runat="server" CssClass="lettera" CommandArgument="13"
                                                    OnClick="FiltroLinkLettere_Click">M</asp:LinkButton>
                                            </td>
                                            <td align="center">
                                                <asp:LinkButton ID="LKBn" runat="server" CssClass="lettera" CommandArgument="14"
                                                    OnClick="FiltroLinkLettere_Click">N</asp:LinkButton>
                                            </td>
                                            <td align="center">
                                                <asp:LinkButton ID="LKBo" runat="server" CssClass="lettera" CommandArgument="15"
                                                    OnClick="FiltroLinkLettere_Click">O</asp:LinkButton>
                                            </td>
                                            <td align="center">
                                                <asp:LinkButton ID="LKBp" runat="server" CssClass="lettera" CommandArgument="16"
                                                    OnClick="FiltroLinkLettere_Click">P</asp:LinkButton>
                                            </td>
                                            <td align="center">
                                                <asp:LinkButton ID="LKBq" runat="server" CssClass="lettera" CommandArgument="17"
                                                    OnClick="FiltroLinkLettere_Click">Q</asp:LinkButton>
                                            </td>
                                            <td align="center">
                                                <asp:LinkButton ID="LKBr" runat="server" CssClass="lettera" CommandArgument="18"
                                                    OnClick="FiltroLinkLettere_Click">R</asp:LinkButton>
                                            </td>
                                            <td align="center">
                                                <asp:LinkButton ID="LKBs" runat="server" CssClass="lettera" CommandArgument="19"
                                                    OnClick="FiltroLinkLettere_Click">S</asp:LinkButton>
                                            </td>
                                            <td align="center">
                                                <asp:LinkButton ID="LKBt" runat="server" CssClass="lettera" CommandArgument="20"
                                                    OnClick="FiltroLinkLettere_Click">T</asp:LinkButton>
                                            </td>
                                            <td align="center">
                                                <asp:LinkButton ID="LKBu" runat="server" CssClass="lettera" CommandArgument="21"
                                                    OnClick="FiltroLinkLettere_Click">U</asp:LinkButton>
                                            </td>
                                            <td align="center">
                                                <asp:LinkButton ID="LKBv" runat="server" CssClass="lettera" CommandArgument="22"
                                                    OnClick="FiltroLinkLettere_Click">V</asp:LinkButton>
                                            </td>
                                            <td align="center">
                                                <asp:LinkButton ID="LKBw" runat="server" CssClass="lettera" CommandArgument="23"
                                                    OnClick="FiltroLinkLettere_Click">W</asp:LinkButton>
                                            </td>
                                            <td align="center">
                                                <asp:LinkButton ID="LKBx" runat="server" CssClass="lettera" CommandArgument="24"
                                                    OnClick="FiltroLinkLettere_Click">X</asp:LinkButton>
                                            </td>
                                            <td align="center">
                                                <asp:LinkButton ID="LKBy" runat="server" CssClass="lettera" CommandArgument="25"
                                                    OnClick="FiltroLinkLettere_Click">Y</asp:LinkButton>
                                            </td>
                                            <td align="center">
                                                <asp:LinkButton ID="LKBz" runat="server" CssClass="lettera" CommandArgument="26"
                                                    OnClick="FiltroLinkLettere_Click">Z</asp:LinkButton>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                                <td align="right">
                                    <asp:Label ID="LBnumeroRecord_t" runat="server" CssClass="Filtro_TestoPaginazione">N° Record</asp:Label>
                                    <asp:DropDownList ID="DDLNumeroRecord" CssClass="Filtro_RecordPaginazione" runat="server"
                                        AutoPostBack="true">
                                        <asp:ListItem Value="15"></asp:ListItem>
                                        <asp:ListItem Value="30" Selected="true"></asp:ListItem>
                                        <asp:ListItem Value="45"></asp:ListItem>
                                        <asp:ListItem Value="50"></asp:ListItem>
                                        <asp:ListItem Value="70"></asp:ListItem>
                                        <asp:ListItem Value="100"></asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                            </tr>
                        </table>
                    </asp:TableCell>
                </asp:TableRow>
                <asp:TableRow>
                    <asp:TableCell HorizontalAlign="center" ColumnSpan="2">
                        <asp:Table ID="TBLutenti" runat="server" HorizontalAlign="center">
                            <asp:TableRow>
                                <asp:TableCell HorizontalAlign="Left">
                                    <asp:DataGrid ID="DGPersone" runat="server" DataKeyField="PRSN_id" AllowPaging="true"
                                        AutoGenerateColumns="False" ShowFooter="false" AllowSorting="true" CssClass="DataGrid_Generica">
                                        <AlternatingItemStyle CssClass="ROW_Alternate_Small"></AlternatingItemStyle>
                                        <HeaderStyle CssClass="ROW_header_Small_Center"></HeaderStyle>
                                        <ItemStyle CssClass="ROW_Normal_Small" Height="22px"></ItemStyle>
                                        <PagerStyle CssClass="ROW_Page_Small" Position="TopAndBottom" Mode="NumericPages"
                                            Visible="true" HorizontalAlign="Right" Height="18px" VerticalAlign="Bottom">
                                        </PagerStyle>
                                        <Columns>
                                            <asp:BoundColumn DataField="PRSN_id" Visible="False"></asp:BoundColumn>
                                            <asp:TemplateColumn runat="server" HeaderText="" ItemStyle-Width="25px" ItemStyle-CssClass="ROW_TD_Small_Center">
                                                <ItemTemplate>
                                                    <asp:ImageButton ID="IMBinfo" runat="server" CausesValidation="False" CommandName="infoPersona"
                                                        ImageUrl="../../images/proprieta.gif"></asp:ImageButton>
                                                </ItemTemplate>
                                            </asp:TemplateColumn>
                                            <asp:TemplateColumn ItemStyle-CssClass="ROW_TD_Small" HeaderText="Cognome" SortExpression="PRSN_Cognome">
                                                <ItemTemplate>
                                                    &nbsp;<%# DataBinder.Eval(Container.DataItem, "PRSN_Cognome") %>
                                                </ItemTemplate>
                                            </asp:TemplateColumn>
                                            <asp:TemplateColumn ItemStyle-CssClass="ROW_TD_Small" HeaderText="Nome" SortExpression="PRSN_Nome">
                                                <ItemTemplate>
                                                    &nbsp;<%# DataBinder.Eval(Container.DataItem, "PRSN_Nome") %>
                                                </ItemTemplate>
                                            </asp:TemplateColumn>
                                            <asp:BoundColumn DataField="PRSN_Anagrafica" HeaderText="Anagrafica" Visible="False">
                                            </asp:BoundColumn>
                                            <asp:TemplateColumn runat="server" HeaderText="Mail" ItemStyle-Width="200px" ItemStyle-CssClass="ROW_TD_Small"
                                                HeaderStyle-CssClass="ROW_header_Small_Center">
                                                <ItemTemplate>
                                                    &nbsp;<asp:HyperLink NavigateUrl='<%# "mailto:" & Container.Dataitem("PRSN_mail")%>'
                                                        Text='<%# Container.Dataitem("PRSN_mail")%>' runat="server" ID="HYPMail" CssClass="ROW_ItemLink_Small" />&nbsp;
                                                </ItemTemplate>
                                            </asp:TemplateColumn>
                                            <asp:TemplateColumn ItemStyle-CssClass="ROW_TD_Small" HeaderText="Tipo Ruolo" SortExpression="TPRL_nome">
                                                <ItemTemplate>
                                                    &nbsp;<%# DataBinder.Eval(Container.DataItem, "TPRL_nome") %>
                                                </ItemTemplate>
                                            </asp:TemplateColumn>
                                            <asp:BoundColumn DataField="PRSN_TPPR_id" Visible="False"></asp:BoundColumn>
                                            <asp:TemplateColumn ItemStyle-CssClass="ROW_TD_Small" HeaderText="Tipo Persona" SortExpression="TPPR_descrizione"
                                                Visible="False">
                                                <ItemTemplate>
                                                    &nbsp;<%# DataBinder.Eval(Container.DataItem, "TPPR_descrizione") %>
                                                </ItemTemplate>
                                            </asp:TemplateColumn>
                                            <asp:BoundColumn DataField="PRSN_TPPR_id" Visible="False"></asp:BoundColumn>
                                            <asp:BoundColumn DataField="TPPR_descrizione" HeaderText="Tipo Persona" SortExpression="TPPR_descrizione"
                                                Visible="False"></asp:BoundColumn>
                                            <asp:TemplateColumn runat="server" HeaderText="Seleziona" ItemStyle-HorizontalAlign="Center"
                                                HeaderStyle-CssClass="ROW_HeaderLink_Small">
                                                <ItemTemplate>
                                                    <asp:LinkButton ID="LNBseleziona" runat="server" CommandName="seleziona" Visible="True"
                                                        CssClass="ROW_ItemLink_Small">Seleziona</asp:LinkButton>
                                                </ItemTemplate>
                                            </asp:TemplateColumn>
                                        </Columns>
                                        <PagerStyle Width="850px" PageButtonCount="5" Mode="NumericPages"></PagerStyle>
                                    </asp:DataGrid>
                                    <asp:Label ID="LBnouser" CssClass="avviso" runat="server"></asp:Label>
                                </asp:TableCell>
                            </asp:TableRow>
                        </asp:Table>
                    </asp:TableCell>
                </asp:TableRow>
            </asp:Table>
        </asp:TableCell>
    </asp:TableRow>
</asp:Table>
