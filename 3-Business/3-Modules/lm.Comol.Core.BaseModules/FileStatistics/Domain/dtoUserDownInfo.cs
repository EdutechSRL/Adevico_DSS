using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Core.BaseModules.FileStatistics.Domain
{
    public class dtoUserDownInfo
    {
        public String downBy { get; set; }  //User name
        public IList<dtoDownInfo> downOnList { get; set; }
        public Int32 TotalDownload { get; set; }
    }
}
