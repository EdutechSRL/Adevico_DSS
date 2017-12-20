using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel;
using lm.Comol.Modules.CallForPapers.Domain;

namespace lm.Comol.Modules.CallForPapers.Presentation
{
    public interface IViewCallEditor : IViewBaseEditCall
    {
        String DefaultSectionName {get;}
        String DefaultSectionDescription { get; }
        List<dtoSubmitterType> Availablesubmitters { get; set; }


       // void InitializeAddFieldControl(long idCall);
        List<dtoCallSection<dtoCallField>> GetSections();
        void ReloadEditor(String url);
        void LoadSections(List<dtoCallSection<dtoCallField>> sections);
        void LoadSubmitterTypes(List<dtoSubmitterType> submitters);
        void DisplayError(EditorErrors err);
        void DisplaySettingsSaved();
        void HideErrorMessages();
        void SendUserAction(int idCommunity, int idModule, long idCall, ModuleCallForPaper.ActionType action);
        void SendUserAction(int idCommunity, int idModule, long idCall, ModuleRequestForMembership.ActionType action);

        String TagCurrent { get; set; }
        
    }
}