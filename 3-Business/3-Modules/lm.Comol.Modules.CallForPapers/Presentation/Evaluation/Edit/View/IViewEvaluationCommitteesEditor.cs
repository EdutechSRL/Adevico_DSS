using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel;
using lm.Comol.Modules.CallForPapers.Domain;
using lm.Comol.Modules.CallForPapers.Domain.Evaluation;

namespace lm.Comol.Modules.CallForPapers.Presentation.Evaluation
{
    public interface IViewEvaluationCommitteesEditor : IViewBaseEditEvaluationSettings
    {
     
        Int32 CommitteesCount { get; set; }
        Boolean AllowSaveBaseInfo { get; set; }
        Boolean AllowSubmittersSelection { get; set; }
        Boolean AllowUseOfDssMethods { get; }
        Boolean UseDssMethods { get; set; }
        String DefaultCommitteeName {get;}
        String DefaultCommitteeDescription { get; }
        List<dtoSubmitterType> AvailableSubmitters { get; set; }
        List<lm.Comol.Core.Dss.Domain.Templates.dtoSelectMethod> CurrentMethods { get; set; }
        List<dtoCommittee> GetCommittees();
        Dictionary<long, lm.Comol.Core.Dss.Domain.Templates.dtoItemMethodSettings> GetCommiteesDssMethods();
        lm.Comol.Core.Dss.Domain.Templates.dtoItemMethodSettings GetCallDssSettings();
        void ReloadEditor(String url);
        void LoadCommittees(List<dtoCommittee> items);
        void LoadSubmitterTypes(List<dtoSubmitterType> submitters);
        void DisplayError(EvaluationEditorErrors err);
        void DisplayDssErrors(List<dtoCommittee> commitees);
        void DisplaySettingsSaved();
        void HideErrorMessages();
        void SendUserAction(int idCommunity, int idModule, long idCall, ModuleCallForPaper.ActionType action);
        void InitializeAggregationMethods(List<lm.Comol.Core.Dss.Domain.Templates.dtoSelectMethod> methods, long idDssMethod, long idRatingSet, List<lm.Comol.Core.Dss.Domain.Templates.dtoItemWeightBase> weightItems);
        void HideCallAggregationMethods(List<lm.Comol.Core.Dss.Domain.Templates.dtoSelectMethod> methods, long idDssMethod, long idRatingSet, List<lm.Comol.Core.Dss.Domain.Templates.dtoItemWeightBase> weightItems);
    }
}