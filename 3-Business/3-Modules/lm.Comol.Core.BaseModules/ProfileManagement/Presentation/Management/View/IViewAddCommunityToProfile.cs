using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.Authentication;
using lm.Comol.Core.DomainModel;
using lm.Comol.Core.Subscriptions;
using lm.Comol.Core.Communities;
using lm.Comol.Core.BaseModules.CommunityManagement;

namespace lm.Comol.Core.BaseModules.ProfileManagement.Presentation
{
    public interface IViewAddCommunityToProfile : lm.Comol.Core.DomainModel.Common.iDomainView 
    {
        Int32 PreloadedIdProfile { get; }
        CommunityAvailability PreloadedAvailability { get; }
        Int32 IdProfile { get; set; }
        Boolean AllowManagement { set; }
        Boolean AllowSubscribeToCommunity { set; }
        CommunityStatus CurrentStatus { get; }


        CommunitySubscriptionWizardStep CurrentStep { get; set; }
        List<CommunitySubscriptionWizardStep> AvailableSteps { get; set; }
        List<CommunitySubscriptionWizardStep> SkipSteps { get; set; }
       
       // Boolean IsInitialized(ProfileWizardStep pStep);
        void GotoStep(CommunitySubscriptionWizardStep pStep);
        void GotoStep(CommunitySubscriptionWizardStep pStep, Boolean initialize);
        void InitializeStep(CommunitySubscriptionWizardStep pStep);

        // STEP community
        CommunityAvailability CurrentAvailability { get; set; }
        dtoCommunitiesFilters CommunityFilters { get; }
        Boolean HasAvailableCommunities { get; }
        List<dtoBaseCommunityNode> SelectedCommunities();
        List<Int32> SelectedIdCommunities();
        List<dtoBaseCommunityNode> GetNodesById( List<Int32> idCommunities);

        // STEP subscriptions
        Boolean HasAvailableSubscriptions { get; }
        List<dtoUserSubscription> SelectedSubscriptions();

        // STEP unsubscriptions
        Boolean HasUnsubscriptions { get; }
        List<dtoBaseUserSubscription> SelectedUnsubscriptions();
        void InitializeUnsubscriptionControl(Int32 idProfile, List<dtoBaseUserSubscription> unsubscriptions);

        //STEP SUMMARY
        void DisplaySummaryInfo(List<String> toSubscribe, List<String> toEdit, List<String> toUnsubscribe);
        void UpdateUserSubscriptions(Int32 idProfile, String profileName, String currentUser, List<dtoUserSubscription> subscribed, List<Int32> unsubscribed);
        Boolean isCompleted { get; set; }
        //STEP SUMMARY
        void DisplayError();

        void LoadProfileName(string displayName);
        void DisplayProfileUnknown();
        void DisplaySessionTimeout();
        void DisplayNoPermission();
        void DisplayNoPermissionForProfile();
    }
}