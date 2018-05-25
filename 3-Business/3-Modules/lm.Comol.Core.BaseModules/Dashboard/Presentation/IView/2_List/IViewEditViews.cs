using lm.Comol.Core.BaseModules.Tiles.Domain;
using lm.Comol.Core.Dashboard.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Core.BaseModules.Dashboard.Presentation 
{
    public interface IViewEditViews : IViewBaseEditSettings
    {

        //dtoBaseDashboardSettings GetSettings();
        //String GetNewSettingsName();
        //void DisplaySettingsAdded();
        //void LoadSettings(liteDashboardSettings settings, List<lm.Comol.Core.DomainModel.dtoTranslatedProfileType> items, List<Int32> idSelected);
        //void LoadSettings(liteDashboardSettings settings, List<lm.Comol.Core.DomainModel.dtoTranslatedRoleType> items, List<Int32> idSelected);
        void LoadSettings(dtoViewSettings settings, bool initialize);
        void DisplayRangeError(dtoViewSettings settings, List<dtoPageSettings> pages);
        void DisplayNoPageError();
        void DisplayTileUnableToRedirectOn();
    }
}