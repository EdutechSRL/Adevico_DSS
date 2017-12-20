using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel;

namespace lm.Comol.Modules.CallForPapers.Domain.Evaluation
{
    [Serializable]
    public class CallEvaluator : DomainBaseObjectLiteMetaInfo<long>
    {
        public virtual BaseForPaper Call { get; set; }
        public virtual litePerson Person { get; set; }
        public virtual IList<CommitteeMember> Memberships { get; set; }
        public virtual IList<Evaluation> Evaluations { get; set; }
        public CallEvaluator()
        {
            Memberships = new List<CommitteeMember>();
        }
    }
}