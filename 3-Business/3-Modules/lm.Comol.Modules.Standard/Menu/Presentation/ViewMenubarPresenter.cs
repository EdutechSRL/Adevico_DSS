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
    public class ViewMenubarPresenter: lm.Comol.Core.DomainModel.Common.DomainPresenter
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
            public ViewMenubarPresenter(iApplicationContext oContext)
                : base(oContext)
            {
                this.CurrentManager = new BaseModuleManager(oContext);
            }
            public ViewMenubarPresenter(iApplicationContext oContext, IViewViewMenubar view)
                : base(oContext, view)
            {
                this.CurrentManager = new BaseModuleManager(oContext);
            }
            protected virtual IViewViewMenubar View
            {
                get { return (IViewViewMenubar)base.View; }
            }
        #endregion

        public void InitView()
        {
            MenuBarType view = View.PreviousView;

            if (UserContext.isAnonymous)
                View.DisplaySessionTimeout(rootObject.ViewMenuBar(View.PreloadIdMenubar, view));
            else
            {
                ModuleMenu module = ModuleMenu.CreatePortalmodule(UserContext.UserTypeID);
                Boolean allowManage = module.ManagePortalMenubar || module.ManageAdministrationMenubar || module.ManageCommunitiesMenubar;
                View.AllowManage = allowManage;
                if (!allowManage)
                    View.NoPermission();
                else
                {
                    long IdMenubar = View.PreloadIdMenubar;
                    if (!Service.MenubarExist(IdMenubar))
                        View.MenubarUnknown();
                    else
                    {
                        View.IdMenubar = IdMenubar;
                        dtoTree tree = Service.MenubarToTree(IdMenubar);
                        View.LoadMenubar(tree, new dtoItem() { Id = tree.Id, Type = MenuItemType.Menubar });
                        View.LoadMenuBarInfo(Service.GetDtoMenubar(tree.Id));
                    }
                }
            }
        }

        public void SelectItem(dtoItem item)
        {
            MenuBarType menubarType = (item.Type == MenuItemType.Menubar) ? Service.GetMenubarType(item.Id) : Service.GetItemMenubarType(item.Id);
            if (UserContext.isAnonymous)
                View.DisplaySessionTimeout(rootObject.ViewMenuBar(View.IdMenubar, menubarType));
            else
            {
                Boolean loadProfileTypes = (menubarType == MenuBarType.Portal || menubarType == MenuBarType.PortalAdministration);
                switch (item.Type)
                {
                    case MenuItemType.Menubar:
                        View.LoadMenuBarInfo(Service.GetDtoMenubar(item.Id));
                        break;
                    case MenuItemType.ItemColumn:
                        View.LoadColumnItem(Service.GetDtoColumn(item.Id));
                        break;
                    case MenuItemType.TopItemMenu:
                        dtoTopMenuItem topItem = Service.GetDtoTopMenuItem(item.Id);
                        if (topItem == null)
                            View.ItemUnknown();
                        else if (loadProfileTypes)
                            View.LoadTopMenuItem(topItem, Service.GetDtoTranslations(item.Id), Service.GetItemProfilesAssignments(item.Id));
                        else
                            View.LoadTopMenuItem(topItem, Service.GetDtoTranslations(item.Id));
                        break;
                    case MenuItemType.Separator:
                        dtoMenuItem separator = Service.GetDtoMenuItem(item.Id);
                        if (item == null)
                            View.ItemUnknown();
                        else
                            View.LoadSeparatorItem(separator);
                        break;
                    case MenuItemType.None:
                        View.ItemUnknown();
                        break;
                    default:
                        dtoMenuItem menuItem = Service.GetDtoMenuItem(item.Id);
                        if (menuItem == null)
                            View.ItemUnknown();
                        else if (loadProfileTypes)
                            View.LoadMenuItem(menuItem, Service.GetDtoTranslations(menuItem.Id), Service.GetItemProfilesAssignments(menuItem.Id), Service.GetItemAvailableProfileTypes((menuItem)), Service.GetMenuItemAvailableTypes(menuItem), Service.GetAvailableSubTypes(menuItem));
                        else
                            View.LoadMenuItem(menuItem, Service.GetDtoTranslations(menuItem.Id), Service.GetMenuItemAvailableTypes(menuItem), Service.GetAvailableSubTypes(menuItem));
                        break;
                }
            }
        }
    }
}