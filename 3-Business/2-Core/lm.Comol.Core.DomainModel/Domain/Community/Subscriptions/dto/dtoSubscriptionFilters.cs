using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Core.Subscriptions
{
    [Serializable]
    public class dtoSubscriptionFilters
    {
        public virtual int IdOrganization { get; set; }
        public virtual int IdcommunityType { get; set; }
        public virtual int IdOwner { get; set; }
        //public virtual int Code { get; set; }
        //public virtual int Year { get; set; }

        public virtual lm.Comol.Core.DomainModel.SubscriptionStatus Status { get; set; }

        public virtual String Value { get; set; }
        public virtual String StartWith { get; set; }
        public virtual int PageSize { get; set; }
        public virtual int PageIndex { get; set; }
        public virtual Boolean Ascending { get; set; }
        public virtual OrderSubscriptionsBy OrderBy { get; set; }
        public virtual SearchSubscriptionsBy SearchBy { get; set; }
    }
}