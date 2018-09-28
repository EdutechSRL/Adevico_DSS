using System;
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
    public class EditMenubarPresenter: lm.Comol.Core.DomainModel.Common.DomainPresenter
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
            public EditMenubarPresenter(iApplicationContext oContext)
                : base(oContext)
            {
                this.CurrentManager = new BaseModuleManager(oContext);
            }
            public EditMenubarPresenter(iApplicationContext oContext, IViewEditMenubar view)
                : base(oContext, view)
            {
                this.CurrentManager = new BaseModuleManager(oContext);
            }
            protected virtual IViewEditMenubar View
            {
                get { return (IViewEditMenubar)base.View; }
            }
        #endregion

        public void InitView()
        {
            MenuBarType view = View.PreviousView;

            if (UserContext.isAnonymous)
                View.DisplaySessionTimeout(rootObject.EditMenuBar(View.PreloadIdMenubar, view));
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
                        View.AllowEdit = !Service.IsActiveMenubar(IdMenubar);
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
                View.DisplaySessionTimeout(rootObject.EditMenuBar(View.IdMenubar, menubarType));
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

        #region "SaveItem"

        public void SaveItem(dtoMenubar dto) {
            long IdMenubar = View.IdMenubar;
            if (UserContext.isAnonymous)
                View.DisplaySessionTimeout(rootObject.EditMenuBar(IdMenubar, Service.GetMenubarType(IdMenubar)));
            else
            {
                Menubar menubar = Service.SaveMenubar(dto, IdMenubar);
                if (menubar != null)
                    View.ChangeTreeItemName(dto, dto.Name, 0);
                else
                    View.SaveError();
            }
        }
        public void SaveItem(dtoColumn dto)
        {
            if (UserContext.isAnonymous)
                View.DisplaySessionTimeout(rootObject.EditMenuBar(View.IdMenubar, Service.GetMenubarType(View.IdMenubar)));
            else
            {
                ItemColumn column = Service.SaveItem(dto);
                if (column != null)
                    View.LoadMenubar(Service.MenubarToTree(View.IdMenubar), new dtoItem() { Id = column.Id, Type = MenuItemType.ItemColumn });
                else
                    View.SaveError();
            }
        }
        public void SaveItem(dtoTopMenuItem dto, List<dtoTranslation> translations)
        {
            if (UserContext.isAnonymous)
                View.DisplaySessionTimeout(rootObject.EditMenuBar(View.IdMenubar, Service.GetMenubarType(View.IdMenubar)));
            else
            {
                TopMenuItem item = Service.SaveItem(dto, translations);
                if (item != null)
                    //View.ChangeTreeItemName(dto, dto.Name, item.DisplayOrder);
                    View.LoadMenubar(Service.MenubarToTree(View.IdMenubar), new dtoItem() { Id = item.Id, Type = MenuItemType.TopItemMenu });
                else
                {
                    //View.LoadTopMenuItem(Service.GetDtoTopMenuItem(dto.Id), Service.GetDtoTranslations(dto.Id));
                    View.SaveError();
                }
            }
        }
        public void SaveItem(dtoTopMenuItem dto, List<dtoTranslation> translations, List<int> selectedTypes)
        {
            if (UserContext.isAnonymous)
                View.DisplaySessionTimeout(rootObject.EditMenuBar(View.IdMenubar, Service.GetMenubarType(View.IdMenubar)));
            else
            {
                TopMenuItem item = Service.SaveItem(dto, translations, selectedTypes);
                if (item != null)
                    View.LoadMenubar(Service.MenubarToTree(View.IdMenubar), new dtoItem() { Id = item.Id, Type = MenuItemType.TopItemMenu });
                //View.ChangeTreeItemName(dto, dto.Name, item.DisplayOrder);
                else
                {
                    //View.LoadTopMenuItem(Service.GetDtoTopMenuItem(dto.Id), Service.GetDtoTranslations(dto.Id), Service.GetItemProfilesAssignments(dto.Id));
                    View.SaveError();
                }
            }
        }
        public void SaveItem(dtoMenuItem dto, List<dtoTranslation> translations)
        {
            if (UserContext.isAnonymous)
                View.DisplaySessionTimeout(rootObject.EditMenuBar(View.IdMenubar, Service.GetMenubarType(View.IdMenubar)));
            else
            {
                MenuItem item = Service.SaveItem(dto, translations);
                if (item != null)
                    //View.ChangeTreeItemName(dto, dto.Name, item.DisplayOrder);
                    View.LoadMenubar(Service.MenubarToTree(View.IdMenubar), new dtoItem() { Id = item.Id, Type = dto.Type });
                else
                {
                    //View.LoadTopMenuItem(Service.GetDtoTopMenuItem(dto.Id), Service.GetDtoTranslations(dto.Id));
                    View.SaveError();
                }
            }
        }
        public void SaveItem(dtoMenuItem dto, List<dtoTranslation> translations, List<int> selectedTypes)
        {
            if (UserContext.isAnonymous)
                View.DisplaySessionTimeout(rootObject.EditMenuBar(View.IdMenubar, Service.GetMenubarType(View.IdMenubar)));
            else
            {
                MenuItem item = Service.SaveItem(dto, translations, selectedTypes);
                if (item != null)
                    View.LoadMenubar(Service.MenubarToTree(View.IdMenubar), new dtoItem() { Id = item.Id, Type = dto.Type });
                //View.ChangeTreeItemName(dto, dto.Name, item.DisplayOrder);
                else
                {
                    //View.LoadTopMenuItem(Service.GetDtoTopMenuItem(dto.Id), Service.GetDtoTranslations(dto.Id), Service.GetItemProfilesAssignments(dto.Id));
                    View.SaveError();
                }
            }
        }
        public void AddItem(long idOwner, MenuItemType itemOwner, MenuItemType itemToCreate){
            if (UserContext.isAnonymous)
                View.DisplaySessionTimeout(rootObject.EditMenuBar(View.IdMenubar, Service.GetMenubarType(View.IdMenubar)));
            else
            {
                MenuBarType menubarType = Service.GetMenubarType(View.IdMenubar);
                Boolean loadProfileTypes = (menubarType == MenuBarType.Portal || menubarType == MenuBarType.PortalAdministration);
                switch (itemOwner)
                {
                    case MenuItemType.Menubar:
                        TopMenuItem item = Service.AddItemToMenubar(idOwner);
                        if (item != null)
                        {
                            View.LoadMenubar(Service.MenubarToTree(View.IdMenubar), new dtoItem() { Id = idOwner, Type = itemOwner });
                            View.LoadMenuBarInfo(Service.GetDtoMenubar(View.IdMenubar));
                            //if (loadProfileTypes)
                            //    View.LoadTopMenuItem(Service.GetDtoTopMenuItem(item.Id) , Service.GetDtoTranslations(item.Id), Service.GetItemProfilesAssignments(item.Id));
                            //else
                            //    View.LoadTopMenuItem(Service.GetDtoTopMenuItem(item.Id), Service.GetDtoTranslations(item.Id));
                        }
                        break;
                    case MenuItemType.TopItemMenu:
                        ItemColumn column = Service.AddItemToTopItem(idOwner);
                        if (column != null)
                        {
                            View.LoadMenubar(Service.MenubarToTree(View.IdMenubar), new dtoItem() { Id = idOwner, Type = itemOwner });
                            SelectItem(new dtoItem() { Id = idOwner, Type = itemOwner });
                            //View.LoadColumnItem(Service.GetDtoColumn(column.Id));
                        }
                        break;
                    case MenuItemType.ItemColumn:
                        MenuItem subColumnItem = Service.AddToColumn(idOwner, itemToCreate);
                        if (subColumnItem != null)
                        {
                            //dtoMenuItem dto = Service.GetDtoMenuItem(subColumnItem.Id);
                            View.LoadMenubar(Service.MenubarToTree(View.IdMenubar), new dtoItem() { Id = idOwner, Type = itemOwner });
                            //if (itemToCreate == MenuItemType.Separator)
                            //    View.LoadSeparatorItem(dto);
                            //else if (loadProfileTypes)
                            //    View.LoadMenuItem(dto, Service.GetDtoTranslations(subColumnItem.Id), Service.GetItemProfilesAssignments(subColumnItem.Id), Service.GetItemAvailableProfileTypes(subColumnItem), Service.GetMenuItemAvailableTypes(subColumnItem));
                            //else
                            //    View.LoadMenuItem(dto, Service.GetDtoTranslations(subColumnItem.Id), Service.GetMenuItemAvailableTypes(subColumnItem));
                            SelectItem(new dtoItem() { Id = idOwner, Type = itemOwner });
                        }
                        break;

                    case MenuItemType.Link:
                    case MenuItemType.Text:
                        if (!(itemToCreate == MenuItemType.IconManage || itemToCreate == MenuItemType.IconNewItem || itemToCreate == MenuItemType.IconStatistic))
                        {
                            View.LoadMenubar(Service.MenubarToTree(View.IdMenubar), new dtoItem() { Id = idOwner, Type = itemOwner });
                            break;
                        }
                        goto case MenuItemType.TextContainer;
                    case MenuItemType.TextContainer:
                    case MenuItemType.LinkContainer:
                        MenuItem subItem = Service.AddToItem(idOwner, itemToCreate);
                        if (subItem != null)
                        {
                            View.LoadMenubar(Service.MenubarToTree(View.IdMenubar), new dtoItem() { Id = idOwner, Type = itemOwner });
                            SelectItem(new dtoItem() { Id = idOwner, Type = itemOwner });
                            //if (itemToCreate == MenuItemType.Separator)
                            //    View.LoadSeparatorItem(Service.GetDtoMenuItem(subItem.Id));
                            //else if (loadProfileTypes)
                            //    View.LoadMenuItem(Service.GetDtoMenuItem(subItem.Id), Service.GetDtoTranslations(subItem.Id), Service.GetItemProfilesAssignments(subItem.Id), Service.GetItemAvailableProfileTypes(subItem), Service.GetMenuItemAvailableTypes(subItem));
                            //else
                            //    View.LoadMenuItem(Service.GetDtoMenuItem(subItem.Id), Service.GetDtoTranslations(subItem.Id), Service.GetMenuItemAvailableTypes(subItem));
                        }

                        break;
                    default:
                        View.LoadMenubar(Service.MenubarToTree(View.IdMenubar), new dtoItem() { Id = idOwner, Type = itemOwner });
                        break;
                }
            }
        }

        #endregion

        public void DeleteItem(long IdItem, MenuItemType itemType)
        {
            if (UserContext.isAnonymous)
                View.DisplaySessionTimeout(rootObject.EditMenuBar(View.IdMenubar, View.PreviousView));
            else
            {
                dtoItem item = Service.GetFather(IdItem, itemType);
                Boolean deleted = false;
                if (item != null)
                {
                    // deleted = Service.VirtualDeleteItem(IdItem, itemType);
                    deleted = Service.PhisicalDeleteItem(IdItem, itemType);
                }
                if (!deleted)
                    View.DeleteError(itemType);
                else
                {
                    View.LoadMenubar(Service.MenubarToTree(View.IdMenubar), item);
                    this.SelectItem(item);
                }
            }
        }

        public void VirtualDeleteItem(long IdItem, MenuItemType itemType)
        {
            if (UserContext.isAnonymous)
                View.DisplaySessionTimeout(rootObject.EditMenuBar(View.IdMenubar, View.PreviousView));
            else
            {
                dtoItem item = Service.GetFather(IdItem, itemType);
                Boolean deleted = false;
                if (item != null)
                    deleted = Service.VirtualDeleteItem(IdItem, itemType);
                if (!deleted)
                    View.DeleteError(itemType);
                else
                {
                    View.LoadMenubar(Service.MenubarToTree(View.IdMenubar), item);
                    this.SelectItem(item);
                }
            }
        }
        public void MoveItemTo(dtoItem source , dtoItem destination)
        {
            if (UserContext.isAnonymous)
                View.DisplaySessionTimeout(rootObject.EditMenuBar(View.IdMenubar, View.PreviousView));
            else
            {
                Service.MoveItemTo(source, destination);
                View.LoadMenubar(Service.MenubarToTree(View.IdMenubar), source);
            }
        }
        public void ItemReorderedTo(dtoItem source , dtoItem destination)
        {
            if (UserContext.isAnonymous)
                View.DisplaySessionTimeout(rootObject.EditMenuBar(View.IdMenubar, View.PreviousView));
            else
            {
                Service.ItemReorderedTo(source, destination);
                View.LoadMenubar(Service.MenubarToTree(View.IdMenubar), source);
            }
        }
        public void ItemToFirstDisplay(dtoItem source)
        {
            if (UserContext.isAnonymous)
                View.DisplaySessionTimeout(rootObject.EditMenuBar(View.IdMenubar, View.PreviousView));
            else {
                Service.ItemToFirstDisplay(source);
                View.LoadMenubar(Service.MenubarToTree(View.IdMenubar), source);
            }
        }
    }
}