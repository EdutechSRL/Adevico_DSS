using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using lm.Comol.Core.DomainModel;

namespace lm.Comol.Modules.CallForPapers.Advanced.Domain
{
    /// <summary>
    /// Tracciatora membri precedenti
    /// </summary>
    /// <remarks>
    /// La data di sostituzione è la data di creazione dell'oggetto: "_CreatedOn" dell'oggetto padre
    /// </remarks>
    public class AdvOldMember : DomainBaseObjectLiteMetaInfo<long>
    {
        /// <summary>
        /// ID membro sostituito
        /// </summary>
        public virtual long MemberId { get; set; }
        /// <summary>
        /// ID dati persona precedente
        /// </summary>
        public virtual int OldPersonId { get; set; }
    }
}
