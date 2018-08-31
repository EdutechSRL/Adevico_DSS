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
    public class EvaluationsSummaryPresenter : lm.Comol.Core.DomainModel.Common.DomainPresenter
    {
         #region "Initialize"
            private ServiceCallOfPapers _ServiceCall;
            private ServiceRequestForMembership _ServiceRequest;
            public virtual BaseModuleManager CurrentManager { get; set; }
            protected virtual IViewEvaluationsSummary View
            {
                get { return (IViewEvaluationsSummary)base.View; }
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
            public EvaluationsSummaryPresenter(iApplicationContext oContext)
                : base(oContext)
            {
                this.CurrentManager = new BaseModuleManager(oContext);
            }
            public EvaluationsSummaryPresenter(iApplicationContext oContext, IViewEvaluationsSummary view)
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
                View.DisplaySessionTimeout(RootObject.EvaluationsSummary(idCall,  idCommunity, View.PreloadView,0, View.PreloadIdSubmitterType, View.PreloadFilterBy, View.PreloadOrderBy, View.PreloadAscending, View.PreloadPageIndex, View.PreloadPageSize, View.GetItemEncoded(View.PreloadSearchForName)));
            else if (call == null)
                View.DisplayUnknownCall(idCommunity, idModule, idCall);
            else if (type == CallForPaperType.RequestForMembership)
                View.DisplayEvaluationUnavailable();
            else if (allowAdmin)
            {
                //
                //EvaluationType EvalType = (call.AdvacedEvaluation) ? 
                //    ServiceCall.CommissionGetEvalType(idCommission):
                //    call.EvaluationType;

                //EvaluationType EvalType = call.EvaluationType;
                //call.EvaluationType;

                if (call.EvaluationType == EvaluationType.Dss)
                    View.CallUseFuzzy = Service.CallUseFuzzy(idCall);
                View.AllowExportAll = Service.HasEvaluations(idCall);
                View.DisplayEvaluationInfo(call.EndEvaluationOn);

                if (!call.AdvacedEvaluation)
                {
                    View.CurrentEvaluationType = call.EvaluationType;
                    InitializeView(idCall, call.EvaluationType, idCommunity, View.PreloadFilters);
                    View.SetStepSummaryLink(0, 0, false);
                } 
                
            }

            if (call.AdvacedEvaluation)
            {
                long idCommission = View.IdCallAdvCommission;
                long StepId = ServiceCall.StepGetIdFromComm(idCommission);


                bool ShowGenericLink = ServiceCall.CommissionUserIsPresidentOrSegretaryInMaster(idCommission, UserContext.CurrentUserID);
                

                View.SetStepSummaryLink(StepId, idCommission, ShowGenericLink);

                EvaluationType oldEvtype = EvaluationType.Average;

                Advanced.dto.dtoCommEvalInfo evinfo = ServiceCall.CommissionEvalTypeGet(idCommission);
                
                switch (evinfo.Type)
                {
                    case Advanced.EvalType.Average:
                        oldEvtype = EvaluationType.Average;
                        break;
                    case Advanced.EvalType.Sum:
                        oldEvtype = EvaluationType.Sum;
                        break;
                }
                View.CurrentEvaluationType = (evinfo.Type == Advanced.EvalType.Sum) ? EvaluationType.Sum : EvaluationType.Average;
                View.minRange = evinfo.minRange;
                View.LockBool = evinfo.LockBool;


                bool cancloseAdvance = ServiceCall.CommissionCanClose(idCommission);

                View.ShowCloseCommission(cancloseAdvance);


                InitializeViewAvance(idCall,
                    oldEvtype,
                    idCommunity,
                    idCommission,
                    View.PreloadFilters,
                    call.Type);
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
        private void InitializeView(long idCall, EvaluationType type,Int32 idCommunity, dtoEvaluationsFilters filters)
        {
            List<EvaluationFilterStatus> items = Service.GetAvailableEvaluationFilterStatus(idCall);
            List<dtoSubmitterType> types = Service.GetSubmitterTypes(idCall, ManageEvaluationsAction.None);
            if (!items.Contains(filters.Status))
                filters.Status = items[0];
            if (types==null || (types!=null && (!types.Any() && !types.Where(i=>i.Id== filters.IdSubmitterType).Any())))
                filters.IdSubmitterType=-1;
            View.CurrentFilters = filters;
            View.LoadAvailableStatus(items, filters.Status);
            View.LoadAvailableSubmitterTypes(types, filters.IdSubmitterType);
            LoadEvaluations(idCall,type, idCommunity, filters, View.PreloadPageIndex, View.PreloadPageSize);
        }
        public void LoadEvaluations(long idCall, EvaluationType EvalType, Int32 idCommunity, dtoEvaluationsFilters filters, int pageIndex, int pageSize)
        {
            Boolean allowManage = true;
            View.CurrentFilters = filters;

            ModuleCallForPaper module = Service.CallForPaperServicePermission(UserContext.CurrentUserID, idCommunity);
            allowManage = (module.CreateCallForPaper || module.Administration || module.ManageCallForPapers || module.EditCallForPaper);

            lm.Comol.Modules.CallForPapers.Domain.CallForPaperType type = ServiceCall.GetCallType(idCall);
            dtoCall call = (type == CallForPaperType.CallForBids) ? ServiceCall.GetDtoCall(idCall) : null;
            
            if(call.AdvacedEvaluation)
            {
                bool isPresident = ServiceCall.CommissionUserIsPresident(View.IdCallAdvCommission, UserContext.CurrentUserID);
                bool isSecretary = ServiceCall.CommissionUserIsSecretary(View.IdCallAdvCommission, UserContext.CurrentUserID);

                if(isPresident || isSecretary)
                {
                    allowManage = true;
                }
            }


            if (UserContext.isAnonymous)
                View.DisplaySessionTimeout();
            else if (allowManage)
                LoadItems(idCall, EvalType, idCommunity, filters, pageIndex, pageSize);

            else
                View.DisplayNoPermission(idCommunity, View.IdCallModule);
        }
        private void LoadItems(long idCall, EvaluationType type, Int32 idCommunity, dtoEvaluationsFilters filters, int pageIndex, int pageSize)
        {
            if (type == EvaluationType.Dss)
                InitializeDssInfo(idCall);
            else
                View.HideDssWarning();

            bool isAdvance = ServiceCall.CallIsAdvanced(idCall);

            List<dtoEvaluationSummaryItem> items =
                isAdvance ?
                ServiceCall.GetEvaluationsList(idCall, View.IdCallAdvCommission, type, filters, View.AnonymousDisplayname, View.UnknownDisplayname) :
                Service.GetEvaluationsList(idCall, type, filters, View.AnonymousDisplayname, View.UnknownDisplayname, true);

     


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

            View.AllowExportCurrent = (items != null && items.Any() && items.Skip(pageIndex * pageSize).Take(pageSize).Any());
            if (pager.Count < 0)
                View.DisplayNoEvaluationsFound();
            else
            {
                List<long> committees = Service.GetIdCommittees(idCall);
                if (committees.Count == 1)
                    View.DisplayLinkToSingleCommittee(committees.FirstOrDefault());
                View.LoadEvaluations(items.Skip(pageIndex * pageSize).Take(pageSize).ToList(), committees.Count);
            }
            View.SendUserAction(View.IdCallCommunity, View.IdCallModule, ModuleCallForPaper.ActionType.ViewEvaluationsSummary);


            if(isAdvance)
            {
                bool cancloseAdvance = ServiceCall.CommissionCanClose(View.IdCallAdvCommission);
                View.ShowCloseCommission(cancloseAdvance);
            }
        }

        public void CommissionClose()
        {
            bool success = ServiceCall.CommissionClose(View.IdCallAdvCommission);

            if (success)
                InitView();

        }

        private void InitializeViewAvance(
            long idCall, 
            EvaluationType type, 
            Int32 idCommunity, 
            long idCommission,
            dtoEvaluationsFilters filters,
            CallForPaperType CpType
            )
        {
            type = ServiceCall.CommissionGetEvalType(idCommission);

            List<EvaluationFilterStatus> items = Service.GetAvailableEvaluationFilterStatus(idCall);

            List<dtoSubmitterType> types = Service.GetSubmitterTypes(idCall, ManageEvaluationsAction.None);

            


            if (!items.Contains(filters.Status))
                filters.Status = items[0];

            if (types == null || (types != null && (!types.Any() && !types.Where(i => i.Id == filters.IdSubmitterType).Any())))
                filters.IdSubmitterType = -1;

            View.CurrentFilters = filters;
            View.LoadAvailableStatus(items, filters.Status);
            View.LoadAvailableSubmitterTypes(types, filters.IdSubmitterType);

            

            LoadEvaluationsAdvance(
                idCall, 
                type, 
                idCommunity, 
                idCommission,
                filters, 
                View.PreloadPageIndex, 
                View.PreloadPageSize,
                CpType);
        }

        public void LoadEvaluationsAdvance(
            long idCall, 
            EvaluationType type, 
            Int32 idCommunity,
            long idCommission,
            dtoEvaluationsFilters filters, 
            int pageIndex, 
            int pageSize,
            CallForPaperType CpType)
        {
            Boolean allowManage = true;
            View.CurrentFilters = filters;

            ModuleCallForPaper module = Service.CallForPaperServicePermission(UserContext.CurrentUserID, idCommunity);
            allowManage = (module.CreateCallForPaper || module.Administration || module.ManageCallForPapers || module.EditCallForPaper);

            bool IsPresident = ServiceCall.CommissionUserIsPresident(idCommission, UserContext.CurrentUserID);
            bool IsSecretary = ServiceCall.CommissionUserIsSecretary(idCommission, UserContext.CurrentUserID);

            allowManage |= IsPresident | IsSecretary;

            //E controllo sui membri della commissione!

            View.SetActionUrl((allowManage) ? CallStandardAction.Manage : CallStandardAction.List, RootObject.ViewCalls(idCall, CpType, ((allowManage) ? CallStandardAction.Manage : CallStandardAction.List), idCommunity, View.PreloadView));
            
            if (UserContext.isAnonymous)
                View.DisplaySessionTimeout();
            else if (allowManage)
                LoadItemsAdv(idCall, 
                    type, 
                    idCommunity,
                    idCommission,
                    filters, 
                    pageIndex, 
                    pageSize);
            else
                View.DisplayNoPermission(idCommunity, View.IdCallModule);
        }

        private void LoadItemsAdv(
           long idCall,
           EvaluationType type,
           Int32 idCommunity,
           long idCommission,
           dtoEvaluationsFilters filters,
           int pageIndex,
           int pageSize)
        {
            //if (type == EvaluationType.Dss)
            //    InitializeDssInfo(idCall);
            //else
            View.HideDssWarning();

            List<dtoEvaluationSummaryItem> items = ServiceCall.GetEvaluationsList(idCall, 
                idCommission,
                type, filters, 
                View.AnonymousDisplayname, 
                View.UnknownDisplayname);

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

            View.AllowExportCurrent = (items != null && items.Any() && items.Skip(pageIndex * pageSize).Take(pageSize).Any());
            if (pager.Count < 0)
                View.DisplayNoEvaluationsFound();
            else
            {
                List<long> committees = Service.GetIdCommittees(idCall);
                if (committees.Count == 1)
                    View.DisplayLinkToSingleCommittee(committees.FirstOrDefault());
                View.LoadEvaluations(items.Skip(pageIndex * pageSize).Take(pageSize).ToList(), committees.Count);
            }
            View.SendUserAction(View.IdCallCommunity, View.IdCallModule, ModuleCallForPaper.ActionType.ViewEvaluationsSummary);
        }

        private void LoadItems(
            long idCall, 
            EvaluationType type, 
            Int32 idCommunity,
            long idCommission, 
            dtoEvaluationsFilters filters, 
            int pageIndex, 
            int pageSize)
        {
            //if (type == EvaluationType.Dss)
            //    InitializeDssInfo(idCall);
            //else
            View.HideDssWarning();
            
            List<dtoEvaluationSummaryItem> items = Service.GetEvaluationsList(idCall, type, filters, View.AnonymousDisplayname, View.UnknownDisplayname, true);
 
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

            View.AllowExportCurrent = (items != null && items.Any() && items.Skip(pageIndex * pageSize).Take(pageSize).Any());
            if (pager.Count < 0)
                View.DisplayNoEvaluationsFound();
            else
            {
                List<long> committees = Service.GetIdCommittees(idCall);
                if (committees.Count == 1)
                    View.DisplayLinkToSingleCommittee(committees.FirstOrDefault());
                View.LoadEvaluations(items.Skip(pageIndex * pageSize).Take(pageSize).ToList(), committees.Count);
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
        public String GetFileName(String filename, ItemsToExport items, ExportData xdata)
        {
            return Service.GetStatisticFileName(View.IdCall, ServiceCall.GetCallName(View.IdCall),filename, SummaryType.Evaluations,items,xdata);
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

        public String ExportTo(
           dtoEvaluationsFilters filters,
           SummaryType summaryType,
           ItemsToExport itemsToExport,
           ExportData xdata,
           lm.Comol.Core.DomainModel.Helpers.Export.ExportFileType fileType,
           Dictionary<lm.Comol.Modules.CallForPapers.Domain.Evaluation.EvaluationTranslations, String> translations,
           Dictionary<lm.Comol.Modules.CallForPapers.Domain.Evaluation.EvaluationStatus, String> status)
        {

            dtoCall call = ServiceCall.GetDtoCall(View.IdCall);

            //NOTA: CALL.EVALUATIONTYPE NON SERVE A NULLA!!!

            List<dtoEvaluationSummaryItem> items = ServiceCall.GetEvaluationsList(
                call.Id,
                  View.IdCallAdvCommission,
                  call.EvaluationType,
                  filters,
                  View.AnonymousDisplayname,
                  View.UnknownDisplayname);

            //"#"; "Domanda di"; "Tipo domanda"; "Punti"; "N. valutazioni"; "Completate"; "In valutazione"; "Non iniziate"


            //TODO: RECUPERARE IL VALORE CORRETTO!!!
            EvaluationType CurrentEvalType = View.CurrentEvaluationType;// EvaluationType.Average;


            string output = call.Name + "\r\n\r\n";


            //Header
            output += "#;";
            output += translations[Domain.Evaluation.EvaluationTranslations.CellTitleSubmissionOwner] + ";";
            output += translations[Domain.Evaluation.EvaluationTranslations.CellTitleSubmitterType] + ";";

            if(CurrentEvalType == EvaluationType.Average)
                output += translations[Domain.Evaluation.EvaluationTranslations.CellTitleAverage] + ";";
            else
                output += translations[Domain.Evaluation.EvaluationTranslations.CellTitleSum] + ";";

            output += translations[Domain.Evaluation.EvaluationTranslations.CellTitleEvaluationsCount] + ";";

            output += "Confermate;";
            output += translations[Domain.Evaluation.EvaluationTranslations.CellTitleEvaluationsEvaluated] + ";";
            output += translations[Domain.Evaluation.EvaluationTranslations.CellTitleEvaluationsEvaluating] + ";";
            output += translations[Domain.Evaluation.EvaluationTranslations.CellTitleEvaluationsNotStarted] + ";";
            //output += translations[Domain.Evaluation.EvaluationTranslations.CellTitleEvaluationsNotStarted] + ";";

            output += "\r\n";

            foreach (dtoEvaluationSummaryItem itm in items.OrderBy(it => it.Position))
            {
                //"#"; 
                output += itm.Position + ";";

                //"Domanda di"; 
                output += itm.DisplayName + ";";

                //"Tipo domanda"; 
                output += itm.SubmitterType + ";";

                //"Punti"; 
                output += ((CurrentEvalType == EvaluationType.Average) ? itm.AverageRating : itm.SumRating) + ";";

                //"N. valutazioni"; 
                output += itm.Evaluations.Count() + ";";

                //"Confermate"
                output += itm.GetEvaluationsCount(EvaluationStatus.Confirmed) + ";";

                //"Completate"; 
                output += itm.GetEvaluationsCount(EvaluationStatus.Evaluated) + ";";

                //"In valutazione"; 
                output += itm.GetEvaluationsCount(EvaluationStatus.Evaluating) + ";";

                //"Non iniziate"
                output += itm.GetEvaluationsCount(EvaluationStatus.None) + ";";

                output += "\r\n";
            }

            return output;

            //return Service.ExportSummaryStatistics(ServiceCall.GetDtoCall(View.IdCall), filters, View.AnonymousDisplayname, View.UnknownDisplayname, summaryType, itemsToExport, xdata, fileType, translations, status);
        }

    }
}