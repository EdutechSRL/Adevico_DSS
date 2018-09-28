using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel;

namespace lm.Comol.Modules.Standard.Menu.Domain
{
    [Serializable]
    public class _MenuItem : _BaseItem
    {
        public virtual MenuItemType Type { get; set; }         
        public virtual IList<_MenuItem> Childrens { get; set; }  
        public virtual _MenuItem ItemOwner { get; set; }
        public virtual long IdTopOwner { get; set; }
        public virtual long IdColumnOwner { get; set; }

        protected internal virtual dtoTree ToTree()
        {
            dtoTree tree = new dtoTree() { Id = Id, Type = Type, Deleted = Deleted, Name = Name, Items = new List<dtoTree>() };

            foreach (_MenuItem item in Childrens.Where(i => i.Deleted == BaseStatusDeleted.None).OrderBy(i => i.DisplayOrder))
            {
                tree.Items.Add(item.ToTree());
            }
            return tree;
        }
        public virtual String Render(int IdProfileType, int IdLanguage, String Baseurl)
        {
            Boolean available = IsValid(IdProfileType);
            if (available || ShowDisabledItems)
            {
                String renderMenuItem = "<li {0}>{1}{2}</li>\n\r";
                String renderSubItems = "";

                switch (Type)
                {
                    case MenuItemType.IconManage:
                    case MenuItemType.IconNewItem:
                    case MenuItemType.IconStatistic:
                        return RenderIcon(available, IdLanguage, Baseurl);

                    case MenuItemType.Link:
                    case MenuItemType.Text:

                        // if children>0 are only ICONS !!
                        foreach (_MenuItem item in Childrens.Where(m => m.Deleted == BaseStatusDeleted.None && m.IsEnabled).OrderByDescending(m => m.DisplayOrder)){
                            renderSubItems += item.Render(IdProfileType, IdLanguage, Baseurl);
                        }
                        return String.Format(renderMenuItem,String.IsNullOrEmpty(CssClass) ? "" : " class=\"" + CssClass + "\"",RenderItem(available, IdLanguage, Baseurl),
                            String.IsNullOrEmpty(renderSubItems) ? "" : " <span class=\"actions\">" + renderSubItems +"</span>");

                    case MenuItemType.Separator:
                        return String.Format(renderMenuItem, String.IsNullOrEmpty(CssClass) ? "" : " class=\"" + CssClass + "\"", "<h2 class=\"separator\"> &nbsp; </h2>", "");

                    case MenuItemType.LinkContainer:
                    case MenuItemType.TextContainer:

                         foreach (_MenuItem item in Childrens.Where(m => m.Deleted == BaseStatusDeleted.None && m.IsEnabled).OrderBy(m => m.DisplayOrder)){
                            renderSubItems += item.Render(IdProfileType, IdLanguage, Baseurl);
                            }
                         if (String.IsNullOrEmpty(renderSubItems) && Type == MenuItemType.TextContainer)
                             return "";
                         else
                             return String.Format(renderMenuItem, String.IsNullOrEmpty(CssClass) ? "" : " class=\"" + CssClass + "\"", RenderContainerItem(available, IdLanguage, Baseurl),
                                String.IsNullOrEmpty(renderSubItems) ? "" : " <ul>" + renderSubItems +"</ul>");

                    default:
                        break;
                }
            }
            return "";    
        }
        public virtual String Render(List<CommunityRoleModulePermission> permissions, int IdLanguage, String Baseurl)
        {
            Boolean available = IsValid(permissions);
            if (available || ShowDisabledItems)
            {
                String renderMenuItem = "<li {0}>{1}{2}</li>\n\r";
                String renderSubItems = "";

                switch (Type)
                {
                    case MenuItemType.IconManage:
                    case MenuItemType.IconNewItem:
                    case MenuItemType.IconStatistic:
                        return RenderIcon(available, IdLanguage, Baseurl);

                    case MenuItemType.Link:
                    case MenuItemType.Text:

                        // if children>0 are only ICONS !!
                        foreach (_MenuItem item in Childrens.Where(m => m.Deleted == BaseStatusDeleted.None && m.IsEnabled).OrderByDescending(m => m.DisplayOrder))
                        {
                            renderSubItems += item.Render(permissions, IdLanguage, Baseurl);
                        }
                        if (renderSubItems == "" && Type == MenuItemType.Text)
                            return "";
                        else
                            return String.Format(renderMenuItem, String.IsNullOrEmpty(CssClass) ? "" : " class=\"" + CssClass + "\"", RenderItem(available, IdLanguage, Baseurl),
                            String.IsNullOrEmpty(renderSubItems) ? "" : " <span class=\"actions\">" + renderSubItems + "</span>");

                    case MenuItemType.Separator:
                        return String.Format(renderMenuItem, String.IsNullOrEmpty(CssClass) ? "" : " class=\"" + CssClass + "\"", "<h2 class=\"separator\"> &nbsp; </h2>", "");

                    case MenuItemType.LinkContainer:
                    case MenuItemType.TextContainer:

                        foreach (_MenuItem item in Childrens.Where(m => m.Deleted == BaseStatusDeleted.None && m.IsEnabled).OrderBy(m => m.DisplayOrder))
                        {
                            renderSubItems += item.Render(permissions, IdLanguage, Baseurl);
                        }
                        if (renderSubItems == "" && Type == MenuItemType.TextContainer)
                            return "";
                        else 
                            return String.Format(renderMenuItem, String.IsNullOrEmpty(CssClass) ? "" : " class=\"" + CssClass + "\"", RenderContainerItem(available, IdLanguage, Baseurl),
                           String.IsNullOrEmpty(renderSubItems) ? "" : " <ul>" + renderSubItems + "</ul>");

                    default:
                        break;
                }
            }
            return "";
        }
        
        protected internal virtual String RenderIcon(Boolean available,int IdLanguage, String Baseurl)
        {
            String render = "<a class=\"{0}{1}\" title=\"{2}\" href=\"{3}\">{4}</a>";
            _ItemTranslation translation = Translate(IdLanguage);
                //"<a class=\"management" title="Gestione" href="http://www.wikipedia.com">wiiii</a>
                //    <a class="stats" title="Statistiche" href="http://www.wikipedia.com">wi</a>
                //    <a class="new" title="Nuovo" href="http://www.google.com">pppp</a>

            return String.Format(render,(Type== MenuItemType.IconNewItem) ? "new" : (Type== MenuItemType.IconManage) ?  "management" : "stats",
                String.IsNullOrEmpty(CssClass) ? "" : " class=\"" + CssClass + "\" ", translation.ToolTip,
                (string.IsNullOrEmpty(Link) || (!available && ShowDisabledItems)) ? "#": Baseurl+ Link, translation.Name);
        }

        protected internal virtual String RenderItem(Boolean available, int IdLanguage, String Baseurl)
        {
            String render = "<a {0} title=\"{1}\" href=\"{2}\">{3}</a>";
            _ItemTranslation translation = Translate(IdLanguage);

            return String.Format(render,   String.IsNullOrEmpty(CssClass) ? "" : " class=\"" + CssClass+ "\" ", translation.ToolTip,
                (string.IsNullOrEmpty(Link) || (!available && ShowDisabledItems)) ? "#" : Baseurl + Link, translation.Name);
        }
        protected internal virtual String RenderContainerItem(Boolean available, int IdLanguage, String Baseurl)
        {
            String renderText = "<h2 {0} title=\"{1}\">{2}</h2>";
            String renderLink = "<h2><a {0} title=\"{1}\" href=\"{2}\">{3}</a></h2>";
            _ItemTranslation translation = Translate(IdLanguage);

            if (string.IsNullOrEmpty(Link) || (!available && ShowDisabledItems))
                return String.Format(renderText, String.IsNullOrEmpty(CssClass) ? "" : " class=\"" + CssClass + "\" ", translation.ToolTip,translation.Name);
            else
                return String.Format(renderLink, String.IsNullOrEmpty(CssClass) ? "" : " class=\"" + CssClass + "\" ", translation.ToolTip,
                Baseurl + Link, translation.Name);
        }
    }
}