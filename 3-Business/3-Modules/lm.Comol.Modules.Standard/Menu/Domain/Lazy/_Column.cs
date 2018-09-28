using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using lm.Comol.Core.DomainModel;

namespace lm.Comol.Modules.Standard.Menu.Domain
{
    [Serializable]
    public class _Column : DomainBaseObject<long>
    {
        public virtual Boolean HasSeparator { get; set; }
        public virtual Int16 WidthPx { get; set; }
        public virtual Int16 HeightPx { get; set; }
        public virtual String CssClass { get; set; }
        public virtual IList<_MenuItem> Items { get; set; }
        public virtual long IdTopItem { get; set; }
        public virtual long DisplayOrder { get; set; } //*
        public virtual Boolean IsEnabled { get; set; }      //*

        public virtual long IdMenubar { get; set; }
        protected internal virtual dtoTree ToTree(int number)
        {
            dtoTree tree = new dtoTree() { Id = Id, Type = MenuItemType.ItemColumn, Deleted = Deleted, Name = number.ToString(),  Items = new List<dtoTree>() };

            foreach (_MenuItem item in Items.Where(i => i.Deleted == BaseStatusDeleted.None).OrderBy(i => i.DisplayOrder))
            {
                tree.Items.Add(item.ToTree());
            }
            return tree;
        }
        public virtual String Render(int IdProfileType, int IdLanguage, String Baseurl)
        {
           // 
            String renderColumnItem = "<div class=\"col{1}\" {2}>\n\r<ul>\n\r{0}\n\r</ul>\n\r</div>\n\r";
           
            String renderSubItems = "";

            foreach (_MenuItem item in Items.Where(m => m.Deleted == BaseStatusDeleted.None && m.IsEnabled).OrderBy(m => m.DisplayOrder))
            {
                renderSubItems += item.Render(IdProfileType, IdLanguage, Baseurl);
            }
            if (!string.IsNullOrEmpty(renderSubItems))
                return String.Format(renderColumnItem, renderSubItems, (String.IsNullOrEmpty(CssClass) ? "" : " " + CssClass), DimensionStyle());
            return "";    
        }
        public virtual String Render(List<CommunityRoleModulePermission> permissions, int IdLanguage, String Baseurl)
        {
            // 
            String renderColumnItem = "<div class=\"col{1}\" {2}>\n\r<ul>\n\r{0}\n\r</ul>\n\r</div>\n\r";

            String renderSubItems = "";

            foreach (_MenuItem item in Items.Where(m => m.Deleted == BaseStatusDeleted.None && m.IsEnabled).OrderBy(m => m.DisplayOrder))
            {
                renderSubItems += item.Render(permissions, IdLanguage, Baseurl);
            }
            if (!string.IsNullOrEmpty(renderSubItems))
                return String.Format(renderColumnItem, renderSubItems, (String.IsNullOrEmpty(CssClass) ? "" : " " + CssClass), DimensionStyle());
            return "";
        }

        protected virtual String DimensionStyle() {
            String result = " style=\"{0}{1}\"";

            return (WidthPx == 0 && HeightPx == 0) ? "" : string.Format(result, (WidthPx == 0) ? "" : " width: " + WidthPx.ToString() + "px; ", (HeightPx == 0) ? "" : " height: " + HeightPx.ToString() + "px; ");
        }
    }
}