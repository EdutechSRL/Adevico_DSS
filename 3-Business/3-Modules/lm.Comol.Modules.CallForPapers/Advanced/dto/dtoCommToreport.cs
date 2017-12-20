using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Modules.CallForPapers.Advanced.dto
{
    /// <summary>
    /// dto con i dati per la navigazione al report della commissione
    /// </summary>
    public class dtoCommToreport
    {
        /// <summary>
        /// Se il link è attivo (permessi)
        /// </summary>
        public bool IsActive { get; set; }

        /// <summary>
        /// Se la commissione è la commissione Master
        /// </summary>
        public bool IsMaster { get; set; }
        
        /// <summary>
        /// Nome visualizzato
        /// </summary>
        public string CommissionName { get; set; }

        /// <summary>
        /// Id Commissione (per generazione URL)
        /// </summary>
        public long CommissionId { get; set; }

        /// <summary>
        /// Id Comunità del bando
        /// </summary>
        public int CommunityId { get; set; }

        /// <summary>
        /// Stato commissione
        /// </summary>
        public CommissionStatus Status { get; set; }

        /// <summary>
        /// Id Bando
        /// </summary>
        public long CallId { get; set; }
    }
}
