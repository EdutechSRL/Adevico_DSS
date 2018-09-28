using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Modules.Standard.WebConferencing.Domain.eWorks.DTO
{
    public class DTOuser
    {
        public String UserId { get; set; }
        public String UserName { get; set; }
        public String Key { get; set; }
        public Boolean IsMaster { get; set; }
    }
}
