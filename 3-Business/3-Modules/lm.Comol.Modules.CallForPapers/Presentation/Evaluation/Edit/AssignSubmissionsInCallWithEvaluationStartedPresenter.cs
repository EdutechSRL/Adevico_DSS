﻿using System;
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
    public class AssignSubmissionWithNoEvaluationPresenter : lm.Comol.Core.DomainModel.Common.DomainPresenter
    {
        #region "Initialize"

            public virtual BaseModuleManager CurrentManager { get; set; }
            protected virtual IViewAssignSubmissionWithNoEvaluation View
            {
                get { return (IViewAssignSubmissionWithNoEvaluation)base.View; }
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
            public AssignSubmissionWithNoEvaluationPresenter(iApplicationContext oContext)
                : base(oContext)
            {
                this.CurrentManager = new BaseModuleManager(oContext);
            }
            public AssignSubmissionWithNoEvaluationPresenter(iApplicationContext oContext, IViewAssignSubmissionWithNoEvaluation view)
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
                else if (!Service.HasSubmissionsNotInEvaluation(idCall, SubmissionStatus.accepted))
                    View.RedirectToUrl(RootObject.EditCommiteeByStep(idCall, idCommunity, WizardEvaluationStep.ManageEvaluators,View.PreloadView));
                else if (allowManage || allowSave)
                {
                    View.AllowSave = allowSave;
                    View.IdCall = idCall;
                    View.SetActionUrl(RootObject.ViewCalls(idCall, type, CallStandardAction.Manage, idCommunity, View.PreloadView));

                    LoadAssignments(idCall);
                    View.SendUserAction(idCommunity, idModule, idCall, ModuleCallForPaper.ActionType.ManageEvaluations);
                }
                else if (allowView)
                    View.RedirectToUrl(RootObject.ViewCalls(idCall, type, CallStandardAction.List,idCommunity, View.PreloadView));
                else
                    View.DisplayNoPermission(idCommunity, idModule);
            }
        }

        #region "Common"
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
            //else if (call.IsPortal)
            //{
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
            private void LoadAssignments(long idCall)
            {
                CallForPaper call = CallService.GetCallForPaper(idCall);
                if (call != null)
                    LoadAssignments(call);
            }
            private void LoadStep(long idCall, List<lm.Comol.Core.Wizard.NavigableWizardItem<dtoEvaluationStep>> steps, Boolean displayErrors)
            {
                lm.Comol.Core.Wizard.NavigableWizardItem<dtoEvaluationStep> step = steps.Where(i => i.Id.Type == WizardEvaluationStep.AssignSubmissionWithNoEvaluation).FirstOrDefault();
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

            //public void SaveSettings(List<dtoSubmissionAssignment> assignments) { 
            
            //}
        #endregion
      
        private void LoadAssignments(CallForPaper call)
        {
            List<lm.Comol.Core.Wizard.NavigableWizardItem<dtoEvaluationStep>> steps = Service.GetAvailableSteps(call, WizardEvaluationStep.AssignSubmissionWithNoEvaluation);
            List<dtoBaseCommittee> committees = Service.GetAvailableCommittees(call);
            View.MultiComittees = (committees.Count > 1);
            if (committees.Count == 0)
            {
                View.AllowSave = false;
                View.DisplayError(EvaluationEditorErrors.NoCommittees);
                LoadStep(call.Id,steps,false);
            }
            else if (committees.Count == 1)
            {
                View.IdCommittee = committees[0].Id;
                Dictionary<SubmissionStatus, int> infos = Service.GetSubmissionsInfoWithNoEvaluation(call);
                View.DisplayStartup(infos.Select(i => i.Value).Sum(), infos.ContainsKey(SubmissionStatus.accepted) ? infos[SubmissionStatus.accepted] : 0, infos.ContainsKey(SubmissionStatus.rejected) ? infos[SubmissionStatus.rejected] : 0);
                List<CommitteeMember> members = Service.GetCommitteeMembers(committees[0].Id);
                List<dtoSubmissionAssignment> assignments = Service.GetCommitteeAssignmentsForNoEvaluations(call, committees[0].Id, members);

                if (members == null || members.Count == 0)
                {
                    View.AllowSave = false;
                    View.DisplayError(EvaluationEditorErrors.NoEvaluators);
                    View.LoadWizardSteps(call.Id, View.IdCommunity, steps, EvaluationEditorErrors.NoEvaluators);
                }
                else
                {
                    LoadStep(call.Id,steps, true);
                    View.LoadSubmissions(assignments, members.Where(m => m.Evaluator != null && m.Evaluator.Person != null).ToDictionary(m => m.Evaluator.Id, m => m.Evaluator.Person.SurnameAndName));
                    View.LoadWizardSteps(call.Id, View.IdCommunity, steps);
                }
            }
            else
                LoadMultipleAssignments(call, committees, steps);
        }

        public void LoadMultipleAssignments(long idCall)
        {
            CallForPaper call = CallService.GetCallForPaper(idCall);
            if (call != null)
                LoadMultipleAssignments(call, Service.GetAvailableCommittees(call), Service.GetAvailableSteps(call, WizardEvaluationStep.AssignSubmissionWithNoEvaluation));
        }
        private void LoadMultipleAssignments(CallForPaper call, List<dtoBaseCommittee> committees, List<lm.Comol.Core.Wizard.NavigableWizardItem<dtoEvaluationStep>> steps)
        {
            Dictionary<SubmissionStatus, int> infos = Service.GetSubmissionsInfoWithNoEvaluation(call);
            View.DisplayStartup(infos.Select(i => i.Value).Sum(), infos.ContainsKey(SubmissionStatus.accepted) ? infos[SubmissionStatus.accepted] : 0, infos.ContainsKey(SubmissionStatus.rejected) ? infos[SubmissionStatus.rejected] : 0);
            Dictionary<long, Dictionary<long, String>> members = Service.GetCommitteesMemberships(call);
            List<dtoSubmissionMultipleAssignment> assignments = GetItems(call);

            if (members == null || members.Count == 0)
            {
                View.DisplayError(EvaluationEditorErrors.NoEvaluators);
                View.LoadWizardSteps(call.Id, View.IdCommunity, steps, EvaluationEditorErrors.NoEvaluators);
            }
            else
            {
                LoadStep(call.Id, steps, true);
                View.LoadSubmissions(assignments, members);
            }
        }
        private List<dtoSubmissionMultipleAssignment> GetItems(CallForPaper call)
        {
            List<dtoBaseSubmission> items = Service.GetSubmissionsForNoEvaluations(call.Id, View.CurrentOrderBy, View.CurrentAscending);
            if (items == null)
                return new List<dtoSubmissionMultipleAssignment>();
            else
                return Service.GetCommitteesAssignmentsForNoEvaluations(call, items);
        }

        public void SaveSettings(List<dtoSubmissionMultipleAssignment> assignments)
        {
            if (UserContext.isAnonymous)
                View.DisplaySessionTimeout();
            else {
                Dictionary<long, Dictionary<long, String>> availableEvaluators = View.AvailableEvaluators;
                List<dtoCommitteePartialAssignment> items = assignments.Where(a => a.Committees.Where(c => c.Evaluators.Count != availableEvaluators[c.IdCommittee].Values.Count).Any()).SelectMany(na => na.GetPartialAssignments(availableEvaluators)).ToList();
                View.ConfirmSettings(items);
            }
            //long idCall = View.IdCall;
            //Int32 idCommunity = View.IdCommunity;
            

            //if (assignments.Where(a=>a.NoEvaluators).Any() || assignments.Where(a=> a.e
            //if (!Service.SaveSubmissionsAssignments(idCall, assignments))
            //    View.DisplayError(EvaluationEditorErrors.Saving);
            //else
            //{
            //    LoadMultipleAssignments(View.IdCall);
            //    View.DisplaySettingsSaved();
            //    View.SendUserAction(idCommunity, View.IdCallModule, idCall, ModuleCallForPaper.ActionType.SaveSingleSubmissionAssignmentForEvaluation);
            //}
        }
        public void SaveSettings(List<dtoSubmissionAssignment> assignments)
        {
            if (UserContext.isAnonymous)
                View.DisplaySessionTimeout();
            else
            {
                Int32 count = View.AvailableEvaluators[View.IdCommittee].Values.Count;
                String comittee = Service.GetCommitteeName(View.IdCommittee);
                List<dtoCommitteePartialAssignment> items = assignments.Where(a => a.Evaluators.Count != count)
                                                        .Select(a => new dtoCommitteePartialAssignment() {
                                                            IdSubmission= a.IdSubmission,
                                                            SubmitterName= a.DisplayName,
                                                            AssignedEvaluators= a.Evaluators.Count,
                                                            AvailableEvaluators = count,
                                                            SubmittedOn= a.SubmittedOn,
                                                            SubmitterType= a.SubmitterType,
                                                            CommitteeName = comittee
                                                        }).ToList();
                View.ConfirmSettings(items);
            }
        }
        public void ConfirmSettings(List<dtoSubmissionAssignment> assignments)
        {
            if (UserContext.isAnonymous)
                View.DisplaySessionTimeout();
            else
            {
                long idCall = View.IdCall;
                Int32 idCommunity = View.IdCommunity;
                if (!Service.SaveSubmissionsAssignments(idCall, View.IdCommittee, assignments))
                    View.DisplayError(EvaluationEditorErrors.Saving);
                else
                {
                    View.SendUserAction(idCommunity, View.IdCallModule, idCall, ModuleCallForPaper.ActionType.SaveSingleSubmissionAssignmentForEvaluation);
                    if (!Service.HasSubmissionsNotInEvaluation(idCall, SubmissionStatus.accepted))
                        View.RedirectToUrl(RootObject.EditCommiteeByStep(idCall, idCommunity, WizardEvaluationStep.ManageEvaluators, View.PreloadView));
                    else
                    {
                        LoadAssignments(View.IdCall);
                        View.DisplaySettingsSaved();
                    }
                }
            }
        }
        public void ConfirmSettings(List<dtoSubmissionMultipleAssignment> assignments)
        {
            if (UserContext.isAnonymous)
                View.DisplaySessionTimeout();
            else
            {
                long idCall = View.IdCall;
                Int32 idCommunity = View.IdCommunity;

                if (!Service.SaveSubmissionsAssignments(idCall, assignments))
                    View.DisplayError(EvaluationEditorErrors.Saving);
                else
                {
                    View.SendUserAction(idCommunity, View.IdCallModule, idCall, ModuleCallForPaper.ActionType.SaveSingleSubmissionAssignmentForEvaluation);

                    if (!Service.HasSubmissionsNotInEvaluation(idCall, SubmissionStatus.accepted))
                        View.RedirectToUrl(RootObject.EditCommiteeByStep(idCall, idCommunity, WizardEvaluationStep.ManageEvaluators,View.PreloadView));
                    else
                    {
                        LoadMultipleAssignments(View.IdCall);
                        View.DisplaySettingsSaved();
                    }
                }
            }
        }
        //public void CreateAssignments(Boolean byDefault)
        //{
        //    long idCall = View.IdCall;
        //    Int32 idCommunity = View.IdCommunity;
        //    if (byDefault)
        //    {
        //        if (!Service.SaveDefaultAssignments(idCall, View.IdCommittee))
        //            View.DisplayError(EvaluationEditorErrors.Saving);
        //        else
        //        {

        //            View.DisplaySettingsSaved();
        //            View.SendUserAction(idCommunity, View.IdCallModule, idCall, ModuleCallForPaper.ActionType.SaveSingleSubmissionAssignmentForEvaluation);
        //        }
        //    }
        //    LoadAssignments(View.IdCall);
        //}
        //public void SetEvaluatorsToAllSubmissions()
        //{
        //    long idCall = View.IdCall;
        //    Int32 idCommunity = View.IdCommunity;
        //    if (!Service.SetEvaluatorsToAllSubmissions(idCall, View.IdCommittee))
        //        View.DisplayError(EvaluationEditorErrors.Saving);
        //    else
        //    {
        //        View.DisplaySettingsSaved();
        //        View.SendUserAction(idCommunity, View.IdCallModule, idCall, ModuleCallForPaper.ActionType.SaveSingleSubmissionAssignmentForEvaluation);
        //    }
        //    LoadAssignments(View.IdCall);
        //}

        //public void CreateAssignments(Boolean byDefault, dtoSubmissionFilters filters)
        //{
        //    long idCall = View.IdCall;
        //    Int32 idCommunity = View.IdCommunity;
        //    if (byDefault)
        //    {
        //        if (!Service.SaveDefaultAssignments(idCall))
        //            View.DisplayError(EvaluationEditorErrors.Saving);
        //        else
        //        {

        //            View.DisplaySettingsSaved();
        //            View.SendUserAction(idCommunity, View.IdCallModule, idCall, ModuleCallForPaper.ActionType.SaveSingleSubmissionAssignmentForEvaluation);
        //        }
        //    }
        //    LoadAssignments(View.IdCall, filters, 0, View.PageSize);
        //}
        //public void SetEvaluatorsToAllSubmissions(dtoSubmissionFilters filters, int pageIndex, int pageSize)
        //{
        //    long idCall = View.IdCall;
        //    Int32 idCommunity = View.IdCommunity;
        //    if (!Service.SetEvaluatorsToAllSubmissions(idCall))
        //        View.DisplayError(EvaluationEditorErrors.Saving);
        //    else
        //    {
        //        View.DisplaySettingsSaved();
        //        View.SendUserAction(idCommunity, View.IdCallModule, idCall, ModuleCallForPaper.ActionType.SaveSingleSubmissionAssignmentForEvaluation);
        //    }
        //    LoadAssignments(View.IdCall, filters, pageIndex, pageSize);
        //}
    }
}