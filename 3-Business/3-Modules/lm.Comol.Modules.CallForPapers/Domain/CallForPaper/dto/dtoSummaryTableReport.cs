using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Modules.CallForPapers.Domain
{
    [Serializable]
    public class dtoSummaryTableReportItem
    {
        public string Name { get; set; }
        public double Total { get; set; }
        public double MaxTotal { get; set; }
    }

    [Serializable]
    public class dtoSummaryTableReportTotal
    {
        public dtoSummaryTableReportTotal()
        {
            Tables = new List<dtoSummaryTableReportItem>();
        }

        public IList<dtoSummaryTableReportItem> Tables { get; set; }

        public double Total
        {
            get
            {
                if (Tables == null || !Tables.Any())
                    return 0;

                return Tables.Sum(itm => itm.Total);
            }
        }

        public double MaxTotal
        {
            get
            {
                if (Tables == null || !Tables.Any())
                    return 0;

                return Tables.Sum(itm => itm.MaxTotal);
            }
        }
    }
}
