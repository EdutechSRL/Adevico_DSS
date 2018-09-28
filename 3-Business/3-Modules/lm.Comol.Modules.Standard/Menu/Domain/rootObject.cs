using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Modules.Standard.Menu.Domain
{
    public static class rootObject
    {
        public static String MenuBarList()
        {
            return "Modules/Menu/List.aspx?View=AllType";
        }

        public static String MenuBarList(MenuBarType type)
        {
            return "Modules/Menu/List.aspx?View=" + type.ToString() ;
        }
        public static String EditMenuBar(long IdMenubar, MenuBarType type)
        {
            return "Modules/Menu/Edit.aspx?Id=" + IdMenubar.ToString() + "&View=" + type.ToString();
        }
        public static String ViewMenuBar(long IdMenubar, MenuBarType type)
        {
            return "Modules/Menu/View.aspx?Id=" + IdMenubar.ToString() + "&View=" + type.ToString();
        }
        public static String CreateMenuBar(MenuBarType type)
        {
            return "Modules/Menu/Add.aspx?type=" + type.ToString();
        }
    }
}
