<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/AjaxPortal.Master" CodeBehind="EditRequestMessages.aspx.vb" Inherits="Comunita_OnLine.EditRequestMessages" %>
<%@ Register TagPrefix="CTRL" TagName="WizardSteps" Src="~/Modules/CallForPapers/UC/UC_WizardSteps.ascx" %>
<%@ Register Src="~/Modules/Common/Editor/UC_Editor.ascx" TagName="CTRLeditor" TagPrefix="CTRL" %>
<%@ Register TagPrefix="CTRL" TagName="Messages" Src="~/Modules/Common/UC/UC_ActionMessages.ascx" %>
<%@ MasterType VirtualPath="~/AjaxPortal.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="PageTitleContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="HeadContent" runat="server">
    <link href="../../../Graphics/Modules/CallForPapers/css/callforpapers.css" rel="Stylesheet" />
    <link href="../../../Jscript/Modules/Common/Choosen/chosen.css" rel="Stylesheet" />
     <script type="text/javascript" src="../../../Jscript/Modules/Common/jquery.simpleEqualize.js"></script>
    <script type="text/javascript" src="../../../Jscript/Modules/Common/Choosen/chosen.jquery.js"></script>
    <script type="text/javascript" src="../../../Jscript/Modules/Common/jquery.blockableFieldset.js"></script>
    <script type="text/javascript" src="../../../Jscript/Modules/Common/jquery.inputActivator.js"></script>
    <script type="text/javascript" src="../../../Jscript/Modules/Common/jquery.collapsableTreeAdv.js"></script>
    <script type="text/javascript" src="../../../Jscript/Modules/CallForPapers/callforpapers.js"></script>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="TitleContent" runat="server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="CPHservice" runat="server">
    <asp:MultiView id="MLVsettings" runat="server" ActiveViewIndex="1">
        <asp:View ID="VIWempty" runat="server">
            <br /><br /><br /><br />
            <asp:Label ID="LBnocalls" runat="server"></asp:Label>
            <br /><br /><br /><br />
        </asp:View>
        <asp:View ID="VIWsettings" runat="server">
            <div class="contentwrapper edit clearfix persist-area">
                <div class="column left persist-header copyThis">
                    <CTRL:WizardSteps runat="server" ID="CTRLsteps"></CTRL:WizardSteps>
                </div>
                <div class="column right resizeThis">
                    <div class="rightcontent">
                        <div class="header">
                            <div class="DivEpButton">
                                <asp:HyperLink ID="HYPbackTop" runat="server" CssClass="Link_Menu" Text="Back"></asp:HyperLink>
                                <asp:HyperLink ID="HYPpreviewCallTop" runat="server" CssClass="Link_Menu" Text="*Preview" Visible="false"  Target="_blank"></asp:HyperLink>
                                <asp:button ID="BTNsaveMessagesTop" runat="server" Text="Save"/>
                            </div>
                            <CTRL:Messages ID="CTRLmessages"  runat="server" Visible="false" />
                        </div>
                        <div class="contentouter">
                            <div class="content">
                                 <!-- @Start Open/Close message -->
                                 <div class="fieldrow long">
                                    <asp:Label ID="LBstartMessage_t" runat="server" AssociatedControlID="CTRLstartMessage" CssClass="Titolo_campo">Start message:</asp:Label>
                                    <CTRL:CTRLeditor id="CTRLstartMessage" runat="server" ContainerCssClass="containerclass" 
                                        LoaderCssClass="loadercssclass" EditorCssClass="editorcssclass" EditorHeight="180px" MaxTextLength="650000"/>
                                </div>
                                <div class="fieldrow long">
                                    <asp:Label ID="LBendMessage_t" runat="server" AssociatedControlID="CTRLendMessage" CssClass="Titolo_campo">End message:</asp:Label>
                                    <CTRL:CTRLeditor id="CTRLendMessage" runat="server" ContainerCssClass="containerclass" 
                                        LoaderCssClass="loadercssclass" EditorCssClass="editorcssclass" EditorHeight="180px" MaxTextLength="650000"/>
                                </div>
                                <!-- @End Call Open/Close message -->
                            </div>
                        </div>
                        <div class="footer">
                            <div class="DivEpButton">
                                <asp:HyperLink ID="HYPbackBottom" runat="server" CssClass="Link_Menu" Text="Back"></asp:HyperLink>
                                <asp:HyperLink ID="HYPpreviewCallBottom" runat="server" CssClass="Link_Menu" Text="*Preview" Visible="false" Target="_blank"></asp:HyperLink>
                                <asp:button ID="BTNsaveMessagesBottom" runat="server" Text="Save"/>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </asp:View>
    </asp:MultiView>
</asp:Content>