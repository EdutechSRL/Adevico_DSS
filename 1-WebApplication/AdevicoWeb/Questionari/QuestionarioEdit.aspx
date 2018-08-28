<%@ Page Language="vb" AutoEventWireup="false" MasterPageFile="~/AjaxPortal.Master"
    CodeBehind="QuestionarioEdit.aspx.vb" Inherits="Comunita_OnLine.QuestionarioEdit" %>
<%@ Register TagPrefix="CTRL" TagName="Messages" Src="~/Modules/Common/UC/UC_ActionMessages.ascx" %>

<%@ MasterType VirtualPath="~/AjaxPortal.Master" %>
<%@ Reference Control="UserControls/ucDomandaMultiplaEdit.ascx" %>
<asp:Content ID="Content3" ContentPlaceHolderID="HeadContent" runat="server">
   <link media="screen" href="stile.css?v=201604071200lm" type="text/css" rel="StyleSheet" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="CPHservice" runat="Server">
    <div id="QuestionariEdit">
    <asp:Panel ID="PNLmenu" runat="server" CssClass="panelMenu">
        <asp:LinkButton ID="LNBCartellaPrincipale" Visible="true" runat="server" CssClass="Link_Menu" CausesValidation="false"></asp:LinkButton>
        <asp:LinkButton ID="LNBGestioneQuestionario" Visible="true" runat="server" CssClass="Link_Menu" CausesValidation="false"></asp:LinkButton>
        <asp:LinkButton ID="LNBAggiungiPagina" Visible="true" runat="server" CssClass="Link_Menu"></asp:LinkButton>
        <div class="DIVHelp">
            <asp:ImageButton ID="IMBHelp" runat="server" ImageUrl="img/Help20px.png" CssClass="helpButton" />
            <asp:Label runat="server" ID="LBHelp" CssClass="helpLabel"></asp:Label>
        </div>
    </asp:Panel>
    
    <asp:MultiView runat="server" ID="MLVquestionari">
        <asp:View ID="VIWdati" runat="server">
            <asp:Panel ID="PNLElenco" runat="server">
                <h2>
                    <asp:Label ID="LBTitolo" runat="server" Text="" class="NomePagina"></asp:Label>
                </h2>
                <CTRL:Messages ID="CTRLmessages"  runat="server" Visible="false" />
                <asp:DataList ID="DLPagine" runat="server" DataKeyField="id" 
                    OnItemCommand="DLPagineEditCommand"
                    CssClass="datalistPagine" RepeatLayout="Flow">
                    <ItemTemplate>
                        <div class="pageHeaderContainer">
                            <div class="pageInfo">
                                <div class="NomePagina">
                                    <%#Eval("nomePagina")%>
                                </div>
                                <asp:Literal ID="LTpageNumber" runat="server" Text='<%#Container.DataItem.NumeroPagina %>' Visible="false"></asp:Literal>
                                <div class="pageAction">
                                    <asp:ImageButton ID="IMBPagina" runat="server" ImageUrl="img/modifica-documento.gif"
                                        CommandName="paginaEdit"></asp:ImageButton>
                                    <asp:ImageButton ID="IMBEliminaPag" runat="server" ImageUrl="img/elimina.gif" CommandName="elimina"
                                        AlternateText="" Visible='<%#isAperto%>'></asp:ImageButton>
                                </div>
                                <div class="pageDescription">
                                    <%#Eval("descrizione")%>
                                </div>
                            </div>
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
                        </div>
                        <asp:DataList ID="DLDomande" runat="server" OnItemDataBound="loadDomandeOpzioni"
                            DataKeyField="id" OnItemCommand="DLDomandeEditCommand"
                            CssClass="datalistDomande"
                            RepeatLayout="Flow">
                            <ItemTemplate>
                                <div class="ContenitoreDomanda0">
                                    <div class="TestoDomanda">
                                        <div class="difficultyInfo show">
                                            (Cod.<%#Eval("id")%>) Diff.<%#Eval("difficolta")%><br />
                                            <asp:Label runat="server" ID="LBSelezionaDomanda"></asp:Label><asp:CheckBox runat="server"
                                                ID="CHKSelect" Checked='<%#Eval("isSelected")%>' />
                                        </div>
                                        <div class="question-number" title="<%#MandatoryToolTip(Container.Dataitem)%>">
                                            <span class="number"><%#Eval("numero") & "."%></span>
                                            <%#MandatoryDisplay(Container.Dataitem)%>
                                        </div>
                                        <div class="question-name"><%#me.SmartTagsAvailable.TagAll(Eval("testo"))%></div>
                                    </div>
                                    <div class="buttoncontainer">
                                        <asp:ImageButton ID="IMBEdit" runat="server" ImageUrl="img/modifica-documento.gif"
                                            CommandName="edit" AlternateText=""></asp:ImageButton>
                                        <asp:ImageButton ID="IMBElimina" Visible='<%#isAperto%>' runat="server" ImageUrl="img/elimina.gif"
                                            CommandName="elimina" AlternateText="" OnClientClick="return confirm('Sei sicuro di voler cancellare la domanda?');">
                                        </asp:ImageButton>
                                        <asp:ImageButton ID="IMBSpostaSu" Visible='<%#isAperto%>' runat="server" ImageUrl="img/up.jpg"
                                            CommandName="su" AlternateText=""></asp:ImageButton>
                                        <asp:ImageButton ID="IMBSpostaGiu" Visible='<%#isAperto%>' runat="server" ImageUrl="img/down.jpg"
                                            CommandName="giu" AlternateText=""></asp:ImageButton>
                                    </div>
                                    <div class="Risposte">
                                        <asp:PlaceHolder ID="PHOpzioni" runat="server" Visible="true"></asp:PlaceHolder>
                                    </div>
                                </div>
                            </ItemTemplate>
                            <FooterStyle CssClass="footer"/>
                            <SelectedItemStyle CssClass="item-question Selected"/>
                            <AlternatingItemStyle CssClass="item-question Alternate"/>
                            <ItemStyle CssClass="item-question"/>
                            <HeaderStyle CssClass="header"/>
                        </asp:DataList>
                        <div class="selectionAction">
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
                        </div>
                    </ItemTemplate>
                    <FooterStyle CssClass="footer"/>
                    <SelectedItemStyle CssClass="item-page Selected"/>
                    <AlternatingItemStyle CssClass="item-page alternate" />
                    <ItemStyle CssClass="item-page"/>
                    <HeaderStyle CssClass="header"/>
                </asp:DataList>
            </asp:Panel>
        </asp:View>

        <asp:View ID="VIWSelezionaLibrerie" runat="server">
            <asp:Panel runat="server" ID="PNLQuestionarioRandom" CssClass="librarySelector">
                <asp:Label ID="LBMessaggioLibrerie" runat="server" Text=""></asp:Label>
                
                <asp:GridView ID="GRVLibrerie" runat="server" DataKeyNames="ID" AutoGenerateColumns="false"
                    ShowFooter="false" AllowPaging="True" PageSize="20" AllowSorting="True" 
                    CssClass="grvLibrerie">
                    <Columns>
                        <asp:TemplateField HeaderText="">
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
                    <RowStyle CssClass="ROW_Normal_Small" />
                    <SelectedRowStyle CssClass="ROW_Normal_Small Selected" />
                    <PagerStyle CssClass="ROW_Page_Small" />
                    <HeaderStyle CssClass="ROW_header_Small_Center" />
                    <AlternatingRowStyle CssClass="ROW_Alternate_Small" />
                </asp:GridView>
                
                <div class="buttoncontainer libraryAction">
                    <asp:Button runat="server" ID="BTNAggiungiLibreria" Text="" />
                    <asp:Button runat="server" ID="BTNConferma" Text="" Visible="false" />
                </div>
                <div class="libraryContainer">
                    <asp:PlaceHolder ID="PHLibrerie" runat="server"></asp:PlaceHolder>
                </div>
            </asp:Panel>
        </asp:View>
        <asp:View ID="VIWImpostaLibrerie" runat="server">
            <asp:Label ID="LBMessaggioImpostaLibrerie" runat="server" Text=""></asp:Label><br />
            <br />
            <table class="tblSetLibrary">
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
                                    ></asp:TextBox>
                                /
                                <asp:TextBox runat="server" ID="TXBnDomandeDiffBassaDisponibili"
                                    ReadOnly="true" Text='<%#Eval("nDomandeDiffBassaDisponibili")%>'></asp:TextBox>
                            </td>
                            <td class="CellaRisposta">
                                <asp:RangeValidator ID="RVdiffMedia" runat="server" ControlToValidate="TXBDiffMedia"
                                    MaximumValue='<%#Eval("nDomandeDiffMediaDisponibili")%>' MinimumValue="0" Type="Integer"
                                    ErrorMessage="" Display="Static">* 
                                </asp:RangeValidator>
                                <asp:TextBox ID="TXBDiffMedia" runat="server" Text='<%#Eval("nDomandeDiffMedia")%>'></asp:TextBox>
                                /
                                <asp:TextBox runat="server" ID="TXBnDomandeDiffMediaDisponibili"
                                    ReadOnly="true" Text='<%#Eval("nDomandeDiffMediaDisponibili")%>'></asp:TextBox>
                            </td>
                            <td class="CellaRisposta">
                                <asp:RangeValidator ID="RVdiffAlta" runat="server" ControlToValidate="TXBDiffAlta"
                                    MaximumValue='<%#Eval("nDomandeDiffAltaDisponibili")%>' MinimumValue="0" Type="Integer"
                                    ErrorMessage="" Display="Static">* 
                                </asp:RangeValidator>
                                <asp:TextBox ID="TXBDiffAlta" runat="server" Text='<%#Eval("nDomandeDiffAlta")%>'></asp:TextBox>
                                /
                                <asp:TextBox runat="server" ID="TXBnDomandeDiffAltaDisponibili"
                                    ReadOnly="true" Text='<%#Eval("nDomandeDiffAltaDisponibili")%>'></asp:TextBox>
                            </td>
                            <td class="CellaRisposta">
                                <asp:ImageButton ID="IMBElimina" runat="server" ImageUrl="img/elimina.gif" CommandName="elimina"
                                    AlternateText=""></asp:ImageButton>
                            </td>
                        </tr>
                    </ItemTemplate>
                </asp:Repeater>
                <tr>
                    <td colspan="5">
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
    </div>
</asp:Content>