using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using lm.Comol.Core.DomainModel;
using iTextSharp.text;
using lm.Comol.Modules.CallForPapers.Domain;
using lm.Comol.Modules.CallForPapers.Domain.Evaluation;
using lm.Comol.Modules.CallForPapers.Domain.Evaluation.Export;
using lm.Comol.Core.Dss.Domain.Templates;
using lm.Comol.Core.Dss.Domain;
namespace lm.Comol.Modules.CallForPapers.Business
{
    public partial class ServiceEvaluation
    {
        protected const int maxItemsForQuery = 50;
        protected lm.Comol.Core.Business.BaseModuleManager Manager { get; set; }
        protected iUserContext UC { set; get; }

       

        #region initClass
            public ServiceEvaluation() { }
            public ServiceEvaluation(iApplicationContext oContext)
            {
                Manager = new lm.Comol.Core.Business.BaseModuleManager(oContext.DataContext);
                UC = oContext.UserContext;
                _ServiceDss = new Core.Dss.Business.ServiceDss(oContext);
            }
            public ServiceEvaluation(iDataContext oDC)
            {
                Manager = new lm.Comol.Core.Business.BaseModuleManager(oDC);
                UC = null;
                _ServiceDss = new Core.Dss.Business.ServiceDss(oDC);
            }
        #endregion

        protected int ServiceModuleID(String code)
        {
            return this.Manager.GetModuleID(code);
        }

        public Boolean CallUseDss(long idCall)
        {
            Boolean result = false;
            try
            {
                result = (from c in Manager.GetIQ<CallForPaper>()
                          where c.Id == idCall && c.EvaluationType== EvaluationType.Dss select c.Id).Any();
            }
            catch (Exception ex)
            {

            }
            return result;
        }
        public Boolean CallUseFuzzy(long idCall)
        {
            Boolean result = false;
            try
            {
                CallForPaper call = Manager.Get<CallForPaper>(idCall);
                result = (call != null && call.EvaluationType == EvaluationType.Dss && call.IsDssMethodFuzzy);
            }
            catch (Exception ex)
            {

            }
            return result;
        }

        public Boolean isNewCommittee(BaseForPaper call)
        {
            return !(from s in Manager.GetIQ<EvaluationCommittee>() where s.Call == call select s.Id).Any();
        }
        public Boolean CallHasEvaluation(long idCall)
        {
            return CallHasEvaluation(idCall, Domain.Evaluation.EvaluationStatus.None, true);
        }
        public Boolean CallHasEvaluation(BaseForPaper call)
        {
            return CallHasEvaluation(call, Domain.Evaluation.EvaluationStatus.None, true);
        }
        //public Boolean CallHasEvaluation(BaseForPaper call)
        //{
        //    Boolean found = false;
        //    try
        //    {
        //        Manager.BeginTransaction();
        //        found = (from e in Manager.GetIQ<Evaluation>() where e.Call.Id == call.Id && e.Deleted == BaseStatusDeleted.None select e.Id).Any();
        //        Manager.Commit();
        //    }
        //    catch (Exception ex)
        //    {
        //        found = true;
        //        Manager.RollBack();
        //    }
        //    return found;
        //}
        //public Boolean CallHasSubmissionEvaluated(long idCall)
        //{
        //    BaseForPaper call = Manager.Get<BaseForPaper>(idCall);
        //    return (call == null || CallHasSubmissionEvaluated(call));
        //}
        //public Boolean CallHasSubmissionEvaluated(BaseForPaper call)
        //{
        //    Boolean found = false;
        //    try
        //    {
        //        Manager.BeginTransaction();
        //        found = (from e in Manager.GetIQ<Evaluation>() 
        //                 where e.Call.Id == call.Id && e.Deleted == BaseStatusDeleted.None && e.Status== Domain.Evaluation.EvaluationStatus.Evaluated
        //                 select e.Id).Any();
        //        Manager.Commit();
        //    }
        //    catch (Exception ex)
        //    {
        //        found = true;
        //        Manager.RollBack();
        //    }
        //    return found;
        //}


        public Boolean CallHasEvaluationStarted(long idCall)
        {
            return CallHasEvaluation(idCall, Domain.Evaluation.EvaluationStatus.Evaluating, true);
        }
        public Boolean CallHasEvaluationStarted(BaseForPaper call)
        {
            return CallHasEvaluation(call, Domain.Evaluation.EvaluationStatus.Evaluating, true);
        }
       
        public Boolean CallHasEvaluation(long idCall, Domain.Evaluation.EvaluationStatus status, Boolean orMore)
        {
            BaseForPaper call = Manager.Get<BaseForPaper>(idCall);
            return (call == null || CallHasEvaluation(call,status, orMore));
        }
        public Boolean CallHasEvaluation(BaseForPaper call, Domain.Evaluation.EvaluationStatus status, Boolean orMore)
        {
            Boolean found = false;
            try
            {
                Manager.BeginTransaction();
                found = (from e in Manager.GetIQ<Evaluation>() where e.Call.Id == call.Id && e.Deleted == BaseStatusDeleted.None && ((orMore && e.Status >= status) || e.Status == status) select e.Id).Any();
                Manager.Commit();
            }
            catch (Exception ex)
            {
                found = true;
                Manager.RollBack();
            }
            return found;
        }
        public Int32 GetCommitteesCount(BaseForPaper call)
        {
            Int32 count = 0;
            try
            {
                count = (from s in Manager.GetIQ<EvaluationCommittee>()
                         where s.Deleted == BaseStatusDeleted.None && s.Call.Id == call.Id
                         select s.Id).Count();
            }
            catch (Exception ex)
            {

            }
            return count;
        }
        public Int32 GetCommitteesCount(long idCall)
        {
            Int32 count = 0;
            try
            {
                count = (from s in Manager.GetIQ<EvaluationCommittee>()
                         where s.Deleted == BaseStatusDeleted.None && s.Call.Id == idCall
                         select s.Id).Count();
            }
            catch (Exception ex)
            {

            }
            return count;
        }
        #region "Common" 
            protected Boolean AllowReorder(BaseForPaper call, litePerson user)
            {
                Boolean iResponse = false;
                switch (call.Type)
                {
                    case CallForPaperType.CallForBids:
                        ModuleCallForPaper moduleCall = CallForPaperServicePermission(user.Id, (call.Community == null) ? 0 : call.Community.Id);
                        iResponse = (moduleCall.Administration || moduleCall.ManageCallForPapers || (moduleCall.EditCallForPaper && call.CreatedBy == user));
                        break;

                    case CallForPaperType.RequestForMembership:
                        ModuleRequestForMembership moduleMembership = RequestForMembershipServicePermission(user.Id, (call.Community == null) ? 0 : call.Community.Id);
                        iResponse = (moduleMembership.Administration || moduleMembership.ManageBaseForPapers || (moduleMembership.EditBaseForPaper && call.CreatedBy == user));
                        break;

                    default:
                        iResponse = false;
                        break;
                }

                return iResponse;
            }

            #region "Permission"
                public ModuleCallForPaper CallForPaperServicePermission(int personId, int idCommunity)
                {
                    litePerson person = Manager.GetLitePerson(personId);
                    return CallForPaperServicePermission(person, idCommunity);
                }
                public ModuleCallForPaper CallForPaperServicePermission(litePerson person, int idCommunity)
                {
                    ModuleCallForPaper module = new ModuleCallForPaper();
                    if (person == null)
                        person = (from p in Manager.GetIQ<litePerson>() where p.TypeID == (int)UserTypeStandard.Guest select p).Skip(0).Take(1).ToList().FirstOrDefault();
                    if (idCommunity == 0)
                        module = ModuleCallForPaper.CreatePortalmodule((person == null) ? (int)UserTypeStandard.Guest : person.TypeID);
                    else
                        module = new ModuleCallForPaper(Manager.GetModulePermission(person.Id, idCommunity, ServiceModuleID(ModuleCallForPaper.UniqueCode)));
                    return module;
                }
                public ModuleRequestForMembership RequestForMembershipServicePermission(int personId, int idCommunity)
                {
                    litePerson person = Manager.GetLitePerson(personId);
                    return RequestForMembershipServicePermission(person, idCommunity);
                }
                public ModuleRequestForMembership RequestForMembershipServicePermission(litePerson person, int idCommunity)
                {
                    ModuleRequestForMembership module = new ModuleRequestForMembership();
                    if (person == null)
                        person = (from p in Manager.GetIQ<litePerson>() where p.TypeID == (int)UserTypeStandard.Guest select p).Skip(0).Take(1).ToList().FirstOrDefault();

                    if (idCommunity == 0)
                        module = ModuleRequestForMembership.CreatePortalmodule((person == null) ? (int)UserTypeStandard.Guest : person.TypeID);
                    else
                        module = new ModuleRequestForMembership(Manager.GetModulePermission(person.Id, idCommunity, ServiceModuleID(ModuleRequestForMembership.UniqueCode)));
                    return module;
                }
            #endregion
        #endregion
      
        #region "Status"
            public List<lm.Comol.Core.Wizard.NavigableWizardItem<dtoEvaluationStep>> GetAvailableSteps(long idCall, WizardEvaluationStep current) {
                BaseForPaper call = Manager.Get<BaseForPaper>(idCall);
                if (call != null)
                    return GetAvailableSteps(call, current);
                else
                    return new List<lm.Comol.Core.Wizard.NavigableWizardItem<dtoEvaluationStep>>();
            }
            public List<lm.Comol.Core.Wizard.NavigableWizardItem<dtoEvaluationStep>> GetAvailableSteps(BaseForPaper call, WizardEvaluationStep current)
            {
                List<lm.Comol.Core.Wizard.NavigableWizardItem<dtoEvaluationStep>> items = new List<lm.Comol.Core.Wizard.NavigableWizardItem<dtoEvaluationStep>>();
                WizardEvaluationStep startStep = WizardEvaluationStep.GeneralSettings; // (CallHasEvaluation(call) ? WizardEvaluationStep.GeneralSettings : WizardEvaluationStep.GeneralInfo);
                if (current == WizardEvaluationStep.none)
                    current = startStep;

                dtoEvaluationCommitteeStep step = GetCommitteeStepInfo(call, startStep);
                items.Add(new lm.Comol.Core.Wizard.NavigableWizardItem<dtoEvaluationStep>()
                {
                    DisplayOrderDetail = Core.Wizard.DisplayOrderEnum.first,
                    Id = step,
                    Status = step.Status,
                    AutoPostBack = current != startStep,
                    Active = (current == startStep)
                });

                List<lm.Comol.Core.Wizard.NavigableWizardItem<dtoEvaluationStep>> subItems = GetStepsForCall(call, current, startStep);
                
                if (step.CommitteesCriteriaCount.Where(c=>c.Value==0).Any()){
                    foreach (lm.Comol.Core.Wizard.NavigableWizardItem<dtoEvaluationStep> item in subItems.Where(s => s.Id.Type == WizardEvaluationStep.AssignSubmission || s.Id.Type == WizardEvaluationStep.MultipleAssignSubmission || s.Id.Type == WizardEvaluationStep.AssignSubmissionWithNoEvaluation))
                    {
                        item.Status = Core.Wizard.WizardItemStatus.disabled;
                    }
                }
                else if (step.Errors.Contains(EditingErrors.CommitteeDssSettings))
                {
                    foreach (lm.Comol.Core.Wizard.NavigableWizardItem<dtoEvaluationStep> item in subItems.Where(s => s.Id.Type == WizardEvaluationStep.AssignSubmission || s.Id.Type == WizardEvaluationStep.MultipleAssignSubmission || s.Id.Type == WizardEvaluationStep.AssignSubmissionWithNoEvaluation))
                    {
                        item.Status = Core.Wizard.WizardItemStatus.disabled;
                    }
                }

                items.AddRange(subItems);
                return items;
            }
            private List<lm.Comol.Core.Wizard.NavigableWizardItem<dtoEvaluationStep>> GetStepsForCall(BaseForPaper call, WizardEvaluationStep current, WizardEvaluationStep startStep)
            {
                List<lm.Comol.Core.Wizard.NavigableWizardItem<dtoEvaluationStep>> items = new List<lm.Comol.Core.Wizard.NavigableWizardItem<dtoEvaluationStep>>();
                Int32 count = GetCommitteesCount(call);
                dtoEvaluationStep mStep = GetStepInfo(call, (CallHasEvaluationStarted(call) ? WizardEvaluationStep.ManageEvaluators : WizardEvaluationStep.FullManageEvaluators));
                items.Add(new lm.Comol.Core.Wizard.NavigableWizardItem<dtoEvaluationStep>()
                {
                    Id = mStep,
                    Status = mStep.Status ,
                    AutoPostBack = true,
                    Active = (current == mStep.Type)
                });

                dtoEvaluationStep mEvaluationsStep = null;
                if (CallHasEvaluation(call))
                    mEvaluationsStep = GetStepInfo(call, WizardEvaluationStep.ManageEvaluations);

                if (mStep.Type == WizardEvaluationStep.FullManageEvaluators)
                {
                    WizardEvaluationStep assignStep = (count > 1) ? WizardEvaluationStep.MultipleAssignSubmission : WizardEvaluationStep.AssignSubmission;
                    dtoEvaluationStep aStep = GetStepInfo(call, assignStep);
                    items.Add(new lm.Comol.Core.Wizard.NavigableWizardItem<dtoEvaluationStep>()
                    {
                        Id = aStep,
                        Status = aStep.Status,
                        AutoPostBack = true,
                        Active = (current == assignStep),
                        DisplayOrderDetail = (mEvaluationsStep == null) ? Core.Wizard.DisplayOrderEnum.last : Core.Wizard.DisplayOrderEnum.none
                    });
                    AddManageEvaluationsStep(current, items, mEvaluationsStep);
                    if (mEvaluationsStep !=null)
                        items.Last().DisplayOrderDetail = Core.Wizard.DisplayOrderEnum.last;
                }
                else
                {
                    if (HasSubmissionsNotInEvaluation(call.Id, SubmissionStatus.accepted))
                        AddAssignSubmissionWithNoEvaluationtStep(current, items,GetStepInfo(call, WizardEvaluationStep.AssignSubmissionWithNoEvaluation));

                    AddManageEvaluationsStep(current, items, mEvaluationsStep);
                    items.Last().DisplayOrderDetail = Core.Wizard.DisplayOrderEnum.last;
                }
                return items;
            }
            private void AddManageEvaluationsStep(WizardEvaluationStep current, List<lm.Comol.Core.Wizard.NavigableWizardItem<dtoEvaluationStep>> items, dtoEvaluationStep mEvaluationsStep)
            {
                if (mEvaluationsStep != null && items !=null )
                {
                    items.Add(new lm.Comol.Core.Wizard.NavigableWizardItem<dtoEvaluationStep>()
                    {
                        Id = mEvaluationsStep,
                        Status = mEvaluationsStep.Status,
                        AutoPostBack = true,
                        Active = (current == mEvaluationsStep.Type)
                    });
                }
            }
            private void AddAssignSubmissionWithNoEvaluationtStep(WizardEvaluationStep current, List<lm.Comol.Core.Wizard.NavigableWizardItem<dtoEvaluationStep>> items, dtoEvaluationStep aStep)
            {
                if (aStep != null && items != null)
                {
                    items.Add(new lm.Comol.Core.Wizard.NavigableWizardItem<dtoEvaluationStep>()
                    {
                        Id = aStep,
                        Status = aStep.Status,
                        AutoPostBack = true,
                        Active = (current == aStep.Type)
                    });
                }
            }

            private dtoEvaluationStep GetStepInfo(BaseForPaper call, WizardEvaluationStep step)
            {
                //Core.Wizard.WizardItemStatus status = Core.Wizard.WizardItemStatus.none;
                switch (step)
                {
                    //case WizardEvaluationStep.GeneralInfo:
                    case WizardEvaluationStep.GeneralSettings:
                        return GetCommitteeStepInfo(call, step);
                    case WizardEvaluationStep.FullManageEvaluators:
                        if (call.Type == CallForPaperType.CallForBids)
                            return GetFullManageEvaluatorsStepInfo((CallForPaper)call, step);
                        else
                            return new dtoEvaluationEvaluatorsStep(step, Core.Wizard.WizardItemStatus.disabled);
                    case WizardEvaluationStep.ManageEvaluators:
                        if (call.Type == CallForPaperType.CallForBids)
                            return GetInEvaluationStepInfo((CallForPaper)call, step); /*new dtoEvaluationEvaluatorsStep(step, Core.Wizard.WizardItemStatus.none);*/
                        else
                            return new dtoEvaluationEvaluatorsStep(step, Core.Wizard.WizardItemStatus.disabled);
                    case WizardEvaluationStep.AssignSubmission:
                        if (call.Type == CallForPaperType.CallForBids)
                            return GetSingleEvaluationAssignmentStepInfo((CallForPaper)call, step);
                        else
                            return new dtoEvaluationSingleAssignmentStep(step, Core.Wizard.WizardItemStatus.disabled);
                    case WizardEvaluationStep.MultipleAssignSubmission:
                        if (call.Type == CallForPaperType.CallForBids)
                            return GetMultipleEvaluationAssignmentStepInfo((CallForPaper)call, step);
                        else
                            return new dtoEvaluationMultipleAssignmentStep(step, Core.Wizard.WizardItemStatus.disabled);

                    case WizardEvaluationStep.AssignSubmissionWithNoEvaluation:
                        if (call.Type == CallForPaperType.CallForBids)
                            return GetAssignSubmissionWithNoEvaluationtStepInfo((CallForPaper)call, step);
                        else
                            return new dtoAssignSubmissionWithNoEvaluationtStep(step, Core.Wizard.WizardItemStatus.disabled);
                    case WizardEvaluationStep.ManageEvaluations:
                        if (call.Type == CallForPaperType.CallForBids)
                            return GetManageEvalutationsStepInfo((CallForPaper)call, step);
                        else
                            return new dtoEvaluationsManageStep(step, Core.Wizard.WizardItemStatus.disabled);
                }
                return new dtoEvaluationStep();
            }

            //public List<lm.Comol.Core.Wizard.NavigableWizardItem<WizardEvaluationStep>> GetAvailableSteps(BaseForPaper call, WizardEvaluationStep current)
            //{
            //    List<lm.Comol.Core.Wizard.NavigableWizardItem<WizardEvaluationStep>> items = new List<lm.Comol.Core.Wizard.NavigableWizardItem<WizardEvaluationStep>>();
            //    WizardEvaluationStep startStep = (CallHasEvaluation(call) ? WizardEvaluationStep.GeneralSettings : WizardEvaluationStep.GeneralInfo);
            //    if (current == WizardEvaluationStep.none)
            //        current = startStep;
            //    items.Add(new lm.Comol.Core.Wizard.NavigableWizardItem<WizardEvaluationStep>()
            //    {
            //        DisplayOrderDetail = Core.Wizard.DisplayOrderEnum.first,
            //        Id = startStep,
            //        Status = GetStepStatus(call, startStep),
            //        AutoPostBack = current != startStep,
            //        Active = (current == startStep)
            //    });

            //    items.AddRange(GetStepsForCall(call, current, startStep));
            //    return items;
            //}
            //private List<lm.Comol.Core.Wizard.NavigableWizardItem<WizardEvaluationStep>> GetStepsForCall(BaseForPaper call, WizardEvaluationStep current, WizardEvaluationStep startStep)
            //{
            //    List<lm.Comol.Core.Wizard.NavigableWizardItem<WizardEvaluationStep>> items = new List<lm.Comol.Core.Wizard.NavigableWizardItem<WizardEvaluationStep>>();
            //    //if (startStep == WizardEvaluationStep.GeneralSettings)
            //    //{
            //    //    items.Add(new lm.Comol.Core.Wizard.NavigableWizardItem<WizardEvaluationStep>()
            //    //    {
            //    //        Id = WizardEvaluationStep.FullManageEvaluators,
            //    //        Status = Core.Wizard.WizardItemStatus.disabled,
            //    //        AutoPostBack = true,
            //    //        Active = false
            //    //    });
            //    //    items.Add(new lm.Comol.Core.Wizard.NavigableWizardItem<WizardEvaluationStep>()
            //    //    {
            //    //        Id = WizardEvaluationStep.AssignSubmission,
            //    //        Status = Core.Wizard.WizardItemStatus.disabled,
            //    //        AutoPostBack = true,
            //    //        Active = false,
            //    //        DisplayOrderDetail = Core.Wizard.DisplayOrderEnum.last
            //    //    });
            //    //}
            //    //else
            //    //{
            //    Int32 count = GetCommitteesCount(call);
            //    WizardEvaluationStep manageStep = (CallHasEvaluation(call) ? WizardEvaluationStep.ManageEvaluators : WizardEvaluationStep.FullManageEvaluators);
            //    items.Add(new lm.Comol.Core.Wizard.NavigableWizardItem<WizardEvaluationStep>()
            //    {
            //        Id = manageStep,
            //        Status = GetStepStatus(call, manageStep),
            //        AutoPostBack = true,
            //        Active = (current == manageStep)
            //    });
            //    WizardEvaluationStep assignStep = (count > 1) ? WizardEvaluationStep.MultipleAssignSubmission : WizardEvaluationStep.AssignSubmission;
            //    items.Add(new lm.Comol.Core.Wizard.NavigableWizardItem<WizardEvaluationStep>()
            //    {
            //        Id = assignStep,
            //        Status = GetStepStatus(call, assignStep),
            //        AutoPostBack = true,
            //        Active = (current == assignStep),
            //        DisplayOrderDetail = Core.Wizard.DisplayOrderEnum.last
            //    });
            //    //}
            //    return items;
            //}

            //private Core.Wizard.WizardItemStatus GetStepStatus(BaseForPaper call, WizardEvaluationStep step)
            //{
            //    Core.Wizard.WizardItemStatus status = Core.Wizard.WizardItemStatus.none;
            //    switch (step)
            //    {
            //        case WizardEvaluationStep.GeneralInfo:
            //        case WizardEvaluationStep.GeneralSettings:
            //            return GetCommitteeStatus(call, step);
            //        case WizardEvaluationStep.FullManageEvaluators:
            //            if (call.Type == CallForPaperType.CallForBids)
            //                return FullManageEvaluatorsStatus((CallForPaper)call, step);
            //            else
            //                return Core.Wizard.WizardItemStatus.disabled;
            //        case WizardEvaluationStep.ManageEvaluators:
            //            if (call.Type == CallForPaperType.CallForBids)
            //                return Core.Wizard.WizardItemStatus.none;
            //            else
            //                return Core.Wizard.WizardItemStatus.disabled;
            //        case WizardEvaluationStep.AssignSubmission:
            //            if (call.Type == CallForPaperType.CallForBids)
            //                return GetSingleEvaluationAssignmentStatus((CallForPaper)call, step);
            //            else
            //                return Core.Wizard.WizardItemStatus.disabled;
            //        case WizardEvaluationStep.MultipleAssignSubmission:
            //            if (call.Type == CallForPaperType.CallForBids)
            //                return GetMultipleEvaluationAssignmentStatus((CallForPaper)call, step);
            //            else
            //                return Core.Wizard.WizardItemStatus.disabled;
            //    }
            //    return status;
            //}

            protected dtoEvaluationCommitteeStep GetCommitteeStepInfo(BaseForPaper call, WizardEvaluationStep step)
            {
                dtoEvaluationCommitteeStep s = new dtoEvaluationCommitteeStep(step);
                List<EvaluationCommittee> committees = (from c in Manager.GetIQ<EvaluationCommittee>() 
                             where c.Call == call && c.Deleted == BaseStatusDeleted.None select c).ToList();
                s.ItemsCount = committees.Count;
                foreach (EvaluationCommittee c in committees) {
                    s.CommitteesCriteriaCount.Add(c.Id, c.Criteria.Any() ? c.Criteria.Count : 0);
                }
                s.CriteriaCount = s.CommitteesCriteriaCount.Select(c => c.Value).Sum();
                s.Errors = GetCommitteeErrors(call,committees);
                if (s.Errors.Count == 0)
                    s.Status= Core.Wizard.WizardItemStatus.valid;
                else if (s.Errors.Contains(EditingErrors.NoCommitteeAvailable))
                    s.Status= Core.Wizard.WizardItemStatus.error;
                else if (s.Errors.Count == 1 && s.Errors.Contains(EditingErrors.None))
                    s.Status= Core.Wizard.WizardItemStatus.valid;
                else
                    s.Status = (s.Errors.Any()) ? Core.Wizard.WizardItemStatus.warning : Core.Wizard.WizardItemStatus.none;
                return s;
            }
            protected dtoEvaluationEvaluatorsStep GetFullManageEvaluatorsStepInfo(CallForPaper call, WizardEvaluationStep step)
            {
                dtoEvaluationEvaluatorsStep s = new dtoEvaluationEvaluatorsStep(step);
                s.ItemsCount = (from e in Manager.GetIQ<CallEvaluator>() where e.Call.Id == call.Id && e.Deleted == BaseStatusDeleted.None select e.Id).Count();
                List<EditingErrors> errors = GetFullManageEvaluatorsErrors(call);
                s.Errors = errors;
                if (errors.Contains(EditingErrors.NoCommitteeAvailable))
                    s.Status = Core.Wizard.WizardItemStatus.disabled;
                else if (errors.Count == 1 && errors.Contains(EditingErrors.None))
                    s.Status = Core.Wizard.WizardItemStatus.valid;
                else
                    s.Status = (errors.Any()) ? Core.Wizard.WizardItemStatus.warning : Core.Wizard.WizardItemStatus.none;
                return s;
            }
            protected dtoEvaluationViewEvaluatorsStep GetInEvaluationStepInfo(CallForPaper call, WizardEvaluationStep step)
            {
                dtoEvaluationViewEvaluatorsStep s = new dtoEvaluationViewEvaluatorsStep(step);
                s.Counters = GetMembershipsStatistics(call);
                s.CommitteesCount = (from e in Manager.GetIQ<EvaluationCommittee>() where e.Call.Id == call.Id && e.Deleted == BaseStatusDeleted.None select e.Id).Count();
                s.EvaluatorsCount = (from e in Manager.GetIQ<CallEvaluator>() where e.Call.Id == call.Id && e.Deleted == BaseStatusDeleted.None select e.Id).Count();
                List<EditingErrors> errors = GetInEvaluationEvaluatorsErrors(call);
                s.Errors = errors;
                if (errors.Contains(EditingErrors.NoCommitteeAvailable))
                    s.Status = Core.Wizard.WizardItemStatus.disabled;
                else if (errors.Count == 1 && errors.Contains(EditingErrors.None))
                    s.Status = Core.Wizard.WizardItemStatus.valid;
                else
                    s.Status = (errors.Any()) ? Core.Wizard.WizardItemStatus.warning : Core.Wizard.WizardItemStatus.none;
                return s;
            }
            protected dtoEvaluationSingleAssignmentStep GetSingleEvaluationAssignmentStepInfo(CallForPaper call, WizardEvaluationStep step)
            {
                dtoEvaluationSingleAssignmentStep s = new dtoEvaluationSingleAssignmentStep(step);
                var query = (from u in Manager.GetIQ<UserSubmission>() where u.Deleted == BaseStatusDeleted.None && u.Call.Id == call.Id select u);

                s.ItemsCount = query.Where(u=> u.Status >= SubmissionStatus.submitted).Select(u=> u.Id).Count();
                s.RejectedCount = query.Where(u => u.Status >= SubmissionStatus.rejected).Select(u => u.Id).Count();
                s.AcceptedCount = s.ItemsCount - s.RejectedCount;
                
                List<EditingErrors> errors = GetSingleEvaluationAssignmentErrors(call);
                s.Errors = errors;
                if (errors.Contains(EditingErrors.NoCommitteeAvailable))
                    s.Status = Core.Wizard.WizardItemStatus.disabled;
                else if (errors.Count == 1 && errors.Contains(EditingErrors.None))
                    s.Status = Core.Wizard.WizardItemStatus.valid;
                else if (errors.Contains(EditingErrors.NoEvaluatorsForAssignments) || errors.Contains(EditingErrors.NoSubmissionToEvaluate))
                    s.Status = Core.Wizard.WizardItemStatus.error;
                else
                    s.Status = (errors.Any()) ? Core.Wizard.WizardItemStatus.warning : Core.Wizard.WizardItemStatus.none;
                return s;
            }
            protected dtoEvaluationMultipleAssignmentStep GetMultipleEvaluationAssignmentStepInfo(CallForPaper call, WizardEvaluationStep step)
            {
                dtoEvaluationMultipleAssignmentStep s = new dtoEvaluationMultipleAssignmentStep(step);
                var query = (from u in Manager.GetIQ<UserSubmission>() where u.Deleted == BaseStatusDeleted.None && u.Call.Id == call.Id select u);

                s.ItemsCount = query.Where(u => u.Status >= SubmissionStatus.submitted).Select(u => u.Id).Count();
                s.RejectedCount = query.Where(u => u.Status >= SubmissionStatus.rejected).Select(u => u.Id).Count();
                s.AcceptedCount = s.ItemsCount - s.RejectedCount;
                List<EditingErrors> errors = GetMultipleEvaluationAssignmentErrors(call);
                s.Errors = errors;
                if (errors.Contains(EditingErrors.NoCommitteeAvailable))
                    s.Status = Core.Wizard.WizardItemStatus.disabled;
                else if (errors.Count == 1 && errors.Contains(EditingErrors.None))
                    s.Status = Core.Wizard.WizardItemStatus.valid;
                else if (errors.Contains(EditingErrors.NoEvaluatorsForAssignments) || errors.Contains(EditingErrors.NoSubmissionToEvaluate))
                    s.Status = Core.Wizard.WizardItemStatus.error;
                else
                    s.Status =(errors.Any()) ? Core.Wizard.WizardItemStatus.warning : Core.Wizard.WizardItemStatus.none;
                return s;
            }
            protected dtoAssignSubmissionWithNoEvaluationtStep GetAssignSubmissionWithNoEvaluationtStepInfo(CallForPaper call, WizardEvaluationStep step)
            {
                dtoAssignSubmissionWithNoEvaluationtStep s = new dtoAssignSubmissionWithNoEvaluationtStep(step);
                List<long> idInEvaluations = (from u in Manager.GetIQ<Evaluation>() where u.Deleted == BaseStatusDeleted.None && u.Call.Id == call.Id select u.Submission.Id).Distinct().ToList();
                var query = (from u in Manager.GetIQ<UserSubmission>() where u.Deleted == BaseStatusDeleted.None && u.Call.Id == call.Id select u);

                s.InEvaluations = idInEvaluations.Count;
                if (idInEvaluations.Count <= maxItemsForQuery)
                {
                    s.DraftItems = query.Where(u => u.Status == SubmissionStatus.draft && !idInEvaluations.Contains(u.Id)).Select(u => u.Id).Count();
                    s.SubmittedCount = query.Where(u => u.Status == SubmissionStatus.submitted && !idInEvaluations.Contains(u.Id)).Select(u => u.Id).Count();
                    s.RejectedCount = query.Where(u => u.Status >= SubmissionStatus.rejected && !idInEvaluations.Contains(u.Id)).Select(u => u.Id).Count();
                    s.AcceptedCount = query.Where(u => u.Status >= SubmissionStatus.accepted && !idInEvaluations.Contains(u.Id)).Select(u => u.Id).Count();
                }
                else {
                    s.DraftItems = query.Where(u => u.Status == SubmissionStatus.draft).Select(u => u.Id).ToList().Where(u => !idInEvaluations.Contains(u)).Count();
                    s.SubmittedCount = query.Where(u => u.Status == SubmissionStatus.submitted).Select(u => u.Id).ToList().Where(u => !idInEvaluations.Contains(u)).Count();
                    s.RejectedCount = query.Where(u => u.Status >= SubmissionStatus.rejected).Select(u => u.Id).ToList().Where(u => !idInEvaluations.Contains(u)).Count();
                    s.AcceptedCount = query.Where(u => u.Status >= SubmissionStatus.accepted).Select(u => u.Id).ToList().Where(u => !idInEvaluations.Contains(u)).Count();
                }
                s.Status = (s.AcceptedCount>0) ? Core.Wizard.WizardItemStatus.warning : Core.Wizard.WizardItemStatus.none;
                return s;
            }

            protected dtoEvaluationsManageStep GetManageEvalutationsStepInfo(CallForPaper call, WizardEvaluationStep step)
            {
                dtoEvaluationsManageStep s = new dtoEvaluationsManageStep(step);
                var query = (from u in Manager.GetIQ<Evaluation>() where u.Deleted == BaseStatusDeleted.None && u.Call.Id == call.Id && u.Status != EvaluationStatus.EvaluatorReplacement && u.Status!= EvaluationStatus.Invalidated  select u);
                s.ItemsCount = query.Count();
                s.EvaluatingCount = query.Where(u => u.Status == EvaluationStatus.Evaluating).Count();
                s.EvaluatedCount = query.Where(u => u.Status == EvaluationStatus.Evaluated).Count();
                s.NotStartedCount = query.Where(u => u.Status == EvaluationStatus.None).Count();
                s.Status = (s.EvaluatingCount == 0 && s.EvaluatedCount == 0) ? Core.Wizard.WizardItemStatus.disabled : Core.Wizard.WizardItemStatus.none;
                return s;
            }

            protected List<EditingErrors> GetCommitteeErrors(BaseForPaper call, List<EvaluationCommittee> items)
            {
                List<EditingErrors> errors = new List<EditingErrors>();
                Boolean multipleCommittees = (items != null && items.Count > 1);
                if (items.Count() == 0 && !isNewCommittee(call))
                    errors.Add(EditingErrors.NoCommitteeAvailable);
                else if (items.Count() >= 0)
                {
                    Core.Wizard.WizardItemStatus status = (!items.Any(c => !c.ForAllSubmittersType) || !items.Where(c => !c.ForAllSubmittersType && c.AssignedTypes.Where(at => at.Deleted == BaseStatusDeleted.None).Count() == 0).Any()) ? Core.Wizard.WizardItemStatus.valid : Core.Wizard.WizardItemStatus.warning;
                    if (status != Core.Wizard.WizardItemStatus.valid)
                        errors.Add(EditingErrors.NoSubmitterTypeAssignments);
                    if (status == Core.Wizard.WizardItemStatus.valid && items.Any(c => !c.ForAllSubmittersType))
                    {
                        List<long> submitters = (from s in Manager.GetIQ<SubmitterType>()
                                                 where s.Call == call && s.Deleted == BaseStatusDeleted.None
                                                 select s.Id).ToList();

                        List<long> usedTypes = new List<long>();
                        items.Where(c => !c.ForAllSubmittersType && c.AssignedTypes != null && c.AssignedTypes.Count > 0).ToList().ForEach(c => usedTypes.AddRange(c.AssignedTypes.Select(a => a.SubmitterType.Id).ToList()));
                        if (submitters.Any(s => !usedTypes.Contains(s)) && !items.Any(i=> i.ForAllSubmittersType))
                        {
                            status = Core.Wizard.WizardItemStatus.warning;
                            errors.Add(EditingErrors.SubmittersTypeNotAssigned);
                        }
                    }
                    if (status == Core.Wizard.WizardItemStatus.valid && items.Any(c => !c.Criteria.Any() || !c.Criteria.Any(cr => cr.Deleted == BaseStatusDeleted.None)))
                        errors.Add(EditingErrors.NoCriteria);

                    if (items.Any(i => i.UseDss) && (items.Count > 1 && (call.IdDssMethod < 1 || (call.UseManualWeights && !call.IsValidFuzzyMeWeights) || (call.IdDssRatingSet < 1 && !call.IsValidFuzzyMeWeights)) || items.Any(c => c.HasDssErrors(multipleCommittees))))
                    {
                        errors.Add(EditingErrors.CommitteeDssSettings);
                        if (status == Core.Wizard.WizardItemStatus.valid || status == Core.Wizard.WizardItemStatus.none)
                            status = Core.Wizard.WizardItemStatus.warning;
                    }
                    if (status == Core.Wizard.WizardItemStatus.valid)
                        errors.Add(EditingErrors.None);
                }
                return errors.Distinct().ToList();
            }
            protected List<EditingErrors> GetFullManageEvaluatorsErrors(CallForPaper call)
            {
                List<EditingErrors> errors = new List<EditingErrors>();
                var items = (from c in Manager.GetIQ<EvaluationCommittee>() where c.Call == call && c.Deleted == BaseStatusDeleted.None select c).ToList();
                if (items.Count() == 0)
                    errors.Add(EditingErrors.NoCommitteeAvailable);
                else
                {
                    List<CallEvaluator> evaluators = (from e in Manager.GetIQ<CallEvaluator>() where e.Call.Id == call.Id && e.Deleted == BaseStatusDeleted.None select e).ToList();
                    if (evaluators.Count == 0)
                        errors.Add(EditingErrors.NoEvaluators);
                    else if (items.Count == 1 && !items[0].Members.Where(m => m.Deleted == BaseStatusDeleted.None).Any())
                        errors.Add(EditingErrors.UnassingedEvaluators);
                    else
                    {
                        List<long> uEvaluators = new List<long>();
                        foreach (EvaluationCommittee c in items)
                        {
                            List<long> members = c.Members.Where(m => m.Deleted == BaseStatusDeleted.None).Select(m => m.Evaluator.Id).ToList();
                            if (!members.Any())
                                errors.Add(EditingErrors.CommitteeWithNoEvaluators);
                            else if (call.OneCommitteeMembership && uEvaluators.Where(u => members.Contains(u)).Any())
                                errors.Add(EditingErrors.MoreEvaluatorAssignment);
                            uEvaluators.AddRange(members);
                        }
                        uEvaluators = uEvaluators.Distinct().ToList();
                        if (!call.OneCommitteeMembership && (!uEvaluators.Any() || evaluators.Where(e => !uEvaluators.Contains(e.Id)).Any()))
                            errors.Add(EditingErrors.UnassingedEvaluators);
                        else if (!uEvaluators.Where(e => !evaluators.Select(ev => ev.Id).ToList().Contains(e)).Any() && uEvaluators.Any())
                            errors.Add(EditingErrors.None);
                    }
                }
                return errors.Distinct().ToList();
            }
            protected List<EditingErrors> GetInEvaluationEvaluatorsErrors(CallForPaper call)
            {
                List<EditingErrors> errors = new List<EditingErrors>();
                var items = (from c in Manager.GetIQ<EvaluationCommittee>() where c.Call == call && c.Deleted == BaseStatusDeleted.None select c).ToList();
                if (items.Count() == 0)
                    errors.Add(EditingErrors.NoCommitteeAvailable);
                else
                {
                    List<CallEvaluator> evaluators = (from e in Manager.GetIQ<CallEvaluator>() where e.Call.Id == call.Id && e.Deleted == BaseStatusDeleted.None select e).ToList();
                    if (evaluators.Count == 0)
                        errors.Add(EditingErrors.NoEvaluators);
                    else if (items.Count == 1 && !items[0].Members.Where(m => m.Deleted == BaseStatusDeleted.None).Any())
                        errors.Add(EditingErrors.UnassingedEvaluators);
                    else
                    {
                        List<long> uEvaluators = new List<long>();
                        foreach (EvaluationCommittee c in items)
                        {
                            var members = c.Members.Where(m => m.Deleted == BaseStatusDeleted.None && m.Status != MembershipStatus.Replaced).Select(m => new { IdEvaluator = m.Evaluator.Id, Status = m.Status }).ToList();
                            if (!members.Any())
                                errors.Add(EditingErrors.CommitteeWithNoEvaluators);
                            else if (call.OneCommitteeMembership && uEvaluators.Where(u => members.Where(m=> m.Status == MembershipStatus.Standard && m.IdEvaluator== u).Any()).Any())
                                errors.Add(EditingErrors.MoreEvaluatorAssignment);
                            uEvaluators.AddRange(members.Where(m=>m.Status != MembershipStatus.Replacing).Select(m=>m.IdEvaluator).ToList());
                        }
                        uEvaluators = uEvaluators.Distinct().ToList();
                        if (!call.OneCommitteeMembership && (!uEvaluators.Any() || evaluators.Where(e => !uEvaluators.Contains(e.Id)).Any()))
                            errors.Add(EditingErrors.UnassingedEvaluators);
                        else if (!uEvaluators.Where(e => !evaluators.Select(ev => ev.Id).ToList().Contains(e)).Any() && uEvaluators.Any())
                            errors.Add(EditingErrors.None);
                    }
                }
                return errors.Distinct().ToList();
            }
            protected List<EditingErrors> GetSingleEvaluationAssignmentErrors(CallForPaper call)
            {
                List<EditingErrors> errors = new List<EditingErrors>();
                var items = (from c in Manager.GetIQ<EvaluationCommittee>() where c.Call == call && c.Deleted == BaseStatusDeleted.None select c).ToList();
                if (items.Count() == 0)
                    errors.Add(EditingErrors.NoCommitteeAvailable);
                else if (items.Count > 0)
                    return GetMultipleEvaluationAssignmentErrors(call);
                else
                    errors.AddRange(GetEvaluationAssignmentErrors(items[0], call));
                return errors.Distinct().ToList();
            }
            protected List<EditingErrors> GetMultipleEvaluationAssignmentErrors(CallForPaper call)
            {
                List<EditingErrors> errors = new List<EditingErrors>();
                var items = (from c in Manager.GetIQ<EvaluationCommittee>() where c.Call == call && c.Deleted == BaseStatusDeleted.None select c).ToList();
                if (items.Count() == 0)
                    errors.Add(EditingErrors.NoCommitteeAvailable);
                else
                    items.ForEach(i => errors.AddRange(GetEvaluationAssignmentErrors(i, call)));
                return errors.Distinct().ToList();
            }
            protected List<EditingErrors> GetEvaluationAssignmentErrors(EvaluationCommittee comittee, CallForPaper call)
            {
                List<EditingErrors> errors = new List<EditingErrors>();
                List<CallEvaluator> evaluators = comittee.Members.Where(m => m.Deleted == BaseStatusDeleted.None).Select(m => m.Evaluator).ToList();

                if (evaluators.Count == 0)
                    errors.Add(EditingErrors.NoEvaluatorsForAssignments);
                else
                {
                    List<long> aTypes = (comittee.AssignedTypes.Any() ? comittee.AssignedTypes.Where(a => a.Deleted == BaseStatusDeleted.None).Select(a => a.SubmitterType.Id).ToList() : new List<long>());
                    var qSubmission = (from s in Manager.GetIQ<UserSubmission>() where s.Deleted == BaseStatusDeleted.None && s.Call.Id == call.Id && s.Status >= SubmissionStatus.accepted && s.Status != SubmissionStatus.rejected && (comittee.ForAllSubmittersType || aTypes.Contains(s.Type.Id)) select s);
                    if (!qSubmission.Any())
                        errors.Add(EditingErrors.NoSubmissionToEvaluate);
                    else
                    {
                        List<long> submissions = qSubmission.Select(s => s.Id).ToList();
                        List<Evaluation> evaluations = (from e in Manager.GetIQ<Evaluation>() where e.Call.Id == call.Id && e.Committee.Id == comittee.Id && e.Deleted == BaseStatusDeleted.None select e).ToList();
                        if (submissions.Where(id=> !evaluations.Where(e=> e.Submission.Id==id).Any()).Any() ||
                            evaluations.Where(e=> !submissions.Contains(e.Submission.Id)).Any())
                            errors.Add(EditingErrors.SubmissionToAssign);
                        else
                            errors.Add(EditingErrors.None);
                    }
                    //if (comittee.ForAllSubmittersType && (from u in Manager.Get<
                    //List<long> uEvaluators = new List<long>();
                    //foreach (EvaluationCommittee c in items)
                    //{
                    //    List<long> members = c.Members.Where(m => m.Deleted == BaseStatusDeleted.None).Select(m => m.Evaluator.Id).ToList();
                    //    if (call.OneCommitteeMembership && uEvaluators.Where(u => members.Contains(u)).Any())
                    //        errors.Add(EditingErrors.MoreEvaluatorAssignment);
                    //    uEvaluators.AddRange(members);
                    //}
                    //uEvaluators = uEvaluators.Distinct().ToList();
                    //if (!call.OneCommitteeMembership && (!uEvaluators.Any() || evaluators.Where(e => !uEvaluators.Contains(e.Id)).Any()))
                    //    errors.Add(EditingErrors.UnassingedEvaluators);
                    //else if (!uEvaluators.Where(e => !evaluators.Select(ev => ev.Id).ToList().Contains(e)).Any() && uEvaluators.Any())
                    //    errors.Add(EditingErrors.None);
                }

                return errors.Distinct().ToList();
            }
        #endregion

        #region "Editing"
            #region "1 - Editing committee"
                #region "1.0 - DSS methods"
                    public CallForPaper GetCallForCommiteeSettings(long idCall)
                    {
                        CallForPaper call = null;
                        try
                        {
                            call = Manager.Get<CallForPaper>(idCall);
                            if (call != null)
                                Manager.Refresh(call);
                        }
                        catch { };
                        return call;
                    }
                #endregion
                #region "1.1 - Editing committee"
                    public EvaluationCommittee AddCommitteeToCall(long idCall, List<dtoCommittee> committees, String name, String description, Boolean useDss, lm.Comol.Core.Dss.Domain.Templates.dtoItemMethodSettings settings)
                    {
                        BaseForPaper call = Manager.Get<BaseForPaper>(idCall);
                        if (call != null)
                        {
                            if (useDss && committees.Count == 1)
                            {
                                call.IdDssMethod = committees[0].MethodSettings.IdMethod;
                                call.IdDssRatingSet = committees[0].MethodSettings.IdRatingSet;
                                call.IsDssMethodFuzzy = committees[0].MethodSettings.IsFuzzyMethod;
                                call.UseManualWeights = committees[0].MethodSettings.UseManualWeights;
                                call.UseOrderedWeights = committees[0].MethodSettings.UseOrderedWeights;
                                call.FuzzyMeWeights = "";
                                Manager.SaveOrUpdate(call);
                                settings = new Core.Dss.Domain.Templates.dtoItemMethodSettings();
                                settings.IsDefaultForChildren = true;
                                settings.IdMethod = call.IdDssMethod;
                                settings.IdRatingSet = call.IdDssRatingSet;
                                settings.IsFuzzyMethod = call.IsDssMethodFuzzy;
                                settings.UseManualWeights = call.UseManualWeights;
                                settings.UseOrderedWeights = call.UseOrderedWeights;
                                committees[0].MethodSettings.InheritsFromFather = true;
                            }
                            SaveCommittees(call, committees, settings);
                        }
                        return AddCommittee(call, name, description, useDss, settings);
                    }
                    public EvaluationCommittee AddCommittee(long idCall, String name, String description, Boolean useDss, lm.Comol.Core.Dss.Domain.Templates.dtoItemMethodSettings settings)
                    {
                        BaseForPaper call = Manager.Get<BaseForPaper>(idCall);
                        return AddCommittee(call, name, description, useDss, settings);
                    }
                    public EvaluationCommittee AddFirstCommittee(BaseForPaper call, String name, String description, Boolean useDss)
                    {
                        EvaluationCommittee committee = null;
                        try
                        {
                            Manager.BeginTransaction();
                            litePerson person = Manager.GetLitePerson(UC.CurrentUserID);
                            if (call != null && person != null)
                            {
                                committee = new EvaluationCommittee();
                                committee.CreateMetaInfo(person, UC.IpAddress, UC.ProxyIpAddress);
                                committee.DisplayOrder = (from s in Manager.GetIQ<EvaluationCommittee>() where s.Call.Id == call.Id select s.DisplayOrder).Max() + 1;
                                committee.Name = name + committee.DisplayOrder.ToString();
                                committee.Description = description;
                                committee.UseDss = useDss;
                                if (useDss)
                                {
                                    committee.MethodSettings = new ItemMethodSettings();
                                    committee.WeightSettings = new ItemWeightSettings();
                                    committee.MethodSettings.InheritsFromFather = false;

                                    committee.WeightSettings.IdRatingValue = -1;
                                    committee.WeightSettings.IdRatingValueEnd = -1;
                                    committee.WeightSettings.Weight = 0;
                                    committee.WeightSettings.WeightFuzzy = "";
                                    committee.WeightSettings.IsFuzzyWeight = committee.MethodSettings.IsFuzzyMethod;
                                    if (call.UseManualWeights)
                                    {
                                        call.IsValidFuzzyMeWeights = false;
                                        if (call.UseOrderedWeights)
                                        {
                                            List<String> values = (String.IsNullOrWhiteSpace(call.FuzzyMeWeights) ? new List<String>() : call.FuzzyMeWeights.Split('#').ToList());
                                            if (values.Any())
                                            {
                                                switch (values.Count())
                                                {
                                                    case 0:
                                                    case 1:
                                                        values.Add("");
                                                        break;
                                                    default:
                                                        if (!String.IsNullOrWhiteSpace(values[values.Count - 1]) && values[values.Count - 1].Contains(":"))
                                                        {
                                                            values.Add((values.Count + 1).ToString() + ":" + values[values.Count - 1].Split(':')[1]);
                                                            values[values.Count - 1] = "";
                                                        }
                                                        else
                                                            values.Add((values.Count + 1).ToString() + ":");
                                                        break;
                                                }
                                                call.FuzzyMeWeights = (values.Count == 1 ? values.FirstOrDefault() : String.Join("#", values));
                                            }
                                        }
                                        Manager.SaveOrUpdate(call);
                                    }
                                }
                                committee.Call = call;
                                committee.ForAllSubmittersType = true;
                                Manager.SaveOrUpdate(committee);
                            }
                            Manager.Commit();
                        }
                        catch (Exception ex)
                        {
                            committee = null;
                            Manager.RollBack();
                        }
                        return committee;
                    }
                    private EvaluationCommittee AddCommittee(BaseForPaper call, String name, String description, Boolean useDss, lm.Comol.Core.Dss.Domain.Templates.dtoItemMethodSettings settings)
                    {
                        EvaluationCommittee committee = null;
                        try
                        {
                            Manager.BeginTransaction();
                            litePerson person = Manager.GetLitePerson(UC.CurrentUserID);
                            if (call != null && person != null)
                            {
                                committee = new EvaluationCommittee();
                                committee.CreateMetaInfo(person, UC.IpAddress, UC.ProxyIpAddress);
                                committee.DisplayOrder = (from s in Manager.GetIQ<EvaluationCommittee>() where s.Call.Id == call.Id select s.DisplayOrder).Max() + 1;
                                committee.Name = name + committee.DisplayOrder.ToString();
                                committee.Description = description;
                                committee.UseDss = useDss;
                                if (useDss)
                                {
                                    committee.MethodSettings = new ItemMethodSettings();
                                    committee.WeightSettings = new ItemWeightSettings();
                                    committee.MethodSettings.InheritsFromFather = true;
                                    if (settings!=null){
                                        committee.MethodSettings.IdMethod = settings.IdMethod;
                                        committee.MethodSettings.IdRatingSet = settings.IdRatingSet;
                                        committee.MethodSettings.IsFuzzyMethod = settings.IsFuzzyMethod;
                                        committee.MethodSettings.UseManualWeights = settings.UseManualWeights;
                                        committee.MethodSettings.UseOrderedWeights = settings.UseOrderedWeights;
                                    }
                                    else
                                    {
                                        committee.MethodSettings.IdMethod = -1;
                                        committee.MethodSettings.IdRatingSet = -1;
                                    }
                                    committee.WeightSettings.IdRatingValue = -1;
                                    committee.WeightSettings.IdRatingValueEnd = -1;
                                    committee.WeightSettings.Weight = 0;
                                    committee.WeightSettings.WeightFuzzy = "";
                                    committee.WeightSettings.IsFuzzyWeight = committee.MethodSettings.IsFuzzyMethod;
                                    if (call.UseManualWeights)
                                    {
                                        call.IsValidFuzzyMeWeights = false;
                                        if (call.UseOrderedWeights){
                                            List<String> values = (String.IsNullOrWhiteSpace(call.FuzzyMeWeights) ? new List<String>() : call.FuzzyMeWeights.Split('#').ToList());
                                            if (values.Any())
                                            {
                                                switch (values.Count())
                                                {
                                                    case 0:
                                                    case 1:
                                                        values.Add("");
                                                        break;
                                                    default:
                                                        if (!String.IsNullOrWhiteSpace(values[values.Count - 1]) && values[values.Count - 1].Contains(":"))
                                                        {
                                                            values.Add((values.Count +1).ToString() + ":" + values[values.Count - 1].Split(':')[1]);
                                                            values[values.Count - 1] = "";
                                                        }
                                                        else
                                                            values.Add((values.Count +1).ToString() + ":");
                                                        break;
                                                }
                                                call.FuzzyMeWeights = (values.Count == 1 ? values.FirstOrDefault() : String.Join("#", values));
                                            }
                                        }
                                        Manager.SaveOrUpdate(call);
                                    }
                                }
                                committee.Call = call;
                                committee.ForAllSubmittersType = true;
                                Manager.SaveOrUpdate(committee);
                            }
                            Manager.Commit();
                        }
                        catch (Exception ex)
                        {
                            committee = null;
                            Manager.RollBack();
                        }
                        return committee;
                    }


                    public Boolean RemoveDssInheritsFromCommittee(long idCall, IEnumerable<dtoCommittee> committees, lm.Comol.Core.Dss.Domain.Templates.dtoItemMethodSettings settings)
                    {
                        Boolean result = false;
                        try
                        {
                            if (settings != null && committees.Count() == 1 && committees.Any(c => c.MethodSettings.InheritsFromFather))
                            {
                                dtoCommittee item = committees.FirstOrDefault();
                                litePerson person = Manager.GetLitePerson(UC.CurrentUserID);
                                if (person != null)
                                {
                                    Manager.BeginTransaction();
                                    EvaluationCommittee committee = Manager.Get<EvaluationCommittee>(item.Id);
                                    if (committee != null)
                                    {
                                        committee.UpdateMetaInfo(person, UC.IpAddress, UC.ProxyIpAddress);
                                        committee.MethodSettings.IdMethod = settings.IdMethod;
                                        committee.MethodSettings.IdRatingSet = settings.IdRatingSet;
                                        committee.MethodSettings.InheritsFromFather = false;
                                        Manager.SaveOrUpdate(committee);
                                        item.MethodSettings.IdMethod = settings.IdMethod;
                                        item.MethodSettings.IdRatingSet = settings.IdRatingSet;
                                        item.MethodSettings.InheritsFromFather = false;
                                        item.MethodSettings.IsFuzzyMethod = settings.IsFuzzyMethod;
                                        result = true;
                                    }
                                    BaseForPaper bCall = Manager.Get<BaseForPaper>(idCall);
                                    CallForPaper call = Manager.Get<CallForPaper>(idCall);
                                    if (bCall != null && bCall.UseManualWeights)
                                    {
                                        bCall.UseManualWeights = false;
                                        bCall.UseOrderedWeights = false;
                                        bCall.IdDssMethod = 0;
                                        bCall.IdDssRatingSet = 0;
                                        bCall.IsDssMethodFuzzy = false;
                                        bCall.FuzzyMeWeights = "";
                                        bCall.IsValidFuzzyMeWeights = false;
                                        Manager.SaveOrUpdate(bCall);
                                    }
                                    if (call != null && call.UseManualWeights)
                                    {
                                        call.UseManualWeights = false;
                                        call.UseOrderedWeights = false;
                                        call.IdDssMethod = 0;
                                        call.IdDssRatingSet = 0;
                                        call.IsDssMethodFuzzy = false;
                                        call.FuzzyMeWeights = "";
                                        call.IsValidFuzzyMeWeights = false;
                                        Manager.SaveOrUpdate(call);
                                    }
                                    Manager.Commit();
                                    result = true;
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            if (Manager.IsInTransaction())
                                Manager.RollBack();
                        }
                        return result;
                    }
                    public Boolean SaveCommittees(long idCall, List<dtoCommittee> committees, lm.Comol.Core.Dss.Domain.Templates.dtoItemMethodSettings settings)
                    {
                        BaseForPaper call = Manager.Get<BaseForPaper>(idCall);
                        return SaveCommittees(call, committees, settings);
                    }
                    public Boolean SaveCommittees(BaseForPaper call, List<dtoCommittee> committees, lm.Comol.Core.Dss.Domain.Templates.dtoItemMethodSettings settings)
                    {
                        Boolean result = false;
                        try
                        {
                            litePerson person = Manager.GetLitePerson(UC.CurrentUserID);
                            if (call != null && person != null)
                            {
                                Manager.BeginTransaction();
                                if (settings != null)
                                {
                                    CallForPaper cfp = GetCallForCommiteeSettings(call.Id);
                                    if (cfp != null && cfp.EvaluationType == EvaluationType.Dss)
                                    {
                                        if (committees.Count == 1 && settings.IdMethod < 1)
                                        {
                                            settings.IdMethod = committees[0].MethodSettings.IdMethod;
                                            settings.IdRatingSet = committees[0].MethodSettings.IdRatingSet;
                                            settings.IsFuzzyMethod = committees[0].MethodSettings.IsFuzzyMethod;
                                            settings.UseOrderedWeights = committees[0].MethodSettings.UseOrderedWeights;
                                            settings.UseManualWeights = committees[0].MethodSettings.UseManualWeights;
                                            settings.FuzzyMeWeights = committees[0].MethodSettings.FuzzyMeWeights;
                                            settings.Error = committees[0].MethodSettings.Error;
                                        }
                                        cfp.IdDssMethod = settings.IdMethod;
                                        cfp.IdDssRatingSet = settings.IdRatingSet;
                                        cfp.IsDssMethodFuzzy = settings.IsFuzzyMethod;
                                        cfp.UseOrderedWeights = settings.UseOrderedWeights;
                                        cfp.UseManualWeights = settings.UseManualWeights;
                                        cfp.FuzzyMeWeights = settings.FuzzyMeWeights;
                                        cfp.IsValidFuzzyMeWeights = (settings.Error == DssError.None);
                                        cfp.UpdateMetaInfo(person, UC.IpAddress, UC.ProxyIpAddress);
                                        Manager.SaveOrUpdate(cfp);
                                        call.IdDssMethod = settings.IdMethod;
                                        call.IdDssRatingSet = settings.IdRatingSet;
                                        call.IsDssMethodFuzzy = settings.IsFuzzyMethod;
                                        call.UseOrderedWeights = settings.UseOrderedWeights;
                                        call.UseManualWeights = settings.UseManualWeights;
                                        call.FuzzyMeWeights = settings.FuzzyMeWeights;
                                        call.IsValidFuzzyMeWeights = (settings.Error == DssError.None);
                                    }
                                    else
                                        settings = null;
                                }
                                int displayNumber = 1;
                                foreach (dtoCommittee item in committees)
                                {
                                    if (String.IsNullOrEmpty(item.Name))
                                        item.Name = displayNumber.ToString();
                                    EvaluationCommittee committee = Manager.Get<EvaluationCommittee>(item.Id);
                                    if (committee == null)
                                    {
                                        committee = new EvaluationCommittee();
                                        committee.Call = call;
                                        committee.CreateMetaInfo(person, UC.IpAddress, UC.ProxyIpAddress);
                                        committee.DisplayOrder = GetNewCommitteeDisplayOrder(call);
                                        committee.Name = item.Name;
                                        committee.Description = item.Description;
                                    }
                                    else if (committee.Name != item.Name || committee.Description != item.Description)
                                    {
                                        committee.Name = item.Name;
                                        committee.Description = item.Description;
                                        committee.UpdateMetaInfo(person, UC.IpAddress, UC.ProxyIpAddress);
                                    }
                                    if (settings == null)
                                        committee.UseDss = false;
                                    else
                                    {
                                        committee.UseDss = true;
                                        committee.MethodSettings.InheritsFromFather = item.MethodSettings.InheritsFromFather;
                                        if (item.MethodSettings.InheritsFromFather)
                                        {
                                            committee.MethodSettings.IdMethod = settings.IdMethod;
                                            committee.MethodSettings.IdRatingSet = settings.IdRatingSet;
                                            committee.MethodSettings.IsFuzzyMethod = settings.IsFuzzyMethod;
                                            committee.MethodSettings.UseManualWeights = settings.UseManualWeights;
                                            committee.MethodSettings.UseOrderedWeights = settings.UseOrderedWeights;
                                        }
                                        else
                                        {
                                            committee.MethodSettings.IdMethod = item.MethodSettings.IdMethod;
                                            committee.MethodSettings.IdRatingSet = item.MethodSettings.IdRatingSet;
                                            committee.MethodSettings.UseManualWeights = item.MethodSettings.UseManualWeights;
                                            committee.MethodSettings.UseOrderedWeights = item.MethodSettings.UseOrderedWeights;
                                            committee.MethodSettings.IsFuzzyMethod = item.MethodSettings.IsFuzzyMethod;
                                        }
                                        committee.WeightSettings = item.WeightSettings.Copy();
                                        if (committee.MethodSettings.UseManualWeights)
                                            committee.WeightSettings.IsValidFuzzyMeWeights = (!String.IsNullOrWhiteSpace(committee.WeightSettings.FuzzyMeWeights) && item.WeightSettings.IsValidFuzzyMeWeights);
                                    }
                                    committee.DisplayOrder = displayNumber;
                                    Manager.SaveOrUpdate(committee);

                                    if (item.ForAllSubmittersType && committee.Id > 0 && committee.AssignedTypes.Any()) {
                                        committee.AssignedTypes.Where(t => t.Deleted == BaseStatusDeleted.None).ToList().ForEach(t => t.SetDeleteMetaInfo(person, UC.IpAddress, UC.ProxyIpAddress));
                                    }
                                    else if (!item.ForAllSubmittersType) {
                                        if (item.Submitters.Count == 0)
                                            committee.AssignedTypes.Where(t => t.Deleted == BaseStatusDeleted.None).ToList().ForEach(t => t.SetDeleteMetaInfo(person, UC.IpAddress, UC.ProxyIpAddress));
                                        else {
                                            committee.AssignedTypes.Where(t => t.Deleted == BaseStatusDeleted.None && !item.Submitters.Contains(t.SubmitterType.Id)).ToList().ForEach(t => t.SetDeleteMetaInfo(person, UC.IpAddress, UC.ProxyIpAddress));
                                            committee.AssignedTypes.Where(t => t.Deleted != BaseStatusDeleted.None &&  item.Submitters.Contains(t.SubmitterType.Id)).ToList().ForEach(t => t.RecoverMetaInfo(person, UC.IpAddress, UC.ProxyIpAddress));
                                            foreach (long idType in item.Submitters.Where(i => !committee.AssignedTypes.Select(t => t.SubmitterType.Id).ToList().Contains(i)).ToList()) { 
                                                CommitteeAssignedSubmitterType aType = new CommitteeAssignedSubmitterType();
                                                aType.CreateMetaInfo(person, UC.IpAddress, UC.ProxyIpAddress);
                                                aType.Committee = committee;
                                                aType.SubmitterType = Manager.Get<SubmitterType>(idType);
                                                if (aType.SubmitterType != null) {
                                                    Manager.SaveOrUpdate(aType);
                                                    committee.AssignedTypes.Add(aType);
                                                }
                                            }
                                        }
                                    }
                                    committee.ForAllSubmittersType = item.ForAllSubmittersType;

                                    Manager.SaveOrUpdate(committee);
                                    SaveCriteria(call, committee, item.Criteria);
                                    displayNumber++;
                                }
                                Manager.Commit();
                                result = true;
                            }
                        }
                        catch (Exception ex)
                        {
                            if (Manager.IsInTransaction())
                                Manager.RollBack();
                        }
                        return result;
                    }

                    public List<dtoCommittee> GetEditorCommittees(long idCall)
                    {
                        BaseForPaper call = Manager.Get<BaseForPaper>(idCall);
                        return GetEditorCommittees(call);
                    }
                    public List<dtoCommittee> GetEditorCommittees(BaseForPaper call)
                    {
                        List<dtoCommittee> items = new List<dtoCommittee>();
                        try
                        {
                            items = (from s in Manager.GetIQ<EvaluationCommittee>()
                                     where s.Deleted == BaseStatusDeleted.None && s.Call == call select s).ToList().Select(
                                     s=>  new dtoCommittee()
                                     {
                                         Id = s.Id,
                                         Name = s.Name,
                                         Description = s.Description,
                                         DisplayOrder = s.DisplayOrder,
                                         ForAllSubmittersType = s.ForAllSubmittersType,
                                         IdCall = call.Id,
                                         WeightSettings = (s.UseDss ?  lm.Comol.Core.Dss.Domain.Templates.dtoItemWeightSettings.Create(s.WeightSettings, call.UseManualWeights): new dtoItemWeightSettings()),
                                         MethodSettings = (s.UseDss ? lm.Comol.Core.Dss.Domain.Templates.dtoItemMethodSettings.Create(s.MethodSettings, call.UseManualWeights) : new dtoItemMethodSettings()),
                                         UseDss= s.UseDss
                                     }).ToList();
                            List<long> idCommitees = items.Select(c => c.Id).ToList();
                            List<lm.Comol.Modules.CallForPapers.Domain.Evaluation.dtoCriterion> criteria = (from bc in Manager.GetIQ<BaseCriterion>()
                                                                                                            where bc.Committee!=null && idCommitees.Contains(bc.Committee.Id) && bc.Deleted == BaseStatusDeleted.None
                                                                                                            select bc).ToList().Select(bc => new lm.Comol.Modules.CallForPapers.Domain.Evaluation.dtoCriterion(bc)).ToList();

                            foreach (dtoCommittee comittee in items)
                            {
                                comittee.Criteria = (from c in criteria where c.IdCommittee == comittee.Id orderby c.DisplayOrder, c.Name select c).ToList();
                                foreach (dtoCriterion criterion in comittee.Criteria)
                                { 
                                    criterion.HasInEvaluations = (from e in Manager.GetIQ<CriterionEvaluated>() where e.Criterion.Id == criterion.Id && e.Deleted== BaseStatusDeleted.None && e.Evaluation.Status== EvaluationStatus.Evaluating select e.Id ).Any();
                                    criterion.HasEvaluations = (from e in Manager.GetIQ<CriterionEvaluated>()
                                                                where e.Criterion.Id == criterion.Id && e.Deleted == BaseStatusDeleted.None && (e.Evaluation.Status != EvaluationStatus.Evaluating && e.Evaluation.Status != EvaluationStatus.None) select e.Id ).Any();
                                }
                            }
                            //items.ForEach(i => i.Criteria = (from c in criteria where c.IdCommittee == i.Id orderby c.DisplayOrder, c.Name select c).ToList());
                            items.Where(i => !i.ForAllSubmittersType).ToList().ForEach(c => c.Submitters = (from s in Manager.GetIQ<CommitteeAssignedSubmitterType>()
                                                                                                            where s.Deleted == BaseStatusDeleted.None && s.SubmitterType != null && s.Committee != null && s.Committee.Id == c.Id
                                                                                                            select s.SubmitterType.Id).ToList());
                        }
                        catch (Exception ex)
                        {

                        }
                        if (items.Any(i => i.UseDss))
                        {
                            foreach (dtoCommittee c in items)
                            {
                                c.BaseWeights = GetAvailableWeights(call, c);
                            }
                        }
                        
                        return items.OrderBy(s => s.DisplayOrder).ThenBy(s => s.Name).ToList();
                    }


                    private List<lm.Comol.Core.Dss.Domain.Templates.dtoItemWeightBase> GetAvailableWeights(BaseForPaper call, dtoCommittee committee)
                    {
                        Dictionary<long, String> weights = committee.GetFuzzyMeItems();
                        List<lm.Comol.Core.Dss.Domain.Templates.dtoItemWeightBase> items = null;
                        Boolean isFuzzy = (committee.MethodSettings.InheritsFromFather && call.IsDssMethodFuzzy) || (!committee.MethodSettings.InheritsFromFather && committee.MethodSettings.IsFuzzyMethod);
                        if ((!committee.MethodSettings.InheritsFromFather && committee.MethodSettings.UseOrderedWeights) || (committee.MethodSettings.InheritsFromFather && call.UseOrderedWeights))
                        {
                            if (committee.Criteria.Count() >0)
                            {
                                if (committee.Criteria.Count() == weights.Count)
                                    items = (from int i in Enumerable.Range(1, committee.Criteria.Count()) select i).ToList().Select(i => new lm.Comol.Core.Dss.Domain.Templates.dtoItemWeightBase() { IdObject = (long)i, IsFuzzyValue = isFuzzy, OrderedItem = true, Name = i.ToString(), Value = (weights.ContainsKey((long)i) ? weights[(long)i] : "") }).ToList();
                                else
                                {
                                    Int32 index = 1;
                                    items = (from int i in Enumerable.Range(1, committee.Criteria.Count()) select i).ToList().Select(i => new lm.Comol.Core.Dss.Domain.Templates.dtoItemWeightBase() { IdObject = (long)i, IsFuzzyValue = isFuzzy, OrderedItem = true, Name = i.ToString(), Value = "" }).ToList();
                                    List<String> values = weights.Values.ToList();
                                    if (values.Any())
                                    {
                                        items[0].Value = values[0];
                                        switch (values.Count)
                                        {
                                            case 0:
                                            case 1:
                                                break;
                                            case 2:
                                                items.Last().Value = values.Last();
                                                break;
                                            default:
                                                items.Last().Value = values.Last();
                                                foreach (String v in values.Skip(1).Take(values.Count - 2))
                                                {
                                                    items[index++].Value = v;
                                                }
                                                break;
                                        }
                                    }
                                }
                            }
                        }
                        else
                            items = committee.Criteria.Select(c => c.ToWeightItem(weights, isFuzzy)).ToList();
                        return items;
                    }
                    public Boolean UpdateCommitteesDisplayOrder(List<long> idCommittees)
                    {
                        litePerson person = Manager.GetLitePerson(UC.CurrentUserID);
                        if (idCommittees.Count > 0 && AllowReorder(GetCallFromCommittees(idCommittees), person))
                        {
                            DateTime CurrentTime = DateTime.Now;
                            EvaluationCommittee committee = null;
                            try
                            {
                                Manager.BeginTransaction();
                                int displayOrder = 1;
                                foreach (var idCommittee in idCommittees)
                                {
                                    committee = Manager.Get<EvaluationCommittee>(idCommittee);
                                    if (committee != null)
                                    {
                                        committee.DisplayOrder = displayOrder;
                                        committee.UpdateMetaInfo(person, UC.IpAddress, UC.ProxyIpAddress);
                                        Manager.SaveOrUpdate<EvaluationCommittee>(committee);
                                        displayOrder++;
                                    }
                                }
                                Manager.Commit();
                                return true;
                            }
                            catch (Exception ex)
                            {
                                Manager.RollBack();
                                return false;
                            }
                        }
                        return false;
                    }
                    private BaseForPaper GetCallFromCommittees(List<long> idCommittees)
                    {
                        List<long> idCalls = (from s in Manager.GetIQ<EvaluationCommittee>() where idCommittees.Contains(s.Id) select s.Call.Id).ToList();
                        if (idCalls.Distinct<long>().ToList().Count != 1)
                            return null;
                        else
                            return Manager.Get<BaseForPaper>(idCalls[0]);
                    }
                    private Dictionary<long, BaseForPaper> GetCallsFromCommittees(List<long> idCommittees)
                    {
                        return (from s in Manager.GetIQ<EvaluationCommittee>() where idCommittees.Contains(s.Id) select s).ToDictionary(s => s.Id, s => s.Call);
                    }
                    public Boolean VirtualDeleteCommittee(long idCommittee, Boolean delete, ref long outputIdCommittee)
                    {
                        Boolean result = false;
                        try
                        {
                            litePerson person = Manager.GetLitePerson(UC.CurrentUserID);
                            EvaluationCommittee committee = Manager.Get<EvaluationCommittee>(idCommittee);
                            if (committee != null)
                            {
                                outputIdCommittee = committee.Id;
                                if (committee.Call != null &&
                                     (committee.Call.Type == CallForPaperType.CallForBids && (from s in Manager.GetIQ<Evaluation>()
                                                                                            where s.Committee.Id == committee.Id  && s.Deleted == BaseStatusDeleted.None && s.Status!= Domain.Evaluation.EvaluationStatus.None 
                                                                                            select s.Id).Any())
                                    )
                                    throw new EvaluationStarted();
                                else
                                {
                                    Manager.BeginTransaction();
                                    committee.UpdateMetaInfo(person, UC.IpAddress, UC.ProxyIpAddress);
                                    committee.Deleted = delete ? BaseStatusDeleted.Manual : BaseStatusDeleted.None;
                                    foreach (BaseCriterion criterion in committee.Criteria)
                                    {
                                        criterion.Deleted = delete ? (criterion.Deleted | BaseStatusDeleted.Cascade) : (criterion.Deleted = (BaseStatusDeleted)((int)criterion.Deleted - (int)BaseStatusDeleted.Cascade));
                                        Manager.SaveOrUpdate(criterion);
                                    }
                                    foreach (CommitteeMember member in committee.Members)
                                    {
                                        foreach (Evaluation evaluation in (from e in Manager.GetIQ<Evaluation>() where e.Committee.Id== committee.Id && e.Evaluator.Id == member.Evaluator.Id select e).ToList())
                                        {
                                            evaluation.Deleted = delete ? (evaluation.Deleted | BaseStatusDeleted.Cascade) : (evaluation.Deleted = (BaseStatusDeleted)((int)evaluation.Deleted - (int)BaseStatusDeleted.Cascade));
                                            Manager.SaveOrUpdate(evaluation);
                                        }
                                        member.Deleted = delete ? (member.Deleted | BaseStatusDeleted.Cascade) : (member.Deleted = (BaseStatusDeleted)((int)member.Deleted - (int)BaseStatusDeleted.Cascade));
                                        Manager.SaveOrUpdate(member);
                                    }
                                    foreach (CommitteeAssignedSubmitterType assignment in committee.AssignedTypes)
                                    {
                                        assignment.Deleted = delete ? (assignment.Deleted | BaseStatusDeleted.Cascade) : (assignment.Deleted = (BaseStatusDeleted)((int)assignment.Deleted - (int)BaseStatusDeleted.Cascade));
                                        Manager.SaveOrUpdate(assignment);
                                    }
                                    if (delete)
                                    {
                                        var query = (from s in Manager.GetIQ<EvaluationCommittee>()
                                                     where s.Call.Id == committee.Call.Id && s.Deleted == BaseStatusDeleted.None && s.Id != committee.Id
                                                     select s);
                                        outputIdCommittee = (from s in query where s.DisplayOrder <= committee.DisplayOrder orderby s.DisplayOrder descending select s.Id).FirstOrDefault();
                                        if (outputIdCommittee == 0)
                                            outputIdCommittee = (from s in query where s.DisplayOrder > committee.DisplayOrder orderby s.DisplayOrder select s.Id).FirstOrDefault();

                                    }
                                    Manager.SaveOrUpdate(committee);
                                    Manager.Commit();
                                    result = true;
                                }
                            }
                            else
                                result = true;
                        }
                        catch (EvaluationStarted ex)
                        {
                            throw ex;
                        }
                        catch (Exception ex)
                        {
                            Manager.RollBack();
                        }
                        return result;
                    }

                    private int GetNewCommitteeDisplayOrder(BaseForPaper call)
                    {
                        try
                        {
                            int displayOrder = (from s in Manager.GetIQ<EvaluationCommittee>()
                                                where s.Call == call && s.Deleted == BaseStatusDeleted.None
                                                select s.DisplayOrder).Max();
                            displayOrder++;
                            return displayOrder;
                        }
                        catch (Exception)
                        {
                            return 1;

                        }
                    }
                #endregion
                #region "1.2 - Editing Criteria"
                    public List<BaseCriterion> AddCriteria(long idCall, List<dtoCommittee> committees, lm.Comol.Core.Dss.Domain.Templates.dtoItemMethodSettings settings, long idCommittee, List<lm.Comol.Modules.CallForPapers.Domain.Evaluation.dtoCriterion> items)
                    {
                        List<BaseCriterion> criteria = new List<BaseCriterion>();
                        SaveCommittees(idCall, committees,settings);
                        try
                        {
                            Manager.BeginTransaction();
                            litePerson person = Manager.GetLitePerson(UC.CurrentUserID);
                            EvaluationCommittee committee = Manager.Get<EvaluationCommittee>(idCommittee);
                            if (committee != null && person != null)
                            {
                                List<SubmitterType> submitters = (from s in Manager.GetIQ<SubmitterType>() where s.Deleted == BaseStatusDeleted.None && s.Call.Id == committee.Call.Id select s).ToList();
                                Int32 displayOrder = (from f in Manager.GetIQ<BaseCriterion>() where f.Deleted == BaseStatusDeleted.None && f.Committee.Id == committee.Id select f.DisplayOrder).Max() + 1;
                                foreach (lm.Comol.Modules.CallForPapers.Domain.Evaluation.dtoCriterion dto in items)
                                {
                                    BaseCriterion criterion = null;
                                    switch (dto.Type) {
                                        case CriterionType.Textual:
                                            criterion = new TextualCriterion();
                                            ((TextualCriterion)criterion).MaxLength = dto.MaxLength;
                                            break;
                                        case CriterionType.StringRange:
                                            criterion = new StringRangeCriterion();
                                            ((StringRangeCriterion)criterion).MaxOption = dto.MaxOption;
                                            ((StringRangeCriterion)criterion).MinOption = dto.MinOption;
                                            break;
                                        case CriterionType.IntegerRange:
                                        case CriterionType.DecimalRange:
                                            criterion = new NumericRangeCriterion(dto.DecimalMinValue, dto.DecimalMaxValue, dto.Type);
                                            break;
                                        case CriterionType.RatingScale:
                                        case CriterionType.RatingScaleFuzzy:
                                            criterion = new DssCriterion();
                                            criterion.Type = dto.Type;
                                            ((DssCriterion)criterion).IsFuzzy = (dto.Type== CriterionType.RatingScaleFuzzy);
                                            ((DssCriterion)criterion).IdRatingSet = dto.IdRatingSet;
                                            break;
                                        default:
                                            criterion = new BaseCriterion();
                                            criterion.Type = CriterionType.Textual;
                                            break;
                                    }
                                    criterion.UseDss = committee.UseDss;
                                    if (criterion.UseDss){
                                        //criterion.WeightSettings = committee.WeightSettings.Copy();
                                        criterion.MethodSettings = committee.MethodSettings.Copy();
                                    }
                                    criterion.CreateMetaInfo(person, UC.IpAddress, UC.ProxyIpAddress);
                                    criterion.Committee = committee;
                                    criterion.Description = dto.Description;
                                    criterion.DisplayOrder = displayOrder;
                                    criterion.CommentType = dto.CommentType;
                                    if (String.IsNullOrEmpty(dto.Name))
                                        criterion.Name = displayOrder.ToString();
                                    else if (dto.Name.Contains("{0}"))
                                        criterion.Name = String.Format(dto.Name, displayOrder.ToString());
                                    else
                                        criterion.Name = dto.Name;
                                    Manager.SaveOrUpdate(criterion);
                                    switch (criterion.Type)
                                    {
                                        case CriterionType.StringRange:
                                            SaveOptions(person, ((StringRangeCriterion)criterion), dto.Options);
                                            break;
                                        case CriterionType.RatingScale:
                                        case CriterionType.RatingScaleFuzzy:
                                            SaveOptions(person, ((DssCriterion)criterion), dto.Options);
                                            break;
                                    }

                                    if (criterion.Type == CriterionType.StringRange)
                                        SaveOptions(person, ((StringRangeCriterion)criterion), dto.Options);
                                    Manager.SaveOrUpdate(criterion);
                                    displayOrder++;
                                    committee.Criteria.Add(criterion);
                                    criteria.Add(criterion);
                                }
                            }
                            Manager.Commit();
                        }
                        catch (Exception ex)
                        {
                            Manager.RollBack();
                            criteria.Clear();
                        }
                        return criteria;
                    }
                    private List<BaseCriterion> SaveCriteria(BaseForPaper call, EvaluationCommittee committee, IList<lm.Comol.Modules.CallForPapers.Domain.Evaluation.dtoCriterion> items)
                    {
                        List<BaseCriterion> criteria = new List<BaseCriterion>();
                        try
                        {
                            litePerson person = Manager.GetLitePerson(UC.CurrentUserID);
                            if (call != null && committee != null && person != null)
                            {
                                int displayNumber = 1;
                                foreach (lm.Comol.Modules.CallForPapers.Domain.Evaluation.dtoCriterion item in items)
                                {
                                    if (String.IsNullOrEmpty(item.Name))
                                        item.Name = item.DisplayOrder.ToString();
                                    BaseCriterion criterion = Manager.Get<BaseCriterion>(item.Id);
                                    if (criterion == null)
                                    {
                                        switch (item.Type)
                                        {
                                            case CriterionType.Textual:
                                                criterion = new TextualCriterion();
                                                ((TextualCriterion)criterion).MaxLength = item.MaxLength;
                                                break;
                                            case CriterionType.StringRange:
                                                criterion = new StringRangeCriterion();
                                                ((StringRangeCriterion)criterion).MaxOption = item.MaxOption;
                                                ((StringRangeCriterion)criterion).MinOption = item.MinOption;
                                                break;
                                            case CriterionType.IntegerRange:
                                            case CriterionType.DecimalRange:
                                                criterion = new NumericRangeCriterion(item.DecimalMinValue, item.DecimalMaxValue ,item.Type);
                                                break;
                                            case CriterionType.RatingScaleFuzzy:
                                                criterion = new DssCriterion();
                                                ((DssCriterion)criterion).IsFuzzy = (item.Type == CriterionType.RatingScaleFuzzy);
                                                ((DssCriterion)criterion).IdRatingSet = item.IdRatingSet;
                                                break;
                                            default:
                                                criterion = new BaseCriterion();
                                                criterion.Type = CriterionType.Textual;
                                                break;
                                        }
                                        criterion.CreateMetaInfo(person, UC.IpAddress, UC.ProxyIpAddress);
                                    }
                                    else
                                    {
                                        switch (item.Type)
                                        {
                                            case CriterionType.Textual:
                                                ((TextualCriterion)criterion).MaxLength = item.MaxLength;
                                                break;
                                            case CriterionType.StringRange:
                                                ((StringRangeCriterion)criterion).MaxOption = item.MaxOption;
                                                ((StringRangeCriterion)criterion).MinOption = item.MinOption;
                                                break;
                                            case CriterionType.IntegerRange:
                                            case CriterionType.DecimalRange:
                                                ((NumericRangeCriterion)criterion).DecimalMinValue = item.DecimalMinValue;
                                                ((NumericRangeCriterion)criterion).DecimalMaxValue = item.DecimalMaxValue;
                                                criterion.Type = item.Type;
                                                break;
                                        }
                                        criterion.UpdateMetaInfo(person, UC.IpAddress, UC.ProxyIpAddress);
                                    }
                                    criterion.Committee = committee;
                                    if (committee.UseDss)
                                    {
                                        criterion.UseDss = true;
                                        criterion.WeightSettings = item.WeightSettings.Copy();
                                        criterion.MethodSettings = committee.MethodSettings.Copy();
                                    }
                                    else
                                        criterion.UseDss = false;
                                    criterion.Name = item.Name;
                                    criterion.Description = item.Description;
                                    criterion.DisplayOrder = item.DisplayOrder;
                                    criterion.CommentType = item.CommentType;
                                    Manager.SaveOrUpdate(criterion);
                                    if (item.Type ==  CriterionType.StringRange)
                                    {
                                        SaveOptions(person, ((StringRangeCriterion)criterion), item.Options);
                                    }
                                    displayNumber++;
                                    criteria.Add(criterion);
                                }
                            }
                        }
                        catch (Exception ex)
                        {

                        }
                        return criteria;
                    }
                    public Boolean UpdateCriteriaDisplayOrder(List<long> idCriteria, long idCommittee)
                    {
                        litePerson oPerson = Manager.GetLitePerson(UC.CurrentUserID);
                        EvaluationCommittee committee = Manager.Get<EvaluationCommittee>(idCommittee);
                        if (idCriteria.Count > 0 && committee != null && committee.Call != null & AllowReorder(committee.Call, oPerson))
                        {
                            try
                            {
                                Manager.BeginTransaction();

                                BaseCriterion criterion;
                                int displayOrder = 1;
                                foreach (var idCriterion in idCriteria)
                                {
                                    criterion = Manager.Get<BaseCriterion>(idCriterion);
                                    if (criterion != null)
                                    {
                                        criterion.DisplayOrder = displayOrder;
                                        if (criterion.Committee.Id != committee.Id)
                                            criterion.Committee = committee;
                                        criterion.UpdateMetaInfo(oPerson, UC.IpAddress, UC.ProxyIpAddress);
                                        Manager.SaveOrUpdate(criterion);
                                        displayOrder++;
                                    }
                                }
                                Manager.Commit();
                                return true;
                            }
                            catch (Exception ex)
                            {
                                Manager.RollBack();
                                return false;
                            }
                        }
                        return false;
                    }
        public Boolean UpdateCriteriaAdvDisplayOrder(List<long> idCriteria, long idCommittee)
        {
            litePerson oPerson = Manager.GetLitePerson(UC.CurrentUserID);
            Advanced.Domain.AdvCommission committee = Manager.Get<Advanced.Domain.AdvCommission>(idCommittee);
            if (idCriteria.Count > 0 && committee != null && committee.Call != null & AllowReorder(committee.Call, oPerson))
            {
                try
                {
                    Manager.BeginTransaction();

                    BaseCriterion criterion;
                    int displayOrder = 1;
                    foreach (var idCriterion in idCriteria)
                    {
                        criterion = Manager.Get<BaseCriterion>(idCriterion);
                        if (criterion != null)
                        {
                            criterion.DisplayOrder = displayOrder;
                            if (criterion.AdvCommitee == null || criterion.AdvCommitee.Id != committee.Id)
                                criterion.AdvCommitee = committee;

                            criterion.UpdateMetaInfo(oPerson, UC.IpAddress, UC.ProxyIpAddress);
                            Manager.SaveOrUpdate(criterion);
                            displayOrder++;
                        }
                    }
                    Manager.Commit();
                    return true;
                }
                catch (Exception ex)
                {
                    Manager.RollBack();
                    return false;
                }
            }
            return false;
        }
        public Boolean VirtualDeleteCriterion(long idCriterion, Boolean delete, ref long outputIdCommittee, ref long outputIdCriterion)
                    {
                        Boolean result = false;
                        try
                        {
                            litePerson person = Manager.GetLitePerson(UC.CurrentUserID);
                            BaseCriterion criterion = Manager.Get<BaseCriterion>(idCriterion);
                            if (criterion != null && criterion.Committee != null)
                            {
                                outputIdCommittee = criterion.Committee.Id;
                                outputIdCriterion = criterion.Id;
                                if (criterion.Committee!=null && criterion.Committee.Call != null &&
                                     (from s in Manager.GetIQ<Evaluation>()
                                      where s.Call.Id == criterion.Committee.Call.Id && s.Deleted == BaseStatusDeleted.None && s.Status != Domain.Evaluation.EvaluationStatus.None 
                                                select s.Id).Any()
                                    )
                                    throw new EvaluationStarted();
                                else
                                {
                                    Manager.BeginTransaction();
                                    criterion.UpdateMetaInfo(person, UC.IpAddress, UC.ProxyIpAddress);
                                    criterion.Deleted = delete ? BaseStatusDeleted.Manual : BaseStatusDeleted.None;
                                    if (criterion.Type== CriterionType.StringRange)
                                    {
                                        foreach (CriterionOption option in ((StringRangeCriterion)criterion).Options)
                                        {
                                            option.Deleted = delete ? (option.Deleted | BaseStatusDeleted.Cascade) : (option.Deleted = (BaseStatusDeleted)((int)option.Deleted - (int)BaseStatusDeleted.Cascade));
                                            Manager.SaveOrUpdate(option);
                                        }
                                    }
                                    if (delete)
                                    {
                                        var query = (from s in Manager.GetIQ<BaseCriterion>()
                                                     where s.Committee != null && s.Committee.Id == criterion.Committee.Id && s.Deleted == BaseStatusDeleted.None && s.Id != criterion.Id
                                                     select s);
                                        outputIdCriterion = (from s in query where s.DisplayOrder <= criterion.DisplayOrder orderby s.DisplayOrder descending select s.Id).FirstOrDefault();
                                        if (outputIdCriterion == 0)
                                            outputIdCriterion = (from s in query where s.DisplayOrder > criterion.DisplayOrder orderby s.DisplayOrder select s.Id).FirstOrDefault();

                                    }
                                    Manager.SaveOrUpdate(criterion);
                                    Manager.Commit();
                                    result = true;
                                }
                            }
                            else
                                result = true;
                        }
                        catch (EvaluationStarted ex)
                        {
                            throw ex;
                        }
                        catch (Exception ex)
                        {
                            Manager.RollBack();
                        }
                        return result;
                    }
                   
                    #region "CriterionOptions"
                        public CriterionOption AddOptionToCriterion(long idCriterion, List<dtoCommittee> committees, lm.Comol.Core.Dss.Domain.Templates.dtoItemMethodSettings settings, String name)
                        {
                            return AddOptionToCriterion(idCriterion, committees,settings, name, null);
                        }
                        public CriterionOption AddOptionToCriterion(long idCriterion, List<dtoCommittee> committees, lm.Comol.Core.Dss.Domain.Templates.dtoItemMethodSettings settings, String name, Decimal? value)
                        {
                            StringRangeCriterion criterion = Manager.Get<StringRangeCriterion>(idCriterion);
                            if (criterion != null && criterion.Committee != null && criterion.Committee.Call != null)
                                SaveCommittees(criterion.Committee.Call, committees, settings);
                            return AddOptionToCriterion(criterion, name, value);
                        }
                        public CriterionOption AddOptionToCriterion(StringRangeCriterion criterion, String name, Decimal? value)
                        {
                            CriterionOption option = null;
                            try
                            {
                                Manager.BeginTransaction();
                                litePerson person = Manager.GetLitePerson(UC.CurrentUserID);
                                if (criterion != null && person != null)
                                {
                                    option = new CriterionOption();
                                    option.CreateMetaInfo(person, UC.IpAddress, UC.ProxyIpAddress);
                                    option.DisplayOrder = (from s in Manager.GetIQ<CriterionOption>() where s.Criterion.Id == criterion.Id select s.DisplayOrder).Max() + 1;
                                    if (value.HasValue)
                                        option.Value = value.Value;
                                    else
                                        option.Value = GetOptionValue(criterion);
                                    if (string.IsNullOrEmpty(name))
                                        option.Name = option.Value.ToString();
                                    else
                                        option.Name = name;
                                    option.ShortName = "";
                                    option.Criterion = criterion;
                                    Manager.SaveOrUpdate(option);
                                    criterion.Options.Add(option);
                                    Manager.SaveOrUpdate(criterion);
                                }
                                Manager.Commit();
                            }
                            catch (Exception ex)
                            {
                                option = null;
                                Manager.RollBack();
                            }
                            return option;
                        }
                        public Boolean VirtualDeleteCriterionOption(long idOption, Boolean delete, ref long outputIdCommittee, ref long outputIdCriterion, ref long outputIdOption)
                        {
                            Boolean result = false;
                            try
                            {
                                litePerson person = Manager.GetLitePerson(UC.CurrentUserID);
                                CriterionOption option = Manager.Get<CriterionOption>(idOption);
                                if (option != null && option.Criterion != null)
                                {
                                    List<CriterionEvaluated> evaluatedCriteria = new List<CriterionEvaluated>();
                                    Boolean withEvaluations = false;
                                    if (option.Criterion.Committee != null)
                                        outputIdCommittee = option.Criterion.Committee.Id;
                                    outputIdCriterion = option.Criterion.Id;
                                    outputIdOption = option.Id;

                                    if (option.Criterion.Committee != null && option.Criterion.Committee.Call != null)
                                    {
                                        evaluatedCriteria = (from c in Manager.GetIQ<CriterionEvaluated>()
                                                             where c.Deleted == BaseStatusDeleted.None && c.Criterion.Id == option.Criterion.Id
                                                             select c).ToList();

                                    }
                                    if (evaluatedCriteria.Where(ec=> ec.Evaluation.Status!= EvaluationStatus.None && ec.Evaluation.Status!= EvaluationStatus.Evaluating).Any())
                                        throw new EvaluationStarted();
                                    else
                                    {
                                        Manager.BeginTransaction();
                                        option.UpdateMetaInfo(person, UC.IpAddress, UC.ProxyIpAddress);
                                        option.Deleted = delete ? BaseStatusDeleted.Manual : BaseStatusDeleted.None;
                                        if (delete)
                                        {
                                            var query = (from s in Manager.GetIQ<FieldOption>()
                                                         where s.Field != null && s.Field.Id == option.Criterion.Id && s.Deleted == BaseStatusDeleted.None && s.Id != option.Id
                                                         select s);
                                            outputIdOption = (from s in query where s.DisplayOrder <= option.DisplayOrder orderby s.DisplayOrder descending select s.Id).FirstOrDefault();
                                            if (outputIdOption == 0)
                                                outputIdOption = (from s in query where s.DisplayOrder > option.DisplayOrder orderby s.DisplayOrder select s.Id).FirstOrDefault();

                                        }
                                        Manager.SaveOrUpdate(option);
                                        if (evaluatedCriteria.Any())
                                        {
                                            foreach(var ev in evaluatedCriteria.GroupBy(ec=>ec.Evaluation)){
                                                List<dtoCriterionEvaluated> eCriteria = GetEvaluationCriteria(ev.Key, ev.ToList());
                                                if (eCriteria.Where(c => c.IsValidForCriterionSaving).Any())
                                                {
                                                    ev.Key.AverageRating = (double)(from c in eCriteria where c.IsValidForCriterionSaving select c.DecimalValue).Average();
                                                    ev.Key.SumRating = (double)(from c in eCriteria where c.IsValidForCriterionSaving select c.DecimalValue).Sum();
                                                }
                                                else{
                                                    ev.Key.AverageRating = 0;
                                                    ev.Key.SumRating = 0;
                                                }
                                                Manager.SaveOrUpdate(ev.Key);
                                            }
                                            

                                            foreach (CriterionEvaluated cEvaluated in evaluatedCriteria)
                                            {
                                                cEvaluated.IsValueEmpty = true;
                                                cEvaluated.DecimalValue = 0;
                                                cEvaluated.Option = null;
                                                cEvaluated.UpdateMetaInfo(person, UC.IpAddress, UC.ProxyIpAddress);
                                            }
                                            Manager.SaveOrUpdateList(evaluatedCriteria);
                                        }

                                       
                                        Manager.Commit();
                                        result = true;
                                    }
                                }
                                else
                                    result = true;
                            }
                            catch (SubmissionLinked ex)
                            {
                                throw ex;
                            }
                            catch (Exception ex)
                            {
                                Manager.RollBack();
                            }
                            return result;
                        }
                        private List<CriterionOption> SaveOptions(litePerson person, StringRangeCriterion criterion, List<dtoCriterionOption> items)
                        {
                            List<CriterionOption> options = new List<CriterionOption>();
                            try
                            {
                                if (criterion != null && person != null)
                                {
                                    Boolean isNew = false;
                                    int displayNumber = 1;
                                    foreach (dtoCriterionOption item in items)
                                    {
                                        if (String.IsNullOrEmpty(item.Name))
                                            item.Name = item.DisplayOrder.ToString();
                                        CriterionOption opt = Manager.Get<CriterionOption>(item.Id);
                                        isNew = (opt == null);
                                        if (isNew)
                                        {
                                            opt = new CriterionOption();
                                            opt.CreateMetaInfo(person, UC.IpAddress, UC.ProxyIpAddress);
                                        }
                                        else
                                            opt.UpdateMetaInfo(person, UC.IpAddress, UC.ProxyIpAddress);
                                        opt.DisplayOrder = displayNumber; // item.DisplayOrder;
                                        opt.Criterion = criterion;
                                        if (String.IsNullOrEmpty(item.Name))
                                            opt.Name = item.DisplayOrder.ToString();
                                        else if (item.Name.Contains("{0}"))
                                            opt.Name = String.Format(item.Name, item.DisplayOrder.ToString());
                                        else
                                            opt.Name = item.Name;
                                        opt.ShortName = item.ShortName;
                                        //if (isNew)
                                        //    opt.Value = GetOptionValue(criterion);
                                        //else
                                            opt.Value = item.Value;
                                        Manager.SaveOrUpdate(opt);
                                        options.Add(opt);
                                        displayNumber++;
                                    }
                                }
                            }
                            catch (Exception ex)
                            {

                            }
                            return options;
                        }
                        private Decimal GetOptionValue(StringRangeCriterion criterion)
                        {
                            List<Decimal> optionsValue = (from f in Manager.GetIQ<CriterionOption>()
                                                         where f.Deleted == BaseStatusDeleted.None && f.Criterion == criterion
                                                         select f.Value).ToList();
                            return (optionsValue.Count == 0) ? 0 : optionsValue.Max() + 1;
                        }

                        private List<CriterionOption> SaveOptions(litePerson person, DssCriterion criterion, List<dtoCriterionOption> items)
                        {
                            List<CriterionOption> options = new List<CriterionOption>();
                            try
                            {
                                if (criterion != null && person != null)
                                {
                                    Boolean isNew = false;
                                    int displayNumber = 1;
                                    foreach (dtoCriterionOption item in items)
                                    {
                                        if (String.IsNullOrEmpty(item.Name))
                                            item.Name = item.DisplayOrder.ToString();
                                        CriterionOption opt = Manager.Get<CriterionOption>(item.Id);
                                        isNew = (opt == null);
                                        if (isNew)
                                        {
                                            opt = new CriterionOption();
                                            opt.CreateMetaInfo(person, UC.IpAddress, UC.ProxyIpAddress);
                                        }
                                        else
                                            opt.UpdateMetaInfo(person, UC.IpAddress, UC.ProxyIpAddress);
                                        opt.IdRatingSet = item.IdRatingSet;
                                        opt.IdRatingValue = item.IdRatingValue;
                                        opt.IsFuzzy = item.IsFuzzy;
                                        opt.UseDss = true;
                                        opt.DoubleValue = item.DoubleValue;
                                        opt.FuzzyValue = item.FuzzyValue;
                                        opt.DisplayOrder = displayNumber; // item.DisplayOrder;
                                        opt.Criterion = criterion;
                                        if (String.IsNullOrEmpty(item.Name))
                                            opt.Name = item.DisplayOrder.ToString();
                                        else if (item.Name.Contains("{0}"))
                                            opt.Name = String.Format(item.Name, item.DisplayOrder.ToString());
                                        else
                                            opt.Name = item.Name;
                                        opt.ShortName = item.ShortName;
                                        //if (isNew)
                                        //    opt.Value = GetOptionValue(criterion);
                                        //else
                                        opt.Value = item.Value;
                                        Manager.SaveOrUpdate(opt);
                                        options.Add(opt);
                                        displayNumber++;
                                    }
                                }
                            }
                            catch (Exception ex)
                            {

                            }
                            return options;
                        }

                        public Boolean UpdateOptionsDisplayOrder(List<long> idOptions)
                        {
                            litePerson person = Manager.GetLitePerson(UC.CurrentUserID);
                            StringRangeCriterion criterion = GetCriterionFromOptions(idOptions);
                            if (idOptions.Count > 0 && criterion != null && criterion.Committee != null && criterion.Committee.Call != null & AllowReorder(criterion.Committee.Call, person))
                            {
                                try
                                {
                                    Manager.BeginTransaction();

                                    CriterionOption opt;
                                    int displayOrder = 1;
                                    foreach (var idOption in idOptions)
                                    {
                                        opt = Manager.Get<CriterionOption>(idOption);
                                        if (opt != null && criterion.Id == opt.Criterion.Id)
                                        {
                                            opt.DisplayOrder = displayOrder;
                                            opt.UpdateMetaInfo(person, UC.IpAddress, UC.ProxyIpAddress);
                                            Manager.SaveOrUpdate(opt);
                                            displayOrder++;
                                        }
                                    }
                                    Manager.Commit();
                                    return true;
                                }
                                catch (Exception ex)
                                {
                                    Manager.RollBack();
                                    return false;
                                }
                            }
                            return false;
                        }
                        private StringRangeCriterion GetCriterionFromOptions(List<long> idOptions)
                        {
                            List<long> idCriteria = (from s in Manager.GetIQ<CriterionOption>() where idOptions.Contains(s.Id) select s.Criterion.Id).ToList();
                            if (idCriteria.Distinct<long>().ToList().Count != 1)
                                return null;
                            else
                                return Manager.Get<StringRangeCriterion>(idCriteria[0]);
                        }
                    #endregion
                #endregion
            #endregion

            #region "2 - Manage Evaluators"
                        public List<long> GetIdCommittees(long idCall)
                {
                    List<long> items = new List<long>();
                    try
                    {
                        items = (from s in Manager.GetIQ<EvaluationCommittee>()
                                       where s.Deleted == BaseStatusDeleted.None && s.Call.Id == idCall
                                       orderby s.DisplayOrder, s.Name 
                                    select s.Id).ToList();
                    }
                    catch (Exception ex)
                    {

                    }
                    return items;
                }
                public List<dtoBaseCommittee> GetAvailableCommittees(BaseForPaper call)
                {
                    List<dtoBaseCommittee> items = new List<dtoBaseCommittee>();
                    try
                    {
                        items = (from s in Manager.GetIQ<EvaluationCommittee>()
                                    where s.Deleted == BaseStatusDeleted.None && s.Call == call
                                 select new dtoBaseCommittee()
                                    {
                                        Id = s.Id,
                                        Name = s.Name,
                                        Description = s.Description,
                                        DisplayOrder = s.DisplayOrder,
                                        ForAllSubmittersType = s.ForAllSubmittersType,
                                    }).ToList();
                    }
                    catch (Exception ex)
                    {

                    }
                    return items.OrderBy(s => s.DisplayOrder).ThenBy(s => s.Name).ToList();
                }
                public List<dtoCommitteeMember> GetCommitteesMembers(BaseForPaper call)
                {
                    List<dtoCommitteeMember> items = new List<dtoCommitteeMember>();
                    try
                    {
                        var query = (from s in Manager.GetIQ<CallEvaluator>()
                                 where s.Deleted == BaseStatusDeleted.None && s.Call == call select s).ToList();
                        items = (from s in query select new dtoCommitteeMember(GetCommitteeMembership(s)) {
                              IdCallEvaluator= s.Id, IdPerson= (s.Person==null) ? 0 : s.Person.Id, DisplayName= (s.Person==null) ? "" : s.Person.SurnameAndName }).ToList();
                    }
                    catch (Exception ex)
                    {

                    }
                    return items.OrderBy(s => s.DisplayName).ToList();
                }
                public CommitteeMember GetCommitteeMembership(long idMembership)
                {
                    CommitteeMember member = null;
                    try
                    {
                        member = Manager.Get<CommitteeMember>(idMembership);
                    }
                    catch (Exception ex)
                    {

                    }
                    return member;
                }

                public Dictionary<long,long> GetCommitteeMembership(CallEvaluator evaluator)
                {
                    Dictionary<long,long> items = new Dictionary<long,long>();
                    try
                    {
                        var query = (from m in Manager.GetIQ<CommitteeMember>() where m.Evaluator.Id== evaluator.Id && m.Deleted== BaseStatusDeleted.None 
                                    select new { IdCommittee= m.Committee.Id, IdMembership = m.Id});
                        items = query.ToDictionary(c => c.IdCommittee, m => m.IdMembership);
                    }
                    catch (Exception ex)
                    {

                    }
                    return items;
                }
                public List<long> GetCommitteeIdMembers(long idCommittee)
                {
                    List<long> items = new List<long>();
                    try
                    {
                        items = (from m in Manager.GetIQ<CommitteeMember>()
                                 where m.Deleted == BaseStatusDeleted.None
                                 select m.Id).ToList();
                    }
                    catch (Exception ex)
                    {

                    }
                    return items;
                }
                public List<CommitteeMember> GetCommitteeMembers(long idCommittee)
                {
                    List<CommitteeMember> items = new List<CommitteeMember>();
                    try
                    {
                        items = (from m in Manager.GetIQ<CommitteeMember>()
                                 where m.Deleted == BaseStatusDeleted.None && m.Committee.Id==idCommittee 
                                 select m).ToList();
                    }
                    catch (Exception ex)
                    {

                    }
                    return items;
                }
                public List<expCommitteeMember> GetCommitteeMembersForSummary(long idCommittee)
                {
                    List<expCommitteeMember> items = new List<expCommitteeMember>();
                    try
                    {
                        items = (from m in Manager.GetIQ<expCommitteeMember>()
                                 where m.Deleted == BaseStatusDeleted.None && m.IdCommittee == idCommittee
                                 select m).ToList();
                    }
                    catch (Exception ex)
                    {

                    }
                    return items;
                }
                public Dictionary<long, Dictionary<long, String>> GetCommitteesMemberships(BaseForPaper call)
                {
                    Dictionary<long, Dictionary<long, String>> items = new Dictionary<long, Dictionary<long, String>>();
                    try
                    {
                        var query = (from m in Manager.GetIQ<EvaluationCommittee>()
                                     where m.Deleted == BaseStatusDeleted.None && m.Call.Id == call.Id
                                     select m);
                        query.ToList().ForEach(c => items.Add(c.Id,(from m in c.Members where m.Deleted == BaseStatusDeleted.None && m.Evaluator!=null  select m).ToDictionary(m=> m.Evaluator.Id, m=> ( m.Evaluator.Person== null)? "--" : m.Evaluator.Person.SurnameAndName)));
                    }
                    catch (Exception ex)
                    {

                    }
                    return items;
                }
          
                public List<CallEvaluator> AddEvaluatorsToCall(long idCall, List<dtoCommitteeMember> members, List<Int32> persons)
                {
                    BaseForPaper call = Manager.Get<BaseForPaper>(idCall);
                    if (call != null)
                        SaveCallEvaluators(call, members);
                    return AddEvaluators(call, persons);
                }
                public List<CallEvaluator> AddEvaluators(BaseForPaper call, List<Int32> persons)
                {
                    List<CallEvaluator> evaluators = new List<CallEvaluator>();
                    try
                    {
                        Manager.BeginTransaction();
                        litePerson person = Manager.GetLitePerson(UC.CurrentUserID);
                        if (call != null && person != null)
                        {
                            List<EvaluationCommittee> committees = (from c in Manager.GetIQ<EvaluationCommittee>() where c.Call.Id == call.Id && c.Deleted == BaseStatusDeleted.None select c).ToList();
                            foreach (Int32 idPerson in persons)
                            {
                                CallEvaluator evaluator = new CallEvaluator();
                                evaluator.CreateMetaInfo(person, UC.IpAddress, UC.ProxyIpAddress);
                                evaluator.Call = call;
                                evaluator.Person = Manager.GetLitePerson(idPerson);
                                if (evaluator.Person != null)
                                {
                                    Manager.SaveOrUpdate(evaluator);
                                    if (committees.Count == 1)
                                    {
                                        CommitteeMember member = new CommitteeMember();
                                        member.CreateMetaInfo(person, UC.IpAddress, UC.ProxyIpAddress);
                                        member.Committee = committees[0];
                                        member.Evaluator = evaluator;
                                        member.Status = MembershipStatus.Standard;
                                        evaluator.Memberships.Add(member);
                                    }
                                    evaluators.Add(evaluator);
                                }
                            }
                        }
                        Manager.Commit();
                    }
                    catch (Exception ex)
                    {
                        evaluators.Clear();
                        Manager.RollBack();
                    }
                    return evaluators;
                }
                public Boolean SaveCallEvaluators(long idCall, List<dtoCommitteeMember> evaluators)
                {
                    BaseForPaper call = Manager.Get<BaseForPaper>(idCall);
                    return SaveCallEvaluators(call, evaluators);
                }
                public Boolean SaveCallEvaluators(BaseForPaper call, List<dtoCommitteeMember> evaluators)
                {
                    Boolean result = false;
                    try
                    {
                        litePerson person = Manager.GetLitePerson(UC.CurrentUserID);
                        if (call != null && person != null)
                        {
                            Manager.BeginTransaction();
                            foreach (dtoCommitteeMember item in evaluators)
                            {
                                CallEvaluator evaluator = Manager.Get<CallEvaluator>(item.IdCallEvaluator);
                                if (evaluator == null)
                                {
                                    evaluator = new CallEvaluator();
                                    evaluator.CreateMetaInfo(person, UC.IpAddress, UC.ProxyIpAddress);
                                    evaluator.Call = call;
                                    evaluator.Person = Manager.GetLitePerson(item.IdPerson);
                                }
                                else
                                    evaluator.UpdateMetaInfo(person, UC.IpAddress, UC.ProxyIpAddress);
                                Manager.SaveOrUpdate(evaluator);

                                if (item.Committees.Count == 0)
                                {
                                    evaluator.Memberships.Where(t => t.Deleted == BaseStatusDeleted.None).ToList().ForEach(t => t.SetDeleteMetaInfo(person, UC.IpAddress, UC.ProxyIpAddress));
                                    evaluator.Evaluations.Where(t => t.Deleted == BaseStatusDeleted.None).ToList().ForEach(t => t.SetDeleteMetaInfo(person, UC.IpAddress, UC.ProxyIpAddress));
                                }
                                else
                                {
                                    evaluator.Memberships.Where(t => t.Deleted == BaseStatusDeleted.None && !item.Committees.Contains(t.Committee.Id)).ToList().ForEach(t => t.SetDeleteMetaInfo(person, UC.IpAddress, UC.ProxyIpAddress));
                                    evaluator.Memberships.Where(t => t.Deleted != BaseStatusDeleted.None && item.Committees.Contains(t.Committee.Id)).ToList().ForEach(t => t.RecoverMetaInfo(person, UC.IpAddress, UC.ProxyIpAddress));
                                    evaluator.Evaluations.Where(t => t.Deleted == BaseStatusDeleted.None && !item.Committees.Contains(t.Committee.Id)).ToList().ForEach(t => t.SetDeleteMetaInfo(person, UC.IpAddress, UC.ProxyIpAddress));
                                    evaluator.Evaluations.Where(t => t.Deleted == BaseStatusDeleted.None && item.Committees.Contains(t.Committee.Id)).ToList().ForEach(t => t.RecoverMetaInfo(person, UC.IpAddress, UC.ProxyIpAddress));
                                    foreach (long idCommittee in item.Committees.Where(i => !evaluator.Memberships.Select(t => t.Committee.Id).ToList().Contains(i)).ToList())
                                    {
                                        CommitteeMember member = new CommitteeMember();
                                        member.CreateMetaInfo(person, UC.IpAddress, UC.ProxyIpAddress);
                                        member.Committee = Manager.Get<EvaluationCommittee>(idCommittee);
                                        member.Evaluator = evaluator;
                                        member.Status = MembershipStatus.Standard;
                                        if (member.Committee != null)
                                        {
                                            Manager.SaveOrUpdate(member);
                                            evaluator.Memberships.Add(member);
                                        }
                                    }
                                }
                                Manager.SaveOrUpdate(evaluator);
                            }
                            Manager.Commit();
                            result = true;
                        }
                    }
                    catch (Exception ex)
                    {
                        if (Manager.IsInTransaction())
                            Manager.RollBack();
                    }
                    return result;
                }

                public Boolean SaveCommitteeAssignmentPolicy(long idCall, List<dtoCommitteeMember> evaluators, Boolean multipleCommittee)
                {
                    CallForPaper call = Manager.Get<CallForPaper>(idCall);
                    return SaveCommitteeAssignmentPolicy(call, evaluators, multipleCommittee);
                }
                public Boolean SaveCommitteeAssignmentPolicy(CallForPaper call, List<dtoCommitteeMember> evaluators, Boolean multipleCommittee)
                {
                    Boolean result = false;
                    try
                    {
                        litePerson person = Manager.GetLitePerson(UC.CurrentUserID);
                        if (call != null && person != null)
                        {
                            result = SaveCallEvaluators(call, evaluators);
                            if (result) {
                                Manager.BeginTransaction();
                                call.OneCommitteeMembership=  !multipleCommittee;
                                call.UpdateMetaInfo(person,UC.IpAddress, UC.ProxyIpAddress);
                                Manager.Commit();
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        if (result)
                            result = false;
                        if (Manager.IsInTransaction())
                            Manager.RollBack();
                    }
                    return result;
                }
      
                public Boolean VirtualDeleteEvaluator(long idEvaluator, Boolean delete, ref long outputIdEvaluator)
                {
                    Boolean result = false;
                    try
                    {
                        litePerson person = Manager.GetLitePerson(UC.CurrentUserID);
                        CallEvaluator evaluator = Manager.Get<CallEvaluator>(idEvaluator);
                        if (evaluator != null)
                        {
                            outputIdEvaluator = evaluator.Id;
                            if (evaluator.Call != null &&
                                 (evaluator.Call.Type == CallForPaperType.CallForBids && (from s in Manager.GetIQ<Evaluation>()
                                                                                          where s.Call.Id == evaluator.Call.Id && s.Deleted == BaseStatusDeleted.None && s.Status != Domain.Evaluation.EvaluationStatus.None 
                                                                                          select s.Id).Any())
                                )
                                throw new EvaluationStarted();
                            else
                            {
                                Manager.BeginTransaction();
                                evaluator.UpdateMetaInfo(person, UC.IpAddress, UC.ProxyIpAddress);
                                evaluator.Deleted = delete ? BaseStatusDeleted.Manual : BaseStatusDeleted.None;
                                foreach (CommitteeMember membership in evaluator.Memberships)
                                {
                                    membership.Deleted = delete ? (membership.Deleted | BaseStatusDeleted.Cascade) : (membership.Deleted = (BaseStatusDeleted)((int)membership.Deleted - (int)BaseStatusDeleted.Cascade));
                                    Manager.SaveOrUpdate(membership);
                                }
                                foreach (Evaluation evaluation in evaluator.Evaluations)
                                {
                                    evaluation.Deleted = delete ? (evaluation.Deleted | BaseStatusDeleted.Cascade) : (evaluation.Deleted = (BaseStatusDeleted)((int)evaluation.Deleted - (int)BaseStatusDeleted.Cascade));
                                    Manager.SaveOrUpdate(evaluation);
                                }
                                if (delete)
                                {
                                    var query = (from s in Manager.GetIQ<CallEvaluator>()
                                                 where s.Call.Id == evaluator.Call.Id && s.Deleted == BaseStatusDeleted.None && s.Id != evaluator.Id
                                                 select s);
                                    //outputIdEvaluator = (from s in query where s.Person.SurnameAndName < evaluator.Person.SurnameAndName orderby s.DisplayOrder descending select s.Id).FirstOrDefault();
                                    //if (outputIdEvaluator == 0)
                                        outputIdEvaluator = (from s in query where s.CreatedOn > evaluator.CreatedOn orderby s.CreatedOn select s.Id).FirstOrDefault();

                                }
                                Manager.SaveOrUpdate(evaluator);
                                Manager.Commit();
                                result = true;
                            }
                        }
                        else
                            result = true;
                    }
                    catch (EvaluationStarted ex)
                    {
                        throw ex;
                    }
                    catch (Exception ex)
                    {
                        Manager.RollBack();
                    }
                    return result;
                }

                public Dictionary<MembershipStatus, long> GetMembershipsStatistics(CallForPaper call)
                {
                    Dictionary<MembershipStatus, long> results = new Dictionary<MembershipStatus, long>();
                    try
                    {
                        var query = (from m in Manager.GetIQ<CommitteeMember>() where m.Deleted == BaseStatusDeleted.None && m.Committee.Call.Id == call.Id select m);
                        results[MembershipStatus.Replaced] = query.Where(q => q.Status == MembershipStatus.Replaced).Count();
                        results[MembershipStatus.Replacing] = query.Where(q => q.Status == MembershipStatus.Replacing).Count();
                        results[MembershipStatus.Standard] = query.Where(q => q.Status == MembershipStatus.Standard).Count();
                    }
                    catch (Exception ex)
                    {

                    }
                    return results;
                }
            #endregion

            #region "3 - Manage Submission Assignments"
                public Dictionary<SubmissionStatus,int> GetSubmissionsInfo(BaseForPaper call)
                {
                    Dictionary<SubmissionStatus, int> results = new Dictionary<SubmissionStatus, int>();
                    try
                    {
                        Manager.BeginTransaction();
                        var query = (from u in Manager.GetIQ<UserSubmission>() where u.Deleted == BaseStatusDeleted.None && u.Call.Id == call.Id select u.Status).ToList();
                        results = query.GroupBy(i => i).ToDictionary(i => i.Key, k => k.Count());
                        Manager.Commit();
                    }
                    catch (Exception ex)
                    {
                        Manager.RollBack();
                    }
                    return results;
                }
                public Dictionary<SubmissionStatus, int> GetSubmissionsInfoWithNoEvaluation(BaseForPaper call)
                {
                    Dictionary<SubmissionStatus, int> results = new Dictionary<SubmissionStatus, int>();
                    try
                    {
                        Manager.BeginTransaction();
                        List<long> idInEvaluations = (from u in Manager.GetIQ<Evaluation>() where u.Deleted == BaseStatusDeleted.None && u.Call.Id == call.Id select u.Submission.Id).Distinct().ToList();
                        var query = (from u in Manager.GetIQ<UserSubmission>() where u.Deleted == BaseStatusDeleted.None && u.Call.Id == call.Id select u).ToList();
                        if (idInEvaluations.Count<= maxItemsForQuery)
                            results = query.Where(s=> !idInEvaluations.Contains(s.Id)).Select(s=>s.Status).GroupBy(i => i).ToDictionary(i => i.Key, k => k.Count());
                        else
                            results = query.Select(s => new { IdSubmission = s.Id, Status = s.Status }).ToList().Where(s => !idInEvaluations.Contains(s.IdSubmission)).Select(s => s.Status).GroupBy(i => i).ToDictionary(i => i.Key, k => k.Count());
                        Manager.Commit();
                    }
                    catch (Exception ex)
                    {
                        Manager.RollBack();
                    }
                    return results;
                }
                public long GetSubmissionsCount(BaseForPaper call, SubmissionStatus status) {
                    long count = 0;
                    try
                    {
                        Manager.BeginTransaction();
                        count = (from u in Manager.GetIQ<UserSubmission>() where u.Deleted == BaseStatusDeleted.None && u.Call.Id == call.Id && (status == SubmissionStatus.none || u.Status == status) select u.Id).Count();
                        Manager.Commit();
                    }
                    catch (Exception ex) {
                        count = -1;
                        Manager.RollBack();
                    }
                    return count;
                }

                public Int32 GetCommitteeDisplayOrder(long idCommittee)
                {
                    Int32 displayOrder = 0;
                    try
                    {
                        displayOrder = (from c in Manager.GetIQ<EvaluationCommittee>() where c.Id == idCommittee select c.DisplayOrder).Skip(0).Take(1).ToList().FirstOrDefault();
                    }
                    catch (Exception ex)
                    {

                    }
                    return displayOrder;
                }
                public String GetCommitteeName( long idCommittee)
                {
                    String name = "";
                    try
                    {
                        name = (from c in Manager.GetIQ<EvaluationCommittee>() where c.Id == idCommittee select c.Name).Skip(0).Take(1).ToList().FirstOrDefault();
                    }
                    catch (Exception ex)
                    {

                    }
                    return name;
                }
                public List<dtoSubmissionAssignment> GetCommitteeAssignments(BaseForPaper call, long idCommittee)
                {
                    List<dtoSubmissionAssignment> items = new List<dtoSubmissionAssignment>();
                    try
                    {
                        var query = (from s in Manager.GetIQ<UserSubmission>()
                                     where s.Deleted == BaseStatusDeleted.None && s.Call.Id == call.Id && s.Status >= SubmissionStatus.accepted && s.Status != SubmissionStatus.rejected
                                     select s).ToList();
                        items = (from i in query
                                 select new dtoSubmissionAssignment()
                                 {
                                     DisplayName = (i.Owner!=null) ? i.Owner.SurnameAndName : "--",
                                     IdSubmission = i.Id,
                                     IdSubmitterType = i.Type.Id,
                                     SubmitterType = i.Type.Name,
                                     SubmittedOn= i.SubmittedOn,
                                     Evaluators = (from m in Manager.GetIQ<Evaluation>()
                                                   where m.Deleted == BaseStatusDeleted.None && m.Committee.Id == idCommittee && m.Submission.Id==i.Id 
                                                   select m.Evaluator.Id).ToList()
                                 }).ToList();
                    }
                    catch (Exception ex)
                    {

                    }
                    return items.OrderBy(s => s.SubmitterType).ThenBy(s=>s.DisplayName).ToList();
                }
                public List<dtoSubmissionAssignment> GetCommitteeAssignmentsForNoEvaluations(BaseForPaper call, long idCommittee,List<CommitteeMember> members)
                {
                    List<dtoSubmissionAssignment> items = new List<dtoSubmissionAssignment>();
                    try
                    {
                        List<long> idInEvaluations = (from u in Manager.GetIQ<Evaluation>() where u.Deleted == BaseStatusDeleted.None && u.Call.Id == call.Id select u.Submission.Id).Distinct().ToList();
                        var query = (from s in Manager.GetIQ<UserSubmission>()
                                     where s.Deleted == BaseStatusDeleted.None && s.Call.Id == call.Id && s.Status >= SubmissionStatus.accepted && s.Status != SubmissionStatus.rejected
                                     select s);
                        if (idInEvaluations.Count <= maxItemsForQuery)
                            query = query.Where(s => !idInEvaluations.Contains(s.Id));
                        items = (from i in query.ToList().Where(s => !idInEvaluations.Contains(s.Id)).ToList()
                                 select new dtoSubmissionAssignment()
                                 {
                                     DisplayName = (i.Owner != null) ? i.Owner.SurnameAndName : "--",
                                     IdSubmission = i.Id,
                                     IdSubmitterType = i.Type.Id,
                                     SubmitterType = i.Type.Name,
                                     SubmittedOn = i.SubmittedOn,
                                     Evaluators = members.Where(m=>m.Deleted== BaseStatusDeleted.None && (m.Status== MembershipStatus.Standard || m.Status== MembershipStatus.Replacing) && m.Evaluator != null && m.Evaluator.Person != null && m.Evaluator.Deleted== BaseStatusDeleted.None).Select(m=>m.Evaluator.Id).ToList()
                                 }).ToList();
                    }
                    catch (Exception ex)
                    {

                    }
                    return items.OrderBy(s => s.SubmitterType).ThenBy(s => s.DisplayName).ToList();
                }

                public List<dtoBaseSubmission> GetSubmissionsForNoEvaluations(long idCall, SubmissionsOrder orderBy, Boolean ascending)
                {
                    List<dtoBaseSubmission> items = null;
                    List<long> idInEvaluations = (from u in Manager.GetIQ<Evaluation>() where u.Deleted == BaseStatusDeleted.None && u.Call.Id == idCall select u.Submission.Id).Distinct().ToList();
                    var query = (from s in Manager.GetIQ<UserSubmission>()
                                 where s.Deleted == BaseStatusDeleted.None && s.Call.Id == idCall && s.Status >= SubmissionStatus.accepted && s.Status != SubmissionStatus.rejected
                                 select s);

                    if (orderBy == SubmissionsOrder.None)
                    {
                        ascending = false;
                        orderBy = SubmissionsOrder.ByDate;
                    }

                    switch (orderBy)
                    {
                        case SubmissionsOrder.BySubmittedOn:
                            if (ascending)
                                query = query.OrderBy(s => s.SubmittedOn);
                            else
                                query = query.OrderByDescending(s => s.SubmittedOn);
                            break;
                        case SubmissionsOrder.ByDate:
                            if (ascending)
                                query = query.OrderBy(r => r.ModifiedOn);
                            else
                                query = query.OrderByDescending(r => r.ModifiedOn);
                            break;
                        case SubmissionsOrder.ByType:
                            if (ascending)
                                query = query.OrderBy(s => s.Type.Name);
                            else
                                query = query.OrderByDescending(s => s.Type.Name);
                            break;
                        case SubmissionsOrder.ByUser:
                            if (ascending)
                                query = query.OrderBy(s => s.Owner.Surname).ThenBy(s => s.Owner.Name);
                            else
                                query = query.OrderByDescending(s => s.Owner.Surname).ThenBy(s => s.Owner.Name);
                            break;
                    }

                    if (idInEvaluations.Count <= maxItemsForQuery)
                        query = query.Where(s => !idInEvaluations.Contains(s.Id));

                    items = (from r in query.ToList().Where(s => !idInEvaluations.Contains(s.Id)).ToList()
                             select new dtoBaseSubmission()
                             {
                                 Type = new dtoSubmitterType() { Name = r.Type.Name, Id= r.Type.Id  },
                                 Status = r.Status,
                                 Deleted = r.Deleted,
                                 Id = r.Id,
                                 ExtensionDate = r.ExtensionDate,
                                 SubmittedOn = r.SubmittedOn,
                                 ModifiedOn = r.ModifiedOn,
                                 Owner = r.Owner,
                                 IsAnonymous = r.isAnonymous
                             }).ToList();

                    return items;
                }
                public List<dtoSubmissionMultipleAssignment> GetCommitteesAssignments(BaseForPaper call, List<dtoBaseSubmission> submissions)
                {
                    List<dtoSubmissionMultipleAssignment> items = new List<dtoSubmissionMultipleAssignment>();
                    try
                    {
                        List<EvaluationCommittee> committees = (from c in Manager.GetIQ<EvaluationCommittee>() where c.Deleted== BaseStatusDeleted.None && c.Call.Id==call.Id select c).ToList();
                        if (submissions != null && submissions.Count > 0) {
                            items = (from s in submissions
                                     select new dtoSubmissionMultipleAssignment()
                                         {
                                            DisplayName= (s.Owner != null && !s.IsAnonymous) ? s.Owner.SurnameAndName : "--",
                                            IdSubmission = s.Id,
                                            IdSubmitterType =s.Type.Id,
                                            SubmitterType= s.Type.Name,
                                            SubmittedOn= s.SubmittedOn, 
                                         }).ToList();
                            foreach (EvaluationCommittee c in committees.Where(c=> c.ForAllSubmittersType).ToList()) {
                                items.ForEach(i => i.Committees.Add(new dtoCommitteeAssignment()
                                {
                                    IdCommittee = c.Id,
                                    IdSubmission = i.IdSubmission,
                                    Name = c.Name,
                                    Evaluators = (from m in Manager.GetIQ<Evaluation>()
                                                  where m.Deleted == BaseStatusDeleted.None && m.Committee.Id == c.Id && m.Submission.Id == i.IdSubmission
                                                  select m.Evaluator.Id).ToList()
                                }));
                            }
                            foreach (EvaluationCommittee c in committees.Where(c => !c.ForAllSubmittersType && c.AssignedTypes.Where(a=> a.Deleted== BaseStatusDeleted.None).Any()).ToList())
                            {
                                items.Where(i=> c.AssignedTypes.Where(a=> a.Deleted== BaseStatusDeleted.None && a.SubmitterType.Id== i.IdSubmitterType).Any()).ToList().ForEach(i => i.Committees.Add(new dtoCommitteeAssignment()
                                {
                                    IdCommittee = c.Id,
                                    IdSubmission = i.IdSubmission,
                                    Name = c.Name,
                                    Evaluators = (from m in Manager.GetIQ<Evaluation>()
                                                  where m.Deleted == BaseStatusDeleted.None && m.Committee.Id == c.Id && m.Submission.Id == i.IdSubmission
                                                  select m.Evaluator.Id).ToList()
                                }));
                            }
                            items.Where(i => i.Committees.Count == 1).ToList().ForEach(i => i.Committees[0].Display = displayAs.first);
                            items.Where(i => i.Committees.Count > 1).ToList().ForEach(i => i.Committees[0].Display = displayAs.first);
                            items.Where(i => i.Committees.Count > 1).ToList().ForEach(i => i.Committees.Last().Display = displayAs.last);
                        }
                    }
                    catch (Exception ex)
                    {

                    }
                    return items;
                }
                public List<dtoSubmissionMultipleAssignment> GetCommitteesAssignmentsForNoEvaluations(BaseForPaper call, List<dtoBaseSubmission> submissions)
                {
                    List<dtoSubmissionMultipleAssignment> items = new List<dtoSubmissionMultipleAssignment>();
                    try
                    {
                        List<EvaluationCommittee> committees = (from c in Manager.GetIQ<EvaluationCommittee>() where c.Deleted == BaseStatusDeleted.None && c.Call.Id == call.Id select c).ToList();
                        if (submissions != null && submissions.Count > 0)
                        {
                            items = (from s in submissions
                                     select new dtoSubmissionMultipleAssignment()
                                     {
                                         DisplayName = (s.Owner != null && !s.IsAnonymous) ? s.Owner.SurnameAndName : "--",
                                         IdSubmission = s.Id,
                                         IdSubmitterType = s.Type.Id,
                                         SubmitterType = s.Type.Name,
                                         SubmittedOn = s.SubmittedOn,
                                     }).ToList();
                            foreach (EvaluationCommittee c in committees.Where(c => c.ForAllSubmittersType).ToList())
                            {
                                items.ForEach(i => i.Committees.Add(new dtoCommitteeAssignment()
                                {
                                    IdCommittee = c.Id,
                                    IdSubmission = i.IdSubmission,
                                    Name = c.Name,
                                    Evaluators = c.Members.Where(m => m.Deleted == BaseStatusDeleted.None && (m.Status == MembershipStatus.Standard || m.Status == MembershipStatus.Replacing) && m.Evaluator != null && m.Evaluator.Person != null && m.Evaluator.Deleted == BaseStatusDeleted.None).Select(m => m.Evaluator.Id).ToList()
                                }));
                            }
                            foreach (EvaluationCommittee c in committees.Where(c => !c.ForAllSubmittersType && c.AssignedTypes.Where(a => a.Deleted == BaseStatusDeleted.None).Any()).ToList())
                            {
                                items.Where(i => c.AssignedTypes.Where(a => a.Deleted == BaseStatusDeleted.None && a.SubmitterType.Id == i.IdSubmitterType).Any()).ToList().ForEach(i => i.Committees.Add(new dtoCommitteeAssignment()
                                {
                                    IdCommittee = c.Id,
                                    IdSubmission = i.IdSubmission,
                                    Name = c.Name,
                                    Evaluators = c.Members.Where(m => m.Deleted == BaseStatusDeleted.None && (m.Status == MembershipStatus.Standard || m.Status == MembershipStatus.Replacing) && m.Evaluator != null && m.Evaluator.Person != null && m.Evaluator.Deleted == BaseStatusDeleted.None).Select(m => m.Evaluator.Id).ToList()
                                }));
                            }
                            items.Where(i => i.Committees.Count == 1).ToList().ForEach(i => i.Committees[0].Display = displayAs.first);
                            items.Where(i => i.Committees.Count > 1).ToList().ForEach(i => i.Committees[0].Display = displayAs.first);
                            items.Where(i => i.Committees.Count > 1).ToList().ForEach(i => i.Committees.Last().Display = displayAs.last);
                        }
                    }
                    catch (Exception ex)
                    {

                    }
                    return items;
                }
                public Boolean SaveSubmissionsAssignments(long idCall, long idcommittee, List<dtoSubmissionAssignment> assignments)
                {
                    BaseForPaper call = Manager.Get<BaseForPaper>(idCall);
                    EvaluationCommittee committee = Manager.Get<EvaluationCommittee>(idcommittee);
                    if (committee != null && call !=null )
                        return SaveSubmissionsAssignments(call,CallUseDss(idCall), committee, assignments);
                    else
                        return false;
                }
                public Boolean SaveSubmissionsAssignments(BaseForPaper call,Boolean useDss, EvaluationCommittee committee, List<dtoSubmissionAssignment> assignments)
                {
                    Boolean result = false;
                    try
                    {
                        litePerson person = Manager.GetLitePerson(UC.CurrentUserID);
                        if (call != null && person != null && committee!=null )
                        {
                            Manager.BeginTransaction();
                            foreach (dtoSubmissionAssignment item in assignments)
                            {
                                //List<Evaluation> evaluations = (from e in Manager.GetIQ<Evaluation>() where e.Deleted== BaseStatusDeleted.None && e.Call.Id== call.Id
                                //                           && e.Submission.Id == item.IdSubmission select e).ToList();
                                List<Evaluation> evaluations = (from e in Manager.GetIQ<Evaluation>() where e.Submission.Id == item.IdSubmission && (e.Deleted == BaseStatusDeleted.None || (e.Deleted != BaseStatusDeleted.None && e.Status == Domain.Evaluation.EvaluationStatus.None)) select e).ToList();
                                foreach (long idEvaluator in item.Evaluators) {
                                    Evaluation evaluation = evaluations.Where(e => e.Evaluator.Id == idEvaluator).FirstOrDefault();
                                    if (evaluation==null ) {
                                        evaluation = new Evaluation();
                                        evaluation.CreateMetaInfo(person, UC.IpAddress, UC.ProxyIpAddress);
                                        evaluation.Evaluator= Manager.Get<CallEvaluator>(idEvaluator);
                                        evaluation.Submission = Manager.Get<UserSubmission>(item.IdSubmission);
                                        evaluation.Call = call;
                                        evaluation.UseDss = useDss;
                                        evaluation.Community = call.Community;
                                        evaluation.Comment = "";
                                        evaluation.Committee = committee;
                                        evaluation.Evaluated = false;
                                        evaluation.Status = Domain.Evaluation.EvaluationStatus.None;
                                        Manager.SaveOrUpdate(evaluation);
                                    }
                                    else if (evaluation.Deleted!= BaseStatusDeleted.None){
                                        evaluation.RecoverMetaInfo(person, UC.IpAddress, UC.ProxyIpAddress);
                                        Manager.SaveOrUpdate(evaluation);
                                    }

                                }
                                evaluations.Where(t => t.Deleted == BaseStatusDeleted.None && !item.Evaluators.Contains(t.Evaluator.Id)).ToList().ForEach(t => t.SetDeleteMetaInfo(person, UC.IpAddress, UC.ProxyIpAddress));
                            }
                            Manager.Commit();
                            result = true;
                        }
                    }
                    catch (Exception ex)
                    {
                        if (Manager.IsInTransaction())
                            Manager.RollBack();
                    }
                    return result;
                }

                public Boolean SaveSubmissionsAssignments(long idCall, List<dtoSubmissionMultipleAssignment> assignments)
                {
                    BaseForPaper call = Manager.Get<BaseForPaper>(idCall);
                    if (call !=null )
                        return SaveSubmissionsAssignments(call, CallUseDss(idCall), assignments);
                    else
                        return false;
                }
                public Boolean SaveSubmissionsAssignments(BaseForPaper call,Boolean useDss, List<dtoSubmissionMultipleAssignment> assignments)
                {
                    Boolean result = false;
                    try
                    {
                        litePerson person = Manager.GetLitePerson(UC.CurrentUserID);
                        if (call != null && person != null)
                        {
                            Manager.BeginTransaction();
                            List<dtoCommitteeAssignment> items = new List<dtoCommitteeAssignment>();
                            assignments.ForEach(a=> items.AddRange(a.Committees));
                              var query = (from e in Manager.GetIQ<Evaluation>() where (e.Deleted == BaseStatusDeleted.None || (e.Deleted != BaseStatusDeleted.None && e.Status == Domain.Evaluation.EvaluationStatus.None)) select e);
                            foreach (dtoCommitteeAssignment cAssignment in items)
                            {
                                List<Evaluation> evaluations = query.Where(e => e.Committee.Id == cAssignment.IdCommittee && e.Submission.Id == cAssignment.IdSubmission).ToList();
                                foreach (long idEvaluator in cAssignment.Evaluators)
                                {
                                    Evaluation evaluation = evaluations.Where(e => e.Evaluator.Id == idEvaluator).FirstOrDefault();
                                    if (evaluation == null)
                                    {
                                        evaluation = new Evaluation();
                                        evaluation.CreateMetaInfo(person, UC.IpAddress, UC.ProxyIpAddress);
                                        evaluation.Evaluator = Manager.Get<CallEvaluator>(idEvaluator);
                                        evaluation.Submission = Manager.Get<UserSubmission>(cAssignment.IdSubmission);
                                        evaluation.Call = call;
                                        evaluation.UseDss = useDss;
                                        evaluation.Comment = "";
                                        evaluation.Committee = Manager.Get<EvaluationCommittee>(cAssignment.IdCommittee);
                                        evaluation.Community = call.Community;
                                        evaluation.Evaluated = false;
                                        evaluation.Status = Domain.Evaluation.EvaluationStatus.None;
                                        if (evaluation.Committee !=null && evaluation.Submission !=null)
                                            Manager.SaveOrUpdate(evaluation);
                                    }
                                    else if (evaluation.Deleted != BaseStatusDeleted.None)
                                    {
                                        evaluation.RecoverMetaInfo(person, UC.IpAddress, UC.ProxyIpAddress);
                                        Manager.SaveOrUpdate(evaluation);
                                    }
                                    evaluations.Where(t => t.Deleted == BaseStatusDeleted.None && !cAssignment.Evaluators.Contains(t.Evaluator.Id)).ToList().ForEach(t => t.SetDeleteMetaInfo(person, UC.IpAddress, UC.ProxyIpAddress));
                                }   
                            }
                            Manager.Commit();
                            result = true;
                        }
                    }
                    catch (Exception ex)
                    {
                        if (Manager.IsInTransaction())
                            Manager.RollBack();
                    }
                    return result;
                }


        public Boolean SaveDefaultFirstAssignments(long idCall, long idcommittee)
        {
            BaseForPaper call = Manager.Get<BaseForPaper>(idCall);
            EvaluationCommittee committee = Manager.Get<EvaluationCommittee>(idcommittee);
            if (committee != null && call != null)
            {
                Boolean result = false;
                try
                {
                    litePerson person = Manager.GetLitePerson(UC.CurrentUserID);
                    if (call != null && person != null && committee != null)
                    {
                        Boolean useDss = CallUseDss(call.Id);

                        Manager.BeginTransaction();
                        
                        List<long> idTypes = committee.AssignedTypes.Where(a => a.Deleted == BaseStatusDeleted.None).Select(a => a.SubmitterType.Id).ToList();
                        var evaluations = (from e in Manager.GetIQ<Evaluation>()
                                           where (e.Deleted == BaseStatusDeleted.None || (e.Deleted != BaseStatusDeleted.None && e.Status == Domain.Evaluation.EvaluationStatus.None)) && e.Call.Id == call.Id
                                               && e.Committee.Id == committee.Id
                                           select e).ToList();

                        UserSubmission FirstSubmission = (from e in Manager.GetIQ<UserSubmission>()
                                                            where e.Deleted == BaseStatusDeleted.None && e.Call.Id == call.Id
                                                            && e.Status == SubmissionStatus.accepted
                                                                && (committee.ForAllSubmittersType || idTypes.Contains(e.Type.Id))
                                                            select e).Skip(0).Take(1).ToList().FirstOrDefault();

                        CommitteeMember member = committee.Members.FirstOrDefault();

                        if(FirstSubmission != null && member != null)
                        {
                            Evaluation evaluation = evaluations.Where(
                                e => e.Committee.Id == committee.Id 
                                && e.Evaluator.Id == member.Evaluator.Id 
                                && e.Submission.Id == FirstSubmission.Id).FirstOrDefault();

                            if (evaluation == null)
                            {
                                evaluation = new Evaluation();
                                evaluation.CreateMetaInfo(person, UC.IpAddress, UC.ProxyIpAddress);
                                evaluation.Evaluator = member.Evaluator;
                                evaluation.Submission = FirstSubmission;
                                evaluation.Call = call;
                                evaluation.UseDss = useDss;
                                evaluation.Community = call.Community;
                                evaluation.Comment = "";
                                evaluation.Committee = committee;
                                evaluation.Evaluated = false;
                                evaluation.Status = Domain.Evaluation.EvaluationStatus.None;
                                Manager.SaveOrUpdate(evaluation);
                            }
                            else if (evaluation.Deleted != BaseStatusDeleted.None)
                            {
                                evaluation.RecoverMetaInfo(person, UC.IpAddress, UC.ProxyIpAddress);
                                Manager.SaveOrUpdate(evaluation);
                            }
                        }

                        Manager.Commit();
                        result = true;
                    }
                }
                catch (Exception ex)
                {
                    if (Manager.IsInTransaction())
                        Manager.RollBack();
                }
                return result;
            }
            else
                return false;
        }


        public Boolean SaveDefaultAssignments(long idCall, long idcommittee)
                {
                    BaseForPaper call = Manager.Get<BaseForPaper>(idCall);
                    EvaluationCommittee committee = Manager.Get<EvaluationCommittee>(idcommittee);
                    if (committee != null && call != null)
                        return SaveDefaultAssignments(call, committee);
                    else
                        return false;
                }

       
        public Boolean SaveDefaultAssignments(BaseForPaper call, EvaluationCommittee committee)
                {
                    Boolean result = false;
                    try
                    {
                        litePerson person = Manager.GetLitePerson(UC.CurrentUserID);
                        if (call != null && person != null && committee != null)
                        {
                            Manager.BeginTransaction();
                            SaveDefaultAssignments(person, call, CallUseDss(call.Id), committee);
                            Manager.Commit();
                            result = true;
                        }
                    }
                    catch (Exception ex)
                    {
                        if (Manager.IsInTransaction())
                            Manager.RollBack();
                    }
                    return result;
                }


                public Boolean SaveDefaultAssignments(long idCall)
                {
                    BaseForPaper call = Manager.Get<BaseForPaper>(idCall);
                    if (call != null)
                        return SaveDefaultAssignments(call);
                    else
                        return false;
                }
                public Boolean SaveDefaultAssignments(BaseForPaper call)
                {
                    Boolean result = false;
                    try
                    {
                        litePerson person = Manager.GetLitePerson(UC.CurrentUserID);
                        if (call != null && person != null )
                        {
                            Manager.BeginTransaction();
                            Boolean useDss = CallUseDss(call.Id);
                            var committees = (from c in Manager.GetIQ<EvaluationCommittee>() where c.Call.Id == call.Id && c.Deleted == BaseStatusDeleted.None select c);
                            foreach (EvaluationCommittee committee in committees)
                            {
                                SaveDefaultAssignments(person, call,useDss, committee);
                            }
                            
                            Manager.Commit();
                            result = true;
                        }
                    }
                    catch (Exception ex)
                    {
                        if (Manager.IsInTransaction())
                            Manager.RollBack();
                    }
                    return result;
                }

                private void SaveDefaultAssignments(litePerson person,BaseForPaper call,Boolean useDss, EvaluationCommittee committee)
                {
                    List<long> idTypes = committee.AssignedTypes.Where(a => a.Deleted == BaseStatusDeleted.None).Select(a => a.SubmitterType.Id).ToList();
                    var evaluations = (from e in Manager.GetIQ<Evaluation>()
                                        where (e.Deleted == BaseStatusDeleted.None || (e.Deleted != BaseStatusDeleted.None && e.Status == Domain.Evaluation.EvaluationStatus.None)) && e.Call.Id == call.Id
                                            && e.Committee.Id == committee.Id
                                        select e).ToList();
                    List<UserSubmission> submissions = (from e in Manager.GetIQ<UserSubmission>()
                                                        where e.Deleted == BaseStatusDeleted.None && e.Call.Id == call.Id
                                                        && e.Status== SubmissionStatus.accepted 
                                                            && (committee.ForAllSubmittersType || idTypes.Contains(e.Type.Id))
                                                        select e).ToList();

                    foreach (CommitteeMember member in committee.Members.Where(m=>m.Deleted== BaseStatusDeleted.None).ToList())
                    {
                        foreach (UserSubmission sub in submissions)
                        {
                            Evaluation evaluation = evaluations.Where(e => e.Committee.Id == committee.Id && e.Evaluator.Id == member.Evaluator.Id && e.Submission.Id == sub.Id).FirstOrDefault();

                            if (evaluation == null)
                            {
                                evaluation = new Evaluation();
                                evaluation.CreateMetaInfo(person, UC.IpAddress, UC.ProxyIpAddress);
                                evaluation.Evaluator = member.Evaluator;
                                evaluation.Submission = sub;
                                evaluation.Call = call;
                                evaluation.UseDss = useDss;
                                evaluation.Community = call.Community;
                                evaluation.Comment = "";
                                evaluation.Committee = committee;
                                evaluation.Evaluated = false;
                                evaluation.Status = Domain.Evaluation.EvaluationStatus.None;
                                Manager.SaveOrUpdate(evaluation);
                            }
                            else if (evaluation.Deleted != BaseStatusDeleted.None)
                            {
                                evaluation.RecoverMetaInfo(person, UC.IpAddress, UC.ProxyIpAddress);
                                Manager.SaveOrUpdate(evaluation);
                            }
                        }
                    }
                           
                }

                public Boolean SetEvaluatorsToAllSubmissions(long idCall, long idcommittee)
                {
                    BaseForPaper call = Manager.Get<BaseForPaper>(idCall);
                    EvaluationCommittee committee = Manager.Get<EvaluationCommittee>(idcommittee);
                    if (committee != null && call != null)
                        return SetEvaluatorsToAllSubmissions(call, committee);
                    else
                        return false;
                }
                public Boolean SetEvaluatorsToAllSubmissions(BaseForPaper call, EvaluationCommittee committee)
                {
                    Boolean result = false;
                    try
                    {
                        litePerson person = Manager.GetLitePerson(UC.CurrentUserID);
                        if (call != null && person != null && committee != null)
                        {
                            Manager.BeginTransaction();
                            SetEvaluatorsToAllSubmissions(person, call, CallUseDss(call.Id), committee);
                            Manager.Commit();
                            result = true;
                        }
                    }
                    catch (Exception ex)
                    {
                        if (Manager.IsInTransaction())
                            Manager.RollBack();
                    }
                    return result;
                }

                public Boolean SetEvaluatorsToAllSubmissions(long idCall)
                {
                    BaseForPaper call = Manager.Get<BaseForPaper>(idCall);
                    if (call != null)
                        return SetEvaluatorsToAllSubmissions(call);
                    else
                        return false;
                }
                public Boolean SetEvaluatorsToAllSubmissions(BaseForPaper call)
                {
                    Boolean result = false;
                    try
                    {
                        litePerson person = Manager.GetLitePerson(UC.CurrentUserID);
                        if (call != null && person != null)
                        {
                            Manager.BeginTransaction();
                            Boolean useDss = CallUseDss(call.Id);
                            var committees = (from c in Manager.GetIQ<EvaluationCommittee>() where c.Call.Id == call.Id && c.Deleted == BaseStatusDeleted.None select c);
                            foreach (EvaluationCommittee committee in committees)
                            {
                                SetEvaluatorsToAllSubmissions(person, call, useDss, committee);
                            }
                            Manager.Commit();
                            result = true;
                        }
                    }
                    catch (Exception ex)
                    {
                        if (Manager.IsInTransaction())
                            Manager.RollBack();
                    }
                    return result;
                }
                private void SetEvaluatorsToAllSubmissions(litePerson person,BaseForPaper call,Boolean useDss, EvaluationCommittee committee)
                {
                   
                    List<long> idTypes = committee.AssignedTypes.Where(a => a.Deleted == BaseStatusDeleted.None).Select(a => a.SubmitterType.Id).ToList();

                    List<UserSubmission> submissions = (from e in Manager.GetIQ<UserSubmission>()
                                                        where e.Deleted == BaseStatusDeleted.None && e.Call.Id == call.Id
                                                        && e.Status== SubmissionStatus.accepted
                                                            && (committee.ForAllSubmittersType || idTypes.Contains(e.Type.Id))
                                                        select e).ToList();
                    foreach (UserSubmission sub in submissions)
                    {
                        var query = (from e in Manager.GetIQ<Evaluation>() where e.Submission.Id == sub.Id && e.Committee.Id == committee.Id && (e.Deleted == BaseStatusDeleted.None || (e.Deleted != BaseStatusDeleted.None && e.Status == Domain.Evaluation.EvaluationStatus.None)) select e);
                        foreach (CommitteeMember member in committee.Members.Where(m=>m.Deleted == BaseStatusDeleted.None).ToList())
                        {
                            Evaluation evaluation = query.Where(e => e.Evaluator.Id == member.Evaluator.Id).FirstOrDefault();
                            if (evaluation == null)
                            {
                                evaluation = new Evaluation();
                                evaluation.CreateMetaInfo(person, UC.IpAddress, UC.ProxyIpAddress);
                                evaluation.Evaluator = member.Evaluator;
                                evaluation.Submission = sub;
                                evaluation.Call = call;
                                evaluation.UseDss = useDss;
                                evaluation.Community = call.Community;
                                evaluation.Comment = "";
                                evaluation.Committee = committee;
                                evaluation.Evaluated = false;
                                evaluation.Status = Domain.Evaluation.EvaluationStatus.None;
                                Manager.SaveOrUpdate(evaluation);
                            }
                            else if (evaluation.Deleted != BaseStatusDeleted.None)
                            {
                                evaluation.RecoverMetaInfo(person, UC.IpAddress, UC.ProxyIpAddress);
                                Manager.SaveOrUpdate(evaluation);
                            }
                        }
                    }
                }
            #endregion

    #region "4 - View Evaluators"
        public Boolean EvaluationsCompleted(long idMembership)
        {
            Boolean result = false;
            try
            {
                CommitteeMember member = Manager.Get<CommitteeMember>(idMembership);
                if (member != null && member.Committee != null)
                    result = !(from e in Manager.GetIQ<Evaluation>()
                            where e.Deleted== BaseStatusDeleted.None && (e.Status== Domain.Evaluation.EvaluationStatus.Evaluating || e.Status== Domain.Evaluation.EvaluationStatus.None)
                            && e.Committee.Id== member.Committee.Id && e.Evaluator.Id == member.Evaluator.Id select e.Id).Any();
            }
            catch (Exception ex)
            {

            }
            return result;
        }
        public List<long>  GetIdCommitteesWithEvaluationCompleted(long idCall)
        {
            List<long> items = new List<long>();
            try
            {
                var committees = (from c in Manager.GetIQ<EvaluationCommittee>() where c.Deleted == BaseStatusDeleted.None && c.Call.Id == idCall select c.Id);
                var evaluations = (from e in Manager.GetIQ<Evaluation>() where e.Deleted== BaseStatusDeleted.None select e);
                items.AddRange(committees.ToList().Where(i => !evaluations.Where(e => e.Committee.Id == i && e.Status != Domain.Evaluation.EvaluationStatus.Evaluating).Any()).ToList());
            }
            catch (Exception ex) { 
            
            }
            return items;
        }
        public List<dtoCommitteeEvaluators> GetCommitteesEvaluationInfo(BaseForPaper call)
        {
            List<dtoCommitteeEvaluators> items = new List<dtoCommitteeEvaluators>();
            try
            {
                items = (from s in Manager.GetIQ<EvaluationCommittee>()
                            where s.Deleted == BaseStatusDeleted.None && s.Call == call
                            select new dtoCommitteeEvaluators()
                            {
                                Id = s.Id,
                                Name = s.Name,
                                Description = s.Description,
                                DisplayOrder = s.DisplayOrder,
                                ForAllSubmittersType = s.ForAllSubmittersType
                            }).ToList();
                List<long> idCommitees = items.Select(c => c.Id).ToList();
                List<lm.Comol.Modules.CallForPapers.Domain.Evaluation.dtoBaseEvaluatorStatistics> evaluators = (from bc in Manager.GetIQ<CommitteeMember>()
                                        where bc.Committee != null && idCommitees.Contains(bc.Committee.Id) && bc.Deleted == BaseStatusDeleted.None
                                        select bc).ToList().Select(bc => new lm.Comol.Modules.CallForPapers.Domain.Evaluation.dtoBaseEvaluatorStatistics(bc)).ToList();

                var query = (from e in Manager.GetIQ<Evaluation>()
                                where e.Call.Id == call.Id && e.Deleted == BaseStatusDeleted.None
                                select e);

                evaluators.ForEach(i => i.Counters = GetEvaluatorStatistics(i.IdCallEvaluator, i.IdCommittee, query));
                //items.Where(i => !i.ForAllSubmittersType).ToList().ForEach(c => c.Submitters = (from s in Manager.GetIQ<CommitteeAssignedSubmitterType>()
                //                                                                                where s.Deleted == BaseStatusDeleted.None && s.SubmitterType != null && s.Committee != null && s.Committee.Id == c.Id
                //    select s.SubmitterType.Id).ToList());

                // items.ForEach(i => i.Evaluators = (from e in evaluators where e.IdCommittee == i.Id orderby e.DisplayName select e).ToList());
                        
                foreach (dtoCommitteeEvaluators item in items) {
                    //item.Evaluators.AddRange((from e in evaluators where e.IdCommittee == item.Id && (e.Status== MembershipStatus.Standard || e.Status== MembershipStatus.Replacing) orderby e.DisplayName select e).ToList());
                    //foreach (dtoBaseEvaluatorStatistics evaluator in item.Evaluators.Where(e=> e.Status== MembershipStatus.Replacing)){
                                
                    //}
                    (from e in evaluators where e.IdCommittee == item.Id && (e.Status== MembershipStatus.Standard || e.Status== MembershipStatus.Replacing) 
                        orderby e.DisplayName select e).ToList().ForEach(e=> item.Evaluators.AddRange(ReorderEvaluators(e, (from i in evaluators where i.IdCommittee == item.Id select i).ToList())));
                    item.Evaluators.AddRange((from e in evaluators where e.Status == MembershipStatus.Removed && e.IdCommittee == item.Id orderby e.DisplayName select e).ToList());
                }
                items.Where(i => i.Evaluators.Count > 1).ToList().ForEach(i => i.Evaluators.Last().Display = displayAs.last);

            }
            catch (Exception ex)
            {

            }
            return items.OrderBy(s => s.DisplayOrder).ThenBy(s => s.Name).ToList();
        }


        public List<dtoCommitteeEvaluationsInfo> GetCommitteesEvaluationInfo(long idCall, long idEvaluator)
        {
            List<dtoCommitteeEvaluationsInfo> items = new List<dtoCommitteeEvaluationsInfo>();
            
            try
            {

                List<long> idCommittees = (from m in Manager.GetIQ<CommitteeMember>()
                                            where m.Deleted == BaseStatusDeleted.None 
                                            && m.Status != MembershipStatus.Removed 
                                            && m.Evaluator.Id == idEvaluator
                                            select m.Committee.Id).ToList();


                items = (from s in Manager.GetIQ<EvaluationCommittee>()
                            where s.Deleted == BaseStatusDeleted.None && idCommittees.Contains(s.Id)
                            select new dtoCommitteeEvaluationsInfo()
                            {
                                IdCommittee = s.Id,
                                Name = s.Name,
                                Description = s.Description,
                                DisplayOrder = s.DisplayOrder,
                                IdEvaluator = idEvaluator,
                                isFuzzy = s.WeightSettings.IsFuzzyWeight 
                            }).ToList();
                var query = (from e in Manager.GetIQ<Evaluation>()
                                where e.Call.Id == idCall && e.Deleted == BaseStatusDeleted.None && e.Evaluator.Id == idEvaluator
                                select e);

                items.ForEach(i => i.Counters = GetEvaluatorStatistics(idEvaluator, i.IdCommittee, query));
            }
            catch (Exception ex)
            {

            }
            return items.OrderBy(s => s.DisplayOrder).ThenBy(s => s.Name).ToList();
        }


        


        private List<dtoBaseEvaluatorStatistics> ReorderEvaluators(dtoBaseEvaluatorStatistics evaluator,List<dtoBaseEvaluatorStatistics> evaluators) {
            List<dtoBaseEvaluatorStatistics> items = new List<dtoBaseEvaluatorStatistics>();
            items.Add(evaluator);
            dtoBaseEvaluatorStatistics child = evaluators.Where(e => evaluator.ReplacingEvaluator != null && e.IdCallEvaluator == evaluator.ReplacingEvaluator.Id).FirstOrDefault();
            if (child != null)
                items.AddRange(ReorderEvaluators(child,evaluators));
            return items;
        }
            
        public Dictionary<Domain.Evaluation.EvaluationStatus, long> GetEvaluatorStatistics(CommitteeMember membership)
        {
            Dictionary<Domain.Evaluation.EvaluationStatus, long> results = new Dictionary<Domain.Evaluation.EvaluationStatus, long>();
            try
            {
                var query = (from e in Manager.GetIQ<Evaluation>() where e.Deleted == BaseStatusDeleted.None && e.Committee.Id == membership.Committee.Id && e.Evaluator.Id == membership.Evaluator.Id select e);

                results[Domain.Evaluation.EvaluationStatus.Evaluated] = query.Where(e =>  e.Status == Domain.Evaluation.EvaluationStatus.Evaluated).Count();
                results[Domain.Evaluation.EvaluationStatus.Evaluating] = query.Where(e => e.Status == Domain.Evaluation.EvaluationStatus.Evaluating).Count();
                results[Domain.Evaluation.EvaluationStatus.None] = query.Where(e => e.Status == Domain.Evaluation.EvaluationStatus.None).Count();
                results[Domain.Evaluation.EvaluationStatus.EvaluatorReplacement] = query.Where(e => e.Status == Domain.Evaluation.EvaluationStatus.EvaluatorReplacement).Count();
            }
            catch (Exception ex)
            {

            }
            return results;
        }
        private Dictionary<Domain.Evaluation.EvaluationStatus, long> GetEvaluatorStatistics(long idEvaluator, long idCommittee, IQueryable<Evaluation> query)
        {
            Dictionary<Domain.Evaluation.EvaluationStatus, long> results = new Dictionary<Domain.Evaluation.EvaluationStatus, long>();
            try
            {
                //var p = query.Where(q => q.Committee.Id == idCommittee && q.Evaluator.Id == idEvaluator).GroupBy(q => q.Status).ToList();
                //p.ForEach(t => results.Add(t.Key, t.Count()));

                results[Domain.Evaluation.EvaluationStatus.Evaluated] = query.Where(q => q.Deleted==  BaseStatusDeleted.None && q.Committee.Id == idCommittee && q.Evaluator.Id == idEvaluator && q.Status == Domain.Evaluation.EvaluationStatus.Evaluated).Count();
                results[Domain.Evaluation.EvaluationStatus.Evaluating] = query.Where(q => q.Deleted == BaseStatusDeleted.None && q.Committee.Id == idCommittee && q.Evaluator.Id == idEvaluator && q.Status == Domain.Evaluation.EvaluationStatus.Evaluating).Count();
                results[Domain.Evaluation.EvaluationStatus.None] = query.Where(q => q.Deleted == BaseStatusDeleted.None && q.Committee.Id == idCommittee && q.Evaluator.Id == idEvaluator && q.Status == Domain.Evaluation.EvaluationStatus.None).Count();
                results[Domain.Evaluation.EvaluationStatus.EvaluatorReplacement] = query.Where(q => q.Deleted == BaseStatusDeleted.None && q.Committee.Id == idCommittee && q.Evaluator.Id == idEvaluator && q.Status == Domain.Evaluation.EvaluationStatus.EvaluatorReplacement).Count();
            }
            catch (Exception ex) { 
                    
            }
            return results;
        }
        public CommitteeMember ReplaceEvaluator(long idCall,long idOldMembership, Int32 idNewEvaluator, Boolean assignAllEvaluations, Boolean useDss)
        {
            CommitteeMember result = null;
            CommitteeMember old = GetCommitteeMembership(idOldMembership);
            if (old != null && old.Committee !=null && old.Committee.Call  !=null){
                try
                {
                    Manager.BeginTransaction();
                    litePerson person = Manager.GetLitePerson(UC.CurrentUserID);
                    litePerson newPerson = Manager.GetLitePerson(idNewEvaluator);
                    if (person != null && newPerson != null) {
                        CallEvaluator evaluator = (from e in Manager.GetIQ<CallEvaluator>()
                                                    where e.Call.Id == old.Committee.Call.Id && e.Person.Id == idNewEvaluator
                                                    select e).Skip(0).Take(1).ToList().FirstOrDefault();
                        if (evaluator == null)
                        {
                            evaluator = new CallEvaluator();
                            evaluator.CreateMetaInfo(person, UC.IpAddress, UC.ProxyIpAddress);
                            evaluator.Call = old.Committee.Call;
                            evaluator.Person = newPerson;
                            Manager.SaveOrUpdate(evaluator);
                        }
                        else if (evaluator.Deleted != BaseStatusDeleted.None){
                            evaluator.Deleted= BaseStatusDeleted.None;
                            evaluator.UpdateMetaInfo(person, UC.IpAddress, UC.ProxyIpAddress);
                            Manager.SaveOrUpdate(evaluator);
                        }
                        if (evaluator != null)
                        {
                            DateTime updatedOn = DateTime.Now;
                            result = new CommitteeMember();
                            result.CreateMetaInfo(person, UC.IpAddress, UC.ProxyIpAddress, updatedOn);
                            result.Committee = old.Committee;
                            result.Evaluator = evaluator;
                            result.Status = MembershipStatus.Replacing;
                            result.ReplacingEvaluator = old.Evaluator;
                            result.ReplacingUser = old.Evaluator.Person;
                            Manager.SaveOrUpdate(result);

                            old.Status = MembershipStatus.Replaced;
                            old.ReplacedBy = newPerson;
                            old.ReplacedByEvaluator = evaluator;
                            old.UpdateMetaInfo(person, UC.IpAddress, UC.ProxyIpAddress, updatedOn);
                                   
                            List<Evaluation> evaluations = (from e in Manager.GetIQ<Evaluation>()
                                                            where e.Deleted == BaseStatusDeleted.None && e.Evaluator.Id == old.Evaluator.Id && e.Committee.Id == old.Committee.Id
                                                            && (assignAllEvaluations || (!assignAllEvaluations && e.Status != Domain.Evaluation.EvaluationStatus.Evaluated))
                                                            select e).ToList();
                            foreach(Evaluation evaluation in evaluations){
                                evaluation.Status = Domain.Evaluation.EvaluationStatus.EvaluatorReplacement;
                                Evaluation nEvaluation = new Evaluation();
                                nEvaluation.UseDss = useDss;
                                nEvaluation.CreateMetaInfo(person, UC.IpAddress, UC.ProxyIpAddress, updatedOn);
                                nEvaluation.Committee = result.Committee;
                                nEvaluation.Evaluated = false;
                                nEvaluation.Evaluator = result.Evaluator;
                                nEvaluation.Status = Domain.Evaluation.EvaluationStatus.None;
                                nEvaluation.Submission = evaluation.Submission;
                                nEvaluation.Community = evaluation.Community;
                                nEvaluation.Call = evaluation.Call;
                                nEvaluation.Comment = "";
                                Manager.SaveOrUpdate(nEvaluation);
                                Manager.SaveOrUpdate(evaluation);
                            }

                            if (useDss)
                            {
                                ///remove dss evaluations
                                ///
                                foreach (DssCallEvaluation item in GetQueryDssCallEvaluation(e => e.IdEvaluator == old.Evaluator.Id))
                                {
                                    item.Deleted = BaseStatusDeleted.Manual;
                                    Manager.SaveOrUpdate(item);
                                }
                                DssRatingSetForCall(idCall, out updatedOn);
                            }

                        }
                    }
                    Manager.Commit();
                }
                catch (Exception ex)
                {
                    Manager.RollBack();
                    result = null;
                }
            }
            return result;
        }
        public Boolean RemoveEvaluator(long idCall, long idMembership, Boolean removeAllEvaluations)
        {
            Boolean result = false;
            CommitteeMember old = GetCommitteeMembership(idMembership);
            if (old != null && old.Committee != null)
            {
                try
                {
                    Manager.BeginTransaction();
                    litePerson person = Manager.GetLitePerson(UC.CurrentUserID);
                    if (person != null)
                    {
                        old.Status = MembershipStatus.Removed;
                        old.UpdateMetaInfo(person, UC.IpAddress, UC.ProxyIpAddress);
                        Manager.SaveOrUpdate(old);
                        DateTime updatedOn = old.ModifiedOn.Value;
                        List<Evaluation> evaluations = (from e in Manager.GetIQ<Evaluation>()
                                                        where e.Deleted == BaseStatusDeleted.None && e.Evaluator.Id == old.Evaluator.Id && e.Committee.Id == old.Committee.Id
                                                        && ((removeAllEvaluations && e.Status != Domain.Evaluation.EvaluationStatus.Invalidated) || (!removeAllEvaluations && e.Status != Domain.Evaluation.EvaluationStatus.Evaluated))
                                                        select e).ToList();
                        evaluations.ForEach(e => e.Status = Domain.Evaluation.EvaluationStatus.Invalidated);
                        evaluations.ForEach(e => e.UpdateMetaInfo(person, UC.IpAddress, UC.ProxyIpAddress, updatedOn));

                        Manager.SaveOrUpdateList(evaluations);
                        if (CallUseDss(idCall) && old.Evaluator!=null )
                        {
                            ///remove dss evaluations
                            ///
                            foreach(DssCallEvaluation item in GetQueryDssCallEvaluation(e => e.IdEvaluator == old.Evaluator.Id)){
                                item.Deleted = BaseStatusDeleted.Manual;
                                Manager.SaveOrUpdate(item);
                            }
                            DssRatingSetForCall(idCall,out updatedOn);
                        }
                    }
                    Manager.Commit();
                    result = true;
                }
                catch (Exception ex)
                {
                    Manager.RollBack();
                }
            }
            else
                result = true;
            return result;
        }
    #endregion

            #region "5 - Manage Evaluations"
                public List<ManageEvaluationsAction> GetAvailableActionsForManageEvaluations(long idCall) {
                    List<ManageEvaluationsAction> actions = new List<ManageEvaluationsAction>();
                    var query = (from u in Manager.GetIQ<Evaluation>() where u.Deleted == BaseStatusDeleted.None && u.Call.Id == idCall select u);
                    if (query.Where(e => e.Status == EvaluationStatus.Evaluating || e.Status == EvaluationStatus.None).Any())
                        actions.Add(ManageEvaluationsAction.CloseAll);
                    if (query.Where(e => e.Status == EvaluationStatus.Evaluated).Any())
                        actions.Add(ManageEvaluationsAction.OpenAll);
                    return actions;
                }
                public List<dtoBasicCommitteeItem> GetItemsByEvaluatorsForEvaluationsManagement(long idCall, ManageEvaluationsAction action)
                {
                    List<dtoBasicCommitteeItem> items = new List<dtoBasicCommitteeItem>();
                    try
                    {
                        var query = (from c in Manager.GetIQ<EvaluationCommittee>()
                                 where c.Deleted == BaseStatusDeleted.None && c.Call != null && c.Call.Id == idCall
                                 orderby c.DisplayOrder, c.Name select c);

                        items = query.ToList().Select(c=> new dtoBasicCommitteeItem()
                                 {
                                     Id = c.Id,
                                     Name = c.Name,
                                     DisplayOrder = c.DisplayOrder
                                 }).ToList();

                       foreach (dtoBasicCommitteeItem item in items) {
                            var queryEvaluators = (from e in Manager.GetIQ<Evaluation>()
                                                   where e.Deleted == BaseStatusDeleted.None && e.Call != null && e.Call.Id == idCall && 
                                                   (
                                                   (action== ManageEvaluationsAction.OpenAll && e.Status ==  EvaluationStatus.Evaluated)
                                                   ||
                                                   (action == ManageEvaluationsAction.CloseAll && (e.Status == EvaluationStatus.Evaluating || e.Status == EvaluationStatus.None)
                                                   ))
                                                   && action != ManageEvaluationsAction.None && e.Committee != null && e.Committee.Id == item.Id && e.Evaluator.Deleted == BaseStatusDeleted.None && e.Evaluator.Person != null
                                                   select e);
                            List<CallEvaluator> evaluators = queryEvaluators.Select(e=> e.Evaluator).ToList();

                            item.Evaluators = (from m in evaluators select m).ToList().Distinct().Select(e => new lm.Comol.Modules.CallForPapers.Domain.Evaluation.dtoBasicComitteeEvaluatorItem()
                                               {
                                                    IdCommittee = item.Id,
                                                    IdEvaluator= e.Id,
                                                    Name = e.Person.SurnameAndName,
                                                    Completed = queryEvaluators.Where(ev=>ev.Evaluator.Id== e.Id && ev.Status== EvaluationStatus.Evaluated).Count(),
                                                    Started = queryEvaluators.Where(ev => ev.Evaluator.Id == e.Id && (ev.Status == EvaluationStatus.Evaluating || ev.Status == EvaluationStatus.None)).Count(),
                                               }).OrderBy(i => i.Name).ToList();
                        }
                        items = items.Where(i => i.Evaluators.Any() && i.Evaluators.Count > 0).OrderBy(i => i.DisplayOrder).ThenBy(i => i.Name).ToList();
                    }
                    catch (Exception ex) { 
                    
                    }
                    return items;
                }
                public Boolean SaveEvaluationsEndDate(long idCall, DateTime? endEvaluationOn)
                {
                    Boolean result = false;
                    try
                    {
                        Manager.BeginTransaction();
                        CallForPaper call = Manager.Get<CallForPaper>(idCall);
                        litePerson person = Manager.GetLitePerson(UC.CurrentUserID);
                        if (call != null && call.EndEvaluationOn != endEvaluationOn)
                        {
                            if (person != null && person.TypeID != (Int32)UserTypeStandard.Guest)
                            {
                                call.UpdateMetaInfo(person, UC.IpAddress, UC.ProxyIpAddress);
                                call.EndEvaluationOn = endEvaluationOn;
                                result = true;
                            }
                        }
                        else
                            result = true;
                        Manager.Commit();
                       
                    }
                    catch (Exception ex) {
                        result = false;
                        if (Manager.IsInTransaction())
                            Manager.RollBack();
                    }
                    return result;
                }
                public Boolean BulkEditEvaluationsStatus(long idCall,ManageEvaluationsAction action, Dictionary<long, List<long>> items)
                {
                    Boolean result = false;
                    try
                    {
                        Manager.BeginTransaction();
                        CallForPaper call = Manager.Get<CallForPaper>(idCall);
                        litePerson p = Manager.GetLitePerson(UC.CurrentUserID);
                        if (call != null && p != null && p.TypeID != (Int32)UserTypeStandard.Guest)
                        {
                            DateTime modifyedOn = DateTime.Now;
                            var query = (from e in Manager.GetIQ<Evaluation>() where e.Deleted == BaseStatusDeleted.None && e.Committee !=null select e);
                            foreach (var item in items.Where(i => i.Value.Any() && i.Value.Count > 0))
                            {
                                List<Evaluation> evaluations = query.Where(e => e.Committee.Id == item.Key && e.Evaluator != null && item.Value.Contains(e.Evaluator.Id) &&
                                        (
                                        (action== ManageEvaluationsAction.OpenAll && e.Status ==  EvaluationStatus.Evaluated)
                                        ||
                                        (action == ManageEvaluationsAction.CloseAll && (e.Status == EvaluationStatus.Evaluating || e.Status == EvaluationStatus.None)
                                        ))
                                        && action != ManageEvaluationsAction.None ).ToList();
                                foreach (Evaluation evaluation in evaluations) {
                                    switch(action){
                                        case ManageEvaluationsAction.OpenAll:
                                            evaluation.UpdateMetaInfo(p, UC.IpAddress, UC.ProxyIpAddress, modifyedOn);
                                            evaluation.Status = EvaluationStatus.Evaluating;
                                            evaluation.Evaluated = false;
                                            break;
                                        case ManageEvaluationsAction.CloseAll:
                                            evaluation.UpdateMetaInfo(p, UC.IpAddress, UC.ProxyIpAddress, modifyedOn);
                                            evaluation.Status = EvaluationStatus.Evaluated;
                                            evaluation.Evaluated = true;
                                            evaluation.EvaluatedOn = modifyedOn;
                                            break;
                                    }
                                }
                                if (evaluations.Any())
                                    Manager.SaveOrUpdateList(evaluations);
                            }
                        }
                        Manager.Commit();
                        result = true;
                    }
                    catch (Exception ex)
                    {
                        result = false;
                        if (Manager.IsInTransaction())
                            Manager.RollBack();
                    }
                    return result;
                }

                public Boolean BulkEditEvaluationsStatus(long idCall, ManageEvaluationsAction action, List<long> idEvaluations, Boolean selectAll)
                {
                    Boolean result = false;
                    try
                    {
                        Manager.BeginTransaction();
                        CallForPaper call = Manager.Get<CallForPaper>(idCall);
                        litePerson p = Manager.GetLitePerson(UC.CurrentUserID);
                        if (call != null && p != null && p.TypeID != (Int32)UserTypeStandard.Guest && idEvaluations.Any())
                        {
                            DateTime modifyedOn = DateTime.Now;
                            var query = (from e in Manager.GetIQ<Evaluation>() where e.Deleted == BaseStatusDeleted.None && e.Committee !=null &&  (
                                        (action== ManageEvaluationsAction.OpenAll && e.Status ==  EvaluationStatus.Evaluated)
                                        ||
                                        (action == ManageEvaluationsAction.CloseAll && (e.Status == EvaluationStatus.Evaluating || e.Status == EvaluationStatus.None)
                                        ))
                                        && action != ManageEvaluationsAction.None select e);


                            Int32 pageIndex = 0;
                            List<Evaluation> evaluations = null;
                            if (selectAll)
                                evaluations = query.ToList();
                            else if (idEvaluations.Count <= maxItemsForQuery)
                                evaluations = query.Where(e => idEvaluations.Contains(e.Id)).ToList();
                            else
                            {
                                evaluations = new List<Evaluation>();

                                var idQuery = idEvaluations.Skip(pageIndex * maxItemsForQuery).Take(maxItemsForQuery).ToList();
                                while (idQuery.Any())
                                {
                                    evaluations.AddRange(query.Where(e => idQuery.Contains(e.Id)).ToList());
                                    pageIndex++;
                                    idQuery = idEvaluations.Skip(pageIndex * maxItemsForQuery).Take(maxItemsForQuery).ToList();
                                }
                            }
                        
                            foreach (Evaluation evaluation in evaluations) {
                                switch(action){
                                    case ManageEvaluationsAction.OpenAll:
                                        evaluation.UpdateMetaInfo(p, UC.IpAddress, UC.ProxyIpAddress, modifyedOn);
                                        evaluation.Status = EvaluationStatus.Evaluating;
                                        evaluation.Evaluated = false;
                                        break;
                                    case ManageEvaluationsAction.CloseAll:
                                        evaluation.UpdateMetaInfo(p, UC.IpAddress, UC.ProxyIpAddress, modifyedOn);
                                        evaluation.Status = EvaluationStatus.Evaluated;
                                        evaluation.Evaluated = true;
                                        evaluation.EvaluatedOn = modifyedOn;
                                        break;
                                }
                            }
                            if (evaluations.Any())
                                Manager.SaveOrUpdateList(evaluations);
                        }
                        Manager.Commit();
                        result = true;
                    }
                    catch (Exception ex)
                    {
                        result = false;
                        if (Manager.IsInTransaction())
                            Manager.RollBack();
                    }
                    return result;
                }
                public List<dtoSubmitterType> GetSubmitterTypes(long idCall, ManageEvaluationsAction action, long idCommittee=0) { 
                    List<dtoSubmitterType> items = new List<dtoSubmitterType>();
                    try
                    {
                        var query = (from u in Manager.GetIQ<expEvaluation>() where u.Deleted == BaseStatusDeleted.None && (idCommittee==0 || (idCommittee>0 && u.IdCommittee ==idCommittee)) && u.IdCall == idCall select u);
                        switch (action) { 
                            case ManageEvaluationsAction.CloseAll:
                                query = query.Where(e => e.Status == EvaluationStatus.Evaluating || e.Status == EvaluationStatus.None);
                                break;
                            case ManageEvaluationsAction.OpenAll:
                                query=  query.Where(e => e.Status == EvaluationStatus.Evaluated);
                                break;
                        }
                        List<long> idSubmitters = (from e in query select e).ToList().Where(e=> e.Submission !=null).Select(e=> e.Submission.Type.Id).Distinct().ToList();
                        items = (from st in Manager.GetIQ<expSubmitterType>() where idSubmitters.Contains(st.Id) orderby st.DisplayOrder, st.Name  select new dtoSubmitterType() 
                                    {
                                     Id= st.Id,
                                     Name= st.Name,
                                     DisplayOrder = st.DisplayOrder}).ToList();
                    }
                    catch (Exception ex) {
                        items = new List<dtoSubmitterType>();
                    }
                    return items;
                }
                //public Int32 GetSubmissionsCount(long idCall, ManageEvaluationsAction action)
                //{
                //    Int32 count = 0;
                //    try
                //    {
                //        Manager.BeginTransaction();
                //        var query = (from u in Manager.GetIQ<Evaluation>() where u.Deleted == BaseStatusDeleted.None && u.Call.Id == idCall select u);
                //        switch (action)
                //        {
                //            case ManageEvaluationsAction.CloseAll:
                //                query = query.Where(e => e.Status == EvaluationStatus.Evaluating || e.Status == EvaluationStatus.None);
                //                break;
                //            case ManageEvaluationsAction.OpenAll:
                //                query = query.Where(e => e.Status == EvaluationStatus.Evaluated);
                //                break;
                //        }
                //        count = (from e in query select e.Submission.Id).Distinct().ToList().Count();
                //        Manager.Commit();
                //    }
                //    catch (Exception ex)
                //    {
                //        if (Manager.IsInTransaction())
                //            Manager.RollBack();
                //    }
                //    return count;
                //}
                public List<dtoBasicSubmissionItem> GetItemsBySubmissionsForEvaluationsManagement(long idCall,String name, long idSubmitterType,  ManageEvaluationsAction action,String anonymousTranslation)
                {
                    List<dtoBasicSubmissionItem> items = new List<dtoBasicSubmissionItem>();
                    try
                    {
                        Manager.BeginTransaction();
                        var query = (from u in Manager.GetIQ<Evaluation>() where u.Deleted == BaseStatusDeleted.None && u.Call!=null && u.Call.Id == idCall && u.Submission != null select u);
                        switch (action)
                        {
                            case ManageEvaluationsAction.CloseAll:
                                query = query.Where(e => e.Status == EvaluationStatus.Evaluating || e.Status == EvaluationStatus.None);
                                break;
                            case ManageEvaluationsAction.OpenAll:
                                query = query.Where(e => e.Status == EvaluationStatus.Evaluated);
                                break;
                        }
                        if (idSubmitterType >0)
                            query = query.Where(e => e.Submission.Type != null && e.Submission.Type.Id == idSubmitterType);

                        List<Evaluation> evaulations = query.ToList();
                        List<UserSubmission> submissions = evaulations.Select(e => e.Submission).Distinct().ToList();
                       
                        items = submissions.Where(s => s.Deleted == BaseStatusDeleted.None).ToList().Select(s => new dtoBasicSubmissionItem()
                            {
                                Id = s.Id,
                                IdOwner = (s.isAnonymous || s.Owner == null) ? 0 : s.Owner.Id,
                                DisplayName = (s.isAnonymous || s.Owner == null) ? anonymousTranslation : s.Owner.SurnameAndName,
                                isAnonymous = s.isAnonymous
                            }).ToList();

                        if (!String.IsNullOrEmpty(name))
                            items = items.Where(s => s.DisplayName.ToLower().Contains(name.ToLower())).ToList();


                        foreach (dtoBasicSubmissionItem item in items) {
                            item.Committees = (from e in evaulations where e.Submission.Id == item.Id select e.Committee).Distinct().ToList().OrderBy(c => c.DisplayOrder).Select(c => new dtoBasicSubmissionCommitteeItem()
                                                    {
                                                        DisplayOrder = c.DisplayOrder,
                                                        Id = c.Id,
                                                        IdSubmission = item.Id,
                                                        Name = c.Name
                                                    }).ToList().OrderBy(c=>c.DisplayOrder).ThenBy(c=>c.Name).ToList();
                            foreach (dtoBasicSubmissionCommitteeItem committee in item.Committees)
                            {
                                committee.Evaluators = (from e in query where e.Submission.Id == item.Id && e.Committee.Id == committee.Id && e.Evaluator != null && e.Evaluator.Person != null select e).ToList().Select(e => new dtoComitteeEvaluatorItem()
                                                {
                                                    IdEvaluation= e.Id,
                                                    IdCommittee = committee.Id,
                                                    IdEvaluator= e.Evaluator.Id ,
                                                    Name = e.Evaluator.Person.SurnameAndName,
                                                    IdSubmission = item.Id,
                                                    Completed =  (e.Status== EvaluationStatus.Evaluated) ? 1 : 0,
                                                    Started = (e.Status == EvaluationStatus.None || e.Status == EvaluationStatus.Evaluating) ? 1 : 0
                                               }).ToList().OrderBy(i => i.Name).ToList();
                            }
                        }
                        Manager.Commit();
                    }
                    catch (Exception ex) {
                        if (Manager.IsInTransaction())
                            Manager.RollBack();
                    }
                    return items.OrderBy(s => s.DisplayName).ToList();
                }

                public Int32 CalculateBySubmissionsPageSize(long idCall, String name, long idSubmitterType, ManageEvaluationsAction action, String anonymousTranslation)
                {
                    Int32 pageSize = 25;
                    List<dtoBasicSubmissionItem> items = new List<dtoBasicSubmissionItem>();
                    try
                    {
                        Manager.BeginTransaction();
                        var query = (from u in Manager.GetIQ<Evaluation>() where u.Deleted == BaseStatusDeleted.None && u.Call != null && u.Call.Id == idCall && u.Submission != null select u);
                        switch (action)
                        {
                            case ManageEvaluationsAction.CloseAll:
                                query = query.Where(e => e.Status == EvaluationStatus.Evaluating || e.Status == EvaluationStatus.None);
                                break;
                            case ManageEvaluationsAction.OpenAll:
                                query = query.Where(e => e.Status == EvaluationStatus.Evaluated);
                                break;
                        }
                        if (idSubmitterType > 0)
                            query = query.Where(e => e.Submission.Type != null && e.Submission.Type.Id == idSubmitterType);

                        Int32 evaluationsCount = query.Count();
                        Int32 submissionsCount = query.Select(e => e.Submission.Id).Distinct().Count();
                        Int32 comitteesCount = query.Select(e => e.Committee.Id ).Distinct().Count();
                        Int32 displayItems = evaluationsCount + comitteesCount;
                        if (displayItems + submissionsCount <= 80)
                            pageSize = submissionsCount;
                        else
                            pageSize = 25;
                        Manager.Commit();
                    }
                    catch (Exception ex)
                    {
                        if (Manager.IsInTransaction())
                            Manager.RollBack();
                        pageSize = 25;
                    }
                    return pageSize;
                }
            #endregion

        #endregion

        #region "Evaluations"
            public List<dtoBaseEvaluation> GetMemberEvaluations(CommitteeMember membership,String anonymousUser, String unknonwUser)
            {
                List<dtoBaseEvaluation> items = new List<dtoBaseEvaluation>();
                try
                {
                    if (membership!=null && membership.Committee !=null && membership.Evaluator !=null)
                        items = (from s in Manager.GetIQ<expEvaluation>()
                                where s.Deleted == BaseStatusDeleted.None && s.IdCommittee == membership.Committee.Id && s.IdEvaluator == membership.Evaluator.Id
                                 select s).ToList().Select(s => new dtoBaseEvaluation(s, anonymousUser, unknonwUser)).ToList();
                }
                catch (Exception ex)
                {

                }
                return items.OrderBy(s => s.DisplayName).ToList();
            }
            public Boolean isEvaluationOwner(long idEvaluator, long idEvaluation, long idSubmission,DisplayEvaluations display, Int32 idPerson)
            {
                Boolean result = false;
                try
                {
                    switch (display) { 
                        case DisplayEvaluations.Single:
                            long id = (from s in Manager.GetIQ<Evaluation>()
                                        where s.Deleted == BaseStatusDeleted.None && s.Id == idEvaluation
                                        select s.Evaluator.Id).Skip(0).Take(1).ToList().FirstOrDefault();

                            result = (from m in Manager.GetIQ<CallEvaluator>() where m.Id == idEvaluator && m.Deleted == BaseStatusDeleted.None && m.Person.Id == idPerson select m.Id).Any();
                            break;
                        case DisplayEvaluations.ForSubmission:
                            result = (from s in Manager.GetIQ<Evaluation>()
                                      where s.Deleted == BaseStatusDeleted.None && s.Submission.Id == idSubmission && s.Evaluator.Deleted == BaseStatusDeleted.None && s.Evaluator.Person.Id == idPerson
                                      select s.Id).Any();
                            break;
                        case DisplayEvaluations.ForUser:
                            result = (from s in Manager.GetIQ<Evaluation>()
                                      where s.Deleted == BaseStatusDeleted.None && s.Submission.Id == idSubmission && s.Evaluator.Deleted == BaseStatusDeleted.None && s.Evaluator.Person.Id == idPerson
                                      select s.Id).Any();
                            break;
                    }
                   
                }
                catch (Exception ex)
                {

                }
                return result;
            }
            public Boolean isEvaluationOwner(long idEvaluation, Int32 idPerson)
            {
                Boolean result = false;
                try
                {
                    long idEvaluator = (from s in Manager.GetIQ<Evaluation>()
                              where s.Deleted == BaseStatusDeleted.None && s.Id == idEvaluation
                              select s.Evaluator.Id).Skip(0).Take(1).ToList().FirstOrDefault();

                    result = (from m in Manager.GetIQ<CallEvaluator>() where m.Id==idEvaluator && m.Deleted== BaseStatusDeleted.None && m.Person.Id == idPerson select m.Id).Any();
                }
                catch (Exception ex)
                {

                }
                return result;
            }
            public Boolean isEvaluator(long idCall, Int32 idPerson)
            {
                Boolean result = false;
                try
                {
                    long idEvaluator = (from e in Manager.GetIQ<CallEvaluator>()
                                        where e.Deleted == BaseStatusDeleted.None && e.Person.Id == idPerson && e.Call.Id == idCall
                                        select e.Id).Skip(0).Take(1).ToList().FirstOrDefault();
                    result = (from e in Manager.GetIQ<CommitteeMember>()
                              where e.Deleted == BaseStatusDeleted.None && e.Evaluator.Id == idEvaluator && e.Status != MembershipStatus.Removed && e.Status != MembershipStatus.Replaced
                              select e.Id).Any();
                }
                catch (Exception ex)
                {

                }
                return result;
            }
            public long GetIdEvaluator(long idCall, Int32 idPerson)
            {
                long idEvaluator = 0;
                try
                {
                    idEvaluator = (from e in Manager.GetIQ<CallEvaluator>()
                                        where e.Deleted == BaseStatusDeleted.None && e.Person.Id == idPerson && e.Call.Id == idCall
                                        select e.Id).Skip(0).Take(1).ToList().FirstOrDefault();
                }
                catch (Exception ex)
                {

                }
                return idEvaluator;
            }
            public lm.Comol.Modules.CallForPapers.Domain.Evaluation.dtoEvaluation GetEvaluation(long idEvaluation, String anonymousUser, String unknonwUser)
            {
                return GetEvaluation(idEvaluation, false, anonymousUser, unknonwUser);
            }
            public lm.Comol.Modules.CallForPapers.Domain.Evaluation.dtoEvaluation GetFullEvaluation(long idEvaluation, String anonymousUser, String unknonwUser)
            {
                return GetEvaluation(idEvaluation, true, anonymousUser, unknonwUser);
            }
            private lm.Comol.Modules.CallForPapers.Domain.Evaluation.dtoEvaluation GetEvaluation(long idEvaluation, Boolean loadCriteria, String anonymousUser, String unknonwUser)
            {
                lm.Comol.Modules.CallForPapers.Domain.Evaluation.dtoEvaluation evaluation = null;
                try
                {
                    expEvaluation ev = Manager.Get<expEvaluation>(idEvaluation);
                    if (ev != null && ev.Id > 0)
                        evaluation = new lm.Comol.Modules.CallForPapers.Domain.Evaluation.dtoEvaluation(ev, DssRatingGetByEvaluation(ev.Id), anonymousUser, unknonwUser);

                    if (evaluation!= null && loadCriteria) { 
                        List<lm.Comol.Modules.CallForPapers.Domain.Evaluation.dtoCriterion> criteria = (from bc in Manager.GetIQ<BaseCriterion>()
                                            where bc.Deleted== BaseStatusDeleted.None && bc.Committee !=null && bc.Committee.Id  == evaluation.IdCommittee  
                                            select bc).ToList().Select(bc => new lm.Comol.Modules.CallForPapers.Domain.Evaluation.dtoCriterion(bc)).ToList();

                        criteria.OrderBy(c => c.DisplayOrder).ThenBy(c => c.Name).ToList().ForEach(c => evaluation.Criteria.Add(new dtoCriterionEvaluated(c, (ev.EvaluatedCriteria==null? null :  ev.EvaluatedCriteria.Where(v=> v.Criterion.Id== c.Id).FirstOrDefault()))));
                    }
                }
                catch (Exception ex)
                {

                }
                return evaluation;
            }
            private List<dtoCriterionEvaluated> GetEvaluationCriteria(Evaluation evaluation, List<CriterionEvaluated> removeValues)
            {
                List<dtoCriterionEvaluated> cValues = new List<dtoCriterionEvaluated>();
                try
                {
                    if (evaluation != null)
                    {
                        List<lm.Comol.Modules.CallForPapers.Domain.Evaluation.dtoCriterion> criteria = (from bc in Manager.GetIQ<BaseCriterion>()
                                                                                                        where bc.Committee != null && bc.Committee.Id == evaluation.Committee.Id && bc.Deleted == BaseStatusDeleted.None
                                                                                                        select bc).ToList().Select(bc => new lm.Comol.Modules.CallForPapers.Domain.Evaluation.dtoCriterion(bc)).ToList();
                        List<long> idToRemove = removeValues.Select(i=> i.Id).ToList();
                        List<CriterionEvaluated> values = (from cv in Manager.GetIQ<CriterionEvaluated>() 
                                                           where cv.Evaluation == evaluation && cv.Deleted == BaseStatusDeleted.None && !idToRemove.Contains(cv.Id) select cv).ToList();

                        foreach (dtoCriterion criterion in criteria) {
                            cValues.Add(new dtoCriterionEvaluated(criterion, values.Where(v => v.Criterion.Id == criterion.Id).FirstOrDefault()));
                        }
                    }
                }
                catch (Exception ex)
                {
                    cValues = new List<dtoCriterionEvaluated>();
                }
                return cValues;
            }

            public Evaluation SaveEvaluation(long idEvaluation,long idEvaluator,List<dtoCriterionEvaluated> criteria, String comment, Boolean completed)
            {
                Evaluation evaluation = null;
                try
                {
                    Manager.BeginTransaction();
                    litePerson person = Manager.GetLitePerson(UC.CurrentUserID);
                    evaluation = Manager.Get<Evaluation>(idEvaluation);

                    EvaluationError exception = new EvaluationError();

                    if (evaluation!=null && person!= null && (person.TypeID!= (int)(UserTypeStandard.PublicUser) && person.TypeID!= (int)(UserTypeStandard.Guest))){
                        DateTime saveTime = DateTime.Now;    
                        evaluation.UpdateMetaInfo(person, UC.IpAddress, UC.ProxyIpAddress,saveTime);
                        if (!evaluation.EvaluationStartedOn.HasValue)
                            evaluation.EvaluationStartedOn = saveTime;

                        if (completed && !criteria.Where(c => !c.IsValidForEvaluation).Any())
                        {
                            evaluation.EvaluatedOn = saveTime;
                            evaluation.Evaluated = true;
                            evaluation.Status = Domain.Evaluation.EvaluationStatus.Evaluated;
                        }
                        else
                        {
                            evaluation.Evaluated = false;
                            evaluation.Status = Domain.Evaluation.EvaluationStatus.Evaluating;
                        }
                        evaluation.Comment = comment;
                        List<CriterionEvaluated> savedValues = new List<CriterionEvaluated>();
                        foreach (dtoCriterionEvaluated dto in criteria)
                        {
                            CriterionEvaluated criterion = Manager.Get<CriterionEvaluated>(dto.IdValueCriterion);
                            if (criterion==null)
                                criterion = (from e in Manager.GetIQ<CriterionEvaluated>() where e.Evaluation.Id== idEvaluation && e.Deleted==  BaseStatusDeleted.None 
                                             && e.Criterion != null && e.Criterion.Id == dto.IdCriterion select e).Skip(0).Take(1).ToList().FirstOrDefault();
                            
                            if (criterion==null){
                                criterion = new CriterionEvaluated();
                                criterion.CreateMetaInfo(person, UC.IpAddress, UC.ProxyIpAddress,saveTime);
                                criterion.Call = evaluation.Call;
                                criterion.Criterion = Manager.Get<BaseCriterion>(dto.IdCriterion);
                                criterion.Evaluation = evaluation;
                                criterion.Submission = evaluation.Submission;
                            }
                            else
                                criterion.UpdateMetaInfo(person, UC.IpAddress, UC.ProxyIpAddress,saveTime);
                            criterion.Comment= dto.Comment;
                            criterion.DecimalValue= dto.DecimalValue;
                            criterion.StringValue = dto.StringValue;
                            criterion.IsValueEmpty = dto.IsValueEmpty;
                            switch (criterion.Criterion.Type)
                            {
                                case CriterionType.RatingScale:
                                case CriterionType.RatingScaleFuzzy:
                                    if (dto.DssValue.Error == Core.Dss.Domain.DssError.None){
                                        criterion.DssValue = dto.DssValue;
                                        criterion.DssValue.IsFuzzy = (criterion.Criterion.Type == CriterionType.RatingScaleFuzzy);
                                    }
                                    else if( dto.DssValue.IdRatingValue> 0 || dto.DssValue.IdRatingValueEnd>0){
                                        criterion.DssValue = dto.DssValue;
                                        criterion.DssValue.IsFuzzy = (criterion.Criterion.Type == CriterionType.RatingScaleFuzzy);
                                    }
                                    break;
                                default:
                                    break;
                            }
                            if (dto.IdOption>0)
                                criterion.Option = Manager.Get<CriterionOption>(dto.IdOption);
                            else
                                criterion.Option= null;
                            if (criterion.Option != null) {
                                criterion.DecimalValue = criterion.Option.Value;
                                dto.DecimalValue = criterion.Option.Value;
                            }
                            try
                            {
                                if (criterion.Criterion != null)
                                {
                                    Manager.SaveOrUpdate(criterion);
                                    if (!dto.IsValidForEvaluation && completed)
                                        exception.Criteria.Add(dto);
                                    savedValues.Add(criterion);
                                }
                                else
                                    exception.Criteria.Add(dto);
                            }
                            catch (Exception ex)
                            {
                                exception.Criteria.Add(dto);
                            }
                        }
                        if (criteria.Where(c => c.IsValidForCriterionSaving).Any())
                        {
                            List<double> standardValues = (from c in criteria
                                                           where c.IsValidForCriterionSaving
                                                           && c.Criterion.Type != CriterionType.Boolean
                                                           select (double)c.DecimalValue).ToList();

                            if (evaluation.Committee != null && evaluation.Committee.UseDss)
                            {
                                standardValues.AddRange((from c in criteria
                                                         where c.IsValidForCriterionSaving
                                                         where c.Criterion.Type == CriterionType.RatingScale || c.Criterion.Type == CriterionType.RatingScaleFuzzy
                                                         select c.DssValue.Value).ToList());
                            }
                            evaluation.AverageRating = (standardValues.Any() ? (double)standardValues.Average() : 0);
                            evaluation.SumRating = (standardValues.Any() ? (double)standardValues.Sum() : 0);
                            if (evaluation.Call != null && evaluation.Call.IdDssMethod > 0)
                            {
                                EvaluatorSetDssRating(idEvaluator, evaluation.Committee, idEvaluation, saveTime);
                            }
                        }
                        else {
                        }
                    }
                    Manager.Commit();
                    if (exception.Criteria.Count > 0)
                        throw exception;   
                }
                catch (EvaluationError ex)
                {
                    Manager.RollBack();
                    throw ex;
                }
                catch (Exception ex)
                {
                    Manager.RollBack();
                    evaluation = null;
                }
                return evaluation;
            }

            #region "Export"
                public dtoEvaluatorCommitteeStatistic GetEvaluatorCommitteeStatistics(
                    EvaluationType type, 
                    dtoEvaluationsFilters filters, 
                    String anonymousDisplayName, 
                    String unknownUserName, 
                    long idCommittee, 
                    long idEvaluator, 
                    Boolean applyFilters = true)
                {
                    return GetEvaluatorStatistics(type,filters, anonymousDisplayName, unknownUserName, Manager.Get<expCommittee>(idCommittee), idEvaluator, applyFilters);
                }
                public List<dtoEvaluatorCommitteeStatistic> GetEvaluatorCommitteesStatistics(
                    EvaluationType type, 
                    dtoEvaluationsFilters filters, 
                    String anonymousDisplayName, 
                    String unknownUserName, 
                    List<long> idCommittees, 
                    long idEvaluator, 
                    Boolean applyFilters = true)
                {
                    List <dtoEvaluatorCommitteeStatistic> statistics = new List<dtoEvaluatorCommitteeStatistic>();
                    if (idCommittees == null && !idCommittees.Any())
                        return new List<dtoEvaluatorCommitteeStatistic>();
                    else {
                        foreach (long idCommittee in idCommittees) {
                            dtoEvaluatorCommitteeStatistic statistic = 
                                GetEvaluatorCommitteeStatistics(type, filters, anonymousDisplayName, unknownUserName, idCommittee, idEvaluator, applyFilters);
                            if (statistic != null)
                                statistics.Add(statistic);
                        }
                    }
                    return statistics;
                }
                private dtoEvaluatorCommitteeStatistic GetEvaluatorStatistics(EvaluationType type, dtoEvaluationsFilters filters, String anonymousDisplayName, String unknownUserName, expCommittee committee, long idEvaluator, Boolean applyFilters = true)
                {
                    dtoEvaluatorCommitteeStatistic statistic = null;
                    try
                    { 
                        long index = 1;
                        if (committee != null)
                        {
                            List<DssCallEvaluation> dssEvaluations = DssRatingGetValues(committee.IdCall, committee.Id, idEvaluator);
                            statistic = new dtoEvaluatorCommitteeStatistic();
                            statistic.IdCommittee = committee.Id;
                            statistic.Name = committee.Name;
                            statistic.IsFuzzy = committee.UseDss && committee.MethodSettings.IsFuzzyMethod;
                            statistic.Description = committee.Description;
                            statistic.Criteria = (committee.Criteria != null) ? committee.Criteria.OrderBy(cr => cr.DisplayOrder).ThenBy(cr => cr.Name).Select(cr => new dtoCriterion(cr)).ToList() : new List<dtoCriterion>();
                            statistic.IdEvaluator = idEvaluator;
                            statistic.Evaluations = (committee.Evaluations != null) ? committee.Evaluations.Where(e => e.IdEvaluator == idEvaluator).Select(e => new lm.Comol.Modules.CallForPapers.Domain.Evaluation.dtoEvaluation(e,dssEvaluations.Where(d=> d.IdSubmission== e.IdSubmission).FirstOrDefault(), anonymousDisplayName, unknownUserName, statistic.Criteria)).OrderByDescending(e => e.SumRating).ThenBy(e => e.DisplayName).ToList() : new List<lm.Comol.Modules.CallForPapers.Domain.Evaluation.dtoEvaluation>();

                            if (statistic.Evaluations != null)
                            {
                                statistic.Evaluations = FilterAndReorderEvaluations(type,filters, statistic.Evaluations, applyFilters);
                                List<long> idEvaluations = statistic.Evaluations.Select(e => e.Id).ToList();
                                List<expEvaluation> evaulations = committee.Evaluations.Where(e => idEvaluations.Contains(e.Id)).ToList();
                                if (committee.Criteria != null)
                                {
                                    foreach (Domain.Evaluation.dtoEvaluation e in statistic.Evaluations)
                                    {
                                        e.UpdateValues(evaulations.Where(ev => ev.Id == e.Id).FirstOrDefault());
                                    }
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        statistic = null;
                    }
                    return statistic;
                }

                private List<Domain.Evaluation.dtoEvaluation> FilterAndReorderEvaluations(EvaluationType type, dtoEvaluationsFilters filters, List<Domain.Evaluation.dtoEvaluation> evaluations, Boolean applyFilters = true )
                {
                    long index = 1;
                    switch (type)
                    {
                        case EvaluationType.Average:
                            evaluations.OrderByDescending(e => e.AverageRating).ThenBy(e => e.DisplayName).ToList().ForEach(e => e.Position = index++);
                            break;
                        case EvaluationType.Dss:
                            evaluations.OrderByDescending(e => e.DssRating.Ranking).ThenBy(e => e.DisplayName).ToList().ForEach(e => e.Position = index++);
                            break;
                        default:
                            evaluations.OrderByDescending(e => e.SumRating).ThenBy(e => e.DisplayName).ToList().ForEach(e => e.Position = index++);
                            break;
                    }
                    

                    var query = (from e in evaluations select e);
                    if (applyFilters) {
                        query = query.Where(e => (filters.IdSubmitterType == -1 || filters.IdSubmitterType == e.IdSubmitterType));

                        if (!string.IsNullOrEmpty(filters.SearchForName) && !String.IsNullOrEmpty(filters.SearchForName.Trim()))
                        {
                            List<long> idEvaluations = new List<long>();
                            List<String> searchName = filters.SearchForName.ToLower().Split(' ').ToList();
                            searchName.ForEach(s => query.Where(i => i.DisplayName.ToLower().Contains(s)).ToList().ForEach(t => idEvaluations.Add(t.Id)));
                            query = query.Where(e => idEvaluations.Contains(e.Id)).ToList();
                        }
                        switch (filters.Status) { 
                            case EvaluationFilterStatus.AllValid:
                                query = query.Where(e => e.Status != EvaluationStatus.Invalidated && e.Status != EvaluationStatus.EvaluatorReplacement).ToList();
                                break;
                            case EvaluationFilterStatus.Evaluated:
                                query = query.Where(e => e.Status == EvaluationStatus.Evaluated).ToList();
                                break;
                            case EvaluationFilterStatus.Evaluating:
                                query = query.Where(e => e.Status == EvaluationStatus.Evaluating).ToList();
                                break;
                            case EvaluationFilterStatus.EvaluatorReplacement:
                                query = query.Where(e => e.Status == EvaluationStatus.EvaluatorReplacement).ToList();
                                break;
                            case EvaluationFilterStatus.Invalidated:
                                query = query.Where(e => e.Status == EvaluationStatus.Invalidated).ToList();
                                break;
                            case EvaluationFilterStatus.None:
                                query = query.Where(e => e.Status== EvaluationStatus.None).ToList();
                                break;
                        }
                    }
                    SubmissionsOrder orderBy = filters.OrderBy;
                    Boolean ascending = filters.Ascending;
                    if (orderBy == SubmissionsOrder.None)
                    {
                        ascending = true;
                        //switch (filters.Status)
                        //{
                        //    case SubmissionFilterStatus.OnlySubmitted:
                        //        orderBy = SubmissionsOrder.BySubmittedOn;
                        //        ascending = true;
                        //        break;
                        //    case SubmissionFilterStatus.VirtualDeletedSubmission:
                        //        orderBy = SubmissionsOrder.ByDate;
                        //        break;
                        //    case SubmissionFilterStatus.WaitingSubmission:
                        //        orderBy = SubmissionsOrder.ByDate;
                        //        break;
                        //    default:
                        //        orderBy = SubmissionsOrder.ByDate;
                        //        break;
                        //}
                    }

                    #region "order"
                    switch (orderBy)
                    {
                        case SubmissionsOrder.BySubmittedOn:
                            if (ascending)
                                query = query.OrderBy(s => s.SubmittedOn);
                            else
                                query = query.OrderByDescending(s => s.SubmittedOn);
                            break;
                        case SubmissionsOrder.ByDate:
                            if (ascending)
                                query = query.OrderBy(r => r.ModifiedOn);
                            else
                                query = query.OrderByDescending(r => r.ModifiedOn);
                            break;
                        case SubmissionsOrder.ByEvaluationStatus:
                            if (ascending)
                                query = query.OrderBy(r => filters.TranslationsEvaluationStatus[r.Status]);
                            else
                                query = query.OrderByDescending(r => filters.TranslationsEvaluationStatus[r.Status]);
                            break;
                        case SubmissionsOrder.ByType:
                            if (ascending)
                                query = query.OrderBy(s => s.SubmitterType);
                            else
                                query = query.OrderByDescending(s => s.SubmitterType);
                            break;
                        case SubmissionsOrder.ByUser:
                            if (ascending)
                                query = query.OrderBy(s => s.DisplayName);
                            else
                                query = query.OrderByDescending(s => s.DisplayName);
                            break;
                        case SubmissionsOrder.ByEvaluationPoints:
                            if (ascending)
                            {
                                switch (type)
                                {
                                    case EvaluationType.Average:
                                        query = query.OrderBy(s => s.AverageRating).ThenBy(e => e.DisplayName);
                                        break;
                                    case EvaluationType.Dss:
                                        query = query.OrderBy(s => s.DssRating.Ranking).ThenBy(e => e.DisplayName);
                                        break;
                                    default:
                                        query = query.OrderBy(s => s.SumRating).ThenBy(e => e.DisplayName);
                                        break;
                                }
                            }
                            else
                            {
                                switch (type)
                                {
                                    case EvaluationType.Average:
                                        query = query.OrderByDescending(s => s.AverageRating).ThenBy(e => e.DisplayName);
                                        break;
                                    case EvaluationType.Dss:
                                        query = query.OrderByDescending(s => s.DssRating.Value).ThenBy(e => e.DisplayName);
                                        break;
                                    default:
                                        query = query.OrderByDescending(s => s.SumRating).ThenBy(e => e.DisplayName);
                                        break;
                                }
                            }
                            break;
                        case SubmissionsOrder.ByEvaluationIndex:
                            if (ascending)
                                query = query.OrderBy(s => s.Position).ThenBy(e => e.DisplayName);
                            else
                                query = query.OrderByDescending(s => s.Position).ThenBy(e => e.DisplayName);
                            break;
                        default:
                             if (ascending)
                                query = query.OrderBy(s => s.DisplayName).ThenBy(s=> s.SubmitterType);
                            else
                                 query = query.OrderByDescending(s => s.DisplayName).ThenBy(s => s.SubmitterType);
                            break;
                    }
                    #endregion 

                    //if (pageSize > 0)
                    //    query = query.Skip(pageIndex * pageSize).Take(pageSize).ToList();
                    //else
                    //    query = query.ToList();
                    //items = (from r in query
                    //         select new dtoBaseSubmission()
                    //         {
                    //             Type = new dtoSubmitterType() { Name = r.Type.Name },
                    //             Status = r.Status,
                    //             Deleted = r.Deleted,
                    //             Id = r.Id,
                    //             ExtensionDate = r.ExtensionDate,
                    //             SubmittedOn = r.SubmittedOn,
                    //             ModifiedOn = r.ModifiedOn,
                    //             Owner = r.Owner,
                    //             IsAnonymous = r.isAnonymous
                    //         }).ToList();

                    return query.ToList();
                }

                public String ExportEvaluatorStatistics(dtoCall call, dtoEvaluationsFilters filters, String anonymousDisplayName, String unknownUserName, long idCommittee, long idEvaluator,Boolean applyFilters, lm.Comol.Core.DomainModel.Helpers.Export.ExportFileType fileType, Dictionary<lm.Comol.Modules.CallForPapers.Domain.Evaluation.EvaluationTranslations, String> translations, Dictionary<lm.Comol.Modules.CallForPapers.Domain.Evaluation.EvaluationStatus, String> status)
                {
                    List<dtoEvaluatorCommitteeStatistic> statistics = null;
                    litePerson person = null;
                    try
                    {
                        person = Manager.GetLitePerson(UC.CurrentUserID);

                        if (idCommittee > 0){
                            dtoEvaluatorCommitteeStatistic statistic = GetEvaluatorCommitteeStatistics(call.EvaluationType,filters, anonymousDisplayName, unknownUserName, idCommittee, idEvaluator, applyFilters);
                            if (statistic != null)
                            {
                                statistics = new List<dtoEvaluatorCommitteeStatistic>();
                                statistics.Add(statistic);
                            }
                        }
                        else
                        {
                            List<long> idCommittees = new List<long>();
                            idCommittees.AddRange((from m in Manager.GetIQ<expCommitteeMember>()
                                                   where m.Deleted == BaseStatusDeleted.None && m.Evaluator.Id == idEvaluator && m.Status != MembershipStatus.Removed
                                                   select m.IdCommittee).ToList());
                            statistics = GetEvaluatorCommitteesStatistics(call.EvaluationType, filters, anonymousDisplayName, unknownUserName, idCommittees, idEvaluator, applyFilters);
                        }
                    }
                    catch (Exception ex)
                    {
                        return GetErrorDocument(call, person, fileType, translations, status);
                    }

                    try
                    {
                        switch (fileType)
                        {
                            case Core.DomainModel.Helpers.Export.ExportFileType.xml:
                            case Core.DomainModel.Helpers.Export.ExportFileType.xls:
                                HelperExportToXml helperXml = new HelperExportToXml(translations, status);
                                return helperXml.ExportEvaluatorStatistics(call, statistics, person);
                            case Core.DomainModel.Helpers.Export.ExportFileType.csv:
                                HelperExportToCsv helperCsv = new HelperExportToCsv(translations, status);
                                return helperCsv.ExportEvaluatorStatistics(call, statistics, person);;
                        }
                    }
                    catch (Exception ex)
                    {
                        return GetErrorDocument(call, person, fileType, translations, status);
                    }
                    return GetErrorDocument(call, person, fileType, translations, status);
                }
            #endregion

            #region "Summary"
                public Boolean HasEvaluations(long idCall) {
                    return (from e in Manager.GetIQ<expEvaluation>() where e.Deleted == BaseStatusDeleted.None && e.IdCall == idCall select e.IdSubmission).Any();
                }
                public Boolean HasSubmissionsNotInEvaluation(long idCall, SubmissionStatus status)
                {
                    Boolean result = false;
                    try
                    {
                        List<long> idInEvaluations = (from u in Manager.GetIQ<expEvaluation>() where u.Deleted == BaseStatusDeleted.None && u.IdCall == idCall select u.IdSubmission).Distinct().ToList();

                        var query = (from u in Manager.GetIQ<expSubmission>() where  u.IdCall == idCall select u);
                        if (idInEvaluations.Count <= maxItemsForQuery) 
                            result = query.Where(s => s.Status == status && !idInEvaluations.Contains(s.Id)).Any();
                        else
                            result = query.Where(s => s.Status == status).Select(s=> s.Id).ToList().Where(s=> !idInEvaluations.Contains(s)).Any();
                    }
                    catch (Exception ex) { 
                    
                    }
                    return result;
                }

                public List<EvaluationFilterStatus> GetAvailableEvaluationFilterStatus(long idCall, long idCommittee=0, long idEvaluator = 0)
                {
                    List<EvaluationFilterStatus> filters = new List<EvaluationFilterStatus>();
                    try
                    {
                        var query = (from s in Manager.GetIQ<expEvaluation>() where s.IdCall == idCall 
                                                                                    && (idCommittee<=0 || (idCommittee>0 && s.IdCommittee==idCommittee))
                                                                                    && (idEvaluator==0 || (idEvaluator>0 && s.IdEvaluator==idEvaluator))
                                                                                    && s.Deleted== BaseStatusDeleted.None select s);

                        if (query.Where(s => s.Status == EvaluationStatus.EvaluatorReplacement).Select(s => s.Id).Any())
                             filters.Add(EvaluationFilterStatus.EvaluatorReplacement);
                        if (query.Where(s => s.Status == EvaluationStatus.Invalidated).Select(s => s.Id).Any())
                             filters.Add(EvaluationFilterStatus.Invalidated);
                        if (query.Where(s => s.Status == EvaluationStatus.Evaluating).Select(s => s.Id).Any())
                            filters.Add(EvaluationFilterStatus.Evaluating);

                        List<long> idSubmission = query.Where(s => s.Status == EvaluationStatus.None).Select(s=> s.IdSubmission).ToList();

                        if (idSubmission.Any() && (idEvaluator>0 || (idEvaluator==0 && !query.Where(s => idSubmission.Contains(s.IdSubmission) 
                                                    && s.Status != EvaluationStatus.None && s.Status != EvaluationStatus.EvaluatorReplacement && s.Status != EvaluationStatus.Invalidated).Any())))
                            filters.Add(EvaluationFilterStatus.None);


                        idSubmission = query.Where(s => s.Status == EvaluationStatus.Evaluated).Select(s => s.IdSubmission).ToList();
                        if (idSubmission.Any() && (idEvaluator>0 || (idEvaluator==0 &&!query.Where(s => idSubmission.Contains(s.IdSubmission)
                                                    && s.Status != EvaluationStatus.Evaluated && s.Status != EvaluationStatus.EvaluatorReplacement && s.Status != EvaluationStatus.Invalidated).Any())))
                            filters.Add(EvaluationFilterStatus.Evaluated);

                    }
                    catch (Exception ex) { 
                
                    }
                    if (filters.Count > 1)
                        filters.Insert(0, EvaluationFilterStatus.All);
                    else
                        filters = new List<EvaluationFilterStatus>() {{EvaluationFilterStatus.All}};
                    return filters;   
                }

        public List<EvaluationFilterStatus> GetAvailableEvaluationFilterStatusAdv(long idCall, long idCommittee = 0)
        {
            List<EvaluationFilterStatus> filters = new List<EvaluationFilterStatus>();

            try
            {
                var query = (from ev in Manager.GetIQ<Evaluation>()
                             where ev.Call != null && ev.Call.Id == idCall
                             && ev.AdvCommission != null && ev.AdvCommission.Id == idCommittee
                             && ev.AdvEvaluator != null && ev.AdvEvaluator.Member != null && ev.AdvEvaluator.Member.Id == UC.CurrentUserID
                             && ev.Deleted == BaseStatusDeleted.None
                             select ev
                             );

                if (query.Where(s => s.Status == EvaluationStatus.EvaluatorReplacement).Select(s => s.Id).Any())
                    filters.Add(EvaluationFilterStatus.EvaluatorReplacement);
                if (query.Where(s => s.Status == EvaluationStatus.Invalidated).Select(s => s.Id).Any())
                    filters.Add(EvaluationFilterStatus.Invalidated);
                if (query.Where(s => s.Status == EvaluationStatus.Evaluating).Select(s => s.Id).Any())
                    filters.Add(EvaluationFilterStatus.Evaluating);

                List<long> idSubmission = query.Where(s => s.Status == EvaluationStatus.None && s.Submission != null).Select(s => s.Submission.Id).ToList();

                if (
                        idSubmission.Any() 
                        //&& (
                        //    idEvaluator > 0 
                        //    || 
                        //    (
                        //        idEvaluator == 0 && !query.Where(s => idSubmission.Contains(s.IdSubmission)
                        //        && s.Status != EvaluationStatus.None 
                        //        && s.Status != EvaluationStatus.EvaluatorReplacement 
                        //        && s.Status != EvaluationStatus.Invalidated).Any()
                        //    )
                        //)
                    )
                    filters.Add(EvaluationFilterStatus.None);


                idSubmission = query.Where(s => s.Status == EvaluationStatus.Evaluated && s.Submission != null).Select(s => s.Submission.Id).ToList();

                if (idSubmission.Any()
                    //&& (idEvaluator > 0 || (idEvaluator == 0 && !query.Where(s => idSubmission.Contains(s.IdSubmission)
                    //                             && s.Status != EvaluationStatus.Evaluated && s.Status != EvaluationStatus.EvaluatorReplacement && s.Status != EvaluationStatus.Invalidated).Any()))
                    )
                    filters.Add(EvaluationFilterStatus.Evaluated);

            } catch (Exception ex)
            {

            }

            if (filters.Count > 1)
                filters.Insert(0, EvaluationFilterStatus.All);
            else
                filters = new List<EvaluationFilterStatus>() { { EvaluationFilterStatus.All } };
            return filters;
        }







        public List<dtoEvaluationSummaryItem> GetEvaluationsList(
            long idCall,
            EvaluationType type, 
            dtoEvaluationsFilters filters, 
            String anonymousUser, 
            String unknownUser, 
            Boolean loadRevisionInfo)
        {

            

            List<dtoEvaluationSummaryItem> items = null;
            try{
                List<DssCallEvaluation> dssRatings = DssRatingGetValues(idCall);

                List<expEvaluation> evaluations = 
                        (from e in Manager.GetIQ<expEvaluation>()
                            where e.Deleted == BaseStatusDeleted.None 
                            && e.IdCall == idCall select e)
                            .ToList();

                List<long> idSubmissions = evaluations.Select(e => e.IdSubmission).Distinct().ToList();

                List<dtoBaseSummaryItem> temp = GetBaseEvaluationDisplayItems(
                    idSubmissions, 
                    anonymousUser, 
                    unknownUser, 
                    loadRevisionInfo);

                items = temp
                    .Where(i => idSubmissions.Contains(i.IdSubmission))
                    .Select(i => 
                        new dtoEvaluationSummaryItem(
                            i, 
                            evaluations.Where(e => e.IdSubmission == i.IdSubmission && e.Deleted == BaseStatusDeleted.None).ToList(), 
                            dssRatings.Where(e=> e.IdSubmission==i.IdSubmission).FirstOrDefault()
                            )
                        )
                    .ToList();

                long position = 1;

                switch (type)
                {
                    case EvaluationType.Average:
                        items.OrderByDescending(e => e.AverageRating).ThenBy(i=> i.DisplayName).ToList().ForEach(i => i.Position = position++);
                        break;
                    case EvaluationType.Dss:
                        items.OrderByDescending(e => e.DssRanking).ThenBy(i => i.DisplayName).ToList().ForEach(i => i.Position = position++);
                        break;
                    default:
                        items.OrderByDescending(e => e.SumRating).ThenBy(i=> i.DisplayName).ToList().ForEach(i => i.Position = position++);
                        break;
                }
                    

                if (filters.IdSubmitterType > 0)
                    items = items.Where(i => i.IdSubmitterType == filters.IdSubmitterType).ToList();
                if (!string.IsNullOrEmpty(filters.SearchForName) && !String.IsNullOrEmpty(filters.SearchForName.Trim()))
                {
                    idSubmissions.Clear();
                    List<String> searchName = filters.SearchForName.ToLower().Split(' ').ToList();
                    searchName.ForEach(s => temp.Where(i => i.DisplayName.ToLower().Contains(s)).ToList().ForEach(t => idSubmissions.Add(t.IdSubmission)));
                    items = items.Where(i => idSubmissions.Contains(i.IdSubmission)).ToList();
                }

                Boolean ascending = filters.Ascending; 
                switch (filters.Status) {
                    case EvaluationFilterStatus.All:
                        break;
                    case  EvaluationFilterStatus.AllValid:
                        items = items.Where(i => !i.Evaluations.Where(e => e.Status == EvaluationStatus.EvaluatorReplacement && e.Status == EvaluationStatus.Invalidated).Any()).ToList();
                        break;
                    case EvaluationFilterStatus.EvaluatorReplacement:
                        items = items.Where(i => i.Evaluations.Where(e => e.Status == EvaluationStatus.EvaluatorReplacement).Any()).ToList();
                        break;
                    case EvaluationFilterStatus.Invalidated:
                        items = items.Where(i => i.Evaluations.Where(e => e.Status == EvaluationStatus.Invalidated).Any()).ToList();
                        break;
                    case EvaluationFilterStatus.Evaluated:
                        items = items.Where(i => !i.Evaluations.Where(e => !e.Evaluated).Any()).ToList();
                        break;
                    case EvaluationFilterStatus.Evaluating:
                        items = items.Where(i => i.Evaluations.Where(e => !e.Evaluated && e.Status != EvaluationStatus.None).Any()).ToList();
                        break;
                    case EvaluationFilterStatus.None:
                        items = items.Where(i => !i.Evaluations.Where(e => e.Status!= EvaluationStatus.None && e.Status != EvaluationStatus.EvaluatorReplacement && e.Status != EvaluationStatus.Invalidated).Any()).ToList();
                        break;
                }
                var query = (from q in items select q);

                #region "order"
                switch (filters.OrderBy)
                {
                    case SubmissionsOrder.BySubmittedOn:
                        if (ascending)
                            query = query.OrderBy(s => s.SubmittedOn);
                        else
                            query = query.OrderByDescending(s => s.SubmittedOn);
                        break;
                    //case SubmissionsOrder.ByDate:
                    //    if (ascending)
                    //        query = query.OrderBy(r => r.ModifiedOn);
                    //    else
                    //        query = query.OrderByDescending(r => r.ModifiedOn);
                    //    break;
                    case SubmissionsOrder.ByEvaluationStatus:
                        if (ascending)
                            query = query.OrderBy(r => filters.TranslationsEvaluationStatus[r.Status]);
                        else
                            query = query.OrderByDescending(r => filters.TranslationsEvaluationStatus[r.Status]);
                        break;
                    case SubmissionsOrder.ByType:
                        if (ascending)
                            query = query.OrderBy(s => s.SubmitterType);
                        else
                            query = query.OrderByDescending(s => s.SubmitterType);
                        break;
                    case SubmissionsOrder.ByUser:
                        if (ascending)
                            query = query.OrderBy(s => s.DisplayName);
                        else
                            query = query.OrderByDescending(s => s.DisplayName);
                        break;
                    case SubmissionsOrder.ByEvaluationPoints:
                        //if (ascending)
                        //    query = query.OrderBy(s => s.SumRating).ThenBy(e => e.DisplayName);
                        //else
                        //    query = query.OrderByDescending(s => s.SumRating).ThenBy(e => e.DisplayName);
                        //break;

                        if (ascending)
                        {
                            switch (type)
                            {
                                case EvaluationType.Average:
                                    query = query.OrderBy(s => s.AverageRating).ThenBy(e => e.DisplayName);
                                    break;
                                case EvaluationType.Dss:
                                    query = query.OrderBy(s => s.DssRanking).ThenBy(e => e.DisplayName);
                                    break;
                                default:
                                    query = query.OrderBy(s => s.SumRating).ThenBy(e => e.DisplayName);
                                    break;
                            }
                        }
                        else
                        {
                            switch (type)
                            {
                                case EvaluationType.Average:
                                    query = query.OrderByDescending(s => s.AverageRating).ThenBy(e => e.DisplayName);
                                    break;
                                case EvaluationType.Dss:
                                    query = query.OrderByDescending(s => s.DssRanking).ThenBy(e => e.DisplayName);
                                    break;
                                default:
                                    query = query.OrderByDescending(s => s.SumRating).ThenBy(e => e.DisplayName);
                                    break;
                            }
                        }
                        break;
                    case SubmissionsOrder.ByEvaluationIndex:
                        if (ascending)
                            query = query.OrderBy(s => s.Position).ThenBy(e => e.DisplayName);
                        else
                            query = query.OrderByDescending(s => s.Position).ThenBy(e => e.DisplayName);
                        break;
                    default:
                        if (ascending)
                            query = query.OrderBy(s => s.DisplayName).ThenBy(s => s.SubmitterType);
                        else
                            query = query.OrderByDescending(s => s.DisplayName).ThenBy(s => s.SubmitterType);
                        break;
                }
                #endregion 
                       
                items = query.ToList();
                items.ForEach(i => i.UpdateCounters());
            }
            catch(Exception ex){
                items = new List<dtoEvaluationSummaryItem> ();
            }
            return items;
        }
                public List<expEvaluation> GetEvaluationsList(long idCall, EvaluationType type, expCommittee committee, dtoEvaluationsFilters filters, String anonymousUser, String unknownUser)
                {
                    List<expEvaluation> items = new List<expEvaluation>();
                    try
                    {
                        List<long> idSubmissions = (committee.Evaluations == null) ? new List<long>() : committee.Evaluations.Where(e=>e.Submission !=null).Select(e=>e.Submission.Id).ToList().Distinct().ToList();
                        List<DssCallEvaluation> dssRatings = DssRatingGetValues(idCall);
                        List<dtoBaseSummaryItem> temp = GetBaseEvaluationDisplayItems(idSubmissions, anonymousUser, unknownUser,true);

                        List<dtoEvaluationSummaryItem> summary = temp.Where(i => idSubmissions.Contains(i.IdSubmission)).Select(i => new dtoEvaluationSummaryItem(i, (committee.Evaluations == null) ? null : committee.Evaluations.Where(e => e.IdSubmission == i.IdSubmission).ToList(), dssRatings.Where(e => e.IdSubmission == i.IdSubmission).FirstOrDefault())).ToList();

                        long position = 1;
                        //summary.OrderByDescending(i => i.SumRating).ThenBy(i => i.DisplayName).ToList().ForEach(i => i.Position = position++);
                        switch (type)
                        {
                            case EvaluationType.Average:
                                summary.OrderByDescending(e => e.AverageRating).ThenBy(i => i.DisplayName).ToList().ForEach(i => i.Position = position++);
                                break;
                            case EvaluationType.Dss:
                                summary.OrderByDescending(e => e.DssRanking).ThenBy(i => i.DisplayName).ToList().ForEach(i => i.Position = position++);
                                break;
                            default:
                                summary.OrderByDescending(e => e.SumRating).ThenBy(i => i.DisplayName).ToList().ForEach(i => i.Position = position++);
                                break;
                        }
                        if (filters.IdSubmitterType > 0)
                            summary = summary.Where(i => i.IdSubmitterType == filters.IdSubmitterType).ToList();
                        if (!string.IsNullOrEmpty(filters.SearchForName) && !String.IsNullOrEmpty(filters.SearchForName.Trim()))
                        {
                            idSubmissions.Clear();
                            List<String> searchName = filters.SearchForName.ToLower().Split(' ').ToList();
                            searchName.ForEach(s => temp.Where(i => i.DisplayName.ToLower().Contains(s)).ToList().ForEach(t => idSubmissions.Add(t.IdSubmission)));
                            summary = summary.Where(i => idSubmissions.Contains(i.IdSubmission)).ToList();
                        }

                        Boolean ascending = filters.Ascending;
                        switch (filters.Status)
                        {
                            case EvaluationFilterStatus.All:
                                break;
                            case EvaluationFilterStatus.AllValid:
                                summary = summary.Where(i => !i.Evaluations.Where(e => e.Status == EvaluationStatus.EvaluatorReplacement && e.Status == EvaluationStatus.Invalidated).Any()).ToList();
                                break;
                            case EvaluationFilterStatus.EvaluatorReplacement:
                                summary = summary.Where(i => i.Evaluations.Where(e => e.Status == EvaluationStatus.EvaluatorReplacement).Any()).ToList();
                                break;
                            case EvaluationFilterStatus.Invalidated:
                                summary = summary.Where(i => i.Evaluations.Where(e => e.Status == EvaluationStatus.Invalidated).Any()).ToList();
                                break;
                            case EvaluationFilterStatus.Evaluated:
                                summary = summary.Where(i => !i.Evaluations.Where(e => !e.Evaluated).Any()).ToList();
                                break;
                            case EvaluationFilterStatus.Evaluating:
                                summary = summary.Where(i => i.Evaluations.Where(e => !e.Evaluated && e.Status != EvaluationStatus.None).Any()).ToList();
                                break;
                            case EvaluationFilterStatus.None:
                                summary = summary.Where(i => !i.Evaluations.Where(e => e.Status != EvaluationStatus.None && e.Status != EvaluationStatus.EvaluatorReplacement && e.Status != EvaluationStatus.Invalidated).Any()).ToList();
                                break;
                        }
                        var query = (from q in summary select q);

                        #region "order"
                        switch (filters.OrderBy)
                        {
                            case SubmissionsOrder.BySubmittedOn:
                                if (ascending)
                                    query = query.OrderBy(s => s.SubmittedOn);
                                else
                                    query = query.OrderByDescending(s => s.SubmittedOn);
                                break;
                            //case SubmissionsOrder.ByDate:
                            //    if (ascending)
                            //        query = query.OrderBy(r => r.ModifiedOn);
                            //    else
                            //        query = query.OrderByDescending(r => r.ModifiedOn);
                            //    break;
                            case SubmissionsOrder.ByEvaluationStatus:
                                if (ascending)
                                    query = query.OrderBy(r => filters.TranslationsEvaluationStatus[r.Status]);
                                else
                                    query = query.OrderByDescending(r => filters.TranslationsEvaluationStatus[r.Status]);
                                break;
                            case SubmissionsOrder.ByType:
                                if (ascending)
                                    query = query.OrderBy(s => s.SubmitterType);
                                else
                                    query = query.OrderByDescending(s => s.SubmitterType);
                                break;
                            case SubmissionsOrder.ByUser:
                                if (ascending)
                                    query = query.OrderBy(s => s.DisplayName);
                                else
                                    query = query.OrderByDescending(s => s.DisplayName);
                                break;
                            case SubmissionsOrder.ByEvaluationPoints:
                                //if (ascending)
                                //    query = query.OrderBy(s => s.SumRating).ThenBy(e => e.DisplayName);
                                //else
                                //    query = query.OrderByDescending(s => s.SumRating).ThenBy(e => e.DisplayName);
                                if (ascending)
                                {
                                    switch (type)
                                    {
                                        case EvaluationType.Average:
                                            query = query.OrderBy(s => s.AverageRating).ThenBy(e => e.DisplayName);
                                            break;
                                        case EvaluationType.Dss:
                                            query = query.OrderBy(s => s.DssRanking).ThenBy(e => e.DisplayName);
                                            break;
                                        default:
                                            query = query.OrderBy(s => s.SumRating).ThenBy(e => e.DisplayName);
                                            break;
                                    }
                                }
                                else
                                {
                                    switch (type)
                                    {
                                        case EvaluationType.Average:
                                            query = query.OrderByDescending(s => s.AverageRating).ThenBy(e => e.DisplayName);
                                            break;
                                        case EvaluationType.Dss:
                                            query = query.OrderByDescending(s => s.DssRanking).ThenBy(e => e.DisplayName);
                                            break;
                                        default:
                                            query = query.OrderByDescending(s => s.SumRating).ThenBy(e => e.DisplayName);
                                            break;
                                    }
                                }
                                break;
                            case SubmissionsOrder.ByEvaluationIndex:
                                if (ascending)
                                    query = query.OrderBy(s => s.Position).ThenBy(e => e.DisplayName);
                                else
                                    query = query.OrderByDescending(s => s.Position).ThenBy(e => e.DisplayName);
                                break;
                            default:
                                if (ascending)
                                    query = query.OrderBy(s => s.DisplayName).ThenBy(s => s.SubmitterType);
                                else
                                    query = query.OrderByDescending(s => s.DisplayName).ThenBy(s => s.SubmitterType);
                                break;
                        }
                        #endregion
                        Int32 pageSize = 100;
                        Int32 pageIndex = 0;
                        var evaluations = (from u in Manager.GetIQ<expEvaluation>() select u);
                        List<long> idEvaluations = query.SelectMany(i => i.Evaluations.Select(e => e.Id)).ToList();
                        var evaluationQuery = idEvaluations.Skip(pageIndex * pageSize).Take(pageSize).ToList();
                        while (evaluationQuery.Any())
                        {
                            items.AddRange(evaluations.Where(s => evaluationQuery.Contains(s.Id)).ToList());

                            pageIndex++;
                            evaluationQuery = idEvaluations.Skip(pageIndex * pageSize).Take(pageSize).ToList();
                        }
                    }
                    catch (Exception ex)
                    {
                        items = new List<expEvaluation>();
                    }
                    return items;
                }
                public List<dtoCommittee> GetCommitteesForSummary(long idCall, Boolean loadCriteria)
                {
                    List<dtoCommittee> items = new List<dtoCommittee>();
                    try
                    {
                        List<expCommittee> committes = (from s in Manager.GetIQ<expCommittee>()
                                 where s.Deleted == BaseStatusDeleted.None && s.IdCall == idCall select s).ToList();

                        foreach(expCommittee c in committes){
                            dtoCommittee item = new dtoCommittee() {
                                     Id = c.Id,
                                     Name = c.Name,
                                     Description = c.Description,
                                     DisplayOrder = c.DisplayOrder,
                                     ForAllSubmittersType = c.ForAllSubmittersType,
                                     IdCall = idCall
                                 };
                            if (loadCriteria)
                                item.Criteria = (c.Criteria == null) ? new List<dtoCriterion>() : c.Criteria.Select(cr => new dtoCriterion(cr)).ToList().OrderBy(cr=>cr.DisplayOrder).ThenBy(cr=>cr.Name).ToList();
                            if (!item.ForAllSubmittersType)
                                item.Submitters = (c.SubmitterTypes!=null) ? c.SubmitterTypes.Where(s=>s.Deleted== BaseStatusDeleted.None).Select(s=> s.IdSubmitterType).ToList() : new List<long>();
                            items.Add(item);
                        }
                        //List<long> idCommitees = items.Select(c => c.Id).ToList();
                        //List<lm.Comol.Modules.CallForPapers.Domain.Evaluation.dtoCriterion> criteria = (from bc in Manager.GetIQ<BaseCriterion>()
                        //                                                                                where bc.Committee != null && idCommitees.Contains(bc.Committee.Id) && bc.Deleted == BaseStatusDeleted.None
                        //                                                                                select bc).ToList().Select(bc => new lm.Comol.Modules.CallForPapers.Domain.Evaluation.dtoCriterion(bc)).ToList();
                        
                        //    items.ForEach(i => i.Criteria = (from c in criteria where c.IdCommittee == i.Id orderby c.DisplayOrder, c.Name select c).ToList());
                        //items.Where(i => !i.ForAllSubmittersType).ToList().ForEach(c =>);
                    }
                    catch (Exception ex)
                    {

                    }
                    return items.OrderBy(s => s.DisplayOrder).ThenBy(s => s.Name).ToList();
                }
                public List<dtoCommitteesSummaryItem> GetCommitteesSummary(long idCall, EvaluationType type, dtoEvaluationsFilters filters, String anonymousUser, String unknownUser, List<dtoCommittee> committees)
                {
                    List<dtoCommitteesSummaryItem> items = null;
                    try
                    {
                        List<expEvaluation> evaluations = (from e in Manager.GetIQ<expEvaluation>() where e.IdCall == idCall select e).ToList();
                        List<long> idSubmissions = evaluations.Select(e => e.IdSubmission).Distinct().ToList();
                        List<dtoBaseSummaryItem> temp = GetBaseEvaluationDisplayItems(idSubmissions, anonymousUser, unknownUser,false);

                        List<DssCallEvaluation> dssRatings = DssRatingGetValues(idCall, DssEvaluationType.Committee);
                        items = temp.Where(i => idSubmissions.Contains(i.IdSubmission)).Select(i => CreateCommitteesSummaryItem(i, committees, evaluations.Where(e => e.IdSubmission == i.IdSubmission).ToList(), dssRatings.Where(r=> r.IdSubmission== i.IdSubmission))).ToList();

                        long position = 1;
                        //items.OrderByDescending(i => i.SumRating).ThenBy(i => i.DisplayName).ToList().ForEach(i => i.Position = position++);

                        switch (type)
                        {
                            case EvaluationType.Average:
                                items.OrderByDescending(e => e.AverageRating).ThenBy(i => i.DisplayName).ToList().ForEach(i => i.Position = position++);
                                break;
                            case EvaluationType.Dss:
                                items.OrderByDescending(e => e.DssRanking).ThenBy(i => i.DisplayName).ToList().ForEach(i => i.Position = position++);
                                break;
                            default:
                                items.OrderByDescending(e => e.SumRating).ThenBy(i => i.DisplayName).ToList().ForEach(i => i.Position = position++);
                                break;
                        }

                        if (filters.IdSubmitterType > 0)
                            items = items.Where(i => i.IdSubmitterType == filters.IdSubmitterType).ToList();

                        if (!string.IsNullOrEmpty(filters.SearchForName) && !String.IsNullOrEmpty(filters.SearchForName.Trim()))
                        {
                            idSubmissions.Clear();
                            List<String> searchName = filters.SearchForName.ToLower().Split(' ').ToList();
                            searchName.ForEach(s => temp.Where(i => i.DisplayName.ToLower().Contains(s)).ToList().ForEach(t => idSubmissions.Add(t.IdSubmission)));
                            items = items.Where(i => idSubmissions.Contains(i.IdSubmission)).ToList();
                        }
                        Boolean ascending = filters.Ascending;
                        switch (filters.Status)
                        {
                            case EvaluationFilterStatus.All:
                                break;
                            case EvaluationFilterStatus.AllValid:
                                items = items.Where(i => !i.Committees.Where(e => e.Status == EvaluationStatus.EvaluatorReplacement && e.Status == EvaluationStatus.Invalidated).Any()).ToList();
                                break;
                            case EvaluationFilterStatus.EvaluatorReplacement:
                                items = items.Where(i => i.Committees.Where(e => e.Status == EvaluationStatus.EvaluatorReplacement).Any()).ToList();
                                break;
                            case EvaluationFilterStatus.Invalidated:
                                items = items.Where(i => i.Committees.Where(e => e.Status == EvaluationStatus.Invalidated).Any()).ToList();
                                break;
                            case EvaluationFilterStatus.Evaluated:
                                items = items.Where(i => !i.Committees.Where(e => !e.Evaluated).Any()).ToList();
                                break;
                            case EvaluationFilterStatus.Evaluating:
                                items = items.Where(i => i.Committees.Where(e => !e.Evaluated && e.Status != EvaluationStatus.None).Any()).ToList();
                                break;
                            case EvaluationFilterStatus.None:
                                items = items.Where(i => !i.Committees.Where(e => e.Status != EvaluationStatus.None && e.Status != EvaluationStatus.EvaluatorReplacement && e.Status != EvaluationStatus.Invalidated).Any()).ToList();
                                break;
                        }
                        var query = (from q in items select q);

                        #region "order"
                        switch (filters.OrderBy)
                        {
                            case SubmissionsOrder.BySubmittedOn:
                                if (ascending)
                                    query = query.OrderBy(s => s.SubmittedOn);
                                else
                                    query = query.OrderByDescending(s => s.SubmittedOn);
                                break;
                            //case SubmissionsOrder.ByDate:
                            //    if (ascending)
                            //        query = query.OrderBy(r => r.ModifiedOn);
                            //    else
                            //        query = query.OrderByDescending(r => r.ModifiedOn);
                            //    break;
                            case SubmissionsOrder.ByEvaluationStatus:
                                if (ascending)
                                    query = query.OrderBy(r => filters.TranslationsEvaluationStatus[r.Status]);
                                else
                                    query = query.OrderByDescending(r => filters.TranslationsEvaluationStatus[r.Status]);
                                break;
                            case SubmissionsOrder.ByType:
                                if (ascending)
                                    query = query.OrderBy(s => s.SubmitterType);
                                else
                                    query = query.OrderByDescending(s => s.SubmitterType);
                                break;
                            case SubmissionsOrder.ByUser:
                                if (ascending)
                                    query = query.OrderBy(s => s.DisplayName);
                                else
                                    query = query.OrderByDescending(s => s.DisplayName);
                                break;
                            case SubmissionsOrder.ByEvaluationPoints:
                                //if (ascending)
                                //    query = query.OrderBy(s => s.SumRating).ThenBy(e => e.DisplayName);
                                //else
                                //    query = query.OrderByDescending(s => s.SumRating).ThenBy(e => e.DisplayName);
                                if (ascending)
                                {
                                    switch (type)
                                    {
                                        case EvaluationType.Average:
                                            query = query.OrderBy(s => s.AverageRating).ThenBy(e => e.DisplayName);
                                            break;
                                        case EvaluationType.Dss:
                                            query = query.OrderBy(s => s.DssRanking).ThenBy(e => e.DisplayName);
                                            break;
                                        default:
                                            query = query.OrderBy(s => s.SumRating).ThenBy(e => e.DisplayName);
                                            break;
                                    }
                                }
                                else
                                {
                                    switch (type)
                                    {
                                        case EvaluationType.Average:
                                            query = query.OrderByDescending(s => s.AverageRating).ThenBy(e => e.DisplayName);
                                            break;
                                        case EvaluationType.Dss:
                                            query = query.OrderByDescending(s => s.DssRanking).ThenBy(e => e.DisplayName);
                                            break;
                                        default:
                                            query = query.OrderByDescending(s => s.SumRating).ThenBy(e => e.DisplayName);
                                            break;
                                    }
                                }
                                break;
                            case SubmissionsOrder.ByEvaluationIndex:
                                if (ascending)
                                    query = query.OrderBy(s => s.Position).ThenBy(e => e.DisplayName);
                                else
                                    query = query.OrderByDescending(s => s.Position).ThenBy(e => e.DisplayName);
                                break;
                            default:
                                if (ascending)
                                    query = query.OrderBy(s => s.DisplayName).ThenBy(s => s.SubmitterType);
                                else
                                    query = query.OrderByDescending(s => s.DisplayName).ThenBy(s => s.SubmitterType);
                                break;
                        }
                        #endregion

                        items = query.ToList();
                        items.ForEach(i => i.UpdateCounters());
                    }
                    catch (Exception ex)
                    {
                        items = new List<dtoCommitteesSummaryItem>();
                    }
                    return items;
                }

                public List<dtoCommitteeSummaryItem> GetCommitteeSummary(long idCommittee, long idCall, EvaluationType type, dtoEvaluationsFilters filters, String anonymousUser, String unknownUser, List<dtoBaseCommitteeMember> members, List<expCommitteeMember> evaluators)
                {
                    List<dtoCommitteeSummaryItem> items = null;
                    try
                    {
                        expCommittee committee = Manager.Get<expCommittee>(idCommittee);
                        List<long> idSubmissions =(committee == null || committee.Evaluations == null) ? new List<long>() : committee.Evaluations.Where(e=>e.IdCommittee== idCommittee).Select(e=> e.IdSubmission).ToList().Distinct().ToList();
                        List<dtoBaseSummaryItem> temp = GetBaseEvaluationDisplayItems(idSubmissions, anonymousUser, unknownUser,false);

                        List<DssCallEvaluation> dssRatings = DssRatingGetValues(idCall, idCommittee);
                        items = temp.Where(i => idSubmissions.Contains(i.IdSubmission)).Select(i => CreateCommitteeSummaryItem(committee, i, (committee.Criteria != null) ? committee.Criteria.ToList().Select(bc => new dtoCriterionSummaryItem(bc)).ToList() : new List<dtoCriterionSummaryItem>(), members, evaluators, dssRatings.Where(r=> r.IdSubmission== i.IdSubmission && r.IdCommittee== idCommittee).FirstOrDefault())).ToList();

                        long position = 1;

                        switch (type)
                        {
                            case EvaluationType.Average:
                                items.OrderByDescending(e => e.AverageRating).ThenBy(i => i.DisplayName).ToList().ForEach(i => i.Position = position++);
                                break;
                            case EvaluationType.Dss:
                                items.OrderByDescending(e => e.DssRanking).ThenBy(i => i.DisplayName).ToList().ForEach(i => i.Position = position++);
                                break;
                            default:
                                items.OrderByDescending(e => e.SumRating).ThenBy(i => i.DisplayName).ToList().ForEach(i => i.Position = position++);
                                break;
                        }

                        if (filters.IdSubmitterType > 0)
                            temp = temp.Where(i => i.IdSubmitterType == filters.IdSubmitterType).ToList();
                        if (!string.IsNullOrEmpty(filters.SearchForName) && !String.IsNullOrEmpty(filters.SearchForName.Trim()))
                        {
                            idSubmissions.Clear();
                            List<String> searchName = filters.SearchForName.ToLower().Split(' ').ToList();
                            searchName.ForEach(s => temp.Where(i => i.DisplayName.ToLower().Contains(s)).ToList().ForEach(t => idSubmissions.Add(t.IdSubmission)));
                            items = items.Where(i => idSubmissions.Contains(i.IdSubmission)).ToList();
                        }

                        Boolean ascending = filters.Ascending;

                        #region "filter Status"
                        switch (filters.Status)
                        {
                            case EvaluationFilterStatus.All:
                                break;
                            case EvaluationFilterStatus.AllValid:
                                items = items.Where(i => !i.Evaluators.Where(e => e.Status == EvaluationStatus.EvaluatorReplacement && e.Status == EvaluationStatus.Invalidated).Any()).ToList();
                                break;
                            case EvaluationFilterStatus.EvaluatorReplacement:
                                items = items.Where(i => i.Evaluators.Where(e => e.Status == EvaluationStatus.EvaluatorReplacement).Any()).ToList();
                                break;
                            case EvaluationFilterStatus.Invalidated:
                                items = items.Where(i => i.Evaluators.Where(e => e.Status == EvaluationStatus.Invalidated).Any()).ToList();
                                break;
                            case EvaluationFilterStatus.Evaluated:
                                items = items.Where(i => !i.Evaluators.Where(e => !e.Evaluated).Any()).ToList();
                                break;
                            case EvaluationFilterStatus.Evaluating:
                                items = items.Where(i => i.Evaluators.Where(e => !e.Evaluated && e.Status != EvaluationStatus.None).Any()).ToList();
                                break;
                            case EvaluationFilterStatus.None:
                                items = items.Where(i => !i.Evaluators.Where(e => e.Status != EvaluationStatus.None && e.Status != EvaluationStatus.EvaluatorReplacement && e.Status != EvaluationStatus.Invalidated).Any()).ToList();
                                break;
                        }
                        #endregion

                        var query = (from q in items select q);

                        #region "order"
                        switch (filters.OrderBy)
                        {
                            case SubmissionsOrder.BySubmittedOn:
                                if (ascending)
                                    query = query.OrderBy(s => s.SubmittedOn);
                                else
                                    query = query.OrderByDescending(s => s.SubmittedOn);
                                break;
                            //case SubmissionsOrder.ByDate:
                            //    if (ascending)
                            //        query = query.OrderBy(r => r.ModifiedOn);
                            //    else
                            //        query = query.OrderByDescending(r => r.ModifiedOn);
                            //    break;
                            case SubmissionsOrder.ByEvaluationStatus:
                                if (ascending)
                                    query = query.OrderBy(r => filters.TranslationsEvaluationStatus[r.Status]);
                                else
                                    query = query.OrderByDescending(r => filters.TranslationsEvaluationStatus[r.Status]);
                                break;
                            case SubmissionsOrder.ByType:
                                if (ascending)
                                    query = query.OrderBy(s => s.SubmitterType);
                                else
                                    query = query.OrderByDescending(s => s.SubmitterType);
                                break;
                            case SubmissionsOrder.ByUser:
                                if (ascending)
                                    query = query.OrderBy(s => s.DisplayName);
                                else
                                    query = query.OrderByDescending(s => s.DisplayName);
                                break;
                            case SubmissionsOrder.ByEvaluationPoints:
                                //if (ascending)
                                //    query = query.OrderBy(s => s.SumRating).ThenBy(e => e.DisplayName);
                                //else
                                //    query = query.OrderByDescending(s => s.SumRating).ThenBy(e => e.DisplayName);
                                if (ascending)
                                {
                                    switch (type)
                                    {
                                        case EvaluationType.Average:
                                            query = query.OrderBy(s => s.AverageRating).ThenBy(e => e.DisplayName);
                                            break;
                                        case EvaluationType.Dss:
                                            query = query.OrderBy(s => s.DssRanking).ThenBy(e => e.DisplayName);
                                            break;
                                        default:
                                            query = query.OrderBy(s => s.SumRating).ThenBy(e => e.DisplayName);
                                            break;
                                    }
                                }
                                else
                                {
                                    switch (type)
                                    {
                                        case EvaluationType.Average:
                                            query = query.OrderByDescending(s => s.AverageRating).ThenBy(e => e.DisplayName);
                                            break;
                                        case EvaluationType.Dss:
                                            query = query.OrderByDescending(s => s.DssRanking).ThenBy(e => e.DisplayName);
                                            break;
                                        default:
                                            query = query.OrderByDescending(s => s.SumRating).ThenBy(e => e.DisplayName);
                                            break;
                                    }
                                }
                                break;
                            case SubmissionsOrder.ByEvaluationIndex:
                                if (ascending)
                                    query = query.OrderBy(s => s.Position).ThenBy(e => e.DisplayName);
                                else
                                    query = query.OrderByDescending(s => s.Position).ThenBy(e => e.DisplayName);
                                break;
                            default:
                                if (ascending)
                                    query = query.OrderBy(s => s.DisplayName).ThenBy(s => s.SubmitterType);
                                else
                                    query = query.OrderByDescending(s => s.DisplayName).ThenBy(s => s.SubmitterType);
                                break;
                        }
                        #endregion

                        items = query.ToList();
                        //items.ForEach(i => i.UpdateCounters());
                    }
                    catch (Exception ex)
                    {
                        items = new List<dtoCommitteeSummaryItem>();
                    }
                    return items;
                }
                private dtoCommitteeSummaryItem CreateCommitteeSummaryItem(expCommittee committee,dtoBaseSummaryItem item, List<dtoCriterionSummaryItem> criteria,List<dtoBaseCommitteeMember> members, List<expCommitteeMember> evaluators,DssCallEvaluation dssEvaluation)
                {
                    dtoCommitteeSummaryItem result = new dtoCommitteeSummaryItem(item);
                    result.DssEvaluation = dssEvaluation;
                    result.Criteria = criteria.OrderBy(c=> c.DisplayOrder).ThenBy(c=>c.Name).ToList();
                    result.Criteria.ForEach(c => c.IdSubmissionReferee = item.IdSubmission);
                    if (result.Criteria.Any())
                    {
                        if (result.Criteria.Count == 1)
                            result.Criteria[0].DisplayAs = displayAs.first | displayAs.last;
                        else
                        {
                            result.Criteria.First().DisplayAs = displayAs.first;
                            result.Criteria.Last().DisplayAs = displayAs.last;
                        }
                    }
                    result.Evaluators = members.Select(e => CreateEvaluatorDisplayItem(committee, criteria, evaluators.Where(ev => ev.Id == e.IdMembership).FirstOrDefault(), item.DisplayName, item.IdSubmission)).ToList();
                    result.Criteria.ForEach(c=> c.LoadEvaluations(result.Evaluators));
                    return result;
                }
                private dtoEvaluatorDisplayItem CreateEvaluatorDisplayItem(expCommittee committee, List<dtoCriterionSummaryItem> criteria, expCommitteeMember membership, String submitterName, long idSubmission)
                {
                    dtoEvaluatorDisplayItem item = new dtoEvaluatorDisplayItem();
                    item.IdSubmission = idSubmission;
                    item.IdMembership = membership.Id;
                    item.IdEvaluator = (membership.Evaluator != null) ? membership.Evaluator.Id : 0;
                    item.Name = (membership.Evaluator !=null && membership.Evaluator.Person != null) ? membership.Evaluator.Person.Name : "";
                    item.Surname = (membership.Evaluator !=null && membership.Evaluator.Person != null) ? membership.Evaluator.Person.Surname: "";
                    item.SubmitterName = submitterName;
                    item.MembershipStatus = membership.Status;
                    expEvaluation evaluation = null;
                    DssCallEvaluation dssEvaluation = null;
                    try
                    {
                        evaluation = committee.Evaluations.Where(e=> e.IdSubmission== idSubmission && e.IdEvaluator== membership.Evaluator.Id).FirstOrDefault();
                                             /*where e.IdSubmission == idSubmission && e.IdEvaluator==membership.Evaluator.Id 
                                             select e).Skip(0).Take(1).ToList().FirstOrDefault();*/
                        if (evaluation != null)
                            dssEvaluation = DssRatingGetByEvaluation(evaluation.Id);
                    }
                    catch(Exception ex){
                    
                    }
                    if (evaluation != null)
                    {
                        //List<CriterionEvaluated> values = (from cv in Manager.GetIQ<CriterionEvaluated>() where cv.Evaluation.Id == evaluation.Id && cv.Deleted == BaseStatusDeleted.None select cv).ToList();
                        item.AverageRating = evaluation.AverageRating;
                        item.Comment = evaluation.Comment;
                        item.Evaluated = evaluation.Evaluated;
                        item.EvaluatedOn = evaluation.EvaluatedOn;
                        item.EvaluationStartedOn = evaluation.EvaluationStartedOn;
                        item.IdEvaluation = evaluation.Id;
                        item.ModifiedOn = evaluation.LastUpdateOn;
                        item.Status = evaluation.Status;
                        item.SumRating = evaluation.SumRating;
                        item.DssEvaluation = dssEvaluation;
                        item.AverageRating = evaluation.AverageRating;
                        criteria.OrderBy(c => c.DisplayOrder).ThenBy(c => c.Name).ToList().ForEach(c => item.Values.Add(new dtoCriterionEvaluated(c, (evaluation.EvaluatedCriteria!=null)? evaluation.EvaluatedCriteria.Where(v => v.Criterion.Id == c.Id).FirstOrDefault():null)));
                    }
                    else {
                        item.IgnoreEvaluation = true;
                        switch (membership.Status) { 
                            case MembershipStatus.Removed:
                                item.Status = EvaluationStatus.Invalidated;
                                break;
                            case MembershipStatus.Replaced:
                                item.Status = EvaluationStatus.EvaluatorReplacement;
                                break;
                            default:
                                item.Status = EvaluationStatus.None;
                                break;
                        }
                        criteria.OrderBy(c => c.DisplayOrder).ThenBy(c => c.Name).ToList().ForEach(c => item.Values.Add(new dtoCriterionEvaluated(c, true )));
                    }
                    return item;
                }
                private dtoCommitteesSummaryItem CreateCommitteesSummaryItem(dtoBaseSummaryItem item, List<dtoCommittee> committees, List<expEvaluation> evaluations, IEnumerable<DssCallEvaluation> dssRatings)
                {
                    dtoCommitteesSummaryItem result = new dtoCommitteesSummaryItem(item, dssRatings.Where(r=> r.Type== DssEvaluationType.Call).FirstOrDefault());
                    result.Committees = committees.Select(c=> new dtoCommitteeDisplayItem(){ IdCommittee = c.Id , CommitteeName= c.Name,
                                                                                             Evaluations = evaluations.Where(e => e.IdCommittee == c.Id).Select(e => dtoEvaluationDisplayItem.GetForCommitteesSummaryItem(e)).ToList(),
                                                                                             DssEvaluation= dssRatings.Where(r=> r.Type== DssEvaluationType.Committee && r.IdCommittee== c.Id).FirstOrDefault()}).ToList();
                    result.UpdateCounters();
                    return result;
                }
                private List<dtoBaseSummaryItem> GetBaseEvaluationDisplayItems(
                        List<long> idSubmissions, 
                        String anonymousUser, 
                        String unknownUser, 
                        Boolean loadIdRevision)
                {
                    List<dtoBaseSummaryItem> results = new List<dtoBaseSummaryItem>();
                    Int32 pageSize = 100;
                    Int32 pageIndex = 0;
                    List<expRevision> revisions = null;
                    var submissions = (from u in Manager.GetIQ<expSubmission>() select u);
                    var rQuery = (from r in Manager.GetIQ<expRevision>() where r.IsActive select r);
                    var submissionQuery = idSubmissions.Skip(pageIndex * pageSize).Take(pageSize).ToList();
                    while (submissionQuery.Any())
                    {
                        if (loadIdRevision)
                            revisions = rQuery.Where(r => submissionQuery.Contains(r.IdSubmission)).ToList();
                        results.AddRange(submissions.Where(s => submissionQuery.Contains(s.Id)).ToList().Select(s => new dtoBaseSummaryItem(s, anonymousUser,unknownUser,revisions)).ToList());
                        pageIndex++;
                        submissionQuery = idSubmissions.Skip(pageIndex * pageSize).Take(pageSize).ToList();
                    }
                    return results;
                }

                public String GetStatisticFileName(long idCall,String callName,String filename, SummaryType type, ItemsToExport items, ExportData exportData)
                {
                    return GetStatisticFileName(idCall,callName, 0,"", filename, type, items, exportData);
                }
                public String GetStatisticFileName(long idCall, String callName, Int32 comitteeDisplayOrder, String commissionName, String filename, SummaryType type, ItemsToExport items, ExportData exportData)
                {
                    String result = filename;
                    DateTime data = DateTime.Now;
                    if (!String.IsNullOrEmpty(callName))
                        callName = callName.Trim();
                    if (!String.IsNullOrEmpty(callName) && callName.Length > 30)
                        callName = idCall.ToString();
                    if (!String.IsNullOrEmpty(commissionName))
                        commissionName = commissionName.Trim();
                    if (!String.IsNullOrEmpty(commissionName) && commissionName.Length > 30)
                        commissionName = comitteeDisplayOrder.ToString();
                    if (type== SummaryType.Committee && ( items== ItemsToExport.Filtered || exportData== ExportData.DisplayData))
                        result = String.Format(filename, commissionName, callName, data.Year, ((data.Month < 10) ? "0" : "") + data.Month.ToString(), ((data.Day < 10) ? "0" : "") + data.Day.ToString());
                    else
                        result = String.Format(filename, callName, data.Year, ((data.Month < 10) ? "0" : "") + data.Month.ToString(), ((data.Day < 10) ? "0" : "") + data.Day.ToString());

                    return lm.Comol.Core.DomainModel.Helpers.Export.ExportBaseHelper.HtmlCheckFileName(result);
                }

                public String GetStatisticFileName(long idCall, String callName, long idCommittee, Int32 comitteeDisplayOrder, String commissionName, long idSubmission, String filename, SummaryType type)
                {
                    String result = filename;
                    DateTime data = DateTime.Now;
                    if (!String.IsNullOrEmpty(callName))
                        callName = callName.Trim();
                    if (!String.IsNullOrEmpty(callName) && callName.Length > 30)
                        callName = idCall.ToString();
                    if (!String.IsNullOrEmpty(commissionName))
                        commissionName = commissionName.Trim();
                    if (!String.IsNullOrEmpty(commissionName) && commissionName.Length > 30)
                        commissionName = comitteeDisplayOrder.ToString();
                    switch (type) { 
                        case SummaryType.EvaluationCommittee:
                            result = String.Format(filename, commissionName, callName, data.Year, ((data.Month < 10) ? "0" : "") + data.Month.ToString(), ((data.Day < 10) ? "0" : "") + data.Day.ToString(),idSubmission);
                            break;
                        case SummaryType.EvaluationCommittees:
                            result = String.Format(filename,  callName, data.Year, ((data.Month < 10) ? "0" : "") + data.Month.ToString(), ((data.Day < 10) ? "0" : "") + data.Day.ToString(),idSubmission);
                            break;
                    }
                    return lm.Comol.Core.DomainModel.Helpers.Export.ExportBaseHelper.HtmlCheckFileName(result);
                }
        public String ExportSummaryStatistics(
            dtoCall call, 
            dtoEvaluationsFilters filters, 
            String anonymousDisplayName, 
            String unknownUserDisplayName, 
            SummaryType summaryType, 
            ItemsToExport itemsToExport, 
            ExportData exportData, 
            lm.Comol.Core.DomainModel.Helpers.Export.ExportFileType fileType, 
            Dictionary<lm.Comol.Modules.CallForPapers.Domain.Evaluation.EvaluationTranslations, String> translations, 
            Dictionary<lm.Comol.Modules.CallForPapers.Domain.Evaluation.EvaluationStatus, String> status, 
            long idCommittee = 0)
        {
            HelperExportToXml helperXml = null;
            HelperExportToCsv helperCsv = null; 
            litePerson person = null;
            try
            {
                switch (fileType) { 
                    case Core.DomainModel.Helpers.Export.ExportFileType.xml:
                    case Core.DomainModel.Helpers.Export.ExportFileType.xls:
                        helperXml = new HelperExportToXml(translations, status);
                        break;
                    case Core.DomainModel.Helpers.Export.ExportFileType.csv:
                        helperCsv = new HelperExportToCsv(translations, status);
                        break;
                }
                dtoEvaluationsFilters filterToUse = new dtoEvaluationsFilters();
                person = Manager.GetLitePerson(UC.CurrentUserID);
                if (person != null)
                {
                    if (itemsToExport == ItemsToExport.All)
                    {
                        filterToUse.Ascending = true;
                        filterToUse.OrderBy = SubmissionsOrder.ByEvaluationIndex;
                        filterToUse.Status = EvaluationFilterStatus.All;
                        filterToUse.TranslationsEvaluationStatus = filters.TranslationsEvaluationStatus;
                    }
                    else
                        filterToUse = filters;

                    switch (exportData)
                    {
                        case ExportData.DisplayData:
                            switch (summaryType)
                            {
                                case SummaryType.Evaluations:
                                    return (helperXml != null) ? helperXml.ExportSummaryDisplayStatistics(call, GetEvaluationsList(call.Id,call.EvaluationType, filterToUse, anonymousDisplayName, unknownUserDisplayName, false ), person)
                                        :
                                        (helperCsv != null) ? helperCsv.ExportSummaryDisplayStatistics(call, GetEvaluationsList(call.Id, call.EvaluationType, filterToUse, anonymousDisplayName, unknownUserDisplayName, false), person)
                                        :"";
                                case SummaryType.Committees:
                                    List<dtoCommittee> committees = GetCommitteesForSummary(call.Id, false);
                                    return (helperXml != null) ? helperXml.ExportSummaryDisplayStatistics(call, committees, GetCommitteesSummary(call.Id, call.EvaluationType, filters, anonymousDisplayName, unknownUserDisplayName, committees), person)
                                        :
                                        (helperCsv != null) ? helperCsv.ExportSummaryDisplayStatistics(call, committees, GetCommitteesSummary(call.Id, call.EvaluationType, filters, anonymousDisplayName, unknownUserDisplayName, committees), person)
                                        :"";
                                case SummaryType.Committee:
                                    expCommittee committee = Manager.Get<expCommittee>(idCommittee);
                                    if (committee != null && committee.Evaluations != null && committee.Evaluations.Any())
                                    {
                                        return (helperXml != null) ? ""// helperXml.ExportSummaryDisplayStatistics(call, committee, GetEvaluationsList(call.Id, committee.Id,filterToUse, anonymousDisplayName), cvQuery, anonymousDisplayName, person)
                                        :
                                        (helperCsv != null) ? helperCsv.ExportSummaryDisplayStatistics(call, committee, GetEvaluationsList(call.Id,call.EvaluationType, committee, filterToUse, anonymousDisplayName, unknownUserDisplayName), anonymousDisplayName, person)
                                        :"";
                                    }
                                    else
                                        return GetErrorDocument(call, person, fileType, translations, status);
                            }
                            break;
                        case ExportData.Fulldata:
                        case ExportData.FulldataToAnalyze:
                            if(!call.AdvacedEvaluation)
                            {
                                List<lm.Comol.Modules.CallForPapers.Domain.Evaluation.Export.expCommittee> committes = (from c in Manager.GetIQ<lm.Comol.Modules.CallForPapers.Domain.Evaluation.Export.expCommittee>() where c.IdCall == call.Id select c).OrderBy(c => c.DisplayOrder).ThenBy(c => c.Name).ToList();
                                switch (summaryType)
                                {
                                    case SummaryType.Evaluations:
                                    case SummaryType.Committees:

                                        return (helperXml != null) ? "" //helperXml.ExportFullSummaryStatistics(call, committes, anonymousDisplayName, person, (exportData == ExportData.FulldataToAnalyze))
                                            :
                                            (helperCsv != null) ? helperCsv.ExportFullSummaryStatistics(call, committes, anonymousDisplayName, person, (exportData == ExportData.FulldataToAnalyze))
                                                : "";
                                    case SummaryType.Committee:

                                        break;
                                    }
                            } else
                            {
                                List<lm.Comol.Modules.CallForPapers.Domain.Evaluation.Export.expCommittee> committes = 
                                    (from c in Manager.GetIQ<lm.Comol.Modules.CallForPapers.Domain.Evaluation.Export.expCommittee>()
                                     where c.IdCall == call.Id select c)
                                     .OrderBy(c => c.DisplayOrder)
                                     .ThenBy(c => c.Name).ToList();

                                switch (summaryType)
                                {
                                    case SummaryType.Evaluations:
                                    case SummaryType.Committees:

                                        return (helperXml != null) ? "" //helperXml.ExportFullSummaryStatistics(call, committes, anonymousDisplayName, person, (exportData == ExportData.FulldataToAnalyze))
                                            :
                                            (helperCsv != null) ? helperCsv.ExportFullSummaryStatistics(call, committes, anonymousDisplayName, person, (exportData == ExportData.FulldataToAnalyze))
                                                : "";
                                    case SummaryType.Committee:

                                        break;
                                }



                            }
                        break;
                                    
                    }
                }
            }
            catch(Exception ex){
                return GetErrorDocument(call, person, fileType, translations, status);
            }
            return GetErrorDocument(call, person, fileType, translations,status);
        }



      

        public String ExportSummaryStatistics(SummaryType summaryType, dtoCall call, dtoSubmissionRevision submission,long idSubmission, long idCommittee, String anonymousDisplayName, String unknownUserDisplayName, ExportData exportData, lm.Comol.Core.DomainModel.Helpers.Export.ExportFileType fileType, Dictionary<lm.Comol.Modules.CallForPapers.Domain.Evaluation.EvaluationTranslations, String> translations, Dictionary<lm.Comol.Modules.CallForPapers.Domain.Evaluation.EvaluationStatus, String> status)
                {
                    HelperExportToXml helperXml = null;
                    HelperExportToCsv helperCsv = null;
                    litePerson person = null;
                    try
                    {
                        switch (fileType)
                        {
                            case Core.DomainModel.Helpers.Export.ExportFileType.xml:
                            case Core.DomainModel.Helpers.Export.ExportFileType.xls:
                                helperXml = new HelperExportToXml(translations, status);
                                break;
                            case Core.DomainModel.Helpers.Export.ExportFileType.csv:
                                helperCsv = new HelperExportToCsv(translations, status);
                                break;
                        }
                        person = Manager.GetLitePerson(UC.CurrentUserID);
                        if (person != null)
                        {
                            List<dtoSubmissionCommitteeItem> evaluations = GetSubmissionEvaluations(call.Id, idSubmission, idCommittee, unknownUserDisplayName);
                            String owner = (submission == null || (submission != null && (submission.IsAnonymous))) ? anonymousDisplayName : ((submission.Owner != null) ? submission.Owner.SurnameAndName : unknownUserDisplayName);
                            litePerson submitter = Manager.GetLitePerson(submission.IdSubmittedBy);
                            String submittedBy = (submission.IdPerson == submission.IdSubmittedBy) ? "" : (submitter == null || submitter.TypeID == (int)UserTypeStandard.Guest) ? anonymousDisplayName : submitter.SurnameAndName;

                            Dictionary<long, Boolean> dssCommitteesInfo = GetCommitteeDssMethodIsFuzzy(call.Id);
                            switch (summaryType)
                            {
                                case SummaryType.EvaluationCommittee:
                                    return (helperXml != null) ? ""//helperXml.ExportSummaryDisplayStatistics(call, evaluations, person, unknownUserDisplayName )
                                        :
                                        (helperCsv != null) ? helperCsv.ExportSummaryDisplayStatistics(person, call, dssCommitteesInfo,idSubmission, owner, submittedBy, submission.SubmittedOn, evaluations, (exportData == ExportData.FulldataToAnalyze))
                                        : "";
                                case SummaryType.EvaluationCommittees:
                                    return (helperXml != null) ? ""//helperXml.ExportSummaryDisplayStatistics(call, evaluations, person)
                                        :
                                        (helperCsv != null) ? helperCsv.ExportSummaryDisplayStatistics(person, call, dssCommitteesInfo, idSubmission, owner, submittedBy, submission.SubmittedOn, evaluations, (exportData == ExportData.FulldataToAnalyze))
                                        : "";
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        return GetErrorDocument(call, person, fileType, translations, status);
                    }
                    return GetErrorDocument(call, person, fileType, translations, status);
                }


                public String GetErrorDocument(dtoCall call, litePerson person, lm.Comol.Core.DomainModel.Helpers.Export.ExportFileType fileType, Dictionary<lm.Comol.Modules.CallForPapers.Domain.Evaluation.EvaluationTranslations, String> translations, Dictionary<lm.Comol.Modules.CallForPapers.Domain.Evaluation.EvaluationStatus, String> status)
                {
                    switch (fileType)
                    {
                        case Core.DomainModel.Helpers.Export.ExportFileType.xls:
                        case Core.DomainModel.Helpers.Export.ExportFileType.xml:
                            HelperExportToXml hXML = new HelperExportToXml(translations, status);
                            return hXML.GetErrorDocument(call, person);
                        case Core.DomainModel.Helpers.Export.ExportFileType.csv:
                            HelperExportToCsv hCSV = new HelperExportToCsv(translations, status);
                            return hCSV.GetErrorDocument(call, person);
                        default:
                            return "";
                    }
                }
            #endregion
    
            #region "View evaluation"
                public List<dtoCommitteeEvaluation> GetEvaluationsInfo(long idEvaluator,long idSubmission, String anonymousUser, String unknownUser)
                {
                    List<dtoCommitteeEvaluation> items = new List<dtoCommitteeEvaluation>();
                    try
                    {
                        List<long> idCommittees = (from m in Manager.GetIQ<expCommitteeMember>()
                                                   where m.Deleted == BaseStatusDeleted.None && m.Status != MembershipStatus.Removed && m.Evaluator.Id == idEvaluator
                                                   select m.IdCommittee).ToList();
                        items = (from s in Manager.GetIQ<EvaluationCommittee>()
                                 where s.Deleted == BaseStatusDeleted.None && idCommittees.Contains(s.Id)
                                 select new dtoCommitteeEvaluation()
                                 {
                                     IdCommittee = s.Id,
                                     Name = s.Name,
                                     DisplayOrder = s.DisplayOrder,
                                     Status = EvaluationStatus.None
                                 }).ToList();

                        var query = (from e in Manager.GetIQ<Evaluation>()
                                     where e.Deleted == BaseStatusDeleted.None && e.Evaluator.Id == idEvaluator
                                     && e.Submission != null && e.Submission.Id == idSubmission
                                     select e);
                        foreach (dtoCommitteeEvaluation c in items)
                        {
                            long idEvaluation = query.Where(e => e.Committee.Id == c.IdCommittee).Select(e => e.Id).Skip(0).Take(1).ToList().FirstOrDefault();
                            if (idEvaluation > 0)
                            {
                                c.Evaluation = GetFullEvaluation(idEvaluation, anonymousUser, unknownUser);
                                c.Status = (c.Evaluation != null) ? c.Evaluation.Status : EvaluationStatus.None;
                            }
                        }
                    }
                    catch (Exception ex)
                    {

                    }
                    return items.OrderBy(s => s.DisplayOrder).ThenBy(s => s.Name).ToList();
                }
                public List<dtoCommitteeEvaluationInfo> GetCommitteesInfoForEvaluator(long idEvaluator)
                {
                    List<dtoCommitteeEvaluationInfo> items = new List<dtoCommitteeEvaluationInfo>();
                    try
                    {
                        List<long> idCommittees = (from m in Manager.GetIQ<CommitteeMember>()
                                                   where m.Deleted == BaseStatusDeleted.None && m.Status != MembershipStatus.Removed && m.Evaluator.Id == idEvaluator
                                                   select m.Committee.Id).ToList();
                        items = (from s in Manager.GetIQ<EvaluationCommittee>()
                                 where s.Deleted == BaseStatusDeleted.None && idCommittees.Contains(s.Id)
                                 select new dtoCommitteeEvaluationInfo()
                                 {
                                     IdCommittee = s.Id,
                                     Name = s.Name,
                                     DisplayOrder= s.DisplayOrder,
                                     Status= EvaluationStatus.None
                                 }).ToList();

                        var query = (from e in Manager.GetIQ<Evaluation>()
                                     where e.Deleted == BaseStatusDeleted.None && e.Evaluator.Id == idEvaluator
                                     select e);
                        foreach (dtoCommitteeEvaluationInfo c in items) {
                            c.Status = GetCommitteeEvaluationsStatistics(c.IdCommittee, query);
                        }
                    }
                    catch (Exception ex)
                    {

                    }
                    return items.OrderBy(s => s.DisplayOrder).ThenBy(s => s.Name).ToList();
                }
                public List<dtoCommitteeEvaluationsDisplayItem> GetSubmissionEvaluations(long idSubmission, long idCall, String unknownUser)
                {
                    List<dtoCommitteeEvaluationsDisplayItem> items = new List<dtoCommitteeEvaluationsDisplayItem>();
                    try
                    {
                        long idType = (from s in Manager.GetIQ<UserSubmission>() where s.Id == idSubmission select s.Type.Id).Skip(0).Take(1).ToList().FirstOrDefault();
                        List<long> idCommittees = (from m in Manager.GetIQ<CommitteeAssignedSubmitterType>()
                                                   where m.Deleted == BaseStatusDeleted.None && m.SubmitterType.Id == idType
                                                   select m.Committee.Id).ToList();
                        List<DssCallEvaluation> dssCommitteesEvaluation = DssRatingGetValues(idCall, DssEvaluationType.Committee).Where(e =>  e.Type == DssEvaluationType.Committee && e.IdSubmission == idSubmission).ToList();
                        items = (from s in Manager.GetIQ<EvaluationCommittee>()
                                 where s.Deleted == BaseStatusDeleted.None && (idCommittees.Contains(s.Id) || (s.ForAllSubmittersType && s.Call.Id==idCall))
                                 orderby s.DisplayOrder orderby s.Name select s).ToList().Select(s=>
                                  new dtoCommitteeEvaluationsDisplayItem()
                                 {
                                    IdCommittee = s.Id,
                                    CommitteeName = s.Name,
                                    Id= s.Id,
                                    DssEvaluation = dssCommitteesEvaluation.Where(e => e.IdCommittee == s.Id).FirstOrDefault()
                                 }).ToList();

                        if (items.Any() && items.Count == 1)
                            items[0].Display = displayAs.first | displayAs.last;
                        else if (items.Any() && items.Count > 1) {
                            items.First().Display = displayAs.first;
                            items.Last().Display = displayAs.last;
                        }
                        List<DssCallEvaluation> dssSubmissionsEvaluation = DssRatingGetEvaluationValues(idCall, items.Select(i=> i.IdCommittee).Distinct().ToList());
                        //List<dtoCommitteeEvaluatorEvaluationDisplayItem> evaluations = (from e in Manager.GetIQ<expEvaluation>()
                        //                                              where e.IdSubmission == idSubmission && e.Deleted == BaseStatusDeleted.None
                        //                                                                select e).ToList().Select(e => dtoCommitteeEvaluatorEvaluationDisplayItem.GetForEvaluationsDisplay(e, dssEvaluations.Where(d => d.IdSubmission == e.Id && d.Type == DssEvaluationType.Committee && d.IdCommittee == e.IdCommittee).FirstOrDefault(), unknownUser)).ToList();
                        List<dtoCommitteeEvaluatorEvaluationDisplayItem> evaluations = (from e in Manager.GetIQ<expEvaluation>()
                                                                                        where e.IdSubmission == idSubmission && e.Deleted == BaseStatusDeleted.None
                                                                                        select e).ToList().Select(e => dtoCommitteeEvaluatorEvaluationDisplayItem.GetForEvaluationsDisplay(e, dssSubmissionsEvaluation.Where(d => d.IdEvaluation == e.Id).FirstOrDefault(), unknownUser)).ToList();

                        foreach (dtoCommitteeEvaluationsDisplayItem comittee in items)
                        {

                            List<lm.Comol.Modules.CallForPapers.Domain.Evaluation.dtoCriterion> criteria = (from bc in Manager.GetIQ<BaseCriterion>()
                                                                                                            where bc.Committee != null && bc.Committee.Id == comittee.IdCommittee && bc.Deleted == BaseStatusDeleted.None
                                                                                                            select bc).ToList().Select(bc => new lm.Comol.Modules.CallForPapers.Domain.Evaluation.dtoCriterion(bc)).ToList();
                            comittee.Evaluations = evaluations.Where(e => e.IdCommittee == comittee.IdCommittee).ToList();
                            if (comittee.Evaluations.Any() && comittee.Evaluations.Count == 1)
                                comittee.Evaluations[0].Display = displayAs.first | displayAs.last;
                            else if (comittee.Evaluations.Any() && comittee.Evaluations.Count > 1)
                            {
                                comittee.Evaluations.First().Display = displayAs.first;
                                comittee.Evaluations.Last().Display = displayAs.last;
                            }

                            foreach (dtoCommitteeEvaluatorEvaluationDisplayItem evaluation in comittee.Evaluations)
                            {
                                List<CriterionEvaluated> values = (from cv in Manager.GetIQ<CriterionEvaluated>() where cv.Evaluation.Id == evaluation.Id && cv.Deleted == BaseStatusDeleted.None select cv).ToList();
                                evaluation.Criteria = criteria.OrderBy(c => c.DisplayOrder).ThenBy(c => c.Name).ToList().Select(c => new dtoCriterionEvaluatedDisplayItem(c, values.Where(v => v.Criterion.Id == c.Id).FirstOrDefault())).ToList();
                                if (evaluation.Criteria.Any() && evaluation.Criteria.Count == 1)
                                    evaluation.Criteria[0].Display = displayAs.first | displayAs.last;
                                else if (evaluation.Criteria.Any() && evaluation.Criteria.Count > 1)
                                {
                                    evaluation.Criteria.First().Display = displayAs.first;
                                    evaluation.Criteria.Last().Display = displayAs.last;
                                }
                                foreach (dtoCriterionEvaluatedDisplayItem criterion in evaluation.Criteria)
                                {
                                    criterion.IdCommittee = comittee.IdCommittee;
                                    criterion.IdEvaluator = evaluation.IdEvaluator;
                                }

                            }
                        }
                    }
                    catch (Exception ex)
                    {

                    }
                    return items;
                }
                public List<dtoCommitteeEvaluationInfo> GetCommitteesInfoForSubmission(long idSubmission, long idCall)
                {
                    List<dtoCommitteeEvaluationInfo> items = new List<dtoCommitteeEvaluationInfo>();
                    try
                    {
                        long idType = (from s in Manager.GetIQ<UserSubmission>() where s.Id == idSubmission select s.Type.Id).Skip(0).Take(1).ToList().FirstOrDefault();
                        List<long> idCommittees = (from m in Manager.GetIQ<CommitteeAssignedSubmitterType>()
                                                   where m.Deleted == BaseStatusDeleted.None && m.SubmitterType.Id == idType
                                                   select m.Committee.Id).ToList();
                        items = (from s in Manager.GetIQ<EvaluationCommittee>()
                                 where s.Deleted == BaseStatusDeleted.None && (idCommittees.Contains(s.Id) || (s.ForAllSubmittersType && s.Call.Id == idCall))
                                 select new dtoCommitteeEvaluationInfo()
                                 {
                                     IdCommittee = s.Id,
                                     Name = s.Name,
                                     DisplayOrder = s.DisplayOrder
                                  }).ToList();
                        var query = (from e in Manager.GetIQ<Evaluation>()
                                     where e.Deleted == BaseStatusDeleted.None && e.Submission.Id== idSubmission
                                     select e);

                        foreach (dtoCommitteeEvaluationInfo c in items)
                        {
                            c.Status = GetCommitteeEvaluationsStatistics(c.IdCommittee, query);
                        }
                    }
                    catch (Exception ex)
                    {

                    }
                    return items.OrderBy(s => s.DisplayOrder).ThenBy(s => s.Name).ToList();
                }
                //public List<dtoCommitteeEvaluationInfo> GetCommitteesInfoForEvaluation(long idEvaluation)
                //{
                //    List<dtoCommitteeEvaluationInfo> items = new List<dtoCommitteeEvaluationInfo>();
                //    try
                //    {
                //        long idType = (from s in Manager.GetIQ<Evaluation>() where s.Id == idEvaluation select s.).Skip(0).Take(1).ToList().FirstOrDefault();
                //        List<long> idCommittees = (from m in Manager.GetIQ<CommitteeAssignedSubmitterType>()
                //                                   where m.Deleted == BaseStatusDeleted.None && m.SubmitterType.Id == idType && m.
                //                                   select m.Committee.Id).ToList();
                //        items = (from s in Manager.GetIQ<EvaluationCommittee>()
                //                 where s.Deleted == BaseStatusDeleted.None && idCommittees.Contains(s.Id)
                //                 select new dtoCommitteeEvaluationsInfo()
                //                 {
                //                     IdCommittee = s.Id,
                //                     Name = s.Name,
                //                     Description = s.Description,
                //                     DisplayOrder = s.DisplayOrder,
                //                     IdEvaluator = idEvaluator
                //                 }).ToList();
                //        var query = (from e in Manager.GetIQ<Evaluation>()
                //                     where e.Call.Id == idCall && e.Deleted == BaseStatusDeleted.None && e.Evaluator.Id == idEvaluator
                //                     select e);

                //        items.ForEach(i => i.Counters = GetEvaluatorStatistics(idEvaluator, i.IdCommittee, query));
                //    }
                //    catch (Exception ex)
                //    {

                //    }
                //    return items.OrderBy(s => s.DisplayOrder).ThenBy(s => s.Name).ToList();
                //}

                private Domain.Evaluation.EvaluationStatus GetCommitteeEvaluationsStatistics(long idCommittee, IQueryable<Evaluation> query)
                {
                    Domain.Evaluation.EvaluationStatus status = EvaluationStatus.None;
                    List<Domain.Evaluation.EvaluationStatus> items = query.Where(q => q.Deleted == BaseStatusDeleted.None && q.Committee.Id == idCommittee).Select(q => q.Status).ToList();
                    if (items.Any()){
                        List<Domain.Evaluation.EvaluationStatus> availableStatus = items.Where(i=> i != EvaluationStatus.Invalidated && i != EvaluationStatus.EvaluatorReplacement).ToList();

                        status = (items.Distinct().Count() == 1) ? items[0] : (availableStatus.Distinct().Count() == 1) ? availableStatus[0] : EvaluationStatus.Evaluating;
                        }
                    else
                        status = EvaluationStatus.None;

                    return status;
                }

        public List<dtoSubmissionCommitteeItem> GetSubmissionEvaluations(long idCall,long idSubmission, long idCommittee, String unknownUser)
        {
            List<dtoSubmissionCommitteeItem> items = new List<dtoSubmissionCommitteeItem>();
            try
            {
                List<expEvaluation> evaluations = (from e in Manager.GetIQ<expEvaluation>()
                                                    where e.IdSubmission == idSubmission 
                                                        && (idCommittee <= 0 || (idCommittee > 0 && e.IdCommittee == idCommittee)) 
                                                        && e.Deleted == BaseStatusDeleted.None
                                                    select e)
                                                    .ToList();

                List<long> idCommittees = evaluations.Select(e => e.IdCommittee).Distinct().ToList();

                List<DssCallEvaluation> dssEvaluations = DssRatingGetValues(idCall, idCommittees);

                List<DssCallEvaluation> dssEvaluatorEvaluations = DssRatingGetEvaluationValues(idCall, idCommittees);

                foreach (var c in evaluations.GroupBy(e => e.Committee))
                {
                    List<expCommitteeMember> members = 
                            (from m in Manager.GetIQ<expCommitteeMember>()
                                where m.Deleted == BaseStatusDeleted.None 
                                && m.IdCommittee == c.Key.Id
                                select m)
                                .ToList();

                    dtoSubmissionCommitteeItem item = new dtoSubmissionCommitteeItem()
                    {
                        Name = c.Key.Name,
                        IdSubmission = idSubmission,
                        IdCommittee = c.Key.Id,
                        DssEvaluation = dssEvaluations.Where(e=> e.IdCommittee==c.Key.Id && e.IdSubmission== idSubmission).FirstOrDefault()
                    };

                    item.Criteria = 
                            (c.Key.Criteria != null) ? 
                            c.Key.Criteria
                                .OrderBy(cr => cr.DisplayOrder)
                                .ThenBy(cr => cr.Name)
                                .Select(cr => new dtoCriterionSummaryItem(cr, members.Count))
                                .ToList() 
                                : new List<dtoCriterionSummaryItem>();

                    if (item.Criteria.Any())
                    {
                        if (item.Criteria.Count == 1)
                            item.Criteria[0].DisplayAs = displayAs.first | displayAs.last;
                        else
                        {
                            item.Criteria.First().DisplayAs = displayAs.first;
                            item.Criteria.Last().DisplayAs = displayAs.last;
                        }
                    }
                    item.Evaluators = 
                        members.Select(e => 
                            CreateEvaluatorDisplayItem(
                                c.Key.Id, 
                                idSubmission,
                                item.Criteria, 
                                e.Status, 
                                e.Evaluator, 
                                c.FirstOrDefault(g => g.IdEvaluator == e.Evaluator.Id), 
                                dssEvaluatorEvaluations
                                    .Where(d => 
                                        d.IdEvaluation == 
                                            c.Where(x => x.IdEvaluator == e.Evaluator.Id).Select(x => x.Id).DefaultIfEmpty(0).FirstOrDefault())
                                    .FirstOrDefault(), 
                                unknownUser)
                            ).ToList();

                    item.Criteria.ForEach(cr => cr.LoadEvaluations(item.Evaluators));
                    items.Add(item);
                }
            }
            catch (Exception ex)
            {

            }
                    
            //ToDo: verificare, poi ottimizzare sta roba
            foreach(var itm in items)
            {

                itm.Evaluators = itm.Evaluators.OrderBy(e => e.Name).ThenBy(e => e.IdMembership).ToList();
                    
                foreach (var eval in itm.Criteria)
                {
                    eval.Evaluations = 
                        eval.Evaluations.OrderBy(e => e.Evaluator.Name).ThenBy(e => e.Evaluator.IdMembership).ToList();
                }
            }

            return items;
        }

                private dtoEvaluatorDisplayItem CreateEvaluatorDisplayItem(long idCommittees, long idSubmission,List<dtoCriterionSummaryItem> criteria, MembershipStatus status, expEvaluator evaluator, expEvaluation evaluation, DssCallEvaluation dssEvaluation, String unknownUser)
                {
                    dtoEvaluatorDisplayItem item = new dtoEvaluatorDisplayItem();
                    item.IdEvaluator = (evaluator != null) ? evaluator.Id : 0;
                    item.Name = (evaluator != null && evaluator.Person !=null) ? evaluator.Person.Name : "";
                    item.Surname = (evaluator != null && evaluator.Person !=null) ? evaluator.Person.Surname : unknownUser;
                    item.EvaluatorName = unknownUser;
                    item.MembershipStatus = status;
                    item.IdSubmission = idSubmission;
                    item.IdCommittee = idCommittees;
                    if (evaluation != null)
                    {
                        item.AverageRating = evaluation.AverageRating;
                        item.Comment = evaluation.Comment;
                        item.Evaluated = evaluation.Evaluated;
                        item.EvaluatedOn = evaluation.EvaluatedOn;
                        item.EvaluationStartedOn = evaluation.EvaluationStartedOn;
                        item.IdEvaluation = evaluation.Id;
                        item.ModifiedOn = evaluation.LastUpdateOn;
                        item.Status = evaluation.Status;
                        item.SumRating = evaluation.SumRating;
                        item.AverageRating = evaluation.AverageRating;
                        item.DssEvaluation = dssEvaluation;
                        criteria.OrderBy(c => c.DisplayOrder).ThenBy(c => c.Name).ToList().ForEach(c => item.Values.Add(new dtoCriterionEvaluated(c, (evaluation.EvaluatedCriteria != null) ? evaluation.EvaluatedCriteria.Where(v => v.Criterion.Id == c.Id).FirstOrDefault() : null)));
                    }
                    else
                    {
                        item.IgnoreEvaluation = true;
                        switch (status)
                        {
                            case MembershipStatus.Removed:
                                item.Status = EvaluationStatus.Invalidated;
                                break;
                            case MembershipStatus.Replaced:
                                item.Status = EvaluationStatus.EvaluatorReplacement;
                                break;
                            default:
                                item.Status = EvaluationStatus.None;
                                break;
                        }
                        criteria.OrderBy(c => c.DisplayOrder).ThenBy(c => c.Name).ToList().ForEach(c => item.Values.Add(new dtoCriterionEvaluated(c, true)));
                    }
                    return item;
                }
            #endregion
        #endregion
    }
}