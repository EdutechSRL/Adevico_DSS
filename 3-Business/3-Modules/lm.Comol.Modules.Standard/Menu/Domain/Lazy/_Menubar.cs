using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using lm.Comol.Core.DomainModel;

namespace lm.Comol.Modules.Standard.Menu.Domain
{
    /// <summary>
    /// Classe che rappresenta un MENU nel suo insieme
    /// Puo' contenere SOLO elementi di "primo livello"...
    /// </summary>
    [Serializable]
    public class _Menubar : DomainBaseObject<long>
    {
        public virtual string CssClass { get; set; }    //255
        public virtual String Name { get; set; }    //150
        public virtual Boolean IsCurrent { get; set; }
        public virtual IList<_TopItem> Items { get; set; }
        public virtual MenuBarType MenuBarType { get; set; }
        public virtual ItemStatus Status { get; set; }

        protected internal virtual dtoTree ToTree()
        {
            dtoTree tree = new dtoTree() { Id = Id, Type = MenuItemType.Menubar, Deleted = Deleted, Name = Name,  Items = new List<dtoTree>() };
            foreach (_TopItem topItem in Items.Where(c=>c.Deleted== BaseStatusDeleted.None).OrderBy(c=> c.DisplayOrder)){
                tree.Items.Add(topItem.ToTree());
            }
            return tree;
        }

        public virtual String Render(int IdProfileType, int IdLanguage, String Baseurl)
        {
            String render = "<div id=\"nav-main\">\n\r<div class=\"page-width\">\n\r<ul id=\"top\">\n\r";
            String topItemRender ="";

            foreach (_TopItem topItem in Items.Where(c => c.Deleted == BaseStatusDeleted.None && c.IsEnabled ).OrderBy(c => c.DisplayOrder))
            {
                topItemRender = topItem.Render(IdProfileType, IdLanguage, Baseurl);
                if (!string.IsNullOrEmpty(topItemRender))
                    render += topItemRender; //String.Format(columnTemplate, topItemRender);
            }
            render += "</ul>\n\r</div></div>\n\r";
            return render;
        }

        public virtual String Render(List<CommunityRoleModulePermission> permissions, int IdLanguage, String Baseurl, String DefaultModuleToolTip, String DefaultModuleUrl, String DefaultModuleText)
        {
            String render = "<div id=\"nav-main\">\n\r<div class=\"page-width\">\n\r<ul id=\"top\">\n\r";
            String topItemRender = "";

            if (!String.IsNullOrEmpty(DefaultModuleUrl)  && (MenuBarType == Domain.MenuBarType.GenericCommunity || MenuBarType == Domain.MenuBarType.ForCommunity || MenuBarType == Domain.MenuBarType.ForCommunitiesTemplate))
                render += "<li class=\"communitydefaultview\"><a href=\"" + Baseurl + DefaultModuleUrl + "\" title=\"" + DefaultModuleToolTip + "\" ><span>" + DefaultModuleText + "</span></a></li>\n\r";

            foreach (_TopItem topItem in Items.Where(c => c.Deleted == BaseStatusDeleted.None && c.IsEnabled).OrderBy(c => c.DisplayOrder))
            {
                topItemRender = topItem.Render(permissions, IdLanguage, Baseurl);
                if (!string.IsNullOrEmpty(topItemRender))
                    render += topItemRender; //String.Format(columnTemplate, topItemRender);
            }
            render += "</ul>\n\r</div></div>\n\r";
            return render;
        }
    }
}