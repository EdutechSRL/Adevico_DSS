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
    public class EvaluateSubmissionPresenter : lm.Comol.Core.DomainModel.Common.DomainPresenter
    {
         #region "Initialize"
            private ServiceCallOfPapers _ServiceCall;
            private ServiceRequestForMembership _ServiceRequest;
            public virtual BaseModuleManager CurrentManager { get; set; }
            protected virtual IViewEvaluateSubmission View
            {
                get { return (IViewEvaluateSubmission)base.View; }
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
            public EvaluateSubmissionPresenter(iApplicationContext oContext)
                : base(oContext)
            {
                this.CurrentManager = new BaseModuleManager(oContext);
            }
            public EvaluateSubmissionPresenter(iApplicationContext oContext, IViewEvaluateSubmission view)
                : base(oContext, view)
            {
                this.CurrentManager = new BaseModuleManager(oContext);
            }
        #endregion
        public void InitView(Boolean forPublicCall)
        {
            Boolean isAnonymousUser = UserContext.isAnonymous;
            long idCall = View.PreloadIdCall;
            long idEvaluation = View.PreloadedIdEvaluation;
            Int32 idUser = UserContext.CurrentUserID;

            lm.Comol.Modules.CallForPapers.Domain.CallForPaperType type = ServiceCall.GetCallType(idCall);
            int idModule = (type == CallForPaperType.CallForBids) ? ServiceCall.ServiceModuleID() : ServiceRequest.ServiceModuleID();

            dtoBaseForPaper call = ServiceCall.GetDtoBaseCall(idCall);
            int idCommunity = GetCurrentCommunity(call);
                       

            View.IdCall = idCall;
            View.IdCallModule = idModule;
            View.IdCallCommunity = idCommunity;
            View.IdEvaluation = idEvaluation;
            View.CallType = type;

        

            if (UserContext.isAnonymous)
                View.DisplaySessionTimeout(RootObject.EvaluateSubmission(idCall,View.PreloadIdCommunity, View.PreloadedIdEvaluation));
            if (call == null)
                View.DisplayUnknownCall(idCommunity, idModule, idCall, type);
            else if (type == CallForPaperType.RequestForMembership)
                View.DisplayEvaluationUnavailable();
            else
            {
                bool canEvaluate = true;
                //Evantualmente aggiungere i tag anche sulle vecchie commissioni...
                string tags = (call.AdvacedEvaluation) ?
                    ServiceCall.TagGetForEvaluation(idEvaluation, idCall, ref canEvaluate) :
                    call.Tags;

                if (call.AdvacedEvaluation && !canEvaluate)
                {
                    View.DisplayEvaluationUnavailable();
                }

                View.BindTag(tags);

                ModuleCallForPaper module = ServiceCall.CallForPaperServicePermission(idUser, idCommunity);
                Boolean allowEvaluate = false;
                Boolean allowAdmin = ((module.ManageCallForPapers || module.Administration || ((module.CreateCallForPaper || module.EditCallForPaper) && call.Owner.Id == idUser)));

                Boolean isEvaluator = 
                    (call.AdvacedEvaluation) ? 
                    ServiceCall.isEvaluationOwner(idEvaluation, UserContext.CurrentUserID) :
                    Service.isEvaluationOwner(idEvaluation, UserContext.CurrentUserID);
                
                allowEvaluate = isEvaluator; //(isEvaluator || module.ManageCallForPapers || module.Administration || ((module.CreateCallForPaper || module.EditCallForPaper) && call.Owner.Id == idUser));

                if (!allowEvaluate)
                    View.DisplayEvaluationUnavailable();
                else
                {
                    lm.Comol.Modules.CallForPapers.Domain.Evaluation.dtoEvaluation evaluation =
                        (call.AdvacedEvaluation) ?
                        ServiceCall.GetFullEvaluation(idEvaluation, View.AnonymousOwnerName, View.UnknownDisplayname):
                        Service.GetFullEvaluation(idEvaluation, View.AnonymousOwnerName, View.UnknownDisplayname);
                    if (evaluation == null)
                        View.DisplayUnknownEvaluation(idCommunity, idModule, idEvaluation);
                    else
                    {
                        View.IdEvaluator = evaluation.IdEvaluator;
                        dtoSubmissionRevision submission = ServiceCall.GetSubmissionWithRevisions(evaluation.IdSubmission, true);
                        if (submission == null)
                            View.DisplayUnknownSubmission(idCommunity, idModule, evaluation.IdSubmission, type);
                        else
                            LoadData(idCall, evaluation, submission);
                    }
                }
            }
        }

        private int GetCurrentCommunity(dtoBaseForPaper call)
        {
            int idCommunity = ServiceCall.GetCallIdCommunityContext(call);
            //Community currentCommunity = CurrentManager.GetCommunity(this.UserContext.CurrentCommunityID);
            //Community community = null;
            //if (call != null)
            //    idCommunity = (call.IsPortal) ? 0 : (call.Community != null) ? call.Community.Id : 0;
            //community = CurrentManager.GetCommunity(idCommunity);

            //if (community == null && currentCommunity != null && !call.IsPortal)
            //    idCommunity = this.UserContext.CurrentCommunityID;
            //else if (community==null)
            //    idCommunity = 0;
            View.IdCallCommunity = idCommunity;
            return idCommunity;
        }
        //private int SetCallCurrentCommunity(dtoBaseForPaper call)
        //{
        //    int idCommunity = 0;
        //    Community currentCommunity = CurrentManager.GetCommunity(this.UserContext.CurrentCommunityID);
        //    Community community = null;
        //    if (call != null)
        //        idCommunity = (call.IsPortal) ? 0 : (call.Community != null) ? call.Community.Id : 0;


        //    community = CurrentManager.GetCommunity(idCommunity);
        //    if (community != null){}
        //       // View.SetContainerName(community.Name, (call != null) ? call.Name : "");
        //    else if (currentCommunity != null)
        //    {
        //        idCommunity = this.UserContext.CurrentCommunityID;
        //       // View.SetContainerName(currentCommunity.Name, (call != null) ? call.Name : "");
        //    }
        //    else
        //    {
        //        idCommunity = 0;
        //        //View.SetContainerName(View.Portalname, (call != null) ? call.Name : "");
        //    }
        //    View.IdCallCommunity = idCommunity;
        //    return idCommunity;
        //}
        private void LoadData(long idCall, lm.Comol.Modules.CallForPapers.Domain.Evaluation.dtoEvaluation evaluation, dtoSubmissionRevision submission)
        {
            // LOAD CALL INFO
            //dtoCall dtoC = ServiceCall.GetDtoCall(idCall;
            //                View.LoadCallInfo(dtoC);
            //View.LoadAttachments(ServiceCall.GetAvailableCallAttachments(idCall, submission.Type.Id));
            //Boolean submissionSubmitted = (submission.Status == SubmissionStatus.accepted);
            dtoCall call = ServiceCall.GetDtoCall(idCall);
            LoadSubmission(call, submission);
            LoadEvaluationData(call, evaluation);
        }
        private void LoadEvaluationData(long idCall, lm.Comol.Modules.CallForPapers.Domain.Evaluation.dtoEvaluation evaluation) {
            LoadEvaluationData(ServiceCall.GetDtoCall(idCall), evaluation);
        }
        private void LoadEvaluationData(dtoCall call, lm.Comol.Modules.CallForPapers.Domain.Evaluation.dtoEvaluation evaluation)
        {
           
            Boolean inEvaluation = (evaluation.Status == Domain.Evaluation.EvaluationStatus.Evaluating || evaluation.Status == Domain.Evaluation.EvaluationStatus.None);
            View.AllowEvaluate = inEvaluation && !(call.EndEvaluationOn.HasValue && call.EndEvaluationOn.Value <= DateTime.Now);
            View.AllowCompleteEvaluation = inEvaluation && !(call.EndEvaluationOn.HasValue && call.EndEvaluationOn.Value <= DateTime.Now);

            evaluation.Criteria.Where(c=> View.SavingForComplete || !c.IsValueEmpty).ToList().ForEach(c => c.CriterionError = (c.IsValidForEvaluation) ? FieldError.None : (c.IsValidForCriterionSaving) ? FieldError.Mandatory : FieldError.Invalid);

            List<dtoCriterionForEvaluation> tabs = (from e in evaluation.Criteria
                                                    orderby e.Criterion.DisplayOrder
                                                    select new dtoCriterionForEvaluation(e.Criterion.Id, e.Criterion.Name, e.Criterion.DisplayOrder, e.Status, e.CriterionError!= FieldError.None )).ToList();
            tabs.Add(new dtoCriterionForEvaluation(0,View.GeneralTabName,tabs.Select(t=>t.DisplayOrder).Max()+1, Domain.Evaluation.EvaluationStatus.None, false ));
            evaluation.Criteria.Add(dtoCriterionEvaluated.GetEvaluationPlaceHolder(evaluation.Comment, tabs.Where(t => t.Id == 0).Select(t => t.DisplayOrder).FirstOrDefault()));
            View.InitializeEvaluationSettings(tabs,evaluation);
        }
        private void LoadSubmission(dtoCall call, dtoSubmissionRevision submission)
        {
            String owner = "";
            String submittedBy = "";
            if (submission.Owner == null || submission.Owner.TypeID == (int)UserTypeStandard.Guest)
                owner = View.AnonymousOwnerName;
            else
                owner = submission.Owner.SurnameAndName;

            if (submission.SubmittedBy == null || submission.SubmittedBy.TypeID == (int)UserTypeStandard.Guest)
                submittedBy = View.AnonymousOwnerName;
            else
                submittedBy = submission.SubmittedBy.SurnameAndName;
            View.SetContainerName(call.Name, owner, submission.Type.Name, submittedBy);
            if (submission.Deleted != BaseStatusDeleted.None)
                View.LoadSubmissionInfo(submission.Type.Name, owner, SubmissionStatus.deleted);
            else if (!submission.SubmittedOn.HasValue)
                View.LoadSubmissionInfo(submission.Type.Name, owner, submission.Status);
            else if (submission.IdPerson == submission.IdSubmittedBy)
                View.LoadSubmissionInfo(submission.Type.Name, owner, submission.Status, submission.SubmittedOn.Value);
            else
                View.LoadSubmissionInfo(submission.Type.Name, owner, submission.Status, submission.SubmittedOn.Value, submittedBy);

            LoadSections(call.Id, submission);
        }
        //private void LoadSubmission(long idSubmission,long idRevision, int idUser)
        //{
        //    dtoSubmissionRevision revision = ServiceCall.GetSubmissionWithRevisions(idSubmission,true);
        //    if (revision != null)
        //    {
        //        String owner = "";
        //        String submittedBy = "";
        //        if (revision.Owner == null || revision.Owner.TypeID == (int)UserTypeStandard.Guest)
        //            owner = View.AnonymousOwnerName;
        //        else
        //            owner = revision.Owner.SurnameAndName;

        //        if (revision.SubmittedBy == null || revision.SubmittedBy.TypeID == (int)UserTypeStandard.Guest)
        //            submittedBy = View.AnonymousOwnerName;
        //        else
        //            submittedBy = revision.SubmittedBy.SurnameAndName;

        //        if (revision.Deleted != BaseStatusDeleted.None)
        //            View.LoadSubmissionInfo(revision.Type.Name, owner, SubmissionStatus.deleted);
        //        else if (!revision.SubmittedOn.HasValue)
        //            View.LoadSubmissionInfo(revision.Type.Name, owner, revision.Status);
        //        else if (revision.IdPerson == revision.IdSubmittedBy)
        //            View.LoadSubmissionInfo(revision.Type.Name, owner, revision.Status, revision.SubmittedOn.Value);
        //        else
        //            View.LoadSubmissionInfo(revision.Type.Name, owner, revision.Status, revision.SubmittedOn.Value, submittedBy);

        //        LoadSections(revision.IdCall, revision, View.ShowAdministrationTools);
        //    }
        //}
        //public void LoadSections(long idCall, CallForPaperType type, long idSubmission, long idRevision, long idSubmitter, Boolean allowAdmin)
        //{
        //    BaseForPaper call = ServiceCall.GetCall(idCall);
        //    if (call != null)
        //    {
        //        LoadSections(call, ServiceCall.GetSubmissionWithRevisions(idSubmission, true), allowAdmin);
        //    }
        //}
        public void LoadSections(long idCall, dtoSubmissionRevision submission)
        {
            BaseForPaper call = ServiceCall.GetCall(idCall);
            if (call != null)
                LoadSections(call, submission);
        }
        //private void LoadSections(dtoBaseForPaper baseCall, dtoSubmissionRevision subRev, Boolean allowAdmin)
        //{
        //    BaseForPaper call = ServiceCall.GetCall(baseCall.Id);
        //    if (call != null)
        //    {
        //        LoadSections(call, subRev, allowAdmin);
        //    }
        //}
        private void LoadSections(BaseForPaper call, dtoSubmissionRevision submission)
        {
            //if (call.Type == CallForPaperType.CallForBids)
            //{
            //    List<dtoCallSubmissionFile> requiredFiles = ServiceCall.GetRequiredFiles(call, subRev.Type.Id, subRev.Id);
            //    Dictionary<long, FieldError> filesError = ServiceCall.GetSubmissionRequiredFileErrors(subRev.Id);
            //    if (requiredFiles != null && requiredFiles.Count > 0 && filesError != null)
            //        requiredFiles.ForEach(f => f.SetError(filesError));
            //}
            long idLastRevision = submission.GetIdLastActiveRevision();
            List<dtoCallSection<dtoSubmissionValueField>> sections = ServiceCall.GetSubmissionFields(call, submission.Type.Id, submission.Id, idLastRevision);
            Dictionary<long, FieldError> fieldsError = ServiceCall.GetSubmissionFieldErrors(submission.Id, idLastRevision);
            if (sections != null && sections.Count > 0 && fieldsError != null)
                sections.ForEach(s => s.Fields.ForEach(f => f.SetError(fieldsError)));
            View.LoadSections(sections);
            View.InitializeExportControl((submission.Owner != null && submission.Owner.Id == UserContext.CurrentUserID), (submission.Owner != null) ? submission.Owner.Id : 0, call.Id, submission.Id, idLastRevision, View.IdCallModule, View.IdCallCommunity, View.CallType);
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

        public void SaveEvaluation(
            List<lm.Comol.Modules.CallForPapers.Domain.Evaluation.dtoCriterionEvaluated> criteria, 
            String comment, 
            Boolean completed)
        {
           

            if (UserContext.isAnonymous)
                View.DisplaySessionTimeout(
                    RootObject.EvaluateSubmission(View.IdCall, View.PreloadIdCommunity, View.PreloadedIdEvaluation)
                    );
            else {
                long idCall = View.IdCall;
                bool isAdvance = ServiceCall.CallIsAdvanced(idCall);

                long idEvaluation = View.IdEvaluation;
                long idEvaluator = View.IdEvaluator;
                lm.Comol.Modules.CallForPapers.Domain.Evaluation.Evaluation evaluation = null;
                try
                {
                    if (!View.SavingForComplete && completed)
                        View.SavingForComplete = completed;
                    evaluation = isAdvance ?
                        ServiceCall.SaveEvaluation(idEvaluation, idEvaluator, criteria, comment, completed):
                        Service.SaveEvaluation(idEvaluation, idEvaluator, criteria, comment, completed);

                    if (evaluation == null)
                        View.DisplayError((completed) ? EvaluationEditorErrors.SavingEvaluationCompleted : EvaluationEditorErrors.SavingEvaluation);
                    else
                    {
                        View.DisplaySettingsSaved();
                        View.SendUserAction(View.IdCallCommunity, View.IdCallModule, idEvaluation, ModuleCallForPaper.ActionType.EvaluateSubmission);
                    }
                }
                catch (EvaluationError ex)
                {
                    View.DisplayWarning((completed) ? EvaluationEditorErrors.SavingEvaluationCompleted : EvaluationEditorErrors.SavingEvaluation, ex.Criteria.Count());
                }
                catch (Exception ex)
                {
                    View.DisplayError((completed) ? EvaluationEditorErrors.SavingEvaluationCompleted : EvaluationEditorErrors.SavingEvaluation);
                }
                
                LoadEvaluationData(
                    idCall,
                    (isAdvance) ?
                        ServiceCall.GetFullEvaluation(idEvaluation, View.AnonymousOwnerName, View.UnknownDisplayname) :
                        Service.GetFullEvaluation(idEvaluation, View.AnonymousOwnerName, View.UnknownDisplayname));
            }
        }
    }
}