using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Modules.Standard.Menu.Domain
{
    [CLSCompliant(true), Serializable ]
    public class TopMenuItem : BaseMenuItem
    {
        /// <summary>
        /// Sottovoci
        /// </summary>
        public virtual IList<ItemColumn> Columns { get; set; }
       
        //public virtual List<MenuItemType> AvailableSubTypes()
        //{
        //    List<MenuItemType> list = new List<MenuItemType>();
        //    list.Add(MenuItemType.ItemColumn);
        //    return list;
        //}
    }
}