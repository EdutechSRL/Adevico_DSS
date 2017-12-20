<%@ Page Language="vb" AutoEventWireup="false" MasterPageFile="~/AjaxPortal.Master"
    CodeBehind="QuestionarioEdit.aspx.vb" Inherits="Comunita_OnLine.QuestionarioEdit" %>
<%@ Register TagPrefix="CTRL" TagName="Messages" Src="~/Modules/Common/UC/UC_ActionMessages.ascx" %>

<%@ MasterType VirtualPath="~/AjaxPortal.Master" %>
<%@ Reference Control="UserControls/ucDomandaMultiplaEdit.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="Server">
   <link media="screen" href="stile.css?v=201604071200lm" type="text/css" rel="StyleSheet" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="CPHservice" runat="Server">
    <asp:Panel ID="PNLmenu" runat="server" Width="100%" HorizontalAlign="right">
        <asp:LinkButton ID="LNBCartellaPrincipale" Visible="true" runat="server" CssClass="Link_Menu"
            CausesValidation="false"></asp:LinkButton>&nbsp;
        <asp:LinkButton ID="LNBGestioneQuestionario" Visible="true" runat="server" CssClass="Link_Menu"
            CausesValidation="false"></asp:LinkButton>
        <asp:LinkButton ID="LNBAggiungiPagina" Visible="true" runat="server" CssClass="Link_Menu"></asp:LinkButton>
        <div class="DIVHelp">
            <asp:ImageButton ID="IMBHelp" runat="server" ImageUrl="img/Help20px.png" Style="margin-top: 5px;
                float: right;" />
            <asp:Label runat="server" ID="LBHelp" Style="margin-top: 7px; float: right;"></asp:Label>
        </div>
    </asp:Panel>
    
    <asp:MultiView runat="server" ID="MLVquestionari">
        <asp:View ID="VIWdati" runat="server">
            <asp:Panel ID="PNLElenco" runat="server">
                <h2><asp:Label ID="LBTitolo" runat="server" Text="" class="NomePagina"></asp:Label></h2>
                <CTRL:Messages ID="CTRLmessages"  runat="server" Visible="false" />
                <br />
                <asp:DataList ID="DLPagine" runat="server" DataKeyField="id" CellPadding="4" ForeColor="#333333"
                    Width="100%" OnItemCommand="DLPagineEditCommand">
                    <ItemTemplate>
                        <b>
                            <%#Eval("nomePagina")%>
                        </b>
                        <asp:Literal ID="LTpageNumber" runat="server" Text='<%#Container.DataItem.NumeroPagina %>' Visible="false"></asp:Literal>
                        <asp:ImageButton ID="IMBPagina" runat="server" ImageUrl="img/modifica-documento.gif"
                            CommandName="paginaEdit"></asp:ImageButton>
                        <asp:ImageButton ID="IMBEliminaPag" runat="server" ImageUrl="img/elimina.gif" CommandName="elimina"
                            AlternateText="" Visible='<%#isAperto%>'></asp:ImageButton>
                        <br />
                        <%#Eval("descrizione")%>
                        <br />
                        <div class="quizactions">
                            <asp:Label runat="server" ID="LBScegliAzione"></asp:Label>
                            <asp:DropDownList runat="server" ID="DDLAzioni">
                                <asp:ListItem Text="" Value="0"></asp:ListItem>
                                <asp:ListItem Text="" Value="1"></asp:ListItem>
                                <asp:ListItem Text="" Value="2"></asp:ListItem>
                                <asp:ListItem Text="" Value="3"></asp:ListItem>
                                <asp:ListItem Text="" Value="4"></asp:ListItem>
                                <asp:ListItem Text="" Value="5"></asp:ListItem>
                            </asp:DropDownList>
                            <asp:LinkButton runat="server" ID="LNBConfermaAzione" CssClass="Link_Menu" CommandName="ConfermaAzione" />
                        </div>
                        <div class="quizactions" runat="server" id="DVcopyActions" Visible='<%#isCopyVisible%>'>
                            <asp:Label runat="server" ID="LBCopia"></asp:Label>
                            <asp:DropDownList runat="server" ID="DDLQuestionariDestinazione" DataTextField="nome" DataValueField="id"></asp:DropDownList>
                            <asp:LinkButton runat="server" ID="LNBCopiaDomande" Text="Copy" CssClass="Link_Menu" CommandName="CopiaDomande" />
                        </div>
                        <div class="quizactions" runat="server" id="DVdeleteActions" Visible='<%#isDeleteVisible%>'>
                            <asp:Label runat="server" ID="LBMessaggioElimina"></asp:Label>
                            <asp:LinkButton runat="server" ID="LNBEliminaSi" CssClass="Link_Menu" CommandName="ConfermaElimina" />
                            <asp:LinkButton runat="server" ID="LNBEliminaNo" CssClass="Link_Menu" CommandName="AnnullaElimina" />
                        </div>
                        <asp:RadioButtonList runat="server" ID="RBLfiltraDomande" AutoPostBack="true" RepeatDirection="Horizontal"
                            OnSelectedIndexChanged="FiltraDomande">
                            <asp:ListItem Value="1"></asp:ListItem>
                            <asp:ListItem Value="2"></asp:ListItem>
                            <asp:ListItem Value="3"></asp:ListItem>
                            <asp:ListItem Value="4"></asp:ListItem>
                        </asp:RadioButtonList>
                        <hr />
                        <asp:DataList ID="DLDomande" runat="server" OnItemDataBound="loadDomandeOpzioni"
                            Width="100%" DataKeyField="id" OnItemCommand="DLDomandeEditCommand">
                            <ItemTemplate>
                                <div style="text-align: right;">
                                    (Cod.<%#Eval("id")%>) Diff.<%#Eval("difficolta")%><br />
                                    <asp:Label runat="server" ID="LBSelezionaDomanda"></asp:Label><asp:CheckBox runat="server"
                                        ID="CHKSelect" Checked='<%#Eval("isSelected")%>' />
                                </div>
                                <br />
                                <span class="question" title="<%#MandatoryToolTip(Container.Dataitem)%>">
                                    <span class="number"><%#Eval("numero")%></span>
                                    <span class="separator">.</span>
                                    <%#MandatoryDisplay(Container.Dataitem)%>
                                    <span class="name">
                                    <%#me.SmartTagsAvailable.TagAll(Eval("testo"))%>
                                    </span>
                                </span>
                                <asp:ImageButton ID="IMBEdit" runat="server" ImageUrl="img/modifica-documento.gif"
                                    CommandName="edit" AlternateText=""></asp:ImageButton>
                                <asp:ImageButton ID="IMBElimina" Visible='<%#isAperto%>' runat="server" ImageUrl="img/elimina.gif"
                                    CommandName="elimina" AlternateText="" OnClientClick="return confirm('Sei sicuro di voler cancellare la domanda?');">
                                </asp:ImageButton>
                                <asp:ImageButton ID="IMBSpostaSu" Visible='<%#isAperto%>' runat="server" ImageUrl="img/up.jpg"
                                    CommandName="su" AlternateText=""></asp:ImageButton>
                                <asp:ImageButton ID="IMBSpostaGiu" Visible='<%#isAperto%>' runat="server" ImageUrl="img/down.jpg"
                                    CommandName="giu" AlternateText=""></asp:ImageButton>
                                <br />
                                <br />
                                <asp:PlaceHolder ID="PHOpzioni" runat="server" Visible="true"></asp:PlaceHolder>
                                <br />
                                <br />
                            </ItemTemplate>
                            <FooterStyle BackColor="WHITE" Font-Bold="True" ForeColor="White" />
                            <SelectedItemStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                            <AlternatingItemStyle BackColor="WHITE" />
                            <ItemStyle BackColor="WHITE" />
                            <HeaderStyle BackColor="#EFF3FB" Font-Bold="True" ForeColor="White" />
                        </asp:DataList>
                        <asp:Label runat="server" ID="LBScegliAzioneBottom"></asp:Label>
                        <asp:DropDownList runat="server" ID="DDLAzioniBottom">
                            <asp:ListItem Text="" Value="0"></asp:ListItem>
                            <asp:ListItem Text="" Value="1"></asp:ListItem>
                            <asp:ListItem Text="" Value="2"></asp:ListItem>
                            <asp:ListItem Text="" Value="3"></asp:ListItem>
                            <asp:ListItem Text="" Value="4"></asp:ListItem>
                            <asp:ListItem Text="" Value="5"></asp:ListItem>
                        </asp:DropDownList>
                        <asp:LinkButton runat="server" ID="LNBConfermaAzioneBottom" CssClass="Link_Menu"
                            CommandName="ConfermaAzioneBottom" />
                        <%-- <asp:Label runat="server" ID="LBCopiaBottom" Visible='<%#isCopyVisible%>'></asp:Label>
                        <asp:DropDownList runat="server" ID="DDLQuestionariDestinazioneBottom" Visible='<%#isCopyVisible%>'
                            DataTextField="nome" DataValueField="id">
                        </asp:DropDownList>
                        <asp:LinkButton runat="server" ID="LNBCopiaDomandeBottom" Text="Copy" Visible='<%#isCopyVisible%>'
                            CssClass="Link_Menu" CommandName="CopiaDomandeBottom" />--%>
                    </ItemTemplate>
                    <FooterStyle BackColor="#1C5E55" Font-Bold="True" ForeColor="White" />
                    <SelectedItemStyle BackColor="#C5BBAF" Font-Bold="True" ForeColor="#333333" />
                    <AlternatingItemStyle BackColor="#E3EAEB" />
                    <ItemStyle BackColor="#E3EAEB" />
                    <HeaderStyle BackColor="#1C5E55" Font-Bold="True" ForeColor="White" />
                </asp:DataList>&nbsp;</asp:Panel>
        </asp:View>
        <asp:View ID="VIWSelezionaLibrerie" runat="server">
            <asp:Panel runat="server" ID="PNLQuestionarioRandom" BackColor="white" BorderColor="black">
                <asp:Label ID="LBMessaggioLibrerie" runat="server" Text=""></asp:Label><br />
                <br />
                <asp:GridView ID="GRVLibrerie" runat="server" DataKeyNames="ID" AutoGenerateColumns="false"
                    Font-Size="8" ShowFooter="false" BackColor="transparent"
                    BorderColor="#8080FF" AllowPaging="True" PageSize="20" AllowSorting="True" Width="99%">
                    <Columns>
                        <asp:TemplateField ItemStyle-Width="20" HeaderText="">
                            <ItemTemplate>
                                <asp:CheckBox runat="server" ID="CHKisSelected" />
                                <asp:literal ID="LTidLanguage" runat="server" Visible="false" text='<%#Container.DataItem.IdLingua %>'></asp:literal>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="nome" HeaderText="" />
                        <asp:BoundField DataField="nDomandeDiffBassa" HeaderText="" />
                        <asp:BoundField DataField="nDomandeDiffMedia" HeaderText="" />
                        <asp:BoundField DataField="nDomandeDiffAlta" HeaderText="" />
                    </Columns>
                    <RowStyle CssClass="ROW_Normal_Small" Height="22px" />
                    <EditRowStyle BackColor="#2461BF" />
                    <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                    <PagerStyle CssClass="ROW_Page_Small" HorizontalAlign="Right" Height="25px" VerticalAlign="Bottom" />
                    <HeaderStyle CssClass="ROW_header_Small_Center" />
                    <AlternatingRowStyle CssClass="ROW_Alternate_Small" />
                </asp:GridView>
                <br />
                <br />
                <asp:Button runat="server" ID="BTNAggiungiLibreria" Text="" />
                <asp:Button runat="server" ID="BTNConferma" Text="" Visible="false" />
                <br />
                <br />
                <asp:PlaceHolder ID="PHLibrerie" runat="server"></asp:PlaceHolder>
            </asp:Panel>
        </asp:View>
        <asp:View ID="VIWImpostaLibrerie" runat="server">
            <asp:Label ID="LBMessaggioImpostaLibrerie" runat="server" Text=""></asp:Label><br />
            <br />
            <table width="100%">
                <tr>
                    <td class="ROW_header_Small_Center">
                        <asp:Label ID="LBHeaderNome" runat="server"></asp:Label>
                    </td>
                    <td class="ROW_header_Small_Center">
                        <asp:Label ID="LBHeaderDiffBassa" runat="server"></asp:Label>
                    </td>
                    <td class="ROW_header_Small_Center">
                        <asp:Label ID="LBHeaderDiffMedia" runat="server"></asp:Label>
                    </td>
                    <td class="ROW_header_Small_Center">
                        <asp:Label ID="LBHeaderDiffAlta" runat="server"></asp:Label>
                    </td>
                    <td class="ROW_header_Small_Center">
                        <asp:Label ID="LBHeaderElimina" runat="server"></asp:Label>
                    </td>
                    <td>
                    </td>
                </tr>
                <asp:Repeater ID="RPTLibrerieQuestionario" runat="server">
                    <ItemTemplate>
                        <tr>
                            <td class="CellaRisposta">
                                <%#Eval("nomeLibreria")%>
                                <asp:Label ID="LBIDLibreriaQuest" runat="server" Text='<%#Eval("idLibreria")%>' Visible="false"></asp:Label>
                            </td>
                            <td class="CellaRisposta">
                                <asp:RangeValidator ID="RVdiffBassa" runat="server" ControlToValidate="TXBDiffBassa"
                                    MaximumValue='<%#Eval("nDomandeDiffBassaDisponibili")%>' MinimumValue="0" Type="Integer"
                                    ErrorMessage="" Display="Static">* 
                                </asp:RangeValidator>
                                <asp:TextBox ID="TXBDiffBassa" runat="server" Text='<%#Eval("nDomandeDiffBassa")%>'
                                    Width="30"></asp:TextBox>
                                /
                                <asp:TextBox runat="server" ID="TXBnDomandeDiffBassaDisponibili" Style="border: none;"
                                    ReadOnly="true" Width="30" Text='<%#Eval("nDomandeDiffBassaDisponibili")%>'></asp:TextBox>
                            </td>
                            <td class="CellaRisposta">
                                <asp:RangeValidator ID="RVdiffMedia" runat="server" ControlToValidate="TXBDiffMedia"
                                    MaximumValue='<%#Eval("nDomandeDiffMediaDisponibili")%>' MinimumValue="0" Type="Integer"
                                    ErrorMessage="" Display="Static">* 
                                </asp:RangeValidator>
                                <asp:TextBox ID="TXBDiffMedia" runat="server" Text='<%#Eval("nDomandeDiffMedia")%>'
                                    Width="30"></asp:TextBox>
                                /
                                <asp:TextBox runat="server" ID="TXBnDomandeDiffMediaDisponibili" Style="border: none;"
                                    ReadOnly="true" Width="30" Text='<%#Eval("nDomandeDiffMediaDisponibili")%>'></asp:TextBox>
                            </td>
                            <td class="CellaRisposta">
                                <asp:RangeValidator ID="RVdiffAlta" runat="server" ControlToValidate="TXBDiffAlta"
                                    MaximumValue='<%#Eval("nDomandeDiffAltaDisponibili")%>' MinimumValue="0" Type="Integer"
                                    ErrorMessage="" Display="Static">* 
                                </asp:RangeValidator>
                                <asp:TextBox ID="TXBDiffAlta" runat="server" Text='<%#Eval("nDomandeDiffAlta")%>'
                                    Width="30"></asp:TextBox>
                                /
                                <asp:TextBox runat="server" ID="TXBnDomandeDiffAltaDisponibili" Style="border: none;"
                                    ReadOnly="true" Width="30" Text='<%#Eval("nDomandeDiffAltaDisponibili")%>'></asp:TextBox>
                            </td>
                            <td bordercolor="#EFF3FB" align="right" class="CellaRisposta">
                                <asp:ImageButton ID="IMBElimina" runat="server" ImageUrl="img/elimina.gif" CommandName="elimina"
                                    AlternateText=""></asp:ImageButton>
                            </td>
                        </tr>
                    </ItemTemplate>
                </asp:Repeater>
                <tr>
                    <td colspan="5">
                        <br />
                        <asp:Label runat="server" ID="LBMessaggioErrore" CssClass="Errore" Visible="false"></asp:Label>
                    </td>
                </tr>
            </table>
            <br />
            <asp:Button runat="server" ID="BTNSelezionaLibrerie" Text="" CssClass="Link_Menu"
                CausesValidation="false" />
            <asp:Button runat="server" ID="BTNSalvaLibrerie" Text="" CssClass="Link_Menu" CausesValidation="false" />
        </asp:View>
        <asp:View runat="server" ID="VIWmessaggi">
            <CTRL:Messages ID="CTRLerrorMessage"  runat="server" Visible="false" />
            <asp:Panel ID="PNLisRisposte" runat="server" Visible="false">
                <asp:Label ID="LBIsRisposte" runat="server"></asp:Label>
                <br />
                <br />
                <asp:Label ID="LBCopiaQuestionario" runat="server"></asp:Label><br />
                <asp:Button ID="BTNCopiaEEdita" runat="server" CssClass="Link_Menu" /><br />
                <br />
                <asp:Label ID="LBCancellaRisposte" runat="server" /><br />
                <asp:Button ID="BTNCancellaRisposte" runat="server" CssClass="Link_Menu" />
                <br />
                <asp:Label ID="LBreadOnly" runat="server" /><br />
                <asp:Button ID="BTNreadOnly" runat="server" CssClass="Link_Menu" />
            </asp:Panel>
        </asp:View>
    </asp:MultiView>
</asp:Content>