using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel;
using lm.Comol.Core.Business;
using lm.Comol.Modules.EduPath.BusinessLogic;
using lm.Comol.Core.FileRepository.Domain;
using lm.Comol.Modules.EduPath.Domain;


namespace lm.Comol.Modules.EduPath.Presentation
{
    public class AddRepositoryActionPresenter  : lm.Comol.Core.DomainModel.Common.DomainPresenter
    {
        #region "Initialize"
            private int _IdModule;
            private int CurrentIdModule
            {
                get
                {
                    if (_IdModule <= 0)
                        _IdModule = Service.ServiceModuleID();
                    return _IdModule;
                }
            }
            private Service _Service;
            private lm.Comol.Core.FileRepository.Business.ServiceFileRepository _ServiceRepository;
            private Service Service
            {
                get
                {
                    if (_Service == null)
                        _Service = new Service(AppContext);
                    return _Service;
                }
            }
            private lm.Comol.Core.FileRepository.Business.ServiceFileRepository ServiceRepository
            {
                get
                {
                    if (_ServiceRepository == null)
                        _ServiceRepository = new lm.Comol.Core.FileRepository.Business.ServiceFileRepository(AppContext);
                    return _ServiceRepository;
                }
            }
            public virtual BaseModuleManager CurrentManager { get; set; }
            protected virtual IViewAddRepositoryAction View
            {
                get { return (IViewAddRepositoryAction)base.View; }
            }
            public AddRepositoryActionPresenter(iApplicationContext oContext):base(oContext){
                CurrentManager = new BaseModuleManager(oContext);
                _Service = new Service(oContext);
                _ServiceRepository = new Core.FileRepository.Business.ServiceFileRepository(oContext);
            }
            public AddRepositoryActionPresenter(iApplicationContext oContext, IViewAddRepositoryAction view)
                : base(oContext, view)
            {
                CurrentManager = new BaseModuleManager(oContext);
                _Service = new Service(oContext);
                _ServiceRepository = new Core.FileRepository.Business.ServiceFileRepository(oContext);
            }
        #endregion

        public void InitView(RepositoryIdentifier identifier,long idActivity, List<long> unloadItems = null)
        {
            View.Identifier = identifier;
            unloadItems = unloadItems ?? new List<long>();
            View.UnloadItems = unloadItems;
            View.CurrentAction = DisplayRepositoryAction.none;
            View.IdActivity = idActivity;
            if (UserContext.isAnonymous)
                View.DisplayWorkingSessionExpired(identifier.IdCommunity, CurrentIdModule);
            else
            {
                ModuleRepository module = ServiceRepository.GetPermissions(identifier, UserContext.CurrentUserID);
                List<DisplayRepositoryAction> actions = GetAvailableActions(identifier, module);
                View.LoadAvailableActions(actions, DisplayRepositoryAction.select);
                if (actions.Any())
                {
                    if (actions.Contains(DisplayRepositoryAction.repositoryDownloadOrPlay))
                        View.InitializeCommunityUploader(identifier);
                    View.InitializeInternalUploader(identifier);
                }
            }
        }

        private List<DisplayRepositoryAction> GetAvailableActions(RepositoryIdentifier identifier, ModuleRepository module)
        {
            if (UserContext.isAnonymous)
                View.DisplayWorkingSessionExpired(identifier.IdCommunity, CurrentIdModule);
            List<DisplayRepositoryAction> actions = new List<DisplayRepositoryAction>();
            if ((module.Administration || module.ManageItems || module.UploadFile) && identifier.Type == RepositoryType.Community && identifier.IdCommunity > 0)
                actions.Add(DisplayRepositoryAction.repositoryDownloadOrPlay);
            if (module.Administration || module.ManageItems || module.UploadFile || module.ViewItemsList || module.DownloadOrPlay)
            {
                List<lm.Comol.Core.FileRepository.Domain.ItemType> availableTypes = ServiceRepository.GetAvailableTypes(identifier, module, UserContext.CurrentUserID);
                if (availableTypes.Contains(lm.Comol.Core.FileRepository.Domain.ItemType.File))
                    actions.Add(DisplayRepositoryAction.downloadItem);
                if (availableTypes.Contains(lm.Comol.Core.FileRepository.Domain.ItemType.Multimedia))
                    actions.Add(DisplayRepositoryAction.playMultimedia);
                if (availableTypes.Contains(lm.Comol.Core.FileRepository.Domain.ItemType.ScormPackage))
                    actions.Add(DisplayRepositoryAction.playScormPackage);
            }

            actions.Add(DisplayRepositoryAction.internalDownloadOrPlay);
            return actions;
        }

        public void ChangeAction(long idActivity, RepositoryIdentifier identifier, DisplayRepositoryAction action, List<long> unloadItems)
        {
            if (UserContext.isAnonymous)
                View.DisplayWorkingSessionExpired(identifier.IdCommunity, CurrentIdModule);
            else
            {
                switch (action)
                {
                    case DisplayRepositoryAction.playMultimedia:
                    case DisplayRepositoryAction.downloadItem:
                    case DisplayRepositoryAction.playScormPackage:
                        ModuleRepository module = ServiceRepository.GetPermissions(identifier, UserContext.CurrentUserID);
                        switch (action)
                        {
                            case DisplayRepositoryAction.downloadItem:
                                View.InitializeLinkRepositoryItems(UserContext.CurrentUserID, module, identifier, unloadItems, lm.Comol.Core.FileRepository.Domain.ItemType.File);
                                break;
                            case DisplayRepositoryAction.playMultimedia:
                                View.InitializeLinkRepositoryItems(UserContext.CurrentUserID, module, identifier, unloadItems, lm.Comol.Core.FileRepository.Domain.ItemType.Multimedia);
                                break;
                            case DisplayRepositoryAction.playScormPackage:
                                View.InitializeLinkRepositoryItems(UserContext.CurrentUserID, module, identifier, unloadItems, lm.Comol.Core.FileRepository.Domain.ItemType.ScormPackage);
                                break;
                        }
                        break;
                }
                View.DisplayAction(action);
            }
        }
        public void AddFilesToItem(long idActivity, RepositoryIdentifier identifier)
        {
            if (UserContext.isAnonymous)
                View.DisplayWorkingSessionExpired(identifier.IdCommunity, CurrentIdModule);
            else
            {
                List<lm.Comol.Core.FileRepository.Domain.dtoModuleUploadedItem> files = null;
                Activity activity = Service.GetActivity(idActivity);
                if (activity != null)
                    files = View.UploadFiles(ModuleEduPath.UniqueCode, (Int32)ModuleEduPath.ObjectType.SubActivity, (Int32)ModuleEduPath.ActionType.DoSubActivity,  false);
                else
                {
                    View.DisplayActivityNotFound();
                    return;
                }

                if (files != null && files.Any(f => f.IsAdded))
                {
                    List<SubActivity> items = Service.SubActivityAddFiles(activity, identifier, files.Where(f => f.IsAdded).ToList(),CurrentIdModule, ServiceRepository.GetIdModule(), UserContext.CurrentUserID, UserContext.IpAddress, UserContext.ProxyIpAddress );
                    if (items == null || !items.Any())
                        View.DisplayItemsNotAdded();
                    else
                        View.DisplayItemsAdded();
                }
                else
                    View.DisplayNoFilesToAdd();
            }
        }
        public void AddCommunityFilesToItem(long idActivity, RepositoryIdentifier identifier)
        {
            if (UserContext.isAnonymous)
                View.DisplayWorkingSessionExpired(identifier.IdCommunity, CurrentIdModule);
            else
            {
                List<lm.Comol.Core.FileRepository.Domain.dtoModuleUploadedItem> files = null;
                Activity activity = Service.GetActivity(idActivity);
                if (activity != null)
                    files = View.UploadFiles(ModuleEduPath.UniqueCode, (Int32)ModuleEduPath.ObjectType.SubActivity, (Int32)ModuleEduPath.ActionType.DoSubActivity,  true);
                else
                {
                    View.DisplayActivityNotFound();
                    return;
                }
                if (files != null && files.Any(f => f.IsAdded))
                    AddCommunityFilesToItem(activity, identifier, (files == null) ? null : files.Where(f => f.IsAdded).Select(f => f.Link).ToList());
                else
                    View.DisplayNoFilesToAdd();
            }
        }
        public void AddCommunityFilesToItem(long idActivity, RepositoryIdentifier identifier, List<ModuleActionLink> links)
        {
            if (UserContext.isAnonymous)
                View.DisplayWorkingSessionExpired(identifier.IdCommunity, CurrentIdModule);
            else if (links != null && links.Count > 0)
                AddCommunityFilesToItem(Service.GetActivity(idActivity), identifier, links);
            else
                View.DisplayNoFilesToAdd();
        }

        private void AddCommunityFilesToItem(Activity activity, RepositoryIdentifier identifier,  List<ModuleActionLink> links)
        {
            if (links != null && links.Count > 0 && activity!=null)
            {
                Person person = CurrentManager.GetPerson(UserContext.CurrentUserID);
                List<SubActivity> inserted = new List<SubActivity>();
                DateTime createdOn = DateTime.Now;
                foreach(ModuleActionLink l in links){
                    SubActivity subActivity = new SubActivity();
                    subActivity.ParentActivity = activity;
                    subActivity.CreateMetaInfo(UserContext.CurrentUserID,UserContext.IpAddress, UserContext.ProxyIpAddress, createdOn);
                    subActivity.IdObjectLong = l.ModuleObject.ObjectLongID;
                    subActivity.IdObjectVersion=0;
                    subActivity.IdModuleAction = l.Action;
                    subActivity.Description = "";
                    subActivity.Link = l.Link;
                    subActivity.ContentPermission = l.Permission;
                    subActivity.Name = "";
                    subActivity.IdModule = l.ModuleObject.ServiceID;
                    subActivity.CodeModule = l.ModuleObject.ServiceCode;
                    subActivity.ContentType = SubActivityType.File;

                    SubActivity added = Service.SaveOrUpdateSubActivity(subActivity, l, CurrentIdModule, ModuleEduPath.UniqueCode, identifier.IdCommunity, person, UserContext.IpAddress, UserContext.ProxyIpAddress);
                    if (added!=null)
                        inserted.Add(added);
                }
                if (inserted.Any())
                    View.DisplayItemsAdded();
                else
                    View.DisplayItemsNotAdded();
            }
            else
                View.DisplayNoFilesToAdd();
        }
    }
}