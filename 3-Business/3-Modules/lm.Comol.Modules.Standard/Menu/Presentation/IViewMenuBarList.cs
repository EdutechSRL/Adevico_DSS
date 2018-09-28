using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel.Common;
using lm.Comol.Modules.Standard.Menu.Domain;
using lm.Comol.Core.DomainModel;

namespace lm.Comol.Modules.Standard.Menu.Presentation
{

    public interface IViewMenubarList : iDomainView
    {
        MenuBarType PreloadView { get; }
        MenuBarType CurrentView { get; set; }
        int CurrentPageSize { get; set; }
        Boolean AllowCreate { get; set; }
        PagerBase Pager { get; set; }
        void LoadItems(List<dtoMenubarItemPermission> items);
        void LoadAvailableViews(List<MenuBarType> views);
        void NoPermission();
        void EditMenubar(long idMenubar, MenuBarType type);
        void ReloadPage(long idMenubar, MenuBarType type, int pageIndex, int pageSize);
        void DisplaySessionTimeout(String url);
    }
}