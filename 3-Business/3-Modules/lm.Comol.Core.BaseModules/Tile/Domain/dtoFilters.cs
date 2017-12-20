using lm.Comol.Core.Dashboard.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Core.BaseModules.Tiles.Domain
{
    [Serializable]
    public class dtoFilters
    {
        public virtual DashboardType DashboardType { get; set; }
        public virtual Boolean FromRecycleBin { get; set; }
        public virtual Int32 IdCommunity { get; set; }
        public virtual Int32 IdModifiedBy { get; set; }
        public virtual AvailableStatus Status { get; set; }
        public virtual long IdTileType { get; set; }

        public virtual String Name { get; set; }
        public virtual String StartWith { get; set; }
        public virtual Boolean Ascending { get; set; }
        public virtual OrderTilesBy OrderBy { get; set; }
        public virtual Int32 IdSelectedLanguage { get; set; }

        public dtoFilters()
        {
            DashboardType = Core.Dashboard.Domain.DashboardType.Portal;
            IdModifiedBy = -1;
            Status = AvailableStatus.Any;
            StartWith = "";
            Name = "";
            Ascending = true;
            OrderBy = OrderTilesBy.Name;
            IdCommunity = 0;
            FromRecycleBin = false;
            IdSelectedLanguage = -1;
            IdTileType = -1;
        }
        public dtoFilters(DashboardType type, Int32 idCommunity, Boolean loadFromRecycleBin, List<lm.Comol.Core.DomainModel.Filters.Filter> filters, Int32 idLanguage =-1)
        {
            DashboardType = type;
            IdModifiedBy = -1;
            Status = (AvailableStatus)GetSingleValue(filters, searchFilterType.status, (long)AvailableStatus.Any);
            StartWith = "";
            Name = "";
            Ascending = true;
            OrderBy = OrderTilesBy.Name;
            IdCommunity = idCommunity;
            FromRecycleBin = loadFromRecycleBin;
            IdSelectedLanguage = idLanguage;
            IdTileType = (long)GetSingleValue(filters, searchFilterType.type, -1);
        }

        private long GetSingleValue(List<lm.Comol.Core.DomainModel.Filters.Filter> filters, searchFilterType type, long defaultValue = -1)
        {
            lm.Comol.Core.DomainModel.Filters.Filter filter = filters.Where(f => f.Name == type.ToString()).FirstOrDefault();
            if (filter == null)
                return defaultValue;
            else
            {
                switch (filter.FilterType)
                {
                    case DomainModel.Filters.FilterType.Select:
                        if (filter.Selected != null)
                            return filter.Selected.Id;
                        break;
                }
            }
            return defaultValue;
        }
    }
}