using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Core.Dashboard.Domain
{
    [Serializable]
    public class DefaultSettings : ICloneable, IDisposable, ISettingsBase
    {
        public virtual DashboardViewType View { get; set; }
        public virtual DisplaySearchItems Search { get; set; }
        public virtual DisplayNoticeboard DefaultNoticeboard { get; set; }
        public virtual DisplayNoticeboard TileNoticeboard { get; set; }
        public virtual DisplayNoticeboard ListNoticeboard { get; set; }
        public virtual DisplayNoticeboard CombinedNoticeboard { get; set; }
        public virtual OnLoadSettings AfterUserLogon { get; set; }
        public virtual OrderItemsBy OrderBy { get; set; }
        public virtual GroupItemsBy GroupBy { get; set; }

        public DefaultSettings()
        {
            View =  DashboardViewType.List;
            Search =  DisplaySearchItems.Simple;
            DefaultNoticeboard = DisplayNoticeboard.OnRight;
            TileNoticeboard = DisplayNoticeboard.InheritsFromDefault;
            ListNoticeboard = DisplayNoticeboard.InheritsFromDefault;
            CombinedNoticeboard = DisplayNoticeboard.InheritsFromDefault;
            AfterUserLogon = OnLoadSettings.AlwaysDefault;
            OrderBy = OrderItemsBy.LastAccess;
            GroupBy = GroupItemsBy.None;
        }
        public DefaultSettings(DashboardType type, DashboardSettings dSettings = null)
        {
            if (dSettings == null || (dSettings.Type == DashboardType.Portal && type != dSettings.Type))
            {
                switch (type) { 
                    case DashboardType.Portal:
                        View =  DashboardViewType.Tile;
                        Search =  DisplaySearchItems.Simple;
                        DefaultNoticeboard = DisplayNoticeboard.OnRight;
                        TileNoticeboard = DisplayNoticeboard.InheritsFromDefault;
                        ListNoticeboard = DisplayNoticeboard.InheritsFromDefault;
                        CombinedNoticeboard = DisplayNoticeboard.InheritsFromDefault;
                        AfterUserLogon = OnLoadSettings.AlwaysDefault;
                        OrderBy = OrderItemsBy.LastAccess;
                        GroupBy = GroupItemsBy.Tile;
                        break;
                    case DashboardType.Community:
                    case DashboardType.AllCommunities:
                        View = DashboardViewType.Tile;
                        Search =  DisplaySearchItems.Hide;
                        DefaultNoticeboard = DisplayNoticeboard.OnRight;
                        TileNoticeboard = DisplayNoticeboard.InheritsFromDefault;
                        ListNoticeboard = DisplayNoticeboard.InheritsFromDefault;
                        CombinedNoticeboard = DisplayNoticeboard.InheritsFromDefault;
                        AfterUserLogon = OnLoadSettings.AlwaysDefault;
                        OrderBy = OrderItemsBy.Name;
                        GroupBy = GroupItemsBy.None;
                        break;
                }
            }
            else
            {
                View = dSettings.Container.Default.View;
                Search = dSettings.Container.Default.Search;
                DefaultNoticeboard = dSettings.Container.Default.DefaultNoticeboard;
                TileNoticeboard = dSettings.Container.Default.TileNoticeboard;
                ListNoticeboard = dSettings.Container.Default.ListNoticeboard;
                CombinedNoticeboard = dSettings.Container.Default.CombinedNoticeboard;
                AfterUserLogon = dSettings.Container.Default.AfterUserLogon;
                OrderBy = dSettings.Container.Default.OrderBy;
                GroupBy = dSettings.Container.Default.GroupBy;
            }
        }

        public virtual object Clone()
        {
            DefaultSettings clone = new DefaultSettings();
            clone.View = View;
            clone.Search = Search;
            clone.DefaultNoticeboard = DefaultNoticeboard;
            clone.TileNoticeboard = TileNoticeboard;
            clone.ListNoticeboard = ListNoticeboard;
            clone.CombinedNoticeboard = CombinedNoticeboard;
            clone.AfterUserLogon = AfterUserLogon;
            clone.OrderBy = OrderBy;
            clone.GroupBy = GroupBy;
            return clone;
        }

        public void Dispose()
        {
            
        }
        public virtual DisplayNoticeboard GetNoticeboard(DashboardViewType view)
        {
            DisplayNoticeboard current = DefaultNoticeboard;
            if (current == DisplayNoticeboard.DefinedOnAllPages)
            {
                switch (view)
                {
                    case DashboardViewType.Combined:
                        current = CombinedNoticeboard;
                        break;
                    case DashboardViewType.List:
                        current = ListNoticeboard;
                        break;
                    case DashboardViewType.Tile:
                        current = TileNoticeboard;
                        break;
                }
                if (current == DisplayNoticeboard.InheritsFromDefault)
                    current = DisplayNoticeboard.Hide;
            }
            return current;
        }
    }
}