using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Core.Tag.Domain
{
    [Serializable]
    public class dtoFilters
    {
        public virtual Boolean ForOrganization { get; set; }
        public virtual Boolean FromRecycleBin { get; set; }
        public virtual Int32 IdOrganization { get; set; }
        public virtual Int32 IdCreatedBy { get; set; }
        public virtual lm.Comol.Core.Dashboard.Domain.AvailableStatus Status { get; set; }

        public virtual String Name { get; set; }
        public virtual String StartWith { get; set; }
        public virtual Boolean Ascending { get; set; }
        public virtual OrderTagsBy OrderBy { get; set; }
        public virtual Int32 IdSelectedLanguage { get; set; }

        public dtoFilters(Boolean forOrganization)
        {
            IdOrganization = (forOrganization) ? -1 : -3;
            IdCreatedBy = -1;
            Status = lm.Comol.Core.Dashboard.Domain.AvailableStatus.Any;
            StartWith = "";
            Name = "";
            Ascending = true;
            OrderBy = OrderTagsBy.Name;
            ForOrganization = forOrganization;
            FromRecycleBin = false;
            IdSelectedLanguage = -1;
        }
    }
}