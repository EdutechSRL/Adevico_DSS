using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel;

namespace lm.Comol.Modules.CallForPapers.Domain.Evaluation
{
    [Serializable]
    public class dtoEvaluationStep 
    {
        public virtual WizardEvaluationStep Type { get; set; }
        public virtual String Message { get; set; }
        public virtual List<EditingErrors> Errors { get; set; }
        public lm.Comol.Core.Wizard.WizardItemStatus Status { get; set; }
        public dtoEvaluationStep() {
            Message = "";
            Errors = new List<EditingErrors>();
            Status = Core.Wizard.WizardItemStatus.none;
        }
        public dtoEvaluationStep(WizardEvaluationStep type) :this()
        {
            Type = type;
            Errors = new List<EditingErrors>();
        }
    }
    [Serializable]
    public class dtoEvaluationCommitteeStep :dtoEvaluationStep
    {
        public virtual long ItemsCount { get; set; }
        public virtual long CriteriaCount { get; set; }
        public virtual Dictionary<long, long> CommitteesCriteriaCount { get; set; }
        public dtoEvaluationCommitteeStep() :base()
        {
            CommitteesCriteriaCount = new Dictionary<long, long>();
        }
        public dtoEvaluationCommitteeStep(WizardEvaluationStep type)
                : base(type)
        {
            CommitteesCriteriaCount = new Dictionary<long, long>();
        }
    }
    [Serializable]
    public class dtoEvaluationEvaluatorsStep : dtoEvaluationStep
    {
        public virtual long ItemsCount { get; set; }
        public dtoEvaluationEvaluatorsStep()
            : base()
        {
        }
        public dtoEvaluationEvaluatorsStep(WizardEvaluationStep type)
            : base(type)
        {
        }
        public dtoEvaluationEvaluatorsStep(WizardEvaluationStep type, lm.Comol.Core.Wizard.WizardItemStatus status)
            : base(type)
        {
            Status = status;
        }
    }
    [Serializable]
    public class dtoEvaluationViewEvaluatorsStep : dtoEvaluationStep
    {
        public virtual long CommitteesCount { get; set; }
        public virtual Dictionary<MembershipStatus, long> Counters { get; set; }
        public virtual long EvaluatorsCount { get; set; }
        public dtoEvaluationViewEvaluatorsStep()
            : base()
        {
        }
        public dtoEvaluationViewEvaluatorsStep(WizardEvaluationStep type)
            : base(type)
        {
        }
        public dtoEvaluationViewEvaluatorsStep(WizardEvaluationStep type, lm.Comol.Core.Wizard.WizardItemStatus status)
            : base(type)
        {
            Status = status;
        }
        public long GetMembershipsCount(MembershipStatus status)
        {
            return (Counters == null || !Counters.ContainsKey(status)) ? 0 : Counters[status];
        }
        public long GetMembershipsCount()
        {

            return (Counters == null ? 0 : Counters.Values.Select(v => v).Sum());
        }
    }
    [Serializable]
    public class dtoEvaluationSingleAssignmentStep : dtoEvaluationStep
    {
        public virtual long ItemsCount { get; set; }
        public virtual long AcceptedCount { get; set; }
        public virtual long RejectedCount { get; set; }
        public dtoEvaluationSingleAssignmentStep()
            : base()
        {
        }
        public dtoEvaluationSingleAssignmentStep(WizardEvaluationStep type)
            : base(type)
        {
        }
        public dtoEvaluationSingleAssignmentStep(WizardEvaluationStep type, lm.Comol.Core.Wizard.WizardItemStatus status)
            : base(type)
        {
            Status = status;
        }
    }
    [Serializable]
    public class dtoEvaluationMultipleAssignmentStep : dtoEvaluationStep
    {
        public virtual long ItemsCount { get; set; }
        public virtual long AcceptedCount { get; set; }
        public virtual long RejectedCount { get; set; }
        public dtoEvaluationMultipleAssignmentStep()
            : base()
        {
        }
        public dtoEvaluationMultipleAssignmentStep(WizardEvaluationStep type)
            : base(type)
        {
        }
        public dtoEvaluationMultipleAssignmentStep(WizardEvaluationStep type, lm.Comol.Core.Wizard.WizardItemStatus status)
            : base(type)
        {
            Status = status;
        }
    }
    [Serializable]
    public class dtoEvaluationsManageStep : dtoEvaluationStep
    {
        public virtual long ItemsCount { get; set; }
        public virtual long EvaluatedCount { get; set; }
        public virtual long EvaluatingCount { get; set; }
        public virtual long NotStartedCount { get; set; }
        public virtual List<ManageEvaluationsAction> GetAvailableActions()
        {
            List<ManageEvaluationsAction> items = new List<ManageEvaluationsAction>();
            if (EvaluatedCount > 0)
                items.Add(ManageEvaluationsAction.OpenAll);
            if (EvaluatingCount > 0 || NotStartedCount > 0)
                items.Add(ManageEvaluationsAction.CloseAll);
            return items;
        }
        public dtoEvaluationsManageStep()
            : base()
        {
        }
        public dtoEvaluationsManageStep(WizardEvaluationStep type)
            : base(type)
        {
        }
        public dtoEvaluationsManageStep(WizardEvaluationStep type, lm.Comol.Core.Wizard.WizardItemStatus status)
            : base(type)
        {
            Status = status;
        }
    }
    [Serializable]
    public class dtoAssignSubmissionWithNoEvaluationtStep : dtoEvaluationStep
    {
        public virtual long SubmittedCount { get; set; }
        public virtual long AcceptedCount { get; set; }
        public virtual long RejectedCount { get; set; }
        public virtual long InEvaluations { get; set; }
        public virtual long NotEvaluated { get; set; }
        public virtual long DraftItems { get; set; }
        public virtual long ItemsCount { get { return SubmittedCount + AcceptedCount + RejectedCount ; } }
        public dtoAssignSubmissionWithNoEvaluationtStep()
            : base()
        {
        }
        public dtoAssignSubmissionWithNoEvaluationtStep(WizardEvaluationStep type)
            : base(type)
        {
        }
        public dtoAssignSubmissionWithNoEvaluationtStep(WizardEvaluationStep type, lm.Comol.Core.Wizard.WizardItemStatus status)
            : base(type)
        {
            Status = status;
        }
    }
}