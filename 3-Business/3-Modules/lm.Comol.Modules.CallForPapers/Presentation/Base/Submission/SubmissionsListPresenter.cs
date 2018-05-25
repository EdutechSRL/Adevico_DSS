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
    public class SubmissionsListPresenter : lm.Comol.Core.DomainModel.Common.DomainPresenter
    {
         #region "Initialize"
            private ServiceCallOfPapers _Service;
            private ServiceRequestForMembership _ServiceRequest;
        
            public virtual BaseModuleManager CurrentManager { get; set; }
            protected virtual IViewSubmissionsList View
            {
                get { return (IViewSubmissionsList)base.View; }
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
            public SubmissionsListPresenter(iApplicationContext oContext)
                : base(oContext)
            {
                this.CurrentManager = new BaseModuleManager(oContext);
            }
            public SubmissionsListPresenter(iApplicationContext oContext, IViewSubmissionsList view)
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

            CallForPaperType type = View.PreloadCallType;
            long idCall = View.PreloadIdCall;
            dtoBaseForPaper call = Service.GetDtoBaseCall(idCall);
            int idCommunity =  0;
            if (call != null)
            {
                type = call.Type;
                if (call.IsPortal)
                    idCommunity = 0;
                else if (call.Community != null)
                    idCommunity = call.Community.Id;
                else
                    idCommunity = View.PreloadIdCommunity;
                View.IdCallCommunity = idCommunity;
            }
            else
            {
                idCommunity = SetCallsCurrentCommunity();
            }
                

            View.CallType = type;
            View.IdCall = idCall;
            View.CurrentView = View.PreloadView;
            if (UserContext.isAnonymous)
            {
                View.DisplaySessionTimeout();
            }
            else
            {
                Int32 idUser = UserContext.CurrentUserID;
                Boolean allowView = false;
                Boolean allowManage = false;
                Boolean allowEvaluate = false;

                Int32 idCallModule = Service.ServiceModuleID();
                switch(type){
                    case CallForPaperType.CallForBids:
                        //ToDo: add permission on Advance
                        
                        Advanced.SubmissionListPermission advPermission = Service.SubmissionCanList(idCall);
                        
                        if((advPermission & Advanced.SubmissionListPermission.View) == Advanced.SubmissionListPermission.View)
                        {
                            allowView = true;
                        }

                        if ((advPermission & Advanced.SubmissionListPermission.Evaluate) == Advanced.SubmissionListPermission.Evaluate)
                        {
                            allowManage = true;
                        }
                        
                        ModuleCallForPaper module = Service.CallForPaperServicePermission(idUser, idCommunity);
                        allowView = allowView || (module.ViewCallForPapers || module.Administration || module.ManageCallForPapers);
                        allowManage = allowManage || (module.CreateCallForPaper || module.Administration || module.ManageCallForPapers || module.EditCallForPaper);
                        View.HasSignature = (call != null) && call.AttachSign;
                        
                        break;

                    case CallForPaperType.RequestForMembership:
                        ModuleRequestForMembership moduleR = Service.RequestForMembershipServicePermission(idUser, idCommunity);
                        allowView = (moduleR.ViewBaseForPapers || moduleR.Administration || moduleR.ManageBaseForPapers);
                        allowManage = (moduleR.CreateBaseForPaper || moduleR.Administration || moduleR.ManageBaseForPapers || moduleR.EditBaseForPaper);
                        idCallModule = ServiceRequest.ServiceModuleID();
                        View.HasSignature = false;
                        break;
                }

                switch (action)
                {
                    case CallStandardAction.List:
                        View.AllowManage = allowManage;
                        if (allowManage)
                            View.SetActionUrl(CallStandardAction.Manage, RootObject.ViewCalls(idCall,type, CallStandardAction.Manage, idCommunity, View.PreloadView));
                        break;
                    case CallStandardAction.Manage:
                        View.AllowView = true;
                        View.SetActionUrl(CallStandardAction.List, RootObject.ViewCalls(idCall, type, CallStandardAction.List, idCommunity, View.PreloadView));
                        break;
                }
                View.IdCallModule = idCallModule;
                if (call == null)
                    View.DisplayUnknownCall();
                else if (allowManage)
                {
                    View.AllowExport = true;
                    View.SetContainerName(call.Name, call.Edition, call.Type);
                    InitializeView(allowManage, idCall, type, View.PreloadFilters);
                }
                else if(allowView)
                {
                    View.AllowExport = false;
                    View.SetContainerName(call.Name, call.Edition, call.Type);
                    InitializeView(false, idCall, type, View.PreloadFilters);
                }
                else //ToDo: verificare la possibilità di visualizzare le sottomissioni!!!
                {
                    
                    View.SetContainerName(call.Name, call.Edition, call.Type);
                    View.DisplayNoPermission(idCommunity, idCallModule);
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
                if (community == null && currentCommunity != null)
                    idCommunity = this.UserContext.CurrentCommunityID;
                else if (community==null)
                    idCommunity = 0;
            }
            View.IdCallCommunity = idCommunity;
            return idCommunity;
        }
        private void InitializeView(Boolean allowManage, long idCall, CallForPaperType type, dtoSubmissionFilters filters)
        {
            List<SubmissionFilterStatus> items = Service.GetAvailableSubmissionStatus(idCall);
            if (!items.Contains(filters.Status))
                filters.Status = items[0];
            filters.CallType = type;
            View.CurrentFilters = filters;
            View.LoadSubmissionStatus(items, filters.Status);

            LoadSubmissions(allowManage, idCall,type, filters, View.PreloadPageIndex, View.PreloadPageSize);
        }
        public void LoadSubmissions(Int32 idCommunity, CallForPaperType type, dtoSubmissionFilters filters, int pageIndex, int pageSize)
        {
            Boolean allowManage = true;
            View.CurrentFilters = filters;
            switch (type)
            {
                case CallForPaperType.CallForBids:
                    ModuleCallForPaper module = Service.CallForPaperServicePermission(UserContext.CurrentUserID, idCommunity);
                    allowManage = (module.CreateCallForPaper || module.Administration || module.ManageCallForPapers || module.EditCallForPaper);
                    break;
                case CallForPaperType.RequestForMembership:
                    ModuleRequestForMembership moduleR = Service.RequestForMembershipServicePermission(UserContext.CurrentUserID, idCommunity);
                    allowManage = (moduleR.CreateBaseForPaper || moduleR.Administration || moduleR.ManageBaseForPapers || moduleR.EditBaseForPaper);
                    break;
            }
            if (UserContext.isAnonymous)
                View.DisplaySessionTimeout();

            bool isAdvance = Service.CallIsAdvanced(View.IdCall);

            if(isAdvance)
            {
                allowManage = allowManage || Service.UserIsInCallCommissionPresidentOrSecreatary(View.IdCall, UserContext.CurrentUserID);
            }

            if (allowManage)
                LoadSubmissions(allowManage, View.IdCall, type, filters, pageIndex, pageSize);
            else
                View.DisplayNoPermission(idCommunity, View.IdCallModule);
        }
        private void LoadSubmissions(Boolean allowManage, long idCall, CallForPaperType type, dtoSubmissionFilters filters, int pageIndex, int pageSize)
        {
            PagerBase pager = new PagerBase();
            pager.PageSize = pageSize;

            if (pageSize == 0)
                pageSize = 50;
            pager.Count = (int)Service.SubmissionsCount(idCall, filters) - 1;
            pager.PageIndex = pageIndex;// Me.View.CurrentPageIndex
            View.Pager = pager;

            View.CurrentOrderBy = filters.OrderBy;
            View.CurrentFilterBy = filters.Status;
            View.CurrentAscending = filters.Ascending;
            View.PageSize = pageSize;
            if (pager.Count < 0)
                View.LoadNoSubmissionsFound();
            else
            {
                if (Service.CallIsAdvanced(idCall))
                {
                    View.IsAdvance = true;
                    View.LoadSubmissions(Service.GetSubmissionListIntegration(allowManage, idCall, filters, pager.PageIndex, pageSize));
                }
                else
                {
                    View.IsAdvance = false;
                    View.LoadSubmissions(Service.GetSubmissionList(allowManage, idCall, filters, pager.PageIndex, pageSize));
                }
            }
                
                
            switch (type) { 
                case CallForPaperType.CallForBids:
                    View.SendUserAction(View.IdCallCommunity, View.IdCallModule , ModuleCallForPaper.ActionType.LoadSubmissionsList);
                    break;
                case CallForPaperType.RequestForMembership:
                    View.SendUserAction(View.IdCallCommunity, View.IdCallModule, ModuleRequestForMembership.ActionType.LoadSubmissionsList);
                    break;
            }
        }

        public void VirtualDeleteSubmission(long idSubmission, Boolean delete,  CallForPaperType type, dtoSubmissionFilters filters, int pageIndex, int pageSize)
        {
            if (UserContext.isAnonymous)
                View.DisplaySessionTimeout();
            else
            {
                Service.VirtualDeleteSubmission(idSubmission, delete);



                switch (View.CallType)
                {
                    case CallForPaperType.CallForBids:
                        if (delete)
                        {
                            View.SendUserAction(View.IdCallCommunity, View.IdCallModule, idSubmission,
                                ModuleCallForPaper.ActionType.VirtualDeleteSubmission);
                        }
                        else
                        {
                            View.SendUserAction(View.IdCallCommunity, View.IdCallModule, idSubmission, ModuleCallForPaper.ActionType.VirtualUndeleteSubmission);
                        }

                        break;

                    case CallForPaperType.RequestForMembership:
                        if (delete)
                        {
                            View.SendUserAction(View.IdCallCommunity, View.IdCallModule, idSubmission,
                                ModuleRequestForMembership.ActionType.VirtualDeleteSubmission);
                        }
                        else
                        {
                            View.SendUserAction(View.IdCallCommunity, View.IdCallModule, idSubmission, ModuleRequestForMembership.ActionType.VirtualUndeleteSubmission);
                        }
                        
                        break;
                }
                //LoadSubmissions(View.IdCallCommunity, type, filters, pageIndex, pageSize);
                View.GotoUrl(RootObject.ViewSubmissions(type, View.IdCall, 0, 0, View.PreloadView, filters.Status, filters.OrderBy, filters.Ascending, pageIndex, pageSize));
            }
        }

        public void PhisicalDeleteSubmission(long idSubmission, String baseFilePath, CallForPaperType type, dtoSubmissionFilters filters, int pageIndex, int pageSize)
        {
            if (UserContext.isAnonymous)
                View.DisplaySessionTimeout();
            else
            {
                Boolean allowManage = false;
                Int32 idCommunity = View.IdCallCommunity;
                switch (View.CallType)
                {
                    case CallForPaperType.CallForBids:
                        ModuleCallForPaper module = Service.CallForPaperServicePermission(UserContext.CurrentUserID, idCommunity);
                        allowManage = (module.CreateCallForPaper || module.Administration || module.ManageCallForPapers || module.EditCallForPaper);
                        break;
                    case CallForPaperType.RequestForMembership:
                        ModuleRequestForMembership moduleR = Service.RequestForMembershipServicePermission(UserContext.CurrentUserID, idCommunity);
                        allowManage = (moduleR.CreateBaseForPaper || moduleR.Administration || moduleR.ManageBaseForPapers || moduleR.EditBaseForPaper);
                        break;
                }
                if (allowManage)
                {

                    Service.PhisicalDeleteSubmission(idSubmission, baseFilePath);



                    switch (View.CallType)
                    {
                        case CallForPaperType.CallForBids:
                            View.SendUserAction(View.IdCallCommunity, View.IdCallModule, idSubmission, ModuleCallForPaper.ActionType.VirtualDeleteSubmission);
                            break;
                        case CallForPaperType.RequestForMembership:
                            View.SendUserAction(View.IdCallCommunity, View.IdCallModule, idSubmission, ModuleRequestForMembership.ActionType.VirtualDeleteSubmission);
                            break;
                    }
                    View.GotoUrl(RootObject.ViewSubmissions(type, View.IdCall, 0, 0, View.PreloadView, filters.Status, filters.OrderBy, filters.Ascending, pageIndex, pageSize));
                }
                else
                    LoadSubmissions(View.IdCallCommunity, type, filters, pageIndex, pageSize);
            }
        }

        public String ExportSubmissions(lm.Comol.Core.DomainModel.Helpers.Export.ExportFileType exportType, dtoSubmissionFilters filters, Dictionary<SubmissionsListTranslations, string> translations, Dictionary<SubmissionStatus, String> status, Dictionary<RevisionStatus, string> revisions)
        {
            Boolean allowManage = false;
            switch (View.CallType)
            {
                case CallForPaperType.CallForBids:
                    ModuleCallForPaper module = Service.CallForPaperServicePermission(UserContext.CurrentUserID, View.IdCallCommunity);
                    allowManage = (module.CreateCallForPaper || module.Administration || module.ManageCallForPapers || module.EditCallForPaper);
                    break;
                case CallForPaperType.RequestForMembership:
                    ModuleRequestForMembership moduleR = Service.RequestForMembershipServicePermission(UserContext.CurrentUserID, View.IdCallCommunity);
                    allowManage = (moduleR.CreateBaseForPaper || moduleR.Administration || moduleR.ManageBaseForPapers || moduleR.EditBaseForPaper);
                    break;
            } return Service.ExportSubmissionList(exportType,allowManage, View.IdCall, filters, translations, status, revisions);
        }

        public String ExportSubmissionsData(lm.Comol.Core.DomainModel.Helpers.Export.ExportFileType exportType, dtoSubmissionFilters filters, Dictionary<SubmissionsListTranslations, string> translations, Dictionary<SubmissionStatus, String> status, Dictionary<RevisionStatus, string> revisions)
        {
            Boolean allowManage = false;
            switch (View.CallType)
            {
                case CallForPaperType.CallForBids:
                    ModuleCallForPaper module = Service.CallForPaperServicePermission(UserContext.CurrentUserID, View.IdCallCommunity);
                    allowManage = (module.CreateCallForPaper || module.Administration || module.ManageCallForPapers || module.EditCallForPaper);
                    break;
                case CallForPaperType.RequestForMembership:
                    ModuleRequestForMembership moduleR = Service.RequestForMembershipServicePermission(UserContext.CurrentUserID, View.IdCallCommunity);
                    allowManage = (moduleR.CreateBaseForPaper || moduleR.Administration || moduleR.ManageBaseForPapers || moduleR.EditBaseForPaper);
                    break;
            } return Service.ExportSubmissionsData(exportType,allowManage, View.IdCall, filters, translations, status, revisions);
        }
    }
}