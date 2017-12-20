using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Core.BaseModules.FileStatistics.Domain
{
    public class dtoFileDetail
    {
        //Preso dai dettagli attuali sul file
        //Indicati quali vengono "esclusi" in caso di stat generiche...

        public String FileName { get; set; }
        public String ComService { get; set; }
        public String Path { get; set; }   //SOLO per genericità in relazione alle info sui file
        public long Size { get; set; }
        public String LoadedBy { get; set; }
        public DateTime LoadedOn { get; set; }
        public long Downloads { get; set; }

        public Boolean isInternal { get; set; }

        public String Visibility { get; set; }
        public String Permission { get; set; } //SOLO per genericità in relazione alle info sui file

        public IList<dtoUserDownInfo> DownDetails { get; set; }
    }
}
