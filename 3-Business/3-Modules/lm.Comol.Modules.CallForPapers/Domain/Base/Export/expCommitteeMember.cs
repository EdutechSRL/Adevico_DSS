using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel;

namespace lm.Comol.Modules.CallForPapers.Domain.Evaluation.Export
{
    [Serializable]
    public class expCommitteeMember : DomainObject<long>
    {
        public virtual long IdCommittee { get; set; }
        public virtual expEvaluator Evaluator { get; set; }
        public virtual MembershipStatus Status { get; set; }
        public virtual expPerson ReplacedBy { get; set; }
        public virtual expPerson ReplacingUser { get; set; }
        public virtual expEvaluator ReplacedByEvaluator { get; set; }
        public virtual expEvaluator ReplacingEvaluator { get; set; }
        public virtual BaseStatusDeleted Deleted { get; set; }
        public expCommitteeMember()
        {

        }
    }
}