using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Modules.CallForPapers.Advanced.dto
{
    /// <summary>
    /// dto Edit STEP
    /// </summary>
    public class dtoStepsEdit
    {
        /// <summary>
        /// Step validazione
        /// </summary>
        public dtoAdvStepContainer ValidationStep { get; set; }
        /// <summary>
        /// Step personalizzati
        /// </summary>
        public IList<dtoAdvStepContainer> CustomSteps { get; set; }
        /// <summary>
        /// Step economico
        /// </summary>
        public dtoAdvStepContainer EconomicStep { get; set; }
        /// <summary>
        /// Id bando di riferiemnto
        /// </summary>
        public Int64 CallId { get; set; }
        /// <summary>
        /// Permessi generici
        /// </summary>
        public GenericStepPermission StepPermission { get; set; }
        
        /// <summary>
        /// Crea uno step vuoto
        /// </summary>
        /// <returns></returns>
        public static dtoStepsEdit GetEmpty()
        {
            return new dtoStepsEdit();
        }

        /// <summary>
        /// Costruttore vuoto
        /// </summary>
        public dtoStepsEdit()
        {
            CallId = -1;
            ValidationStep = null;
            EconomicStep = null;
            CustomSteps = null;
        }
        
        /// <summary>
        /// Costruttore da oggetti dominio
        /// </summary>
        /// <param name="steps">Elenco step</param>
        /// <param name="userId">Id utente: per permessi</param>
        public dtoStepsEdit(IList<Domain.AdvStep> steps, int userId, int creatorId)
        {
            if (steps == null || !steps.Any())
            {
                return;
            }

            IList<Int64> CallIds = (from Domain.AdvStep stp in steps
                                    where stp.Call != null
                                    select stp.Call.Id).Distinct().ToList();

            if (CallIds.Count != 1)
                return;

            CallId = CallIds.FirstOrDefault();

            ValidationStep = new dtoAdvStepContainer(
                (from Domain.AdvStep stp in steps
                 where stp.Type == StepType.validation
                 select stp).FirstOrDefault(),
                userId, creatorId);


            if (steps.Any(st => st.Type == StepType.economics))
            {
                EconomicStep = new dtoAdvStepContainer(
                (from Domain.AdvStep stp in steps
                 where stp.Type == StepType.economics
                 select stp).FirstOrDefault(),
                userId, creatorId);
            }

            if (steps.Any(st => st.Type == StepType.custom))
            {
                CustomSteps = (from Domain.AdvStep stp in steps.OrderBy(st => st.Order)
                               where stp.Type == StepType.custom
                               select new dtoAdvStepContainer(stp, userId, creatorId)).ToList();
            }
        }


    }
}
