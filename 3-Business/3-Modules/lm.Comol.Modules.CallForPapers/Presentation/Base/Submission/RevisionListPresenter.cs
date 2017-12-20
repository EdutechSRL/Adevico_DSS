using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel;
using lm.Comol.Modules.CallForPapers.Business;
using lm.Comol.Modules.CallForPapers.Domain;
using lm.Comol.Core.Business;

namespace lm.Comol.Modules.CallForPapers.Presentation
{
    public class RevisionListPresenter : lm.Comol.Core.DomainModel.Common.DomainPresenter
    {
        private const int currentPageSize = 25;
         #region "Initialize"
            private ServiceCallOfPapers _Service;
            private ServiceRequestForMembership _ServiceRequest;
        
            public virtual BaseModuleManager CurrentManager { get; set; }
            protected virtual IViewRevisionList View
            {
                get { return (IViewRevisionList)base.View; }
            }
            private ServiceCallOfPapers Service
            {
                get
                {
                    if (_Service == null)
                        _Service = new ServiceCallOfPapers(AppContext);
                    return _Service;
                }
            }
            private ServiceRequestForMembership ServiceRequest
            {
                get
                {
                    if (_ServiceRequest == null)
                        _ServiceRequest = new ServiceRequestForMembership(AppContext);
                    return _ServiceRequest;
                }
            }
            public RevisionListPresenter(iApplicationContext oContext)
                : base(oContext)
            {
                this.CurrentManager = new BaseModuleManager(oContext);
            }
            public RevisionListPresenter(iApplicationContext oContext, IViewRevisionList view)
                : base(oContext, view)
            {
                this.CurrentManager = new BaseModuleManager(oContext);
            }
        #endregion

        public void InitView()
        {
            CallForPaperType type = View.PreloadCallType;
            View.CallType= type;
            CallStandardAction action = View.PreloadAction;
            if (action != CallStandardAction.Manage)
                action = CallStandardAction.List;
            View.CurrentAction = action;

            int idCommunity = SetCallsCurrentCommunity();
            if (UserContext.isAnonymous)
                View.DisplaySessionTimeout();
            else
            {
                Int32 idUser = UserContext.CurrentUserID;
                Boolean allowView = false;
                Boolean allowManage = false;

                dtoRevisionFilters filters = View.PreloadFilters;
                View.CurrentFilters = filters;
                switch(type){
                    case CallForPaperType.CallForBids:
                        ModuleCallForPaper module = Service.CallForPaperServicePermission(idUser, idCommunity);
                        allowView = (module.ViewCallForPapers || module.Administration || module.ManageCallForPapers);
                        allowManage = (module.CreateCallForPaper || module.Administration || module.ManageCallForPapers || module.EditCallForPaper);

                        if (allowView || allowManage)
                        {
                            InitializeView(action, allowManage, idCommunity, View.PreloadView, type, filters.Status);
                            LoadRevisions(module, action, idCommunity, filters, View.PreloadPageIndex, View.PreloadPageSize);
                        }
                        else
                            View.DisplayNoPermission(idCommunity, Service.ServiceModuleID());
                        break;
                    case CallForPaperType.RequestForMembership:
                        ModuleRequestForMembership moduleR = Service.RequestForMembershipServicePermission(idUser, idCommunity);
                        allowView = (moduleR.ViewBaseForPapers || moduleR.Administration || moduleR.ManageBaseForPapers);
                        allowManage = (moduleR.CreateBaseForPaper || moduleR.Administration || moduleR.ManageBaseForPapers || moduleR.EditBaseForPaper);
                        if (allowView || allowManage)
                        {
                            InitializeView(action, allowManage, idCommunity, View.PreloadView, type, filters.Status);
                            LoadRevisions(moduleR, action, idCommunity, filters, View.PreloadPageIndex, View.PreloadPageSize);
                        }
                        else
                            View.DisplayNoPermission(idCommunity, Service.ServiceModuleID());
                        break;
                }
                switch (action)
                {
                    case CallStandardAction.List:
                        View.AllowManage = allowManage;
                        if (allowManage)
                            View.SetActionUrl(CallStandardAction.Manage, RootObject.ViewCalls(CallForPaperType.CallForBids, CallStandardAction.Manage, idCommunity, View.CurrentView));
                        break;
                    case CallStandardAction.Manage:
                        View.AllowView = allowView;
                        if (allowView)
                            View.SetActionUrl(CallStandardAction.List, RootObject.ViewCalls(CallForPaperType.CallForBids, CallStandardAction.List, idCommunity, View.CurrentView));
                        break;
                }
            }
        }

        private int SetCallsCurrentCommunity()
        {
            int idCommunity = this.UserContext.CurrentCommunityID;
            Community currentCommunity = CurrentManager.GetCommunity(idCommunity);
            Community community = null;
            if (View.PreloadIdCommunity > 0)
                idCommunity = View.PreloadIdCommunity;

            if (idCommunity > 0)
            {
                community = CurrentManager.GetCommunity(idCommunity);
                if (community != null)
                    View.SetContainerName(idCommunity, community.Name);
                else if (currentCommunity != null)
                {
                    idCommunity = this.UserContext.CurrentCommunityID;
                    View.SetContainerName(idCommunity, community.Name);
                }
                else
                {
                    idCommunity = 0;
                    View.SetContainerName(idCommunity, View.Portalname);
                }
            }
            else
                View.SetContainerName(0, View.Portalname);
            View.IdCallCommunity = idCommunity;
            return idCommunity;
        }
        private void InitializeView(CallStandardAction action, Boolean allowManage, int idCommunity, CallStatusForSubmitters preloadView, CallForPaperType type, RevisionStatus status)
        {
            //Boolean initialize = true;
            List<CallStatusForSubmitters> views = null;
            if (type== CallForPaperType.CallForBids)
                views = Service.GetAvailableViews(action, Service.CallForPaperServicePermission(UserContext.CurrentUserID, idCommunity), (action == CallStandardAction.List && idCommunity == 0), idCommunity, UserContext.CurrentUserID);
            else
                views = ServiceRequest.GetAvailableViews(action, allowManage, (action == CallStandardAction.List && idCommunity == 0), (idCommunity == 0), idCommunity, UserContext.CurrentUserID, type);
            if ((!views.Contains(preloadView) && views.Count == 0) || !views.Contains(CallStatusForSubmitters.Revisions) )
            {
                preloadView = CallStatusForSubmitters.Revisions;
                views.Add(CallStatusForSubmitters.Revisions);
            }
            else if (!views.Contains(preloadView))
                preloadView = views[0];
            /*if ((preloadView == CallStatusForSubmitters.Revisions && !views.Contains(CallStatusForSubmitters.Revisions)) || preloadView != CallStatusForSubmitters.Revisions)
                initialize = false;
            else
            {*/
                View.LoadAvailableView(idCommunity, views);
                List<RevisionStatus> items = LoadAvailableStatus(action, idCommunity, type);
                if (items.Count == 0 || items.Count > 1)
                    items.Insert(0, RevisionStatus.None);
                View.LoadRevisionStatus(items, status);
                View.CurrentView = preloadView;
            //}
           // return initialize;
        }
        public void LoadRevisions(dtoRevisionFilters filters, int pageIndex, int pageSize)
        {
            View.CurrentFilters = filters;
            switch (View.CallType)
            {
                case CallForPaperType.CallForBids:
                    ModuleCallForPaper module = Service.CallForPaperServicePermission(UserContext.CurrentUserID, View.IdCallCommunity);
                    LoadRevisions(module, View.CurrentAction, View.IdCallCommunity, filters, pageIndex, pageSize);
                    break;
                case CallForPaperType.RequestForMembership:
                    ModuleRequestForMembership moduleR = Service.RequestForMembershipServicePermission(UserContext.CurrentUserID, View.IdCallCommunity);
                    LoadRevisions(moduleR, View.CurrentAction, View.IdCallCommunity, filters, pageIndex, pageSize);
                    break;
            }
        }
        private void LoadRevisions(ModuleCallForPaper module, CallStandardAction action, int idCommunity, dtoRevisionFilters filters, int pageIndex, int pageSize)
        {
            liteCommunity community = CurrentManager.GetLiteCommunity(idCommunity);
            litePerson person = CurrentManager.GetLitePerson(UserContext.CurrentUserID);
            Boolean fromAllcommunities = false;
            PagerBase pager = new PagerBase();
            if (pageSize == 0)
                pageSize = 50;
            pager.PageSize = pageSize;

            if (action == CallStandardAction.Manage)
                pager.Count = (int)Service.RevisionCount(fromAllcommunities, (idCommunity == 0), community, person, CallForPaperType.CallForBids, filters, RevisionType.Manager)-1;
            else
            {
                fromAllcommunities = (idCommunity == 0);
                pager.Count = (int)Service.RevisionCount(fromAllcommunities, (idCommunity == 0), community, person, CallForPaperType.CallForBids, filters, RevisionType.UserRequired) - 1;
            }
            pager.PageIndex = pageIndex;// Me.View.CurrentPageIndex
            View.Pager = pager;
            if (pager.Count < 0)
                View.LoadNoRevisionsFound();
            else
                View.LoadRevisions(Service.GetRevisionList(module, action, fromAllcommunities, (idCommunity == 0), community, person, filters, pageIndex, pageSize));
            
            View.SendUserAction(View.IdCallCommunity, Service.ServiceModuleID(), ModuleCallForPaper.ActionType.LoadRevisionsList);
        }
        private void LoadRevisions(ModuleRequestForMembership module, CallStandardAction action, int idCommunity, dtoRevisionFilters filters, int pageIndex, int pageSize)
        {
            liteCommunity community = CurrentManager.GetLiteCommunity(idCommunity);
            litePerson person = CurrentManager.GetLitePerson(UserContext.CurrentUserID);
            Boolean fromAllcommunities = false;
            PagerBase pager = new PagerBase();
            pager.PageSize = pageSize;

            if (action == CallStandardAction.Manage)
                pager.Count = (int)Service.RevisionCount(fromAllcommunities, (idCommunity == 0), community, person, CallForPaperType.RequestForMembership, filters, RevisionType.Manager) - 1;
            else
            {
                fromAllcommunities = (idCommunity == 0);
                pager.Count = (int)Service.RevisionCount(fromAllcommunities, (idCommunity == 0), community, person, CallForPaperType.RequestForMembership, filters, RevisionType.UserRequired) - 1;
            }
            pager.PageIndex = pageIndex;// Me.View.CurrentPageIndex
            View.Pager = pager;
            if (pager.Count < 0)
                View.LoadNoRevisionsFound();
            else
                View.LoadRevisions(Service.GetRevisionList(module, action, fromAllcommunities, (idCommunity == 0), community, person, filters, pager.PageIndex, pageSize));
            
            View.SendUserAction(View.IdCallCommunity, ServiceRequest.ServiceModuleID(), ModuleRequestForMembership.ActionType.LoadRevisionsList);
        }

        private List<RevisionStatus> LoadAvailableStatus(CallStandardAction action, int idCommunity, CallForPaperType type)
        {
            liteCommunity community = CurrentManager.GetLiteCommunity(idCommunity);
            litePerson person = CurrentManager.GetLitePerson(UserContext.CurrentUserID);
            Boolean fromAllcommunities = false;
            dtoRevisionFilters filters = new dtoRevisionFilters(){ OrderBy= RevisionOrder.None, Status= RevisionStatus.None, CallType=type};

            if (action == CallStandardAction.Manage)
                return Service.GetAvailableRevisionStatus(fromAllcommunities, (idCommunity==0), community, person, type,filters,RevisionType.Manager);
            else
            {
                fromAllcommunities = (idCommunity == 0);
                return Service.GetAvailableRevisionStatus(fromAllcommunities, (idCommunity == 0), community, person, type, filters, RevisionType.UserRequired);
            }
            
        }

        public void RemoveSelfRequest(long idRevision, dtoRevisionMessage selfRemoveMessage, String webSiteurl, dtoRevisionFilters filters, int pageIndex, int pageSize)
        {
            int idUser = UserContext.CurrentUserID;
            if (!UserContext.isAnonymous)
            {
                long idSubmission = Service.GetIdSubmissionFromRevision(idRevision);
                Service.RemoveSelfRequest(View.CallType, idSubmission, idRevision, idUser, View.PreloadIdCommunity, selfRemoveMessage, webSiteurl);
            }
            LoadRevisions(filters, pageIndex, pageSize);
        }
        public void RemoveUserRequest(long idRevision, dtoRevisionMessage managerMessage, String webSiteurl, dtoRevisionFilters filters, int pageIndex, int pageSize)
        {
            int idUser = UserContext.CurrentUserID;
            if (!UserContext.isAnonymous)
            {
                long idSubmission = Service.GetIdSubmissionFromRevision(idRevision);
                Service.RemoveSelfRequest(View.CallType, idSubmission, idRevision, idUser, View.PreloadIdCommunity, managerMessage, webSiteurl);
            }
            LoadRevisions(filters, pageIndex, pageSize);
        }
        public void RemoveManagerRequest(long idRevision, dtoRevisionMessage managerMessage, String webSiteurl, dtoRevisionFilters filters, int pageIndex, int pageSize)
        {
            int idUser = UserContext.CurrentUserID;
            if (!UserContext.isAnonymous)
            {
                long idSubmission = Service.GetIdSubmissionFromRevision(idRevision);
                Service.RemoveManagerRequest(View.CallType, idSubmission, idRevision, idUser, View.PreloadIdCommunity, "", managerMessage, webSiteurl);
            }
            LoadRevisions(filters, pageIndex, pageSize);
        }
    }
}