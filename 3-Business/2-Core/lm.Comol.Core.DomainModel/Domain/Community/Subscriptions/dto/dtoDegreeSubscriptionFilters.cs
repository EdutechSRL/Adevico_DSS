using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Core.Subscriptions
{
    [Serializable]
    public class dtoDegreeSubscriptionFilters : dtoSubscriptionFilters
    {
        public virtual int IdcommunityType { get; set; }
        public virtual int IdDegreeType { get; set; }
    }
}