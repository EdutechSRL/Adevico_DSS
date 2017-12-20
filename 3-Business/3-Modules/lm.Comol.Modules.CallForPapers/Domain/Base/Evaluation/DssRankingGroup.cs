using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel;

namespace lm.Comol.Modules.CallForPapers.Domain.Evaluation
{
    [Serializable]
    public class DssRankingGroup : DomainBaseObjectIdLiteMetaInfo<long>
    {
        public virtual long IdCall { get; set; }
        public virtual String Name { get; set; }
        public virtual IList<DssRankingGroupItem> Items { get; set; }

        public DssRankingGroup()
        {
            Deleted = BaseStatusDeleted.None;
            Items = new List<DssRankingGroupItem>();
        }
    }

    [Serializable]
    public class DssRankingGroupItem : DomainBaseObject<long>
    {
        public virtual long IdRankingGroup { get; set; }
        public virtual long IdCall { get; set; }
        public virtual long IdSubmitterType { get; set; }

        public DssRankingGroupItem()
        {
            Deleted = BaseStatusDeleted.None;
        }
    }
}