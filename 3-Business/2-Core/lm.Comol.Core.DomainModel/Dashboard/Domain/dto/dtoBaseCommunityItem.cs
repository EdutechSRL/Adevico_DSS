using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Core.Dashboard.Domain
{
    [Serializable]
    public class dtoBaseCommunityItem : lm.Comol.Core.DomainModel.DomainObject<Int32>
    {
        public Int32 Id {get;set;}
        public String Name {get;set;}
        public Int32 IdOrganization { get; set; }
        public Int32 IdType { get; set; }
        public lm.Comol.Core.Communities.CommunityStatus Status { get; set; }
        public List<String> Tags { get; set; }
        public List<long> IdTags { get; set; }

        public dtoBaseCommunityItem() {
            Tags = new List<String>();
            IdTags = new List<long>();
        }
    }
}