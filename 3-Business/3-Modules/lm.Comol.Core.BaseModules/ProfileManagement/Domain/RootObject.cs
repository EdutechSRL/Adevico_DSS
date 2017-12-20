using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Core.BaseModules.ProfileManagement
{
    public class RootObject
    {
        public static String ActivateUserMail(System.Guid identifier)
        {
            return "Modules/ProfileManagement/ActivateUserMail.aspx?Identifier=" + identifier.ToString();
        }
        public static string InternalProfileWizard() 
        {
            return "Authentication/WizardInternalProfile.aspx";
        }
        public static string ShibbolethProfileWizard(long idProvider) 
        {
            return "Authentication/SHBlogin/WizardShibbolethProfile.aspx?idProvider=" + idProvider.ToString();
        }
        public static string UrlProfileWizard(long idProvider)
        {
            return "Authentication/WizardUrlProfile.aspx?idProvider=" + idProvider.ToString();
        }
        public static string MacUrlProfileWizard(long idProvider, List<lm.Comol.Core.Authentication.dtoMacUrlUserAttribute> attributes)
        {
            String baseUrl = "Authentication/WizardUrlTokenValidateProfile.aspx?idProvider=" + idProvider.ToString();
            if (attributes != null && attributes.Count > 0) {
                baseUrl += "&" + String.Join("&", attributes.Select(a => a.QueryName + "=" + a.QueryValue).ToArray());
            }
            return baseUrl;
        }
        public static String EndProfileWizard(ProfileSubscriptionMessage message)
        {
            return "Authentication/WizardEnd.aspx?message=" + message.ToString();
        }
        public static string DisabledProfile(long idProvider, Int32 idUser)
        {
            return "Authentication/ProfileDisabled.aspx?idProvider=" + idProvider.ToString() + "&IdUser=" + idUser.ToString();
        }
        public static String AddPortalProfile()
        {
            return "Authentication/WizardAddProfile.aspx";
        }


        public static String ImportProfiles()
        {
            return "Modules/ProfileManagement/ImportProfiles.aspx";
        }

        public static String AddCommunitiesToProfile(Int32 IdUser)
        {
            return "Modules/ProfileManagement/AddCommunityToProfile.aspx?IdUser=" + IdUser.ToString();
        }

        public static String AddPortalProfile(Int32 IdProfileType)
        {
            return "Authentication/WizardAddProfile.aspx?IdProfileType=" + IdProfileType.ToString();
        }
        public static String ManagementProfiles()
        {
            return "Modules/ProfileManagement/ProfilesManagement.aspx";
        }
        public static String ManagementProfilesWithFilters()
        {
            return ManagementProfiles() + "?ReloadFilters=true";
        }
        public static String ActivateUserProfile(Int32 IdUser, System.Guid UrlIdentifier)
        {
            return "Authentication/ActivateUserProfile.aspx?identifier=" + UrlIdentifier.ToString() +  "&IdUser=" + IdUser.ToString();
        }
        public static String MyProfile()
        {
            return "Modules/ProfileManagement/ProfileSettings.aspx";
        }
        public static String ProfileInfo(Int32 IdUser, Int32 idProfileType)
        {
            return "Modules/ProfileManagement/ProfileInfo.aspx?idProfileType=" + idProfileType.ToString() + "&IdUser=" + IdUser.ToString();
        }
        public static String EditProfile(Int32 IdUser, Int32 idProfileType)
        {
            return "Modules/ProfileManagement/EditProfile.aspx?idProfileType=" + idProfileType.ToString() + "&IdUser=" + IdUser.ToString();
        }
        public static String DeleteProfile(Int32 IdUser, Int32 idProfileType)
        {
            return "Modules/ProfileManagement/DeleteProfile.aspx?idProfileType=" + idProfileType.ToString() + "&IdUser=" + IdUser.ToString();
        }
        public static String EditProfileType(Int32 IdUser, Int32 idProfileType)
        {
            return "Modules/ProfileManagement/EditProfileType.aspx?idProfileType=" + idProfileType.ToString() + "&IdUser=" + IdUser.ToString();
        }
        public static String EditProfileAuthentications(Int32 IdUser, Int32 idProfileType)
        {
            return "Modules/ProfileManagement/EditProfileAuthentications.aspx?idProfileType=" + idProfileType.ToString() + "&IdUser=" + IdUser.ToString();
        }
        public static String EditPassword()
        {
            return "Authentication/EditPassword.aspx";
        }

        public static String AgencyInfo(long idAgency)
        {
            return "Modules/ProfileManagement/AgencyInfo.aspx?idAgency=" + idAgency.ToString();
        }
        public static String EditAgency(long idAgency)
        {
            return "Modules/ProfileManagement/EditAgency.aspx?idAgency=" + idAgency.ToString();
        }
        public static String DeleteAgency(long idAgency)
        {
            return "Modules/ProfileManagement/DeleteAgency.aspx?idAgency=" + idAgency.ToString();
        }
        public static String ManagementAgencies()
        {
            return "Modules/ProfileManagement/AgencyManagement.aspx";
        }
        public static String ManagementAgenciesWithFilters()
        {
            return ManagementAgencies() + "?ReloadFilters=true";
        }
        public static String ImportAgencies()
        {
            return "Modules/ProfileManagement/ImportAgencies.aspx";
        }
        public static String AddAgency()
        {
            return "Modules/ProfileManagement/EditAgency.aspx";
        }
        public static String MustChangePassword()
        {
            return "Authentication/ExpiredPassword.aspx";
        }
        public static String SearchUsersForModule(String moduleCode)
        {
            return "Modules/ProfileManagement/ProfilesManagement.aspx?ModuleCode=" + moduleCode;
        }
        public static String SearchUsersForModule(String moduleCode, Boolean preloadFilters)
        {
            return SearchUsersForModule(moduleCode) + ((preloadFilters) ? "?ReloadFilters=true" :"");
        }

        public static String PortalContacts()
        {
            return CommunityContacts(ContactsSelectionMode.SystemUsers);
        }
        public static String MyCommunityContacts()
        {
            return CommunityContacts(ContactsSelectionMode.MyCommunities);
        }
        public static String CommunityContacts(ContactsSelectionMode mode,Int32 idCommunity = 0 )
        {
            return "Modules/Community/Contacts.aspx?mode=" + mode.ToString() + "&idCommunity=" + idCommunity.ToString();
        }
    }
}