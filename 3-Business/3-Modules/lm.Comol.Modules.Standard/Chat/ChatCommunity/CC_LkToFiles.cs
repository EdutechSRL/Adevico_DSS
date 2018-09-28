using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Modules.Standard.ChatCommunity
{
    class CC_LkToFiles
    {
        /// <summary>
        /// File/link ID
        /// </summary>
        Int32 ExternalId { get; set; }


        /* PERMESSI: da definire...*/
        IList<CC_FilePermission> Permissions { get; set; }
        

    }
}