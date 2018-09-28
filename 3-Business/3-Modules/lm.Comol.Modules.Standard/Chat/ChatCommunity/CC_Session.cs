using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Modules.Standard.ChatCommunity
{
    public class CC_Session
    {
        /// <summary>
        /// Session Name
        /// </summary>
        String Name { get; set; }

        /// <summary>
        /// Messages list
        /// </summary>
        IList<CC_Message> Messages { get; set; }
        /// <summary>
        /// Partecipant list
        /// </summary>
        IList<CC_LkToPartecipant> Partecipants { get; set; }
        /// <summary>
        /// File list
        /// </summary>
        IList<CC_LkToFiles> Files { get; set; }

        /// <summary>
        /// Start Date
        /// </summary>
        DateTime? StartOn { get; set; }
        /// <summary>
        /// End Date
        /// </summary>
        DateTime? EndOn { get; set; }


        /* For this data can be use BaseObject property... */
        /// <summary>
        /// Session ID
        /// </summary>
        Int64 ID { get; set; }

        /// <summary>
        /// Owner Id
        /// </summary>
        Int32 OwnerId { get; set; }

        /// <summary>
        /// If the owner is a Community or an User
        /// </summary>
        Int16 OwnerType { get; set; }   
    }
}
