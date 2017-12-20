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
    public class MacUrlProviderSettingsPresenter : lm.Comol.Core.DomainModel.Common.DomainPresenter
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
            protected virtual IViewMacUrlProviderSettings View
            {
                get { return (IViewMacUrlProviderSettings)base.View; }
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
            public MacUrlProviderSettingsPresenter(iApplicationContext oContext)
                : base(oContext)
            {
                this.CurrentManager = new BaseModuleManager(oContext);
            }
            public MacUrlProviderSettingsPresenter(iApplicationContext oContext, IViewMacUrlProviderSettings view)
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
                    else if (typeof(dtoMacUrlProvider) != provider.GetType())
                        View.GotoManagement();
                    else if (provider.Deleted != BaseStatusDeleted.None)
                        View.DisplayDeletedProvider((provider.Translation == null) ? provider.Name : provider.Translation.Name, provider.Type);
                    else
                    {
                        View.AllowEdit = true;
                        long idAttribute = View.PreloadedIdInEditingAttribute;
                        View.IdAttributeEditing = (Service.MacAttributeExist(idAttribute) ? idAttribute : 0);
                        View.LoadAvailableTypes(Service.GetAvailableAttributeTypes(idProvider), View.PreloadedAttributeType);
                        View.LoadProviderInfo((dtoMacUrlProvider)provider);
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

        /// <summary>
        ///     Add new attribute
        /// </summary>
        /// <param name="nAttribute">new attribute to add</param>
        /// <param name="attributes">current attribute items</param>
        /// <returns></returns>
        public void AddAttribute(BaseUrlMacAttribute nAttribute) {
            BaseUrlMacAttribute att = Service.AddUrlMacAttribute(View.IdProvider, nAttribute);
            if (att == null)
                View.DisplayAttributeNotAdded();
            else
                View.GotoUrl(RootObject.EditUrlMacProviderSettings(View.IdProvider, nAttribute.Type, att.Id, true));
        }

        public void VirtualDeleteAttribute(long idAttribute)
        {
            long idProvider = View.IdProvider;
            if (Service.VirtualDeleteMacAttribute(idProvider, idAttribute, true))
                View.GotoUrl(RootObject.EditUrlMacProviderSettings(idProvider,View.CurrentAttributeToAdd, Service.GetPreviousMacAttribute(idProvider, idAttribute), false));
            else
                View.DisplayAttributeNotDeleted();
        }
        public void AddMacAttributeItem(long idAttribute, long idAttributeItem)
        {
            long idProvider = View.IdProvider;
            MacAttributeItem item = Service.AddMacAttributeItem(idAttribute, idAttributeItem);
            if (item != null)
                View.GotoUrl(RootObject.EditUrlMacProviderSettings(idProvider, View.CurrentAttributeToAdd, idAttribute, item.Id, true));
            else
                View.DisplayAttributeOptionNotAdded();
        }
        public void VirtualDeleteUrlMacAttributeItem(long idAttribute, long idItem, UrlMacAttributeType type)
        {
            long idProvider = View.IdProvider;
            long pIdAttributeItem = 0;
            if (Service.VirtualDeleteUrlMacAttributeItem(idAttribute, idItem, type, ref pIdAttributeItem, true))
                View.GotoUrl(RootObject.EditUrlMacProviderSettings(idProvider, View.CurrentAttributeToAdd, idAttribute, pIdAttributeItem, true));
            else
                View.DisplayAttributeOptionNotDeleted();
        }

        public void AddCompositeAttributeItem(long idAttribute, long idAttributeItem)
        {
            long idProvider = View.IdProvider;
            CompositeAttributeItem item = Service.AddCompositeAttributeItem(idAttribute, idAttributeItem);
            if (item != null)
                View.GotoUrl(RootObject.EditUrlMacProviderSettings(idProvider, View.CurrentAttributeToAdd, idAttribute, item.Id, true));
            else
                View.DisplayAttributeOptionNotAdded();
        }
        public void AddOrganizationAttributeItem(long idAttribute, Int32 idOrganization, long idDefaultPage, Int32 idDefaultProfile, String remoteCode)
        {
            long idProvider = View.IdProvider;
            OrganizationAttributeItem item = Service.AddOrganizationAttributeItem(idAttribute, idOrganization,idDefaultPage,idDefaultProfile,remoteCode);
            if (item != null)
                View.GotoUrl(RootObject.EditUrlMacProviderSettings(idProvider, View.CurrentAttributeToAdd, idAttribute, item.Id, true));
            else
                View.DisplayAttributeOptionNotAdded();
        }
        public void AddCatalogueAttributeItem(long idAttribute, long idCatalogue, String remoteCode)
        {
            long idProvider = View.IdProvider;
            CatalogueAttributeItem item = Service.AddCatalogueAttributeItem(idAttribute, idCatalogue, remoteCode);
            if (item != null)
                View.GotoUrl(RootObject.EditUrlMacProviderSettings(idProvider, View.CurrentAttributeToAdd, idAttribute, item.Id, true));
            else
                View.DisplayAttributeOptionNotAdded();
        }
    }
}