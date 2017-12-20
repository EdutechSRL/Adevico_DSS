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
    public class EditMacAttributePresenter : lm.Comol.Core.DomainModel.Common.DomainPresenter
    {
         #region "Initialize"
            private int _ModuleID;
            private ProviderManagementService _Service;
            private lm.Comol.Core.BaseModules.ProfileManagement.Business.ProfileManagementService _ProfileService;
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
            protected virtual IViewEditMacAttribute View
            {
                get { return (IViewEditMacAttribute)base.View; }
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
            private lm.Comol.Core.BaseModules.ProfileManagement.Business.ProfileManagementService ProfileService
            {
                get
                {
                    if (_ProfileService == null)
                        _ProfileService = new lm.Comol.Core.BaseModules.ProfileManagement.Business.ProfileManagementService(AppContext);
                    return _ProfileService;
                }
            }
        
            public EditMacAttributePresenter(iApplicationContext oContext)
                : base(oContext)
            {
                this.CurrentManager = new BaseModuleManager(oContext);
            }
            public EditMacAttributePresenter(iApplicationContext oContext, IViewEditMacAttribute view)
                : base(oContext, view)
            {
                this.CurrentManager = new BaseModuleManager(oContext);
            }
        #endregion

        public void InitView(long idProvider, BaseUrlMacAttribute attribute,Boolean allowSave, Boolean taxCodeRequired)
        {
            InitView(idProvider, attribute, allowSave, taxCodeRequired, false);
        }
        public void InitView(long idProvider, BaseUrlMacAttribute attribute)
        {
            View.IdProvider = idProvider;
            InitView(idProvider, attribute, false, false, true);
        }

        private void InitView(long idProvider, BaseUrlMacAttribute attribute, Boolean allowSave, Boolean taxCodeRequired, Boolean displayMode)
        {
            View.IdProvider = idProvider;
            View.IdAttribute = 0;
            View.DisplayMode = displayMode;
            if (UserContext.isAnonymous)
            {
                View.DisplaySessionTimeout();
                View.AllowSave = false;
            }
            else if (attribute == null)
            {
                View.DisplayAttributeUnknown();
                View.AllowSave = false;
            }
            else
            {
                View.IdAttribute = attribute.Id;
                View.AllowSave = allowSave;
                if (!displayMode)
                {
                    if (attribute.GetType() == typeof(UserProfileAttribute))
                        LoadUserProfileAttribute(idProvider, ((UserProfileAttribute)attribute).Attribute, taxCodeRequired);
                    else if (attribute.GetType() == typeof(CompositeProfileAttribute))
                        LoadUserProfileAttribute(idProvider, ((CompositeProfileAttribute)attribute).Attribute, taxCodeRequired);
                    else if (attribute.GetType() == typeof(OrganizationAttribute))
                    {
                        View.LoadSystemProfileTypes();
                        View.AvailablePages = Service.GetAvailableModulePages(UserContext.Language.Id);
                    }
                }

                View.LoadAttribute(attribute, allowSave);
            }
        }
        private void LoadUserProfileAttribute(long idProvider, ProfileAttributeType aType, Boolean taxCodeRequired)
        {
            List<ProfileAttributeType> usedTypes = Service.GetUsedMacProfileAttribute(idProvider);
            usedTypes.Remove(aType);
            usedTypes.Remove(ProfileAttributeType.skip);
            List<lm.Comol.Core.BaseModules.ProfileManagement.dtoProfileAttributeType> items = ProfileService.GetAllProfileAttributes(taxCodeRequired);
            View.LoadAvailableProfileAttributes(items.Where(a=> !usedTypes.Contains(a.Attribute)).ToList());
        }

        public List<BaseUrlMacAttribute> GetAvailableAttributesForComposite(long idProvider, long idAttribute)
        {
            return Service.GetAvailableAttributesForComposite(idProvider, idAttribute);
        }
        public List<BaseUrlMacAttribute> GetAvailableAttributesForMac(long idProvider, long idMacAttribute) {
            return Service.GetAvailableAttributesForMac(idProvider, idMacAttribute);
        }
        public Dictionary<Int32,String> GetAvailableOrganizations(long idProvider, long idAttribute)
        {
            return Service.GetAvailableItemsForOrganizationAttribute(idProvider, idAttribute);
        }
        public Dictionary<long, String> GetAvailableCatalogues(long idProvider, long idAttribute)
        {
            return Service.GetAvailableItemsForCatalogueAttribute(idProvider, idAttribute, UserContext.Language.Id);
        }

        
        public Boolean isValidRemoteCode(Int32 idOrganization,String remoteCode) {
            return Service.isUniqueOrganizationCode(View.IdAttribute,idOrganization, remoteCode);
        }
        public Boolean isValidCatalogueRemoteCode(long idCatalogue, String remoteCode)
        {
            return Service.isUniqueCatalogueCode(View.IdAttribute, idCatalogue, remoteCode);
        }
        public BaseUrlMacAttribute SaveSettings(long idProvider, long idAttribute,BaseUrlMacAttribute dto, ref Boolean validCodes) {
            return Service.SaveProviderAttribute(idProvider, idAttribute, dto, ref validCodes);
        }
    }
}