using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Core.BaseModules.Tickets.Domain.DTO
{
    /// <summary>
    /// Oggetto risposta per l'aggiunta di Utenti da una category
    /// </summary>
    [CLSCompliant(true)]
    [Serializable]
    public class DTO_AddRolesResponse
    {
        /// <summary>
        /// DTO ROLES che non sono stati aggiunti
        /// </summary>
        public IList<DTO_CategoryRole> UnAddedRoles { get; set; }
        /// <summary>
        /// Manca un MANGER per la categoria
        /// </summary>
        public Boolean NoManager { get; set; }

        /// <summary>
        /// Costruttore
        /// </summary>
        public DTO_AddRolesResponse()
        {
            NoManager = false;
            UnAddedRoles = new List<DTO_CategoryRole>();
        }
    }
}
