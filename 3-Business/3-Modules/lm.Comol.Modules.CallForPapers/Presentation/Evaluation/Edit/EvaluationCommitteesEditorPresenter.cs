using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel;
using lm.Comol.Modules.CallForPapers.Business;
using lm.Comol.Core.Business;
using lm.Comol.Modules.CallForPapers.Domain.Evaluation;
using lm.Comol.Modules.CallForPapers.Domain;
using lm.Comol.Core.Dss.Business;
using lm.Comol.Core.Dss.Domain.Templates;

namespace lm.Comol.Modules.CallForPapers.Presentation.Evaluation
{
    public class EvaluationCommitteesEditorPresenter : lm.Comol.Core.DomainModel.Common.DomainPresenter
    {
        #region "Initialize"

            public virtual BaseModuleManager CurrentManager { get; set; }
            protected virtual IViewEvaluationCommitteesEditor View
            {
                get { return (IViewEvaluationCommitteesEditor)base.View; }
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
            private ServiceDss _ServiceDss;
            private ServiceDss ServiceDss
            {
                get
                {
                    if (_ServiceDss == null)
                        _ServiceDss = new ServiceDss(AppContext);
                    return _ServiceDss;
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
        public EvaluationCommitteesEditorPresenter(iApplicationContext oContext)
            : base(oContext)
        {
            this.CurrentManager = new BaseModuleManager(oContext);
        }
        public EvaluationCommitteesEditorPresenter(iApplicationContext oContext, IViewEvaluationCommitteesEditor view)
            : base(oContext, view)
        {
            this.CurrentManager = new BaseModuleManager(oContext);
        }
        #endregion

        public void InitView(Boolean allowUseOfDssMethods)
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
                    View.AllowSave = allowSave && (!Service.CallHasEvaluationStarted(idCall));
                    View.AllowSaveBaseInfo = allowSave && (Service.CallHasEvaluationStarted(idCall));
                    View.IdCall = idCall;
                    View.SetActionUrl(RootObject.ViewCalls(idCall, type, CallStandardAction.Manage, idCommunity, View.PreloadView));
                    LoadCommittees(idCall, allowUseOfDssMethods);
                    View.SendUserAction(idCommunity, idModule, idCall, ModuleCallForPaper.ActionType.ManageCommittee); 
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
        private void LoadCommittees(long idCall,Boolean allowUseOfDssMethods)
        {
            BaseForPaper call = CallService.GetCall(idCall);
            if (call != null)
                LoadCommittees(call, allowUseOfDssMethods);
        }
        private void LoadCommittees(BaseForPaper call, Boolean allowUseOfDssMethods, long idMethod =-1, long idRatingSet = -1)
        {
            List<dtoCommittee> committees = Service.GetEditorCommittees(call);
            List<dtoSubmitterType> submitters = CallService.GetCallAvailableSubmittersType(call);
            CallForPaper cfp = Service.GetCallForCommiteeSettings(call.Id);
            Boolean useDssMethod = allowUseOfDssMethods && (cfp != null && cfp != null && cfp.EvaluationType == EvaluationType.Dss);
            View.AllowSubmittersSelection = !useDssMethod && (committees.Count > 0) && submitters.Count > 0;
            View.UseDssMethods = useDssMethod;
            View.LoadSubmitterTypes(submitters);

            List<lm.Comol.Core.Wizard.NavigableWizardItem<dtoEvaluationStep>> steps = Service.GetAvailableSteps(call, WizardEvaluationStep.none);
            Int32 count = committees.Count;
            View.CommitteesCount = count;
            List<dtoSelectMethod> methods = null;
            if (useDssMethod)
            {
                methods = ServiceDss.MethodsGetAvailable(UserContext.Language.Id);
                View.CurrentMethods=methods;
            }
            if (committees == null || committees.Count == 0)
            {
                if (!Service.isNewCommittee(call))
                {
                    View.CommitteesCount = 0;
                    View.DisplayError(EvaluationEditorErrors.NoCommittees);
                    View.LoadWizardSteps(call.Id, View.IdCommunity, steps, EvaluationEditorErrors.NoCommittees);
                    View.DisplayDssErrors(new List<dtoCommittee>());
                }
                else
                {
                    EvaluationCommittee committee = Service.AddFirstCommittee(call, View.DefaultCommitteeName, View.DefaultCommitteeDescription, useDssMethod);
                    if (committee != null)
                    {
                        committees = new List<dtoCommittee>();
                        committees.Add(new dtoCommittee() {
                            Id = committee.Id,
                            Description = committee.Description,
                            DisplayOrder = committee.DisplayOrder,
                            UseDss = committee.UseDss,
                            WeightSettings = new Core.Dss.Domain.Templates.dtoItemWeightSettings(),
                            MethodSettings = new Core.Dss.Domain.Templates.dtoItemMethodSettings()
                                {
                                    InheritsFromFather = false
                                },
                            Name = committee.Name,
                            ForAllSubmittersType = true });

                        count = 1;
                        View.CommitteesCount = count;
                        View.LoadCommittees(committees);
                    }
                    else
                        View.CommitteesCount = 0;
                    if (useDssMethod && committees.Any(c => c.HasDssErrors(count)))
                    {
                        //lm.Comol.Core.Wizard.NavigableWizardItem<dtoEvaluationStep> step = steps.Where(s=> s.Id.Type== WizardEvaluationStep.GeneralSettings).FirstOrDefault();
                        //if (step!=null){
                        //    step.Id.Errors.Add(EditingErrors.CommitteeDssSettings);
                        //    if (step.Status== Core.Wizard.WizardItemStatus.valid || step.Status== Core.Wizard.WizardItemStatus.none)
                        //        step.Status= Core.Wizard.WizardItemStatus.error;
                        //}
                        View.DisplayDssErrors(committees.Where(c => c.HasDssErrors(1)).ToList());
                    }
                    else
                        View.DisplayDssErrors(new List<dtoCommittee>());
                    View.LoadWizardSteps(call.Id, View.IdCommunity, steps);
                }
            }
            else
            {
                if (useDssMethod && committees.Any(c => c.HasDssErrors(count)))
                {
                    View.DisplayDssErrors(committees.Where(c => c.HasDssErrors(count)).ToList());
                    //lm.Comol.Core.Wizard.NavigableWizardItem<dtoEvaluationStep> step = steps.Where(s => s.Id.Type == WizardEvaluationStep.GeneralSettings).FirstOrDefault();
                    //if (step != null)
                    //{
                    //    step.Id.Errors.Add(EditingErrors.CommitteeDssSettings);
                    //    if (step.Status == Core.Wizard.WizardItemStatus.valid || step.Status == Core.Wizard.WizardItemStatus.none)
                    //        step.Status = Core.Wizard.WizardItemStatus.error;
                    //}
                }
                else
                    View.DisplayDssErrors(new List<dtoCommittee>());
                View.LoadWizardSteps(call.Id, View.IdCommunity, steps);
                if (useDssMethod)
                {
                    if(committees.Count > 1 )
                        View.InitializeAggregationMethods(methods, (idMethod == -1 ? cfp.IdDssMethod : idMethod), (idRatingSet == -1 ? cfp.IdDssRatingSet : idRatingSet), GetAvailableWeights(call,call.IsDssMethodFuzzy, call.UseOrderedWeights, committees));
                    else
                        View.HideCallAggregationMethods(methods, (idMethod == -1 ? cfp.IdDssMethod : idMethod), (idRatingSet == -1 ? cfp.IdDssRatingSet : idRatingSet), GetAvailableWeights(call, call.IsDssMethodFuzzy, call.UseOrderedWeights, committees));
                }

                    
                View.LoadCommittees(committees);
            }
        }
        public List<lm.Comol.Core.Dss.Domain.Templates.dtoItemWeightBase> RequireCommitteesWeight(long idCall, Boolean fuzzyWeights, Boolean orderedWeights, List<dtoCommittee> committees)
        {
            BaseForPaper call = CallService.GetCall(idCall);
            if (call != null)
            {
                return GetAvailableWeights(call,fuzzyWeights,orderedWeights, committees,true);
            }
            else
                return new List<dtoItemWeightBase>();
        }
        public List<lm.Comol.Core.Dss.Domain.Templates.dtoItemWeightBase> RequireCommitteeWeight(long idCall, Boolean fuzzyWeights, Boolean orderedWeights, dtoCommittee committee)
        {
            return GetAvailableWeights(fuzzyWeights, orderedWeights,committee);
        }
        private List<lm.Comol.Core.Dss.Domain.Templates.dtoItemWeightBase> GetAvailableWeights(BaseForPaper call, Boolean fuzzyWeights, Boolean orderedWeights, List<dtoCommittee> committees, Boolean emptyValues = false )
        {
            Dictionary<long, String> weights = null;
            if (!emptyValues &&(orderedWeights == call.UseOrderedWeights || fuzzyWeights == call.IsDssMethodFuzzy))
            {
                weights = call.GetFuzzyMeItems();
                if (!fuzzyWeights)
                    weights.Values.Where(v => v.Contains(";")).ToList().ForEach(v => v = "");
            }
            else
                weights = new Dictionary<long, string>();
            
            List<lm.Comol.Core.Dss.Domain.Templates.dtoItemWeightBase> items = null;
            if (orderedWeights)
            {
                if (committees.Count() > 1)
                {
                    if (committees.Count() == weights.Count)
                        items = (from int i in Enumerable.Range(1, committees.Count()) select i).ToList().Select(i => new lm.Comol.Core.Dss.Domain.Templates.dtoItemWeightBase() { IdObject = (long)i, IsFuzzyValue = fuzzyWeights, OrderedItem = true, Name = i.ToString(), Value = (weights.ContainsKey((long)i) ? weights[(long)i] : "") }).ToList();
                    else
                    {
                        Int32 index = 1;
                        items = (from int i in Enumerable.Range(1, committees.Count()) select i).ToList().Select(i => new lm.Comol.Core.Dss.Domain.Templates.dtoItemWeightBase() { IdObject = (long)i, IsFuzzyValue = fuzzyWeights, OrderedItem = true, Name = i.ToString(), Value = "" }).ToList();
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
                                    foreach (String v in values.Skip(1).Take(values.Count - 1))
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
                items = committees.Select(c => c.ToWeightItem(weights, fuzzyWeights)).ToList();
            return items;
        }
        private List<lm.Comol.Core.Dss.Domain.Templates.dtoItemWeightBase> GetAvailableWeights(Boolean fuzzyWeights, Boolean orderedWeights, dtoCommittee committee)
        {
            Dictionary<long, String> weights = new Dictionary<long, string>();
            //if (orderedWeights == committee.MethodSettings.UseOrderedWeights || fuzzyWeights == committee.MethodSettings.IsFuzzyMethod)
            //{
            //    weights = committee.GetFuzzyMeItems();
            //    if (!fuzzyWeights)
            //        weights.Values.Where(v => v.Contains(";")).ToList().ForEach(v => v = "");
            //}
            //else
            //    weights = new Dictionary<long, string>();

            List<lm.Comol.Core.Dss.Domain.Templates.dtoItemWeightBase> items = null;
            if (orderedWeights)
            {
                if (committee.Criteria.Count() > 1)
                {
                    if (committee.Criteria.Count() == weights.Count)
                        items = (from int i in Enumerable.Range(1, committee.Criteria.Count()) select i).ToList().Select(i => new lm.Comol.Core.Dss.Domain.Templates.dtoItemWeightBase() { IdObject = (long)i, IsFuzzyValue = fuzzyWeights, OrderedItem = true, Name = i.ToString(), Value = (weights.ContainsKey((long)i) ? weights[(long)i] : "") }).ToList();
                    else
                    {
                        Int32 index = 1;
                        items = (from int i in Enumerable.Range(1, committee.Criteria.Count()) select i).ToList().Select(i => new lm.Comol.Core.Dss.Domain.Templates.dtoItemWeightBase() { IdObject = (long)i, IsFuzzyValue = fuzzyWeights, OrderedItem = true, Name = i.ToString(), Value = "" }).ToList();
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
                                    foreach (String v in values.Skip(1).Take(values.Count - 1))
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
                items = committee.Criteria.Select(c => c.ToWeightItem(weights, fuzzyWeights)).ToList();
            return items;
        }
 
        public void SaveSettings(List<dtoCommittee> committees, Boolean allowUseOfDssMethods, Boolean useDssMethod)
        {
            long idCall = View.IdCall;
            Int32 idCommunity = View.IdCommunity;
            if (!Service.SaveCommittees(idCall, committees, (useDssMethod ? View.GetCallDssSettings(): null)))
                View.DisplayError(EvaluationEditorErrors.Saving);
            else
            {
                LoadCommittees(View.IdCall, allowUseOfDssMethods);
                View.DisplaySettingsSaved();
                View.SendUserAction(idCommunity, View.IdCallModule, idCall, ModuleCallForPaper.ActionType.SaveCommitteeSettings);
            }
        }

        #region Committee management
            public void AddCommittee(List<dtoCommittee> committees, Boolean useDssMethod, String name, String description)
            {
                long idCall = View.IdCall;
                Int32 idCommunity = View.IdCommunity;
                EvaluationCommittee committee = Service.AddCommitteeToCall(idCall, committees, name, description, useDssMethod, (useDssMethod ? View.GetCallDssSettings() : null));
                if (committee == null)
                    View.DisplayError(EvaluationEditorErrors.AddingCommittee);
                else
                {
                    View.SendUserAction(idCommunity, View.IdCallModule, idCall, ModuleCallForPaper.ActionType.AddCommittee);
                    View.ReloadEditor(RootObject.CommitteeAddedToCall(committee.Id, idCall, idCommunity, View.PreloadView));
                }
            }
            public void RemoveCommitte(List<dtoCommittee> committees, Boolean useDssMethod, long idCommittee)
            {
                long pIdCommittee = 0;
                long idCall = View.IdCall;
                Int32 idCommunity = View.IdCommunity;

                try
                {
                    lm.Comol.Core.Dss.Domain.Templates.dtoItemMethodSettings method = (useDssMethod ? View.GetCallDssSettings() : null);
                    Service.SaveCommittees(idCall, committees, method);
                    if (Service.VirtualDeleteCommittee(idCommittee, true, ref pIdCommittee))
                    {
                        if (useDssMethod)
                        {
                            if (!Service.RemoveDssInheritsFromCommittee(idCall, committees.Where(c => c.Id != pIdCommittee), method)) {
                                Service.UpdateCallManualSettings(idCall,committees, idCommittee);
                            }
                        }
                        View.SendUserAction(idCommunity, View.IdCallModule, idCall, ModuleCallForPaper.ActionType.VirtualDeleteCommittee);
                        View.ReloadEditor(RootObject.CommitteeRemovedFromCall(pIdCommittee, idCall, idCommunity, View.PreloadView));
                    }
                    else
                        View.DisplayError(EvaluationEditorErrors.RemovingCommittee);
                }
                catch (EvaluationStarted exSubmission)
                {
                    View.DisplayError(EvaluationEditorErrors.RemovingCommittee);
                }
                catch (Exception ex)
                {
                    View.DisplayError(EvaluationEditorErrors.RemovingCommittee);
                }
            }
        #endregion
        #region Criterion management
            public void RemoveCriterion(List<dtoCommittee> committees, Boolean useDssMethod, long idCriterion)
            {
                long pIdCommittee = 0;
                long pIdCriterion = 0;
                long idCall = View.IdCall;
                Int32 idCommunity = View.IdCommunity;

                try
                {
                    lm.Comol.Core.Dss.Domain.Templates.dtoItemMethodSettings settings = (useDssMethod ? View.GetCallDssSettings() : null);
                    Service.SaveCommittees(idCall, committees, settings);
                    if (Service.VirtualDeleteCriterion(idCriterion, true, ref pIdCommittee, ref pIdCriterion))
                    {
                        if (useDssMethod)
                            Service.UpdateCommitteeManualSettings(committees,idCriterion);
                        View.SendUserAction(idCommunity, View.IdCallModule, idCall, ModuleCallForPaper.ActionType.VirtualDeleteCriterion);
                        if (pIdCriterion != 0)
                            View.ReloadEditor(RootObject.CriterionRemovedFromCommittee(pIdCriterion, idCall, idCommunity, View.PreloadView));
                        else
                            View.ReloadEditor(RootObject.CommitteeRemovedFromCall(pIdCommittee, idCall, idCommunity, View.PreloadView));
                    }
                    else
                        View.DisplayError(EvaluationEditorErrors.RemovingCriterion);
                }
                catch (EvaluationStarted exSubmission)
                {
                    View.DisplayError(EvaluationEditorErrors.RemovingCriterion);
                }
                catch (Exception ex)
                {
                    View.DisplayError(EvaluationEditorErrors.RemovingCriterion);
                }
            }
        #endregion

        #region Options management
            public void RemoveOption(List<dtoCommittee> committees, Boolean useDssMethod, long idOption)
            {
                long pIdOption = 0;
                long pIdCommittee = 0;
                long pIdCriterion = 0;
                long idCall = View.IdCall;
                Int32 idCommunity = View.IdCommunity;

                try
                {
                    Service.SaveCommittees(idCall, committees, (useDssMethod ? View.GetCallDssSettings() : null));
                    if (Service.VirtualDeleteCriterionOption(idOption, true, ref pIdCommittee, ref pIdCriterion, ref pIdOption))
                    {
                        View.SendUserAction(idCommunity, View.IdCallModule, idCall, ModuleCallForPaper.ActionType.VirtualDeleteCriterionOption);
                        if (pIdOption != 0)
                            View.ReloadEditor(RootObject.OptionRemovedFromCriterion(pIdOption, idCall, idCommunity, View.PreloadView));
                        else if (pIdCriterion != 0)
                            View.ReloadEditor(RootObject.CriterionRemovedFromCommittee(pIdCriterion, idCall, idCommunity, View.PreloadView));
                        else
                            View.ReloadEditor(RootObject.CommitteeRemovedFromCall(pIdCommittee, idCall, idCommunity, View.PreloadView));
                    }
                    else
                        View.DisplayError(EvaluationEditorErrors.RemovingOption);
                }
                catch (EvaluationStarted exSubmission)
                {
                    View.DisplayError(EvaluationEditorErrors.RemovingOption);
                }
                catch (Exception ex)
                {
                    View.DisplayError(EvaluationEditorErrors.RemovingOption);
                }
            }
            public void AddOption(List<dtoCommittee> committees, Boolean useDssMethod, long idCriterion, String name, Decimal value)
            {
                long idCall = View.IdCall;
                Int32 idCommunity = View.IdCommunity;
                CriterionOption option = Service.AddOptionToCriterion(idCriterion, committees,(useDssMethod ? View.GetCallDssSettings() : null), name, value);
                if (option == null)
                    View.DisplayError(EvaluationEditorErrors.AddingOption);
                else
                {
                    View.SendUserAction(idCommunity, View.IdCallModule, idCall, ModuleCallForPaper.ActionType.AddCriterionOption);
                    View.ReloadEditor(RootObject.OptionAddedToCriterion(option.Id,idCall, idCommunity, View.PreloadView));
                }
            }
        #endregion
    }
}