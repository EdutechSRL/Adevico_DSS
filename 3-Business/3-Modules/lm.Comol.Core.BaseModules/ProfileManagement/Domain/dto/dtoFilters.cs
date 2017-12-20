using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Core.BaseModules.ProfileManagement
{
    [Serializable]
    public class dtoFilters: ICloneable
    {
        //public virtual Boolean AllOrganizations { get; set; }
        public virtual List<int> IdAvailableOrganization { get; set; }
        public virtual int IdOrganization { get; set; }
        public virtual int IdProfileType { get; set; }
        public virtual StatusProfile Status { get; set; }
        public virtual long idProvider { get; set; }
        public virtual long IdAgency { get; set; }

        public virtual String Value { get; set; }
        public virtual String StartWith { get; set; }
        public virtual int PageSize { get; set; }
        public virtual int PageIndex { get; set; }
        public virtual Boolean Ascending { get; set; }
        public virtual Boolean DisplayLoginInfo { get; set; }
        public virtual OrderProfilesBy OrderBy { get; set; }
        public virtual SearchProfilesBy SearchBy { get; set; }

        public object Clone()
        {
            dtoFilters filter = new dtoFilters();
            filter.IdAvailableOrganization = IdAvailableOrganization;
            filter.IdOrganization = IdOrganization;
            filter.IdProfileType = IdProfileType;
            filter.Status = Status;
            filter.idProvider = idProvider;
            filter.IdAgency = IdAgency;
            filter.Value = Value;
            filter.StartWith = StartWith;
            filter.PageSize = PageSize;
            filter.PageIndex = PageIndex;
            filter.Ascending = Ascending;
            filter.DisplayLoginInfo = DisplayLoginInfo;
            filter.OrderBy = OrderBy;
            filter.SearchBy = SearchBy;
            return filter;
        }
    }
}