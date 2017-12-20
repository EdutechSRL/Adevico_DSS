using lm.Comol.Core.BaseModules.Tiles.Domain;
using lm.Comol.Core.Dashboard.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Core.BaseModules.Dashboard.Presentation 
{
    public interface IViewEditSettings : IViewBaseEditSettings
    {
        dtoBaseDashboardSettings GetSettings();
        String GetNewSettingsName();
        void DisplaySettingsAdded();
        void LoadSettings(liteDashboardSettings settings, List<lm.Comol.Core.DomainModel.dtoTranslatedProfileType> items, List<Int32> idSelected);
        void LoadSettings(liteDashboardSettings settings, List<lm.Comol.Core.DomainModel.dtoTranslatedRoleType> items, List<Int32> idSelected);
        void LoadSettings(liteDashboardSettings settings);
        void EnableSelector(Boolean enable);
      
    }
}