using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Modules.Standard.Menu.Domain
{
    [Serializable]
    public class dtoTopMenuItem : dtoBaseMenuItem
    {
        public virtual long ParentsNumber { get; set; }
        public dtoTopMenuItem() {
            this.Type = MenuItemType.TopItemMenu;
        }
         public dtoTopMenuItem(TopMenuItem item) {
            this.Type = MenuItemType.TopItemMenu;
            Id = item.Id;
            CssClass = item.CssClass;
            DisplayOrder = item.DisplayOrder;
            IdModule = item.IdModule;
            Name = item.Name;
            IsEnabled = item.IsEnabled;
            Link= item.Link;
            Permission = item.Permission;
            TextPosition= item.TextPosition;
            ShowDisabledItems= item.ShowDisabledItems;
            IdMenubar =item.Menubar.Id;
        }
    }
}