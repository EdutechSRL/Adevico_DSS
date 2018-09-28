using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel;

namespace lm.Comol.Modules.Standard.Menu.Domain
{
    [ Serializable ]
    public class _TopItem : _BaseItem
    {
        /// <summary>
        /// Sottovoci
        /// </summary>
        public virtual IList<_Column> Columns { get; set; }

        protected internal virtual dtoTree ToTree()
        {
            int columNumber = 1;
            dtoTree tree = new dtoTree() { Id = Id, Type = MenuItemType.TopItemMenu, Deleted = Deleted, Name = Name, Items = new List<dtoTree>() };

            foreach (_Column column in Columns.Where(c => c.Deleted == BaseStatusDeleted.None).OrderBy(c => c.DisplayOrder))
            {
                tree.Items.Add(column.ToTree(columNumber));
                columNumber++;
            }
            return tree;
        }
        public virtual String Render(int IdProfileType, int IdLanguage, String Baseurl)
        {
            Boolean available = IsValid(IdProfileType);
            if (available || ShowDisabledItems) {
                String columnTemplate = "<div class=\"sub\">\n\r{0}\n\r</div>\n\r";
                String renderTopItem = "";
                if (String.IsNullOrEmpty(CssClass))
                    renderTopItem = "<li>{0}{1}</li>\n\r";
                else
                    renderTopItem = "<li class=\"" + CssClass + "\" >{0}{1}</li>\n\r";

                String renderSubItems = "";
                String renderItem = "";
                foreach (_Column column in Columns.Where(c => c.Deleted == BaseStatusDeleted.None && c.IsEnabled).OrderBy(c => c.DisplayOrder))
                {
                    renderSubItems += column.Render(IdProfileType, IdLanguage, Baseurl);
                }
                //String renderItem =  ItemToString(IdLanguage, Baseurl, available);
                if (!(String.IsNullOrEmpty(renderSubItems) && string.IsNullOrEmpty(Link)))
                    renderItem = ItemToString(IdLanguage, Baseurl, available);
                if (!string.IsNullOrEmpty(renderItem))
                    return String.Format(renderTopItem, ItemToString(IdLanguage,Baseurl,available), String.IsNullOrEmpty(renderSubItems) ? "" : String.Format(columnTemplate, renderSubItems));
                else
                    return "";
            }
            return "";            
        }
        public virtual String Render(List<CommunityRoleModulePermission> permissions, int IdLanguage, String Baseurl)
        {
            Boolean available = IsValid(permissions);
            if (available || ShowDisabledItems) {
                String columnTemplate = "<div class=\"sub\">\n\r{0}\n\r</div>\n\r";
                String renderTopItem = "";
                 if (String.IsNullOrEmpty(CssClass))
                    renderTopItem = "<li>{0}{1}</li>\n\r";
                else
                    renderTopItem = "<li class=\"" + CssClass + "\" >{0}{1}</li>\n\r";
                String renderSubItems = "";

                foreach (_Column column in Columns.Where(c => c.Deleted == BaseStatusDeleted.None && c.IsEnabled).OrderBy(c => c.DisplayOrder))
                {
                    renderSubItems += column.Render(permissions, IdLanguage, Baseurl);
                }
                String renderItem = "";
                if (!(String.IsNullOrEmpty(renderSubItems) && string.IsNullOrEmpty(Link)))  
                    renderItem = ItemToString(IdLanguage, Baseurl, available);
                if (!string.IsNullOrEmpty(renderItem))
                    return String.Format(renderTopItem, ItemToString(IdLanguage,Baseurl,available), String.IsNullOrEmpty(renderSubItems) ? "" : String.Format(columnTemplate, renderSubItems));
                else
                    return "";
            }
            return "";            
        }

    }
}