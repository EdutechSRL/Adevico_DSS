using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel;
namespace lm.Comol.Modules.CallForPapers.Domain
{
    [Serializable()]
    public class dtoRevisionDisplayItemPermission : dtoBase 
    {
        public virtual dtoRevisionRequestPermission Permission { get; set; }
        public virtual dtoRevisionDisplay Revision { get; set; }

        public dtoRevisionDisplayItemPermission()
        {
            Permission = new dtoRevisionRequestPermission();
        }
    }
    
}