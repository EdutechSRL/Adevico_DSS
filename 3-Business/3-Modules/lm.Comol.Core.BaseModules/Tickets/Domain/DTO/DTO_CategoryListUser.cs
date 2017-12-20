using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Core.BaseModules.Tickets.Domain.DTO
{
    /// <summary>
    /// DTO per l'elenco utenti assegnati a Categoria
    /// </summary>
    [Serializable]
    [CLSCompliant(true)]
    public class DTO_CategoryListUser
    {
        /// <summary>
        /// Id Utente
        /// </summary>
        public Int64 UserId { get; set; }
        /// <summary>
        /// Nome utente
        /// </summary>
        public String DisplayName { get; set; }
        /// <summary>
        /// Se è manager (altrimenti resolver)
        /// </summary>
        public bool IsManager { get; set; }
    }
}
