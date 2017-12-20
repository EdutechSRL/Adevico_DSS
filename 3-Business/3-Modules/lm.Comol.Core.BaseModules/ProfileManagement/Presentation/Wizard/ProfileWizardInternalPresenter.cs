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
    public class UserProfileWizardInternalPresenter : lm.Comol.Core.DomainModel.Common.DomainPresenter
    {
         #region "Initialize"
            private int _ModuleID;
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
            protected virtual IViewUserProfileWizardInternal View
            {
                get { return (IViewUserProfileWizardInternal)base.View; }
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
            private ProfileManagementService Service
            {
                get
                {
                    if (_Service == null)
                        _Service = new ProfileManagementService(AppContext);
                    return _Service;
                }
            }
            private lm.Comol.Core.BaseModules.PolicyManagement.Business.PolicyService _PolicyService;
            private lm.Comol.Core.BaseModules.PolicyManagement.Business.PolicyService PolicyService
            {
                get
                {
                    if (_PolicyService == null)
                        _PolicyService = new lm.Comol.Core.BaseModules.PolicyManagement.Business.PolicyService(AppContext);
                    return _PolicyService;
                }
            }
            public UserProfileWizardInternalPresenter(iApplicationContext oContext)
                : base(oContext)
            {
                this.CurrentManager = new BaseModuleManager(oContext);
            }
            public UserProfileWizardInternalPresenter(iApplicationContext oContext, IViewUserProfileWizardInternal view)
                : base(oContext, view)
            {
                this.CurrentManager = new BaseModuleManager(oContext);
            }
        #endregion

        public void InitView()
        {
            if (View.isSystemOutOfOrder)
                View.DisplaySystemOutOfOrder();
            else if (!View.SubscriptionActive)
                View.LoadDefaultLogonPage();
            else
            {
                List<ProfileWizardStep> steps = Service.GetStandardProfileWizardStep(WizardType.Internal);

                View.AvailableSteps = steps;
                View.InitializeStep(ProfileWizardStep.StandardDisclaimer);
                View.InitializeStep(ProfileWizardStep.OrganizationSelector);
                View.GotoStep(ProfileWizardStep.StandardDisclaimer);
            }
        }

        public void MoveToNextStep(ProfileWizardStep step){
            switch (step) { 
                case ProfileWizardStep.StandardDisclaimer:
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
                    break;
                case ProfileWizardStep.OrganizationSelector:
                    View.InitializeStep(ProfileWizardStep.ProfileTypeSelector);
                                
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
                    break;
                case ProfileWizardStep.ProfileTypeSelector:
                    //if (!View.IsInitialized(ProfileWizardStep.ProfileUserData))
                    //    View.InitializeStep(ProfileWizardStep.ProfileUserData);
                    if (View.SelectedProfileTypeId > 0)
                        View.GotoStep(ProfileWizardStep.ProfileUserData,true);
                    break;
                case ProfileWizardStep.ProfileUserData:
                    List<ProfilerError> errors = InternalService.VerifyProfileInfo(View.ProfileInfo);
                    View.SaveUserPassword();
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
                    if (View.AcceptedMandatoryPolicy)
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
                case ProfileWizardStep.OrganizationSelector:
                    //if (!View.IsInitialized(ProfileWizardStep.ProfileTypeSelector))
                    //    View.InitializeStep(ProfileWizardStep.ProfileTypeSelector);
                    //if (View.AvailableProfileTypes.Count == 1 && View.SelectedProfileTypeId > 0)
                    //    MoveToNextStep(ProfileWizardStep.ProfileTypeSelector);
                    //else
                        View.GotoStep(ProfileWizardStep.StandardDisclaimer);
                    break;
                case ProfileWizardStep.ProfileTypeSelector:
                    if (View.AvailableOrganizationsId.Count == 1 && View.SelectedOrganizationId > 0)
                        MoveToNextStep(ProfileWizardStep.StandardDisclaimer);
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
        public void CreateProfile(dtoBaseProfile profile, Int32 idProfileType, String ProfileName, Int32 idOrganization)
        {
            List<ProfilerError> errors = InternalService.VerifyProfileInfo(profile);
            if (errors.Count == 0)
            {
                InternalAuthenticationProvider provider = InternalService.GetActiveProvider();
                ProfileSubscriptionMessage message = View.CreateProfile(profile, idProfileType, ProfileName, idOrganization, AuthenticationProviderType.Internal, (provider == null) ? (long)0 : provider.Id );
                Int32 idPerson = View.IdProfile;
                if (idPerson > 0)
                    PolicyService.SaveUserSelection(CurrentManager.GetPerson(idPerson), View.GetPolicyInfo);
                if (message == ProfileSubscriptionMessage.CreatedWithAutoLogon && idPerson > 0){
                      Person person = CurrentManager.GetPerson(View.IdProfile);
                      View.LogonUser(person, provider.Id, lm.Comol.Core.BaseModules.AuthenticationManagement.RootObject.InternalLogin(false), true, CurrentManager.GetUserDefaultIdOrganization(idPerson));
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
    }
}