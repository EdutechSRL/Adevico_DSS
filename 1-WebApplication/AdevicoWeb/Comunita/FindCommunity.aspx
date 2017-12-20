<%@ Page Language="vb" AutoEventWireup="false" MasterPageFile="~/AjaxPortal.Master"
    CodeBehind="FindCommunity.aspx.vb" Inherits="Comunita_OnLine.FindCommunity" %>

<%@ MasterType VirtualPath="~/AjaxPortal.Master" %>
<%@ Register TagPrefix="DETTAGLI" TagName="CTRLDettagli" Src="../UC/UC_dettaglicomunita.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <script language="Javascript" src="./../jscript/generali.js" type="text/javascript"></script>
    <script language="javascript" type="text/javascript">
         
        function ChiudiMi() {
            this.window.close();
        }
      
        function SelectMe(Me) {
            var HIDcheckbox, selezionati;
            //eval('HIDcheckbox= this.document.forms[0].HDNcomunitaSelezionate')
            //eval('HIDcheckbox=<%=Me.HDNcomunitaSelezionate.ClientID%>')
            HIDcheckbox = this.document.getElementById('<%=Me.HDNcomunitaSelezionate.ClientID%>');
            selezionati = 0
            for (i = 0; i < document.forms[0].length; i++) {
                e = document.forms[0].elements[i];
                if (e.type == 'checkbox' && e.name.indexOf("CBcorso") != -1) {
                    if (e.checked == true) {
                        selezionati++
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
            //eval('HDNcomunitaSelezionate= this.document.forms[0].HDNcomunitaSelezionate')
            //eval('HDNcomunitaSelezionate=<%=Me.HDNcomunitaSelezionate.ClientID%>')
            HDNcomunitaSelezionate = this.document.getElementById('<%=Me.HDNcomunitaSelezionate.ClientID%>');
            HDNcomunitaSelezionate.value = ""
            for (i = 0; i < document.forms[0].length; i++) {
                e = document.forms[0].elements[i];
                if (e.type == 'checkbox' && e.name.indexOf("CBcorso") != -1) {
                    e.checked = actVar;
                    if (e.checked == true)
                        if (HDNcomunitaSelezionate.value == "")
                            HDNcomunitaSelezionate.value = ',' + e.value + ','
                        else
                            HDNcomunitaSelezionate.value = HDNcomunitaSelezionate.value + e.value + ','
                    }
                }
            }

        function HasComunitaSelezionate(conferma, Messaggio, MessaggioConferma) {
            var HIDcheckbox, selezionati;
            //eval('HIDcheckbox= this.document.forms[0].HDNcomunitaSelezionate')
            //eval('HIDcheckbox=<%=Me.HDNcomunitaSelezionate.ClientID%>')
            HIDcheckbox = this.document.getElementById('<%=Me.HDNcomunitaSelezionate.ClientID%>');
            if (HIDcheckbox.value == "," || HIDcheckbox.value == "") {
                alert(Messaggio)
                return false;
            }
            else {
                if (conferma == true) {
                    return confirm(MessaggioConferma);
                }
                else
                    return true;
            }
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="CPHservice" runat="server">
    <table cellspacing="0" cellpadding="0" width="900px" align="center" border="0">
        <tr>
            <td colspan="3">
                <table width="900px" align="center">
<%--                    <tr>
                        <td class="RigaTitolo" align="left">
                            <asp:Label ID="LBtitolo" runat="server">Ricerca comunità</asp:Label>
                        </td>
                    </tr>--%>
                    <tr>
                        <td align="right">
                            <asp:Panel ID="PNLmenu" runat="server" HorizontalAlign="Right" Visible="true">
                                <asp:LinkButton ID="LNBiscriviMultipli" Enabled="False" runat="server" CssClass="LINK_MENU"
                                    Text="Iscrivi ai selzionati"></asp:LinkButton>
                            </asp:Panel>
                            <asp:Panel ID="PNLmenuDettagli" runat="server" HorizontalAlign="Right" Visible="False">
                                <asp:LinkButton ID="LNBannullaDettagli" runat="server" Text="Torna all'elenco" CausesValidation="false"
                                    CssClass="LINK_MENU"></asp:LinkButton>
                                <asp:LinkButton ID="LNBiscriviDettagli" runat="server" CssClass="LINK_MENU" CausesValidation="True"
                                    Text="Iscrivi"></asp:LinkButton>
                                <asp:LinkButton ID="LNBentraDettagli" runat="server" CssClass="LINK_MENU" CausesValidation="True"
                                    Text="Entra"></asp:LinkButton>
                            </asp:Panel>
                            <asp:Panel ID="PNLmenuConferma" runat="server" HorizontalAlign="Right" Visible="False">
                                <asp:LinkButton ID="LNBannullaConferma" runat="server" Text="Torna all'elenco" CausesValidation="false"
                                    CssClass="LINK_MENU"></asp:LinkButton>
                                <asp:LinkButton ID="LNBiscriviConferma" runat="server" CssClass="LINK_MENU" CausesValidation="True"
                                    Text="Iscrivi"></asp:LinkButton>
                            </asp:Panel>
                            <asp:Panel ID="PNLmenuIscritto" runat="server" HorizontalAlign="Right" Visible="False">
                                <asp:LinkButton ID="LNBelencoIscritte" runat="server" Text="Torna alle iscritte"
                                    CausesValidation="false" CssClass="LINK_MENU"></asp:LinkButton>
                                <asp:LinkButton ID="LNBiscriviAltre" runat="server" CssClass="LINK_MENU" CausesValidation="True"
                                    Text="Altra iscrizione"></asp:LinkButton>
                            </asp:Panel>
                            <asp:Panel ID="PNLmenuAccesso" runat="server" HorizontalAlign="Right" Visible="False">
                                <asp:LinkButton ID="LNBannulla" runat="server" Text="Torna all'elenco" CausesValidation="false"
                                    CssClass="LINK_MENU"></asp:LinkButton>
                            </asp:Panel>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td align="center">
                <asp:Panel ID="PNLconferma" runat="server" Visible="False" HorizontalAlign="Center">
                    <input type="hidden" id="HDisChiusa" runat="server" name="HDisChiusa" />
                    <table align="center">
                        <tr>
                            <td height="50">
                                &nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td align="left">
                                <asp:Label ID="LBconferma" CssClass="messaggioIscrizione" runat="server">Conferma l'iscrizione alla comunità #nomeComunita# - #nomeResponsabile#</asp:Label>
                                <asp:Label ID="LBconfermaMultipla" CssClass="messaggioIscrizione" runat="server"
                                    Visible="False"></asp:Label>
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
                    <input type="hidden" runat="server" id="HDNselezionato" name="HDNselezionato" />
                    <input type="hidden" runat="server" id="HDNcomunitaSelezionate" name="HDNcomunitaSelezionate" />
                    <input type="hidden" runat="server" id="HDN_filtroFacolta" name="HDN_filtroFacolta" />
                    <input type="hidden" runat="server" id="HDN_filtroValore" name="HDN_filtroValore" />
                    <input type="hidden" runat="server" id="HDN_filtroResponsabileID" name="HDN_filtroResponsabileID" />
                    <input type="hidden" runat="server" id="HDN_filtroLaureaID" name="HDN_filtroLaureaID" />
                    <input type="hidden" runat="server" id="HDN_filtroTipoCdl" name="HDN_filtroTipoCdl" />
                    <input type="hidden" runat="server" id="HDN_filtroAnno" name="HDN_filtroAnno" />
                    <input type="hidden" runat="server" id="HDN_filtroTipoComunitaID" name="HDN_filtroTipoComunitaID" />
                    <input type="hidden" runat="server" id="HDN_filtroRicercaByIscrizione" name="HDN_filtroRicercaByIscrizione" />
                    <input type="hidden" runat="server" id="HDN_filtroStatus" name="HDN_filtroStatus" />
                    <asp:Table ID="TBLfiltroNew" runat="server" Width="900px" CellPadding="0" CellSpacing="0">
                        <asp:TableRow ID="TBRchiudiFiltro" Height="22px">
                            <asp:TableCell CssClass="Filtro_CellSelezionato" HorizontalAlign="center" Width="150px"
                                Height="22px" VerticalAlign="Middle">
							    &nbsp;
                            </asp:TableCell>
                            <asp:TableCell CssClass="Filtro_CellDeSelezionato" Width="750px" Height="22px">&nbsp;
                            </asp:TableCell>
                        </asp:TableRow>
                        <asp:TableRow ID="TBRfiltri">
                            <asp:TableCell CssClass="Filtro_CellFiltri" ColumnSpan="2" Width="900px" HorizontalAlign="center">
                                <asp:Table runat="server" ID="TBLfiltro" CellPadding="1" CellSpacing="0" Width="900px"
                                    HorizontalAlign="center">
                                    <asp:TableRow>
                                        <asp:TableCell CssClass="FiltroVoceSmall" ColumnSpan="3">
                                            <table cellspacing="0" border="0" align="left">
                                                <tr>
                                                    <td height="30px">
                                                        &nbsp;
                                                    </td>
                                                    <td nowrap="nowrap" colspan="2">
                                                        <asp:Label ID="LBorganizzazione_c" runat="server" CssClass="FiltroVoceSmall">Organizzazione:&nbsp;</asp:Label>
                                                        <asp:DropDownList ID="DDLorganizzazione" runat="server" AutoPostBack="True" CssClass="FiltroCampoSmall">
                                                        </asp:DropDownList>
                                                    </td>
                                                    <td height="30px">
                                                        &nbsp;
                                                    </td>
                                                    <td height="30px" nowrap="nowrap">
                                                        <asp:Label ID="LBtipoComunita_c" runat="server" CssClass="FiltroVoceSmall">Tipo Comunità</asp:Label>&nbsp;
                                                        <asp:DropDownList ID="DDLTipo" runat="server" CssClass="FiltroCampoSmall" AutoPostBack="true">
                                                        </asp:DropDownList>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td height="30px">
                                                        &nbsp;
                                                    </td>
                                                    <td>
                                                        <asp:Table ID="TBLcorsi" CellPadding="2" CellSpacing="2" BorderStyle="None" runat="server"
                                                            Visible="False">
                                                            <asp:TableRow>
                                                                <asp:TableCell>
                                                                    <asp:Label ID="LBannoAccademico_c" runat="server" CssClass="FiltroVoceSmall">A.A.:&nbsp;</asp:Label>
                                                                    <asp:DropDownList ID="DDLannoAccademico" runat="server" CssClass="FiltroCampoSmall">
                                                                    </asp:DropDownList>
                                                                    &nbsp;&nbsp;&nbsp;
                                                                </asp:TableCell>
                                                            </asp:TableRow>
                                                        </asp:Table>
                                                        <asp:Table ID="TBLcorsiDiStudio" CellPadding="2" CellSpacing="2" BorderStyle="None"
                                                            runat="server" Visible="False">
                                                            <asp:TableRow>
                                                                <asp:TableCell>
                                                                    <asp:Label ID="LBcorsoDiStudi_t" runat="server" CssClass="FiltroVoceSmall">A.A.:&nbsp;</asp:Label>
                                                                    <asp:DropDownList ID="DDLtipoCorsoDiStudi" runat="server" CssClass="FiltroCampoSmall">
                                                                    </asp:DropDownList>
                                                                    &nbsp;&nbsp;&nbsp;
                                                                </asp:TableCell>
                                                            </asp:TableRow>
                                                        </asp:Table>
                                                        <asp:Label ID="LBnoCorsi" runat="server"></asp:Label>
                                                    </td>
                                                    <td height="30px">
                                                        <asp:Label ID="LBresponsabile_t" runat="server" CssClass="FiltroVoceSmall">Responsabile:&nbsp;</asp:Label>
                                                        <asp:DropDownList ID="DDLresponsabile" runat="server" CssClass="FiltroCampoSmall">
                                                        </asp:DropDownList>
                                                    </td>
                                                    <td height="30px">
                                                        &nbsp;&nbsp;&nbsp;
                                                    </td>
                                                    <td nowrap="nowrap" height="30px">
                                                        <asp:Label ID="LBnomeComunita_t" runat="server" CssClass="FiltroVoceSmall">Nome comunità:</asp:Label>&nbsp;
                                                        <asp:TextBox ID="TXBValore" runat="server" CssClass="FiltroCampoSmall" MaxLength="100"
                                                            Columns="30"></asp:TextBox>
                                                    </td>
                                                </tr>
                                            </table>
                                        </asp:TableCell>
                                    </asp:TableRow>
                                    <asp:TableRow Height="30px">
                                        <asp:TableCell Height="30px">&nbsp;</asp:TableCell>
                                        <asp:TableCell Height="30px">
                                            <asp:Label ID="LBricercaByIscrizione_c" runat="server" CssClass="FiltroVoceSmall">Comunità:</asp:Label>
                                            <asp:RadioButtonList ID="RBLricercaByIscrizione" runat="server" CssClass="FiltroCampoSmall"
                                                AutoPostBack="true" RepeatDirection="Horizontal" RepeatLayout="Flow">
                                                <asp:ListItem Value="0" Selected="True">a cui iscriversi</asp:ListItem>
                                                <asp:ListItem Value="1">a cui si è iscritti</asp:ListItem>
                                            </asp:RadioButtonList>
                                            &nbsp;&nbsp;&nbsp;&nbsp;
                                            <asp:Label ID="LBstatoComunita_t" runat="server" CssClass="FiltroVoceSmall">Stato:</asp:Label>
                                            <asp:RadioButtonList ID="RBLstatoComunita" runat="server" CssClass="FiltroCampoSmall"
                                                AutoPostBack="true" RepeatDirection="Horizontal" RepeatLayout="Flow">
                                                <asp:ListItem Value="0" Selected="True">Attivate</asp:ListItem>
                                                <asp:ListItem Value="1">Archiviate</asp:ListItem>
                                            </asp:RadioButtonList>
                                        </asp:TableCell>
                                        <asp:TableCell HorizontalAlign="right" Height="30px">
                                            <asp:Button ID="BTNCerca" runat="server" CssClass="PulsanteFiltro" Text="Cerca">
                                            </asp:Button>
                                        </asp:TableCell>
                                    </asp:TableRow>
                                </asp:Table>
                            </asp:TableCell>
                        </asp:TableRow>
                        <asp:TableRow Visible="true" ID="TBRfiltriGenerici">
                            <asp:TableCell ColumnSpan="2" Width="900px" HorizontalAlign="center">
                                <table cellpadding="0" cellspacing="0" align="center" width="100%" border="0">
                                    <tr>
                                        <td align="right">
                                            <asp:Label ID="LBnumeroRecord_c" runat="server" CssClass="Filtro_TestoPaginazione">N° Record</asp:Label>
                                            <asp:DropDownList ID="DDLNumeroRecord" CssClass="Filtro_RecordPaginazione" runat="server"
                                                AutoPostBack="true">
                                                <asp:ListItem Value="15"></asp:ListItem>
                                                <asp:ListItem Value="30" Selected="true"></asp:ListItem>
                                                <asp:ListItem Value="45"></asp:ListItem>
                                                <asp:ListItem Value="50"></asp:ListItem>
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                </table>
                            </asp:TableCell>
                        </asp:TableRow>
                        <asp:TableRow>
                            <asp:TableCell ColumnSpan="2">
                                <asp:DataGrid ID="DGComunita" runat="server" PageSize="30" DataKeyField="CMNT_id"
                                    AllowPaging="true" AutoGenerateColumns="False" AllowSorting="true" ShowFooter="false"
                                    CssClass="DataGrid_Generica">
                                    <AlternatingItemStyle CssClass="ROW_Alternate_Small"></AlternatingItemStyle>
                                    <HeaderStyle CssClass="ROW_header_Small_Center"></HeaderStyle>
                                    <ItemStyle CssClass="ROW_Normal_Small" Height="22px"></ItemStyle>
                                    <PagerStyle CssClass="ROW_Page_Small" Position="TopAndBottom" Mode="NumericPages"
                                        Visible="true" HorizontalAlign="Right" Height="18px" VerticalAlign="Bottom">
                                    </PagerStyle>
                                    <Columns>
                                        <asp:TemplateColumn runat="server" HeaderText="Tipo" ItemStyle-Width="40" SortExpression="TPCM_Descrizione"
                                            ItemStyle-CssClass="ROW_TD_Small_Center">
                                            <ItemTemplate>
                                                <img runat="server" src='<%# DataBinder.Eval(Container.DataItem, "TPCM_icona") %>'
                                                    alt='<%# DataBinder.Eval(Container.DataItem, "TPCM_descrizione") %>' id="Img2" />
                                            </ItemTemplate>
                                        </asp:TemplateColumn>
                                        <asp:TemplateColumn HeaderText="Nome" SortExpression="CMNT_Nome">
                                            <ItemTemplate>
                                                <asp:Table ID="TBLnome" runat="server" HorizontalAlign="left">
                                                    <asp:TableRow ID="TBRnome" runat="server">
                                                        <asp:TableCell>
														    &nbsp;
                                                        </asp:TableCell>
                                                        <asp:TableCell ID="TBCchiusa" runat="server">
                                                            <asp:Image ID="IMGisChiusa" runat="server" Visible="False" BorderStyle="None"></asp:Image>
                                                        </asp:TableCell>
                                                        <asp:TableCell ID="TBCnome" runat="server">
                                                            <asp:Label ID="LBcomunitaNome" runat="server">
															    <%# DataBinder.Eval(Container.DataItem, "CMNT_Esteso") %>
                                                            </asp:Label>
                                                            (<b><asp:LinkButton ID="LNBlogin" runat="server" CommandName="Login" CausesValidation="False">Entra</asp:LinkButton>
                                                                <asp:LinkButton ID="LNBiscrivi" runat="server" CommandName="Iscrivi" Visible="False"
                                                                    CausesValidation="False">Entra</asp:LinkButton></b> |
                                                            <asp:LinkButton ID="LNBdettagli" runat="server" CommandName="dettagli" CausesValidation="False">Dettagli</asp:LinkButton>
                                                            <asp:Label ID="LBseparatorNews" runat="server" Visible="False">|</asp:Label>
                                                            <asp:Literal ID="LThasnews" runat="server" Visible="false"></asp:Literal>
                                                            )
                                                        </asp:TableCell>
                                                    </asp:TableRow>
                                                </asp:Table>
                                            </ItemTemplate>
                                        </asp:TemplateColumn>
                                        <asp:BoundColumn DataField="AnnoAccademico" HeaderText="A.A." Visible="false" SortExpression="CMNT_Anno"
                                            ItemStyle-CssClass="ROW_TD_Small_Center"></asp:BoundColumn>
                                        <asp:BoundColumn DataField="Periodo" HeaderText="Periodo" Visible="false" SortExpression="CMNT_PRDO_descrizione"
                                            ItemStyle-CssClass="ROW_TD_Small"></asp:BoundColumn>
                                        <asp:ButtonColumn Text="Mostra" CommandName="dettagli" HeaderText="Dettagli" ItemStyle-Width="60"
                                            Visible="False"></asp:ButtonColumn>
                                        <asp:BoundColumn DataField="AnagraficaResponsabile" HeaderText="Responsabile" ItemStyle-Width="150"
                                            SortExpression="CMNT_Responsabile" ItemStyle-CssClass="ROW_TD_Small"></asp:BoundColumn>
                                        <asp:BoundColumn HeaderText="Iscritti" DataField="Iscritti" ItemStyle-CssClass="ROW_TD_Small"
                                            HeaderStyle-CssClass="ROW_Header_Small"></asp:BoundColumn>
                                        <asp:BoundColumn DataField="CMNT_ID" Visible="False"></asp:BoundColumn>
                                        <asp:BoundColumn DataField="CMNT_Anno" Visible="False"></asp:BoundColumn>
                                        <asp:BoundColumn DataField="CMNT_dataInizioIscrizione" Visible="False"></asp:BoundColumn>
                                        <asp:BoundColumn DataField="CMNT_dataFineIscrizione" Visible="False"></asp:BoundColumn>
                                        <asp:BoundColumn DataField="ALCM_Path" Visible="false"></asp:BoundColumn>
                                        <asp:BoundColumn DataField="CMNT_Responsabile" Visible="False"></asp:BoundColumn>
                                        <asp:BoundColumn DataField="TPCM_icona" Visible="false"></asp:BoundColumn>
                                        <asp:BoundColumn DataField="CMNT_EstesoNoSpan" Visible="false"></asp:BoundColumn>
                                        <asp:BoundColumn DataField="CMNT_Iscritti" Visible="false"></asp:BoundColumn>
                                        <asp:BoundColumn DataField="RLPC_attivato" Visible="false"></asp:BoundColumn>
                                        <asp:BoundColumn DataField="RLPC_abilitato" Visible="false"></asp:BoundColumn>
                                        <asp:BoundColumn DataField="RLPC_TPRL_ID" Visible="false"></asp:BoundColumn>
                                        <asp:BoundColumn DataField="CMNT_MaxIscritti" Visible="false"></asp:BoundColumn>
                                        <asp:BoundColumn DataField="ALCM_isChiusaForPadre" Visible="false"></asp:BoundColumn>
                                        <asp:BoundColumn DataField="CMNT_TPCM_id" Visible="false"></asp:BoundColumn>
                                        <asp:BoundColumn DataField="CMNT_CanSubscribe" Visible="false"></asp:BoundColumn>
                                        <asp:BoundColumn DataField="CMNT_CanUnsubscribe" Visible="false"></asp:BoundColumn>
                                        <asp:BoundColumn DataField="CMNT_MaxIscrittiOverList" Visible="false"></asp:BoundColumn>
                                        <asp:BoundColumn DataField="CMNT_Nome" Visible="false"></asp:BoundColumn>
                                        <asp:BoundColumn DataField="CMNT_Archiviata" Visible="false"></asp:BoundColumn>
                                        <asp:BoundColumn DataField="CMNT_Bloccata" Visible="false"></asp:BoundColumn>
                                        <asp:TemplateColumn ItemStyle-Width="30px" ItemStyle-CssClass="ROW_TD_Small_center">
                                            <HeaderTemplate>
                                                <input type="checkbox" id="SelectAll2" onclick="SelectAll(this);" runat="server"
                                                    name="SelectAll" />
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <input runat="server" type="checkbox" id="CBcorso" name="CBcorso" onclick="SelectMe(this);" />
                                            </ItemTemplate>
                                        </asp:TemplateColumn>
                                    </Columns>
                                </asp:DataGrid><br />
                                <asp:Label ID="LBmessageFind" runat="server" CssClass="avviso_normal" Visible="False"></asp:Label>
                            </asp:TableCell>
                        </asp:TableRow>
                    </asp:Table>
                </asp:Panel>
                <asp:Panel ID="PNLdettagli" runat="server" HorizontalAlign="Center" Visible="false">
                    <table width="700" align="center" border="0">
                        <tr>
                            <td align="center">
                                <fieldset>
                                    <legend class="tableLegend">
                                        <asp:Label ID="LBlegenda" runat="server" CssClass="tableLegend">Dettagli comunità</asp:Label>
                                    </legend>
                                    <input type="hidden" runat="server" id="HDNcmnt_ID" name="HDNcmnt_ID" />
                                    <input type="hidden" runat="server" id="HDNtprl_id" name="HDNtprl_id" />
                                    <input type="hidden" runat="server" id="HDNcmnt_Path" name="HDNcmnt_Path" />
                                    <input type="hidden" runat="server" id="HDNisChiusaForPadre" name="HDNisChiusaForPadre" />
                                    <asp:Label Visible="False" ID="LBtprl_id" runat="server"></asp:Label>
                                    <DETTAGLI:CTRLDettagli ID="CTRLDettagli" runat="server"></DETTAGLI:CTRLDettagli>
                                </fieldset>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                &nbsp;
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
                <asp:Panel ID="PNLmessaggi" runat="server" Visible="False">
                    <table cellspacing="0" cellpadding="0" align="center" border="0">
                        <tr>
                            <td height="30">
                                &nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="LBMessaggi" runat="server" CssClass="avviso"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td height="30">
                                &nbsp;
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
                <asp:Panel ID="PNLiscrizioneAvvenuta" runat="server" Visible="False">
                    <table cellspacing="0" cellpadding="0" align="center" border="0">
                        <tr>
                            <td height="30">
                                &nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="LBiscrizione" runat="server" CssClass="avviso"></asp:Label>
                                <br />
                                <br />
                            </td>
                        </tr>
                        <tr>
                            <td height="30">
                                &nbsp;
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
                <asp:Panel ID="PNLpermessi" runat="server" HorizontalAlign="Center" Visible="False">
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
                            <td valign="top" height="50">
                                &nbsp;
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
            </td>
        </tr>
    </table>
</asp:Content>
<%--<!DOCTYPE HTML PUBLIC "-//W3C//Dtd HTML 4.0 transitional//EN">
<html>
  <head id="Head1" runat="server">
		<title>Comunità On Line - Ricerca Comunità</title>
		
		<meta content="Microsoft Visual Studio.NET 7.0" name="GENERATOR"/>
		<meta content="Visual Basic 7.0" name="CODE_LANGUAGE"/>
		<meta content="JavaScript" name="vs_defaultClientScript"/>
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema"/>
		<link href="./../Styles.css" type="text/css" rel="stylesheet"/>
</head>

	<body onkeydown="return SubmitRicerca(event);">
		<form id="aspnetForm" method="post" runat="server">
		<asp:ScriptManager ID="SCMmanager" runat="server"></asp:ScriptManager>

        	<tr>
				<td colSpan="3"><HEADER:CtrLHEADER id="Intestazione" runat="server"></HEADER:CtrLHEADER></td>
			</tr>




					</td>
				</tr>
			</table>
			<FOOTER:CtrLFOOTER id="CtrLFooter" runat="server"></FOOTER:CtrLFOOTER>
		</form>
	</body>
</html>--%>