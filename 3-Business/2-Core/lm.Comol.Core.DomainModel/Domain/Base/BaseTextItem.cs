using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Core.DomainModel
{
    public class BaseTextItem<T> : DomainBaseObjectMetaInfo<T>
    {
        public virtual String Title { get; set; }
        public virtual String Text { get; set; }
        public virtual String Description { get; set; }
        public virtual Boolean isForPortal { get; set; }
        public virtual Boolean isPersonal { get; set; }
    }
}