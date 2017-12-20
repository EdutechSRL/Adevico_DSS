using lm.Comol.Core.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Core.Tag.Domain
{
    [Serializable]
    public class dtoCommunityTags
    {
        public virtual List<long> Tags { get; set; }
        public virtual Int32 IdCommunity { get; set; }
        public dtoCommunityTags()
        {
            Tags = new List<long>();
        }
        public Boolean HasTag(long idTag)
        {
            return Tags != null && Tags.Contains(idTag);
        }
        public Boolean HasTags(List<long> idTags)
        {
            return Tags != null && !idTags.Except(Tags).Any();
        }
    }
}