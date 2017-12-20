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
    public class ItemDetailsPresenter : lm.Comol.Core.DomainModel.Common.DomainPresenter
    {
        #region "Initialize"
            private lm.Comol.Core.InLineTags.Business.ServiceInLineTags _ServiceInLineTags;
            private ServiceRepository service;
            public virtual BaseModuleManager CurrentManager { get; set; }
            private Int32 currentIdModule;
            protected virtual IViewItemDetails View
            {
                get { return (IViewItemDetails)base.View; }
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
            private lm.Comol.Core.InLineTags.Business.ServiceInLineTags ServiceTags
            {
                get
                {
                    if (_ServiceInLineTags == null)
                        _ServiceInLineTags = new lm.Comol.Core.InLineTags.Business.ServiceInLineTags(AppContext);
                    return _ServiceInLineTags;
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
            public ItemDetailsPresenter(iApplicationContext oContext)
                : base(oContext)
            {
                this.CurrentManager = new BaseModuleManager(oContext);
            }
            public ItemDetailsPresenter(iApplicationContext oContext, IViewItemDetails view)
                : base(oContext, view)
            {
                this.CurrentManager = new BaseModuleManager(oContext);
            }
        #endregion
        public Boolean SaveItem(long idItem, String description, String name,String url,DisplayMode? mode, Boolean isVisible, Boolean allowUpload, List<String> tags)
        {
            Boolean executed = false;
            if (SessionTimeout())
                View.DisplaySessionTimeout();
            else
            {
                ModuleRepository.ObjectType oType = ModuleRepository.ObjectType.File;
                ModuleRepository.ActionType uAction = ModuleRepository.ActionType.GenericError;
                Int32 idCommunity = UserContext.CurrentCommunityID;
                liteRepositoryItem item = Service.ItemGet(idItem);
                if (item == null)
                {
                    View.DisplayUnknownItem(ItemAction.edit);
                    uAction = ModuleRepository.ActionType.UnknownItemFound;
                }
                else
                {
                    idCommunity = item.Repository.IdCommunity;
                    Int32 idCurrentUser = UserContext.CurrentUserID;
                    oType = ModuleRepository.GetObjectType(item.Type);

                    ItemSaving saving = Service.ItemSetBaseSettings(idItem, description, name, url, mode,isVisible, allowUpload, tags);
                    if (saving== ItemSaving.None)
                    {
                        View.DisplayUnavailableItem(ItemAction.edit);
                        uAction = ModuleRepository.ActionType.UnavailableItem;
                    }
                    else
                    {
                        executed = (saving== ItemSaving.Saved);
                        View.DisplayUpdateMessage(saving);
                        switch (saving)
                        {
                            case ItemSaving.Saved:
                                uAction = ModuleRepository.ActionType.ItemSavedDetails;
                                View.UpdateDefaultTags(ServiceTags.GetAvailableTags(idCurrentUser, idCommunity, Service.GetIdModule(), ModuleRepository.UniqueCode));
                                break;
                            case ItemSaving.None:
                                uAction = ModuleRepository.ActionType.ItemTryToSaveDetails;
                                break;
                            default:
                                uAction = ModuleRepository.ActionType.ItemSavedSomeDetails;
                                break;
                        }
                    }
                }
                View.SendUserAction(idCommunity, Service.GetIdModule(), uAction, idItem, oType);
            }
            return executed;
        }
        public Boolean ExecuteAction(long idItem, ItemAction action)
        {
            Boolean executed = false;
            Int32 idCommunity = UserContext.CurrentCommunityID;
            Boolean reloadItems = true;
            ModuleRepository.ObjectType oType = ModuleRepository.ObjectType.File;
            ModuleRepository.ActionType uAction = ModuleRepository.ActionType.GenericError;
            if (SessionTimeout())
                return executed;
            liteRepositoryItem item = Service.ItemGet(idItem);
            if (item == null)
            {
                View.DisplayUnknownItem(action);
                uAction = ModuleRepository.ActionType.UnknownItemFound;
            }
            else
            {
                idCommunity = item.Repository.IdCommunity;
                oType = ModuleRepository.GetObjectType(item.Type);


                switch (action)
                {
                    case ItemAction.hide:
                    case ItemAction.show:
                        liteRepositoryItem rItem = Service.ItemSetVisibility(idItem, (action == ItemAction.show), item.Repository.Type, item.Repository.IdCommunity);
                        if (rItem == null)
                        {
                            View.DisplayUnavailableItem(action);
                            uAction = ModuleRepository.ActionType.UnavailableItem;
                        }
                        else
                        {
                            executed = (rItem.IsVisible == (action == ItemAction.show));
                            View.DisplayUpdateMessage(action, executed, rItem.DisplayName, rItem.Extension, rItem.Type);
                            String folderName = (rItem.IdFolder == 0 ? View.GetRootFolderFullname() : Service.FolderGetName(rItem.IdFolder));
                            View.NotifyVisibilityChanged(Service.GetIdModule(), rItem.IdFolder, folderName, RootObject.RepositoryItems(rItem.Repository.Type, rItem.Repository.IdCommunity, rItem.Id, rItem.IdFolder), rItem);
                            if (executed)
                            {
                                switch (item.Type)
                                {
                                    case ItemType.Folder:
                                        if (action == ItemAction.show)
                                            uAction = ModuleRepository.ActionType.ShowFolder;
                                        else
                                            uAction = ModuleRepository.ActionType.HideFolder;
                                        break;
                                    default:
                                        if (action == ItemAction.show)
                                            uAction = ModuleRepository.ActionType.ShowItem;
                                        else
                                            uAction = ModuleRepository.ActionType.HideItem;
                                        break;
                                }
                                reloadItems = executed;
                            }
                            else
                            {
                                if (action == ItemAction.show)
                                    uAction = ModuleRepository.ActionType.UnableToShow;
                                else
                                    uAction = ModuleRepository.ActionType.UnableToHide;
                            }
                        }
                        break;
                }
            }
            View.SendUserAction(idCommunity, Service.GetIdModule(), uAction, idItem, oType);
            return executed;
        }
        private Boolean SessionTimeout()
        {
            if (UserContext.isAnonymous)
                return true;
            else
                return false;
        }
    }
}