using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Core.BaseModules.FileStatistics.Domain
{
    public class dtoDownInfo
    {
        public String downService { get; set; }    //Servizio da cui è stato effettuato il download
        public DateTime downDate { get; set; }
    }
}
