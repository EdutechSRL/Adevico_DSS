using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel;
using lm.Comol.Modules.CallForPapers.Domain.Evaluation;

namespace lm.Comol.Modules.CallForPapers.Presentation.Evaluation
{
    public interface IViewRenderCriterion : IViewBase
    {
        long IdCriterion { get; set; }
        CriterionType CriterionType { get; set; }
        lm.Comol.Modules.CallForPapers.Domain.FieldError CurrentError { get; set; }
        Boolean Disabled { get; set; }
        Int32 MaxChars { get; set; }
        Int32 MaxOptions { get; set; }
        Int32 MinOptions { get; set; }

        void InitializeControl(dtoCriterionEvaluated criterion, Boolean disabled, Boolean isPublic);
        void InitializeControl(dtoCriterionEvaluated criterion, Boolean disabled, Boolean isPublic, lm.Comol.Modules.CallForPapers.Domain.FieldError err);
        void SetupView(dtoCriterionEvaluated criterion, Boolean isPublic);
        void DisplayEmptyCriterion();
    }
}