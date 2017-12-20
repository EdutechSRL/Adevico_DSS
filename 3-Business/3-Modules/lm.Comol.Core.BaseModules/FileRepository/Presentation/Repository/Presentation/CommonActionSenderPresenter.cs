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
    public class CommonActionSenderPresenter : lm.Comol.Core.DomainModel.Common.DomainPresenter
    {
        #region "Initialize"
        private ServiceRepository service;
            public virtual BaseModuleManager CurrentManager { get; set; }
            private Int32 currentIdModule;
            protected virtual IViewCommonActionSender View
            {
                get { return (IViewCommonActionSender)base.View; }
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
            public CommonActionSenderPresenter(iApplicationContext oContext)
                : base(oContext)
            {
                this.CurrentManager = new BaseModuleManager(oContext);
            }
            public CommonActionSenderPresenter(iApplicationContext oContext, IViewCommonActionSender view)
                : base(oContext, view)
            {
                this.CurrentManager = new BaseModuleManager(oContext);
            }
        #endregion

        public void InitView(long idItem, long idVersion, long idLink)
        {
            if (SessionTimeout()) 
                View.StopTimer();
            else
            {
                liteRepositoryItemVersion version = Service.ItemGetVersion(idItem, idVersion);
                if (version != null)
                    View.InitializeContext(version.IdItem, version.Id, idLink, version.UniqueIdItem, version.UniqueIdVersion);
                else
                    View.StopTimer();
            }
        }
        public Boolean SessionTimeout()
        {
            if (UserContext.isAnonymous)
                return true;
            else
                return false;
        }
    }
}