<%@ Page Language="vb" AutoEventWireup="false" MasterPageFile="~/AjaxPortal.Master"
    CodeBehind="QuestionarioView.aspx.vb" Inherits="Comunita_OnLine.QuestionarioView" %>

<%@ MasterType VirtualPath="~/AjaxPortal.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="Server">
   <link media="screen" href="stile.css?v=201604071200lm" type="text/css" rel="StyleSheet" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="CPHservice" runat="Server">
    <asp:MultiView runat="server" ID="MLVquestionari">
        <asp:View ID="VIWdati" runat="server">
            <asp:Panel ID="PNLmenu" runat="server" Width="100%" HorizontalAlign="right">
                <asp:LinkButton ID="LNBCartellaPrincipale" Visible="true" runat="server" CssClass="Link_Menu"
                    Style="margin-right: auto;" />
                <asp:LinkButton ID="LNBGestioneDomande" Visible="false" runat="server" CssClass="Link_Menu"
                    Style="margin-right: auto;" />
                <asp:LinkButton ID="LNBCestino" Visible="false" runat="server" CssClass="Link_Menu"
                    Style="margin-right: auto;" />
                <asp:LinkButton ID="LNBImportaModello" Visible="true" runat="server" CssClass="Link_Menu">
                </asp:LinkButton>
            </asp:Panel>
            <br />
            <br />
            <asp:Panel ID="PNLElenco" runat="server">
                <asp:Label ID="LBTitolo" runat="server" Text=""></asp:Label>
                <br />
                <br />
                <asp:DataList ID="DLPagine" runat="server" DataKeyField="id" CellPadding="4" ForeColor="#333333"
                    BorderWidth="1" Width="100%">
                    <ItemTemplate>
                        <b>
                            <%#Eval("nomePagina")%>
                        </b>
                        <br />
                        <i>
                            <%#Eval("descrizione")%>
                        </i>
                        <hr />
                        <asp:DataList ID="DLDomande" runat="server" DataKeyField="id" OnItemDataBound="loadDomandeOpzioni"
                            Width="100%">
                            <ItemTemplate>
                                <div style='<%# me.displayDifficulty %>'>
                                    (Cod.<%#Eval("id")%>
                                    <asp:Label runat="server" ID="LBDifficoltaTesto" Text='<%#Eval("difficoltaTesto")%>'></asp:Label>)
                                </div>
                                <br />
                                <asp:Label ID="Label1" runat="server" Text='<%#Eval("numero")%>' Visible='<%# me.showDifficulty %>'></asp:Label>
                                <%#me.SmartTagsAvailable.TagAll(Eval("testo"))%>
                                <br />
                                <br />
                                <asp:PlaceHolder ID="PHOpzioni" runat="server" Visible="true"></asp:PlaceHolder>
                                <br />
                                <asp:Label runat="server" ID="LBSuggerimento" Text='<%#Eval("suggerimento")%>' Visible="false"></asp:Label>
                            </ItemTemplate>
                            <FooterStyle BackColor="WHITE" Font-Bold="True" ForeColor="White" />
                            <SelectedItemStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                            <AlternatingItemStyle BackColor="#EFF3FB" />
                            <ItemStyle BackColor="WHITE" />
                            <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                        </asp:DataList>
                        <br />
                        <div class="NomePaginaFooter">
                            <%#Eval("nomePagina")%>
                        </div>
                    </ItemTemplate>
                    <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                    <SelectedItemStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                    <AlternatingItemStyle BackColor="#507CD1" />
                    <ItemStyle BackColor="#EFF3FB" />
                    <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                </asp:DataList>
            </asp:Panel>
            <br />
            <div style="width: 100%; margin-top: 10px; margin-bottom: 20px; text-align: center;">
                <asp:Label ID="LBpagina" runat="server"></asp:Label>
                <asp:ImageButton ID="IMBprima" ImageUrl="img/indietro.gif" runat="server" Visible="False">
                </asp:ImageButton>
                &nbsp; &nbsp;
                <asp:PlaceHolder ID="PHnumeroPagina" runat="server"></asp:PlaceHolder>
                &nbsp; &nbsp;
                <asp:ImageButton ID="IMBdopo" runat="server" ImageUrl="img/avanti.gif" Visible="False">
                </asp:ImageButton>
            </div>
        </asp:View>
        <asp:View runat="server" ID="VIWmessaggi">
            <br />
            <br />
            <asp:Label ID="LBerrore" runat="server"></asp:Label>
        </asp:View>
    </asp:MultiView>
</asp:Content>
