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
    public class UrlProviderSettingsPresenter : lm.Comol.Core.DomainModel.Common.DomainPresenter
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
            protected virtual IViewUrlProviderSettings View
            {
                get { return (IViewUrlProviderSettings)base.View; }
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
            public UrlProviderSettingsPresenter(iApplicationContext oContext)
                : base(oContext)
            {
                this.CurrentManager = new BaseModuleManager(oContext);
            }
            public UrlProviderSettingsPresenter(iApplicationContext oContext, IViewUrlProviderSettings view)
                : base(oContext, view)
            {
                this.CurrentManager = new BaseModuleManager(oContext);
            }
        #endregion

        public void InitView()
        {
            long idProvider = View.PreloadedIdProvider;
            View.IdProvider = idProvider;
            if (UserContext.isAnonymous)
                View.DisplaySessionTimeout();
            else
            {
                ModuleProviderManagement module = ModuleProviderManagement.CreatePortalmodule(UserContext.UserTypeID);
                View.AllowManagement = (module.Administration || module.ViewProviders);
                if (module.EditProvider || module.Administration)
                {
                    dtoProvider provider = Service.GetAuthenticationProvider(idProvider);
                    if (provider == null)
                        View.DisplayProviderUnknown();
                    else if (typeof(dtoUrlProvider) != provider.GetType())
                        View.GotoManagement();
                    else if (provider.Deleted != BaseStatusDeleted.None)
                        View.DisplayDeletedProvider((provider.Translation == null) ? provider.Name : provider.Translation.Name, provider.Type);
                    else
                    {
                        View.LoadProviderInfo((dtoUrlProvider)provider);
                        View.AllowEdit = true;
                    }
                }
                else
                    View.DisplayNoPermission();
            }
        }

        public Boolean UpdateEncryptionInfo(long idProvider,lm.Comol.Core.Authentication.Helpers.EncryptionInfo dto)
        {
            Boolean result = false;
            if (idProvider > 0)
                result = Service.UpdateProviderEncryptionInfo(idProvider, dto);
            return result;
        }

     
        #region "Validation"
            public Boolean ValidateUrlProviderKey(String key)
            {
                byte[] bytesKey;
                try
                {
                    bytesKey = Convert.FromBase64String(key);
                    if (bytesKey.Length == 32)
                        return true;
                    else
                        return false;
                }
                catch (Exception ex)
                {
                    return false;
                }
            }
            public Boolean ValidateUrlProviderInitializationVector(String vector)
            {
                byte[] initVec;
                try
                {
                    initVec = Convert.FromBase64String(vector);
                    if (initVec.Length == 16)
                        return true;
                    else
                        return false;
                }
                catch (Exception ex)
                {
                    return false;
                }
            }
            public String DecryptString(Authentication.Helpers.EncryptionInfo encryptionInfo, String encrypted)
            {
                return ValidateUserString(encryptionInfo, encrypted, true);
            }
            public String CryptString(Authentication.Helpers.EncryptionInfo encryptionInfo, String decrypted)
            {
                return ValidateUserString(encryptionInfo, decrypted, false);
            }
            public String ValidateUserString(Authentication.Helpers.EncryptionInfo encryptionInfo, String value, Boolean decrypt)
            {
                if (!String.IsNullOrEmpty(encryptionInfo.Key) && !String.IsNullOrEmpty(encryptionInfo.InitializationVector) && !String.IsNullOrEmpty(value))
                {
                    try
                    {
                        return (decrypt) ? lm.Comol.Core.Authentication.Helpers.CryptoUtils.DecryptValue(value, encryptionInfo) : lm.Comol.Core.Authentication.Helpers.CryptoUtils.Crypt(value, encryptionInfo);
                    }
                    catch (Exception ex)
                    {
                        return "";
                    }
                }
                else
                    return "";
            }
        #endregion
       
        #region "Login Format"
            
            public Boolean SaveLoginFormat(dtoLoginFormat loginFormat) {
                Boolean result = (Service.SaveLoginFormat(View.IdProvider, loginFormat) != null);
                if (result)
                    View.LoadLoginFormats(Service.GetProviderLoginFormats(View.IdProvider));
                return result;
            }
            public void VirtualDeleteFormat(long idFormat) {
                Boolean deleted = Service.VirtualDeleteLoginFormat(idFormat);
                View.LoadLoginFormats(Service.GetProviderLoginFormats(View.IdProvider));
            }
            public void VirtualUndeleteFormat(long idFormat) {
                Boolean deleted = Service.VirtualUndeleteLoginFormat(idFormat);
                View.LoadLoginFormats(Service.GetProviderLoginFormats(View.IdProvider));
            }
            public void DeleteFormat(long idFormat) {
                Boolean deleted = Service.PhisicalDeleteLoginFormat(idFormat);
                View.LoadLoginFormats(Service.GetProviderLoginFormats(View.IdProvider));
            }
            public dtoLoginFormat GetLoginFormat(long idFormat)
            {
                return Service.GetProviderLoginFormat(idFormat);
            }
        #endregion

    }
}