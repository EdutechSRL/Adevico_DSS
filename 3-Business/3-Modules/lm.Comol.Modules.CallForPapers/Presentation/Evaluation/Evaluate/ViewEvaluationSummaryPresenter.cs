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
    public class ViewEvaluationSummaryPresenter : lm.Comol.Core.DomainModel.Common.DomainPresenter
    {
         #region "Initialize"
            private ServiceCallOfPapers _ServiceCall;
            private ServiceRequestForMembership _ServiceRequest;
            public virtual BaseModuleManager CurrentManager { get; set; }
            protected virtual IViewEvaluationSummary View
            {
                get { return (IViewEvaluationSummary)base.View; }
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
            public ViewEvaluationSummaryPresenter(iApplicationContext oContext)
                : base(oContext)
            {
                this.CurrentManager = new BaseModuleManager(oContext);
            }
            public ViewEvaluationSummaryPresenter(iApplicationContext oContext, IViewEvaluationSummary view)
                : base(oContext, view)
            {
                this.CurrentManager = new BaseModuleManager(oContext);
            }
        #endregion

        public void InitView()
        {
            Boolean isAnonymousUser = UserContext.isAnonymous;
            long idCall = View.PreloadIdCall;
            long idSubmission = View.PreloadIdSubmission;
            Int32 idUser = UserContext.CurrentUserID;

            lm.Comol.Modules.CallForPapers.Domain.CallForPaperType type = ServiceCall.GetCallType(idCall);
            int idModule = (type == CallForPaperType.CallForBids) ? ServiceCall.ServiceModuleID() : ServiceRequest.ServiceModuleID();

            dtoBaseForPaper call = ServiceCall.GetDtoBaseCall(idCall);
            int idCommunity = GetCurrentCommunity(call);

            View.IdCall = idCall;
            View.IdCallModule = idModule;
            View.IdCallCommunity = idCommunity;
            View.IdSubmission = idSubmission;
            if (UserContext.isAnonymous)
                View.DisplaySessionTimeout(RootObject.ViewSubmissionTableEvaluations(idSubmission, idCall, idCommunity));
            else
            {
                if (call == null)
                    View.DisplayUnknownCall(idCommunity, idModule, idCall,type);
                else if (type == CallForPaperType.RequestForMembership)
                    View.DisplayEvaluationUnavailable();
                else
                {
                    ModuleCallForPaper module = ServiceCall.CallForPaperServicePermission(idUser, idCommunity);
                    Boolean allowAdmin = ((module.ManageCallForPapers || module.Administration || ((module.CreateCallForPaper || module.EditCallForPaper) && call.Owner.Id == idUser)));
                    EvaluationType evaluationType = ServiceCall.GetEvaluationType(idCall);

                    if(call.AdvacedEvaluation)
                    {
                        bool isPresident = ServiceCall.CommissionUserIsPresident(View.AdvCommissionId, idUser);
                        bool isSecretary = ServiceCall.CommissionUserIsSecretary(View.AdvCommissionId, idUser);

                        allowAdmin |= isPresident | isSecretary;
                    }


                    //View.CurrentEvaluationType = evaluationType;
                    if (!allowAdmin)
                        View.DisplayNoPermissionToView();
                    else
                    {
                        dtoSubmissionRevision submission = ServiceCall.GetSubmissionWithRevisions(idSubmission, false);
                        if (submission == null)
                            View.DisplayUnknownSubmission(idCommunity, idModule, idSubmission, type);
                        else
                        {
                            LoadData(idCommunity, call, evaluationType, submission);
                        }
                    }
                }
            }
        }

        private int GetCurrentCommunity(dtoBaseForPaper call)
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
        private void LoadData(
            Int32 idCommunity, 
            dtoBaseForPaper call, 
            EvaluationType type, 
            dtoSubmissionRevision submission)
        {
            List<dtoCommitteeEvaluationInfo> committees =
                call.AdvacedEvaluation ?
                ServiceCall.GetCommitteesInfoForSubmission(submission.Id, call.Id, View.AdvCommissionId) :
                Service.GetCommitteesInfoForSubmission(submission.Id, call.Id );

            if(call.AdvacedEvaluation)
            {
                //EvaluationType oldEvtype = ServiceCall.CommissionGetEvalType(View.AdvCommissionId);
                EvaluationType oldEvtype = EvaluationType.Average;

                Advanced.dto.dtoCommEvalInfo evinfo = ServiceCall.CommissionEvalTypeGet(View.AdvCommissionId);

                switch (evinfo.Type)
                {
                    case Advanced.EvalType.Average:
                        oldEvtype = EvaluationType.Average;
                        break;
                    case Advanced.EvalType.Sum:
                        oldEvtype = EvaluationType.Sum;
                        break;
                }

                View.CurrentEvaluationType = oldEvtype;
            } else
            {
                View.CurrentEvaluationType = type;
            }


            View.CommitteesCount = committees.Count;
            String owner = (submission == null || (submission != null && (submission.IsAnonymous))) ? View.AnonymousDisplayName : ((submission.Owner != null) ? submission.Owner.SurnameAndName : View.UnknownDisplayname);
            litePerson submitter = CurrentManager.GetLitePerson(submission.IdSubmittedBy);
            String submittedBy = (submission.IdPerson == submission.IdSubmittedBy) ? "" : (submitter == null || submitter.TypeID == (int)UserTypeStandard.Guest) ? View.AnonymousDisplayName : submitter.SurnameAndName;

            View.SetViewEvaluationsUrl(RootObject.ViewSubmissionEvaluations(submission.Id,call.Id, idCommunity, View.AdvCommissionId));
            // View.SetViewSubmissionUrl(RootObject.ViewSubmissionAsManager(
            bool isPresident = false;

            if (call.AdvacedEvaluation)
            {
                isPresident = ServiceCall.CommissionUserIsPresident(View.AdvCommissionId, UserContext.CurrentUserID);
            }
            int minscore = (!call.AdvacedEvaluation) ? 0 :
                (committees == null || !committees.Any()) ? 0 :
                committees[0].MinValue;


            View.LoadSubmissionInfo(
                call.Name,
                owner, 
                submission.SubmittedOn, 
                submittedBy, 
                committees, 
                (committees == null || !committees.Any()) ? 0 : committees[0].IdCommittee, 
                isPresident,
                minscore);

            if (type == EvaluationType.Dss)
                InitializeDssInfo(call.Id);
            LoadEvaluations(submission.Id, call.Id,type, View.IdCurrentCommittee, committees.Count);
        }
        public void LoadEvaluations(long idSubmission, long idCall, EvaluationType type, long idCommittee, Int32 count)
        {
            if (UserContext.isAnonymous)
                View.DisplaySessionTimeout(RootObject.ViewSubmissionTableEvaluations(idSubmission, idCall, View.IdCallCommunity));
            else
            {
                bool isAdvance = ServiceCall.CallIsAdvanced(idCall);

                List<dtoSubmissionCommitteeItem> evaluations =
                    isAdvance ?
                    ServiceCall.GetSubmissionEvaluations(idCall, idSubmission, idCommittee, View.UnknownDisplayname) :
                    Service.GetSubmissionEvaluations(idCall, idSubmission, idCommittee, View.UnknownDisplayname);
                
                View.AllowHideComments = (evaluations != null && evaluations.Where(e => e.HasComments()).Any());
                View.AllowExportCurrent = (evaluations != null && (idCommittee >0 || count==1));
                View.AllowExportAll = (evaluations != null && count >1);
                View.LoadEvaluations(evaluations);
            }
        }
        private void InitializeDssInfo(long idCall)
        {
            View.CallUseFuzzy = Service.CallUseFuzzy(idCall);
            View.CommitteeIsFuzzy = Service.GetCommitteeDssMethodIsFuzzy(idCall);
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
        public String GetFileName(String filename, SummaryType summaryType, long idCall, long idSubmission, long idCommittee)
        {
            return Service.GetStatisticFileName(idCall, ServiceCall.GetCallName(idCall),idCommittee, Service.GetCommitteeDisplayOrder(idCommittee), Service.GetCommitteeName(idCommittee),idSubmission, filename, summaryType);
        }

        public String ExportTo(
            SummaryType summaryType, 
            long idCall, 
            long idSubmission, 
            long idCommittee, 
            ExportData exportData, 
            lm.Comol.Core.DomainModel.Helpers.Export.ExportFileType fileType, 
            Dictionary<lm.Comol.Modules.CallForPapers.Domain.Evaluation.EvaluationTranslations, String> translations, 
            Dictionary<lm.Comol.Modules.CallForPapers.Domain.Evaluation.EvaluationStatus, String> status)
        {
            if (UserContext.isAnonymous)
            {
                View.DisplaySessionTimeout(RootObject.ViewSubmissionTableEvaluations(idSubmission, idCall, View.IdCallCommunity));
                return "";
            }

            dtoBaseForPaper call = ServiceCall.GetDtoBaseCall(idCall);

            dtoSubmissionRevision submission = ServiceCall.GetSubmissionWithRevisions(idSubmission, false);

            List<dtoCommitteeEvaluationInfo> committees = new List<dtoCommitteeEvaluationInfo>();

            

                

            EvaluationType evalType = View.CurrentEvaluationType;

            bool isAdvance = ServiceCall.CallIsAdvanced(idCall);

            List<dtoSubmissionCommitteeItem> evaluations = new List<dtoSubmissionCommitteeItem>();
            if (isAdvance)
            {
                evaluations = ServiceCall.GetSubmissionEvaluations(idCall, idSubmission, idCommittee, View.UnknownDisplayname);
                committees = ServiceCall.GetCommitteesInfoForSubmission(submission.Id, call.Id, View.AdvCommissionId);
            }
            else
            {
                evaluations = Service.GetSubmissionEvaluations(idCall, idSubmission, idCommittee, View.UnknownDisplayname);
                committees = Service.GetCommitteesInfoForSubmission(submission.Id, call.Id);
            }
            
                

            String export = "";

            //INTESTAZIONE GENERICA

            export += string.Format(translations[EvaluationTranslations.CallTitle], call.Name) + ";";
            export += "\r\n";

            export += translations[EvaluationTranslations.CellTitleSubmissionOwner] + " ";
            export += submission.Owner.SurnameAndName + ";";
            export += "\r\n";

            if(submission.SubmittedOn != null)
            {
                DateTime submitOn = (DateTime)submission.SubmittedOn;

                export += string.Format(translations[EvaluationTranslations.SubmittedOn],
                    submitOn.ToString("dd MM yyyy"),
                    submitOn.ToString("h:mm:ss")
                    ) + ";";

                export += "\r\n";
            }

            export += translations[EvaluationTranslations.CellTitleCommittee] + ";";

            if(committees != null && committees.Any())
            {
                export += committees.FirstOrDefault().Name;
            } else
            {
                export += "--";
            }

            
            export += "\r\n";
            export += "\r\n";


            if (exportData == ExportData.Fulldata)
            {
                //INTESTAZIONE ESPORTAZIONE

                export += translations[EvaluationTranslations.CellTitleEvaluator] + ";";
                export += translations[EvaluationTranslations.CellTitleGenericCriterion] + ";";
                export += "Tipo criterio;";
                export += translations[EvaluationTranslations.CellTitleGenericCriterionUserValue] + ";";

                //export += ((evalType == EvaluationType.Average) ? 
                //    translations[Domain.Evaluation.EvaluationTranslations.CellTitleAverage] :
                //    translations[Domain.Evaluation.EvaluationTranslations.CellTitleSum])
                //    + ";";

                export += translations[EvaluationTranslations.CellTitleGenericCriterionComment] + ";";
                export += "Commento complessivo;";

                export += "\r\n";

                //DATA
                foreach (dtoEvaluatorDisplayItem evaluator in evaluations.FirstOrDefault().Evaluators)
                {

                    foreach (dtoCriterionEvaluated criterion in evaluator.Values)
                    {
                        export += evaluator.EvaluatorName + ";";
                        export += criterion.Criterion.Name + ";";



                        switch (criterion.Criterion.Type)
                        {
                            case CriterionType.Boolean:
                                export += "Boolean;";
                                export += (criterion.DecimalValue > 0) ? "1;" : "0;";
                                break;
                            case CriterionType.IntegerRange:
                                export += "Interi;";
                                export += criterion.DecimalValue.ToString("F0") + ";";
                                break;
                            case CriterionType.DecimalRange:
                                export += "Decimale;";
                                export += criterion.DecimalValue + ";";
                                break;
                            case CriterionType.StringRange:
                                export += "Qualitativo;";
                                export += criterion.StringValue + ";";
                                break;


                        }

                        export += criterion.Comment + ";";
                        export += evaluator.Comment + ";";
                        export += "\r\n";
                    }

                }
            }
            else
            {

                //INTESTAZIONE ESPORTAZIONE

                //export += translations[EvaluationTranslations.CellTitleEvaluator] + ";";
                //export += translations[EvaluationTranslations.CellTitleGenericCriterion] + ";";
                //export += "Tipo criterio;";
                //export += translations[EvaluationTranslations.CellTitleGenericCriterionUserValue] + ";";

                ////export += ((evalType == EvaluationType.Average) ? 
                ////    translations[Domain.Evaluation.EvaluationTranslations.CellTitleAverage] :
                ////    translations[Domain.Evaluation.EvaluationTranslations.CellTitleSum])
                ////    + ";";

                //export += translations[EvaluationTranslations.CellTitleGenericCriterionComment] + ";";
                //export += "Commento complessivo;";

                //export += "\r\n";

                //DATA
                foreach (dtoEvaluatorDisplayItem evaluator in evaluations.FirstOrDefault().Evaluators)
                {

                    export += evaluator.EvaluatorName + ";";

                    foreach (dtoCriterionEvaluated criterion in evaluator.Values)
                    {
                        export += criterion.Criterion.Name + ";";


                        switch (criterion.Criterion.Type)
                        {
                            case CriterionType.Boolean:
                                export += "Boolean;";
                                export += (criterion.DecimalValue > 0) ? "Approvato;" : "Non approvato;";
                                break;
                            case CriterionType.IntegerRange:
                                export += "Interi;";
                                export += criterion.DecimalValue.ToString("F0") + ";";
                                break;
                            case CriterionType.DecimalRange:
                                export += "Decimale;";
                                export += criterion.DecimalValue + ";";
                                break;
                            case CriterionType.StringRange:
                                export += "Qualitativo;";
                                export += criterion.StringValue + ";";
                                break;


                        }

                        export += criterion.Comment + ";";

                    }

                    export += evaluator.Comment + ";";

                    export += "\r\n";

                }

            }

            return export;


            //return Service.ExportSummaryStatistics(summaryType, ServiceCall.GetDtoCall(idCall), ServiceCall.GetSubmissionWithRevisions(idSubmission, false), idSubmission, idCommittee, View.AnonymousDisplayName, View.UnknownDisplayname, exportData, fileType, translations, status);
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



        public void EvalSetDraf(long evalId)
        {
            //Todo: check permission!!!

            bool success = ServiceCall.EvalSetDraft(evalId);

            if (success)
                InitView();

        }

        public void EvalSetConfirmed(long evalId)
        {
            //Todo: checkpermission!!!!

            bool success = ServiceCall.EvalSetConfirmed(evalId);
            
            if (success)
                InitView();
        }

        

    }
}