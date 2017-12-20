using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel;
using lm.Comol.Modules.CallForPapers.Domain.Evaluation;

namespace lm.Comol.Modules.CallForPapers.Presentation.Evaluation
{
    public interface IViewInputCriterion : IViewBase
    {
        long IdCriterionEvaluated { get; set; }
        long IdCriterion { get; set; }
        Decimal MinValue { get; set; }
        Decimal MaxValue { get; set; }
        CriterionType CriterionType { get; set; }
        lm.Comol.Modules.CallForPapers.Domain.FieldError CurrentError { get; set; }
        Boolean Disabled { get; set; }
        Boolean isValid { get; }
        Boolean Mandatory { get; set; }
        long IdCall { get; set; }
        long IdEvaluation { get; set; }
        long IdSubmission { get; set; }
        List<dtoCriterionOption> AvailableOptions { get; set; }
        Int32 IdCallCommunity { get; set; }

        Int32 MaxOptions { get; set; }
        Int32 MinOptions { get; set; }

        void InitializeControl(long idCall, long idSubmission, long idEvaluation, Int32 idCommunity, dtoCriterionEvaluated criterion, Boolean disabled);
        void InitializeControl(long idCall, long idSubmission, long idEvaluation, Int32 idCommunity, dtoCriterionEvaluated criterion, Boolean disabled, lm.Comol.Modules.CallForPapers.Domain.FieldError err);
        dtoCriterionEvaluated GetCriterion();
        void SetupView(dtoCriterionEvaluated criterion, Int32 idCommunity);
        void DisplayInputError();
        void HideInputError();
        void DisplayEmptyCriterion();
    }
}