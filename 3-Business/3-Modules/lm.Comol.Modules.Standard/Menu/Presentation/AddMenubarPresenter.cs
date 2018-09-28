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
    public class AddMenubarPresenter: lm.Comol.Core.DomainModel.Common.DomainPresenter
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
            public AddMenubarPresenter(iApplicationContext oContext)
                : base(oContext)
            {
                this.CurrentManager = new BaseModuleManager(oContext);
            }
            public AddMenubarPresenter(iApplicationContext oContext, IViewAddMenubar view)
                : base(oContext, view)
            {
                this.CurrentManager = new BaseModuleManager(oContext);
            }
            protected virtual IViewAddMenubar View
            {
                get { return (IViewAddMenubar)base.View; }
            }
        #endregion

        public void InitView()
        {
            MenuBarType view = View.PreloadType;

            if (view == MenuBarType.None)
                view = MenuBarType.GenericCommunity;
            
            if (UserContext.isAnonymous)
                View.DisplaySessionTimeout(rootObject.CreateMenuBar(view));
            else
            {
                Boolean allowManage = false;
                ModuleMenu module = ModuleMenu.CreatePortalmodule(UserContext.UserTypeID);
                List<MenuBarType> views = GetAvailableViews(module);
                if (views.Count==0)
                     View.NoPermission();
                else{
                    if (!views.Contains(view))
                        view= views[0];
                    View.LoadAvailableTypes(views);

                    allowManage = module.ManagePortalMenubar || module.ManageAdministrationMenubar || module.ManageCommunitiesMenubar;
                    View.AllowCreate = allowManage;
                    if (allowManage){
                        View.TopItemsnumber = 5;
                        View.SubItemsnumber = 5;
                        dtoMenubar dto = new dtoMenubar();
                        dto.MenuBarType=view;
                        dto.Name= View.DefaultMenuName;
                        View.MenuBar = dto;
                    }
                    else
                        View.NoPermission();
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

        public void AddMenuBar() {
            if (UserContext.isAnonymous)
            {
                View.CreationError();
                View.DisplaySessionTimeout(rootObject.CreateMenuBar(View.PreloadType));
            }
            else
            {
                dtoMenubar dto = View.MenuBar;
                dto.Deleted = BaseStatusDeleted.None;
                dto.Status = ItemStatus.Draft;
                Menubar menubar = Service.CreateMenubar(dto, View.TopItemsnumber, View.SubItemsnumber);
                if (menubar == null)
                    View.CreationError();
                else
                    View.EditMenubar(menubar.Id, menubar.MenuBarType);
            }
        }
    }
}