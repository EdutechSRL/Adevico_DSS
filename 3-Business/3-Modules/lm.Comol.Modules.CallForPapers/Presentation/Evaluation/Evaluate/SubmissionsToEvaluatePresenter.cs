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
    public class SubmissionsToEvaluatePresenter : lm.Comol.Core.DomainModel.Common.DomainPresenter
    {
         #region "Initialize"
            private ServiceCallOfPapers _ServiceCall;
            private ServiceRequestForMembership _ServiceRequest;
            public virtual BaseModuleManager CurrentManager { get; set; }
            protected virtual IViewSubmissionsToEvaluate View
            {
                get { return (IViewSubmissionsToEvaluate)base.View; }
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
            public SubmissionsToEvaluatePresenter(iApplicationContext oContext)
                : base(oContext)
            {
                this.CurrentManager = new BaseModuleManager(oContext);
            }
            public SubmissionsToEvaluatePresenter(iApplicationContext oContext, IViewSubmissionsToEvaluate view)
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

            if(call != null && call.AdvacedEvaluation)
            {
                //Advanced.dto.dtoCommEvalInfo evInfo = ServiceCall.CommissionEvalTypeGet(View.IdAdvCommittee);

                //if (evInfo.Type == Advanced.EvalType.Average)
                //    View.CurrentEvaluationType = EvaluationType.Average;
                //else
                //    View.CurrentEvaluationType = EvaluationType.Sum;


                //Il TIPO di valutazione IMPOSTATO ha impatto SOLO nell'aggregazione tra COMMISSARI.
                //Nell'aggregazione tra i criteri di un VALUTATORE, uso SEMPRE E SOLO LA SOMMA!
                View.CurrentEvaluationType = EvaluationType.Sum;


            } else
            {
                View.CurrentEvaluationType = (call != null ? call.EvaluationType : EvaluationType.Sum);
            }

            



            dtoEvaluationsFilters filters = View.PreloadFilters;
            ModuleCallForPaper module = ServiceCall.CallForPaperServicePermission(idUser, idCommunity);
            
            Boolean allowEvaluate = false;
            Boolean allowAdmin =  ((module.ManageCallForPapers || module.Administration || ((module.CreateCallForPaper || module.EditCallForPaper) && call.Owner.Id == idUser)));

            View.SetActionUrl(
                (allowAdmin) ? CallStandardAction.Manage : CallStandardAction.List, 
                RootObject.ViewCalls(idCall,
                    type, 
                    ((allowAdmin) ? CallStandardAction.Manage : CallStandardAction.List), 
                    idCommunity, 
                    View.PreloadView),
                call.AdvacedEvaluation,
                RootObject.AdvCommissionEdit(idCall, View.IdAdvCommittee, Advanced.CommiteeEditPage.Members));

            if (UserContext.isAnonymous)
                View.DisplaySessionTimeout(RootObject.ViewSubmissionsToEvaluate(CallForPaperType.CallForBids, idCall, idCommunity, View.PreloadView, View.PreloadOrderBy, View.PreloadAscending, filters.IdSubmitterType, filters.Status, View.GetItemEncoded(filters.SearchForName)));
            if (call == null)
                View.DisplayUnknownCall(idCommunity, idModule, idCall, type);
            else if (type == CallForPaperType.RequestForMembership)
                View.DisplayEvaluationUnavailable();
            else
            {
                if(call.AdvacedEvaluation)
                {
                    Advanced.EvaluationAdvPermission advPermission = ServiceCall.EvaluationAdvPermission(idCall, View.IdAdvCommittee);

                    allowEvaluate = ((advPermission & Advanced.EvaluationAdvPermission.Evaluate) == Advanced.EvaluationAdvPermission.Evaluate);
                    bool allowView = ((advPermission & Advanced.EvaluationAdvPermission.View) == Advanced.EvaluationAdvPermission.View);

                    if (allowEvaluate | allowView)
                    {
                        View.AllowEvaluate = allowEvaluate; // (allowEvaluate && (!call.EndEvaluationOn.HasValue || call.EndEvaluationOn.Value >= DateTime.Now));
                        View.DisplayEvaluationInfo(call.EndEvaluationOn);
                        long idMember = ServiceCall.MemberIdGet(View.IdAdvCommittee);
                        InitalizeViewAdv(idCall, idCommunity, View.IdAdvCommittee, idMember, filters);
                    } else
                    {

                    }

                }
                else
                {
                    long idEvaluator = Service.GetIdEvaluator(idCall, UserContext.CurrentUserID);
                    Boolean isEvaluator = (idEvaluator > 0);//Service.isEvaluator(idCall, UserContext.CurrentUserID);
                    allowEvaluate = isEvaluator; //(isEvaluator || module.ManageCallForPapers || module.Administration || ((module.CreateCallForPaper || module.EditCallForPaper) && call.Owner.Id == idUser));
                    View.IdEvaluator = idEvaluator;
                    if (!allowEvaluate)
                        View.DisplayEvaluationUnavailable();
                    else if (!isEvaluator)
                        View.DisplayNotEvaluationPermission();
                    else
                    {
                        View.AllowEvaluate = (allowEvaluate && (!call.EndEvaluationOn.HasValue || call.EndEvaluationOn.Value >= DateTime.Now));
                        View.DisplayEvaluationInfo(call.EndEvaluationOn);
                        InitalizeView(idCall, idCommunity, View.PreloadIdCommittee, idEvaluator, filters);
                    }
                }
                



               
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
        private void InitalizeView(long idCall,Int32 idCommunity, long idCommittee, long idMember, dtoEvaluationsFilters filters )
        {
            LoadCommitteData(idCall,idCommunity, idCommittee, idMember, InitializeFilters(idCall,idCommittee, idMember, filters));
        }

        private void InitalizeViewAdv(long idCall, Int32 idCommunity, long idCommittee, long idMember, dtoEvaluationsFilters filters)
        {
            LoadCommitteDataAdv(idCall, idCommunity, idCommittee, idMember, InitializeFiltersAdv(idCall, idCommittee, filters));
        }

        private dtoEvaluationsFilters InitializeFilters(long idCall, long idCommittee, long idMember, dtoEvaluationsFilters filters)
        {
            List<EvaluationFilterStatus> items = Service.GetAvailableEvaluationFilterStatus(idCall, idCommittee, idMember);
            List<dtoSubmitterType> types = Service.GetSubmitterTypes(idCall, ManageEvaluationsAction.None, idCommittee);
            if (!items.Contains(filters.Status))
                filters.Status = items[0];
            if (types == null || (types != null && (!types.Any() && !types.Where(i => i.Id == filters.IdSubmitterType).Any())))
                filters.IdSubmitterType = -1;
            View.CurrentFilters = filters;
            View.LoadAvailableStatus(items, filters.Status);
            View.LoadAvailableSubmitterTypes(types, filters.IdSubmitterType);
            return filters;
        }
        private dtoEvaluationsFilters InitializeFiltersAdv(long idCall, long idCommittee, dtoEvaluationsFilters filters)
        {
            List<EvaluationFilterStatus> items = Service.GetAvailableEvaluationFilterStatusAdv(idCall, idCommittee);

            List<dtoSubmitterType> types = Service.GetSubmitterTypes(idCall, ManageEvaluationsAction.None, idCommittee);
            if (!items.Contains(filters.Status))
                filters.Status = items[0];
            if (types == null || (types != null && (!types.Any() && !types.Where(i => i.Id == filters.IdSubmitterType).Any())))
                filters.IdSubmitterType = -1;
            View.CurrentFilters = filters;
            View.LoadAvailableStatus(items, filters.Status);
            View.LoadAvailableSubmitterTypes(types, filters.IdSubmitterType);
            return filters;
        }
        public void LoadCommitteData(long idCommittee, dtoEvaluationsFilters filters)
        {
            LoadCommitteData(View.IdCall,View.IdCallCommunity , idCommittee, View.IdEvaluator, filters);
        }
       
        private void LoadCommitteData(long idCall,Int32 idCommunity,  long idCommittee, long idEvaluator, dtoEvaluationsFilters filters)
        {

            bool isAdvance = ServiceCall.CallIsAdvanced(idCall);
            if(isAdvance)
            {
                LoadCommitteDataAdv(idCall, idCommunity, idCommittee, idEvaluator, filters);
                return;
            }

            List<dtoCommitteeEvaluationsInfo> committees = 
                isAdvance ?
                ServiceCall.GetCommitteesEvaluationInfoAdv(idCall, idEvaluator, idCommittee) :
                Service.GetCommitteesEvaluationInfo(idCall, idEvaluator);

            View.CurrentFilters = filters;
            View.CurrentOrderBy = filters.OrderBy;
            //View.CurrentFilterBy = filters.Status;
            View.CurrentAscending = filters.Ascending;
            if (committees.Any()){
                String name = View.GetItemEncoded(filters.SearchForName);
                foreach(dtoCommitteeEvaluationsInfo c in committees){
                    c.NavigationUrl = RootObject.ViewSubmissionsToEvaluate(c.IdCommittee, CallForPaperType.CallForBids, idCall, idCommunity, View.PreloadView, SubmissionsOrder.ByUser,true, filters.IdSubmitterType, filters.Status, name);
                }
                View.AllowExportAll = (committees.Count > 1);
                View.AllowExportCurrent = true;
                if (!committees.Where(c=> c.IdCommittee==idCommittee).Any())
                    idCommittee = committees.Select(c=>c.IdCommittee).FirstOrDefault();
                View.IdCurrentCommittee= idCommittee;

                dtoEvaluatorCommitteeStatistic statistic =
                    isAdvance ?
                    ServiceCall.GetEvaluatorCommitteeStatistics(
                        View.CurrentEvaluationType,
                        filters,
                        View.AnonymousDisplayname,
                        View.UnknownDisplayname,
                        idCommittee,
                        idEvaluator
                        ) :
                    Service.GetEvaluatorCommitteeStatistics(
                        View.CurrentEvaluationType,
                        filters, 
                        View.AnonymousDisplayname, 
                        View.UnknownDisplayname, 
                        idCommittee, 
                        idEvaluator);

                Dictionary<SubmissionsOrder, Boolean> allowedReorder = new Dictionary<SubmissionsOrder, Boolean>(); //{{ SubmissionsOrder.ByEvaluationIndex, true}};
                Boolean allow = (statistic!= null && statistic.Evaluations != null && statistic.Evaluations.Count > 1);
                allowedReorder.Add(SubmissionsOrder.ByEvaluationIndex, allow);
                allowedReorder.Add(SubmissionsOrder.ByUser, allow);
                allowedReorder.Add(SubmissionsOrder.ByEvaluationPoints, allow);
                allowedReorder.Add(SubmissionsOrder.ByType, allow && statistic.Evaluations.Select(t=>t.SubmitterType).Distinct().Count() > 1);
                View.AvailableOrderBy = allowedReorder;

                View.CriteriaCount = statistic.Criteria.Where(c => c.Deleted == BaseStatusDeleted.None).Count();

                
                dtoBaseEvaluatorStatistics globalStat = new dtoBaseEvaluatorStatistics();


               globalStat.Counters[Domain.Evaluation.EvaluationStatus.Evaluated] = committees.Where(c => c.Counters.ContainsKey(Domain.Evaluation.EvaluationStatus.Evaluated)).Select(c => c.Counters[Domain.Evaluation.EvaluationStatus.Evaluated]).Sum();
               globalStat.Counters[Domain.Evaluation.EvaluationStatus.Evaluating] = committees.Where(c => c.Counters.ContainsKey(Domain.Evaluation.EvaluationStatus.Evaluating)).Select(c => c.Counters[Domain.Evaluation.EvaluationStatus.Evaluating]).Sum();
               globalStat.Counters[Domain.Evaluation.EvaluationStatus.None] = committees.Where(c => c.Counters.ContainsKey(Domain.Evaluation.EvaluationStatus.None)).Select(c => c.Counters[Domain.Evaluation.EvaluationStatus.None]).Sum();


                View.LoadEvaluationData(globalStat, committees, statistic, filters.IdSubmitterType, filters.Status, (String.IsNullOrEmpty(filters.SearchForName) ? "" : View.GetItemEncoded(filters.SearchForName)));
            }
            else{
                View.AllowEvaluate=false;
                View.DisplayEvaluationUnavailable();
            }
        }

        private void LoadCommitteDataAdv(long idCall, Int32 idCommunity, long idCommittee, long idMember, dtoEvaluationsFilters filters)
        {   

            List<dtoCommitteeEvaluationsInfo> committees = ServiceCall.GetCommitteesEvaluationInfoAdv(idCall, idMember, idCommittee);

            View.CurrentFilters = filters;
            View.CurrentOrderBy = filters.OrderBy;
            //View.CurrentFilterBy = filters.Status;
            View.CurrentAscending = filters.Ascending;
            if (committees.Any())
            {
                String name = View.GetItemEncoded(filters.SearchForName);
                foreach (dtoCommitteeEvaluationsInfo c in committees)
                {
                    c.NavigationUrl = RootObject.ViewSubmissionsToEvaluate(
                        c.IdCommittee, 
                        CallForPaperType.CallForBids, 
                        idCall, 
                        idCommunity, 
                        View.PreloadView, 
                        SubmissionsOrder.ByUser, 
                        true, 
                        filters.IdSubmitterType, 
                        filters.Status, 
                        name);
                }
                View.AllowExportAll = (committees.Count > 1);
                View.AllowExportCurrent = true;
                if (!committees.Where(c => c.IdCommittee == idCommittee).Any())
                    idCommittee = committees.Select(c => c.IdCommittee).FirstOrDefault();
                View.IdCurrentCommittee = idCommittee;

                //dtoEvaluatorCommitteeStatistic statistic = Service.GetEvaluatorCommitteeStatistics(View.CurrentEvaluationType, filters, View.AnonymousDisplayname, View.UnknownDisplayname, idCommittee, idEvaluator);
                dtoEvaluatorCommitteeStatistic statistic = 
                    ServiceCall.GetEvaluatorCommitteeStatistics(
                        View.CurrentEvaluationType, 
                        filters, 
                        View.AnonymousDisplayname, 
                        View.UnknownDisplayname, 
                        idCommittee, idMember);


                Dictionary<SubmissionsOrder, Boolean> allowedReorder = new Dictionary<SubmissionsOrder, Boolean>(); //{{ SubmissionsOrder.ByEvaluationIndex, true}};
                Boolean allow = (statistic != null && statistic.Evaluations != null && statistic.Evaluations.Count > 1);
                allowedReorder.Add(SubmissionsOrder.ByEvaluationIndex, allow);
                allowedReorder.Add(SubmissionsOrder.ByUser, allow);
                allowedReorder.Add(SubmissionsOrder.ByEvaluationPoints, allow);
                allowedReorder.Add(SubmissionsOrder.ByType, allow && statistic.Evaluations.Select(t => t.SubmitterType).Distinct().Count() > 1);
                View.AvailableOrderBy = allowedReorder;

                View.CriteriaCount = statistic.Criteria.Where(c => c.Deleted == BaseStatusDeleted.None).Count();


                dtoBaseEvaluatorStatistics globalStat = new dtoBaseEvaluatorStatistics();
                globalStat.Counters[Domain.Evaluation.EvaluationStatus.Evaluated] = committees.Where(c => c.Counters.ContainsKey(Domain.Evaluation.EvaluationStatus.Evaluated)).Select(c => c.Counters[Domain.Evaluation.EvaluationStatus.Evaluated]).Sum();
                globalStat.Counters[Domain.Evaluation.EvaluationStatus.Evaluating] = committees.Where(c => c.Counters.ContainsKey(Domain.Evaluation.EvaluationStatus.Evaluating)).Select(c => c.Counters[Domain.Evaluation.EvaluationStatus.Evaluating]).Sum();
                globalStat.Counters[Domain.Evaluation.EvaluationStatus.None] = committees.Where(c => c.Counters.ContainsKey(Domain.Evaluation.EvaluationStatus.None)).Select(c => c.Counters[Domain.Evaluation.EvaluationStatus.None]).Sum();

               try
                { 
                    globalStat.Counters[Domain.Evaluation.EvaluationStatus.Confirmed] = committees.Where(c => c.Counters.ContainsKey(Domain.Evaluation.EvaluationStatus.Confirmed)).Select(c => c.Counters[Domain.Evaluation.EvaluationStatus.Confirmed]).Sum();
                } catch { }
                
                View.LoadEvaluationData(globalStat, committees, statistic, filters.IdSubmitterType, filters.Status, (String.IsNullOrEmpty(filters.SearchForName) ? "" : View.GetItemEncoded(filters.SearchForName)));
            }
            else
            {
                View.AllowEvaluate = false;
                View.DisplayEvaluationUnavailable();
            }
        }

        public String GetFileName(String filename, long idCommittee)
        {
            String result = filename;
            DateTime data = DateTime.Now;
            String commissionName = (idCommittee>0) ? Service.GetCommitteeName(idCommittee) : "";
            String callName = ServiceCall.GetCallName(View.IdCall);

            if (!String.IsNullOrEmpty(commissionName))
                commissionName = commissionName.Trim();
            if (idCommittee > 0 && !String.IsNullOrEmpty(commissionName) && commissionName.Length > 30)
                commissionName = Service.GetCommitteeDisplayOrder(idCommittee).ToString();

            if (idCommittee > 0)
                result = String.Format(filename, commissionName, data.Year, ((data.Month < 10) ? "0" : "") + data.Month.ToString(), ((data.Day < 10) ? "0" : "") + data.Day.ToString());
            else
                result = String.Format(filename, callName, data.Year, ((data.Month < 10) ? "0" : "") + data.Month.ToString(), ((data.Day < 10) ? "0" : "") + data.Day.ToString());

            return lm.Comol.Core.DomainModel.Helpers.Export.ExportBaseHelper.HtmlCheckFileName(result);
        }
        public String ExportTo(dtoEvaluationsFilters filters, long idCommittee, long idEvaluator, Boolean applyFilters,lm.Comol.Core.DomainModel.Helpers.Export.ExportFileType fileType, Dictionary<lm.Comol.Modules.CallForPapers.Domain.Evaluation.EvaluationTranslations, String> translations, Dictionary<lm.Comol.Modules.CallForPapers.Domain.Evaluation.EvaluationStatus, String> status)
        {
            dtoCall call = ServiceCall.GetDtoCall(View.IdCall);

            if(!call.AdvacedEvaluation)
                return Service.ExportEvaluatorStatistics(call, filters, View.AnonymousDisplayname, View.UnknownDisplayname, idCommittee, idEvaluator, applyFilters, fileType, translations, status);

            return ServiceCall.ExportEvaluatorStatistics(
                call,
                filters,
                View.AnonymousDisplayname,
                View.UnknownDisplayname,
                idCommittee,
                idEvaluator,
                applyFilters,
                fileType,
                translations,
                status);

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