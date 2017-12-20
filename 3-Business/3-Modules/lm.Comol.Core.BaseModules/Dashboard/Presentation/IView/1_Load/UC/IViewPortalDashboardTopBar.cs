using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel;
using lm.Comol.Core.Dashboard.Domain;

namespace lm.Comol.Core.BaseModules.Dashboard.Presentation 
{
    public interface IViewPortalDashboardTopBar : lm.Comol.Core.DomainModel.Common.iDomainView
    {
        DashboardViewType CurrentViewType { get; set; }
        DisplaySearchItems SelectedSearch { get; set; }
        GroupItemsBy SelectedGroupBy { get; set; }
        void DisplaySessionTimeout();
        void InitalizeControl(DashboardViewType cView, liteDashboardSettings settings, UserCurrentSettings userSettings, Boolean moreTiles, String searchBy ="");
        void InitializeViewSelector(List<dtoItemFilter<DashboardViewType>> items);
        void InitializeGroupBySelector(List<dtoItemFilter<GroupItemsBy>> items, GroupItemsBy selected);
        void InitializeSearch(DisplaySearchItems settings);
        void HideDisplayMessage();
        void DisplayMessage(String message, lm.Comol.Core.DomainModel.Helpers.MessageType type);
    }
}