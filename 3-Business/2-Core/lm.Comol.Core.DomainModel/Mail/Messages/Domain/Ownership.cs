using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel;

namespace lm.Comol.Core.Mail.Messages
{
    [Serializable]
    public class Ownership
    {
        public virtual liteCommunity Community { get; set; }
        public virtual ModuleObject ModuleObject { get; set; }
        public virtual Int32 IdModule { get; set; }
        public virtual String ModuleCode { get; set; }
        public virtual Boolean IsPortal { get; set; }
    }
}