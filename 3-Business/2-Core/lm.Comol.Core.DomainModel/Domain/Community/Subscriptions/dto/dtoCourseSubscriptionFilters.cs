using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Core.Subscriptions
{
    [Serializable]
    public class dtoCourseSubscriptionFilters : dtoSubscriptionFilters
    {
        public virtual int IdcommunityType { get; set; }
        public virtual int Code { get; set; }
        public virtual int Year { get; set; }
        public virtual int IdPeriod { get; set; }
        public virtual int IdDegree { get; set; }
    }
}