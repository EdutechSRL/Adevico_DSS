using System.Runtime.Serialization;
using System;
using System.Linq;
using lm.Comol.Core.Communities;
using System.Collections.Generic;

namespace lm.Comol.Core.BaseModules.Dashboard.Domain
{
	[Serializable(), CLSCompliant(true)]
    public class dtoPathItem
	{
        public Int32 IdFather { get; set; }
        public List<String> FathersName { get; set; }
        public Boolean isPrimary { get; set; }
        public String Path { get; set; }
        public Int32 PathDepth { get
            {
                return Path.ToCharArray().Count(x => x == '.')-2;
            }
        }
        public dtoPathItem()
        {
            FathersName = new List<String>();
        }        
    }
}