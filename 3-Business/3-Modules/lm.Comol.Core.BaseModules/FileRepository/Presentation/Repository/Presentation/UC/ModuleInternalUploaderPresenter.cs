using lm.Comol.Core.BaseModules.FileRepository.Business;
using lm.Comol.Core.Business;
using lm.Comol.Core.DomainModel;
using lm.Comol.Core.FileRepository.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Core.BaseModules.FileRepository.Presentation 
{
    public class ModuleInternalUploaderPresenter : lm.Comol.Core.DomainModel.Common.DomainPresenter
    {
        #region "Initialize"
            private ServiceRepository service;
            public virtual BaseModuleManager CurrentManager { get; set; }
            private Int32 currentIdModule;
            protected virtual IViewModuleInternalUploader View
            {
                get { return (IViewModuleInternalUploader)base.View; }
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
            public ModuleInternalUploaderPresenter(iApplicationContext oContext)
                : base(oContext)
            {
                this.CurrentManager = new BaseModuleManager(oContext);
            }
            public ModuleInternalUploaderPresenter(iApplicationContext oContext, IViewModuleInternalUploader view)
                : base(oContext, view)
            {
                this.CurrentManager = new BaseModuleManager(oContext);
            }
        #endregion

        public void InitView(Int32 idUploaderUser, RepositoryIdentifier identifier, Boolean allowAnonymousUpload )
        {
            View.IdUploaderUser =idUploaderUser;
            View.RepositoryIdentifier = identifier;
            if (identifier==null || (SessionTimeout() && !allowAnonymousUpload))
            {
                View.DisableControl();
                return;
            }
            liteRepositorySettings settings = Service.SettingsGetByRepositoryIdentifier(identifier);
            View.LoadItemTypes(((settings==null ||settings.ItemTypes == null)  ? new List<ItemType>(){ ItemType.File} : settings.ItemTypes.Where(t => t.Deleted == BaseStatusDeleted.None && t.Type != ItemType.Folder && t.Type != ItemType.Link && t.Type != ItemType.RootFolder).Select(t => t.Type).Distinct().ToList()));
        }

        public List<dtoModuleUploadedItem> AddModuleInternalFiles(String istance,Int32 idUploaderUser, Boolean allowAnonymousUpload,RepositoryIdentifier identifier, List<dtoUploadedItem> items, Object obj, long idObject, Int32 idObjectType, String moduleCode, Int32 idModuleAjaxAction, Int32 idModuleAction = 0)
        {
            List<dtoModuleUploadedItem> addedFiles = new List<dtoModuleUploadedItem>();
            if (!SessionTimeout() || allowAnonymousUpload)
            {
                litePerson person = (idUploaderUser > 0 ? CurrentManager.GetLitePerson(idUploaderUser) : null);
                if (person == null && allowAnonymousUpload)
                    person = CurrentManager.GetLiteUnknownUser();
                if (person != null)
                {
                    liteRepositorySettings settings = Service.SettingsGetByRepositoryIdentifier(identifier);
                    addedFiles = Service.FileAddToInternalRepository(settings, istance, identifier, person, allowAnonymousUpload, items,  obj, idObject, idObjectType, moduleCode, idModuleAjaxAction, idModuleAction);
                }
            }
            return addedFiles;
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