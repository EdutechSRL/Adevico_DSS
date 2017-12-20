using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel;
using lm.Comol.Modules.CallForPapers.Domain;
using lm.Comol.Modules.CallForPapers.Domain.Evaluation;

namespace lm.Comol.Modules.CallForPapers.Presentation.Evaluation
{
    public interface IViewDeleteInEvaluationMembership : IViewBaseEditEvaluationSettings
    {
        long PreloadIdMembership { get; }
        long IdMembershipToRemove { get; set; }
        Boolean UseDss { get; set; }
        Boolean RemoveAll { get; set; }
        String AnonymousDisplayname { get; }
        String UnknownDisplayname { get; }

        void LoadEvaluationInfos(List<dtoBaseEvaluation> items);
        void DisplayMemberInfo(string name, long evaluated, long inevaluation, long notstarted);
        void DisplayMemberNotFound();
        //void DisplayReplaceNotAvailable();
        void ReloadEditor(String url);
        void LoadCommitteesInfo(List<dtoCommitteeEvaluators> committees);
        void DisplayError(EvaluationEditorErrors err);
        void DisplayWarning(EvaluationEditorErrors err);
        void DisplaySettingsSaved();
        void HideErrorMessages();
        void SendUserAction(int idCommunity, int idModule, long idCall, ModuleCallForPaper.ActionType action);
    }
}