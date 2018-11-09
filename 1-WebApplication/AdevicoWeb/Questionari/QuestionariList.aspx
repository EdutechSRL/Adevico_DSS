<%@ Page Language="vb" AutoEventWireup="false" MasterPageFile="~/AjaxPortal.Master"
    CodeBehind="QuestionariList.aspx.vb" Inherits="Comunita_OnLine.QuestionariList" %>

<%@ MasterType VirtualPath="~/AjaxPortal.Master" %>
<%--OLD radtabstrip TELERIK--%>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>

<%@ Register Assembly="RadChart.Net2" Namespace="Telerik.WebControls" TagPrefix="radC" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="Server">
   <link media="screen" href="stile.css?v=201604071200lm" type="text/css" rel="StyleSheet" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="CPHservice" runat="Server">
    <div class="DIVHelp">
        <asp:ImageButton ID="IMBHelp" runat="server" ImageUrl="img/Help20px.png" Style="margin-top: 5px;
            float: right;" />
        <asp:Label runat="server" ID="LBHelp" Style="margin-top: 7px; float: right; vertical-align: middle;"></asp:Label>
          
        <br style="clear: all;" />
    </div>
    <asp:MultiView runat="server" ID="MLVquestionari">
        <asp:View ID="VIWdati" runat="server">
            <br />
            <br />
            <telerik:radtabstrip id="TBSQuestionari" runat="server" align="left" Width="900px" Height="26px" SelectedIndex="0" causesvalidation="false" autopostback="true" skin="Outlook" enableembeddedskins="true">
                <tabs>
                    <telerik:RadTab text="" value="0" runat="server"></telerik:RadTab>
                    <telerik:RadTab text="" value="3" runat="server"></telerik:RadTab>
                    <telerik:RadTab text="" value="4" runat="server"></telerik:RadTab>
                    <telerik:RadTab text="" value="2" runat="server"></telerik:RadTab>
                    <telerik:RadTab text="" value="5" runat="server"></telerik:RadTab>
                </tabs>
            </telerik:radtabstrip>
<%--            <radt:RadTabStrip ID="" runat="server" Skin="ClassicBlue" Align="justify"
                SelectedIndex="0" Width="98%" Height="31" CausesValidation="False" AutoPostBack="true">
                <Tabs>
                    <radt:Tab ID="TABquestionariComunita" Value="0">
                    </radt:Tab>
                    <radt:Tab ID="TABquestionariPubblici" Value="3">
                    </radt:Tab>
                    <radt:Tab ID="TABquestionariInvito" Value="4">
                    </radt:Tab>
                    <radt:Tab ID="TABquestionariCompilati" Value="2">
                    </radt:Tab>
                    <radt:Tab ID="TABquestionariCompilatiTutti" Value="5">
                    </radt:Tab>
                </Tabs>
            </radt:RadTabStrip>--%>
            <asp:Panel runat="server" ID="PNLQuestionari" BorderWidth="1" Width="100%">
                <asp:Label runat="server" ID="LBTitolo" Text=""></asp:Label><br />
                <br />
                <asp:GridView ID="GRVElenco" runat="server" DataKeyNames="ID" AutoGenerateColumns="false"
                     ShowFooter="false"  AllowPaging="True" AllowSorting="True" PageSize="20" CssClass="light fullwidth">
                    <Columns>
                        <asp:TemplateField>
                            <ItemTemplate>
                                <center>
                                    <asp:Image runat="server" Visible='<%#Eval("isBloccato")%>' ImageUrl="img/rosso.gif"
                                        ID="IMBloccato"></asp:Image>
                                    <asp:Image runat="server" Visible='<%#not Eval("isBloccato")%>' ImageUrl="img/verde.gif"
                                        ID="IMAperto"></asp:Image>
                                </center>
                            </ItemTemplate>
                            <ItemStyle Width="40px" />
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <ItemTemplate>
                                <a name="quest_<%#Eval("Id")%>"></a>
                                <asp:HyperLink id="HYPquestionnaire" runat="server"></asp:HyperLink>
                                <asp:Literal ID="LTname" runat="server" Visible="false" ></asp:Literal>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField Visible="false">
                            <ItemTemplate>
                                <asp:LinkButton runat="server" ID="LNBnRisposte" Text='<%#Eval ("nRisposte") %>'
                                    CommandName="vediRisposte" CommandArgument='<%#Eval("id")%>'>
                                </asp:LinkButton>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <%-- <asp:BoundField DataField="nRisposte" Visible="false" />--%>
                        <asp:BoundField DataField="dataInizio" />
                        <asp:BoundField DataField="dataFine" />
                        <asp:BoundField DataField="creator" />
                        <asp:BoundField Visible="false"/>
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
            <div id="DVmenu" style="width: 900px; text-align: right; padding-top: 5px; margin: 0px auto;" runat="server">
                <asp:LinkButton ID="LNBautoevaluationList" runat="server" CssClass="Link_Menu" Text="Back" CausesValidation="false" Height="18px"></asp:LinkButton>
            </div>
            <asp:Panel runat="server" ID="PNLlistaRisposte" Width="100%">
                <br />
                <asp:Label runat="server" ID="LBTitoloListaRisposte" Text=""></asp:Label><br /><br />
                <asp:GridView ID="GRVlistaRisposte" runat="server" DataKeyNames="ID"
                    AutoGenerateColumns="false" OnRowCommand="GRVlistaRisposte_RowCommand" ShowFooter="false" CssClass="light fullwidth" AllowPaging="True" AllowSorting="True" PageSize="20">
                    <Columns>
                        <asp:TemplateField HeaderStyle-Width="">
                            <ItemTemplate>
                                <a name="attempt_<%#Eval("Id")%>"></a>
                                <asp:LinkButton runat="server" ID="LNBdataEsecuzione" Text='<%#Eval("dataInizio")%>'
                                    CommandName="visualizza" CommandArgument='<%#Eval("idQuestionarioRandom")%>'></asp:LinkButton>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <ItemTemplate>
                                <asp:Label ID="LBcoeffDifficolta" runat="server" Text='<%#Eval("oStatistica.coeffDifficolta") %>' />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <ItemTemplate>
                                <asp:Label ID="LBpunteggio" runat="server" Text='<%#Eval("oStatistica.punteggio") %>' />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <ItemTemplate>
                                <asp:Label ID="LBpunteggioRelativo" runat="server" Text='<%#Eval("oStatistica.punteggioRelativo") %>' />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <ItemTemplate>
                                <asp:Label ID="LBnRisposteTotali" runat="server" Text='<%#Eval("oStatistica.nRisposteTotali") %>' />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <ItemTemplate>
                                <asp:Label ID="LBnRisposteCorrette" runat="server" Text='<%#Eval("oStatistica.nRisposteCorrette") %>' />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <ItemTemplate>
                                <asp:Label ID="LBnRisposteParzialmenteCorrette" runat="server" Text='<%#Eval("oStatistica.nRisposteParzialmenteCorrette") %>' />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <ItemTemplate>
                                <asp:Label ID="LBnRisposteErrate" runat="server" Text='<%#Eval("oStatistica.nRisposteErrate") %>' />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <ItemTemplate>
                                <asp:Label ID="LBnRisposteNonValutate" runat="server" Text='<%#Eval("oStatistica.nRisposteNonValutate") %>' />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <ItemTemplate>
                                <asp:Label ID="LBnRisposteSaltate" runat="server" Text='<%#Eval("oStatistica.nRisposteSaltate") %>' />
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
            <div id="DVmenuStatistics" style="width: 900px; text-align: right; padding-top: 5px; margin: 0px auto;" runat="server">
                <asp:HyperLink ID="HYPautoevaluationStatistics" runat="server" CssClass="Link_Menu" Text="Back"
                    Height="18px" CausesValidation="false"></asp:HyperLink>
            </div>
            <asp:DataList Visible="True" DataKeyField="nomeUtente" ID="DLDettagli" runat="server"
                CssClass="light fullwidth" OnItemDataBound="DLDettagli_ItemDataBound">
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
                                    Margins-Bottom="5" Margins-Top="20" Margins-Left="10" Margins-Right="100" Palette="ExcelClassic"
                                    SeriesOrientation="Horizontal" ImageQuality="None" Width="330" Height="150" Background-MainColor="transparent">
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
                                                <radC:ChartSeriesItem Name="Parzialmente Corrette" YValue="20">
                                                    <Appearance MainColor="Maroon" SecondColor="160, 100, 90" />
                                                </radC:ChartSeriesItem>
                                                <radC:ChartSeriesItem Name="Saltate" YValue="20">
                                                    <Appearance MainColor="gold" SecondColor="yellow" />
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
                        </tr>
                        <tr>
                            <td width="50%">
                                <asp:Label runat="server" ID="LBPunteggioRelativo">
                                </asp:Label>
                                <%#DataBinder.Eval(Container, "DataItem.punteggioRelativo", "{0:f}")%>
                                <br />
                                <asp:Label runat="server" ID="LBnRisposteTotali">
                                </asp:Label>
                                <%#DataBinder.Eval(Container, "DataItem.nRisposteTotali")%>
                                <br />
                                <asp:Label runat="server" ID="LBnRisposteSaltate">
                                </asp:Label>
                                <%#DataBinder.Eval(Container, "DataItem.nRisposteSaltate")%>
                                <br />
                                <asp:Label runat="server" ID="LBnRisposteCorrette">
                                </asp:Label>
                                <%#DataBinder.Eval(Container, "DataItem.nRisposteCorrette")%>
                                <br />
                            </td>
                            <td width="50%">
                                <asp:Label runat="server" ID="LBPunteggioTotale">
                                </asp:Label>
                                <%#DataBinder.Eval(Container, "DataItem.punteggio", "{0:f}")%>
                                <br />
                                <asp:Label runat="server" ID="LBnRisposteNonValutate">
                                </asp:Label>
                                <%#DataBinder.Eval(Container, "DataItem.nRisposteNonValutate")%>
                                <br />
                                <asp:Label runat="server" ID="LBnRisposteParzialmenteCorrette">
                                </asp:Label>
                                <%#DataBinder.Eval(Container, "DataItem.nRisposteParzialmenteCorrette")%>
                                <br />
                                <asp:Label runat="server" ID="LBnRisposteErrate">
                                </asp:Label>
                                <%#DataBinder.Eval(Container, "DataItem.nRisposteErrate")%>
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
</asp:Content>
