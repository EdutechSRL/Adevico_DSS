using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel;

namespace lm.Comol.Modules.CallForPapers.Domain.Evaluation
{
    [Serializable]
    public class CommitteeMember : DomainBaseObjectLiteMetaInfo<long>
    {
        public virtual EvaluationCommittee Committee { get; set; }
        public virtual CallEvaluator Evaluator { get; set; }
        public virtual MembershipStatus Status { get; set; }
        public virtual litePerson ReplacedBy { get; set; }
        public virtual litePerson ReplacingUser { get; set; }
        public virtual CallEvaluator ReplacedByEvaluator { get; set; }
        public virtual CallEvaluator ReplacingEvaluator { get; set; }

        public CommitteeMember()
        {

        }
    }

    [Serializable]
    public enum MembershipStatus
    {
        Standard = 0,
        Replaced = 1,
        Replacing = 2,
        Removed = 3
    }
}