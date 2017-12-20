<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/AjaxPortal.Master"
    CodeBehind="EduPathSelect.aspx.vb" Inherits="Comunita_OnLine.EduPathSelect" %>
<%@ Register Src="~/Modules/Common/UC/UC_ActionMessages.ascx" TagPrefix="CTRL" TagName="Messages" %>
<%@ MasterType VirtualPath="~/AjaxPortal.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="CPHservice" runat="server">
    <asp:MultiView runat="server" ID="MLVpathSelect">
        <asp:View runat="server" ID="VIWdata">
            <CTRL:Messages runat="server" ID="CTRLmoduleStatusMessage" Visible="false" />
            <div>
                <div style="width: 100%; padding: 10px, auto; margin: 0, auto; clear: both;" id="DIVtimeAuto"
                    enableviewstate="false" runat="server">
                    <div style="width: 30%; float: left; text-align: left;">
                        <asp:Label runat="server" ID="LBtimeAutoTitle" Font-Bold="true"></asp:Label><br />
                        <asp:HyperLink runat="server" ID="HYPtimeAuto" CssClass="Link_Menu"></asp:HyperLink>
                    </div>
                    <div style="width: 65%; float: left; text-align: left;" class="bordato">
                        <asp:Label ID="LBtimeAuto" runat="server" Font-Bold="true"></asp:Label>
                    </div>
                    <div style="clear: both;">
                        &nbsp;</div>
                </div>
                <div style="width: 100%; padding: 10px, auto; margin: 0, auto; clear: both;" id="DIVvoteManual"
                    enableviewstate="false" runat="server">
                    <div style="width: 30%; float: left; text-align: left;">
                        <asp:Label runat="server" ID="LBvoteManualTitle" Font-Bold="true"></asp:Label><br />
                        <asp:HyperLink runat="server" ID="HYPvoteManual" CssClass="Link_Menu"></asp:HyperLink>
                    </div>
                    <div style="width: 65%; float: left; text-align: left;" class="bordato">
                        <asp:Label ID="LBvoteManual" runat="server" Font-Bold="true"></asp:Label>
                    </div>
                    <div style="clear: both;">
                        &nbsp;</div>
                </div>
                <div style="width: 100%; padding: 10px, auto; margin: 0, auto; clear: both;" id="DIVvoteAuto"
                    enableviewstate="false" runat="server" visible="false">
                    <div style="width: 30%; float: left; text-align: left;">
                        <asp:Label runat="server" ID="LBvoteAutoTitle" Font-Bold="true"></asp:Label><br />
                        <asp:HyperLink runat="server" ID="HYPvoteAuto" CssClass="Link_Menu"></asp:HyperLink>
                    </div>
                    <div style="width: 65%; float: left; text-align: left;" class="bordato">
                        <asp:Label ID="LBvoteAuto" runat="server" Font-Bold="true"></asp:Label>
                    </div>
                    <div style="clear: both;">
                        &nbsp;</div>
                </div>
                <div style="width: 100%; padding: 10px, auto; margin: 0, auto; clear: both;" id="DIVtimeManual"
                    enableviewstate="false" runat="server" visible="false">
                    <div style="width: 30%; float: left; text-align: left;">
                        <asp:Label runat="server" ID="LBtimeManualTitle" Font-Bold="true"></asp:Label><br />
                        <asp:HyperLink runat="server" ID="HYPtimeManual" CssClass="Link_Menu"></asp:HyperLink>
                    </div>
                    <div style="width: 65%; float: left; text-align: left;" class="bordato">
                        <asp:Label ID="LBtimeManual" runat="server" Font-Bold="true"></asp:Label>
                    </div>
                    <div style="clear: both;">
                        &nbsp;</div>
                </div>
            </div>
        </asp:View>
        <asp:View runat="server" ID="VIWerror">
            <div id="DVerror" align="center">
                <div align="right" style="text-align: right; clear: right;">
                    <asp:Button ID="BTNerror" runat="server" CssClass="Link_Menu" />
                </div>
                <div align="center">
                    <asp:Label ID="LBerror" runat="server" CssClass="messaggio"></asp:Label>
                </div>
            </div>
        </asp:View>
         <asp:View ID="VIWmessages" runat="server">
            <CTRL:Messages runat="server" ID="CTRLmessages" />
        </asp:View>
    </asp:MultiView>
</asp:Content>