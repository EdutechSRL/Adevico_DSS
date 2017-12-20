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
    public class RequestsListPresenter : lm.Comol.Core.DomainModel.Common.DomainPresenter
    {
        private const int currentPageSize = 25;
         #region "Initialize"
            private ServiceRequestForMembership _Service;

            public virtual BaseModuleManager CurrentManager { get; set; }
            protected virtual IViewRequestsList View
            {
                get { return (IViewRequestsList)base.View; }
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
            public RequestsListPresenter(iApplicationContext oContext)
                : base(oContext)
            {
                this.CurrentManager = new BaseModuleManager(oContext);
            }
            public RequestsListPresenter(iApplicationContext oContext, IViewRequestsList view)
                : base(oContext, view)
            {
                this.CurrentManager = new BaseModuleManager(oContext);
            }
        #endregion

        public void InitView()
        {
            CallStandardAction action = View.PreloadAction;
            if (action != CallStandardAction.Manage)
                action = CallStandardAction.List;
            View.CurrentAction = action;

            int idCommunity = SetCallsCurrentCommunity();
            if (UserContext.isAnonymous)
                View.DisplaySessionTimeout();
            else
            {
                ModuleRequestForMembership module = Service.RequestForMembershipServicePermission(UserContext.CurrentUserID, idCommunity);
                Boolean allowView = (module.ViewBaseForPapers || module.Administration || module.ManageBaseForPapers);
                Boolean allowManage = module.CreateBaseForPaper || module.Administration || module.ManageBaseForPapers || module.EditBaseForPaper;
                View.AllowSubmmissions = (action == CallStandardAction.List) && module.ViewBaseForPapers;
                
                
                if (allowView || allowManage) {
                    InitializeView(action, module, idCommunity, View.PreloadView, allowView, allowManage);
                    LoadCalls(module,action, idCommunity, View.CurrentView, 0, currentPageSize);
                }
                else
                    View.DisplayNoPermission(idCommunity, Service.ServiceModuleID());
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
        private void InitializeView(CallStandardAction action, ModuleRequestForMembership module, int idCommunity, CallStatusForSubmitters preloadView)
        {
            InitializeView(action, module, idCommunity, preloadView, (module.ViewBaseForPapers || module.Administration || module.ManageBaseForPapers), (module.CreateBaseForPaper || module.Administration || module.ManageBaseForPapers || module.EditBaseForPaper));
        }
        private void InitializeView(CallStandardAction action, ModuleRequestForMembership module, int idCommunity, CallStatusForSubmitters preloadView, Boolean allowView, Boolean allowManage)
        {
            List<CallStatusForSubmitters> views = Service.GetAvailableViews(action, (module.ManageBaseForPapers || module.Administration), (action == CallStandardAction.List && idCommunity == 0), (idCommunity == 0), idCommunity, UserContext.CurrentUserID, CallForPaperType.RequestForMembership);
            if (!views.Contains(preloadView) && views.Count == 0)
            {
                preloadView = CallStatusForSubmitters.SubmissionOpened;
                views.Add(CallStatusForSubmitters.SubmissionOpened);
            }
            else if (!views.Contains(preloadView))
                preloadView = views[0];

            View.LoadAvailableView(idCommunity, views);
            View.CurrentView = preloadView;
            switch (action)
            {
                case CallStandardAction.List:
                    View.AllowManage = allowManage;
                    if (allowManage)
                        View.SetActionUrl(CallStandardAction.Manage, RootObject.ViewCalls(CallForPaperType.RequestForMembership, CallStandardAction.Manage, idCommunity, preloadView));
                    break;
                case CallStandardAction.Manage:
                    if (module.CreateBaseForPaper || module.Administration)
                        View.SetActionUrl(CallStandardAction.Add, RootObject.AddCall(CallForPaperType.RequestForMembership, idCommunity, View.CurrentView));
                    View.AllowView = allowView;
                    if (allowView)
                        View.SetActionUrl(CallStandardAction.List, RootObject.ViewCalls(CallForPaperType.RequestForMembership, CallStandardAction.List, idCommunity, preloadView));
                    break;
            }
            if (action == CallStandardAction.Manage)
            {
                List<FilterCallVisibility> filters = Service.GetCallVisibilityFilters(module, (idCommunity == 0), idCommunity, UserContext.CurrentUserID, preloadView);
                View.LoadAvailableFilters(filters, (filters.Count==0) ? FilterCallVisibility.OnlyVisible : filters[0]);
            }

        }

        public void LoadCalls(int pageIndex, int pageSize)
        {
            LoadCalls(Service.RequestForMembershipServicePermission( UserContext.CurrentUserID,View.IdCallCommunity),  View.CurrentAction, View.IdCallCommunity, View.CurrentView, pageIndex, pageSize);
        }
        public void LoadCalls(ModuleRequestForMembership module, CallStandardAction action, int idCommunity, CallStatusForSubmitters view, int pageIndex, int pageSize)
        {
            Boolean fromAllcommunities = false;
            PagerBase pager = new PagerBase();
            pager.PageSize = pageSize;
            if (action == CallStandardAction.Manage)
                pager.Count = (int)Service.CallCount((idCommunity == 0), idCommunity, UserContext.CurrentUserID, CallForPaperType.RequestForMembership, view, View.CurrentFilter) - 1;
            else{
                fromAllcommunities = (idCommunity == 0) && (view == CallStatusForSubmitters.SubmissionOpened || view == CallStatusForSubmitters.Submitted || view == CallStatusForSubmitters.ToEvaluate || view == CallStatusForSubmitters.Evaluated);
                pager.Count = (int)Service.CallCountBySubmission(false,fromAllcommunities, (idCommunity == 0), idCommunity, UserContext.CurrentUserID, view) - 1;
            }
            pager.PageIndex = pageIndex;// Me.View.CurrentPageIndex
            View.Pager = pager;
            if (pager.Count<0)
                View.LoadCalls(new List<dtoRequestItemPermission>());
            else
                View.LoadCalls(Service.GetRequests(module, action, fromAllcommunities, (idCommunity == 0), idCommunity, UserContext.CurrentUserID, view, View.CurrentFilter, pager.PageIndex, pageSize));
        }
        public void VirtualDelete(long idCall)
        {
            VirtualDelete(idCall, true);
        }
        public void UnDelete(long idCall)
        {
            VirtualDelete(idCall, false);
        }

        private void VirtualDelete(long idCall, Boolean delete) {
            if (UserContext.isAnonymous)
                View.DisplaySessionTimeout();
            else
            {
                int idCommunity = 0;
                BaseForPaper call = Service.VirtualDeleteCall(idCall, UserContext.CurrentUserID, delete);
                if (call != null)
                {
                    if (!call.IsPortal && call.Community != null)
                        idCommunity = call.Community.Id;
                    View.SendUserAction(idCommunity, Service.ServiceModuleID(), idCall, (delete) ? ModuleRequestForMembership.ActionType.VirtualDeleteRequest : ModuleRequestForMembership.ActionType.VirtualUndeleteRequest);
                }
                idCommunity = View.IdCallCommunity;
                CallStandardAction action = View.CurrentAction;
                ModuleRequestForMembership module = Service.RequestForMembershipServicePermission(UserContext.CurrentUserID, idCommunity);
                FilterCallVisibility filter = View.CurrentFilter;
                InitializeView(action, module, idCommunity, View.CurrentView);
                View.CurrentFilter = filter;
                LoadCalls(module, action, idCommunity, View.CurrentView, View.Pager.PageIndex, currentPageSize);
            }
        }

        public void CloneCall(long idCall, String prefix, String filePath,String thumbnailPath)
        {
            if (UserContext.isAnonymous)
                View.DisplaySessionTimeout();
            else
            {
                int idCommunity = View.IdCallCommunity;
                int idUser = UserContext.CurrentUserID;
                CallStandardAction action = View.CurrentAction;
                ModuleRequestForMembership module = Service.RequestForMembershipServicePermission(idUser, idCommunity);
                if (module.CreateBaseForPaper || module.Administration || module.ManageBaseForPapers || module.EditBaseForPaper)
                {
                    Boolean reloadPage = (View.CurrentView== CallStatusForSubmitters.Draft);
                    BaseForPaper call = Service.CloneCall(idCall, idUser, idCommunity, prefix, filePath, thumbnailPath);
                    if (call != null)
                    {
                        View.SendUserAction(idCommunity, Service.ServiceModuleID(), idCall, ModuleRequestForMembership.ActionType.CloneRequest);
                        View.CloneSkinAssociation(idUser, Service.ServiceModuleID(), ((call.IsPortal || call.Community == null) ? 0 : call.Community.Id), idCall, call.Id, (Int32)ModuleRequestForMembership.ObjectType.RequestForMembership, typeof(RequestForMembership).FullName);
                    }
                    if (reloadPage || call==null){
                        FilterCallVisibility filter = View.CurrentFilter;
                        InitializeView(action, module, idCommunity, View.CurrentView);
                        View.CurrentFilter = filter;
                        LoadCalls(module, action, idCommunity, View.CurrentView, View.Pager.PageIndex, currentPageSize);
                    }
                    else
                        View.GotoUrl(RootObject.ViewCalls(call.Id,CallForPaperType.RequestForMembership,View.CurrentAction, idCommunity, CallStatusForSubmitters.Draft));
                }
                else
                    View.DisplayNoPermission(idCommunity, Service.ServiceModuleID());
            }
        }

        public void Delete(long idCall,  String baseFilePath, String baseThumbnailPath)
        {
            if (UserContext.isAnonymous)
                View.DisplaySessionTimeout();
            else
            {
                int idCommunity = View.IdCallCommunity;
                int idUser = UserContext.CurrentUserID;
                CallStandardAction action = View.CurrentAction;
                ModuleRequestForMembership module = Service.RequestForMembershipServicePermission(idUser, idCommunity);
                BaseForPaper call = Service.GetCall(idCall);
                if (call == null)
                    View.DisplayUnableToDeleteUnknownCall();
                else if (module.Administration || module.ManageBaseForPapers || (module.DeleteOwnBaseForPaper && call.CreatedBy != null && call.CreatedBy.Id == idUser))
                {
                    idCommunity =((call.IsPortal || call.Community == null) ? 0 : call.Community.Id);
                    if (Service.DeleteCall(idCall, baseFilePath, baseThumbnailPath))
                    {
                        View.SendUserAction(idCommunity, Service.ServiceModuleID(), idCall, ModuleRequestForMembership.ActionType.DeleteRequest);
                        View.RemoveSkinAssociation(idUser, Service.ServiceModuleID(), idCommunity, idCall, (Int32)ModuleRequestForMembership.ObjectType.RequestForMembership, typeof(RequestForMembership).FullName);
                    }
                    else
                        View.DisplayUnableToDeleteCall();
                }
                FilterCallVisibility filter = View.CurrentFilter;
                InitializeView(action, module, idCommunity, View.CurrentView);
                View.CurrentFilter = filter;
                LoadCalls(module, action, idCommunity, View.CurrentView, View.Pager.PageIndex, currentPageSize);
            }
        }
    }
}