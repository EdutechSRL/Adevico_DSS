using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Modules.CallForPapers.Advanced.dto
{
    /// <summary>
    /// dto Integrazione 
    /// (contiene l'elenco delle integrazioni per un bando)
    /// </summary>
    public class dtoIntegration
    {
        /// <summary>
        /// Permessi: lettura (es: per i valutatori)
        /// </summary>
        public bool CanView { get; set; }
        /// <summary>
        /// ID segretario
        /// </summary>
        public int SecretaryId { get; set; }
        /// <summary>
        /// Se l'utente corrente è il segretario
        /// </summary>
        public bool IsSecretary { get; set; }
        /// <summary>
        /// Se l'utente corrente è il membro
        /// </summary>
        public bool IsSubmitter { get; set; }
        /// <summary>
        /// Elenco integrazioni
        /// </summary>
        public IList<dtoIntegrationItem> items { get; set; }
        
    }
}
