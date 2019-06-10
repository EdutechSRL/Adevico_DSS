using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Modules.CallForPapers.Advanced.dto
{
    /// <summary>
    /// dto STEP
    /// </summary>
    public class dtoAdvStepContainer
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
        /// Ordine di visualizzazione
        /// </summary>
        public int Order { get; set; }
        /// <summary>
        /// Stato dello step
        /// </summary>
        public StepStatus Status { get; set; }
        /// <summary>
        /// ID bando di riferimento
        /// </summary>
        public Int64 CallId { get; set; }
        /// <summary>
        /// Elenco commissioni che compongono lo step
        /// </summary>
        public IList<dtoAdvCommissionContainer> Commissions { get; set; }
        /// <summary>
        /// Permessi step (tipo attore)
        /// </summary>
        public GenericStepPermission StepPermission { get; set; }

        /// <summary>
        /// Costruttore vuoto
        /// </summary>
        public dtoAdvStepContainer()
        {
            Id = 0;
            Name = "";
            Order = 0;
            Status = StepStatus.Draft;
            CallId = -1;
            Commissions = new List<dtoAdvCommissionContainer>();
            StepPermission = GenericStepPermission.none;
        }

        //public dtoAdvStepContainer(Domain.AdvStep step)
        //{
        //    Id = step.Id;
        //    Name = step.Name;
        //    Order = step.Order;
        //    Status = step.Status;
        //    CallId = (step.Call != null) ? step.Call.Id : -1;
            
        //    if(step.Commissions != null && step.Commissions.Any())
        //    {


        //        Commissions = (from Domain.AdvCommission comm in step.Commissions.OrderByDescending(c => c.IsMaster).ThenBy(c => c.Name).ToList()
        //                       select new dtoAdvCommissionContainer(comm)).ToList();
            
        //    }
        //}
        /// <summary>
        /// Costruttore da oggetti dominio
        /// </summary>
        /// <param name="step">Step di riferimento</param>
        /// <param name="userId">Id Utente (per calcolo permessi)</param>
        public dtoAdvStepContainer(Domain.AdvStep step, int userId, int creatorId)
        {
            Id = step.Id;
            Name = step.Name;
            Order = step.Order;
            Status = step.Status;
            CallId = (step.Call != null) ? step.Call.Id : -1;
            StepPermission = GenericStepPermission.none;

            if (step.Commissions != null) // && step.Commissions.Any())
            {

                //Comm.Criteria.Select(bc => new Eval.dtoCriterion(bc)).OrderBy(c => c.DisplayOrder).ThenBy(c => c.Name).ToList();
                Commissions = step.Commissions.Select(cm => new dtoAdvCommissionContainer(cm, userId)).OrderByDescending(c => c.IsMaster).ThenBy(c => c.Name).ToList();

                StepPermission = GenericStepPermission.none;

                if(Commissions.Any(cm => (cm.GenericPermission & GenericStepPermission.MainPresident) == GenericStepPermission.MainPresident))
                {
                    StepPermission |= GenericStepPermission.MainPresident;
                }

                if (Commissions.Any(cm => (cm.GenericPermission & GenericStepPermission.MainSecretary) == GenericStepPermission.MainSecretary))
                {
                    StepPermission |= GenericStepPermission.MainSecretary;
                }

                if (Commissions.Any(cm => (cm.GenericPermission & GenericStepPermission.President) == GenericStepPermission.President))
                {
                    StepPermission |= GenericStepPermission.President;
                }

                if (Commissions.Any(cm => (cm.GenericPermission & GenericStepPermission.Secretary) == GenericStepPermission.Secretary))
                {
                    StepPermission |= GenericStepPermission.Secretary;
                }

                if (Commissions.Any(cm => (cm.GenericPermission & GenericStepPermission.Member) == GenericStepPermission.Member))
                {
                    StepPermission |= GenericStepPermission.Member;
                }
            }

            if (creatorId > 0 && creatorId == userId)
            {
                StepPermission |= GenericStepPermission.MainPresident;
                StepPermission |= GenericStepPermission.MainSecretary;
                StepPermission |= GenericStepPermission.President;
                StepPermission |= GenericStepPermission.Secretary;
            }

        }
    }
}
