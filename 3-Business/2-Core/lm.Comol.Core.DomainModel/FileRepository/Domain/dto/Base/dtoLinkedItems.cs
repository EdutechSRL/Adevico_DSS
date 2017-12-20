using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Core.FileRepository.Domain
{
    [Serializable]
    public class dtoLinkedItems
    {
        public List<long> IdItems { get; set; }
        public String ModuleCode { get; set; }
        public Int32 IdModule { get; set; }
    }
}