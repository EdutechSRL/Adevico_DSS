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
    public class ManageEvaluatorsPresenter : lm.Comol.Core.DomainModel.Common.DomainPresenter
    {
        #region "Initialize"

            public virtual BaseModuleManager CurrentManager { get; set; }
            protected virtual IViewManageEvaluators View
            {
                get { return (IViewManageEvaluators)base.View; }
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
            public ManageEvaluatorsPresenter(iApplicationContext oContext)
                : base(oContext)
            {
                this.CurrentManager = new BaseModuleManager(oContext);
            }
            public ManageEvaluatorsPresenter(iApplicationContext oContext, IViewManageEvaluators view)
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
                    View.AllowAddEvaluator = View.AllowSave;
                    View.IdCall = idCall;
                    View.SetActionUrl(RootObject.ViewCalls(idCall, type, CallStandardAction.Manage, idCommunity, View.PreloadView));
                    LoadMembers(idCall);
                    View.SendUserAction(idCommunity, idModule, idCall, ModuleCallForPaper.ActionType.ManageEvaluators); 
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
            //else if (currentCommunity != null && (call == null || !call.IsPortal))
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
        private void LoadMembers(long idCall)
        {
            CallForPaper call = CallService.GetCallForPaper(idCall);
            if (call != null)
                LoadMembers(call);
        }
        private void LoadMembers(CallForPaper call)
        {
            List<dtoBaseCommittee> committees = Service.GetAvailableCommittees(call);
            List<dtoCommitteeMember> members = Service.GetCommitteesMembers(call);
            List<lm.Comol.Core.Wizard.NavigableWizardItem<dtoEvaluationStep>> steps = Service.GetAvailableSteps(call, WizardEvaluationStep.FullManageEvaluators);

            View.AllowMultipleCommittees = (committees.Count>1);
            Boolean multipleCommittee = !call.OneCommitteeMembership && committees.Count>1;
            View.IdOnlyOneCommittee = (committees == null || committees.Count == 0 || committees.Count > 1) ? 0 : committees[0].Id;
            if (committees == null || committees.Count == 0)
                View.DisplayError(EvaluationEditorErrors.NoCommittees);
            else if (members == null || members.Count == 0)
            {
                View.DisplayWarning(EvaluationEditorErrors.NoEvaluators);
                View.LoadWizardSteps(call.Id, View.IdCommunity, steps, EvaluationEditorErrors.NoEvaluators);
            }
            else
            {
                dtoEvaluationStep s = steps.Where(i=>i.Id.Type== WizardEvaluationStep.FullManageEvaluators).Select(i=> i.Id).FirstOrDefault();
                if (s!=null && s.Errors.Any()){
                    if (s.Errors.Contains(EditingErrors.CommitteeWithNoEvaluators))
                        View.DisplayWarning(EvaluationEditorErrors.CommitteeWithNoEvaluators);
                    else if (s.Errors.Contains(EditingErrors.UnassingedEvaluators))
                        View.DisplayWarning(EvaluationEditorErrors.UnassignedEvaluators);
                    else if (s.Errors.Contains(EditingErrors.MoreEvaluatorAssignment))
                        View.DisplayWarning(EvaluationEditorErrors.MoreEvaluatorAssignment);
                }
                View.LoadWizardSteps(call.Id, View.IdCommunity, steps);
                View.LoadEvaluators(members, multipleCommittee, committees);
            }
        }

        public void SaveSettings(List<dtoCommitteeMember> evaluators)
        {
            long idCall = View.IdCall;
            Int32 idCommunity = View.IdCommunity;
            if (!Service.SaveCallEvaluators(idCall, evaluators))
                View.DisplayError(EvaluationEditorErrors.Saving);
            else
            {
                LoadMembers(View.IdCall);
                View.DisplaySettingsSaved();
                View.SendUserAction(idCommunity, View.IdCallModule, idCall, ModuleCallForPaper.ActionType.SaveEvaluatorsSettings);
            }
        }
        public void AddEvaluators(List<dtoCommitteeMember> members, List<Int32> persons)
        {
            long idCall = View.IdCall;
            Int32 idCommunity = View.IdCommunity;
            List<CallEvaluator> evaluators = Service.AddEvaluatorsToCall(idCall,members, persons);
            if (evaluators == null || evaluators.Count==0)
                View.DisplayError(EvaluationEditorErrors.AddingEvaluators);
            else
            {
                View.SendUserAction(idCommunity, View.IdCallModule, idCall, ModuleCallForPaper.ActionType.AddEvaluator);
                View.ReloadEditor(RootObject.EvaluatorAddedToCall(evaluators.OrderBy(e=> e.Person.SurnameAndName).FirstOrDefault().Id, idCall, idCommunity, View.PreloadView));
            }
        }
        public void RemoveEvaluator(List<dtoCommitteeMember> members, long idEvaluator)
        {
            long pIdEvaluator = 0;
            long idCall = View.IdCall;
            Int32 idCommunity = View.IdCommunity;

            try
            {
                Service.SaveCallEvaluators(idCall, members);
                if (Service.VirtualDeleteEvaluator(idEvaluator, true, ref pIdEvaluator))
                {
                    View.SendUserAction(idCommunity, View.IdCallModule, idCall, ModuleCallForPaper.ActionType.VirtualDeleteEvaluator);
                    View.ReloadEditor(RootObject.EvaluatorRemovedFromCall(pIdEvaluator, idCall, idCommunity, View.PreloadView));
                }
                else
                    View.DisplayError(EvaluationEditorErrors.RemovingEvaluators);
            }
            catch (EvaluationStarted exSubmission)
            {
                View.DisplayError(EvaluationEditorErrors.RemovingEvaluators);
            }
            catch (Exception ex)
            {
                View.DisplayError(EvaluationEditorErrors.RemovingEvaluators);
            }
        }

        public void SaveCommitteeAssignmentPolicy(List<dtoCommitteeMember> evaluators, Boolean multipleCommittee)
        {
            long idCall = View.IdCall;
            Int32 idCommunity = View.IdCommunity;
            if (!Service.SaveCommitteeAssignmentPolicy(idCall, evaluators, multipleCommittee))
                View.DisplayError(EvaluationEditorErrors.Saving);
            else
            {
                LoadMembers(View.IdCall);
                View.DisplaySettingsSaved();
                View.SendUserAction(idCommunity, View.IdCallModule, idCall, ModuleCallForPaper.ActionType.SaveEvaluatorsSettings);
            }
        }
    }
}