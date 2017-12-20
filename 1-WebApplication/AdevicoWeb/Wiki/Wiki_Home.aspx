<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Wiki_Home.aspx.vb"  MasterPageFile="~/AjaxPortal.Master" Inherits="Comunita_OnLine.Wiki_Home" %>

<%--<%@ Register TagPrefix="HEADER" TagName="CtrLHeader" Src="../uc/UC_Header.ascx" %>
<%@ Register TagPrefix="FOOTER" TagName="CtrLFooter" Src="../uc/UC_Footer.ascx" %>--%>
<%@ Register TagPrefix="rade" Namespace="Telerik.WebControls" Assembly="RadEditor.Net2" %>

<%@ MasterType VirtualPath="~/AjaxPortal.Master" %>




<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">

    <link href="../Graphics/Modules/Wiki/wiki.new.css?v=201508040900lm" rel="Stylesheet" />

    <link href="./../dhtmlcentral.css" rel="STYLESHEET" type="text/css" />

</asp:Content>



<asp:Content ID="Content2" ContentPlaceHolderID="CPHservice" runat="server">
    <div id="DVmenu" style="width: 900px; text-align:right;" align="center">
        <asp:Button ID="BTN_Homewiki" runat="server" CommandArgument="Homewiki" CommandName="Home wiki"
            CssClass="Link_Menu" Height="21px" Text="Home wiki" />
        <asp:Button ID="BTN_Lista" runat="server" CommandArgument="Sezione corrente" CommandName="Sezione corrente"
            CssClass="Link_Menu" Height="21px" Text="Sezione corrente" Visible="False" />
    </div>

    <div class="content">
    
        <asp:Panel ID="PNL_NoPermessi" runat="server">
            <asp:Label ID="LBL_NoPermessi" runat="server">
			    Non si dispone dei permessi necessari per visualizzare la pagina.
            </asp:Label>
        </asp:Panel>
        
        <asp:Panel ID="PNL_search" runat="server" BorderWidth="1" Width="100%" Style="text-align: right">
            <br />
            <asp:Label ID="LBL_ricerca" runat="server" Text="Cerca: " CssClass="Titolo_campo"></asp:Label>
                
            <asp:DropDownList ID="DDL_ricerca" runat="server">
                <asp:ListItem>(Topic)</asp:ListItem>
                <asp:ListItem>Inizia per</asp:ListItem>
                <asp:ListItem>Finisce per</asp:ListItem>
                <asp:ListItem>Contiene</asp:ListItem>
            </asp:DropDownList>
                
            <asp:TextBox ID="TXB_search" runat="server" CssClass="Testo_campoSmall" MaxLength="60">
            </asp:TextBox>
                
            <asp:Button ID="BTN_search" runat="server" CommandArgument="TXB_search.text" CommandName="Cerca"
                CssClass="PulsanteFiltro" Height="21px" Text="Cerca" />
            <br />
            <br />
        </asp:Panel>
        
        <br />
        <asp:Panel ID="PNL_Wiki" runat="server">
            <div class="ContenitoreWiki">
                <div class="NavigatoreWiki">
                    <asp:Panel ID="PNL_NavigatoreNoTopic" runat="server">
                        <div class="TestoInfo">
                            <asp:Label ID="LBL_Nav_NoSezione" runat="server">
                                Nessuna sezione presente
                            </asp:Label>
                        </div>
                    </asp:Panel>
                    <asp:Panel ID="PNL_Navigatore" runat="server" Width="190" Height="400" BorderWidth="1">
                        <br />
                        <asp:Label ID="LBL_Navigatore_t" runat="server" CssClass="Titolo_campo">
                            Elenco sezioni
                        </asp:Label>
                        <br />
                        <br />
                        <div style="display: block; overflow: auto; height: 350px">
                            <asp:Repeater ID="RPT_LinkNavigatore" runat="server">
                                <ItemTemplate>
                                    <asp:LinkButton ID="LNK_VoceNavi" runat="server" CssClass="LinkHome"></asp:LinkButton>
                                    <br />
                                    <br />
                                </ItemTemplate>
                            </asp:Repeater>
                        </div>
                    </asp:Panel>
                </div>
                <div class="ContenutoWiki">
                    <asp:Panel runat="server" ID="PNL_NoSezioni">
                        <div class="TestoInfo">
                            <asp:Label ID="LBL_Con_SezioneNO" runat="server" CssClass="Testo_campoSmall">
                                Nessuna sezione presente
                            </asp:Label>
                        </div>
                    </asp:Panel>
                    <asp:Panel ID="PNL_NoTopic" runat="server">
                        <div class="TestoInfo">
                            <asp:Label ID="LBL_NoTopic_t" runat="server" CssClass="Testo_campoSmall">
						            Nessun Topic presente
                            </asp:Label>
                            <br />
                        </div>
                    </asp:Panel>
                    <asp:Panel ID="PNL_TopicElenco" runat="server">
                        <div class="DatiSezione">
                            Sezione:
                            <asp:Label ID="LBL_SezioneAttuale" runat="server" CssClass="Titolo_campoSmall" Text="Label"></asp:Label>
                            <br />
                            <br />
                            <asp:Label ID="LBL_ElencoSezioneDescrizioneData_t" runat="server" CssClass="Testo_campoSmall">
                                Modificata il: 
                            </asp:Label>
                            <asp:Label ID="LBL_ElencoSezioneDescrizioneData" runat="server" CssClass="Titolo_campoSmall">
                            </asp:Label>
                            <asp:Label ID="LBL_ElencoSezionePersona_t" runat="server" CssClass="Testo_campoSmall">
                                da 
                            </asp:Label>
                            <asp:Label ID="LBL_ElencoSezionePersona" runat="server" CssClass="Titolo_campoSmall">
                            </asp:Label>
                            -
                            <asp:Label ID="LBL_ElencoSezioneComunita_t" runat="server" CssClass="Testo_campoSmall">
                                Appartenente alla comunità di:
                            </asp:Label>
                            &nbsp;
                            <asp:Label ID="LBL_ElencoSezioneComunita" runat="server" CssClass="Titolo_campoSmall">
                            </asp:Label>
                            <br />
                            <br />
                        </div>
                        <div class="LinkTopic">
                        </div>
                        <asp:DataList CssClass="DataGrid_Generica" ID="DLS_topics" runat="server" CellPadding="4"
                            DataKeyField="id" Width="100%">
                            <%--ForeColor="#333333"--%>
                            <%--<FooterStyle
                                            BackColor="#507CD1"
                                            Font-Bold="True"
                                            ForeColor="White" />--%>
                            <%--<SelectedItemStyle
                                            BackColor="#D1DDF1"
                                            Font-Bold="True"
                                            ForeColor="#333333" />--%>
                            <%--<AlternatingItemStyle BackColor="#EFF3FB" />--%>
                            <AlternatingItemStyle CssClass="ROW_Alternate_Small"></AlternatingItemStyle>
                            <HeaderStyle CssClass="ROW_header_Small_Center"></HeaderStyle>
                            <ItemStyle CssClass="ROW_Normal_Small" Height="22px"></ItemStyle>
                            <SelectedItemStyle CssClass="ROW_Evidenziate_Small" />
                            <%--<ItemStyle BackColor="White" />--%>
                            <%-- <HeaderStyle 
                                            BackColor="#507CD1"
                                            Font-Bold="True"
                                            ForeColor="White" />--%>
                            <HeaderTemplate>
                                <table>
                                    <tr>
                                        <td width="15px">
                                        </td>
                                        <td width="28px">
                                        </td>
                                        <td width="250px">
                                            <asp:Label ID="LBL_intest1" runat="server" ForeColor="White" Text="Voce"></asp:Label>
                                        </td>
                                        <td width="180px">
                                            <asp:Label ID="Label1" runat="server" ForeColor="White" Text="Autore"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:Label ID="Label2" runat="server" ForeColor="White" Text="Ultima modifica"></asp:Label>
                                        </td>
                                    </tr>
                                </table>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <table>
                                    <asp:Label runat="server" ID="LBL_separatore" Text="<tr><td colspan='6' ><hr/></td></tr>"
                                        Visible="fALSE" />
                                    <tr>
                                        <td width="15px">
                                            <asp:Label runat="server" ID="LBL_Iniziale" Text="A" Font-Size="Medium" Visible="true" />
                                        </td>
                                        <td width="28px">
                                            <%--<a href="<%=Me.BaseUrl %>wiki/wiki_Home.aspx?id=<%#DataBinder.Eval(Container.DataItem, "ID")%>"><img src="../images/search.gif" alt="Visualizza voce:<%#DataBinder.Eval(Container.DataItem, "Nome")%>"/> </a>--%>
                                        </td>
                                        <td width="250px">
                                            <span style="left" class="Titolo_Campo" style="text-align: left;"><a href="<%=Me.BaseUrl %>wiki/wiki_Home.aspx?id=<%#DataBinder.Eval(Container.DataItem, "ID")%>">
                                                <asp:Label runat="server" ID="LBL_topic" Text='<%#DataBinder.Eval(Container.DataItem, "Nome")%>' />
                                            </a></span>
                                            <td />
                                            <td width="180px">
                                                <span width="right" class="Testo_Campo" style="text-align: left;">
                                                    <%#DataBinder.Eval(Container.DataItem, "Persona.anagrafica")%>
                                                </span>
                                            </td>
                                            <td width="180px">
                                                <span width="right" class="Testo_Campo" style="text-align: left;">
                                                    <%#DataBinder.Eval(Container.DataItem, "DataModifica".toString)%>
                                                </span>
                                            </td>
                                    </tr>
                                </table>
                            </ItemTemplate>
                        </asp:DataList>
                    </asp:Panel>
                    <asp:Panel ID="PNL_TopicView" runat="server" Visible="False">
                        <table width="100%">
                        <tr>
                            <td>
                                <asp:Label ID="LBL_TitoloTopicView" runat="server" Text="" class="Titolo_Campo"></asp:Label>
                                <hr />
                                Autore:
                                <asp:Label ID="LBL_autore" runat="server" Text="LBL_autore"></asp:Label>
                                &nbsp;<br />
                                <br />
                                <br />
                                <br />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <div class="renderedtext">
                                    <asp:Label ID="LBL_TestView" runat="server" Text="TESTO DI PROVA"></asp:Label>
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td align="right">
                                &nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td align="right">
                                &nbsp;
                            </td>
                        </tr>
                    </table>
                    </asp:Panel>
                    <asp:Panel ID="PNL_ResultSearch" runat="server" Visible="False" Width="100%">
                        <asp:Label ID="LBL_RisultatoRicerca" runat="server" CssClass="Titolo_campo">
                            Elenco dei topic trovati:
                        </asp:Label>
                        <br />
                        <asp:DataList ID="DLS_result" runat="server" CellPadding="4" DataKeyField="id" Width="100%"
                            CssClass="DataGrid_Generica">
                            <AlternatingItemStyle CssClass="ROW_Alternate_Small"></AlternatingItemStyle>
                            <HeaderStyle CssClass="ROW_header_Small_Center"></HeaderStyle>
                            <ItemStyle CssClass="ROW_Normal_Small" Height="22px"></ItemStyle>
                            <SelectedItemStyle CssClass="ROW_Evidenziate_Small" />
                            <HeaderTemplate>
                                <table>
                                    <tr>
                                        <td width="28px">
                                        </td>
                                        <td width="250px">
                                            <asp:Label ID="LBL_intest1" runat="server" ForeColor="White" Text="Voce"></asp:Label>
                                        </td>
                                        <td width="180px">
                                            <asp:Label ID="Label1" runat="server" ForeColor="White" Text="Sezione"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:Label ID="Label2" runat="server" ForeColor="White" Text="Comunità"></asp:Label>
                                        </td>
                                    </tr>
                                </table>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <table>
                                    <tr>
                                        <td width="28px">
                                            <a href="<%=Me.BaseUrl %>wiki/wiki_Home.aspx?id=<%#DataBinder.Eval(Container.DataItem, "ID")%>">
                                                <img src="../images/search.gif" alt="Visualizza voce:<%#DataBinder.Eval(Container.DataItem, "Nome")%>" />
                                            </a>
                                        </td>
                                        <td width="250px">
                                            <span style="left" class="Titolo_Campo" style="text-align: left;"><a href="<%=Me.BaseUrl %>wiki/wiki_Home.aspx?id=<%#DataBinder.Eval(Container.DataItem, "ID")%>">
                                                <%#DataBinder.Eval(Container.DataItem, "Nome")%></a> </span>
                                            <td />
                                            <td width="180px">
                                                <span width="right" class="Testo_Campo" style="text-align: left;">
                                                    <%#DataBinder.Eval(Container.DataItem, "Sezione.NomeSezione")%>
                                                </span>
                                            </td>
                                            <td>
                                                <span width="right" class="Testo_Campo" style="text-align: left;">
                                                    <%#DataBinder.Eval(Container.DataItem, "Comunita")%>
                                                </span>
                                            </td>
                                    </tr>
                                </table>
                            </ItemTemplate>
                        </asp:DataList>
                    </asp:Panel>
                </div>
            </div>
        </asp:Panel>
    </div>
</asp:Content>