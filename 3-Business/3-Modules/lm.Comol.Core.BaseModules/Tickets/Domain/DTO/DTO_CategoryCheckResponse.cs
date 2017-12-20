using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Core.BaseModules.Tickets.Domain.DTO
{
    public class DTO_CategoryCheckResponse
    {
        public DTO_CategoryCheckResponse()
        {
            UserDisplayName = "";
            IsCurrentUser = false;
            NoUser = false;
            NoCategory = false;
            PreviousAssigned = false;
        }
        public String UserDisplayName { get; set; }

        public bool IsCurrentUser { get; set; }
        public bool NoUser { get; set; }
        public bool NoCategory { get; set; }

        public bool PreviousAssigned { get; set; }
    }
}
