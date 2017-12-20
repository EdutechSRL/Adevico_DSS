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
    public class UrlWizardPresenter : lm.Comol.Core.DomainModel.Common.DomainPresenter
    {
         #region "Initialize"
            private int _ModuleID;
            private lm.Comol.Core.BaseModules.PolicyManagement.Business.PolicyService _PolicyService;
            private UrlAuthenticationService _UrlService;
            private InternalAuthenticationService _InternalService;
            private ProfileManagementService _Service;
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
            protected virtual IViewUserProfileWizardUrl View
            {
                get { return (IViewUserProfileWizardUrl)base.View; }
            }
            private UrlAuthenticationService UrlService
            {
                get
                {
                    if (_UrlService == null)
                        _UrlService = new UrlAuthenticationService(AppContext);
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
            public UrlWizardPresenter(iApplicationContext oContext)
                : base(oContext)
            {
                this.CurrentManager = new BaseModuleManager(oContext);
            }
            public UrlWizardPresenter(iApplicationContext oContext, IViewUserProfileWizardUrl view)
                : base(oContext, view)
            {
                this.CurrentManager = new BaseModuleManager(oContext);
            }
        #endregion

       public void InitView(String urlIdentifier, String urlIdentifierValue)
        {
            View.UrlIdentifier = urlIdentifier;
            View.UrlIdentifierValue = urlIdentifierValue;
            UrlAuthenticationProvider provider = UrlService.GetActivePovider(urlIdentifier);

            if (View.isSystemOutOfOrder)
                View.DisplaySystemOutOfOrder();
            else if (String.IsNullOrEmpty(urlIdentifier) || provider == null)
                View.GotoDefaultPage();
            else if (String.IsNullOrEmpty(urlIdentifierValue)) {
                if (String.IsNullOrEmpty(provider.RemoteLoginUrl))
                    View.GotoDefaultPage();
                else
                    View.GotoRemoteLogonPage(provider.RemoteLoginUrl);
            }
            else
            {
                List<ProfileWizardStep> steps = Service.GetStandardProfileWizardStep(WizardType.UrlProvider);
                View.idProvider = View.PreloadedIdProvider;
                View.AvailableSteps = steps;
                List<AuthenticationProviderType> providers = new List<AuthenticationProviderType>();
                providers.Add(AuthenticationProviderType.Internal);
                providers.Add(AuthenticationProviderType.Url);
                View.ExternalUserInfo = GetExternalUserInfo();
                View.InitializeUnknownProfileStep(AuthenticationProviderType.Url, providers);
                View.InitializeStep(ProfileWizardStep.OrganizationSelector);
                View.GotoStep(ProfileWizardStep.UnknownProfileDisclaimer);
            }
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
                    List<ProfilerError> errors = UrlService.VerifyProfileInfo(View.ProfileInfo);
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
                        UrlAuthenticationProvider provider = GetProvider();
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
                    if (View.AvailableProfileTypes.Count == 1 && View.SelectedProfileTypeId > 0)
                        MoveToPreviousStep(ProfileWizardStep.OrganizationSelector);
                    else
                        View.GotoStep(ProfileWizardStep.ProfileTypeSelector);
                    break;
                case ProfileWizardStep.Privacy:
                    if (View.SelectedProvider== AuthenticationProviderType.Internal)
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
                if (View.AvailableOrganizationsId.Count <= 1 && View.SelectedOrganizationId > 0)
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
                UrlAuthenticationProvider provider = GetProvider();
                if (provider == null)
                    View.DisplayInternalCredentialsMessage(ProfileSubscriptionMessage.ProviderUnknown);
                else {

                    ExternalLoginInfo account =  UrlService.AddFromInternalAccount(info, provider,View.UrlIdentifierValue);
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
        private void MoveFromStepOrganizationSelector(){
            View.InitializeProfileTypeSelector(new List<Int32>(), 0);
                                
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
        private void MoveFromStepProfileTypeSelector(){
            UrlAuthenticationProvider provider = GetProvider();
            dtoBaseProfile profile = new dtoBaseProfile();
            if (provider != null)
               profile.IdProfileType = View.SelectedProfileTypeId;
            if (View.SelectedProfileTypeId > 0)
                View.GotoStepProfileInfo(profile);
         }
        public void CreateProfile(dtoBaseProfile profile, Int32 idProfileType, String ProfileName, Int32 idOrganization)
        {
            UrlAuthenticationProvider provider = GetProvider();
            List<ProfilerError> errors = UrlService.VerifyProfileInfo(profile, View.idProvider ,View.ExternalCredentials);
            if (errors.Count == 0)
            {
                ProfileSubscriptionMessage message = View.CreateProfile(profile, idProfileType, ProfileName, idOrganization, AuthenticationProviderType.Url,View.idProvider);
                Int32 idPerson = View.IdProfile;
                if (idPerson>0)
                    PolicyService.SaveUserSelection(CurrentManager.GetPerson(idPerson), View.GetPolicyInfo);

                if (message == ProfileSubscriptionMessage.CreatedWithAutoLogon && idPerson > 0){
                    Person person = CurrentManager.GetPerson(View.IdProfile);
                    View.LogonUser(person, View.idProvider, provider.RemoteLoginUrl,false, CurrentManager.GetUserDefaultIdOrganization(View.IdProfile));
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
            if (provider== AuthenticationProviderType.Internal)
            {
                UpdateStepsToSkip(ProfileWizardStep.InternalCredentials, false);
                UpdateStepsToSkip(ProfileWizardStep.OrganizationSelector, true);
                UpdateStepsToSkip(ProfileWizardStep.ProfileTypeSelector, true);
                UpdateStepsToSkip(ProfileWizardStep.ProfileUserData, true);
                UpdateStepsToSkip(ProfileWizardStep.Summary, true);
                UpdateStepsToSkip(ProfileWizardStep.WaitingLogon, false);
            }
            else {
                UpdateStepsToSkip(ProfileWizardStep.InternalCredentials, true);
                UpdateStepsToSkip(ProfileWizardStep.OrganizationSelector, false);
                UpdateStepsToSkip(ProfileWizardStep.ProfileTypeSelector, false);
                UpdateStepsToSkip(ProfileWizardStep.ProfileUserData, false);
                UpdateStepsToSkip(ProfileWizardStep.Summary, false);
                UpdateStepsToSkip(ProfileWizardStep.WaitingLogon, true);
            }
        }

        private void UpdateStepsToSkip(ProfileWizardStep step, Boolean add) {
            List<ProfileWizardStep> toSkip = View.SkipSteps;

            if (add && !toSkip.Contains(step))
                toSkip.Add(step);
            else if (!add && toSkip.Contains(step))
                toSkip.Remove(step);
            View.SkipSteps = toSkip;
        }

        private UrlAuthenticationProvider GetProvider()
        {
            UrlAuthenticationProvider provider = UrlService.GetActivePovider(View.UrlIdentifier);
            if (provider!=null && provider.Id !=View.idProvider)
                View.idProvider = provider.Id;

            return provider;
        }
        private String GetExternalUserInfo()
        {
            String result = "";
            UrlAuthenticationProvider provider = GetProvider();
            if (provider != null)
                result = View.UrlIdentifierValue;
            return result;
        }
        //public String GetRemoteUserIdentifier() { 
        //    ShibbolethAuthenticationProvider provider = GetProvider();
        //    if (provider != null)
        //    {
        //        Dictionary<AttributeType, String> values = View.GetShibbolethValues(provider.Attributes.Where(a => a.Deleted == BaseStatusDeleted.None).ToList());

        //        if (values.ContainsKey(AttributeType.externalId) && !String.IsNullOrEmpty(values[AttributeType.externalId]))
        //            return values[AttributeType.externalId];
        //    }
        //        return "";
        //}
    }
}
