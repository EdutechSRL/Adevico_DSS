using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel;
using lm.Comol.Modules.CallForPapers.Domain;

namespace lm.Comol.Modules.CallForPapers.Presentation
{
    public interface IViewEditNotificationMail : IViewBaseEditCall
    {
        long IdTemplate { get; set; }

        dtoManagerTemplateMail GetDefaultTemplate { get; }
        void LoadTemplate(dtoManagerTemplateMail template);
        void DisplayNoTemplate();
        void DisplayErrorSaving();
        void DisplaySettingsSaved();
        void DisplayMessagePreview(lm.Comol.Core.Mail.dtoMailMessagePreview previewItem, String recipient);
        void HideErrorMessages();
        void SendUserAction(int idCommunity, int idModule, long idCall, ModuleCallForPaper.ActionType action);
        void SendUserAction(int idCommunity, int idModule, long idCall, ModuleRequestForMembership.ActionType action);
    }
}