using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.BaseModules.ProviderManagement.Business;
using lm.Comol.Core.Business;
using lm.Comol.Core.DomainModel;


namespace lm.Comol.Core.BaseModules.ProviderManagement.Presentation
{
    public class ProvidersManagementPresenter : lm.Comol.Core.DomainModel.Common.DomainPresenter
    {
         #region "Initialize"
            private int _ModuleID;
            private ProviderManagementService _Service;
            //private int ModuleID
            //{
            //    get
            //    {
            //        if (_ModuleID <= 0)
            //        {
            //            _ModuleID = this.Service.ServiceModuleID();
            //        }
            //        return _ModuleID;
            //    }
            //}
            public virtual BaseModuleManager CurrentManager { get; set; }
            protected virtual IViewProvidersManagement View
            {
                get { return (IViewProvidersManagement)base.View; }
            }
            private ProviderManagementService Service
            {
                get
                {
                    if (_Service == null)
                        _Service = new ProviderManagementService(AppContext);
                    return _Service;
                }
            }
            public ProvidersManagementPresenter(iApplicationContext oContext)
                : base(oContext)
            {
                this.CurrentManager = new BaseModuleManager(oContext);
            }
            public ProvidersManagementPresenter(iApplicationContext oContext, IViewProvidersManagement view)
                : base(oContext, view)
            {
                this.CurrentManager = new BaseModuleManager(oContext);
            }
        #endregion

        public void InitView()
        {
            if (UserContext.isAnonymous)
                View.DisplaySessionTimeout();
            else
            {
                ModuleProviderManagement module = ModuleProviderManagement.CreatePortalmodule(UserContext.UserTypeID);
                if (module.ViewProviders || module.Administration)
                {
                    View.CurrentPageSize = View.PreLoadedPageSize;
                    LoadProviders(0, View.CurrentPageSize);
                }
                else
                    View.NoPermission();
            }
        }

        public void LoadProviders(int currentPageIndex, int currentPageSize)
        {
            if (UserContext.isAnonymous)
                View.DisplaySessionTimeout();
            else
            {
                ModuleProviderManagement module = ModuleProviderManagement.CreatePortalmodule(UserContext.UserTypeID);
                PagerBase pager = new PagerBase();
                pager.PageSize = currentPageSize;
                pager.Count = (int)Service.ProvidersCount() - 1;
                pager.PageIndex = currentPageIndex;
                View.Pager = pager;
                View.LoadProviders(Service.GetAuthenticationProviders(pager.PageIndex, currentPageSize, module));
            }
        }

        public void VirtualDelete(long idProvider) {
            if (UserContext.isAnonymous)
                View.DisplaySessionTimeout();
            else
            {
                Boolean deleted = Service.VirtualDelete(idProvider);
                LoadProviders(View.Pager.PageIndex, View.CurrentPageSize);
            }
        }
        public void VirtualUndelete(long idProvider) {
            if (UserContext.isAnonymous)
                View.DisplaySessionTimeout();
            else
            {
                Boolean deleted = Service.VirtualUndelete(idProvider);
                LoadProviders(View.Pager.PageIndex, View.CurrentPageSize);
            }
        }
        public void PhisicalDelete(long idProvider) {
            if (UserContext.isAnonymous)
                View.DisplaySessionTimeout();
            else
            {
                Boolean deleted = Service.PhisicalDelete(idProvider);
                LoadProviders(View.Pager.PageIndex, View.CurrentPageSize);
            }
        }
        public void Enable(long idProvider,Boolean enable)
        {
            if (UserContext.isAnonymous)
                View.DisplaySessionTimeout();
            else
            {
                Boolean deleted = Service.Enable(idProvider, enable);
                LoadProviders(View.Pager.PageIndex, View.CurrentPageSize);
            }
        }
    }
}