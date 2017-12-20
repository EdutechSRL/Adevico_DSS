using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Core.BaseModules.Tickets.Domain.DTO
{
    /// <summary>
    /// Response aggiunta utente
    /// </summary>
    [Serializable]
    [CLSCompliant(true)]
    public class DTO_ExtUserAddResult
    {
        ///// <summary>
        ///// User Id
        ///// </summary>
        //public Int64 UserId { get; set; }
        ///// <summary>
        ///// Person Id
        ///// </summary>
        //public Int64 PersonId { get; set; }
        /// <summary>
        /// Eventuale errore
        /// </summary>
        public Enums.ExternalUserCreateError Errors { get; set; }

        //public String DisplayName { get; set; }
        //public String Mail { get; set; }
        public Domain.DTO.DTO_User User { get; set; }
        //Temporaneo
        public string Note { get; set; }

        /// <summary>
        /// Costruttore
        /// </summary>
        public DTO_ExtUserAddResult()
        {
            User = new DTO_User();
            User.UserId = 0;
            User.PersonId = 0;
            Errors = Enums.ExternalUserCreateError.none;
        }
    }
}
