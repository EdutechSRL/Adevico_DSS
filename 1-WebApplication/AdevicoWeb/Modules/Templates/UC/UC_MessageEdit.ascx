<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="UC_MessageEdit.ascx.vb" Inherits="Comunita_OnLine.UC_MessageEdit" %>
<%@ Register TagPrefix="CTRL" TagName="MailSettings" Src="~/Modules/Common/UC/UC_MailSettings.ascx" %>
<%@ Register TagPrefix="CTRL" TagName="Selector" Src="~/Modules/Common/UC/UC_ContentTranslationSelector.ascx" %>
<%@ Register TagPrefix="CTRL" TagName="Translator" Src="~/Modules/Common/UC/UC_ContentTranslator.ascx" %>
<%@ Register TagPrefix="CTRL" TagName="TemplateSelector" Src="~/Modules/Templates/UC/UC_TemplateAssociation.ascx" %>
<%@ Register TagPrefix="CTRL" TagName="Messages" Src="~/Modules/Common/UC/UC_ActionMessages.ascx" %>
<%@ Register TagPrefix="CTRL" TagName="CallSelector" Src="~/Modules/CallForPapers/UC/UC_SelectUsersForMessageService.ascx" %>

<%@ Register Src="~/Modules/Common/UC/UC_MailMessage.ascx" TagName="CTRLmailMessage" TagPrefix="CTRL" %>
<asp:MultiView ID="MLVcontrol" runat="server">
    <asp:View ID="VIWsessionTimeout" runat="server"></asp:View>
    <asp:View ID="VIWeditor" runat="server">
        <div class="fieldobject title clearfix" id="DVtitle" runat="server" visible="true">
            <div class="fieldrow inline left">
                <label  for=""><asp:Literal ID="LTmessageEdit_t" runat="server"></asp:Literal></label>
            </div>
            <div class="fieldrow inline right">
               <%-- <div class="ddbuttonlist enabled" id="DVsaveButtons" runat="server"><!--
                    --><asp:LinkButton ID="LNBsaveAsTemplate" runat="server" Text="*Save template" CssClass="linkMenu" Visible="false"/><!--
                    --><asp:LinkButton ID="LNBsaveAsObjectTemplate" runat="server" Text="*Save object template" CssClass="linkMenu" Visible="false"/><!--
                    --><asp:LinkButton ID="LNBsaveAsPersonalTemplate" runat="server" Text="*Save as personal template" CssClass="linkMenu" Visible="false"/><!--
                --></div>--%>
            </div>
        </div>
        <CTRL:Messages ID="CTRLmessages"  runat="server" Visible="false" />
        <div class="fieldobject clearfix" id="DVtemplateSelector" runat="server" visible="false">
            <div class="fieldrow templateselector">
                <asp:Label ID="LBinuseTemplate_t" runat="server" CssClass="fieldlabel">*Template:</asp:Label>
                <CTRL:TemplateSelector id="CTRLtemplateSelector" runat="server" RaiseSelectionEvent="true" />
            </div>
        </div>
        <div class="fieldobject mailsettings" id="DVmailSettings" runat="server" visible="false">
            <CTRL:MailSettings id="CTRLmailSettings" runat="server" RaiseUpdateEvent="false" AllowSignatureFromTemplate="true" />
        </div>
        <div class="fieldobject">
            <div class="fieldrow">
                <div class="languagegroup clearfix">
                    <CTRL:Selector ID="CTRLlanguageSelector" runat="server" RaiseSelectionEvent="true" />
                </div>
            </div>
        </div>
        <div class="fieldobject fieldeditor">
            <CTRL:Translator ID="CTRLtranslator" runat="server" ShowName="false" ShowSubject="true"  />
        </div>
        <div class="fieldobject">
            <div class="footer">
                <div class="DivEpButton">
                    <asp:button ID="BTNdeleteCurrentTranslation" CssClass="needconfirm" runat="server" Text="*Delete" Visible="false"/>
                    <asp:button ID="BTNtemplateMessagePreview" runat="server" Text="*Preview" Visible="false"/>
                    <asp:button ID="BTNselectRecipients" runat="server" Text="*Recipients" Visible="false"/>
                </div>
            </div>
        </div>
        <div class="view-modal view-previewmail" id="DVpreview" runat="server" visible="false">
            <CTRL:CTRLmailMessage ID="CTRLmailpreview" runat="server" AllowSendMail="true" DisplayTopWindowCloseButton="false" EditAddressTo="true"  />
            <div class="fieldobject clearfix">
                <div class="fieldrow right">
                    <asp:Button ID="BTNcloseMailMessageWindow" runat="server" CssClass="Link_Menu" />
                </div>
            </div>
        </div>
    </asp:View>
    <asp:View ID="VIWusersSelection" runat="server">
        
        <CTRL:Messages ID="CTRLrecipientsMessages"  runat="server" Visible="false" />
        <div class="fieldobject title clearfix" id="DVmanualSelection" runat="server" visible="false">
            <div class="fieldrow">
                <label  for=""><asp:Literal ID="LTrecipientManualSelection_t" runat="server"></asp:Literal></label>
            </div>
        </div>
        <div class="fieldobject mailaddressgroup" id="DVmailRecipients" runat="server" visible="false">
            <asp:Repeater ID="RPTrecipients" runat="server">
                <ItemTemplate>
                    <div class="fieldrow mailaddress">
                        <asp:Literal ID="LTidLanguage" runat="server" Visible="false" Text='<%#Container.DataItem.Id %>'></asp:Literal>
                        <asp:Literal ID="LTcodeLanguage" runat="server" Visible="false" Text='<%#Container.DataItem.Code %>'></asp:Literal>
                        <label class="fieldlabel" for="">
                            <asp:Literal ID="LTmailAddressLanguage_t" runat="server">*Addresses</asp:Literal>
                            <asp:Label ID="LBlanguageCode" runat="server" CssClass="templatelanguage"></asp:Label>&nbsp;:
                        </label>
                        <asp:TextBox ID="TXBaddresses" runat="server" CssClass="fieldinput tokeninputmail"></asp:TextBox>
                        <span class="fieldinfo showiferror">
                            <span class="message">
                                <asp:Literal id="LTmailAddressInvalidErrorsInfo" runat="server"></asp:Literal>                            
                                <asp:HyperLink id="HYPhideErrors" CssClass="hideerrors" runat="server">*clicca qui per nascondere</asp:HyperLink>
                            </span>
                            <span class="details"></span>
                        </span>
                    </div>  
                </ItemTemplate>
            </asp:Repeater>
        </div>
        <div class="fieldobject title clearfix" id="DVinternalRecipients" runat="server">
            <div class="fieldrow">
                <label  for=""><asp:Literal ID="LTrecipientSelection_t" runat="server"></asp:Literal></label>
            </div>
        </div>
        <CTRL:CallSelector ID="CTRLcallUsersSelection" runat="server" Visible="false" />
       
        
        <div class="fieldobject">
            <div class="footer">
                <div class="DivEpButton">
                    <asp:button ID="BTNbackToMessage" runat="server" Text="*edit message"/>
                    <asp:button ID="BTNsendMessage" runat="server" Text="*send message"/>
                </div>
            </div>
        </div>
    </asp:View>
    <asp:View ID="VIWmessageSent" runat="server">
        <div class="fieldobject title clearfix">
            <div class="fieldrow">
                <label  for=""><asp:Literal ID="LTmessageSentInfos_t" runat="server"></asp:Literal></label>
            </div>
        </div>
        <CTRL:Messages ID="CTRLmessageSent"  runat="server" Visible="false" />
        <div class="fieldobject">
            <div class="footer">
                <div class="DivEpButton">
                    <asp:button ID="BTNbackToRecipients" runat="server" Text="*To recipients" Visible="false"/>
                    <asp:button ID="BTNnewMessage" runat="server" Text="*New message" Visible="true"/>
                </div>
            </div>
        </div>
    </asp:View>
</asp:MultiView>