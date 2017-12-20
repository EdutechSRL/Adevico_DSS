<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="ucGraph.ascx.vb" Inherits="Comunita_OnLine.ucGraph" %>
<%@ Register Assembly="RadChart.Net2" Namespace="Telerik.WebControls" TagPrefix="radC" %>
<radC:RadChart ID="RCStatUtente" runat="server" AlternateText="" DataGroupsColumn=""
    DefaultType="Pie" Margins-Bottom="12%" Margins-Left="10%" Palette="ExcelClassic"
    SeriesOrientation="Horizontal" ImageQuality="None">
    <Series>
        <radC:ChartSeries Name="Series 1" Type="Pie">
            <Items>
                <radC:ChartSeriesItem MainColor="Green" Name="Risposte Corrette" YValue="20" Label="">
                    <Appearance MainColor="Green" SecondColor="128, 255, 128" />
                </radC:ChartSeriesItem>
                <radC:ChartSeriesItem MainColor="Red" Name="Risposte Errate" YValue="20" Label="">
                    <Appearance MainColor="Red" SecondColor="255, 128, 128" />
                </radC:ChartSeriesItem>
            </Items>
            <LabelAppearance Distance="5" TextColor="Black">
            </LabelAppearance>
        </radC:ChartSeries>
    </Series>
    <Legend HAlignment="Right" HSpacing="10" VSpacing="0" ItemColor="Black">
        <Background BorderColor="227, 227, 227" FillStyle="Solid" MainColor="White" />
    </Legend>
    <Titles>
        <radC:ChartTitle HorPadding="10" HSpacing="10" TextColor="Black">
            <Background BorderColor="199, 199, 199" FillStyle="Solid" MainColor="White" />
        </radC:ChartTitle>
    </Titles>
    <YAxis MaxValue="140" Step="20">
    </YAxis>
    <XAxis MaxValue="5" MinValue="1" Step="1" AxisColor="DarkGray">
    </XAxis>
    <PlotArea BorderColor="Transparent" Corners-BottomLeft="Round" Corners-BottomRight="Round" Corners-RoundSize="6"
        Corners-TopLeft="Round" Corners-TopRight="Round" />
    <Gridlines Color="Transparent">
        <VerticalGridlines Visible="False" Color="Transparent" />
        <HorizontalGridlines Color="Transparent" />
    </Gridlines>
    <Title HorPadding="10" HSpacing="10" TextColor="Black">
        <Background BorderColor="199, 199, 199" FillStyle="Solid" MainColor="White" />
    </Title>
    <Background BorderColor="Transparent" />
</radC:RadChart>

<!-- pippo prova aggiornameto -->
