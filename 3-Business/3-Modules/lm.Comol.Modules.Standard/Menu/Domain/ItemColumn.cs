using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using lm.Comol.Core.DomainModel;

namespace lm.Comol.Modules.Standard.Menu.Domain
{
    [CLSCompliant(true), Serializable]
    public class ItemColumn : DomainBaseObjectLiteMetaInfo<long>
    {
        public virtual Boolean HasSeparator { get; set; }
        public virtual Int16 WidthPx { get; set; }
        public virtual Int16 HeightPx { get; set; }
        public virtual String CssClass { get; set; }
        public virtual IList<MenuItem> Items { get; set; }
        public virtual TopMenuItem TopItemOwner { get; set; }
        public virtual long DisplayOrder { get; set; } //*
        public virtual Boolean IsEnabled { get; set; }      //*
        public virtual Menubar Menubar { get; set; }

        //public virtual List<MenuItemType> AvailableSubTypes()
        //{
        //    List<MenuItemType> list = new List<MenuItemType>();
             
        //    list.Add(MenuItemType.Link);
        //    list.Add(MenuItemType.Separator);
        //    list.Add(MenuItemType.Text);
        //    list.Add(MenuItemType.LinkContainer);
        //    list.Add(MenuItemType.TextContainer);
        //    return list;
        //}
    }
}