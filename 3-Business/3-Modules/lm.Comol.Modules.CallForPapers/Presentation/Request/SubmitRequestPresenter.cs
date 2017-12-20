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
    public class SubmitRequestPresenter : lm.Comol.Core.DomainModel.Common.DomainPresenter
    {
         #region "Initialize"
            private ServiceRequestForMembership _Service;

            public virtual BaseModuleManager CurrentManager { get; set; }
            protected virtual IViewSubmitRequest View
            {
                get { return (IViewSubmitRequest)base.View; }
            }
            private ServiceRequestForMembership Service
            {
                get
                {
                    if (_Service == null)
                        _Service = new ServiceRequestForMembership(AppContext);
                    return _Service;
                }
            }
            public SubmitRequestPresenter(iApplicationContext oContext)
                : base(oContext)
            {
                this.CurrentManager = new BaseModuleManager(oContext);
            }
            public SubmitRequestPresenter(iApplicationContext oContext, IViewSubmitRequest view)
                : base(oContext, view)
            {
                this.CurrentManager = new BaseModuleManager(oContext);
            }
        #endregion

        public void InitView(Boolean forPublicCall)
        {
            long idCall = View.PreloadIdCall;
            Boolean skinInitialized = false;
            dtoRequest call = null;
            CallForPaperType type = Service.GetCallType(idCall);
            call = Service.GetDtoRequest(idCall);
            if (type == CallForPaperType.None || call == null)
                type = CallForPaperType.RequestForMembership;
            View.CallType = type;
            int idCommunity = SetCallCurrentCommunity(call);
            int idModule = Service.ServiceModuleID();

            Boolean hasPermissionToSubmit = Service.IsCallAvailableByUser(idCall, UserContext.CurrentUserID);
            if (call == null)
                View.LoadUnknowCall(idCommunity, idModule, idCall, type);
            else if (UserContext.isAnonymous && !forPublicCall)
            {
                if (call.IsPublic)
                    View.GoToUrl(RootObject.SubmitToCallBySubmitterType(type, idCall, call.IsPublic, View.PreloadedIdSubmission, View.PreloadedIdSubmitter, View.FromPublicList, View.PreloadView, View.PreloadIdOtherCommunity));
                else
                    View.DisplaySessionTimeout();
            }
            else if (UserContext.isAnonymous && forPublicCall && !call.IsPublic)
                View.DisplayCallUnavailableForPublic();
            else if (!hasPermissionToSubmit && call.IsPublic && forPublicCall == false)
                View.GoToUrl(RootObject.SubmitToCallBySubmitterType(type, idCall, call.IsPublic, View.IdSubmission, View.PreloadedIdSubmitter, View.FromPublicList, View.PreloadView, View.PreloadIdOtherCommunity));
            else
            {
                int idUser = UserContext.CurrentUserID;
                litePerson currenUser = GetCurrentUser(ref idUser);

                View.isAnonymousSubmission = forPublicCall;
                View.TryToComplete = false;
                ModuleRequestForMembership module = Service.RequestForMembershipServicePermission(idUser, idCommunity);

                View.IdCallModule = idModule;
                if (call == null)
                {
                    View.LoadUnknowCall(idCommunity, idModule, idCall, type);
                    if (module.ViewBaseForPapers)
                    {
                        if (View.FromPublicList)
                            View.SetActionUrl(CallStandardAction.List, RootObject.PublicCollectorCalls(CallForPaperType.RequestForMembership, idCall, View.PreloadIdCommunity));
                        else if (!UserContext.isAnonymous)
                            View.SetActionUrl(CallStandardAction.List, RootObject.ViewCalls(idCall, CallForPaperType.RequestForMembership, CallStandardAction.List, (module.ViewBaseForPapers) ? idCommunity : View.PreloadIdOtherCommunity, View.PreloadView));
                    }
                }
                else if (call.Status <= CallForPaperStatus.Published || call.Status == CallForPaperStatus.SubmissionClosed || call.Status == CallForPaperStatus.SubmissionsLimitReached)
                {
                    View.DisplayCallUnavailable(call.Status);
                    if (module.ViewBaseForPapers)
                    {
                        if (View.FromPublicList)
                            View.SetActionUrl(CallStandardAction.List, RootObject.PublicCollectorCalls(CallForPaperType.RequestForMembership, idCall, View.PreloadIdCommunity));
                        else if (!UserContext.isAnonymous)
                            View.SetActionUrl(CallStandardAction.List, RootObject.ViewCalls(idCall, CallForPaperType.RequestForMembership, CallStandardAction.List, (module.ViewBaseForPapers) ? idCommunity : View.PreloadIdOtherCommunity, View.PreloadView));
                    }
                    if (forPublicCall)
                    {
                        View.InitializeView(Service.GetExternalContext(idCall), Service.CallWithFileToUpload(idCall, 0));
                        skinInitialized = true;
                    }
                }
                else if (module.ViewBaseForPapers || call.IsPublic || hasPermissionToSubmit)
                {
                    long idSubmission = View.PreloadedIdSubmission;
                    View.IdCall = idCall;
                    View.CallRepository = call.GetRepositoryIdentifier();
                    if (module.ViewBaseForPapers || hasPermissionToSubmit)
                    {
                        if (View.FromPublicList)
                            View.SetActionUrl(CallStandardAction.List, RootObject.PublicCollectorCalls(CallForPaperType.RequestForMembership, idCall, View.PreloadIdCommunity));
                        else if (!UserContext.isAnonymous)
                            View.SetActionUrl(CallStandardAction.List, RootObject.ViewCalls(idCall, CallForPaperType.RequestForMembership, CallStandardAction.List, (module.ViewBaseForPapers) ? idCommunity : View.PreloadIdOtherCommunity, View.PreloadView));
                    }

                    // Find active submission
                    dtoSubmissionRevision submission = null;
                    if (forPublicCall && UserContext.isAnonymous)
                    {
                        submission = Service.GetActiveUserSubmission(idCall, UserContext.CurrentUserID, idSubmission, View.PreloadedUniqueID);
                        if (submission != null && submission.Owner.Id != idUser)
                            submission = null;
                    }
                    else
                        submission = Service.GetActiveUserSubmission(idCall, UserContext.CurrentUserID, idSubmission);


                    if (forPublicCall)
                    {
                        View.InitializeView(Service.GetExternalContext(idCall), Service.CallWithFileToUpload(idCall, 0));
                        skinInitialized = true;
                    }
                    else
                    {
                        SubmitterType submitter = null;
                        if (submission != null)
                            submitter = Service.GetSubmitterType(submission.Type.Id);
                        else
                            submitter = Service.GetSubmitterType(View.PreloadedIdSubmitter);

                        if (idSubmission == 0 && submitter != null && submitter.AllowMultipleSubmissions && Service.GetSubmissionCountBySubmitter(idCall, UserContext.CurrentUserID, submitter.Id) < submitter.MaxMultipleSubmissions)
                            submission = null;
                        View.InitializeView(Service.CallWithFileToUpload(idCall, (submission == null) ? 0 : submission.Type.Id));
                    }
                   
                    DateTime InitTime = DateTime.Now;
                    if (call.StartDate > InitTime)
                        View.DisplaySubmissionTimeBefore(call.StartDate);
                    else if ((submission == null && !call.AllowSubmission(InitTime)) || (submission != null && submission.ExtensionDate.HasValue && call.AllowLateSubmission(InitTime, submission.ExtensionDate.Value)))
                        View.DisplaySubmissionTimeAfter(call.EndDate);
                    else if (submission != null && (submission.Deleted != BaseStatusDeleted.None || (submission.Status != SubmissionStatus.none && submission.Status != SubmissionStatus.draft)))
                    {
                        if (forPublicCall)
                            View.GoToUrl(RootObject.ViewSubmission(call.Type, call.Id, submission.Id, submission.UniqueId, true, View.FromPublicList, CallStatusForSubmitters.Submitted, View.PreloadIdOtherCommunity, 0));
                        else if (!UserContext.isAnonymous)
                            View.GoToUrl(RootObject.ViewSubmission(call.Type, call.Id, submission.Id, View.FromPublicList, CallStatusForSubmitters.Submitted, View.PreloadIdOtherCommunity, 0));
                    }
                    else if (submission != null && submission.Revisions.Where(r => r.Type != RevisionType.Original).Any())
                        if (!forPublicCall || (forPublicCall && !UserContext.isAnonymous))
                            View.GoToUrl(RootObject.UserReviewCall(call.Type, call.Id, submission.Id, submission.GetIdWorkingRevision(), CallStatusForSubmitters.Submitted, View.PreloadIdOtherCommunity));
                        else
                            View.DisplayCallUnavailableForPublic();
                    else
                    {
                        idSubmission = (submission == null) ? 0 : submission.Id;
                        long idRevision = (submission == null) ? 0 : submission.GetIdOriginal();
                        View.IdRevision = idRevision;
                        View.IdSubmission = idSubmission;

                        View.AllowDeleteSubmission = false;
                        Boolean allowSave = false;
                        Boolean allowCompleteSubmission = false;
                        LoadCall(call, submission);

                        View.AllowSubmitterChange = false;
                        if (module.AddSubmission || call.IsPublic || hasPermissionToSubmit)
                        {
                            View.AllowDeleteSubmission = (submission != null && submission.Deleted == BaseStatusDeleted.None && submission.Status == SubmissionStatus.draft);
                            allowCompleteSubmission = (submission != null && submission.Deleted == BaseStatusDeleted.None && submission.Status == SubmissionStatus.draft);
                            allowSave = (submission != null && submission.Deleted == BaseStatusDeleted.None && submission.Status == SubmissionStatus.draft);
                            View.AllowSave = allowSave;
                            View.AllowCompleteSubmission = allowCompleteSubmission;
                            if (submission == null)
                            {
                                View.IdSubmission = 0;
                                View.AllowSubmitterChange = true;
                                View.SendStartSubmission(idCommunity, idModule, idCall, ModuleRequestForMembership.ActionType.StartSubmission);
                            }
                            else
                            {
                                View.InitSubmissionTime = InitTime;
                                //if (submission.Status >= SubmissionStatus.submitted && viewList == CallStatusForSubmitters.SubmissionOpened)
                                //    viewList = CallStatusForSubmitters.Submitted;
                                View.AllowSubmitterSelection = false;
                                View.IdSubmission = submission.Id;
                                LoadSections(idCall, submission.Type.Id, submission.Id,idRevision);
                                View.SendUserAction(idCommunity, idModule, submission.Id, ModuleRequestForMembership.ActionType.ViewSubmission);
                            }
                        }
                        else
                        {
                            View.AllowSave = allowSave;
                            View.AllowCompleteSubmission = allowCompleteSubmission;
                            if (idSubmission == 0)
                                View.DisableSubmitterTypesSelection();
                        }
                    }
                }
                else
                    View.DisplayNoPermission(idCommunity, idModule);
            }
            if (forPublicCall && !skinInitialized)
                View.InitializeEmptyView();
        }

        //public Boolean isPublicSubmission() {
        //    Boolean isPublic = false;
        //    long idCall = View.PreloadIdCall;

        //    dtoCall call = Service.GetDtoCall(idCall);

        //    if (UserContext.isAnonymous)
        //        isPublic = (call==null) ? false : call.IsPublic;
        //    else if (call !=null)
        //    {
        //        int idCommunity = (call.Community == null) ? 0 : call.Community.Id;
        //        Person currenUser = CurrentManager.GetLitePerson(UserContext.CurrentUserID);
        //        ModuleCallForPaper module = Service.CallForPaperServicePermission(UserContext.CurrentUserID, idCommunity);
        //        if (!module.ViewCallForPapers)
        //            isPublic = call.IsPublic;
        //    }
        //    return isPublic;
        //}
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
            //else if (currentCommunity != null && (call==null || !call.IsPortal ))
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

        private void LoadCall(dtoRequest dto, dtoBaseSubmission submission)
        {
            BaseForPaper call = Service.GetCall(dto.Id);
            List<dtoSubmitterType> submitters = null;
            if (call != null)
            {
                submitters = Service.GetCallAvailableSubmittersType(call);
                View.HasMultipleSubmitters = (submitters.Count > 1);
            }
            View.LoadCallInfo(dto);
            if (submission == null)
            {
                if (call != null)
                {
                    long idSubmitter = 0;
                    if (submitters.Count == 1)
                    {
                        idSubmitter = submitters[0].Id;
                        View.IdSelectedSubmitterType = idSubmitter;
                    }
                    View.LoadSubmitterTypes(submitters);
                    View.AllowSubmitterSelection = (submitters.Count>1);
                    if (idSubmitter != 0 && submitters.Where(s => s.Id == idSubmitter).Any())
                    {
                        if (submitters.Count>1)
                            View.SetSubmitterName(submitters.Where(s => s.Id == idSubmitter).FirstOrDefault().Name);
                        else
                            View.SetGenericSubmitterName();
                    }
                }
            }
            else
            {
                View.IdSelectedSubmitterType = submission.Type.Id;
                if (submitters != null && submitters.Count > 1)
                    View.SetSubmitterName(submission.Type.Name);
                else
                    View.SetGenericSubmitterName();
            }
            View.LoadAttachments(Service.GetAvailableCallAttachments(dto.Id, (submission == null) ? 0 : submission.Type.Id));
             
        }

        public void LoadSections(long idCall, long idSubmitter, long idSubmission, long idRevision)
        {
            BaseForPaper call = Service.GetCall(idCall);
            if (call != null)
            {
                SubmitterType submitter = Service.GetSubmitterType(idSubmitter);
                if (View.HasMultipleSubmitters)
                {
                    if (submitter != null)
                        View.SetSubmitterName(submitter.Name);
                    else
                    {
                        View.SetSubmitterName("");
                        idSubmitter = 0;
                    }
                }
                else if (submitter == null)
                    idSubmitter = 0;
                LoadSections(call, idSubmitter, idSubmission, idRevision, null);
            }
        }

        private void LoadSections(BaseForPaper call, long idSubmitter, long idSubmission, long idRevision, Dictionary<long, FieldError> fieldsError)
        {

            View.LoadAttachments(Service.GetAvailableCallAttachments(call.Id, idSubmitter));
            List<dtoCallSection<dtoSubmissionValueField>> sections = Service.GetSubmissionFields(call, idSubmitter, idSubmission, idRevision);
            if (sections != null && sections.Count > 0 && View.TryToComplete && fieldsError != null)
                sections.ForEach(s => s.Fields.ForEach(f=>f.SetError(fieldsError)));
            View.LoadSections(sections.Where(s => s.Fields.Count > 0).OrderBy(s => s.DisplayOrder).ThenBy(s => s.Name).ToList());
        }
        public void SelectSubmitterType(long idCall,long idSubmitter)
        {
            View.InitSubmissionTime = DateTime.Now;
            View.AllowSave = true;
            View.AllowCompleteSubmission = true;
            View.AllowSubmitterSelection = false;
            
            
            View.InitializeView(Service.CallWithFileToUpload(idCall, idSubmitter));
            if (idSubmitter == View.IdSelectedSubmitterType && View.IdSubmission > 0)
                LoadSections(idCall, idSubmitter, View.IdSubmission, View.IdRevision);
            else {
                View.IdSubmission = 0;
                View.IdRevision  = 0;
                View.IdSelectedSubmitterType = idSubmitter;
                View.AllowSubmitterChange = true;
                LoadSections(idCall, idSubmitter, 0,0);
            }
        }

        public void SaveSubmission(List<dtoSubmissionValueField> fields, DateTime clickDt)
        {
            UserSubmission submission = Service.GetUserSubmission(View.IdSubmission);
            if (submission == null || submission.AllowEditSubmission(View.InitSubmissionTime, clickDt))
            {
                Boolean tryToComplete = View.TryToComplete;
                submission = SaveUserSubmission(fields);
                if (submission != null)
                    LoadSections(submission.Call, submission.Type.Id, submission.Id, View.IdRevision, (tryToComplete) ? Service.GetSubmissionFieldErrors(submission, View.IdRevision) : null);
            }
            else if (submission != null)
                View.GoToUrl(CallStandardAction.None, RootObject.ViewSubmission(submission.Call.Type, submission.Call.Id, submission.Id, View.FromPublicList, CallStatusForSubmitters.Submitted, View.PreloadIdOtherCommunity, 0));
            else
                View.GoToUrl(CallStandardAction.List, RootObject.ViewCalls(View.IdCall, CallForPaperType.RequestForMembership, CallStandardAction.List, View.IdCallCommunity, CallStatusForSubmitters.SubmissionClosed));
        }
        private UserSubmission SaveUserSubmission(List<dtoSubmissionValueField> fields)
        {
            long idCall = View.IdCall;
            long idSubmission = this.View.IdSubmission;
            long idRevision = View.IdRevision;
            UserSubmission submission = null;
            try
            {
                submission = Service.SaveSubmission(idSubmission, ref idRevision, idCall, View.IdSelectedSubmitterType, UserContext.CurrentUserID, fields);
            }
            catch (SubmissionNotFound ex)
            {
                View.LoadError(SubmissionErrorView.SubmissionDeleted, View.IdCallCommunity, idCall, CallStatusForSubmitters.SubmissionOpened);
            }
            catch (SubmissionNotSaved ex)
            {
                View.LoadError(SubmissionErrorView.SubmissionValueSaving, View.IdCallCommunity, idCall, CallStatusForSubmitters.SubmissionOpened);
            }
            catch (SubmissionNotCreated ex)
            {
                View.LoadError(SubmissionErrorView.SubmissionValueSaving, View.IdCallCommunity, idCall, CallStatusForSubmitters.SubmissionOpened);
            }
            catch (SubmissionCallUnavailable ex)
            {
                View.LoadError(SubmissionErrorView.UnavailableCall, View.IdCallCommunity, idCall, CallStatusForSubmitters.SubmissionOpened);
                View.AllowCompleteSubmission = false;
                View.AllowSave = false;
            }
            if (submission != null)
            {
                View.IdRevision = idRevision;
                View.IdSubmission = submission.Id;
                try
                {
                    Revision revision = Service.GetRevision(idRevision);
                    Service.SaveSubmissionFiles(submission, revision, UserContext.CurrentUserID, new List<dtoRequestedFileUpload>(), View.GetFileValues(submission, ModuleRequestForMembership.UniqueCode, View.IdCallModule, (int)ModuleRequestForMembership.ActionType.DownloadSubmittedFile, (int)ModuleRequestForMembership.ObjectType.UserSubmission));
                }
                catch (SubmissionInternalFileNotLinked ex)
                {
                    // remove internal files !
                }
                if (idSubmission == 0)
                    View.SendUserAction(View.IdCallCommunity, View.IdCallModule, submission.Id, ModuleRequestForMembership.ActionType.AddSubmission);
                else
                    View.SendUserAction(View.IdCallCommunity, View.IdCallModule, submission.Id, ModuleRequestForMembership.ActionType.EditSubmission);
            }
            else
                LoadSections(idCall, View.IdSelectedSubmitterType, idSubmission, idRevision);
            return submission;
        }

        public void SaveCompleteSubmission(List<dtoSubmissionValueField> fields, lm.Comol.Core.MailCommons.Domain.Configurations.SmtpServiceConfig smtpConfig, String websiteUrl, Dictionary<SubmissionTranslations, string> translations, String BaseFilePath, String translationFileName, DateTime clickDt)
        {
            long idSubmission = this.View.IdSubmission;
            long idCall = View.IdCall;
            UserSubmission submission = Service.GetUserSubmission(View.IdSubmission);
            if (submission == null || submission.AllowEditSubmission(View.InitSubmissionTime, clickDt))
            {
                submission = SaveUserSubmission(fields);
                if (submission != null || submission.AllowEditSubmission(View.InitSubmissionTime, clickDt))
                {
                    idSubmission = submission.Id;
                    //View.IdSubmission = idSubmission;
                    View.TryToComplete = true;
                    Revision revision = Service.GetRevision(View.IdRevision);
                    try
                    {
                        Int32 idUser = UserContext.CurrentUserID;
                        litePerson person = GetCurrentUser(ref idUser);
                        if (submission.Owner.Id == idUser)
                        {
                            if (fields.Any(f => f.Field.Type == FieldType.FileInput && (f.Value == null || f.Value.IdLink == 0)))
                            {
                                List<SubmissionFieldFileValue> fValues = (from f in CurrentManager.GetIQ<SubmissionFieldFileValue>()
                                                                          where f.Submission == submission && f.Deleted == BaseStatusDeleted.None && f.Revision.Id == revision.Id
                                                                          select f).ToList();
                                if (fValues.Any())
                                {
                                    foreach (dtoSubmissionValueField field in fields.Where(f => f.Field.Type == FieldType.FileInput && (f.Value == null || f.Value.IdLink == 0)))
                                    {
                                        field.Value.IdLink = fValues.Where(f => f.Field.Id == field.IdField).Select(f => f.Link.Id).FirstOrDefault();
                                    }
                                }
                            }

                            Service.UserCompleteSubmission(submission, revision, View.InitSubmissionTime, idUser, BaseFilePath, fields, smtpConfig, websiteUrl, translations, translationFileName, clickDt);
                            View.AllowCompleteSubmission = false;
                            View.AllowDeleteSubmission = false;
                            View.AllowSave = false;
                            View.SendUserAction(View.IdCallCommunity, View.IdCallModule, submission.Id, ModuleRequestForMembership.ActionType.ConfirmSubmission);
                            View.GoToUrl(RootObject.FinalMessage(CallForPaperType.RequestForMembership, idCall, idSubmission, View.IdRevision, submission.UserCode, View.isAnonymousSubmission, View.FromPublicList, CallStatusForSubmitters.Submitted, View.PreloadIdOtherCommunity));
                        }
                        else if (!View.isAnonymousSubmission && UserContext.isAnonymous)
                            View.DisplayCallUnavailableForPublic();
                        else
                            View.DisplaySessionTimeout();
                    }
                    catch (SubmissionStatusChange ex)
                    {
                        View.AllowDeleteSubmission = (submission != null && submission.Deleted == BaseStatusDeleted.None && submission.Status == SubmissionStatus.draft);
                        View.LoadError(SubmissionErrorView.SubmissionValueSaving, View.IdCallCommunity, idCall, CallStatusForSubmitters.SubmissionOpened);
                    }
                    catch (SubmissionItemsEmpty ex)
                    {
                        Dictionary<long, FieldError> errors = Service.GetSubmissionFieldErrors(submission, revision);
                        View.AllowDeleteSubmission = (submission != null && submission.Deleted == BaseStatusDeleted.None && submission.Status == SubmissionStatus.draft);
                        View.LoadError((errors.ToList().Where(er => er.Value != FieldError.Mandatory).Any()) ? SubmissionErrorView.InvalidFields : SubmissionErrorView.RequiredItems, View.IdCallCommunity, idCall, CallStatusForSubmitters.SubmissionOpened);
                        LoadSections(submission.Call, submission.Type.Id, idSubmission, View.IdRevision, errors);

                    }
                    catch (SubmissionTimeExpired ex)
                    {
                        View.LoadError(SubmissionErrorView.SubmissionValueSaving, View.IdCallCommunity, idCall, CallStatusForSubmitters.SubmissionClosed);
                    }
                    //View.InitSubmissionTime = DateTime.Now;
                }
            }
            else if (submission != null)
                View.GoToUrl(CallStandardAction.None, RootObject.ViewSubmission(CallForPaperType.RequestForMembership, idCall, idSubmission, View.FromPublicList, CallStatusForSubmitters.Submitted, View.PreloadIdOtherCommunity, 0));
            else
                View.GoToUrl(CallStandardAction.List, RootObject.ViewCalls(idCall, CallForPaperType.RequestForMembership, CallStandardAction.List, View.IdCallCommunity, CallStatusForSubmitters.SubmissionClosed));
        }

        public void RemoveFieldFile(long idSubmittedField, List<dtoSubmissionValueField> fields, DateTime clickDt)
        {
            UserSubmission submission = Service.GetUserSubmission(View.IdSubmission);
            if (submission == null)
                View.GoToUrl(CallStandardAction.List, RootObject.ViewCalls(View.IdCall, CallForPaperType.RequestForMembership, CallStandardAction.List, View.IdCallCommunity, CallStatusForSubmitters.SubmissionClosed));
            else if (submission.AllowEditSubmission(View.InitSubmissionTime, clickDt))
            {
                try
                {
                    Service.RemoveFieldFileValue(idSubmittedField);
                    View.SendUserAction(View.IdCallCommunity, View.IdCallModule, submission.Id, ModuleRequestForMembership.ActionType.VirtualDeleteSubmittedFile);
                }
                catch (Exception ex)
                {
                }
                this.SaveSubmission(fields, clickDt);
            }
            else
                View.GoToUrl(RootObject.ViewSubmission(submission.Call.Type, submission.Call.Id, submission.Id, View.FromPublicList, CallStatusForSubmitters.Submitted, View.PreloadIdOtherCommunity, 0));
        }
        public void VirtualDeleteSubmission(DateTime clickDt)
        {
            UserSubmission submission = Service.GetUserSubmission(View.IdSubmission);
            if (submission == null)
                View.GoToUrl(CallStandardAction.List, RootObject.ViewCalls(View.IdCall, CallForPaperType.RequestForMembership, CallStandardAction.List, View.IdCallCommunity, CallStatusForSubmitters.SubmissionClosed));
            else if (submission.AllowEditSubmission(View.InitSubmissionTime, clickDt))
            {
                Service.VirtualDeleteSubmission(View.IdSubmission, true);
                View.SendUserAction(View.IdCallCommunity, View.IdCallModule, submission.Id, ModuleRequestForMembership.ActionType.VirtualDeleteSubmission);
                View.GoToUrl(RootObject.ViewCalls(View.IdCall, CallForPaperType.RequestForMembership, CallStandardAction.List, View.IdCallCommunity, CallStatusForSubmitters.SubmissionOpened));
            }
            else
                View.GoToUrl(RootObject.ViewSubmission(submission.Call.Type, submission.Call.Id, submission.Id, View.FromPublicList, CallStatusForSubmitters.Submitted, View.PreloadIdOtherCommunity, 0));
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