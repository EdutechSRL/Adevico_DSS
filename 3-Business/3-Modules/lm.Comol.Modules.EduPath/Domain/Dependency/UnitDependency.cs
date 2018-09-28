using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Modules.EduPath.Domain
{
    [Serializable()]
    public class UnitDependency : lm.Comol.Core.DomainModel.DomainBaseObjectIdLiteMetaInfo<long>
    {
        public virtual Unit PreviuosUnit { get; set; }
        public virtual Unit NextUnit { get; set; }
        public virtual Int16 Ma { get; set; }
    
    }
}
