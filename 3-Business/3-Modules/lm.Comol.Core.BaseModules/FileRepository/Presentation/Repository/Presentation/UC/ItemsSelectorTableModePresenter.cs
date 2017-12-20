using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel;
using lm.Comol.Core.Business;
using lm.Comol.Core.FileRepository.Business;
using lm.Comol.Core.FileRepository.Domain;
using lm.Comol.Core.BaseModules.FileRepository.Business;

namespace lm.Comol.Core.BaseModules.FileRepository.Presentation 
{
    public class ItemsSelectorTableModePresenter : lm.Comol.Core.DomainModel.Common.DomainPresenter
    {
        #region "Initialize"
            private ServiceRepository service;
            public virtual BaseModuleManager CurrentManager { get; set; }
            private Int32 currentIdModule;
            protected virtual IViewItemsSelectorTableMode View
            {
                get { return (IViewItemsSelectorTableMode)base.View; }
            }
            private ServiceRepository Service
            {
                get
                {
                    if (service == null)
                        service = new ServiceRepository(AppContext);
                    return service;
                }
            }
            private Int32 CurrentIdModule
            {
                get
                {
                    if (currentIdModule == 0)
                        currentIdModule = CurrentManager.GetModuleID(ModuleRepository.UniqueCode);
                    return currentIdModule;
                }
            }
            public ItemsSelectorTableModePresenter(iApplicationContext oContext)
                : base(oContext)
            {
                this.CurrentManager = new BaseModuleManager(oContext);
            }
            public ItemsSelectorTableModePresenter(iApplicationContext oContext, IViewItemsSelectorTableMode view)
                : base(oContext, view)
            {
                this.CurrentManager = new BaseModuleManager(oContext);
            }
        #endregion

        public void InitView(Boolean allowPaging, Boolean selectAll, Int32 pageSize, RepositoryIdentifier identifier, Boolean adminMode, Boolean showHiddenItems, Boolean disableNotAvailableItems, List<ItemType> typesToLoad, ItemAvailability availability, List<StatisticType> displayStatistics, List<long> idRemovedItems, List<long> idSelectedItems = null, OrderBy orderBy = OrderBy.name, Boolean ascending = true)
        {
            if (SessionTimeout())
                return;
            View.IdUserLoader = UserContext.CurrentUserID;
            View.AvailableTypes = typesToLoad;
            View.CurrentAvailability = availability;
            View.IdRemovedItems = idRemovedItems;
            View.LoadForModule = false;
            View.ModuleCode = ModuleRepository.UniqueCode;
            View.RepositoryIdentifier = identifier;
            View.CurrentOrderBy = orderBy;
            View.CurrentAscending = ascending;
            View.isInitialized = true;
            View.CurrentAdminMode = adminMode;
            View.CurrentShowHiddenItems = showHiddenItems;
            View.CurrentDisableNotAvailableItems = disableNotAvailableItems;
            if (!typesToLoad.Any(t => t == ItemType.Multimedia || t == ItemType.ScormPackage || t == ItemType.VideoStreaming))
            {
                displayStatistics.Remove(StatisticType.plays);
                displayStatistics.Remove(StatisticType.myplays);
            }
            if (!typesToLoad.Any(t => t != ItemType.Folder && t!= ItemType.Link))
            {
                displayStatistics.Remove(StatisticType.downloads);
                displayStatistics.Remove(StatisticType.mydownloads);
            }
            View.DisplayStatistics = displayStatistics;
            LoadItems(Service.GetPermissions(identifier, UserContext.CurrentUserID), UserContext.CurrentUserID, allowPaging, identifier, adminMode, showHiddenItems, disableNotAvailableItems, typesToLoad, availability,displayStatistics, idRemovedItems, idSelectedItems, selectAll, orderBy, ascending, 0, pageSize);
        }

        public void LoadItems(RepositoryIdentifier identifier, Boolean adminMode, Boolean showHiddenItems, Boolean disableNotAvailableItems, List<ItemType> typesToLoad, ItemAvailability availability, List<StatisticType> displayStatistics, List<long> idRemovedItems, List<long> idSelectedItems, Boolean selectAll, OrderBy orderBy, Boolean ascending, Int32 pageIndex, Int32 pageSize)
        {
            if (!SessionTimeout())
                LoadItems(Service.GetPermissions(identifier, UserContext.CurrentUserID), UserContext.CurrentUserID, true, identifier, adminMode, showHiddenItems, disableNotAvailableItems, typesToLoad, availability, displayStatistics, idRemovedItems, idSelectedItems, selectAll, orderBy, ascending, pageIndex, pageSize);
        }
        private void LoadItems(ModuleRepository module, Int32 idCurrentUser, Boolean allowPaging, RepositoryIdentifier identifier, Boolean adminMode, Boolean showHiddenItems, Boolean disableNotAvailableItems, List<ItemType> typesToLoad, ItemAvailability availability, List<StatisticType> displayStatistics, List<long> idRemovedItems, List<long> idSelectedItems, Boolean selectAll, OrderBy orderBy, Boolean ascending, Int32 pageIndex, Int32 pageSize)
        {
            List<long> selectedItems = idSelectedItems;
            if (!selectAll)
            {
                Dictionary<Boolean, List<long>> selections = View.GetCurrentSelection();
                selectedItems = selectedItems.Except(selections[false]).ToList();
                selectedItems.AddRange(selections[true]);
                View.IdSelectedItems = selectedItems;
            }

            List<dtoRepositoryItemToSelect> items = Service.ItemsToSelectGet(idCurrentUser, "/", module, identifier, adminMode, showHiddenItems, disableNotAvailableItems, typesToLoad, availability, displayStatistics, idRemovedItems, idSelectedItems, selectAll, !typesToLoad.Contains(ItemType.Folder));

            PagerBase pager = new PagerBase();
            if (allowPaging)
            {
                Int32 itemsCount = (items == null ? 0 : items.Count());
                pager.PageSize = pageSize;
                pager.Count = (itemsCount > 0) ? itemsCount - 1 : 0;
                pager.PageIndex = pageIndex;
                View.Pager = pager;
                pageIndex = pager.PageIndex;
            }

            if (items == null)
                View.LoadItems(null);
            else
            {
                items = Service.ItemsToSelectReorder(items, orderBy, ascending);
                if (items != null){
                    if (allowPaging)
                        View.LoadItems(items.Skip(pageIndex * pageSize).Take(pageSize).ToList());
                    else
                        View.LoadItems(items);
                }
                else
                    View.LoadItems(new List<dtoRepositoryItemToSelect>());
            }
        }


        public void EditItemsSelection(Boolean selectAll)
        {
            View.AllSelected = selectAll;
            View.IdSelectedItems = (selectAll) ? UpdateItemsSelection() : new List<long>();
        }
        private List<long> UpdateItemsSelection()
        {
            List<long> idItems = View.IdSelectedItems;
            Dictionary<Boolean, List<long>> selected = View.GetCurrentSelection();

            idItems.AddRange(selected[true]);
            idItems = idItems.Where(i => selected[false].Contains(i)).ToList();

            return idItems.Distinct().ToList();
        }
        public List<dtoRepositoryItemToSelect> GetSelectedItems(RepositoryIdentifier identifier, Boolean adminMode, List<StatisticType> displayStatistics, List<ItemType> typesToLoad, List<long> idSelectedItems, Boolean selectAll)
        {
            if (!SessionTimeout())
            {
                Dictionary<Boolean, List<long>> selections = View.GetCurrentSelection();
                idSelectedItems = idSelectedItems.Except(selections[false]).ToList();
                idSelectedItems.AddRange(selections[true]);
                if (idSelectedItems.Any())
                    idSelectedItems = idSelectedItems.Distinct().ToList();
                View.IdSelectedItems = idSelectedItems;
                if (selectAll)
                    return Service.ItemsToSelectGet(View.IdUserLoader, "/", Service.GetPermissions(identifier, UserContext.CurrentUserID), identifier, adminMode, View.CurrentShowHiddenItems, View.CurrentDisableNotAvailableItems, typesToLoad, View.CurrentAvailability, displayStatistics, View.IdRemovedItems, idSelectedItems, selectAll, !typesToLoad.Contains(ItemType.Folder)).Where(i=> i.Selectable && i.Selected).ToList();
                else
                    return Service.ItemsToSelectGet(UserContext.CurrentUserID, "/", Service.GetPermissions(identifier, UserContext.CurrentUserID),identifier, adminMode, displayStatistics, idSelectedItems);
            }
            else
                return new List<dtoRepositoryItemToSelect>();
        }
        public Boolean SessionTimeout()
        {
            if (UserContext.isAnonymous)
            {
                View.DisplaySessionTimeout();
                return true;
            }
            else
                return false;
        }
    }
}