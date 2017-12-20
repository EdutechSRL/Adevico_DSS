<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/AjaxPopup.Master" CodeBehind="MessageTemplate.aspx.vb" Inherits="Comunita_OnLine.MailMessageTemplate" %>
<%@ MasterType VirtualPath="~/AjaxPopup.Master" %>
<%@ Register TagPrefix="CTRL" TagName="Selector" Src="~/Modules/Common/UC/UC_ContentTranslationSelector.ascx" %>
<%@ Register TagPrefix="CTRL" TagName="MailSettings" Src="~/Modules/Common/UC/UC_MailSettings.ascx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="PageTitleContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="CPHserviceLocalization" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="HeadContent" runat="server">
    <link href="../../Graphics/Modules/TemplateMessages/css/TemplateMessages.css" rel="Stylesheet" />
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="TitleContent" runat="server">

</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="CPHservice" runat="server">
    <div id="content">
        <div class="contentouter">
            <div class="content">
                <asp:MultiView id="MLVtemplate" runat="server" ActiveViewIndex="0">
                    <asp:View ID="VIWtemplate" runat="server">
                        <div class="fieldobject title clearfix" id="DVtitle" runat="server" visible="true">
                            <div class="fieldrow inline left">
                                <label  for="">
                                    <asp:Literal ID="LTmessageInfo" runat="server"></asp:Literal>
                                    <asp:Literal ID="LTmessageInfoSentOn" runat="server"></asp:Literal>
                                    <asp:Literal ID="LTmessageInfoSentAt" runat="server"></asp:Literal>
                                </label>
                            </div>
                            <div class="fieldrow inline right">
                            </div>
                        </div>
                        <div class="fieldobject mailsettings" id="DVmailSettings" runat="server">
                            <CTRL:MailSettings id="CTRLmailSettings" runat="server" RaiseUpdateEvent="false" AllowSignatureFromTemplate="true" />
                        </div>
                        <div class="fieldobject">
                            <div class="fieldrow">
                                <div class="languagegroup clearfix">
                                    <CTRL:Selector ID="CTRLlanguageSelector" runat="server" RaiseSelectionEvent="true" />
                                </div>
                            </div>
                            <div class="fieldobject fieldeditor">
                                <div class="fieldrow subject readonly">
                                    <asp:Label ID="LBmessageSubject_t" runat="server" CssClass="fieldlabel" AssociatedControlID="LBmessageSubject">*Subject:</asp:Label>
                                    <asp:Label ID="LBmessageSubjectLanguage" runat="server" CssClass="templatelanguage"></asp:Label>&nbsp;
                                    <asp:Label ID="LBmessageSubject" runat="server" CssClass="readonlyinput"></asp:Label>
                                </div>
                                <div class="fieldrow editor">
                                    <asp:Label ID="LBmessage" runat="server" CssClass="readonlytextarea"></asp:Label>
                                </div>
                            </div>
                        </div>
                    </asp:View>
                    <asp:View id="VIWunknownTemplate" runat="server">
                        <div class="fieldobject">
                            <div class="fieldrow">
                                <asp:Label ID="LBnoTemplateFound" runat="server"></asp:Label>
                            </div>
                        </div>
                    </asp:View>
                </asp:MultiView>
            </div>
        </div>
    </div>
    <asp:Literal ID="LTplaceHolder" runat="server" Visible="false"><span class="renderplaceholder" title="{1}">{0}</span></asp:Literal>
</asp:Content>