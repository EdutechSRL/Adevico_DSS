using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel;
using lm.Comol.Modules.CallForPapers.Domain;
using lm.Comol.Modules.CallForPapers.Domain.Evaluation;

namespace lm.Comol.Modules.CallForPapers.Presentation.Evaluation
{
    public interface IViewManageEvaluators : IViewBaseEditEvaluationSettings
    {
        Boolean AllowAddEvaluator { get; set; }
        Boolean AllowMultipleCommittees { get; set; }
        Boolean MultipleCommittees { get; set; }
        long IdOnlyOneCommittee { get; set; }
        List<dtoBaseCommittee> AvailableCommittees { get; set; }
        List<dtoCommitteeMember> GetMembers();
        void ReloadEditor(String url);

        void LoadEvaluators(List<dtoCommitteeMember> evaluators, Boolean multipleCommittee,List<dtoBaseCommittee> committees);
//        void LoadCommittees(List<dtoBaseCommittee> committees);
        void DisplayError(EvaluationEditorErrors err);
        void DisplayWarning(EvaluationEditorErrors err);
        void DisplaySettingsSaved();
        void HideErrorMessages();
        void SendUserAction(int idCommunity, int idModule, long idCall, ModuleCallForPaper.ActionType action);
    }
}