using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Modules.Standard.ChatCommunity
{
    public class CC_LkToPartecipant
    {
        /// <summary>
        /// Id Partecipante
        /// </summary>
        Int32 UserId { get; set; }
        
        /// <summary>
        /// Se l'utente è attualmente in chat
        /// </summary>
        Boolean IsOnline { get; set; }
        
        /// <summary>
        /// Last Visit
        /// </summary>
        /// <remarks>If it's null, User never enter in chat.</remarks>
        DateTime? LastVisit { get; set; }

        /// <summary>
        /// User Permission for this chat
        /// </summary>
        CC_Permission Permission { get; set; }
    }
}
