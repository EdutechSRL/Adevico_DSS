<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/AjaxPortal.Master" CodeBehind="EditNotificationTemplate.aspx.vb" Inherits="Comunita_OnLine.EditNotificationTemplate" %>
<%@ Register TagPrefix="CTRL" TagName="WizardSteps" Src="~/Modules/CallForPapers/UC/UC_WizardSteps.ascx" %>
<%@ Register Src="~/Modules/Common/UC/UC_MailEditor.ascx" TagName="CTRLtemplateMail" TagPrefix="CTRL" %>
<%@ Register Src="~/Modules/Common/UC/UC_MailEditorHeader.ascx" TagName="CTRLtemplateMailHeader" TagPrefix="CTRL" %>
<%@ Register Src="~/Modules/Common/UC/UC_MailMessage.ascx" TagName="CTRLmailMessage" TagPrefix="CTRL" %>
<%@ Register Src="~/Modules/Common/UC/UC_MailMessageHeader.ascx" TagName="CTRLmailMessageHeader" TagPrefix="CTRL" %>
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

    <script type="text/javascript">
        $(document).ready(function () {
            $(".view-modal.view-previewmail").dialog({
                appendTo: "form",
                closeOnEscape: true,
                modal: true,
                width: 700,
                height: 450,
                minHeight: 200,
                minWidth: 400,
                open: function (type, data) {
                    //$(this).parent().appendTo("form");
                    //$(this).parent().children().children('.ui-dialog-titlebar-close').hide();
                }
            });
        });
    </script>


    <CTRL:CTRLtemplateMailHeader ID="CTRLtemplateMailHeader" runat="server" />
    <CTRL:CTRLmailMessageHeader ID="CTRLmailMessageHeader" runat="server" />
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
                                <asp:HyperLink ID="HYPpreviewCallTop" runat="server" CssClass="Link_Menu" Text="*Preview" Visible="false" Target="_blank"></asp:HyperLink>
                                <asp:button ID="BTNpreviewNotificationMailTop" runat="server" Text="*Preview mail message"/>
                                <asp:button ID="BTNsaveNotificationMailTop" runat="server" Text="Save"/>
                            </div>
                            <CTRL:Messages ID="CTRLmessages"  runat="server" Visible="false" />
                        </div>
                        <div class="contentouter">
                            <div class="content">
                                <!-- @Start NOTIFICATION MAIL -->
                                <ul class="templates">
                                    <li class="template edit" runat="server" id="LItemplateAdd">
                                        <div class="templatecontent">
                                            <div class="title clearfix">
                                                <div class="left">
                                                    <asp:Label ID="LBnotificationTemplateName_t" runat="server" CssClass="fieldlabel" AssociatedControlID="LBnotificationTemplateName"></asp:Label>
                                                    <asp:Label ID="LBnotificationTemplateName" runat="server"></asp:Label>
                                                    <asp:Label ID="LBtemplateName" runat="server" Visible="false"></asp:Label>
                                                </div>
                                                <div class="right">
                                                    <span class="icons">
                                                    </span>
                                                </div>
                                                <div class="clearer"></div>
                                            </div>
                                            <hr/>
                                            <div class="fieldrow">
                                                <asp:Label ID="LBnotifyTo" runat="server" AssociatedControlID="TXBnotifyTo" CssClass="fieldlabel"></asp:Label>
                                                <asp:TextBox ID="TXBnotifyTo" ReadOnly="false" CssClass="inputtext" runat="server" Columns="60"></asp:TextBox>
                                                <asp:RequiredFieldValidator ID="RFVnotifyTo" runat="server" ControlToValidate="TXBnotifyTo" EnableClientScript="true" SetFocusOnError="true" Message="*"></asp:RequiredFieldValidator>
                                            </div>
                                            <div class="fieldobject">
                                                <CTRL:CTRLtemplateMail id="CTRLtemplate" runat="server" ContainerLeft="[" ContainerRight="]" AllowCopyToSender="False" AllowNotifyToSender="False" 
                                                         MustValidate="false" RaiseUpdateEvent="false" AllowFormatSelection="False"/>
                                            </div>
                                        </div>
                                    </li>
                                </ul>
                                <!-- @End NOTIFICATION MAIL -->
                            </div>
                        </div>
                        <div class="footer">
                            <div class="DivEpButton">
                                <asp:HyperLink ID="HYPbackBottom" runat="server" CssClass="Link_Menu" Text="Back"></asp:HyperLink>
                                <asp:HyperLink ID="HYPpreviewCallBottom" runat="server" CssClass="Link_Menu" Text="*Preview" Visible="false" Target="_blank"></asp:HyperLink>
                                <asp:button ID="BTNpreviewNotificationMailBottom" runat="server" Text="*Preview mail message"/>
                                <asp:button ID="BTNsaveNotificationMailBottom" runat="server" Text="Save"/>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </asp:View>
    </asp:MultiView>
    <div class="view-modal view-previewmail" id="DVpreview" runat="server" visible="false">
        <CTRL:CTRLmailMessage ID="CTRLmailpreview" runat="server" AllowSendMail="true" EditAddressTo="true" DisplayTopWindowCloseButton="true" />
        <div class="fieldobject clearfix">
            <div class="fieldrow right">
                <asp:Button ID="BTNcloseMailMessageWindow" runat="server" CssClass="Link_Menu" />
            </div>
        </div>
    </div>
</asp:Content>