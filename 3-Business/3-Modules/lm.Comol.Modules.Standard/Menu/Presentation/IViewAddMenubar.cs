using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel.Common;
using lm.Comol.Modules.Standard.Menu.Domain;
using lm.Comol.Core.DomainModel;

namespace lm.Comol.Modules.Standard.Menu.Presentation
{
    public interface IViewAddMenubar : iDomainView
    {
        MenuBarType PreloadType { get; }
        Boolean AllowCreate { get; set; }
        Int32 TopItemsnumber { get; set; }
        Int32 SubItemsnumber { get; set; }
        dtoMenubar MenuBar { get; set; }
        String DefaultMenuName { get; }

        void LoadAvailableTypes(List<MenuBarType> views);
        void NoPermission();
        void CreationError();
        void EditMenubar(long IdMenubar, MenuBarType view);
        void DisplaySessionTimeout(String url);
    }
 }