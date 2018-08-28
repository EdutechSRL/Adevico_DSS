using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel;
using lm.Comol.Modules.CallForPapers.Business;
using lm.Comol.Core.Business;
using lm.Comol.Modules.CallForPapers.Domain.Evaluation;
using lm.Comol.Modules.CallForPapers.Domain;

namespace lm.Comol.Modules.CallForPapers.Presentation.Evaluation
{
    public class CommitteesEvaluationAssignmentsPresenter : lm.Comol.Core.DomainModel.Common.DomainPresenter
    {
        #region "Initialize"

            public virtual BaseModuleManager CurrentManager { get; set; }
            protected virtual IViewCommitteesSubmissionAssignments View
            {
                get { return (IViewCommitteesSubmissionAssignments)base.View; }
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
            private ServiceCallOfPapers _ServiceCall;
            private ServiceCallOfPapers CallService
            {
                get
                {
                    if (_ServiceCall == null)
                        _ServiceCall = new ServiceCallOfPapers(AppContext);
                    return _ServiceCall;
                }
            }
            private ServiceRequestForMembership _ServiceRequest;
            private ServiceRequestForMembership RequestService
            {
                get
                {
                    if (_ServiceRequest == null)
                        _ServiceRequest = new ServiceRequestForMembership(AppContext);
                    return _ServiceRequest;
                }
            }
            public CommitteesEvaluationAssignmentsPresenter(iApplicationContext oContext)
                : base(oContext)
            {
                this.CurrentManager = new BaseModuleManager(oContext);
            }
            public CommitteesEvaluationAssignmentsPresenter(iApplicationContext oContext, IViewCommitteesSubmissionAssignments view)
                : base(oContext, view)
            {
                this.CurrentManager = new BaseModuleManager(oContext);
            }
        #endregion

        public void InitView()
        {
            long idCall = View.PreloadIdCall;

            dtoBaseForPaper call = null;
            CallForPaperType type = CallService.GetCallType(idCall);
            call = CallService.GetDtoBaseCall(idCall);

            int idCommunity = SetCallCurrentCommunity(call);
            if (UserContext.isAnonymous)
                View.DisplaySessionTimeout();
            else if (type != CallForPaperType.CallForBids)
                View.RedirectToUrl(RootObject.ViewCalls(type, CallStandardAction.Manage, idCommunity, View.PreloadView));
            else
            {
                litePerson currenUser = CurrentManager.GetLitePerson(UserContext.CurrentUserID);
                ModuleCallForPaper module =  CallService.CallForPaperServicePermission(UserContext.CurrentUserID, idCommunity);
                Boolean allowView  = (module.ViewCallForPapers || module.Administration || module.ManageCallForPapers);
                Boolean allowManage = module.CreateCallForPaper || module.Administration || module.ManageCallForPapers || module.EditCallForPaper;
                Boolean allowSave = (module.Administration || module.ManageCallForPapers || (module.CreateCallForPaper && idCall == 0) || (call != null && module.EditCallForPaper && currenUser == call.Owner));
            
                int idModule = CallService.ServiceModuleID();
                View.IdCallModule = idModule;
                if (call == null)
                    View.LoadUnknowCall(idCommunity, idModule, idCall, type);
                else if (Service.CallHasEvaluationStarted(idCall))
                    View.RedirectToUrl(RootObject.EditCommiteeByStep(idCall, idCommunity, WizardEvaluationStep.ManageEvaluators,View.PreloadView));
                else if (allowManage || allowSave)
                {
                    View.AllowSave = allowSave && (!Service.CallHasEvaluationStarted(idCall));
                    View.IdCall = idCall;
                    View.SetActionUrl(RootObject.ViewCalls(idCall, type, CallStandardAction.Manage, idCommunity, View.PreloadView));
                    LoadAssignments(idCall, View.PreloadFilters, View.PreloadPageIndex, View.PreloadPageSize);
                    View.SendUserAction(idCommunity, idModule, idCall, ModuleCallForPaper.ActionType.ManageEvaluations); 
                }
                else if (allowView)
                    View.RedirectToUrl(RootObject.ViewCalls(idCall, type, CallStandardAction.List,idCommunity, View.PreloadView));
                else
                    View.DisplayNoPermission(idCommunity, idModule);
            }
        }

        private int SetCallCurrentCommunity(dtoBaseForPaper call)
        {
            dtoCallCommunityContext context = CallService.GetCallCommunityContext(call, View.Portalname);
            if (String.IsNullOrEmpty(context.CommunityName))
                View.SetContainerName(context.CallName);
            else
                View.SetContainerName(context.CommunityName, context.CallName);

            View.IdCommunity = context.IdCommunity;
            return context.IdCommunity;
            //int idCommunity = 0;
            //Community currentCommunity = CurrentManager.GetCommunity(this.UserContext.CurrentCommunityID);
            //Community community = null;
            //if (call!=null)
            //    idCommunity = (call.IsPortal) ? 0 : (call.Community !=null) ? call.Community.Id : 0;

            //String cName = "";
            //String callName = (call != null) ? call.Name : "";
            //community = CurrentManager.GetCommunity(idCommunity);

            //if (community != null )
            //    cName =  (community.Id != UserContext.CurrentCommunityID) ? community.Name : "";
            //else if (currentCommunity != null && !call.IsPortal)
            //{
            //    idCommunity = this.UserContext.CurrentCommunityID;
            //    cName = (call.IsPortal) ? View.Portalname : "";
            //}
            //else if (call.IsPortal) {
            //    idCommunity = 0;
            //    cName = View.Portalname;
            //}
            //else
            //    idCommunity = 0;
           

            //if (String.IsNullOrEmpty(cName))
            //    View.SetContainerName(callName);
            //else
            //    View.SetContainerName(cName, callName);

            //View.IdCommunity = idCommunity;
            //return idCommunity;
        }
        public void LoadAssignments(long idCall, dtoSubmissionFilters filters, int pageIndex, int pageSize)
        {
            CallForPaper call = CallService.GetCallForPaper(idCall);
            if (call != null)
                LoadAssignments(call, filters, pageIndex, pageSize);
        }
        private void LoadAssignments(CallForPaper call, dtoSubmissionFilters filters, int pageIndex, int pageSize)
        {
            List<lm.Comol.Core.Wizard.NavigableWizardItem<dtoEvaluationStep>> steps = Service.GetAvailableSteps(call, WizardEvaluationStep.MultipleAssignSubmission);
            View.AllowChangeAssignModeToEvalutors = (call.EvaluationType != EvaluationType.Dss);
            List<dtoBaseCommittee> committees = Service.GetAvailableCommittees(call);
            if (committees.Count == 0)
            {
                View.DisplayError(EvaluationEditorErrors.NoCommittees);
                LoadStep(call.Id,steps,false);
            }
            else if (committees.Count == 1)
                View.ReloadEditor(RootObject.EditCommiteeByStep(call.Id, View.IdCommunity, WizardEvaluationStep.AssignSubmission, View.PreloadView));
            else
            {
                Dictionary<SubmissionStatus, int> infos = Service.GetSubmissionsInfo(call);
                if (Service.CallHasEvaluation(call))
                {
                    Dictionary<long, Dictionary<long, String>> members = Service.GetCommitteesMemberships(call);
                    List<dtoSubmissionMultipleAssignment> assignments = GetItems(call, filters, pageIndex, pageSize);

                    if (members == null || members.Count == 0)
                    {
                        View.DisplayError(EvaluationEditorErrors.NoEvaluators);
                        View.LoadWizardSteps(call.Id, View.IdCommunity, steps, EvaluationEditorErrors.NoEvaluators);
                    }
                    else
                    {
                        View.AllowAssignEvaluatorsToAll = View.AllowSave;
                        LoadStep(call.Id,steps, true);
                        View.LoadSubmissions(assignments, members);
                        View.LoadWizardSteps(call.Id, View.IdCommunity, steps);
                    }
                }
                else
                {
                    View.AllowAssignEvaluatorsToAll = false;
                    LoadStep(call.Id,steps, true);
                    if (infos.Where(i => i.Value > 0).Select(i => i.Key).Where(k => k >= SubmissionStatus.accepted && k != SubmissionStatus.rejected).Any())
                        View.DisplayStartup(infos.Select(i => i.Value).Sum(), infos.ContainsKey(SubmissionStatus.accepted) ? infos[SubmissionStatus.accepted] : 0, infos.ContainsKey(SubmissionStatus.rejected) ? infos[SubmissionStatus.rejected] : 0);
                    else
                        View.DisplayNoAvailableSubmission(infos.Select(i => i.Value).Sum(), (infos.ContainsKey(SubmissionStatus.rejected) ? infos[SubmissionStatus.rejected] : 0));
                    View.LoadWizardSteps(call.Id, View.IdCommunity, steps);
                }
            }

        }
        private List<dtoSubmissionMultipleAssignment> GetItems(CallForPaper call,dtoSubmissionFilters filters, int pageIndex, int pageSize)
        {
            PagerBase pager = new PagerBase();
            pager.PageSize = pageSize;

            if (pageSize == 0)
                pageSize = 50;
            pager.Count = (int)CallService.SubmissionsCount(call.Id, filters) - 1;
            pager.PageIndex = pageIndex;// Me.View.CurrentPageIndex
            View.Pager = pager;

            View.CurrentOrderBy = filters.OrderBy;
            View.CurrentFilterBy = filters.Status;
            View.CurrentAscending = filters.Ascending;
            View.PageSize = pageSize;
            if (pager.Count < 0)
                return new  List<dtoSubmissionMultipleAssignment>();
            else
                return Service.GetCommitteesAssignments(call,CallService.GetSubmissionsForEvaluation(call.Id, filters, pager.PageIndex, pageSize));
        }


        private void LoadStep(long idCall,List<lm.Comol.Core.Wizard.NavigableWizardItem<dtoEvaluationStep>> steps, Boolean displayErrors)
        {
            lm.Comol.Core.Wizard.NavigableWizardItem<dtoEvaluationStep> step = steps.Where(i => i.Id.Type == WizardEvaluationStep.AssignSubmission).FirstOrDefault();
            if (displayErrors && step != null && step.Id != null && step.Id.Errors.Any())
            {
                if (step.Id.Errors.Contains(EditingErrors.NoEvaluatorsForAssignments))
                    View.DisplayError(EvaluationEditorErrors.NoEvaluatorsForAssignments);
                else if (step.Id.Errors.Contains(EditingErrors.NoSubmissionToEvaluate))
                    View.DisplayWarning(EvaluationEditorErrors.NoSubmissionToEvaluate);
                else if (step.Id.Errors.Contains(EditingErrors.SubmissionToAssign))
                    View.DisplayWarning(EvaluationEditorErrors.SubmissionToAssign);
            }

            View.LoadWizardSteps(idCall, View.IdCommunity, steps);
        }
        public void SaveSettings(List<dtoSubmissionMultipleAssignment> assignments, dtoSubmissionFilters filters, int pageIndex, int pageSize)
        {
            long idCall = View.IdCall;
            Int32 idCommunity = View.IdCommunity;
            if (!Service.SaveSubmissionsAssignments(idCall, assignments))
                View.DisplayError(EvaluationEditorErrors.Saving);
            else
            {
                LoadAssignments(View.IdCall, filters, pageIndex, pageSize);
                View.DisplaySettingsSaved();
                View.SendUserAction(idCommunity, View.IdCallModule, idCall, ModuleCallForPaper.ActionType.SaveSingleSubmissionAssignmentForEvaluation);
            }
        }
        public void CreateAssignments(Boolean byDefault, dtoSubmissionFilters filters)
        {
            //ToDo: togliere e sistemare le commissioni!!!
            byDefault = true;

            long idCall = View.IdCall;
            Int32 idCommunity = View.IdCommunity;
            if (byDefault)
            {
                if (!Service.SaveDefaultAssignments(idCall))
                    View.DisplayError(EvaluationEditorErrors.Saving);
                else
                {

                    View.DisplaySettingsSaved();
                    View.SendUserAction(idCommunity, View.IdCallModule, idCall, ModuleCallForPaper.ActionType.SaveSingleSubmissionAssignmentForEvaluation);
                }
            }
            LoadAssignments(View.IdCall, filters,0,View.PageSize);
        }
        public void SetEvaluatorsToAllSubmissions(dtoSubmissionFilters filters, int pageIndex, int pageSize)
        {
            long idCall = View.IdCall;
            Int32 idCommunity = View.IdCommunity;
            if (!Service.SetEvaluatorsToAllSubmissions(idCall))
                View.DisplayError(EvaluationEditorErrors.Saving);
            else
            {
                View.DisplaySettingsSaved();
                View.SendUserAction(idCommunity, View.IdCallModule, idCall, ModuleCallForPaper.ActionType.SaveSingleSubmissionAssignmentForEvaluation);
            }
            LoadAssignments(View.IdCall, filters, pageIndex, pageSize);
        }
    }
}