using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Modules.CallForPapers.Domain.Evaluation
{
    [Serializable]
    public enum SummaryType { 
        Evaluations = 0,
        Committees = 1,
        Committee = 2,
        EvaluationCommittee =3,
        EvaluationCommittees = 4
    }
}