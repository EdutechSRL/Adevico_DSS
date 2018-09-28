using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel;

namespace lm.Comol.Modules.Standard.Menu.Domain
{
    [Serializable]
    public class dtoItem
    {
        public virtual long Id { get; set; }
        public virtual MenuItemType Type { get; set; }
        public virtual BaseStatusDeleted Deleted { get; set; }


        public virtual List<MenuItemType> AvailableSubTypes()
        {
            List<MenuItemType> list = new List<MenuItemType>();
            switch (Type)
            {
                case MenuItemType.Menubar:
                    list.Add(MenuItemType.TopItemMenu);
                    break;
                case MenuItemType.TopItemMenu:
                    list.Add(MenuItemType.ItemColumn);
                    break;
                case MenuItemType.ItemColumn:
                    list.Add(MenuItemType.Link);
                    list.Add(MenuItemType.Separator);
                    list.Add(MenuItemType.Text);
                    list.Add(MenuItemType.LinkContainer);
                    list.Add(MenuItemType.TextContainer);
                    break;
                case MenuItemType.Separator:
                    break;
                case MenuItemType.LinkContainer:
                case MenuItemType.TextContainer:
                    list.Add(MenuItemType.Link);
                    list.Add(MenuItemType.Separator);
                    list.Add(MenuItemType.Text);
                    break;
                case MenuItemType.Link:
                case MenuItemType.Text:
                    list.Add(MenuItemType.IconNewItem);
                    list.Add(MenuItemType.IconManage);
                    list.Add(MenuItemType.IconStatistic);
                    break;
                default:
                    break;
            }
            return list;
        }
    }
}