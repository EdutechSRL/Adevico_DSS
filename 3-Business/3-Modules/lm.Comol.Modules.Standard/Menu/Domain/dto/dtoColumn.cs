using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel;

namespace lm.Comol.Modules.Standard.Menu.Domain
{
    [Serializable]
    public class dtoColumn :dtoItem
    {
        public virtual Boolean HasSeparator { get; set; }
        public virtual Int16 WidthPx { get; set; }
        public virtual Int16 HeightPx { get; set; }
        public virtual String CssClass { get; set; }
        public virtual long IdTopItem { get; set; }
        public virtual long DisplayOrder { get; set; } //*
        public virtual Boolean IsEnabled { get; set; }      //*
        public virtual long ParentsNumber { get; set; }
        public dtoColumn()
        {
            this.Type = MenuItemType.ItemColumn;
        }

        public dtoColumn(ItemColumn column)
        {
            CssClass = column.CssClass;
            Deleted = column.Deleted;
            Id = column.Id;
            DisplayOrder = column.DisplayOrder;
            HasSeparator = column.HasSeparator;
            HeightPx = column.HeightPx;
            IsEnabled = column.IsEnabled;
            IdTopItem = column.TopItemOwner.Id;
            Type = MenuItemType.ItemColumn;
            WidthPx = column.WidthPx;
        }
       
    }
}
