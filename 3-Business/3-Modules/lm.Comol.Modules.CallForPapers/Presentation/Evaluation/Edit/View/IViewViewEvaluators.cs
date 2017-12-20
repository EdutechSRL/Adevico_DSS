using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel;
using lm.Comol.Modules.CallForPapers.Domain;
using lm.Comol.Modules.CallForPapers.Domain.Evaluation;

namespace lm.Comol.Modules.CallForPapers.Presentation.Evaluation
{
    public interface IViewViewEvaluators : IViewBaseEditEvaluationSettings
    {
        List<long> CommiteesWithEvaluationsCompleted { get; set; }
        void ReloadEditor(String url);
        void LoadCommitteesInfo(List<dtoCommitteeEvaluators> committees);
        void DisplayError(EvaluationEditorErrors err);
        void DisplayWarning(EvaluationEditorErrors err);
        void DisplaySettingsSaved();
        void HideErrorMessages();
        void SendUserAction(int idCommunity, int idModule, long idCall, ModuleCallForPaper.ActionType action);
    }
}