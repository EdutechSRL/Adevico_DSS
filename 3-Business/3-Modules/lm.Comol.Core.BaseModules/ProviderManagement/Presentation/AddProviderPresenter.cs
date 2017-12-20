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
    public class AddProviderPresenter : lm.Comol.Core.DomainModel.Common.DomainPresenter
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
            protected virtual IViewAddProvider View
            {
                get { return (IViewAddProvider)base.View; }
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
            public AddProviderPresenter(iApplicationContext oContext)
                : base(oContext)
            {
                this.CurrentManager = new BaseModuleManager(oContext);
            }
            public AddProviderPresenter(iApplicationContext oContext, IViewAddProvider view)
                : base(oContext, view)
            {
                this.CurrentManager = new BaseModuleManager(oContext);
            }
        #endregion

        public void InitView()
        {
            View.IdProvider = 0;
            if (UserContext.isAnonymous)
                View.DisplaySessionTimeout();
            else
            {
                ModuleProviderManagement module = ModuleProviderManagement.CreatePortalmodule(UserContext.UserTypeID);
                View.AllowManagement = (module.Administration || module.ViewProviders) ;
                
                if (module.AddProvider || module.Administration)
                {
                    View.AvailableSteps = GetAvailableSteps();
                    View.LoadAvailableTypes(GetAuthenticationProviderTypes());
                    View.GotoStep(ProviderWizardStep.SelectType);
                }
                else
                    View.DisplayNoPermission();
            }
        }

        public void MoveToNextStep(ProviderWizardStep step)
        {
            switch (step)
            {
                case ProviderWizardStep.SelectType:
                    if (View.ProviderInfoType != View.SelectedType)
                        MoveToProviderInfo(View.SelectedType);
                    else
                        View.GotoStep(ProviderWizardStep.ProviderData);
                    break;
                case ProviderWizardStep.ProviderData:
                    if (View.ValidateProviderInfo())
                        MoveToDefaultTranslation(View.IdentifierFields);
                    break;
                case ProviderWizardStep.DefaultTranslation:
                    if (View.ValidateDefaultTranslation())
                        MoveToSummary();
                    break;
            }
        }
        public void MoveToPreviousStep(ProviderWizardStep step)
        {
            switch (step)
            {
                case ProviderWizardStep.ProviderData:
                    View.GotoStep(ProviderWizardStep.SelectType);
                    break;
                case ProviderWizardStep.DefaultTranslation:
                    View.GotoStep(ProviderWizardStep.ProviderData);
                    break;
                case ProviderWizardStep.Summary:
                    View.GotoStep(ProviderWizardStep.DefaultTranslation);
                    break;
                case ProviderWizardStep.ErrorMessages:
                    View.GotoStep(ProviderWizardStep.DefaultTranslation);
                    break;
            }
        }

        private void MoveToProviderInfo(AuthenticationProviderType type) { 
            switch(type){
                case AuthenticationProviderType.Internal:
                    dtoInternalProvider internalProvider = new dtoInternalProvider();
                    internalProvider.IdentifierFields = IdentifierField.none;
                    internalProvider.ChangePasswordAfterDays= 180;
                    internalProvider.AllowAdminProfileInsert = true;
                    internalProvider.DisplayToUser = true;
                    internalProvider.AllowMultipleInsert = false;
                    View.LoadProviderInfo(internalProvider, false);
                    break;
                case AuthenticationProviderType.Url:
                    dtoUrlProvider urlProvider = new dtoUrlProvider();
                    urlProvider.IdentifierFields = IdentifierField.stringField;
                    urlProvider.VerifyRemoteUrl = false;
                    urlProvider.AllowAdminProfileInsert = true;
                    urlProvider.DisplayToUser = false;
                    urlProvider.AllowMultipleInsert = true;
                    urlProvider.TokenFormat = UrlUserTokenFormat.LoginDateTime;
                    urlProvider.EncryptionInfo.EncryptionAlgorithm = Authentication.Helpers.EncryptionAlgorithm.Rijndael;
                    View.LoadProviderInfo(urlProvider, false);
                    break;
                    
                  case AuthenticationProviderType.UrlMacProvider:
                    dtoMacUrlProvider macProvider = new dtoMacUrlProvider();
                    macProvider.IdentifierFields = IdentifierField.stringField;
                    macProvider.VerifyRemoteUrl = false;
                    macProvider.AllowAdminProfileInsert = true;
                    macProvider.DisplayToUser = false;
                    macProvider.AllowMultipleInsert = true;
                    macProvider.AutoAddAgency = true;
                    macProvider.AutoEnroll = true;
                    macProvider.AllowTaxCodeDuplication = true;
                    macProvider.EncryptionInfo.EncryptionAlgorithm = Authentication.Helpers.EncryptionAlgorithm.Md5;
                    macProvider.AllowRequestFromIpAddresses = "";
                    macProvider.DenyRequestFromIpAddresses = "";
                    View.LoadProviderInfo(macProvider, false);
                    break;

                default:
                    dtoProvider provider = new dtoProvider();
                    provider.AllowAdminProfileInsert = true;
                    provider.DisplayToUser = true;
                    provider.AllowMultipleInsert = false;
                    provider.IdentifierFields = IdentifierField.stringField;
                    View.LoadProviderInfo(provider, false);
                    break;

            }
            View.GotoStep(ProviderWizardStep.ProviderData);
        }
        private void MoveToDefaultTranslation(IdentifierField fields)
        {
            if (View.isTranslationInitialized){
                View.UpdateTranslationView(fields);
                View.GotoStep(ProviderWizardStep.DefaultTranslation);
            }
            else{
                Language language = CurrentManager.GetDefaultLanguage();
                if (language != null)
                {
                    View.InitializeTranslation(0, language.Id, fields);
                    View.GotoStep(ProviderWizardStep.DefaultTranslation);
                }
            }
        }
        private void MoveToSummary()
        {
            View.LoadSummaryInfo(View.ProviderInfo, View.SelectedType);
            View.GotoStep(ProviderWizardStep.Summary);
        }

        public void AddProvider(dtoProvider dtoProvider, dtoProviderTranslation translation) {
            AuthenticationProvider provider = Service.AddProvider(dtoProvider, translation);
            if (provider == null)
                View.DisplayErrorSaving();
            else {
                View.IdProvider = provider.Id;
                if (provider.ProviderType == AuthenticationProviderType.None || provider.ProviderType == AuthenticationProviderType.Internal)
                    View.GotoManagement(provider.Id);
                else
                    View.GotoSettings(provider.Id,provider.ProviderType);
            }
        }
        private List<ProviderWizardStep> GetAvailableSteps()
        {
            List<ProviderWizardStep> list = new List<ProviderWizardStep>();
            list.Add(ProviderWizardStep.SelectType);
            list.Add(ProviderWizardStep.ProviderData);
            list.Add(ProviderWizardStep.DefaultTranslation);
            list.Add(ProviderWizardStep.Summary);
            return list;
        }
        private List<AuthenticationProviderType> GetAuthenticationProviderTypes()
        {
            List<AuthenticationProviderType> list = new List<AuthenticationProviderType>();
            if (!Service.ProviderTypeExist(AuthenticationProviderType.Internal))
                list.Add(AuthenticationProviderType.Internal);
            list.Add(AuthenticationProviderType.Url);
            list.Add(AuthenticationProviderType.UrlMacProvider);
            return list;
        }
    }
}