<%@ Page Language="vb" AutoEventWireup="false" MasterPageFile="~/AjaxPortal.Master"
    CodeBehind="QuestionariCestinoList.aspx.vb" Inherits="Comunita_OnLine.QuestionariCestinoList" %>

<%@ MasterType VirtualPath="~/AjaxPortal.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="Server">
    <link media="screen" href="stile.css?v=201604071200lm" type="text/css" rel="StyleSheet" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="CPHservice" runat="server">
    <asp:Panel ID="PNLmenu" runat="server" Width="100%" HorizontalAlign="right">
        <asp:LinkButton ID="LNBSvuotaCestino" Visible="true" runat="server" CssClass="Link_Menu"></asp:LinkButton>
        <asp:LinkButton ID="LNBIndietro" Visible="true" runat="server" CssClass="Link_Menu"></asp:LinkButton>
        <div class="DIVHelp">
            <asp:ImageButton ID="IMBHelp" runat="server" ImageUrl="img/Help20px.png" Style="margin-top: 10px;
                float: right;" />
            <asp:Label runat="server" ID="LBHelp" Style="margin-top: 12px; float: right;"></asp:Label>
        </div>
    </asp:Panel>
    <asp:MultiView runat="server" ID="MLVquestionari">
        <asp:View ID="VIWdati" runat="server">
            <asp:Panel runat="server" ID="PNLQuestionari" Width="100%">
                <asp:Label runat="server" ID="LBTitolo" Text=""></asp:Label><br />
                <br />
                <asp:GridView Width="100%" ID="GRVElenco" runat="server" DataKeyNames="idQuestionarioMultilingua"
                    AutoGenerateColumns="false"  ShowFooter="false"
                    CssClass="light fullwidth" AllowSorting="True">
                    <Columns>
                        <asp:TemplateField>
                            <ItemTemplate>
                                <asp:ImageButton runat="server" ImageUrl="img/entra.gif" ID="IMBGestione" CommandName="Anteprima">
                                </asp:ImageButton>
                                <asp:LinkButton runat="server" ID="LNKNomeQuestionario" Text='<%#Eval("nome")%>'
                                    CommandName="Anteprima"></asp:LinkButton>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="dataInizio" />
                        <asp:BoundField DataField="dataFine" />
                        <asp:TemplateField>
                            <ItemTemplate>
                                <asp:ImageButton runat="server" ImageUrl="img/ripristina.gif" ID="IMBRipristina"
                                    CommandName="Ripristina"></asp:ImageButton>
                                <asp:ImageButton runat="server" ImageUrl="img/elimina.gif" ID="IMBElimina" CommandName="Elimina">
                                </asp:ImageButton>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <ItemTemplate>
                                <asp:Label runat="server" ID="LBIDQuestionario" Text='<%#Eval("id")%>' Visible="false"></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <ItemTemplate>
                                <asp:Label runat="server" ID="LBIDLingua" Text='<%#Eval("idLingua")%>' Visible="false"></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                  <%--  <RowStyle CssClass="ROW_Normal_Small" Height="22px" />
                    <EditRowStyle BackColor="#2461BF" />--%>
                    <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                    <PagerStyle CssClass="ROW_Page_Small" HorizontalAlign="Right" Height="25px" VerticalAlign="Bottom" />
                 <%--   <HeaderStyle CssClass="ROW_header_Small_Center" />
                    <AlternatingRowStyle CssClass="ROW_Alternate_Small" />--%>
                </asp:GridView>
            </asp:Panel>
        </asp:View>
        <asp:View runat="server" ID="VIWmessaggi">
            <br />
            <br />
            <asp:Label ID="LBerrore" runat="server"></asp:Label>
        </asp:View>
    </asp:MultiView>
</asp:Content>
