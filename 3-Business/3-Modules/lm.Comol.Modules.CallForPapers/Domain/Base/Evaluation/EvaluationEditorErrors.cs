using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Modules.CallForPapers.Domain.Evaluation
{
    [Serializable]
    public enum EvaluationEditorErrors
    {
        None = 0,
        NoCommittees = 1,
        Saving = 2,
        AddingCommittee = 4,
        RemovingCommittee = 8,
        AddingCriterion = 16,
        RemovingCriterion = 24,
        AddingOption = 32,
        RemovingOption = 64,
        EvaluationLinked = 128,
        NoEvaluators = 256,
        AddingEvaluators = 512,
        RemovingEvaluators = 1024,
        UnassignedEvaluators = 2048,
        CommitteeWithNoEvaluators = 4096,
        MoreEvaluatorAssignment = 8192,
        NoEvaluatorsForAssignments= 16384,
        NoSubmissionToEvaluate= 32768,
        SubmissionEvaluated= 65536,
        SubmissionToAssign = 131072,
        ReplacingEvaluator = 262144,
        RemovingInEvaluationEvaluator = 524288,
        SavingEvaluation = 1048576,
        SavingEvaluationCompleted = 2097152,
        DssSettings = 4194304

        //SavingTemplateSettings = 2048,
        //AddingRequestedFile = 4096,
        //EditingRequestedFiles = 8192,
        //RemovingRequestedFiles = 16384,
    }
   
}
