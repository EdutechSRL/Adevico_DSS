using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Core.BaseModules.Tickets.Domain.DTO
{
    [Serializable, CLSCompliant(true)]
    public class DTO_TkLanguage
    {
        public int Id { get; set; }
        public String Code { get; set; }
        public String DisplayName { get; set; }
    }
}
