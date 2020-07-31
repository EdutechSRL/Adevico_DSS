using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel;
using lm.Comol.Modules.CallForPapers.Domain.Evaluation;
using lm.Comol.Modules.CallForPapers.Business;
using lm.Comol.Core.Business;
using lm.Comol.Modules.CallForPapers.Domain;
using lm.Comol.Modules.CallForPapers.Domain.Evaluation.Export;

namespace lm.Comol.Modules.CallForPapers.Presentation.Evaluation
{
    public class CommitteeSummaryPresenter : lm.Comol.Core.DomainModel.Common.DomainPresenter
    {
         #region "Initialize"
            private ServiceCallOfPapers _ServiceCall;
            private ServiceRequestForMembership _ServiceRequest;
            public virtual BaseModuleManager CurrentManager { get; set; }
            protected virtual IViewCommitteeSummary View
            {
                get { return (IViewCommitteeSummary)base.View; }
            }
            private ServiceEvaluation _Service;
            private ServiceEvaluation Service
            {
                get
                {
                    if (_Service == null)
                        _Service = new ServiceEvaluation(AppContext);
                    return _Service;
                }
            }
            private ServiceCallOfPapers ServiceCall
            {
                get
                {
                    if (_ServiceCall == null)
                        _ServiceCall = new ServiceCallOfPapers(AppContext);
                    return _ServiceCall;
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
            public CommitteeSummaryPresenter(iApplicationContext oContext)
                : base(oContext)
            {
                this.CurrentManager = new BaseModuleManager(oContext);
            }
            public CommitteeSummaryPresenter(iApplicationContext oContext, IViewCommitteeSummary view)
                : base(oContext, view)
            {
                this.CurrentManager = new BaseModuleManager(oContext);
            }
        #endregion

        public void InitView()
        {
            Boolean isAnonymousUser = UserContext.isAnonymous;
            long idCall = View.PreloadIdCall;
            Int32 idUser = UserContext.CurrentUserID;

            lm.Comol.Modules.CallForPapers.Domain.CallForPaperType type = ServiceCall.GetCallType(idCall);
            int idModule = (type == CallForPaperType.CallForBids) ? ServiceCall.ServiceModuleID() : ServiceRequest.ServiceModuleID();

            dtoCall call = (type == CallForPaperType.CallForBids) ? ServiceCall.GetDtoCall(idCall) : null;
            if (call != null)
                View.SetContainerName(call.Name, call.EndEvaluationOn);
            int idCommunity = GetCurrentCommunity(call);  
            View.IdCall = idCall;
            View.IdCallModule = idModule;
            View.IdCallCommunity = idCommunity;
            View.CallType = type;

            ModuleCallForPaper module = ServiceCall.CallForPaperServicePermission(idUser, idCommunity);
            Boolean allowAdmin =  ((module.ManageCallForPapers || module.Administration || ((module.CreateCallForPaper || module.EditCallForPaper) && call.Owner.Id == idUser)));

            View.SetActionUrl((allowAdmin) ? CallStandardAction.Manage : CallStandardAction.List, RootObject.ViewCalls(idCall,type, ((allowAdmin) ? CallStandardAction.Manage : CallStandardAction.List), idCommunity, View.PreloadView));

            if (UserContext.isAnonymous)
                View.DisplaySessionTimeout(RootObject.CommitteeSummary(View.PreloadIdCommittee,idCall, idCommunity, View.PreloadView, View.PreloadIdSubmitterType, View.PreloadFilterBy, View.PreloadOrderBy, View.PreloadAscending, View.PreloadPageIndex, View.PreloadPageSize, View.GetItemEncoded(View.PreloadSearchForName)));
            else if (call == null)
                View.DisplayUnknownCall(idCommunity, idModule, idCall);
            else if (type == CallForPaperType.RequestForMembership)
                View.DisplayEvaluationUnavailable();
            else if (allowAdmin)
            {
                View.CurrentEvaluationType = call.EvaluationType;
                if (call.EvaluationType == EvaluationType.Dss)
                    View.CallUseFuzzy = Service.CallUseFuzzy(idCall);
                View.AllowHideComments = true;
                View.AllowExportAll = Service.HasEvaluations(idCall);
                View.DisplayEvaluationInfo(call.EndEvaluationOn);
                InitializeView(idCall, call.EvaluationType,idCommunity, View.PreloadFilters);
            }
        }

        private int GetCurrentCommunity(dtoCall call)
        {
            int idCommunity = 0;
            Community currentCommunity = CurrentManager.GetCommunity(this.UserContext.CurrentCommunityID);
            Community community = null;
            if (call != null)
                idCommunity = (call.IsPortal) ? 0 : (call.Community != null) ? call.Community.Id : 0;
            community = CurrentManager.GetCommunity(idCommunity);

            if (community == null && currentCommunity != null && !call.IsPortal)
                idCommunity = this.UserContext.CurrentCommunityID;
            else if (community==null)
                idCommunity = 0;
            View.IdCallCommunity = idCommunity;
            
            return idCommunity;
        }
        private void InitializeView(long idCall, EvaluationType type, Int32 idCommunity, dtoEvaluationsFilters filters)
        {
            long idCommittee = View.PreloadIdCommittee;
            if (idCommittee <= 0)
                View.DisplayCommitteesSummary(idCall, idCommunity, filters);
            else{
                List<long> items = Service.GetIdCommittees(idCall);
                if (!items.Any())
                {
                    View.AllowExportAll = false;
                    View.DisplayCommitteesSummary(idCall, idCommunity, filters);
                }
                else
                {
                    idCommittee = (items.Contains(idCommittee)) ? idCommittee : items.FirstOrDefault();
                    View.SetCommitteeName(Service.GetCommitteeName(idCommittee));
                    View.CurrentIdCommittee = idCommittee;
                    List<expCommitteeMember> evaluators = Service.GetCommitteeMembersForSummary(idCommittee);
                    View.EvaluatorsCount = evaluators.Count();

                    List<EvaluationFilterStatus> sItems = Service.GetAvailableEvaluationFilterStatus(idCall, idCommittee);
                    List<dtoSubmitterType> types = Service.GetSubmitterTypes(idCall, ManageEvaluationsAction.None, idCommittee);
                    if (!sItems.Contains(filters.Status))
                        filters.Status = sItems[0];
                    if (types == null || (types != null && (!types.Any() && !types.Where(i => i.Id == filters.IdSubmitterType).Any())))
                        filters.IdSubmitterType = -1;
                    View.CurrentFilters = filters;
                    View.LoadAvailableStatus(sItems, filters.Status);
                    View.LoadAvailableSubmitterTypes(types, filters.IdSubmitterType);
                    View.PageSize = View.PreloadPageSize;
                    View.CurrentPageIndex = View.PreloadPageIndex;
                    LoadEvaluations(idCommittee, idCall,type, idCommunity, evaluators, filters, View.PreloadPageIndex, View.PreloadPageSize);
                } 
            }
        }
        public void LoadEvaluations(long idCommittee, EvaluationType type, dtoEvaluationsFilters filters, int pageIndex, int pageSize)
        {
            LoadEvaluations(idCommittee, View.IdCall, type,View.IdCallCommunity, Service.GetCommitteeMembersForSummary(idCommittee), filters, pageIndex, pageSize);
        }
        private void LoadEvaluations(long idCommittee, long idCall, EvaluationType type, Int32 idCommunity, List<expCommitteeMember> evaluators, dtoEvaluationsFilters filters, int pageIndex, int pageSize)
        {
            Boolean allowManage = true;
            View.CurrentFilters = filters;

            ModuleCallForPaper module = Service.CallForPaperServicePermission(UserContext.CurrentUserID, idCommunity);
            allowManage = (module.CreateCallForPaper || module.Administration || module.ManageCallForPapers || module.EditCallForPaper);

            if (UserContext.isAnonymous)
                View.DisplaySessionTimeout();
            else if (allowManage)
            {
                View.AllowBackToSummary = true;
                View.SetBackToSummary(RootObject.EvaluationsSummary(idCall, idCommunity, View.PreloadView, 0, filters.IdSubmitterType, filters.Status, filters.OrderBy, filters.Ascending, pageIndex, pageSize, View.GetItemEncoded(filters.SearchForName)));
                LoadItems(idCommittee,idCall,type, idCommunity, evaluators, filters, pageIndex, pageSize);
            }
            else
                View.DisplayNoPermission(idCommunity, View.IdCallModule);
        }
        private void LoadItems(long idCommittee, long idCall, EvaluationType type, Int32 idCommunity, List<expCommitteeMember> evaluators, dtoEvaluationsFilters filters, int pageIndex, int pageSize)
        {
            if (type == EvaluationType.Dss)
                InitializeDssInfo(idCall);
            else
                View.HideDssWarning();
            List<dtoBaseCommitteeMember> members = evaluators.Select(e => new dtoBaseCommitteeMember(e, View.AnonymousDisplayname)).ToList().OrderBy(e => e.DisplayName).ThenByDescending(e => e.IdMembership).ToList();
            List<dtoCommitteeSummaryItem> items = Service.GetCommitteeSummary(idCommittee,idCall,type, filters, View.AnonymousDisplayname,View.UnknownDisplayname, members, evaluators);

            View.AllowHideComments = items.Where(i => i.HasComments()).Any();
            PagerBase pager = new PagerBase();
            pager.PageSize = pageSize;

            if (pageSize == 0)
                pageSize = 50;
            pager.Count = (int)items.Count - 1;
            pager.PageIndex = pageIndex;// Me.View.CurrentPageIndex
            View.Pager = pager;

            View.CurrentOrderBy = filters.OrderBy;
            View.CurrentFilterBy = filters.Status;
            View.CurrentAscending = filters.Ascending;
            View.PageSize = pageSize;

            if (pager.Count < 0)
            {
                View.AllowExportCurrent = false;
                View.DisplayNoEvaluationsFound();
            }
            else
            {
                View.AllowExportCurrent = true;
                View.LoadEvaluations(idCommittee, Service.GetCommitteesForSummary(idCall, false),members , items.Skip(pageIndex * pageSize).Take(pageSize).ToList());
            }
            View.SendUserAction(View.IdCallCommunity, View.IdCallModule, ModuleCallForPaper.ActionType.ViewEvaluationsSummary); 
        }
        private void InitializeDssInfo(long idCall)
        {
            List<DssCallEvaluation> items = Service.DssRatingGetValues(idCall);
            DateTime lastUpdate = (items.Any() ? items.Select(i => i.LastUpdateOn).Min() : DateTime.MinValue);
            if (Service.DssRatingMustUpdate(idCall, lastUpdate))
            {
                Service.DssRatingSetForCall(idCall, out lastUpdate);
                items = Service.DssRatingGetValues(idCall);
            }
            if (items.Any())
                View.DisplayDssWarning(lastUpdate, !items.Any(i => !i.IsCompleted || !i.IsValid));
            else
                View.HideDssWarning();

        }
        public String GetFileName(String filename, SummaryType summaryType, ItemsToExport items, ExportData xdata)
        {
            return Service.GetStatisticFileName(View.IdCall, ServiceCall.GetCallName(View.IdCall), Service.GetCommitteeDisplayOrder(View.CurrentIdCommittee), Service.GetCommitteeName(View.CurrentIdCommittee), filename, summaryType, items, xdata);
        }
        public String ExportTo(dtoEvaluationsFilters filters, SummaryType summaryType, ItemsToExport itemsToExport, ExportData xdata,lm.Comol.Core.DomainModel.Helpers.Export.ExportFileType fileType,  Dictionary<lm.Comol.Modules.CallForPapers.Domain.Evaluation.EvaluationTranslations, String> translations, Dictionary<lm.Comol.Modules.CallForPapers.Domain.Evaluation.EvaluationStatus, String> status)
        {
            return Service.ExportSummaryStatistics(
                ServiceCall.GetDtoCall(View.IdCall), 
                filters, 
                View.AnonymousDisplayname, 
                View.UnknownDisplayname, 
                summaryType, 
                itemsToExport, 
                xdata, 
                fileType, 
                translations, 
                status, 
                View.CurrentIdCommittee);
        }
        private litePerson GetCurrentUser(ref Int32 idUser)
        {
            litePerson person = null;
            if (UserContext.isAnonymous)
            {
                person = (from p in CurrentManager.GetIQ<litePerson>() where p.TypeID == (int)UserTypeStandard.Guest select p).Skip(0).Take(1).ToList().FirstOrDefault();//CurrentManager.GetLitePerson(UserContext.CurrentUserID);
                idUser = (person != null) ? person.Id : UserContext.CurrentUserID; //if(Person!=null) { IdUser = PersonId} else {IdUser = UserContext...}
            }
            else
                person = CurrentManager.GetLitePerson(idUser);
            return person;
        }
    }
}