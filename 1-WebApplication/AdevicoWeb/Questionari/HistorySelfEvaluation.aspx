<%@ Page Language="vb" AutoEventWireup="false" MasterPageFile="~/AjaxPortal.Master"
    CodeBehind="HistorySelfEvaluation.aspx.vb" Inherits="Comunita_OnLine.HistorySelfEvaluation" %>

<%@ MasterType VirtualPath="~/AjaxPortal.Master" %>
<%@ Register Assembly="RadChart.Net2" Namespace="Telerik.WebControls" TagPrefix="radC" %>
<asp:Content ID="Content2" ContentPlaceHolderID="HeadContent" runat="Server">
    <link media="screen" href="stile.css?v=201604071200lm" type="text/css" rel="StyleSheet" />
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="CPHservice" runat="Server">
    <div>
        <div class="DIVHelp">
            <asp:ImageButton ID="IMBHelp" runat="server" ImageUrl="img/Help20px.png" Style="margin-top: 5px;
                float: right;" />
            <asp:Label runat="server" ID="LBHelp" Style="margin-top: 7px; float: right; vertical-align: middle;"></asp:Label>
        </div>
        <br />
        <asp:MultiView runat="server" ID="MLVquestionari">
            <asp:View ID="VIWlistaQuestionari" runat="server">
                <asp:Panel runat="server" ID="PNLQuestionari" BorderWidth="1" Width="100%">
                    <asp:Label runat="server" ID="LBtitolo" Text=""></asp:Label><br />
                    <asp:GridView Width="100%" ID="GRVelenco" runat="server" DataKeyNames="ID" AutoGenerateColumns="false"
                        OnRowCommand="GRVelenco_RowCommand" Font-Size="8" CellPadding="3"
                        ShowFooter="false" BackColor="transparent" BorderColor="#8080FF" AllowPaging="True"
                        AllowSorting="True" PageSize="20">
                        <Columns>
                            <asp:TemplateField HeaderStyle-Width="">
                                <ItemTemplate>
                                    <asp:LinkButton runat="server" ID="LNBnomeQuestionario" Text='<%#Eval("nome")%>'
                                        CommandName="Seleziona" CommandArgument='<%#Eval("nome")%>'></asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="nRisposte" />
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
            <asp:View ID="VIWlistaRisposte" runat="server">
                <asp:Panel runat="server" ID="PNLlistaRisposte" BorderWidth="1" Width="100%">
                    <asp:Label runat="server" ID="LBTitoloListaRisposte" Text=""></asp:Label><br />
                    <asp:GridView Width="100%" ID="GRVlistaRisposte" runat="server" DataKeyNames="ID"
                        AutoGenerateColumns="false" OnRowCommand="GRVlistaRisposte_RowCommand" Font-Size="8"
                        CellPadding="3" ShowFooter="false" BackColor="transparent"
                        BorderColor="#8080FF" AllowPaging="True" AllowSorting="True" PageSize="20">
                        <Columns>
                            <asp:TemplateField HeaderStyle-Width="">
                                <ItemTemplate>
                                    <asp:LinkButton runat="server" ID="LNBdataEsecuzione" Text='<%#Eval("dataInizio")%>'
                                        CommandName="visualizza" CommandArgument='<%#Eval("idQuestionarioRandom")%>'></asp:LinkButton>
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
            <asp:View runat="server" ID="VIWquestionario">
                <asp:DataList Visible="True" DataKeyField="nomeUtente" ID="DLDettagli" runat="server"
                    Width="100%" BackColor="White" BorderColor="#E7E7FF" BorderStyle="None" BorderWidth="1px"
                    CellPadding="3" GridLines="Horizontal" OnItemDataBound="DLDettagli_ItemDataBound">
                    <ItemTemplate>
                        <table>
                            <tr>
                                <td colspan="2" align="center">
                                    <asp:Label runat="server" ID="LBUtente">
                                    </asp:Label>
                                    <%#DataBinder.Eval(Container, "DataItem.nomeUtente")%>
                                </td>
                                <td rowspan="6" align="right">
                                    <radC:RadChart ID="RCRisposte" runat="server" AlternateText="" Visible="true" DefaultType="Pie"
                                        Margins-Bottom="0" Margins-Top="40" Margins-Left="0" Margins-Right="100" Palette="ExcelClassic"
                                        SeriesOrientation="Horizontal" ImageQuality="None" Width="250" Height="150" Background-MainColor="transparent">
                                        <Series>
                                            <radC:ChartSeries Type="Pie">
                                                <Items>
                                                    <radC:ChartSeriesItem Name="Corrette" YValue="10">
                                                        <Appearance MainColor="Green" SecondColor="128, 255, 128" />
                                                    </radC:ChartSeriesItem>
                                                    <radC:ChartSeriesItem Name="Errate" YValue="20">
                                                        <Appearance MainColor="Red" SecondColor="255, 128, 128" />
                                                    </radC:ChartSeriesItem>
                                                    <radC:ChartSeriesItem Name="Non Valutate" YValue="20">
                                                        <Appearance MainColor="gray" SecondColor="128, 128, 128" />
                                                    </radC:ChartSeriesItem>
                                                </Items>
                                                <LabelAppearance Distance="5" TextColor="Black">
                                                </LabelAppearance>
                                            </radC:ChartSeries>
                                        </Series>
                                        <Legend HAlignment="Right" HSpacing="10" VAlignment="Middle" VSpacing="0" ItemColor="Black">
                                            <Background BorderColor="transparent" FillStyle="Solid" MainColor="transparent" />
                                        </Legend>
                                        <Gridlines Color="Transparent">
                                            <VerticalGridlines Visible="False" Color="Transparent" />
                                            <HorizontalGridlines Color="Transparent" />
                                        </Gridlines>
                                        <Background BorderColor="Transparent" />
                                    </radC:RadChart>
                                </td>
                                <td rowspan="6" align="left">
                                    <radC:RadChart ID="RCOpzioni" runat="server" AlternateText="" Visible="true" DefaultType="Pie"
                                        Margins-Bottom="0" Margins-Top="40" Margins-Left="0" Margins-Right="0" Palette="ExcelClassic"
                                        SeriesOrientation="Horizontal" ImageQuality="None" Width="150" Height="150" Background-MainColor="transparent">
                                        <Series>
                                            <radC:ChartSeries Type="Pie">
                                                <Items>
                                                    <radC:ChartSeriesItem Name="Corrette" YValue="10">
                                                        <Appearance MainColor="Green" SecondColor="128, 255, 128" />
                                                    </radC:ChartSeriesItem>
                                                    <radC:ChartSeriesItem Name="Errate" YValue="20">
                                                        <Appearance MainColor="Red" SecondColor="255, 128, 128" />
                                                    </radC:ChartSeriesItem>
                                                    <radC:ChartSeriesItem Name="Non Valutate" YValue="20">
                                                        <Appearance MainColor="gray" SecondColor="128, 128, 128" />
                                                    </radC:ChartSeriesItem>
                                                </Items>
                                                <LabelAppearance Distance="5" TextColor="Black">
                                                </LabelAppearance>
                                            </radC:ChartSeries>
                                        </Series>
                                        <Legend Visible="False"></Legend>
                                        <Gridlines Color="Transparent">
                                            <VerticalGridlines Visible="False" Color="Transparent" />
                                            <HorizontalGridlines Color="Transparent" />
                                        </Gridlines>
                                        <Background BorderColor="Transparent" />
                                        <Title Text="Risposte" HAlignment="Left" Background-BorderColor="transparent" Background-MainColor="transparent"
                                            TextColor="Black" VAlignment="Top"></Title><Title Text="Opzioni" HAlignment="right"
                                                Background-BorderColor="transparent" Background-MainColor="transparent" TextColor="Black"
                                                VAlignment="Top"></Title>
                                    </radC:RadChart>
                                </td>
                            </tr>
                            <tr>
                                <td width="28%">
                                    <asp:Label runat="server" ID="LBPunteggioRelativo">
                                    </asp:Label>
                                    <%#DataBinder.Eval(Container, "DataItem.punteggioRelativo", "{0:f}")%>
                                    <br />
                                    <asp:Label runat="server" ID="LBnRisposteTotali">
                                    </asp:Label>
                                    <%#DataBinder.Eval(Container, "DataItem.nRisposteTotali")%>
                                    <br />
                                    <asp:Label runat="server" ID="LBnRisposteCorrette">
                                    </asp:Label>
                                    <%#DataBinder.Eval(Container, "DataItem.nRisposteCorrette")%>
                                    <br />
                                    <asp:Label runat="server" ID="LBnRisposteErrate">
                                    </asp:Label>
                                    <%#DataBinder.Eval(Container, "DataItem.nRisposteErrate")%>
                                    <br />
                                    <asp:Label runat="server" ID="LBnRisposteNonValutate">
                                    </asp:Label>
                                    <%#DataBinder.Eval(Container, "DataItem.nRisposteNonValutate")%>
                                    <br />
                                </td>
                                <td width="23%">
                                    <asp:Label runat="server" ID="LBPunteggioTotale">
                                    </asp:Label>
                                    <%#DataBinder.Eval(Container, "DataItem.punteggio", "{0:f}")%>
                                    <br />
                                    <asp:Label runat="server" ID="LBnOpzioniTotali">
                                    </asp:Label>
                                    <%#DataBinder.Eval(Container, "DataItem.nOpzioniTotali")%>
                                    <br />
                                    <asp:Label runat="server" ID="LBnOpzioniCorrette">
                                    </asp:Label>
                                    <%#DataBinder.Eval(Container, "DataItem.nOpzioniCorrette")%>
                                    <br />
                                    <asp:Label runat="server" ID="LBnOpzioniErrate"></asp:Label>
                                    <%#DataBinder.Eval(Container, "DataItem.nOpzioniErrate")%>
                                    <br />
                                    <asp:Label runat="server" ID="LBnOpzioniNonValutate">
                                    </asp:Label>
                                    <%#DataBinder.Eval(Container, "DataItem.nOpzioniNonValutate")%>
                                    <br />
                                </td>
                            </tr>
                        </table>
                    </ItemTemplate>
                    <FooterStyle BackColor="#B5C7DE" ForeColor="#4A3C8C" />
                    <SelectedItemStyle BackColor="#738A9C" Font-Bold="True" ForeColor="#F7F7F7" />
                    <AlternatingItemStyle BackColor="#F7F7F7" />
                    <ItemStyle BackColor="#E7E7FF" ForeColor="#4A3C8C" />
                    <HeaderStyle BackColor="#4A3C8C" Font-Bold="True" ForeColor="#F7F7F7" />
                </asp:DataList>
                <br />
                <asp:LinkButton Visible="false" runat="server" ID="LNBStampaQuestionario" Text="Stampa Questionario"
                    Width="160px"></asp:LinkButton><br />
                <asp:DataList Width="100%" ID="DLPagine" runat="server" DataKeyField="id" CellPadding="4"
                    ForeColor="#333333" OnItemDataBound="DLPagine_ItemDataBound">
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
            </asp:View>
            <asp:View runat="server" ID="VIWmessaggi">
                <asp:Label ID="LBerrore" runat="server"></asp:Label>
            </asp:View>
        </asp:MultiView>
    </div>
</asp:Content>
