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
using lm.Comol.Modules.CallForPapers.Presentation.Evaluation;

namespace lm.Comol.Modules.CallForPapers.Advanced.Presentation
{
    /// <summary>
    /// Presenter modifca commissione
    /// </summary>
    public class AdvComEditPresenter : lm.Comol.Core.DomainModel.Common.DomainPresenter
    {
        #region "Initialize"
        /// <summary>
        /// Manager
        /// </summary>
        public virtual BaseModuleManager CurrentManager { get; set; }
        /// <summary>
        /// View
        /// </summary>
        protected virtual Advanced.Presentation.iView.iViewAdvCommissionEdit View
        {
            get { return (Advanced.Presentation.iView.iViewAdvCommissionEdit)base.View; }
        }
        /// <summary>
        /// Service
        /// </summary>
        private ServiceEvaluation _Service;
        /// <summary>
        /// Service Evalutation
        /// </summary>
        private ServiceEvaluation Service
        {
            get
            {
                if (_Service == null)
                    _Service = new ServiceEvaluation(AppContext);
                return _Service;
            }
        }
        /// <summary>
        /// Service Call For Peaper
        /// </summary>
        private ServiceCallOfPapers _ServiceCall;

        /// <summary>
        /// Service Call For Peaper
        /// </summary>
        private ServiceCallOfPapers CallService
        {
            get
            {
                if (_ServiceCall == null)
                    _ServiceCall = new ServiceCallOfPapers(AppContext);
                return _ServiceCall;
            }
        }
        /// <summary>
        /// Service Richieste di adesione
        /// </summary>
        private ServiceRequestForMembership _ServiceRequest;
        /// <summary>
        /// Service Richieste di adesione
        /// </summary>
        private ServiceRequestForMembership RequestService
        {
            get
            {
                if (_ServiceRequest == null)
                    _ServiceRequest = new ServiceRequestForMembership(AppContext);
                return _ServiceRequest;
            }
        }
        /// <summary>
        /// Costruttore
        /// </summary>
        /// <param name="oContext">Application Context</param>
        public AdvComEditPresenter(iApplicationContext oContext)
            : base(oContext)
        {
            this.CurrentManager = new BaseModuleManager(oContext);
        }
        /// <summary>
        /// Costruttore
        /// </summary>
        /// <param name="oContext">Application Context</param>
        /// <param name="view">View: pagina</param>
        public AdvComEditPresenter(iApplicationContext oContext, Advanced.Presentation.iView.iViewAdvCommissionEdit view)
            : base(oContext, view)
        {
            this.CurrentManager = new BaseModuleManager(oContext);
        }
        #endregion


        #region Permission Helper
        /// <summary>
        /// Controlla se l'utente corrente è manager (permessi comunità)
        /// </summary>
        /// <returns>True se è amministratore dei bandi</returns>
        private bool isManager()
        {
            ModuleCallForPaper module = null;
            module = CallService.CallForPaperServicePermission(UserContext.CurrentUserID, UserContext.CurrentCommunityID);
            bool allowManage = module.CreateCallForPaper || module.Administration || module.EditCallForPaper;

            return allowManage;
        }

        #endregion

        /// <summary>
        /// Inizializzazione pagina
        /// </summary>
        public void InitView()
        {
            if (UserContext.isAnonymous)
            {
                View.DisplaySessionTimeout();
                return;
            }


            long idCall = View.IdCall;
            long idComm = View.IdComm;
            
            if (idCall > 0 && idComm > 0)
            {
                dto.dtoCommissionEdit dto = CallService.CommissionGet(idComm);

                if(dto == null)
                {
                    View.DisplayNoPermission(UserContext.CurrentCommunityID, CallService.ServiceModuleID());
                    SendAction(ModuleCallForPaper.ActionType.NoPermission, ModuleCallForPaper.ObjectType.AdvCommission, idComm.ToString());
                    return;
                }
                
                

                bool _isManager = isManager();

                bool canAccess = _isManager || ((int)dto.Permission > (int)CommissionPermission.None);

                dto.canEdit = (_isManager || dto.HasCommissionPermission(CommissionPermission.Edit));

                if(!(dto.Status == CommissionStatus.Draft || dto.Status == CommissionStatus.ViewSubmission))
                {
                    dto.canEdit = false;
                }   

                if (dto != null && dto.Id > 0 && canAccess)
                {
                    View.Init(dto, isManager());
                    if(isManager())
                    {
                        SendAction(ModuleCallForPaper.ActionType.AdvCommissionModify, ModuleCallForPaper.ObjectType.AdvCommission, idComm.ToString());
                    } else
                    {
                        SendAction(ModuleCallForPaper.ActionType.AdvCommissionView, ModuleCallForPaper.ObjectType.AdvCommission, idComm.ToString());
                    }                    
                }                    
                else
                {
                    View.DisplayNoPermission(UserContext.CurrentCommunityID, CallService.ServiceModuleID());
                    SendAction(ModuleCallForPaper.ActionType.NoPermission, ModuleCallForPaper.ObjectType.AdvCommission, idComm.ToString());
                    
                }
                    
            }
            
        }

        #region Action management
        /// <summary>
        /// Salva le modifiche alla commissione
        /// </summary>
        /// <param name="Name">Nome commissione</param>
        /// <param name="Description">Descrizione Commissione</param>
        /// <param name="Tags">Stringa con i tag, separati da ,</param>
        /// <param name="IsMaster">Indica se la commissione è la commissione principale</param>
        /// <param name="EvType">Tipo di aggregazione delle valutazioni dei commissari</param>
        /// <param name="EvMinVal">Valore minimo della valutazione per il superamento della commissione</param>
        /// <param name="EvLockBool">Indica se considerare i criteri booleani ai fini del superamento</param>
        /// <param name="UpdateView">Se TRUE la pagina sarà aggiornata in automatico. A FALSE se sono necessarie altre operazioni prima del refresh.</param>
        /// <param name="StepEvType">Tipo di aggregazione tra le commissioni</param>
        /// <param name="MaxValue"></param>
        /// <param name="TemplateId">Id Template per esportazione documento commissione</param>
        /// <param name="TemplateVersionId">Id Versione Template (se -1, ultima versione)</param>
        /// <param name="criterions">Elenco criteri di valutazione</param>
        public void SaveCommitee(
            string Name, 
            string Description, 
            string Tags,
            bool IsMaster, 
            EvalType EvType,
            int EvMinVal,
            bool EvLockBool,
            bool UpdateView,
            EvalType StepEvType,
            Double MaxValue,
            Int64 TemplateId,
            Int64 TemplateVersionId,
            List<dtoCriterion> criterions = null)
        {
            bool success = CallService.CommissionUpdate(
                View.IdComm, 
                Name, 
                Description,
                Tags,
                IsMaster,
                EvType,
                EvMinVal,
                EvLockBool,
                StepEvType,
                MaxValue, 
                TemplateId,
                TemplateVersionId,
                criterions);

            if (success && UpdateView)
                InitView();
        }

        /// <summary>
        /// Aggiunge un membro alla lista
        /// </summary>
        /// <param name="UsersId">Id utente da aggiungere</param>
        public void MembersAdd(IList<int> UsersId)
        {
            bool success = CallService.CommissionMembersAdd(View.IdComm, UsersId);
            
            if (success)
            {
                InitView();
                SendAction(ModuleCallForPaper.ActionType.AdvMemberAdd, ModuleCallForPaper.ObjectType.AdvCommission, View.IdComm.ToString());
            }
                
        }

        /// <summary>
        /// Cancella un membro
        /// </summary>
        /// <param name="Id">Id Persona dell'utente da cancellare</param>
        public void MemberDel(int Id)
        {
            bool success = CallService.CommissionMemberDelPers(View.IdComm, Id);

            if (success)
            {
                InitView();
                SendAction(ModuleCallForPaper.ActionType.AdvMemberDelete, ModuleCallForPaper.ObjectType.AdvCommission, View.IdComm.ToString());
            }
                
        }

        /// <summary>
        /// Modifica il presidente della commissione
        /// </summary>
        /// <param name="UserId">Id Persona nuovo presidente</param>
        public void PresidentUpdate(int UserId)
        {
            bool success = CallService.CommissionPresidentUpdate(View.IdComm, UserId);

            if (success)
            {
                InitView();
                SendAction(ModuleCallForPaper.ActionType.AdvPresidentModifiy, ModuleCallForPaper.ObjectType.AdvCommission, View.IdComm.ToString());
            }
                
        }
        /// <summary>
        /// Modifica il segretario della commissione
        /// </summary>
        /// <param name="UserId">Id Persona nuovo segretario</param>
        public void SecretaryUpdate(int UserId)
        {
            bool success = CallService.CommissionSecretaryUpdate(View.IdComm, UserId);

            if (success)
            {
                InitView();
                SendAction(ModuleCallForPaper.ActionType.AdvSegretaryModifiy, ModuleCallForPaper.ObjectType.AdvCommission, View.IdComm.ToString());
            }
                
        }
        /// <summary>
        /// Rimuove un criterio
        /// </summary>
        /// <param name="CommId">Id Commissione</param>
        /// <param name="CritId">Id criterio da eliminare</param>
        public void RemoveCriteria(Int64 CommId, Int64 CritId)
        {
            bool success = CallService.CriteriaRemove(CommId, CritId);

            if (success)
            {
                SendAction(ModuleCallForPaper.ActionType.DeleteCriterion, ModuleCallForPaper.ObjectType.Criterion, CritId.ToString());
                InitView();
            }
        }

        /// <summary>
        /// Modifica lo stato di una commissione
        /// </summary>
        /// <param name="commId">Id Commissione</param>
        /// <param name="status">Nuovo stato</param>
        public void changeCommissionStatus(Int64 commId, CommissionStatus status)
        {
            CommissionStatusFeedback feed = CallService.ChangeCommissionStatus(commId, status);

            //Me.CurrentPresenter.SubmissionAssignAll(Me.IdCall, Me.IdComm)

            int newassigned = 0;



            if (feed == CommissionStatusFeedback.Success)
            {
                if (status == CommissionStatus.Started)
                    newassigned = CallService.SubmissionAssignAll(View.IdCall, commId);

                switch(status)
                {
                    case CommissionStatus.Started:
                        SendAction(ModuleCallForPaper.ActionType.AdvCommissionStart, ModuleCallForPaper.ObjectType.AdvCommission, commId.ToString());
                        break;

                    case CommissionStatus.Locked:
                        SendAction(ModuleCallForPaper.ActionType.AdvCommissionStop, ModuleCallForPaper.ObjectType.AdvCommission, commId.ToString());
                        break;

                    case CommissionStatus.ValutationEnded:
                        SendAction(ModuleCallForPaper.ActionType.AdvCommissionClose, ModuleCallForPaper.ObjectType.AdvCommission, commId.ToString());
                        break;
                }
                InitView();
            } else
            {
                //ToDo: showMessage!
            }
        }

        /// <summary>
        /// Assegna tutte le sottomissioni a tutti i membri correnti.
        /// </summary>
        /// <param name="callId">Id Bando</param>
        /// <param name="commId">Id commissione</param>
        public void SubmissionAssignAll(long callId, long commId)
        {
            int newassigned = CallService.SubmissionAssignAll(callId, commId);

            InitView();
            
        }

        /// <summary>
        /// Carica il verbale della commissione
        /// </summary>
        public void UploadVerbale()
        {
            long IdCommission = View.IdComm;

            if (IdCommission <= 0)
                return;

            Domain.AdvCommission advCommission = CallService.AdvCommissionGet(IdCommission);
            if (advCommission == null)
                return;

            if (advCommission.President.Id != UserContext.CurrentUserID
                || !(advCommission.Members != null && advCommission.Members.Any(m => m.IsPresident && m.Member.Id == UserContext.CurrentUserID)))
                return;

            ModuleActionLink aLink = View.AddInternalFile(
                advCommission,
                ModuleCallForPaper.UniqueCode,
                CallService.ServiceModuleID(),
                (int)ModuleCallForPaper.ActionType.DownloadSubmittedFile,
                (int)ModuleCallForPaper.ObjectType.VerbaliCommissione
                );

            if (aLink == null || aLink.Link == null)
            {
                return;
            }
            else
            {
                CallService.AdvCommissionAddVerbale(IdCommission, aLink);
            }

            InitView();

            SendAction(ModuleCallForPaper.ActionType.AdvCommissionUploadVerbal, ModuleCallForPaper.ObjectType.AdvCommission, advCommission.Id.ToString());
        }

        /// <summary>
        /// Recupare una stringa HTML con i dati relativi alla commissione p erl'esportazione/stampa
        /// </summary>
        /// <param name="commId"></param>
        /// <returns></returns>
        public string HTMLEsportSummary(long commId)
        {
            return CallService.ExportCommission(commId);
        }

        /// <summary>
        /// Modifica un membro della commissione
        /// </summary>
        /// <param name="OldPersonId">Id persona del commissario da modificare</param>
        /// <param name="NewPersonId">Id persona del sostituto</param>
        /// <param name="CommissionId">Id commissione</param>
        public void ChangeMember(int OldPersonId, int NewPersonId, long CommissionId)
        {
            long memberId = CallService.CommissionChangeMember(OldPersonId, NewPersonId, CommissionId);

            if (memberId > 0)
            {
                InitView();
                SendAction(ModuleCallForPaper.ActionType.AdvMemberModifiy, ModuleCallForPaper.ObjectType.AdvMember, memberId.ToString());
            }
        }

        /// <summary>
        /// Aggiunge un opzione ad un criterio
        /// </summary>
        /// <param name="AdvCommissionId">Id commissione</param>
        /// <param name="idCriterion">Id criterio</param>
        /// <param name="name">Nome opzione</param>
        /// <param name="value">Valore opzione</param>
        public void AddOption(long AdvCommissionId, long idCriterion, String name, Decimal value)
        {
            long idCall = View.IdCall;
            //Int32 idCommunity = View.IdCommunity;
            CriterionOption option = CallService.AddOptionToCriterion(
                idCriterion,
                AdvCommissionId, 
                name, 
                value);

            if (option == null)
            {
                //View.DisplayError(EvaluationEditorErrors.AddingOption);
            }
            else
            {
                string Id = (option.Criterion == null) ? "0" : option.Criterion.Id.ToString();
                SendAction(ModuleCallForPaper.ActionType.AddCriterionOption, ModuleCallForPaper.ObjectType.Criterion, Id);
                //View.SendUserAction(idCommunity, View.IdCallModule, idCall, ModuleCallForPaper.ActionType.AddCriterionOption);
                //View.ReloadEditor(RootObject.OptionAddedToCriterion(option.Id, idCall, idCommunity, View.PreloadView));
                InitView();
            }
        }

        /// <summary>
        /// Rimuove un opzione da un criterio
        /// </summary>
        /// <param name="CommissionId">Id commissione</param>
        /// <param name="idOption">Id opzione</param>
        public void RemoveOption(long CommissionId, long idOption)
        {
            long pIdOption = 0;
            long pIdCommittee = 0;
            long pIdCriterion = 0;
            long idCall = View.IdCall;

            try
            {
                if (Service.VirtualDeleteCriterionOption(idOption, true, ref pIdCommittee, ref pIdCriterion, ref pIdOption))
                {
                    InitView();
                    
                    SendAction(ModuleCallForPaper.ActionType.PhisicalDeleteCriterionOption, ModuleCallForPaper.ObjectType.Criterion, pIdOption.ToString());
                    //View.SendUserAction(idCommunity, View.IdCallModule, idCall, ModuleCallForPaper.ActionType.VirtualDeleteCriterionOption);
                    //if (pIdOption != 0)
                    //    View.ReloadEditor(RootObject.OptionRemovedFromCriterion(pIdOption, idCall, idCommunity, View.PreloadView));
                    //else if (pIdCriterion != 0)
                    //    View.ReloadEditor(RootObject.CriterionRemovedFromCommittee(pIdCriterion, idCall, idCommunity, View.PreloadView));
                    //else
                    //    View.ReloadEditor(RootObject.CommitteeRemovedFromCall(pIdCommittee, idCall, idCommunity, View.PreloadView));
                }
                    //else
                    //View.DisplayError(EvaluationEditorErrors.RemovingOption);
            }
            catch (EvaluationStarted exSubmission)
            {
                //View.DisplayError(EvaluationEditorErrors.RemovingOption);
            }
            catch (Exception ex)
            {
                //View.DisplayError(EvaluationEditorErrors.RemovingOption);
            }
        }

        #endregion 


        //private int SetCallCurrentCommunity(dtoBaseForPaper call)
        //{
        //    dtoCallCommunityContext context = CallService.GetCallCommunityContext(call, View.Portalname);
        //    if (String.IsNullOrEmpty(context.CommunityName))
        //        View.SetContainerName(context.CallName);
        //    else
        //        View.SetContainerName(context.CommunityName, context.CallName);

        //    View.IdCommunity = context.IdCommunity;
        //    return context.IdCommunity;
        //}
        //private void LoadCommittees(long idCall)
        //{
        //    BaseForPaper call = CallService.GetCall(idCall);
        //    if (call != null)
        //        LoadCommittees(call);
        //}
        //private void LoadCommittees(Int64 CommissionId)
        //{

        //    Domain.AdvCommission = CallService.CommissionAdd

        //    //List<dtoCommittee> committees = Service.GetEditorCommittees(call);



        //    List<dtoSubmitterType> submitters = CallService.GetCallAvailableSubmittersType(call);
        //    CallForPaper cfp = Service.GetCallForCommiteeSettings(call.Id);

        //    //View.AllowSubmittersSelection = !useDssMethod && (committees.Count > 0) && submitters.Count > 0;

        //    View.LoadSubmitterTypes(submitters);



        //    //List<lm.Comol.Core.Wizard.NavigableWizardItem<dtoEvaluationStep>> steps = Service.GetAvailableSteps(call, WizardEvaluationStep.none);
        //    //Int32 count = committees.Count;
        //    //View.CommitteesCount = count;
        //    List<dtoSelectMethod> methods = null;
        //    //if (useDssMethod)
        //    //{
        //    //    methods = ServiceDss.MethodsGetAvailable(UserContext.Language.Id);
        //    //    View.CurrentMethods=methods;
        //    //}
        //    if (committees == null || committees.Count == 0)
        //    {
        //        if (!Service.isNewCommittee(call))
        //        {
        //            View.CommitteesCount = 0;
        //            View.DisplayError(EvaluationEditorErrors.NoCommittees);
        //            View.LoadWizardSteps(call.Id, View.IdCommunity, steps, EvaluationEditorErrors.NoCommittees);
        //            View.DisplayDssErrors(new List<dtoCommittee>());
        //        }
        //        else
        //        {
        //            EvaluationCommittee committee = Service.AddFirstCommittee(call, View.DefaultCommitteeName, View.DefaultCommitteeDescription, useDssMethod);
        //            if (committee != null)
        //            {
        //                committees = new List<dtoCommittee>();
        //                committees.Add(new dtoCommittee() { Id = committee.Id, Description = committee.Description, DisplayOrder = committee.DisplayOrder, UseDss = committee.UseDss, WeightSettings = new Core.Dss.Domain.Templates.dtoItemWeightSettings(), MethodSettings = new Core.Dss.Domain.Templates.dtoItemMethodSettings() { InheritsFromFather = false  }, Name = committee.Name, ForAllSubmittersType = true });
        //                count = 1;
        //                View.CommitteesCount = count;
        //                View.LoadCommittees(committees);
        //            }
        //            else
        //                View.CommitteesCount = 0;
        //            if (useDssMethod && committees.Any(c => c.HasDssErrors(count)))
        //            {
        //               View.DisplayDssErrors(committees.Where(c => c.HasDssErrors(1)).ToList());
        //            }
        //            else
        //                View.DisplayDssErrors(new List<dtoCommittee>());
        //            View.LoadWizardSteps(call.Id, View.IdCommunity, steps);
        //        }
        //    }
        //    else
        //    {
        //        if (useDssMethod && committees.Any(c => c.HasDssErrors(count)))
        //        {
        //            View.DisplayDssErrors(committees.Where(c => c.HasDssErrors(count)).ToList());
        //        }
        //        else
        //            View.DisplayDssErrors(new List<dtoCommittee>());
        //        View.LoadWizardSteps(call.Id, View.IdCommunity, steps);
        //        if (useDssMethod)
        //        {
        //            if(committees.Count > 1 )
        //                View.InitializeAggregationMethods(methods, (idMethod == -1 ? cfp.IdDssMethod : idMethod), (idRatingSet == -1 ? cfp.IdDssRatingSet : idRatingSet), GetAvailableWeights(call,call.IsDssMethodFuzzy, call.UseOrderedWeights, committees));
        //            else
        //                View.HideCallAggregationMethods(methods, (idMethod == -1 ? cfp.IdDssMethod : idMethod), (idRatingSet == -1 ? cfp.IdDssRatingSet : idRatingSet), GetAvailableWeights(call, call.IsDssMethodFuzzy, call.UseOrderedWeights, committees));
        //        }


        //        View.LoadCommittees(committees);
        //    }
        //}
        //public List<lm.Comol.Core.Dss.Domain.Templates.dtoItemWeightBase> RequireCommitteesWeight(long idCall, Boolean fuzzyWeights, Boolean orderedWeights, List<dtoCommittee> committees)
        //{
        //    BaseForPaper call = CallService.GetCall(idCall);
        //    if (call != null)
        //    {
        //        return GetAvailableWeights(call,fuzzyWeights,orderedWeights, committees,true);
        //    }
        //    else
        //        return new List<dtoItemWeightBase>();
        //}
        //public List<lm.Comol.Core.Dss.Domain.Templates.dtoItemWeightBase> RequireCommitteeWeight(long idCall, Boolean fuzzyWeights, Boolean orderedWeights, dtoCommittee committee)
        //{
        //    return GetAvailableWeights(fuzzyWeights, orderedWeights,committee);
        //}
        //private List<lm.Comol.Core.Dss.Domain.Templates.dtoItemWeightBase> GetAvailableWeights(BaseForPaper call, Boolean fuzzyWeights, Boolean orderedWeights, List<dtoCommittee> committees, Boolean emptyValues = false )
        //{
        //    Dictionary<long, String> weights = null;
        //    if (!emptyValues &&(orderedWeights == call.UseOrderedWeights || fuzzyWeights == call.IsDssMethodFuzzy))
        //    {
        //        weights = call.GetFuzzyMeItems();
        //        if (!fuzzyWeights)
        //            weights.Values.Where(v => v.Contains(";")).ToList().ForEach(v => v = "");
        //    }
        //    else
        //        weights = new Dictionary<long, string>();

        //    List<lm.Comol.Core.Dss.Domain.Templates.dtoItemWeightBase> items = null;
        //    if (orderedWeights)
        //    {
        //        if (committees.Count() > 1)
        //        {
        //            if (committees.Count() == weights.Count)
        //                items = (from int i in Enumerable.Range(1, committees.Count()) select i).ToList().Select(i => new lm.Comol.Core.Dss.Domain.Templates.dtoItemWeightBase() { IdObject = (long)i, IsFuzzyValue = fuzzyWeights, OrderedItem = true, Name = i.ToString(), Value = (weights.ContainsKey((long)i) ? weights[(long)i] : "") }).ToList();
        //            else
        //            {
        //                Int32 index = 1;
        //                items = (from int i in Enumerable.Range(1, committees.Count()) select i).ToList().Select(i => new lm.Comol.Core.Dss.Domain.Templates.dtoItemWeightBase() { IdObject = (long)i, IsFuzzyValue = fuzzyWeights, OrderedItem = true, Name = i.ToString(), Value = "" }).ToList();
        //                List<String> values = weights.Values.ToList();
        //                if (values.Any())
        //                {
        //                    items[0].Value = values[0];
        //                    switch (values.Count)
        //                    {
        //                        case 0:
        //                        case 1:
        //                            break;
        //                        case 2:
        //                            items.Last().Value = values.Last();
        //                            break;
        //                        default:
        //                            items.Last().Value = values.Last();
        //                            foreach (String v in values.Skip(1).Take(values.Count - 1))
        //                            {
        //                                items[index++].Value = v;
        //                            }
        //                            break;
        //                    }
        //                }
        //            }
        //        }
        //    }
        //    else
        //        items = committees.Select(c => c.ToWeightItem(weights, fuzzyWeights)).ToList();
        //    return items;
        //}
        //private List<lm.Comol.Core.Dss.Domain.Templates.dtoItemWeightBase> GetAvailableWeights(Boolean fuzzyWeights, Boolean orderedWeights, dtoCommittee committee)
        //{
        //    Dictionary<long, String> weights = new Dictionary<long, string>();

        //    List<lm.Comol.Core.Dss.Domain.Templates.dtoItemWeightBase> items = null;
        //    if (orderedWeights)
        //    {
        //        if (committee.Criteria.Count() > 1)
        //        {
        //            if (committee.Criteria.Count() == weights.Count)
        //                items = (from int i in Enumerable.Range(1, committee.Criteria.Count()) select i).ToList().Select(i => new lm.Comol.Core.Dss.Domain.Templates.dtoItemWeightBase() { IdObject = (long)i, IsFuzzyValue = fuzzyWeights, OrderedItem = true, Name = i.ToString(), Value = (weights.ContainsKey((long)i) ? weights[(long)i] : "") }).ToList();
        //            else
        //            {
        //                Int32 index = 1;
        //                items = (from int i in Enumerable.Range(1, committee.Criteria.Count()) select i).ToList().Select(i => new lm.Comol.Core.Dss.Domain.Templates.dtoItemWeightBase() { IdObject = (long)i, IsFuzzyValue = fuzzyWeights, OrderedItem = true, Name = i.ToString(), Value = "" }).ToList();
        //                List<String> values = weights.Values.ToList();
        //                if (values.Any())
        //                {
        //                    items[0].Value = values[0];
        //                    switch (values.Count)
        //                    {
        //                        case 0:
        //                        case 1:
        //                            break;
        //                        case 2:
        //                            items.Last().Value = values.Last();
        //                            break;
        //                        default:
        //                            items.Last().Value = values.Last();
        //                            foreach (String v in values.Skip(1).Take(values.Count - 1))
        //                            {
        //                                items[index++].Value = v;
        //                            }
        //                            break;
        //                    }
        //                }
        //            }
        //        }
        //    }
        //    else
        //        items = committee.Criteria.Select(c => c.ToWeightItem(weights, fuzzyWeights)).ToList();
        //    return items;
        //}

        //public void SaveSettings(List<dtoCommittee> committees, Boolean allowUseOfDssMethods, Boolean useDssMethod)
        //{
        //    long idCall = View.IdCall;
        //    Int32 idCommunity = View.IdCommunity;
        //    if (!Service.SaveCommittees(idCall, committees, (useDssMethod ? View.GetCallDssSettings(): null)))
        //        View.DisplayError(EvaluationEditorErrors.Saving);
        //    else
        //    {
        //        LoadCommittees(View.IdCall, allowUseOfDssMethods);
        //        View.DisplaySettingsSaved();
        //        View.SendUserAction(idCommunity, View.IdCallModule, idCall, ModuleCallForPaper.ActionType.SaveCommitteeSettings);
        //    }
        //}

        //#region Committee management
        //    public void AddCommittee(List<dtoCommittee> committees, Boolean useDssMethod, String name, String description)
        //    {
        //        long idCall = View.IdCall;
        //        Int32 idCommunity = View.IdCommunity;
        //        EvaluationCommittee committee = Service.AddCommitteeToCall(idCall, committees, name, description, useDssMethod, (useDssMethod ? View.GetCallDssSettings() : null));
        //        if (committee == null)
        //            View.DisplayError(EvaluationEditorErrors.AddingCommittee);
        //        else
        //        {
        //            View.SendUserAction(idCommunity, View.IdCallModule, idCall, ModuleCallForPaper.ActionType.AddCommittee);
        //            View.ReloadEditor(RootObject.CommitteeAddedToCall(committee.Id, idCall, idCommunity, View.PreloadView));
        //        }
        //    }
        //    public void RemoveCommitte(List<dtoCommittee> committees, Boolean useDssMethod, long idCommittee)
        //    {
        //        long pIdCommittee = 0;
        //        long idCall = View.IdCall;
        //        Int32 idCommunity = View.IdCommunity;

        //        try
        //        {
        //            lm.Comol.Core.Dss.Domain.Templates.dtoItemMethodSettings method = (useDssMethod ? View.GetCallDssSettings() : null);
        //            Service.SaveCommittees(idCall, committees, method);
        //            if (Service.VirtualDeleteCommittee(idCommittee, true, ref pIdCommittee))
        //            {
        //                if (useDssMethod)
        //                {
        //                    if (!Service.RemoveDssInheritsFromCommittee(idCall, committees.Where(c => c.Id != pIdCommittee), method)) {
        //                        Service.UpdateCallManualSettings(idCall,committees, idCommittee);
        //                    }
        //                }
        //                View.SendUserAction(idCommunity, View.IdCallModule, idCall, ModuleCallForPaper.ActionType.VirtualDeleteCommittee);
        //                View.ReloadEditor(RootObject.CommitteeRemovedFromCall(pIdCommittee, idCall, idCommunity, View.PreloadView));
        //            }
        //            else
        //                View.DisplayError(EvaluationEditorErrors.RemovingCommittee);
        //        }
        //        catch (EvaluationStarted exSubmission)
        //        {
        //            View.DisplayError(EvaluationEditorErrors.RemovingCommittee);
        //        }
        //        catch (Exception ex)
        //        {
        //            View.DisplayError(EvaluationEditorErrors.RemovingCommittee);
        //        }
        //    }
        //#endregion
        //#region Criterion management
        //    public void RemoveCriterion(List<dtoCommittee> committees, Boolean useDssMethod, long idCriterion)
        //    {
        //        long pIdCommittee = 0;
        //        long pIdCriterion = 0;
        //        long idCall = View.IdCall;
        //        Int32 idCommunity = View.IdCommunity;

        //        try
        //        {
        //            lm.Comol.Core.Dss.Domain.Templates.dtoItemMethodSettings settings = (useDssMethod ? View.GetCallDssSettings() : null);
        //            Service.SaveCommittees(idCall, committees, settings);
        //            if (Service.VirtualDeleteCriterion(idCriterion, true, ref pIdCommittee, ref pIdCriterion))
        //            {
        //                if (useDssMethod)
        //                    Service.UpdateCommitteeManualSettings(committees,idCriterion);
        //                View.SendUserAction(idCommunity, View.IdCallModule, idCall, ModuleCallForPaper.ActionType.VirtualDeleteCriterion);
        //                if (pIdCriterion != 0)
        //                    View.ReloadEditor(RootObject.CriterionRemovedFromCommittee(pIdCriterion, idCall, idCommunity, View.PreloadView));
        //                else
        //                    View.ReloadEditor(RootObject.CommitteeRemovedFromCall(pIdCommittee, idCall, idCommunity, View.PreloadView));
        //            }
        //            else
        //                View.DisplayError(EvaluationEditorErrors.RemovingCriterion);
        //        }
        //        catch (EvaluationStarted exSubmission)
        //        {
        //            View.DisplayError(EvaluationEditorErrors.RemovingCriterion);
        //        }
        //        catch (Exception ex)
        //        {
        //            View.DisplayError(EvaluationEditorErrors.RemovingCriterion);
        //        }
        //    }
        //#endregion

        //#region Options management
        //    public void RemoveOption(List<dtoCommittee> committees, Boolean useDssMethod, long idOption)
        //    {
        //        long pIdOption = 0;
        //        long pIdCommittee = 0;
        //        long pIdCriterion = 0;
        //        long idCall = View.IdCall;
        //        Int32 idCommunity = View.IdCommunity;

        //        try
        //        {
        //            Service.SaveCommittees(idCall, committees, (useDssMethod ? View.GetCallDssSettings() : null));
        //            if (Service.VirtualDeleteCriterionOption(idOption, true, ref pIdCommittee, ref pIdCriterion, ref pIdOption))
        //            {
        //                View.SendUserAction(idCommunity, View.IdCallModule, idCall, ModuleCallForPaper.ActionType.VirtualDeleteCriterionOption);
        //                if (pIdOption != 0)
        //                    View.ReloadEditor(RootObject.OptionRemovedFromCriterion(pIdOption, idCall, idCommunity, View.PreloadView));
        //                else if (pIdCriterion != 0)
        //                    View.ReloadEditor(RootObject.CriterionRemovedFromCommittee(pIdCriterion, idCall, idCommunity, View.PreloadView));
        //                else
        //                    View.ReloadEditor(RootObject.CommitteeRemovedFromCall(pIdCommittee, idCall, idCommunity, View.PreloadView));
        //            }
        //            else
        //                View.DisplayError(EvaluationEditorErrors.RemovingOption);
        //        }
        //        catch (EvaluationStarted exSubmission)
        //        {
        //            View.DisplayError(EvaluationEditorErrors.RemovingOption);
        //        }
        //        catch (Exception ex)
        //        {
        //            View.DisplayError(EvaluationEditorErrors.RemovingOption);
        //        }
        //    }
        //    public void AddOption(List<dtoCommittee> committees, Boolean useDssMethod, long idCriterion, String name, Decimal value)
        //    {
        //        long idCall = View.IdCall;
        //        Int32 idCommunity = View.IdCommunity;
        //        CriterionOption option = Service.AddOptionToCriterion(idCriterion, committees,(useDssMethod ? View.GetCallDssSettings() : null), name, value);
        //        if (option == null)
        //            View.DisplayError(EvaluationEditorErrors.AddingOption);
        //        else
        //        {
        //            View.SendUserAction(idCommunity, View.IdCallModule, idCall, ModuleCallForPaper.ActionType.AddCriterionOption);
        //            View.ReloadEditor(RootObject.OptionAddedToCriterion(option.Id,idCall, idCommunity, View.PreloadView));
        //        }
        //    }
        //#endregion

        private void SendAction(
            ModuleCallForPaper.ActionType actionType,
            ModuleCallForPaper.ObjectType objectType,
            String ObjectId
            )
        {
            View.SendUserAction(
                UserContext.CurrentCommunityID,
                 CallService.ServiceModuleID(),
                 actionType,
                 objectType,
                 ObjectId);

        }

        public int CurrentUserId()
        {
            return UserContext.CurrentUserID;
        }
    }
}