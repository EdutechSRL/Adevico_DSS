using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.Authentication;
using lm.Comol.Core.DomainModel;
using lm.Comol.Core.Subscriptions;
using lm.Comol.Core.Communities;
using lm.Comol.Core.BaseModules.CommunityManagement;
using lm.Comol.Core.DomainModel.Helpers;
using lm.Comol.Core.Mail;
using lm.Comol.Core.MailCommons.Domain.Configurations;

namespace lm.Comol.Core.BaseModules.ProfileManagement.Presentation
{
    public interface IViewProfilesImport : lm.Comol.Core.DomainModel.Common.iDomainView 
    {
        Boolean AllowManagement { set; }

        System.Guid ImportIdentifier { get; set; }
        ProfileImportStep CurrentStep { get; set; }
        List<ProfileImportStep> AvailableSteps { get; set; }
        List<ProfileImportStep> SkipSteps { get; set; }




        Boolean IsInitialized(ProfileImportStep pStep);
        void GotoStep(ProfileImportStep pStep);
        void GotoStep(ProfileImportStep pStep, Boolean initialize);
        void InitializeStep(ProfileImportStep pStep);

        // STEP source
        SourceType CurrentSource { get; }

        // STEP CSV
        CsvFile RetrieveFile();
        ProfileExternalResource GetFileContent(List<ProfileColumnComparer<String>> columns);
        // STEP Request For Membership

        // STEP CSV && Request For Membership
        List<ProfileColumnComparer<String>> AvailableColumns(SourceType type);
        //SETP
        List<ProfileColumnComparer<String>> Fields { get;}
        Boolean ValidDestinationFields { get; }
        dtoImportSettings ImportSettings { get; }
        void InitializeFieldsMatcher(List<ProfileColumnComparer<String>> sourceColumns);

        //Step select Items
        ProfileExternalResource SelectedItems { get;  }
        Boolean HasSelectedItems { get; }
        Int32 ItemsToSelect { get; }

        // SET select organizations
        List<Int32> AvailableOrganizationsId { get;}
        Int32 PrimaryOrganizationId { get;}
        String PrimaryOrganizationName { get;}
        Dictionary<Int32,String> OtherOrganizationsToSubscribe { get;}
        List<lm.Comol.Core.BaseModules.Dashboard.Domain.dtoCommunityPlainItem> OrganizationsNodes { get; set; }
        List<Int32> AllOrganizationsId { get; }

        // STEP community
        List<Int32> SelectedIdCommunities();
        List<lm.Comol.Core.BaseModules.Dashboard.Domain.dtoCommunityPlainItem> SelectedCommunities();
        void UpdateSelectedCommunities(List<Int32> idCommunities);

        // STEP subscription
        Boolean HasAvailableSubscriptions { get; }
        List<dtoNewProfileSubscription> SelectedSubscriptions();

        // STEP Mail
        Boolean SendMailToUsers { get; }
        dtoMailContent MailContent { get; }

        //STEP SUMMARY
        //void DisplaySummaryInfo(List<String> toSubscribe, List<String> toEdit, List<String> toUnsubscribe);
        //void UpdateUserSubscriptions(Int32 idProfile, String profileName, String currentUser, List<dtoUserSubscription> subscribed, List<Int32> unsubscribed);
        Boolean isCompleted { get; set; }
        void SetupProgressInfo(Int32 actionCount, Int32 userCount);
        void UpdateProfileCreation(Int32 actionIndex,Int32 userIndex,Boolean created, String displayName);
        void UpdateAddProfilesToOrganizations(Int32 actionIndex, Int32 orgnCount, Int32 orgIndex, Boolean created, String name);
        void UpdateAddProfilesToCommunities(Int32 actionIndex, Int32 commCount, Int32 commIndex, Boolean created, String name);
        void UpdateSendMailToProfile(Int32 actionIndex, Int32 mailCount, Int32 mailIndex, Boolean created, String name);
        SmtpServiceConfig CurrentSmtpConfig { get; }

        void UpdateSourceItems(SourceType type);
        Int32 AddUserProfile(dtoBaseProfile profile,PersonInfo info, Int32 idDefaultOrganization, AuthenticationProvider provider);

        ////STEP SUMMARY
        void DisplayImportedProfiles(Int32 importedItems,Int32 itemsToImport);
        void DisplayError(ProfileImportStep currentStep);
        void DisplayImportError(Int32 importedItems,List<dtoBaseProfile> notCreatedProfiles,Dictionary<Int32, String> notaddedToOrganizations,Dictionary<Int32, String> notSubscribedCommunities,List<dtoImportedProfile> notSentMail);
        ProfileImportStep PreviousStep { get; set; }

        void DisplaySessionTimeout();
        void DisplayNoPermission();
    }
}