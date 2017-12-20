using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel;
using lm.Comol.Core.Mail;
using lm.Comol.Core.Mail.Messages;
using lm.Comol.Core.DomainModel.Languages;

namespace lm.Comol.Core.BaseModules.MailSender.Presentation
{
    public interface IViewMessageTemplate : IViewModuleMessages
    {

        long PreloadIdMessageTemplate { get; }
        String TagTranslation { get; }
        dtoTemplateMessageContext ContainerContext { get; set; }
        List<lm.Comol.Core.TemplateMessages.Domain.dtoTemplateTranslation> CurrentTranslations { get; set; }
        List<lm.Comol.Core.DomainModel.Helpers.TemplatePlaceHolder> GetContentPlaceHolders(List<string> modulesCodes);
        List<String> ContentModules { get; set; }

        void DisplayMessageInfo(litePerson createdBy, DateTime createdOn);
        void InitializeMailSettings(lm.Comol.Core.MailCommons.Domain.Messages.MessageSettings settings, Boolean editSender, Boolean editSubject, Boolean editSignature);
        void InitializeControls(List<lm.Comol.Core.TemplateMessages.Domain.dtoTemplateTranslation> tTemplates, List<LanguageItem> inUse, LanguageItem current);
        void LoadMessage(lm.Comol.Core.TemplateMessages.Domain.dtoTemplateTranslation content);
        void DisplayNoTemplateFound();
    }
}