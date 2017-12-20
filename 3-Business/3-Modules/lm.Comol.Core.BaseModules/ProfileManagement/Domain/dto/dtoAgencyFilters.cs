using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Core.BaseModules.ProfileManagement
{
    [Serializable]
    public class dtoAgencyFilters
    {
        public virtual String Value { get; set; }
        public virtual String StartWith { get; set; }
        public virtual int PageSize { get; set; }
        public virtual int PageIndex { get; set; }
        public virtual Boolean Ascending { get; set; }
        public virtual OrderAgencyBy OrderBy { get; set; }
        public virtual SearchAgencyBy SearchBy { get; set; }
        public virtual AgencyAvailability Availability { get; set; }

    }
}