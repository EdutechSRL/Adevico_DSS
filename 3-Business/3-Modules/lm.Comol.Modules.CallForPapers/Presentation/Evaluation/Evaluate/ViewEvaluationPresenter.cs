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
    public class ViewEvaluationPresenter : lm.Comol.Core.DomainModel.Common.DomainPresenter
    {
         #region "Initialize"
            private ServiceCallOfPapers _ServiceCall;
            private ServiceRequestForMembership _ServiceRequest;
            public virtual BaseModuleManager CurrentManager { get; set; }
            protected virtual IViewViewEvaluation View
            {
                get { return (IViewViewEvaluation)base.View; }
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
            public ViewEvaluationPresenter(iApplicationContext oContext)
                : base(oContext)
            {
                this.CurrentManager = new BaseModuleManager(oContext);
            }
            public ViewEvaluationPresenter(iApplicationContext oContext, IViewViewEvaluation view)
                : base(oContext, view)
            {
                this.CurrentManager = new BaseModuleManager(oContext);
            }
        #endregion

        public void InitView()
        {
            Boolean isAnonymousUser = UserContext.isAnonymous;
            long idCall = View.PreloadIdCall;
            long idEvaluation = View.PreloadIdEvaluation;
            long idSubmission = View.PreloadIdSubmission;
            long idEvaluator = View.PreloadIdEvaluator;
            Int32 idUser = UserContext.CurrentUserID;

            lm.Comol.Modules.CallForPapers.Domain.CallForPaperType type = ServiceCall.GetCallType(idCall);
            int idModule = (type == CallForPaperType.CallForBids) ? ServiceCall.ServiceModuleID() : ServiceRequest.ServiceModuleID();

            dtoBaseForPaper baseCall = ServiceCall.GetDtoBaseCall(idCall);
            int idCommunity = GetCurrentCommunity(baseCall);
                       

            View.IdCall = idCall;
            View.IdCallModule = idModule;
            View.IdCallCommunity = idCommunity;
            View.IdEvaluation = idEvaluation;
            View.IdSubmission = idSubmission;
            View.IdEvaluator = idEvaluator;
            View.CallType = type;
            DisplayEvaluations dEvaluation = DisplayEvaluations.None;
            if (idEvaluation > 0)
                dEvaluation = DisplayEvaluations.Single;
            else if (idEvaluator > 0)
                dEvaluation = DisplayEvaluations.ForUser;
            else if (idSubmission > 0)
                dEvaluation = DisplayEvaluations.ForSubmission;
          
            if (UserContext.isAnonymous)
            {
                switch (dEvaluation) {
                    case DisplayEvaluations.Single:
                        View.DisplaySessionTimeout(RootObject.ViewSingleEvaluation(idEvaluation,idSubmission,idCall,idCommunity, View.AdvCommId));
                        break;
                    case DisplayEvaluations.ForUser:
                        View.DisplaySessionTimeout(RootObject.ViewUserEvaluations(idEvaluator, idSubmission, idCall, idCommunity, View.AdvCommId));
                        break;
                    default:
                        View.DisplaySessionTimeout(RootObject.ViewSubmissionEvaluations( idSubmission, idCall, idCommunity, View.AdvCommId));
                        break;
                }
            }
            else
            {
                if (baseCall == null)
                    View.DisplayUnknownCall(idCommunity, idModule, idCall, type);
                else if (type == CallForPaperType.RequestForMembership)
                    View.DisplayEvaluationUnavailable();
                else if (dEvaluation == DisplayEvaluations.None)
                    View.DisplayNoEvaluationsToView();
                else
                {
                    ModuleCallForPaper module = ServiceCall.CallForPaperServicePermission(idUser, idCommunity);
                    Boolean allowAdmin = ((module.ManageCallForPapers || module.Administration || ((module.CreateCallForPaper || module.EditCallForPaper) && baseCall.Owner.Id == idUser)));

                    if (!allowAdmin && !Service.isEvaluationOwner(idEvaluator, idEvaluation, idSubmission, dEvaluation, UserContext.CurrentUserID))
                        View.DisplayNoPermissionToView();
                    else
                    {
                        dtoSubmissionRevision submission = ServiceCall.GetSubmissionWithRevisions(idSubmission, false);
                        if (submission == null)
                            View.DisplayUnknownSubmission(idCommunity, idModule, idSubmission, type);
                        else
                        {
                            dtoCall call = ServiceCall.GetDtoCall(idCall);
                            if (call != null)
                            {
                                View.CurrentEvaluationType = call.EvaluationType;
                                if (call.EvaluationType == EvaluationType.Dss)
                                    InitializeDssInfo(idCall);
                                else
                                    View.HideDssWarning();
                                LoadData(dEvaluation, idCommunity, call, submission, idEvaluation, idEvaluator);
                            }
                            else
                                View.DisplayUnknownCall(idCommunity, idModule, idCall, type);
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
        private void LoadData(DisplayEvaluations display, Int32 idCommunity, dtoCall call, dtoSubmissionRevision submission, long idEvaluation, long idEvaluator)
        {
            View.CurrentDisplay = display;
            List<dtoCommitteeEvaluationInfo> committees = null;
            String owner = (submission.IsAnonymous || submission.Owner == null) ? View.AnonymousDisplayName : submission.Owner.SurnameAndName;
            litePerson submitter = CurrentManager.GetLitePerson(submission.IdSubmittedBy);
            String submittedBy = (submission.IdPerson == submission.IdSubmittedBy) ? "" : (submitter == null || submitter.TypeID == (int)UserTypeStandard.Guest) ? View.AnonymousDisplayName : submitter.SurnameAndName;
            View.LoadSubmissionInfo(owner, call.Name, submission.SubmittedOn, submittedBy);
            View.AllowPrint = true;

            if(call.AdvacedEvaluation)
            {
                View.SetViewEvaluationUrl("");
            } else
            {
                View.SetViewEvaluationUrl(RootObject.ViewSubmissionTableEvaluations(submission.Id, call.Id, idCommunity));
            }
            
            View.CommitteeIsFuzzy = Service.GetCommitteeDssMethodIsFuzzy(call.Id);
            switch (display) { 
                case DisplayEvaluations.ForSubmission:
                    committees = (call.AdvacedEvaluation) ?
                        ServiceCall.GetCommitteesInfoForSubmission(submission.Id, call.Id, View.AdvCommId)
                        : Service.GetCommitteesInfoForSubmission(submission.Id, call.Id);
                    View.LoadCommitteesStatus(committees, display);

                    View.LoadEvaluations(
                        (call.AdvacedEvaluation) ?
                        ServiceCall.GetSubmissionEvaluationsDispItem(call.Id, submission.Id, View.AdvCommId, View.UnknonwUserName) :
                        Service.GetSubmissionEvaluations(submission.Id, call.Id,View.UnknonwUserName)
                        );

                    break;
                case DisplayEvaluations.ForUser:
                    committees = (call.AdvacedEvaluation) ? 
                        ServiceCall.GetCommitteesInfoForEvaluator(idEvaluator, View.AdvCommId) :
                        Service.GetCommitteesInfoForEvaluator(idEvaluator);

                    CallEvaluator evaluator = CurrentManager.Get<CallEvaluator>(idEvaluator);
                    if (committees != null)
                        View.LoadEvaluatorInfo((evaluator == null || evaluator.Person ==null) ? View.AnonymousDisplayName : evaluator.Person.SurnameAndName , committees.Count);
                    else
                        View.LoadEvaluatorInfo((evaluator == null || evaluator.Person == null) ? View.AnonymousDisplayName : evaluator.Person.SurnameAndName, 0);

                    View.LoadEvaluations(Service.GetEvaluationsInfo(idEvaluator, submission.Id, View.AnonymousDisplayName, View.UnknonwUserName));
                    View.LoadCommitteesStatus(committees, display);
                    break;

                case DisplayEvaluations.Single:
                    lm.Comol.Modules.CallForPapers.Domain.Evaluation.dtoEvaluation evaluation =
                        (call.AdvacedEvaluation) ?
                        ServiceCall.GetFullEvaluation(idEvaluation, View.AnonymousDisplayName, View.UnknonwUserName):
                        Service.GetFullEvaluation(idEvaluation, View.AnonymousDisplayName, View.UnknonwUserName);

                    if (evaluation == null)
                        View.DisplayUnknownEvaluation(View.IdCallCommunity, View.IdCallModule, idEvaluation);
                    else {
                        dtoCommitteeEvaluation result = new dtoCommitteeEvaluation()
                        {
                            Evaluation =evaluation,
                            IdCommittee = evaluation.IdCommittee
                        };

                        committees = (call.AdvacedEvaluation) ?
                            ServiceCall.GetCommitteesInfoForEvaluator(evaluation.IdEvaluator):
                            Service.GetCommitteesInfoForEvaluator(evaluation.IdEvaluator);

                        if (committees.Where(c => c.IdCommittee == evaluation.IdCommittee).Any())
                        {
                            committees.Where(c => c.IdCommittee == evaluation.IdCommittee).FirstOrDefault().Status = evaluation.Status;
                            result.Status= evaluation.Status;
                            result.Name= committees.Where(c => c.IdCommittee == evaluation.IdCommittee).FirstOrDefault().Name;
                        }

                        View.LoadCommitteesStatus(committees.Where(c => c.IdCommittee == evaluation.IdCommittee).ToList(), display);
                        if (committees != null)
                            View.LoadEvaluatorInfo(evaluation.EvaluatorName, committees.Count);
                        else
                            View.LoadEvaluatorInfo(evaluation.EvaluatorName, 0);
                        View.LoadEvaluation(result);
                    }
                    break;
            }
        }

        private void InitializeDssInfo(long idCall)
        {
            View.CallUseFuzzy = Service.CallUseFuzzy(idCall);
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
        //lm.Comol.Modules.CallForPapers.Domain.Evaluation.dtoEvaluation evaluation = Service.GetFullEvaluation(idEvaluation);
         //if (evaluation == null)
         //                   View.DisplayUnknownEvaluation(idCommunity, idModule, idEvaluation);
         //               else

        //private void LoadData(long idCall, lm.Comol.Modules.CallForPapers.Domain.Evaluation.dtoEvaluation evaluation, dtoSubmissionRevision submission)
        //{
        //    // LOAD CALL INFO
        //    //dtoCall dtoC = ServiceCall.GetDtoCall(idCall;
        //    //                View.LoadCallInfo(dtoC);
        //    //View.LoadAttachments(ServiceCall.GetAvailableCallAttachments(idCall, submission.Type.Id));
        //    //Boolean submissionSubmitted = (submission.Status == SubmissionStatus.accepted);
        //    dtoCall call = ServiceCall.GetDtoCall(idCall);
        //    LoadSubmission(call, submission);
        //    LoadEvaluationData(call, evaluation);
        //}
        //private void LoadEvaluationData(long idCall, lm.Comol.Modules.CallForPapers.Domain.Evaluation.dtoEvaluation evaluation) {
        //    LoadEvaluationData(ServiceCall.GetDtoCall(idCall), evaluation);
        //}
        //private void LoadEvaluationData(dtoCall call, lm.Comol.Modules.CallForPapers.Domain.Evaluation.dtoEvaluation evaluation)
        //{
           
        //    Boolean inEvaluation = (evaluation.Status == Domain.Evaluation.EvaluationStatus.Evaluating || evaluation.Status == Domain.Evaluation.EvaluationStatus.None);
        //    View.AllowEvaluate = inEvaluation && !(call.EndEvaluationOn.HasValue && call.EndEvaluationOn.Value <= DateTime.Now);
        //    View.AllowCompleteEvaluation = inEvaluation && !(call.EndEvaluationOn.HasValue && call.EndEvaluationOn.Value <= DateTime.Now);

        //    evaluation.Criteria.Where(c=> View.SavingForComplete || !c.IsValueEmpty).ToList().ForEach(c => c.CriterionError = (c.IsValidForEvaluation) ? FieldError.None : (c.IsValidForCriterionSaving) ? FieldError.Mandatory : FieldError.Invalid);

        //    List<dtoCriterionForEvaluation> tabs = (from e in evaluation.Criteria
        //                                            orderby e.Criterion.DisplayOrder
        //                                            select new dtoCriterionForEvaluation(e.Criterion.Id, e.Criterion.Name, e.Criterion.DisplayOrder, e.Status, e.CriterionError!= FieldError.None )).ToList();
        //    tabs.Add(new dtoCriterionForEvaluation(0,View.GeneralTabName,tabs.Select(t=>t.DisplayOrder).Max()+1, Domain.Evaluation.EvaluationStatus.None, false ));
        //    evaluation.Criteria.Add(dtoCriterionEvaluated.GetEvaluationPlaceHolder(evaluation.Comment, tabs.Where(t => t.Id == 0).Select(t => t.DisplayOrder).FirstOrDefault()));
        //    View.InitializeEvaluationSettings(tabs,evaluation);
        //}
        //private void LoadSubmission(dtoCall call, dtoSubmissionRevision submission)
        //{
        //    String owner = "";
        //    String submittedBy = "";
        //    if (submission.Owner == null || submission.Owner.TypeID == (int)UserTypeStandard.Guest)
        //        owner = View.AnonymousOwnerName;
        //    else
        //        owner = submission.Owner.SurnameAndName;

        //    if (submission.SubmittedBy == null || submission.SubmittedBy.TypeID == (int)UserTypeStandard.Guest)
        //        submittedBy = View.AnonymousOwnerName;
        //    else
        //        submittedBy = submission.SubmittedBy.SurnameAndName;
        //    View.SetContainerName(call.Name, owner, submission.Type.Name, submittedBy);
        //    if (submission.Deleted != BaseStatusDeleted.None)
        //        View.LoadSubmissionInfo(submission.Type.Name, owner, SubmissionStatus.deleted);
        //    else if (!submission.SubmittedOn.HasValue)
        //        View.LoadSubmissionInfo(submission.Type.Name, owner, submission.Status);
        //    else if (submission.IdPerson == submission.IdSubmittedBy)
        //        View.LoadSubmissionInfo(submission.Type.Name, owner, submission.Status, submission.SubmittedOn.Value);
        //    else
        //        View.LoadSubmissionInfo(submission.Type.Name, owner, submission.Status, submission.SubmittedOn.Value, submittedBy);

        //    LoadSections(call.Id, submission);
        //}
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