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
    public class ReviewSubmissionPresenter : lm.Comol.Core.DomainModel.Common.DomainPresenter
    {
         #region "Initialize"
            private ServiceCallOfPapers _Service;
            private ServiceRequestForMembership _ServiceRequest;
            public virtual BaseModuleManager CurrentManager { get; set; }
            protected virtual IViewReviewSubmission View
            {
                get { return (IViewReviewSubmission)base.View; }
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
            public ReviewSubmissionPresenter(iApplicationContext oContext)
                : base(oContext)
            {
                this.CurrentManager = new BaseModuleManager(oContext);
            }
            public ReviewSubmissionPresenter(iApplicationContext oContext, IViewReviewSubmission view)
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
            Guid uniqueId = View.PreloadedUniqueID;

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
            Int32 containerIdCommunity = (View.PreloadIdOtherCommunity != -1) ? View.PreloadIdOtherCommunity : idCommunity;

            if (isAnonymousUser)
                View.DisplaySessionTimeout();
            else if (submission == null)
            {
                View.DisplayRevisionUnknown();
                View.SetActionUrl(CallStandardAction.ViewRevisions, RootObject.ViewRevisions(type, CallStandardAction.List, containerIdCommunity, CallStatusForSubmitters.Revisions));
            }
            else if (submission != null && (idRevision ==0 || submission.IsAnonymous || submission.IdPerson != UserContext.CurrentUserID))
            {
                if (idRevision ==0)
                    View.DisplayRevisionUnavailable();
                else
                    View.DisplayNoPermission(idCommunity, idModule);
                View.SetActionUrl(CallStandardAction.ViewRevisions, RootObject.ViewRevisions(type, CallStandardAction.List, containerIdCommunity , CallStatusForSubmitters.Revisions));
            }
            else
            {
                int idUser = UserContext.CurrentUserID;
                litePerson currenUser = GetCurrentUser(ref idUser);
                Boolean allowView = false;
                View.CallRepository = call.GetRepositoryIdentifier();
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
                View.IdSubmitterType = submission.Type.Id;
                View.TryToComplete = false;

                dtoRevisionRequest rev = submission.GetWorkingRevision();

                View.InitializeView(Service.RevisionWithFileToUpload(idRevision));
                DateTime InitTime = DateTime.Now;
                Boolean allowTimeRevision = rev.AllowSubmission(InitTime);
                Boolean allowSave = false;
                Boolean allowCompleteSubmission = false;


                if (!allowTimeRevision && (rev.Status == RevisionStatus.Request || rev.Status == RevisionStatus.RequestAccepted || rev.Status == RevisionStatus.Required))
                {
                    View.DisplayRevisionTimeExpired();
                    switch (type)
                    {
                        case CallForPaperType.CallForBids:
                            View.SendUserAction(idCommunity, idModule, idRevision, ModuleCallForPaper.ActionType.ViewRevision);
                            break;
                        case CallForPaperType.RequestForMembership:
                            View.SendUserAction(idCommunity, idModule, idRevision, ModuleRequestForMembership.ActionType.ViewRevision);
                            break;
                    }
                }
                else
                {
                    idSubmission = (submission == null) ? 0 : submission.Id;
                    View.AllowDeleteSubmission = false;
                    //View.AllowDeleteSubmission = (submission != null && submission.Deleted == BaseStatusDeleted.None && submission.Status == SubmissionStatus.draft);
                    allowCompleteSubmission = allowTimeRevision && (rev != null && rev.Deleted == BaseStatusDeleted.None && rev.AllowSave);
                    allowSave = allowCompleteSubmission;
                    View.InitSubmissionTime = InitTime;

                    switch (type)
                    {
                        case CallForPaperType.CallForBids:
                            View.SendUserAction(idCommunity, idModule, idRevision, ModuleCallForPaper.ActionType.ViewRevision);
                            break;
                        case CallForPaperType.RequestForMembership:
                            View.SendUserAction(idCommunity, idModule, idRevision, ModuleRequestForMembership.ActionType.ViewRevision);
                            break;
                    }

                }
                View.AllowSave = allowSave;
                View.AllowCompleteSubmission = allowCompleteSubmission;
                LoadRevision(call, submission, rev);
                if (allowView)
                {
                    if (View.PreloadView == CallStatusForSubmitters.Revisions)
                        View.SetActionUrl(CallStandardAction.ViewRevisions, RootObject.ViewRevisions(type, CallStandardAction.List, containerIdCommunity, CallStatusForSubmitters.Revisions));
                    else
                        View.SetActionUrl(CallStandardAction.List, RootObject.ViewCalls(idCall, type, CallStandardAction.List, containerIdCommunity, View.PreloadView));
                }

            }
        }

        private int SetCallCurrentCommunity(dtoBaseForPaper call)
        {
            dtoCallCommunityContext context = Service.GetCallCommunityContext(call, View.Portalname);
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
        private void LoadRevision(dtoBaseForPaper call, dtoSubmissionRevision sub, dtoRevisionRequest rev)
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
            View.LoadAttachments(Service.GetAvailableCallAttachments(call.Id, sub.Type.Id));

            View.DisplayPendingRequest(rev);
            View.FieldsToReview = rev.ItemsToReview.Where(i => i.Deleted == BaseStatusDeleted.None).Select(i => i.IdField).ToList();
            if (rev.Status== RevisionStatus.Request)
                LoadSections(call.Id, sub.Type.Id, sub.Id, sub.GetIdLastActiveRevision());
            else
                LoadSections(call.Id, sub.Type.Id, sub.Id, rev.Id);
        }
        //private void LoadSubmission(long idSubmission, long idRevision, int idUser)
        //{
        //    dtoSubmissionRevision subRevision = Service.GetSubmissionWithRevisions(idSubmission, true);
        //    if (subRevision != null)
        //        LoadSections(subRevision.IdCall, subRevision);
        //}

        //public void LoadSections(long idCall, CallForPaperType type, long idSubmission, long idRevision, long idSubmitter, Boolean allowAdmin)
        //{
        //    BaseForPaper call = ServiceCall.GetCall(idCall);
        //    if (call != null)
        //    {
        //        LoadSections(call, ServiceCall.GetSubmissionWithRevisions(idSubmission, true), allowAdmin);
        //    }
        //}
        //public void LoadSections(long idCall, dtoSubmissionRevision subRev, Boolean allowAdmin)
        //{
        //    BaseForPaper call = ServiceCall.GetCall(idCall);
        //    if (call != null)
        //        LoadSections(call, subRev, allowAdmin);
        //}
       
        //private void LoadSections(BaseForPaper call, dtoSubmissionRevision subRev, dtoRevisionSubmission rev)
        //{
        //    List<dtoCallSection<dtoSubmissionValueField>> sections = Service.GetSubmissionFields(call, subRev.Type.Id, subRev.Id, View.IdRevision);
        //    Dictionary<long, FieldError> fieldsError = Service.GetSubmissionFieldErrors(subRev.Id, View.IdRevision);
        //    if (sections != null && sections.Count > 0 && fieldsError != null)
        //        sections.ForEach(s => s.Fields.ForEach(f => f.SetError(fieldsError)));
        //    View.LoadSections(sections);
        //}
        private void LoadSections(long idCall, long idSubmitter, long idSubmission, long idRevision)
        {
            LoadSections(Service.GetCall(idCall), idSubmitter, idSubmission, idRevision, null);
        }
        private void LoadSections(BaseForPaper call, long idSubmitter, long idSubmission, long idRevision, Dictionary<long, FieldError> fieldsError)
        {
            List<dtoCallSection<dtoSubmissionValueField>> sections = Service.GetSubmissionFields(call, idSubmitter, idSubmission, idRevision);
            if (sections != null && sections.Count > 0 && View.TryToComplete && fieldsError != null)
                sections.ForEach(s => s.Fields.ForEach(f => f.SetError(fieldsError)));
            View.LoadSections(sections);
        }

        public void SaveRevision(List<dtoSubmissionValueField> fields)
        {
            dtoRevisionRequest rev = Service.GetRevisionRequest(View.IdRevision,false);
            if (rev != null && rev.AllowSubmission(View.InitSubmissionTime) && rev.AllowSave)
            {
                Boolean tryToComplete = View.TryToComplete;
                UserSubmission sub = SaveUserRevision(fields);
                if (sub != null)
                    LoadSections(sub.Call, sub.Type.Id, sub.Id, View.IdRevision, (tryToComplete) ? Service.GetSubmissionFieldErrors(sub, View.IdRevision) : null);
            }
            else if (rev != null)
                View.GoToUrl(RootObject.ViewSubmission(View.CallType, View.IdCall, View.IdSubmission, View.IdRevision, View.PreloadedUniqueID, false, false, View.PreloadView, View.PreloadIdOtherCommunity, 0));
            else
                View.GoToUrl(RootObject.ViewCalls(View.IdCall, View.CallType, CallStandardAction.List, View.IdCallCommunity, View.PreloadView));
        }
        private UserSubmission SaveUserRevision(List<dtoSubmissionValueField> fields)
        {
            long idCall = View.IdCall;
            long idSubmission = this.View.IdSubmission;
            long idRevision = View.IdRevision;
            Int32 idUser = UserContext.CurrentUserID;
            UserSubmission submission = null;
            try
            {
                submission = Service.SaveSubmissionRevision(idSubmission, idRevision, idUser, fields);
            }
            catch (SubmissionNotFound ex)
            {
                View.LoadError(RevisionErrorView.Unknown);
            }
            catch (SubmissionNotSaved ex)
            {
                View.LoadError(RevisionErrorView.StringValueSaving);
            }
            if (submission != null)
            {
                try
                {
                    Service.SaveSubmissionRevisionFiles(submission, idRevision, idUser, View.GetFileValues(submission, ModuleCallForPaper.UniqueCode, View.IdCallModule, (int)ModuleCallForPaper.ActionType.DownloadSubmittedFile, (int)ModuleCallForPaper.ObjectType.UserSubmission));
                }
                catch (SubmissionInternalFileNotLinked ex)
                {
                    View.LoadError(RevisionErrorView.FileSaving);
                }
                switch (View.CallType)
                {
                    case CallForPaperType.CallForBids:
                        View.SendUserAction(View.IdCallCommunity, View.IdCallModule, idRevision, ModuleCallForPaper.ActionType.SaveRevision);
                        break;
                    case CallForPaperType.RequestForMembership:
                        View.SendUserAction(View.IdCallCommunity, View.IdCallModule, idRevision, ModuleRequestForMembership.ActionType.SaveRevision);
                        break;
                }
            }
            else
                LoadSections(idCall,View.IdSubmitterType ,idSubmission, idRevision );
            return submission;
        }

        public void SaveCompleteRevision(List<dtoSubmissionValueField> fields, String baseFilePath, String translationFileName, String webSiteUrl, dtoRevisionMessage message)
        {
            Int32 containerIdCommunity = (View.PreloadIdOtherCommunity != -1) ? View.PreloadIdOtherCommunity : View.IdCallCommunity;
            long idSubmission = this.View.IdSubmission;
            long idCall = View.IdCall;
            long idRevision = View.IdRevision;
            UserSubmission submission = Service.GetUserSubmission(View.IdSubmission);
            dtoRevisionRequest rev = Service.GetRevisionRequest(View.IdRevision, false);

            if (submission != null && rev !=null && rev.AllowSubmission(View.InitSubmissionTime) && rev.AllowSave )
            {
                submission = SaveUserRevision(fields);
                if (submission != null && rev != null )
                {
                    idSubmission = submission.Id;
                    View.TryToComplete = true;
                    try
                    {
                        Int32 idUser = UserContext.CurrentUserID;
                        litePerson person = GetCurrentUser(ref idUser);
                        if (submission.Owner.Id == idUser)
                        {
                            Service.CompleteSubmissionRevision(submission, idRevision, idUser, View.InitSubmissionTime, baseFilePath, fields, translationFileName, webSiteUrl, message);
                            View.AllowCompleteSubmission = false;
                            View.AllowDeleteSubmission = false;
                            View.AllowSave = false;
                            View.SendUserAction(View.IdCallCommunity, View.IdCallModule, idRevision, ModuleCallForPaper.ActionType.ConfirmSubmission);
                            View.GoToUrl(RootObject.FinalMessage(View.CallType, idCall, idSubmission, idRevision, submission.UserCode, false ,false, View.PreloadView,containerIdCommunity));
                        }
                        else if (UserContext.isAnonymous)
                            View.DisplayCallUnavailableForPublic();
                        else
                            View.DisplaySessionTimeout();
                    }
                    catch (SubmissionStatusChange ex)
                    {
                        //View.AllowDeleteSubmission = (submission != null && submission.Deleted == BaseStatusDeleted.None && submission.Status == SubmissionStatus.draft);
                        View.LoadError(RevisionErrorView.StringValueSaving);
                    }
                    catch (SubmissionItemsEmpty ex)
                    {
                        Dictionary<long, FieldError> errors = Service.GetSubmissionFieldErrors(submission, idRevision);
                        View.AllowDeleteSubmission = (submission != null && submission.Deleted == BaseStatusDeleted.None && submission.Status == SubmissionStatus.draft);

                        View.LoadError( (errors.ToList().Where(er=>er.Value != FieldError.Mandatory).Any()) ? RevisionErrorView.InvalidFields : RevisionErrorView.RequiredItems);
                        LoadSections(submission.Call, submission.Type.Id, idSubmission, View.IdRevision, errors);
                    }
                    catch (SubmissionTimeExpired ex)
                    {
                        View.LoadError(RevisionErrorView.TimeExpired);
                    }
                    //View.InitSubmissionTime = DateTime.Now;
                }
            }
            else if (rev != null)
                View.GoToUrl(RootObject.ViewSubmission(View.CallType, View.IdCall, View.IdSubmission, View.IdRevision, View.PreloadedUniqueID, false, false, View.PreloadView, containerIdCommunity, 0));
            else
                View.GoToUrl(RootObject.ViewCalls(View.IdCall, View.CallType, CallStandardAction.List, containerIdCommunity , View.PreloadView));
        }

        public void RemoveFieldFile(long idSubmittedField, List<dtoSubmissionValueField> fields)
        {
            Int32 containerIdCommunity = (View.PreloadIdOtherCommunity != -1) ? View.PreloadIdOtherCommunity : View.IdCallCommunity;
            dtoRevisionRequest rev = Service.GetRevisionRequest(View.IdRevision, false);
            if (rev == null)
            {
                if (View.PreloadView == CallStatusForSubmitters.Revisions)
                    View.SetActionUrl(CallStandardAction.ViewRevisions, RootObject.ViewRevisions(View.CallType, CallStandardAction.List, View.IdCallCommunity, CallStatusForSubmitters.Revisions));
                else
                    View.SetActionUrl(CallStandardAction.List, RootObject.ViewCalls(View.IdCall, View.CallType, CallStandardAction.List, containerIdCommunity, View.PreloadView));
            }
            else if (rev.AllowSubmission(View.InitSubmissionTime) && rev.AllowSave)
            {
                try
                {
                    Service.RemoveFieldFileValue(idSubmittedField);
                    switch (View.CallType)
                    {
                        case CallForPaperType.CallForBids:
                            View.SendUserAction(View.IdCallCommunity, View.IdCallModule, View.IdRevision, ModuleCallForPaper.ActionType.VirtualDeleteSubmittedFile);
                            break;
                        case CallForPaperType.RequestForMembership:
                            View.SendUserAction(View.IdCallCommunity, View.IdCallModule, View.IdRevision, ModuleRequestForMembership.ActionType.VirtualDeleteSubmittedFile);
                            break;
                    }
                }
                catch (Exception ex)
                {

                }
                SaveRevision(fields);
            }
            else
                View.GoToUrl(RootObject.ViewSubmission(View.CallType, View.IdCall, View.IdSubmission, View.IdRevision, View.PreloadedUniqueID, false, false, View.PreloadView, containerIdCommunity, 0));
        }
        //public void VirtualDeleteSubmission()
        //{
        //    UserSubmission submission = Service.GetUserSubmission(View.IdSubmission);
        //    if (submission == null)
        //        View.GoToUrl(CallStandardAction.List, RootObject.ViewCalls(View.IdCall, CallForPaperType.CallForBids, CallStandardAction.List, View.IdCallCommunity, CallStatusForSubmitters.SubmissionClosed));
        //    else if (submission.AllowEditSubmission(View.InitSubmissionTime))
        //    {
        //        Service.VirtualDeleteSubmission(View.IdSubmission, true);
        //        View.SendUserAction(View.IdCallCommunity, View.IdCallModule, submission.Id, ModuleCallForPaper.ActionType.VirtualDeleteSubmission);
        //        View.GoToUrl(RootObject.ViewCalls(View.IdCall, CallForPaperType.CallForBids, CallStandardAction.List, View.IdCallCommunity, CallStatusForSubmitters.SubmissionOpened));
        //    }
        //    else
        //        View.GoToUrl(RootObject.ViewSubmission(submission.Call.Type, submission.Call.Id, submission.Id, CallStatusForSubmitters.Submitted));
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