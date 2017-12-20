using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.BaseModules.ProviderManagement.Business;
using lm.Comol.Core.Business;
using lm.Comol.Core.DomainModel;
using lm.Comol.Core.Authentication;


namespace lm.Comol.Core.BaseModules.ProviderManagement.Presentation
{
    public class ProviderDataPresenter : lm.Comol.Core.DomainModel.Common.DomainPresenter
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
            protected virtual IViewProviderData View
            {
                get { return (IViewProviderData)base.View; }
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
            public ProviderDataPresenter(iApplicationContext oContext)
                : base(oContext)
            {
                this.CurrentManager = new BaseModuleManager(oContext);
            }
            public ProviderDataPresenter(iApplicationContext oContext, IViewProviderData view)
                : base(oContext, view)
            {
                this.CurrentManager = new BaseModuleManager(oContext);
            }
        #endregion

        public void InitView(dtoProvider provider, Boolean allowAdvancedSettings)
        {
            View.IdProvider = (provider==null) ? 0 : provider.IdProvider ;
            if (UserContext.isAnonymous)
                View.DisplayProviderUnknown();
            else
            {
                View.LoadProvider(provider, (allowAdvancedSettings && provider.Type != AuthenticationProviderType.Internal));
            }
        }
        public Boolean SaveProvider()
        {
            dtoProvider provider = View.Current;
            if (Service.isUniqueProviderCode(provider.IdProvider, provider.UniqueCode))
            {
                if (provider is dtoInternalProvider)
                    return (Service.SaveProvider((dtoInternalProvider)provider) != null);
                else if (provider is dtoMacUrlProvider)
                    return (Service.SaveProvider((dtoMacUrlProvider)provider) != null);
                else if (provider is dtoUrlProvider)
                    return (Service.SaveProvider((dtoUrlProvider)provider) != null);
                else 
                    return (Service.SaveProvider(provider) != null);
            }
            else {
                View.DisplayDuplicateCode();
                return false;
            }
        }

        public Boolean ValidateUrlProviderKey(String key) { 
            byte[] bytesKey;
            try
            {
                bytesKey = Convert.FromBase64String(key);
                if (bytesKey.Length==32)
                    return true;
                else
                    return false;
            }
            catch (Exception ex) {
                return false;
            }
        }

        public Boolean ValidateUrlProviderInitializationVector(String vector) {
            byte[] initVec;
            try
            {
                initVec = Convert.FromBase64String(vector);
                if (initVec.Length == 16)
                    return true;
                else
                    return false;
            }
            catch (Exception ex) {
                return false;
            }
        }
    }
}