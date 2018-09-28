<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="UC_ProgressBar.ascx.vb"
    Inherits="Comunita_OnLine.UC_ProgressBar" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Charting" TagPrefix="telerik" %>
<telerik:RadChart ID="RDpathBar" runat="server" Height="120px" Width="930px" Skin="Vista" 
    AutoLayout="true" AutoTextWrap="true" SeriesOrientation="Horizontal" ChartTitle-Visible="false"
    Legend-Visible="false">
    <PlotArea>
        <YAxis MaxValue="100" MinValue="0" Step="10" AutoScale="true" Appearance-Color="Green">
        </YAxis>
        <XAxis AutoScale="false">
        </XAxis>
    </PlotArea>
</telerik:RadChart>