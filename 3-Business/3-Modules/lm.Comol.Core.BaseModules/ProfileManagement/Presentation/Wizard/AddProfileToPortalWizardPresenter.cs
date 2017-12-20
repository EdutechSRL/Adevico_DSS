using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.Business;
using lm.Comol.Core.Authentication.Business;
using lm.Comol.Core.DomainModel;
using lm.Comol.Core.BaseModules.ProfileManagement.Business;
using lm.Comol.Core.Authentication;
using lm.Comol.Core.BaseModules.ProviderManagement;

namespace lm.Comol.Core.BaseModules.ProfileManagement.Presentation
{
    public class AddProfileToPortalWizardPresenter : lm.Comol.Core.DomainModel.Common.DomainPresenter
    {
         #region "Initialize"
            private int _ModuleID;
            private lm.Comol.Core.BaseModules.PolicyManagement.Business.PolicyService _PolicyService;
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
            protected virtual IViewAddProfileToPortalWizard View
            {
                get { return (IViewAddProfileToPortalWizard)base.View; }
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
            public AddProfileToPortalWizardPresenter(iApplicationContext oContext)
                : base(oContext)
            {
                this.CurrentManager = new BaseModuleManager(oContext);
            }
            public AddProfileToPortalWizardPresenter(iApplicationContext oContext, IViewAddProfileToPortalWizard view)
                : base(oContext, view)
            {
                this.CurrentManager = new BaseModuleManager(oContext);
            }
        #endregion

        public void InitView()
        {
            ModuleProfileManagement module = ModuleProfileManagement.CreatePortalmodule(UserContext.UserTypeID);
            Boolean hasPermission = (module.Administration || module.AddProfile);
            Int32 IdProfileType = View.PreloadedIdProfileType;
            View.AllowBackTomanagement = hasPermission;
            if (hasPermission && AllowProfileCreation(IdProfileType))
            {
                View.AvailableSteps = Service.GetStandardProfileWizardStep(WizardType.Administration);
                View.InitializeStep(ProfileWizardStep.OrganizationSelector);
                if (IdProfileType == 0)
                    View.GotoStep(ProfileWizardStep.ProfileTypeSelector, true);
                else
                {
                    View.InitializeProfileTypeSelector(IdProfileType);
                    UpdateStepsToSkip(ProfileWizardStep.ProfileTypeSelector, true);
                    MoveToNextStep(ProfileWizardStep.ProfileTypeSelector);
                }
            }
            else
                View.DisplayNoPermission();
        }

        public void MoveToNextStep(ProfileWizardStep step){
            switch (step) {
                case ProfileWizardStep.ProfileTypeSelector:
                    MoveFromStepProfileTypeSelector();
                    break;
                case ProfileWizardStep.OrganizationSelector:
                    MoveFromStepOrganizationSelector();
                    break;
                case ProfileWizardStep.AuthenticationTypeSelector:
                    MoveFromStepAuthenticationTypeSelector();
                    break;
                case ProfileWizardStep.ProfileUserData:
                    MoveFromStepProfileUserData();
                    break;
                case ProfileWizardStep.Summary:
                    break;
            }
        }
        public void MoveToPreviousStep(ProfileWizardStep step)
        {
            switch (step)
            {
                case ProfileWizardStep.OrganizationSelector:
                    Int32 IdProfileType = View.PreloadedIdProfileType;
                    if (IdProfileType == View.SelectedProfileTypeId && IdProfileType != 0) { }
                    else
                        View.GotoStep(ProfileWizardStep.ProfileTypeSelector);
                    break;
                case ProfileWizardStep.AuthenticationTypeSelector:
                    if (View.AvailableOrganizationsId.Count == 1 && View.SelectedOrganizationId > 0)
                        MoveToPreviousStep(ProfileWizardStep.OrganizationSelector);
                    else
                        View.GotoStep(ProfileWizardStep.OrganizationSelector);
                    break;
                case ProfileWizardStep.ProfileUserData:
                    if (View.AvailableProvidersId.Count == 1 && View.SelectedProviderId>0 )
                        MoveToPreviousStep(ProfileWizardStep.AuthenticationTypeSelector);
                    else
                        View.GotoStep(ProfileWizardStep.AuthenticationTypeSelector);
                    break;
                case ProfileWizardStep.Summary:
                    View.GotoStep(ProfileWizardStep.ProfileUserData);
                    break;
            }
        }

        private void MoveFromStepProfileTypeSelector()
        {
            if (View.SelectedProfileTypeId > 0)
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
        private void MoveFromStepOrganizationSelector()
        {
            List<dtoBaseProvider> providers = Service.GetAuthenticationProviders(UserContext.Language.Id, true);

            if (providers != null && providers.Count>0)
            {
                View.InitializeAuthenticationTypeSelectorStep(providers, (from p in providers where p.Type == AuthenticationProviderType.Internal select p.IdProvider).FirstOrDefault());

                if (providers.Count == 1 && View.SelectedProviderId > 0)
                {
                    UpdateStepsToSkip(ProfileWizardStep.AuthenticationTypeSelector, true);
                    MoveToNextStep(ProfileWizardStep.AuthenticationTypeSelector);
                }
                else if (View.SelectedOrganizationId > 0)
                {
                    UpdateStepsToSkip(ProfileWizardStep.AuthenticationTypeSelector, false);
                    View.GotoStep(ProfileWizardStep.AuthenticationTypeSelector);
                }
            }
        }
        private void MoveFromStepAuthenticationTypeSelector()
        {
            if (View.SelectedProviderId > 0)
            {
                View.GotoStep(ProfileWizardStep.ProfileUserData,true);
            }
       }
        private void MoveFromStepProfileUserData()
        {
            View.SaveUserPassword();
            List<ProfilerError> errors = InternalService.VerifyProfileInfo(View.ProfileInfo, View.SelectedProviderId, View.GetExternalCredentials);
            if (errors.Count > 0)
                View.LoadProfileInfoError(errors);
            else
            {
                View.UnloadProfileInfoError();
                View.GotoStep(ProfileWizardStep.Summary);
            }
        }

        public void CreateProfile(dtoBaseProfile profile, Int32 idProfileType, String ProfileName, Int32 idOrganization)
        {
            List<ProfilerError> errors = InternalService.VerifyProfileInfo(profile);
            if (errors.Count == 0)
            {
                ProfileSubscriptionMessage message = View.CreateProfile(profile, idProfileType, ProfileName, idOrganization, View.OtherSelectedOrganizationId, GetAuthenticationProvider(View.SelectedProviderId), View.GetExternalCredentials);
                Int32 idPerson = View.IdProfile;
                Person person = CurrentManager.GetPerson(idPerson);
                if (idPerson > 0 && person != null )
                    View.LoadProfiles(person);
                else
                    View.LoadRegistrationMessage(message);
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

        private void UpdateStepsToSkip(ProfileWizardStep step, Boolean add) {
            List<ProfileWizardStep> toSkip = View.SkipSteps;

            if (add && !toSkip.Contains(step))
                toSkip.Add(step);
            else if (!add && toSkip.Contains(step))
                toSkip.Remove(step);
            View.SkipSteps = toSkip;
        }

        private Boolean AllowProfileCreation(int IdProfileType)
        {
            if (IdProfileType == 0)
                return true;
            else {
                switch (UserContext.UserTypeID) { 
                    case (int)UserTypeStandard.SysAdmin:
                        return true;
                    case (int)UserTypeStandard.Administrator:
                        return (IdProfileType != (int)UserTypeStandard.SysAdmin && IdProfileType != (int)UserTypeStandard.Administrator);
                    case (int)UserTypeStandard.Administrative:
                        return (IdProfileType != (int)UserTypeStandard.SysAdmin && IdProfileType != (int)UserTypeStandard.Administrator );
                    default:
                        return false;
                }
            }
        }

        public dtoBaseProvider GetAuthenticationProvider(long idProvider)
        {
            return Service.GetAuthenticationProvider(UserContext.Language.Id, idProvider);
        }
    }
}