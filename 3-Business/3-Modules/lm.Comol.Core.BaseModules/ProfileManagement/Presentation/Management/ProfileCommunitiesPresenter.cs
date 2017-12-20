using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.Business;
using lm.Comol.Core.Authentication.Business;
using lm.Comol.Core.DomainModel;
using lm.Comol.Core.BaseModules.ProfileManagement.Business;
using lm.Comol.Core.Authentication;
using lm.Comol.Core.Subscriptions;

namespace lm.Comol.Core.BaseModules.ProfileManagement.Presentation
{
    public class ProfileCommunitiesPresenter : lm.Comol.Core.DomainModel.Common.DomainPresenter
    {
         #region "Initialize"
            private int _ModuleID;
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
            protected virtual IViewProfileCommunitySubscriptions View
            {
                get { return (IViewProfileCommunitySubscriptions)base.View; }
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
            public ProfileCommunitiesPresenter(iApplicationContext oContext)
                : base(oContext)
            {
                this.CurrentManager = new BaseModuleManager(oContext);
            }
            public ProfileCommunitiesPresenter(iApplicationContext oContext, IViewProfileCommunitySubscriptions view)
                : base(oContext, view)
            {
                this.CurrentManager = new BaseModuleManager(oContext);
            }
        #endregion

        public void InitView(Int32 idProfile)
        {
            Person person = CurrentManager.GetPerson(idProfile);
            if (person == null || UserContext.isAnonymous)
                View.DisplayEmpty();
            else
            {
                View.IdProfile = idProfile;
                ModuleProfileManagement module = ModuleProfileManagement.CreatePortalmodule(UserContext.UserTypeID);
                dtoProfilePermission permission = new dtoProfilePermission(UserContext.UserTypeID, person.TypeID);

                if (module.ViewProfiles || module.Administration)
                    InitializeFilters(person);
                else
                    View.DisplayEmpty();
            }
        }

        private void InitializeFilters(Person user)
        {
            View.OrderAscending = false;

            dtoSubscriptionFilters filters = View.GetCurrentFilters;

            if (filters == null) {
                filters = new dtoSubscriptionFilters();
                filters.Ascending = false;
                filters.IdcommunityType = -1;
                filters.IdOrganization = -1;
                filters.IdOwner = -1;
                filters.OrderBy = OrderSubscriptionsBy.SubscriptionDate;
                filters.PageIndex = 0;
                filters.PageSize= (View.CurrentPageSize==0) ? 25 : View.CurrentPageSize;
                filters.SearchBy = SearchSubscriptionsBy.All;
                filters.StartWith = "";
                filters.Value = "";
                filters.Status = SubscriptionStatus.all;
            }
            View.LoadAvaliableStatus(Service.GetAvailableStatus(filters, user));
            filters.Status = View.SelectedStatus;
            LoadSubscriptions(filters,filters.PageIndex, filters.PageSize);
        }

        public void LoadSubscriptions(int currentPageIndex, int currentPageSize)
        {
            LoadSubscriptions(View.GetCurrentFilters, currentPageIndex, currentPageSize);
        }
        public void LoadSubscriptions(dtoSubscriptionFilters filters,int currentPageIndex, int currentPageSize) {
            PagerBase pager = new PagerBase();
            Person person = CurrentManager.GetPerson(View.IdProfile);
            pager.PageSize = currentPageSize;//Me.View.CurrentPageSize
            pager.Count = (int)Service.GetProfileSubscriptionsCount(filters, person) - 1;
            pager.PageIndex = currentPageIndex;// Me.View.CurrentPageIndex
            View.Pager = pager;
            View.LoadCommunities(Service.GetProfileSubscriptions(filters, person, pager.PageIndex, currentPageSize));
        }
    }
}