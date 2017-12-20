using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Modules.CallForPapers.Domain.Evaluation
{
    [Serializable]
    public enum EditingErrors
    {
        None = 0,
        NoEvaluators = 1,
        UnassingedEvaluators = 2,
        MoreEvaluatorAssignment = 4,
        NoCommitteeAvailable = 8,
        NoSubmitterTypeAssignments = 16,
        NoCriteria = 32,
        SubmittersTypeNotAssigned = 64,
        NoEvaluatorsForAssignments = 128,
        NoSubmissionToEvaluate = 256,
        SubmissionToAssign = 512,
        CommitteeWithNoEvaluators = 1024,
        CommitteeDssSettings = 2048
        //AddingCriterion = 16,
        //RemovingCriterion = 24,
        //AddingOption = 32,
        //RemovingOption = 64,
        //EvaluationLinked = 128,
        //AddingTemplate = 256,
        //RemovingTemplate = 512,
        //EditingTemplate = 1024,
        //SavingTemplateSettings = 2048,
        //AddingRequestedFile = 4096,
        //EditingRequestedFiles = 8192,
        //RemovingRequestedFiles = 16384,
    }
}
