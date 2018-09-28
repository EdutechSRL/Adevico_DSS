using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using lm.Comol.Core.DomainModel;

namespace lm.Comol.Modules.Standard.Menu.Domain
{
    [Serializable]
    public class Menubar : DomainBaseObjectLiteMetaInfo<long> 
    {
        public virtual string CssClass { get; set; }    //255
        public virtual String Name { get; set; }    //150
        public virtual Boolean IsCurrent { get; set; }
        public virtual MenuBarType MenuBarType { get; set; }
        public virtual ItemStatus Status { get; set; }
        public virtual IList<TopMenuItem> Items { get; set; }

        //public virtual List<MenuItemType> AvailableSubTypes() { 
        //    List<MenuItemType> list = new List<MenuItemType>();
        //    list.Add(MenuItemType.TopItemMenu);
        //    return list;
        //}
    }
}