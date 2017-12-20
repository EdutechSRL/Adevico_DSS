using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel;

namespace lm.Comol.Core.TemplateMessages.Domain
{
    [Serializable]
    public class dtoSelectorContext
    {
        public virtual Int32 IdModule { get; set; }
        public virtual String ModuleCode { get; set; }
        public virtual long IdAction { get; set; }
        public virtual Int32 IdOrganization { get; set; }
        public virtual Int32 IdOrganizationCommunity  { get; set; }
        public virtual Int32 IdCommunity { get; set; }
        public virtual Boolean IsForPortal {get;set;}
        public virtual ModuleObject ObjectOwner { get; set; }
        public dtoSelectorContext()
        {
        }
    }
}