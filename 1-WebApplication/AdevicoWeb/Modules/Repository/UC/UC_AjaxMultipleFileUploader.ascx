<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="UC_AjaxMultipleFileUploader.ascx.vb"
    Inherits="Comunita_OnLine.UC_AjaxMultipleFileUploader" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="CTRL" TagName="CommunityFolder" Src="~/Modules/Repository/UC/UC_SelectCommunityFolder.ascx" %>
<telerik:radprogressmanager id="RPBmanager" runat="server" registerforsubmit="false" />
<div class="Row">
    <div style="float: left; width: 100px; text-align: left;">
        <asp:Label ID="LBcommunity_t" runat="server" CssClass="Titolo_campoSmall">Community:</asp:Label>
    </div>
    <div style="float: left;">
        <asp:Label ID="LBcommunity" runat="server" CssClass="Testo_campoSmall"></asp:Label>
    </div>
</div>
<div class="Row" style="clear:both;">
    <div style="float: left; width: 100px; text-align: left; padding-top: 5px;">
        <span style="vertical-align: middle;">
            <asp:Label ID="LBpath_t" runat="server" CssClass="Titolo_campoSmall"></asp:Label></span>
    </div>
    <div style="float: left;">
        <span style="vertical-align: middle;">
            <asp:Label ID="LBpath" runat="server" CssClass="Testo_campoSmall"></asp:Label>&nbsp;
            <asp:Button ID="BTNeditPath" runat="server" Text="Change" CausesValidation="false" /></span>
    </div>
</div>
<div class="Row">
    <asp:MultiView ID="MLVfileAndPath" runat="server" ActiveViewIndex="0">
        <asp:View ID="VIWfiles" runat="server">
            <div id="DVfile0" runat="server" style="clear: both; height: 25px;">
                <div style="float: left; width: 100px; text-align: left;">
                    <span style="vertical-align: middle;">
                        <asp:Label ID="LBfileName_0" runat="server" CssClass="Titolo_campoSmall">File name:</asp:Label></span>
                </div>
                <div style="float: left;">
                    <input type="file" runat="server" id="INfile_0" size="50" class="Testo_campoSmall" />&nbsp;&nbsp;
                    <asp:Label ID="LBfileType_0" runat="server" CssClass="Titolo_campoSmall">File type:</asp:Label>
                    <asp:RadioButtonList ID="RBLtype_0" runat="server" CssClass="Testo_campoSmall" RepeatDirection="Horizontal"
                        RepeatLayout="Flow">
                    </asp:RadioButtonList>
                </div>
            </div>
            <div id="DVfile1" runat="server" style="clear: both; height: 25px;">
                <div style="float: left; width: 100px; text-align: left;">
                    <span style="vertical-align: middle;">
                        <asp:Label ID="LBfileName_1" runat="server" CssClass="Titolo_campoSmall">File name:</asp:Label></span>
                </div>
                <div style="float: left;">
                    <input type="file" runat="server" id="INfile_1" size="50" class="Testo_campoSmall" />&nbsp;&nbsp;
                    <asp:Label ID="LBfileType_1" runat="server" CssClass="Titolo_campoSmall">File type:</asp:Label>
                    <asp:RadioButtonList ID="RBLtype_1" runat="server" CssClass="Testo_campoSmall" RepeatDirection="Horizontal"
                        RepeatLayout="Flow">

                    </asp:RadioButtonList>
                </div>
            </div>
            <div id="DVfile2" runat="server" style="clear: both; height: 25px;">
                <div style="float: left; width: 100px; text-align: left;">
                    <span style="vertical-align: middle;">
                        <asp:Label ID="LBfileName_2" runat="server" CssClass="Titolo_campoSmall">File name:</asp:Label></span>
                </div>
                <div style="float: left;">
                    <input type="file" runat="server" id="INfile_2" size="50" class="Testo_campoSmall" />&nbsp;&nbsp;
                    <asp:Label ID="LBfileType_2" runat="server" CssClass="Titolo_campoSmall">File type:</asp:Label>
                    <asp:RadioButtonList ID="RBLtype_2" runat="server" CssClass="Testo_campoSmall" RepeatDirection="Horizontal"
                        RepeatLayout="Flow">
                    </asp:RadioButtonList>
                </div>
            </div>
            <div id="DVfile3" runat="server" style="clear: both; height: 25px;">
                <div style="float: left; width: 100px; text-align: left;">
                    <span style="vertical-align: middle;">
                        <asp:Label ID="LBfileName_3" runat="server" CssClass="Titolo_campoSmall">File name:</asp:Label></span>
                </div>
                <div style="float: left;">
                    <input type="file" runat="server" id="INfile_3" size="50" class="Testo_campoSmall" />&nbsp;&nbsp;
                    <asp:Label ID="LBfileType_3" runat="server" CssClass="Titolo_campoSmall">File type:</asp:Label>
                    <asp:RadioButtonList ID="RBLtype_3" runat="server" CssClass="Testo_campoSmall" RepeatDirection="Horizontal"
                        RepeatLayout="Flow">
                    </asp:RadioButtonList>
                </div>
            </div>
            <div id="DVfile4" runat="server" style="clear: both; height: 25px;">
                <div style="float: left; width: 100px; text-align: left;">
                    <span style="vertical-align: middle;">
                        <asp:Label ID="LBfileName_4" runat="server" CssClass="Titolo_campoSmall">File name:</asp:Label></span>
                </div>
                <div style="float: left;">
                    <input type="file" runat="server" id="INfile_4" size="50" class="Testo_campoSmall" />&nbsp;&nbsp;
                    <asp:Label ID="LBfileType_4" runat="server" CssClass="Titolo_campoSmall">File type:</asp:Label>
                    <asp:RadioButtonList ID="RBLtype_4" runat="server" CssClass="Testo_campoSmall" RepeatDirection="Horizontal"
                        RepeatLayout="Flow">
                    </asp:RadioButtonList>
                </div>
            </div>
        </asp:View>
        <asp:View ID="VIWpath" runat="server">
            <div style="clear:both;">
			    <span class="Titolo_campoSmall"<span class="Titolo_campoSmall" style="width:100px; vertical-align:middle; display:inline-block; *display:inline; *zoom:1; margin-top:3px">
                    <asp:Literal ID="LTselectFolder" runat="server">Select folder:</asp:Literal>
                </span>
                <div class="inlinewrapper">
                    <CTRL:CommunityFolder ID="CTRLCommunityFolder" runat="server" width="420px" SelectionMode="Single"
                        AjaxEnabled="false" />
                </div>
            </div>
        </asp:View>
    </asp:MultiView>
</div>
<div class="Row clearfix">
    <div style="float: left; width: 100px; text-align: left;">
        <asp:Label ID="LBvisibleTo_t" runat="server" CssClass="Titolo_campoSmall">Visible To:</asp:Label>
    </div>
    <div style="float: left;">
        <asp:RadioButtonList ID="RBLvisibleTo" runat="server" CssClass="Testo_campoSmall"
            RepeatDirection="Horizontal" RepeatLayout="Flow">
            <asp:ListItem Selected="true" Value="True">A tutti</asp:ListItem>
            <asp:ListItem Value="False">Al proprietario</asp:ListItem>
        </asp:RadioButtonList>
    </div>
</div>
<asp:Literal ID="LTscript" runat="server"></asp:Literal>
<telerik:radprogressarea id="PRAcommunityFileUpload" runat="server" displaycancelbutton="false"
    skin="Default">
</telerik:radprogressarea>