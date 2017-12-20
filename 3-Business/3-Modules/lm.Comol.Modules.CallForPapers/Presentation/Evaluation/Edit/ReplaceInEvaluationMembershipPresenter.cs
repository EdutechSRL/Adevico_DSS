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
    public class ReplaceInEvaluationMembershipPresenter : lm.Comol.Core.DomainModel.Common.DomainPresenter
    {
        #region "Initialize"

            public virtual BaseModuleManager CurrentManager { get; set; }
            protected virtual IViewReplaceInEvaluationMembership View
            {
                get { return (IViewReplaceInEvaluationMembership)base.View; }
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
            public ReplaceInEvaluationMembershipPresenter(iApplicationContext oContext)
                : base(oContext)
            {
                this.CurrentManager = new BaseModuleManager(oContext);
            }
            public ReplaceInEvaluationMembershipPresenter(iApplicationContext oContext, IViewReplaceInEvaluationMembership view)
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
                else if (allowManage || allowSave)
                {
                    View.UseDss = Service.CallUseDss(idCall);
                    View.AssignAll = true;
                    View.AllowSave = allowSave;
                    View.IdCall = idCall;
                    View.SetActionUrl(RootObject.ViewEvaluators(View.PreloadIdMembership, idCall, idCommunity, View.PreloadView));
                    InitializeView(idCommunity,idCall, View.PreloadIdMembership);
                    View.SendUserAction(idCommunity, idModule, idCall, ModuleCallForPaper.ActionType.ViewAssignedEvaluations); 
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
        private void InitializeView(Int32 idCommunity, long idCall, long idMembership)
        {
            View.IdMembershipToReplace = idMembership;
            List<lm.Comol.Core.Wizard.NavigableWizardItem<dtoEvaluationStep>> steps = Service.GetAvailableSteps(idCall, WizardEvaluationStep.ManageEvaluators);
            steps.ForEach(s => s.Status = Core.Wizard.WizardItemStatus.disabled);
            View.LoadWizardSteps(idCall, View.IdCommunity, steps);
            CommitteeMember member = Service.GetCommitteeMembership(idMembership);
            if (member == null || member.Evaluator == null || member.Evaluator.Person == null)
                View.DisplayMemberNotFound();
            else if (member.Status== MembershipStatus.Replaced)
                View.RedirectToUrl(RootObject.ViewEvaluators(idMembership, idCall, idCommunity, View.PreloadView));
            else {
                Boolean completed = Service.EvaluationsCompleted(idMembership);
                if (completed)
                    View.DisplayEvaluationsCompleted(member.Evaluator.Person.SurnameAndName);
                else { 
                    Dictionary<Domain.Evaluation.EvaluationStatus, long> info = Service.GetEvaluatorStatistics(member);
                    View.DisplayMemberInfo(member.Evaluator.Person.SurnameAndName, info[Domain.Evaluation.EvaluationStatus.Evaluated], info[Domain.Evaluation.EvaluationStatus.Evaluating],  info[Domain.Evaluation.EvaluationStatus.None]);
                    View.LoadEvaluationInfos(Service.GetMemberEvaluations(member,View.AnonymousDisplayname,View.UnknownDisplayname));
                }
            }
        }

        public void SelectEvaluator(Int32 idUser)
        {
            List<Int32> inuseEvaluators = new List<Int32>();
            litePerson person = CurrentManager.GetLitePerson(idUser);
            CommitteeMember member = Service.GetCommitteeMembership(View.IdMembershipToReplace);
            if (member != null && member.Committee != null)
                inuseEvaluators.AddRange(member.Committee.Members.Where(m => m.Deleted == BaseStatusDeleted.None && m.Evaluator != null && m.Evaluator.Person != null).Select(m => m.Evaluator.Person.Id).ToList());
            if (idUser>0)
                inuseEvaluators.Add(idUser);
            if (idUser == 0 || person == null)
                View.InitializeEvaluatorSelection(inuseEvaluators);
            else
                View.LoadSelectEvaluator(inuseEvaluators, idUser, person.SurnameAndName, View.AssignAll);
        }

        public void ReplaceEvaluator(long idCall,Int32 idNewEvaluator, Boolean assignAll) {
            CommitteeMember member = Service.ReplaceEvaluator(idCall,View.IdMembershipToReplace, idNewEvaluator, assignAll, CallService.CallUseDss(View.IdCall));
            if (member!=null)
                View.RedirectToUrl(RootObject.ViewEvaluators(View.IdMembershipToReplace, View.IdCall, View.IdCommunity, View.PreloadView));
            else
                View.DisplayError(EvaluationEditorErrors.ReplacingEvaluator);
        }
    }
}