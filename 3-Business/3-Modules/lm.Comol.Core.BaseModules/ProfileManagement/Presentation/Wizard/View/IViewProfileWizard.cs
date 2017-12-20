using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.BaseModules.PolicyManagement;
using lm.Comol.Core.Authentication;
using lm.Comol.Core.DomainModel;

namespace lm.Comol.Core.BaseModules.ProfileManagement.Presentation
{
    public interface IViewProfileWizard : lm.Comol.Core.DomainModel.Common.iDomainView 
    {
        ProfileWizardStep CurrentStep { get; }
        List<ProfileWizardStep> AvailableSteps { get; set; }
        List<ProfileWizardStep> SkipSteps { get; set; }

        List<Int32> AvailableOrganizationsId { get; }
       
        Int32 SelectedOrganizationId { get; set; }

        List<Int32> AvailableProfileTypes { get; }
        Int32 SelectedProfileTypeId { get; set; }
        String SelectedProfileName { get; }

        dtoBaseProfile ProfileInfo { get; set; }
        void SaveUserPassword();
        void LoadProfileInfoError(List<ProfilerError> errors);
        void UnloadProfileInfoError();

        Int32 IdProfile { get; set; }
       
        void LoadRegistrationMessage(ProfileSubscriptionMessage error);
        
        Boolean IsInitialized(ProfileWizardStep pStep);
        void GotoStep(ProfileWizardStep pStep);
        void GotoStep(ProfileWizardStep pStep, Boolean initialize);
        void InitializeStep(ProfileWizardStep pStep);
    }
}