using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel;
using lm.Comol.Core.BaseModules.CommunityDiary.Domain;
using lm.Comol.Core.BaseModules.CommunityDiary.Business;
using lm.Comol.Core.Business;
namespace lm.Comol.Core.BaseModules.CommunityDiary.Presentation
{
    public class CommunityDiaryPresenter : lm.Comol.Core.DomainModel.Common.DomainPresenter
    {
        private ServiceCommunityDiary _Service;
        private lm.Comol.Core.BaseModules.FileRepository.Business.ServiceRepository _ServiceRepository;

        public virtual BaseModuleManager CurrentManager { get; set; }
        protected virtual IViewCommunityDiary View
        {
            get { return (IViewCommunityDiary)base.View; }
        }
        private ServiceCommunityDiary Service
        {
            get
            {
                if (_Service == null)
                    _Service = new ServiceCommunityDiary(AppContext);
                return _Service;
            }
        }
        private lm.Comol.Core.BaseModules.FileRepository.Business.ServiceRepository ServiceRepository
        {
            get
            {
                if (_ServiceRepository == null)
                    _ServiceRepository = new lm.Comol.Core.BaseModules.FileRepository.Business.ServiceRepository(AppContext);
                return _ServiceRepository;
            }
        }
        public CommunityDiaryPresenter(iApplicationContext oContext):base(oContext){
            CurrentManager = new BaseModuleManager(oContext);
            _ServiceRepository = new lm.Comol.Core.BaseModules.FileRepository.Business.ServiceRepository(oContext);
        }
        public CommunityDiaryPresenter(iApplicationContext oContext, IViewCommunityDiary view)
            : base(oContext, view)
        {
            CurrentManager = new BaseModuleManager(oContext);
            _ServiceRepository = new lm.Comol.Core.BaseModules.FileRepository.Business.ServiceRepository(oContext);
        }

        public void InitView(String unknownUser)
        {
            Int32 idModule =  Service.ServiceModuleID();
            int idCommunity = View.PreloadIdCommunity;
            if (idCommunity==0 && UserContext.CurrentCommunityID>0)
                idCommunity = UserContext.CurrentCommunityID;
            View.IdModuleCommunityDiary = idModule;
            View.IdModuleRepository = CurrentManager.GetModuleID(lm.Comol.Core.FileRepository.Domain.ModuleRepository.UniqueCode);
            View.IdCommunityDiary = idCommunity;
            if (UserContext.isAnonymous)
                View.DisplaySessionTimeout(idCommunity);
            else
            {
                liteCommunity community = CurrentManager.GetLiteCommunity(idCommunity);
                if (community == null && idCommunity > 0)
                {
                    View.IdCommunityDiary = -View.PreloadIdCommunity;
                    View.ShowUnkownCommunityDiary(idCommunity, idModule);
                }
                else
                {
                    ModuleCommunityDiary module = Service.GetPermissions(UserContext.CurrentUserID, idCommunity);
                    if (module == null)
                        module = new ModuleCommunityDiary();
                    if (module.Administration || module.DeleteItem || module.Edit || module.ViewDiaryItems)
                    {
                        View.DisplayOrderAscending = View.PreloadAscending;
                        InternalLoadDiaryItems(module, idCommunity, idModule, unknownUser);
                    }
                    else
                        View.HideItemsForNoPermission(idCommunity, idModule);
                }

                if (community == null && idCommunity == 0)
                    View.SetTitleName(View.GetPortalNameTranslation());
                else if (community != null && community.Id != UserContext.CurrentCommunityID)
                    View.SetTitleName(community.Name);
            }    
        }
        public void LoadDiaryItems(Int32 idCommunity, String unknownUser)
        {
            if (UserContext.isAnonymous)
                View.DisplaySessionTimeout(idCommunity);
            else
                LoadDiaryItems(Service.GetPermissions(UserContext.CurrentUserID, idCommunity), idCommunity, Service.ServiceModuleID(), unknownUser);
        }
        public void LoadDiaryItems(ModuleCommunityDiary module, Int32 idCommunity, Int32 idModule, String unknownUser)
        {
            if (UserContext.isAnonymous)
                View.DisplaySessionTimeout(idCommunity);
            else
            {
                if (idCommunity <= 0)
                    View.ShowUnkownCommunityDiary(idCommunity, idModule);
                else
                {
                    if (module == null)
                        module = new ModuleCommunityDiary();
                    InternalLoadDiaryItems(module, idCommunity, idModule, unknownUser);
                }
            }
        }
        private void InternalLoadDiaryItems(ModuleCommunityDiary module, Int32 idCommunity, Int32 idModule, String unknownUser)
        { 
            lm.Comol.Core.FileRepository.Domain.RepositoryIdentifier identifier = lm.Comol.Core.FileRepository.Domain.RepositoryIdentifier.Create((idCommunity>0 ? lm.Comol.Core.FileRepository.Domain.RepositoryType.Community :  lm.Comol.Core.FileRepository.Domain.RepositoryType.Portal),idCommunity);
            lm.Comol.Core.FileRepository.Domain.ModuleRepository moduleRepository = ServiceRepository.GetPermissions(identifier, UserContext.CurrentUserID);
            List<lm.Comol.Core.DomainModel.Repository.RepositoryAttachmentUploadActions> availableActions = Service.GetAvailableUploadActions(module, moduleRepository);
            lm.Comol.Core.DomainModel.Repository.RepositoryAttachmentUploadActions dAction = (availableActions == null || !availableActions.Any()) ? lm.Comol.Core.DomainModel.Repository.RepositoryAttachmentUploadActions.none : availableActions.FirstOrDefault();
            View.RepositoryIdentifier = identifier;
            View.InitializeAttachmentsControl(availableActions, dAction);
            List<dtoDiaryItem> items = Service.GetDtoDiaryItems(idCommunity, View.DisplayOrderAscending,module, moduleRepository,(module.Administration || module.Edit), availableActions, dAction, unknownUser);


            View.AllowAddItem = module.Administration || module.AddItem;
            View.AllowPrint = module.PrintList;
            View.AllowDeleteDiary = module.Administration;
            if (module.Administration || module.AddItem)
                View.SetAddItemUrl(idCommunity);
            int ItemsCountForDelete = (from item in items where item.Permission.AllowDelete select item.Id).Count();
            View.AllowItemsSelection = ((module.Administration || module.Edit || module.DeleteItem) && ItemsCountForDelete > 0);
            View.AllowMultipleDelete = ((module.Administration || module.Edit || module.DeleteItem) && ItemsCountForDelete > 0);
            View.LoadItems(items, idCommunity, idModule);
        }
        public void DeleteSelectedItems(List<long> idItems, Int32 idCommunity, String baseFilePath, String baseThumbnailPath, String unknownUser)
        {
            if (UserContext.isAnonymous)
                View.DisplaySessionTimeout(View.IdCommunityDiary);
            else
            {
                ModuleCommunityDiary module = Service.GetPermissions(UserContext.CurrentUserID, idCommunity);
                foreach (CommunityEventItem item in Service.EventItemsGet(idCommunity, idItems))
                {
                    DeleteItem(idCommunity, module, item, item.Id, baseFilePath, baseThumbnailPath);
                }
                LoadDiaryItems(idCommunity, unknownUser);
            }
        }
        public void DeleteItem(long idItem, Int32 idCommunity, String baseFilePath, String baseThumbnailPath, String unknownUser)
        {
            if (UserContext.isAnonymous)
                View.DisplaySessionTimeout(View.IdCommunityDiary);
            else
            {
                ModuleCommunityDiary module = Service.GetPermissions(UserContext.CurrentUserID, idCommunity);
                DeleteItem(idCommunity, module, Service.EventItemGet(idItem), idItem, baseFilePath, baseThumbnailPath);
                LoadDiaryItems(idCommunity, unknownUser);
            }
        }
        private void DeleteItem(Int32 idCommunity, ModuleCommunityDiary module, CommunityEventItem item, long idItem, String baseFilePath, String baseThumbnailPath)
        {
            if (item != null && (module.Administration || module.DeleteItem))
            {
                DateTime startDate = item.StartDate;
                DateTime endDate = item.EndDate;
                Boolean isVisible = item.IsVisible;
                Service.PhisicalDeleteItem(item, baseFilePath, baseThumbnailPath);
            }
        }
        public void DeleteCommunityDiary(Int32 idCommunity,String baseFilePath, String baseThumbnailPath, String unknownUser)
        {
            if (UserContext.isAnonymous)
                View.DisplaySessionTimeout(View.IdCommunityDiary);
            else
            {
                ModuleCommunityDiary module = Service.GetPermissions(UserContext.CurrentUserID, idCommunity);
                if (module.Administration)
                {
                    Service.PhisicalDeleteCommunityDiary(idCommunity, baseFilePath, baseThumbnailPath);
                }
                LoadDiaryItems(idCommunity, unknownUser);
            }
        }

        public void ChangeOrderBy(Int32 idCommunity, String unknownUser)
        {
            if (UserContext.isAnonymous)
                View.DisplaySessionTimeout(View.IdCommunityDiary);
            else
                LoadDiaryItems(idCommunity, unknownUser);
        }

        public void EditItemVisibility(long idItem, Int32 idCommunity, String unknownUser)
        {
            if (UserContext.isAnonymous)
                View.DisplaySessionTimeout(idCommunity);
            else
            {
                ModuleCommunityDiary module = Service.GetPermissions(UserContext.CurrentUserID, idCommunity);
                CommunityEventItem item = Service.EventItemGet(idItem);
                if (item != null && module.Edit)
                    Service.EditEventItemVisibility(item);
                LoadDiaryItems(idCommunity, unknownUser);
            }
        }
        public void EditFileItemVisibility(long idItem,long idAttachment, Int32 idCommunity, Boolean visibleForModule, Boolean visibleForRepository, String unknownUser)
        {
            if (UserContext.isAnonymous)
                View.DisplaySessionTimeout(idCommunity, idItem);
            else
            {
                ModuleCommunityDiary module = Service.GetPermissions(UserContext.CurrentUserID, idCommunity);
                CommunityEventItem item = Service.EventItemGet(idItem);
                EventItemFile attachment = Service.EventItemGetAttachment(idAttachment);
                if (item != null && attachment!=null)
                    Service.AttachmentEditVisibility(item,attachment, visibleForModule, visibleForRepository);
                LoadDiaryItems(idCommunity, unknownUser);
            }
        }
    }
}