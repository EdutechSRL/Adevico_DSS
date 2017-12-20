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
    public interface IViewUserMessages : IViewModuleMessages
    {
        String RemovedUserName { get; }
        String UnknownUserName { get; }
        String PreloadMail { get; }
        Int32 PreloadIdPerson { get; }
        long PreloadIdUserModule { get; }
        dtoModuleMessagesContext ContainerContext { get; set; }
        PagerBase Pager { get; set; }
        Int32 CurrentPageSize { get; set; }
        //lm.Comol.Core.TemplateMessages.Domain.TemplateOrder CurrentOrderBy { get; set; }
        //Boolean CurrentAscending { get; set; }
        lm.Comol.Core.Mail.dtoRecipient GetRecipient(String moduleCode, long idUserModule);
        void LoadRecipientName(lm.Comol.Core.Mail.dtoRecipient recipient); 
        void LoadItems(List<dtoDisplayUserMessage> items);
        void DisplayMessagePreview(String languageCode, ItemObjectTranslation translation, lm.Comol.Core.MailCommons.Domain.Messages.MessageSettings mSettings, Int32 idCommunity, ModuleObject obj = null);
        void DisplayNoMessagesFound();
    }
}