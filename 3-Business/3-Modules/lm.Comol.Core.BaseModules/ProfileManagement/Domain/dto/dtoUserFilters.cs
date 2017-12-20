using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel;

namespace lm.Comol.Core.BaseModules.ProfileManagement
{
    [Serializable]
    public class dtoUserFilters : ICloneable
    {
        public virtual List<Int32> IdCommunities { get; set; }
        public virtual int IdRole { get; set; }
        public virtual int IdProfileType { get; set; }
        public virtual SubscriptionStatus Status { get; set; }
        public virtual long IdAgency { get; set; }

        public virtual String Value { get; set; }
        public virtual String StartWith { get; set; }
        public virtual int PageSize { get; set; }
        public virtual int PageIndex { get; set; }
        public virtual Boolean Ascending { get; set; }
        public virtual OrderUsersBy OrderBy { get; set; }
        public virtual SearchProfilesBy SearchBy { get; set; }

        public dtoUserFilters() {
            IdCommunities = new List<Int32>();
        }
        public object Clone()
        {
            dtoUserFilters filter = new dtoUserFilters();
            filter.IdCommunities = IdCommunities;
            filter.IdRole = IdRole;
            filter.IdProfileType = IdProfileType;
            filter.Status = Status;
            filter.IdAgency = IdAgency;
            filter.Value = Value;
            filter.StartWith = StartWith;
            filter.PageSize = PageSize;
            filter.PageIndex = PageIndex;
            filter.Ascending = Ascending;
            filter.OrderBy = OrderBy;
            filter.SearchBy = SearchBy;
            filter.IdCommunities = IdCommunities;
            return filter;
        }
    }
}