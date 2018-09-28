using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel;
//using System.IO;
using lm.Comol.Core.Business;
using lm.Comol.Modules.Standard.Menu.Domain;
using lm.Comol.Core.DomainModel.Helpers;

namespace lm.Comol.Modules.Standard.Menu.Business
{
    public class ServiceMenubar : CoreServices
    {
        private const string UniqueCode = "SRVmenu";
        private iUserContext UC { set; get; }
        private Int32 idModule { set; get; }
        private litePerson currentUser { set; get; }     
        #region initClass
            public ServiceMenubar() { }
            public ServiceMenubar(iApplicationContext oContext)
            {
                Manager = new BaseModuleManager(oContext.DataContext);
                UC = oContext.UserContext;
            }
            public ServiceMenubar(iDataContext datacontext)
            {
                Manager = new BaseModuleManager(datacontext);
                UC = null;
            }
        #endregion

        public int ServiceModuleID()
        {
            if (idModule==0)
                idModule = Manager.GetModuleID(ServiceMenubar.UniqueCode);
            return idModule;
        }
        public litePerson CurrentUser()
        {
            if (currentUser == null)
                currentUser = Manager.GetLitePerson(UC.CurrentUserID);
            return currentUser;
        }

        #region "MenuBar"
            public Boolean MenubarExist(long IdMenubar)
            {
            return (from i in Manager.GetIQ<Menubar>() where i.Id == IdMenubar select i.Id).Any();
            }
            public Boolean IsActiveMenubar(long IdMenubar)
            {
                return (from i in Manager.GetIQ<Menubar>() where i.Id == IdMenubar && i.IsCurrent select i.Id).Any();
            }
            public MenuBarType GetItemMenubarType(long IdItem)
            {
                return (from i in Manager.GetIQ<BaseMenuItem>() where i.Id == IdItem select i.Menubar.MenuBarType).Skip(0).Take(1).ToList().FirstOrDefault();
            }
            public MenuBarType GetMenubarType(long IdMenubar)
            {
                return (from i in Manager.GetIQ<Menubar>() where i.Id == IdMenubar select i.MenuBarType).Skip(0).Take(1).ToList().FirstOrDefault();
            }
            public Menubar CreateMenubar(dtoMenubar dto, Int32 topItemsnumber, Int32 subItemsnumber)
            {
                    Menubar menubar = null;
                    try
                    {
                        Manager.BeginTransaction();
                        menubar = new Menubar();
                        menubar.CreateMetaInfo(CurrentUser(), UC.IpAddress, UC.ProxyIpAddress);
                        menubar.CssClass = dto.CssClass;
                        menubar.Name = dto.Name;
                        menubar.Status = dto.Status;
                        menubar.MenuBarType = dto.MenuBarType;
                        menubar.IsCurrent = false;
                        menubar.Items = new List<TopMenuItem>();
                        menubar.Deleted = BaseStatusDeleted.None;
                        Manager.SaveOrUpdate(menubar);
                        menubar.Items = CreateTopItems(menubar, topItemsnumber, subItemsnumber);
                        //Manager.SaveOrUpdateList(CreateTopItems(menubar, topItemsnumber, subItemsnumber));

                        Manager.Commit();
                    }
                    catch (Exception ex) {
                        Manager.RollBack();
                        menubar = null;
                    }
                    return menubar;
                }
            public List<TopMenuItem> CreateTopItems(Menubar menubar, Int32 topItemsnumber, Int32 subItemsnumber)
                {
                    List<TopMenuItem> list = new List<TopMenuItem>();
                    for (int i =1; i <=topItemsnumber; i++){
                        TopMenuItem topItem = new TopMenuItem();
                        topItem.CreateMetaInfo(CurrentUser(), UC.IpAddress, UC.ProxyIpAddress);
                        topItem.CssClass = "";
                        topItem.DisplayOrder = (short)i;
                        topItem.IsEnabled = true;
                        topItem.Menubar = menubar;
                        topItem.Name = i.ToString();
                        topItem.ShowDisabledItems = false;
                        topItem.TextPosition = TextPosition.DisplayLeft;
                        topItem.Columns = new List<ItemColumn>();
                        topItem.Translations = new List<MenuItemTranslation>();
                        Manager.SaveOrUpdate(topItem);
                        topItem.Translations = CreateItemTranslations(menubar, topItem, topItem);
                        if (subItemsnumber > 0)
                            topItem.Columns.Add(CreateItemColumn(menubar, topItem, subItemsnumber));
                        Manager.SaveOrUpdate(topItem);
                        list.Add(topItem);
                    }
                    return list;
                }
            private ItemColumn CreateItemColumn(Menubar menubar, TopMenuItem itemOwner)
            {
                ItemColumn column = new ItemColumn();
                column.CreateMetaInfo(CurrentUser(), UC.IpAddress, UC.ProxyIpAddress);
                column.TopItemOwner = itemOwner;
                column.Menubar = menubar;
                column.IsEnabled = true;
                column.Items = new List<MenuItem>();
                column.HasSeparator = false;
                column.DisplayOrder = 1;
                column.CssClass = "";
                column.Deleted= BaseStatusDeleted.None;
                column.WidthPx = 0;
                column.HeightPx = 0;

                return column;
            }
            private ItemColumn CreateItemColumn(Menubar menubar, TopMenuItem topItem, Int32 subItemsnumber)
            {
                ItemColumn column = CreateItemColumn(menubar, topItem);
                Manager.SaveOrUpdate(column);
                List<MenuItem> list = new List<MenuItem>();
                for (long i = 1; i <= subItemsnumber; i++)
                {
                    MenuItem menuItem = new MenuItem();
                    menuItem.CreateMetaInfo(CurrentUser(), UC.IpAddress, UC.ProxyIpAddress);
                    menuItem.CssClass = "";
                    menuItem.DisplayOrder = i;
                    menuItem.IsEnabled = true;
                    menuItem.Menubar = menubar;
                    menuItem.Name = topItem.Name + "." + i.ToString();
                    menuItem.ShowDisabledItems = false;
                    menuItem.TextPosition = TextPosition.DisplayLeft;
                    menuItem.Translations = new List<MenuItemTranslation>();
                    menuItem.ColumnOwner = column;
                    menuItem.Type = MenuItemType.Link;
                    menuItem.TopItemOwner = topItem;
                    Manager.SaveOrUpdate(menuItem);
                    menuItem.Translations = CreateItemTranslations(menubar, menuItem, topItem);

                    list.Add(menuItem);
                }
                column.Items = list;
                return column;
            }
            private List<MenuItemTranslation> CreateItemTranslations(Menubar menubar, BaseMenuItem item, TopMenuItem topItem)
            {
                List<MenuItemTranslation> list = new List<MenuItemTranslation>();
                List<Language> languages = (from l in Manager.Linq<Language>() select l).ToList();
                
                foreach (Language language in languages) {
                    MenuItemTranslation translation = new MenuItemTranslation();
                    translation.Language = language;
                    translation.CreateMetaInfo(CurrentUser(), UC.IpAddress, UC.ProxyIpAddress);
                    translation.Item = item;
                    translation.TopMenuItem = topItem;
                    translation.Name=item.Name;
                    translation.ToolTip = item.Name;
                    translation.Menubar = menubar;
                    list.Add(translation);
                }
                Manager.SaveOrUpdateList(list);
                return list;
            }


            public Menubar SaveMenubar(dtoMenubar dto, long IdMenubar)
            {
                Menubar menubar = null;
                try
                {
                    Manager.BeginTransaction();
                    menubar = Manager.Get<Menubar>(IdMenubar);
                    if (menubar != null) {
                        menubar.UpdateMetaInfo(CurrentUser(), UC.IpAddress, UC.ProxyIpAddress);
                        menubar.CssClass = dto.CssClass;
                        menubar.Name = dto.Name;
                        menubar.Status = dto.Status;
                        menubar.Deleted = BaseStatusDeleted.None;
                        Manager.SaveOrUpdate(menubar);
                    }
                    Manager.Commit();
                }
                catch (Exception ex)
                {
                    Manager.RollBack();
                    menubar = null;
                }
                return menubar;
            }
        #endregion

        #region "DTOitems"
            public Int32 GetMenubarListCount(MenuBarType type)
            {

                return (from m in Manager.GetIQ<Menubar >()
                        where m.MenuBarType == type
                        orderby m.Deleted
                        select m.Id).Count();
            }
            public List<dtoMenubar> GetMenubarList(MenuBarType type,int currentPageIndex, int currentPageSize) {
                // IN FUTURO CON NHIBERNATE £ SOSTITUIRE CON GETIQ
                return (from m in Manager.GetAll<Menubar>(m=> m.MenuBarType== type)
                        orderby m.Deleted
                        select new dtoMenubar(){
                         CssClass=m.CssClass,
                         Deleted= m.Deleted,
                          Id= m.Id,
                           IsCurrent= m.IsCurrent,
                           MenuBarType= m.MenuBarType,
                           ModifiedOn=m.ModifiedOn,
                            Name= m.Name,
                             Status= m.Status,
                            ModifiedBy=m.ModifiedBy 
                        }).Skip(currentPageIndex * currentPageSize).Take(currentPageSize).ToList();
            }
            public dtoMenubar GetDtoMenubar(long IdMenubar)
            {
                return (from m in Manager.GetAll<Menubar>(m => m.Id == IdMenubar)
                        orderby m.Deleted
                        select new dtoMenubar()
                        {
                            CssClass = m.CssClass,
                            Deleted = m.Deleted,
                            Id = m.Id,
                            IsCurrent = m.IsCurrent,
                            MenuBarType = m.MenuBarType,
                            ModifiedOn = m.ModifiedOn,
                            Name = m.Name,
                            Status = m.Status,
                            ModifiedBy = m.ModifiedBy
                        }).Skip(0).Take(1).ToList().FirstOrDefault();
            }
            public List<dtoTranslation> GetDtoTranslations(long IdItem)
            {
                List<dtoTranslation> list = (from t in Manager.GetIQ<_ItemTranslation>() 
                                             where t.IdItem == IdItem && t.Deleted== BaseStatusDeleted.None 
                                             select  new dtoTranslation() { Id= t.Id, IdLanguage= t.Language.Id, IdMenuItem= t.IdItem, LanguageName= t.Language.Name, Name= t.Name , ToolTip=t.ToolTip}).ToList();
                List<Int32> h = list.Select(t => t.IdLanguage).ToList();
                list.AddRange((from l in Manager.GetIQ<Language>() 
                               where !h.Contains(l.Id) 
                               select new dtoTranslation() { Id= 0, IdLanguage= l.Id, IdMenuItem= IdItem, LanguageName= l.Name, Name= "" , ToolTip=""}).ToList());

                return list;
            }
            public dtoTopMenuItem GetDtoTopMenuItem(long IdItem)
            {
                dtoTopMenuItem item = (from t in Manager.GetIQ<_TopItem>()
                        where t.Id == IdItem && t.Deleted == BaseStatusDeleted.None
                        select new dtoTopMenuItem() {
                            Id = t.Id,
                            CssClass = t.CssClass,
                            DisplayOrder = t.DisplayOrder,
                            IdModule = t.IdModule,
                            Name = t.Name,
                            IsEnabled = t.IsEnabled,
                            Link= t.Link,
                            Permission= t.Permission,
                            TextPosition= t.TextPosition,
                            ShowDisabledItems= t.ShowDisabledItems,IdMenubar =t.IdMenubar
                        }).Skip(0).Take(1).ToList().FirstOrDefault();
                if (item != null)
                    item.ParentsNumber = (from p in Manager.GetIQ<_TopItem>() where p.Deleted == BaseStatusDeleted.None && p.IdMenubar == item.IdMenubar select p.Id).Count();
                return item;
            }
            public List<int> GetItemProfilesAssignments(long IdItem)
            {
                return (from a in Manager.GetIQ<_ProfileAssignment>()
                                       where a.IdItemOwner == IdItem && a.Deleted == BaseStatusDeleted.None
                                      select a.IdProfileType).ToList();
            }
            public dtoColumn GetDtoColumn(long IdColumn)
            {
                dtoColumn item = (from m in Manager.GetAll<ItemColumn>(m => m.Id == IdColumn)
                        select new dtoColumn()
                        {
                            CssClass = m.CssClass,
                            Deleted = m.Deleted,
                            Id = m.Id,
                             DisplayOrder= m.DisplayOrder,
                             HasSeparator= m.HasSeparator,
                              HeightPx = m.HeightPx,
                               IsEnabled= m.IsEnabled,
                                IdTopItem= m.TopItemOwner.Id,
                                 Type= MenuItemType.ItemColumn,
                                  WidthPx = m.WidthPx 

                          
                        }).Skip(0).Take(1).ToList().FirstOrDefault();
                if (item != null)
                    item.ParentsNumber = (from p in Manager.GetIQ<_Column>() where p.Deleted == BaseStatusDeleted.None && p.IdTopItem == item.IdTopItem select p.Id).Count();
                return item;
            }

            public dtoMenuItem GetDtoMenuItem(long IdItem)
            {
                dtoMenuItem item = (from t in Manager.GetIQ<_MenuItem>()
                                       where t.Id == IdItem && t.Deleted == BaseStatusDeleted.None
                                        select new dtoMenuItem()
                                       {
                                           Id = t.Id,
                                           CssClass = t.CssClass,
                                           DisplayOrder = t.DisplayOrder,
                                           IdModule = t.IdModule,
                                           Name = t.Name,
                                           IsEnabled = t.IsEnabled,
                                           Link = t.Link,
                                           Permission = t.Permission,
                                           TextPosition = t.TextPosition,
                                           ShowDisabledItems = t.ShowDisabledItems,
                                           IdMenubar = t.IdMenubar,
                                           IdColumnOwner = t.IdColumnOwner,
                                           IdItemOwner = (t.ItemOwner == null) ? 0 : t.ItemOwner.Id,
                                           Type= t.Type
                                       }).Skip(0).Take(1).ToList().FirstOrDefault();
                if (item != null) {
                    if (item.IdItemOwner==0)
                        item.ParentsNumber = (from p in Manager.GetIQ<_MenuItem>()
                                              where p.Deleted == BaseStatusDeleted.None && p.IdColumnOwner == item.IdColumnOwner && p.ItemOwner==null 
                                              select p.Id).Count();
                    else
                        item.ParentsNumber = (from p in Manager.GetIQ<_MenuItem>()
                                          where p.Deleted == BaseStatusDeleted.None && p.ItemOwner!=null && p.ItemOwner.Id == item.IdItemOwner
                                              select p.Id).Count();

                }
                    
                    
                return item;
            }

            private Boolean IconItemExist(MenuItemType type, long IdItemOwner) { 
                return (from i in Manager.GetIQ<MenuItem>() 
                        where i.ItemOwner.Id== IdItemOwner && i.Deleted== BaseStatusDeleted.None 
                        && i.Type== type select i.Id).Any();

            }
            private List<MenuItemType> GetIconItems(long IdItemOwner)
            {
                return (from i in Manager.GetIQ<MenuItem>()
                        where  i.Deleted == BaseStatusDeleted.None && i.ItemOwner !=null && i.ItemOwner.Id == IdItemOwner
                        select i.Type).ToList();

            }
            public List<MenuItemType> GetMenuItemAvailableTypes(dtoMenuItem item)
            {
                return GetMenuItemAvailableTypes(item.Type, item.IdItemOwner);
            }
            public List<MenuItemType> GetMenuItemAvailableTypes(MenuItem item)
            {
                return GetMenuItemAvailableTypes(item.Type, (item.ItemOwner == null) ? 0 : item.ItemOwner.Id);
            }
            public List<MenuItemType> GetMenuItemAvailableTypes(MenuItemType type, long IdItem)
            {
                List<MenuItemType> list = new List<MenuItemType>();
                switch (type)
                {
                    case MenuItemType.Separator:
                        break;
                    case MenuItemType.LinkContainer:
                    case MenuItemType.TextContainer:
                        list.Add(MenuItemType.LinkContainer);
                        list.Add(MenuItemType.TextContainer);
                        break;
                    case MenuItemType.IconStatistic:
                        if (!IconItemExist(MenuItemType.IconNewItem, IdItem))
                            list.Add(MenuItemType.IconNewItem);
                        if (!IconItemExist(MenuItemType.IconManage, IdItem))
                            list.Add(MenuItemType.IconManage);
                        list.Add(type);
                        break;
                    case MenuItemType.IconManage:
                        if (!IconItemExist(MenuItemType.IconNewItem, IdItem))
                            list.Add(MenuItemType.IconNewItem);
                        list.Add(type);
                        if (!IconItemExist(MenuItemType.IconStatistic, IdItem))
                            list.Add(MenuItemType.IconStatistic);
                        break;
                    case MenuItemType.IconNewItem:
                        list.Add(type);
                        if (!IconItemExist(MenuItemType.IconManage, IdItem))
                            list.Add(MenuItemType.IconManage);
                        if (!IconItemExist(MenuItemType.IconStatistic, IdItem))
                            list.Add(MenuItemType.IconStatistic);
                        break;
                    case MenuItemType.Link:
                    case MenuItemType.Text:
                        list.Add(MenuItemType.Link);
                        list.Add(MenuItemType.Text);
                        break;
                    default:
                        break;
                }
                return list;
            }
            public List<MenuItemType> GetAvailableSubTypes(dtoMenuItem item)
            {
                return GetAvailableSubTypes(item.Type, item.Id);
            }
            private List<MenuItemType> GetAvailableSubTypes(MenuItemType type, long IdItem)
            {
                List<MenuItemType> list = new List<MenuItemType>();
                switch (type)
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
                        List < MenuItemType> added = GetIconItems(IdItem);

                        if (!added.Contains(MenuItemType.IconNewItem))
                            list.Add(MenuItemType.IconNewItem);
                        if (!added.Contains(MenuItemType.IconManage))
                            list.Add(MenuItemType.IconManage);
                        if (!added.Contains(MenuItemType.IconStatistic))
                            list.Add(MenuItemType.IconStatistic);
                        //list.Add(type);
                        break;
                    default:
                        break;
                }
                return list;
            }
            public List<int> GetItemAvailableProfileTypes(dtoMenuItem item)
            {
                var query = (from i in Manager.GetIQ<_MenuItem>() where i.Id == item.Id select new { IdColumnOwner = i.IdColumnOwner, IdTopOwner = i.IdTopOwner, IdItemOwner = (i.ItemOwner == null) ? 0 : i.ItemOwner.Id }).Skip(0).Take(1).ToList().FirstOrDefault();
                if (query==null)
                    return null;
                else if (query.IdItemOwner != 0)
                    return GetItemAvailableProfileTypes(query.IdItemOwner);
                else if (query.IdColumnOwner != 0)
                    return GetItemAvailableProfileTypes(query.IdTopOwner);
                return null;
            }
            public List<int> GetItemAvailableProfileTypes(MenuItem item)
            {
                if (item.ItemOwner ==null)
                    return GetItemAvailableProfileTypes(item.TopItemOwner.Id);
                else
                    return GetItemAvailableProfileTypes(item.ItemOwner.Id);
            }

            private List<int> GetItemAvailableProfileTypes(long IdItem)
            {
                List<int> list = GetItemProfilesAssignments(IdItem);

                if (list.Count > 0)
                    return list;
                else {
                    var query = (from i in Manager.GetIQ<_MenuItem>() where i.Id == IdItem select new { IdColumnOwner = i.IdColumnOwner, IdTopOwner = i.IdTopOwner, IdItemOwner = (i.ItemOwner == null) ? 0 : i.ItemOwner.Id }).Skip(0).Take(1).ToList().FirstOrDefault();
                    if (query==null)
                        return null;
                    else if (query.IdItemOwner != 0){
                        return GetItemAvailableProfileTypes(query.IdItemOwner);
                    }
                    else if (query.IdColumnOwner != 0){
                        return GetItemAvailableProfileTypes(query.IdTopOwner);
                    }
                    else
                        return null;
                }
            }
            public dtoItem GetFather(long IdItem, MenuItemType type) { 
                dtoItem item  = null;
                switch (type){
                    case MenuItemType.Menubar:
                        break;
                    case MenuItemType.TopItemMenu:
                        item = new dtoItem() {Id = (from t in Manager.GetIQ<_TopItem>() where t.Id== IdItem select t.IdMenubar).Skip(0).Take(1).ToList().FirstOrDefault()
                            , Type= MenuItemType.Menubar};
                        break;
                    case MenuItemType.ItemColumn:
                         item = new dtoItem() {Id = (from t in Manager.GetIQ<_Column>() where t.Id== IdItem select t.IdTopItem).Skip(0).Take(1).ToList().FirstOrDefault()
                            , Type= MenuItemType.TopItemMenu};
                        break;
                    default:
                        var query = (from t in Manager.GetIQ<_MenuItem>() where t.Id == IdItem select new { IdColum = t.IdColumnOwner, IdOwner = (t.ItemOwner == null) ? 0 : t.ItemOwner.Id }).Skip(0).Take(1).ToList().FirstOrDefault();
                        if (query!=null && query.IdOwner==0 && query.IdColum!=0)
                            item = new dtoItem()  { Id = query.IdColum,Type = MenuItemType.ItemColumn};
                        else if (query != null && query.IdOwner != 0)
                            item = new dtoItem() { Id = query.IdOwner, Type = (from t in Manager.GetIQ<_MenuItem>() where t.Id == query.IdOwner select t.Type).Skip(0).Take(1).ToList().FirstOrDefault() };

                        break;
                }
                return item;
            }
        #endregion

        #region "SmallItems"
            public _Menubar GetLazyFullMenubar(long IdMenubar) {
                return Manager.Get<_Menubar>(IdMenubar);
            }
            //public T GetLazyFullMenubar<T>(long IdItem)
            //{
            //    return Manager.Get<T>(IdItem);
            //}
            public dtoTree MenubarToTree(long IdMenubar)
            {
                _Menubar menubar = Manager.Get<_Menubar>(IdMenubar);
                if (menubar != null) { 
                    return menubar.ToTree();
                }
                return null;
            }

            public String RenderMenubar(long IdMenubar, int IdProfileType, int IdLanguage, String Baseurl)
            {
                _Menubar menubar = Manager.Get<_Menubar>(IdMenubar);
                if (menubar != null)
                    return menubar.Render(IdProfileType, IdLanguage, Baseurl);
                return "";
            }

            public String RenderMenubar(long IdMenubar, List<CommunityRoleModulePermission> permission, int IdLanguage, String Baseurl, String DefaultModuleToolTip, String DefaultModuleUrl, String defaultModuleText)
            {
                _Menubar menubar = Manager.Get<_Menubar>(IdMenubar);
                if (menubar != null)
                    return menubar.Render(permission, IdLanguage, Baseurl, DefaultModuleToolTip, DefaultModuleUrl, defaultModuleText);

                return "";
            }
          
           
        #endregion

        #region ""
            public String RenderActiveMenubar(Int32 idCommunity, MenuBarType menubarType, String baseUrl, String defaultModuleToolTip, String defaultModuleUrl, String defaultModuleText)
            {
                return RenderActiveMenubar(idCommunity, menubarType, baseUrl, defaultModuleToolTip, defaultModuleUrl, defaultModuleText, false);
            }
            public String RenderCachedMenubar(Int32 idCommunity, MenuBarType menubarType, String baseUrl, String defaultModuleToolTip, String defaultModuleUrl, String defaultModuleText)
            {
                return RenderActiveMenubar(idCommunity, menubarType, baseUrl, defaultModuleToolTip, defaultModuleUrl, defaultModuleText, true);
            }
            private String RenderActiveMenubar(Int32 idCommunity,MenuBarType menubarType, String baseUrl, String defaultModuleToolTip, String defaultModuleUrl, String defaultModuleText, Boolean useCache)
            {
                litePerson person = Manager.Get<litePerson>(UC.CurrentUserID);
                if (person != null)
                {
                    if (idCommunity == 0 && menubarType == MenuBarType.GenericCommunity)
                        menubarType = MenuBarType.Portal;
                    if (menubarType == MenuBarType.GenericCommunity)
                        return RenderActiveMenubar(idCommunity, person, baseUrl, defaultModuleToolTip, defaultModuleUrl, defaultModuleText, useCache);
                    else
                        return RenderActiveMenubar(menubarType, person.TypeID, person.LanguageID, baseUrl, useCache);
                }
                else
                    return "<div id=\"nav-main\"><div class=\"page-width\"> </div></div>";
            }

            private String RenderActiveMenubar(MenuBarType menubarType, int idProfileType, int idLanguage, String baseUrl, Boolean useCache)
            {
                String render = "";
                render = (useCache) ? lm.Comol.Core.DomainModel.Helpers.CacheHelper.Find<String>(CacheKeys.RenderPortal(menubarType, idProfileType, idLanguage)) : "";

                if (String.IsNullOrEmpty(render))
                {
                    _Menubar menubar = (useCache) ? GetCachedMenubar(menubarType) : GetDefaultMenubar(menubarType);
                    if (menubar != null)
                        render = menubar.Render(idProfileType, idLanguage, baseUrl);
                    else
                        render = "<div id=\"nav-main\"><div class=\"page-width\"> </div></div>";
                    if (useCache)
                        CacheHelper.AddToCache<String>(CacheKeys.RenderPortal(menubarType, idProfileType, idLanguage), render, CacheExpiration.Day);
                }
                //else
                //    render = "<div id=\"nav-main\"><div class=\"page-width\"> </div></div>";
                return render;
            }
            private String RenderActiveMenubar(Int32 idCommunity, litePerson person, String baseUrl, String defaultModuleToolTip, String defaultModuleUrl, String defaultModuleText, Boolean useCache)
            {
                String render = "";
                int idRole = Manager.GetSubscriptionIdRole(person.Id, idCommunity);
                int idLanguage = person.LanguageID;
                render = (useCache) ? lm.Comol.Core.DomainModel.Helpers.CacheHelper.Find<String>(CacheKeys.RenderCommunity(idCommunity, idRole, idLanguage)) : "";

                if (String.IsNullOrEmpty(render))
                {
                    _Menubar menubar = (useCache) ? GetCachedMenubar(MenuBarType.GenericCommunity) : GetDefaultMenubar(MenuBarType.GenericCommunity);

                    if (menubar != null)
                    {
                        List<ModuleDefinition> availableModules = (from m in Manager.GetIQ<ModuleCommunity>() where m.Community.Id == idCommunity && m.Enabled == true select m).ToList().Select(m => m.ModuleDefinition).ToList(); 
                        List<CommunityRoleModulePermission> permissions = (from c in Manager.Linq<CommunityRoleModulePermission>()
                                                                           where c.Community.Id == idCommunity && c.Service.Available && c.Role.Id == idRole && availableModules.Contains(c.Service)
                                                                           select c).ToList();
                        if (defaultModuleUrl.Contains("{0}"))
                            defaultModuleUrl = string.Format(defaultModuleUrl, idCommunity);
                        render = menubar.Render(permissions, person.LanguageID, baseUrl, defaultModuleToolTip, defaultModuleUrl, defaultModuleText);
                    }
                    else
                        render = "<div id=\"nav-main\"><div class=\"page-width\"> </div></div>";
                  
                    if (useCache)
                        CacheHelper.AddToCache<String>(CacheKeys.RenderCommunity(idCommunity, idRole, idLanguage), render, CacheExpiration.Week);
                }
                //else
                //    render = "<div id=\"nav-main\"><div class=\"page-width\"> </div></div>";
                return render;
            }

            private _Menubar GetDefaultMenubar(MenuBarType menubarType)
            {
                long idMenubar = (from m in Manager.GetIQ<_Menubar>()
                                  where m.MenuBarType == menubarType && m.Deleted == BaseStatusDeleted.None && m.IsCurrent
                                  select m.Id).Skip(0).Take(1).ToList().FirstOrDefault();
                return Manager.Get<_Menubar>(idMenubar);
            }

            private _Menubar GetCachedMenubar(MenuBarType menubarType)
            {
                _Menubar menubar = CacheHelper.Find<_Menubar>(CacheKeys.MenuBar(menubarType));
                if (menubar == null) {
                    menubar =GetDefaultMenubar(menubarType);
                    if (menubar != null)
                    {
                        Manager.Detach(menubar);
                        CacheHelper.PurgeCacheItems(CacheKeys.RenderMenu(menubarType));
                        CacheHelper.AddToCache<_Menubar>(CacheKeys.MenuBar(menubarType), menubar, CacheExpiration.Month);
                    }
                }
                return menubar;
            }
        #endregion

            #region "Add item"
            public TopMenuItem AddItemToMenubar( long idMenubar) {
                TopMenuItem topItem = null;
                try
                {
                    Menubar menubar = Manager.Get<Menubar>(idMenubar);
                    if (menubar != null)
                    {
                        Manager.BeginTransaction();
                        long displayOrder = (menubar.Items.Count() + 1);
                        topItem = new TopMenuItem();
                        topItem.CreateMetaInfo(CurrentUser(), UC.IpAddress, UC.ProxyIpAddress);
                        topItem.CssClass = "";
                        topItem.DisplayOrder = displayOrder;
                        topItem.IsEnabled = true;
                        topItem.Menubar = menubar;
                        topItem.Name = displayOrder.ToString();
                        topItem.ShowDisabledItems = false;
                        topItem.TextPosition = TextPosition.DisplayLeft;
                        topItem.Columns = new List<ItemColumn>();
                        topItem.Translations = new List<MenuItemTranslation>();
                        Manager.SaveOrUpdate(topItem);
                        topItem.Translations = CreateItemTranslations(menubar, topItem, topItem);
                        menubar.UpdateMetaInfo(topItem.CreatedBy, UC.IpAddress, UC.ProxyIpAddress, topItem.CreatedOn.Value);
                        Manager.SaveOrUpdate(topItem);
                        Manager.Commit();
                    }
                }
                catch (Exception ex) {
                    Manager.RollBack();
                    topItem = null;
                }
                return topItem;
            }
            public ItemColumn AddItemToTopItem(long idTopItem)
            {
                ItemColumn column = null;
                try
                {
                    TopMenuItem item = Manager.Get<TopMenuItem>(idTopItem);
                    if (item != null)
                    {
                        Manager.BeginTransaction();
                        column = CreateItemColumn(item.Menubar, item);
                        long displayOrder = (item.Columns.Count() + 1);
                        column.DisplayOrder = displayOrder;
                        Manager.SaveOrUpdate(column);
                        item.Menubar.UpdateMetaInfo(column.CreatedBy, UC.IpAddress, UC.ProxyIpAddress, column.CreatedOn.Value);
                        Manager.Commit();
                    }
                }
                catch (Exception ex)
                {
                    Manager.RollBack();
                    column = null;
                }
                return column;
            }
            public MenuItem AddToColumn(long idColumn, MenuItemType type)
            {
                ItemColumn item = Manager.Get<ItemColumn>(idColumn);
                if (item != null) {
                    MenuItem newItem = null;
                    try
                    {
                        Manager.BeginTransaction();
                        newItem = new MenuItem();
                        newItem.CreateMetaInfo(CurrentUser(), UC.IpAddress, UC.ProxyIpAddress);
                        newItem.ColumnOwner = item;
                        newItem.CssClass = "";
                        switch (type) { 
                            case MenuItemType.IconManage:
                                newItem.DisplayOrder = 2;
                                break;
                            case MenuItemType.IconNewItem:
                                newItem.DisplayOrder = 1;
                                break;
                            case MenuItemType.IconStatistic:
                                newItem.DisplayOrder = 3;
                                break;
                            default:
                                newItem.DisplayOrder = ((from i in item.Items where i.ItemOwner==null select i.Id).Count() + 1);
                                if (type != MenuItemType.Separator)
                                    newItem.Name = newItem.DisplayOrder.ToString();
                                else
                                    newItem.Name = "";
                                break;
                        }
                       
                        newItem.IdModule = 0;
                        newItem.IsEnabled = true;
                        newItem.ModuleCode = "";
                        newItem.TextPosition = TextPosition.DisplayLeft;
                        newItem.ShowDisabledItems = false;
                        newItem.TopItemOwner = item.TopItemOwner;
                        newItem.Type = type;
                        newItem.Menubar = item.Menubar;
                        newItem.Translations = new List<MenuItemTranslation>();

                        Manager.SaveOrUpdate(newItem);

                        if (!(type == MenuItemType.Separator))
                        { //|| type == MenuItemType.IconManage || type == MenuItemType.IconStatistic ||  type == MenuItemType.IconManage)
                            newItem.Translations = CreateItemTranslations(newItem.Menubar, newItem, newItem.TopItemOwner);
                            Manager.SaveOrUpdate(newItem);
                        }

                        item.Menubar.UpdateMetaInfo(newItem.CreatedBy, UC.IpAddress, UC.ProxyIpAddress, newItem.CreatedOn.Value);
                        Manager.Commit();
                        return newItem;
                    }
                    catch (Exception ex)
                    {
                        Manager.RollBack();
                        return null;
                    }
                }
                else
                    return null;
            }
            public MenuItem AddToItem(long idItem, MenuItemType type)
            {
                MenuItem item = Manager.Get<MenuItem>(idItem);
                if (item != null)
                {
                    MenuItem newItem = null;
                    try
                    {
                        Manager.BeginTransaction();
                        newItem = new MenuItem();
                        newItem.CreateMetaInfo(CurrentUser(), UC.IpAddress, UC.ProxyIpAddress);
                        newItem.ColumnOwner = item.ColumnOwner;
                        newItem.CssClass = "";
                        switch (type)
                        {
                            case MenuItemType.IconManage:
                                newItem.DisplayOrder = 3;
                                break;
                            case MenuItemType.IconNewItem:
                                newItem.DisplayOrder = 1;
                                break;
                            case MenuItemType.IconStatistic:
                                newItem.DisplayOrder = 2;
                                break;
                            default:
                                newItem.DisplayOrder = ((from i in item.Childrens where i.ItemOwner== item select i.Id).Count() + 1);
                                if (type != MenuItemType.Separator)
                                    newItem.Name = newItem.DisplayOrder.ToString();
                                else
                                    newItem.Name = "";
                                break;
                        }

                        newItem.IdModule = 0;
                        newItem.IsEnabled = true;
                        newItem.ModuleCode = "";
                        newItem.TextPosition = TextPosition.DisplayLeft;
                        newItem.ShowDisabledItems = false;
                        newItem.TopItemOwner = item.TopItemOwner;
                        newItem.Type = type;
                        newItem.Menubar = item.Menubar;
                        newItem.Translations = new List<MenuItemTranslation>();
                        newItem.ItemOwner = item;
                        Manager.SaveOrUpdate(newItem);

                        if (!(type == MenuItemType.Separator))
                        { //|| type == MenuItemType.IconManage || type == MenuItemType.IconStatistic || type == MenuItemType.IconManage
                            newItem.Translations = CreateItemTranslations(newItem.Menubar, newItem, newItem.TopItemOwner);
                            Manager.SaveOrUpdate(newItem);
                        }
                        item.Menubar.UpdateMetaInfo(newItem.CreatedBy, UC.IpAddress, UC.ProxyIpAddress, newItem.CreatedOn.Value);
                        Manager.Commit();
                        return newItem;
                    }
                    catch (Exception ex)
                    {
                        Manager.RollBack();
                        return null;
                    }
                }
                else
                    return null;
            }
        #endregion

        #region "Items"
            public ItemColumn SaveItem(dtoColumn dto)
            {
                ItemColumn column = null;
                try
                {
                    Manager.BeginTransaction();
                    TopMenuItem topItem = Manager.Get<TopMenuItem>(dto.IdTopItem);
                    if (topItem != null) {
                        if (dto.Id == 0)
                        {
                            column = new ItemColumn() { TopItemOwner = topItem, Menubar = topItem.Menubar, Items = new List<MenuItem>() };
                            long count = (from c in Manager.GetIQ<ItemColumn>() where c.Deleted == BaseStatusDeleted.None && c.TopItemOwner == topItem select c.Id).Count();
                            if (count >= dto.DisplayOrder || (from c in Manager.GetIQ<ItemColumn>() where c.Deleted == BaseStatusDeleted.None && c.TopItemOwner == topItem && c.DisplayOrder == dto.DisplayOrder select c.Id).Any())
                            {
                                long displayOrder = 1;
                                column.DisplayOrder = count + 1;
                                List<ItemColumn> columns = (from c in Manager.GetIQ<ItemColumn>() where c.Deleted == BaseStatusDeleted.None && c.TopItemOwner == topItem  orderby c.DisplayOrder select c).ToList();
                                columns.ForEach(c => c.DisplayOrder = displayOrder++ );
                                Manager.SaveOrUpdateList(columns);
                            }
                        }
                        else
                            column = Manager.Get<ItemColumn>(dto.Id);
                        if (column != null)
                        {
                            if (dto.Id >= 0)
                            {
                                column.UpdateMetaInfo(CurrentUser(), UC.IpAddress, UC.ProxyIpAddress);
                                if (column.DisplayOrder != dto.DisplayOrder)
                                    UpdateDisplayOrder(dto.DisplayOrder, column);
                                topItem.Menubar.UpdateMetaInfo(column.ModifiedBy, UC.IpAddress, UC.ProxyIpAddress, column.ModifiedOn.Value);
                            }
                            else
                            {
                                column.CreateMetaInfo(CurrentUser(), UC.IpAddress, UC.ProxyIpAddress);
                                topItem.Menubar.UpdateMetaInfo(column.CreatedBy, UC.IpAddress, UC.ProxyIpAddress, column.CreatedOn.Value);
                            }
                            column.CssClass = dto.CssClass;
                            column.Deleted = BaseStatusDeleted.None;
                            column.DisplayOrder = dto.DisplayOrder;
                            column.HasSeparator = dto.HasSeparator;
                            column.HeightPx = dto.HeightPx;
                            column.IsEnabled = dto.IsEnabled;
                            column.WidthPx = dto.WidthPx;
                            Manager.SaveOrUpdate(column);
                        }
                    }
                    Manager.Commit();
                }
                catch (Exception ex)
                {
                    Manager.RollBack();
                    column = null;
                }
                return column;
            }
            public TopMenuItem SaveItem(dtoTopMenuItem dto, List<dtoTranslation> translations)
            {
                TopMenuItem item = null;
                try
                {
                    Manager.BeginTransaction();
                    Menubar menubar = Manager.Get<Menubar>(dto.IdMenubar);
                    if (menubar != null)
                    {
                        item = SaveItem(menubar, dto, translations);

                    }
                    Manager.Commit();
                }
                catch (Exception ex)
                {
                    Manager.RollBack();
                    item = null;
                }
                return item;
            }
            public TopMenuItem SaveItem(dtoTopMenuItem dto, List<dtoTranslation> translations, List<int> selectedTypes)
            {
                TopMenuItem item = null;
                try
                {
                    Manager.BeginTransaction();
                    Menubar menubar = Manager.Get<Menubar>(dto.IdMenubar);
                    if (menubar != null)
                    {
                        item = SaveItem(menubar, dto, translations);
                        SaveProfileAssignments(item, selectedTypes);
                    }
                    Manager.Commit();
                }
                catch (Exception ex)
                {
                    Manager.RollBack();
                    item = null;
                }
                return item;
            }
            private TopMenuItem SaveItem(Menubar menubar,dtoTopMenuItem dto, List<dtoTranslation> translations)
            {
                TopMenuItem item = null;
                if (menubar != null)
                {
                    if (dto.Id == 0)
                    {
                        item = new TopMenuItem() { Menubar = menubar, Columns = new List<ItemColumn>() };
                        long count = (from c in Manager.GetIQ<TopMenuItem>() where c.Deleted == BaseStatusDeleted.None && c.Menubar == menubar select c.Id).Count();
                        if (count >= dto.DisplayOrder || (from c in Manager.GetIQ<TopMenuItem>() where c.Deleted == BaseStatusDeleted.None && c.Menubar == menubar && c.DisplayOrder == dto.DisplayOrder select c.Id).Any())
                        {
                            long displayOrder = 1;
                            dto.DisplayOrder = count + 1;
                            List<TopMenuItem> items = (from c in Manager.GetIQ<TopMenuItem>() where c.Deleted == BaseStatusDeleted.None && c.Menubar == menubar orderby c.DisplayOrder select c).ToList();
                            items.ForEach(i => i.DisplayOrder = displayOrder++);
                            Manager.SaveOrUpdateList(items);
                        }
                    }
                    else
                        item = Manager.Get<TopMenuItem>(dto.Id);
                    if (item != null)
                    {
                        if (dto.Id >= 0)
                        {
                            item.UpdateMetaInfo(CurrentUser(), UC.IpAddress, UC.ProxyIpAddress);
                            if (item.DisplayOrder != dto.DisplayOrder)
                                UpdateDisplayOrder(dto.DisplayOrder, item);
                            menubar.UpdateMetaInfo(item.ModifiedBy, UC.IpAddress, UC.ProxyIpAddress, item.ModifiedOn.Value);
                        }
                        else
                        {
                            item.CreateMetaInfo(CurrentUser(), UC.IpAddress, UC.ProxyIpAddress);
                            menubar.UpdateMetaInfo(item.CreatedBy, UC.IpAddress, UC.ProxyIpAddress, item.CreatedOn.Value);
                        }
                        item.CssClass = dto.CssClass;
                        item.Deleted = BaseStatusDeleted.None;
                        item.DisplayOrder = dto.DisplayOrder;
                        item.IdModule = dto.IdModule;
                        if (dto.IdModule > 0)
                            item.ModuleCode = Manager.GetModuleCode(dto.IdModule);
                        item.IsEnabled = dto.IsEnabled;
                        item.Link = dto.Link;
                        item.Name = dto.Name;
                        item.Permission = dto.Permission;
                        item.ShowDisabledItems = dto.ShowDisabledItems;
                        item.TextPosition = dto.TextPosition;
                        Manager.SaveOrUpdate(item);
                        SaveTranslations(item, item, translations);
                    }
                }
                return item;
            }
            private void UpdateDisplayOrder(long displayOrder, ItemColumn item)
            {
                List<ItemColumn> list = null;
                list = (from i in Manager.GetIQ<ItemColumn>()
                            where i.Deleted == BaseStatusDeleted.None && i.TopItemOwner == item.TopItemOwner && i.DisplayOrder >= displayOrder && i.Id != item.Id
                            orderby i.DisplayOrder
                            select i).ToList();

                long newDisplayOrder = 1;
                foreach (ItemColumn it in list)
                {
                    if (newDisplayOrder == displayOrder)
                        newDisplayOrder++;
                    it.DisplayOrder = newDisplayOrder++;
                }
                if (list.Count > 0)
                    Manager.SaveOrUpdateList(list);
            }
            private void UpdateDisplayOrder(long displayOrder, TopMenuItem item)
            {
                List<TopMenuItem> list = (from i in Manager.GetIQ<TopMenuItem>()
                                          where i.Deleted == BaseStatusDeleted.None && i.Menubar == item.Menubar  && i.Id != item.Id
                                          orderby i.DisplayOrder 
                                          select i).ToList();
                long newDisplayOrder = 1;
                foreach (TopMenuItem it in list)
                {
                    if (newDisplayOrder == displayOrder)
                        newDisplayOrder++;
                    it.DisplayOrder = newDisplayOrder++;
                }
                if (list.Count>0)
                    Manager.SaveOrUpdateList(list);
            }
            private void UpdateDisplayOrder(long displayOrder, MenuItem item)
            {
                List<MenuItem> list = (from i in Manager.GetIQ<MenuItem>()
                                       where i.Deleted == BaseStatusDeleted.None && i.Menubar == item.Menubar && i.ColumnOwner == item.ColumnOwner && i.ItemOwner == item.ItemOwner && i.Id != item.Id
                                          orderby i.DisplayOrder
                                          select i).ToList();
                long newDisplayOrder =1;
                foreach (MenuItem it in list.Where(i=> i.Type != MenuItemType.IconManage && i.Type != MenuItemType.IconNewItem && i.Type != MenuItemType.IconStatistic)) { 
                    if (newDisplayOrder== displayOrder)
                        newDisplayOrder++;
                    it.DisplayOrder = newDisplayOrder++;
                }

                //list.ForEach(i => i.DisplayOrder = newDisplayOrder++);
                if (list.Count > 0) 
                    Manager.SaveOrUpdateList(list);
            }
            private IList<MenuItemTranslation> SaveTranslations(TopMenuItem topItem, BaseMenuItem item, List<dtoTranslation> translations)
            {
                IList<MenuItemTranslation> itemTranslations = item.Translations;
                foreach (dtoTranslation dto in translations)
                {
                    MenuItemTranslation itemTranslation = null;
                    if ((from t in itemTranslations where t.Language.Id == dto.IdLanguage select t).Any())
                    {
                        itemTranslation = (from t in itemTranslations 
                                           where t.Language.Id == dto.IdLanguage && (t.Deleted!= BaseStatusDeleted.None || t.ToolTip != dto.ToolTip || t.Name != dto.Name )
                                           select t).FirstOrDefault();
                        
                        if (itemTranslation!=null){
                            itemTranslation.Deleted= BaseStatusDeleted.None;
                            itemTranslation.ToolTip= dto.ToolTip;
                            itemTranslation.Name = dto.Name;
                            itemTranslation.UpdateMetaInfo(CurrentUser(), UC.IpAddress, UC.ProxyIpAddress);
                        }
                    }
                    else {
                        itemTranslation = new MenuItemTranslation();
                        itemTranslation.Item = item;
                        itemTranslation.CreateMetaInfo(CurrentUser(), UC.IpAddress, UC.ProxyIpAddress);
                        itemTranslation.Menubar = item.Menubar;
                        itemTranslation.Name = dto.Name;
                        itemTranslation.ToolTip = dto.ToolTip;
                        itemTranslation.TopMenuItem = topItem;
                        Manager.SaveOrUpdate(itemTranslation);
                        itemTranslations.Add(itemTranslation);
                    }
                }
                return itemTranslations;
            }
            private IList<ProfileAssignment> SaveProfileAssignments(BaseMenuItem item, List<int> selectedTypes)
            {
                IList<ProfileAssignment> assignments = item.ProfileAvailability;
                foreach (int IdProfileType in selectedTypes)
                {
                    ProfileAssignment assignment = null;
                    if ((from a in assignments where a.IdProfileType == IdProfileType select a).Any())
                    {
                        assignment = (from a in assignments
                                      where a.IdProfileType == IdProfileType && a.Deleted != BaseStatusDeleted.None
                                      select a).FirstOrDefault();

                        if (assignment != null)
                        {
                            assignment.Deleted = BaseStatusDeleted.None;
                            assignment.UpdateMetaInfo(CurrentUser(), UC.IpAddress, UC.ProxyIpAddress);
                        }
                    }
                    else {
                        assignment = new ProfileAssignment();
                        assignment.ItemOwner = item;
                        assignment.Menubar = item.Menubar;
                        assignment.IdProfileType = IdProfileType;
                        assignment.CreateMetaInfo(CurrentUser(), UC.IpAddress, UC.ProxyIpAddress);
                        Manager.SaveOrUpdate(assignment);
                        assignments.Add(assignment);
                    }
                }
                foreach (ProfileAssignment assignment in assignments.Where(a => !selectedTypes.Contains(a.IdProfileType)).ToList())
                {
                    assignment.Deleted = BaseStatusDeleted.Manual;
                    assignment.UpdateMetaInfo(CurrentUser(), UC.IpAddress, UC.ProxyIpAddress);
                }
            //    Manager.DeletePhysicalList(assignments.Where(a => !selectedTypes.Contains(a.IdProfileType)).ToList());
                return assignments;
            }


            public MenuItem SaveItem(dtoMenuItem dto, List<dtoTranslation> translations)
            {
                MenuItem item = null;
                try
                {
                    Manager.BeginTransaction();
                    Menubar menubar = Manager.Get<Menubar>(dto.IdMenubar);
                    if (menubar != null)
                    {
                        item = SaveItem(menubar, dto, translations);
                    }
                    Manager.Commit();
                }
                catch (Exception ex)
                {
                    Manager.RollBack();
                    item = null;
                }
                return item;
            }
            public MenuItem SaveItem(dtoMenuItem dto, List<dtoTranslation> translations, List<int> selectedTypes)
            {
                MenuItem item = null;
                try
                {
                    Manager.BeginTransaction();
                    Menubar menubar = Manager.Get<Menubar>(dto.IdMenubar);
                    if (menubar != null)
                    {
                        item = SaveItem(menubar, dto, translations);
                        SaveProfileAssignments(item, selectedTypes);
                    }
                    Manager.Commit();
                }
                catch (Exception ex)
                {
                    Manager.RollBack();
                    item = null;
                }
                return item;
            }
            private MenuItem SaveItem(Menubar menubar, dtoMenuItem dto, List<dtoTranslation> translations)
            {
                MenuItem item = null;
                if (menubar != null)
                {
                    if (dto.Id == 0)
                    {
                        item = new MenuItem() { Menubar = menubar, ItemOwner = Manager.Get<MenuItem>(dto.IdItemOwner), ColumnOwner= Manager.Get<ItemColumn>(dto.IdColumnOwner) };
                        if (item.ColumnOwner == null)
                            return null;
                        item.TopItemOwner = item.ColumnOwner.TopItemOwner;
                        long count = (from c in Manager.GetIQ<MenuItem>() 
                                      where c.Deleted == BaseStatusDeleted.None && c.Menubar == menubar && c.ColumnOwner == item.ColumnOwner && c.ItemOwner== item.ItemOwner 
                                      select c.Id).Count();
                        if (count >= dto.DisplayOrder || (from c in Manager.GetIQ<MenuItem>() where c.Deleted == BaseStatusDeleted.None && c.Menubar == menubar && c.ColumnOwner == item.ColumnOwner && c.ItemOwner== item.ItemOwner && c.DisplayOrder == dto.DisplayOrder select c.Id).Any())
                        {
                            long displayOrder = 1;
                            dto.DisplayOrder = count + 1;
                            List<MenuItem> items = (from c in Manager.GetIQ<MenuItem>() where c.Deleted == BaseStatusDeleted.None && c.Menubar == menubar && c.ColumnOwner == item.ColumnOwner && c.ItemOwner == item.ItemOwner orderby c.DisplayOrder select c).ToList();
                            items.ForEach(i => i.DisplayOrder = displayOrder++);
                            Manager.SaveOrUpdateList(items);
                        }
                    }
                    else
                        item = Manager.Get<MenuItem>(dto.Id);
                    if (item != null)
                    {
                        if (dto.Id >= 0)
                        {
                            item.UpdateMetaInfo(CurrentUser(), UC.IpAddress, UC.ProxyIpAddress);
                            if (item.DisplayOrder != dto.DisplayOrder)
                            {
                                switch (dto.Type)
                                {
                                    case MenuItemType.IconNewItem:
                                        dto.DisplayOrder = 1;
                                        break;
                                    case MenuItemType.IconStatistic:
                                        dto.DisplayOrder = 2;
                                        break;
                                    case MenuItemType.IconManage:
                                        dto.DisplayOrder = 3;
                                        break;
                                    default:
                                        UpdateDisplayOrder(dto.DisplayOrder, item);
                                        break;
                                }
                            }
                            menubar.UpdateMetaInfo(item.ModifiedBy, UC.IpAddress, UC.ProxyIpAddress, item.ModifiedOn.Value);
                        }
                        else
                        {
                            switch (dto.Type)
                            {
                                case MenuItemType.IconNewItem:
                                    dto.DisplayOrder = 1;
                                    break;
                                case MenuItemType.IconStatistic:
                                    dto.DisplayOrder = 2;
                                    break;
                                case MenuItemType.IconManage:
                                    dto.DisplayOrder = 3;
                                    break;
                                default:
                                    break;
                            }
                            item.CreateMetaInfo(CurrentUser(), UC.IpAddress, UC.ProxyIpAddress);
                            menubar.UpdateMetaInfo(item.CreatedBy, UC.IpAddress, UC.ProxyIpAddress, item.CreatedOn.Value);
                        }
                        item.CssClass = dto.CssClass;
                        item.Deleted = BaseStatusDeleted.None;
                        item.DisplayOrder = dto.DisplayOrder;
                        item.IdModule = dto.IdModule;
                        if (dto.IdModule > 0)
                            item.ModuleCode = Manager.GetModuleCode(dto.IdModule);
                        item.IsEnabled = dto.IsEnabled;
                        item.Link = dto.Link;
                        item.Name = dto.Name;
                        item.Permission = dto.Permission;
                        item.ShowDisabledItems = dto.ShowDisabledItems;
                        item.TextPosition = dto.TextPosition;
                        item.Type = dto.Type;
                        Manager.SaveOrUpdate(item);
                        SaveTranslations(item.TopItemOwner, item, translations);
                    }
                }
                return item;
            }

            private void UpdateMenubarMetaInfo(TopMenuItem item, Boolean auto = false)
            {
                if (item != null && item.Id > 0 && item.Menubar != null)
                {
                    if (auto)
                        item.Menubar.UpdateMetaInfo(CurrentUser(), UC.IpAddress, UC.ProxyIpAddress);
                    else
                        item.Menubar.UpdateMetaInfo(item.ModifiedBy, UC.IpAddress, UC.ProxyIpAddress, item.ModifiedOn.Value);
                }
            }
            private void UpdateMenubarMetaInfo(ItemColumn item, Boolean auto = false)
            {
                if (item != null && item.Id > 0 && item.Menubar != null)
                {
                    if (auto)
                        item.Menubar.UpdateMetaInfo(CurrentUser(), UC.IpAddress, UC.ProxyIpAddress);
                    else
                        item.Menubar.UpdateMetaInfo(item.ModifiedBy, UC.IpAddress, UC.ProxyIpAddress, item.ModifiedOn.Value);
                }
            }
            private void UpdateMenubarMetaInfo(MenuItem item, Boolean auto = false)
            {
                if (item != null && item.Id > 0 && item.Menubar != null)
                {
                    if (auto)
                        item.Menubar.UpdateMetaInfo(CurrentUser(), UC.IpAddress, UC.ProxyIpAddress);
                    else
                        item.Menubar.UpdateMetaInfo(item.ModifiedBy, UC.IpAddress, UC.ProxyIpAddress, item.ModifiedOn.Value);
                }
            }
            public Boolean PhisicalDeleteItem(long IdItem, MenuItemType itemType)
            {
                Boolean deleted = false;
                try
                {
                    Manager.BeginTransaction();
                    switch (itemType)
                    {
                        case MenuItemType.Menubar:
                            Menubar menubar = Manager.Get<Menubar>(IdItem);
                            if (menubar != null)
                            {
                                foreach (TopMenuItem mi in menubar.Items)
                                {
                                    Manager.DeletePhysical(mi);
                                }
                                Manager.DeletePhysical(menubar);
                            }
                            Manager.Commit();
                            break;
                        case MenuItemType.TopItemMenu:
                            TopMenuItem item = Manager.Get<TopMenuItem>(IdItem);
                            UpdateMenubarMetaInfo(item,true );
                          
                           // (item);
                            ReorderForDelete(item);
                            item.Menubar.UpdateMetaInfo(CurrentUser(), UC.IpAddress, UC.ProxyIpAddress);
                            Manager.DeletePhysical(item);
                            Manager.Commit();
                            break;
                        case MenuItemType.ItemColumn:
                            ItemColumn column = Manager.Get<ItemColumn>(IdItem);
                            UpdateMenubarMetaInfo(column, true);
                            //VirtualDeleteItem(column);
                            ReorderForDelete(column);
                            Manager.DeletePhysical(column);
                            Manager.Commit();
                            break;
                        default:
                            MenuItem menuItem = Manager.Get<MenuItem>(IdItem);
                            //PhisicalDeleteItem(menuItem);
                            //Manager.DeletePhysicalList(menuItem.Translations);
                            //Manager.DeletePhysicalList(menuItem.ProfileAvailability);
                            UpdateMenubarMetaInfo(menuItem, true);
                            ReorderForDelete(menuItem);
                            Manager.DeletePhysical(menuItem);
                            Manager.Commit();
                            break;
                    }
                    deleted = true;
                }
                catch (Exception ex)
                {
                    Manager.RollBack();
                }
                return deleted;
            }
            public Boolean VirtualDeleteItem(long IdItem, MenuItemType itemType)
            {
                Boolean deleted = false;
                try
                {
                    Manager.BeginTransaction();
                    switch (itemType) { 
                        case MenuItemType.Menubar:
                            Menubar menuBar = Manager.Get<Menubar>(IdItem);
                            if (menuBar != null) {
                                menuBar.UpdateMetaInfo(CurrentUser(), UC.IpAddress, UC.ProxyIpAddress);
                                menuBar.Deleted = BaseStatusDeleted.Manual;
                                Manager.SaveOrUpdate(menuBar);
                                Manager.Commit();
                            }
                            break;
                        case MenuItemType.TopItemMenu:
                            TopMenuItem item = Manager.Get<TopMenuItem>(IdItem);
                            item.UpdateMetaInfo(CurrentUser(), UC.IpAddress, UC.ProxyIpAddress);
                            item.Deleted = BaseStatusDeleted.Manual;
                            UpdateMenubarMetaInfo(item);
                            VirtualDeleteItem(item);
                            ReorderForDelete(item);
                            Manager.SaveOrUpdate(item);
                            Manager.Commit();
                            break;
                        case MenuItemType.ItemColumn:
                            ItemColumn column = Manager.Get<ItemColumn>(IdItem);
                            column.UpdateMetaInfo(CurrentUser(), UC.IpAddress, UC.ProxyIpAddress);
                            column.Deleted = BaseStatusDeleted.Manual;
                            UpdateMenubarMetaInfo(column);
                            VirtualDeleteItem(column);
                            ReorderForDelete(column);
                            Manager.SaveOrUpdate(column);
                            Manager.Commit();
                            break;
                        default:
                            MenuItem menuItem = Manager.Get<MenuItem>(IdItem);
                            menuItem.UpdateMetaInfo(CurrentUser(), UC.IpAddress, UC.ProxyIpAddress);
                            menuItem.Deleted = BaseStatusDeleted.Manual;
                            UpdateMenubarMetaInfo(menuItem);
                            VirtualDeleteItem(menuItem);
                            ReorderForDelete(menuItem);
                            Manager.SaveOrUpdate(menuItem);
                            Manager.Commit();
                            break;
                    }
                    deleted= true;
                }
                catch (Exception ex)
                {
                    Manager.RollBack();
                }
                return deleted;
            }
            public Boolean VirtualUnDeleteItem(long IdItem, MenuItemType itemType)
            {
                Boolean deleted = false;
                try
                {
                    Manager.BeginTransaction();
                    switch (itemType)
                    {
                        case MenuItemType.Menubar:
                            Menubar menuBar = Manager.Get<Menubar>(IdItem);
                            if (menuBar != null)
                            {
                                menuBar.UpdateMetaInfo(CurrentUser(), UC.IpAddress, UC.ProxyIpAddress);
                                menuBar.Deleted = BaseStatusDeleted.None;
                                Manager.SaveOrUpdate(menuBar);
                                Manager.Commit();
                            }
                            break;
                        case MenuItemType.TopItemMenu:
                            TopMenuItem item = Manager.Get<TopMenuItem>(IdItem);
                            item.UpdateMetaInfo(CurrentUser(), UC.IpAddress, UC.ProxyIpAddress);
                            UpdateMenubarMetaInfo(item);
                            item.Deleted = BaseStatusDeleted.None;
                            VirtualDeleteItem(item);
                            ReorderForDelete(item);
                            Manager.SaveOrUpdate(item);
                            Manager.Commit();
                            break;
                        case MenuItemType.ItemColumn:
                            ItemColumn column = Manager.Get<ItemColumn>(IdItem);
                            column.UpdateMetaInfo(CurrentUser(), UC.IpAddress, UC.ProxyIpAddress);
                            UpdateMenubarMetaInfo(column);
                              column.Deleted = BaseStatusDeleted.None;
                            VirtualDeleteItem(column);
                            ReorderForDelete(column);
                            Manager.SaveOrUpdate(column);
                            Manager.Commit();
                            break;
                        default:
                            MenuItem menuItem = Manager.Get<MenuItem>(IdItem);
                            menuItem.UpdateMetaInfo(CurrentUser(), UC.IpAddress, UC.ProxyIpAddress);
                            UpdateMenubarMetaInfo(menuItem);
                            menuItem.Deleted = BaseStatusDeleted.None;
                            VirtualDeleteItem(menuItem);
                            ReorderForDelete(menuItem);
                            Manager.SaveOrUpdate(menuItem);
                            Manager.Commit();
                            break;
                    }
                    deleted = true;
                }
                catch (Exception ex)
                {
                    Manager.RollBack();
                }
                return deleted;
            }
            private void VirtualDeleteItem(TopMenuItem item)
            {
               foreach (ItemColumn column in item.Columns) {
                   column.UpdateMetaInfo(CurrentUser(), UC.IpAddress, UC.ProxyIpAddress);
                   column.Deleted |= BaseStatusDeleted.Automatic;
                   VirtualDeleteItem(column);
                   Manager.SaveOrUpdate(column);
              }
            }
            private void VirtualDeleteItem(ItemColumn column)
            {
                foreach (MenuItem item in column.Items)
                {
                    item.UpdateMetaInfo(CurrentUser(), UC.IpAddress, UC.ProxyIpAddress);
                    item.Deleted |= BaseStatusDeleted.Automatic;
                    VirtualDeleteItem(item);
                    Manager.SaveOrUpdate(item);
                }
            }
            private void VirtualDeleteItem(MenuItem item)
            {
                foreach (MenuItem child in item.Childrens)
                {
                    child.UpdateMetaInfo(CurrentUser(), UC.IpAddress, UC.ProxyIpAddress);
                    child.Deleted |= BaseStatusDeleted.Automatic;

                    foreach (MenuItem children in child.Childrens)
                        VirtualDeleteItem(children);
                    Manager.SaveOrUpdate(child);
                  
                }
            }

            private void ReorderForDelete(ItemColumn item)
            {
                long displayOrder = 1;
                List<ItemColumn> list = (from i in Manager.GetIQ<ItemColumn>()
                                         where i.Deleted == BaseStatusDeleted.None && i.TopItemOwner == item.TopItemOwner
                                          orderby i.DisplayOrder
                                          select i).ToList();
                if (list.Count > 0)
                {
                    List<ItemColumn> reorder = list.Where(l => l.DisplayOrder < item.DisplayOrder).OrderBy(l => l.DisplayOrder).ToList();
                    reorder.Add(list.Where(l => l.Id == item.Id).FirstOrDefault());
                    reorder.AddRange(list.Where(l => l.DisplayOrder >= item.DisplayOrder && l.Id != item.Id).OrderBy(l => l.DisplayOrder).ToList());
                    reorder.ForEach(i => i.DisplayOrder = displayOrder++);
                    Manager.SaveOrUpdateList(list);
                }
                //long displayOrder = item.DisplayOrder;
                //if (displayOrder <= 0)
                //    displayOrder = 1;
                //List<ItemColumn> list = (from i in Manager.GetIQ<ItemColumn>()
                //        where i.Deleted == BaseStatusDeleted.None && i.TopItemOwner == item.TopItemOwner && i.DisplayOrder >= displayOrder && i.Id != item.Id
                //        orderby i.DisplayOrder
                //        select i).ToList();

                //if (list.Count > 0)
                //{
                //    list.ForEach(i => i.DisplayOrder = displayOrder++);
                //    Manager.SaveOrUpdateList(list);
                //}
            }
            private void ReorderForDelete(TopMenuItem item)
            {
                long displayOrder = 1;
                List<TopMenuItem> list = (from i in Manager.GetIQ<TopMenuItem>()
                                          where i.Deleted == BaseStatusDeleted.None && i.Menubar == item.Menubar
                                       orderby i.DisplayOrder
                                       select i).ToList();
                if (list.Count > 0)
                {
                    List<TopMenuItem> reorder = list.Where(l => l.DisplayOrder < item.DisplayOrder).OrderBy(l => l.DisplayOrder).ToList();
                    reorder.Add(list.Where(l => l.Id == item.Id).FirstOrDefault());
                    reorder.AddRange(list.Where(l => l.DisplayOrder >= item.DisplayOrder && l.Id != item.Id).OrderBy(l => l.DisplayOrder).ToList());
                    reorder.ForEach(i => i.DisplayOrder = displayOrder++);
                    Manager.SaveOrUpdateList(list);
                }
                //long displayOrder = item.DisplayOrder;
                //if (displayOrder <= 0)
                //    displayOrder = 1;
                //List<TopMenuItem> list = (from i in Manager.GetIQ<TopMenuItem>()
                //                         where i.Deleted == BaseStatusDeleted.None && i.Menubar == item.Menubar && i.DisplayOrder >= displayOrder && i.Id != item.Id 
                //                         orderby i.DisplayOrder
                //                         select i).ToList();

                //if (list.Count > 0)
                //{
                //    list.ForEach(i => i.DisplayOrder = displayOrder++);
                //    Manager.SaveOrUpdateList(list);
                //}
            }
            private void ReorderForDelete(MenuItem item)
            {
                long displayOrder = 1;
                List<MenuItem> list = (from i in Manager.GetIQ<MenuItem>()
                                       where i.Deleted == BaseStatusDeleted.None && i.ColumnOwner == item.ColumnOwner && i.ItemOwner == item.ItemOwner
                                         orderby i.DisplayOrder
                                       select i).ToList();
                if (list.Count > 0)
                {
                    List<MenuItem> reorder = list.Where(l => l.DisplayOrder < item.DisplayOrder).OrderBy(l => l.DisplayOrder).ToList();
                    reorder.Add(list.Where(l => l.Id == item.Id).FirstOrDefault());
                    reorder.AddRange(list.Where(l => l.DisplayOrder >= item.DisplayOrder && l.Id !=item.Id).OrderBy(l => l.DisplayOrder).ToList());
                    reorder.ForEach(i => i.DisplayOrder =displayOrder++);
                    Manager.SaveOrUpdateList(list);
                }
                //item.DisplayOrder;
                //if (displayOrder <= 0)
                //    displayOrder = 1;
                //List<MenuItem> list = (from i in Manager.GetIQ<MenuItem>()
                //                       where i.Deleted == BaseStatusDeleted.None && i.ColumnOwner == item.ColumnOwner && i.ItemOwner == item.ItemOwner && i.Id != item.Id && i.DisplayOrder >= displayOrder
                //                         orderby i.DisplayOrder
                //                         select i).ToList();

                //if (list.Count > 0)
                //{
                //    list.ForEach(i => i.DisplayOrder = displayOrder++);
                //    Manager.SaveOrUpdateList(list);
                //}
            }

        #endregion

        #region "Move Items"
            public void MoveItemTo(dtoItem source , dtoItem destination)
            {
                List<MenuItemType> list = GetAvailableSubTypes(destination.Type, source.Id);
                if (list.Contains(source.Type))
                    MoveItemTo(source, destination, (long)1);
                else {
                    switch (destination.Type) { 
                        case  MenuItemType.TopItemMenu:
                            TopMenuItem top = Manager.Get<TopMenuItem>(destination.Id);
                            if (top != null) {
                                MoveItemTo(source, new dtoItem() { Id = top.Menubar.Id, Type = MenuItemType.Menubar }, top.DisplayOrder);
                            }
                            break;
                        case MenuItemType.ItemColumn:
                            ItemColumn column = Manager.Get<ItemColumn>(destination.Id);
                            if (column != null)
                            {
                                MoveItemTo(source, new dtoItem() { Id = column.TopItemOwner.Id, Type = MenuItemType.TopItemMenu }, column.DisplayOrder);
                            }
                            break;
                        case MenuItemType.None:
                        case MenuItemType.Menubar:
                            break;
                        default:
                            MenuItem item = Manager.Get<MenuItem>(destination.Id);
                            if (item != null && item.ItemOwner== null && item.ColumnOwner !=null)
                                MoveItemTo(source, new dtoItem() { Id = item.ColumnOwner.Id, Type = MenuItemType.ItemColumn }, item.DisplayOrder);
                            else if (item != null && item.ItemOwner != null)
                                MoveItemTo(source, new dtoItem() { Id = item.ItemOwner.Id, Type = item.ItemOwner.Type }, item.DisplayOrder);
                            break;

                    }
                }
            }

            private void MoveItemTo(dtoItem source, dtoItem destination, long DisplayOrder)
            {
                List<MenuItemType> list = GetAvailableSubTypes(destination.Type, source.Id);
                try
                {
                    if (list.Contains(source.Type))
                    {
                        switch (destination.Type)
                        {
                            case MenuItemType.Menubar:
                                //if (source.Type == MenuItemType.TopItemMenu) {
                                //    Manager.BeginTransaction();
                                //    TopMenuItem topItem = Manager.Get<TopMenuItem>(source.Id);
                                //    MenuItem owner = item.ItemOwner;

                                //    TopMenuItem top = item.TopItemOwner;
                                //    ItemColumn columnOwner = item.ColumnOwner;

                                //    //item.ColumnOwner = column;
                                //    //'item.TopItemOwner = column.TopItemOwner;
                                //    //item.DisplayOrder = 1;
                                //    Manager.RollBack();   
                                //}
                                break;
                            case MenuItemType.TopItemMenu:
                                 try
                                {
                                    Manager.BeginTransaction();
                                    TopMenuItem topItem = Manager.Get<TopMenuItem>(destination.Id);
                                    if (topItem != null && source.Type == MenuItemType.ItemColumn)
                                    {
                                        MoveItemTo(topItem, Manager.Get<ItemColumn>(source.Id), DisplayOrder);
                                    }
                                    Manager.Commit();
                                }
                                catch (Exception ex)
                                {
                                    Manager.RollBack();
                                }
                                break;
                            case MenuItemType.ItemColumn:
                                try
                                {
                                    Manager.BeginTransaction();
                                    ItemColumn column = Manager.Get<ItemColumn>(destination.Id);
                                    if (column != null && source.Type != MenuItemType.TopItemMenu && source.Type != MenuItemType.ItemColumn)
                                    {
                                        MoveItemTo(column,Manager.Get<MenuItem>(source.Id),DisplayOrder);
                                    }
                                    Manager.Commit();
                                }
                                catch (Exception ex)
                                {
                                    Manager.RollBack();
                                }
                                break;
                            case MenuItemType.TextContainer:
                            case MenuItemType.LinkContainer:
                            case MenuItemType.Link:
                            case MenuItemType.Text:
                                try
                                {
                                    Manager.BeginTransaction();
                                    MenuItem menuOwner = Manager.Get<MenuItem>(destination.Id);
                                    if (menuOwner != null && source.Type != MenuItemType.TopItemMenu && source.Type != MenuItemType.ItemColumn)
                                    {
                                        MoveItemTo(menuOwner, Manager.Get<MenuItem>(source.Id), DisplayOrder);
                                    }
                                    Manager.Commit();
                                }
                                catch (Exception ex)
                                {
                                    Manager.RollBack();
                                }
                                break;

                        }
                    }
                }
                catch (Exception ex)
                {
                    if (Manager.IsInTransaction())
                        Manager.RollBack();
                }
            }
            private void MoveItemTo(TopMenuItem topItem, ItemColumn column, long displayOrder)
            {
                if (column != null)
                {
                    ReorderForDelete(column);
                    // Change owner
                    column.TopItemOwner = topItem;
                    column.DisplayOrder = displayOrder;
                    column.Items.ToList().ForEach(m => ChangeItemOwner(CurrentUser(), m, column, topItem));

                    List<ItemColumn> list = (from i in Manager.GetIQ<ItemColumn>()
                                             where i.Deleted == BaseStatusDeleted.None && i.TopItemOwner == topItem && i.Id != column.Id && i.DisplayOrder >= displayOrder
                                           orderby i.DisplayOrder
                                           select i).ToList();

                    if (list.Count > 0)
                    {
                        list.ForEach(i => i.DisplayOrder = ++displayOrder);
                        Manager.SaveOrUpdateList(list);
                    }

                }
            }
            private void MoveItemTo(ItemColumn column,MenuItem item , long displayOrder)
            {
                if (item != null) {
                    MenuItem owner = item.ItemOwner;
                    TopMenuItem top = item.TopItemOwner;
                    ItemColumn columnOwner = item.ColumnOwner;
                    // Reorder Source Items
                    ReorderForDelete(item);
                    // Change owner
                    item.ColumnOwner = column;
                    item.DisplayOrder = displayOrder;
                    item.ItemOwner = null;
                    ChangeItemOwner(CurrentUser(), item, column, column.TopItemOwner);

                    List<MenuItem> list = (from i in Manager.GetIQ<MenuItem>()
                                           where i.Deleted == BaseStatusDeleted.None && i.ColumnOwner == item.ColumnOwner && i.ItemOwner == item.ItemOwner && i.Id != item.Id && i.DisplayOrder >= displayOrder
                                           orderby i.DisplayOrder
                                           select i).ToList();

                    //foreach (MenuItem friend in list){
                    //    friend.DisplayOrder = displayOrder++;
                    //    Manager.SaveOrUpdate(friend);
                    //}
                    if (list.Count > 0)
                    {
                        list.ForEach(i => i.DisplayOrder = ++displayOrder);
                        Manager.SaveOrUpdateList(list);
                    }

                }
            }
            private void MoveItemTo(MenuItem owner, MenuItem item, long displayOrder)
            {
                if (item != null)
                {
                    ReorderForDelete(item);
                    item.DisplayOrder = displayOrder;
                    item.ItemOwner = owner;
                    litePerson person = Manager.GetLitePerson(UC.CurrentUserID);
                    ChangeItemOwner(person, item,owner.ColumnOwner, owner.TopItemOwner);

                    List<MenuItem> list = (from i in Manager.GetIQ<MenuItem>()
                                           where i.Deleted == BaseStatusDeleted.None && i.ColumnOwner == item.ColumnOwner && i.ItemOwner == item.ItemOwner && i.Id != item.Id && i.DisplayOrder >= displayOrder
                                           orderby i.DisplayOrder
                                           select i).ToList();
                    if (list.Count > 0)
                    {
                        list.ForEach(i => i.DisplayOrder = ++displayOrder);
                        Manager.SaveOrUpdateList(list);
                    }

                }
            }
            private void ChangeItemOwner(litePerson person, MenuItem item,ItemColumn columnOwner,TopMenuItem topOwner)
            {
                item.ColumnOwner = columnOwner;
                item.TopItemOwner = topOwner;
                if (item.Translations.Count>0){
                    item.Translations.ToList().ForEach(t =>  t.TopMenuItem = topOwner);
                    item.Translations.ToList().Where(t => t.Deleted == BaseStatusDeleted.None).ToList().ForEach(t => t.UpdateMetaInfo(person, UC.IpAddress, UC.ProxyIpAddress));
                    Manager.SaveOrUpdateList(item.Translations);
                }
                if (item.Childrens.Count > 0)
                    item.Childrens.ToList().ForEach(n => ChangeItemOwner(person,n,columnOwner, topOwner));
                item.UpdateMetaInfo(person, UC.IpAddress, UC.ProxyIpAddress);
                Manager.SaveOrUpdate(item);
            }
            public void ItemReorderedTo(dtoItem source , dtoItem destination)
            {
                try{
                    if (source.Type== MenuItemType.ItemColumn && destination.Type== MenuItemType.ItemColumn){
                        ItemColumn endColumn = Manager.Get<ItemColumn>(destination.Id);
                        ItemColumn startColumn = Manager.Get<ItemColumn>(source.Id);
                        
                        Manager.BeginTransaction();
                        startColumn.DisplayOrder = endColumn.DisplayOrder;
                        Manager.SaveOrUpdate(startColumn);
                        ReorderForDelete(startColumn);
                        Manager.Commit();
                        
                    }
                    else if (source.Type== MenuItemType.TopItemMenu && destination.Type== MenuItemType.TopItemMenu){
                        TopMenuItem endItem = Manager.Get<TopMenuItem>(destination.Id);
                        TopMenuItem startItem = Manager.Get<TopMenuItem>(source.Id);
                        
                        Manager.BeginTransaction();
                        startItem.DisplayOrder = endItem.DisplayOrder;
                        Manager.SaveOrUpdate(startItem);
                        ReorderForDelete(startItem);
                        Manager.Commit();
                    }
                    else if (isStandardItem(source.Type)  && isStandardItem(destination.Type)){
                        MenuItem endItem = Manager.Get<MenuItem>(destination.Id);
                        MenuItem startItem = Manager.Get<MenuItem>(source.Id);

                        Manager.BeginTransaction();
                        startItem.DisplayOrder = endItem.DisplayOrder;
                        Manager.SaveOrUpdate(startItem);
                        ReorderForDelete(startItem);
                        Manager.Commit();
                    }
                }
                catch(Exception ex){
                    if (Manager.IsInTransaction())
                        Manager.RollBack();
                }
            }
            private Boolean isStandardItem(MenuItemType type) {
                return (type == MenuItemType.Link || type == MenuItemType.LinkContainer || type == MenuItemType.Text || type == MenuItemType.TextContainer || type == MenuItemType.Separator);
            }
            public void ItemToFirstDisplay(dtoItem source)
            {
                ReorderItem(source, (long)1);
            }

            private void ReorderItem(dtoItem source, long displayOrder)
            {
                try
                {
                    switch (source.Type)
                    {
                        case MenuItemType.ItemColumn:
                            ItemColumn column = Manager.Get<ItemColumn>(source.Id);

                            Manager.BeginTransaction();
                            column.DisplayOrder = displayOrder;
                            Manager.SaveOrUpdate(column);
                            ReorderForDelete(column);
                            Manager.Commit();
                            break;
                        case MenuItemType.TopItemMenu:
                            TopMenuItem startItem = Manager.Get<TopMenuItem>(source.Id);

                            Manager.BeginTransaction();
                            startItem.DisplayOrder = displayOrder;
                            Manager.SaveOrUpdate(startItem);
                            ReorderForDelete(startItem);
                            Manager.Commit();
                            break;
                        case MenuItemType.Link:
                        case MenuItemType.LinkContainer:
                        case MenuItemType.Separator:
                        case MenuItemType.Text:
                        case MenuItemType.TextContainer:
                            MenuItem item = Manager.Get<MenuItem>(source.Id);

                            Manager.BeginTransaction();
                            item.DisplayOrder = displayOrder;
                            Manager.SaveOrUpdate(item);
                            ReorderForDelete(item);
                            Manager.Commit();
                            break;
                        default:
                            break;
                    }
                }
                catch (Exception ex)
                {
                    if (Manager.IsInTransaction())
                        Manager.RollBack();
                }
            }
        #endregion


        public Boolean SetActiveMenubar(long IdMenubar)
        {
            Boolean result = false;
            try
            {
               
                Menubar notActive = Manager.Get<Menubar>(IdMenubar);
                if (notActive != null) {
                    Manager.BeginTransaction();
                    List<Menubar> actives = (from m in Manager.GetIQ<Menubar>() where m.Id != IdMenubar && m.MenuBarType == notActive.MenuBarType && m.IsCurrent select m).ToList();
                    notActive.IsCurrent = true;
                    notActive.Status = ItemStatus.Active;
                   // notActive.UpdateMetaInfo(CurrentUser(), UC.IpAddress, UC.ProxyIpAddress);
                    foreach (Menubar m in actives) {
                        m.IsCurrent = false;
                        m.Status = ItemStatus.None;
                     //   m.UpdateMetaInfo(CurrentUser(), UC.IpAddress, UC.ProxyIpAddress);
                        Manager.SaveOrUpdate(m);
                    }
                    Manager.SaveOrUpdate(notActive);

                    Manager.Commit();
                    result = true;
                    CacheHelper.PurgeCacheItems(CacheKeys.MenuBar(notActive.MenuBarType));
                    CacheHelper.PurgeCacheItems(CacheKeys.RenderMenu(notActive.MenuBarType));
                    CacheHelper.AddToCache<_Menubar>(CacheKeys.MenuBar(notActive.MenuBarType), Manager.Get<_Menubar>(notActive.Id), CacheExpiration.Month);
                }             
            }
            catch (Exception ex)
            {
                 Manager.RollBack();
            }
            return result;
        }

        #region "Copy Menubar"
            public Menubar CloneMenubar(long IdMenubar, String name){
                Menubar menubar = null;
                try
                {
                    Manager.BeginTransaction();
                    Menubar source = Manager.Get<Menubar>(IdMenubar);
                    if (source != null) {
                        menubar = new Menubar();
                        menubar.CreateMetaInfo(CurrentUser(), UC.IpAddress, UC.ProxyIpAddress);
                        menubar.CssClass = source.CssClass;
                        menubar.Name = name;
                        menubar.Status = ItemStatus.Draft;
                        menubar.MenuBarType = source.MenuBarType;
                        menubar.IsCurrent = false;
                        menubar.Items = new List<TopMenuItem>();
                        menubar.Deleted = BaseStatusDeleted.None;
                        Manager.SaveOrUpdate(menubar);

                        menubar.Items = CloneTopMenuItem(menubar,source.Items);
                    }
                    Manager.Commit();
                }
                catch (Exception ex)
                {
                    Manager.RollBack();
                    menubar = null;
                }
                return menubar;
            }


            private List<TopMenuItem> CloneTopMenuItem(Menubar menubar, IList<TopMenuItem> source)
            {
                List<TopMenuItem> items = new List<TopMenuItem>();
                long displayOrder = 1;
                foreach (TopMenuItem item in source.Where(i=>i.Deleted== BaseStatusDeleted.None).OrderBy(i=>i.DisplayOrder).ToList())
                {
                    TopMenuItem clone = new TopMenuItem();
                    clone.CreateMetaInfo(CurrentUser(), UC.IpAddress, UC.ProxyIpAddress);
                    clone.CssClass = item.CssClass;
                    clone.Deleted = BaseStatusDeleted.None;
                    clone.DisplayOrder = displayOrder++;
                    clone.IdModule = item.IdModule;
                    clone.IsEnabled = item.IsEnabled;
                    clone.Link = item.Link;
                    clone.Menubar = menubar;
                    clone.ModuleCode = item.ModuleCode;
                    clone.Name = item.Name;
                    clone.Permission = item.Permission;
                    clone.ShowDisabledItems = item.ShowDisabledItems;
                    clone.TextPosition = item.TextPosition;

                    clone.Translations = new List<MenuItemTranslation>();
                    clone.ProfileAvailability = new List<ProfileAssignment>();
                    clone.Columns = new List<ItemColumn>();


                    Manager.SaveOrUpdate(clone);
                    clone.ProfileAvailability = CloneItemAssignments(menubar, clone, item.ProfileAvailability);
                    clone.Translations = CloneItemTranslations(menubar, clone, clone, item.Translations);
                    clone.Columns = CloneItemColumns(menubar, clone, item.Columns);
                    items.Add(clone);
                }

                return items;
            }

            private List<MenuItemTranslation> CloneItemTranslations(Menubar menubar, BaseMenuItem item, TopMenuItem topItem, IList<MenuItemTranslation> source)
            {
                List<MenuItemTranslation> items = new List<MenuItemTranslation>();
                foreach (MenuItemTranslation translation in source.Where(a => a.Deleted == BaseStatusDeleted.None).ToList())
                {
                    MenuItemTranslation clone = new MenuItemTranslation();
                    clone.Language = translation.Language;
                    clone.CreateMetaInfo(CurrentUser(), UC.IpAddress, UC.ProxyIpAddress);
                    clone.Item = item;
                    clone.TopMenuItem = topItem;
                    clone.Name = translation.Name;
                    clone.ToolTip = translation.ToolTip;
                    clone.Menubar = menubar;
                    Manager.SaveOrUpdate(clone);
                    items.Add(clone);
                }
                return items;
            }

            private List<ProfileAssignment> CloneItemAssignments(Menubar menubar, BaseMenuItem item, IList<ProfileAssignment> source)
            {
                List<ProfileAssignment> items = new List<ProfileAssignment>();
                foreach (ProfileAssignment assignment in source.Where(a=> a.Deleted== BaseStatusDeleted.None).ToList())
                {
                    ProfileAssignment clone = new ProfileAssignment();
                    clone.CreateMetaInfo(CurrentUser(), UC.IpAddress, UC.ProxyIpAddress);
                    clone.Menubar = menubar;
                    clone.IdProfileType = assignment.IdProfileType;
                    clone.ItemOwner = item;
                    Manager.SaveOrUpdate(clone);
                    items.Add(clone);
                }
                return items;
            }

            private List<ItemColumn> CloneItemColumns(Menubar menubar, TopMenuItem item, IList<ItemColumn> source)
            {
                List<ItemColumn> columns = new List<ItemColumn>();
                long displayOrder = 1;
                foreach (ItemColumn column in source.Where(c => c.Deleted == BaseStatusDeleted.None).OrderBy(c => c.DisplayOrder).ToList())
                {
                    ItemColumn clone = new ItemColumn();
                    clone.CreateMetaInfo(CurrentUser(), UC.IpAddress, UC.ProxyIpAddress);
                    clone.CssClass = column.CssClass;
                    clone.DisplayOrder = displayOrder++;
                    clone.HasSeparator = column.HasSeparator;
                    clone.HeightPx = column.HeightPx;
                    clone.IsEnabled = column.IsEnabled;
                    clone.Items = new List<MenuItem>();
                    clone.Menubar = menubar;
                    clone.TopItemOwner = item;
                    clone.WidthPx = column.WidthPx;

                    Manager.SaveOrUpdate(clone);

                    clone.Items = CloneMenuItems(menubar, clone, item, null, (from mi in column.Items where mi.ItemOwner==null select mi).ToList());
                    columns.Add(clone);
                }
                return columns;
            }
            private List<MenuItem> CloneMenuItems(Menubar menubar,ItemColumn column, TopMenuItem topItem, MenuItem itemOwner, IList<MenuItem> menuItems)
            {
                List<MenuItem> items = new List<MenuItem>();
                long displayOrder = 1;
                foreach (MenuItem menuItem in menuItems.Where(c => c.Deleted == BaseStatusDeleted.None).OrderBy(c => c.DisplayOrder).ToList())
                {
                    MenuItem clone = new MenuItem();
                    clone.CreateMetaInfo(CurrentUser(), UC.IpAddress, UC.ProxyIpAddress);
                    clone.Childrens = new List<MenuItem>();
                    clone.ColumnOwner= column;
                    clone.CssClass = menuItem.CssClass;
                    clone.DisplayOrder = displayOrder++;
                    clone.IdModule = menuItem.IdModule;
                    clone.IsEnabled = menuItem.IsEnabled;
                    clone.ItemOwner = itemOwner;
                    clone.Link = menuItem.Link;
                    clone.Menubar = menubar;
                    clone.ModuleCode = menuItem.ModuleCode;
                    clone.Name = menuItem.Name;
                    clone.Permission = menuItem.Permission;
                    clone.ProfileAvailability = new List<ProfileAssignment>();
                    clone.ShowDisabledItems = menuItem.ShowDisabledItems;
                    clone.TextPosition = menuItem.TextPosition;
                    clone.TopItemOwner = topItem;
                    clone.Translations = new List<MenuItemTranslation>();
                    clone.Type = menuItem.Type;

                    Manager.SaveOrUpdate(clone);
                    clone.ProfileAvailability = CloneItemAssignments(menubar, clone, menuItem.ProfileAvailability);
                    clone.Translations = CloneItemTranslations(menubar, clone, topItem, menuItem.Translations);
                    var childrens = (from c in menuItem.Childrens where c.ItemOwner == menuItem select c);
                    if (childrens.Count() > 0)
                        clone.Childrens = CloneMenuItems(menubar, column, topItem, clone, childrens.ToList());
                }
                return items;
            }
        #endregion
    }
}