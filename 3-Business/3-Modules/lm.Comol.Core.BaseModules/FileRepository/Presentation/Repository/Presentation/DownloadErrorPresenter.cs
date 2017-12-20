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
    public class DownloadErrorPresenter : lm.Comol.Core.DomainModel.Common.DomainPresenter
    {
        #region "Initialize"
        private ServiceRepository service;
            public virtual BaseModuleManager CurrentManager { get; set; }
            private Int32 currentIdModule;
            protected virtual IViewDownloadError View
            {
                get { return (IViewDownloadError)base.View; }
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
            public DownloadErrorPresenter(iApplicationContext oContext)
                : base(oContext)
            {
                this.CurrentManager = new BaseModuleManager(oContext);
            }
            public DownloadErrorPresenter(iApplicationContext oContext, IViewDownloadError view)
                : base(oContext, view)
            {
                this.CurrentManager = new BaseModuleManager(oContext);
            }
        #endregion

        public void InitView(Boolean isExternalPage, long idItem, long idVersion, Int32 idModule, long idLink, DownloadErrorType type)
        {

            liteRepositoryItem item = (idItem > 0) ? Service.ItemGet(idItem) : null;
            Int32 idCommunity = (idLink > 0 ? Service.ModuleLinkGetIdSourceCommunity(idLink) : 0);

            if (item != null)
            {
                RepositoryType repositoryType = (item.IdCommunity > 0 ? RepositoryType.Community : RepositoryType.Portal);
                switch(repositoryType){
                    case RepositoryType.Community:
                        if (idCommunity == 0)
                            idCommunity = item.IdCommunity;
                        View.InitializeCommunityView(item.DisplayName, item.Extension, type, item.IdCommunity, ((idCommunity > 0) ? CurrentManager.GetCommunityName(idCommunity) : ""));
                        break;
                    case RepositoryType.Portal:
                        View.InitializePortalView(item.DisplayName, item.Extension, type);
                        break;
                        break;
                }
            }
            else
                View.InitializeView(type);
            if (isExternalPage)
                View.InitializeContext(GetContext(idCommunity, idItem, idVersion, idModule, item));
            else
                View.InitializeContext();
            }
        public lm.Comol.Core.DomainModel.Helpers.ExternalPageContext GetContext(Int32 idCommunity, long idItem, long idVersion, Int32 idModule, liteRepositoryItem item)
        {
            lm.Comol.Core.DomainModel.Helpers.ExternalPageContext context = new lm.Comol.Core.DomainModel.Helpers.ExternalPageContext();
            context.Skin = new DomainModel.Helpers.dtoItemSkin();
            context.Skin.IdCommunity = idCommunity;
            if (idCommunity > 0)
                context.Skin.IdOrganization = CurrentManager.GetIdOrganizationFromCommunity(idCommunity);
            else
                context.Skin.IsForPortal = true;

            context.Source= new ModuleObject() { CommunityID= idCommunity, ObjectLongID= idItem, ObjectIdVersion = idVersion, ServiceID=idModule,FQN = typeof(liteRepositoryItem).FullName };
            if (item == null)
                context.Source.ObjectTypeID = (int)ModuleRepository.ObjectType.File;
            else{
                switch(item.Type){
                    case ItemType.File:
                        context.Source.ObjectTypeID =  (int)ModuleRepository.ObjectType.File;
                        break;
                    case ItemType.Multimedia:
                        context.Source.ObjectTypeID =  (int)ModuleRepository.ObjectType.File;
                        break;
                    case ItemType.ScormPackage:
                        context.Source.ObjectTypeID =  (int)ModuleRepository.ObjectType.ScormPackage;
                        break;
                }
            }
            return context;
        }
    }
}