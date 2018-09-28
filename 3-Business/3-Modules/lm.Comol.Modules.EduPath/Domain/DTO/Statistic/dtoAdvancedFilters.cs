using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Modules.EduPath.Domain
{
    [Serializable]
    public class dtoAdvancedFilters
    {
        public ExportNamingMode ObjectNamingMode{ get; set; }
        public ExportNamingMode StatusNamingMode{ get; set; }
        public ItemType StatisticsLevel { get; set; }
        public List<CellType> Cells { get; set; }
        public List<Int32> IdRoles{ get; set; }
        public List<long> IdAgencies{ get; set; }

        public dtoAdvancedFilters()
        {
            Cells = new List<CellType>();
            IdRoles = new List<Int32>();
            IdAgencies = new List<long>();
            StatisticsLevel = ItemType.Activity;
        }
    }
}
