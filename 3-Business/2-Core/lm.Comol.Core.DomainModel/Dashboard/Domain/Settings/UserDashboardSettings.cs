using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Core.Dashboard.Domain
{
    [Serializable]
    public class UserDashboardSettings : lm.Comol.Core.DomainModel.DomainBaseObject<long>, ISettingsBase
    {
        public virtual DashboardViewType View { get; set; }
        public virtual DisplayNoticeboard DefaultNoticeboard { get; set; }
        public virtual DisplayNoticeboard TileNoticeboard { get; set; }
        public virtual DisplayNoticeboard ListNoticeboard { get; set; }
        public virtual DisplayNoticeboard CombinedNoticeboard { get; set; }
        public virtual OnLoadSettings AfterUserLogon { get; set; }
        public virtual OrderItemsBy OrderBy { get; set; }
        public virtual GroupItemsBy GroupBy { get; set; }
        public virtual Boolean  Ascending { get; set; }
        public virtual lm.Comol.Core.DomainModel.litePerson Owner { get; set; }
        public virtual Int32 IdCommunity { get; set; }

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