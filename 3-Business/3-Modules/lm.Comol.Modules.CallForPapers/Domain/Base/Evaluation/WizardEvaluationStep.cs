using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Modules.CallForPapers.Domain.Evaluation
{
    [Serializable]
    public enum WizardEvaluationStep
    {
        none = -1,
        GeneralSettings = 0,
        //GeneralInfo = 1,
        FullManageEvaluators = 2,
        ManageEvaluators = 3,
        AssignSubmission = 4,
        MultipleAssignSubmission = 5,
        ManageEvaluations = 6,
        AssignSubmissionWithNoEvaluation = 7
    }
}