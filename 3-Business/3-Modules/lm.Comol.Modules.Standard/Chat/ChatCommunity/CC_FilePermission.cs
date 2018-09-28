using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Modules.Standard.ChatCommunity
{
    public class CC_FilePermission
    {
        /// <summary>
        /// singolo permesso
        /// </summary>
        CC_Permission Permission { get; set; }

        /// <summary>
        /// Assegnatario del permesso
        /// </summary>
        Int32 OwnerId { get; set; }
        /// <summary>
        /// Discriminante
        /// </summary>
        /// <example> User, role, group, etc...</example>
        PartecipantDisc Discriminator { get; set; }

    }
}
