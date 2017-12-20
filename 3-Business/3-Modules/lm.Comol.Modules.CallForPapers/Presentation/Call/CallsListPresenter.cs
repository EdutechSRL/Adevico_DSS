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
    public class CallsListPresenter : lm.Comol.Core.DomainModel.Common.DomainPresenter
    {
        private const int currentPageSize = 25;
         #region "Initialize"
            private ServiceCallOfPapers _Service;

            public virtual BaseModuleManager CurrentManager { get; set; }
            protected virtual IViewCallsList View
            {
                get { return (IViewCallsList)base.View; }
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
            public CallsListPresenter(iApplicationContext oContext)
                : base(oContext)
            {
                this.CurrentManager = new BaseModuleManager(oContext);
            }
            public CallsListPresenter(iApplicationContext oContext, IViewCallsList view)
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
                ModuleCallForPaper module = Service.CallForPaperServicePermission(UserContext.CurrentUserID, idCommunity);
                Boolean allowView = (module.ViewCallForPapers || module.Administration || module.ManageCallForPapers);
                Boolean allowManage = module.CreateCallForPaper || module.Administration || module.ManageCallForPapers || module.EditCallForPaper;
                View.AllowSubmmissions = (action== CallStandardAction.List) && module.ViewCallForPapers;
                
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

        private void InitializeView(CallStandardAction action, ModuleCallForPaper module, int idCommunity, CallStatusForSubmitters preloadView) {
            InitializeView(action, module, idCommunity, preloadView, (module.ViewCallForPapers || module.Administration || module.ManageCallForPapers), (module.CreateCallForPaper || module.Administration || module.ManageCallForPapers || module.EditCallForPaper));
        }
        private void InitializeView(CallStandardAction action, ModuleCallForPaper module, int idCommunity, CallStatusForSubmitters preloadView, Boolean allowView, Boolean allowManage)
        {
            List<CallStatusForSubmitters> views = Service.GetAvailableViews(action, module, (action == CallStandardAction.List && idCommunity == 0), idCommunity, UserContext.CurrentUserID);
            if (!views.Contains(preloadView) && views.Count == 0)
            {
                preloadView = CallStatusForSubmitters.SubmissionOpened;
                views.Add(CallStatusForSubmitters.SubmissionOpened);
            }
            else if (!views.Where(v => v != CallStatusForSubmitters.Revisions).Contains(preloadView) && views.Where(v => v != CallStatusForSubmitters.Revisions).Any())
                preloadView = views.Where(v => v != CallStatusForSubmitters.Revisions).FirstOrDefault();

            View.LoadAvailableView(idCommunity, views);
            View.CurrentView = preloadView;
            switch (action)
            {
                case CallStandardAction.List:
                    View.AllowManage = allowManage;
                    if (allowManage)
                        View.SetActionUrl(CallStandardAction.Manage, RootObject.ViewCalls(CallForPaperType.CallForBids, CallStandardAction.Manage, idCommunity, preloadView));
                    break;
                case CallStandardAction.Manage:
                    View.AllowView = allowView;
                    if (module.CreateCallForPaper || module.Administration)
                        View.SetActionUrl(CallStandardAction.Add, RootObject.AddCall(CallForPaperType.CallForBids, idCommunity, View.PreloadView));
                    if (allowView)
                        View.SetActionUrl(CallStandardAction.List, RootObject.ViewCalls(CallForPaperType.CallForBids, CallStandardAction.List, idCommunity, preloadView));
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
            LoadCalls(Service.CallForPaperServicePermission( UserContext.CurrentUserID,View.IdCallCommunity),  View.CurrentAction, View.IdCallCommunity, View.CurrentView, pageIndex, pageSize);
        }
        public void LoadCalls(ModuleCallForPaper module,CallStandardAction action, int idCommunity, CallStatusForSubmitters view, int pageIndex, int pageSize)
        {
            Boolean fromAllcommunities = false;
            PagerBase pager = new PagerBase();
            pager.PageSize = pageSize;
            if (action == CallStandardAction.Manage && view != CallStatusForSubmitters.ToEvaluate && view != CallStatusForSubmitters.Evaluated)
                pager.Count = (int)Service.CallCount((idCommunity == 0), idCommunity, UserContext.CurrentUserID, CallForPaperType.CallForBids, view, View.CurrentFilter) - 1;
            else{
                fromAllcommunities = (idCommunity == 0) && (view == CallStatusForSubmitters.SubmissionOpened || view == CallStatusForSubmitters.Submitted || view == CallStatusForSubmitters.ToEvaluate || view == CallStatusForSubmitters.Evaluated);
                pager.Count = (int)Service.CallCountBySubmission(false,fromAllcommunities, (idCommunity == 0), idCommunity, UserContext.CurrentUserID, view) - 1;
            }
            pager.PageIndex = pageIndex;// Me.View.CurrentPageIndex
            View.Pager = pager;
            if (pager.Count < 0)
                View.LoadCalls(new List<dtoCallItemPermission>());
            else
                View.LoadCalls(Service.GetCallForPapers(module, action, fromAllcommunities, (idCommunity == 0), idCommunity, UserContext.CurrentUserID, view, View.CurrentFilter, pager.PageIndex, pageSize));
            View.SendUserActionList(idCommunity, Service.ServiceModuleID());
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
                    View.SendUserAction(idCommunity, Service.ServiceModuleID(), idCall, (delete) ? ModuleCallForPaper.ActionType.VirtualDeleteCallForPaper : ModuleCallForPaper.ActionType.VirtualUndeleteCallForPaper);
                }
                idCommunity = View.IdCallCommunity;
                CallStandardAction action = View.CurrentAction;
                ModuleCallForPaper module = Service.CallForPaperServicePermission(UserContext.CurrentUserID, idCommunity);
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
                ModuleCallForPaper module = Service.CallForPaperServicePermission(idUser, idCommunity);
                if (module.CreateCallForPaper || module.Administration || module.ManageCallForPapers || module.EditCallForPaper) {
                    Boolean reloadPage = (View.CurrentView == CallStatusForSubmitters.Draft);
                    BaseForPaper call = Service.CloneCall(idCall, UserContext.CurrentUserID, idCommunity, prefix, filePath, thumbnailPath);
                    if (call != null) {
                        View.SendUserAction(idCommunity, Service.ServiceModuleID(), idCall, ModuleCallForPaper.ActionType.CloneCall);
                        View.CloneSkinAssociation(idUser,Service.ServiceModuleID(), ((call.IsPortal || call.Community == null) ? 0 : call.Community.Id), idCall, call.Id, (Int32)ModuleCallForPaper.ObjectType.CallForPaper, typeof(CallForPaper).FullName);
                    }
                    if (reloadPage || call == null)
                    {
                        FilterCallVisibility filter = View.CurrentFilter;
                        InitializeView(action, module, idCommunity, View.CurrentView);
                        View.CurrentFilter = filter;
                        LoadCalls(module, action, idCommunity, View.CurrentView, View.Pager.PageIndex, currentPageSize);
                    }
                    else
                        View.GotoUrl(RootObject.ViewCalls(call.Id, CallForPaperType.CallForBids, View.CurrentAction, idCommunity, CallStatusForSubmitters.Draft));
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
                ModuleCallForPaper module = Service.CallForPaperServicePermission(idUser, idCommunity);
                BaseForPaper call = Service.GetCall(idCall);
                if (call == null)
                    View.DisplayUnableToDeleteUnknownCall();
                else if (module.Administration || module.ManageCallForPapers || (module.DeleteOwnCallForPaper && call.CreatedBy != null && call.CreatedBy.Id==idUser))
                {
                    idCommunity = ((call.IsPortal || call.Community == null) ? 0 : call.Community.Id);
                    if (Service.DeleteCall(idCall, baseFilePath, baseThumbnailPath))
                    {
                        View.SendUserAction(idCommunity, Service.ServiceModuleID(), idCall, ModuleCallForPaper.ActionType.DeleteCall );
                        View.RemoveSkinAssociation(idUser, Service.ServiceModuleID(), idCommunity, idCall, (Int32)ModuleCallForPaper.ObjectType.CallForPaper, typeof(CallForPaper).FullName);
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