using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Modules.CallForPapers.Domain;
using lm.Comol.Modules.CallForPapers.Business;
using lm.Comol.Core.DomainModel;
using lm.Comol.Core.DomainModel.Helpers;
namespace lm.Comol.Modules.CallForPapers.Presentation
{
    public class SelectUsersFromCallPresenter : lm.Comol.Core.DomainModel.Common.DomainPresenter
    {

        #region "Initializer"
            private int _ModuleID;
            private ServiceRequestForMembership _Service;

            private int ModuleID
            {
                get
                {
                    if (_ModuleID <= 0)
                    {
                        _ModuleID = this.Service.ServiceModuleID();
                    }
                    return _ModuleID;
                }
            }
            public virtual ManagerRequestForMemebership CurrentManager { get; set; }

            protected virtual IViewSelectUsersFromCall View
            {
                get { return (IViewSelectUsersFromCall)base.View; }
            }
            private ServiceRequestForMembership Service
            {
                get
                {
                    if (_Service == null)
                        _Service = new ServiceRequestForMembership(AppContext);
                    return _Service;
                }
            }
            public SelectUsersFromCallPresenter(iApplicationContext oContext)
                : base(oContext)
            {
                this.CurrentManager = new ManagerRequestForMemebership(oContext);
            }
            public SelectUsersFromCallPresenter(iApplicationContext oContext, IViewSelectUsersFromCall view)
                : base(oContext, view)
            {
                this.CurrentManager = new ManagerRequestForMemebership(oContext);
            }
        #endregion

        public void InitView(CallForPaperType typeToLoad,Boolean fromPortal, List<Int32> fromCommunities,List<Int32> removeUsers)
        {
            View.RemoveUsers = removeUsers;
            if (UserContext.isAnonymous)
                View.DisplaySessionTimeout();
            else
                LoadAvailableCall(typeToLoad, fromPortal, fromCommunities, removeUsers);
            View.isInitialized = true; 
        }

        private void LoadAvailableCall(CallForPaperType type,  Boolean fromPortal, List<Int32> fromCommunities,List<Int32> removeUsers)
        {
            View.FromCommunities = fromCommunities;
            View.FromPortal = fromPortal;
            View.CallsType= type;
            View.SelectedIdCall = 0;
            LoadCalls(type, fromPortal, fromCommunities, removeUsers, 0, View.CallsPageSize);
        }
        public void LoadCalls(Int32 pageIndex, Int32 pageSize) {
            LoadCalls(View.CallsType, View.FromPortal, View.FromCommunities, View.RemoveUsers, pageIndex, pageSize);
        }
        public void LoadCalls(CallForPaperType type, Boolean fromPortal, List<Int32> fromCommunities, List<Int32> removeUsers, Int32 pageIndex, Int32 pageSize)
        {
            List<dtoCallInfo> calls = Service.GetCallsForPersonAssignments(type, fromPortal, fromCommunities, removeUsers, View.UnknownCommunityName, View.PortalName);
            PagerBase pager = new PagerBase();
            pager.PageSize = pageSize;//Me.View.CurrentPageSize
            pager.Count = calls.Count - 1;
            pager.PageIndex = pageIndex;// Me.View.CurrentPageIndex
            View.CallsPager = pager;

            if (calls != null && calls.Any())
            {
                calls = calls.Skip(pager.PageIndex * pageSize).Take(pageSize).OrderBy(c => c.CommunityName).OrderBy(c => c.Name).OrderByDescending(c => c.EndDate).ToList();
                if (calls.Any())
                    View.LoadCalls(calls);
                else
                    View.DisplayNoCallsAvailable();
            }
            else
            {
                View.DisplayNoCallsAvailable();
            }
        }

        public void SelectCall(long idCall)
        {
            dtoFilterSubmissions filter = new dtoFilterSubmissions() { Ascending = true, OrderBy = SubmissionsOrder.ByUser };
            View.SelectedIdCall = idCall;
            List<SubmissionFilterStatus> statusList = Service.GetAvailableSubmissionStatusForPersonAssignments(idCall, View.RemoveUsers);
            SubmissionFilterStatus dStatus = (!statusList.Any() || statusList.Contains(SubmissionFilterStatus.Accepted)) ? SubmissionFilterStatus.Accepted : statusList[0]; 
            View.LoadAvailableStatus(statusList, dStatus);

            Dictionary<long, string> submitters = LoadSubmitters(idCall);
            long idSubmitterType = (submitters != null && submitters.Keys.Count ==1) ? submitters.Keys.FirstOrDefault() : -1;

            View.LoadSubmitterstype(submitters, idSubmitterType);
            filter.IdSubmitterType= idSubmitterType;
            filter.Status = dStatus;
            filter.SearchForName = "";
            View.CurrentFilter = filter;

            View.DisplayCallSubmissions(Service.GetCallName(idCall));
            LoadSubmissions(filter,0, View.SubmissionsPageSize);
        }
        public void ChangeCall()
        {
            View.SelectAllSubmissions = false;
            View.SelectedSubmissions = new List<long>();
            LoadCalls(0, View.CallsPageSize);
        }
        public Dictionary<long, string> LoadSubmitters()
        {
            return LoadSubmitters(View.SelectedIdCall);
        }
        private Dictionary<long, string> LoadSubmitters(long idCall)
        {
            return Service.GetCallSubmittersTypeForPersonAssignments(idCall);
        }

        public void EditItemsSelection(Boolean selectAll)
        {
            View.SelectAllSubmissions = selectAll;
            View.SelectedSubmissions = (selectAll) ? UpdateItemsSelection().Distinct().ToList() : new List<long>();
        }
        public List<Int32> GetSelectedUsers() {
            return Service.GetIdUsersFromSubmissions(View.SelectedIdCall,View.RemoveUsers,  View.SelectAllSubmissions,View.CurrentFilter, UpdateItemsSelection());
        }
        public void LoadSubmissions(dtoFilterSubmissions filter, Int32 pageIndex, Int32 pageSize)
        {
            long idCall = View.SelectedIdCall;
            
            PagerBase pager = new PagerBase();
            pager.PageSize = pageSize;//Me.View.CurrentPageSize
            pager.Count = Service.GetSubmissionsCountForPersonAssignments(idCall, View.RemoveUsers, filter) - 1;
            pager.PageIndex = pageIndex;// Me.View.CurrentPageIndex
            View.SubmissionsPager = pager;

            List<dtoSubmissionDisplayItem> submissions = Service.GetSubmissionsForPersonAssignments(idCall, View.RemoveUsers, filter, pager.PageIndex, pageSize);

            if (submissions.Any())
                View.LoadSubmissions(submissions);
            else
                View.DisplayNoSubmissionsAvailable();
        }
        private List<long> UpdateItemsSelection()
        {
            // SELECTED ITEMS
            List<long> submissions = View.SelectedSubmissions;
            List<dtoSelectItem<long>> cSelectedItems = View.GetCurrentSelectedItems();

            // REMOVE ITEMS
            submissions = submissions.Where(i => !cSelectedItems.Where(si => !si.Selected && si.Id == i).Any()).ToList();
            // ADD ITEMS
            submissions.AddRange(cSelectedItems.Where(si => si.Selected && !submissions.Contains(si.Id)).Select(si => si.Id).Distinct().ToList());
            return submissions;
        }
    }
}