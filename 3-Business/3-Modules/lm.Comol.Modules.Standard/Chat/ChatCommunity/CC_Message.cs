using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Modules.Standard.ChatCommunity
{
    public class CC_Message
    {
        /// <summary>
        /// Message Id
        /// </summary>
        Int64 Id { get; set; }
        /// <summary>
        /// Owner/Creator ID
        /// </summary>
        Int32 CreatorId { get; set; } //oppure direttamente person o gli si fa ereditare da DomainObject (timestamp, etc...)
        /// <summary>
        /// Message recipient
        /// </summary>
        /// <remarks> 0 for all users. Eventually negative number for group...???</remarks>
        Int32 RecipientId { get; set; }
        /// <summary>
        /// Creation Date
        /// </summary>
        DateTime CreatedOn { get; set; }

        /// <summary>
        /// Message Text. It can contain html code... (depend on system)
        /// </summary>
        String HTMLMessage { get; set; }

        /// <summary>
        /// Message Text. It can contain only text.
        /// </summary>
        /// <remarks>Eventualmente sostituire con un medoto che estrae il testo dall'html...</remarks>
        String StringMessage { get; set; }

        //...
    }
}
