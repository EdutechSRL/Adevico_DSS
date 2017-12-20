using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel;
namespace lm.Comol.Modules.CallForPapers.Domain
{
    [Serializable]
    public class dtoGenericPermission
    {
        public virtual Boolean AllowDelete { get; set; }
        public virtual Boolean AllowVirtualDelete { get; set; }
        public virtual Boolean AllowUnDelete { get; set; }
        public virtual Boolean AllowEdit { get; set; }

        public dtoGenericPermission()
            : base()
        {

        }
    }
}