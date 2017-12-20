<%@ Page Language="vb" AutoEventWireup="false" MasterPageFile="~/AjaxPortal.Master"
    CodeBehind="GruppiList.aspx.vb" Inherits="Comunita_OnLine.GruppiList" Title="Pagina senza titolo" %>

<%@ MasterType VirtualPath="~/AjaxPortal.Master" %>
<%--OLD radtabstrip TELERIK--%>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="Server">
    <link media="screen" href="stile.css?v=201604071200lm" type="text/css" rel="StyleSheet" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="CPHservice" runat="Server">
    <asp:Panel ID="PNLmenu" runat="server" Width="100%" HorizontalAlign="right">
        <asp:LinkButton ID="LNBNuovoQuestionario" Visible="true" runat="server" CssClass="Link_Menu"></asp:LinkButton>&nbsp;
        <asp:LinkButton ID="LNBNuovaCartella" Visible="true" runat="server" CssClass="Link_Menu"></asp:LinkButton>
        <asp:LinkButton ID="LNBCestino" Visible="true" runat="server" CssClass="Link_Menu"></asp:LinkButton>
        <div class="DIVHelp">
            <asp:ImageButton ID="IMBHelp" runat="server" ImageUrl="img/Help20px.png" Style="margin-top: 10px;
                float: right;" />
            <asp:Label runat="server" ID="LBHelp" Style="margin-top: 12px; float: right;"></asp:Label>
        </div>
    </asp:Panel>
    <asp:MultiView runat="server" ID="MLVquestionari">
        <asp:View ID="VIWdati" runat="server">
            <br />
            <asp:Label ID="LBLCartella" runat="server" Font-Bold="True"></asp:Label>
            <br />
            <telerik:radtabstrip id="TBSGruppi" runat="server" align="Justify" Width="800px" Height="26px" SelectedIndex="0"
                causesvalidation="false" autopostback="true" skin="Outlook" enableembeddedskins="true">
                <tabs>
                    <telerik:RadTab text="" value="0" runat="server"></telerik:RadTab>
                </tabs>
            </telerik:radtabstrip>
            <asp:Panel runat="server" ID="PNLQuestionari" Width="100%">
                <asp:GridView Width="100%" ID="GRVElenco" runat="server" DataKeyNames="id" AutoGenerateColumns="false"
                    OnRowCommand="GRVElenco_RowCommand" Font-Size="8" CellPadding="3"
                    ShowFooter="false" BackColor="transparent" BorderColor="#8080FF" AllowPaging="True"
                    AllowSorting="True">
                    <Columns>
                        <asp:TemplateField>
                            <ItemTemplate>
                                <asp:ImageButton runat="server" ImageUrl="img/modifica-documento.gif" ID="IMBGestione"
                                    CommandName="Gestione"></asp:ImageButton>
                                <asp:LinkButton runat="server" ID="LNKNomeQuestionario" Text='<%#Eval("nome")%>'
                                    CommandName="Gestione"></asp:LinkButton>
                                <asp:Label ID="LBLNomeQuestionario" runat="server" Text='<%#Eval("nome")%>' Visible="false"></asp:Label></ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField ItemStyle-Width="20%">
                            <ItemTemplate>
                                <center>
                                    <asp:LinkButton runat="server" ID="LNKArgomenti" CommandName="Argomenti"></asp:LinkButton>
                                </center>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField ItemStyle-Width="10%">
                            <ItemTemplate>
                                <center>
                                    <asp:ImageButton runat="server" ImageUrl="img/cestino.gif" ID="IMBElimina" CommandName="Elimina">
                                    </asp:ImageButton>
                                </center>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                    <RowStyle CssClass="ROW_Normal_Small" Height="22px" />
                    <EditRowStyle BackColor="#2461BF" />
                    <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                    <PagerStyle CssClass="ROW_Page_Small" HorizontalAlign="Right" Height="25px" VerticalAlign="Bottom" />
                    <HeaderStyle CssClass="ROW_header_Small_Center" />
                    <AlternatingRowStyle CssClass="ROW_Alternate_Small" />
                </asp:GridView>
            </asp:Panel>
        </asp:View>
        <asp:View runat="server" ID="VIWCancellaQuestionario">
            <div align="center">
                <asp:Label runat="server" ID="LBLNomeQuestionario" Font-Bold="true"></asp:Label><br />
                <asp:Label runat="server" ID="LBLTestoDescrizione"></asp:Label><br />
                <asp:DataList runat="server" ID="DLLingue" DataKeyField="id">
                    <ItemTemplate>
                        <asp:CheckBox runat="server" ID="CHKSelezionaLingua" Text='<%#Eval("nome")%>' />
                    </ItemTemplate>
                </asp:DataList>
                <br />
                <asp:Button runat="server" ID="BTNEliminaLingua" CssClass="Link_Menu" />
            </div>
        </asp:View>
        <asp:View runat="server" ID="VIWmessaggi">
            <br />
            <br />
            <asp:Label ID="LBerrore" runat="server"></asp:Label>
        </asp:View>
    </asp:MultiView>
</asp:Content>
