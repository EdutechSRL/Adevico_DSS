using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel;

namespace lm.Comol.Modules.CallForPapers.Domain.Evaluation
{
    [Serializable]
    public class dtoCommitteeMember : dtoBaseCommitteeMember
    {
        public virtual Dictionary<long, long> IdMemberships { get; set; }
        public virtual List<long> Committees { get; set; }
        public dtoCommitteeMember()
        {
            IdMemberships = new Dictionary<long, long>();
            Committees = new List<long>();
        }
        public dtoCommitteeMember(Dictionary<long, long> info)
        {
            IdMemberships = info;
            Committees = info.Keys.ToList();
        }

        public long GetIdMemberships(long idCommittee){
            if (IdMemberships ==null || IdMemberships.ContainsKey(idCommittee))
                return 0;
            else
                return IdMemberships[idCommittee];
        }
    }
}