<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="UC_CommunityFolderSize.ascx.vb"
    Inherits="Comunita_OnLine.UC_CommunityFolderSize" %>
<style type="text/css">
    ul.grigliaSpazioDisco
    {
        margin: 0;
        padding: 0;
        width: 350px;
        font-size: 11px;
        font-weight: normal;
    }
    ul.grigliaSpazioDisco li
    {
        margin: 0;
        padding: 0;
        display: block;
        clear: left;
        height: 0px;
    }
    ul.grigliaSpazioDisco ul
    {
        margin: 0;
        padding: 0;
        clear: left;
    }
    ul.grigliaSpazioDisco ul li
    {
        margin: 0;
        padding: 0;
        float: left;
        clear: none;
        width: 50px;
    }
    div.riga-grigliaSpazioDisco
    {
        width: 350px;
        font-size: 11px;
        font-weight: normal;
        clear: left;
    }
    div.riga-grigliaSpazioDisco div
    {
        width: 50px;
        float: left;
        margin: 0;
        padding: 0;
    }
    div.hrClear hr
    {
        margin: 0em;
        clear: both;
        visibility: hidden;
    }
    div.hrClear
    {
        clear: both;
        height: 0;
        overflow: hidden;
    }
    hr
    {
        margin: 0em;
        clear: both;
        visibility: hidden;
    }
</style>
<div id="DIVspazioDisco" runat="server">
    <div class="riga-grigliaSpazioDisco">
        <div>
            0%</div>
        <div>
            25%</div>
        <div>
            50%</div>
        <div>
            75%</div>
        <div>
            100%</div>
        <div>
            <asp:Literal ID="LTover" runat="server">30%</asp:Literal></div>
    </div>
    <div class="riga-grigliaSpazioDisco">
        <div>
            <asp:Image ID="IMGsize25" runat="server" Height="10">
            </asp:Image></div>
        <div>
            <asp:Image ID="IMGsize50" runat="server" Height="10">
            </asp:Image></div>
        <div>
            <asp:Image ID="IMGsize75" runat="server" Height="10">
            </asp:Image></div>
        <div>
            <asp:Image ID="IMGsize100" runat="server" Height="10">
            </asp:Image></div>
        <div>
            <asp:Image ID="IMGsize150" runat="server" Height="10">
            </asp:Image></div>
        <div>&nbsp;</div>
    </div>
    <div style="clear: left;">
        <asp:Label ID="LBinfoSize" runat="server">Available size:&nbsp; </asp:Label>
        <asp:Label ID="LBLspazioOccupato" runat="server"></asp:Label>
    </div>
</div>