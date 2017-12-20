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
    public class ManageReviewPresenter : lm.Comol.Core.DomainModel.Common.DomainPresenter
    {
         #region "Initialize"
            private ServiceCallOfPapers _Service;
            private ServiceRequestForMembership _ServiceRequest;
            public virtual BaseModuleManager CurrentManager { get; set; }
            protected virtual IViewManageReview View
            {
                get { return (IViewManageReview)base.View; }
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
            public ManageReviewPresenter(iApplicationContext oContext)
                : base(oContext)
            {
                this.CurrentManager = new BaseModuleManager(oContext);
            }
            public ManageReviewPresenter(iApplicationContext oContext, IViewManageReview view)
                : base(oContext, view)
            {
                this.CurrentManager = new BaseModuleManager(oContext);
            }
        #endregion

        public void InitView()
        {
            Boolean isAnonymousUser = UserContext.isAnonymous;
            long idCall = View.PreloadIdCall;

            CallForPaperType type = Service.GetCallType(idCall);
            long idSubmission = View.PreloadedIdSubmission;
            long idRevision = View.PreloadedIdRevision;

            dtoSubmissionRevision submission = Service.GetSubmissionWithRevisions(idSubmission, true);
            int idModule = (type == CallForPaperType.CallForBids) ? Service.ServiceModuleID() : ServiceRequest.ServiceModuleID();
            View.CallType = type;

            dtoBaseForPaper call = Service.GetDtoBaseCall(idCall);
            int idCommunity = SetCallCurrentCommunity(call);

            View.IdCall = idCall;
            View.IdCallModule = idModule;
            View.IdCallCommunity = idCommunity;
            
            View.IdSubmission = idSubmission;
            if (submission != null && (idRevision == 0  && submission.Revisions.Where(r=>r.Id == idRevision && r.AllowSave).Any()))
                idRevision = submission.GetIdWorkingRevision();
            View.IdRevision = idRevision;

            if (isAnonymousUser)
                View.DisplaySessionTimeout();
            else if (submission == null)
            {
                View.DisplayRevisionUnknown();
                if (View.PreloadFromManagement)
                    View.SetActionUrl(RootObject.ViewSubmissions(type, idCall, idSubmission, idRevision, View.PreloadView, View.PreloadFilterSubmission, View.PreloadOrderSubmission, View.PreloadAscending, View.PreloadPageIndex, View.PreloadPageSize));
                else
                    View.SetActionUrl(RootObject.ViewRevisions(idRevision, type, View.PreloadAction, idCommunity, View.PreloadView));
            }
            else if (submission != null && (idRevision ==0 ))
            {
                View.DisplayRevisionUnavailable();
                View.SetActionUrl(RootObject.ViewRevisions(idRevision, type, View.PreloadAction, idCommunity, View.PreloadView));
            }
            else
            {
                int idUser = UserContext.CurrentUserID;
                litePerson currenUser = GetCurrentUser(ref idUser);
                Boolean allowView = false;

                switch (type)
                {
                    case CallForPaperType.CallForBids:
                        ModuleCallForPaper module = Service.CallForPaperServicePermission(idUser, idCommunity);
                        allowView = (submission.Owner.Id == idUser || module.ManageCallForPapers || module.Administration || ((module.CreateCallForPaper || module.EditCallForPaper) && call.Owner.Id == idUser));
                        break;
                    case CallForPaperType.RequestForMembership:
                        ModuleRequestForMembership moduleR = Service.RequestForMembershipServicePermission(idUser, idCommunity);
                        allowView = (submission.Owner.Id == idUser || moduleR.ManageBaseForPapers || moduleR.Administration || ((moduleR.CreateBaseForPaper || moduleR.EditBaseForPaper) && call.Owner.Id == idUser));
                        break;
                }
                View.IdSubmission = idSubmission;

                if (idRevision == 0)
                    idRevision = submission.GetIdLastActiveRevision();
                dtoRevisionRequest rev = Service.GetRevisionRequest(idRevision,true);
                if (rev == null)
                    View.DisplayRevisionUnknown();
                else if (allowView)
                {
                    switch (type)
                    {
                        case CallForPaperType.CallForBids:
                            View.SendUserAction(idCommunity, idModule, idRevision, ModuleCallForPaper.ActionType.ViewRevision);
                            break;
                        case CallForPaperType.RequestForMembership:
                            View.SendUserAction(idCommunity, idModule, idRevision, ModuleRequestForMembership.ActionType.ViewRevision);
                            break;
                    }
                    LoadSubmission(call, submission, rev);
                }
                else
                    View.DisplayNoPermission(idCommunity, idModule);
                if (allowView)
                {
                    if (View.PreloadFromManagement)
                        View.SetActionUrl(RootObject.ViewSubmissions(type, idCall, idSubmission, idRevision, View.PreloadView, View.PreloadFilterSubmission, View.PreloadOrderSubmission,View.PreloadAscending, View.PreloadPageIndex,View.PreloadPageSize));
                    else
                        View.SetActionUrl(RootObject.ViewRevisions(idRevision, type, View.PreloadAction, idCommunity, View.PreloadView));
                }
            }
        }

        private int SetCallCurrentCommunity(dtoBaseForPaper call)
        {
            Int32 idCommunity = Service.GetCallIdCommunityContext(call);
            View.IdCallCommunity = idCommunity;
            return idCommunity;
            //int idCommunity = 0;
            //Community currentCommunity = CurrentManager.GetCommunity(this.UserContext.CurrentCommunityID);
            //Community community = null;
            //if (call != null)
            //    idCommunity = (call.IsPortal) ? 0 : (call.Community != null) ? call.Community.Id : 0;


            //community = CurrentManager.GetCommunity(idCommunity);
            ////if (community != null)
            ////    View.SetContainerName(community.Name, (call != null) ? call.Name : "");
            //if (currentCommunity != null && !call.IsPortal)
            //{
            //    idCommunity = this.UserContext.CurrentCommunityID;
            //   // View.SetContainerName(currentCommunity.Name, (call != null) ? call.Name : "");
            //}
            //else
            //{
            //    idCommunity = 0;
            //  //  View.SetContainerName(View.Portalname, (call != null) ? call.Name : "");
            //}
            //View.IdCallCommunity = idCommunity;
            //return idCommunity;
            }

        private void LoadSubmission(dtoBaseForPaper call, dtoSubmissionRevision subRev, dtoRevisionRequest rev)
        {
            switch (call.Type)
            {
                case CallForPaperType.CallForBids:
                    dtoCall dtoC = Service.GetDtoCall(call.Id);
                    View.LoadCallInfo(dtoC);
                    break;
                case CallForPaperType.RequestForMembership:
                    dtoRequest dtoR = ServiceRequest.GetDtoRequest(call.Id);
                    View.LoadCallInfo(dtoR);
                    break;
            }

            View.IdSubmitterType = (subRev == null) ? 0 : subRev.Type.Id;
            String owner = "";
            String submittedBy = "";
            if (subRev.Owner == null || subRev.Owner.TypeID == (int)UserTypeStandard.Guest)
            {
                owner = View.AnonymousOwnerName;
                Int32 idUser = 0;
                GetCurrentUser(ref idUser);
                View.IdUserSubmitter = idUser;
            }
            else
            {
                owner = subRev.Owner.SurnameAndName;
                View.IdUserSubmitter = subRev.Owner.Id;
            }

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

            LoadSections(call.Id, subRev.Id, rev);
        }
        private void LoadSubmission(long idCall, long idSubmission, long idRevision)
        {
            LoadSections(idCall, idSubmission, Service.GetRevisionRequest(idRevision,true));
        }
        private void LoadSections(long idCall, long idSubmission, dtoRevisionRequest rev)
        {
            BaseForPaper call = Service.GetCall(idCall);
            if (call != null)
                LoadSections(call, idSubmission, rev);
        }
        private void LoadSections(BaseForPaper call, long idSubmission, dtoRevisionRequest rev)
        {
            List<dtoCallSection<dtoSubmissionValueField>> sections = Service.GetSubmissionFields(Service.GetCall(View.IdCall), View.IdSubmitterType, idSubmission, rev.Id);
            Dictionary<long, FieldError> fieldsError = Service.GetSubmissionFieldErrors(idSubmission, View.IdRevision);
            if (sections != null && sections.Count > 0 && fieldsError != null)
                sections.ForEach(s => s.Fields.ForEach(f => f.SetError(fieldsError)));

            Boolean allowSelection = false;
            allowSelection = (rev.ItemsToReview.Count == 0 || !Service.RevisionIsReviewed(idSubmission, rev.Id, rev.ItemsToReview.Select(r=>r.IdField).ToList())); 

            sections.ForEach(s => s.Fields.ForEach(f => f.AllowSelection = allowSelection));

            View.CurrentStatus = rev.Status;
            View.FieldsToCheck = rev.ItemsToReview.Select(i => i.IdField).ToList();
            View.DisplayRevisionInfo(rev);
            View.InitializeExportControl((View.IdUserSubmitter== UserContext.CurrentUserID), View.IdUserSubmitter, call.Id, idSubmission,rev.Id, View.IdCallModule, View.IdCallCommunity, View.CallType);
            View.LoadSections(sections);
        }
      
        public void StartManagerRevision(long idSubmission, long idRevision)
        {
            List<dtoCallSection<dtoSubmissionValueField>> sections = Service.GetSubmissionFields(Service.GetCall(View.IdCall), View.IdSubmitterType, idSubmission, View.IdRevision);
            Dictionary<long, FieldError> fieldsError = Service.GetSubmissionFieldErrors(idSubmission, View.IdRevision);
            if (sections != null && sections.Count > 0 && fieldsError != null)
                sections.ForEach(s => s.Fields.ForEach(f => f.SetError(fieldsError)));
            sections.ForEach(s => s.Fields.ForEach(f => f.AllowSelection = true));
            View.LoadSections(sections);
        }

        public void AcceptUserRevision(long idSubmission, long idRevision, string reason, dtoRevisionMessage message, String webSiteurl)
        {
            int idUser = UserContext.CurrentUserID;
            if (!UserContext.isAnonymous)
                Service.ManageUserRevision(true, View.CallType  , idUser,View.PreloadIdCommunity, idSubmission, idRevision, reason, message, webSiteurl);
            LoadSubmission(View.IdCall, View.IdSubmission, View.IdRevision);
        }
        public void RefuseUserRevision(long idSubmission, long idRevision, string reason, dtoRevisionMessage message, String webSiteurl)
        {
            int idUser = UserContext.CurrentUserID;
            if (!UserContext.isAnonymous)
                Service.ManageUserRevision(false, View.CallType, idUser, View.PreloadIdCommunity, idSubmission, idRevision, reason, message, webSiteurl);
            LoadSubmission(View.IdCall, View.IdSubmission, View.IdRevision);
        }

        public void AcceptUserRequest(long idSubmission, long idRevision, string reason, DateTime deadline, List<dtoRevisionItem> fieldsToReview,  String baseFilePath, String baseThumbnailPath, dtoRevisionMessage message, String webSiteurl)
        {
            int idUser = UserContext.CurrentUserID;
            if (!UserContext.isAnonymous)
                Service.AcceptUserRequest(idUser, idSubmission, idRevision, reason, message, fieldsToReview, deadline, webSiteurl, baseFilePath, baseThumbnailPath);

            LoadSubmission(View.IdCall, View.IdSubmission, View.IdRevision);
        }
        public void RefuseUserRequest(long idSubmission, long idRevision, string reason, dtoRevisionMessage message, String webSiteurl)
        {
            int idUser = UserContext.CurrentUserID;
            if (!UserContext.isAnonymous)
                Service.RefuseUserRequest(View.CallType, idUser, View.PreloadIdCommunity, idSubmission, idRevision, reason, message, webSiteurl);
            LoadSubmission(View.IdCall,View.IdSubmission, View.IdRevision);
        }
        public void RemoveUserRequest(long idSubmission, long idRevision, string reason, dtoRevisionMessage removeUserMessage, String webSiteurl)
        {
            int idUser = UserContext.CurrentUserID;
            if (!UserContext.isAnonymous)
                Service.RemoveUserRequest(View.CallType, idSubmission, idRevision, idUser, View.PreloadIdCommunity, reason, removeUserMessage, webSiteurl);

            LoadSubmission(View.IdCall, View.IdSubmission, View.IdRevision);
        }
        public void RemoveManagerRequest(long idSubmission, long idRevision, string reason, dtoRevisionMessage managerMessage, String webSiteurl)
        {
            int idUser = UserContext.CurrentUserID;
            if (!UserContext.isAnonymous)
                Service.RemoveManagerRequest(View.CallType, idSubmission, idRevision, idUser, View.PreloadIdCommunity, reason, managerMessage, webSiteurl);
            LoadSubmission(View.IdCall, View.IdSubmission, View.IdRevision);
        }

        public void SaveSettings(long idSubmission, long idRevision, DateTime deadline, List<dtoRevisionItem> fieldsToReview, dtoRevisionMessage userMessage, String webSiteurl)
        {
            int idUser = UserContext.CurrentUserID;
            if (!UserContext.isAnonymous)
            {
                RevisionErrorView result = Service.SaveRevisionSettings(View.CallType,View.IdCall, idSubmission, idRevision, idUser, deadline,fieldsToReview, userMessage, webSiteurl);
                if (result == RevisionErrorView.None)
                    LoadSubmission(View.IdCall, View.IdSubmission, View.IdRevision);
                else
                    View.DisplayRevisionError(result);
            }
            else
                LoadSubmission(View.IdCall, View.IdSubmission, View.IdRevision);
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