using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel;
using lm.Comol.Modules.CallForPapers.Domain.Evaluation;
using lm.Comol.Modules.CallForPapers.Business;
using lm.Comol.Core.Business;
using lm.Comol.Modules.CallForPapers.Domain;

namespace lm.Comol.Modules.CallForPapers.Presentation.Evaluation
{
    public class CommitteesSummaryPresenter : lm.Comol.Core.DomainModel.Common.DomainPresenter
    {
         #region "Initialize"
            private ServiceCallOfPapers _ServiceCall;
            private ServiceRequestForMembership _ServiceRequest;
            public virtual BaseModuleManager CurrentManager { get; set; }
            protected virtual IViewCommitteesSummary View
            {
                get { return (IViewCommitteesSummary)base.View; }
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
            public CommitteesSummaryPresenter(iApplicationContext oContext)
                : base(oContext)
            {
                this.CurrentManager = new BaseModuleManager(oContext);
            }
            public CommitteesSummaryPresenter(iApplicationContext oContext, IViewCommitteesSummary view)
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


            if (call.AdvacedEvaluation)
                View.RedirectToAdvance(call.Id);


            int idCommunity = GetCurrentCommunity(call);  
            View.IdCall = idCall;
            View.IdCallModule = idModule;
            View.IdCallCommunity = idCommunity;
            View.CallType = type;

            ModuleCallForPaper module = ServiceCall.CallForPaperServicePermission(idUser, idCommunity);
            Boolean allowAdmin =  ((module.ManageCallForPapers || module.Administration || ((module.CreateCallForPaper || module.EditCallForPaper) && call.Owner.Id == idUser)));

            View.SetActionUrl((allowAdmin) ? CallStandardAction.Manage : CallStandardAction.List, RootObject.ViewCalls(idCall,type, ((allowAdmin) ? CallStandardAction.Manage : CallStandardAction.List), idCommunity, View.PreloadView));

            if (UserContext.isAnonymous)
                View.DisplaySessionTimeout(RootObject.CommitteesSummary(idCall, idCommunity, View.PreloadView, View.PreloadIdSubmitterType, View.PreloadFilterBy, View.PreloadOrderBy, View.PreloadAscending, View.PreloadPageIndex, View.PreloadPageSize, View.GetItemEncoded(View.PreloadSearchForName)));
            else if (call == null)
                View.DisplayUnknownCall(idCommunity, idModule, idCall);
            else if (type == CallForPaperType.RequestForMembership)
                View.DisplayEvaluationUnavailable();
            else if (allowAdmin)
            {
                View.CurrentEvaluationType = call.EvaluationType;
                if (call.EvaluationType == EvaluationType.Dss)
                    View.CallUseFuzzy = Service.CallUseFuzzy(idCall);
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
            List<dtoCommittee> committees = Service.GetCommitteesForSummary(idCall, false);
            if (committees.Count == 1)
                View.DisplaySingleCommittee(committees.Select(c=>c.Id).FirstOrDefault(), idCall, idCommunity, filters);
            else {
                List<EvaluationFilterStatus> items = Service.GetAvailableEvaluationFilterStatus(idCall);
                List<dtoSubmitterType> types = Service.GetSubmitterTypes(idCall, ManageEvaluationsAction.None);
                if (!items.Contains(filters.Status))
                    filters.Status = items[0];
                if (types == null || (types != null && (!types.Any() && !types.Where(i => i.Id == filters.IdSubmitterType).Any())))
                    filters.IdSubmitterType = -1;
                View.CurrentFilters = filters;
                View.LoadAvailableStatus(items, filters.Status);
                View.LoadAvailableSubmitterTypes(types, filters.IdSubmitterType);
                View.CurrentCommittees = committees;
                View.CommitteesCount = committees.Count();
                View.PageSize = View.PreloadPageSize;
                View.CurrentPageIndex = View.PreloadPageIndex;
                LoadEvaluations(idCall, type, idCommunity, committees, filters, View.PreloadPageIndex, View.PreloadPageSize);
            }
        }
        public void LoadEvaluations(long idCall, EvaluationType type, Int32 idCommunity, List<dtoCommittee> committees, dtoEvaluationsFilters filters, int pageIndex, int pageSize)
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
                View.SetBackToSummary(RootObject.EvaluationsSummary(idCall, idCommunity, View.PreloadView , 0, filters.IdSubmitterType, filters.Status, filters.OrderBy, filters.Ascending, pageIndex, pageSize, View.GetItemEncoded(filters.SearchForName)));
                LoadItems(idCall, type, idCommunity, committees, filters, pageIndex, pageSize);
            }
            else
                View.DisplayNoPermission(idCommunity, View.IdCallModule);
        }

        private void LoadItems(long idCall, EvaluationType type, Int32 idCommunity, List<dtoCommittee> committees, dtoEvaluationsFilters filters, int pageIndex, int pageSize)
        {
            if (type == EvaluationType.Dss)
                InitializeDssInfo(idCall);
            else
                View.HideDssWarning();
            List<dtoCommitteesSummaryItem> items = Service.GetCommitteesSummary(idCall, type, filters, View.AnonymousDisplayname, View.UnknownDisplayname, committees);

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

            //if (pageSize == 0)
            //    pageSize = 50;
            //int itemsCount = (int)items.Count-1;
            //if (itemsCount > 0)
            //{
            //    while (!items.Skip(pageIndex * pageSize).Take(pageSize).Any())
            //    {
            //        pageIndex -= 1;
            //    }
            //    View.CurrentPageIndex = pageIndex;
            //}
            //View.CurrentOrderBy = filters.OrderBy;
            //View.CurrentFilterBy = filters.Status;
            //View.CurrentAscending = filters.Ascending;
            //View.PageSize = pageSize;
            View.AllowExportCurrent = (items != null && items.Any() && items.Skip(pageIndex * pageSize).Take(pageSize).Any());  
            //!(itemsCount < 0);
            if (pager.Count < 0)
                View.DisplayNoEvaluationsFound();
            else
                View.LoadEvaluations(committees,items.Skip(pageIndex * pageSize).Take(pageSize).ToList());
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
        public String GetFileName(String filename, ItemsToExport items, ExportData xdata)
        {
            return Service.GetStatisticFileName(View.IdCall,ServiceCall.GetCallName(View.IdCall), filename, SummaryType.Committees, items, xdata);
        }
        public String ExportTo(dtoEvaluationsFilters filters, SummaryType summaryType, ItemsToExport itemsToExport, ExportData xdata, lm.Comol.Core.DomainModel.Helpers.Export.ExportFileType fileType, Dictionary<lm.Comol.Modules.CallForPapers.Domain.Evaluation.EvaluationTranslations, String> translations, Dictionary<lm.Comol.Modules.CallForPapers.Domain.Evaluation.EvaluationStatus, String> status)
        {
            return Service.ExportSummaryStatistics(ServiceCall.GetDtoCall(View.IdCall), filters, View.AnonymousDisplayname, View.UnknownDisplayname,summaryType, itemsToExport, xdata, fileType, translations, status);
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