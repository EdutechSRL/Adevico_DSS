using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel;
using lm.Comol.Modules.Standard.Menu.Domain;
using lm.Comol.Modules.Standard.Menu.Business;
using lm.Comol.Core.Business;

namespace lm.Comol.Modules.Standard.Menu.Presentation
{
    public class MenubarListPresenter: lm.Comol.Core.DomainModel.Common.DomainPresenter
    {
        #region "Initalize"
            private ServiceMenubar _Service;
            private ServiceMenubar Service
            {
                get
                {
                    if (_Service == null)
                        _Service = new ServiceMenubar(AppContext);
                    return _Service;
                }
            }
            public MenubarListPresenter(iApplicationContext oContext)
                : base(oContext)
            {
                this.CurrentManager = new BaseModuleManager(oContext);
            }
            public MenubarListPresenter(iApplicationContext oContext, IViewMenubarList view)
                : base(oContext, view)
            {
                this.CurrentManager = new BaseModuleManager(oContext);
            }
            protected virtual IViewMenubarList View
            {
                get { return (IViewMenubarList)base.View; }
            }
        #endregion

        public void InitView()
        {
            MenuBarType view = View.PreloadView;

            if (view == MenuBarType.None)
                view = MenuBarType.GenericCommunity;

            if (UserContext.isAnonymous)
                View.DisplaySessionTimeout(rootObject.MenuBarList(view));
            else
            {
                Boolean allowManage = false;
                ModuleMenu module = ModuleMenu.CreatePortalmodule(UserContext.UserTypeID);
                List<MenuBarType> views = GetAvailableViews(module);
                if (views.Count == 0)
                    View.NoPermission();
                else
                {
                    if (!views.Contains(view))
                        view = views[0];
                    View.LoadAvailableViews(views);
                    View.CurrentView = view;

                    switch (view)
                    {
                        case MenuBarType.Portal:
                            allowManage = module.ManagePortalMenubar;
                            break;
                        case MenuBarType.GenericCommunity:
                            allowManage = module.ManageCommunitiesMenubar;
                            break;
                        case MenuBarType.PortalAdministration:
                            allowManage = module.ManageAdministrationMenubar;
                            break;
                    }
                    View.AllowCreate = allowManage;
                    LoadMenubarItems(module, view, 0, View.CurrentPageSize);
                }
            }
        }

        private List<MenuBarType> GetAvailableViews(ModuleMenu module)
        { 
            List<MenuBarType> views = new List<MenuBarType>();
            if (module.ManagePortalMenubar)
                views.Add(MenuBarType.Portal);
            if (module.ManageAdministrationMenubar)
                views.Add(MenuBarType.PortalAdministration);
             if (module.ManageCommunitiesMenubar)
                views.Add(MenuBarType.GenericCommunity);
             return views;
        }


        public void LoadMenubarItems(int currentPageIndex, int currentPageSize)
        {
            ModuleMenu module = ModuleMenu.CreatePortalmodule(UserContext.UserTypeID);
            LoadMenubarItems(module,View.CurrentView, currentPageIndex, currentPageSize);
        }
        private void LoadMenubarItems(ModuleMenu module,MenuBarType view , int currentPageIndex, int currentPageSize)
        {
            int itemsCount = (int)Service.GetMenubarListCount(view);
            PagerBase pager = new PagerBase();

            pager.PageSize = currentPageSize;//Me.View.CurrentPageSize
            pager.Count = itemsCount - 1;
            pager.PageIndex = currentPageIndex;// Me.View.CurrentPageIndex
            View.Pager = pager;
            
            if (itemsCount==0)
                View.LoadItems(new List<dtoMenubarItemPermission>());
            else{
                List<dtoMenubar> items = Service.GetMenubarList(view, pager.PageIndex, currentPageSize);
                View.LoadItems(items.Select(m => new dtoMenubarItemPermission(m, module)).ToList());
            }
        }


        public void EditActiveMenubar(long idMenubar, String menuBarName) {
            if (UserContext.isAnonymous)
                View.DisplaySessionTimeout(rootObject.MenuBarList(View.CurrentView));
            else
            {
                dtoMenubar dto = Service.GetDtoMenubar(idMenubar);
                if (dto != null)
                {
                    ModuleMenu module = ModuleMenu.CreatePortalmodule(UserContext.UserTypeID);
                    Boolean allowManage = (dto.MenuBarType == MenuBarType.PortalAdministration) ? module.ManagePortalMenubar : (dto.MenuBarType == MenuBarType.GenericCommunity) ? module.ManageCommunitiesMenubar : (dto.MenuBarType == MenuBarType.Portal) ? module.ManageAdministrationMenubar : false;
                    if (allowManage)
                    {
                        Menubar clone = Service.CloneMenubar(idMenubar, (menuBarName.Contains("{0}")) ? String.Format(menuBarName, dto.Name) : "[COPY] " + dto.Name);
                        if (clone != null)
                            View.EditMenubar(clone.Id, clone.MenuBarType);
                    }
                }
            }
        }

        public void SetActiveMenubar(long idMenubar)
        {
            if (UserContext.isAnonymous)
                View.DisplaySessionTimeout(rootObject.MenuBarList(View.CurrentView));
            else
            {
                ModuleMenu module = ModuleMenu.CreatePortalmodule(UserContext.UserTypeID);
                MenuBarType view = View.CurrentView;
                Boolean allowManage = (view == MenuBarType.PortalAdministration) ? module.ManagePortalMenubar : (view == MenuBarType.GenericCommunity) ? module.ManageCommunitiesMenubar : (view == MenuBarType.Portal) ? module.ManageAdministrationMenubar : false;
                if (allowManage && Service.SetActiveMenubar(idMenubar))
                    View.ReloadPage(idMenubar, view, View.Pager.PageIndex, View.CurrentPageSize);
                else
                    LoadMenubarItems(View.Pager.PageIndex, View.CurrentPageSize);
            }
        }

        public Boolean VirtualDeleteMenubar(long idMenubar){
            Boolean result = false;
            if (UserContext.isAnonymous)
                View.DisplaySessionTimeout(rootObject.MenuBarList(View.CurrentView));
            else
                result = DeleteMenubar(idMenubar, true, true);
            return result;
        }
        public Boolean VirtualUnDeleteMenubar(long idMenubar){
            Boolean result = false;
            if (UserContext.isAnonymous)
                View.DisplaySessionTimeout(rootObject.MenuBarList(View.CurrentView));
            else
                result = DeleteMenubar(idMenubar, false, true);
            return result;
        }
        public Boolean PhisicalDeleteMenubar(long idMenubar){
            Boolean result = false;
            if (UserContext.isAnonymous)
                View.DisplaySessionTimeout(rootObject.MenuBarList(View.CurrentView));
            else
                result = DeleteMenubar(idMenubar, true, false);
            return result;
        }
        private Boolean DeleteMenubar(long idMenubar, Boolean delete, Boolean isVirtual)
        {
            Boolean result = false;
            if (UserContext.isAnonymous)
                View.DisplaySessionTimeout(rootObject.MenuBarList(View.CurrentView));
            else
            {
             
                if (isVirtual && delete)
                    result = Service.VirtualDeleteItem(idMenubar, MenuItemType.Menubar);
                else if (isVirtual && !delete)
                    result = Service.VirtualUnDeleteItem(idMenubar, MenuItemType.Menubar);
                else if (delete && !isVirtual)
                    result = Service.PhisicalDeleteItem(idMenubar, MenuItemType.Menubar);
                LoadMenubarItems(View.Pager.PageIndex, View.CurrentPageSize);
            }
            return result;
        }
    }
}