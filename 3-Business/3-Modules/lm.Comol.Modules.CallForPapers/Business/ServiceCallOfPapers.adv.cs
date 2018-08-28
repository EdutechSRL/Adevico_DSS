using lm.Comol.Modules.CallForPapers.Advanced;
using lm.Comol.Modules.CallForPapers.Advanced.dto;
using System;
using System.Collections.Generic;
using System.Linq;
using Eval = lm.Comol.Modules.CallForPapers.Domain.Evaluation;

namespace lm.Comol.Modules.CallForPapers.Business
{
    /// <summary>
    /// Service Bandi: estensione per la gestione delle commissioni avanzate
    /// </summary>
    /// <remarks>
    /// Molte funzioni sono le stesse presenti in ServiceCallOfPeaper.cs o service anaologhi,
    /// aggiornate per il funzionamento con i nuovi oggetti relativi alle commissioni avanzate.
    /// </remarks>
    [System.Runtime.InteropServices.Guid("B1232450-8B6C-445F-B31E-FFDE3F90B5D4")]
    public partial class ServiceCallOfPapers : BaseService
    {
        #region Internal

        /// <summary>
        /// Persona corrente
        /// </summary>
        Core.DomainModel.litePerson _curPers = null;

        /// <summary>
        /// Recupera la persona corrente
        /// </summary>
        /// <returns></returns>
        private Core.DomainModel.litePerson GetCurrentPerson()
        {

            if (_curPers == null)
            {
                _curPers = Manager.GetLitePerson(UC.CurrentUserID);
            }
            return _curPers;
        }
        
        /// <summary>
        /// Errori assegnazione automatica sottomissioni
        /// </summary>
        public enum SubmissionAssignAllErrorValue : int
        {
            /// <summary>
            /// Step o commissione non trovata
            /// </summary>
            StepOrCallNotFound = -1,
            /// <summary>
            /// Il bando non è stato chiuso
            /// </summary>
            CallNotClosed = -2,
            /// <summary>
            /// Non ci sono sottomissioni per il bando indicato
            /// </summary>
            NoSubmissionForCall = -3,
            /// <summary>
            /// Errore interno
            /// </summary>
            InternalError = -4,
            /// <summary>
            /// Lo step precedente non è stato chiuso
            /// </summary>
            PreviousStepNotClosed = -5,
            /// <summary>
            /// Lo step precedente non è stato trovato
            /// </summary>
            PreviosStepNotFound = -6
        }
        #endregion

        #region Call
        /// <summary>
        /// Verifica se il bando ha sottomissioni avanzate
        /// </summary>
        /// <param name="callid">Id bando</param>
        /// <returns></returns>
        public bool CallIsAdvanced(long callid)
        {
            bool isAdvance = false;

            try
            {
                isAdvance = (from c in Manager.GetIQ<Domain.CallForPaper>()
                             where c.Id == callid
                             select c.AdvacedEvaluation).Skip(0).Take(1).FirstOrDefault();
            }
            catch { }

            return isAdvance;
        }
        /// <summary>
        /// Aggiorna i tag del bando e dei relativi field,
        /// senza modificare gli altri dati
        /// </summary>
        /// <param name="CallId">Id Bando</param>
        /// <param name="CallTags">Stringa con i tag del bando, separati da ,</param>
        /// <param name="FieldTags"></param>
        /// <returns></returns>
        public bool CallTagUpdate(long CallId, string CallTags, IList<dtoTag> FieldTags)
        {
            //Gestori bando della comunità
            lm.Comol.Modules.CallForPapers.Domain.ModuleCallForPaper _module = CallForPaperServicePermission(UC.CurrentUserID, UC.CurrentCommunityID);
            bool allowManage = _module.CreateCallForPaper || _module.Administration || _module.ManageCallForPapers || _module.EditCallForPaper;
            if (!allowManage)
                return false;

            lm.Comol.Modules.CallForPapers.Domain.BaseForPaper call = Manager.Get<lm.Comol.Modules.CallForPapers.Domain.BaseForPaper>(CallId);

            if (call == null)
                return false;

            call.Tags = CallTags;
            call.UpdateMetaInfo(GetCurrentPerson(), UC.IpAddress, UC.ProxyIpAddress);

            if (call.Sections.Any())
            {
                foreach (Domain.FieldsSection section in call.Sections)
                {
                    foreach (Domain.FieldDefinition field in section.Fields)
                    {
                        dtoTag newTag = FieldTags.FirstOrDefault(ft => ft.FieldId == field.Id);
                        if (newTag != null)
                        {
                            field.Tags = newTag.Tags;
                        }
                    }
                }
            }

            if (!Manager.IsInTransaction())
                Manager.BeginTransaction();

            try
            {
                Manager.SaveOrUpdate<Domain.BaseForPaper>(call);
                Manager.Commit();
            }
            catch (Exception ex)
            {
                return false;
            }


            return true;
        }
        /// <summary>
        /// Verifica se un utente ha fatto sottomissioni (non cancellate ed inviate) per il bando
        /// </summary>
        /// <param name="CallId">Id bando</param>
        /// <param name="UserSubmitterId">Id potenziale sottomittore</param>
        /// <returns></returns>
        public bool CallUserHasSubmission(long CallId, int UserSubmitterId)
        {
            if (CallId <= 0 || UserSubmitterId <= 0)
                return false;

            bool hasSubmission = (from Domain.UserSubmission usrSub in Manager.GetIQ<Domain.UserSubmission>()
                                  where usrSub.Call != null && usrSub.Call.Id == CallId
                                  && usrSub.Id == UserSubmitterId
                                  && usrSub.Deleted == Core.DomainModel.BaseStatusDeleted.None
                                  && (int)usrSub.Status >= (int)Domain.SubmissionStatus.submitted
                                  select usrSub.Id).Skip(0).Take(1).Any();

            return hasSubmission;

        }
        /// <summary>
        /// Verifica se allo step sono associate sottomissioni dell'utente
        /// </summary>
        /// <param name="StepId"></param>
        /// <param name="UserSubmitterId"></param>
        /// <returns></returns>
        public bool CallStepHasSubmission(long StepId, int UserSubmitterId)
        {
            if (StepId <= 0 || UserSubmitterId <= 0)
                return false;

            bool hasSubmissionInStep = (from Advanced.Domain.AdvSubmissionToStep usrSub in Manager.GetIQ<Advanced.Domain.AdvSubmissionToStep>()
                                        where usrSub.Step != null && usrSub.Step.Id == StepId
                                        && usrSub.Submission != null
                                        && ((usrSub.Submission.SubmittedBy != null && usrSub.Submission.SubmittedBy.Id == UserSubmitterId)
                                            || (usrSub.Submission.CreatedBy != null && usrSub.Submission.CreatedBy.Id == UserSubmitterId))
                                        && usrSub.Deleted == Core.DomainModel.BaseStatusDeleted.None
                                        select usrSub.Id
                                        ).Skip(0).Take(1).Any();

            return hasSubmissionInStep;
        }
        #endregion

        #region Steps

        /// <summary>
        /// Recupera il dto per la visualizzazione degli step
        /// </summary>
        /// <param name="CallId">Id bando</param>
        /// <param name="defaultValCommissionName">Localizzazione: nome default commissione valutazione</param>
        /// <param name="defaultEcoCommissionName">Localizzazione: nome default commissione economica</param>
        /// <returns></returns>
        public dtoStepsEdit StepContainerGet(
            Int64 CallId,
            String defaultValCommissionName = "Commissione criteri formali",
            String defaultEcoCommissionName = "Commissione valutazione economica")
        {
            
            if (CallId < 0)
                return dtoStepsEdit.GetEmpty();

            CallForPapers.Domain.CallForPaper call = Manager.Get<Domain.CallForPaper>(CallId);

            if (call == null || !call.AdvacedEvaluation)
                return dtoStepsEdit.GetEmpty();

            //ToDo: permission

            IList<Advanced.Domain.AdvStep> CallSteps = Manager.GetAll<Advanced.Domain.AdvStep>(st => st.Call != null && st.Call.Id == CallId);
            //new List<Advanced.Domain.AdvStep>();

            if (!CallSteps.Any())
                return addValidationIfEmpty(call, defaultValCommissionName, defaultEcoCommissionName);

            dtoStepsEdit step = new dtoStepsEdit(CallSteps, UC.CurrentUserID);

            if(step.ValidationStep != null && step.ValidationStep.StepPermission == GenericStepPermission.none)
            {
                if(StepIsSubmitter(step.ValidationStep.Id, UC.CurrentUserID))
                {
                    step.ValidationStep.StepPermission |= GenericStepPermission.Submitter;
                }
            }

            if (step.EconomicStep != null && step.EconomicStep.StepPermission == GenericStepPermission.none)
            {
                if (StepIsSubmitter(step.EconomicStep.Id, UC.CurrentUserID))
                {
                    step.EconomicStep.StepPermission |= GenericStepPermission.Submitter;
                }
            }

            if (step.CustomSteps != null && step.CustomSteps.Any())
            {
                foreach(dtoAdvStepContainer st in step.CustomSteps)
                {
                    if (st != null && st.StepPermission == GenericStepPermission.none)
                    {
                        if(StepIsSubmitter(st.Id, UC.CurrentUserID))
                        {
                            step.EconomicStep.StepPermission |= GenericStepPermission.Submitter;
                        }
                    }
                }   
            }
            return step;

            //Old: Only Test (Like fake call)
            //dtoStepsEdit stepsEdit = new dtoStepsEdit();

            //dtoAdvStepContainer step0 = new dtoAdvStepContainer();
            //dtoAdvStepContainer step1 = new dtoAdvStepContainer();

            //step0.Id = 1;
            //step0.Name = "Valutazioni tecniche";
            //step0.CallId = CallId;
            //step0.Order = 1;
            //step0.status = StepStatus.Started;
            //step0.Commissions = new List<dtoAdvCommissionContainer>();

            //dtoAdvCommissionContainer s0c0 = new dtoAdvCommissionContainer();
            //dtoAdvCommissionContainer s0c1 = new dtoAdvCommissionContainer();

            //s0c0.Id = 1;
            //s0c0.Description = "Commissione bal bla bla...";
            //s0c0.Name = "Commissione 1 - Step 1";
            //s0c0.Status = CommissionStatus.Closed;
            //s0c1.Id = 2;
            //s0c1.Description = "Commissione bal bla bla...";
            //s0c1.Name = "Commissione 2 - Step 1";
            //s0c1.Status = CommissionStatus.Started;

            //step0.Commissions.Add(s0c0);
            //step0.Commissions.Add(s0c1);

            //step1.Id = 2;
            //step1.Name = "Valutazioni generali";
            //step1.CallId = CallId;
            //step1.Order =2;
            //step1.status = StepStatus.Draft;
            //step1.Commissions = new List<dtoAdvCommissionContainer>();

            //dtoAdvCommissionContainer s1c0 = new dtoAdvCommissionContainer();
            //s1c0.Id = 3;
            //s1c0.Description = "Commissione bal bla bla...";
            //s1c0.Name = "Commissione 1 - Step 1";
            //s1c0.Status = CommissionStatus.Draft;
            //step1.Commissions.Add(s1c0);

            //stepsEdit.CustomSteps = new List<dtoAdvStepContainer>();


            //stepsEdit.ValidationStep = new dtoAdvStepContainer();
            //stepsEdit.ValidationStep.Id = 1;
            //stepsEdit.ValidationStep.CallId = CallId;
            //stepsEdit.ValidationStep.Order = 0;
            //stepsEdit.ValidationStep.status = StepStatus.Closed;
            //stepsEdit.ValidationStep.Commissions = new List<dtoAdvCommissionContainer>();

            //dtoAdvCommissionContainer ValidationCom = new dtoAdvCommissionContainer();
            //ValidationCom.Id = 7;
            //ValidationCom.Description = "Commissione bal bla bla...";
            //ValidationCom.Name = "Validazione";
            //ValidationCom.Status = CommissionStatus.Draft;
            //stepsEdit.ValidationStep.Commissions.Add(s1c0);




            //stepsEdit.EconomicStep = new dtoAdvStepContainer();
            //stepsEdit.EconomicStep.Id = 1;
            //stepsEdit.EconomicStep.CallId = CallId;
            //stepsEdit.EconomicStep.Order = 0;
            //stepsEdit.EconomicStep.status = StepStatus.Closed;
            //stepsEdit.EconomicStep.Commissions = new List<dtoAdvCommissionContainer>();

            //dtoAdvCommissionContainer EconomicStepCom = new dtoAdvCommissionContainer();
            //EconomicStepCom.Id = 7;
            //EconomicStepCom.Description = "Commissione validazione spese ammesse";
            //EconomicStepCom.Name = "Spese ammesse";
            //EconomicStepCom.Status = CommissionStatus.Draft;
            //stepsEdit.EconomicStep.Commissions.Add(EconomicStepCom);

            //if (CallId == 0)
            //{
            //    stepsEdit.CustomSteps.Add(step0);
            //    stepsEdit.CustomSteps.Add(step1);

            //    stepsEdit.CustomSteps = stepsEdit.CustomSteps.OrderBy(st => st.Order).ToList();    

            //} else if (CallId == 1)
            //{
            //    step1.Order = 1;
            //    step1.Id = 1;
            //    stepsEdit.CustomSteps.Add(step1);

            //    step0.Order = 2;
            //    step0.Id = 2;
            //    stepsEdit.CustomSteps.Add(step0);

            //    step0.Order = 3;
            //    step0.Id = 3;
            //    stepsEdit.CustomSteps.Add(step0);

            //}
            //else if (CallId == 2)
            //{
            //    step1.Order = 1;
            //    step1.Id = 1;
            //    stepsEdit.CustomSteps.Add(step1);

            //    step0.Order = 2;
            //    step0.Id = 2;
            //    stepsEdit.CustomSteps.Add(step0);

            //    step0.Order = 3;
            //    step0.Id = 3;
            //    stepsEdit.CustomSteps.Add(step0);

            //    stepsEdit.EconomicStep = null;
            //}
            //else if (CallId == 3)
            //{
            //    stepsEdit.EconomicStep = null;
            //}

            //return stepsEdit;
        }
        
        /// <summary>
        /// Aggiunge uno step
        /// </summary>
        /// <param name="CallId">Id bando</param>
        /// <param name="defaultStepName">Localizzazione: nome step di default</param>
        /// <param name="defaultCommName">Localizzazione: nome commissione di default</param>
        /// <param name="isEconomics">Bool: se è uno step economico</param>
        /// <returns>
        /// -1: NoCall
        /// -2: Internal Error
        /// >0: New Step Id
        /// </returns>
        public Int64 StepAdd(Int64 CallId, string defaultStepName, string defaultCommName, bool isEconomics = false)
        {
            if (CallId < 0)
                return -1;

            CallForPapers.Domain.CallForPaper call = Manager.Get<Domain.CallForPaper>(CallId);

            if (call == null || call.Id <= 0)
                return -1;




            int LastOrder = (from Advanced.Domain.AdvStep stp in Manager.GetIQ<Advanced.Domain.AdvStep>()
                             where stp.Call != null && stp.Call.Id == call.Id && stp.Type == StepType.custom
                             select stp.Order).Max() + 1;

            Advanced.Domain.AdvStep Step = new Advanced.Domain.AdvStep();
            Step.Call = call;

            //Core.DomainModel.litePerson person = Manager.GetLitePerson(UC.CurrentUserID);
            Step.CreateMetaInfo(GetCurrentPerson(), UC.IpAddress, UC.ProxyIpAddress);
            Step.Name = "";
            Step.Order = LastOrder;
            Step.Type = StepType.custom;
            Step.Commissions = new List<Advanced.Domain.AdvCommission>();
            Step.Commissions.Add(getDefaultCommission(call, Step, GetCurrentPerson(), defaultCommName, true));


            if (!Manager.IsInTransaction())
                Manager.BeginTransaction();

            try
            {
                Manager.SaveOrUpdate<Advanced.Domain.AdvStep>(Step);
                Manager.Commit();
            }
            catch (Exception ex)
            {
                Manager.RollBack();
                return -2;
            }

            return Step.Id;
        }

        /// <summary>
        /// Cancella uno step
        /// </summary>
        /// <param name="CallId">Id bando</param>
        /// <param name="StepId">Id step</param>
        /// <returns>True: se cancellato</returns>
        public bool StepDelete(Int64 CallId, Int64 StepId)
        {
            if (CallId <= 0 || StepId <= 0)
                return false;

            CallForPapers.Domain.CallForPaper call = Manager.Get<Domain.CallForPaper>(CallId);
            if (call == null || call.Id <= 0)
                return false;

            Advanced.Domain.AdvStep step = Manager.Get<Advanced.Domain.AdvStep>(StepId);

            if (step == null || step.Id <= 0 || step.Type != StepType.custom || step.Status != StepStatus.Draft)
                return false;

            if (!Manager.IsInTransaction())
                Manager.BeginTransaction();

            try
            {
                Manager.DeletePhysical<Advanced.Domain.AdvStep>(step);
                Manager.Commit();
            }
            catch (Exception ex)
            {
                Manager.RollBack();
                return false;
            }



            return true;
        }

        /// <summary>
        /// Riordina gli step
        /// </summary>
        /// <param name="Orders">KVP con Id step e posizione</param>
        /// <returns></returns>
        public bool StepReorder(IList<KeyValuePair<Int64, int>> Orders, long CallId)
        {

            if (Orders == null || !Orders.Any())
                return false;


            IList<Int64> Ids = (from KeyValuePair<Int64, int> kvp in Orders select kvp.Key).Distinct().ToList();
            IList<Advanced.Domain.AdvStep> steps = Manager.GetAll<Advanced.Domain.AdvStep>(
                st => st.Type == StepType.custom
                && st.Status == StepStatus.Draft
                && Ids.Contains(st.Id)
                && st.Call != null && st.Call.Id == CallId
                );

            //Core.DomainModel.litePerson person = Manager.GetLitePerson(UC.CurrentUserID);

            foreach (Advanced.Domain.AdvStep st in steps)
            {
                st.Order = (from KeyValuePair<Int64, int> kvp in Orders where kvp.Key == st.Id select kvp.Value).FirstOrDefault();
                st.UpdateMetaInfo(GetCurrentPerson(), UC.IpAddress, UC.ProxyIpAddress);
            }

            if (!Manager.IsInTransaction())
                Manager.BeginTransaction();

            try
            {
                Manager.SaveOrUpdateList<Advanced.Domain.AdvStep>(steps);
                Manager.Commit();
            }
            catch (Exception ex)
            {
                Manager.RollBack();
                return false;
            }

            return true;
        }

        /// <summary>
        /// Aggiorna tutte le valutazioni di uno step
        /// </summary>
        /// <param name="StepId">Id Step</param>
        /// <returns></returns>
        public bool StepEvUpdateAll(long StepId)
        {

            bool success = true;

            Advanced.Domain.AdvStep step = Manager.Get<Advanced.Domain.AdvStep>(StepId);
            if (step == null || !step.Commissions.Any())
            {
                return false;
            }

            if (step.Status == StepStatus.Closed)
                return true;

            foreach (Advanced.Domain.AdvCommission comm
                    in step.Commissions
                        .Where(c => c.Status == CommissionStatus.Closed
                            || c.Status == CommissionStatus.Started
                            || c.Status == CommissionStatus.Locked
                            || c.Status == CommissionStatus.ValutationEnded).ToList())
            {
                success = CommissionEvUpdate(comm.Id);
            }


            if (!success)
                return false;


            success = StepEvUpdate(step.Id, false);


            return success;
        }

        /// <summary>
        /// Dato l'Id di una commissione, recupera l'ID di uno step
        /// </summary>
        /// <param name="CommId">Id commissione</param>
        /// <returns>Id step</returns>
        public long StepGetIdFromComm(long CommId)
        {
            long stepId = (from Advanced.Domain.AdvCommission adc in Manager.GetIQ<Advanced.Domain.AdvCommission>()
                           where adc.Id == CommId && adc.Step != null
                           select adc.Step.Id).Skip(0).Take(1).ToList().FirstOrDefault();
            return stepId;
        }

        /// <summary>
        /// Chiude uno step
        /// </summary>
        /// <param name="AdmitSubmissionId">Lista degli Id delle sottomissioni ammesse</param>
        /// <param name="stepId">Id step</param>
        /// <returns></returns>
        public bool CloseStep(IList<long> AdmitSubmissionId, long stepId)
        {
            Advanced.Domain.AdvCommission comm = Manager.GetAll<Advanced.Domain.AdvCommission>(
                c => c.Step != null && c.Step.Id == stepId
                && (c.IsMaster || c.Step.Type == StepType.validation
                )).FirstOrDefault();

            if (comm == null ||
                !((comm.President != null && comm.President.Id != UC.CurrentUserID)
                    || comm.Members.Any(m => m.IsPresident && m.Member != null && m.Member.Id == UC.CurrentUserID)
                    )
                )
            {
                return false;
            }


            IList<Advanced.Domain.AdvSubmissionToStep> subs = Manager.GetAll<Advanced.Domain.AdvSubmissionToStep>(s =>
           s.Commission == null && s.Step != null && s.Step.Id == stepId);

            foreach (Advanced.Domain.AdvSubmissionToStep sub in subs)
            {
                if (AdmitSubmissionId.Contains(sub.Id))
                {
                    sub.Admitted = true;
                }
                else
                {
                    sub.Admitted = false;
                }
                sub.UpdateMetaInfo(GetCurrentPerson(), UC.IpAddress, UC.ProxyIpAddress);
            }

            try
            {
                if (!Manager.IsInTransaction())
                    Manager.BeginTransaction();

                Manager.SaveOrUpdateList<Advanced.Domain.AdvSubmissionToStep>(subs);
                Manager.Commit();

            }
            catch (Exception ex)
            {
                Manager.RollBack();
                return false;
            }


            Advanced.Domain.AdvStep step = Manager.Get<Advanced.Domain.AdvStep>(stepId);
            if (step == null || !step.Commissions.All(c => c.Status == CommissionStatus.Closed))
                return false;

            step.UpdateMetaInfo(GetCurrentPerson(), UC.IpAddress, UC.ProxyIpAddress);
            step.Status = StepStatus.Closed;

            try
            {
                if (!Manager.IsInTransaction())
                    Manager.BeginTransaction();

                Manager.SaveOrUpdate<Advanced.Domain.AdvStep>(step);
                Manager.Commit();

            }
            catch (Exception ex)
            {
                Manager.RollBack();
                return false;
            }

            return true;
        }
        
        /// <summary>
        /// Aggiorna le valutazione di uno step
        /// </summary>
        /// <param name="StepId">Id step</param>
        /// <param name="close">Se è necessario chiudere lo step</param>
        /// <returns>True: success</returns>
        /// <remarks>
        ///     La valutazione di un commissiario è SEMPRE la SOMMA delle sue valutazioni
        ///     La valutazione di una commissione è la somma o la media, secondo le impostazioni di QUELLA commissione
        /// 
        /// </remarks>
        public bool StepEvUpdate(long StepId, bool close)
        {

            Advanced.Domain.AdvStep step = Manager.Get<Advanced.Domain.AdvStep>(StepId);

            bool canclose = true;

            if (step.Commissions == null || step.Commissions.All(c => c.Status == CommissionStatus.Closed))
            {
                canclose = false;
            }

            if (!step.Commissions.Any(c => c.President.Id == UC.CurrentUserID && c.IsMaster))
                canclose = false;

            long mastercommId = step.Commissions.FirstOrDefault(c => c.IsMaster).Id;



            IList<Advanced.Domain.AdvSubmissionToStep> oldSubs = Manager.GetAll<Advanced.Domain.AdvSubmissionToStep>(s =>
                    s.Step != null && s.Step.Id == StepId);

            IList<Advanced.Domain.AdvSubmissionToStep> masterSubs = oldSubs.Where(s =>
                    s.Step != null && s.Step.Id == StepId
                    && s.Commission != null && s.Commission.Id == mastercommId).ToList();

            IList<Advanced.Domain.AdvSubmissionToStep> commSubs = oldSubs.Where(s =>
                    s.Commission != null).ToList();

            IList<Advanced.Domain.AdvSubmissionToStep> stepSubs = oldSubs.Where(s =>
                    s.Commission == null).ToList();

            if (stepSubs == null)
                stepSubs = new List<Advanced.Domain.AdvSubmissionToStep>();

            foreach (Advanced.Domain.AdvSubmissionToStep sub in masterSubs)
            {
                bool toAdd = false;

                Advanced.Domain.AdvSubmissionToStep stepEval = stepSubs.FirstOrDefault(stev =>
                    stev.Submission != null && stev.Submission.Id == sub.Submission.Id);

                if (stepEval == null)
                {
                    toAdd = true;
                    stepEval = new Advanced.Domain.AdvSubmissionToStep();
                    stepEval.CreateMetaInfo(GetCurrentPerson(), UC.IpAddress, UC.ProxyIpAddress);
                    stepEval.Commission = null;
                    stepEval.Step = step;
                    stepEval.Submission = sub.Submission;
                }
                else
                {
                    stepEval.UpdateMetaInfo(GetCurrentPerson(), UC.IpAddress, UC.ProxyIpAddress);
                }

                IList<Advanced.Domain.AdvSubmissionToStep> CommEvals = commSubs.Where(c => c.Submission != null && c.Submission.Id == sub.Submission.Id).ToList();

                // MEDIA: somma su valutatori : NO E' per lo STEP!!!!

                stepEval.AverageRating = Math.Round(CommEvals.Average(ev => (ev.Commission.EvalType == EvalType.Sum) ? ev.SumRating : Math.Round(ev.AverageRating, 2)), 2);

                stepEval.SumRating = CommEvals.Sum(ev => (ev.Commission.EvalType == EvalType.Sum) ? ev.SumRating : Math.Round(ev.AverageRating, 2));

                //stepEval.AverageRating = Math.Round(CommEvals.Average(ev => ev.SumRating), 2);
                //stepEval.SumRating = CommEvals.Sum(ev => ev.SumRating);
                


                stepEval.BoolRating = CommEvals.All(ev => ev.BoolRating);

                stepEval.Passed = CommEvals.All(ev => ev.Passed);
                stepEval.Admitted = stepEval.Passed;

                if (toAdd)
                    stepSubs.Add(stepEval);

            }

            int rank = 1;

            foreach (Advanced.Domain.AdvSubmissionToStep sbst in stepSubs.OrderByDescending(s => s.Passed)
                .ThenByDescending(s => s.AverageRating).ThenBy(s => s.Submission.SubmittedOn))
            {
                sbst.Rank = rank;
                rank++;
            }

            bool updatestep = false;
            if (close && canclose)
            {
                step.UpdateMetaInfo(GetCurrentPerson(), UC.IpAddress, UC.ProxyIpAddress);
                step.Status = (step.Type == StepType.economics) ? StepStatus.Closed : StepStatus.WaitforClosing;
                updatestep = true;
            }

            try
            {

                if (!Manager.IsInTransaction())
                    Manager.BeginTransaction();

                if (updatestep)
                {
                    Manager.SaveOrUpdate<Advanced.Domain.AdvStep>(step);
                }

                Manager.SaveOrUpdateList<Advanced.Domain.AdvSubmissionToStep>(stepSubs);

                Manager.Commit();

            }
            catch (Exception ex)
            {
                Manager.RollBack();
                return false;
            }

            return true;

        }


        /// <summary>
        /// Recupera il riepilogo delle valutazioni di uno Step
        /// </summary>
        /// <param name="StepId">Id step</param>
        /// <returns>Oggetto con il riepilogo</returns>
        public dtoStepSummary StepSummaryGet(long StepId)
        {



            dtoStepSummary summary = new dtoStepSummary();

            Advanced.Domain.AdvStep step = Manager.Get<Advanced.Domain.AdvStep>(StepId);

            if (step == null)
                return summary;

            summary.StepId = step.Id;
            summary.StepName = step.Name;

            summary.CallId = (step.Call != null) ? step.Call.Id : 0;
            summary.CommissionId = 0;

            summary.Status = step.Status;

            switch (step.EvalType)
            {
                case EvalType.Sum:
                    summary.evType = EvalType.Sum;
                    break;
                default:
                    summary.evType = EvalType.Average;
                    break;
            }

            summary.Commissions = new List<dtoCommToreport>();
            summary.items = new List<dtoStepSummarySubmission>();


            int communityId = (step.Call != null && step.Call.Community != null) ? step.Call.Community.Id : 0;

            bool isCommunityManager = false;

            if (communityId > 0)
                isCommunityManager = HasManagePermission(communityId, UC.CurrentUserID, Domain.CallForPaperType.CallForBids);


            IList<Advanced.Domain.AdvSubmissionToStep> items = Manager.GetAll<Advanced.Domain.AdvSubmissionToStep>(
                st => st.Step != null
                && st.Step.Id == StepId);


            if (items == null || !items.Any())
                return summary;



            IList<Domain.UserSubmission> Submissions = (from Advanced.Domain.AdvSubmissionToStep itm in items
                                                        where itm.Commission == null
                                                        orderby itm.Rank
                                                        select itm.Submission).Distinct().ToList();
            //lm.Comol.Modules.CallForPapers.Domain.Evaluation

            bool HasNumericCriteria = step.Commissions.Any(cm =>
                    cm.Criteria.Any(c =>
                        c.Type == Eval.CriterionType.DecimalRange
                        || c.Type == Eval.CriterionType.IntegerRange
                        || c.Type == Eval.CriterionType.StringRange
                        )
                    );

            foreach (Domain.UserSubmission sub in Submissions)
            {

                dtoStepSummarySubmission dtosub = new dtoStepSummarySubmission();
                dtosub.SubmissionId = sub.Id;
                try
                {
                    dtosub.StepEvalId = items.FirstOrDefault(
                        i => i.Commission == null
                        && i.Submission != null
                        && i.Submission.Id == sub.Id).Id;
                }
                catch { }


                //Nota: solo perchè ho sottomissioni "di test/sviluppo" il Submitted mi risulta null...
                dtosub.SubmissionName = (sub.SubmittedBy == null) ? sub.CreatedBy.SurnameAndName : sub.SubmittedBy.SurnameAndName;

                dtosub.Commissions = new List<dtoStepSummaryItem>();

                //dtosub.SubmissionRank = 

                Advanced.Domain.AdvSubmissionToStep stepItem = items
                    .Where(
                        i => i.Submission != null && i.Submission.Id == sub.Id)
                    .FirstOrDefault(i => i.Commission == null);

                dtoStepSummaryItem dtoitmSt = new dtoStepSummaryItem();
                dtoitmSt.AverageRating = stepItem.AverageRating;
                dtoitmSt.SumRating = stepItem.SumRating;
                dtoitmSt.BoolRating = stepItem.BoolRating;
                dtoitmSt.Passed = stepItem.Passed;
                dtoitmSt.Admit = stepItem.Admitted;
                dtoitmSt.ComissionName = "";
                dtoitmSt.ComissionId = 0;
                dtoitmSt.isStep = true;
                dtoitmSt.HasScoreCriteria = HasNumericCriteria;
                dtoitmSt.EvaluationType = summary.evType;

                dtosub.SubmissionRank = stepItem.Rank;
                //dtoitmSt.Rank = stepItem.Rank;

                switch (step.Status)
                {
                    case StepStatus.Draft:
                        dtoitmSt.Status = CommissionStatus.Draft;
                        break;

                    case StepStatus.Locked:
                        dtoitmSt.Status = CommissionStatus.Locked;
                        break;

                    case StepStatus.Open:
                        dtoitmSt.Status = CommissionStatus.Started;
                        break;

                    case StepStatus.Closed:
                        dtoitmSt.Status = CommissionStatus.Closed;
                        break;

                    case StepStatus.Started:
                        dtoitmSt.Status = CommissionStatus.Started;
                        break;

                    case StepStatus.WaitforClosing:
                        dtoitmSt.Status = CommissionStatus.Closed;
                        break;
                }


                Advanced.Domain.AdvCommission mainComm =
                    (step.Type == StepType.validation) ?
                    step.Commissions.FirstOrDefault() :
                    step.Commissions.FirstOrDefault(c => c.IsMaster);

                //if (mainComm != null)
                //    summary.evType = mainComm.EvalType;
                //else
                //    summary.evType = EvalType.Average;


                dtosub.Commissions.Add(dtoitmSt);

                IList<Advanced.Domain.AdvSubmissionToStep> subitems = items
                    .Where(i => i.Submission != null
                        && i.Submission.Id == sub.Id
                        && i.Commission != null)
                    .OrderByDescending(i => i.Commission.IsMaster)
                    .ThenBy(i => i.Commission.Id)
                    .ToList();

                foreach (Advanced.Domain.AdvSubmissionToStep itm in subitems)
                {
                    if (itm.Commission != null)
                    {
                        dtoStepSummaryItem dtoitmCm = new dtoStepSummaryItem();
                        dtoitmCm.AverageRating = itm.AverageRating;
                        dtoitmCm.SumRating = itm.SumRating;
                        dtoitmCm.BoolRating = itm.BoolRating;
                        dtoitmCm.Passed = itm.Passed;
                        dtoitmCm.ComissionName = itm.Commission.Name;
                        dtoitmCm.ComissionId = itm.Commission.Id;
                        dtoitmCm.Status = itm.Commission.Status;
                        dtoitmCm.isStep = false;
                        //dtoitmCm.Rank = itm.Rank;
                        dtoitmCm.minScore = itm.Commission.EvalMinValue;
                        //dtoitmCm.BoolTotal = itm.com
                        dtoitmCm.EvaluationType = itm.Commission.EvalType;

                        dtoitmCm.HasScoreCriteria = (itm.Commission.Criteria != null &&
                            itm.Commission.Criteria.Any(c =>
                                c.Type == Eval.CriterionType.DecimalRange
                                || c.Type == Eval.CriterionType.IntegerRange
                                || c.Type == Eval.CriterionType.StringRange
                            )) ? true : false;

                        dtosub.Commissions.Add(dtoitmCm);

                        string commName = (itm.Commission.IsMaster) ?
                        String.Format("(*) {0}", itm.Commission.Name) :
                        itm.Commission.Name;


                        //if (!summary.Commissions.Contains(commName))
                        //    summary.Commissions.Add(commName);


                        if (!summary.Commissions.Any(c => c.CommissionId == itm.Commission.Id))
                        {
                            dtoCommToreport csum = new dtoCommToreport();
                            csum.CommissionName = itm.Commission.Name;
                            csum.CommissionId = itm.Commission.Id;
                            csum.IsActive = (isCommunityManager) ? true : itm.Commission.IsInCommission(UC.CurrentUserID);
                            csum.IsMaster = itm.Commission.IsMaster;
                            csum.CommunityId = communityId;
                            csum.CallId = (itm.Commission.Call != null) ? itm.Commission.Call.Id : 0;
                            csum.Status = itm.Commission.Status;
                            summary.Commissions.Add(csum); // commName);
                        }


                    }
                    else
                    {
                        dtoStepSummaryItem dtoitmCm = new dtoStepSummaryItem();
                        dtoitmCm.AverageRating = itm.AverageRating;
                        dtoitmCm.SumRating = itm.SumRating;
                        dtoitmCm.BoolRating = itm.BoolRating;
                        dtoitmCm.Passed = false;
                        dtoitmCm.ComissionName = "not started";
                        dtoitmCm.ComissionId = 0;
                        dtoitmCm.Status = CommissionStatus.Draft;
                        dtoitmCm.isStep = false;
                        //dtoitmCm.Rank = itm.Rank;
                        dtoitmCm.minScore = 0;
                        //dtoitmCm.BoolTotal = itm.com

                        dtosub.Commissions.Add(dtoitmCm);


                        string commName = dtoitmCm.ComissionName;
                        //(itm.Commission.IsMaster) ?
                        //String.Format("(*) {0}", dtoitmCm.ComissionName) :
                        //dtoitmCm.ComissionName;


                        if (!summary.Commissions.Any(c => c.CommissionName == commName))
                        {

                            dtoCommToreport csum = new dtoCommToreport();
                            csum.CommissionName = commName;
                            csum.CommissionId = 0;
                            csum.IsActive = false;
                            csum.IsMaster = false;
                            csum.CommunityId = 0;
                            csum.Status = CommissionStatus.Draft;
                            csum.CallId = 0;
                            summary.Commissions.Add(csum); // commName);
                        }

                    }
                }

                summary.items.Add(dtosub);
            }


            bool ispresident = false;

            if (step.Type == StepType.validation)
            {
                ispresident = step.Commissions.Any(c => c.President != null && c.President.Id == UC.CurrentUserID);
            }
            else
            {
                ispresident = step.Commissions.Any(c => c.IsMaster && c.President != null && c.President.Id == UC.CurrentUserID);
            }


            summary.isForClosing = ispresident && step.Status == StepStatus.WaitforClosing;


            //summary.items
            return summary;
        }

        #endregion

        #region Commission

        /// <summary>
        /// Genera una nuova commissione con dati di defualt
        /// </summary>
        /// <param name="call">Bando</param>
        /// <param name="step">Step</param>
        /// <param name="person">Utente (se vuoto viene utilizzato l'utente corrente)</param>
        /// <param name="defaultName">Nome di default commissione</param>
        /// <param name="isMaster">Se è la commissione principale</param>
        /// <returns>Un nuovo oggetto commission</returns>
        private Advanced.Domain.AdvCommission getDefaultCommission(
            Domain.CallForPaper call,
            Advanced.Domain.AdvStep step,
            Core.DomainModel.litePerson person,
            string defaultName,
            bool isMaster)
        {
            if (person == null)
                person = GetCurrentPerson();

            Advanced.Domain.AdvCommission commission = new Advanced.Domain.AdvCommission();
            commission.CreateMetaInfo(person, UC.IpAddress, UC.ProxyIpAddress);

            commission.Call = call;
            commission.Description = "";

            commission.Members = new List<Advanced.Domain.AdvMember>();
            Advanced.Domain.AdvMember member = new Advanced.Domain.AdvMember();
            member.CreateMetaInfo(person, UC.IpAddress, UC.ProxyIpAddress);
            member.Member = person;
            member.Commission = commission;
            member.IsPresident = true;
            commission.Members.Add(member);

            commission.Name = defaultName;
            commission.President = person;
            commission.Secretary = person;
            commission.Status = CommissionStatus.Draft;
            commission.Step = step;
            commission.IsMaster = isMaster;

            if(step.Type == StepType.economics)
            {
                Double sum = 0;
                try
                {
                    IList<Domain.FieldDefinition> field = Manager.GetAll<Domain.FieldDefinition>(
                        fd =>
                            fd.Deleted == Core.DomainModel.BaseStatusDeleted.None
                            && fd.Call.Id == call.Id
                            && fd.Type == Domain.FieldType.TableReport
                        );

                    sum = field.Sum(fl => fl.TableMaxTotal);

                } catch { }

                commission.MaxValue = sum;
            }

            return commission;
        }

        /// <summary>
        /// Aggiunge una commissione
        /// </summary>
        /// <param name="CommissionId">Id commissione "sorella"</param>
        /// <param name="name">Nome nuova commissione</param>
        /// <returns>
        /// -1: Non è possibile aggiungere una commissione (regole business)
        /// -2: Call, step o commission non trovati
        /// -3: errore interno
        /// </returns>
        public long CommissionAdd(Int64 CommissionId, string name)
        {
            Advanced.Domain.AdvCommission comm = Manager.Get<Advanced.Domain.AdvCommission>(CommissionId);

            if(comm == null || comm.Step == null || comm.Step.Call == null)
            {
                return -2;
            }

            if (comm == null
                && comm.Step != null && comm.Step.Call != null
                && (comm.Step.Status == StepStatus.Locked || comm.Step.Status == StepStatus.Closed))
                return -1;

            Advanced.Domain.AdvStep step = comm.Step;

            CallForPapers.Domain.CallForPaper call = Manager.Get<Domain.CallForPaper>(comm.Step.Call.Id);
            if (call == null || call.Id <= 0)
                return -2;
            
            step.Commissions.Add(getDefaultCommission(call, step, GetCurrentPerson(), name, false));

            if (!Manager.IsInTransaction())
                Manager.BeginTransaction();

            try
            {
                Manager.SaveOrUpdate<Advanced.Domain.AdvStep>(step);
                Manager.Commit();
            }
            catch (Exception ex)
            {
                Manager.RollBack();
                return -3;
            }

            long NewComId = -3;

            try
            {
                NewComId = (from Advanced.Domain.AdvCommission stcomm in step.Commissions
                            where stcomm != null && stcomm.Id != CommissionId
                            select stcomm.Id).ToList().OrderByDescending(Id => Id).FirstOrDefault();
            } catch { }

            return NewComId;
        }
        /// <summary>
        /// Cancellazione di una commissione
        /// </summary>
        /// <param name="CommissionId">Id commissione</param>
        /// <returns>True se la commissione è cancellata</returns>
        public bool CommissionDelete(Int64 CommissionId)
        {
            Advanced.Domain.AdvCommission comm = Manager.Get<Advanced.Domain.AdvCommission>(CommissionId);
            if (comm == null || comm.Status != CommissionStatus.Draft)
                return false;

            bool stepdeleted = false;

            if (comm.Step != null)
            {
                Int64 StepId = comm.Step.Id;
                Advanced.Domain.AdvStep step = Manager.Get<Advanced.Domain.AdvStep>(StepId);

                if (step != null && step.Status == StepStatus.Draft && step.Commissions != null && !step.Commissions.Any(com => com.Id != CommissionId))
                {
                    if (!Manager.IsInTransaction())
                        Manager.BeginTransaction();

                    try
                    {
                        Manager.DeletePhysical<Advanced.Domain.AdvStep>(step);
                        Manager.Commit();
                        stepdeleted = true;
                    }
                    catch (Exception ex)
                    {
                        Manager.RollBack();
                        return false;
                    }
                }



                if (!stepdeleted)
                {
                    if (!Manager.IsInTransaction())
                        Manager.BeginTransaction();

                    try
                    {
                        step.Commissions.Remove(comm);
                        Manager.DeletePhysical<Advanced.Domain.AdvCommission>(comm);
                        Manager.Commit();
                        stepdeleted = true;
                    }
                    catch (Exception ex)
                    {
                        Manager.RollBack();
                        return false;
                    }
                }

            }
            else
            {
                if (!Manager.IsInTransaction())
                    Manager.BeginTransaction();

                try
                {
                    Manager.DeletePhysical<Advanced.Domain.AdvCommission>(comm);
                    Manager.Commit();
                    stepdeleted = true;
                }
                catch (Exception ex)
                {
                    Manager.RollBack();
                    return false;
                }
            }
            return true;
        }


        /// <summary>
        /// Modifica lo stato di una commissione
        /// </summary>
        /// <param name="commId">Id commissione</param>
        /// <param name="newStatus">Nuovo stato</param>
        /// <returns>Feedback sulla modifica stato.</returns>
        public CommissionStatusFeedback ChangeCommissionStatus(long commId, CommissionStatus newStatus)
        {
            Advanced.Domain.AdvCommission comm = Manager.Get<Advanced.Domain.AdvCommission>(commId);

            if (comm == null)
                return CommissionStatusFeedback.NotFount;

            Domain.CallForPaperStatus status = comm.Call.Status;

            if (!(newStatus == CommissionStatus.Draft || newStatus == CommissionStatus.ViewSubmission)
                && status != Domain.CallForPaperStatus.SubmissionClosed)
                return CommissionStatusFeedback.NotPermitted;

            //Check! Se va bene che sia solo ed unicamente il presidente!!!

            bool IsPresident = (comm.President.Id == UC.CurrentUserID);
            bool IsSecretary = (comm.Secretary.Id == UC.CurrentUserID);
            bool Creator = comm.CreatedBy.Id == UC.CurrentUserID;



            if (!(IsPresident || IsSecretary || Creator))
                return CommissionStatusFeedback.NoPermission;

            if (comm.Status == newStatus)
                return CommissionStatusFeedback.Success;

            if (newStatus == CommissionStatus.Started && !CommissionCanOpen(comm))
            {
                return CommissionStatusFeedback.NotPermitted;
            }


            switch (comm.Status)
            {
                case CommissionStatus.Draft:
                    if (!(newStatus == CommissionStatus.ViewSubmission || newStatus == CommissionStatus.Started))
                    {
                        return CommissionStatusFeedback.NotPermitted;
                    }
                    if (newStatus == CommissionStatus.Started && !IsPresident)
                        return CommissionStatusFeedback.NotPermitted;

                    break;

                case CommissionStatus.ViewSubmission:
                    if (!(newStatus == CommissionStatus.Draft || newStatus == CommissionStatus.Started))
                    {
                        return CommissionStatusFeedback.NotPermitted;
                    }
                    if (newStatus == CommissionStatus.Started && !IsPresident)
                        return CommissionStatusFeedback.NotPermitted;

                    break;
                case CommissionStatus.Started:

                    if (!(newStatus == CommissionStatus.Locked))
                    {
                        return CommissionStatusFeedback.NotPermitted;
                    }

                    break;
                case CommissionStatus.Locked:
                    if (!(newStatus == CommissionStatus.Started || newStatus == CommissionStatus.Closed))
                    {
                        return CommissionStatusFeedback.NotPermitted;
                    }

                    if (newStatus == CommissionStatus.Closed && !IsPresident)
                        return CommissionStatusFeedback.NotPermitted;
                    break;


                case CommissionStatus.Closed:
                    return CommissionStatusFeedback.NotPermitted;
            }

            return ChangeCommissionStatusInternal(comm, newStatus);
        }

        /// <summary>
        /// Recupera i dati di una commissione per l'edit
        /// </summary>
        /// <param name="CommissionId">Id commissione</param>
        /// <returns>Dati della commissione</returns>
        public Advanced.dto.dtoCommissionEdit CommissionGet(Int64 CommissionId)
        {
            Advanced.Domain.AdvCommission comm = Manager.Get<Advanced.Domain.AdvCommission>(CommissionId);
            int currentUserId = UC.CurrentUserID;

            if (comm == null)
                return null;


            Advanced.dto.dtoCommissionEdit dtoComm = (comm == null) ?
                new Advanced.dto.dtoCommissionEdit(0)
                : dtoComm = new Advanced.dto.dtoCommissionEdit(comm, currentUserId);



            dtoComm.IsPresident = false;

            if (dtoComm.President != null && dtoComm.President.Id == currentUserId)
            {



                dtoComm.IsPresident = true;

                if (dtoComm.Status == CommissionStatus.ValutationEnded)
                {
                    var x = (from Eval.Evaluation ev in Manager.GetIQ<Eval.Evaluation>()
                             where ev.AdvCommission != null && ev.AdvCommission.Id == CommissionId
                             && ev.Deleted == Core.DomainModel.BaseStatusDeleted.None
                             select ev);

                    int Total = x.Count();
                    int evaluated = x.Where(ev => ev.Status == Eval.EvaluationStatus.Evaluated).Count();

                    if (Total == evaluated)
                        dtoComm.Permission |= CommissionPermission.CloseEvaluation;
                }
                else if (dtoComm.Status == CommissionStatus.ValutationConfirmed)
                {
                    dtoComm.Permission |= CommissionPermission.CloseCommission;
                }

                if (comm.Criteria.Any()
                    && (comm.Call == null || comm.Call.Status != Domain.CallForPaperStatus.SubmissionClosed))
                    dtoComm.Permission |= CommissionPermission.OpenEvaluation;
            }

            if ((dtoComm.Permission & CommissionPermission.OpenEvaluation) == CommissionPermission.OpenEvaluation)
            {
                //Contorollo step precedente!!!
                bool prevStepIsClosed = CommissionCanOpen(comm);

                if (!prevStepIsClosed)
                {
                    dtoComm.Permission &= ~CommissionPermission.OpenEvaluation;
                }
            }
            return dtoComm;
        }

        /// <summary>
        /// Verifica secondo le logiche di business se la commissione puo' essere aperta
        /// </summary>
        /// <param name="Comm">Oggetto commissione</param>
        /// <returns>True se puo' essere aperta</returns>
        private bool CommissionCanOpen(Advanced.Domain.AdvCommission Comm)
        {

            bool prevStepIsClosed = false;

            if (Comm.Call != null && Comm.Step != null)
            {
                if (Comm.Step.Type != StepType.validation)
                {
                    Advanced.Domain.AdvStep prevStep = null;

                    if (Comm.Step.Order > 1)
                    {
                        prevStep = Manager.GetAll<Advanced.Domain.AdvStep>(
                            st => st.Call != null
                            && st.Call.Id == Comm.Call.Id
                            && st.Type != StepType.economics
                            && st.Order < Comm.Step.Order
                            )
                        .OrderBy(st => st.Order)
                        .LastOrDefault();
                    }
                    else
                    {
                        prevStep = Manager.GetAll<Advanced.Domain.AdvStep>(
                            st => st.Call != null
                            && st.Call.Id == Comm.Call.Id
                            && st.Type == StepType.validation
                            )
                        .LastOrDefault();
                    }

                    if (prevStep.Status == StepStatus.Closed)
                    {
                        prevStepIsClosed = true;
                    }


                }
                else
                {
                    prevStepIsClosed = true;

                    if (Comm.Call.Status == Domain.CallForPaperStatus.Draft
                        || Comm.Call.Status == Domain.CallForPaperStatus.Published
                        || Comm.Call.Status == Domain.CallForPaperStatus.SubmissionOpened)
                    {
                        prevStepIsClosed = false;
                    }

                }

            }

            return prevStepIsClosed;
        }

        /// <summary>
        /// Aggiorna i dati di una commissione
        /// </summary>
        /// <param name="Id">Id commissione</param>
        /// <param name="Name">Nome commissione</param>
        /// <param name="Description">Descrizione</param>
        /// <param name="Tags">Tag associati alla commissione, separati da ,</param>
        /// <param name="isMaster">Se è la commissione principale</param>
        /// <param name="EvType">Tipo di aggregazione valutazioni membri</param>
        /// <param name="EvMinVal">Valore minimo superamento commissione</param>
        /// <param name="EvLockBool">True per far sì che le valutazioni binarie siano vincolanti ai fini del superamento</param>
        /// <param name="StepEvType">Tipo di aggregazione tra le commissione (solo per commissione principale)</param>
        /// <param name="criterionsToAdd">Criteri della commissione</param>
        /// <returns>True se la modifica ha avuto successo</returns>
        public bool CommissionUpdate(
            Int64 Id,
            String Name,
            String Description,
            string Tags,
            bool isMaster,
            EvalType EvType,
            int EvMinVal,
            bool EvLockBool,
            EvalType StepEvType,
            Double MaxValue,
            Int64 TemplateId,
            Int64 TemplateVersionId,
            List<Eval.dtoCriterion> criterionsToAdd = null)
        {


            Advanced.Domain.AdvCommission Comm = Manager.Get<Advanced.Domain.AdvCommission>(Id);

            if (Comm == null)
                return false;

            Comm.Name = Name;
            Comm.Description = Description;
            Comm.Tags = Tags;
            Comm.MaxValue = MaxValue;
            Comm.EvalType = EvType;
            Comm.EvalMinValue = EvMinVal;
            Comm.EvalBoolBlock = EvLockBool;

            Comm.TemplateId = TemplateId;
            Comm.TemplateVersionId = TemplateVersionId;


            Comm.UpdateMetaInfo(GetCurrentPerson(), UC.IpAddress, UC.ProxyIpAddress);

            if (Comm.Criteria == null)
                Comm.Criteria = new List<Eval.BaseCriterion>();

            Advanced.Domain.AdvCommission CommReMaster = null;

            if (Comm.IsMaster != isMaster)
            {
                if (Comm.Step != null)
                {
                    if (Comm.Step.Commissions != null)
                    {
                        if (isMaster)
                        {
                            CommReMaster = Comm.Step.Commissions.FirstOrDefault(cm => cm.IsMaster);

                            if (CommReMaster != null)
                            {
                                CommReMaster.IsMaster = false;
                                Comm.IsMaster = true;
                                CommReMaster.UpdateMetaInfo(GetCurrentPerson(), UC.IpAddress, UC.ProxyIpAddress);
                            }
                        }
                        else
                        {
                            CommReMaster = Comm.Step.Commissions.Where(cm => cm.Id != Id).OrderBy(cm => cm.Id).FirstOrDefault();
                            if (CommReMaster != null)
                            {
                                CommReMaster.IsMaster = true;
                                Comm.IsMaster = false;
                                CommReMaster.UpdateMetaInfo(GetCurrentPerson(), UC.IpAddress, UC.ProxyIpAddress);
                            }
                        }
                    }
                }
            }

            //Dopo aver impostato eventuali modifiche su Master,
            //decido se modificare il tipo di valutazione dello STEP
            if (Comm.IsMaster && Comm.Step != null)
            {
                Comm.Step.EvalType = (StepEvType == EvalType.none) ? EvalType.Average : StepEvType;
            }

            if (criterionsToAdd != null && criterionsToAdd.Any())
            {
                bool toadd = false;

                int currentorder = Comm.Criteria.Count() + 1;

                foreach (lm.Comol.Modules.CallForPapers.Domain.Evaluation.dtoCriterion item
                    in criterionsToAdd
                    .OrderBy(c => c.DisplayOrder)
                    .ThenBy(c => c.Name))
                {

                    Eval.BaseCriterion criterion = Comm.Criteria.FirstOrDefault(c => c.Id == item.Id);

                    if (criterion == null || criterion.Id == 0)
                    {
                        toadd = true;
                        switch (item.Type)
                        {
                            case Eval.CriterionType.Boolean:
                                criterion = new Eval.BoolCriterion();
                                break;
                            case Eval.CriterionType.Textual:
                                criterion = new Eval.TextualCriterion();
                                ((Eval.TextualCriterion)criterion).MaxLength = item.MaxLength;
                                break;
                            case Eval.CriterionType.StringRange:
                                criterion = new Eval.StringRangeCriterion();
                                ((Eval.StringRangeCriterion)criterion).MaxOption = item.MaxOption;
                                ((Eval.StringRangeCriterion)criterion).MinOption = item.MinOption;
                                break;
                            case Eval.CriterionType.IntegerRange:
                            case Eval.CriterionType.DecimalRange:
                                criterion = new Eval.NumericRangeCriterion(item.DecimalMinValue, item.DecimalMaxValue, item.Type);
                                break;
                            //case Eval.CriterionType.RatingScale:
                            //case Eval.CriterionType.RatingScaleFuzzy:
                            //    criterion = new DssCriterion();
                            //    criterion.Type = dto.Type;
                            //    ((DssCriterion)criterion).IsFuzzy = (dto.Type == CriterionType.RatingScaleFuzzy);
                            //    ((DssCriterion)criterion).IdRatingSet = dto.IdRatingSet;
                            //    break;
                            default:
                                criterion = new Eval.BaseCriterion();
                                criterion.Type = Eval.CriterionType.Textual;
                                break;
                        }
                        criterion.CreateMetaInfo(GetCurrentPerson(), UC.IpAddress, UC.ProxyIpAddress);
                        criterion.DisplayOrder = currentorder;

                        criterion.Name = String.IsNullOrEmpty(item.Name) ?
                            string.Format("Criterio {0}", currentorder)
                            : item.Name.Contains("{0}") ?
                                string.Format(item.Name, currentorder) : item.Name;

                        currentorder++;
                    }
                    else
                    {
                        switch (item.Type)
                        {
                            case Eval.CriterionType.Boolean:
                                break;
                            case Eval.CriterionType.Textual:
                                ((Eval.TextualCriterion)criterion).MaxLength = item.MaxLength;
                                break;
                            case Eval.CriterionType.StringRange:
                                ((Eval.StringRangeCriterion)criterion).MaxOption = item.MaxOption;
                                ((Eval.StringRangeCriterion)criterion).MinOption = item.MinOption;
                                break;
                            case Eval.CriterionType.IntegerRange:
                            case Eval.CriterionType.DecimalRange:
                                ((Eval.NumericRangeCriterion)criterion).DecimalMinValue = item.DecimalMinValue;
                                ((Eval.NumericRangeCriterion)criterion).DecimalMaxValue = item.DecimalMaxValue;
                                criterion.Type = item.Type;
                                break;
                        }
                        criterion.UpdateMetaInfo(GetCurrentPerson(), UC.IpAddress, UC.ProxyIpAddress);
                        criterion.Name = item.Name;
                    }

                    criterion.Committee = null;
                    criterion.AdvCommitee = Comm;

                    criterion.UseDss = false;



                    criterion.Description = item.Description;
                    //criterion.DisplayOrder = displayNumber;//item.DisplayOrder;
                    criterion.CommentType = item.CommentType;
                    //Manager.SaveOrUpdate(criterion);
                    if (item.Type == Eval.CriterionType.StringRange)
                    {
                        SaveOptions(GetCurrentPerson(), ((Eval.StringRangeCriterion)criterion), item.Options);
                    }
                    //

                    if (toadd)
                        Comm.Criteria.Add(criterion);
                }

            }

            if (!Manager.IsInTransaction())
                Manager.BeginTransaction();

            try
            {
                Manager.SaveOrUpdate<Advanced.Domain.AdvCommission>(Comm);

                if (CommReMaster != null)
                {
                    Manager.SaveOrUpdate<Advanced.Domain.AdvCommission>(CommReMaster);
                }

                Manager.Commit();
            }
            catch (Exception ex)
            {
                Manager.RollBack();
                return false;
            }

            return true;
        }
        
        /// <summary>
        /// VErifica se una commissione puo' partire
        /// </summary>
        /// <param name="comm">Id commissione</param>
        /// <returns>True se la commissione puo' essere aperta</returns>
        private bool CheckCommissionStarting(Advanced.Domain.AdvCommission comm)
        {
            if (comm.Step != null && comm.Step.Type == StepType.economics)
                return true;

            return comm.Criteria.Any();
        }

        /// <summary>
        /// VErifica che la commissione possa essere chiusa
        /// </summary>
        /// <param name="comm">Commissione</param>
        /// <returns>True</returns>
        private bool CheckCommissionClosing(Advanced.Domain.AdvCommission comm)
        {
            return true;
        }

        /// <summary>
        /// Dato l'id di una commissione, recupera i dati per l'aggregazione delle valutazioni dei membri
        /// </summary>
        /// <param name="commissionId">Id commissione</param>
        /// <returns>Dati aggregazione valutazioni</returns>
        public Advanced.dto.dtoCommEvalInfo CommissionEvalTypeGet(long commissionId)
        {


            Advanced.dto.dtoCommEvalInfo EvInfo = (from Advanced.Domain.AdvCommission comm in Manager.GetIQ<Advanced.Domain.AdvCommission>()
                                                   where comm.Id == commissionId
                                                   select new dtoCommEvalInfo
                                                   {
                                                       Type = comm.EvalType,
                                                       minRange = comm.EvalMinValue,
                                                       LockBool = comm.EvalBoolBlock
                                                   }).FirstOrDefault();

            if (EvInfo == null)
                return new dtoCommEvalInfo();

            return EvInfo;
        }

        /// <summary>
        /// Recupera le informazioni generiche sullo stato delle valutazioni di una commissione, per una data sottomissione
        /// </summary>
        /// <param name="idSubmission">Id sottomissione</param>
        /// <param name="idCall">Id bando</param>
        /// <param name="idCommission">Id commissioni</param>
        /// <returns>Informazioni generiche stato valutazioni sottomissione</returns>
        public List<Eval.dtoCommitteeEvaluationInfo> GetCommitteesInfoForSubmission(
            long idSubmission,
            long idCall,
            long idCommission)
        {
            List<Eval.dtoCommitteeEvaluationInfo> items = new List<Eval.dtoCommitteeEvaluationInfo>();
            try
            {
                long idType = (from s in Manager.GetIQ<Domain.UserSubmission>() where s.Id == idSubmission select s.Type.Id).Skip(0).Take(1).ToList().FirstOrDefault();


                //List<long> idCommittees = (from m in Manager.GetIQ<CommitteeAssignedSubmitterType>()
                //                           where m.Deleted == BaseStatusDeleted.None && m.SubmitterType.Id == idType
                //                           select m.Committee.Id).ToList();



                Advanced.Domain.AdvCommission comm = Manager.Get<Advanced.Domain.AdvCommission>(idCommission);

                if (comm != null)
                {
                    Eval.dtoCommitteeEvaluationInfo item = new Eval.dtoCommitteeEvaluationInfo()
                    {
                        IdCommittee = comm.Id,
                        Name = comm.Name,
                        DisplayOrder = 1,
                        MinValue = comm.EvalMinValue
                    };

                    items.Add(item);
                }


                //items = (from s in Manager.GetIQ<EvaluationCommittee>()
                //         where s.Deleted == BaseStatusDeleted.None && (idCommittees.Contains(s.Id) 
                //         || (s.ForAllSubmittersType && s.Call.Id == idCall))

                //         select new dtoCommitteeEvaluationInfo()
                //         {
                //             IdCommittee = s.Id,
                //             Name = s.Name,
                //             DisplayOrder = s.DisplayOrder
                //         }).ToList();


                var query = (from e in Manager.GetIQ<Eval.Evaluation>()
                             where e.Deleted == Core.DomainModel.BaseStatusDeleted.None
                             && e.Submission.Id == idSubmission
                             && e.AdvCommission != null && e.AdvCommission.Id == idCommission
                             select e);

                foreach (Eval.dtoCommitteeEvaluationInfo c in items)
                {
                    c.Status = GetCommitteeEvaluationsStatistics(c.IdCommittee, query);
                }
            }
            catch (Exception ex)
            {

            }
            return items.OrderBy(s => s.DisplayOrder).ThenBy(s => s.Name).ToList();
        }

        /// <summary>
        /// Recupera le statistiche relative alle valutazioni date da una commissione
        /// </summary>
        /// <param name="idCommittee">Id commissione</param>
        /// <param name="query">query valutazioni</param>
        /// <returns>Statistiche valutazioni</returns>
        private Eval.EvaluationStatus GetCommitteeEvaluationsStatistics(
            long idCommittee,
            IQueryable<Eval.Evaluation> query)
        {
            Eval.EvaluationStatus status = Eval.EvaluationStatus.None;

            List<Eval.EvaluationStatus> items = query
                .Where(q =>
                    q.Deleted == Core.DomainModel.BaseStatusDeleted.None
                    && q.AdvCommission != null && q.AdvCommission.Id == idCommittee
                    ).Select(q => q.Status).ToList();

            if (items.All(i => i == Eval.EvaluationStatus.Confirmed))
                status = Eval.EvaluationStatus.Confirmed;

            else if (items.Any())
            {
                List<Eval.EvaluationStatus> availableStatus =
                    items.Where(i =>
                        i != Eval.EvaluationStatus.Invalidated
                        && i != Eval.EvaluationStatus.EvaluatorReplacement).ToList();

                status = (items.Distinct().Count() == 1) ?
                    items[0] :
                    (availableStatus.Distinct().Count() == 1) ?
                        availableStatus[0] :
                        Eval.EvaluationStatus.Evaluating;
            }
            else
                status = Eval.EvaluationStatus.None;

            return status;
        }
        /// <summary>
        /// Modifica stato di una commissione
        /// </summary>
        /// <param name="comm">Commissione</param>
        /// <param name="newStatus">Nuovo stato</param>
        /// <returns>Feddback operazione</returns>
        /// <remarks>
        /// Vengono effettuati i vari controlli sullo stato precedente
        /// </remarks>
        public CommissionStatusFeedback ChangeCommissionStatusInternal(Advanced.Domain.AdvCommission comm, CommissionStatus newStatus)
        {

            if (newStatus == CommissionStatus.Started
                 && !CheckCommissionStarting(comm))
            {
                return CommissionStatusFeedback.NotPermitted;
            }
            else if (newStatus == CommissionStatus.Closed
                && CheckCommissionClosing(comm))
            {
                return CommissionStatusFeedback.NotPermitted;
            }

            comm.Status = newStatus;

            if (comm.Status == CommissionStatus.Started &&
                comm.Step != null)
            {
                comm.Step.Status = StepStatus.Started;
            }

            if (comm.IsMaster && comm.Status == CommissionStatus.Closed)
            {
                comm.Step.Status = StepStatus.WaitforClosing;
            }

            if (comm.Step.Type == StepType.economics && comm.Status == CommissionStatus.Closed)
            {
                comm.Step.Status = StepStatus.Closed;
            }

            try
            {
                if (!Manager.IsInTransaction())
                    Manager.BeginTransaction();

                comm.UpdateMetaInfo(GetCurrentPerson(), UC.IpAddress, UC.ProxyIpAddress);

                Manager.SaveOrUpdate<Advanced.Domain.AdvCommission>(comm);

                Manager.Commit();

            }
            catch
            {
                return CommissionStatusFeedback.Unknow;
            }

            return CommissionStatusFeedback.Success;
        }
        /// <summary>
        /// Recupera le valutazioni per un valutatore
        /// </summary>
        /// <param name="idEvaluator">Id valutatore</param>
        /// <returns>Info valutazioni</returns>
        public List<Eval.dtoCommitteeEvaluationInfo> GetCommitteesInfoForEvaluator(long idEvaluator)
        {
            List<Eval.dtoCommitteeEvaluationInfo> items = new List<Eval.dtoCommitteeEvaluationInfo>();
            try
            {
                List<long> idCommittees = (from m in Manager.GetIQ<Advanced.Domain.AdvMember>()
                                           where m.Deleted == Core.DomainModel.BaseStatusDeleted.None
                                           && m.Id == idEvaluator
                                           select m.Commission.Id).ToList();

                items = (from s in Manager.GetIQ<Advanced.Domain.AdvCommission>()
                         where s.Deleted == Core.DomainModel.BaseStatusDeleted.None && idCommittees.Contains(s.Id)
                         select new Eval.dtoCommitteeEvaluationInfo()
                         {
                             IdCommittee = s.Id,
                             Name = s.Name,
                             DisplayOrder = 0,
                             Status = Eval.EvaluationStatus.None
                         }).ToList();

                var query = (from e in Manager.GetIQ<Eval.Evaluation>()
                             where e.Deleted == Core.DomainModel.BaseStatusDeleted.None && e.AdvEvaluator.Id == idEvaluator
                             select e);

                foreach (Eval.dtoCommitteeEvaluationInfo c in items)
                {
                    c.Status = GetCommitteeEvaluationsStatistics(c.IdCommittee, query);
                }
            }
            catch (Exception ex)
            {

            }
            return items.OrderBy(s => s.DisplayOrder).ThenBy(s => s.Name).ToList();
        }

        #endregion

        #region EconomicEvaluation


        /// <summary>
        /// Verifica se per il bando è necessaria una commissione economica
        /// </summary>
        /// <param name="callId">Id Bando</param>
        /// <returns>True se il bando contiene tabelle economiche</returns>
        public bool EconomicCommissionNeed(Int64 callId)
        {
            Domain.CallForPaper call = Manager.Get<Domain.CallForPaper>(callId);
            return EconomicCommissionNeed(call);
        }

        /// <summary>
        ///  Verifica se per il bando è necessaria una commissione economica
        /// </summary>
        /// <param name="call">Bando</param>
        /// <returns>True se il bando contiene tabelle economiche</returns>
        public bool EconomicCommissionNeed(Domain.CallForPaper call)
        {
            bool hasTableReport = false;

            foreach (Domain.FieldsSection s in call.Sections)
            {
                if (s.Fields.Any(f => f.Type == Domain.FieldType.TableReport))
                {
                    hasTableReport = true;
                }
            }

            bool hasStepCommission = (from Advanced.Domain.AdvCommission comm in Manager.GetIQ<Advanced.Domain.AdvCommission>()
                                      where comm.Call != null && comm.Call.Id == call.Id && comm.Step.Type == StepType.economics
                                      select comm.Id).Any();

            return hasTableReport && !hasStepCommission;
        }

        #endregion
        
        #region CommissionMember
        /// <summary>
        /// Aggiunta di un membro alla commissione
        /// </summary>
        /// <param name="CommissionId">Id commissione</param>
        /// <param name="UsersId">Id utente</param>
        /// <returns>True se l'operazione ha successo</returns>
        public bool CommissionMembersAdd(Int64 CommissionId, IList<int> UsersId)
        {

            Advanced.Domain.AdvCommission Comm = Manager.Get<Advanced.Domain.AdvCommission>(CommissionId);

            if (Comm == null)
                return false;


            if (Comm.Members == null)
                Comm.Members = new List<Advanced.Domain.AdvMember>();

            Comm.UpdateMetaInfo(GetCurrentPerson(), UC.IpAddress, UC.ProxyIpAddress);

            IList<int> MemberPersId = (from Advanced.Domain.AdvMember cm
                                       in Comm.Members
                                       where cm.Member != null
                                       select cm.Member.Id).ToList();

            UsersId = UsersId.Where(pid => !MemberPersId.Contains(pid)).ToList();

            IList<Core.DomainModel.litePerson> persons = Manager.GetAll<Core.DomainModel.litePerson>(p => UsersId.Contains(p.Id));

            foreach (Core.DomainModel.litePerson p in persons)
            {
                Advanced.Domain.AdvMember mem = new Advanced.Domain.AdvMember();
                mem.CreateMetaInfo(GetCurrentPerson(), UC.IpAddress, UC.ProxyIpAddress);

                mem.Commission = Comm;
                mem.Member = p;

                Comm.Members.Add(mem);
            }

            if (!Manager.IsInTransaction())
                Manager.BeginTransaction();

            try
            {
                Manager.SaveOrUpdate<Advanced.Domain.AdvCommission>(Comm);
                Manager.Commit();
            }
            catch (Exception ex)
            {
                Manager.RollBack();
                return false;
            }

            return true;
        }
        /// <summary>
        /// Cancella un membro, dato l'id persona
        /// </summary>
        /// <param name="CommissionId">Id commissione</param>
        /// <param name="UserId">Id utente</param>
        /// <returns>true se l'utente è correttamente cancellato</returns>
        public bool CommissionMemberDelPers(Int64 CommissionId, int UserId)
        {
            Advanced.Domain.AdvCommission Comm = Manager.Get<Advanced.Domain.AdvCommission>(CommissionId);

            if (Comm == null)
                return false;


            if (Comm.Members == null)
                return true;

            Comm.UpdateMetaInfo(GetCurrentPerson(), UC.IpAddress, UC.ProxyIpAddress);

            Advanced.Domain.AdvMember delMem = Comm.Members.FirstOrDefault(m => m.Member != null && m.Member.Id == UserId);

            if (delMem == null)
                return true;

            Comm.Members.Remove(delMem);

            if (!Manager.IsInTransaction())
                Manager.BeginTransaction();

            try
            {
                Manager.SaveOrUpdate<Advanced.Domain.AdvCommission>(Comm);
                Manager.Commit();
            }
            catch (Exception ex)
            {
                Manager.RollBack();
                return false;
            }

            return true;
        }
        /// <summary>
        /// Cancella un membro, dato l'Id membro
        /// </summary>
        /// <param name="CommissionId">Id commissione</param>
        /// <param name="MemberId">Id membro</param>
        /// <returns>True se è cancellato</returns>
        public bool CommissionMembersDel(Int64 CommissionId, Int64 MemberId)
        {
            Advanced.Domain.AdvCommission Comm = Manager.Get<Advanced.Domain.AdvCommission>(CommissionId);

            if (Comm == null)
                return false;


            if (Comm.Members == null)
                return true;

            Comm.UpdateMetaInfo(GetCurrentPerson(), UC.IpAddress, UC.ProxyIpAddress);

            Advanced.Domain.AdvMember delMem = Comm.Members.FirstOrDefault(m => m.Member != null && m.Id == MemberId);

            if (delMem == null)
                return true;

            Comm.Members.Remove(delMem);

            if (!Manager.IsInTransaction())
                Manager.BeginTransaction();

            try
            {
                Manager.SaveOrUpdate<Advanced.Domain.AdvCommission>(Comm);
                Manager.Commit();
            }
            catch (Exception ex)
            {
                Manager.RollBack();
                return false;
            }


            return true;
        }
        
        /// <summary>
        /// Modifica il presidente di una commissione
        /// </summary>
        /// <param name="CommissionId">Id commissione</param>
        /// <param name="UserId">Id persona nuovo presidente</param>
        /// <returns>True se è modificato con successo</returns>
        /// <remarks>
        /// Alla commissione è sempre associato un presidente ed il presidente è sempre presente tra i valutatori della commissione.
        /// Di default alla creazione viene impostato l'utente corrente (il creatore della commissione).
        /// </remarks>
        public bool CommissionPresidentUpdate(Int64 CommissionId, int UserId)
        {
            Advanced.Domain.AdvCommission Comm = Manager.Get<Advanced.Domain.AdvCommission>(CommissionId);

            if (Comm == null)
                return false;

            if (Comm.President != null && Comm.President.Id == UserId)
                return true;

            Comm.UpdateMetaInfo(GetCurrentPerson(), UC.IpAddress, UC.ProxyIpAddress);
            Core.DomainModel.litePerson NewPres = Manager.GetLitePerson(UserId);

            if (NewPres == null)
                return false;

            Comm.President = NewPres;


            Advanced.Domain.AdvMember OldPresident = Comm.Members.FirstOrDefault(m => m.IsPresident);
            Advanced.Domain.AdvMember NewPresident = Comm.Members.FirstOrDefault(m => m.Member != null && m.Member.Id == NewPres.Id);

            bool newAdded = false;
            if (NewPresident == null || NewPresident.Id == 0)
            {
                NewPresident = new Advanced.Domain.AdvMember();
                NewPresident.CreateMetaInfo(GetCurrentPerson(), UC.IpAddress, UC.ProxyIpAddress);
                NewPresident.Member = NewPres;
                NewPresident.Commission = Comm;
                NewPresident.IsPresident = true;
                Comm.Members.Add(NewPresident);
                newAdded = true;
            }

            if (OldPresident != null)
            {
                if (OldPresident.Id != NewPresident.Id)
                {
                    OldPresident.IsPresident = false;
                    OldPresident.UpdateMetaInfo(GetCurrentPerson(), UC.IpAddress, UC.ProxyIpAddress);
                }
            }

            if (!newAdded)
            {
                NewPresident.IsPresident = true;
                NewPresident.UpdateMetaInfo(GetCurrentPerson(), UC.IpAddress, UC.ProxyIpAddress);
            }

            if (!Manager.IsInTransaction())
                Manager.BeginTransaction();

            try
            {
                Manager.SaveOrUpdate<Advanced.Domain.AdvMember>(NewPresident);

                if (OldPresident != null)
                    Manager.SaveOrUpdate<Advanced.Domain.AdvMember>(OldPresident);

                Manager.SaveOrUpdate<Advanced.Domain.AdvCommission>(Comm);
                Manager.Commit();
            }
            catch (Exception ex)
            {
                Manager.RollBack();
                return false;
            }

            return true;
        }
        /// <summary>
        /// Modifica il segretario di una commissione
        /// </summary>
        /// <param name="CommissionId">Id commissione</param>
        /// <param name="UserId">Id utente</param>
        /// <returns>Ture se l'operazione ha successo</returns>
        public bool CommissionSecretaryUpdate(Int64 CommissionId, int UserId)
        {
            Advanced.Domain.AdvCommission Comm = Manager.Get<Advanced.Domain.AdvCommission>(CommissionId);

            if (Comm == null)
                return false;

            if (Comm.Secretary != null && Comm.President.Id == UserId)
                return true;

            Comm.UpdateMetaInfo(GetCurrentPerson(), UC.IpAddress, UC.ProxyIpAddress);


            //if(UserId > 0)
            //{
            Core.DomainModel.litePerson NewSecr = Manager.GetLitePerson(UserId);

            if (NewSecr == null)
                return false;

            Comm.Secretary = NewSecr;
            //} else
            //{
            //    Comm.Secretary = null;
            //}

            if (!Manager.IsInTransaction())
                Manager.BeginTransaction();

            try
            {
                Manager.SaveOrUpdate<Advanced.Domain.AdvCommission>(Comm);
                Manager.Commit();
            }
            catch (Exception ex)
            {
                Manager.RollBack();
                return false;
            }

            return true;
        }
        /// <summary>
        /// Recupera l'id di un membro, dato l'id utente e la commissione
        /// </summary>
        /// <param name="commissionId">Id commissione</param>
        /// <param name="UserId">Id utente</param>
        /// <returns>Id membro, 0 se non trovato</returns>
        public long MemberIdGet(long commissionId, int UserId = 0)
        {
            if (UserId <= 0)
            {
                UserId = UC.CurrentUserID;
            }

            Advanced.Domain.AdvCommission commission = Manager.Get<Advanced.Domain.AdvCommission>(commissionId);

            if (commission == null || !commission.Members.Any())
                return 0;

            Advanced.Domain.AdvMember member = commission.Members.FirstOrDefault(m => m.Member != null && m.Member.Id == UserId);

            if (member == null)
                return 0;

            return member.Id;
        }

        /// <summary>
        /// Verifica se la commissione può essere chiusa
        /// </summary>
        /// <param name="CommId">Id commissione</param>
        /// <returns>True se può essere chiusa</returns>
        public bool CommissionCanClose(long CommId)
        {
            Advanced.Domain.AdvCommission comm = Manager.Get<Advanced.Domain.AdvCommission>(CommId);

            return CommissionCanClose(comm);

        }
        /// <summary>
        /// Verifica se la commissione può essere chiusa
        /// </summary>
        /// <param name="comm">Commissione</param>
        /// <returns>True se può essere chiusa</returns>
        public bool CommissionCanClose(Advanced.Domain.AdvCommission comm)
        {
            if (comm == null || comm.President.Id != UC.CurrentUserID)
                return false;

            if (comm.Status == CommissionStatus.Draft || comm.Status == CommissionStatus.ViewSubmission || comm.Status == CommissionStatus.Closed)
                return false;

            bool closeStep = false;

            if (comm.IsMaster && comm.Step.Commissions.Any(c => !c.IsMaster))
            {
                foreach (Advanced.Domain.AdvCommission commsec in comm.Step.Commissions.Where(c => !c.IsMaster))
                {
                    if (commsec.Status != CommissionStatus.Closed)
                        return false;
                }
            }


            if (comm.Step != null && comm.Step.Type == StepType.economics)
            {
                IList<lm.Comol.Modules.CallForPapers.AdvEconomic.Domain.EconomicEvaluation> evals =
                    Manager.GetAll<lm.Comol.Modules.CallForPapers.AdvEconomic.Domain.EconomicEvaluation>(
                        ev => ev.Step != null && ev.Step.Id == comm.Step.Id);

                return evals.All(ev => ev.Status == AdvEconomic.EvalStatus.confirmed);

            }
            else
            {
                var query =
                    (from Eval.Evaluation ev in Manager.GetIQ<Eval.Evaluation>()
                     where ev.AdvCommission != null
                     && ev.AdvCommission.Id == comm.Id
                     && ev.Deleted == Core.DomainModel.BaseStatusDeleted.None
                     select ev);

                int TotalEval = (from Eval.Evaluation ev in query select ev.Id).Count();
                int CompletedEval = (from Eval.Evaluation ev in query where ev.Status == Eval.EvaluationStatus.Confirmed select ev.Id).Count();


                if (TotalEval <= CompletedEval)
                    return true;
            }
            return false;
        }
        /// <summary>
        /// Chiude la commissione
        /// </summary>
        /// <param name="CommId">Id commissione</param>
        /// <returns>True se la commissione viene chiusa</returns>
        public bool CommissionClose(long CommId)
        {
            Advanced.Domain.AdvCommission comm = Manager.Get<Advanced.Domain.AdvCommission>(CommId);

            if (!CommissionCanClose(comm))
                return false;

            long stepId = (comm.Step != null) ? comm.Step.Id : 0;
            if (stepId == 0)
                return false;
            
            bool stepsuccess = false;

            if (comm.Step.Type == StepType.economics)
            {
                stepsuccess = true; //se non posso chiudere, sono già uscito dalla funzione
            }
            else
            {
                stepsuccess = CommissionEvUpdate(CommId);
            }


            if (!stepsuccess)
                return false;

            stepsuccess = StepEvUpdate(stepId, comm.IsMaster);
            if (!stepsuccess)
                return false;
            
            comm = Manager.Get<Advanced.Domain.AdvCommission>(CommId);
            comm.Status = CommissionStatus.Closed;
            comm.UpdateMetaInfo(GetCurrentPerson(), UC.IpAddress, UC.ProxyIpAddress);

            try
            {
                if (!Manager.IsInTransaction())
                    Manager.BeginTransaction();

                Manager.SaveOrUpdate<Advanced.Domain.AdvCommission>(comm);
                Manager.Commit();


            }
            catch (Exception ex)
            {
                Manager.RollBack();
                return false;
            }


            return stepsuccess;
        }

        /// <summary>
        /// Recupera l'oggetto commissione
        /// </summary>
        /// <param name="CommissionId">Id commissione</param>
        /// <returns>L'oggetto Commissione Avanzata</returns>
        public Advanced.Domain.AdvCommission AdvCommissionGet(long CommissionId)
        {
            return Manager.Get<Advanced.Domain.AdvCommission>(CommissionId);
        }
        /// <summary>
        /// Allega il verbale ad una commissione
        /// </summary>
        /// <param name="CommissionId">Id commissione</param>
        /// <param name="Link">ModuleActionLink verbale</param>
        /// <returns>True se assegnato correttamente</returns>
        public bool AdvCommissionAddVerbale(long CommissionId, lm.Comol.Core.DomainModel.ModuleActionLink Link)
        {
            Advanced.Domain.AdvCommission advCommission = Manager.Get<Advanced.Domain.AdvCommission>(CommissionId);

            if (advCommission == null
                ||
                (advCommission.President.Id != UC.CurrentUserID
                || !(advCommission.Members != null && advCommission.Members.Any(m => m.IsPresident && m.Member.Id == UC.CurrentUserID))
                ))
                return false;

            if (!Manager.IsInTransaction())
                Manager.BeginTransaction();

            advCommission.UpdateMetaInfo(GetCurrentPerson(), UC.IpAddress, UC.ProxyIpAddress);


            lm.Comol.Core.DomainModel.ModuleLink mLink = new lm.Comol.Core.DomainModel.ModuleLink(
                Link.Description,
                Link.Permission,
                Link.Action);

            //ToDo: CHECK NULL!!!

            mLink.CreateMetaInfo(Manager.GetPerson(UC.CurrentUserID), UC.IpAddress, UC.ProxyIpAddress);


            int communityId = 0;
            try
            {
                communityId = advCommission.Step.Call.Community.Id;
            }
            catch { }

            if (communityId == 0)
                communityId = UC.CurrentCommunityID;

            mLink.DestinationItem = (Core.DomainModel.ModuleObject)Link.ModuleObject;
            mLink.SourceItem = Core.DomainModel.ModuleObject.CreateLongObject(
                advCommission.Id,
                advCommission,
                (int)lm.Comol.Modules.CallForPapers.Domain.ModuleCallForPaper.ObjectType.VerbaliCommissione,
                communityId,
                lm.Comol.Modules.CallForPapers.Domain.ModuleCallForPaper.UniqueCode,
                ServiceModuleID(lm.Comol.Modules.CallForPapers.Domain.ModuleCallForPaper.UniqueCode));

            advCommission.VerbaleLink = mLink;

            try
            {
                Manager.SaveOrUpdate<lm.Comol.Core.DomainModel.ModuleLink>(mLink);
                Manager.SaveOrUpdate<Advanced.Domain.AdvCommission>(advCommission);
                Manager.Commit();
            }
            catch (Exception ex)
            {
                Manager.RollBack();
                return false;
            }

            return true;
        }
        
        /// <summary>
        /// Verifica i permessi per il download dei verbali
        /// </summary>
        /// <param name="idCommission">Id commissione</param>
        /// <param name="idUser">Id utente</param>
        /// <param name="person">Utente corrente</param>
        /// <param name="idCommunity">Id comunità</param>
        /// <param name="idRole">Ruolo nella comunità</param>
        /// <returns>True se il verbale puo' essere scaricato</returns>
        protected override bool AllowDownloadVerbali(long idCommission, int idUser, lm.Comol.Core.DomainModel.litePerson person, int idCommunity, int idRole)
        {

            Advanced.Domain.AdvCommission advCommission = Manager.Get<Advanced.Domain.AdvCommission>(idCommission);
            
            //if (advCommission == null)
            //{
            //    return AllowDownloadIntegrazioni(idCommission, idUser, person, idCommunity, idRole);
            //}
                

            //Presidente
            if (advCommission.President != null && advCommission.President.Id == idUser)
                return true;

            //Segretario
            if (advCommission.Secretary != null && advCommission.Secretary.Id == idUser)
                return true;

            //Membro
            if (advCommission.Members != null && advCommission.Members.Any(m => m.Member != null && m.Member.Id == idUser))
                return true;

            //if (advCommission.Step == null)
            //    return false;

            //Sottomittori : no!
            //bool isSubmitter = Manager.GetAll<Advanced.Domain.AdvSubmissionToStep>(
            //    sts => sts.Step != null && sts.Step.Id == advCommission.Step.Id
            //    && sts.Submission != null &&
            //        (
            //        (sts.Submission.SubmittedBy != null && sts.Submission.SubmittedBy.Id == UC.CurrentUserID)
            //        || (sts.Submission.CreatedBy != null && sts.Submission.CreatedBy.Id == UC.CurrentUserID)
            //        )).Any();


            int CommunityId = 0;

            try
            {
                CommunityId = advCommission.Step.Call.Community.Id;
            }
            catch
            {

            }

            if (CommunityId <= 0)
                return false;
            //Gestori bando della comunità
            lm.Comol.Modules.CallForPapers.Domain.ModuleCallForPaper _module = CallForPaperServicePermission(idUser, CommunityId);
            bool allowManage = _module.CreateCallForPaper || _module.Administration || _module.ManageCallForPapers || _module.EditCallForPaper;
            return allowManage;

        }
        /// <summary>
        /// Sostituisce un membro di una commissione
        /// </summary>
        /// <param name="OldPersonId">Id persona membro corrente</param>
        /// <param name="NewPersonId">Id persona nuovo membro</param>
        /// <param name="CommissionId">Id commissione</param>
        /// <returns>True se la modifica avviene correttamente</returns>
        /// <remarks>L'associazione tra il membro precedente e la commissione viene persistita su tabella apposita</remarks>
        public long CommissionChangeMember(int OldPersonId, int NewPersonId, long CommissionId)
        {
            Advanced.Domain.AdvMember member = Manager.GetAll<Advanced.Domain.AdvMember>(m =>
                m.Member != null && m.Member.Id == OldPersonId && m.Commission != null && m.Commission.Id == CommissionId)
                .FirstOrDefault();

            if (member == null
                || member.Commission == null
                || member.Commission.President.Id != UC.CurrentUserID
                || member.Commission.Status != CommissionStatus.Locked
                || member.Commission.Members == null
                || !member.Commission.Members.Any())
                return 0;

            if (member.Commission.Members.Any(m => m.Member != null && m.Member.Id == NewPersonId))
            {
                return 0;
            }

            Core.DomainModel.litePerson NewMember = Manager.GetLitePerson(NewPersonId);
            if (NewMember == null)
                return 0;

            Advanced.Domain.AdvOldMember oldMember = new Advanced.Domain.AdvOldMember();
            oldMember.OldPersonId = (member.Member != null) ? member.Member.Id : 0;
            oldMember.MemberId = member.Id;
            oldMember.CreateMetaInfo(GetCurrentPerson(), UC.IpAddress, UC.ProxyIpAddress);

            member.Member = NewMember;

            member.UpdateMetaInfo(GetCurrentPerson(), UC.IpAddress, UC.ProxyIpAddress);


            if (!Manager.IsInTransaction())
                Manager.BeginTransaction();

            try
            {
                Manager.SaveOrUpdate<Advanced.Domain.AdvOldMember>(oldMember);
                Manager.SaveOrUpdate<Advanced.Domain.AdvMember>(member);
                Manager.Commit();
            }
            catch (Exception ex)
            {
                Manager.RollBack();
                return -1;
            }

            return member.Id;
        }
        /// <summary>
        /// Recupera le valutazioni di un valutatore
        /// </summary>
        /// <param name="idEvaluator">Id valutatore</param>
        /// <param name="CommissionId">Id commissione</param>
        /// <returns>lista dto Info valutazioni</returns>
        public List<Eval.dtoCommitteeEvaluationInfo> GetCommitteesInfoForEvaluator(long idEvaluator, long CommissionId)
        {

            List<Eval.dtoCommitteeEvaluationInfo> items = new List<Eval.dtoCommitteeEvaluationInfo>();
            Advanced.Domain.AdvCommission comm = Manager.Get<Advanced.Domain.AdvCommission>(CommissionId);

            Eval.dtoCommitteeEvaluationInfo itm = new Eval.dtoCommitteeEvaluationInfo();

            try
            {

                itm = new Eval.dtoCommitteeEvaluationInfo()
                {
                    IdCommittee = comm.Id,
                    Name = comm.Name,
                    DisplayOrder = 1,
                    Status = Eval.EvaluationStatus.None
                }
                ;

                var query = (from e in Manager.GetIQ<Eval.Evaluation>()
                             where e.Deleted == Core.DomainModel.BaseStatusDeleted.None
                             && e.AdvEvaluator != null
                             && e.AdvEvaluator.Id == idEvaluator
                             select e);

                itm.Status = GetCommitteeEvaluationsStatistics(CommissionId, query);
            }
            catch (Exception ex)
            {

            }

            items.Add(itm);
            return items.OrderBy(s => s.DisplayOrder).ThenBy(s => s.Name).ToList();
        }
        #endregion

        #region Criteria
        /// <summary>
        /// Aggiunge un criterio ad una commissione avanzata
        /// </summary>
        /// <param name="CommId">Id commissione</param>
        /// <param name="items">Elenco criteri</param>
        /// <returns>Lista criteri dopo l'inserimento</returns>
        public List<Eval.BaseCriterion> AddCriteriaAdv(
        Int64 CommId,
        List<lm.Comol.Modules.CallForPapers.Domain.Evaluation.dtoCriterion> items)
        {
            List<Eval.BaseCriterion> criteria = new List<Eval.BaseCriterion>();

            //SaveCommittees(idCall, committees, settings);


            try
            {
                Manager.BeginTransaction();

                Core.DomainModel.litePerson person = GetCurrentPerson(); // Manager.GetLitePerson(UC.CurrentUserID);

                //EvaluationCommittee committee = Manager.Get<EvaluationCommittee>(idCommittee);

                Advanced.Domain.AdvCommission advCommission = Manager.Get<Advanced.Domain.AdvCommission>(CommId);


                if (advCommission == null || person == null)
                {
                    return null;
                }

                int displayNumber = 1;
                foreach (lm.Comol.Modules.CallForPapers.Domain.Evaluation.dtoCriterion item in items)
                {
                    if (String.IsNullOrEmpty(item.Name))
                        item.Name = item.DisplayOrder.ToString();
                    Eval.BaseCriterion criterion = Manager.Get<Eval.BaseCriterion>(item.Id);
                    if (criterion == null)
                    {
                        switch (item.Type)
                        {
                            case Eval.CriterionType.Boolean:
                                criterion = new Eval.BoolCriterion();
                                break;
                            case Eval.CriterionType.Textual:
                                criterion = new Eval.TextualCriterion();
                                ((Eval.TextualCriterion)criterion).MaxLength = item.MaxLength;
                                break;
                            case Eval.CriterionType.StringRange:
                                criterion = new Eval.StringRangeCriterion();
                                ((Eval.StringRangeCriterion)criterion).MaxOption = item.MaxOption;
                                ((Eval.StringRangeCriterion)criterion).MinOption = item.MinOption;
                                break;
                            case Eval.CriterionType.IntegerRange:
                            case Eval.CriterionType.DecimalRange:
                                criterion = new Eval.NumericRangeCriterion(item.DecimalMinValue, item.DecimalMaxValue, item.Type);
                                break;
                            //case Eval.CriterionType.RatingScale:
                            //case Eval.CriterionType.RatingScaleFuzzy:
                            //    criterion = new DssCriterion();
                            //    criterion.Type = dto.Type;
                            //    ((DssCriterion)criterion).IsFuzzy = (dto.Type == CriterionType.RatingScaleFuzzy);
                            //    ((DssCriterion)criterion).IdRatingSet = dto.IdRatingSet;
                            //    break;
                            default:
                                criterion = new Eval.BaseCriterion();
                                criterion.Type = Eval.CriterionType.Textual;
                                break;
                        }
                    }
                    else
                    {
                        switch (item.Type)
                        {
                            case Eval.CriterionType.Boolean:
                                break;
                            case Eval.CriterionType.Textual:
                                ((Eval.TextualCriterion)criterion).MaxLength = item.MaxLength;
                                break;
                            case Eval.CriterionType.StringRange:
                                ((Eval.StringRangeCriterion)criterion).MaxOption = item.MaxOption;
                                ((Eval.StringRangeCriterion)criterion).MinOption = item.MinOption;
                                break;
                            case Eval.CriterionType.IntegerRange:
                            case Eval.CriterionType.DecimalRange:
                                ((Eval.NumericRangeCriterion)criterion).DecimalMinValue = item.DecimalMinValue;
                                ((Eval.NumericRangeCriterion)criterion).DecimalMaxValue = item.DecimalMaxValue;
                                criterion.Type = item.Type;
                                break;
                        }
                        criterion.UpdateMetaInfo(person, UC.IpAddress, UC.ProxyIpAddress);
                    }
                    criterion.Committee = null;
                    criterion.AdvCommitee = advCommission;

                    criterion.UseDss = false;

                    criterion.Name = item.Name;
                    criterion.Description = item.Description;
                    criterion.DisplayOrder = item.DisplayOrder;
                    criterion.CommentType = item.CommentType;
                    Manager.SaveOrUpdate(criterion);
                    if (item.Type == Eval.CriterionType.StringRange)
                    {
                        SaveOptions(person, ((Eval.StringRangeCriterion)criterion), item.Options);
                    }
                    displayNumber++;
                    criteria.Add(criterion);
                }
            }
            catch (Exception ex)
            {
                Manager.RollBack();
                criteria.Clear();
            }
            return criteria;
        }
        /// <summary>
        /// Rimuove un criterio
        /// </summary>
        /// <param name="CommId">Id commissione</param>
        /// <param name="CritId">Id criterio</param>
        /// <returns></returns>
        public bool CriteriaRemove(
            Int64 CommId,
            Int64 CritId)
        {

            Advanced.Domain.AdvCommission commission = Manager.Get<Advanced.Domain.AdvCommission>(CommId);

            if (commission == null || commission.Criteria == null)
            {
                return true;
            }


            Eval.BaseCriterion crit = commission.Criteria.FirstOrDefault(cr => cr.Id == CritId); //Manager.Get<Eval.BaseCriterion>(CritId);

            if (crit == null)
                return true;

            commission.Criteria.Remove(crit);
            commission.UpdateMetaInfo(GetCurrentPerson(), UC.IpAddress, UC.ProxyIpAddress);

            if (!Manager.IsInTransaction())
                Manager.BeginTransaction();

            try
            {
                Manager.SaveOrUpdate<Advanced.Domain.AdvCommission>(commission);
                //Manager.DeletePhysical<Eval.BaseCriterion>(crit);
                Manager.Commit();


            }
            catch (Exception ex)
            {
                Manager.RollBack();
                return false;
            }

            return true;
        }

        /// <summary>
        /// Salva le opzioni di un criterio
        /// </summary>
        /// <param name="person">Persona corrente</param>
        /// <param name="criterion">Criterio</param>
        /// <param name="items">Opzioni criterio</param>
        /// <returns></returns>
        private List<Eval.CriterionOption> SaveOptions(
            Core.DomainModel.litePerson person,
            Eval.StringRangeCriterion criterion,
            List<Eval.dtoCriterionOption> items)
        {
            List<Eval.CriterionOption> options = new List<Eval.CriterionOption>();
            try
            {
                if (criterion != null && person != null)
                {
                    Boolean isNew = false;
                    int displayNumber = 1;
                    foreach (Eval.dtoCriterionOption item in items)
                    {
                        if (String.IsNullOrEmpty(item.Name))
                            item.Name = item.DisplayOrder.ToString();
                        Eval.CriterionOption opt = Manager.Get<Eval.CriterionOption>(item.Id);
                        isNew = (opt == null);
                        if (isNew)
                        {
                            opt = new Eval.CriterionOption();
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

        /// <summary>
        /// Trasforma l'oggetto Base Criterio in un stoCriterionSummaryItem
        /// </summary>
        /// <param name="criterion">Criterio</param>
        /// <param name="count">Numero valutatori</param>
        /// <returns>dto Criterion Summary Item</returns>
        private Eval.dtoCriterionSummaryItem CriterionToCriterionSumdto(Eval.BaseCriterion criterion, int count)
        {
            Eval.dtoCriterionSummaryItem dto = new Eval.dtoCriterionSummaryItem();

            dto.Id = criterion.Id;
            dto.DisplayOrder = criterion.DisplayOrder;
            dto.Name = criterion.Name;
            dto.WeightSettings = new lm.Comol.Core.Dss.Domain.Templates.dtoItemWeightSettings();
            dto.MethodSettings = new lm.Comol.Core.Dss.Domain.Templates.dtoItemMethodSettings();

            if (criterion.AdvCommitee != null)
            {
                dto.IdCommittee = criterion.AdvCommitee.Id;

                //if (criterion.Committee.UseDss)
                //    MethodSettings = dtoItemMethodSettings.Create(criterion.Committee.MethodSettings, criterion.Committee.MethodSettings.UseManualWeights);
            }
            //if (criterion.UseDss)
            //    dto.WeightSettings = dtoItemWeightSettings.Create(criterion.WeightSettings, criterion.Committee.MethodSettings.UseManualWeights);

            dto.Deleted = Core.DomainModel.BaseStatusDeleted.None;
            dto.Description = criterion.Description;
            dto.CommentType = criterion.CommentType;
            dto.Type = criterion.Type;
            dto.UseDss = criterion.UseDss;
            switch (criterion.Type)
            {
                case Eval.CriterionType.IntegerRange:
                case Eval.CriterionType.DecimalRange:

                    Eval.NumericRangeCriterion numCrit = (Eval.NumericRangeCriterion)criterion;

                    dto.DecimalMinValue = numCrit.DecimalMinValue;
                    dto.DecimalMaxValue = numCrit.DecimalMaxValue;
                    break;

                case Eval.CriterionType.StringRange:
                    Eval.StringRangeCriterion strCrit = (Eval.StringRangeCriterion)criterion;

                    dto.MaxOption = strCrit.MaxOption;
                    dto.MinOption = strCrit.MinOption;
                    dto.Options = strCrit.Options
                        .Where(o => o.Deleted == Core.DomainModel.BaseStatusDeleted.None)
                        .OrderBy(o => o.DisplayOrder)
                        .ThenBy(o => o.Name)
                        .Select(o => dtoCritOptionCreate(o.Id,
                        o.Name,
                        o.ShortName,
                        o.Value,
                        o.DisplayOrder,
                        criterion.Id
                            ))
                        .ToList();

                    //.Select(o => new Eval.dtoCriterionOption(o, criterion.Id))
                    break;

                case Eval.CriterionType.Textual:
                    Eval.TextualCriterion txtcrit = (Eval.TextualCriterion)criterion;
                    dto.MaxLength = txtcrit.MaxLength;
                    break;

                    //case Eval.CriterionType.RatingScale:
                    //    dto.IsFuzzy = false;
                    //    dto.IdRatingSet = criterion.IdRatingSet;
                    //    dto.Options = criterion.Options.Where(o => o.Deleted == BaseStatusDeleted.None).OrderBy(o => o.DisplayOrder).ThenBy(o => o.Name).Select(o => new dtoCriterionOption(o, criterion.Id)).ToList();
                    //    break;
                    //case Eval.CriterionType.RatingScaleFuzzy:
                    //    dto.IsFuzzy = true;
                    //    dto.IdRatingSet = criterion.IdRatingSet;
                    //    dto.Options = criterion.Options.Where(o => o.Deleted == BaseStatusDeleted.None).OrderBy(o => o.DisplayOrder).ThenBy(o => o.Name).Select(o => new dtoCriterionOption(o, criterion.Id)).ToList();
                    //    break;
            }
            dto.CommentMaxLength = criterion.CommentMaxLength;


            dto.DisplayAs = Domain.displayAs.item;
            dto.Evaluations = new List<Eval.dtoCriterionValueSummaryItem>();
            dto.EvaluatorsCount = count;

            return dto;
        }
        /// <summary>
        /// crea un oggetto dtoCriterioOption
        /// </summary>
        /// <param name="id">Id criterio</param>
        /// <param name="name">Nome</param>
        /// <param name="shortName">Nome abbreviato</param>
        /// <param name="value">Valore</param>
        /// <param name="display">Valore visualizzato</param>
        /// <param name="idCriterion">Id criterio</param>
        /// <returns></returns>
        private Eval.dtoCriterionOption dtoCritOptionCreate(
            long id,
            String name,
            String shortName,
            Decimal value,
            long display,
            long idCriterion)
        {
            Eval.dtoCriterionOption dco = new Eval.dtoCriterionOption();

            dco.Id = id;
            dco.Name = name;
            dco.ShortName = shortName;
            dco.Value = value;
            dco.DisplayOrder = display;
            dco.Deleted = Core.DomainModel.BaseStatusDeleted.None;
            dco.IdCriterion = idCriterion;

            return dco;
        }
        /// <summary>
        /// Aggiunge un opzione ed un criterio
        /// </summary>
        /// <param name="idCriterion">Id criterio</param>
        /// <param name="CommissionId">Id commissione</param>
        /// <param name="name">Nome</param>
        /// <param name="value">Valore</param>
        /// <returns>Opzione criterio aggiunta</returns>
        public Eval.CriterionOption AddOptionToCriterion(
            long idCriterion,
            long CommissionId,
            String name, Decimal? value)
        {

            Eval.StringRangeCriterion criterion = Manager.Get<Eval.StringRangeCriterion>(idCriterion);

            Eval.CriterionOption option = null;

            if (criterion != null
                && criterion.AdvCommitee != null && criterion.AdvCommitee.Id == CommissionId)
            {

                try
                {
                    Manager.BeginTransaction();
                    Core.DomainModel.litePerson person = Manager.GetLitePerson(UC.CurrentUserID);

                    if (criterion != null && person != null)
                    {
                        option = new Eval.CriterionOption();
                        option.CreateMetaInfo(person, UC.IpAddress, UC.ProxyIpAddress);
                        option.DisplayOrder = (from s in Manager.GetIQ<Eval.CriterionOption>() where s.Criterion.Id == criterion.Id select s.DisplayOrder).Max() + 1;
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

            }

            return option;
        }
        /// <summary>
        /// Recupera il valore massimo di un opzione (indice)
        /// </summary>
        /// <param name="criterion">Criterio</param>
        /// <returns>Indice</returns>
        private Decimal GetOptionValue(Eval.StringRangeCriterion criterion)
        {
            List<Decimal> optionsValue = (from f in Manager.GetIQ<Eval.CriterionOption>()
                                          where f.Deleted == Core.DomainModel.BaseStatusDeleted.None
                                          && f.Criterion == criterion
                                          select f.Value).ToList();
            return (optionsValue.Count == 0) ? 0 : optionsValue.Max() + 1;
        }
        /// <summary>
        /// Cancellazione logica di un opzione di un criterio
        /// </summary>
        /// <param name="idOption">Id opzione</param>
        /// <param name="delete">Se cancellare l'opzione</param>
        /// <param name="outputIdCommittee">Id commissione criterio</param>
        /// <param name="outputIdCriterion">Id criterio opzione</param>
        /// <param name="outputIdOption">Id opzione</param>
        /// <returns></returns>
        public Boolean VirtualDeleteCriterionOption(long idOption,
            Boolean delete,
            ref long outputIdCommittee,
            ref long outputIdCriterion,
            ref long outputIdOption)
        {
            Boolean result = false;
            try
            {
                Core.DomainModel.litePerson person = Manager.GetLitePerson(UC.CurrentUserID);

                Eval.CriterionOption option = Manager.Get<Eval.CriterionOption>(idOption);
                if (option != null && option.Criterion != null)
                {
                    List<Eval.CriterionEvaluated> evaluatedCriteria = new List<Eval.CriterionEvaluated>();

                    Boolean withEvaluations = false;

                    if (option.Criterion.AdvCommitee != null)
                        outputIdCommittee = option.Criterion.AdvCommitee.Id;

                    outputIdCriterion = option.Criterion.Id;
                    outputIdOption = option.Id;

                    if (option.Criterion.AdvCommitee != null) // && option.Criterion.AdvCommitee.Id == )
                    {
                        evaluatedCriteria = (from c in Manager.GetIQ<Eval.CriterionEvaluated>()
                                             where c.Deleted == Core.DomainModel.BaseStatusDeleted.None && c.Criterion.Id == option.Criterion.Id
                                             select c).ToList();

                    }
                    if (evaluatedCriteria.Where(ec => ec.Evaluation.Status != Eval.EvaluationStatus.None
                        && ec.Evaluation.Status != Eval.EvaluationStatus.Evaluating).Any())
                    {
                        //throw new EvaluationStarted();
                        return false;
                    }
                    else
                    {
                        Manager.BeginTransaction();
                        option.UpdateMetaInfo(person, UC.IpAddress, UC.ProxyIpAddress);
                        option.Deleted = delete ? Core.DomainModel.BaseStatusDeleted.Manual : Core.DomainModel.BaseStatusDeleted.None;
                        if (delete)
                        {
                            var query = (from s in Manager.GetIQ<Domain.FieldOption>()
                                         where s.Field != null && s.Field.Id == option.Criterion.Id
                                         && s.Deleted == Core.DomainModel.BaseStatusDeleted.None
                                         && s.Id != option.Id
                                         select s);

                            outputIdOption = (from s in query where s.DisplayOrder <= option.DisplayOrder orderby s.DisplayOrder descending select s.Id).FirstOrDefault();

                            if (outputIdOption == 0)
                                outputIdOption = (from s in query where s.DisplayOrder > option.DisplayOrder orderby s.DisplayOrder select s.Id).FirstOrDefault();

                        }
                        Manager.SaveOrUpdate(option);
                        if (evaluatedCriteria.Any())
                        {
                            foreach (var ev in evaluatedCriteria.GroupBy(ec => ec.Evaluation))
                            {
                                List<Eval.dtoCriterionEvaluated> eCriteria = GetEvaluationCriteria(ev.Key, ev.ToList());
                                if (eCriteria.Where(c => c.IsValidForCriterionSaving).Any())
                                {
                                    ev.Key.AverageRating = (double)(from c in eCriteria where c.IsValidForCriterionSaving select c.DecimalValue).Average();
                                    ev.Key.SumRating = (double)(from c in eCriteria where c.IsValidForCriterionSaving select c.DecimalValue).Sum();
                                }
                                else
                                {
                                    ev.Key.AverageRating = 0;
                                    ev.Key.SumRating = 0;
                                }
                                Manager.SaveOrUpdate(ev.Key);
                            }


                            foreach (Eval.CriterionEvaluated cEvaluated in evaluatedCriteria)
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
            //catch (SubmissionLinked ex)
            //{
            //    throw ex;
            //}
            catch (Exception ex)
            {
                Manager.RollBack();
            }
            return result;
        }
        /// <summary>
        /// Recupera i criteri di una valutazione
        /// </summary>
        /// <param name="evaluation">Valutazione</param>
        /// <param name="removeValues">Criteri da nascondere</param>
        /// <returns>Lista criteri valutati</returns>
        private List<Eval.dtoCriterionEvaluated> GetEvaluationCriteria(Eval.Evaluation evaluation, List<Eval.CriterionEvaluated> removeValues)
        {
            List<Eval.dtoCriterionEvaluated> cValues = new List<Eval.dtoCriterionEvaluated>();
            try
            {
                if (evaluation != null)
                {
                    List<lm.Comol.Modules.CallForPapers.Domain.Evaluation.dtoCriterion> criteria = (from bc in Manager.GetIQ<Eval.BaseCriterion>()
                                                                                                    where bc.AdvCommitee != null
                                                                                                    && bc.AdvCommitee.Id == evaluation.AdvCommission.Id
                                                                                                    && bc.Deleted == Core.DomainModel.BaseStatusDeleted.None
                                                                                                    select bc).ToList().Select(bc => new lm.Comol.Modules.CallForPapers.Domain.Evaluation.dtoCriterion(bc)).ToList();
                    List<long> idToRemove = removeValues.Select(i => i.Id).ToList();
                    List<Eval.CriterionEvaluated> values = (from cv in Manager.GetIQ<Eval.CriterionEvaluated>()
                                                            where cv.Evaluation == evaluation
                                                            && cv.Deleted == Core.DomainModel.BaseStatusDeleted.None && !idToRemove.Contains(cv.Id)
                                                            select cv).ToList();

                    foreach (Eval.dtoCriterion criterion in criteria)
                    {
                        cValues.Add(new Eval.dtoCriterionEvaluated(criterion, values.Where(v => v.Criterion.Id == criterion.Id).FirstOrDefault()));
                    }
                }
            }
            catch (Exception ex)
            {
                cValues = new List<Eval.dtoCriterionEvaluated>();
            }
            return cValues;
        }

        #endregion
        
        #region Permission
        /// <summary>
        /// Permessi utente all'interno della commissione
        /// </summary>
        /// <param name="CommId">ID commissione</param>
        /// <returns></returns>
        public CommissionPermission CommissionGetPermission(Int64 CommId)
        {
            return CommissionGetPermission(Manager.Get<Advanced.Domain.AdvCommission>(CommId));
        }
        /// <summary>
        /// Permessi utente all'interno della commissione
        /// </summary>
        /// <param name="Comm">Oggetto commissione</param>
        /// <returns></returns>
        public CommissionPermission CommissionGetPermission(Advanced.Domain.AdvCommission Comm)
        {
            if (Comm == null)
                return CommissionPermission.None;


            CommissionPermission permission = CommissionPermission.None;

            Int64 cUid = UC.CurrentUserID;

            if (Comm.President.Id == cUid)
            {
                permission |= CommissionPermission.View
                    // | CommissionPermission.ChangeStatus 
                    | CommissionPermission.UploadVerbale;
            }

            if (Comm.Secretary.Id == cUid)
            {
                permission |= CommissionPermission.View
                    | CommissionPermission.Edit
                    | CommissionPermission.UploadVerbale
                    | CommissionPermission.RequestIntegration;
            }


            if (Comm.Members.Any(m => m.Id == cUid))
            {
                if (Comm.Status != CommissionStatus.Draft)
                    permission |= CommissionPermission.View;

                permission |= CommissionPermission.Evaluate;
            }

            return permission;
        }

        /// <summary>
        /// Check di un permesso a livello di commissione
        /// </summary>
        /// <param name="actual">Permesso corrente</param>
        /// <param name="required">Premesso da verificare</param>
        /// <returns></returns>
        public static bool HasCommissionPermission(CommissionPermission actual, CommissionPermission required)
        {
            return ((required & actual) == actual);
        }

        /// <summary>
        /// Verifica se una commissione puo' essere avviata
        /// </summary>
        /// <param name="callId"></param>
        /// <returns></returns>
        public bool CommissionAdvanceCanSet(Int64 callId)
        {
            CallForPapers.Domain.CallForPaper call = Manager.Get<Domain.CallForPaper>(callId);

            if (call == null)
                return false;

            return CommissionAdvanceCanSet(call);
        }
        /// <summary>
        /// Verifica se una commissione puo' essere avviata
        /// </summary>
        /// <param name="call"></param>
        /// <returns></returns>
        public bool CommissionAdvanceCanSet(CallForPapers.Domain.CallForPaper call)
        {
            //CallForPapers.Domain.CallForPaper call = Manager.Get<Domain.CallForPaper>(callId);

            if (call == null)
                return false;


            if (call.Status == Domain.CallForPaperStatus.Draft || call.Status == Domain.CallForPaperStatus.None)
                return true;

            bool hasEval = (from Eval.Evaluation eval in Manager.GetIQ<Eval.Evaluation>()
                            where eval.Call != null && eval.Call.Id == call.Id
                            select eval.Id).Any();

            bool hasStepStarted = (from Advanced.Domain.AdvCommission comm in Manager.GetIQ<Advanced.Domain.AdvCommission>()
                                   where comm.Call != null && comm.Call.Id == call.Id && comm.Status != CommissionStatus.Draft
                                   select comm.Id).Any();

            return !hasEval && !hasStepStarted;
        }

       
        /// <summary>
        /// Premessi valutatori
        /// </summary>
        /// <param name="callId"></param>
        /// <param name="commissionId"></param>
        /// <returns></returns>
        public EvaluationAdvPermission EvaluationAdvPermission(long callId, long commissionId)
        {

            Advanced.Domain.AdvCommission Comm;

            if (commissionId == 0)
            {
                Comm =
                    (from Advanced.Domain.AdvCommission c in Manager.GetIQ<Advanced.Domain.AdvCommission>()
                     where c.Call != null && c.Call.Id == callId
                     && c.Step != null && c.Step.Type == StepType.validation
                     select c).FirstOrDefault();
            }
            else
            {
                Comm =
                    (from Advanced.Domain.AdvCommission c in Manager.GetIQ<Advanced.Domain.AdvCommission>()
                     where c.Call != null && c.Call.Id == callId
                     && c.Id == commissionId
                     select c).FirstOrDefault();
            }
            if (Comm == null)
                return Advanced.EvaluationAdvPermission.None;

            if (Comm.Status == CommissionStatus.Draft)
                return Advanced.EvaluationAdvPermission.None; //Permessi di sistema (creatore/manager bandi)

            Advanced.EvaluationAdvPermission permission = Advanced.EvaluationAdvPermission.None;

            if (Comm.Status == CommissionStatus.Closed)
                permission |= Advanced.EvaluationAdvPermission.View;


            if (
                Comm.Members.Any(m => m.Member != null && m.Member.Id == UC.CurrentUserID)
                )
            {
                switch (Comm.Status)
                {
                    case CommissionStatus.Draft:
                        break;
                    case CommissionStatus.ViewSubmission:
                        permission |= Advanced.EvaluationAdvPermission.View;
                        break;
                    case CommissionStatus.Started:
                        permission |= Advanced.EvaluationAdvPermission.Evaluate;
                        break;
                    case CommissionStatus.Locked:
                        permission |= Advanced.EvaluationAdvPermission.View;
                        break;
                    case CommissionStatus.Closed:
                        permission |= Advanced.EvaluationAdvPermission.View;
                        break;
                }
            }

            if ((Comm.President != null && Comm.President.Id == UC.CurrentUserID)
                || (Comm.Secretary != null && Comm.Secretary.Id == UC.CurrentUserID))
            {
                switch (Comm.Status)
                {
                    case CommissionStatus.Draft:
                        permission |= Advanced.EvaluationAdvPermission.View;
                        break;
                    case CommissionStatus.ViewSubmission:
                        permission |= Advanced.EvaluationAdvPermission.View;
                        break;
                    case CommissionStatus.Started:
                        permission |= Advanced.EvaluationAdvPermission.Evaluate;
                        break;
                    case CommissionStatus.Locked:
                        permission |= Advanced.EvaluationAdvPermission.View;
                        break;
                    case CommissionStatus.Closed:
                        permission |= Advanced.EvaluationAdvPermission.View;
                        break;
                }
            }

            return permission;

        }

        /// <summary>
        /// Verifica se un utente appartiene ad una commissione
        /// </summary>
        /// <param name="userId">Id utente</param>
        /// <param name="CallId">Id Call</param>
        /// <returns></returns>
        public bool UserIsInAdvanceEvaluationCommission(int userId, long CallId)
        {
            bool isInCommission = (from Advanced.Domain.AdvCommission comm in Manager.GetIQ<Advanced.Domain.AdvCommission>()
                                   where comm.Call != null && comm.Call.Id == CallId
                                   && (
                                       (comm.President != null && comm.President.Id == userId)
                                       || (comm.Secretary != null && comm.Secretary.Id == userId)
                                       || comm.Members != null && comm.Members.Any(m => m.Member != null && m.Member.Id == userId)
                                       )
                                   select comm.Id
                   ).Skip(0).Take(1).ToList().Any();

            return isInCommission;
        }
        
        /// <summary>
        /// Verifica se l'utente ha permessi a livello di comunità (Manager)
        /// </summary>
        /// <param name="CommunityId">Id comunità</param>
        /// <param name="idUser">Id utente</param>
        /// <returns></returns>
        public bool UserIsCallManager(int CommunityId, int idUser)
        {
            if (CommunityId <= 0)
                return false;
            //Gestori bando della comunità
            lm.Comol.Modules.CallForPapers.Domain.ModuleCallForPaper _module = CallForPaperServicePermission(idUser, CommunityId);
            bool allowManage = _module.CreateCallForPaper || _module.Administration || _module.ManageCallForPapers || _module.EditCallForPaper;
            return allowManage;
        }
        
        /// <summary>
        /// Verifica se un utente è presidente della commissione
        /// </summary>
        /// <param name="CommissionId">Id Commissione</param>
        /// <param name="UserId">Id Utente</param>
        /// <returns></returns>
        public bool CommissionUserIsPresident(Int64 CommissionId, int UserId)
        {
            Advanced.Domain.AdvCommission comm = Manager.Get<Advanced.Domain.AdvCommission>(CommissionId);

            if (comm != null &&
                ((comm.President != null && comm.President.Id == UserId)
                || comm.Members != null && comm.Members.Any(m => m.Member != null && m.IsPresident && m.Member.Id == UserId)
                ))
                return true;


            return false;
        }


        public bool CommissionUserIsPresidentOrSegretaryInMaster(Int64 CommissionId, int UserId)
        {
            Advanced.Domain.AdvCommission comm = Manager.Get<Advanced.Domain.AdvCommission>(CommissionId);

            if (comm != null && comm.Step != null)
            {
                Advanced.Domain.AdvCommission mainComm = comm.Step.Commissions.FirstOrDefault(c => c.IsMaster);

                if (mainComm != null)
                {
                    if(mainComm.President.Id == UserId || mainComm.Secretary.Id == UserId)
                    {
                        return true;
                    }
                }
            }

            return false;
        }
        
        /// <summary>
        /// Verifica se un utente è segretario della commissione
        /// </summary>
        /// <param name="CommissionId">Id Commissione</param>
        /// <param name="UserId">Id Utente</param>
        /// <returns></returns>
        public bool CommissionUserIsSecretary(Int64 CommissionId, int UserId)
        {
            Advanced.Domain.AdvCommission comm = Manager.Get<Advanced.Domain.AdvCommission>(CommissionId);

            if (comm != null &&
                (comm.Secretary != null && comm.Secretary.Id == UserId)
                )
                return true;


            return false;
        }


        /// <summary>
        /// Verifica se l'utente ha una sottomissione in valutazione allo step indicato.
        /// </summary>
        /// <param name="StepId">Id Step</param>
        /// <param name="UserSubmitterId">Id Sottomittore</param>
        /// <returns></returns>
        public bool StepIsSubmitter(long StepId, int UserSubmitterId)
        {
            Advanced.Domain.AdvStep step = Manager.Get<Advanced.Domain.AdvStep>(StepId);

            if (step == null || step.Call == null)
                return false;

            if (step.Type == StepType.validation)
            {
                return CallUserHasSubmission(step.Call.Id, UserSubmitterId);
            }

            return CallStepHasSubmission(step.Id, UserSubmitterId);
        }

        /// <summary>
        /// Permsso a livello di step, esclusi i sottomittori
        /// </summary>
        /// <param name="StepId">Id Step</param>
        /// <param name="UserId">Id Utente</param>
        /// <returns></returns>
        public Advanced.GenericStepPermission StepGetPermissionNoSubmitter(long StepId, int UserId)
        {
            if (StepId <= 0 || UserId <= 0)
                return GenericStepPermission.none;

            Advanced.Domain.AdvStep Step = Manager.Get<Advanced.Domain.AdvStep>(StepId);

            return StepGetPermissionNoSubmitter(Step, UserId);
        }

        /// <summary>
        /// Permsso a livello di step, esclusi i sottomittori
        /// </summary>
        /// <param name="Step">Step</param>
        /// <param name="UserId">Id Utente</param>
        /// <returns></returns>
        public Advanced.GenericStepPermission StepGetPermissionNoSubmitter(Advanced.Domain.AdvStep Step, int UserId)
        {
            if (Step == null || UserId <= 0 || Step.Commissions == null || !Step.Commissions.Any())
                return GenericStepPermission.none;



            Advanced.GenericStepPermission StepPermission = GenericStepPermission.none;

            if (Step.Commissions.Any(cm => (cm.President != null && cm.President.Id == UserId && cm.IsMaster)))
            {
                StepPermission |= GenericStepPermission.MainPresident;
            }

            if (Step.Commissions.Any(cm => (cm.Secretary != null && cm.Secretary.Id == UserId && cm.IsMaster)))
            {
                StepPermission |= GenericStepPermission.MainSecretary;
            }

            if (Step.Commissions.Any(cm => (cm.President != null && cm.President.Id == UserId)))
            {
                StepPermission |= GenericStepPermission.President;
            }

            if (Step.Commissions.Any(cm => (cm.Secretary != null && cm.Secretary.Id == UserId)))
            {
                StepPermission |= GenericStepPermission.Secretary;
            }

            if (Step.Commissions.Any(cm => (cm.Members.Any(m => m.Member != null && m.Deleted == Core.DomainModel.BaseStatusDeleted.None
                    && m.Member.Id == UserId))))
            {
                StepPermission |= GenericStepPermission.Member;
            }

            return StepPermission;

        }
        
        /// <summary>
        /// VErifica se l'utente è in una commissione
        /// </summary>
        /// <param name="CallId">Id Bando</param>
        /// <param name="UserId">Id utente</param>
        /// <returns></returns>
        public bool UserIsInCallCommission(long CallId, int UserId)
        {
            return Manager.GetAll<Advanced.Domain.AdvCommission>(c =>
            c.Call != null && c.Call.Id == CallId
            && c.IsInCommission(UserId)
            ).Any();
        }
        /// <summary>
        /// Verifica se l'utente è in una commissione in cui puo' visualizzare le sottomissioni
        /// </summary>
        /// <param name="CallId">Id bando</param>
        /// <param name="UserId">Id utente</param>
        /// <returns></returns>
        public bool UserIsInCallCommissionViewSubmission(long CallId, int UserId)
        {
            return Manager.GetAll<Advanced.Domain.AdvCommission>(c =>
            c.Call != null && c.Call.Id == CallId
            && (c.Status != CommissionStatus.Draft && c.IsInCommission(UserId))
            ).Any();
        }


        public bool UserIsInCallCommissionPresidentOrSecreatary(long CallId, int UserId)
        {
            return Manager.GetAll<Advanced.Domain.AdvCommission>(c =>
            c.Call != null && c.Call.Id == CallId
            && (c.Status != CommissionStatus.Draft && c.IsInCommissionAsSecretaryOrPresident(UserId))
            ).Any();

        }
    #endregion

    #region Evaluation

    /// <summary>
    /// Riepilogo valutazioni Commissione
    /// </summary>
    /// <param name="idCall">Id Bando</param>
    /// <param name="idMember">Id membro</param>
    /// <param name="idCommission">Id Commissione</param>
    /// <returns></returns>
    public List<Eval.dtoCommitteeEvaluationsInfo> GetCommitteesEvaluationInfoAdv(
            long idCall, 
            long idMember, 
            long idCommission,
            ref Domain.EvaluationType evType) //, long idEvaluator)
        {

            if (idMember == 0)
            {
                idMember = MemberIdGet(idCommission);
            }


            List<Eval.dtoCommitteeEvaluationsInfo> items = new List<Eval.dtoCommitteeEvaluationsInfo>();

            try
            {
                Advanced.Domain.AdvCommission commission = Manager.Get<Advanced.Domain.AdvCommission>(idCommission);


                if(commission != null)
                {
                    if (commission.EvalType == EvalType.Average)
                        evType = Domain.EvaluationType.Average;
                    else
                        evType = Domain.EvaluationType.Sum;
                }


                items.Add(
                    new Eval.dtoCommitteeEvaluationsInfo()
                    {
                        IdCommittee = commission.Id,
                        Name = commission.Name,
                        Description = commission.Description,
                        DisplayOrder = 1,
                        IdEvaluator = idMember,
                        isFuzzy = false
                    }
                    );

                //items = (from s in Manager.GetIQ<Advanced.Domain.AdvCommission>()
                //         where s.Deleted == Core.DomainModel.BaseStatusDeleted.None 
                //         && s.Id == idCommission
                //         select new Eval.dtoCommitteeEvaluationsInfo()
                //         {
                //             IdCommittee = s.Id,
                //             Name = s.Name,
                //             Description = s.Description,
                //             DisplayOrder = 1,
                //             IdEvaluator = idMember,
                //             isFuzzy = false
                //         }).ToList();

                var query = (from e in Manager.GetIQ<Eval.Evaluation>()
                             where e.Call.Id == idCall
                             && e.Deleted == Core.DomainModel.BaseStatusDeleted.None
                             && e.AdvEvaluator != null && e.AdvEvaluator.Id == idMember
                             select e);

                items.ForEach(i => i.Counters = GetEvaluatorStatistics(idMember, i.IdCommittee, query));
            }
            catch (Exception ex)
            {

            }
            return items.OrderBy(s => s.DisplayOrder).ThenBy(s => s.Name).ToList();
        }

        /// <summary>
        /// Statistiche valutazioni commissione
        /// </summary>
        /// <param name="type">Tipo valutazione</param>
        /// <param name="filters">Filtri valutazione</param>
        /// <param name="anonymousDisplayName">Nome utente anonimo</param>
        /// <param name="unknownUserName">Nome utente sconosciuto</param>
        /// <param name="idCommittee">Id Commissione</param>
        /// <param name="idEvaluator">Id valutatore</param>
        /// <param name="applyFilters">Applica filtri</param>
        /// <returns></returns>
        public Eval.dtoEvaluatorCommitteeStatistic GetEvaluatorCommitteeStatistics(
            Domain.EvaluationType type,
            Eval.dtoEvaluationsFilters filters,
            String anonymousDisplayName,
            String unknownUserName,
            long idCommittee,
            long idEvaluator = 0,
            Boolean applyFilters = true)
        {

            Advanced.Domain.AdvCommission advComm = Manager.Get<Advanced.Domain.AdvCommission>(idCommittee);
            if (advComm == null)
                return null;


            if (idEvaluator == 0)
            {
                Advanced.Domain.AdvMember mem = advComm.Members.FirstOrDefault(m => m.Member != null && m.Member.Id == UC.CurrentUserID);
                if (mem != null)
                    idEvaluator = mem.Id;
            }

            return GetEvaluatorStatistics(
                type,
                filters,
                anonymousDisplayName,
                unknownUserName,
                advComm,
                idEvaluator,
                applyFilters);
        }

        /// <summary>
        /// Statistiche singolo valutatore
        /// </summary>
        /// <param name="type">Tipo valutazione</param>
        /// <param name="filters">Filtri</param>
        /// <param name="anonymousDisplayName">Nome utente anonimo</param>
        /// <param name="unknownUserName">Nome utente sconosciuto</param>
        /// <param name="commission">Commissione</param>
        /// <param name="idEvaluator">Id Valuatore</param>
        /// <param name="applyFilters">Applica filtri</param>
        /// <returns></returns>
        private Eval.dtoEvaluatorCommitteeStatistic GetEvaluatorStatistics(
            Domain.EvaluationType type,
            Eval.dtoEvaluationsFilters filters,
            String anonymousDisplayName,
            String unknownUserName,
            Advanced.Domain.AdvCommission commission,
            long idEvaluator,
            Boolean applyFilters = true)
        {
            Eval.dtoEvaluatorCommitteeStatistic statistic = null;
            try
            {
                long index = 1;
                if (commission != null)
                {
                    //List<DssCallEvaluation> dssEvaluations = DssRatingGetValues(committee.IdCall, committee.Id, idEvaluator);
                    statistic = new Eval.dtoEvaluatorCommitteeStatistic();
                    statistic.IdCommittee = commission.Id;
                    //statistic.IdAdvCommission = commission.Id;
                    statistic.Name = commission.Name;
                    statistic.IsFuzzy = false;//committee.UseDss && committee.MethodSettings.IsFuzzyMethod;
                    statistic.Description = commission.Description;
                    statistic.Criteria = (commission.Criteria != null) ?
                        commission.Criteria
                        .OrderBy(cr => cr.DisplayOrder)
                        .ThenBy(cr => cr.Name)
                        .Select(cr => new Eval.dtoCriterion(cr)).ToList()
                        : new List<Eval.dtoCriterion>();

                    statistic.IdEvaluator = idEvaluator;

                    IList<CallForPapers.Domain.Evaluation.Evaluation> eval =
                        Manager.GetAll<CallForPapers.Domain.Evaluation.Evaluation>(
                            ev => ev.AdvCommission != null
                            && ev.AdvCommission.Id == commission.Id);

                    statistic.Evaluations =
                        (eval != null) ?
                            eval
                            .Where(e => e.AdvEvaluator.Id == idEvaluator)
                            .Select(e => GetdtoEvalFromEval(
                                e,
                                anonymousDisplayName,
                                unknownUserName,
                                statistic.Criteria))
                                .OrderByDescending(e => e.SumRating).ThenBy(e => e.DisplayName).ToList()
                            : new List<lm.Comol.Modules.CallForPapers.Domain.Evaluation.dtoEvaluation>();

                    if (statistic.Evaluations != null)
                    {
                        statistic.Evaluations = FilterAndReorderEvaluations(type, filters, statistic.Evaluations, applyFilters);

                        List<long> idEvaluations = statistic.Evaluations.Select(e => e.Id).ToList();

                        List<Eval.Evaluation> evaluations = eval.Where(e => idEvaluations.Contains(e.Id)).ToList();

                        List<Eval.Export.expEvaluation> evaulations = Manager.GetAll<Eval.Export.expEvaluation>(
                            exev => idEvaluations.Contains(exev.Id)
                            ).ToList();

                        if (commission.Criteria != null)
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

        /// <summary>
        /// Converte una valutazione in un dtoValutazione
        /// </summary>
        /// <param name="evaluation">Valutazione</param>
        /// <param name="anonymousDisplayName">Nome utente anonimo</param>
        /// <param name="unknownUserName">Nome utente sconosciuto</param>
        /// <param name="criteria">Criteri valutazione</param>
        /// <returns></returns>
        private Eval.dtoEvaluation GetdtoEvalFromEval(
            Eval.Evaluation evaluation,
            String anonymousDisplayName,
            String unknownUserName,
            List<Eval.dtoCriterion> criteria)

        {
            Eval.dtoEvaluation dtoEval = new Eval.dtoEvaluation();

            if (evaluation.Submission != null && evaluation.Submission.SubmittedBy != null)
            {
                dtoEval.DisplayName = evaluation.Submission.SubmittedBy.SurnameAndName;
                dtoEval.Anonymous = false;
            }
            else
            {
                dtoEval.Anonymous = true;
            }

            dtoEval.Id = evaluation.Id;
            dtoEval.ModifiedOn = evaluation.ModifiedOn;
            dtoEval.EvaluationStartedOn = evaluation.EvaluationStartedOn;
            dtoEval.EvaluatedOn = evaluation.EvaluatedOn;
            dtoEval.AverageRating = evaluation.AverageRating;
            dtoEval.SumRating = evaluation.SumRating;

            dtoEval.BoolRating = evaluation.BoolRating;
            dtoEval.IsPassed = evaluation.IsPassed;

            dtoEval.DssRating = new Eval.dtoDssRating() { IsValid = false };
            //dtoEval.DssRating.IsValid = evaluation.UseDss && (dssEvaluation != null && dssEvaluation.IsValid);
            //dtoEval.DssRating.IsCompleted = true;//evaluation.UseDss && (dssEvaluation != null && dssEvaluation.IsCompleted);
            //dtoEval.DssRating.Ranking = evaluation.DssRanking;
            //dtoEval.DssRating.Value = evaluation.DssValue;
            //dtoEval.DssRating.ValueFuzzy = evaluation.DssValueFuzzy;
            //dtoEval.DssRating.IsFuzzy = false;
            dtoEval.Comment = evaluation.Comment;
            dtoEval.Criteria = new List<Eval.dtoCriterionEvaluated>();

            dtoEval.IdCommittee = (evaluation.AdvCommission != null) ? evaluation.AdvCommission.Id : 0;
            dtoEval.IdEvaluator = (evaluation.AdvEvaluator != null) ? evaluation.AdvEvaluator.Id : 0;

            dtoEval.IdSubmission = (evaluation.Submission != null) ? evaluation.Submission.Id : 0;


            dtoEval.Deleted = evaluation.Deleted;

            dtoEval.Evaluated = evaluation.Evaluated;
            dtoEval.Status = evaluation.Status;

            dtoEval.Anonymous = (evaluation.Submission == null || evaluation.Submission.isAnonymous);
            dtoEval.DisplayName = (dtoEval.Anonymous) ? anonymousDisplayName : ((evaluation.Submission.Owner == null) ? unknownUserName : evaluation.Submission.Owner.SurnameAndName);

            dtoEval.SubmittedOn = (evaluation.Submission != null) ? evaluation.Submission.SubmittedOn : null;
            dtoEval.EvaluatorName = (evaluation.Evaluator != null && evaluation.Evaluator.Person != null) ? evaluation.Evaluator.Person.SurnameAndName : unknownUserName;
            dtoEval.IdSubmitterType = (evaluation.Submission != null && evaluation.Submission.Type != null) ? evaluation.Submission.Type.Id : 0;




            if (criteria != null)
                dtoEval.Criteria = criteria.Select(c => new Eval.dtoCriterionEvaluated(c)).ToList();

            dtoEval.SubmitterType = (evaluation.Submission != null) ? evaluation.Submission.Type.Name : "";

            return dtoEval;
        }

        /// <summary>
        /// Filtra e riordina le valutazioni
        /// </summary>
        /// <param name="type">Tipo valutazione</param>
        /// <param name="filters">Fitlri</param>
        /// <param name="evaluations">Valutazioni</param>
        /// <param name="applyFilters">Applica filtri</param>
        /// <returns></returns>
        private List<Domain.Evaluation.dtoEvaluation> FilterAndReorderEvaluations(
            Domain.EvaluationType type,
            Eval.dtoEvaluationsFilters filters,
            List<Domain.Evaluation.dtoEvaluation> evaluations,
            Boolean applyFilters = true)
        {
            long index = 1;
            switch (type)
            {
                case Domain.EvaluationType.Average:
                    evaluations.OrderByDescending(e => e.AverageRating).ThenBy(e => e.DisplayName).ToList().ForEach(e => e.Position = index++);
                    break;
                case Domain.EvaluationType.Dss:
                    evaluations.OrderByDescending(e => e.DssRating.Ranking).ThenBy(e => e.DisplayName).ToList().ForEach(e => e.Position = index++);
                    break;
                default:
                    evaluations.OrderByDescending(e => e.SumRating).ThenBy(e => e.DisplayName).ToList().ForEach(e => e.Position = index++);
                    break;
            }


            var query = (from e in evaluations select e);
            if (applyFilters)
            {
                query = query.Where(e => (filters.IdSubmitterType == -1 || filters.IdSubmitterType == e.IdSubmitterType));

                if (!string.IsNullOrEmpty(filters.SearchForName) && !String.IsNullOrEmpty(filters.SearchForName.Trim()))
                {
                    List<long> idEvaluations = new List<long>();
                    List<String> searchName = filters.SearchForName.ToLower().Split(' ').ToList();
                    searchName.ForEach(s => query.Where(i => i.DisplayName.ToLower().Contains(s)).ToList().ForEach(t => idEvaluations.Add(t.Id)));
                    query = query.Where(e => idEvaluations.Contains(e.Id)).ToList();
                }
                switch (filters.Status)
                {
                    case Eval.EvaluationFilterStatus.AllValid:
                        query = query.Where(e => e.Status != Eval.EvaluationStatus.Invalidated && e.Status != Eval.EvaluationStatus.EvaluatorReplacement).ToList();
                        break;
                    case Eval.EvaluationFilterStatus.Evaluated:
                        query = query.Where(e => e.Status == Eval.EvaluationStatus.Evaluated).ToList();
                        break;
                    case Eval.EvaluationFilterStatus.Evaluating:
                        query = query.Where(e => e.Status == Eval.EvaluationStatus.Evaluating).ToList();
                        break;
                    case Eval.EvaluationFilterStatus.EvaluatorReplacement:
                        query = query.Where(e => e.Status == Eval.EvaluationStatus.EvaluatorReplacement).ToList();
                        break;
                    case Eval.EvaluationFilterStatus.Invalidated:
                        query = query.Where(e => e.Status == Eval.EvaluationStatus.Invalidated).ToList();
                        break;
                    case Eval.EvaluationFilterStatus.None:
                        query = query.Where(e => e.Status == Eval.EvaluationStatus.None).ToList();
                        break;
                }
            }
            Domain.SubmissionsOrder orderBy = filters.OrderBy;
            Boolean ascending = filters.Ascending;
            if (orderBy == Domain.SubmissionsOrder.None)
            {
                ascending = true;
            }

            #region "order"
            switch (orderBy)
            {
                case Domain.SubmissionsOrder.BySubmittedOn:
                    if (ascending)
                        query = query.OrderBy(s => s.SubmittedOn);
                    else
                        query = query.OrderByDescending(s => s.SubmittedOn);
                    break;
                case Domain.SubmissionsOrder.ByDate:
                    if (ascending)
                        query = query.OrderBy(r => r.ModifiedOn);
                    else
                        query = query.OrderByDescending(r => r.ModifiedOn);
                    break;
                case Domain.SubmissionsOrder.ByEvaluationStatus:
                    if (ascending)
                        query = query.OrderBy(r => filters.TranslationsEvaluationStatus[r.Status]);
                    else
                        query = query.OrderByDescending(r => filters.TranslationsEvaluationStatus[r.Status]);
                    break;
                case Domain.SubmissionsOrder.ByType:
                    if (ascending)
                        query = query.OrderBy(s => s.SubmitterType);
                    else
                        query = query.OrderByDescending(s => s.SubmitterType);
                    break;
                case Domain.SubmissionsOrder.ByUser:
                    if (ascending)
                        query = query.OrderBy(s => s.DisplayName);
                    else
                        query = query.OrderByDescending(s => s.DisplayName);
                    break;
                case Domain.SubmissionsOrder.ByEvaluationPoints:
                    if (ascending)
                    {
                        switch (type)
                        {
                            case Domain.EvaluationType.Average:
                                query = query.OrderBy(s => s.AverageRating).ThenBy(e => e.DisplayName);
                                break;
                            case Domain.EvaluationType.Dss:
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
                            case Domain.EvaluationType.Average:
                                query = query.OrderByDescending(s => s.AverageRating).ThenBy(e => e.DisplayName);
                                break;
                            case Domain.EvaluationType.Dss:
                                query = query.OrderByDescending(s => s.DssRating.Value).ThenBy(e => e.DisplayName);
                                break;
                            default:
                                query = query.OrderByDescending(s => s.SumRating).ThenBy(e => e.DisplayName);
                                break;
                        }
                    }
                    break;
                case Domain.SubmissionsOrder.ByEvaluationIndex:
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

            return query.ToList();
        }

        /// <summary>
        /// Recupera le statistiche di un valutatore
        /// </summary>
        /// <param name="idEvaluator">Id Valutatore</param>
        /// <param name="idCommittee">Id Commissione</param>
        /// <param name="query">Query valutazioni</param>
        /// <returns></returns>
        private Dictionary<Domain.Evaluation.EvaluationStatus, long> GetEvaluatorStatistics(long idEvaluator, long idCommittee, IQueryable<Eval.Evaluation> query)
        {
            Dictionary<Domain.Evaluation.EvaluationStatus, long> results = new Dictionary<Domain.Evaluation.EvaluationStatus, long>();
            try
            {
                //var p = query.Where(q => q.Committee.Id == idCommittee && q.Evaluator.Id == idEvaluator).GroupBy(q => q.Status).ToList();
                //p.ForEach(t => results.Add(t.Key, t.Count()));

                results[Domain.Evaluation.EvaluationStatus.Evaluated] = query.Where(q => q.Deleted == Core.DomainModel.BaseStatusDeleted.None && q.AdvCommission.Id == idCommittee && q.AdvEvaluator.Id == idEvaluator && q.Status == Domain.Evaluation.EvaluationStatus.Evaluated).Count();
                results[Domain.Evaluation.EvaluationStatus.Evaluating] = query.Where(q => q.Deleted == Core.DomainModel.BaseStatusDeleted.None && q.AdvCommission.Id == idCommittee && q.AdvEvaluator.Id == idEvaluator && q.Status == Domain.Evaluation.EvaluationStatus.Evaluating).Count();
                results[Domain.Evaluation.EvaluationStatus.None] = query.Where(q => q.Deleted == Core.DomainModel.BaseStatusDeleted.None && q.AdvCommission.Id == idCommittee && q.AdvEvaluator.Id == idEvaluator && q.Status == Domain.Evaluation.EvaluationStatus.None).Count();
                results[Domain.Evaluation.EvaluationStatus.EvaluatorReplacement] = query.Where(q => q.Deleted == Core.DomainModel.BaseStatusDeleted.None && q.AdvCommission.Id == idCommittee && q.AdvEvaluator.Id == idEvaluator && q.Status == Domain.Evaluation.EvaluationStatus.EvaluatorReplacement).Count();
                try
                {
                    results[Domain.Evaluation.EvaluationStatus.Confirmed] = query.Where(q => q.Deleted == Core.DomainModel.BaseStatusDeleted.None && q.AdvCommission.Id == idCommittee && q.AdvEvaluator.Id == idEvaluator && q.Status == Domain.Evaluation.EvaluationStatus.Confirmed).Count();
                }
                catch (Exception ex) { }


            }
            catch (Exception ex)
            {

            }
            return results;
        }

        /// <summary>
        /// Riordino valutatori
        /// </summary>
        /// <param name="evaluator">Valutatore corrente</param>
        /// <param name="evaluators">Elenco valutatori</param>
        /// <returns></returns>
        private List<Eval.dtoBaseEvaluatorStatistics> ReorderEvaluators(
            Eval.dtoBaseEvaluatorStatistics evaluator,
            List<Eval.dtoBaseEvaluatorStatistics> evaluators)
        {
            List<Eval.dtoBaseEvaluatorStatistics> items = new List<Eval.dtoBaseEvaluatorStatistics>();
            items.Add(evaluator);
            Eval.dtoBaseEvaluatorStatistics child = evaluators.Where(e => evaluator.ReplacingEvaluator != null && e.IdCallEvaluator == evaluator.ReplacingEvaluator.Id).FirstOrDefault();
            if (child != null)
                items.AddRange(ReorderEvaluators(child, evaluators));
            return items;
        }

        /// <summary>
        /// Recupera le valutazioni di un membro della commissione
        /// </summary>
        /// <param name="member">Membro della commissione</param>
        /// <returns></returns>
        public Eval.dtoBaseEvaluatorStatistics dtoEvalFromMember(Advanced.Domain.AdvMember member)
        {
            Eval.dtoBaseEvaluatorStatistics dto = new Eval.dtoBaseEvaluatorStatistics();
            dto.IdCommittee = member.Commission.Id;

            dto.IdMembership = member.Id;

            dto.IdCallEvaluator = member.Id; // (member.Evaluator != null) ? member.Evaluator.Id : 0;


            if (member.Member != null)// && member.Evaluator.Person != null)
            {
                dto.IdPerson = member.Member.Id;
                dto.DisplayName = member.Member.SurnameAndName;
            }

            dto.ReplacedBy = null; // member.ReplacedBy;
            dto.ReplacingUser = null; //member.ReplacingUser;
            dto.ReplacedByEvaluator = null; //member.ReplacedByEvaluator;
            dto.ReplacingEvaluator = null;//member.ReplacingEvaluator;
            dto.Status = Eval.MembershipStatus.Standard; //member.Status;

            return dto;
        }
        
        /// <summary>
        /// Elenco valutazioni
        /// </summary>
        /// <param name="idSubmissions">Elenco Id Sottomissioni</param>
        /// <param name="anonymousUser">Nome utente anonimo</param>
        /// <param name="unknownUser">Nome utente non trovato</param>
        /// <param name="loadIdRevision">Se caricare l'Id delle revisioni</param>
        /// <returns></returns>
        private List<Eval.dtoBaseSummaryItem> GetBaseEvaluationDisplayItems(
            List<long> idSubmissions,
            String anonymousUser,
            String unknownUser,
            Boolean loadIdRevision)
        {
            List<Eval.dtoBaseSummaryItem> results =
                new List<Eval.dtoBaseSummaryItem>();

            Int32 pageSize = 100;
            Int32 pageIndex = 0;

            List<Eval.Export.expRevision> revisions = null;

            var submissions = (from u in Manager.GetIQ<Eval.Export.expSubmission>() select u);

            var rQuery = (from r in Manager.GetIQ<Eval.Export.expRevision>() where r.IsActive select r);

            var submissionQuery = idSubmissions.Skip(pageIndex * pageSize).Take(pageSize).ToList();

            while (submissionQuery.Any())
            {
                if (loadIdRevision)
                    revisions = rQuery.Where(r => submissionQuery.Contains(r.IdSubmission)).ToList();

                results.AddRange(
                    submissions
                    .Where(s => submissionQuery.Contains(s.Id))
                    .ToList()
                    .Select(s => new Eval.dtoBaseSummaryItem(s, anonymousUser, unknownUser, revisions))
                    .ToList());

                pageIndex++;

                submissionQuery = idSubmissions.Skip(pageIndex * pageSize).Take(pageSize).ToList();
            }
            return results;
        }

        /// <summary>
        /// Verifica se l'utente è il proprietario della valutazione
        /// </summary>
        /// <param name="idEvaluation">Id valutazione</param>
        /// <param name="idPerson">Id utente</param>
        /// <returns></returns>
        public Boolean isEvaluationOwner(long idEvaluation, Int32 idPerson)
        {
            Boolean result = false;
            try
            {
                long idEvaluator = (from s in Manager.GetIQ<Eval.Evaluation>()
                                    where s.Deleted == Core.DomainModel.BaseStatusDeleted.None && s.Id == idEvaluation
                                    select s.AdvEvaluator.Id).Skip(0).Take(1).ToList().FirstOrDefault();

                result = (from m in Manager.GetIQ<Advanced.Domain.AdvMember>() where m.Id == idEvaluator && m.Deleted == Core.DomainModel.BaseStatusDeleted.None && m.Member.Id == idPerson select m.Id).Any();
            }
            catch (Exception ex)
            {

            }
            return result;
        }

        /// <summary>
        /// Recupera tutta la valutazione
        /// </summary>
        /// <param name="idEvaluation">Id valutazione</param>
        /// <param name="anonymousUser">Nome utente anonimo</param>
        /// <param name="unknonwUser">Nome utente non riconosciuto</param>
        /// <returns></returns>
        public Eval.dtoEvaluation GetFullEvaluation(long idEvaluation, String anonymousUser, String unknonwUser)
        {
            return GetEvaluation(idEvaluation, true, anonymousUser, unknonwUser);
        }

        /// <summary>
        /// Recupera dto Valutazione
        /// </summary>
        /// <param name="idEvaluation">Id valutazione</param>
        /// <param name="loadCriteria">Se caricare i criteri</param>
        /// <param name="anonymousUser">Nome utente anonimo</param>
        /// <param name="unknonwUser">Nome utente non trovato</param>
        /// <returns></returns>
        private Eval.dtoEvaluation GetEvaluation(
            long idEvaluation,
            Boolean loadCriteria,
            String anonymousUser,
            String unknonwUser)
        {
            Eval.dtoEvaluation evaluation = null;
            try
            {
                Eval.Export.expEvaluation ev = Manager.Get<Eval.Export.expEvaluation>(idEvaluation);
                if (ev != null && ev.Id > 0)
                    evaluation = new Eval.dtoEvaluation(ev, null, anonymousUser, unknonwUser);  // DssRatingGetByEvaluation(ev.Id)

                if (evaluation != null && loadCriteria)
                {
                    List<Eval.dtoCriterion> criteria = (from bc in Manager.GetIQ<Eval.BaseCriterion>()
                                                        where bc.Deleted == Core.DomainModel.BaseStatusDeleted.None
                                                        && bc.Committee == null && bc.AdvCommitee != null
                                                        && bc.AdvCommitee.Id == evaluation.IdCommittee
                                                        //&& bc.Committee.Id == evaluation.IdCommittee
                                                        select bc).ToList().Select(
                                                                bc => new Eval.dtoCriterion(bc)).ToList();

                    criteria
                        .OrderBy(c => c.DisplayOrder)
                        .ThenBy(c => c.Name)
                        .ToList()
                        .ForEach(c => evaluation.Criteria.Add(
                            new Eval.dtoCriterionEvaluated(c, (ev.EvaluatedCriteria == null ? null : ev.EvaluatedCriteria.Where(v => v.Criterion.Id == c.Id)
                            .FirstOrDefault()))
                        )
                        );
                }
            }
            catch (Exception ex)
            {

            }
            return evaluation;
        }

        /// <summary>
        /// Salva le valutazioni di un valutatore
        /// </summary>
        /// <param name="idEvaluation">Id valutazione</param>
        /// <param name="idEvaluator">Id Valutatore</param>
        /// <param name="criteria">Valutazioni</param>
        /// <param name="comment">Commento generale</param>
        /// <param name="completed">Se FALSE = bozza, se TRUE salvataggio definitivo</param>
        /// <returns></returns>
        public Eval.Evaluation SaveEvaluation(
            long idEvaluation,
            long idEvaluator,
            List<Eval.dtoCriterionEvaluated> criteria,
            String comment,
            Boolean completed)
        {
            Eval.Evaluation evaluation = null;
            try
            {
                Manager.BeginTransaction();
                Core.DomainModel.litePerson person = Manager.GetLitePerson(UC.CurrentUserID);
                evaluation = Manager.Get<Eval.Evaluation>(idEvaluation);

                Domain.EvaluationError exception = new Domain.EvaluationError();

                if (evaluation != null
                    && person != null
                    && (person.TypeID != (int)(Core.DomainModel.UserTypeStandard.PublicUser)
                    && person.TypeID != (int)(Core.DomainModel.UserTypeStandard.Guest)))
                {
                    DateTime saveTime = DateTime.Now;
                    evaluation.UpdateMetaInfo(person, UC.IpAddress, UC.ProxyIpAddress, saveTime);
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
                    List<Eval.CriterionEvaluated> savedValues = new List<Eval.CriterionEvaluated>();
                    foreach (Eval.dtoCriterionEvaluated dto in criteria)
                    {
                        Eval.CriterionEvaluated criterion = Manager.Get<Eval.CriterionEvaluated>(dto.IdValueCriterion);
                        if (criterion == null)
                            criterion = (from e in Manager.GetIQ<Eval.CriterionEvaluated>()
                                         where e.Evaluation.Id == idEvaluation
                                         && e.Deleted == Core.DomainModel.BaseStatusDeleted.None
                                         && e.Criterion != null
                                         && e.Criterion.Id == dto.IdCriterion
                                         select e).Skip(0).Take(1).ToList().FirstOrDefault();

                        if (criterion == null)
                        {
                            criterion = new Eval.CriterionEvaluated();
                            criterion.CreateMetaInfo(person, UC.IpAddress, UC.ProxyIpAddress, saveTime);
                            criterion.Call = evaluation.Call;
                            criterion.Criterion = Manager.Get<Eval.BaseCriterion>(dto.IdCriterion);
                            criterion.Evaluation = evaluation;
                            criterion.Submission = evaluation.Submission;
                        }
                        else
                            criterion.UpdateMetaInfo(person, UC.IpAddress, UC.ProxyIpAddress, saveTime);

                        criterion.Comment = dto.Comment;
                        criterion.DecimalValue = dto.DecimalValue;
                        criterion.StringValue = dto.StringValue;
                        criterion.IsValueEmpty = dto.IsValueEmpty;

                        //switch (criterion.Criterion.Type)
                        //{
                        //    case Eval.CriterionType.RatingScale:
                        //    case Eval.CriterionType.RatingScaleFuzzy:
                        //        if (dto.DssValue.Error == Core.Dss.Domain.DssError.None)
                        //        {
                        //            criterion.DssValue = dto.DssValue;
                        //            criterion.DssValue.IsFuzzy = (criterion.Criterion.Type == Eval.CriterionType.RatingScaleFuzzy);
                        //        }
                        //        else if (dto.DssValue.IdRatingValue > 0 || dto.DssValue.IdRatingValueEnd > 0)
                        //        {
                        //            criterion.DssValue = dto.DssValue;
                        //            criterion.DssValue.IsFuzzy = (criterion.Criterion.Type == Eval.CriterionType.RatingScaleFuzzy);
                        //        }
                        //        break;
                        //    default:
                        //        break;
                        //}
                        if (dto.IdOption > 0)
                            criterion.Option = Manager.Get<Eval.CriterionOption>(dto.IdOption);
                        else
                            criterion.Option = null;
                        if (criterion.Option != null)
                        {
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
                                                       && c.Criterion.Type != Eval.CriterionType.Boolean
                                                       select (double)c.DecimalValue).ToList();

                        //if (evaluation.Committee != null && evaluation.Committee.UseDss)
                        //{
                        //    standardValues.AddRange((from c in criteria
                        //                             where c.IsValidForCriterionSaving
                        //                             where c.Criterion.Type == Eval.CriterionType.RatingScale || c.Criterion.Type == CriterionType.RatingScaleFuzzy
                        //                             select c.DssValue.Value).ToList());
                        //}
                        evaluation.AverageRating = (standardValues.Any() ? (double)standardValues.Average() : 0);
                        evaluation.SumRating = (standardValues.Any() ? (double)standardValues.Sum() : 0);

                        List<double> boolValues = (from c in criteria
                                                   where c.IsValidForCriterionSaving
                                                   && c.Criterion.Type == Eval.CriterionType.Boolean
                                                   select (double)c.DecimalValue).ToList();

                        evaluation.BoolRating = (boolValues.Any() ? boolValues.All(b => b > 0) : false);


                        //---
                        //if (evaluation.AdvCommission != null && evaluation.AdvCommission.EvalType == EvalType.Average)
                        //{
                        //    evaluation.IsPassed = (evaluation.AverageRating >= evaluation.AdvCommission.EvalMinValue);
                        //} else if(evaluation.AdvCommission != null && evaluation.AdvCommission.EvalType == EvalType.Sum)
                        //{
                        //    evaluation.IsPassed = (evaluation.SumRating >= evaluation.AdvCommission.EvalMinValue);
                        //}

                        //if (evaluation.AdvCommission != null && evaluation.AdvCommission.EvalBoolBlock)
                        //{
                        //    evaluation.IsPassed = evaluation.IsPassed & evaluation.BoolRating;
                        //}
                        //---

                        if (evaluation.AdvCommission != null)
                        {
                            bool passed = true;

                            if (evaluation.AdvCommission.EvalBoolBlock)
                            {
                                passed = evaluation.BoolRating;
                            }

                            if(passed)
                            { 

                                //NOTA: i criteri si SOMMANO SEMPRE

                                //if (evaluation.AdvCommission.EvalType == EvalType.Average)
                                //{
                                //    passed = (evaluation.AdvCommission.EvalMinValue > 0) ?
                                //        evaluation.AverageRating >= evaluation.AdvCommission.EvalMinValue
                                //        : true;
                                //}
                                //else if (evaluation.AdvCommission.EvalType == EvalType.Sum)
                                //{
                                    passed = passed && 
                                        (evaluation.AdvCommission.EvalMinValue > 0) ?
                                        evaluation.SumRating >= evaluation.AdvCommission.EvalMinValue :
                                        true;
                                //}
                            }
                            evaluation.IsPassed = passed;
                        }



                        if (evaluation.Call != null && evaluation.Call.IdDssMethod > 0)
                        {
                            //EvaluatorSetDssRating(idEvaluator, evaluation.Committee, idEvaluation, saveTime);
                        }

                    }
                    else
                    {
                    }
                }
                Manager.SaveOrUpdate<Eval.Evaluation>(evaluation);
                Manager.Commit();
                if (exception.Criteria.Count > 0)
                    throw exception;
            }
            catch (Domain.EvaluationError ex)
            {
                Manager.RollBack();
                throw ex;
            }
            catch (Exception ex)
            {
                Manager.RollBack();
                evaluation = null;
            }

            try
            {
                if(evaluation.AdvCommission.Step != null)
                {
                    StepEvUpdateAll(evaluation.AdvCommission.Step.Id);
                } else
                {
                    CommissionEvUpdate(evaluation.AdvCommission.Id);
                }

                
            } catch(Exception ex) { }
            return evaluation;
        }

        /// <summary>
        /// Dto elementi di valutazione per il valutatore
        /// </summary>
        /// <param name="idCommittees">Id commissione</param>
        /// <param name="idSubmission">Id sottomissione</param>
        /// <param name="criteria">Criteri</param>
        /// <param name="member">Membro (valutatore)</param>
        /// <param name="evaluation">Valutazioni</param>
        /// <param name="unknownUser">Nome utente sconosciuto</param>
        /// <returns></returns>
        private Eval.dtoEvaluatorDisplayItem CreateEvaluatorDisplayItem(
            long idCommittees,
            long idSubmission,
            List<Eval.dtoCriterionSummaryItem> criteria,
            Advanced.Domain.AdvMember member,
            Eval.Export.expEvaluation evaluation,
            String unknownUser)
        {
            Eval.dtoEvaluatorDisplayItem item = new Eval.dtoEvaluatorDisplayItem();
            item.IdEvaluator = (member != null) ? member.Id : 0;
            item.Name = (member != null && member.Member != null) ? member.Member.Name : "";
            item.Surname = (member != null && member.Member != null) ? member.Member.Surname : unknownUser;
            item.EvaluatorName = unknownUser;
            item.MembershipStatus = Eval.MembershipStatus.Standard;
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
                item.IsPassed = evaluation.IsPassed;
                item.DssEvaluation = new Eval.DssCallEvaluation(); //dssEvaluation;
                criteria
                    .OrderBy(c => c.DisplayOrder)
                    .ThenBy(c => c.Name)
                    .ToList()
                    .ForEach(c =>
                        item.Values.Add(
                            new Eval.dtoCriterionEvaluated(
                                c,
                                (evaluation.EvaluatedCriteria != null) ?
                                    evaluation.EvaluatedCriteria.Where(v => v.Criterion.Id == c.Id).FirstOrDefault()
                                    : null)
                                    )
                              );
            }
            else
            {
                item.IgnoreEvaluation = true;
                item.Status = Eval.EvaluationStatus.None;
                //switch (status)
                //{
                //    case Eval.MembershipStatus.Removed:
                //        item.Status = Eval.EvaluationStatus.Invalidated;
                //        break;
                //    case Eval.MembershipStatus.Replaced:
                //        item.Status = Eval.EvaluationStatus.EvaluatorReplacement;
                //        break;
                //    default:
                //        item.Status = Eval.EvaluationStatus.None;
                //        break;
                //}
                criteria.OrderBy(c => c.DisplayOrder).ThenBy(c => c.Name).ToList().ForEach(c => item.Values.Add(new Eval.dtoCriterionEvaluated(c, true)));
            }
            return item;
        }
        
        /// <summary>
        /// Reimposta una valutazione in bozza
        /// </summary>
        /// <param name="evalId">Id Valutazione</param>
        /// <returns></returns>
        public bool EvalSetDraft(long evalId)
        {
            Eval.Evaluation eval = Manager.Get<Eval.Evaluation>(evalId);

            if (eval == null)
                return false;

            eval.Status = Eval.EvaluationStatus.Evaluating;
            eval.UpdateMetaInfo(GetCurrentPerson(), UC.IpAddress, UC.ProxyIpAddress);

            try
            {
                if (!Manager.IsInTransaction())
                    Manager.BeginTransaction();

                Manager.SaveOrUpdate<Eval.Evaluation>(eval);
                Manager.Commit();
            }
            catch (Exception ex)
            {
                Manager.RollBack();
                return false;
            }
            return true;
        }

        /// <summary>
        /// Imposta una valutazione come confermata
        /// </summary>
        /// <param name="evalId"></param>
        /// <returns></returns>
        public bool EvalSetConfirmed(long evalId)
        {
            Eval.Evaluation eval = Manager.Get<Eval.Evaluation>(evalId);

            if (eval == null)
                return false;

            eval.Status = Eval.EvaluationStatus.Confirmed;
            eval.UpdateMetaInfo(GetCurrentPerson(), UC.IpAddress, UC.ProxyIpAddress);

            try
            {
                if (!Manager.IsInTransaction())
                    Manager.BeginTransaction();

                Manager.SaveOrUpdate<Eval.Evaluation>(eval);
                Manager.Commit();
            }
            catch (Exception ex)
            {
                Manager.RollBack();
                return false;
            }
            return true;
        }
        
        /// <summary>
        /// Aggiorna i sommari delle valutazioni della commissione
        /// </summary>
        /// <param name="CommId">Id commissione</param>
        /// <returns></returns>
        public bool CommissionEvUpdate(long CommId)
        {
            Advanced.Domain.AdvCommission comm = Manager.Get<Advanced.Domain.AdvCommission>(CommId);
            if (comm.Status == CommissionStatus.Closed)
                return true;

            IList<Eval.Evaluation> evls = Manager.GetAll<Eval.Evaluation>(ev => ev.AdvCommission != null && ev.AdvCommission.Id == CommId);

            long stepId = (comm.Step != null) ? comm.Step.Id : 0;

            if (stepId == 0)
                return false;

            IList<Advanced.Domain.AdvSubmissionToStep> oldSubs = Manager.GetAll<Advanced.Domain.AdvSubmissionToStep>(s =>
                    s.Step != null && s.Step.Id == stepId
                    && s.Commission != null && s.Commission.Id == comm.Id);

            if (oldSubs == null)
                oldSubs = new List<Advanced.Domain.AdvSubmissionToStep>();
            
            foreach (var c in evls.GroupBy(e => e.Submission))
            {
                bool add = false;
                Advanced.Domain.AdvSubmissionToStep substep = oldSubs.FirstOrDefault(s => s.Submission.Id == c.Key.Id);
                if (substep == null)
                {
                    add = true;
                    substep = new Advanced.Domain.AdvSubmissionToStep();
                    substep.CreateMetaInfo(GetCurrentPerson(), UC.IpAddress, UC.ProxyIpAddress);

                    substep.Commission = comm;
                    substep.Step = comm.Step;
                    substep.Submission = c.Key;
                }
                else
                {
                    substep.UpdateMetaInfo(GetCurrentPerson(), UC.IpAddress, UC.ProxyIpAddress);
                }

                //Criteri: lascio entrambe, POI dipende dalle impostazioni della commissione E successivamente dello step!!!
                // SOMMA : anche per la media uso la somma dei criteri
                substep.AverageRating = Math.Round(c.Average(sr => sr.SumRating), 2);
                //substep.AverageRating = Math.Round(c.Average(sr => Math.Round(sr.AverageRating, 2)), 2);
            
                substep.SumRating = c.Sum(sr => sr.SumRating);

                substep.BoolRating = c.All(sr => sr.BoolRating);

                substep.Passed = c.All(sr => sr.IsPassed);
                substep.Admitted = substep.Passed;
                
                if (add)
                    oldSubs.Add(substep);

            }

            int rank = 1;


            foreach (Advanced.Domain.AdvSubmissionToStep sbst in oldSubs.OrderByDescending(s => s.Passed)
                .ThenByDescending(s => s.AverageRating).ThenBy(s => s.Submission.SubmittedOn))
            {
                sbst.Rank = rank;
                rank++;
            }

            if (!Manager.IsInTransaction())
                Manager.BeginTransaction();

            try
            {
                Manager.SaveOrUpdateList<Advanced.Domain.AdvSubmissionToStep>(oldSubs);
                Manager.Commit();
            }
            catch (Exception ex)
            {
                Manager.RollBack();
                return false;
            }

            return true;
        }
        
        /// <summary>
        /// Aggiunge lo step di validazione SE non è presente.
        /// </summary>
        /// <param name="call">Bando</param>
        /// <param name="defaultValName">Nome commissione criteri formali (ammissione)</param>
        /// <param name="defaultEcoName">Nome commissione economica</param>
        /// <returns></returns>
        private dtoStepsEdit addValidationIfEmpty(Domain.CallForPaper call, string defaultValName, string defaultEcoName)
        {
            Advanced.Domain.AdvStep StepVal = new Advanced.Domain.AdvStep();
            StepVal.Call = call;

            StepVal.CreateMetaInfo(GetCurrentPerson(), UC.IpAddress, UC.ProxyIpAddress);
            StepVal.Name = "";
            StepVal.Order = 0;
            StepVal.Type = StepType.validation;
            StepVal.Commissions = new List<Advanced.Domain.AdvCommission>();
            StepVal.Commissions.Add(getDefaultCommission(call, StepVal, GetCurrentPerson(), defaultValName, true));

            Advanced.Domain.AdvStep StepEco = null;
            if (EconomicCommissionNeed(call))
            {
                StepEco = new Advanced.Domain.AdvStep();
                StepEco.Call = call;
                StepEco.CreateMetaInfo(GetCurrentPerson(), UC.IpAddress, UC.ProxyIpAddress);
                StepEco.Name = "";
                StepEco.Order = 0;
                StepEco.Type = StepType.economics;
                StepEco.Commissions = new List<Advanced.Domain.AdvCommission>();
                StepEco.Commissions.Add(getDefaultCommission(call, StepEco, GetCurrentPerson(), defaultEcoName, true));
            }


            if (!Manager.IsInTransaction())
                Manager.BeginTransaction();

            try
            {
                Manager.SaveOrUpdate<Advanced.Domain.AdvStep>(StepVal);
                if (StepEco != null)
                {
                    Manager.SaveOrUpdate<Advanced.Domain.AdvStep>(StepEco);
                }

                Manager.Commit();
            }
            catch (Exception ex)
            {
                Manager.RollBack();
            }

            IList<Advanced.Domain.AdvStep> Steps = new List<Advanced.Domain.AdvStep>();
            Steps.Add(StepVal);

            if (StepEco != null)
                Steps.Add(StepVal);

            return new dtoStepsEdit(Steps, UC.CurrentUserID);
        }
        
        /// <summary>
        /// Recupera i tag previsti per un valutatore
        /// </summary>
        /// <param name="evaluationId">Id valutazione</param>
        /// <param name="callId">Id bando</param>
        /// <param name="canEvaluate">Permesso valutazione</param>
        /// <returns></returns>
        public string TagGetForEvaluation(long evaluationId, long callId, ref bool canEvaluate)
        {
            canEvaluate = false;

            Eval.Evaluation ev = Manager.Get<Eval.Evaluation>(evaluationId);



            if (ev.AdvCommission == null)
            {
                //Attenzione: se non imposto TAG, carico quelli del bando?!
                return "";
            }


            if (
                ev.AdvCommission.Members.Any(m => m.Member != null && m.Member.Id == UC.CurrentUserID)
                && ev.AdvCommission.Status == CommissionStatus.Started)
            {
                canEvaluate = true;
            }

            return ev.AdvCommission.Tags;
        }


        /// <summary>
        /// Recupera l'elenco delle valutazioni
        /// </summary>
        /// <param name="idCall">Id bando</param>
        /// <param name="idCommission">Id commissione</param>
        /// <param name="type">Tipo valuatione</param>
        /// <param name="filters">Filtri</param>
        /// <param name="anonymousUser">Nome utente anonimo (localizzato)</param>
        /// <param name="unknownUser">Nome utente sconosciuto (localizzato)</param>
        /// <returns></returns>
        public List<Eval.dtoEvaluationSummaryItem> GetEvaluationsList(
          long idCall,
          long idCommission,
          Domain.EvaluationType type,
          Eval.dtoEvaluationsFilters filters,
          String anonymousUser,
          String unknownUser)
        {

            Boolean loadRevisionInfo = false;

            List<Eval.dtoEvaluationSummaryItem> items = null;
            try
            {
                //Advanced.Domain.AdvCommission comm = Manager.Get<Advanced.Domain.AdvCommission>(idCommission);
                //if (comm.EvalType == EvalType.Sum)
                //    type = Domain.EvaluationType.Sum;
                //else
                //    type = Domain.EvaluationType.Average;


                List<Eval.DssCallEvaluation> dssRatings = new List<Eval.DssCallEvaluation>();
                //DssRatingGetValues(idCall);

                List<Eval.Export.expEvaluation> evaluations = (
                    from e in Manager.GetIQ<Eval.Export.expEvaluation>()
                    where
                    e.IdCall == idCall
                    && e.AdvCommission != null && e.AdvCommission.Id == idCommission
                    select e).ToList();

                Advanced.Domain.AdvCommission comm = Manager.Get<Advanced.Domain.AdvCommission>(idCommission);


                bool hasScore = (comm != null
                    && comm.Criteria != null
                    && comm.Criteria.Any(
                        c => c.Type == Eval.CriterionType.DecimalRange
                        || c.Type == Eval.CriterionType.IntegerRange
                        || c.Type == Eval.CriterionType.StringRange)
                    ) ? true : false;
                
                List<long> idSubmissions = evaluations.Where(e => e.Deleted == Core.DomainModel.BaseStatusDeleted.None).Select(e => e.IdSubmission).Distinct().ToList();

                List<Eval.dtoBaseSummaryItem> temp =
                    GetBaseEvaluationDisplayItems(
                        idSubmissions,
                        anonymousUser,
                        unknownUser,
                        loadRevisionInfo);


                items = temp
                    .Where(i => idSubmissions.Contains(i.IdSubmission))
                    .Select(i => new Eval.dtoEvaluationSummaryItem(
                        i,
                        evaluations.Where(e =>
                            e.IdSubmission == i.IdSubmission
                            && e.Deleted == Core.DomainModel.BaseStatusDeleted.None)
                            .ToList(),
                        dssRatings.Where(e => e.IdSubmission == i.IdSubmission).FirstOrDefault(),
                        true,
                        hasScore
                        )
                    ).ToList();


                items.ForEach(i => i.IsAdvance = true);

                long position = 1;

                switch (type)
                {
                    case Domain.EvaluationType.Sum:
                        items
                            .OrderByDescending(e => e.SumRating)
                            .ThenBy(i => i.DisplayName)
                            .ToList()
                            .ForEach(i => i.Position = position++);
                        break;


                    default:
                        items
                            .OrderByDescending(e => e.AverageRating)
                            .ThenBy(i => i.DisplayName)
                            .ToList()
                            .ForEach(i => i.Position = position++);
                        break;

                }

                if (filters.IdSubmitterType > 0)
                    items = items
                        .Where(i => i.IdSubmitterType == filters.IdSubmitterType).ToList();

                if (!string.IsNullOrEmpty(filters.SearchForName) && !String.IsNullOrEmpty(filters.SearchForName.Trim()))
                {
                    idSubmissions.Clear();
                    List<String> searchName = filters.SearchForName.ToLower().Split(' ').ToList();

                    searchName
                        .ForEach(s => temp
                            .Where(i => i.DisplayName.ToLower().Contains(s))
                            .ToList()
                            .ForEach(t => idSubmissions.Add(t.IdSubmission))
                        );

                    items = items.Where(i => idSubmissions.Contains(i.IdSubmission)).ToList();
                }

                Boolean ascending = filters.Ascending;
                switch (filters.Status)
                {
                    case Eval.EvaluationFilterStatus.All:
                        break;
                    case Eval.EvaluationFilterStatus.AllValid:
                        items = items
                            .Where(i => !i.Evaluations
                                .Where(e =>
                                    e.Status == Eval.EvaluationStatus.EvaluatorReplacement
                                    && e.Status == Eval.EvaluationStatus.Invalidated)
                                .Any())
                            .ToList();
                        break;

                    case Eval.EvaluationFilterStatus.EvaluatorReplacement:
                        items = items
                                .Where(i => i.Evaluations
                                    .Where(e => e.Status == Eval.EvaluationStatus.EvaluatorReplacement)
                                    .Any())
                                .ToList();
                        break;

                    case Eval.EvaluationFilterStatus.Invalidated:
                        items = items.Where(i => i.Evaluations.Where(e => e.Status == Eval.EvaluationStatus.Invalidated).Any()).ToList();
                        break;
                    case Eval.EvaluationFilterStatus.Evaluated:
                        items = items.Where(i => !i.Evaluations.Where(e => !e.Evaluated).Any()).ToList();
                        break;
                    case Eval.EvaluationFilterStatus.Evaluating:
                        items = items.Where(i => i.Evaluations.Where(e => !e.Evaluated && e.Status != Eval.EvaluationStatus.None).Any()).ToList();
                        break;
                    case Eval.EvaluationFilterStatus.None:
                        items = items.Where(i => !i.Evaluations.Where(e => e.Status != Eval.EvaluationStatus.None && e.Status != Eval.EvaluationStatus.EvaluatorReplacement && e.Status != Eval.EvaluationStatus.Invalidated).Any()).ToList();
                        break;
                }
                var query = (from q in items select q);

                #region "order"
                switch (filters.OrderBy)
                {
                    case Domain.SubmissionsOrder.BySubmittedOn:
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
                    case Domain.SubmissionsOrder.ByEvaluationStatus:
                        if (ascending)
                            query = query.OrderBy(r => filters.TranslationsEvaluationStatus[r.Status]);
                        else
                            query = query.OrderByDescending(r => filters.TranslationsEvaluationStatus[r.Status]);
                        break;
                    case Domain.SubmissionsOrder.ByType:
                        if (ascending)
                            query = query.OrderBy(s => s.SubmitterType);
                        else
                            query = query.OrderByDescending(s => s.SubmitterType);
                        break;
                    case Domain.SubmissionsOrder.ByUser:
                        if (ascending)
                            query = query.OrderBy(s => s.DisplayName);
                        else
                            query = query.OrderByDescending(s => s.DisplayName);
                        break;
                    case Domain.SubmissionsOrder.ByEvaluationPoints:
                        //if (ascending)
                        //    query = query.OrderBy(s => s.SumRating).ThenBy(e => e.DisplayName);
                        //else
                        //    query = query.OrderByDescending(s => s.SumRating).ThenBy(e => e.DisplayName);
                        //break;

                        if (ascending)
                        {
                            switch (type)
                            {
                                case Domain.EvaluationType.Average:
                                    query = query.OrderBy(s => s.AverageRating).ThenBy(e => e.DisplayName);
                                    break;
                                case Domain.EvaluationType.Dss:
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
                                case Domain.EvaluationType.Average:
                                    query = query.OrderByDescending(s => s.AverageRating).ThenBy(e => e.DisplayName);
                                    break;
                                case Domain.EvaluationType.Dss:
                                    query = query.OrderByDescending(s => s.DssRanking).ThenBy(e => e.DisplayName);
                                    break;
                                default:
                                    query = query.OrderByDescending(s => s.SumRating).ThenBy(e => e.DisplayName);
                                    break;
                            }
                        }
                        break;

                    case Domain.SubmissionsOrder.ByEvaluationIndex:
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
                items = new List<Eval.dtoEvaluationSummaryItem>();
            }
            return items;
        }

        /// <summary>
        /// Recupera il tipo di aggregazione delle valutazioni di una commissione
        /// </summary>
        /// <param name="CommId">Id commissione</param>
        /// <returns>tipo di valutazione associata</returns>
        public Domain.EvaluationType CommissionGetEvalType(long CommId)
        {

            Domain.EvaluationType type = Domain.EvaluationType.Average;

            Advanced.Domain.AdvCommission comm = Manager.Get<Advanced.Domain.AdvCommission>(CommId);

            if (comm.EvalType == EvalType.Sum)
                type = Domain.EvaluationType.Sum;
            else
                type = Domain.EvaluationType.Average;


            return type;
        }
        
        #endregion

        #region Submission

        /// <summary>
        /// Assegnazione sottomissioni
        /// </summary>
        /// <param name="callId">Id bando</param>
        /// <param name="commId">Id commissione</param>
        /// <returns></returns>
        public int SubmissionAssignAll(long callId, long commId)
        {
            Advanced.Domain.AdvCommission comm = Manager.Get<Advanced.Domain.AdvCommission>(commId);

            if (comm == null
                || comm.Step == null
                || comm.Call == null
                )
                return (int)SubmissionAssignAllErrorValue.StepOrCallNotFound;

            callId = comm.Call.Id;
            if(callId <= 0)
                return (int)SubmissionAssignAllErrorValue.StepOrCallNotFound;


            Domain.CallForPaperStatus status = comm.Call.Status;

            if (status != Domain.CallForPaperStatus.SubmissionClosed)
                return (int)SubmissionAssignAllErrorValue.CallNotClosed;

            if (comm.Step != null && comm.Step.Type == StepType.economics)
            {
                EcoCreateEvaluation(comm.Id);
                return 1;
            }


            int NewAssignment = 0;


            if (comm.Step.Type == StepType.validation)
            {

                IList<Advanced.Domain.AdvSubmissionToStep> assSubmission =
                Manager.GetAll<Advanced.Domain.AdvSubmissionToStep>(ass => ass.Step != null && ass.Step.Id == comm.Step.Id);

                IList<Advanced.Domain.AdvSubmissionToStep> NewAssignments = new List<Advanced.Domain.AdvSubmissionToStep>();
                IList<Domain.UserSubmission> submission;

                if (assSubmission == null || !assSubmission.Any())
                {
                    submission = Manager.GetAll<Domain.UserSubmission>(
                        us =>
                        us.Call != null && us.Call.Id == callId
                        && us.Status == Domain.SubmissionStatus.submitted
                        );
                }
                else
                {
                    IList<long> SubmissionsId = (from Advanced.Domain.AdvSubmissionToStep assSub in assSubmission
                                                 where assSub.Submission != null
                                                 && assSub.Step != null
                                                 && assSub.Step.Id == comm.Step.Id
                                                 select assSub.Submission.Id).ToList();

                    submission = Manager.GetAll<Domain.UserSubmission>(
                        us =>
                        us.Call != null && us.Call.Id == callId
                        && us.Status == Domain.SubmissionStatus.submitted
                        && !SubmissionsId.Contains(us.Id)
                        );
                }

                if (!(submission == null || !submission.Any()))
                {
                    //    return (int)SubmissionAssignAllErrorValue.NoSubmissionForCall;



                    NewAssignments = new List<Advanced.Domain.AdvSubmissionToStep>();



                    foreach (Domain.UserSubmission us in submission)
                    {
                        Advanced.Domain.AdvSubmissionToStep StepAssignment = new Advanced.Domain.AdvSubmissionToStep();
                        StepAssignment.Commission = null;
                        StepAssignment.Step = comm.Step;
                        StepAssignment.Submission = us;
                        StepAssignment.Passed = false;
                        StepAssignment.Rank = 0;
                        StepAssignment.CreateMetaInfo(GetCurrentPerson(), UC.IpAddress, UC.ProxyIpAddress);

                        NewAssignments.Add(StepAssignment);

                        Advanced.Domain.AdvSubmissionToStep CommAssignment = new Advanced.Domain.AdvSubmissionToStep();
                        CommAssignment.Commission = comm;
                        CommAssignment.Step = comm.Step;
                        CommAssignment.Submission = us;
                        CommAssignment.Passed = false;
                        CommAssignment.Rank = 0;
                        CommAssignment.CreateMetaInfo(GetCurrentPerson(), UC.IpAddress, UC.ProxyIpAddress);

                        NewAssignments.Add(CommAssignment);

                        NewAssignment++;
                    }


                    try
                    {
                        if (!Manager.IsInTransaction())
                            Manager.BeginTransaction();


                        Manager.SaveOrUpdateList<Advanced.Domain.AdvSubmissionToStep>(NewAssignments);

                        Manager.Commit();

                    }
                    catch (Exception ex)
                    {
                        Manager.RollBack();
                        return (int)SubmissionAssignAllErrorValue.InternalError;
                    }
                }
            }
            else
            {

                IList<Advanced.Domain.AdvSubmissionToStep> oldSubmission = Manager.GetAll<Advanced.Domain.AdvSubmissionToStep>(
                    lk => lk.Step != null && lk.Step.Id == comm.Step.Id);

                //IList<Advanced.Domain.AdvSubmissionToStep> assSubmission =
                //    Manager.GetAll<Advanced.Domain.AdvSubmissionToStep>(ass => ass.Step != null && ass.Step.Id == comm.Step.Id);

                //bool hasComSubmission = (from Advanced.Domain.AdvSubmissionToStep lk in Manager.GetIQ<Advanced.Domain.AdvSubmissionToStep>()
                //                         where lk.Step != null && lk.Step.Id == comm.Step.Id
                //                         && lk.Commission != null && lk.Commission.Id == comm.Id
                //                         select lk.Id
                //                          ).Skip(0).Take(0).ToList().Any();

                //if (!(hasStepSubmission && hasComSubmission))
                //{
                Advanced.Domain.AdvStep prevStep;

                if (comm.Step.Type == StepType.economics)
                {
                    prevStep = Manager.GetAll<Advanced.Domain.AdvStep>(
                    st => st.Call != null
                    && st.Call.Id == callId
                    && st.Type != StepType.economics
                    )
                    .OrderBy(st => st.Order)
                    .LastOrDefault();
                }
                else
                {
                    prevStep = Manager.GetAll<Advanced.Domain.AdvStep>(
                    st => st.Call != null
                    && st.Call.Id == callId
                    && st.Type != StepType.economics
                    && st.Order < comm.Step.Order
                    )
                    .OrderBy(st => st.Order)
                    .LastOrDefault();
                }

                if (prevStep == null)
                    return (int)SubmissionAssignAllErrorValue.PreviosStepNotFound;


                if (prevStep.Status != StepStatus.Closed)
                    return (int)SubmissionAssignAllErrorValue.PreviousStepNotClosed;

                IList<Advanced.Domain.AdvSubmissionToStep> PrevAssignments = Manager.GetAll<Advanced.Domain.AdvSubmissionToStep>(
                    lk =>
                    lk.Step != null && lk.Step.Id == prevStep.Id
                    && lk.Commission == null
                    && lk.Admitted == true);

                //NewAssignments = new List<Advanced.Domain.AdvSubmissionToStep>();

                foreach (Advanced.Domain.AdvSubmissionToStep oldAss in PrevAssignments)
                {
                    //Rivedere aggiungendo solo i nuovi!

                    Advanced.Domain.AdvSubmissionToStep StepAssignment = oldSubmission
                        .FirstOrDefault(a =>
                            a.Submission != null && a.Submission.Id == oldAss.Submission.Id
                            && a.Commission == null);

                    if (StepAssignment == null)
                    {
                        StepAssignment = new Advanced.Domain.AdvSubmissionToStep();
                        StepAssignment.Commission = null;
                        StepAssignment.Step = comm.Step;
                        StepAssignment.Submission = oldAss.Submission;
                        StepAssignment.Passed = false;
                        StepAssignment.Rank = 0;
                        StepAssignment.CreateMetaInfo(GetCurrentPerson(), UC.IpAddress, UC.ProxyIpAddress);

                        oldSubmission.Add(StepAssignment);
                    }

                    Advanced.Domain.AdvSubmissionToStep CommAssignment = oldSubmission
                        .FirstOrDefault(a =>
                            a.Submission != null && a.Submission.Id == oldAss.Submission.Id
                            && a.Commission != null && a.Commission.Id == comm.Id);

                    if (CommAssignment == null)
                    {
                        CommAssignment = new Advanced.Domain.AdvSubmissionToStep();
                        CommAssignment.Commission = comm;
                        CommAssignment.Step = comm.Step;
                        CommAssignment.Submission = oldAss.Submission;
                        CommAssignment.Passed = false;
                        CommAssignment.Rank = 0;
                        CommAssignment.CreateMetaInfo(GetCurrentPerson(), UC.IpAddress, UC.ProxyIpAddress);

                        oldSubmission.Add(CommAssignment);

                        NewAssignment++;
                    }
                }


                try
                {
                    if (!Manager.IsInTransaction())
                        Manager.BeginTransaction();


                    Manager.SaveOrUpdateList<Advanced.Domain.AdvSubmissionToStep>(oldSubmission);

                    Manager.Commit();

                }
                catch (Exception ex)
                {
                    Manager.RollBack();
                    return (int)SubmissionAssignAllErrorValue.InternalError;
                }


            }

            //return NewAssignment;
            return SubmissionAssignToOld(callId, commId);
        }
        
        /// <summary>
        /// Assegnazione a valutazioni (vecchi oggetti)
        /// </summary>
        /// <param name="callId">Id bando</param>
        /// <param name="commId">Id commissione</param>
        /// <returns></returns>
        public int SubmissionAssignToOld(long callId, long commId)
        {
            Advanced.Domain.AdvCommission comm = Manager.Get<Advanced.Domain.AdvCommission>(commId);

            if (comm == null
                || comm.Step == null
                || comm.Call == null
                )
                return (int)SubmissionAssignAllErrorValue.StepOrCallNotFound;

            IList<Eval.Evaluation> evals = Manager.GetAll<Eval.Evaluation>(ev =>
                ev.Committee == null
                && ev.AdvCommission != null
                && ev.AdvCommission.Id == commId);

            if (evals == null)
                evals = new List<Eval.Evaluation>();


            IList<Eval.Evaluation> newEvals = new List<Eval.Evaluation>();

            IList<Domain.UserSubmission> Submissions = new List<Domain.UserSubmission>();

            if (comm.Step.Type == StepType.validation)
            {
                Submissions = Manager.GetAll<Domain.UserSubmission>(sub => sub.Call.Id == comm.Call.Id && sub.Status == Domain.SubmissionStatus.submitted);
            }
            else
            {
                Submissions = (from Advanced.Domain.AdvSubmissionToStep comStep in
                                   Manager.GetAll<Advanced.Domain.AdvSubmissionToStep>(
                                        cs => cs.Step != null
                                        && cs.Step.Id == comm.Step.Id
                                        && cs.Commission == null)
                               select comStep.Submission).Distinct().ToList();
            }

            foreach (Domain.UserSubmission sub in Submissions)
            {

                foreach (Advanced.Domain.AdvMember mem in comm.Members)
                {
                    Eval.Evaluation eval = evals.FirstOrDefault(ev => ev.AdvEvaluator != null && ev.AdvEvaluator.Id == mem.Id);
                    if (eval == null || eval.Id <= 0)
                    {
                        Eval.Evaluation neweval = new Eval.Evaluation();
                        neweval.Committee = null;
                        neweval.AdvCommission = comm;
                        neweval.Evaluator = null;
                        neweval.IsPassed = false;
                        neweval.BoolRating = false;
                        neweval.AdvEvaluator = mem;
                        neweval.Call = comm.Call;
                        neweval.Evaluated = false;
                        neweval.AverageRating = 0;
                        neweval.SumRating = 0;
                        neweval.UseDss = false;
                        neweval.DssIsFuzzy = false;
                        neweval.DssRanking = 0;
                        neweval.DssValue = 0;
                        neweval.DssValueFuzzy = null;
                        neweval.Status = Eval.EvaluationStatus.None;
                        neweval.CreateMetaInfo(GetCurrentPerson(), UC.IpAddress, UC.ProxyIpAddress);
                        neweval.Submission = sub;
                        newEvals.Add(neweval);
                    }
                }
            }
            try
            {
                if (!Manager.IsInTransaction())
                    Manager.BeginTransaction();


                Manager.SaveOrUpdateList<Eval.Evaluation>(newEvals);

                Manager.Commit();

            }
            catch (Exception ex)
            {
                Manager.RollBack();
                return (int)SubmissionAssignAllErrorValue.InternalError;
            }

            return 0;
        }
        
        /// <summary>
        /// Recupera le valutazioni di una sottomissione
        /// </summary>
        /// <param name="idCall">Id bando</param>
        /// <param name="idSubmission">Id sottomissione</param>
        /// <param name="idCommittee">IdComitatno</param>
        /// <param name="unknownUser">Nome utente sconosciuto</param>
        /// <returns></returns>
        public List<Eval.dtoSubmissionCommitteeItem> GetSubmissionEvaluations(
            long idCall, 
            long idSubmission, 
            long idCommittee, 
            String unknownUser)
        {
            List<Eval.dtoSubmissionCommitteeItem> items = new List<Eval.dtoSubmissionCommitteeItem>();
            try
            {
                List<Eval.Export.expEvaluation> evaluations = (from e in Manager.GetIQ<Eval.Export.expEvaluation>()
                                                               where e.IdSubmission == idSubmission
                                                               && e.AdvCommission != null && e.AdvCommission.Id == idCommittee
                                                               && e.Deleted == Core.DomainModel.BaseStatusDeleted.None
                                                               select e).ToList();

                Advanced.Domain.AdvCommission commission = Manager.Get<Advanced.Domain.AdvCommission>(idCommittee);
                
                List<Advanced.Domain.AdvMember> members =
                        (from m in Manager.GetIQ<Advanced.Domain.AdvMember>()
                         where m.Deleted == Core.DomainModel.BaseStatusDeleted.None
                         && m.Commission != null
                         && m.Commission.Id == idCommittee 
                         select m).ToList();

                Eval.dtoSubmissionCommitteeItem item =
                        new Eval.dtoSubmissionCommitteeItem()
                        {
                            Name = commission.Name, //c.AdvCommission.Name,    //key
                            IdSubmission = idSubmission,
                            IdCommittee = commission.Id, //c.AdvCommission.Id,         //key
                            DssEvaluation = null, // dssEvaluations.Where(e => e.IdCommittee == c.Key.Id && e.IdSubmission == idSubmission).FirstOrDefault()
                            IsAdvance = true
                        };

                item.Criteria = (commission.Criteria != null) ? //(c.AdvCommission.Criteria != null) ?      //key
                       commission.Criteria  //c.AdvCommission.Criteria                              //key
                           .OrderBy(cr => cr.DisplayOrder)
                           .ThenBy(cr => cr.Name)
                           .Select(cr => CriterionToCriterionSumdto(cr, members.Count()))
                           .ToList()
                       : new List<Eval.dtoCriterionSummaryItem>();

                if (item.Criteria.Any())
                {
                    if (item.Criteria.Count == 1)
                        item.Criteria[0].DisplayAs = Domain.displayAs.first | Domain.displayAs.last;
                    else
                    {
                        item.Criteria.First().DisplayAs = Domain.displayAs.first;
                        item.Criteria.Last().DisplayAs = Domain.displayAs.last;
                    }
                }

                item.Evaluators = members
                       .Select(e =>
                       CreateEvaluatorDisplayItem(
                           idCommittee, // c.AdvCommission.Id,                   //key
                           idSubmission,
                           item.Criteria,
                           e,
                           evaluations.FirstOrDefault(g => g.AdvEvaluator != null && g.AdvEvaluator.Id == e.Id), //c,//.FirstOrDefault(g => g.IdEvaluator == e.Member.Id), 
                           unknownUser))
                       .OrderBy(m => m.Name)
                       .ToList();
                
                item.Criteria.ForEach(cr => cr.LoadEvaluations(item.Evaluators));
                
                items.Add(item);
            }
            catch (Exception ex)
            {

            }

            foreach (var itm in items)
            {
                foreach (var eval in itm.Criteria)
                {
                    eval.Evaluations = eval.Evaluations.OrderBy(e => e.Evaluator.Name).ThenBy(e => e.Evaluator.IdMembership).ToList();
                }
            }

            return items;
        }
        
        /// <summary>
        /// Valutazioni sottomissione
        /// </summary>
        /// <param name="idSubmission">Id sottomissione</param>
        /// <param name="idCall">Id bando</param>
        /// <param name="idCommission">Id commissione</param>
        /// <param name="unknownUser">Nome utente sconosciuto</param>
        /// <returns></returns>
        public List<Eval.dtoCommitteeEvaluationsDisplayItem> GetSubmissionEvaluationsDispItem(
            long idSubmission,
            long idCall,
            long idCommission,
            String unknownUser)
        {
            
            Eval.dtoCommitteeEvaluationsDisplayItem item = new Eval.dtoCommitteeEvaluationsDisplayItem();

            try
            {
                long idType = (from s in Manager.GetIQ<Domain.UserSubmission>() where s.Id == idSubmission select s.Type.Id).Skip(0).Take(1).ToList().FirstOrDefault();
                
                Advanced.Domain.AdvCommission comm = Manager.Get<Advanced.Domain.AdvCommission>(idCommission);

                item = new Eval.dtoCommitteeEvaluationsDisplayItem()
                {
                    IdCommittee = comm.Id,
                    CommitteeName = comm.Name,
                    Id = comm.Id,
                    DssEvaluation = null,
                    Display = Domain.displayAs.first | Domain.displayAs.last
                };
                
                List<Eval.dtoCommitteeEvaluatorEvaluationDisplayItem> evaluations = (from e in Manager.GetIQ<Eval.Export.expEvaluation>()
                                                                                     where e.IdSubmission == idSubmission
                                                                                     && e.AdvCommission != null && e.AdvCommission.Id == idCommission
                                                                                     && e.Deleted == Core.DomainModel.BaseStatusDeleted.None
                                                                                     select
                                                                                         Eval.dtoCommitteeEvaluatorEvaluationDisplayItem
                                                                                         .GetForEvaluationsDisplay(e, null, unknownUser))
                                                                                    .ToList();
                
                List<lm.Comol.Modules.CallForPapers.Domain.Evaluation.dtoCriterion> criteria = comm.Criteria.Select(bc =>
                    new lm.Comol.Modules.CallForPapers.Domain.Evaluation.dtoCriterion(bc)).ToList();
                
                item.Evaluations = evaluations.Where(e => e.IdCommittee == item.IdCommittee).ToList();
                if (item.Evaluations.Any() && item.Evaluations.Count == 1)
                    item.Evaluations[0].Display = Domain.displayAs.first | Domain.displayAs.last;
                else if (item.Evaluations.Any() && item.Evaluations.Count > 1)
                {
                    item.Evaluations.First().Display = Domain.displayAs.first;
                    item.Evaluations.Last().Display = Domain.displayAs.last;
                }

                foreach (Eval.dtoCommitteeEvaluatorEvaluationDisplayItem evaluation in item.Evaluations)
                {
                    List<Eval.CriterionEvaluated> values = (from cv in Manager.GetIQ<Eval.CriterionEvaluated>()
                                                            where cv.Evaluation.Id == evaluation.Id
                                                            && cv.Deleted == Core.DomainModel.BaseStatusDeleted.None
                                                            select cv)
                                                            .ToList();

                    evaluation.Criteria = criteria.OrderBy(c => c.DisplayOrder).ThenBy(c => c.Name).ToList().Select(c => new Eval.dtoCriterionEvaluatedDisplayItem(c, values.Where(v => v.Criterion.Id == c.Id).FirstOrDefault())).ToList();
                    if (evaluation.Criteria.Any() && evaluation.Criteria.Count == 1)
                        evaluation.Criteria[0].Display = Domain.displayAs.first | Domain.displayAs.last;
                    else if (evaluation.Criteria.Any() && evaluation.Criteria.Count > 1)
                    {
                        evaluation.Criteria.First().Display = Domain.displayAs.first;
                        evaluation.Criteria.Last().Display = Domain.displayAs.last;
                    }
                    foreach (Eval.dtoCriterionEvaluatedDisplayItem criterion in evaluation.Criteria)
                    {
                        criterion.IdCommittee = comm.Id;
                        criterion.IdEvaluator = evaluation.IdEvaluator;
                    }

                }
            }
            catch (Exception ex)
            {

            }


            List<Eval.dtoCommitteeEvaluationsDisplayItem> items = new List<Eval.dtoCommitteeEvaluationsDisplayItem>();
            items.Add(item);
            return items;
        }
        
        /// <summary>
        /// Permessi visualizzazione lista sottomissioni
        /// </summary>
        /// <param name="callId">Id bando</param>
        /// <returns>Oggetto permessi lista sottomissioni</returns>
        public SubmissionListPermission SubmissionCanList(Int64 callId)
        {


            IList<Advanced.Domain.AdvCommission> advCommissions = Manager.GetAll<Advanced.Domain.AdvCommission>(c =>
                c.Call != null && c.Call.Id == callId
                );

            if (advCommissions == null || !advCommissions.Any())
                return SubmissionListPermission.None;


            SubmissionListPermission permission = SubmissionListPermission.None;

            if (advCommissions.Any(c => c.Status == CommissionStatus.Closed))
                permission |= SubmissionListPermission.View;

            if (advCommissions.Any(c =>
                (
                (c.President != null && c.President.Id == UC.CurrentUserID)
                || (c.Secretary != null && c.Secretary.Id == UC.CurrentUserID)
                || (c.Members.Any(m => m.Member != null && m.Member.Id == UC.CurrentUserID)
                )
                && (
                    c.Status == CommissionStatus.ViewSubmission
                    || c.Status == CommissionStatus.Started
                    || c.Status == CommissionStatus.Locked
                    || c.Status == CommissionStatus.Closed
                    )
                )))
                permission |= SubmissionListPermission.View;


            if (advCommissions.Any(c =>
                (
                (c.President != null && c.President.Id == UC.CurrentUserID)
                || (c.Secretary != null && c.Secretary.Id == UC.CurrentUserID)
                || (c.Members.Any(m => m.Member != null && m.Member.Id == UC.CurrentUserID)
                )
                && c.Status == CommissionStatus.Started
                )
                ))
            {
                permission |= SubmissionListPermission.View;
                permission |= SubmissionListPermission.Evaluate;
            }

            if (advCommissions.Any(c =>
                (c.President != null && c.President.Id == UC.CurrentUserID)
                || (c.Secretary != null && c.Secretary.Id == UC.CurrentUserID)
                ))
            {
                permission |= SubmissionListPermission.Manage;
            }

            return permission;
        }
        #endregion

        #region Integration
        /// <summary>
        /// Aggiunge integrazione (richiesta)
        /// </summary>
        /// <param name="CommId">Id Commissione</param>
        /// <param name="Text">Testo richiesta</param>
        /// <param name="intType">Tipo integrazione</param>
        /// <param name="submissionId">Id sottomissione</param>
        /// <param name="submissionFieldId">Id campo</param>
        /// <param name="submitterId">Id sottomittore</param>
        /// <param name="integrationId">Id integrazione</param>
        /// <param name="send">Invia</param>
        /// <returns></returns>
        public bool IntegrationAdd(
            long CommId, string Text, IntegrationType intType,
            long submissionId, long submissionFieldId, int submitterId, 
            long integrationId, bool send)
        {
            Advanced.Domain.AdvCommission Comm = Manager.Get<Advanced.Domain.AdvCommission>(CommId);

            if (Comm == null)
                return false;


            if (Comm.Secretary == null || Comm.Secretary.Id != UC.CurrentUserID)
                return false;


            Core.DomainModel.litePerson currentPerson = GetCurrentPerson();


            Advanced.Domain.AdvEvalIntegration integration = null;

            if(integrationId > 0)
                integration = Manager.Get<Advanced.Domain.AdvEvalIntegration>(integrationId);

            if(integration == null)
            {
                integration = new Advanced.Domain.AdvEvalIntegration();
                integration.CreateMetaInfo(currentPerson, UC.IpAddress, UC.ProxyIpAddress);
            } else
            {
                integration.UpdateMetaInfo(currentPerson, UC.IpAddress, UC.ProxyIpAddress);
            }
            

            integration.Commission = Comm;
            integration.Link = null;
            integration.Secretary = currentPerson;

            integration.SecretaryText = Text;
            integration.SubmissionFieldId = submissionFieldId;
            integration.SubmissionId = submissionId;
            integration.Submitter = Manager.GetLitePerson(submitterId);

            integration.SubmitterText = "";
            integration.Type = intType;


            if (send)
            {
                //ToDo: SEND!

                integration.ReqSended = true;
                integration.ReqSendedOn = DateTime.Now;

            }





            if (!Manager.IsInTransaction())
            {
                Manager.BeginTransaction();
            }

            try
            {
                Manager.SaveOrUpdate(integration);
                Manager.Commit();

            }
            catch (Exception ex)
            {
                Manager.RollBack();
                return false;
            }

            


            return true;
        }
        
        /// <summary>
        /// Risposta integrazione
        /// </summary>
        /// <param name="integrationId">Id integrazione</param>
        /// <param name="Text">Testo risposta</param>
        /// <param name="send">Invia</param>
        /// <remarks>Gli allegati sono salvati a parte</remarks>
        /// <returns></returns>
        public bool IntegrationAnswer(
           long integrationId, string Text,
            bool send)
        {
            Advanced.Domain.AdvEvalIntegration integration = Manager.Get<Advanced.Domain.AdvEvalIntegration>(integrationId);

            if(integration == null || integration.Submitter == null || integration.Submitter.Id != UC.CurrentUserID)
            {
                return false;
            }

            integration.UpdateMetaInfo(GetCurrentPerson(), UC.IpAddress, UC.ProxyIpAddress);
            integration.SubmitterText = Text;
            
            if(send)
            {
                integration.AnswerSended = true;
                integration.AnswerSendedOn = DateTime.Now;
            }

            if (!Manager.IsInTransaction())
            {
                Manager.BeginTransaction();
            }

            try
            {
                Manager.SaveOrUpdate(integration);
                Manager.Commit();

            }
            catch (Exception ex)
            {
                Manager.RollBack();
                return false;
            }

            return true;
        }

        /// <summary>
        /// Recuepra l'ID del sottomittore
        /// </summary>
        /// <param name="submissionId">Id sottomissione</param>
        /// <returns></returns>
        public int IntegrationGetSubmitterId(long submissionId)
        {
            int id = (from lm.Comol.Modules.CallForPapers.Domain.UserSubmission sub in Manager.GetIQ<lm.Comol.Modules.CallForPapers.Domain.UserSubmission>()
                       where sub.Id == submissionId
                       select (sub.SubmittedBy != null) ? sub.SubmittedBy.Id : sub.CreatedBy.Id)
                       .Skip(0).Take(1).ToList().FirstOrDefault();

            return id;

        }

        /// <summary>
        /// Recupera un integrazione
        /// </summary>
        /// <param name="IntegrId">Id integrazione</param>
        /// <returns></returns>
        public Advanced.Domain.AdvEvalIntegration GetIntegration(long IntegrId)
        {
            return Manager.Get<Advanced.Domain.AdvEvalIntegration>(IntegrId);
        }

        /// <summary>
        /// Recupera le integrazioni
        /// </summary>
        /// <param name="CommId">Id Commissione</param>
        /// <param name="submissionId">Id sottomissione</param>
        /// <param name="submissionFieldId">Id campo</param>
        /// <param name="submitterId">Id sottomittore</param>
        /// <returns></returns>
        public dtoIntegration GetIntegrations(
            long CommId,
            long submissionId, 
            long submissionFieldId, 
            int submitterId)
        {   

            dtoIntegration dto = new dtoIntegration();

            bool IsForSubmitter = false;

            if(CommId <= 0)
            {
                Domain.UserSubmission Submission = Manager.Get<Domain.UserSubmission>(submissionId);
                if(Submission.SubmittedBy != null && Submission.SubmittedBy.Id == UC.CurrentUserID)
                {
                    IsForSubmitter = true;
                }
            }
            
            Advanced.Domain.AdvCommission commission = Manager.Get<Advanced.Domain.AdvCommission>(CommId);

            //if (commission == null && !IsForSubmitter)
            //    return dto;

            int SecrId = 0;
            bool isSecretary = false;
            if (commission == null || IsForSubmitter)
            {
                //Generico (Utente: id commissione == 0!!!
                dto.items = Manager.GetAll<Advanced.Domain.AdvEvalIntegration>(
                    it => it.Deleted == Core.DomainModel.BaseStatusDeleted.None
                    //&& it.Commission != null && it.Commission.Id == CommId
                    && it.SubmissionId == submissionId
                    && it.SubmissionFieldId == submissionFieldId
                    && it.Submitter != null && it.Submitter.Id == submitterId
                    && it.ReqSended
                    )
                    .Select(i => new dtoIntegrationItem(i))
                    .ToList();

            } else
            {
                //Per segretario!
                SecrId = (commission.Secretary != null) ? commission.Secretary.Id : 0;
                isSecretary = SecrId == UC.CurrentUserID;


                dto.items = Manager.GetAll<Advanced.Domain.AdvEvalIntegration>(
                    it => it.Deleted == Core.DomainModel.BaseStatusDeleted.None
                    //&& it.Commission != null && it.Commission.Id == CommId
                    && it.SubmissionId == submissionId
                    && it.SubmissionFieldId == submissionFieldId
                    && it.Submitter != null && it.Submitter.Id == submitterId
                    && (isSecretary || it.ReqSended)
                    )
                    .Select(i => new dtoIntegrationItem(i))
                    .ToList();
            }

            dto.SecretaryId = SecrId;
            dto.IsSecretary = isSecretary && (commission != null && commission.Status == CommissionStatus.Started);
            dto.IsSubmitter = submitterId == UC.CurrentUserID;
            

            dto.CanView = (
                dto.IsSecretary
                || dto.IsSubmitter
                || ((commission != null) &&
                    ((commission.President != null && commission.President.Id == UC.CurrentUserID)
                    || (commission.Members != null && commission.Members.Any(m => m.Member.Id == UC.CurrentUserID))
                    )
                )
                );
            
            return dto;
        }
        /// <summary>
        /// Associa il file caricato all'integrazione
        /// </summary>
        /// <param name="idIntegration">Id integrazione</param>
        /// <param name="Link">Oggetto ModuleActionLink (allegato)</param>
        /// <param name="Text">Testo risposta</param>
        /// <param name="send">Se impostare la risposta come inviata o lasciarla in bozza</param>
        /// <returns>True se l'operazione ha successo</returns>
        public bool AdvIntegrationAddFile(long idIntegration, lm.Comol.Core.DomainModel.ModuleActionLink Link, string Text, bool send)
        {
            Advanced.Domain.AdvEvalIntegration advIntegration = Manager.Get<Advanced.Domain.AdvEvalIntegration>(idIntegration);

            if (advIntegration == null
                || advIntegration.Submitter == null
                || advIntegration.Submitter.Id != UC.CurrentUserID
                )
                return false;

            if (!Manager.IsInTransaction())
                Manager.BeginTransaction();

            advIntegration.UpdateMetaInfo(GetCurrentPerson(), UC.IpAddress, UC.ProxyIpAddress);


            lm.Comol.Core.DomainModel.ModuleLink mLink = new lm.Comol.Core.DomainModel.ModuleLink(
                Link.Description,
                Link.Permission,
                Link.Action);

            //ToDo: CHECK NULL!!!

            mLink.CreateMetaInfo(Manager.GetPerson(UC.CurrentUserID), UC.IpAddress, UC.ProxyIpAddress);


            int communityId = 0;
            try
            {
                communityId = advIntegration.Commission.Step.Call.Community.Id;
            }
            catch { }

            if (communityId == 0)
                communityId = UC.CurrentCommunityID;

            mLink.DestinationItem = (Core.DomainModel.ModuleObject)Link.ModuleObject;
            mLink.SourceItem = Core.DomainModel.ModuleObject.CreateLongObject(
                advIntegration.Id,
                advIntegration,
                (int)lm.Comol.Modules.CallForPapers.Domain.ModuleCallForPaper.ObjectType.Integrazioni,
                communityId,
                lm.Comol.Modules.CallForPapers.Domain.ModuleCallForPaper.UniqueCode,
                ServiceModuleID(lm.Comol.Modules.CallForPapers.Domain.ModuleCallForPaper.UniqueCode));

            advIntegration.Link = mLink;
            advIntegration.SubmitterText = Text;

            if(send)
            {
                advIntegration.AnswerSended = true;
                advIntegration.AnswerSendedOn = DateTime.Now;
            }

            try
            {
                Manager.SaveOrUpdate<lm.Comol.Core.DomainModel.ModuleLink>(mLink);
                Manager.SaveOrUpdate<Advanced.Domain.AdvEvalIntegration>(advIntegration);
                Manager.Commit();
            }
            catch (Exception ex)
            {
                Manager.RollBack();
                return false;
            }

            return true;
        }
        /// <summary>
        /// Verifica se mostrare le integrazioni inviate per una sottomissione in una commissione
        /// </summary>
        /// <param name="SubmissionId">Id sottomissione</param>
        /// <param name="CommissionId">Id commissione</param>
        /// <returns></returns>
        public bool ShowSendIntegration(long SubmissionId, long CommissionId)
        {
            if (CommissionId == 0 || SubmissionId == 0)
                return false;


            Advanced.Domain.AdvSubmissionToStep sub = Manager.GetAll<Advanced.Domain.AdvSubmissionToStep>(sb =>
                sb.Submission != null && sb.Submission.Id == SubmissionId
                && sb.Commission != null && sb.Commission.Id == CommissionId).FirstOrDefault();
            
            if (sub != null 
                && sub.Commission != null 
                && sub.Commission.Secretary != null 
                && sub.Commission.Secretary.Id == UC.CurrentUserID
                && 
                    (sub.Commission.Status == CommissionStatus.Started)
                )
                return true;

            //if (Manager.GetAll<Advanced.Domain.AdvEvalIntegration>(
            //    i => i.Commission != null
            //    && i.Commission.Id == CommissionId
            //    && i.ReqSended && !i.AnswerSended).Any())
            //    return true;

            return false;
        }
        /// <summary>
        /// Verifica i permessi di download di un integrazione
        /// </summary>
        /// <param name="idIntegration">Id integrazione</param>
        /// <param name="idUser">Id utente</param>
        /// <param name="person">Utente corrente</param>
        /// <param name="idCommunity">Id comunità</param>
        /// <param name="idRole">Id ruolo</param>
        /// <returns>True se l'utente puo' scaricare l'integrazione</returns>
        protected override bool AllowDownloadIntegrazioni(long idIntegration, int idUser, lm.Comol.Core.DomainModel.litePerson person, int idCommunity, int idRole)
        {
            Advanced.Domain.AdvEvalIntegration integration = Manager.Get<Advanced.Domain.AdvEvalIntegration>(idIntegration);

            if (integration == null || integration.Deleted != Core.DomainModel.BaseStatusDeleted.None)
                return false;

            if (integration.Submitter != null && integration.Submitter.Id == idUser)
                return true;

            long callid = 0;
            try
            {
                callid = integration.Commission.Step.Call.Id;
            }
            catch (Exception ex)
            {

            }

            int CommunityId = 0;

            try
            {
                CommunityId = integration.Commission.Step.Call.Community.Id;
            }
            catch
            {

            }

            if (callid == 0 || CommunityId == 0)
                return true;


            return UserIsCallManager(CommunityId, idUser) || UserIsInAdvanceEvaluationCommission(idUser, callid);
        }
        
        /// <summary>
        /// Notifica la richiesta di integrazioni
        /// </summary>
        /// <param name="SubmissionId">Id sottomissione</param>
        /// <param name="CommissionId">Id commissione</param>
        /// <param name="smtpConfig">Configurazione SMTP</param>
        /// <param name="IdSkin">Id skin corrente</param>
        /// <param name="Subject">Testo oggetto della mail (se vuoto: "Nome bando - Richiesta integrazioni")</param>
        /// <param name="Body">Testo corpo della mail (se vuoto: "Nome bando - Link integrazione - data integrazione</param>
        /// <param name="IntegrationEndOn">Data scadenza compilazione</param>
        /// <param name="BaseUrl">Base Url dell'applicativo</param>
        /// <param name="AddCreator">Se true aggiunge il creatore del bando in copia nascosta</param>
        /// <param name="AddCurrent">Se true aggiunge l'utente corrente (segretario) in copia nascosta</param>
        /// <returns>True se la mail è inviata con successo</returns>
        /// <remarks>
        /// Nell'oggetto e nel corpo del testo, sono previsti i seguenti TAG che vengono in automatico sostituiti:
        /// [Call.Name]                 Nome del bando
        /// [Call.Integration.Url]      Url diretto alla sottomissione
        /// [Call.Integration.EndOn]    Data scadenza per la compilazione
        /// </remarks>
        public bool SendIntegration(
            long SubmissionId,
            long CommissionId,
            lm.Comol.Core.MailCommons.Domain.Configurations.SmtpServiceConfig smtpConfig,
            long IdSkin,
            string Subject = "[Call.Name] - Richiesta integrazioni",
            string Body = "[Call.Name] - [Call.Integration.Url] - [Call.Integration.EndOn]",
            string IntegrationEndOn = "",
            string BaseUrl = "https://agora.trentinosviluppo.it/",
            bool AddCreator = false,
            bool AddCurrent = true
            )
        {

            Advanced.Domain.AdvCommission Commission = Manager.Get<Advanced.Domain.AdvCommission>(CommissionId);
            Domain.UserSubmission Submission = Manager.Get<Domain.UserSubmission>(SubmissionId);

            if (Commission == null || Commission.Secretary.Id != UC.CurrentUserID
                || Submission == null || Submission.Call == null || Submission.Call.Community == null)
                return false;

            bool hasError = false;
            string error = "";

            try
            {

                string url = Domain.RootObject.ViewSubmission(
                    Submission.Call.Type,
                    Submission.Call.Id,
                    Submission.Id,
                    false,
                    Domain.CallStatusForSubmitters.Submitted,
                    (Submission.Community != null) ? Submission.Community.Id : Submission.Call.Community.Id,
                    Commission.Id);

                Subject = Subject.Replace("[Call.Name]", Submission.Call.Name);

                String Anchor = String.Format("<a href={0}>{1}</a>", String.Format("{0}{1}", BaseUrl, url), BaseUrl);

                Body = Body.Replace("[Call.Name]", Submission.Call.Name);
                Body = Body.Replace("[Call.Integration.Url]", Anchor);
                Body = Body.Replace("[Call.Integration.EndOn]", IntegrationEndOn);

                Core.MailCommons.Domain.Messages.MessageSettings msgSet = new Core.MailCommons.Domain.Messages.MessageSettings();
                msgSet.CopyToSender = true;
                msgSet.IdSkin = IdSkin;
                msgSet.IsBodyHtml = true;
                msgSet.NotifyToSender = false;
                msgSet.PrefixType = Core.MailCommons.Domain.SubjectPrefixType.SystemConfiguration;
                msgSet.SenderType = Core.MailCommons.Domain.SenderUserType.System;
                msgSet.Signature = Core.MailCommons.Domain.Signature.FromConfigurationSettings;

                lm.Comol.Core.Mail.MailService mailService =
                            new lm.Comol.Core.Mail.MailService(smtpConfig, msgSet);

                lm.Comol.Core.Mail.dtoMailMessage message =
                            new lm.Comol.Core.Mail.dtoMailMessage(Subject,
                            Body);

                message.FromUser = smtpConfig.GetSystemSender();
                message.BCC = new List<System.Net.Mail.MailAddress>();

                if (AddCreator)
                {
                    try
                    {
                        message.BCC.Add(new System.Net.Mail.MailAddress(Submission.Call.NotificationEmail));
                    }
                    catch (Exception ex)
                    {
                    }
                }
                if (AddCurrent)
                {
                    try
                    {
                        message.BCC.Add(new System.Net.Mail.MailAddress(UC.CurrentUser.Mail));
                    }
                    catch (Exception ex)
                    {
                    }
                }


                Core.DomainModel.Language language = Manager.Get<Core.DomainModel.Language>(UC.Language.Id);

                mailService.SendMail(
                    UC.Language.Id,
                    language,
                    message,
                    Submission.SubmittedBy.Mail,
                    lm.Comol.Core.MailCommons.Domain.RecipientType.To);
            }
            catch (Exception ex)
            {
                hasError = true;
                error = ex.ToString();
            }

            //if (hasError)
            //{
            //    try
            //    {
            //        //dtoSubmitterTemplateMail submitterTemplate = GetSubmitterTemplateMail(submission,
            //        //   submitter);

            //        //Int32 idUserLanguage =
            //        //   (submitter.TypeID == (int)UserTypeStandard.Guest ||
            //        //    submitter.TypeID == (int)UserTypeStandard.PublicUser)
            //        //       ? 0
            //        //       : submitter.LanguageID;

            //        //Language dLanguage = Manager.GetDefaultLanguage();

            //        //lm.Comol.Core.Mail.dtoMailMessage errmessage =
            //        //    new lm.Comol.Core.Mail.dtoMailMessage("ERROR", error);

            //        //lm.Comol.Core.Mail.MailService mailService =
            //        //            new lm.Comol.Core.Mail.MailService(smtpConfig,
            //        //                submitterTemplate.MailSettings);

            //        //mailService.SendMail(idUserLanguage, dLanguage, errmessage, "mborsato@edutech.it",
            //        //            lm.Comol.Core.MailCommons.Domain.RecipientType.To);
            //    }
            //    catch (Exception)
            //    {

            //        //throw;
            //    }

            //}
            return !hasError;
        }

        #endregion

        #region Comments
        /// <summary>
        /// Recupera i commenti per le commissioni avanzate
        /// </summary>
        /// <param name="CommissionId">Id commissione</param>
        /// <param name="SubmissionId">Id sottomissione</param>
        /// <returns>Lista dto commenti</returns>
        public IList<Advanced.dto.dtoAdvComment> AdvCommentsGet(long CommissionId, long SubmissionId)
        {
            IList<Advanced.dto.dtoAdvComment> GenericComments = new List<Advanced.dto.dtoAdvComment>();

            //
            //&& ev.AdvCommission.Deleted == Core.DomainModel.BaseStatusDeleted.None

            GenericComments = Manager.GetAll<Eval.Evaluation>(ev =>
                ev.AdvCommission != null && ev.AdvCommission.Id == CommissionId
                && ev.Submission != null && ev.Submission.Id == SubmissionId
                && ev.Comment != null && ev.Comment != ""
                ).OrderBy(ev => ev.CreatedOn)
                .Select(ev => new Advanced.dto.dtoAdvComment
                {
                    isDraft = ev.Status == Eval.EvaluationStatus.None,
                    MemberName = (ev.AdvEvaluator != null && ev.AdvEvaluator.Member != null) ? ev.AdvEvaluator.Member.SurnameAndName : "--",
                    Comment = ev.Comment,
                    IsCriteria = false,
                    CriteriaName = "",
                    SaveOn = (ev.ModifiedOn != null) ? ev.ModifiedOn : ev.CreatedOn
                })
                .ToList()
                ;

            IList<Advanced.dto.dtoAdvComment> CriteriaComments = new List<Advanced.dto.dtoAdvComment>();
            CriteriaComments = Manager.GetAll<Eval.Export.expCriterionEvaluated>(ec =>
               ec.Criterion != null 
               && ec.Evaluation.Deleted == Core.DomainModel.BaseStatusDeleted.None
               && ec.Evaluation != null && ec.Evaluation.AdvCommission != null && ec.Evaluation.AdvCommission.Id == CommissionId
               && ec.Evaluation.IdSubmission == SubmissionId).Select(ec =>
                new Advanced.dto.dtoAdvComment
                {
                    isDraft = ec.Evaluation.Status == Eval.EvaluationStatus.None,
                    MemberName = (ec.Evaluation.AdvEvaluator != null && ec.Evaluation.AdvEvaluator.Member != null) ? ec.Evaluation.AdvEvaluator.Member.SurnameAndName : "--",
                    Comment = ec.Comment,
                    IsCriteria = true,
                    CriteriaName = ec.Criterion.Name,
                    SaveOn = (ec.Evaluation.LastUpdateOn != null) ? ec.Evaluation.LastUpdateOn : ec.Evaluation.EvaluationStartedOn
                }).ToList();

            return GenericComments.Union(CriteriaComments).OrderBy(cm => cm.SaveOn).ToList();

        }
        
        #endregion
             
        #region Export
            
        /// <summary>
        /// Esporta i dati della commissione (Di default in HTML)
        /// </summary>
        /// <param name="CommissionId">Id Commissione</param>
        /// <param name="CommissionHelper">HTMLHelper commissione</param>
        /// <param name="SummaryHelper">Hepler sommario</param>
        /// <returns></returns>
        public string ExportCommission(long CommissionId, 
            Advanced.Helpers.CommissionHTMLExpHelper CommissionHelper = null,
            Advanced.Helpers.CommissionSummaryHTMLexpHelper SummaryHelper = null)
        {
            string HTML = "";


            if (!AllowDownloadVerbali(CommissionId, UC.CurrentUserID, GetCurrentPerson(), 0, 0))
                return HTML;
            
            Advanced.Domain.AdvCommission commission = Manager.Get<Advanced.Domain.AdvCommission>(CommissionId);

            if (commission == null || commission.Step == null)
                return HTML;

            if (CommissionHelper == null)
                CommissionHelper = new Advanced.Helpers.CommissionHTMLExpHelper();
            try
            {
                HTML = CommissionHelper.CommissionData(commission);
            } catch(Exception ex)
            {

            }

            if(commission.Step.Type != StepType.economics)
            { 
           
                IList<Advanced.Domain.AdvSubmissionToStep> comSubmission = Manager.GetAll<Advanced.Domain.AdvSubmissionToStep>(s =>
                s.Commission != null && s.Commission.Id == CommissionId
                );
           

                if (comSubmission == null || !comSubmission.Any())
                    return HTML;

                if (SummaryHelper == null)
                    SummaryHelper = new Advanced.Helpers.CommissionSummaryHTMLexpHelper();
                try
                {
                    SummaryHelper.ItemContent = "{0}{1}{2}{3}{4}{5}{6}";
                    HTML = String.Format("{0}{1}", HTML,
                    SummaryHelper.Summary(comSubmission)
                    );
                }
                catch (Exception ex)
                {

                }
            } else
            {
                lm.Comol.Modules.CallForPapers.AdvEconomic.dto.dtoEcoSummaryContainer EcoSummary = EcoSummaryGet(CommissionId);
                lm.Comol.Modules.CallForPapers.AdvEconomic.Helpers.EconomicSummaryHTMLexpHelper EcoHelper = new AdvEconomic.Helpers.EconomicSummaryHTMLexpHelper();

                HTML = String.Format("{0}{1}", 
                    HTML,
                    EcoHelper.EcoSummaryGetHTMLTable(EcoSummary)
                    );

            }

            return HTML;
        }


        /// <summary>
        /// Stringa con l'espotazione delle statistiche di un valutatore
        /// </summary>
        /// <param name="call">Id bando</param>
        /// <param name="filters">filtri</param>
        /// <param name="anonymousDisplayName">Nome utente anonimo (localizzato)</param>
        /// <param name="unknownUserName">Nome utente sconosciuto (localizzato)</param>
        /// <param name="idCommission">Id commissione</param>
        /// <param name="idEvaluator">Id valutatore</param>
        /// <param name="applyFilters">Filtri</param>
        /// <param name="fileType">Tipo di esportazione</param>
        /// <param name="translations">Traduzioni etichette</param>
        /// <param name="status">traduzione stati</param>
        /// <returns></returns>
        public String ExportEvaluatorStatistics(
            Domain.dtoCall call,
            Eval.dtoEvaluationsFilters filters,
            String anonymousDisplayName,
            String unknownUserName,
            long idCommission,
            long idEvaluator,
            Boolean applyFilters,
            lm.Comol.Core.DomainModel.Helpers.Export.ExportFileType fileType,
            Dictionary<lm.Comol.Modules.CallForPapers.Domain.Evaluation.EvaluationTranslations, String> translations,
            Dictionary<lm.Comol.Modules.CallForPapers.Domain.Evaluation.EvaluationStatus, String> status)
        {
            List<Eval.dtoEvaluatorCommitteeStatistic> statistics = null;
            lm.Comol.Core.DomainModel.litePerson person = null;
            try
            {
                person = Manager.GetLitePerson(UC.CurrentUserID);

                Advanced.Domain.AdvCommission commission = Manager.Get<Advanced.Domain.AdvCommission>(idCommission);

                Domain.EvaluationType EvalType = Domain.EvaluationType.Average;
                if (commission.EvalType == Advanced.EvalType.Sum)
                    EvalType = Domain.EvaluationType.Sum;



                if (idCommission > 0)
                {
                    Eval.dtoEvaluatorCommitteeStatistic statistic =
                            GetEvaluatorCommitteeStatistics(
                                EvalType,
                                filters,
                                anonymousDisplayName,
                                unknownUserName,
                                idCommission,
                                idEvaluator,
                                applyFilters);
                    if (statistic != null)
                    {
                        statistics = new List<Eval.dtoEvaluatorCommitteeStatistic>();
                        statistics.Add(statistic);
                    }
                }
                else
                {
                    List<long> idCommissions = new List<long>();
                    idCommissions.Add(idCommission);

                    //idCommittees.AddRange((from m in Manager.GetIQ<Advanced.Domain.AdvMember>()
                    //                       where 
                    //                        m.Commission != null && m.Commission.Id == idCommission
                    //                        && m.Member != null
                    //                        && m.Deleted == Core.DomainModel.BaseStatusDeleted.None 
                    //                        && m.Id == idEvaluator 
                    //                       select m.IdCommittee).ToList());

                    Eval.dtoEvaluatorCommitteeStatistic stat = GetEvaluatorStatistics(
                        call.EvaluationType,
                        filters,
                        anonymousDisplayName,
                        unknownUserName,
                        commission,
                        idEvaluator,
                        applyFilters);


                    statistics.Add(stat);
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
                        return helperCsv.ExportEvaluatorStatistics(call, statistics, person); ;
                }
            }
            catch (Exception ex)
            {
                return GetErrorDocument(call, person, fileType, translations, status);
            }
            return GetErrorDocument(call, person, fileType, translations, status);
        }

        /// <summary>
        /// Genera un documento di default in caso di errori
        /// </summary>
        /// <param name="call">Bando</param>
        /// <param name="person">Utente</param>
        /// <param name="fileType">Tipo di esportazione</param>
        /// <param name="translations">Traduzioni etichette</param>
        /// <param name="status">Traduzioni stato</param>
        /// <returns>Stringa con il documento di errore</returns>
        public String GetErrorDocument(
            Domain.dtoCall call,
            Core.DomainModel.litePerson person,
            lm.Comol.Core.DomainModel.Helpers.Export.ExportFileType fileType,
            Dictionary<lm.Comol.Modules.CallForPapers.Domain.Evaluation.EvaluationTranslations, String> translations,
            Dictionary<lm.Comol.Modules.CallForPapers.Domain.Evaluation.EvaluationStatus, String> status)
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


        /// <summary>
        /// Recupera l'elenco sottomissioni con l'indicazione delle integrazioni
        /// </summary>
        /// <param name="allowManage">Se è abilitato a gestire le sottomissioni</param>
        /// <param name="idCall">Id bando</param>
        /// <param name="filters">Filtri</param>
        /// <param name="pageIndex">Indice pagina</param>
        /// <param name="pageSize">Dimensione pagina</param>
        /// <returns>Lista di sottomissioni</returns>
        public List<Domain.dtoSubmissionDisplayItemPermission> GetSubmissionListIntegration(
            Boolean allowManage,
            long idCall,
            Domain.dtoSubmissionFilters filters,
            int pageIndex,
            int pageSize)
        {
            List<Domain.dtoSubmissionDisplayItemPermission> submissions = null;

            try
            {
                List<Domain.dtoSubmissionDisplay> items = GetSubmissionList(idCall, filters, pageIndex, pageSize);

                submissions = (from s in items
                               select new Domain.dtoSubmissionDisplayItemPermission()
                               {
                                   Id = s.Id,
                                   Deleted = s.Deleted,
                                   Submission = SubmissionUpdateRevisionCount(s),
                                   Permission = new Domain.dtoSubmissionDisplayPermission(s, allowManage, filters.CallType)
                               }
                               ).ToList();
            }
            catch (Exception ex)
            {

            }
            return submissions;
        }

        /// <summary>
        /// Aggiorna il numero di richieste di integrazione inviate/ricevute in un dto sottomissione
        /// </summary>
        /// <param name="submission">dto sottomissione</param>
        /// <returns>il dto con i dati aggiornati</returns>
        private Domain.dtoSubmissionDisplay SubmissionUpdateRevisionCount(Domain.dtoSubmissionDisplay submission)
        {

            long SubId = submission.Id;

            if (submission != null)
            {
                var query = from Advanced.Domain.AdvEvalIntegration integr in Manager.GetIQ<Advanced.Domain.AdvEvalIntegration>()
                            where integr.SubmissionId == SubId && integr.Deleted == Core.DomainModel.BaseStatusDeleted.None
                            select integr;

                submission.RevisionSended = query.Count(integr => integr.ReqSended && !integr.AnswerSended);
                submission.RevisionAswered = query.Count(integr => integr.ReqSended && integr.AnswerSended);
            }

            return submission;
        }


        #endregion
        
    }
   
}
