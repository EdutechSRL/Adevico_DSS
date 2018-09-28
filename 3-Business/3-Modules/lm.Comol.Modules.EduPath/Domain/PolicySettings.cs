using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel;


namespace lm.Comol.Modules.EduPath.Domain
{
    [Serializable()]
    public class PolicySettings
    {
        public virtual EditingPolicy Editing { get; set; }
        public virtual CompletionPolicy Statistics { get; set; }
        public virtual DisplayPolicy DisplaySubActivity { get; set; }
        public PolicySettings()
        {
            Statistics = CompletionPolicy.NoUpdateIfCompleted;
            DisplaySubActivity = DisplayPolicy.NoModal;
            Editing = (EditingPolicy.ScormSettings | EditingPolicy.UnitNotes | EditingPolicy.ActivityNotes | EditingPolicy.MultimediaSettings);
        }
    }

    [Serializable]
    public enum CompletionPolicy
    {
        /// <summary>
        /// se l'elemento del percorso formativo è stato compeltato e superato NON ricalcolo nulla
        /// </summary>
        NoUpdateIfCompleted = 0,
        /// <summary>
        /// procedo con l'aggiornamento SOLO se il risultato ottenuto è migliore del precedente (su elementi completati e superati)
        /// </summary>
        UpdateOnlyIfBetter = 2,
        /// <summary>
        ///  procedo con l'aggiornamento SOLO se il risultato ottenuto è peggiore del precedente (su elementi completati e superati)
        /// </summary>
        UpdateOnlyIfWorst = 4,
        /// <summary>
        ///  aggiorno SEMPRE lo stato dell'elemento con il risultato attuale sull'attività
        /// </summary>
        UpdateAlways = 8
    }
    [Serializable]
    public enum DisplayPolicy
    {
        NoModal = 0,
        /// <summary>
        /// in base alle impostazioni messe sull'oggetto creato nel percorso
        /// </summary>
        ModalByItem = 2,
        /// <summary>
        /// tutti gli oggetti del percorso formativo che possono consentire la visualizzazione modale
        /// </summary>
        ModalForAllByAvailability = 4,
        /// <summary>
        /// Impostazioni ereditate dal percorso: opzione in uso per unità, attività e sotto-attività
        /// </summary>
        InheritedByPath = 8,
        /// <summary>
        /// Impostazioni ereditate dal percorso: opzione in uso per  attività e sotto-attività
        /// </summary>
        InheritedByUnit = 16,
        /// <summary>
        /// Impostazioni ereditate dall'attività: opzione in uso per le sotto-attività
        /// </summary>
        InheritedByActivity = 32,
    }
}