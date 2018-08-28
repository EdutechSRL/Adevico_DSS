using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel;
using lm.Comol.Modules.CallForPapers.Domain;

namespace lm.Comol.Modules.CallForPapers.Advanced.Domain
{
    /// <summary>
    /// Commissione avanzata
    /// </summary>
    public class AdvCommission : DomainBaseObjectLiteMetaInfo<long>
    {
        /// <summary>
        /// Bando di riferimento
        /// </summary>
        public virtual BaseForPaper Call { get; set; }
        /// <summary>
        /// Step di appartenenza
        /// </summary>
        public virtual AdvStep Step { get; set; }
        /// <summary>
        /// Nome commissione
        /// </summary>
        public virtual String Name { get; set; }
        /// <summary>
        /// Descrizione commissione
        /// </summary>
        public virtual String Description { get; set; }
        /// <summary>
        /// Stato commissione
        /// </summary>
        public virtual CommissionStatus Status { get; set; }
        /// <summary>
        /// Presidente commissione (presente tra i membri)
        /// </summary>
        public virtual litePerson President { get; set; }
        /// <summary>
        /// Segretario
        /// </summary>
        public virtual litePerson Secretary { get; set; }
        /// <summary>
        /// Elenco membri
        /// </summary>
        public virtual IList<AdvMember> Members { get; set; }
        /// <summary>
        /// Commissione Master: determina la chiusura dello step
        /// e la selezione dei bandi che accedono allo step successivo
        /// </summary>
        public virtual bool IsMaster { get; set; }
        /// <summary>
        /// Elenco criteri di valutazione
        /// </summary>
        public virtual IList<CallForPapers.Domain.Evaluation.BaseCriterion> Criteria { get; set; }
        /// <summary>
        /// Tipo di valutazione (Media/Somma)
        /// </summary>
        public virtual EvalType EvalType { get; set; }
        /// <summary>
        /// Valore minimo valutazione
        /// </summary>
        public virtual int EvalMinValue { get; set; }
        /// <summary>
        /// Indica se i criteri booleani sono vincolanti o meno ai fini del superamento della commissione
        /// </summary>
        public virtual bool EvalBoolBlock { get; set; }
        /// <summary>
        /// Link al verbale
        /// </summary>
        public virtual ModuleLink VerbaleLink { get; set; }
        /// <summary>
        /// TAG di riferimento della commissione (presenti come filtri nella visualizzazione delle sottomissioni)
        /// </summary>
        public virtual string Tags { get; set; }
        

        /// <summary>
        /// Id Template per esportazione documento di riepilogo
        /// </summary>
        public virtual Int64 TemplateId { get; set; }
        /// <summary>
        /// Id Versione Template per esportazione documento di riepilogo
        /// </summary>
        /// <remarks>
        /// Se -1, verrà utilizzata sempre l'ultima versione
        /// </remarks>
        public virtual Int64 TemplateVersionId { get; set; }

        /// <summary>
        /// Verifica se l'utente appartiene alla commissione
        /// </summary>
        /// <param name="PersonId">Id persona da controllare</param>
        /// <returns>True se l'utente fa parte della commissione, come Presidente, Segretario o Membro</returns>
        public virtual bool IsInCommission(int PersonId)
        {
            if (President != null && President.Id == PersonId)
                return true;

            if (Secretary != null && Secretary.Id == PersonId)
                return true;

            if (Members != null && Members.Any(m => m.Member != null && m.Member.Id == PersonId))
            {  
                return true;
            }
            return false;
        }

        /// <summary>
        /// Massimale ammesso.
        /// Per commissione economica.
        /// </summary>
        public virtual double MaxValue { get; set; }

        /// <summary>
        /// Verifica se l'utente è in commissione e puo' visualizzare le sottomissioni
        /// </summary>
        /// <param name="PersonId">Id Persona</param>
        /// <returns>True se è presidente o segretario. Se è membro la commissione deve avere uno stato diverso da bozza.</returns>
        public virtual bool IsInCommissionToShowSubmission(int PersonId)
        {
            if (President != null && President.Id == PersonId)
                return true;

            if (Secretary != null && Secretary.Id == PersonId)
                return true;

            if (Members != null && Members.Any(m => m.Member != null && m.Member.Id == PersonId))
            {
                if (Status != CommissionStatus.Draft)
                    return true;
            }

            return false;


        }

        public virtual bool IsInCommissionAsSecretaryOrPresident(int PersonId)
        {
            if (President != null && President.Id == PersonId)
                return true;

            if (Secretary != null && Secretary.Id == PersonId)
                return true;
            
            return false;


        }
    }
}
