using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Core.BaseModules.Tickets.Domain.DTO
{
    public class DTO_SettingsPermissionList
    {
        public IList<DTO_SettingsPermissionPersonType> PersonTypePermission { get; set; }
        public IList<DTO_SettingsPermissionUsers> UserPermission { get; set; }

    }
}
