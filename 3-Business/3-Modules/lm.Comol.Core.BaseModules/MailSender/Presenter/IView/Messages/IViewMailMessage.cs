using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel;
using lm.Comol.Core.DomainModel.Helpers;
using lm.Comol.Core.Mail;
using lm.Comol.Core.DomainModel.Repository;
using lm.Comol.Core.MailCommons.Domain.Messages;

namespace lm.Comol.Core.BaseModules.MailSender.Presentation
{
    public interface IViewMailMessage : lm.Comol.Core.DomainModel.Common.iDomainView 
    {
        
        PreviewMode CurrentMode { get; set; }
        Boolean EditAddressTo { get; set; }
        Boolean AllowSendMail { get; set; }
        Boolean DisplayOptions { get; set; }
        Boolean DisplayOtherRecipients { get; set; }
        Boolean isInitialized { get; set; }
        MessageSettings MailSettings { get; set; }
        String FromDisplayName { get; set; }
        String FromMailAddress { get; set; }


        void InitializeControlForPreview(lm.Comol.Core.Mail.dtoMailMessagePreview dtoContent, String recipients = "", List<dtoBaseGenericFile> attachments = null);
        void InitializeControlForPreview(String languageCode,lm.Comol.Core.DomainModel.Languages.ItemObjectTranslation content, List<lm.Comol.Core.TemplateMessages.Domain.ChannelSettings> channelSettings, Int32 idCommunity, ModuleObject obj = null );
        void InitializeControlForPreview(String languageCode, lm.Comol.Core.DomainModel.Languages.ItemObjectTranslation content, List<String> modules, lm.Comol.Core.MailCommons.Domain.Messages.MessageSettings settings, Int32 idCommunity, ModuleObject obj = null);
        void InitializeControlForPreview(String languageCode, lm.Comol.Core.DomainModel.Languages.ItemObjectTranslation content, List<String> modules, List<lm.Comol.Core.TemplateMessages.Domain.ChannelSettings> channelSettings, Int32 idCommunity, ModuleObject obj = null);
        lm.Comol.Core.DomainModel.Languages.ItemObjectTranslation ParseContent(Int32 idLanguage,String languageCode, lm.Comol.Core.DomainModel.Languages.ItemObjectTranslation content, List<String> modules, Int32 idCommunity, String communityName, Person currentUser, String organizationName, ModuleObject obj = null);
        lm.Comol.Core.DomainModel.Languages.ItemObjectTranslation ParseContent(Int32 idLanguage, String languageCode, lm.Comol.Core.DomainModel.Languages.ItemObjectTranslation content, List<String> modules, Community community, Person currentUser, String organizationName, ModuleObject obj = null);

        void LoadDisplayItems(List<DisplayItem> items, Boolean switchOptionsOn, List<DisplayItem> onItems);
        String GetPortalName(Int32 idLanguage);

        void LoadPreviewMessage(PreviewMode pMode, dtoMailMessagePreview dtoContent,  String recipients, List<dtoBaseGenericFile> attachments = null);
        void LoadPreviewTemplateMessage(PreviewMode pMode, dtoMailMessagePreview dtoContent, String recipients);
        void LoadPreviewTemplateMessage(PreviewMode pMode, lm.Comol.Core.DomainModel.Languages.ItemObjectTranslation content);
        void HideContent();
    }
}