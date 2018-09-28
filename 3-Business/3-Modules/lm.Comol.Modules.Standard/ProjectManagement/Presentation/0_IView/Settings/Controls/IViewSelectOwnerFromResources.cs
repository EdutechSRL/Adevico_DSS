using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Modules.Standard.ProjectManagement.Domain;

namespace lm.Comol.Modules.Standard.ProjectManagement.Presentation
{
    public interface IViewSelectOwnerFromResources : lm.Comol.Core.DomainModel.Common.iDomainView 
    {
        Boolean isInitialized { get; set; }
        Boolean DisplayDescription { get; set; }
        Boolean RaiseCommandEvents { get; set; }
        void InitializeControl(List<dtoProjectResource> resources,String description = "");
        long GetSelectedIdResource();
    }
}