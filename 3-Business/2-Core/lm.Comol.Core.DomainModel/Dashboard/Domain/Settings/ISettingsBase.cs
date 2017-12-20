using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Core.Dashboard.Domain
{
    public interface ISettingsBase
    {
        DashboardViewType View { get; set; }
        DisplayNoticeboard DefaultNoticeboard { get; set; }
        DisplayNoticeboard TileNoticeboard { get; set; }
        DisplayNoticeboard ListNoticeboard { get; set; }
        DisplayNoticeboard CombinedNoticeboard { get; set; }
        OnLoadSettings AfterUserLogon { get; set; }
        OrderItemsBy OrderBy { get; set; }
        GroupItemsBy GroupBy { get; set; }


        DisplayNoticeboard GetNoticeboard(DashboardViewType view);
    }
}
