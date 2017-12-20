using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Core.Dashboard.Domain
{
    [Serializable]
    public class dtoContainerSettings 
    {
        public virtual IList<DashboardViewType> AvailableViews { get; set; }
        public virtual DefaultSettings Default { get; set; }
        public virtual IList<OrderItemsBy> AvailableOrderBy { get; set; }
        public virtual IList<GroupItemsBy> AvailableGroupBy { get; set; }

        public dtoContainerSettings() {
            AvailableViews = new List<DashboardViewType>();
            AvailableOrderBy = new List<OrderItemsBy>();
            AvailableGroupBy = new List<GroupItemsBy>();
            Default = new DefaultSettings();
        }
        //public ContainerSettings(DashboardType type, DashboardSettings dSettings = null): this()
        //{
        //    if (dSettings==null || (dSettings.Type ==  DashboardType.Portal && type != dSettings.Type)){
        //        AvailableViews =  GetAvailableView(type);
        //        AvailableOrderBy = GetAvailableOrderBy(type);
        //        AvailableGroupBy =  GetAvailableGroupBy(type);
        //        Default = new DefaultSettings(type, dSettings);
        //    }
        //    else{
        //        Default = dSettings.Container.Default;
        //        AvailableViews = dSettings.Container.AvailableViews;
        //        AvailableOrderBy = dSettings.Container.AvailableOrderBy;
        //        AvailableGroupBy = dSettings.Container.AvailableGroupBy;
        //    }
        //}

        public virtual List<DashboardViewType> GetAvailableView(DashboardType type)
        {
            switch (type)
            {
                case DashboardType.Portal:
                    return new List<DashboardViewType>() { DashboardViewType.Tile, DashboardViewType.Combined, DashboardViewType.List };
                case DashboardType.Community:
                case DashboardType.AllCommunities:
                    return new List<DashboardViewType>() { DashboardViewType.Tile };
            }
            return new List<DashboardViewType>();
        }
        public virtual List<OrderItemsBy> GetAvailableOrderBy(DashboardType type)
        {
            switch (type)
            {
                case DashboardType.Portal:
                    return new List<OrderItemsBy>() { OrderItemsBy.LastAccess, OrderItemsBy.CreatedOn, OrderItemsBy.ClosedOn, OrderItemsBy.Name };
                case DashboardType.Community:
                case DashboardType.AllCommunities:
                    return new List<OrderItemsBy>() { OrderItemsBy.ActivatedOn, OrderItemsBy.Name };
            }
            return new List<OrderItemsBy>();
        }
        public virtual List<GroupItemsBy> GetAvailableGroupBy(DashboardType type)
        {
            switch (type)
            {
                case DashboardType.Portal:
                    return new List<GroupItemsBy>() { GroupItemsBy.Tag, GroupItemsBy.CommunityType };
                case DashboardType.Community:
                case DashboardType.AllCommunities:
                    return new List<GroupItemsBy>() { GroupItemsBy.Service };
            }
            return new List<GroupItemsBy>();
        }
    }
}