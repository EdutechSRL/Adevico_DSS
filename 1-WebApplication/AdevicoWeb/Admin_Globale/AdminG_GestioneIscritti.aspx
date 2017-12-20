<%@ Page Language="vb" AutoEventWireup="false" MasterPageFile="~/Admin_Globale/AdminPortal.Master"
    CodeBehind="AdminG_GestioneIscritti.aspx.vb" Inherits="Comunita_OnLine.AdminG_GestioneIscritti" %>
<%@ MasterType VirtualPath="~/Admin_Globale/AdminPortal.Master" %>
<%--OLD radtabstrip TELERIK--%>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<asp:Content ContentPlaceHolderID="HeadContent" runat="server" ID="Content1">
    <script type="text/javascript" language="Javascript" src="./../jscript/generali.js"></script>
    <script type="text/javascript" language="javascript">
    	function SelectMe(Me) {
    		var HIDcheckbox;
    		//eval('HIDcheckbox= this.document.forms[0].HDazione')
    		eval('HIDcheckbox=<%=Me.HDazione.ClientID%>')
            HIDcheckbox = this.document.getElementById('<%=Me.HDazione.ClientID%>');
    		for (i = 0; i < document.forms[0].length; i++) {
    		    e = document.forms[0].elements[i];
    		    if (e.type == 'checkbox' && e.name.indexOf("CBazione") != -1) {
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

    	function UserForCancella() {
    	    //if (document.forms[0].HDazione.value == "," || document.forms[0].HDazione.value == "")
    	    if (<%=Me.HDazione.ClientID%>.value == "," || <%=Me.HDazione.ClientID%>.value == "")
    		    return false;
    		else
    		    return confirm('Sei sicuro di cancellare l\'iscrizione degli utenti selezionati?');
    	}

    	//Indica se è stato selezionato almeni un utente !!
    	function UserSelezionati() {
    		if (<%=Me.HDazione.ClientID%>.value == "," || <%=Me.HDazione.ClientID%>.value == "")
    		    return false;
    		else
    		    return true;
    	}
    	function SelectAll(SelectAllBox) {
    		var actVar = SelectAllBox.checked;
    		var TBcheckbox;
    		//eval('HDazione= this.document.forms[0].HDazione')
            //eval('HDazione=<%=Me.HDazione.ClientID%>')
            HDazione = this.document.getElementById('<%=Me.HDazione.ClientID%>');
    		HDazione.value = ""
    		for (i = 0; i < document.forms[0].length; i++) {
    		    e = document.forms[0].elements[i];
    		    if (e.type == 'checkbox' && e.name.indexOf("CBazione") != -1) {
    		        e.checked = actVar;
    		        if (e.checked == true)
    		            if (HDazione.value == "")
    		                HDazione.value = ',' + e.value + ','
    		            else
    		                HDazione.value = HDazione.value + e.value + ','
    		        }
    		    }
    		}
	
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="CPHservice" runat="server">
    <table width="900px" align="center">
        <tr>
            <td bgcolor="#a3b2cd" height="1">
            </td>
        </tr>
        <tr>
            <td class="titolo" align="center">
                <asp:Label ID="LBtitolo" CssClass="TitoloServizio" runat="server">- Gestione Iscritti Comunita -</asp:Label>
            </td>
        </tr>
        <tr>
            <td bgcolor="#a3b2cd" height="1">
            </td>
        </tr>
        <tr>
            <td align="center">
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
                                <asp:LinkButton ID="LNBnascondi" runat="server">Indietro</asp:LinkButton>&nbsp;
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
                <asp:Panel ID="PNLnoquery" runat="server" Visible="False">
                    <table align="center">
                        <tr>
                            <td height="50" colspan="2">
                                &nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td align="center" colspan="2">
                                <asp:Label ID="LBnoquery" CssClass="messaggio" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td height="50">
                                &nbsp;<asp:Button CssClass="pulsante" ID="BTNlistacmnt" Text="Lista Comunità" runat="server">
                                </asp:Button>
                            </td>
                            <td height="50">
                                &nbsp;<asp:Button CssClass="pulsante" ID="BTNricercacmnt" Text="Ricerca per Persona"
                                    runat="server"></asp:Button>
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
                <asp:Panel ID="PNLcontenuto" runat="server" HorizontalAlign="Center">
                    <asp:Panel ID="PNLiscritti" runat="server" Visible="true" HorizontalAlign="Center">
                        <br />
                        <table border="0" width="100%" align="center">
                            <tr>
                                <td align="center" colspan="2">
                                    <table align="center" width="100%" cellpadding="0" cellspacing="0">
                                        <tr>
                                            <td class="NoborderBottom" height="32px">
                                                <telerik:RadTabStrip ID="TBSmenu" runat="server" Align="Justify" Width="100%" Height="26px"
                                                    SelectedIndex="0" CausesValidation="false" AutoPostBack="true" Skin="Outlook"
                                                    EnableEmbeddedSkins="true">
                                                    <Tabs>
                                                        <telerik:RadTab Text="Elenco Comunita" Value="TABelencoComunita" runat="server"/>
                                                        <telerik:RadTab Text="Ultimi Iscritti" Value="TABlast" runat="server"/>
                                                        <telerik:RadTab Text="Tutti" Value="TABtutti"  runat="server"/>
                                                        <telerik:RadTab Text="Abilitati" Value="TABabilitati" runat="server"/>
                                                        <telerik:RadTab Text="Bloccati" Value="TABbloccati" runat="server"/>
                                                        <telerik:RadTab Text="In attesa conferma" Value="TABinAttesa" runat="server"/>
                                                        <telerik:RadTab Text="Aggiungi Utente" Value="TABaggiungi" runat="server"/>
                                                    </Tabs>
                                                </telerik:RadTabStrip>	
                                            </td>
                                            <td class="borderTop" height="31px">
                                                <asp:ImageButton ImageUrl="./../images/print_big.gif" runat="server" ID="IMBstampa">
                                                </asp:ImageButton>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td align="center">
                                    <asp:Panel ID="PNLfiltro" runat="server">
                                        <fieldset>
                                            <legend class="tableLegend">Filtro</legend>
                                            <br />
                                            <table cellspacing="1" cellpadding="1" width="100%" border="0">
                                                <tr>
                                                    <td class="FiltroVoce">
                                                        Tipo Ruolo:
                                                    </td>
                                                    <td class="FiltroVoce">
                                                        Numero Record
                                                    </td>
                                                    <td class="FiltroVoce">
                                                        Tipo Ricerca
                                                    </td>
                                                    <td class="FiltroVoce">
                                                        Valore
                                                    </td>
                                                    <td class="FiltroVoce">
                                                        &nbsp;
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:DropDownList ID="DDLTipoRuolo" runat="server" CssClass="FiltroCampo" AutoPostBack="true">
                                                        </asp:DropDownList>
                                                    </td>
                                                    <td>
                                                        <asp:DropDownList ID="DDLNumeroRecord" CssClass="FiltroCampo" runat="server" AutoPostBack="true">
                                                            <asp:ListItem Value="20" Selected="true"></asp:ListItem>
                                                            <asp:ListItem Value="40"></asp:ListItem>
                                                            <asp:ListItem Value="80"></asp:ListItem>
                                                        </asp:DropDownList>
                                                    </td>
                                                    <td>
                                                        <asp:DropDownList ID="DDLTipoRicerca" CssClass="FiltroCampo" runat="server" AutoPostBack="false">
                                                            <asp:ListItem Selected="true" Value="-2">Nome</asp:ListItem>
                                                            <asp:ListItem Value="-3">Cognome</asp:ListItem>
                                                            <asp:ListItem Value="-4">Nome/Cognome</asp:ListItem>
                                                            <asp:ListItem Value="-5">Data di nascita(gg/mm/aaaa)</asp:ListItem>
                                                        </asp:DropDownList>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="TXBValore" CssClass="FiltroCampo" runat="server" MaxLength="50"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <asp:Button ID="BTNCerca" CssClass="pulsante" runat="server" Text="Cerca"></asp:Button>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td colspan="5">
                                                        <table width="60%" align="center">
                                                            <tr>
                                                                <td align="center">
                                                                    <asp:LinkButton ID="LKBtutti" runat="server" CssClass="lettera" CommandArgument="-1"
                                                                        OnClick="FiltroLinkLettere_Click">Tutti</asp:LinkButton>
                                                                </td>
                                                                <td align="center">
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
                                                </tr>
                                            </table>
                                        </fieldset>
                                    </asp:Panel>
                                </td>
                            </tr>
                        </table>
                        <table cellspacing="1" cellpadding="1" width="100%" border="0">
                            <tr>
                                <td align="center">
                                    <input id="HDazione" type="hidden" name="HDazione" runat="server" />
                                    <asp:DataGrid ID="DGiscritti" runat="server" AllowSorting="true" ShowFooter="false"
                                        OnPageIndexChanged="DGiscritti_pageindexchanged" AutoGenerateColumns="False"
                                        AllowPaging="true" DataKeyField="RLPC_ID" PageSize="20" AllowCustomPaging="True"
                                        CssClass="DataGrid_Generica">
                                        <AlternatingItemStyle CssClass="Righe_Alternate_Center"></AlternatingItemStyle>
                                        <HeaderStyle CssClass="Riga_Header"></HeaderStyle>
                                        <ItemStyle CssClass="Righe_Normali_center" Height="22px"></ItemStyle>
                                        <PagerStyle CssClass="Riga_Paginazione" Position="Bottom" Mode="NumericPages" Visible="true"
                                            HorizontalAlign="Right" Height="25px" VerticalAlign="Bottom"></PagerStyle>
                                        <Columns>
                                            <asp:TemplateColumn ItemStyle-Width="20px" ItemStyle-HorizontalAlign="Center">
                                                <HeaderTemplate>
                                                    <input type="checkbox" id="SelectAll" name="SelectAll" onclick="SelectAll(this);"
                                                        runat="server" />
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <input type="checkbox" value='<%# DataBinder.Eval(Container.DataItem, "PRSN_ID") %>'
                                                        id="CBazione" name="CBazione" <%# DataBinder.Eval(Container.DataItem, "oCheck") %>
                                                        onclick="SelectMe(this);">
                                                </ItemTemplate>
                                            </asp:TemplateColumn>
                                            <asp:BoundColumn DataField="RLPC_ID" HeaderText="RLPC" Visible="false"></asp:BoundColumn>
                                            <asp:TemplateColumn runat="server" HeaderText="" ItemStyle-HorizontalAlign="Center"
                                                ItemStyle-Width="10">
                                                <ItemTemplate>
                                                    <asp:ImageButton ID="IMBCancella" runat="server" CausesValidation="False" CommandName="deiscrivi"
                                                        ImageUrl="../images/x.gif"></asp:ImageButton>
                                                </ItemTemplate>
                                            </asp:TemplateColumn>
                                            <asp:TemplateColumn runat="server" HeaderText="" ItemStyle-HorizontalAlign="Center"
                                                ItemStyle-Width="10">
                                                <ItemTemplate>
                                                    <asp:ImageButton ID="IMBmodifica" runat="server" CausesValidation="False" CommandName="modifica"
                                                        ImageUrl="../images/m.gif"></asp:ImageButton>
                                                </ItemTemplate>
                                            </asp:TemplateColumn>
                                            <asp:TemplateColumn runat="server" HeaderText="" ItemStyle-HorizontalAlign="Center"
                                                ItemStyle-Width="10">
                                                <ItemTemplate>
                                                    <asp:ImageButton ID="IMBinfo" runat="server" CausesValidation="False" CommandName="infoPersona"
                                                        ImageUrl="../images/proprieta.gif"></asp:ImageButton>
                                                </ItemTemplate>
                                            </asp:TemplateColumn>
                                            <asp:BoundColumn DataField="PRSN_Anagrafica" HeaderText="Anagrafica" SortExpression="PRSN_Anagrafica">
                                            </asp:BoundColumn>
                                            <asp:BoundColumn DataField="PRSN_login" HeaderText="Login" SortExpression="PRSN_login">
                                            </asp:BoundColumn>
                                            <asp:BoundColumn DataField="TPRL_id" HeaderText="idRuolo" SortExpression="TPRL_id"
                                                Visible="False"></asp:BoundColumn>
                                            <asp:BoundColumn DataField="TPRL_nome" HeaderText="Ruolo" SortExpression="TPRL_nome"
                                                ItemStyle-Width="60px"></asp:BoundColumn>
                                            <asp:BoundColumn DataField="PRSN_TPPR_id" HeaderText="idtipopersona" SortExpression="PRSN_TPPR_id"
                                                Visible="False"></asp:BoundColumn>
                                            <asp:BoundColumn DataField="RLPC_ultimoCollegamento" HeaderText="Ultimo Collegamento"
                                                SortExpression="RLPC_ultimoCollegamento" Visible="true" ItemStyle-Width="150">
                                            </asp:BoundColumn>
                                            <asp:BoundColumn DataField="TPRL_gerarchia" Visible="false"></asp:BoundColumn>
                                            <asp:BoundColumn DataField="PRSN_ID" Visible="false"></asp:BoundColumn>
                                            <asp:BoundColumn DataField="RLPC_Attivato" Visible="false"></asp:BoundColumn>
                                            <asp:BoundColumn DataField="RLPC_Abilitato" Visible="false"></asp:BoundColumn>
                                            <asp:BoundColumn DataField="RLPC_Responsabile" Visible="false"></asp:BoundColumn>
                                            <asp:BoundColumn DataField="oIscrittoIl" HeaderText="Iscritto il" SortExpression="RLPC_IscrittoIl"
                                                Visible="false" ItemStyle-HorizontalAlign="Center"></asp:BoundColumn>
                                        </Columns>
                                        <PagerStyle Width="600px" PageButtonCount="5" Mode="NumericPages"></PagerStyle>
                                    </asp:DataGrid><br />
                                    <asp:Label ID="LBnoIscritti" Visible="False" runat="server" CssClass="avviso"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td align="center">
                                    <asp:Panel ID="PNLmessaggio" Visible="False" runat="server">
                                        <p align="center">
                                            <asp:Label ID="LBmessaggio" runat="server"></asp:Label><br />
                                            <asp:Button ID="BTNok" runat="server" Text="ok" CssClass="pulsante"></asp:Button></p>
                                    </asp:Panel>
                                </td>
                            </tr>
                            <tr>
                                <td align="center">
                                    <asp:Label ID="LBseleziona" runat="server" CssClass="Filtro">Selezionati:</asp:Label>
                                    <asp:LinkButton ID="LKBabilita" Visible="False" runat="server" Text="-Abilita-"></asp:LinkButton>
                                    <asp:LinkButton ID="LKBdisabilita" Visible="False" runat="server" Text="-Disabilita-"></asp:LinkButton>
                                    <asp:LinkButton ID="LKBelimina" Visible="False" runat="server" Text="-Elimina-"></asp:LinkButton>
                                    <asp:LinkButton ID="LKBcancellaInAttesa" Visible="False" runat="server" Text="-Elimina in attesa-"></asp:LinkButton>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Button ID="BTNindietro" runat="server" Text="Indietro" CssClass="pulsante">
                                    </asp:Button>
                                </td>
                            </tr>
                        </table>
                    </asp:Panel>
                    <asp:Panel ID="PNLmodifica" runat="server" Visible="False">
                        <table border="0" align="center">
                            <tr>
                                <td align="center">
                                    <fieldset>
                                        <legend class="tableLegend">Cambia il ruolo</legend>
                                        <br />
                                        <input id="HDrlpc" type="hidden" name="HDrlpc" runat="server" />
                                        <input id="HDNprsnID" type="hidden" name="HDNprsnID" runat="server" />
                                        <input id="HDNrlpc_Attivato" type="hidden" name="HDNrlpc_Attivato" runat="server" />
                                        <input id="HDNrlpc_Abilitato" type="hidden" name="HDNrlpc_Abilitato" runat="server" />
                                        <input id="HDNrlpc_Responsabile" type="hidden" name="HDNrlpc_Responsabile" runat="server" />
                                        <table border="0" align="center">
                                            <tr>
                                                <td colspan="4" height="10px" class="nosize0">
                                                    &nbsp;
                                                </td>
                                            </tr>
                                            <tr>
                                                <td width="5px" class="nosize0">
                                                    &nbsp;
                                                </td>
                                                <td>
                                                    <asp:Label ID="LBanagrafica_t" CssClass="Titolo_campo" runat="server">Anagrafica:&nbsp;</asp:Label>
                                                </td>
                                                <td>
                                                    <asp:Label ID="LBNomeCognome" runat="server" CssClass="Testo_campo"></asp:Label>
                                                </td>
                                                <td width="5px" class="nosize0">
                                                    &nbsp;
                                                </td>
                                            </tr>
                                            <tr>
                                                <td width="5px" class="nosize0">
                                                    &nbsp;
                                                </td>
                                                <td>
                                                    <asp:Label ID="LBruolo_t" CssClass="Titolo_campo" runat="server">Ruolo:&nbsp;</asp:Label>
                                                </td>
                                                <td>
                                                    <asp:DropDownList ID="DDLruolo" runat="server" CssClass="Testo_campo">
                                                    </asp:DropDownList>
                                                </td>
                                                <td width="5px" class="nosize0">
                                                    &nbsp;
                                                </td>
                                            </tr>
                                            <tr>
                                                <td width="5px" class="nosize0">
                                                    &nbsp;
                                                </td>
                                                <td>
                                                    <asp:Label ID="LBresponsabile_t" CssClass="Titolo_campo" runat="server">Responsabile:&nbsp;</asp:Label>
                                                </td>
                                                <td>
                                                    <asp:CheckBox ID="CHBresponsabile" runat="server" Text="Si" CssClass="Testo_campo">
                                                    </asp:CheckBox>
                                                </td>
                                                <td width="5px" class="nosize0">
                                                    &nbsp;
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="4">
                                                    &nbsp;
                                                </td>
                                            </tr>
                                            <tr align="center">
                                                <td width="5px" class="nosize0">
                                                    &nbsp;
                                                </td>
                                                <td align="left">
                                                    <asp:Button ID="BTNannulla" runat="server" Text="Annulla" CssClass="pulsante"></asp:Button>
                                                </td>
                                                <td align="right">
                                                    <asp:Button ID="BTNmodifica" runat="server" Text="Modifica" CssClass="pulsante">
                                                    </asp:Button>
                                                </td>
                                                <td width="5px" class="nosize0">
                                                    &nbsp;
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="4" height="10px" class="nosize0">
                                                    &nbsp;
                                                </td>
                                            </tr>
                                        </table>
                                        <br />
                                    </fieldset>
                                </td>
                            </tr>
                        </table>
                    </asp:Panel>
                </asp:Panel>
                <asp:Panel ID="PNLdeiscrivi" Visible="False" runat="server">
                    <input type="hidden" id="HDNcmnt_ID" runat="server" name="HDNcmnt_ID" />
                    <input type="hidden" id="HDNprsn_Id" runat="server" name="HDNprsn_Id" />
                    <input type="hidden" id="HDNcmnt_Path" runat="server" name="HDNcmnt_Path" />
                    <table cellspacing="0" cellpadding="0" align="center" border="0" width="600px">
                        <tr>
                            <td height="50" colspan="3">
                                &nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td colspan="3">
                                <asp:Label ID="LBinfoDeIscrivi" runat="server" CssClass="confirmDelete"></asp:Label>
                                <br />
                                <br />
                                <fieldset>
                                    <legend>Lista</legend>
                                    <br />
                                    <asp:Table ID="TBLcomunita" runat="server" HorizontalAlign="Center" Width="550px">
                                    </asp:Table>
                                    <br />
                                </fieldset>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="3">
                                &nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Button ID="BTNannullaDeiscrizione" Text="Annulla" CssClass="Pulsante" CausesValidation="False"
                                    runat="server"></asp:Button>
                            </td>
                            <td colspan="2" align="right">
                                &nbsp;
                                <asp:Button ID="BTNdeIscriviCorrente" Text="Solo selezionata" CausesValidation="False"
                                    runat="server" CssClass="Pulsante"></asp:Button>
                                &nbsp; &nbsp;
                                <asp:Button ID="BTNdeIscriviTutte" Text="DeIscrivi a tutte" CausesValidation="False"
                                    runat="server" CssClass="Pulsante"></asp:Button>
                                &nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td height="30" colspan="3">
                                &nbsp;
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
                <asp:Panel ID="PNLdeiscriviMultiplo" Visible="False" runat="server">
                    <input type="hidden" id="HDNelencoID" runat="server" name="HDNelencoID" />
                    <table cellspacing="0" cellpadding="0" align="center" border="0" width="600px">
                        <tr>
                            <td height="50" colspan="3">
                                &nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td colspan="3">
                                <asp:Label ID="LBinfoDeIscrivi_multiplo" runat="server" CssClass="confirmDelete"></asp:Label>
                                <br />
                                <br />
                            </td>
                        </tr>
                        <tr>
                            <td colspan="3">
                                &nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Button ID="BTNannullaDeiscrizione_multi" Text="Annulla" CssClass="Pulsante"
                                    CausesValidation="False" runat="server"></asp:Button>
                            </td>
                            <td colspan="2" align="right">
                                &nbsp;
                                <asp:Button ID="BTNdeIscriviCorrente_multi" Text="Solo selezionata" CausesValidation="False"
                                    runat="server" CssClass="Pulsante"></asp:Button>
                                &nbsp; &nbsp;
                                <asp:Button ID="BTNdeIscriviTutte_multi" Text="DeIscrivi a tutte" CausesValidation="False"
                                    runat="server" CssClass="Pulsante"></asp:Button>
                                &nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td height="30" colspan="3">
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
<HTML>
  <head runat="server">
		<title>Comunità On Line - Gestione Iscritti Comunita</title>

		<meta content="Microsoft Visual Studio.NET 7.0" name="GENERATOR"/>
		<meta content="Visual Basic 7.0" name="CODE_LANGUAGE"/>
		<meta content="JavaScript" name="vs_defaultClientScript"/>
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema"/>
		<LINK href="./../Styles.css" type="text/css" rel="stylesheet"/>

</HEAD>
	<body>
		<form id="aspnetForm" method="post" encType="multipart/form-data" runat="server">
		<asp:ScriptManager ID="SCMmanager" runat="server"></asp:ScriptManager>
			<table cellSpacing="0" cellPadding="0" width="780" align="center" border="0">
				<tr>
					<td colSpan="3"><HEADER:CtrLHEADER id="Intestazione" runat="server"></HEADER:CtrLHEADER></td>
				</tr>
				<tr>
					<td colSpan="3">

					</td>
				</tr>
			</table>
			<FOOTER:CTRLFOOTER id="Piede" runat="server"></FOOTER:CTRLFOOTER>
		</form>
	</body>
</HTML>--%>