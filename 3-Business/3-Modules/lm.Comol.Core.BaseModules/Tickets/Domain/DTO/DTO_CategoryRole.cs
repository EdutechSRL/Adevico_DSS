using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Core.BaseModules.Tickets.Domain.DTO
{
    /// <summary>
    /// DTO per la modifica dei ruoli degli utenti
    /// </summary>
    [Serializable]
    [CLSCompliant(true)]
    public class DTO_CategoryRole
    {
        /// <summary>
        /// Id Person (TICKET)
        /// </summary>
        public Int32 PersonId { get; set; }
        /// <summary>
        /// Se è Manager (altrimenti resolver
        /// </summary>
        public Boolean IsManager { get; set; }

        /// <summary>
        /// Costruttore
        /// </summary>
        public DTO_CategoryRole()
        {
            PersonId = 0;
            IsManager = false;
        }
    }
}
