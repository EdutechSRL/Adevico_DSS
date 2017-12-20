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
    public class FolderSelectorPresenter : lm.Comol.Core.DomainModel.Common.DomainPresenter
    {
        #region "Initialize"
            private ServiceRepository service;
            public virtual BaseModuleManager CurrentManager { get; set; }
            private Int32 currentIdModule;
            protected virtual IViewFolderSelector View
            {
                get { return (IViewFolderSelector)base.View; }
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
            public FolderSelectorPresenter(iApplicationContext oContext)
                : base(oContext)
            {
                this.CurrentManager = new BaseModuleManager(oContext);
            }
            public FolderSelectorPresenter(iApplicationContext oContext, IViewFolderSelector view)
                : base(oContext, view)
            {
                this.CurrentManager = new BaseModuleManager(oContext);
            }
        #endregion

        public void InitView(RepositoryType repositoryType , Int32 idRepositoryCommunity)
        {
            liteRepositorySettings settings = Service.SettingsGetDefault(repositoryType, idRepositoryCommunity);
        }
        public dtoContainerQuota GetFolderQuota(String repositoryPath, long idFolder, RepositoryType repositoryType, Int32 idRepositoryCommunity)
        {
            if (SessionTimeout())
                return null;
            else
                return Service.GetFolderQuota(repositoryPath,idFolder, repositoryType, idRepositoryCommunity);
        }
        public Boolean SessionTimeout()
        {
            if (UserContext.isAnonymous)
            {
                return true;
            }
            else
                return false;
        }
    }
}