using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.Business;
using lm.Comol.Core.Authentication.Business;
using lm.Comol.Core.DomainModel;
using lm.Comol.Core.BaseModules.ProfileManagement.Business;
using lm.Comol.Core.Authentication;
using lm.Comol.Core.Communities;
using lm.Comol.Core.BaseModules.CommunityManagement.Business;
using lm.Comol.Core.BaseModules.CommunityManagement;

namespace lm.Comol.Core.BaseModules.ProfileManagement.Presentation
{
    public class AddCommunityToProfilePresenter : lm.Comol.Core.DomainModel.Common.DomainPresenter
    {
         #region "Initialize"
            private int _ModuleID;
            private ProfileManagementService _Service;
            private ServiceCommunityManagement _ServiceCommunity;
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
            protected virtual IViewAddCommunityToProfile View
            {
                get { return (IViewAddCommunityToProfile)base.View; }
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
            private ServiceCommunityManagement ServiceCommunity
            {
                get
                {
                    if (_ServiceCommunity == null)
                        _ServiceCommunity = new ServiceCommunityManagement(AppContext);
                    return _ServiceCommunity;
                }
            }
            public AddCommunityToProfilePresenter(iApplicationContext oContext)
                : base(oContext)
            {
                this.CurrentManager = new BaseModuleManager(oContext);
            }
            public AddCommunityToProfilePresenter(iApplicationContext oContext, IViewAddCommunityToProfile view)
                : base(oContext, view)
            {
                this.CurrentManager = new BaseModuleManager(oContext);
            }
        #endregion

        public void InitView(Int32 idProfile)
        {
            if (UserContext.isAnonymous)
                View.DisplaySessionTimeout();
            else
            {
                ModuleProfileManagement module = ModuleProfileManagement.CreatePortalmodule(UserContext.UserTypeID);
                View.AllowManagement = (module.EditProfile || module.Administration || module.ViewProfiles);
                if (module.EditProfile || module.Administration)
                {
                    Person person = CurrentManager.GetPerson(idProfile);
                    if (person == null)
                        View.DisplayProfileUnknown();
                    else
                    {
                        View.IdProfile = person.Id;
                        dtoProfilePermission permission = new dtoProfilePermission(UserContext.UserTypeID, person.TypeID);
                        View.AllowSubscribeToCommunity = permission.Edit;
                        View.LoadProfileName(person.SurnameAndName);
                        if (permission.Edit)
                        {
                            InitializeSteps();
                            View.GotoStep(CommunitySubscriptionWizardStep.SelectCommunities, true);
                        }
                        else
                            View.DisplayNoPermission();
                    }
                }
                else
                    View.DisplayNoPermission();
            }
        }

        private void InitializeSteps() { 
            List<CommunitySubscriptionWizardStep> steps = new  List<CommunitySubscriptionWizardStep>();
            steps.Add(CommunitySubscriptionWizardStep.SelectCommunities);
            steps.Add(CommunitySubscriptionWizardStep.SubscriptionsSettings);
            steps.Add(CommunitySubscriptionWizardStep.RemoveSubscriptions);
            steps.Add(CommunitySubscriptionWizardStep.Summary);

            View.AvailableSteps = steps;

            List<CommunitySubscriptionWizardStep> stepsToSkip = new  List<CommunitySubscriptionWizardStep>();
            stepsToSkip.Add(CommunitySubscriptionWizardStep.RemoveSubscriptions);
            View.SkipSteps = stepsToSkip;       
        }

        public void MoveToNextStep(CommunitySubscriptionWizardStep step)
        {
            switch (step)
            {
                case CommunitySubscriptionWizardStep.SelectCommunities:
                    MoveFromCommunities();
                    break;
                case CommunitySubscriptionWizardStep.SubscriptionsSettings:
                    MoveFromSubscriptions();
                    break;
                case CommunitySubscriptionWizardStep.RemoveSubscriptions:
                    MoveToSummary();
                    break;
                case CommunitySubscriptionWizardStep.Summary:
                    break;
            }
        }
        public void MoveToPreviousStep(CommunitySubscriptionWizardStep step)
        {
            if (UserContext.isAnonymous)
                View.DisplaySessionTimeout();
            else
            {
                switch (step)
                {
                    case CommunitySubscriptionWizardStep.SubscriptionsSettings:
                        View.GotoStep(CommunitySubscriptionWizardStep.SelectCommunities);
                        break;
                    case CommunitySubscriptionWizardStep.RemoveSubscriptions:
                        if (View.SkipSteps.Contains(CommunitySubscriptionWizardStep.SubscriptionsSettings))
                            View.GotoStep(CommunitySubscriptionWizardStep.SelectCommunities);
                        else
                            View.GotoStep(CommunitySubscriptionWizardStep.SubscriptionsSettings);
                        break;
                    case CommunitySubscriptionWizardStep.Summary:
                        if (View.SkipSteps.Contains(CommunitySubscriptionWizardStep.RemoveSubscriptions))
                            if (View.SkipSteps.Contains(CommunitySubscriptionWizardStep.SubscriptionsSettings))
                                View.GotoStep(CommunitySubscriptionWizardStep.SelectCommunities);
                            else
                                View.GotoStep(CommunitySubscriptionWizardStep.SubscriptionsSettings);
                        else
                            View.GotoStep(CommunitySubscriptionWizardStep.RemoveSubscriptions);
                        break;
                    case CommunitySubscriptionWizardStep.Errors:
                        View.GotoStep(CommunitySubscriptionWizardStep.Summary);
                        break;
                }
            }
        }

        private void MoveFromCommunities() {
            if (UserContext.isAnonymous)
                View.DisplaySessionTimeout();
            else if (View.HasAvailableCommunities == true)
            {
                List<dtoBaseCommunityNode> nodes = View.SelectedCommunities();
                Boolean hasSubscriptions = (nodes.Count != 0);
                Boolean hasUnsubscriptions = false;
                if (View.CurrentAvailability== CommunityAvailability.All || View.CurrentAvailability== CommunityAvailability.Subscribed)
                    hasUnsubscriptions = ServiceCommunity.ProfileHasCommunityToUnsubscribe(View.IdProfile, View.CommunityFilters, (nodes.Select(n => n.Id).ToList()));
                UpdateStepsToSkip(CommunitySubscriptionWizardStep.SubscriptionsSettings, !hasSubscriptions);
                UpdateStepsToSkip(CommunitySubscriptionWizardStep.RemoveSubscriptions, !hasUnsubscriptions);

                if (hasSubscriptions)
                    View.GotoStep(CommunitySubscriptionWizardStep.SubscriptionsSettings, true);
                else if (hasUnsubscriptions){
                    SetupUnsubscriptions();
                    View.GotoStep(CommunitySubscriptionWizardStep.RemoveSubscriptions);
                };
            }
        }

        private void MoveFromSubscriptions()
        {
            if (UserContext.isAnonymous)
                View.DisplaySessionTimeout();
            else if (View.HasAvailableSubscriptions == true)
            {
                if (View.SkipSteps.Contains(CommunitySubscriptionWizardStep.RemoveSubscriptions))
                    MoveToSummary();
                else if (View.HasUnsubscriptions)
                    View.GotoStep(CommunitySubscriptionWizardStep.RemoveSubscriptions);
                else{
                    SetupUnsubscriptions();
                    View.GotoStep(CommunitySubscriptionWizardStep.RemoveSubscriptions);
                }
            }
        }

        private void SetupUnsubscriptions()
        {
            Int32 idProfile = View.IdProfile;
            List<dtoBaseUserSubscription> unsubscriptions = ServiceCommunity.ProfileIdCommunitiesToUnsubscribe(idProfile,View.CommunityFilters,View.SelectedIdCommunities());
            List<dtoBaseCommunityNode> nodes = View.GetNodesById(unsubscriptions.Select(s=>s.IdCommunity).ToList());

            unsubscriptions.ForEach(s => s.Path = nodes.Where(n => n.Id == s.IdCommunity).Select(n => n.Path).ToList());
            View.InitializeUnsubscriptionControl(idProfile, unsubscriptions);
        }

        private void MoveToSummary()
        {
            if (UserContext.isAnonymous)
                View.DisplaySessionTimeout();
            else if (View.HasAvailableSubscriptions || View.HasUnsubscriptions)
            {
                List<dtoBaseUserSubscription> toDelete = View.SelectedUnsubscriptions();
                List<dtoUserSubscription> subscriptions = View.SelectedSubscriptions();

                View.DisplaySummaryInfo((from s in subscriptions where s.isNew select s.CommunityName).ToList(), (from s in subscriptions where s.isToUpdate && s.isNew == false select s.CommunityName).ToList(), (from s in toDelete select s.CommunityName).ToList());
                View.GotoStep(CommunitySubscriptionWizardStep.Summary);
            }
        }

        public void SaveChanges(List<dtoUserSubscription> subscriptions, List<dtoBaseUserSubscription> toDelete)
        {
            Int32 idProfile = View.IdProfile;
            Person person = CurrentManager.GetPerson(idProfile);
            if (person == null || UserContext.isAnonymous)
                View.DisplaySessionTimeout();
            else { 
                Person current = CurrentManager.GetPerson(UserContext.CurrentUserID);

                if (ServiceCommunity.UpdateUserSubscriptions(person, subscriptions, toDelete))
                    View.UpdateUserSubscriptions(idProfile, person.SurnameAndName, current.SurnameAndName, (from s in subscriptions where s.isNew select s).ToList(), toDelete.Select(s => s.IdCommunity).ToList());
                else
                    View.DisplayError();
            }
        }



        private void UpdateStepsToSkip(CommunitySubscriptionWizardStep step, Boolean add)
        {
            List<CommunitySubscriptionWizardStep> toSkip = View.SkipSteps;

            if (add && !toSkip.Contains(step))
                toSkip.Add(step);
            else if (!add && toSkip.Contains(step))
                toSkip.Remove(step);
            View.SkipSteps = toSkip;
        }

    }
}