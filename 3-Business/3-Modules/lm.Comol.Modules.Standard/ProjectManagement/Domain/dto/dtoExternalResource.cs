using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel;
using lm.Comol.Modules.Standard.ProjectManagement.Domain;

namespace lm.Comol.Modules.Standard.ProjectManagement.Domain
{
    [Serializable]
    public class dtoExternalResource : dtoResource
    {
        public virtual String Mail { get; set; }
        public virtual ResourceType ResourceType { get { return Domain.ResourceType.External; } }
        public dtoExternalResource(){ }
    }
}