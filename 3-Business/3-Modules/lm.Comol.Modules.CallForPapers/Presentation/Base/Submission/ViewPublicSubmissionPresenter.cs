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
    public class ViewPublicSubmissionPresenter : lm.Comol.Core.DomainModel.Common.DomainPresenter
    {
         #region "Initialize"
            private ServiceCallOfPapers _ServiceCall;
            private ServiceRequestForMembership _ServiceRequest;
            public virtual BaseModuleManager CurrentManager { get; set; }
            protected virtual IViewViewPublicSubmission View
            {
                get { return (IViewViewPublicSubmission)base.View; }
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
            public ViewPublicSubmissionPresenter(iApplicationContext oContext)
                : base(oContext)
            {
                this.CurrentManager = new BaseModuleManager(oContext);
            }
            public ViewPublicSubmissionPresenter(iApplicationContext oContext, IViewViewPublicSubmission view)
                : base(oContext, view)
            {
                this.CurrentManager = new BaseModuleManager(oContext);
            }
        #endregion

        public void InitView()
        {
            Boolean allowAdmin = false;
            Boolean isAnonymousUser = UserContext.isAnonymous;
            long idCall = View.PreloadIdCall;
            CallForPaperType type = ServiceCall.GetCallType(idCall);
            long idSubmission = View.PreloadedIdSubmission;
            long idRevision = View.PreloadedIdRevision;
            Guid uniqueId = View.PreloadedUniqueID;

            dtoSubmissionRevision submission = ServiceCall.GetSubmissionWithRevisions(idSubmission, true);
            int idModule = (type== CallForPaperType.CallForBids) ? ServiceCall.ServiceModuleID() : ServiceRequest.ServiceModuleID();
            
            dtoBaseForPaper call = ServiceCall.GetDtoBaseCall(idCall);
            int idCommunity = SetCallCurrentCommunity(call);

            View.isAnonymousSubmission = true;
            View.IdCall = idCall;
            View.IdCallModule = idModule;
            View.IdCallCommunity = idCommunity;
            View.CallType = type;
            View.IdSubmission = idSubmission;
            if (idRevision == 0 && submission != null)
                idRevision = submission.GetIdLastActiveRevision();
            View.IdRevision = idRevision;
            if (submission==null || submission.Owner==null || (submission != null && submission.UniqueId !=uniqueId))
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
                        View.SetActionUrl(CallStandardAction.List, RootObject.ViewCalls(type, CallStandardAction.List, idCommunity, View.PreloadView));
                }
                else if (isAnonymousUser && !submission.IsAnonymous )
                    View.DisplaySessionTimeout();
                else if (isAnonymousUser && !call.IsPublic)
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
                            allowAdmin = (module.ManageCallForPapers || module.Administration || ((module.CreateCallForPaper || module.EditCallForPaper) && call.Owner.Id == idUser));
                            break;
                        case CallForPaperType.RequestForMembership:
                            ModuleRequestForMembership moduleR = ServiceCall.RequestForMembershipServicePermission(idUser, idCommunity);
                            allowView = (submission.Owner.Id == idUser || moduleR.ManageBaseForPapers || moduleR.Administration || ((moduleR.CreateBaseForPaper || moduleR.EditBaseForPaper) && call.Owner.Id == idUser));
                            allowAdmin = (moduleR.ManageBaseForPapers || moduleR.Administration || ((moduleR.CreateBaseForPaper || moduleR.EditBaseForPaper) && call.Owner.Id == idUser));
                            break;
                    }
                    View.InitializeView(ServiceCall.GetExternalContext(idCall));
                    CallStatusForSubmitters fromView = View.PreloadView;
                    if (fromView == CallStatusForSubmitters.None)
                        fromView = (allowAdmin) ? CallStatusForSubmitters.SubmissionClosed : CallStatusForSubmitters.Submitted;

                    if (allowView)
                    {
                        if (!isAnonymousUser)
                            View.SetActionUrl(CallStandardAction.List, RootObject.ViewCalls(type, CallStandardAction.List, idCommunity, fromView));
                        else if (View.FromPublicList)
                            View.SetActionUrl(CallStandardAction.List, RootObject.PublicCollectorCalls(type, idCall, idCommunity));
                    }
                    if (submission.Deleted != BaseStatusDeleted.None && !allowAdmin)
                        View.DisplaySubmissionUnavailable();
                    else if (allowAdmin || allowView || (isAnonymousUser && submission.IsAnonymous && submission.UniqueId == View.PreloadedUniqueID))
                        LoadSubmission(call,submission,idUser,allowAdmin);
                    else
                        View.DisplayNoPermission(idCommunity, idModule);
                }
            }
        }
       
        private int SetCallCurrentCommunity(dtoBaseForPaper call)
        {
            dtoCallCommunityContext context = ServiceCall.GetCallCommunityContext(call, View.Portalname);
            View.CallType = call.Type;
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
            //else if (currentCommunity != null && !call.IsPortal )
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

                LoadSections(revision.IdCall, revision, false);
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
            View.InitializeExportControl(
                (subRev.Owner != null && UserContext.CurrentUserID == subRev.Owner.Id),
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