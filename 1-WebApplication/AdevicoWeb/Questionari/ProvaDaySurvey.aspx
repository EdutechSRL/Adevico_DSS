<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/AjaxPortal.Master"
    CodeBehind="ProvaDaySurvey.aspx.vb" Inherits="Comunita_OnLine.ProvaDaySurvey" %>

<%@ MasterType VirtualPath="~/AjaxPortal.Master" %>
<%@ Register Src="~/Questionari/UserControls/ucDaySurvey.ascx" TagName="UC_DaySurvey"
    TagPrefix="uc1" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Charting" Assembly="Telerik.Web.UI" %>
<asp:Content ID="Content2" ContentPlaceHolderID="HeadContent" runat="Server">
   <link media="screen" href="stile.css?v=201604071200lm" type="text/css" rel="StyleSheet" />
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="CPHservice" runat="server">
    <div>
        <uc1:UC_DaySurvey runat="server" ID="UCDaySurvey1" IsActive="true" ShowPreview="false"
            ShowResults="false" Width="300" Height="300" NumberOfDaySurveys="1" />
        <uc1:UC_DaySurvey runat="server" ID="UCDaySurvey2" IsActive="false" ShowPreview="false"
            ShowResults="false" Width="300" Height="300" NumberOfDaySurveys="3" />
    </div>
    <div>
        <telerik:RadChart ID="RadChart1" runat="server" DataSourceID="SqlDataSource1" AutoTextWrap="True"
            Skin="Office2007" AutoLayout="True" Height="400px" SeriesOrientation="Horizontal"
            Legend-Visible="false" Width="800px">
            <Series>
                <telerik:ChartSeries DataYColumn="ID" Name="Testo">
                    <Appearance>
                        <FillStyle MainColor="69, 115, 167" FillType="Solid">
                        </FillStyle>
                        <TextAppearance TextProperties-Color="Black">
                        </TextAppearance>
                        <Border Color="69, 115, 167" />
                    </Appearance>
                </telerik:ChartSeries>
            </Series>
            <PlotArea>
                <XAxis DataLabelsColumn="TenMostExpensiveProducts">
                    <Appearance Color="134, 134, 134" MajorTick-Color="134, 134, 134">
                        <MajorGridLines Color="134, 134, 134" Width="0"></MajorGridLines>
                        <TextAppearance TextProperties-Color="Black">
                        </TextAppearance>
                    </Appearance>
                    <AxisLabel>
                        <Appearance RotationAngle="270">
                        </Appearance>
                        <TextBlock>
                            <Appearance TextProperties-Color="Black">
                            </Appearance>
                        </TextBlock>
                    </AxisLabel>
                    <Items>
                        <telerik:ChartAxisItem>
                            <TextBlock>
                                <Appearance TextProperties-Font="Georgia, 8pt">
                                </Appearance>
                            </TextBlock>
                        </telerik:ChartAxisItem>
                        <telerik:ChartAxisItem TextBlock-Appearance-MaxLength="300" TextBlock-Text="questo è il valore 1 vediamo se va a capo"
                            Value="1">
                            <TextBlock>
                                <Appearance TextProperties-Font="Georgia, 8pt">
                                </Appearance>
                            </TextBlock>
                        </telerik:ChartAxisItem>
                        <telerik:ChartAxisItem TextBlock-Text="questo è il valore 2 vediamo se va a capo">
                            <TextBlock>
                                <Appearance TextProperties-Font="Georgia, 8pt">
                                </Appearance>
                            </TextBlock>
                        </telerik:ChartAxisItem>
                        <telerik:ChartAxisItem TextBlock-Text="questo è il valore 3 vediamo se va a capo">
                            <TextBlock>
                                <Appearance TextProperties-Font="Georgia, 8pt">
                                </Appearance>
                            </TextBlock>
                        </telerik:ChartAxisItem>
                        <telerik:ChartAxisItem TextBlock-Text="questo è il valore 4 vediamo se va a capo">
                            <TextBlock>
                                <Appearance TextProperties-Font="Georgia, 8pt">
                                </Appearance>
                            </TextBlock>
                        </telerik:ChartAxisItem>
                        <telerik:ChartAxisItem TextBlock-Text="questo è il valore 5 vediamo se va a capo">
                            <TextBlock>
                                <Appearance TextProperties-Font="Georgia, 8pt">
                                </Appearance>
                            </TextBlock>
                        </telerik:ChartAxisItem>
                        <telerik:ChartAxisItem TextBlock-Text="questo è il valore 6 vediamo se va a capo">
                            <TextBlock>
                                <Appearance TextProperties-Font="Georgia, 8pt">
                                </Appearance>
                            </TextBlock>
                        </telerik:ChartAxisItem>
                        <telerik:ChartAxisItem TextBlock-Text="questo è il valore 7 vediamo se va a capo">
                            <TextBlock>
                                <Appearance TextProperties-Font="Georgia, 8pt">
                                </Appearance>
                            </TextBlock>
                        </telerik:ChartAxisItem>
                    </Items>
                </XAxis>
                <YAxis>
                    <Appearance Color="134, 134, 134" MinorTick-Color="134, 134, 134" MajorTick-Color="134, 134, 134">
                        <MajorGridLines Color="134, 134, 134"></MajorGridLines>
                        <MinorGridLines Color="134, 134, 134"></MinorGridLines>
                        <TextAppearance TextProperties-Color="Black">
                        </TextAppearance>
                    </Appearance>
                    <AxisLabel>
                        <Appearance RotationAngle="0">
                        </Appearance>
                        <TextBlock>
                            <Appearance TextProperties-Color="Black">
                            </Appearance>
                        </TextBlock>
                    </AxisLabel>
                </YAxis>
                <YAxis2>
                    <AxisLabel>
                        <Appearance RotationAngle="0">
                        </Appearance>
                    </AxisLabel>
                </YAxis2>
                <Appearance>
                    <FillStyle MainColor="" FillType="Solid">
                    </FillStyle>
                </Appearance>
            </PlotArea>
            <Appearance>
                <Border Color="134, 134, 134"></Border>
            </Appearance>
            <ChartTitle>
                <Appearance>
                    <FillStyle MainColor="">
                    </FillStyle>
                </Appearance>
                <TextBlock Text="Ten Most Expensive Products in the Northwind database">
                    <Appearance AutoTextWrap="True" TextProperties-Color="Black" TextProperties-Font="Arial, 18px">
                    </Appearance>
                </TextBlock>
            </ChartTitle>
            <Legend>
                <Appearance Position-AlignedPosition="TopRight" Dimensions-Margins="15%, 2%, 1px, 1px"
                    Dimensions-Paddings="2px, 8px, 6px, 3px">
                    <ItemTextAppearance TextProperties-Color="Black">
                    </ItemTextAppearance>
                    <ItemMarkerAppearance Figure="Square">
                    </ItemMarkerAppearance>
                </Appearance>
            </Legend>
        </telerik:RadChart>
    </div>
</asp:Content>
