using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel;
using lm.Comol.Core.BaseModules.Repository.Business;
using lm.Comol.Core.Business;
namespace lm.Comol.Core.BaseModules.Repository.Presentation
{
    public class ScormPackageSettingsPresenter : lm.Comol.Core.DomainModel.Common.DomainPresenter
    {
          private int _ModuleID;
        private ServiceCommunityRepository _Service;

        private int ModuleID
        {
            get
            {
                if (_ModuleID <= 0)
                {
                    _ModuleID = this.Service.ServiceModuleID();
                }
                return _ModuleID;
            }
        }
        public virtual BaseModuleManager CurrentManager { get; set; }
        protected virtual IViewScormPackageSettings View
        {
            get { return (IViewScormPackageSettings)base.View; }
        }
        private ServiceCommunityRepository Service
        {
            get
            {
                if (_Service == null)
                    _Service = new ServiceCommunityRepository(AppContext);
                return _Service;
            }
        }
        public ScormPackageSettingsPresenter(iApplicationContext oContext):base(oContext){
            this.CurrentManager = new BaseModuleManager(oContext);
        }
        public ScormPackageSettingsPresenter(iApplicationContext oContext, IViewScormPackageSettings view)
            : base(oContext, view)
        {
            this.CurrentManager = new BaseModuleManager(oContext);
        }
        public void InitView(){
            int IdCommunity = UserContext.CurrentCommunityID;
            long idFile = View.PreloadedFileId;
            BaseCommunityFile file = Service.GetItem(idFile);
            if (file != null && file.CommunityOwner != null)
                IdCommunity = file.CommunityOwner.Id;

            if (UserContext.isAnonymous)
                View.SendToSessionExpiredPage(IdCommunity, UserContext.Language.Code);
            else {
                View.BackUrl = View.PreloadedBackUrl;
              
                long moduleLinkId = View.PreloadedLinkId;
                View.ModuleLinkId = moduleLinkId;
                View.FileId = idFile;
                if (idFile == 0 && moduleLinkId == 0)
                    View.ShowUnkownFile(IdCommunity, ModuleID, CoreModuleRepository.UniqueID);
                else {
                    Person person = CurrentManager.GetPerson(UserContext.CurrentUserID);
                    ModuleLink link = CurrentManager.Get<ModuleLink>(moduleLinkId);
                    ScormMetadataPermission permission = ScormMetadataPermission.none;
                    if (link == null && file == null)
                        View.ShowUnkownFile(IdCommunity, ModuleID, CoreModuleRepository.UniqueID);
                    else if (!file.isSCORM)
                        View.ShowNoScormFile(file.DisplayName);
                    else if ((link == null && file != null) || file.GetType()== typeof(CommunityFile))
                    {
                        IdCommunity = (file.CommunityOwner == null ? 0 : file.CommunityOwner.Id);
                        CoreModuleRepository module = Service.ServicePermission(UserContext.CurrentUserID, IdCommunity);
                        if (Service.HasPermissionToSeeRepositoryItem(idFile, module.Administration, module.Administration, UserContext.CurrentUserID))
                        {
                            permission = (module.Administration || (module.Edit && file.Owner == person)) ? ScormMetadataPermission.edit : ScormMetadataPermission.view;
                            View.InitializeMetadataControl(file.UniqueID, file.DisplayName, permission);
                        }
                        else
                            View.ShowNoPermissionToEditMetadata(IdCommunity, ModuleID, CoreModuleRepository.UniqueID, file.DisplayName);
                    }
                    else {
                        IdCommunity = link.SourceItem.CommunityID;
                        
                        ModuleObject linkedObject= ModuleObject.CreateLongObject(file.Id,file, (int)CoreModuleRepository.ObjectType.ScormPackage,IdCommunity,CoreModuleRepository.UniqueID);
                        permission = View.GetModuleLinkPermission(link.SourceItem.CommunityID,moduleLinkId,link.SourceItem, linkedObject, UserContext.CurrentUserID );
                        if (permission == ScormMetadataPermission.none)
                            View.ShowNoPermissionToEditMetadata(IdCommunity, link.SourceItem.ServiceID, link.SourceItem.ServiceCode, file.DisplayName);
                        else
                            View.InitializeMetadataControl(file.UniqueID, file.DisplayName, permission);
                    }
                }
            }
        }
    }
}

