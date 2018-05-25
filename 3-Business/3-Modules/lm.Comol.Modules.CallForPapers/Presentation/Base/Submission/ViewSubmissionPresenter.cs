using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel;
using lm.Comol.Modules.CallForPapers.Business;
using lm.Comol.Modules.CallForPapers.Domain;
using lm.Comol.Core.Business;
using lm.Comol.Core.DomainModel.Helpers.Export;

namespace lm.Comol.Modules.CallForPapers.Presentation
{
    public class ViewSubmissionPresenter : lm.Comol.Core.DomainModel.Common.DomainPresenter
    {
         #region "Initialize"
            private ServiceCallOfPapers _ServiceCall;
            private ServiceRequestForMembership _ServiceRequest;
            public virtual BaseModuleManager CurrentManager { get; set; }
            protected virtual IViewViewSubmission View
            {
                get { return (IViewViewSubmission)base.View; }
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
            public ViewSubmissionPresenter(iApplicationContext oContext)
                : base(oContext)
            {
                this.CurrentManager = new BaseModuleManager(oContext);
            }
            public ViewSubmissionPresenter(iApplicationContext oContext, IViewViewSubmission view)
                : base(oContext, view)
            {
                this.CurrentManager = new BaseModuleManager(oContext);
            }
        #endregion

        public void InitView(Boolean forPublicCall)
        {
            Boolean allowAdmin = false;
            Boolean isAnonymousUser = UserContext.isAnonymous;
            long idCall = View.PreloadIdCall;
            CallForPaperType type = ServiceCall.GetCallType(idCall);
            long idSubmission = View.PreloadedIdSubmission;
            long idRevision = View.PreloadedIdRevision;
            Guid uniqueId = View.PreloadedUniqueID;


            View.IsAdvance = ServiceCall.CallIsAdvanced(idCall);


            dtoSubmissionRevision submission = ServiceCall.GetSubmissionWithRevisions(idSubmission, true);
            int idModule = (type== CallForPaperType.CallForBids) ? ServiceCall.ServiceModuleID() : ServiceRequest.ServiceModuleID();
            
            dtoBaseForPaper call = ServiceCall.GetDtoBaseCall(idCall);
            int idCommunity = SetCallCurrentCommunity(call);

            View.isAnonymousSubmission = forPublicCall;
            View.IdCall = idCall;
            View.IdCallModule = idModule;
            View.IdCallCommunity = idCommunity;
            View.CallType = type;
            View.IdSubmission = idSubmission;
            if (idRevision == 0 && submission != null)
                idRevision = submission.GetIdLastActiveRevision();
            View.IdRevision = idRevision;
            if (submission==null || submission.Owner==null )
                View.DisplayUnknownSubmission(idCommunity,idModule,idSubmission,type);
            else{
                if (idCall != submission.IdCall){
                    idCall = submission.IdCall;
                    call = ServiceCall.GetDtoBaseCall(idCall);
                    idCommunity = SetCallCurrentCommunity(call);
                    type = (call!=null) ? call.Type : type;
                    idModule = (type== CallForPaperType.CallForBids) ? ServiceCall.ServiceModuleID() : ServiceRequest.ServiceModuleID();
                   
                    View.IdCall = idCall;
                    View.IdCallModule = idModule;
                    View.IdCallCommunity = idCommunity;
                    View.CallType = type;
                }               
                if (call == null){
                    View.DisplayUnknownCall(idCommunity, idModule, idCall, type);
                    if (!isAnonymousUser)
                        View.SetActionUrl(CallStandardAction.List, RootObject.ViewCalls(type, CallStandardAction.List, View.PreloadIdOtherCommunity, View.PreloadView));
                }
                else if (isAnonymousUser && !forPublicCall)
                {
                    if (call.IsPublic && submission.IsAnonymous )
                        View.GoToUrl(RootObject.ViewSubmission(type, idCall, idSubmission, idRevision, uniqueId, true, false, View.PreloadView, View.PreloadIdOtherCommunity, 0));
                    else
                        View.DisplaySessionTimeout();
                }
                else if (isAnonymousUser && forPublicCall && !call.IsPublic)
                    View.DisplayCallUnavailableForPublic();
                else{
                    int idUser = UserContext.CurrentUserID;
                    litePerson currenUser = GetCurrentUser(ref idUser);
                    Boolean allowView = false;
                    View.CallRepository = call.GetRepositoryIdentifier();
                    switch(type){
                        case CallForPaperType.CallForBids:
                            ModuleCallForPaper module = ServiceCall.CallForPaperServicePermission(idUser, idCommunity);
                            allowView = (submission.Owner.Id == idUser || module.ManageCallForPapers || module.Administration || ((module.CreateCallForPaper || module.EditCallForPaper) && call.Owner.Id == idUser));
                            allowAdmin = (View.PreloadFromManagement && (module.ManageCallForPapers || module.Administration || ((module.CreateCallForPaper || module.EditCallForPaper) && call.Owner.Id == idUser)));
                            break;
                        case CallForPaperType.RequestForMembership:
                            ModuleRequestForMembership moduleR = ServiceCall.RequestForMembershipServicePermission(idUser, idCommunity);
                            allowView = (submission.Owner.Id == idUser || moduleR.ManageBaseForPapers || moduleR.Administration || ((moduleR.CreateBaseForPaper || moduleR.EditBaseForPaper) && call.Owner.Id == idUser));
                            allowAdmin = (View.PreloadFromManagement && (moduleR.ManageBaseForPapers || moduleR.Administration || ((moduleR.CreateBaseForPaper || moduleR.EditBaseForPaper) && call.Owner.Id == idUser)));
                            break;
                    }

                    if(View.IsAdvance)
                    {
                        Advanced.SubmissionListPermission permission = ServiceCall.SubmissionCanList(idCall);
                        
                        if((permission & Advanced.SubmissionListPermission.View) == Advanced.SubmissionListPermission.View)
                        {
                            allowView = true;
                        }

                        if ((permission & Advanced.SubmissionListPermission.Manage) == Advanced.SubmissionListPermission.Manage)
                        {
                            allowAdmin = true;
                        }
                    }


                    View.ShowAdministrationTools = allowAdmin;
                    CallStatusForSubmitters fromView = View.PreloadView;
                    if (fromView == CallStatusForSubmitters.None)
                        fromView = (allowAdmin) ? CallStatusForSubmitters.SubmissionClosed : CallStatusForSubmitters.Submitted;

                    if (!allowAdmin && allowView)
                        View.SetActionUrl(CallStandardAction.List, RootObject.ViewCalls(type, CallStandardAction.List, View.PreloadIdOtherCommunity, fromView));
                    else if (allowAdmin)
                        View.SetActionUrl(CallStandardAction.Manage, RootObject.ViewSubmissions(type, idCall, idSubmission, idRevision, fromView, View.PreloadFilterSubmission, View.PreloadOrderSubmission, View.PreloadAscending, View.PreloadPageIndex, View.PreloadPageSize));

                    if (submission.Deleted != BaseStatusDeleted.None && !allowAdmin)
                        View.DisplaySubmissionUnavailable();
                    else if (allowAdmin || allowView || (isAnonymousUser && submission.IsAnonymous && submission.UniqueId == View.PreloadedUniqueID))
                        LoadSubmission(call,submission,idUser,allowAdmin);
                    else
                    {
                       View.DisplayNoPermission(idCommunity, idModule);
                    }
                        
                }
            }

            bool ShowSendIntegration = false;
            if (call.AdvacedEvaluation)
                ShowSendIntegration = ServiceCall.ShowSendIntegration(idSubmission, View.CommissionId);

            View.ShowHideSendIntegration(ShowSendIntegration);

            if(View.IsAdvance)
            {
                if (!ServiceCall.SignSubmissionIsNotExpired(idSubmission, idRevision, DateTime.Now, DateTime.Now))
                {
                    bool ShowMessage = (submission.Status == SubmissionStatus.waitforsignature);
                    View.DisplayOutOfTime("Expired", ShowMessage, false);
                }
            }
        }
       
        private int SetCallCurrentCommunity(dtoBaseForPaper call)
        {
            dtoCallCommunityContext context = ServiceCall.GetCallCommunityContext(call, View.Portalname);
            View.SetContainerName(context.CommunityName, context.CallName);
            View.IdCallCommunity = context.IdCommunity;
            return context.IdCommunity;
            //int idCommunity = 0;
            //Community currentCommunity = CurrentManager.GetCommunity(this.UserContext.CurrentCommunityID);
            //Community community = null;
            //if (call != null)
            //    idCommunity = (call.IsPortal) ? 0 : (call.Community != null) ? call.Community.Id : 0;


            //community = CurrentManager.GetCommunity(idCommunity);
            //if (community != null)
            //    View.SetContainerName(community.Name, (call != null) ? call.Name : "");
            //else if (currentCommunity != null && !call.IsPortal)
            //{
            //    idCommunity = this.UserContext.CurrentCommunityID;
            //    View.SetContainerName(currentCommunity.Name, (call != null) ? call.Name : "");
            //}
            //else
            //{
            //    idCommunity = 0;
            //    View.SetContainerName(View.Portalname, (call != null) ? call.Name : "");
            //}
            //View.IdCallCommunity = idCommunity;
            //return idCommunity;
        }
        private void LoadSubmission(dtoBaseForPaper call, dtoSubmissionRevision subRev, int idUser, Boolean allowAdmin)
        {
            if (!allowAdmin){
                switch(call.Type){
                    case CallForPaperType.CallForBids:
                        dtoCall dtoC = ServiceCall.GetDtoCall(call.Id);
                        View.LoadCallInfo(dtoC);
                        break;
                    case CallForPaperType.RequestForMembership:
                        dtoRequest dtoR = ServiceRequest.GetDtoRequest(call.Id);
                        View.LoadCallInfo(dtoR);
                        break;
                }
                View.LoadAttachments(ServiceCall.GetAvailableCallAttachments(call.Id, subRev.Type.Id));
            }
            View.IdSubmitterType = (subRev == null) ? 0 : subRev.Type.Id;
            String owner = "";
            String submittedBy = "";
            if (subRev.Owner == null || subRev.Owner.TypeID == (int)UserTypeStandard.Guest)
                owner = View.AnonymousOwnerName;
            else
                owner = subRev.Owner.SurnameAndName;

            if (subRev.SubmittedBy == null || subRev.SubmittedBy.TypeID == (int)UserTypeStandard.Guest)
                submittedBy = View.AnonymousOwnerName;
            else
                submittedBy = subRev.SubmittedBy.SurnameAndName;

            if (subRev.Deleted != BaseStatusDeleted.None)
                View.LoadSubmissionInfo(subRev.Type.Name, owner, SubmissionStatus.deleted);
            else if (!subRev.SubmittedOn.HasValue)
                View.LoadSubmissionInfo(subRev.Type.Name, owner, subRev.Status);
            else if (subRev.IdPerson == subRev.IdSubmittedBy)
                View.LoadSubmissionInfo(subRev.Type.Name, owner, subRev.Status, subRev.SubmittedOn.Value);
            else
                View.LoadSubmissionInfo(subRev.Type.Name, owner, subRev.Status, subRev.SubmittedOn.Value, submittedBy);

            LoadSections(call, subRev, allowAdmin);


            //Gestione firme
            if (!call.AttachSign)
            {
                View.HideSignSubmission();
            }
            else
            {
                ModuleLink mLink = ServiceCall.GetSignModuleLink(call.Id, (View.IdRevision > 0) ? View.IdRevision : View.PreloadedIdRevision);

                if (mLink != null)
                {
                    View.InitializeDownloadSign(mLink);
                }
                else
                {
                    if (UserContext.CurrentUserID == subRev.IdSubmittedBy)
                    {
                        //dtoSubmissionRevision subRev = Service.GetSubmissionWithRevisions(idSubmission, true);

                        View.InitSignSubmission((call.Community != null) ? call.Community.Id : 0);
                    }
                    else
                    {
                        View.ShowSignNotSubmitted();
                    }
                }
            }


        }
        private void LoadSubmission(long idSubmission,long idRevision, int idUser)
        {
            dtoSubmissionRevision revision = ServiceCall.GetSubmissionWithRevisions(idSubmission,true);
            if (revision != null)
            {
                String owner = "";
                String submittedBy = "";
                if (revision.Owner == null || revision.Owner.TypeID == (int)UserTypeStandard.Guest)
                    owner = View.AnonymousOwnerName;
                else
                    owner = revision.Owner.SurnameAndName;

                if (revision.SubmittedBy == null || revision.SubmittedBy.TypeID == (int)UserTypeStandard.Guest)
                    submittedBy = View.AnonymousOwnerName;
                else
                    submittedBy = revision.SubmittedBy.SurnameAndName;

                if (revision.Deleted != BaseStatusDeleted.None)
                    View.LoadSubmissionInfo(revision.Type.Name, owner, SubmissionStatus.deleted);
                else if (!revision.SubmittedOn.HasValue)
                    View.LoadSubmissionInfo(revision.Type.Name, owner, revision.Status);
                else if (revision.IdPerson == revision.IdSubmittedBy)
                    View.LoadSubmissionInfo(revision.Type.Name, owner, revision.Status, revision.SubmittedOn.Value);
                else
                    View.LoadSubmissionInfo(revision.Type.Name, owner, revision.Status, revision.SubmittedOn.Value, submittedBy);

                LoadSections(revision.IdCall, revision, View.ShowAdministrationTools);
            }
        }
        public void LoadSections(long idCall, CallForPaperType type, long idSubmission, long idRevision, long idSubmitter, Boolean allowAdmin)
        {
            BaseForPaper call = ServiceCall.GetCall(idCall);
            if (call != null)
            {
                LoadSections(call, ServiceCall.GetSubmissionWithRevisions(idSubmission, true), allowAdmin);
            }
        }
        public void LoadSections(long idCall, dtoSubmissionRevision subRev, Boolean allowAdmin)
        {
            BaseForPaper call = ServiceCall.GetCall(idCall);
            if (call != null)
                LoadSections(call, subRev, allowAdmin);
        }
        private void LoadSections(dtoBaseForPaper baseCall, dtoSubmissionRevision subRev, Boolean allowAdmin)
        {
            BaseForPaper call = ServiceCall.GetCall(baseCall.Id);
            if (call != null)
            {
                LoadSections(call, subRev, allowAdmin);
            }
        }
        private void LoadSections(BaseForPaper call, dtoSubmissionRevision subRev, Boolean allowAdmin)
        {
            if (allowAdmin)
                LoadAvailableStatus(subRev);
            if (call.Type == CallForPaperType.CallForBids) {
                List<dtoCallSubmissionFile> requiredFiles = ServiceCall.GetRequiredFiles(call, subRev.Type.Id, subRev.Id);
                Dictionary<long, FieldError> filesError = ServiceCall.GetSubmissionRequiredFileErrors(subRev.Id);
                if (requiredFiles != null && requiredFiles.Count > 0 && filesError != null)
                    requiredFiles.ForEach(f => f.SetError(filesError));
                View.LoadRequiredFiles(requiredFiles);
            }

            List<dtoCallSection<dtoSubmissionValueField>> sections = ServiceCall.GetSubmissionFields(call, subRev.Type.Id, subRev.Id, View.IdRevision);
            Dictionary<long, FieldError> fieldsError = ServiceCall.GetSubmissionFieldErrors(subRev.Id, View.IdRevision);
            if (sections != null && sections.Count > 0 && fieldsError != null)
                sections.ForEach(s => s.Fields.ForEach(f => f.SetError(fieldsError)));
            View.LoadSections(sections);
            LoadRevisionInfos(View.ShowAdministrationTools, call, subRev);
            View.InitializeExportControl(
                (subRev.Owner != null && subRev.Owner.Id == UserContext.CurrentUserID),
                (subRev.Owner != null) ? subRev.Owner.Id : 0,
                call.Id,
                subRev.Id,
                View.IdRevision,
                View.IdCallModule,
                View.IdCallCommunity,
                View.CallType,
                subRev.Type.Id,
                subRev.Status == SubmissionStatus.draft);

        }

        private void LoadAvailableStatus(dtoSubmissionRevision revision)
        {
            List<SubmissionStatus> items = new List<SubmissionStatus>();
            switch (revision.Status)
            { 
                case SubmissionStatus.submitted:
                    items.Add(SubmissionStatus.accepted);
                    items.Add(SubmissionStatus.rejected);
                    break;
                case SubmissionStatus.draft:
                case SubmissionStatus.none:
                    items.Add(SubmissionStatus.tosubmit);
                    break;
            }
            View.LoadAvailableStatus(items);
        }

        public void SaveStatus(SubmissionStatus status, string webSiteurl, lm.Comol.Core.MailCommons.Domain.Configurations.SmtpServiceConfig smtpConfig, String body, String subject)
        {
            try
            {
                if (UserContext.isAnonymous)
                {
                    if (View.isAnonymousSubmission)
                        View.GoToUrl(RootObject.ViewSubmission(View.CallType, View.IdCall, View.IdSubmission, View.IdRevision, View.PreloadedUniqueID, true, false, View.PreloadView, View.PreloadIdOtherCommunity, 0));
                    else
                        View.DisplaySessionTimeout();
                }
                else
                {
                    if (ServiceCall.EditSubmissionStatus(View.IdSubmission, UserContext.CurrentUserID, status,webSiteurl, smtpConfig, body, subject))
                    {
                        switch (View.CallType)
                        {
                            case CallForPaperType.CallForBids:
                                View.SendUserAction(View.IdCallCommunity, View.IdCallModule, View.IdSubmission, (status == SubmissionStatus.accepted ? ModuleCallForPaper.ActionType.AcceptSubmission : (status == SubmissionStatus.rejected) ? ModuleCallForPaper.ActionType.RejectSubmission : ModuleCallForPaper.ActionType.EditSubmission));
                                break;
                            case CallForPaperType.Conference:
                                View.SendUserAction(View.IdCallCommunity, View.IdCallModule, View.IdSubmission, (status == SubmissionStatus.accepted ? ModuleRequestForMembership.ActionType.AcceptSubmission : (status == SubmissionStatus.rejected) ? ModuleRequestForMembership.ActionType.RejectSubmission : ModuleRequestForMembership.ActionType.EditSubmission));
                                break;
                        }
                        LoadSubmission(View.IdSubmission, View.IdRevision, UserContext.CurrentUserID);
                    }
                }

                //View.GoToCallForPaperPage(View.CallCommunityID, View.CallForPaperID, View.PreloadView, View.PreloadFilterSubmission, View.PreloadOrderSubmission, View.PreloadPageIndex);
            }
            catch (SubmissionStatusChange ex)
            {
  
            }
        }

        public void ChangeRevision(long idRevision){
            View.IdRevision = idRevision;
            LoadSubmission(View.IdSubmission, idRevision, UserContext.CurrentUserID);
        }
        private void LoadRevisionInfos(Boolean allowAdmin, BaseForPaper call, dtoSubmissionRevision submission)
        {
            Boolean allowRevision = false;
            Boolean isOwner = (submission.IdPerson == UserContext.CurrentUserID);
            switch (call.RevisionSettings)
            {
                case RevisionMode.OnlyManager:
                    allowRevision = allowAdmin && !submission.IsAnonymous;
                    break;
                case RevisionMode.ManagerSubmitter:
                    allowRevision = (allowAdmin && !submission.IsAnonymous) || (isOwner && !submission.IsAnonymous );
                    break;
                case RevisionMode.OnlySubmitter:
                    allowRevision = (isOwner && !submission.IsAnonymous);
                    break;
                default:
                    allowRevision = false;
                    break;

            }
            Boolean submissionRevision = !call.AdvacedEvaluation && (allowRevision && submission.Status >= SubmissionStatus.submitted && submission.Status != SubmissionStatus.rejected) ;
            
            // SE posso fare la revisione e NON ci sono revisioni in corso !
            View.AllowRevisionRequest = submissionRevision && submission.HasWorkingRevision() == false;

            dtoRevisionRequest revision = submission.GetWorkingRevision();
            List<dtoRevision> acceptRevisions = submission.GetAcceptedRevisions();

            long currentRevision = View.IdRevision;
            if (allowAdmin && currentRevision > 0)
            {
                if (revision != null) {
                    Int32 version = acceptRevisions.Where(r => r.Id <= currentRevision).Select(r => r.Number).Max();
                    Int32 subVersion = submission.Revisions.Where(r => r.Id > acceptRevisions.Where(a => a.IsActive && a.Id <= currentRevision).Select(t=> t.Id).FirstOrDefault() &&  r.Id < currentRevision).Count()+1;
                    //revision.SubVersion = subVersion;
                    //acceptRevisions.Insert(0,revision);

                    //if (currentRevision == revision.Id && revision.Status != RevisionStatus.Submitted) {
                        switch (revision.Type)
                        {
                            case RevisionType.UserRequired:
                                View.DisplayUserPendingRequest(revision, RootObject.UserReviewCall(View.CallType, View.IdCall, View.IdSubmission, revision.Id, CallStatusForSubmitters.Submitted, View.PreloadIdOtherCommunity));
                                break;
                            case RevisionType.Manager:
                                View.DisplayManagePendingRevision(revision, RootObject.UserReviewCall(View.CallType, View.IdCall, View.IdSubmission, revision.Id, CallStatusForSubmitters.Submitted, View.PreloadIdOtherCommunity));
                                break;
                        }
                    //}
                }
                if (submissionRevision && !submission.HasWorkingRevision())
                    View.InitializeRevisionRequest(((allowAdmin) ? RevisionType.Manager : RevisionType.None));
            }
            else
            {

                // Imposto le richieste di revisione
                if (submissionRevision && !submission.HasWorkingRevision())
                    View.InitializeRevisionRequest((isOwner) ? RevisionType.UserRequired : ((allowAdmin) ? RevisionType.Manager : RevisionType.None));
                if (revision != null && revision.Status != RevisionStatus.Submitted)
                {
                    View.IdPendingRevision = revision.Id;
                    if (isOwner)
                    {
                        switch (revision.Type)
                        {
                            case RevisionType.UserRequired:
                                View.DisplaySelfPendingRequest(revision, RootObject.UserReviewCall(View.CallType, View.IdCall, View.IdSubmission, revision.Id, CallStatusForSubmitters.Submitted, View.PreloadIdOtherCommunity));
                                break;
                            case RevisionType.Manager:
                                View.DisplayPendingRevision(revision, RootObject.UserReviewCall(View.CallType, View.IdCall, View.IdSubmission, revision.Id, CallStatusForSubmitters.Submitted, View.PreloadIdOtherCommunity));
                                break;
                        }
                    }

                    else if (allowAdmin)
                    {
                        switch (revision.Type)
                        {
                            case RevisionType.UserRequired:
                                View.DisplayUserPendingRequest(revision, RootObject.UserReviewCall(View.CallType, View.IdCall, View.IdSubmission, revision.Id, CallStatusForSubmitters.Submitted, View.PreloadIdOtherCommunity));
                                break;
                            case RevisionType.Manager:
                                View.DisplayManagePendingRevision(revision, RootObject.UserReviewCall(View.CallType, View.IdCall, View.IdSubmission, revision.Id, CallStatusForSubmitters.Submitted, View.PreloadIdOtherCommunity));
                                break;
                        }
                    }
                }
                if (revision == null || (revision != null && (revision.Id != View.IdRevision || revision.Status == RevisionStatus.Submitted)))
                    View.DisplayRevisionInfo(submission.Revisions.Where(r => r.Id == View.IdRevision).FirstOrDefault());
            }

            // Carico la lista delle revisioni attualmente accettate !
            View.LoadAvailableRevision(acceptRevisions, View.IdRevision);
        }

        //
        public void AskForRevision(long idSubmission, string reason, dtoRevisionMessage message, String webSiteurl)
        {
            int idUser = UserContext.CurrentUserID;
            if (!UserContext.isAnonymous)
            {
                Revision rev = ServiceCall.AskForRevision(idSubmission, idUser, reason,webSiteurl, RootObject.ViewSubmission(View.CallType,View.IdCall, View.IdSubmission,false, CallStatusForSubmitters.Submitted,View.PreloadIdOtherCommunity, 0), message);
                if (rev != null)
                    View.IdPendingRevision = rev.Id; 
            }
            LoadSubmission(View.IdSubmission, View.IdRevision, idUser);
        }
        public void RequireRevision(long idSubmission, string reason, dtoRevisionMessage message, List<dtoRevisionItem> fieldsToReview, DateTime deadline, String webSiteurl, String baseFilePath, String baseThumbnailPath)
        {
            int idUser = UserContext.CurrentUserID;
            if (!UserContext.isAnonymous)
            {
                Revision rev = ServiceCall.RequireRevision(idSubmission, idUser, View.PreloadIdOtherCommunity, reason, message, fieldsToReview, deadline, webSiteurl, baseFilePath, baseThumbnailPath);
                if (rev != null)
                    View.IdPendingRevision = rev.Id; 
            }
            LoadSubmission(View.IdSubmission, View.IdRevision, idUser);
        }
        public void RemoveSelfRequest(long idSubmission,long idRevision, dtoRevisionMessage selfRemoveMessage,String webSiteurl)
        {
            int idUser = UserContext.CurrentUserID;
            if (!UserContext.isAnonymous) {
                if (ServiceCall.RemoveSelfRequest(View.CallType,idSubmission, idRevision, idUser, View.PreloadIdOtherCommunity, selfRemoveMessage, webSiteurl))
                    View.IdPendingRevision = 0;
            }
            LoadSubmission(View.IdSubmission, View.IdRevision, idUser);
        }
        public void RemoveManagerRequest(long idSubmission, long idRevision, dtoRevisionMessage managerMessage, String webSiteurl)
        {
            int idUser = UserContext.CurrentUserID;
            if (!UserContext.isAnonymous)
            {
                if (ServiceCall.RemoveManagerRequest(View.CallType, idSubmission, idRevision, idUser,View.PreloadIdOtherCommunity, "", managerMessage, webSiteurl))
                    View.IdPendingRevision = 0;
            }
            LoadSubmission(View.IdSubmission, View.IdRevision, idUser);
        }

         public void StartManagerRevision(long idSubmission, long idRevision)
        {
            List<dtoCallSection<dtoSubmissionValueField>> sections = ServiceCall.GetSubmissionFields(ServiceCall.GetCall(View.IdCall), View.IdSubmitterType, idSubmission, View.IdRevision);
            Dictionary<long, FieldError> fieldsError = ServiceCall.GetSubmissionFieldErrors(idSubmission, View.IdRevision);
            if (sections != null && sections.Count > 0 && fieldsError != null)
                sections.ForEach(s => s.Fields.ForEach(f => f.SetError(fieldsError)));
            sections.ForEach(s => s.Fields.ForEach(f => f.AllowSelection=true));
            View.LoadSections(sections);
        }


        //public void AcceptUserRequest(long idSubmission, long idRevision, string reason, dtoRevisionMessage message, List<dtoRevisionItem> fieldsToReview, DateTime deadline)
        //{
        //    int idUser = UserContext.CurrentUserID;
        //    if (!UserContext.isAnonymous)
        //    {
        //        ServiceCall.AcceptUserRequest(idUser,idSubmission, idRevision, reason, message, fieldsToReview, deadline);
        //    }
        //    LoadSubmission(View.IdSubmission, View.IdRevision, idUser);
        //}
        //public void RefuseUserRequest(long idSubmission, long idRevision, string reason, dtoRevisionMessage message)
        //{
        //    int idUser = UserContext.CurrentUserID;
        //    if (!UserContext.isAnonymous)
        //    {
        //        ServiceCall.RefuseUserRequest(idUser, idSubmission, idRevision, reason, message);
        //    }
        //    LoadSubmission(View.IdSubmission, View.IdRevision, idUser);
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

         public void UploadSign(Boolean forPublicCall,
             List<dtoSubmissionValueField> fields,
             lm.Comol.Core.MailCommons.Domain.Configurations.SmtpServiceConfig smtpConfig,
             String websiteUrl,
              Dictionary<SubmissionTranslations, string> translations,
             DateTime clickDt
             )
         {
            //Lo tengo da parte, per evitare che lo scadere della sessione dia problemi.
            Int32 idUser = UserContext.CurrentUserID;



            long idSubmission = View.IdSubmission;
            long idRevision = View.IdRevision;
             //System.Guid uniqueID = View.PreloadedUniqueID;
            long idCall = View.PreloadIdCall;

            DateTime initDt = clickDt;

            //Aggiugnere blocco se call scaduta!
            if(!ServiceCall.SignSubmissionIsNotExpired(idSubmission, idRevision, initDt, clickDt))
            {
                View.DisplayOutOfTime("Expired", true, false);
                return;
            }


             UserSubmission submission = CurrentManager.Get<UserSubmission>(idSubmission);
             Revision revision = ServiceCall.GetRevision(idRevision);

             ModuleActionLink aLink = View.AddInternalFile(
                 revision,
                 ModuleCallForPaper.UniqueCode,
                 ServiceCall.ServiceModuleID(),
                 (int)ModuleCallForPaper.ActionType.DownloadSubmittedFile,
                 (int)ModuleCallForPaper.ObjectType.UserSubmission
                 );

             if (aLink == null || aLink.Link == null)
             {
                 //ToDo: error
             }
             else
             {
                 //Manager.SaveOrUpdate(mLink);

                 ServiceCall.SetSignLink(
                     idCall,
                     revision.Id,
                     aLink);

                 

                bool success = ServiceCall.UserCompleteSubmissionSign(
                     idSubmission,
                     idRevision,
                     DateTime.Now,
                     idUser,
                     fields,
                     smtpConfig,
                     websiteUrl,
                     translations,
                     clickDt
                     );

                if(!success)
                {
                    View.DisplayOutOfTime("Expired", true, true);
                    return;
                }
                    
            }

            View.SendUserAction(
                   UserContext.CurrentCommunityID,
                   ServiceCall.ServiceModuleID(),
                   idCall,
                   ModuleCallForPaper.ActionType.SignUploaded);

             InitView(forPublicCall);
         }

        public void SendIntegration(
            lm.Comol.Core.MailCommons.Domain.Configurations.SmtpServiceConfig smtpConfig,
            string Subject, string Body, string BaseUrl, string IntegrationEndOn)
        {
            bool success = ServiceCall.SendIntegration(
                  View.IdSubmission,
                  View.CommissionId,
                  smtpConfig,
                  0,
                  Subject, Body,
                  IntegrationEndOn, 
                  BaseUrl,
                  false, true);

            View.ShowSendInfo(success);

        }
    }
}