using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel;
using lm.Comol.Core.Business;
using lm.Comol.Core.FileRepository.Business;
using lm.Comol.Core.FileRepository.Domain;
using lm.Comol.Core.BaseModules.FileRepository.Business;
using lm.Comol.Core.BaseModules.FileRepository.Presentation.Domain;

namespace lm.Comol.Core.BaseModules.FileRepository.Presentation 
{
    public class ItemsSelectorTreeModePresenter : lm.Comol.Core.DomainModel.Common.DomainPresenter
    {
        #region "Initialize"
            private ServiceRepository service;
            public virtual BaseModuleManager CurrentManager { get; set; }
            private Int32 currentIdModule;
            protected virtual IViewItemsSelectorTreeMode View
            {
                get { return (IViewItemsSelectorTreeMode)base.View; }
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
            public ItemsSelectorTreeModePresenter(iApplicationContext oContext)
                : base(oContext)
            {
                this.CurrentManager = new BaseModuleManager(oContext);
            }
            public ItemsSelectorTreeModePresenter(iApplicationContext oContext, IViewItemsSelectorTreeMode view)
                : base(oContext, view)
            {
                this.CurrentManager = new BaseModuleManager(oContext);
            }
        #endregion

        public void InitView(RepositoryIdentifier identifier, Boolean adminMode, Boolean showHiddenItems, Boolean disableNotAvailableItems, List<ItemType> typesToLoad, ItemAvailability availability, List<StatisticType> displayStatistics, List<long> idRemovedItems, List<long> idSelectedItems = null, OrderBy orderBy = OrderBy.name, Boolean ascending = true)
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
            View.isInitialized = true;
            View.CurrentAdminMode = adminMode;
            View.CurrentShowHiddenItems = showHiddenItems;
            View.CurrentDisableNotAvailableItems = disableNotAvailableItems;
            if (!typesToLoad.Any(t => t == ItemType.Multimedia || t == ItemType.ScormPackage || t == ItemType.VideoStreaming))
            {
                displayStatistics.Remove(StatisticType.plays);
                displayStatistics.Remove(StatisticType.myplays);
            }
            if (!typesToLoad.Any(t => t != ItemType.Folder && t != ItemType.Link))
            {
                displayStatistics.Remove(StatisticType.downloads);
                displayStatistics.Remove(StatisticType.mydownloads);
            }
            View.DisplayStatistics = displayStatistics;
            LoadItems(Service.GetPermissions(identifier, UserContext.CurrentUserID), UserContext.CurrentUserID, identifier, adminMode, showHiddenItems, disableNotAvailableItems, typesToLoad, availability, displayStatistics, idRemovedItems, idSelectedItems);
        }
        public void InitView( RepositoryIdentifier identifier, Boolean loadSelectedItems, Boolean adminMode, Boolean showHiddenItems, Boolean disableNotAvailableItems, ItemAvailability availability, Boolean allowSelectFolder, List<long> idSelectedItems, List<long> idRemovedItems = null)
        {
            if (SessionTimeout())
                return;
            if (idRemovedItems == null)
                idRemovedItems = new List<long>();
            if (!loadSelectedItems)
            {
                idRemovedItems.AddRange(idSelectedItems);
                idSelectedItems = new List<long>();
            }
            List<ItemType> typesToLoad = new List<ItemType>() { { ItemType.File }, { ItemType.Multimedia }, { ItemType.Link }, { ItemType.ScormPackage }, { ItemType.SharedDocument }, { ItemType.VideoStreaming } };
            InitView(identifier, adminMode, showHiddenItems, disableNotAvailableItems, typesToLoad, availability, new List<StatisticType>(), idRemovedItems, idSelectedItems);
        }
        public void InitView(RepositoryIdentifier identifier, Boolean loadSelectedItems, Boolean adminMode, Boolean showHiddenItems, Boolean disableNotAvailableItems, List<ItemType> typesToLoad, ItemAvailability availability, Boolean allowSelectFolder, List<long> idSelectedItems, List<long> idRemovedItems = null)
        {
            if (SessionTimeout())
                return;
            if (idRemovedItems == null)
                idRemovedItems = new List<long>();
            if (!loadSelectedItems)
            {
                idRemovedItems.AddRange(idSelectedItems);
                idSelectedItems = new List<long>();
            }
            InitView(identifier, adminMode, showHiddenItems, disableNotAvailableItems, typesToLoad, availability, new List<StatisticType>(), idRemovedItems, idSelectedItems);
        }


        private void LoadItems(ModuleRepository module, Int32 idCurrentUser, RepositoryIdentifier identifier, Boolean adminMode, Boolean showHiddenItems, Boolean disableNotAvailableItems, List<ItemType> typesToLoad, ItemAvailability availability, List<StatisticType> displayStatistics, List<long> idRemovedItems, List<long> idSelectedItems)
        {
            List<ItemType> aTypes = new List<ItemType>();
            if (!typesToLoad.Contains(ItemType.Folder))
                aTypes.Add(ItemType.Folder);
            aTypes.AddRange(typesToLoad);
            List<dtoRepositoryItemToSelect> items = Service.ItemsToSelectGet(idCurrentUser, "/", module, identifier, adminMode, showHiddenItems, disableNotAvailableItems, aTypes, availability, displayStatistics, idRemovedItems, idSelectedItems, false, false);
            idSelectedItems = idSelectedItems.Where(i => items.Any(x => x.Id == i)).ToList();
            View.IdSelectedItems = idSelectedItems;
            if (items == null)
                View.LoadItems(null);
            else
                View.LoadItems(items);
        }

        public List<dtoRepositoryItemToSelect> GetSelectedItems(List<dtoRepositoryItemToSelect> selectedItems, RepositoryIdentifier identifier, Boolean adminMode, List<ItemType> typesToLoad, List<StatisticType> displayStatistics)
        {
            if (!SessionTimeout())
                return Service.ItemsToSelectGet(selectedItems,!View.LoadForModule,  UserContext.CurrentUserID, "/", Service.GetPermissions(identifier, UserContext.CurrentUserID), identifier, adminMode,typesToLoad, displayStatistics);
            else
                return new List<dtoRepositoryItemToSelect>();
        }
        public List<ModuleActionLink> GetSelectedItemsActionLink(List<long> idItems)
        {
            return Service.LinkItemsGetModuleAction(true,idItems, (Int32)ModuleRepository.Base2Permission.DownloadOrPlay, Service.GetIdModule());
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