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
    public class SubmitCallPresenter : lm.Comol.Core.DomainModel.Common.DomainPresenter
    {
         #region "Initialize"
            private ServiceCallOfPapers _Service;

            public virtual BaseModuleManager CurrentManager { get; set; }
            protected virtual IViewSubmitCall View
            {
                get { return (IViewSubmitCall)base.View; }
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
            public SubmitCallPresenter(iApplicationContext oContext)
                : base(oContext)
            {
                this.CurrentManager = new BaseModuleManager(oContext);
            }
            public SubmitCallPresenter(iApplicationContext oContext, IViewSubmitCall view)
                : base(oContext, view)
            {
                this.CurrentManager = new BaseModuleManager(oContext);
            }
        #endregion

        public void InitView(Boolean forPublicCall)
        {
            long idCall = View.PreloadIdCall;
            Boolean skinInitialized = false;
            dtoCall call = null;
            CallForPaperType type = Service.GetCallType(idCall);
            call = Service.GetDtoCall(idCall);
            if (type == CallForPaperType.None || call == null)
                type = CallForPaperType.CallForBids;

            View.CallType = type;
            int idCommunity = SetCallCurrentCommunity(call);
            int idModule = Service.ServiceModuleID();

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
            {
                View.DisplayCallUnavailableForPublic();
            }
            else
            {
                int idUser = UserContext.CurrentUserID;
                litePerson currenUser = GetCurrentUser(ref idUser);
                
                View.isAnonymousSubmission = forPublicCall;
                View.TryToComplete = false;
                ModuleCallForPaper module = Service.CallForPaperServicePermission(idUser, idCommunity);
                View.IdCallModule = idModule;

                Boolean hasPermissionToSubmit = Service.IsCallAvailableByUser(idCall, UserContext.CurrentUserID);

                if (call == null)
                {
                    View.LoadUnknowCall(idCommunity, idModule, idCall, type);
                    if (module.ViewCallForPapers || hasPermissionToSubmit)
                    { 
                        if (View.FromPublicList)
                            View.SetActionUrl(CallStandardAction.List, RootObject.PublicCollectorCalls(CallForPaperType.CallForBids, idCall, View.PreloadIdCommunity));
                        else if (!UserContext.isAnonymous)
                            View.SetActionUrl(CallStandardAction.List, RootObject.ViewCalls(idCall, CallForPaperType.CallForBids, CallStandardAction.List, (module.ViewCallForPapers) ? idCommunity : View.PreloadIdOtherCommunity, View.PreloadView));
                    }
                }
                else if (call.Status <= CallForPaperStatus.Published || call.Status == CallForPaperStatus.SubmissionClosed || call.Status == CallForPaperStatus.SubmissionsLimitReached)
                {
                    View.DisplayCallUnavailable(call.Status);
                    if (module.ViewCallForPapers || hasPermissionToSubmit)
                    {
                        if (View.FromPublicList)
                            View.SetActionUrl(CallStandardAction.List, RootObject.PublicCollectorCalls(CallForPaperType.CallForBids, idCall, View.PreloadIdCommunity));
                        else if (!UserContext.isAnonymous)
                            View.SetActionUrl(CallStandardAction.List, RootObject.ViewCalls(idCall, CallForPaperType.CallForBids, CallStandardAction.List, (module.ViewCallForPapers) ? idCommunity : View.PreloadIdOtherCommunity, View.PreloadView));
                    }
                    if (forPublicCall)
                    {
                        View.InitializeView(Service.GetExternalContext(idCall), Service.CallWithFileToUpload(idCall, 0));
                        skinInitialized = true;
                    }
                }
                else if (!hasPermissionToSubmit && call.IsPublic && forPublicCall==false)
                    View.GoToUrl(RootObject.SubmitToCallBySubmitterType(type, idCall, call.IsPublic, View.IdSubmission, View.PreloadedIdSubmitter, View.FromPublicList, View.PreloadView, View.PreloadIdOtherCommunity));
                else if (module.ViewCallForPapers || call.IsPublic || hasPermissionToSubmit)
                {
                    long idSubmission = View.PreloadedIdSubmission;
                    View.IdCall = idCall;
                    View.CallRepository = call.GetRepositoryIdentifier();
                    if (module.ViewCallForPapers || hasPermissionToSubmit)
                    {
                        if (View.FromPublicList)
                            View.SetActionUrl(CallStandardAction.List, RootObject.PublicCollectorCalls(CallForPaperType.CallForBids, idCall, View.PreloadIdCommunity));
                        else if (!UserContext.isAnonymous)
                            View.SetActionUrl(CallStandardAction.List, RootObject.ViewCalls(idCall, CallForPaperType.CallForBids, CallStandardAction.List, (module.ViewCallForPapers) ? idCommunity : View.PreloadIdOtherCommunity, View.PreloadView));
                    }

                    // Find active submission
                    dtoSubmissionRevision submission = null;
                    long defaultSubmitter = 0;
                    SubmitterType submitter = null;
                    //if (View.PreloadedIdSubmitter>0)
                    //    submitter = Service.GetSubmitterType(View.PreloadedIdSubmitter);

                    if (forPublicCall && UserContext.isAnonymous)
                    {
                        submission = Service.GetActiveUserSubmission(idCall, UserContext.CurrentUserID, idSubmission, View.PreloadedUniqueID);
                        if (submission != null && submission.Owner.Id != idUser)
                            submission = null;
                    }
                    else
                    {
                        submission = Service.GetActiveUserSubmission(idCall, UserContext.CurrentUserID, idSubmission);
                    }

                    if (forPublicCall)
                    {
                        View.InitializeView(Service.GetExternalContext(idCall), Service.CallWithFileToUpload(idCall, 0));
                        skinInitialized = true;
                    }
                    else
                    {

                        if (submission != null)
                            submitter = Service.GetSubmitterType(submission.Type.Id);
                        else
                            submitter = Service.GetSubmitterType(View.PreloadedIdSubmitter);

                        if (idSubmission == 0 && submitter != null && submitter.AllowMultipleSubmissions && Service.GetSubmissionCountBySubmitter(idCall, UserContext.CurrentUserID, submitter.Id) < submitter.MaxMultipleSubmissions)
                        {
                            submission = null;
                            defaultSubmitter = submitter.Id;
                        }
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
                        else
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
                        View.IdSubmission = idSubmission;
                        long idRevision = (submission == null) ? 0 : submission.GetIdOriginal();
                        View.AllowDeleteSubmission = false;
                        View.IdRevision = idRevision;
                        Boolean allowSave = false;
                        Boolean allowCompleteSubmission = false;
                        LoadCall(call, submission, defaultSubmitter);

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
                                View.SendStartSubmission(idCommunity, idModule, idCall, ModuleCallForPaper.ActionType.StartSubmission);
                                if (defaultSubmitter > 0)
                                    SelectSubmitterType(idCall, defaultSubmitter);
                            }
                            else
                            {
                                View.InitSubmissionTime = InitTime;
                                //if (submission.Status >= SubmissionStatus.submitted && viewList == CallStatusForSubmitters.SubmissionOpened)
                                //    viewList = CallStatusForSubmitters.Submitted;
                                View.AllowSubmitterSelection = false;
                                View.IdSubmission = submission.Id;
                                LoadSections(idCall, submission.Type.Id, submission.Id, idRevision);
                                View.SendUserAction(idCommunity, idModule, submission.Id, ModuleCallForPaper.ActionType.ViewSubmission);
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

            if (call != null && call.AttachSign)
                View.SetTextForSignatures();
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

        private void LoadCall(dtoCall dto, dtoBaseSubmission submission, long defaultSubmitter)
        {
            View.LoadCallInfo(dto);
            BaseForPaper call = Service.GetCall(dto.Id);
            List<dtoSubmitterType> submitters = null;
            if (call != null)
            {
                submitters = Service.GetCallAvailableSubmittersType(call);
                View.HasMultipleSubmitters = (submitters.Count > 1);
            }
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
                    else
                    {
                        idSubmitter = defaultSubmitter;
                        View.LoadSubmitterTypes(submitters);
                    }
                    View.AllowSubmitterSelection = (submitters.Count > 1 && defaultSubmitter==0);
                    if (idSubmitter != 0 && submitters.Where(s => s.Id == idSubmitter).Any())
                    {
                        if (submitters.Count > 1)
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
                LoadSections(call, idSubmitter, idSubmission, idRevision, null, null);
            }
        }

        private void LoadSections(BaseForPaper call, long idSubmitter, long idSubmission, long idRevision, Dictionary<long, FieldError> fieldsError, Dictionary<long, FieldError> filesError)
        {

            View.LoadAttachments(Service.GetAvailableCallAttachments(call.Id, idSubmitter));

            List<dtoCallSubmissionFile> requiredFiles = Service.GetRequiredFiles(call, idSubmitter, idSubmission);
            if (requiredFiles != null && requiredFiles.Count > 0 && View.TryToComplete && filesError != null)
                requiredFiles.ForEach(f => f.SetError(filesError));
            View.LoadRequiredFiles(requiredFiles);
            List<dtoCallSection<dtoSubmissionValueField>> sections = Service.GetSubmissionFields(call, idSubmitter, idSubmission, idRevision);
            if (sections != null && sections.Count > 0 && View.TryToComplete && fieldsError != null)
                sections.ForEach(s => s.Fields.ForEach(f=>f.SetError(fieldsError)));

            View.LoadSections(sections.Where(s => s.Fields.Count > 0).OrderBy(s => s.DisplayOrder).ThenBy(s => s.Name).ToList());
        }
        public void SelectSubmitterType(long idCall,long idSubmitter)
        {

            //ToDo: check che non abbia già il limite di sottomissioni!!!
            Boolean hasPermissionToSubmit = Service.IsCallAvailableByUser(idCall, UserContext.CurrentUserID);
            
            SubmitterType submitter = null;
            submitter = Service.GetSubmitterType(idSubmitter);


            if (submitter == null)
                hasPermissionToSubmit = false;


            if(hasPermissionToSubmit)
            {
                hasPermissionToSubmit = !Service.UserHasOtherSubmissionType(idCall, UserContext.CurrentUserID, submitter.Id);
            }

            if(hasPermissionToSubmit)
            {
                long submissionCount = Service.GetSubmissionCountBySubmitter(idCall, UserContext.CurrentUserID, submitter.Id);

                if (submitter.AllowMultipleSubmissions)
                {   
                    hasPermissionToSubmit = submissionCount < submitter.MaxMultipleSubmissions;
                } else
                {
                    hasPermissionToSubmit = (submissionCount <= 0);
                }
            }

            if(hasPermissionToSubmit)
            {
                View.InitSubmissionTime = DateTime.Now;
                View.AllowSave = true;
                View.AllowCompleteSubmission = true;
                View.AllowSubmitterSelection = false;
                View.IdSelectedSubmitterType = idSubmitter;
                View.InitializeView(Service.CallWithFileToUpload(idCall, idSubmitter));
                View.IdRevision = 0;
                View.IdSubmission = 0;
                LoadSections(idCall, idSubmitter, 0,0);
            } else
            {
                View.DisableSubmitterTypesSelection();
                View.SendToList();
            }

        }

        public void SaveSubmission(List<dtoSubmissionValueField> fields, DateTime clickDt, bool forsubmit = false)
        {
            //Attenzione!
            //Senza questo controllo, allo scadere della sessione l'utente legittimo si ritrova con la propria
            //sottomissione associata ad "anonimo", il che implica che:
            //l'utente non puo' più accedere, 
            //gli amministratori si trovano con un bando sottomesso da ignoti.

            if (!View.isAnonymousSubmission & UserContext.isAnonymous)
            {
                View.DisplayCallUnavailableForPublic();
                //View.DisplaySessionTimeout();    
                return;
            }
            
            UserSubmission submission = Service.GetUserSubmission(View.IdSubmission);
            
            if (forsubmit)
            {
                if (submission == null || submission.AllowEditSubmission(View.InitSubmissionTime, clickDt))
                {
                    Boolean tryToComplete = View.TryToComplete;
                    submission = SaveUserSubmission(fields);
                }
                
                return;
            }


            if (submission == null || submission.AllowEditSubmission(View.InitSubmissionTime, clickDt))
            {
                Boolean tryToComplete = View.TryToComplete;
                submission = SaveUserSubmission(fields);

                if (submission != null)
                    LoadSections(submission.Call, submission.Type.Id, submission.Id, View.IdRevision, (tryToComplete) ? Service.GetSubmissionFieldErrors(submission, View.IdRevision) : null, (tryToComplete) ? Service.GetSubmissionRequiredFileErrors(submission) : null);
            }
            else if (submission != null)
                View.GoToUrl(CallStandardAction.None, RootObject.ViewSubmission(submission.Call.Type, submission.Call.Id, submission.Id, View.FromPublicList, CallStatusForSubmitters.Submitted, View.PreloadIdOtherCommunity, 0));
            else
                View.GoToUrl(CallStandardAction.List, RootObject.ViewCalls(View.IdCall, CallForPaperType.CallForBids, CallStandardAction.List, View.IdCallCommunity, CallStatusForSubmitters.SubmissionClosed));

        }
        private UserSubmission SaveUserSubmission(List<dtoSubmissionValueField> fields)
        {
            long idCall = View.IdCall;
            long idSubmission = this.View.IdSubmission;
            long idRevision = View.IdRevision;
            UserSubmission submission = null;
            try
            {
                submission = Service.SaveSubmission(idSubmission,ref idRevision, idCall, View.IdSelectedSubmitterType, UserContext.CurrentUserID, fields);
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
                View.IdSubmission = submission.Id;
                View.IdRevision = idRevision;
                try
                {
                    Revision revision = Service.GetRevision(idRevision);

                    Service.SaveSubmissionFiles(
                        submission,
                        revision, 
                        UserContext.CurrentUserID, 
                        View.GetRequiredSubmittedFiles(
                            submission, 
                            ModuleCallForPaper.UniqueCode, 
                            View.IdCallModule, 
                            (int)ModuleCallForPaper.ActionType.DownloadSubmittedFile, 
                            (int)ModuleCallForPaper.ObjectType.UserSubmission),
                            View.GetFileValues(submission, ModuleCallForPaper.UniqueCode, 
                            View.IdCallModule, 
                            (int)ModuleCallForPaper.ActionType.DownloadSubmittedFile, 
                            (int)ModuleCallForPaper.ObjectType.UserSubmission)
                        );
                }
                catch (SubmissionInternalFileNotLinked ex)
                {
                    // remove internal files !
                }
                if (idSubmission == 0)
                    View.SendUserAction(View.IdCallCommunity, View.IdCallModule, submission.Id, ModuleCallForPaper.ActionType.AddSubmission);
                else
                    View.SendUserAction(View.IdCallCommunity, View.IdCallModule, submission.Id, ModuleCallForPaper.ActionType.EditSubmission);
            }
            else
                LoadSections(idCall, View.IdSelectedSubmitterType, idSubmission, idRevision );
            return submission;
        }

        public void SaveCompleteSubmission(
            List<dtoSubmissionValueField> fields, 
            lm.Comol.Core.MailCommons.Domain.Configurations.SmtpServiceConfig smtpConfig, 
            String websiteUrl, 
            Dictionary<SubmissionTranslations, string> translations, 
            String BaseFilePath, 
            String translationFileName,
            DateTime clickDt)
        {
            long idSubmission = this.View.IdSubmission;
            long idCall = View.IdCall;
            UserSubmission submission = Service.GetUserSubmission(View.IdSubmission);

            if (submission == null || submission.AllowEditSubmission(View.InitSubmissionTime, clickDt))
            {
                submission = SaveUserSubmission(fields);
                Revision revision = Service.GetRevision(View.IdRevision);
                if ((submission != null || submission.AllowEditSubmission(View.InitSubmissionTime, clickDt)) && revision != null)
                {
                    idSubmission = submission.Id;
                    //View.IdSubmission = idSubmission;
                    View.TryToComplete = true;
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



                            bool needSignature = Service.NeedSignature(idCall);


                            if (needSignature)
                            {
                                Service.UserLockSubmission(submission, revision, View.InitSubmissionTime, idUser, BaseFilePath, fields, smtpConfig, websiteUrl, translations, translationFileName, clickDt);
                            }
                            else
                            {
                                //Service.UserCompleteSubmission(submission, revision, View.InitSubmissionTime, idUser, baseFilePath, fields, smtpConfig, websiteUrl, translations, translationZIPFileName, clickDt);
                                Service.UserCompleteSubmission(submission, revision, View.InitSubmissionTime, idUser, BaseFilePath, fields, smtpConfig, websiteUrl, translations, translationFileName, clickDt);
                            }



                            //Service.UserCompleteSubmission(submission, revision, View.InitSubmissionTime, idUser, BaseFilePath, fields, smtpConfig, websiteUrl, translations, translationFileName, clickDt);
                            View.AllowCompleteSubmission = false;
                            View.AllowDeleteSubmission = false;
                            View.AllowSave = false;
                            View.SendUserAction(View.IdCallCommunity, View.IdCallModule, submission.Id, ModuleCallForPaper.ActionType.ConfirmSubmission);




                            if (needSignature)
                            {
                                View.GoToUrl(
                                    RootObject.ContinueSubmission(CallForPaperType.CallForBids, idCall, Service.CallIsPublic(idCall), idSubmission,
                                        View.FromPublicList, CallStatusForSubmitters.Draft, View.PreloadIdOtherCommunity)
                                    );

                                //(CallForPaperType.CallForBids, idCall, idSubmission,
                                //    View.FromPublicList, CallStatusForSubmitters.Draft, View.PreloadIdOtherCommunity)
                                //);

                            }
                            else
                            {
                                View.GoToUrl(RootObject.FinalMessage(CallForPaperType.CallForBids, idCall, idSubmission, View.IdRevision, submission.UserCode, View.isAnonymousSubmission, View.FromPublicList, CallStatusForSubmitters.Submitted, View.PreloadIdOtherCommunity));
                            }






                            //View.GoToUrl(RootObject.FinalMessage(CallForPaperType.CallForBids, idCall, idSubmission, View.IdRevision, submission.UserCode, View.isAnonymousSubmission, View.FromPublicList, CallStatusForSubmitters.Submitted, View.PreloadIdOtherCommunity));
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
                        List<dtoSubmissionValueField> errorFields = fields.Where(f => !errors.ContainsKey(f.IdField) && f.FieldError != FieldError.None).ToList();
                        errorFields.ForEach(e => errors.Add(e.IdField, e.FieldError));
                        View.AllowDeleteSubmission = (submission != null && submission.Deleted == BaseStatusDeleted.None && submission.Status == SubmissionStatus.draft);
                        View.LoadError((errors.ToList().Where(er => er.Value != FieldError.Mandatory).Any()) ? SubmissionErrorView.InvalidFields : SubmissionErrorView.RequiredItems, View.IdCallCommunity, idCall, CallStatusForSubmitters.SubmissionOpened);
                        LoadSections(submission.Call, submission.Type.Id, idSubmission, View.IdRevision, errors, Service.GetSubmissionRequiredFileErrors(submission));
                    }
                    catch (SubmissionTimeExpired ex)
                    {
                        View.LoadError(SubmissionErrorView.SubmissionValueSaving, View.IdCallCommunity, idCall, CallStatusForSubmitters.SubmissionClosed);
                    }
                    //View.InitSubmissionTime = DateTime.Now;
                }
            }
            else if (submission != null)
                View.GoToUrl(CallStandardAction.None, RootObject.ViewSubmission(CallForPaperType.CallForBids, idCall, idSubmission, View.FromPublicList, CallStatusForSubmitters.Submitted, View.PreloadIdOtherCommunity, 0));
            else
                View.GoToUrl(CallStandardAction.List, RootObject.ViewCalls(idCall, CallForPaperType.CallForBids, CallStandardAction.List, View.IdCallCommunity, CallStatusForSubmitters.SubmissionClosed));
        }

        public void RemoveSubmittedFile(long idSubmittedFile, List<dtoSubmissionValueField> fields, DateTime clickDt)
        {
            UserSubmission submission = Service.GetUserSubmission(View.IdSubmission);
            if (submission == null)
                View.GoToUrl(CallStandardAction.List, RootObject.ViewCalls(View.IdCall, CallForPaperType.CallForBids, CallStandardAction.List, View.IdCallCommunity, CallStatusForSubmitters.SubmissionClosed));
            else if (submission.AllowEditSubmission(View.InitSubmissionTime, clickDt))
            {
                try
                {
                    Service.RemoveSubmittedFile(idSubmittedFile);
                    View.SendUserAction(View.IdCallCommunity, View.IdCallModule, submission.Id, ModuleCallForPaper.ActionType.VirtualDeleteSubmittedFile);
                }
                catch (Exception ex)
                {
                }
                this.SaveSubmission(fields, clickDt);
            }
            else
                View.GoToUrl(RootObject.ViewSubmission(submission.Call.Type, submission.Call.Id, submission.Id, View.FromPublicList, CallStatusForSubmitters.Submitted, View.PreloadIdOtherCommunity, 0));

        }
        public void RemoveFieldFile(long idSubmittedField, List<dtoSubmissionValueField> fields, DateTime clickDt)
        {
            UserSubmission submission = Service.GetUserSubmission(View.IdSubmission);
            if (submission == null)
                View.GoToUrl(CallStandardAction.List, RootObject.ViewCalls(View.IdCall, CallForPaperType.CallForBids, CallStandardAction.List, View.IdCallCommunity, CallStatusForSubmitters.SubmissionClosed));
            else if (submission.AllowEditSubmission(View.InitSubmissionTime, clickDt))
            {
                try
                {
                    Service.RemoveFieldFileValue(idSubmittedField);
                    View.SendUserAction(View.IdCallCommunity, View.IdCallModule, submission.Id, ModuleCallForPaper.ActionType.VirtualDeleteSubmittedFile);
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
                View.GoToUrl(CallStandardAction.List, RootObject.ViewCalls(View.IdCall, CallForPaperType.CallForBids, CallStandardAction.List, View.IdCallCommunity, CallStatusForSubmitters.SubmissionClosed));
            else if (submission.AllowEditSubmission(View.InitSubmissionTime, clickDt))
            {
                Service.VirtualDeleteSubmission(View.IdSubmission, true);
                View.SendUserAction(View.IdCallCommunity, View.IdCallModule, submission.Id, ModuleCallForPaper.ActionType.VirtualDeleteSubmission);
                View.GoToUrl(RootObject.ViewCalls(View.IdCall, CallForPaperType.CallForBids, CallStandardAction.List, View.IdCallCommunity, CallStatusForSubmitters.SubmissionOpened));
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