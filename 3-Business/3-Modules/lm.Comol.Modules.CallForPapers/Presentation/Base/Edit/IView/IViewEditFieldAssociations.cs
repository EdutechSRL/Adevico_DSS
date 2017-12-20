using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel;
using lm.Comol.Modules.CallForPapers.Domain;

namespace lm.Comol.Modules.CallForPapers.Presentation
{
    public interface IViewEditFieldAssociations : IViewBaseEditCall
    {
        List<TranslatedItem<lm.Comol.Core.Authentication.ProfileAttributeType>> AvailableAttributes { get; set; }
        void DisplayNoAssociations();
        void DisplaySettingsSaved();
        void DisplaySettingsUnSaved();
        void HideErrorMessages();
        void SendUserAction(int idCommunity, int idModule, long idCall, ModuleRequestForMembership.ActionType action);
        void SendUserAction(int idCommunity, int idModule, long idCall, ModuleCallForPaper.ActionType action);
        void LoadAssociations(List<dtoCallSection<dtoFieldAssociation>> associations);
        List<dtoFieldAssociation> GetAssociations();
    }
}