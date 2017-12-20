using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Core.BaseModules.Tickets.Domain.DTO
{
    public class DTO_SettingsPermissionPersonType
    {
        public Int32 PersonTypeId { get; set; }
        public String DisplayName { get; set; }

        public Boolean IsSelected { get; set; }
    }
}
