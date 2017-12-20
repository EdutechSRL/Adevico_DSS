using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Core.BaseModules.Tickets.Domain.DTO
{
    /// <summary>
    /// Impostazioni permessi
    /// </summary>
    [Serializable, CLSCompliant(true)]
    public class DTO_SettingsPermissionUsers
    {
        public Int64 Id { get; set; }
        public Int64 UserId { get; set; }

        public Int32 PersonId { get; set; }
        public String DisplayName { get; set; }

        public String PersonType { get; set; }


    }
}
