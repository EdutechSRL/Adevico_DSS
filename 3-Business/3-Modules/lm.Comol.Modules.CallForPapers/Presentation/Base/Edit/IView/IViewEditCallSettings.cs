using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel;
using lm.Comol.Modules.CallForPapers.Domain;

namespace lm.Comol.Modules.CallForPapers.Presentation
{
    public interface IViewEditCallSettings : IViewEditCall
    {
        Boolean ForPortal { get; set; }
        Boolean AllowStatusEdit { get; set; }
        Boolean InvalidStatusFound { get; set; }
        dtoBaseForPaper CurrentCallForPaper { get; }

        void DisplaySettingsSaved();
        void DisplaySettingsError();
        void DisplayDateError(CallForPaperType type);
        void DisplayDateError(DateTime startDate,DateTime endDate);
        void DisplayEvaluationDateError(DateTime endDate, DateTime evaluationDate);
        void DisplaySkippedRequiredSteps(List<WizardCallStep> steps);

        void LoadCall(dtoBaseForPaper dtoCall, bool canEditAdvancedCommission);
        void LoadEvaluationSettings(dtoEvaluationSettings settings);
        void LoadEmptyCall();
        void LoadStatus(List<CallForPaperStatus> list);
        void LoadStatus(List<CallForPaperStatus> list, CallForPaperStatus selected);
        void LoadInvalidStatus(CallForPaperStatus status, DateTime? endDate);
    }
}