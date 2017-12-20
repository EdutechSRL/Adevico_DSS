using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.Business;
using lm.Comol.Core.Authentication.Business;
using lm.Comol.Core.DomainModel;
using lm.Comol.Core.BaseModules.ProfileManagement.Business;
using lm.Comol.Core.Authentication;

namespace lm.Comol.Core.BaseModules.ProfileManagement.Presentation
{
    public class MacUrlWizardPresenter : lm.Comol.Core.DomainModel.Common.DomainPresenter
    {
         #region "Initialize"
            private int _ModuleID;
            private lm.Comol.Core.BaseModules.PolicyManagement.Business.PolicyService _PolicyService;
            private UrlMacAuthenticationService _UrlService;
            private InternalAuthenticationService _InternalService;
            private ProfileManagementService _Service;
            private lm.Comol.Core.BaseModules.AuthenticationManagement.Business.MacProviderHelper _Helper;

            public virtual BaseModuleManager CurrentManager { get; set; }
            protected virtual IViewUserProfileWizardMacUrl View
            {
                get { return (IViewUserProfileWizardMacUrl)base.View; }
            }
            private UrlMacAuthenticationService UrlService
            {
                get
                {
                    if (_UrlService == null)
                        _UrlService = new UrlMacAuthenticationService(AppContext);
                    return _UrlService;
                }
            }
            private InternalAuthenticationService InternalService
            {
                get
                {
                    if (_InternalService == null)
                        _InternalService = new InternalAuthenticationService(AppContext);
                    return _InternalService;
                }
            }

            private lm.Comol.Core.BaseModules.PolicyManagement.Business.PolicyService PolicyService
                {
                    get
                    {
                        if (_PolicyService == null)
                            _PolicyService = new lm.Comol.Core.BaseModules.PolicyManagement.Business.PolicyService(AppContext);
                        return _PolicyService;
                    }
                }
        
            private ProfileManagementService Service
            {
                get
                {
                    if (_Service == null)
                        _Service = new ProfileManagementService(AppContext);
                    return _Service;
                }
            }
            private lm.Comol.Core.BaseModules.AuthenticationManagement.Business.MacProviderHelper Helper
            {
                get
                {
                    if (_Helper == null)
                        _Helper = new lm.Comol.Core.BaseModules.AuthenticationManagement.Business.MacProviderHelper(CurrentManager, UrlService, Service);
                    return _Helper;
                }
            }
            public MacUrlWizardPresenter(iApplicationContext oContext)
                : base(oContext)
            {
                this.CurrentManager = new BaseModuleManager(oContext);
            }
            public MacUrlWizardPresenter(iApplicationContext oContext, IViewUserProfileWizardMacUrl view)
                : base(oContext, view)
            {
                this.CurrentManager = new BaseModuleManager(oContext);
            }
        #endregion

       public void InitView(String internalMac, String userIdentifier, long idProvider)
        {
            View.PostInternalMac = internalMac;
            View.PostUserIdentifier = userIdentifier;
            MacUrlAuthenticationProvider provider = UrlService.GetProvider(idProvider);

            if (View.isSystemOutOfOrder)
                View.DisplaySystemOutOfOrder();
            else if (provider == null)
                View.GotoDefaultPage();
            else {
                List<dtoMacUrlUserAttribute> attributes = View.GetTokenAttributes(provider.GetUserAttributes());
                View.TokenAttributes = attributes;
                if (!provider.IsInternalToken(internalMac, attributes) || String.IsNullOrEmpty(internalMac) || String.IsNullOrEmpty(userIdentifier))
                {
                    if (String.IsNullOrEmpty(provider.RemoteLoginUrl))
                        View.GotoDefaultPage();
                    else
                        View.GotoRemoteLogonPage(provider.RemoteLoginUrl);
                }
                else {
                    List<ProfileWizardStep> steps = GetAvailableSteps(provider, attributes);
                    View.idProvider = View.PreloadedIdProvider;
                    View.AvailableSteps = steps;
                    List<AuthenticationProviderType> providers = new List<AuthenticationProviderType>();
                    providers.Add(AuthenticationProviderType.Internal);
                    providers.Add(AuthenticationProviderType.UrlMacProvider);
                    View.InitializeUnknownProfileStep(AuthenticationProviderType.UrlMacProvider, providers);
                    View.GotoStep(ProfileWizardStep.UnknownProfileDisclaimer);
                }
            }
        }
        private List<ProfileWizardStep> GetAvailableSteps(MacUrlAuthenticationProvider provider,List<dtoMacUrlUserAttribute> attributes)
        {
            List<ProfileWizardStep> steps = Service.GetStandardProfileWizardStep(WizardType.MacUrl);
            List<OrganizationAttributeItem> orgInfos = provider.GetOrganizationsInfo(attributes);

            View.InitializeStep(ProfileWizardStep.OrganizationSelector);

            if (orgInfos != null && orgInfos.Any())
            {
                View.TokenIdOrganization = orgInfos[0].Organization.Id;
                View.TokenIdProfileType= orgInfos[0].IdDefaultProfile;
                View.SelectedOrganizationId = orgInfos[0].Organization.Id;
                steps.Remove(ProfileWizardStep.OrganizationSelector);
                steps.Remove(ProfileWizardStep.ProfileTypeSelector);
            }
           return steps;
       }
        public void MoveToNextStep(ProfileWizardStep step){
            switch (step) {
                case ProfileWizardStep.UnknownProfileDisclaimer:
                    MoveFromStepUnknownProfileDisclaimer();
                    break;
                case ProfileWizardStep.InternalCredentials:
                    MoveFromStepInternalCredentials();
                    break;
                case ProfileWizardStep.OrganizationSelector:
                    MoveFromStepOrganizationSelector();
                    break;
                case ProfileWizardStep.ProfileTypeSelector:
                    MoveFromStepProfileTypeSelector();
                    break;
                case ProfileWizardStep.ProfileUserData:
                    List<ProfilerError> errors = UrlService.VerifyProfileInfo(View.ProfileInfo,View.idProvider);
                    if (errors.Count > 0)
                        View.LoadProfileInfoError(errors);
                    else
                    {
                        View.UnloadProfileInfoError();
                        if (!View.AvailableSteps.Contains(ProfileWizardStep.Privacy))
                        {
                            UpdateStepsToSkip(ProfileWizardStep.Privacy, true);
                            View.GotoStep(ProfileWizardStep.Summary);
                        }
                        else
                        {
                            if (!View.IsInitialized(ProfileWizardStep.Privacy))
                                View.InitializeStep(ProfileWizardStep.Privacy);
                            View.GotoStep(ProfileWizardStep.Privacy);
                        }
                    }
                    break;
                case ProfileWizardStep.Privacy:
                    Boolean UseInternalCredentials = (View.SelectedProvider == AuthenticationProviderType.Internal);
                    if (View.AcceptedMandatoryPolicy && UseInternalCredentials && View.IdProfile > 0)
                    {
                        Person person = CurrentManager.GetPerson(View.IdProfile);
                        MacUrlAuthenticationProvider provider = GetProvider();
                        View.LogonUser(person, View.idProvider, provider.RemoteLoginUrl, false, CurrentManager.GetUserDefaultIdOrganization(View.IdProfile));
                    }
                    else if (View.AcceptedMandatoryPolicy && !UseInternalCredentials)
                        View.GotoStep(ProfileWizardStep.Summary);
                    break;
                case ProfileWizardStep.Summary:
                    break;
            }
        }
        public void MoveToPreviousStep(ProfileWizardStep step)
        {
            switch (step)
            {
                case ProfileWizardStep.InternalCredentials:
                    View.GotoStep(ProfileWizardStep.UnknownProfileDisclaimer);
                    break;
                case ProfileWizardStep.OrganizationSelector:
                    View.GotoStep(ProfileWizardStep.UnknownProfileDisclaimer);
                    break;
                case ProfileWizardStep.ProfileTypeSelector:
                    if (View.AvailableOrganizationsId.Count == 1 && View.SelectedOrganizationId > 0)
                        MoveToPreviousStep(ProfileWizardStep.OrganizationSelector);
                    else
                        View.GotoStep(ProfileWizardStep.OrganizationSelector);
                    break;
                case ProfileWizardStep.ProfileUserData:
                    if (View.TokenIdOrganization>0 && View.TokenIdProfileType>0)
                        View.GotoStep(ProfileWizardStep.UnknownProfileDisclaimer);
                    if (View.TokenIdProfileType>0 || (View.AvailableProfileTypes.Count == 1 && View.SelectedProfileTypeId > 0))
                        MoveToPreviousStep(ProfileWizardStep.OrganizationSelector);
                    else
                        View.GotoStep(ProfileWizardStep.ProfileTypeSelector);
                    break;
                case ProfileWizardStep.Privacy:
                    if (View.SelectedProvider == AuthenticationProviderType.Internal)
                        View.GotoStep(ProfileWizardStep.UnknownProfileDisclaimer);
                    else
                        View.GotoStep(ProfileWizardStep.ProfileUserData);
                    break;
                case ProfileWizardStep.Summary:
                    if (View.AvailableSteps.Contains(ProfileWizardStep.Privacy))
                        View.GotoStep(ProfileWizardStep.Privacy);
                    else
                        View.GotoStep(ProfileWizardStep.ProfileUserData);
                    break;
            }
        }

        private void MoveFromStepUnknownProfileDisclaimer(){
            AuthenticationProviderType sProvider = View.SelectedProvider;
            UpdateStepsToSkipForInternal(sProvider);
            if (sProvider == AuthenticationProviderType.Internal)
                View.GotoStep(ProfileWizardStep.InternalCredentials);
            else
            {
                if (View.TokenIdOrganization > 0 && View.TokenIdProfileType > 0)
                    MoveToNextStep(ProfileWizardStep.ProfileTypeSelector);
                else if (View.TokenIdOrganization > 0)
                    MoveToNextStep(ProfileWizardStep.OrganizationSelector);
                else if (View.AvailableOrganizationsId.Count <= 1 && View.SelectedOrganizationId > 0)
                {
                    UpdateStepsToSkip(ProfileWizardStep.OrganizationSelector, true);        
                    MoveToNextStep(ProfileWizardStep.OrganizationSelector);
                }
                else
                {
                    View.GotoStep(ProfileWizardStep.OrganizationSelector);
                    UpdateStepsToSkip(ProfileWizardStep.OrganizationSelector, false);
                }
            }
        }
        private void MoveFromStepInternalCredentials(){
            dtoInternalCredentials credentials = View.GetInternalCredentials;
            InternalLoginInfo info = InternalService.GetAuthenticatedUser(credentials.Login, credentials.Password);
            if (info == null || info.Person == null)
                View.DisplayInvalidCredentials();
            else{
                MacUrlAuthenticationProvider provider = GetProvider();
                if (provider == null)
                    View.DisplayInternalCredentialsMessage(ProfileSubscriptionMessage.ProviderUnknown);
                else
                {
                    ExternalLoginInfo account = UrlService.AddFromInternalAccount(info, provider, View.PostUserIdentifier);
                    if (account == null)
                        View.DisplayInternalCredentialsMessage(ProfileSubscriptionMessage.UnableToConnectToInternalProvider);
                    else if (account != null && account.Person.isDisabled)
                        View.LoadRegistrationMessage(ProfileSubscriptionMessage.AccountDisabled);
                    else if (PolicyService.UserHasPolicyToAccept(account.Person))
                    {
                        View.IdProfile = account.Person.Id;
                        InternalService.UpdateUserAccessTime(account.Person);
                        View.DisplayPrivacyPolicy(account.Person.Id, provider.Id, provider.RemoteLoginUrl, false);
                    }
                    else
                        View.LogonUser(account.Person, View.idProvider, provider.RemoteLoginUrl, false, CurrentManager.GetUserDefaultIdOrganization(account.Person.Id));
                }
            }
        }
        private void MoveFromStepOrganizationSelector()
        {
            View.InitializeProfileTypeSelector(new List<Int32>(), 0);
            if (View.TokenIdProfileType > 0)
                MoveFromStepProfileTypeSelector();
            if (View.AvailableProfileTypes.Count == 1 && View.SelectedProfileTypeId > 0)
            {
                UpdateStepsToSkip(ProfileWizardStep.ProfileTypeSelector, true);
                MoveToNextStep(ProfileWizardStep.ProfileTypeSelector);
            }
            else if (View.SelectedOrganizationId > 0)
            {
                UpdateStepsToSkip(ProfileWizardStep.ProfileTypeSelector, false);
                View.GotoStep(ProfileWizardStep.ProfileTypeSelector);
            }
        }
        private void MoveFromStepProfileTypeSelector()
        {
            Int32 tokenIdType = View.TokenIdProfileType;
            Int32 idOrganization = View.TokenIdOrganization;
            if (idOrganization==0)
                idOrganization = View.SelectedOrganizationId;
            MacUrlAuthenticationProvider provider = GetProvider();
            dtoBaseProfile profile = new dtoBaseProfile();

            if (tokenIdType > 0)
                profile.IdProfileType = tokenIdType;
            else if (provider != null)
                profile.IdProfileType = View.SelectedProfileTypeId;

            if (profile.IdProfileType>0){
                profile = Helper.GetProfileData(provider, provider.GetProfileAttributes(), View.TokenAttributes, idOrganization, profile.IdProfileType);
                View.GotoStepProfileInfo(profile);
                View.DisableInput(provider.GetNotEditableAttributes(View.TokenAttributes));
            }
        }
        public void CreateProfile(dtoBaseProfile profile, Int32 idProfileType, String ProfileName, Int32 idOrganization)
        {
            MacUrlAuthenticationProvider provider = GetProvider();
            List<ProfilerError> errors = UrlService.VerifyProfileInfo(profile, View.idProvider, View.ExternalCredentials);
            if (errors.Count == 0)
            {
                ProfileSubscriptionMessage message = View.CreateProfile(profile, idProfileType, ProfileName, idOrganization, AuthenticationProviderType.UrlMacProvider, View.idProvider);
                Int32 idPerson = View.IdProfile;
                if (idPerson > 0)
                    PolicyService.SaveUserSelection(CurrentManager.GetPerson(idPerson), View.GetPolicyInfo);

                if (message == ProfileSubscriptionMessage.CreatedWithAutoLogon && idPerson > 0)
                {
                    Person person = CurrentManager.GetPerson(idPerson);
                    UrlService.UpdateCatalogueAssocation(idPerson, provider, View.TokenAttributes);
                    View.LogonUser(person, View.idProvider, provider.RemoteLoginUrl, false, CurrentManager.GetUserDefaultIdOrganization(idPerson));
                }
                else
                {
                    if (message == ProfileSubscriptionMessage.CreatedWithAutoLogon)
                        message = ProfileSubscriptionMessage.Created;
                    View.LoadRegistrationMessage(message);
                }
            }
            else
            {
                if (errors.Contains(ProfilerError.loginduplicate))
                    View.LoadRegistrationMessage(ProfileSubscriptionMessage.LoginDuplicated);
                else if (errors.Contains(ProfilerError.mailDuplicate))
                    View.LoadRegistrationMessage(ProfileSubscriptionMessage.MailDuplicated);
                else if (errors.Contains(ProfilerError.taxCodeDuplicate))
                    View.LoadRegistrationMessage(ProfileSubscriptionMessage.TaxCodeDuplicated);
                else if (errors.Contains(ProfilerError.uniqueIDduplicate))
                    View.LoadRegistrationMessage(ProfileSubscriptionMessage.MatriculaDuplicated);
                else if (errors.Contains(ProfilerError.externalUniqueIDduplicate))
                    View.LoadRegistrationMessage(ProfileSubscriptionMessage.externalUniqueIDduplicate);
                else
                    View.LoadRegistrationMessage(ProfileSubscriptionMessage.UnknownError);
            }
        }

        private void UpdateStepsToSkipForInternal(AuthenticationProviderType provider)
        {
            List<ProfileWizardStep> available = View.AvailableSteps;
            if (provider == AuthenticationProviderType.Internal)
            {
                UpdateStepsToSkip(ProfileWizardStep.InternalCredentials, false);
                if(available.Contains(ProfileWizardStep.OrganizationSelector))
                    UpdateStepsToSkip(ProfileWizardStep.OrganizationSelector, true);
                if (available.Contains(ProfileWizardStep.ProfileTypeSelector))
                    UpdateStepsToSkip(ProfileWizardStep.ProfileTypeSelector, true);
                UpdateStepsToSkip(ProfileWizardStep.ProfileUserData, true);
                UpdateStepsToSkip(ProfileWizardStep.Summary, true);
                UpdateStepsToSkip(ProfileWizardStep.WaitingLogon, false);
            }
            else
            {
                UpdateStepsToSkip(ProfileWizardStep.InternalCredentials, true);
                if (available.Contains(ProfileWizardStep.OrganizationSelector))
                    UpdateStepsToSkip(ProfileWizardStep.OrganizationSelector, false);
                if (available.Contains(ProfileWizardStep.ProfileTypeSelector))
                    UpdateStepsToSkip(ProfileWizardStep.ProfileTypeSelector, false);
                UpdateStepsToSkip(ProfileWizardStep.ProfileUserData, false);
                UpdateStepsToSkip(ProfileWizardStep.Summary, false);
                UpdateStepsToSkip(ProfileWizardStep.WaitingLogon, true);
            }
        }
        private void UpdateStepsToSkip(ProfileWizardStep step, Boolean add)
        {
            List<ProfileWizardStep> toSkip = View.SkipSteps;

            if (add && !toSkip.Contains(step))
                toSkip.Add(step);
            else if (!add && toSkip.Contains(step))
                toSkip.Remove(step);
            View.SkipSteps = toSkip;
        }
        private MacUrlAuthenticationProvider GetProvider()
        {
            MacUrlAuthenticationProvider provider = UrlService.GetProvider(View.idProvider);
            if (provider != null && provider.Id != View.idProvider)
                View.idProvider = provider.Id;

            return provider;
        }

        #region "Default profile data"
        
        #endregion
    }
}