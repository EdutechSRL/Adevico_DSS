using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel;

namespace lm.Comol.Core.PersonalInfo
{
    [Serializable]
    public class DataPolicy  : DomainBaseObjectMetaInfo<long>
    {
        public virtual String Name { get; set; }
        public virtual String Text { get; set; }
        public virtual Boolean Mandatory { get; set; }
        public virtual Int32 DisplayOrder  { get; set; }
        public virtual PolicyType Type { get; set; }
        public virtual Boolean isActive { get; set; }
        public virtual PersonalFieldType InvolvedFields { get; set; }
    }
}