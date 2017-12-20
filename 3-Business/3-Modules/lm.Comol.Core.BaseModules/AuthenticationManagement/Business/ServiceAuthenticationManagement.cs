using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel;
using lm.Comol.Core.Authentication;
using lm.Comol.Core.Business;
using lm.Comol.Core.PersonalInfo;

namespace lm.Comol.Core.BaseModules.AuthenticationManagement.Business
{
    public class ServiceAuthenticationManagement : CoreServices 
    {
        protected iApplicationContext _Context;

        #region initClass
            public ServiceAuthenticationManagement() :base() { }
            public ServiceAuthenticationManagement(iApplicationContext oContext) :base(oContext.DataContext) {
                _Context = oContext;
                this.UC = oContext.UserContext;
            }
            public ServiceAuthenticationManagement(iDataContext oDC) : base(oDC) { }
        #endregion

            //public List<ProfileWizardStep> GetStandardProfileWizardStep(WizardType type)
            //{
            //    List<ProfileWizardStep> steps = new List<ProfileWizardStep>();
            //    switch (type) { 
            //        case  WizardType.Internal:
            //            steps.Add(ProfileWizardStep.StandardDisclaimer);
            //            steps.Add(ProfileWizardStep.OrganizationSelector);
            //            steps.Add(ProfileWizardStep.ProfileTypeSelector);
            //            steps.Add(ProfileWizardStep.ProfileUserData);
            //            if (ExistPolicyToAccept())
            //                steps.Add(ProfileWizardStep.Privacy);
            //            steps.Add(ProfileWizardStep.Summary);
            //            break;
            //        case WizardType.UrlProvider:
            //            steps.Add(ProfileWizardStep.UnknownProfileDisclaimer);
            //            steps.Add(ProfileWizardStep.OrganizationSelector);
            //            steps.Add(ProfileWizardStep.ProfileTypeSelector);
            //            steps.Add(ProfileWizardStep.ProfileUserData);
            //            if (ExistPolicyToAccept())
            //                steps.Add(ProfileWizardStep.Privacy);
            //            steps.Add(ProfileWizardStep.Summary);
            //            break;
            //        case WizardType.Ldap:
            //            steps.Add(ProfileWizardStep.StandardDisclaimer);
            //            steps.Add(ProfileWizardStep.OrganizationSelector);
            //            steps.Add(ProfileWizardStep.ProfileTypeSelector);
            //            steps.Add(ProfileWizardStep.LdapCredentials);
            //            steps.Add(ProfileWizardStep.ProfileUserData);
            //            if (ExistPolicyToAccept())
            //                steps.Add(ProfileWizardStep.Privacy);
            //            steps.Add(ProfileWizardStep.Summary);
            //            break;
            //        case WizardType.Shibboleth:
            //            steps.Add(ProfileWizardStep.UnknownProfileDisclaimer);
            //            steps.Add(ProfileWizardStep.InternalCredentials);
            //            steps.Add(ProfileWizardStep.OrganizationSelector);
            //            steps.Add(ProfileWizardStep.ProfileTypeSelector);
            //            steps.Add(ProfileWizardStep.ProfileUserData);
            //            if (ExistPolicyToAccept())
            //                steps.Add(ProfileWizardStep.Privacy);
            //            steps.Add(ProfileWizardStep.Summary);
            //            break;
            //        default:
            //            break;
            //    }
            //    return steps;
            //}
            //private Boolean ExistPolicyToAccept()
            //{
            //    return (from dp in Manager.GetIQ<DataPolicy>() where dp.Deleted == BaseStatusDeleted.None && dp.isActive && dp.Mandatory select dp.Id).Any();
            //}
    }
}