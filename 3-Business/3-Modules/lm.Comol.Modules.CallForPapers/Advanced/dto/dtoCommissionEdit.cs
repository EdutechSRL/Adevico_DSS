using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel;

using Eval = lm.Comol.Modules.CallForPapers.Domain.Evaluation;

namespace lm.Comol.Modules.CallForPapers.Advanced.dto
{
    /// <summary>
    /// dto Edit commissione
    /// </summary>
    public class dtoCommissionEdit
    {
        /// <summary>
        /// Id
        /// </summary>
        public Int64 Id { get; set; }
        /// <summary>
        /// Nome
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Descrizione
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// Stato commissione
        /// </summary>
        public CommissionStatus Status { get; set; }
        /// <summary>
        /// Dati presidente
        /// </summary>
        public virtual litePerson President { get; set; }
        /// <summary>
        /// Dati segretario
        /// </summary>
        public virtual litePerson Secretary { get; set; }
        /// <summary>
        /// Lista membri
        /// </summary>
        public virtual IList<litePerson> Members { get; set; }
        /// <summary>
        /// Lista criteri previsti
        /// </summary>
        public virtual IList<Eval.dtoCriterion> Criterion { get; set; }
        /// <summary>
        /// Ordinamento dello step padre
        /// </summary>
        public int StepOrder { get; set; }
        /// <summary>
        /// Nome dello step padre
        /// </summary>
        public string StepName { get; set; }
        /// <summary>
        /// Sato dello step padre
        /// </summary>
        public StepStatus StepStatus { get; set; }
        /// <summary>
        /// Nome del bando di riferimento
        /// </summary>
        public string CallName { get; set; }
        /// <summary>
        /// Se è la commissione "Master"
        /// </summary>
        public bool IsMaster { get; set; }
        /// <summary>
        /// Se puo' essere abilitata come Master
        /// </summary>
        public bool EnableMaster { get; set; }
        /// <summary>
        /// SE la commissione puo' essere modificata (logiche di business, basate sullo stato della commissione)
        /// </summary>
        public bool canEdit { get; set; }
        /// <summary>
        /// Permessi
        /// </summary>
        public CommissionPermission Permission { get; set; }
        /// <summary>
        /// Tipo valutazione
        /// </summary>
        public EvalType EvalType { get; set; }
        /// <summary>
        /// Tipo valutazione
        /// </summary>
        public EvalType StepEvalType { get; set; }
        /// <summary>
        /// Punteggio minimo per il superamento
        /// </summary>
        public int EvalMinValue { get; set; }
        /// <summary>
        /// Se i valori binari sono cloccanti
        /// </summary>
        public bool EvalBoolBlock { get; set; }
        /// <summary>
        /// TAG del bando (tutti, per selezione)
        /// </summary>
        public string CallTags { get; set; }
        /// <summary>
        /// Tag assegnati alla commissione
        /// </summary>
        public string CommissionTags { get; set; }
        /// <summary>
        /// Se è una commissione Economica
        /// </summary>
        public bool isEconomic { get; set; }
        //public bool CanAssignSubmission { get; set; }
        //public bool HasAssignSubmission { get; set; }
        /// <summary>
        /// Link verbale
        /// </summary>
        public ModuleLink VerbaleLink { get; set; }

        /// <summary>
        /// Indica se l'utente corrente è il presidente della commissione
        /// </summary>
        public bool IsPresident { get; set; }

        /// <summary>
        /// Massimale ammesso per commissione economica
        /// </summary>
        public double MaxValue { get; set; }

        /// <summary>
        /// Costruttore vuoto
        /// </summary>
        public dtoCommissionEdit(Double MaxValue)
        {
            Id = 0;
            Name = "";
            Description = "";
            Status = CommissionStatus.Draft;
            President = null;
            Secretary = null;
            Members = new List<litePerson>();

            Criterion = null;

            StepOrder = 0;
            StepName = "";
            StepStatus = StepStatus.Draft;
            CallName = "";
            IsMaster = true;

            EnableMaster = false;

            Permission = CommissionPermission.None;

            EvalType = EvalType.Average;    //Dafult: impostato AVERAGE!
            EvalMinValue = 0;
            EvalBoolBlock = false;

            VerbaleLink = null;

            this.MaxValue = MaxValue;
        }


        /// <summary>
        /// Costruttore da oggetto dominio
        /// </summary>
        /// <param name="Comm">Commissione</param>
        /// <param name="currentUserId">Id utente (per permessi)</param>
        public dtoCommissionEdit(Advanced.Domain.AdvCommission Comm, int currentUserId)
        {
            Id = Comm.Id;
            Name = Comm.Name;
            Description = Comm.Description;
            Status = Comm.Status;
            President = Comm.President;
            Secretary = Comm.Secretary;

            EvalType = Comm.EvalType;
            StepEvalType = (Comm.Step != null || Comm.Step.EvalType == EvalType.none) ? Comm.Step.EvalType : EvalType.Average;

            EvalMinValue = Comm.EvalMinValue;
            EvalBoolBlock = Comm.EvalBoolBlock;

            CommissionTags = Comm.Tags;

            Members = (from Advanced.Domain.AdvMember mem 
                       in Comm.Members.OrderByDescending(m => m.IsPresident).ThenBy(m => m.Member.SurnameAndName)
                       select mem.Member)                
                .ToList();

            EnableMaster = false;
            
            if (Comm.Criteria != null)// && Comm.Criteria.Any())
            {
                Criterion = Comm.Criteria.Select(bc => new Eval.dtoCriterion(bc)).OrderBy(c => c.DisplayOrder).ThenBy(c => c.Name).ToList();
            }
            else
            {
                Criterion = new List<Eval.dtoCriterion>();
            }

            if (Comm.Step != null)
            {
                StepOrder = Comm.Step.Order;
                StepName = Comm.Step.Name;
                StepStatus = Comm.Step.Status;

                isEconomic = Comm.Step.Type == StepType.economics;

                if (Comm.Step.Commissions != null && Comm.Step.Commissions.Count() > 1)
                {
                    EnableMaster = true;
                }

                if(Comm.Step.Call != null)
                {
                    CallTags = Comm.Step.Call.Tags;
                }
            }
            else
            {
                StepOrder = 0;
                StepName = "";
                StepStatus = StepStatus.Draft;

            }

            if (Comm.Call != null)
            {
                CallName = Comm.Call.Name;
            }
            else
            {
                CallName = "";
            }

            IsMaster = Comm.IsMaster;

            Permission = CommissionPermission.None;

            //Permission!!!

            if (currentUserId > 0)
            {
                Permission = CommissionPermission.None;

                if (President.Id == currentUserId)
                {
                    Permission |= CommissionPermission.View;

                    if (Comm.Call != null)
                    {
                        if (Comm.Call.Status == CallForPapers.Domain.CallForPaperStatus.SubmissionClosed)
                            Permission |= CommissionPermission.OpenEvaluation;
                    }

                    if (Comm.Status == CommissionStatus.Closed)
                    //    || (Comm.Status & CommissionStatus.Locked) == CommissionStatus.Locked)
                    {
                        Permission |= CommissionPermission.UploadVerbale;
                    }


                    //CloseEvaluation = 1 << 7,
                    //CloseCommission = 1 << 8
                }

                if (Comm.Secretary.Id == currentUserId)
                {
                    Permission |= CommissionPermission.View
                        | CommissionPermission.Edit
                        | CommissionPermission.RequestIntegration;

                }


                if (Comm.Members.Any(m => m.Member.Id == currentUserId))
                {
                    if (Comm.Status != CommissionStatus.Draft)
                        Permission |= CommissionPermission.View;

                    Permission |= CommissionPermission.Evaluate;
                }
            }

            VerbaleLink = Comm.VerbaleLink;

            MaxValue = Comm.MaxValue;
        }

        /// <summary>
        /// Verifica permessi
        /// </summary>
        /// <param name="required">Permesso richiesto</param>
        /// <returns>True se ha il permesso</returns>
        public bool HasCommissionPermission(CommissionPermission required)
        {
            return ((required & Permission) == required);
        }




    }
}



 

       