using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Core.BaseModules.Tickets.Domain.DTO
{
    [Serializable, CLSCompliant(true)]
    public class DTO_SysCategoryInfo
    {
        public DTO_SysCategoryInfo()
        {
            Public = 0;
            Ticket = 0;
            Community = 0;
        }

        public int Public { get; set; }
        public int Ticket { get; set; }
        public int Community { get; set; }
    }
}
