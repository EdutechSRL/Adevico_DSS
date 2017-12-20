<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="ucPNLValutazione.ascx.vb" Inherits="Comunita_OnLine.UCpnlValutazione" %>
<%@ Register Assembly="RadChart.Net2" Namespace="Telerik.WebControls" TagPrefix="radC" %>

    <div runat="server" id="DIVstatistiche" >
        <asp:DataList Visible="True" DataKeyField="nomeUtente" ID="DLDettagli" runat="server"
            Width="100%" BackColor="White" BorderColor="Black" BorderStyle="Solid"  BorderWidth="1px"
            CellPadding="3" GridLines="Horizontal" OnItemDataBound="DLDettagli_ItemDataBound">
            <ItemTemplate>
                <table>
                    <tr>
                        <td colspan="2">
                        </td>
                        <td rowspan="6" align="right">
                            <radC:RadChart ID="RCRisposte" runat="server" AlternateText="" Visible="true" DefaultType="Pie"
                                Margins-Bottom="5" Margins-Top="20" Margins-Left="0" Margins-Right="100" Palette="ExcelClassic"
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
            <ItemStyle BackColor="239, 243, 251"   ForeColor="Black"   />
        </asp:DataList>
    </div>