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
    public class ManageEvaluationsPresenter : lm.Comol.Core.DomainModel.Common.DomainPresenter
    {
        #region "Initialize"

            public virtual BaseModuleManager CurrentManager { get; set; }
            protected virtual IViewManageEvaluations View
            {
                get { return (IViewManageEvaluations)base.View; }
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
            public ManageEvaluationsPresenter(iApplicationContext oContext)
                : base(oContext)
            {
                this.CurrentManager = new BaseModuleManager(oContext);
            }
            public ManageEvaluationsPresenter(iApplicationContext oContext, IViewManageEvaluations view)
                : base(oContext, view)
            {
                this.CurrentManager = new BaseModuleManager(oContext);
            }
        #endregion

        public void InitView(Boolean allowUseOfDss)
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
                else if (call.Type != CallForPaperType.CallForBids)
                    View.RedirectToUrl(RootObject.ViewCalls(idCall, type, CallStandardAction.List, idCommunity, View.PreloadView));
                else if (allowManage || allowSave)
                {
                    View.AllowSave = true;
                    dtoEvaluationSettings settings = CallService.GetEvaluationSettings(idCall, allowUseOfDss);
                   
                    
                    View.EndEvaluationOn = (settings == null) ? null : settings.EndEvaluationOn;
                    View.IdCall = idCall;
                    View.SetActionUrl(RootObject.ViewCalls(type, CallStandardAction.Manage, idCommunity, View.PreloadView));

                    if (Service.CallHasEvaluation(idCall))
                    {
                        InitializeBy(idCall,idCommunity);
                        View.SendUserAction(idCommunity, idModule, idCall, ModuleCallForPaper.ActionType.ManageEvaluators);
                    }
                    else
                        View.DisplayNoEvaluations(false, false, false);
                    View.SetActionUrl(RootObject.ViewCalls(idCall, type, CallStandardAction.Manage, idCommunity, View.PreloadView));
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

        private List<lm.Comol.Core.Wizard.NavigableWizardItem<dtoEvaluationStep>> LoadWizardStep(long idCall, Int32 idCommunity)
        {
            List<lm.Comol.Core.Wizard.NavigableWizardItem<dtoEvaluationStep>> steps = Service.GetAvailableSteps(idCall, WizardEvaluationStep.ManageEvaluations);
            View.LoadWizardSteps(idCall, idCommunity, steps);
            return steps;
        }
        private List<ManageEvaluationsAction> BaseInitialize(long idCall, Int32 idCommunity)
        {
            List<lm.Comol.Core.Wizard.NavigableWizardItem<dtoEvaluationStep>> steps = LoadWizardStep(idCall,idCommunity);
            List<ManageEvaluationsAction> items = null;
            if (steps.Where(s => s.Id.Type == WizardEvaluationStep.ManageEvaluations).Any())
                items = steps.Where(s => s.Id.Type == WizardEvaluationStep.ManageEvaluations).Select(s => (dtoEvaluationsManageStep)s.Id).FirstOrDefault().GetAvailableActions();
            return items;
        }
        private void InitializeBy(long idCall, Int32 idCommunity )
        {
            List<ManageEvaluationsAction> items = BaseInitialize(idCall,idCommunity);
            if (items.Any() && items.Count>0){
                View.DisplayByEvaluator = true;
                ManageEvaluationsAction current = (items.Contains(ManageEvaluationsAction.OpenAll)) ? ManageEvaluationsAction.OpenAll : (items.Contains(ManageEvaluationsAction.CloseAll)) ? ManageEvaluationsAction.CloseAll : ManageEvaluationsAction.None;
                View.LoadAvailableActions(items);
                View.CurrentAction = current;
                LoadItemsByEvaluators(idCall,current);
            }
            else
                View.DisplayNoEvaluations(false,false,false);
        }

        #region "ByEvaluator"
            private void InitializeByEvaluator(long idCall, Int32 idCommunity, ManageEvaluationsAction current)
            {
                List<ManageEvaluationsAction> items = BaseInitialize(idCall, idCommunity);
                View.LoadAvailableActions(items);
                View.CurrentAction = current;
                LoadItemsByEvaluators(idCall, current);
            }
            private void LoadItemsByEvaluators(long idCall, ManageEvaluationsAction action)
            {
                View.DisplayByEvaluator = true;
                List<dtoBasicCommitteeItem> items = Service.GetItemsByEvaluatorsForEvaluationsManagement(idCall, action);
                if (items != null && items.Any() && items.Count > 0)
                    View.LoadItems(items);
                else
                {
                    List<ManageEvaluationsAction> availableActions = Service.GetAvailableActionsForManageEvaluations(idCall);
                    View.DisplayNoEvaluations(false, false, (availableActions.Count > 0));
                    View.LoadAvailableActions(availableActions);
                }
            }
            public void SaveSettings(ManageEvaluationsAction action, Dictionary<long, List<long>> items)
            {
                if (UserContext.isAnonymous)
                    View.DisplaySessionTimeout();
                else
                {
                    long idCall = View.IdCall;
                    if (Service.BulkEditEvaluationsStatus(idCall, action, items))
                    {
                        View.DisplaySettingsSaved();
                        LoadWizardStep(idCall, View.IdCommunity);
                        LoadItemsByEvaluators(idCall, View.CurrentAction);
                    }
                    else
                        View.DisplayStatusEditingError((action == ManageEvaluationsAction.CloseAll));
                }
            }
        #endregion

        public void LoadItems(ManageEvaluationsAction action, Boolean byEvaluators, Boolean initialize)
        {
            if (UserContext.isAnonymous)
                View.DisplaySessionTimeout();
            else
            {
                ManageEvaluationsAction current = View.CurrentAction;
                if (!initialize && byEvaluators && byEvaluators == View.DisplayByEvaluator && current == action)
                    LoadItemsByEvaluators(View.IdCall, action);
                else if (byEvaluators)
                    InitializeByEvaluator(View.IdCall, View.IdCommunity, action);
                else if (!initialize && !byEvaluators && byEvaluators == View.DisplayByEvaluator && current == action)
                    LoadItemsBySubmissions(View.IdCall, action, View.FilterByName,View.FilterByType, View.Pager.PageIndex, View.CurrentPageSize);
                else
                    InitializeBySubmissions(View.IdCall, View.IdCommunity, action, View.SelectedSubmissionName, View.SelectedIdSubmitterType);
            }
        }

        public void SaveSettings(DateTime? endEvaluationsOn)
        {
            if (UserContext.isAnonymous)
                View.DisplaySessionTimeout();
            else
            {
                long idCall = View.IdCall;
                if (Service.SaveEvaluationsEndDate(idCall, endEvaluationsOn))
                    View.DisplayEndEvaluationDateSaved();
                else
                    View.DisplayDateChangingError();
            }
        }
        

        #region "BySubmissions"
            public void LoadItemsBySubmissions(ManageEvaluationsAction action, Boolean initialize, String submitterName, long idSubmitterType, int pageIndex, int pageSize)
            {
                if (initialize)
                    InitializeBySubmissions(View.IdCall, View.IdCommunity, action, submitterName, idSubmitterType);
                else
                    LoadItemsBySubmissions(View.IdCall, action, submitterName, idSubmitterType, pageIndex, pageSize);
            }
            private void InitializeBySubmissions(long idCall, Int32 idCommunity, ManageEvaluationsAction current, String submitterName, long idSubmitterType)
            {
                List<ManageEvaluationsAction> items = BaseInitialize(idCall, idCommunity);
                View.LoadAvailableActions(items);
                View.CurrentAction = current;
                View.FilterByName = submitterName;
                View.FilterByType = idSubmitterType;
                List<dtoSubmitterType> types = Service.GetSubmitterTypes(idCall, current);
                View.LoadSubmitterstype(types, (types.Any() && types.Count == 1) ? types.FirstOrDefault().Id : -1);
                View.CurrentPageSize = Service.CalculateBySubmissionsPageSize(idCall, submitterName, idSubmitterType, current, View.AnonymousTranslation);
                View.SelectedEvaluations = new List<long>();
                View.SelectAllItems = false;
                LoadItemsBySubmissions(idCall, current,submitterName, idSubmitterType, 0, View.CurrentPageSize);
            }
            private void LoadItemsBySubmissions(long idCall, ManageEvaluationsAction action,String submitterName, long idSubmitterType, int pageIndex, int pageSize)
            {
                View.DisplayByEvaluator = false;

                PagerBase pager = new PagerBase();
                List<dtoBasicSubmissionItem> items = Service.GetItemsBySubmissionsForEvaluationsManagement(idCall, submitterName, idSubmitterType, action, View.AnonymousTranslation);
                InitializeSelectedItems(items);

                pager.PageSize = pageSize;//Me.View.CurrentPageSize
                pager.Count = items.Count - 1;
                pager.PageIndex = pageIndex;// Me.View.CurrentPageIndex
                View.Pager = pager;

                if (items != null && items.Any() && items.Count > 0)
                    View.LoadItems(items.Skip(pager.PageIndex * pageSize).Take(pageSize).ToList());
                else
                {
                    List<ManageEvaluationsAction> availableActions = Service.GetAvailableActionsForManageEvaluations(idCall);
                    View.DisplayNoEvaluations(true, false, (availableActions.Count > 0));
                    View.LoadAvailableActions(availableActions);
                }
            }
            private void InitializeSelectedItems(List<dtoBasicSubmissionItem> itemsToLoad)
            {
                List<long> sEvaluations = UpdateItemsSelection();
                if (itemsToLoad.Any() && View.SelectAllItems)
                {
                    sEvaluations.AddRange(itemsToLoad.SelectMany(i => i.GetIdEvaluations()));
                }
                View.SelectedEvaluations = sEvaluations.Distinct().ToList();
            }

            private List<long> UpdateItemsSelection() {
                // SELECTED ITEMS
                List<long> sEvaluations = View.SelectedEvaluations;
                List<dtoSelectEvaluationItem> cSelectedItems = View.GetCurrentSumbissionsItems();

                // REMOVE ITEMS
                sEvaluations = sEvaluations.Where(i => !cSelectedItems.Where(si => !si.Selected && si.IdEvaluation == i).Any()).ToList();
                // ADD ITEMS
                sEvaluations.AddRange(cSelectedItems.Where(si => si.Selected && !sEvaluations.Contains(si.IdEvaluation)).Select(si => si.IdEvaluation).Distinct().ToList());
                return sEvaluations;
            }
            public void EditItemsSelection(Boolean selectAll)
            {
                View.SelectAllItems = selectAll;
                View.SelectedEvaluations = (selectAll) ? UpdateItemsSelection().Distinct().ToList() : new List<long>();
            }
            public void SaveSettings(ManageEvaluationsAction action, Boolean selectAll)
            {
                if (UserContext.isAnonymous)
                    View.DisplaySessionTimeout();
                else
                {
                    long idCall = View.IdCall;
                    List<long> idEvaluations = UpdateItemsSelection();
                    if (Service.BulkEditEvaluationsStatus(idCall, action, idEvaluations, selectAll))
                    {
                        View.DisplaySettingsSaved();
                        LoadWizardStep(idCall, View.IdCommunity);
                        LoadItemsBySubmissions(idCall, View.CurrentAction,View.FilterByName,View.FilterByType,0, View.CurrentPageSize);
                    }
                    else
                        View.DisplayStatusEditingError((action == ManageEvaluationsAction.CloseAll));
                }
            }
        #endregion
    }
}