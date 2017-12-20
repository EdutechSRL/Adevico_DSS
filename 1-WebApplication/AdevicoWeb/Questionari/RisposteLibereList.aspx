<%@ Page Language="vb" AutoEventWireup="false" MasterPageFile="~/AjaxPortal.Master"
    CodeBehind="RisposteLibereList.aspx.vb" Inherits="Comunita_OnLine.RisposteLibereList" %>

<%@ MasterType VirtualPath="~/AjaxPortal.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="Server">
   <link media="screen" href="stile.css?v=201604071200lm" type="text/css" rel="StyleSheet" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="CPHservice" runat="Server">
    <asp:MultiView runat="server" ID="MLVRisposte" ActiveViewIndex="0">
        <asp:View runat="server" ID="VIWMessaggi">
            <br />
            <br />
            <asp:Label ID="LBErrore" runat="server"></asp:Label>
        </asp:View>
        <asp:View runat="server" ID="VIWDati">
            <asp:Panel ID="PNLmenu" runat="server" Width="100%" HorizontalAlign="right">
                <asp:LinkButton ID="LNBListaRisposte" Visible="false" runat="server" CssClass="Link_Menu"></asp:LinkButton>&nbsp;
                <asp:LinkButton ID="LNBStatistiche" Visible="true" runat="server" CssClass="Link_Menu"></asp:LinkButton>&nbsp;
            </asp:Panel>
            <br />
            <asp:Label runat="server" ID="LBConferma" CssClass="errore" Visible="false"> </asp:Label>
            <br />
            <br />
            <asp:Panel runat="server" ID="PNLQuestionari" Width="100%" BorderWidth="1">
                <asp:Label runat="server" ID="LBTitolo" Text=""></asp:Label><br />
                <asp:Label runat="server" ID="LBTestoDomanda" Text=""></asp:Label><br />
                <br />
                <asp:GridView Width="100%" ID="GRVElenco" runat="server" DataKeyNames="idRispostaQuestionario"
                    AutoGenerateColumns="false" Visible="true" Font-Size="8"
                    ShowFooter="false" BackColor="transparent" BorderColor="#8080FF" AllowPaging="True"
                    AllowSorting="True">
                    <Columns>
                        <asp:BoundField DataField="testoOpzione" Visible="true" />
                        <asp:BoundField DataField="valore" Visible="true" />
                        <asp:TemplateField ItemStyle-Width="80px" Visible="true">
                            <ItemTemplate>
                                <asp:TextBox runat="server" Width="65px" ID="TXBValutazioneOpzioneLibera" Text='<%#Eval("Valutazione")%>'>
                                </asp:TextBox>%
                                <asp:CustomValidator ID="CUVvalutazioneOpzioneLibera" runat="server" OnServerValidate="CUVvalutazioneOpzioneLibera_ServerValidate"
                                    ControlToValidate="TXBValutazioneOpzioneLibera" Text="error:-100 ~ 100"></asp:CustomValidator>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:ButtonField CommandName="select" ItemStyle-Width="50px" ButtonType="Image" ImageUrl="~/Questionari/img/entra.gif" />
                        <asp:TemplateField Visible="false">
                            <ItemTemplate>
                                <asp:Label runat="server" ID="LBRispostaID" Text='<%#Eval("id")%>'></asp:Label>
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
                <asp:Button runat="server" ID="BTNConferma" Text="Conferma punteggi" OnClick="BTNConferma_Click">
                </asp:Button>
            </asp:Panel>
            <br />
            <asp:Label runat="server" ID="LBNomeUtente"></asp:Label>
            <br />
            <asp:Panel runat="server" ID="PNLQuestionarioUtente" Visible="false" Width="100%"
                BorderWidth="1">
                <asp:DataList Width="100%" ID="DLPagine" runat="server" DataKeyField="id" CellPadding="4"
                    Enabled="false" ForeColor="#333333">
                    <ItemTemplate>
                        <b>
                            <%#DataBinder.Eval(Container.DataItem, "nomePagina")%>
                        </b>
                        <%#DataBinder.Eval(Container.DataItem, "descrizione")%>
                        <br />
                        <hr />
                        <asp:DataList ID="DLDomande" runat="server" OnItemDataBound="loadDomandeOpzioni"
                            Width="100%" DataKeyField="id">
                            <ItemTemplate>
                                <%#DataBinder.Eval(Container, "DataItem.numero")%>
                                .
                                <%#DataBinder.Eval(Container, "DataItem.testo")%>
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
                    </ItemTemplate>
                    <FooterStyle BackColor="#1C5E55" Font-Bold="True" ForeColor="White" />
                    <SelectedItemStyle BackColor="#C5BBAF" Font-Bold="True" ForeColor="#333333" />
                    <AlternatingItemStyle BackColor="#E3EAEB" />
                    <ItemStyle BackColor="#E3EAEB" />
                    <HeaderStyle BackColor="#1C5E55" Font-Bold="True" ForeColor="White" />
                </asp:DataList>
            </asp:Panel>
        </asp:View>
    </asp:MultiView>
</asp:Content>
