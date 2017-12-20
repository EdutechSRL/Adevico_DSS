<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/AjaxPortal.Master" CodeBehind="EditTemplate.aspx.vb" Inherits="Comunita_OnLine.EditTemplate" %>
<%@ Register TagPrefix="CTRL" TagName="WizardSteps" Src="~/Modules/Templates/UC/UC_WizardSteps.ascx" %>
<%@ Register TagPrefix="CTRL" TagName="Messages" Src="~/Modules/Common/UC/UC_ActionMessages.ascx" %>
<%@ Register TagPrefix="CTRL" TagName="Selector" Src="~/Modules/Common/UC/UC_ContentTranslationSelector.ascx" %>
<%@ Register TagPrefix="CTRL" TagName="SelectorHeader" Src="~/Modules/Common/UC/UC_ContentTranslationSelectorHeader.ascx" %>
<%@ Register TagPrefix="CTRL" TagName="Translator" Src="~/Modules/Common/UC/UC_ContentTranslator.ascx" %>
<%@ Register TagPrefix="CTRL" TagName="TranslatorHeader" Src="~/Modules/Common/UC/UC_ContentTranslatorHeader.ascx" %>
<%@ Register Src="~/Modules/Common/UC/UC_MailMessage.ascx" TagName="CTRLmailMessage" TagPrefix="CTRL" %>
<%@ Register Src="~/Modules/Common/UC/UC_MailMessageHeader.ascx" TagName="CTRLmailMessageHeader" TagPrefix="CTRL" %>

<%@ MasterType VirtualPath="~/AjaxPortal.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="PageTitleContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="CPHserviceLocalization" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="HeadContent" runat="server">
    <link href="../../Graphics/Modules/TemplateMessages/css/TemplateMessages.css" rel="Stylesheet" />
    <link href="../../Jscript/Modules/Common/Choosen/chosen.css" rel="Stylesheet" />
    <script type="text/javascript" src="../../Jscript/Modules/Common/jquery.simpleEqualize.js"></script>
    <script type="text/javascript" src="../../Jscript/Modules/Common/Choosen/chosen.jquery.js"></script>
    <script type="text/javascript" src="../../Jscript/Modules/Common/jquery.blockableFieldset.js"></script>
    <script type="text/javascript" src="../../Jscript/Modules/Common/jquery.inputActivator.js"></script>
    <script type="text/javascript" src="../../Jscript/Modules/Common/jquery.collapsableTreeAdv.js"></script>
    <script type="text/javascript" src="../../Jscript/Modules/Common/jquery.ddbuttonlist.js"></script>
    <CTRL:SelectorHeader ID="CTRLselectorHeader" runat="server" />
    <CTRL:TranslatorHeader ID="CTRLtranslatorHeader" runat="server" />
    <CTRL:CTRLmailMessageHeader ID="CTRLmailMessageHeader" runat="server" />
    <script language="javascript" type="text/javascript">
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
                   // $(this).parent().appendTo("form");
                    //$(this).parent().children().children('.ui-dialog-titlebar-close').hide();
                }
            });
        });
    </script>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="TitleContent" runat="server">
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="CPHservice" runat="server">
    <asp:MultiView id="MLVsettings" runat="server" ActiveViewIndex="1">
        <asp:View ID="VIWempty" runat="server">
            <br /><br /><br /><br />
            <asp:Label ID="LBnoTemplate" runat="server"></asp:Label>
            <br /><br /><br /><br />
        </asp:View>
        <asp:View ID="VIWtranslations" runat="server">
            <div class="contentwrapper edit clearfix persist-area">
                <div class="column left persist-header copyThis">
                    <CTRL:WizardSteps runat="server" ID="CTRLsteps"></CTRL:WizardSteps>
                </div>
                <div class="column right resizeThis">
                    <div class="rightcontent">
                        <div class="header">
                            <div class="DivEpButton">
                                <asp:HyperLink ID="HYPbackTop" runat="server" CssClass="Link_Menu" Text="Back"></asp:HyperLink>
                            </div>
                            <CTRL:Messages ID="CTRLmessages" runat="server" Visible="false" />
                            <div class="fieldobject">
                                <div class="fieldrow">
                                    <CTRL:Selector ID="CTRLlanguageSelector" runat="server" RaiseSelectionEvent="true" />
                                </div>
                            </div>
                        </div>
                        <div class="contentouter">
                            <div class="content">
                                <CTRL:Translator ID="CTRLtranslator" runat="server" ShowName="true" ShowSubject="true" NewLineMode="Br"  />
                            </div>
                        </div>
                        <div class="footer">
                            <div class="DivEpButton">
                                <asp:HyperLink ID="HYPbackBottom" runat="server" CssClass="Link_Menu" Text="Back"></asp:HyperLink>
                                <asp:button ID="BTNdeleteTranslationBottom" CssClass="needconfirm" runat="server" Text="*Delete" Visible="false"/>
                                <asp:button ID="BTNtemplateMessagePreview" runat="server" Text="*Preview" Visible="false"/>
                                <asp:button ID="BTNsaveTranslationBottom" runat="server" Text="*Save" Visible="false"/>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <div class="view-modal view-previewmail" id="DVpreview" runat="server" visible="false">
                <CTRL:CTRLmailMessage ID="CTRLmailpreview" runat="server" AllowSendMail="false" DisplayTopWindowCloseButton="false" EditAddressTo="false"  />
                <div class="fieldobject clearfix">
                    <div class="fieldrow right">
                        <asp:Button ID="BTNcloseMailMessageWindow" runat="server" CssClass="Link_Menu" />
                    </div>
                </div>
            </div>
        </asp:View>
    </asp:MultiView>
</asp:Content>