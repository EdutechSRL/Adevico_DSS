using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Core.FileRepository.Domain
{
    [Serializable]
    public class dtoLinkedModules
    {
        public long IdItem { get; set; }
        public List<String> ModuleCodes { get; set; }
    }
}