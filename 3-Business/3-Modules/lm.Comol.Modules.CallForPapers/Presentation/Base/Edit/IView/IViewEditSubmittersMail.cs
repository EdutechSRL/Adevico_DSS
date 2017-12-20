using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel;
using lm.Comol.Modules.CallForPapers.Domain;

namespace lm.Comol.Modules.CallForPapers.Presentation
{
    public interface IViewEditSubmittersMail : IViewBaseEditCall
    {
        Boolean AllowAdd { get; set; }
        List<long> InEditing { get; set; }
        List<dtoSubmitterType> Availablesubmitters { get; set; }
        List<dtoSubmitterTemplateMail> GetTemplates();


        void AddNewTemplate(Int32 number);
        void LoadTemplates(List<dtoSubmitterTemplateMail> templates);
        void LoadSubmitterTypes(List<dtoSubmitterType> submitters);
        void DisplayError(EditorErrors errors);
        void DisplayNoTemplates();
        void DisplaySettingsSaved(Boolean missing);
        void DisplayTemplateMissing();
        void DisplayTemplateAdded(Boolean missing);
        void DisplayTemplateRemoved();
        void DisplayMessagePreview(lm.Comol.Core.Mail.dtoMailMessagePreview previewItem, String recipient);
        void HideErrorMessages();
        void SendUserAction(int idCommunity, int idModule, long idCall, ModuleCallForPaper.ActionType action);
        void SendUserAction(int idCommunity, int idModule, long idCall, ModuleRequestForMembership.ActionType action);
    }
}